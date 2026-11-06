using System;
using UnityEngine;

namespace SurvivalIsland
{
    public enum GateKind { Bridge, DiaryChapter, FollowsPrevious }

    /// <summary>Aturan satu seam. Data, jadi mengubah usulan syarat tidak mengubah kode (RegionGateSystem.md).</summary>
    [Serializable]
    public class GateRule
    {
        public string seamId;          // S1, S2, S3 (RegionStreaming.md)
        public RegionType unlocks;     // Amazon, Logistic, Snow
        public GateKind kind;
        public int diaryChapter;       // Dipakai GateKind.DiaryChapter
        public GameObject[] barriers = Array.Empty<GameObject>();   // GateBarrier dan GateBlocker milik seam ini
    }

    /// <summary>
    /// Pemilik status akses tiap region, setoran kayu jembatan, dan pengaman supaya pemain tidak melewati seam tertutup.
    /// Blueprint: Island_Logic/RegionGateSystem.md (#68). GDD bagian 4 (Diary Bab 2 memberi Blueprint Jembatan) dan
    /// bagian 5 (akses bertahap, jembatan gantung, bridgeWoodCost nilai awal 500). Claude.md Tahap 4, Region 2.
    /// Syarat Region 3 dan 4 adalah usulan disetujui developer (#85).
    ///
    /// Penyimpangan sementara (dicatat di blueprint, Catatan Implementasi Langkah 6):
    /// - RegionStreaming.GetRegionAt dan RegionBoundaryGuard.LastSafePosition (#88) belum ada. Region pemain dibaca dari
    ///   ZoneTracker.Active, dan posisi aman dicatat sendiri di sini (LastSafePosition).
    /// - DiaryChapterUnlockedEvent (#81) didengar dan diteruskan ke HandleDiaryChapter(int).
    /// - NavMeshLink seam (#88) dan DevConsole (#56) belum ada.
    /// </summary>
    public class RegionGateSystem : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "world_gates";

        public static RegionGateSystem Instance { get; private set; }

        [SerializeField] private GateRule[] rules = Array.Empty<GateRule>();

        private int woodDeposited;
        private bool hasBridgeBlueprint;
        private bool bridgeComplete;
        private int unlockedMask = 1 << (int)RegionType.Volcano;
        private float verifyTimer;
        private bool runEnded;
        private bool registeredForSave;
        private Vector3 lastSafePosition;
        private bool hasSafePosition;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;
        private static int BridgeWoodCost => Balance != null ? Mathf.Max(0, Balance.region.bridgeWoodCost) : 500;

        public int WoodDeposited => woodDeposited;
        public int WoodRequired => BridgeWoodCost;
        public int WoodRemaining => Mathf.Max(0, BridgeWoodCost - woodDeposited);
        public bool HasBridgeBlueprint => hasBridgeBlueprint;
        public bool BridgeComplete => bridgeComplete;

        /// <summary>Posisi terakhir pemain di region yang terbuka. Pengganti sementara RegionBoundaryGuard (#88).</summary>
        public Vector3 LastSafePosition => lastSafePosition;
        public bool HasSafePosition => hasSafePosition;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
            if (rules == null || rules.Length == 0) rules = DefaultRules();
        }

        private void OnEnable()
        {
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            EventChannel<DiaryChapterUnlockedEvent>.Raised += HandleDiaryUnlocked;   // #81
            if (WorldStateSave.Instance != null)
            {
                WorldStateSave.Instance.Register(this);
                registeredForSave = true;
            }
            ApplyBarriers();
        }

        private void Start()
        {
            EventChannel<BridgeProgressChangedEvent>.Publish(new BridgeProgressChangedEvent(woodDeposited, BridgeWoodCost));
        }

        private void OnDisable()
        {
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            EventChannel<DiaryChapterUnlockedEvent>.Raised -= HandleDiaryUnlocked;
            if (registeredForSave && WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            registeredForSave = false;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void HandleRunEnded(RunEndedEvent e) => runEnded = true;

        /// <summary>Tabel nilai awal (usulan disetujui #85). barriers diisi di Inspector.</summary>
        private static GateRule[] DefaultRules()
        {
            return new[]
            {
                new GateRule { seamId = "S1", unlocks = RegionType.Amazon, kind = GateKind.Bridge },
                new GateRule { seamId = "S2", unlocks = RegionType.Logistic, kind = GateKind.DiaryChapter, diaryChapter = 3 },
                new GateRule { seamId = "S3", unlocks = RegionType.Snow, kind = GateKind.FollowsPrevious },
            };
        }

        // ---------------------------------------------------------------- Fungsi publik

        public bool IsUnlocked(RegionType r) => (unlockedMask & (1 << (int)r)) != 0;

        /// <summary>Setor kayu. Mengembalikan jumlah yang diterima. Dipanggil BridgeSite.</summary>
        public int Deposit(int amount)
        {
            if (bridgeComplete || amount <= 0) return 0;
            int accepted = Mathf.Min(amount, WoodRemaining);
            if (accepted <= 0) return 0;
            woodDeposited += accepted;
            EventChannel<BridgeProgressChangedEvent>.Publish(new BridgeProgressChangedEvent(woodDeposited, BridgeWoodCost));
            TryCompleteBridge();
            return accepted;
        }

        /// <summary>Juga menangani bridgeWoodCost yang diturunkan di bawah woodDeposited (dipanggil saat Interact dan pemeriksaan).</summary>
        public void TryCompleteBridge()
        {
            if (!bridgeComplete && hasBridgeBlueprint && WoodRemaining == 0) CompleteBridge();
        }

        private void CompleteBridge()
        {
            // 1.
            bridgeComplete = true;
            // 2. BridgeSite mendengar BridgeProgressChangedEvent dan RegionUnlockedEvent, lalu menyegarkan visual sendiri.
            EventChannel<BridgeProgressChangedEvent>.Publish(new BridgeProgressChangedEvent(woodDeposited, BridgeWoodCost));
            // 3. RegionUnlockedEvent diterbitkan sekali oleh Unlock. Notifikasi "Akses baru terbuka" berasal dari event itu.
            Unlock(RegionType.Amazon);
            // 4.
            EventChannel<SaveRequestedEvent>.Publish(new SaveRequestedEvent(SaveReason.BridgeComplete));
        }

        /// <summary>Buka region, matikan barrier seam-nya, lalu terbitkan RegionUnlockedEvent. Juga untuk DevConsole (#56).</summary>
        public void Unlock(RegionType r)
        {
            int bit = 1 << (int)r;
            if ((unlockedMask & bit) != 0) return;
            unlockedMask |= bit;
            ApplyBarriers();
            EventChannel<RegionUnlockedEvent>.Publish(new RegionUnlockedEvent(r));
        }

        /// <summary>
        /// Bab Diary terbuka. Bab 2 = Blueprint Jembatan (GDD bagian 4). Bab 3 membuka S2, lalu S3 ikut terbuka (usulan #85).
        /// Dipanggil lewat DiaryChapterUnlockedEvent (#81). Idempoten, jadi aman dipanggil langsung juga.
        /// </summary>
        private void HandleDiaryUnlocked(DiaryChapterUnlockedEvent e) => HandleDiaryChapter(e.chapter);

        public void HandleDiaryChapter(int chapter)
        {
            // 1. Usulan 4: Blueprint Jembatan berupa penanda, bukan item, supaya tidak bisa hilang.
            if (chapter == 2) hasBridgeBlueprint = true;

            // 2. Aturan DiaryChapter dengan bab yang sama, lalu aturan FollowsPrevious sesudahnya.
            for (int i = 0; i < rules.Length; i++)
            {
                var rule = rules[i];
                if (rule == null || rule.kind != GateKind.DiaryChapter || rule.diaryChapter != chapter) continue;
                Unlock(rule.unlocks);
                for (int j = i + 1; j < rules.Length && rules[j] != null && rules[j].kind == GateKind.FollowsPrevious; j++)
                    Unlock(rules[j].unlocks);
            }
            TryCompleteBridge();
        }

        private void ApplyBarriers()
        {
            if (rules == null) return;
            for (int i = 0; i < rules.Length; i++)
            {
                var rule = rules[i];
                if (rule?.barriers == null) continue;
                bool open = IsUnlocked(rule.unlocks);
                for (int b = 0; b < rule.barriers.Length; b++)
                    if (rule.barriers[b] != null) rule.barriers[b].SetActive(!open);
            }
        }

        // ---------------------------------------------------------------- Pemeriksaan posisi

        private void Update()
        {
            var bal = Balance;
            if (bal == null || runEnded) return;
            verifyTimer += Time.deltaTime;   // Game dijeda: ikut berhenti
            if (verifyTimer < bal.region.gateVerifySeconds) return;
            verifyTimer = 0f;
            VerifyPlayerRegion();
            TryCompleteBridge();
        }

        /// <summary>Lapis 6 pencegah jurang: kalau pemain berada di region tertutup (bug fisika, teleport), kembalikan.</summary>
        private void VerifyPlayerRegion()
        {
            var player = IslandPlayerController.Instance;
            if (player == null || (RegionStreaming.Instance == null && !ZoneTracker.HasActive)) return;

            // Region pemain dari RegionStreaming (#88), cadangan ZoneTracker.Active.
            RegionType here = RegionStreaming.RegionOf(player.transform.position);
            if (IsUnlocked(here))
            {
                if (player.IsGrounded)
                {
                    lastSafePosition = player.transform.position;
                    hasSafePosition = true;
                }
                return;
            }
            if (!hasSafePosition) return;
            Debug.LogWarning($"[RegionGateSystem] Pemain berada di region tertutup {here}. Dikembalikan ke posisi aman.");
            player.Teleport(lastSafePosition);
        }

        // ---------------------------------------------------------------- Save (#80)

        [Serializable]
        private struct GateSaveData
        {
            public int woodDeposited;
            public bool bridgeComplete;
            public int unlockedMask;
            public bool hasBridgeBlueprint;
        }

        public string SaveId => SaveIdConst;
        public int LoadOrder => 30;

        public string CaptureState()
        {
            return JsonUtility.ToJson(new GateSaveData
            {
                woodDeposited = woodDeposited,
                bridgeComplete = bridgeComplete,
                unlockedMask = unlockedMask,
                hasBridgeBlueprint = hasBridgeBlueprint,
            });
        }

        public void RestoreState(string json)
        {
            // Tidak mengirim event.
            if (string.IsNullOrEmpty(json)) return;
            var d = JsonUtility.FromJson<GateSaveData>(json);
            woodDeposited = Mathf.Max(0, d.woodDeposited);
            bridgeComplete = d.bridgeComplete;
            unlockedMask = d.unlockedMask | (1 << (int)RegionType.Volcano);
            hasBridgeBlueprint = d.hasBridgeBlueprint;
            ApplyBarriers();
        }

        public void OnRestoreFinished()
        {
            EventChannel<BridgeProgressChangedEvent>.Publish(new BridgeProgressChangedEvent(woodDeposited, BridgeWoodCost));
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        /// <summary>DevConsole (#56): depositwood jumlah. Melewati syarat Blueprint.</summary>
        public void DebugDeposit(int amount)
        {
            hasBridgeBlueprint = true;
            Deposit(amount);
        }
#endif
    }
}
