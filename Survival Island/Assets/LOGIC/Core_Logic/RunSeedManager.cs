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
    /// Belum diimplementasikan di Langkah 1 (butuh data dari Langkah lain):
    /// - Isi BuildLayout (suku cadang #69, NPC kapal #76, jadwal bencana #70) dan RunLayoutValidator.
    ///   Keduanya butuh RunLayoutSources (PartSpawnPointTable, daftar NPC, DisasterData) yang belum ada.
    ///   Untuk sementara BuildLayout mengembalikan susunan kosong dan pemeriksaan selalu lolos.
    /// - EnsureDisasterScheduleThrough (DisasterScheduler, Langkah 7).
    /// - Alat Editor RunSeedAudit.
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

        // Ditambahkan Langkah 7 (#70): sumber DisasterData untuk jadwal bencana. Pengganti sementara
        // RunLayoutSources.disasterData sampai RunLayoutSources ditulis (RunSeedManager.md bagian 1).
        [Tooltip("Aset DisasterData (Eruption, Earthquake, TropicalStorm). Pindah ke RunLayoutSources saat ada.")]
        [SerializeField] private DisasterData[] disasterData = Array.Empty<DisasterData>();

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
            int maxRerolls = MaxSeedRerolls();

            for (int attempt = 0; attempt <= maxRerolls; attempt++)
            {
                RunLayout layout = BuildLayout(seed);
                if (ValidateLayout(layout, seed, out string failure))
                    return layout;

                if (isForced)
                {
                    // Seed dari DevConsole dipakai apa adanya, karena tujuannya mengulang bug.
                    Debug.LogWarning($"[RunSeedManager] Seed paksa {FormatSeed(seed)} gagal pemeriksaan ({failure}). Tetap dipakai.");
                    return layout;
                }

                seed = DeriveNextSeed(seed);
            }

            // Semua gagal: pakai safeLayout supaya pemain tetap bisa bermain.
            Debug.LogError("[RunSeedManager] Semua seed gagal pemeriksaan. Memakai safeLayout.");
            UsedFallbackLayout = true;
            // TODO (RunLayoutSources belum ada): ganti dengan RunLayoutSources.safeLayout.
            return new RunLayout();
        }

        /// <summary>
        /// Fungsi murni: hasil hanya ditentukan seed dan RunLayoutSources.
        /// TODO Langkah 6/7: isi suku cadang (#69), NPC kapal (#76), dan blok jadwal bencana pertama (#70)
        /// setelah RunLayoutSources, PartSpawnPointTable, dan DisasterData tersedia.
        /// </summary>
        private RunLayout BuildLayout(uint seed)
        {
            var layout = new RunLayout();
            // Langkah 7 (#70): blok pertama jadwal bencana, hari 1 sampai scheduleBlockDays, lewat Fork(Disaster, 0).
            // Memakai seed parameter (bukan CurrentSeed), karena BuildLayout dipanggil sebelum seed ditetapkan.
            int block = ScheduleBlockDays();
            var s = new RandomStream(DeriveSeed(seed, RandomStreamId.Disaster, 0UL));
            DisasterScheduler.Build(ref s, disasterData, 1, block, null, out var schedule);
            layout.disasterSchedule = schedule;
            layout.disasterScheduleThroughDay = block;
            return layout;
        }

        /// <summary>TODO Langkah 6/7: RunLayoutValidator.Validate (6 syarat di RunSeedManager.md).</summary>
        private static bool ValidateLayout(RunLayout layout, uint seed, out string failure)
        {
            failure = null;
            return layout != null;
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
                DisasterScheduler.Build(ref s, disasterData, fromDay, toDay, layout.disasterSchedule, out var added);
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
