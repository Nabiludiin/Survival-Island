using System;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    public enum PartState { Lying, Carried, Installed }

    public enum DropReason { Manual, HeavyHit, Pinned, Water, Death, Installed, Unstuck }

    /// <summary>Status satu suku cadang. Tetap ada walaupun region dilepas.</summary>
    [Serializable]
    public class PartRecord
    {
        public string partId;
        public PartState state;
        public Vector3 position;
        public Quaternion rotation = Quaternion.identity;
        public Vector3 lastValidPosition;
        public RegionType region = RegionType.Logistic;
        public bool isDiscovered;
        public int spawnPointIndex = -1;   // Titik asal di PartSpawnPointTable, cadangan ReturnToValid
    }

    /// <summary>
    /// Mengatur suku cadang helikopter sejak tersebar di Region 4, diangkut dan dijatuhkan, sampai tiba di titik pemasangan.
    /// Blueprint: Island_Logic/HeavyItemSystem.md (#69). GDD bagian 1 dan 5, Region 4: suku cadang tersebar, tidak bisa
    /// dibawa sekaligus, dan bolak-balik ke Region 3. Claude.md Tahap 4: saat digenggam kecepatan setengah, Sprint dan Jump mati.
    ///
    /// Pembagian efek saat membawa: kecepatan lewat SpeedModifierId.HeavyItem (di sini), Sprint dan Jump dimatikan
    /// IslandPlayerController, regenerasi stamina oleh SurvivalStats, kunci senjata dan obor oleh EquipmentManager.
    /// Ketiganya mendengar HeavyItemChangedEvent (Langkah 3), jadi file mereka tidak diubah.
    ///
    /// Penyimpangan sementara (Catatan Implementasi Langkah 6 di blueprint):
    /// - #89 (keputusan developer 7 November 2026): titik dibaca dari RunSeedManager.CurrentLayout.partPointByPart, yang diisi
    ///   BuildLayout dan diperiksa RunLayoutValidator. Kalau layout kosong atau tidak sah (RunLayoutSources belum diisi), cadangan
    ///   lama tetap jalan dengan peringatan lewat RunSeedManager.PickPartPoints (aturan yang sama, satu tempat).
    /// - RegionStreaming (#88): barang Lying hanya aktif kalau regionnya Loaded (RegionLoadStateChangedEvent), dan
    ///   RecoveryTick memeriksa batas bawah RegionRecord.bounds. Tanpa RegionStreaming (scene uji), semua barang aktif.
    ///   Data barang tetap di world_parts (manager IslandPhase), bukan RegionStateStore.
    /// - SwimStateChangedEvent (#13, cadangan PlayerTraversal.Mode), CompassSystem (#30), LoreHintGrantedEvent (#100), dan
    ///   notifikasi (#22) sudah disambung. Debu saat barang dijatuhkan lewat VFXManager (#15), tag dropDustTag.
    /// </summary>
    public class HeavyItemSystem : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "world_parts";

        public static HeavyItemSystem Instance { get; private set; }

        [SerializeField] private HeavyItemData[] parts = Array.Empty<HeavyItemData>();
        [SerializeField] private PartSpawnPointTable spawnPoints;
        [Tooltip("Efek debu saat barang dijatuhkan (HeavyItemSystem.md \"Objek pool\"). Tag dari katalog VFXManager.")]
        [SerializeField] private string dropDustTag = "VFX_ImpactDust";
        [Tooltip("Titik pegang di depan kamera pemain. Diisi dari prefab pemain Fase 2.")]
        [SerializeField] private Transform carryAnchor;
        [Tooltip("Induk barang yang tergeletak. Kosong = objek ini.")]
        [SerializeField] private Transform partsRoot;

        private PartRecord[] records = Array.Empty<PartRecord>();
        private HeavyItem[] instances = Array.Empty<HeavyItem>();
        private bool built;
        private bool runEnded;
        private bool registeredForSave;
        private float validSampleTimer;
        private float recoveryTimer;
        private PlayerTraversal playerTraversal;

        public int CarriedIndex { get; private set; } = -1;
        public bool IsCarrying => CarriedIndex >= 0;
        public int PartCount => records.Length;
        public PartRecord GetRecord(int i) => i >= 0 && i < records.Length ? records[i] : null;
        public PartState GetState(int i) => i >= 0 && i < records.Length ? records[i].state : PartState.Installed;

        public int InstalledCount
        {
            get
            {
                int n = 0;
                for (int i = 0; i < records.Length; i++) if (records[i].state == PartState.Installed) n++;
                return n;
            }
        }

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;
        private Transform Root => partsRoot != null ? partsRoot : transform;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
            // #89: tabel titik harus sama dengan RunLayoutSources.partSpawnPoints, karena partPointByPart berisi indeks tabel itu.
            var sources = RunSeedManager.Instance != null ? RunSeedManager.Instance.LayoutSources : null;
            if (sources != null && sources.partSpawnPoints != null)
            {
                if (spawnPoints == null) spawnPoints = sources.partSpawnPoints;
                else if (spawnPoints != sources.partSpawnPoints)
                    Debug.LogWarning("[HeavyItemSystem] spawnPoints berbeda dari RunLayoutSources.partSpawnPoints. Indeks layout bisa salah.", this);
            }
        }

        private void OnEnable()
        {
            EventChannel<RunStartedEvent>.Raised += HandleRunStarted;
            EventChannel<PlayerDamagedEvent>.Raised += HandleDamaged;
            EventChannel<PlayerPinnedEvent>.Raised += HandlePinned;
            EventChannel<PlayerDiedEvent>.Raised += HandleDied;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            EventChannel<LoreHintGrantedEvent>.Raised += HandleLoreHint;   // #100
            // SwimStateChangedEvent (#13) dibaca dari nilai sticky saat membawa barang.
            EventChannel<RegionLoadStateChangedEvent>.Raised += HandleRegionLoadState;   // #88
            if (WorldStateSave.Instance != null)
            {
                WorldStateSave.Instance.Register(this);
                registeredForSave = true;
            }
        }

        private void Start()
        {
            // RunStartedEvent sticky: scene bisa dimuat setelah event terkirim.
            if (!built && EventChannel<RunStartedEvent>.HasLast && EventChannel<RunStartedEvent>.Last.isNewRun) BuildRecords();
        }

        private void OnDisable()
        {
            EventChannel<RunStartedEvent>.Raised -= HandleRunStarted;
            EventChannel<PlayerDamagedEvent>.Raised -= HandleDamaged;
            EventChannel<PlayerPinnedEvent>.Raised -= HandlePinned;
            EventChannel<PlayerDiedEvent>.Raised -= HandleDied;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            EventChannel<LoreHintGrantedEvent>.Raised -= HandleLoreHint;
            EventChannel<RegionLoadStateChangedEvent>.Raised -= HandleRegionLoadState;
            if (registeredForSave && WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            registeredForSave = false;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        /// <summary>Titik pegang dari prefab pemain (dipanggil saat pemain muncul, atau diisi di Inspector).</summary>
        public void SetCarryAnchor(Transform anchor) => carryAnchor = anchor;

        // ---------------------------------------------------------------- Penyebaran di Region 4

        private void HandleRunStarted(RunStartedEvent e)
        {
            if (e.isNewRun && !built) BuildRecords();
        }

        private void BuildRecords()
        {
            built = true;
            int count = parts != null ? parts.Length : 0;
            records = new PartRecord[count];
            int[] pointByPart = ResolvePoints(count);
            for (int i = 0; i < count; i++)
            {
                // 2.
                int p = pointByPart[i];
                var pt = spawnPoints != null && p >= 0 && p < spawnPoints.Count ? spawnPoints.points[p] : default;
                records[i] = new PartRecord
                {
                    partId = parts[i] != null ? parts[i].partId : $"part_{i}",
                    state = PartState.Lying,
                    position = pt.position,
                    rotation = Quaternion.Euler(pt.eulerRotation),
                    lastValidPosition = pt.position,
                    region = RegionType.Logistic,
                    spawnPointIndex = p,
                };
            }
            // 3. Dibuat sekali lewat Instantiate. Barang bukan objek pool, karena jumlahnya tetap dan tidak pernah dibuang.
            CreateInstances();
        }

        /// <summary>Indeks titik per barang. Utamakan RunSeedManager.CurrentLayout; kalau kosong, pilih dengan aturan RunSeedManager.md.</summary>
        private int[] ResolvePoints(int count)
        {
            var result = new int[count];
            var layout = RunSeedManager.Instance != null ? RunSeedManager.Instance.CurrentLayout : null;
            bool layoutOk = layout != null && layout.partPointByPart != null && layout.partPointByPart.Length >= count;
            if (layoutOk)
            {
                for (int i = 0; i < count; i++)
                {
                    int p = layout.partPointByPart[i];
                    if (spawnPoints == null || !spawnPoints.IsValidIndex(p)) { layoutOk = false; break; }
                    result[i] = p;
                }
                if (layoutOk) return result;
            }

            // Cadangan kompatibilitas (#89): BuildLayout kini mengisi partPointByPart. Jalur ini hanya untuk
            // RunSeedManager.layoutSources kosong, scene uji tanpa RunSeedManager, atau save lama yang titiknya tidak sah.
            // Aturan pemilihan tidak lagi ditulis di sini: memakai RunSeedManager.PickPartPoints dengan Fork(PartLocation, 0).
            Debug.LogWarning("[HeavyItemSystem] CurrentLayout.partPointByPart kosong atau tidak sah (RunLayoutSources belum diisi?). " +
                             "Titik dipilih dengan cadangan lama.", this);
            // Scene uji tanpa RunSeedManager: aliran tetap dari seed 0, jadi hasilnya tetap sama tiap kali dicoba.
            var s = RunSeedManager.Instance != null ? RunSeedManager.Instance.Fork(RandomStreamId.PartLocation, 0) : new RandomStream(0UL);
            float minSpacing = Balance != null ? Balance.runSeed.minPartPointSpacing : 30f;
            int filled = RunSeedManager.PickPartPoints(spawnPoints, count, minSpacing, ref s, result);
            if (filled < count)
            {
                Debug.LogError($"[HeavyItemSystem] Titik suku cadang tidak cukup ({filled} / {count}). Sisa barang memakai titik pertama yang sah.");
                for (int i = filled; i < count; i++) result[i] = FirstValidPoint();
            }
            return result;
        }

        private int FirstValidPoint()
        {
            if (spawnPoints == null) return -1;
            for (int i = 0; i < spawnPoints.Count; i++) if (spawnPoints.IsValidIndex(i)) return i;
            return spawnPoints.Count > 0 ? 0 : -1;
        }

        private void CreateInstances()
        {
            DestroyInstances();
            instances = new HeavyItem[records.Length];
            for (int i = 0; i < records.Length; i++)
            {
                var data = i < parts.Length ? parts[i] : null;
                if (data == null || data.prefab == null)
                {
                    Debug.LogError($"[HeavyItemSystem] Prefab suku cadang indeks {i} kosong.");
                    continue;
                }
                var r = records[i];
                var go = Instantiate(data.prefab, r.position, r.rotation, Root);
                go.name = $"part_{i}_{r.partId}";
                var item = go.GetComponent<HeavyItem>();
                if (item == null) item = go.AddComponent<HeavyItem>();
                item.Bind(i);
                instances[i] = item;
                // Barang tergeletak dimatikan sampai regionnya (awalnya Region4_Village) berstatus Loaded (#88).
                // Barang Carried dipasang ke pemain di OnRestoreFinished, jadi tetap aktif.
                bool show = r.state == PartState.Carried
                    || (r.state == PartState.Lying && RegionStreaming.IsRegionLoaded(r.region));
                go.SetActive(show);
            }
        }

        private void DestroyInstances()
        {
            for (int i = 0; i < instances.Length; i++)
                if (instances[i] != null) Destroy(instances[i].gameObject);
            instances = Array.Empty<HeavyItem>();
        }

        // ---------------------------------------------------------------- Mengambil

        public bool PickUp(int index)
        {
            if (IsCarrying || index < 0 || index >= records.Length || records[index].state != PartState.Lying) return false;
            var item = instances[index];
            var player = IslandPlayerController.Instance;
            if (item == null || player == null) return false;

            // 1. Hanya satu CarriedIndex, jadi pemain hanya bisa membawa satu barang.
            CarriedIndex = index;
            records[index].state = PartState.Carried;
            // 2. Trigger supaya titik pasang (#14) tetap mendeteksi tag HeavyItem, dan barang tidak mendorong apa pun.
            AttachToAnchor(item, player.transform);
            // 3. GDD bagian 5, Region 4: kecepatan setengah. Pengali, bukan speed /= 2, jadi tidak berkurang berulang.
            player.SetSpeedModifier(SpeedModifierId.HeavyItem, ResolveMultiplier(index));
            validSampleTimer = 0f;
            // 4.
            EventChannel<HeavyItemChangedEvent>.Publish(new HeavyItemChangedEvent(records[index].partId, true));
            return true;
        }

        private void AttachToAnchor(HeavyItem item, Transform player)
        {
            var anchor = carryAnchor != null ? carryAnchor : player;
            item.transform.SetParent(anchor, false);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            if (item.Body != null)
            {
                item.Body.linearVelocity = Vector3.zero;
                item.Body.angularVelocity = Vector3.zero;
                item.Body.isKinematic = true;
            }
            if (item.MainCollider != null) item.MainCollider.isTrigger = true;
        }

        private float ResolveMultiplier(int index)
        {
            var data = index < parts.Length ? parts[index] : null;
            // Usulan 2 (disetujui): semua 0,5 pada versi awal. Override tersedia kalau Mesin ingin lebih lambat.
            if (data != null && data.speedMultiplierOverride > 0f) return data.speedMultiplierOverride;
            return Balance != null ? Balance.region.heavyItemSpeedMultiplier : 0.5f;
        }

        // ---------------------------------------------------------------- Melepas dan menjatuhkan

        public void Drop(DropReason reason)
        {
            // 1.
            if (!IsCarrying) return;
            int index = CarriedIndex;
            var item = instances[index];
            var player = IslandPlayerController.Instance;

            // 2-3.
            if (item != null)
            {
                item.transform.SetParent(Root, true);
                if (item.MainCollider != null) item.MainCollider.isTrigger = false;
                bool physics = reason != DropReason.Installed;
                if (item.Body != null) item.Body.isKinematic = !physics;
                if (player != null && physics)
                {
                    Vector3 pos = DropPosition(player.transform, reason);
                    item.transform.SetPositionAndRotation(pos, Quaternion.Euler(0f, player.transform.eulerAngles.y, 0f));
                    // Debu saat jatuh (#15). Di air barang langsung dipindah ke posisi valid, jadi tidak ada debu.
                    if (reason != DropReason.Water && !string.IsNullOrEmpty(dropDustTag))
                        VFXManager.Instance?.Play(dropDustTag, pos, Quaternion.identity);
                }
                records[index].position = item.transform.position;
                records[index].rotation = item.transform.rotation;
                // Barang jatuh di sisi tempat pemain berada (#88 Kasus Khusus "Serangan saat menyeberang seam").
                records[index].region = RegionStreaming.RegionOf(item.transform.position);
            }

            // 4.
            if (player != null) player.ClearSpeedModifier(SpeedModifierId.HeavyItem);
            records[index].state = PartState.Lying;
            CarriedIndex = -1;

            // 5.
            EventChannel<HeavyItemChangedEvent>.Publish(new HeavyItemChangedEvent(records[index].partId, false));

            // Berenang: barang langsung kembali ke posisi valid (usulan 4).
            if (reason == DropReason.Water) ReturnToValid(index);
        }

        private Vector3 DropPosition(Transform player, DropReason reason)
        {
            var bal = Balance;
            float dist = bal != null ? bal.heavyItem.dropForwardDistance : 1.2f;
            Vector3 feet = player.position;
            if (reason == DropReason.Pinned) return feet;
            Vector3 chest = feet + Vector3.up * 1f;
            Vector3 fwd = Vector3.ProjectOnPlane(player.forward, Vector3.up).normalized;
            if (PhysicsQuery.Raycast(new Ray(chest, fwd), out _, dist, PhysicsMasks.InteractBlock)) return feet;
            return feet + fwd * dist;
        }

        private void HandleDamaged(PlayerDamagedEvent e)
        {
            var bal = Balance;
            if (!IsCarrying || bal == null) return;
            // Usulan 3 (disetujui): damage 25 ke atas atau ledakan. Zombie biasa tidak menjatuhkan barang.
            if (e.info.amount >= bal.heavyItem.dropDamageThreshold || e.info.type == DamageType.Explosion) Drop(DropReason.HeavyHit);
        }

        private void HandlePinned(PlayerPinnedEvent e)
        {
            if (e.isPinned) Drop(DropReason.Pinned);
        }

        private void HandleDied(PlayerDiedEvent e)
        {
            // Supaya barang tidak terlihat melayang di DeathCinematic (#53).
            Drop(DropReason.Death);
        }

        private void HandleRunEnded(RunEndedEvent e) => runEnded = true;

        /// <summary>
        /// Petunjuk lore PartLocation (#100): barang Lying dengan indeks terkecil yang belum ditemukan menjadi ditemukan dan
        /// mendapat penanda kompas. Diabaikan kalau semua sudah ditemukan.
        /// </summary>
        private void HandleLoreHint(LoreHintGrantedEvent e)
        {
            if (e.hintKind != LoreHintKind.PartLocation || records == null) return;
            for (int i = 0; i < records.Length; i++)
            {
                var r = records[i];
                if (r == null || r.state != PartState.Lying || r.isDiscovered) continue;
                r.isDiscovered = true;
                SetPartMarker(r);
                return;
            }
        }

        /// <summary>Penanda kompas part_&lt;partId&gt; (#30).</summary>
        private static void SetPartMarker(PartRecord r)
        {
            if (CompassSystem.Instance != null && r != null) CompassSystem.Instance.SetMarker("part_" + r.partId, r.position);
        }

        // ---------------------------------------------------------------- Update: tombol Q dan pengaman

        private void Update()
        {
            var bal = Balance;
            if (bal == null || runEnded) return;
            var player = IslandPlayerController.Instance;

            if (IsCarrying)
            {
                // Tombol Q. Terkunci saat jeda dan menu (UIFocusStack.IsGameplayInputBlocked).
                var input = InputManager.Instance;
                bool blocked = UIFocusStack.Instance != null && UIFocusStack.Instance.IsGameplayInputBlocked;
                if (!blocked && input != null && input.DropHeavyItemPressed)
                {
                    Drop(DropReason.Manual);
                    return;
                }

                // SwimStateChangedEvent.isSwimming (#13). Air dangkal yang masih bisa dilewati tidak menjatuhkan barang.
                // Cadangan tanpa SwimmingController: PlayerTraversal.Mode.
                var trav = Traversal(player);
                bool swimming = EventChannel<SwimStateChangedEvent>.HasLast
                    ? EventChannel<SwimStateChangedEvent>.Last.isSwimming
                    : trav != null && trav.Mode == TraversalMode.Swimming;
                if (swimming)
                {
                    Drop(DropReason.Water);
                    return;
                }

                validSampleTimer += Time.deltaTime;
                if (validSampleTimer >= bal.heavyItem.validSampleSeconds)
                {
                    validSampleTimer = 0f;
                    SampleValidPosition(player, trav, bal.heavyItem);
                }
            }

            recoveryTimer += Time.deltaTime;
            if (recoveryTimer >= bal.heavyItem.recoveryCheckSeconds)
            {
                recoveryTimer = 0f;
                RecoveryTick(player, bal.heavyItem);
            }
        }

        private PlayerTraversal Traversal(IslandPlayerController player)
        {
            if (playerTraversal == null && player != null) playerTraversal = player.GetComponent<PlayerTraversal>();
            return playerTraversal;
        }

        private void SampleValidPosition(IslandPlayerController player, PlayerTraversal trav, HeavyItemBalance hb)
        {
            if (player == null || !IsCarrying) return;
            // 1-3. Di tanah, mode Ground, ada NavMesh, dan tidak di air.
            if (!player.IsGrounded || (trav != null && trav.Mode != TraversalMode.Ground)) return;
            if (!NavMesh.SamplePosition(player.transform.position, out var hit, hb.navSampleRadius, NavMesh.AllAreas)) return;
            records[CarriedIndex].lastValidPosition = hit.position;
        }

        private void RecoveryTick(IslandPlayerController player, HeavyItemBalance hb)
        {
            Vector3 playerPos = player != null ? player.transform.position : Vector3.zero;
            float discoverSqr = hb.discoverRadius * hb.discoverRadius;
            for (int i = 0; i < records.Length; i++)
            {
                var r = records[i];
                var item = i < instances.Length ? instances[i] : null;
                if (r.state != PartState.Lying || item == null || !item.gameObject.activeInHierarchy) continue;

                // 1. Barang yang masih bergerak dilewati, kecuali sudah di bawah RegionRecord.bounds.min.y (#88).
                Vector3 pos = item.transform.position;
                bool belowRegion = IsBelowRegion(r.region, pos);
                if (!belowRegion && item.Body != null && !item.Body.isKinematic && !item.Body.IsSleeping()) continue;

                // 2-3.
                if (belowRegion || !NavMesh.SamplePosition(pos, out _, hb.navSampleRadius, NavMesh.AllAreas))
                {
                    ReturnToValid(i);
                    continue;
                }
                r.position = pos;
                r.rotation = item.transform.rotation;
                r.lastValidPosition = pos;
                r.region = RegionStreaming.RegionOf(pos);

                // Penemuan: barang dalam discoverRadius ditandai (usulan 5), dan penanda kompas mengikuti posisinya (#30).
                if (player != null && !r.isDiscovered && (pos - playerPos).sqrMagnitude <= discoverSqr) r.isDiscovered = true;
                if (r.isDiscovered) SetPartMarker(r);
            }
        }

        /// <summary>
        /// Dipanggil TideManager.StepCollider (#91): barang Lying yang tertinggal di pantai di bawah permukaan air dipindah ke
        /// TideSafePoint terdekat, sama seperti WorldItemManager.SweepBelow. Barang yang dibawa pemain tidak diperiksa.
        /// </summary>
        public void SweepBelow(Bounds zone, float waterY, TideSafePoint[] safe)
        {
            bool moved = false;
            for (int i = 0; i < records.Length; i++)
            {
                var r = records[i];
                var item = i < instances.Length ? instances[i] : null;
                if (r == null || r.state != PartState.Lying || item == null || !item.gameObject.activeInHierarchy) continue;
                Vector3 p = item.transform.position;
                if (p.y >= waterY) continue;
                if (p.x < zone.min.x || p.x > zone.max.x || p.z < zone.min.z || p.z > zone.max.z) continue;
                var point = TideSafePoint.Nearest(safe, p);
                if (point == null) continue;   // Tanpa titik aman: dibiarkan, sama dengan WorldItemManager
                Vector3 target = point.Position + Vector3.up * 0.5f;
                if (item.Body != null)
                {
                    item.Body.linearVelocity = Vector3.zero;
                    item.Body.angularVelocity = Vector3.zero;
                }
                item.transform.position = target;
                if (item.Body != null) item.Body.position = target;
                r.position = target;
                r.region = RegionStreaming.RegionOf(target);
                moved = true;
            }
            // Notifikasi "Suku cadang dikembalikan" (#22), sekali per sapuan.
            if (moved && NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.part_returned", NotificationKind.Info);
        }

        /// <summary>Kembalikan barang ke posisi valid terakhir. Barang tidak pernah bisa hilang (#116 hanya membaca status).</summary>
        public void ReturnToValid(int index)
        {
            if (index < 0 || index >= records.Length) return;
            var r = records[index];
            var item = index < instances.Length ? instances[index] : null;
            Vector3 target = r.lastValidPosition;
            var hb = Balance != null ? Balance.heavyItem : null;
            float radius = hb != null ? hb.navSampleRadius : 2f;
            // Titik itu juga tidak valid: pakai titik asal dari PartSpawnPointTable.
            if (!NavMesh.SamplePosition(target, out _, radius, NavMesh.AllAreas) && spawnPoints != null && spawnPoints.IsValidIndex(r.spawnPointIndex))
                target = spawnPoints.points[r.spawnPointIndex].position;

            target += Vector3.up * 0.5f;
            if (item != null)
            {
                if (item.Body != null)
                {
                    item.Body.linearVelocity = Vector3.zero;
                    item.Body.angularVelocity = Vector3.zero;
                }
                item.transform.position = target;
                if (item.Body != null) item.Body.position = target;
            }
            r.position = target;
            r.region = RegionStreaming.RegionOf(target);
            // Notifikasi "Suku cadang dikembalikan" (#22).
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.part_returned", NotificationKind.Info);
        }

        /// <summary>Posisi di bawah RegionRecord.bounds.min.y (jatuh keluar peta). Tanpa RegionStreaming selalu false.</summary>
        private static bool IsBelowRegion(RegionType region, Vector3 pos)
        {
            var rs = RegionStreaming.Instance;
            return rs != null && rs.TryGetBounds(region, out var b) && b.size != Vector3.zero && pos.y < b.min.y;
        }

        // ---------------------------------------------------------------- Region dilepas dan dimuat (#88)

        /// <summary>
        /// HeavyItemSystem.md "Region dilepas dan dipindahkan": Unloading menyalin barang Lying di region itu ke records,
        /// membuatnya kinematic, lalu mematikannya. Loaded menyalakan barang di posisi tersimpan. Barang yang dibawa adalah
        /// anak pemain, jadi tidak terpengaruh.
        /// </summary>
        private void HandleRegionLoadState(RegionLoadStateChangedEvent e)
        {
            if (e.state != RegionLoadState.Unloading && e.state != RegionLoadState.Loaded) return;
            for (int i = 0; i < records.Length; i++)
            {
                var r = records[i];
                var item = i < instances.Length ? instances[i] : null;
                if (r == null || item == null || r.state != PartState.Lying) continue;

                if (e.state == RegionLoadState.Unloading)
                {
                    if (!item.gameObject.activeSelf) continue;
                    // Region dihitung dari posisi saat ini, karena barang bisa terguling melewati batas.
                    r.position = item.transform.position;
                    r.rotation = item.transform.rotation;
                    r.region = RegionStreaming.RegionOf(r.position);
                    if (r.region != e.region) continue;
                    if (item.Body != null)
                    {
                        item.Body.linearVelocity = Vector3.zero;
                        item.Body.angularVelocity = Vector3.zero;
                        item.Body.isKinematic = true;
                    }
                    item.gameObject.SetActive(false);
                }
                else
                {
                    if (r.region != e.region || item.gameObject.activeSelf) continue;
                    item.transform.SetPositionAndRotation(r.position, r.rotation);
                    item.gameObject.SetActive(true);
                    // Fisika kembali seperti barang yang dijatuhkan (Drop memakai isKinematic = false).
                    if (item.Body != null)
                    {
                        item.Body.position = r.position;
                        item.Body.rotation = r.rotation;
                        item.Body.isKinematic = false;
                    }
                }
            }
        }

        // ---------------------------------------------------------------- Titik pasang dan penanda

        /// <summary>Dipanggil RescueEventManager (#14, Langkah 7) saat trigger titik pasang menerima barang.</summary>
        public void MarkInstalled(string partId)
        {
            int i = IndexOf(partId);
            if (i < 0 || records[i].state == PartState.Installed) return;
            // 1. Menang saat membawa barang: lepas dulu, jadi pengali kecepatan tidak tertinggal.
            if (CarriedIndex == i) Drop(DropReason.Installed);
            // 2. Penanda kompas dihapus (#30).
            if (CompassSystem.Instance != null) CompassSystem.Instance.RemoveMarker("part_" + partId);
            records[i].state = PartState.Installed;
            if (i < instances.Length && instances[i] != null) instances[i].gameObject.SetActive(false);
            // 3. Perakitan dan kemenangan milik #14 (HelicopterPartInstalledEvent).
        }

        public int IndexOf(string partId)
        {
            for (int i = 0; i < records.Length; i++) if (records[i].partId == partId) return i;
            return -1;
        }

        /// <summary>Barang terdekat yang sudah ditemukan, tergeletak, dan belum dipasang (#81). Tetap bekerja saat region tidak dimuat.</summary>
        public bool TryGetNearestPart(Vector3 position, out Vector3 partPosition)
        {
            partPosition = default;
            float best = float.MaxValue;
            bool found = false;
            for (int i = 0; i < records.Length; i++)
            {
                var r = records[i];
                if (!r.isDiscovered || r.state != PartState.Lying) continue;
                float d = (r.position - position).sqrMagnitude;
                if (d >= best) continue;
                best = d;
                partPosition = r.position;
                found = true;
            }
            return found;
        }

        // ---------------------------------------------------------------- Save (#80)

        [Serializable]
        private struct PartsSaveData
        {
            public PartRecord[] records;
            public int carriedIndex;
        }

        public string SaveId => SaveIdConst;
        public int LoadOrder => 100;

        public string CaptureState()
        {
            // Salin posisi barang yang tergeletak sebelum disimpan.
            for (int i = 0; i < records.Length; i++)
            {
                var item = i < instances.Length ? instances[i] : null;
                if (item == null || records[i].state != PartState.Lying) continue;
                records[i].position = item.transform.position;
                records[i].rotation = item.transform.rotation;
            }
            return JsonUtility.ToJson(new PartsSaveData { records = records, carriedIndex = CarriedIndex });
        }

        public void RestoreState(string json)
        {
            // Tidak mengirim event.
            if (string.IsNullOrEmpty(json)) return;
            var d = JsonUtility.FromJson<PartsSaveData>(json);
            records = d.records ?? Array.Empty<PartRecord>();
            CarriedIndex = d.carriedIndex >= 0 && d.carriedIndex < records.Length && records[d.carriedIndex].state == PartState.Carried
                ? d.carriedIndex : -1;
            built = true;
            CreateInstances();
        }

        public void OnRestoreFinished()
        {
            // Barang Carried dipasang kembali ke carryAnchor setelah pemain dipulihkan (player_transform LoadOrder 80).
            var player = IslandPlayerController.Instance;
            if (IsCarrying && player != null && instances[CarriedIndex] != null)
            {
                AttachToAnchor(instances[CarriedIndex], player.transform);
                player.SetSpeedModifier(SpeedModifierId.HeavyItem, ResolveMultiplier(CarriedIndex));
                EventChannel<HeavyItemChangedEvent>.Publish(new HeavyItemChangedEvent(records[CarriedIndex].partId, true));
            }
            else
            {
                CarriedIndex = -1;
                for (int i = 0; i < records.Length; i++)
                    if (records[i].state == PartState.Carried) records[i].state = PartState.Lying;
                EventChannel<HeavyItemChangedEvent>.Publish(new HeavyItemChangedEvent(string.Empty, false));
            }
        }
    }
}
