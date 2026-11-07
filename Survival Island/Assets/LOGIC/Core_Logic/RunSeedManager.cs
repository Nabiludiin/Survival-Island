using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pemilik seed utama tiap run, aliran acak per sistem, dan susunan run.
    /// Blueprint: Core_Logic/RunSeedManager.md (#89). GDD bagian 1 (Roguelike, Permadeath).
    ///
    /// Urutan bootstrap ke-15. Sistem gameplay tidak boleh memakai UnityEngine.Random untuk hal yang memengaruhi permainan.
    ///
    /// Keputusan developer (7 November 2026): BuildLayout, RunLayoutSources, dan RunLayoutValidator ditulis di sistem pendukung.
    /// - BuildLayout adalah fungsi statis murni dari seed dan RunLayoutSources: suku cadang (#69), NPC kapal (#76), dan blok
    ///   pertama jadwal bencana (#70). Pemeriksaan lewat RunLayoutValidator, percobaan ulang sampai maxSeedRerolls, lalu
    ///   RunLayoutSources.safeLayout.
    /// - Kompatibilitas: kalau layoutSources kosong di Inspector, perilaku lama tetap jalan dengan peringatan (hanya jadwal
    ///   bencana dari field disasterData lama, tanpa pemeriksaan). HeavyItemSystem lalu memilih titik sendiri.
    /// - Alat Editor RunSeedAudit: menu konteks di aset RunLayoutSources.
    /// </summary>
    public class RunSeedManager : MonoBehaviour, IBootstrapService
    {
        public static RunSeedManager Instance { get; private set; }

        /// <summary>Versi RunSeedSaveData.</summary>
        public const int SaveDataVersion = 1;

        private static readonly int StreamCount = Enum.GetValues(typeof(RandomStreamId)).Length;

        /// <summary>Seed utama. 0 berarti belum ada run.</summary>
        public uint CurrentSeed { get; private set; }

        /// <summary>Format XXXX-XXXX, contoh 7F3A-91C2. Ditampilkan di layar akhir.</summary>
        public string CurrentSeedText { get; private set; } = string.Empty;

        /// <summary>Susunan run yang sudah lolos pemeriksaan.</summary>
        public RunLayout CurrentLayout { get; private set; }

        /// <summary>Run ini memakai safeLayout karena semua seed gagal.</summary>
        public bool UsedFallbackLayout { get; private set; }

        [Tooltip("Sumber susunan run (#89): PartSpawnPointTable, NPC kapal, DisasterData, spawner kapal, safeLayout.")]
        [SerializeField] private RunLayoutSources layoutSources;

        // Ditambahkan Langkah 7 (#70). Sekarang hanya cadangan: dipakai kalau layoutSources kosong atau
        // layoutSources.disasterData kosong, supaya aset lama tetap jalan.
        [Tooltip("Cadangan lama. Utamakan RunLayoutSources.disasterData.")]
        [SerializeField] private DisasterData[] disasterData = Array.Empty<DisasterData>();

        private static readonly RunSeedBalance FallbackRunSeed = new RunSeedBalance();
        private static readonly RegionBalance FallbackRegion = new RegionBalance();

        /// <summary>Sumber susunan run. null berarti perilaku lama (lihat ringkasan kelas).</summary>
        public RunLayoutSources LayoutSources => layoutSources;

        /// <summary>DisasterData yang dipakai jadwal: RunLayoutSources.disasterData, atau field lama kalau kosong.</summary>
        public DisasterData[] DisasterDataSource => layoutSources != null && layoutSources.disasterData != null && layoutSources.disasterData.Length > 0
            ? layoutSources.disasterData : disasterData;

        /// <summary>DisasterData dari sumber; cadangan field lama RunSeedManager.Instance (dipakai juga RunLayoutValidator).</summary>
        public static DisasterData[] ResolveDisasterData(RunLayoutSources sources)
        {
            if (sources != null && sources.disasterData != null && sources.disasterData.Length > 0) return sources.disasterData;
            return Instance != null ? Instance.disasterData : Array.Empty<DisasterData>();
        }

        private RandomStream[] sequential = Array.Empty<RandomStream>();
        private uint forcedNextSeed;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[RunSeedManager] Instance ganda. Periksa prefab PersistentRoot.");
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        /// <summary>Belum membuat seed, hanya menyiapkan data (RunLifecycle.md urutan ke-15).</summary>
        public void Initialize()
        {
            sequential = new RandomStream[StreamCount];
        }

        /// <summary>Permadeath: kosongkan seed dan aliran. forcedNextSeed tidak dihapus, karena dipakai run berikutnya.</summary>
        public void ResetForNewRun()
        {
            CurrentSeed = 0;
            CurrentSeedText = string.Empty;
            CurrentLayout = null;
            UsedFallbackLayout = false;
            sequential = new RandomStream[StreamCount];
        }

        // ---------------------------------------------------------------- Membuat seed

        /// <summary>Dipanggil RunLifecycle.StartNewRun sebelum scene dimuat.</summary>
        public void BeginNewRun()
        {
            bool isForced = forcedNextSeed != 0;
            uint seed = isForced ? forcedNextSeed : GenerateSeed();
            forcedNextSeed = 0;

            CurrentLayout = ResolveLayout(ref seed, isForced);
            CurrentSeed = seed;
            CurrentSeedText = FormatSeed(seed);

            sequential = new RandomStream[StreamCount];
            for (int i = 0; i < StreamCount; i++)
                sequential[i] = new RandomStream(DeriveSeed(seed, (RandomStreamId)i, 0));
        }

        /// <summary>Waktu sistem tidak dipakai, karena dua run berdekatan bisa punya seed mirip.</summary>
        private static uint GenerateSeed()
        {
            uint seed;
            do { seed = unchecked((uint)Guid.NewGuid().GetHashCode()); } while (seed == 0);
            return seed;
        }

        private static uint DeriveNextSeed(uint seed)
        {
            ulong z = Mix(seed ^ 0xA5A5A5A5UL);
            uint next = unchecked((uint)z);
            return next == 0 ? 1u : next;
        }

        public static string FormatSeed(uint seed)
        {
            return (seed >> 16).ToString("X4") + "-" + (seed & 0xFFFF).ToString("X4");
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        /// <summary>Perintah DevConsole "seed XXXX-XXXX" (#56). Hanya build pengembangan.</summary>
        public void DebugForceNextSeed(uint seed)
        {
            forcedNextSeed = seed;
        }
#endif

        // ---------------------------------------------------------------- Fungsi aliran

        /// <summary>Aliran berkunci: hasil sama untuk seed dan kunci yang sama, tanpa alokasi.</summary>
        public RandomStream Fork(RandomStreamId id, int key)
        {
            WarnIfNoRun();
            return new RandomStream(DeriveSeed(CurrentSeed, id, unchecked((ulong)(uint)key)));
        }

        /// <summary>Aliran berkunci dengan kunci teks (hash FNV-1a 64-bit, bukan string.GetHashCode).</summary>
        public RandomStream Fork(RandomStreamId id, string key)
        {
            WarnIfNoRun();
            return new RandomStream(DeriveSeed(CurrentSeed, id, Fnv1a64(key)));
        }

        /// <summary>
        /// Aliran berkunci untuk seed tertentu, bukan CurrentSeed. Dipakai RunLayoutValidator (syarat 4) saat seed calon
        /// belum ditetapkan. Hasilnya sama persis dengan Fork(id, key) setelah CurrentSeed = seed.
        /// </summary>
        public static RandomStream ForkForSeed(uint seed, RandomStreamId id, string key)
        {
            return new RandomStream(DeriveSeed(seed, id, Fnv1a64(key)));
        }

        /// <summary>
        /// Aliran berurutan. Pemanggil WAJIB memakai "ref var s = ref RunSeedManager.Instance.GetStream(id);",
        /// karena menyalin ke variabel biasa membuat status tidak maju.
        /// </summary>
        public ref RandomStream GetStream(RandomStreamId id)
        {
            WarnIfNoRun();
            return ref sequential[(int)id];
        }

        private void WarnIfNoRun()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (CurrentSeed == 0)
                Debug.LogWarning("[RunSeedManager] Aliran acak dipakai sebelum run dimulai (CurrentSeed = 0).");
#endif
        }

        private static ulong DeriveSeed(uint seed, RandomStreamId id, ulong keyHash)
        {
            // Campur seed, id, dan kunci lewat finalizer SplitMix64.
            ulong h = Mix(seed + 0x9E3779B97F4A7C15UL);
            h = Mix(h ^ (((ulong)id + 1UL) * 0xD1B54A32D192ED03UL));
            h = Mix(h ^ keyHash);
            return h;
        }

        private static ulong Mix(ulong z)
        {
            unchecked
            {
                z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
                z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
                return z ^ (z >> 31);
            }
        }

        private static ulong Fnv1a64(string text)
        {
            unchecked
            {
                ulong hash = 14695981039346656037UL;
                if (text == null) return hash;
                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    hash ^= (byte)(c & 0xFF);
                    hash *= 1099511628211UL;
                    hash ^= (byte)(c >> 8);
                    hash *= 1099511628211UL;
                }
                return hash;
            }
        }

        // ---------------------------------------------------------------- Susunan run

        private RunLayout ResolveLayout(ref uint seed, bool isForced)
        {
            UsedFallbackLayout = false;
            var sources = layoutSources;

            // Kompatibilitas: tanpa RunLayoutSources, perilaku lama (jadwal bencana saja, tanpa pemeriksaan).
            if (sources == null)
            {
                Debug.LogWarning("[RunSeedManager] layoutSources belum diisi di Inspector. Suku cadang dipilih HeavyItemSystem, " +
                                 "NPC kapal tidak muncul, dan susunan tidak diperiksa (#89).", this);
                return BuildLayout(seed, null, disasterData);
            }

            uint originalSeed = seed;
            int maxRerolls = MaxSeedRerolls();
            string failure = null;
            for (int attempt = 0; attempt <= maxRerolls; attempt++)
            {
                // 1. Bangun dan periksa.
                RunLayout layout = BuildLayout(seed, sources, DisasterDataSource);
                if (RunLayoutValidator.Validate(layout, seed, sources, out failure))
                    return layout;

                // 2. Seed dari DevConsole dipakai apa adanya, karena tujuannya mengulang bug.
                if (isForced)
                {
                    Debug.LogWarning($"[RunSeedManager] Seed paksa {FormatSeed(seed)} gagal pemeriksaan ({failure}). Tetap dipakai.");
                    return layout;
                }

                // 3. Seed yang lolos adalah seed yang ditampilkan, jadi mengulangnya langsung memberi susunan sama.
                Debug.Log($"[RunSeedManager] Seed {FormatSeed(seed)} gagal ({failure}). Mencoba seed berikutnya.");
                seed = DeriveNextSeed(seed);
            }

            // 4. Semua gagal: safeLayout supaya pemain tetap bisa bermain. Tafsiran: seed awal tetap ditampilkan, supaya
            // memasukkan seed itu lewat DevConsole mengulang rantai percobaan yang sama. Aliran acak memakai seed itu juga.
            seed = originalSeed;
            UsedFallbackLayout = true;
            Debug.LogError($"[RunSeedManager] Semua seed gagal pemeriksaan (terakhir: {failure}). Memakai safeLayout.", this);
            var safe = sources.CloneSafeLayout();
            if (safe.disasterScheduleThroughDay <= 0)
            {
                // Tafsiran: safeLayout tanpa jadwal diisi blok pertama dari seed. DisasterScheduler sudah menaati syarat 6.
                int block = ScheduleBlockDays();
                var s = new RandomStream(DeriveSeed(seed, RandomStreamId.Disaster, 0UL));
                DisasterScheduler.Build(ref s, DisasterDataSource, 1, block, null, out var schedule);
                safe.disasterSchedule = schedule;
                safe.disasterScheduleThroughDay = block;
            }
            return safe;
        }

        /// <summary>
        /// Fungsi murni (RunSeedManager.md "Membangun susunan run yang valid"): hasil hanya ditentukan seed dan sumber, tanpa
        /// membaca scene. Aliran memakai seed parameter (bukan CurrentSeed), karena dipanggil sebelum seed ditetapkan.
        /// DeriveSeed(seed, id, 0) sama dengan Fork(id, 0) setelah CurrentSeed = seed.
        /// sources null = perilaku lama: hanya jadwal bencana dari disasters.
        /// </summary>
        public static RunLayout BuildLayout(uint seed, RunLayoutSources sources, DisasterData[] disasters)
        {
            var layout = new RunLayout();

            if (sources != null)
            {
                // 1. Suku cadang: Fork(PartLocation, 0), Shuffle, ambil helicopterPartCount titik yang memenuhi jarak minimal.
                int partCount = Mathf.Max(0, RegionBal.helicopterPartCount);
                var ps = new RandomStream(DeriveSeed(seed, RandomStreamId.PartLocation, 0UL));
                var picked = new int[partCount];
                int filled = PickPartPoints(sources.partSpawnPoints, partCount, RunSeedBal.minPartPointSpacing, ref ps, picked);
                // Titik kurang: hasil dipotong, dan syarat 1 menolaknya (percobaan seed lain, lalu safeLayout).
                if (filled < partCount) Array.Resize(ref picked, filled);
                layout.partPointByPart = picked;

                // 2. NPC: Fork(NPC, 0), pilih NPC dari daftar #76, pasangkan ke titik kapal buatan tangan.
                var ns = new RandomStream(DeriveSeed(seed, RandomStreamId.NPC, 0UL));
                PickShipNpcs(sources, ref ns, out layout.shipNpcIds, out layout.shipNpcPointByNpc);
            }

            // 3. Bencana: blok pertama, hari 1 sampai scheduleBlockDays, lewat Fork(Disaster, 0) (Langkah 7, #70).
            int block = ScheduleBlockDays();
            var s = new RandomStream(DeriveSeed(seed, RandomStreamId.Disaster, 0UL));
            DisasterScheduler.Build(ref s, disasters, 1, block, null, out var schedule);
            layout.disasterSchedule = schedule;
            layout.disasterScheduleThroughDay = block;
            return layout;
        }

        /// <summary>
        /// Pemilihan titik suku cadang (BuildLayout langkah 1). Shuffle semua indeks, lalu ambil titik sah (isReachable)
        /// yang berjarak minimal minSpacing dari titik terpilih sebelumnya. Mengembalikan jumlah titik yang terisi.
        /// Dipakai juga cadangan HeavyItemSystem saat layoutSources kosong, supaya aturannya hanya ada di satu tempat.
        /// </summary>
        public static int PickPartPoints(PartSpawnPointTable table, int count, float minSpacing, ref RandomStream s, int[] result)
        {
            int n = table != null ? table.Count : 0;
            if (result == null || count <= 0 || n == 0) return 0;
            var order = new int[n];
            for (int i = 0; i < n; i++) order[i] = i;
            s.Shuffle(order);

            float minSq = minSpacing * minSpacing;
            int filled = 0;
            for (int k = 0; k < n && filled < count && filled < result.Length; k++)
            {
                int p = order[k];
                if (!table.IsValidIndex(p)) continue;   // Syarat 2: titik isReachable = false tidak dipakai
                bool farEnough = true;
                for (int j = 0; j < filled; j++)
                    if ((table.points[result[j]].position - table.points[p].position).sqrMagnitude < minSq) { farEnough = false; break; }
                if (farEnough) result[filled++] = p;
            }
            return filled;
        }

        /// <summary>
        /// Jumlah NPC kapal per run: runSeed.shipNpcCount, dibatasi jumlah NPC di sumber. Tafsiran sementara (pertanyaan ke
        /// developer): nilai awal 3 = semua NPC #76, jadi yang berubah tiap run adalah posisinya.
        /// </summary>
        public static int ShipNpcTargetCount(RunLayoutSources sources)
        {
            if (sources == null || sources.shipNpcs == null) return 0;
            int valid = 0;
            for (int i = 0; i < sources.shipNpcs.Length; i++)
                if (sources.shipNpcs[i] != null && !string.IsNullOrEmpty(sources.shipNpcs[i].npcId)) valid++;
            return Mathf.Clamp(RunSeedBal.shipNpcCount, 0, valid);
        }

        private static void PickShipNpcs(RunLayoutSources sources, ref RandomStream s, out string[] ids, out int[] points)
        {
            var npcs = sources.shipNpcs ?? Array.Empty<NPCData>();
            var pts = sources.shipNpcPoints ?? Array.Empty<ShipNpcPointInfo>();
            int target = ShipNpcTargetCount(sources);

            // Urutan aliran tetap: acak NPC dulu, lalu titik.
            var npcOrder = new int[npcs.Length];
            for (int i = 0; i < npcOrder.Length; i++) npcOrder[i] = i;
            s.Shuffle(npcOrder);
            var pointOrder = new int[pts.Length];
            for (int i = 0; i < pointOrder.Length; i++) pointOrder[i] = i;
            s.Shuffle(pointOrder);

            float maxWalk = RunSeedBal.maxShipNpcWalkSeconds;
            var idList = new System.Collections.Generic.List<string>(target);
            var pointList = new System.Collections.Generic.List<int>(target);
            int nextPoint = 0;
            for (int k = 0; k < npcOrder.Length && idList.Count < target; k++)
            {
                var data = npcs[npcOrder[k]];
                if (data == null || string.IsNullOrEmpty(data.npcId) || idList.Contains(data.npcId)) continue;
                // Titik yang gagal syarat 5 dilewati. Titik kurang: NPC dipotong, dan syarat 5 menolak susunan ini.
                while (nextPoint < pointOrder.Length && !RunLayoutValidator.IsShipNpcPointUsable(pts[pointOrder[nextPoint]], maxWalk)) nextPoint++;
                if (nextPoint >= pointOrder.Length) break;
                idList.Add(data.npcId);
                pointList.Add(pointOrder[nextPoint++]);
            }
            ids = idList.ToArray();
            points = pointList.ToArray();
        }

        /// <summary>
        /// Lanjutkan jadwal bencana per blok tanpa batas (RunSeedManager.md, #70). Blok ke-n memakai Fork(Disaster, n),
        /// jadi hasilnya hanya bergantung pada seed dan nomor blok, bukan kapan fungsi dipanggil. Entri baru hanya
        /// ditambahkan di belakang, sehingga indeks DisasterManager.nextIndex tetap sah. Ditulis Langkah 7.
        /// </summary>
        public void EnsureDisasterScheduleThrough(int day)
        {
            var layout = CurrentLayout;
            if (layout == null || CurrentSeed == 0) return;
            int block = ScheduleBlockDays();
            if (block <= 0) return;
            if (layout.disasterSchedule == null) layout.disasterSchedule = Array.Empty<DisasterScheduleEntry>();

            while (layout.disasterScheduleThroughDay < day)
            {
                int fromDay = layout.disasterScheduleThroughDay + 1;
                int toDay = layout.disasterScheduleThroughDay + block;
                int blockIndex = layout.disasterScheduleThroughDay / block;
                var s = Fork(RandomStreamId.Disaster, blockIndex);
                DisasterScheduler.Build(ref s, DisasterDataSource, fromDay, toDay, layout.disasterSchedule, out var added);
                if (added.Length > 0)
                {
                    var merged = new DisasterScheduleEntry[layout.disasterSchedule.Length + added.Length];
                    Array.Copy(layout.disasterSchedule, merged, layout.disasterSchedule.Length);
                    Array.Copy(added, 0, merged, layout.disasterSchedule.Length, added.Length);
                    layout.disasterSchedule = merged;
                }
                layout.disasterScheduleThroughDay = toDay;
            }
        }

        private static int ScheduleBlockDays()
        {
            var bal = GameManager.CurrentBalance;
            return bal != null ? bal.disaster.scheduleBlockDays : 60;
        }

        private static int MaxSeedRerolls()
        {
            var gm = GameManager.Instance;
            return gm != null && gm.Balance != null ? gm.Balance.runSeed.maxSeedRerolls : 0;
        }

        private static RunSeedBalance RunSeedBal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.runSeed : FallbackRunSeed;
        private static RegionBalance RegionBal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.region : FallbackRegion;

        // ---------------------------------------------------------------- Save dan load

        /// <summary>Salin seed, status aliran berurutan, dan susunan run. Dipanggil WorldStateSave (#80).</summary>
        public RunSeedSaveData CaptureSave()
        {
            var states = new ulong[sequential.Length];
            for (int i = 0; i < sequential.Length; i++) states[i] = sequential[i].State;

            return new RunSeedSaveData
            {
                version = SaveDataVersion,
                seed = CurrentSeed,
                sequentialStates = states,
                layout = CurrentLayout,
                usedFallbackLayout = UsedFallbackLayout
            };
        }

        /// <summary>
        /// Dipanggil RunLifecycle.ContinueRun. Susunan dibaca dari save dan TIDAK dibangun ulang,
        /// supaya perubahan data tidak memindahkan suku cadang yang sudah tercatat.
        /// </summary>
        public void RestoreFromSave(RunSeedSaveData data)
        {
            if (data.version != SaveDataVersion)
                Debug.LogWarning($"[RunSeedManager] Versi data seed {data.version} berbeda dari {SaveDataVersion}. Aturan versi #80 berlaku.");

            CurrentSeed = data.seed;
            CurrentSeedText = FormatSeed(data.seed);
            CurrentLayout = data.layout ?? new RunLayout();
            UsedFallbackLayout = data.usedFallbackLayout;

            sequential = new RandomStream[StreamCount];
            for (int i = 0; i < StreamCount; i++)
            {
                bool hasState = data.sequentialStates != null && i < data.sequentialStates.Length;
                sequential[i] = hasState
                    ? new RandomStream(data.sequentialStates[i])
                    : new RandomStream(DeriveSeed(data.seed, (RandomStreamId)i, 0));
            }
        }
    }
}
