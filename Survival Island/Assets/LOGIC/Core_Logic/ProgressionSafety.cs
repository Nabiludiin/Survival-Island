using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    public enum SafetySeverity { Ok, Fixed, Warning, Blocked }
    public enum UnstuckResult { Started, Busy, Cooldown, NotAllowed, NoSafePoint }

    /// <summary>Hasil satu pemeriksaan RunCheck (ProgressionSafety.md, #116).</summary>
    public struct SafetyFinding
    {
        public string checkId;
        public SafetySeverity severity;
        public string message;
        public SafetyFinding(string checkId, SafetySeverity severity, string message)
        {
            this.checkId = checkId; this.severity = severity; this.message = message;
        }
    }

    /// <summary>Titik aman buatan tangan per region. Harus di atas batas pasang (#91).</summary>
    [Serializable]
    public struct RegionSafePoint
    {
        public RegionType region;
        public Vector3 position;
    }

    /// <summary>
    /// Pengaman jalur menang: aturan yang menjaga run tetap bisa dimenangkan, pemeriksaan kondisi run berkala, dan jalan keluar
    /// saat pemain tersangkut. Blueprint: Core_Logic/ProgressionSafety.md (#116). GDD bagian 1, 3, 4 (jalur menang dan Diary).
    /// Pengaman ini usulan arsitektur yang disetujui developer (#85). Sistem ini membaca status sistem lain dan tidak memiliki
    /// data baru, jadi tidak ada yang disimpan. Tidak pernah mengakhiri run: akhir run tetap ditentukan GameManager.
    ///
    /// Manager scene IslandPhase (Fase 1 hanya 60 detik, jadi sistem ini tidak aktif di kapal).
    ///
    /// Catatan implementasi (sistem pendukung, juga di blueprint):
    /// - Batu kecil aktif dan penumbuhan paksa lewat ResourceRespawn.CountActiveStones dan ForceRegrowStones (#73),
    ///   yang menghitung PickupInteractable (Mat_Stone) di Region 1.
    /// - Tombol "Keluar dari Tersangkut" di PausePanel (kode lama) belum ada. Pemanggil memakai RequestUnstuck().
    /// - path_blocked memeriksa pusat struktur terhadap NoBuildVolume.IsBlocked.
    /// </summary>
    public class ProgressionSafety : MonoBehaviour
    {
        public const string StoneId = "Mat_Stone";
        public const string WoodId = "Mat_Wood";
        private static readonly string[] CoreItemIds = { "Base_Kail", "Base_Pistol", "Base_Pisau" };
        private static readonly string[] ToolIds = { "Tool_Axe", "Tool_Pickaxe" };

        public static ProgressionSafety Instance { get; private set; }

        [Tooltip("Satu titik per region, buatan tangan. Data, karena region bisa tidak dimuat.")]
        [SerializeField] private RegionSafePoint[] regionFallbacks = Array.Empty<RegionSafePoint>();

        private readonly List<SafetyFinding> lastReport = new List<SafetyFinding>(12);
        private Vector3[] safePoints = Array.Empty<Vector3>();
        private int safeCount;
        private int safeHead;
        private float sampleTimer;
        private int lastTimeWarningDay;
        private float unstuckCooldownLeft;
        private Coroutine unstuckRoutine;
        private bool unstuckCancelled;
        private bool firstCheckDone;
        private bool runEnded;
        private PlayerTraversal traversal;

        private float stuckTimer;
        private Vector3 stuckAnchor;
        private bool stuckHintShown;

        public IReadOnlyList<SafetyFinding> LastReport => lastReport;
        public float UnstuckCooldownLeft => unstuckCooldownLeft;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        // ---------------------------------------------------------------- Aturan KeyItem

        /// <summary>
        /// Item inti dan KeyItem tidak boleh keluar dari kepemilikan pemain. Satu aturan dipakai peti, drop, resep, dan
        /// Lucky Machine (ProgressionSafety.md "Perlindungan KeyItem").
        /// </summary>
        public static bool IsProtected(ItemData item) => item != null && (item.isCoreItem || item.type == ItemType.KeyItem);

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[ProgressionSafety] Instance ganda di scene IslandPhase.");
                enabled = false;
                return;
            }
            Instance = this;
            var b = Balance;
            safePoints = new Vector3[Mathf.Max(1, b != null ? b.safety.unstuckRingSize : 12)];
        }

        private void OnEnable()
        {
            EventChannel<DayChangedEvent>.Raised += HandleDayChanged;
            EventChannel<PlayerDamagedEvent>.Raised += HandlePlayerDamaged;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            // PlayerPinnedEvent sticky: dibaca lewat Last saat RequestUnstuck, tanpa pendengar.
        }

        private void Start()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            RegisterConsoleCommands();
#endif
        }

        private void OnDisable()
        {
            EventChannel<DayChangedEvent>.Raised -= HandleDayChanged;
            EventChannel<PlayerDamagedEvent>.Raised -= HandlePlayerDamaged;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void Update()
        {
            if (runEnded) return;
            var b = Balance;
            if (b == null) return;
            // Semua timer memakai Time.deltaTime: berhenti saat jeda.
            float dt = Time.deltaTime;
            if (unstuckCooldownLeft > 0f) unstuckCooldownLeft = Mathf.Max(0f, unstuckCooldownLeft - dt);

            var life = RunLifecycle.Instance;
            bool onIsland = life == null || life.State == RunState.OnIsland;
            if (!onIsland) return;

            // Sekali pada frame pertama OnIsland, termasuk setelah load (semua ISaveable sudah pulih).
            if (!firstCheckDone)
            {
                firstCheckDone = true;
                SeedSafePointsAfterLoad();
                RunCheck();
            }

            SampleSafePoint(b.safety, dt);
            TickStuckHint(b.safety, dt);
        }

        private void HandleDayChanged(DayChangedEvent e)
        {
            if (e.isReplay || runEnded) return;
            RunCheck();
        }

        private void HandlePlayerDamaged(PlayerDamagedEvent e)
        {
            // Hitung mundur batal tanpa memakai cooldown, supaya fitur tidak dipakai untuk kabur dari pertempuran.
            if (unstuckRoutine != null) unstuckCancelled = true;
        }

        private void HandleRunEnded(RunEndedEvent e)
        {
            runEnded = true;
            if (unstuckRoutine != null) StopCoroutine(unstuckRoutine);
            unstuckRoutine = null;
        }

        // ---------------------------------------------------------------- Pemeriksaan kondisi run

        /// <summary>Dipanggil saat DayChangedEvent, frame pertama OnIsland, dan perintah softlockcheck.</summary>
        public IReadOnlyList<SafetyFinding> RunCheck()
        {
            lastReport.Clear();
            Add("core_items", AuditCoreItems());
            Add("tool_path", EnsureToolPath());
            CheckParts();
            CheckBridgeAndGates();
            CheckPathBlocked();
            Add("time_budget", CheckTimeBudget());
            return lastReport;
        }

        private void Add(string id, SafetySeverity s, string message = "")
        {
            lastReport.Add(new SafetyFinding(id, s, message));
            if (s != SafetySeverity.Blocked) return;
            // Blocked berarti bug: catat dengan seed. Pesan "Terjadi masalah" hanya di build pengembangan.
            var seeds = RunSeedManager.Instance;
            Debug.LogError($"[ProgressionSafety] {id} Blocked: {message} (seed {(seeds != null ? seeds.CurrentSeedText : "?")})");
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.safety_problem", NotificationKind.Danger);
#endif
        }

        /// <summary>Pengaman bug: item inti yang hilang dari inventori dan dunia dikembalikan.</summary>
        private SafetySeverity AuditCoreItems()
        {
            var inv = InventoryManager.Instance;
            if (inv == null) return SafetySeverity.Ok;
            var result = SafetySeverity.Ok;
            for (int i = 0; i < CoreItemIds.Length; i++)
            {
                string id = CoreItemIds[i];
                if (inv.GetCount(id) > 0 || ExistsAsWorldItem(id)) continue;
                var data = ItemDatabase.Get(id);
                if (data == null) continue;
                int left = inv.AddItem(data, 1);
                if (left > 0 && WorldItemManager.Instance != null && IslandPlayerController.Instance != null)
                    WorldItemManager.Instance.Spawn(inv.CreateInstance(data), left, IslandPlayerController.Instance.transform.position + Vector3.up, false);
                Debug.LogWarning($"[ProgressionSafety] Item inti {id} hilang dan dikembalikan.");
                result = SafetySeverity.Fixed;
            }
            return result;
        }

        private static bool ExistsAsWorldItem(string itemId)
        {
            var wim = WorldItemManager.Instance;
            if (wim == null) return false;
            var list = wim.Active;
            for (int i = 0; i < list.Count; i++)
                if (list[i] != null && list[i].Instance != null && list[i].Instance.itemId == itemId) return true;
            return false;
        }

        /// <summary>Rantai alat minimum dari tangan kosong (ProgressionSafety.md "Alat dasar").</summary>
        private SafetySeverity EnsureToolPath()
        {
            var inv = InventoryManager.Instance;
            var chests = FindObjectsByType<StorageChest>(FindObjectsSortMode.None);
            // 1. Kapak atau Beliung yang tidak Destroyed, di inventori atau peti.
            for (int t = 0; t < ToolIds.Length; t++)
            {
                if (inv != null && HasUsableTool(inv.Slots, ToolIds[t])) return SafetySeverity.Ok;
                for (int c = 0; c < chests.Length; c++)
                    if (chests[c] != null && HasUsableTool(chests[c].Slots, ToolIds[t])) return SafetySeverity.Ok;
            }
            // 2. Mat_Stone >= 2 di inventori dan peti.
            int stones = inv != null ? inv.GetCount(StoneId) : 0;
            for (int c = 0; c < chests.Length; c++) if (chests[c] != null) stones += chests[c].CountItem(StoneId);
            if (stones >= 2) return SafetySeverity.Ok;
            // 3. Batu kecil aktif di Region 1 (#73).
            var respawn = ResourceRespawn.Instance;
            if (respawn == null)
            {
                Debug.LogWarning("[ProgressionSafety] ResourceRespawn tidak ada di scene, batu kecil tidak bisa diperiksa.");
                return SafetySeverity.Blocked;
            }
            if (respawn.CountActiveStones(RegionType.Volcano) > 0) return SafetySeverity.Ok;
            // 4.
            var b = Balance;
            int grown = respawn.ForceRegrowStones(RegionType.Volcano, b != null ? b.safety.minStonePickupsRegion1 : 4);
            Debug.LogWarning($"[ProgressionSafety] Jalur alat tertutup. {grown} batu kecil Region 1 ditumbuhkan lagi.");
            return SafetySeverity.Fixed;
        }

        private static bool HasUsableTool(IReadOnlyList<InventorySlot> slots, string itemId)
        {
            if (slots == null) return false;
            for (int i = 0; i < slots.Count; i++)
            {
                var s = slots[i];
                if (s == null || s.IsEmpty || s.instance.itemId != itemId) continue;
                if (DurabilitySystem.GetState(s.instance) != DurabilityState.Destroyed) return true;
            }
            return false;
        }

        private void CheckParts()
        {
            var heavy = HeavyItemSystem.Instance;
            var b = Balance;
            int expected = b != null ? b.region.helicopterPartCount : 5;
            if (heavy == null)
            {
                Add("parts_exist", SafetySeverity.Ok, "HeavyItemSystem tidak ada di scene ini");
                Add("parts_reachable", SafetySeverity.Ok);
                return;
            }
            Add("parts_exist", heavy.PartCount == expected ? SafetySeverity.Ok : SafetySeverity.Blocked,
                $"{heavy.PartCount}/{expected} suku cadang");

            // Hanya barang Lying di region yang dimuat. #69 memulihkan barang yang tidak terjangkau.
            var reach = SafetySeverity.Ok;
            string msg = string.Empty;
            for (int i = 0; i < heavy.PartCount; i++)
            {
                var r = heavy.GetRecord(i);
                if (r == null || r.state != PartState.Lying || !RegionStreaming.IsRegionLoaded(r.region)) continue;
                if (NavMesh.SamplePosition(r.position, out _, 2f, NavMesh.AllAreas)) continue;
                reach = SafetySeverity.Warning;
                msg = r.partId;
                heavy.ReturnToValid(i);
            }
            Add("parts_reachable", reach, msg);
        }

        private void CheckBridgeAndGates()
        {
            var gate = RegionGateSystem.Instance;
            if (gate == null)
            {
                Add("bridge", SafetySeverity.Ok, "RegionGateSystem tidak ada di scene ini");
                return;
            }
            // bridge: Blueprint ada, jembatan belum selesai, tetapi BridgeSite tidak aktif.
            var bridge = SafetySeverity.Ok;
            if (gate.HasBridgeBlueprint && !gate.BridgeComplete && RegionStreaming.IsRegionLoaded(RegionType.Volcano))
            {
                var sites = FindObjectsByType<BridgeSite>(FindObjectsSortMode.None);
                bool active = false;
                for (int i = 0; i < sites.Length; i++) if (sites[i] != null && sites[i].isActiveAndEnabled) active = true;
                if (!active) bridge = SafetySeverity.Blocked;
            }
            Add("bridge", bridge, bridge == SafetySeverity.Blocked ? "BridgeSite tidak aktif" : string.Empty);

            // gates: jembatan selesai berarti Amazon terbuka.
            bool gatesOk = !(gate.HasBridgeBlueprint && gate.BridgeComplete) || gate.IsUnlocked(RegionType.Amazon);
            Add("gates", gatesOk ? SafetySeverity.Ok : SafetySeverity.Blocked, gatesOk ? string.Empty : "Amazon terkunci");

            // gates_diary: Bab 3 berarti Logistic terbuka.
            var om = ObjectiveManager.Instance;
            bool diaryOk = om == null || !om.IsChapterUnlocked(3) || gate.IsUnlocked(RegionType.Logistic);
            Add("gates_diary", diaryOk ? SafetySeverity.Ok : SafetySeverity.Blocked, diaryOk ? string.Empty : "Logistic terkunci");
        }

        private void CheckPathBlocked()
        {
            // Tidak ada struktur di NoBuildVolume jalur wajib (BuildMenu.md). Jarang: hanya saat pemeriksaan.
            var nodes = FindObjectsByType<StructureNode>(FindObjectsSortMode.None);
            for (int i = 0; i < nodes.Length; i++)
            {
                var n = nodes[i];
                if (n == null || string.IsNullOrEmpty(n.StructureId)) continue;
                if (!NoBuildVolume.IsBlocked(n.transform.position)) continue;
                Add("path_blocked", SafetySeverity.Warning, n.StructureId);
                return;
            }
            Add("path_blocked", SafetySeverity.Ok);
        }

        // ---------------------------------------------------------------- Peringatan zombie menguat

        private float EstimateSecondsNeeded(SafetyBalance s)
        {
            float seconds = 0f;
            var gate = RegionGateSystem.Instance;
            // 1. Kayu.
            if (gate != null && !gate.IsUnlocked(RegionType.Amazon) && s.woodPerSecondEstimate > 0f)
                seconds += gate.WoodRemaining / s.woodPerSecondEstimate;
            // 2. Region yang belum terbuka.
            if (gate != null)
            {
                int locked = 0;
                if (!gate.IsUnlocked(RegionType.Amazon)) locked++;
                if (!gate.IsUnlocked(RegionType.Logistic)) locked++;
                if (!gate.IsUnlocked(RegionType.Snow)) locked++;
                seconds += s.regionTravelSeconds * locked;
            }
            // 3-4. Suku cadang dan pemanasan mesin.
            var rescue = RescueEventManager.Instance;
            var bal = Balance;
            int total = rescue != null ? rescue.TotalParts : (bal != null ? bal.region.helicopterPartCount : 5);
            int installed = rescue != null ? rescue.InstalledCount : 0;
            seconds += s.partRoundTripSeconds * Mathf.Max(0, total - installed);
            if (rescue == null || rescue.State != RescueState.Departed) seconds += bal != null ? bal.rescue.engineWarmupSeconds : 120f;
            // 5.
            return seconds * s.estimateMargin;
        }

        private SafetySeverity CheckTimeBudget()
        {
            var gm = GameManager.Instance;
            var bal = Balance;
            // Hari ke-60 bukan kekalahan: setelah target terlewat, time_budget selalu Ok.
            if (gm == null || bal == null || gm.IsPastTargetDay) return SafetySeverity.Ok;
            var rescue = RescueEventManager.Instance;
            if (rescue != null && rescue.State == RescueState.Departed) return SafetySeverity.Ok;

            float mult = bal.time.timeMultiplier > 0f ? bal.time.timeMultiplier : 96f;
            float dayLength = 24f * 3600f / mult;
            var dn = DayNightDirector.Instance;
            float hour = dn != null ? dn.CurrentTimeOfDay : DayPhaseTracker.CurrentHour;
            float remaining = (GameManager.TargetDay - gm.currentDay) * dayLength + (24f - hour) / 24f * dayLength;
            float needed = EstimateSecondsNeeded(bal.safety);
            if (remaining >= needed) return SafetySeverity.Ok;

            // Hanya pesan, sekali per hari. Game tetap berjalan, dan run tidak dipaksa berakhir.
            if (lastTimeWarningDay != gm.currentDay)
            {
                lastTimeWarningDay = gm.currentDay;
                int days = Mathf.Max(1, Mathf.CeilToInt((needed - remaining) / dayLength));
                if (NotificationManager.Instance != null)
                    NotificationManager.Instance.ShowFormat("notif.zombies_stronger", NotificationKind.Warning, "days", days.ToString());
            }
            return SafetySeverity.Warning;
        }

        // ---------------------------------------------------------------- Keluar dari posisi tersangkut

        private void SeedSafePointsAfterLoad()
        {
            // Setelah load: safePoints diisi posisi pemain, cooldown dan peringatan kembali 0.
            safeCount = 0;
            safeHead = 0;
            unstuckCooldownLeft = 0f;
            lastTimeWarningDay = 0;
            var player = IslandPlayerController.Instance;
            if (player != null && NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                PushSafePoint(hit.position);
        }

        private void SampleSafePoint(SafetyBalance s, float dt)
        {
            sampleTimer += dt;
            if (sampleTimer < s.unstuckSampleSeconds) return;
            sampleTimer = 0f;
            var player = IslandPlayerController.Instance;
            if (player == null || !player.IsGrounded) return;
            if (traversal == null) traversal = player.GetComponent<PlayerTraversal>();
            // Hanya di darat: titik riwayat selalu tempat yang pernah diinjak, di luar air (#91).
            if (traversal != null && traversal.Mode != TraversalMode.Ground) return;
            if (EventChannel<SwimStateChangedEvent>.HasLast && EventChannel<SwimStateChangedEvent>.Last.isSwimming) return;
            if (!NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, s.unstuckNavSampleRadius, NavMesh.AllAreas)) return;
            PushSafePoint(hit.position);
        }

        private void PushSafePoint(Vector3 p)
        {
            safePoints[safeHead] = p;
            safeHead = (safeHead + 1) % safePoints.Length;
            if (safeCount < safePoints.Length) safeCount++;
        }

        /// <summary>Dari tombol "Keluar dari Tersangkut" di panel Pause, lewat ConfirmDialog.</summary>
        public UnstuckResult RequestUnstuck(bool ignoreCooldown = false)
        {
            // 1.
            var life = RunLifecycle.Instance;
            if (runEnded || (life != null && life.State != RunState.OnIsland)) return UnstuckResult.NotAllowed;
            if (EventChannel<PlayerPinnedEvent>.HasLast && EventChannel<PlayerPinnedEvent>.Last.isPinned) return UnstuckResult.NotAllowed;
            if (GameManager.Instance != null && GameManager.Instance.IsRunEnding) return UnstuckResult.NotAllowed;
            // 2.
            if (!ignoreCooldown && unstuckCooldownLeft > 0f) return UnstuckResult.Cooldown;
            // 3.
            if (unstuckRoutine != null) return UnstuckResult.Busy;
            var player = IslandPlayerController.Instance;
            if (player == null || !TryPickPoint(player.transform.position, out _)) return UnstuckResult.NoSafePoint;
            // 4. Tutup Pause, lalu hitung mundur.
            var focus = UIFocusStack.Instance;
            if (focus != null && focus.TopPanel != null && focus.TopPanel.id == UIPanelId.Pause) focus.Close(focus.TopPanel, UICloseReason.User);
            unstuckCancelled = false;
            unstuckRoutine = StartCoroutine(UnstuckRoutine());
            return UnstuckResult.Started;
        }

        private IEnumerator UnstuckRoutine()
        {
            var b = Balance;
            float channel = b != null ? b.safety.unstuckChannelSeconds : 5f;
            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowFormat("notif.unstuck_countdown", NotificationKind.Info, "seconds", Mathf.CeilToInt(channel).ToString());
            // 1. Time.deltaTime: berhenti saat jeda. PlayerDamagedEvent membatalkan tanpa cooldown.
            float t = 0f;
            while (t < channel)
            {
                if (unstuckCancelled)
                {
                    unstuckRoutine = null;
                    if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.unstuck_cancelled", NotificationKind.Warning);
                    yield break;
                }
                t += Time.deltaTime;
                yield return null;
            }
            var player = IslandPlayerController.Instance;
            // 2.
            if (player == null || !TryPickPoint(player.transform.position, out Vector3 point))
            {
                unstuckRoutine = null;
                yield break;
            }
            // 3. Barang punya pengaman posisinya sendiri (#69).
            var heavy = HeavyItemSystem.Instance;
            if (heavy != null && heavy.IsCarrying) heavy.Drop(DropReason.Unstuck);
            // 4.
            player.Teleport(point);
            if (traversal == null) traversal = player.GetComponent<PlayerTraversal>();
            if (traversal != null) traversal.ResetFallTracking();
            // 5.
            unstuckCooldownLeft = b != null ? b.safety.unstuckCooldownSeconds : 180f;
            stuckTimer = 0f;
            stuckHintShown = false;
            unstuckRoutine = null;
        }

        /// <summary>safePoints terbaru yang cukup jauh, lalu regionFallbacks region saat ini, lalu titik pendaratan.</summary>
        private bool TryPickPoint(Vector3 stuckPos, out Vector3 point)
        {
            var b = Balance;
            float min = b != null ? b.safety.unstuckMinRewindDistance : 3f;
            for (int k = 0; k < safeCount; k++)
            {
                int idx = (safeHead - 1 - k + safePoints.Length * 2) % safePoints.Length;
                if ((safePoints[idx] - stuckPos).sqrMagnitude < min * min) continue;
                point = safePoints[idx];
                return true;
            }
            RegionType here = RegionStreaming.RegionOf(stuckPos);
            for (int i = 0; i < regionFallbacks.Length; i++)
            {
                if (regionFallbacks[i].region != here) continue;
                point = regionFallbacks[i].position;
                return true;
            }
            if (IslandArrival.Instance != null)
            {
                point = IslandArrival.Instance.LandingPosition;
                return true;
            }
            point = default;
            return false;
        }

        /// <summary>Input maju aktif, tetapi posisi berubah kurang dari ambang selama stuckHintSeconds: petunjuk sekali.</summary>
        private void TickStuckHint(SafetyBalance s, float dt)
        {
            var player = IslandPlayerController.Instance;
            var input = InputManager.Instance;
            var focus = UIFocusStack.Instance;
            if (player == null || input == null || (focus != null && focus.IsGameplayInputBlocked)) return;
            bool pushing = input.MoveInput.y > 0.5f;
            Vector3 pos = player.transform.position;
            if (!pushing || (pos - stuckAnchor).sqrMagnitude > s.stuckHintMoveThreshold * s.stuckHintMoveThreshold)
            {
                stuckAnchor = pos;
                stuckTimer = 0f;
                stuckHintShown = false;   // Kejadian baru
                return;
            }
            stuckTimer += dt;
            if (stuckHintShown || stuckTimer < s.stuckHintSeconds) return;
            stuckHintShown = true;
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.stuck_hint", NotificationKind.Info);
        }

        // ---------------------------------------------------------------- DevConsole (#56)

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private static bool consoleRegistered;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics() => consoleRegistered = false;

        private static void RegisterConsoleCommands()
        {
            var console = DevConsole.Instance;
            if (console == null || consoleRegistered) return;
            consoleRegistered = true;
            // Handler tidak menyimpan rujukan ke objek scene: selalu membaca Instance (DevConsole.md).
            console.Register(new ConsoleCommand("softlockcheck", "softlockcheck", "Menjalankan pemeriksaan jalur menang", 0, false, args =>
            {
                var ps = Instance;
                if (ps == null) return "ProgressionSafety tidak ada di scene ini";
                var report = ps.RunCheck();
                var sb = new System.Text.StringBuilder();
                for (int i = 0; i < report.Count; i++)
                    sb.Append(report[i].checkId).Append(": ").Append(report[i].severity).Append(' ').Append(report[i].message).Append('\n');
                return sb.ToString();
            }));
            console.Register(new ConsoleCommand("unstuck", "unstuck", "Keluar dari tersangkut tanpa cooldown", 0, true, args =>
            {
                var ps = Instance;
                return ps == null ? "ProgressionSafety tidak ada di scene ini" : ps.RequestUnstuck(true).ToString();
            }));
            console.Register(new ConsoleCommand("clearstones", "clearstones", "Menandai semua batu kecil Region 1 sebagai diambil", 0, true, args =>
            {
                var all = FindObjectsByType<PickupInteractable>(FindObjectsSortMode.None);
                int n = 0;
                for (int i = 0; i < all.Length; i++)
                {
                    if (!ResourceRespawn.IsSmallStone(all[i]) || RegionStreaming.RegionOf(all[i].transform.position) != RegionType.Volcano) continue;
                    all[i].DebugConsume();
                    n++;
                }
                return n + " batu kecil diambil";
            }));
            // breaktool <itemId> didaftarkan DevConsole sendiri (DevConsole.CmdBreakTool, #113, #56).
        }
#endif
    }
}
