using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu buku Diary dengan tiga bab yang terbuka bertahap, hadiah tiap bab, dan daftar tujuan HUD yang tetap maju walaupun
    /// pemain mengerjakannya tidak berurutan. Blueprint: Island_Logic/ObjectiveManager.md (#81).
    /// GDD bagian 4: satu buku Diary di shelter tua, Bab 1-3 terbuka bertahap, Bab 2 memberi Blueprint Jembatan.
    /// GDD bagian 1 dan 3: syarat menang. GDD bagian 5: urutan region dan logistik.
    /// Keputusan developer: Bab 1 saat buku ditemukan, Bab 2 saat obj_base selesai atau hari chapter2TriggerDay,
    /// Bab 3 saat masuk chapter3TriggerRegion. Pemicunya tidak boleh membuat softlock. Hadiah diberikan saat bab terbuka.
    ///
    /// Manager scene IslandPhase. Tujuan didorong event; Update hanya membatasi penyegaran progres kayu (2 kali per detik).
    /// DiaryEntry, DiaryReward, dan DiaryRewardKind sudah ada (DiaryEntry.cs) dan dipakai tanpa diulang.
    ///
    /// Catatan implementasi (sistem pendukung, juga di blueprint):
    /// - StructureRegistry tidak punya daftar publik. Jumlah dinding/pintu dihitung dari StructureBuiltEvent, dan dihitung ulang
    ///   lewat FindObjectsByType&lt;StructureNode&gt; saat load (pola BaseNavigation.RescanStructures).
    /// - RegionGateSystem mendengar DiaryChapterUnlockedEvent sendiri (#81), jadi tidak dipanggil langsung lagi.
    ///   HandleDiaryChapter tetap idempoten.
    /// - Tujuan cadangan memakai objectiveId "obj_fallback_book" dan "obj_fallback_bridge" pada ActiveObjectiveChangedEvent.
    ///   Judul HUD dibaca lewat GetTitle(objectiveId).
    /// </summary>
    public class ObjectiveManager : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "world_objectives";
        public const string FallbackBookId = "obj_fallback_book";
        public const string FallbackBridgeId = "obj_fallback_bridge";
        private const string DoorPrefix = "Door_";
        private const string DefaultWallPrefix = "Wall_";
        private const float ProgressRefreshSeconds = 0.5f;   // "paling sering 2 kali per detik" (blueprint, bukan penyetelan)

        public static ObjectiveManager Instance { get; private set; }

        [Tooltip("Bab 1 sampai 3, urut.")]
        [SerializeField] private DiaryEntry[] chapters = Array.Empty<DiaryEntry>();
        [Tooltip("Delapan tujuan. Diurutkan menurut order saat Awake.")]
        [SerializeField] private ObjectiveData[] objectives = Array.Empty<ObjectiveData>();
        [Tooltip("Penanda shelter tua. Data, karena region bisa tidak dimuat.")]
        [SerializeField] private Vector3 bookMarkerPosition;
        [Tooltip("Penanda BridgeSite. Data, karena region bisa tidak dimuat.")]
        [SerializeField] private Vector3 bridgeMarkerPosition;
        [SerializeField] private string fallbackBookTitleKey = "objective.find_diary";
        [SerializeField] private string fallbackBridgeTitleKey = "objective.cross_bridge";

        private bool bookFound;
        private bool chapter3RegionReached;
        private readonly List<int> unlockedChapters = new List<int>(3);
        private readonly List<int> grantedRewards = new List<int>(3);
        private readonly HashSet<string> completed = new HashSet<string>(StringComparer.Ordinal);

        private int wallCount;
        private int doorCount;
        private bool isCarryingPart;
        private bool runEnded;
        private bool progressDirty;
        private bool recountPending;
        private float nextProgressTime;
        private bool registeredForSave;

        private string activeId;
        private int activeCurrent = -1, activeTarget = -1;
        private bool markerHas;
        private Vector3 markerPos;
        private string markerLabel;
        private bool markerPublished;

        public ObjectiveData ActiveObjective { get; private set; }
        public bool BookFound => bookFound;
        public IReadOnlyList<int> UnlockedChapters => unlockedChapters;
        public IReadOnlyList<ObjectiveData> Objectives => objectives;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[ObjectiveManager] Instance ganda di scene IslandPhase.");
                enabled = false;
                return;
            }
            Instance = this;
            if (objectives == null) objectives = Array.Empty<ObjectiveData>();
            Array.Sort(objectives, (a, b) => (a != null ? a.order : int.MaxValue).CompareTo(b != null ? b.order : int.MaxValue));
        }

        private void OnEnable()
        {
            EventChannel<DayChangedEvent>.Raised += HandleDayChanged;
            EventChannel<StructureBuiltEvent>.Raised += HandleStructureBuilt;
            EventChannel<StructureDestroyedEvent>.Raised += HandleStructureDestroyed;
            EventChannel<InventoryChangedEvent>.Raised += HandleInventoryChanged;
            EventChannel<RegionUnlockedEvent>.Raised += HandleRegionUnlocked;
            EventChannel<ItemCraftedEvent>.Raised += HandleItemCrafted;
            EventChannel<RegionChangedEvent>.Raised += HandleRegion;
            EventChannel<HelicopterPartInstalledEvent>.Raised += HandlePartInstalled;
            EventChannel<HelicopterReadyEvent>.Raised += HandleHelicopterReady;
            EventChannel<HeavyItemChangedEvent>.Raised += HandleHeavyItemChanged;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            EventChannel<BridgeProgressChangedEvent>.Raised += HandleBridgeProgress;
            if (EventChannel<HeavyItemChangedEvent>.HasLast) isCarryingPart = EventChannel<HeavyItemChangedEvent>.Last.isCarried;
            if (WorldStateSave.Instance != null)
            {
                WorldStateSave.Instance.Register(this);
                registeredForSave = true;
            }
            HookDiaryManager();
            RefreshAll();
        }

        private void Start()
        {
            // DiaryManager bisa Awake sesudah kelas ini.
            HookDiaryManager();
        }

        private void OnDisable()
        {
            EventChannel<DayChangedEvent>.Raised -= HandleDayChanged;
            EventChannel<StructureBuiltEvent>.Raised -= HandleStructureBuilt;
            EventChannel<StructureDestroyedEvent>.Raised -= HandleStructureDestroyed;
            EventChannel<InventoryChangedEvent>.Raised -= HandleInventoryChanged;
            EventChannel<RegionUnlockedEvent>.Raised -= HandleRegionUnlocked;
            EventChannel<ItemCraftedEvent>.Raised -= HandleItemCrafted;
            EventChannel<RegionChangedEvent>.Raised -= HandleRegion;
            EventChannel<HelicopterPartInstalledEvent>.Raised -= HandlePartInstalled;
            EventChannel<HelicopterReadyEvent>.Raised -= HandleHelicopterReady;
            EventChannel<HeavyItemChangedEvent>.Raised -= HandleHeavyItemChanged;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            EventChannel<BridgeProgressChangedEvent>.Raised -= HandleBridgeProgress;
            if (registeredForSave && WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            registeredForSave = false;
            var dm = DiaryManager.Instance;
            if (dm != null && dm.ChapterUnlockedQuery == (Func<int, bool>)IsChapterUnlocked) dm.ChapterUnlockedQuery = null;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void HookDiaryManager()
        {
            // DiaryArchivePanel membaca bab terbuka lewat DiaryManager.IsChapterUnlocked.
            var dm = DiaryManager.Instance;
            if (dm != null) dm.ChapterUnlockedQuery = IsChapterUnlocked;
        }

        private void Update()
        {
            // Progres kayu dari InventoryChangedEvent, paling sering 2 kali per detik.
            if (!progressDirty || Time.unscaledTime < nextProgressTime) return;
            progressDirty = false;
            if (recountPending)
            {
                recountPending = false;
                RecountStructures();
            }
            nextProgressTime = Time.unscaledTime + ProgressRefreshSeconds;
            RefreshAll();
        }

        // ---------------------------------------------------------------- Fungsi publik

        public bool IsChapterUnlocked(int chapter) => unlockedChapters.Contains(chapter);

        public bool IsCompleted(string objectiveId) => !string.IsNullOrEmpty(objectiveId) && completed.Contains(objectiveId);

        public DiaryEntry GetChapter(int chapter)
        {
            for (int i = 0; i < chapters.Length; i++) if (chapters[i] != null && chapters[i].chapter == chapter) return chapters[i];
            return null;
        }

        /// <summary>Judul HUD dari objectiveId, termasuk tujuan cadangan. Teks dari #104.</summary>
        public string GetTitle(string objectiveId)
        {
            if (objectiveId == FallbackBookId) return Localize(fallbackBookTitleKey);
            if (objectiveId == FallbackBridgeId) return Localize(fallbackBridgeTitleKey);
            var o = Find(objectiveId);
            return o != null ? Localize(o.title) : string.Empty;
        }

        /// <summary>Dipanggil DiaryBook. Penemuan pertama membuka Bab 1, berikutnya membuka arsip.</summary>
        public void InteractBook()
        {
            var dm = DiaryManager.Instance;
            // 1.
            if (!bookFound)
            {
                bookFound = true;
                RefreshAll();
                // OpenDiary memakai UIFocusStack.Push(diaryPanel), sehingga game dijeda (Claude.md Tahap 5).
                if (dm != null) dm.OpenDiary(GetChapter(1));
                return;
            }
            // 2.
            if (dm != null) dm.OpenArchive();
        }

        // ---------------------------------------------------------------- Bab Diary

        private void EvaluateChapterTriggers()
        {
            if (!bookFound) return;
            var bal = Balance;
            int day = GameManager.Instance != null ? GameManager.Instance.currentDay : 1;
            int ch2Day = bal != null ? bal.objective.chapter2TriggerDay : 2;
            // Bab 1: selalu, karena buku sudah ditemukan.
            UnlockChapter(1);
            // Bab 2: hari selalu berjalan, jadi Blueprint Jembatan pasti didapat (tanpa softlock).
            if (day >= ch2Day || completed.Contains("obj_base") || chapter3RegionReached) UnlockChapter(2);
            // Bab 3: otomatis saat masuk chapter3TriggerRegion.
            if (chapter3RegionReached) UnlockChapter(3);
        }

        private void UnlockChapter(int chapter)
        {
            if (unlockedChapters.Contains(chapter)) return;
            // 1.
            unlockedChapters.Add(chapter);
            // 2. Hadiah saat bab terbuka, bukan saat dibaca.
            GrantRewards(chapter);
            // 3. NotificationManager menampilkan "Halaman baru di Diary". RegionGateSystem mengisi Blueprint Jembatan (Bab 2).
            EventChannel<DiaryChapterUnlockedEvent>.Publish(new DiaryChapterUnlockedEvent(chapter));
        }

        private void GrantRewards(int chapter)
        {
            if (grantedRewards.Contains(chapter)) return;
            grantedRewards.Add(chapter);
            var entry = GetChapter(chapter);
            if (entry == null || entry.rewards == null) return;
            var inv = InventoryManager.Instance;
            for (int i = 0; i < entry.rewards.Length; i++)
            {
                var r = entry.rewards[i];
                // Blueprint Jembatan bukan entri rewards (RegionGateSystem, #68). RevealObjectives cukup lewat unlockedChapters.
                if (r.kind != DiaryRewardKind.GiveItem || r.item == null || r.amount <= 0 || inv == null) continue;
                // Usulan disetujui 1: Bab 1 memberi Kit_Campfire x1.
                int left = inv.AddItem(r.item, r.amount);
                if (left > 0) DropNearPlayer(inv, r.item, left);
            }
        }

        private static void DropNearPlayer(InventoryManager inv, ItemData item, int amount)
        {
            // Inventori penuh: sisa menjadi WorldItem di dekat pemain tanpa kedaluwarsa (#66, #116).
            var wim = WorldItemManager.Instance;
            var player = IslandPlayerController.Instance;
            if (wim == null || player == null) return;
            Vector3 pos = player.transform.position + player.transform.forward * 1f + Vector3.up * 0.5f;
            wim.Spawn(inv.CreateInstance(item), amount, pos, false);
        }

        // ---------------------------------------------------------------- Tujuan

        private bool IsVisible(ObjectiveData o) => o.revealedByChapter == 0 || unlockedChapters.Contains(o.revealedByChapter);

        private ObjectiveData Find(string id)
        {
            for (int i = 0; i < objectives.Length; i++)
                if (objectives[i] != null && string.Equals(objectives[i].objectiveId, id, StringComparison.Ordinal)) return objectives[i];
            return null;
        }

        private static int BridgeWoodCost => Balance != null ? Balance.region.bridgeWoodCost : 500;

        private bool Evaluate(ObjectiveData o)
        {
            var gate = RegionGateSystem.Instance;
            switch (o.condition)
            {
                case ObjectiveCondition.BuildCount:
                {
                    int need = Balance != null ? Balance.objective.baseWallCount : 4;
                    return wallCount >= need && doorCount >= 1;
                }
                case ObjectiveCondition.ItemTotal:
                {
                    if (gate != null && gate.IsUnlocked(RegionType.Amazon)) return true;
                    GetItemTotal(o, out int current, out int target);
                    return current >= target;
                }
                case ObjectiveCondition.BridgeBuilt:
                    return gate != null && gate.IsUnlocked(RegionType.Amazon);
                case ObjectiveCondition.PartsInstalled:
                {
                    GetParts(out int installed, out int total);
                    return total > 0 && installed >= total;
                }
                default:
                    // ItemCrafted, RegionEntered, Escaped: hanya berubah lewat event.
                    return false;
            }
        }

        private void GetItemTotal(ObjectiveData o, out int current, out int target)
        {
            var inv = InventoryManager.Instance;
            var gate = RegionGateSystem.Instance;
            target = o.targetAmount > 0 ? o.targetAmount : BridgeWoodCost;
            int held = inv != null ? inv.GetCount(o.targetId) : 0;
            int deposited = gate != null ? BridgeWoodCost - gate.WoodRemaining : 0;
            current = held + Mathf.Max(0, deposited);
        }

        private static void GetParts(out int installed, out int total)
        {
            var rescue = RescueEventManager.Instance;
            if (EventChannel<HelicopterPartInstalledEvent>.HasLast)
            {
                var last = EventChannel<HelicopterPartInstalledEvent>.Last;
                installed = last.installed;
                total = last.total;
                return;
            }
            installed = rescue != null ? rescue.InstalledCount : 0;
            total = rescue != null ? rescue.TotalParts : (Balance != null ? Balance.region.helicopterPartCount : 5);
        }

        private void GetProgress(ObjectiveData o, out int current, out int target)
        {
            switch (o.condition)
            {
                case ObjectiveCondition.BuildCount:
                {
                    int need = Balance != null ? Balance.objective.baseWallCount : 4;
                    current = Mathf.Min(wallCount, need) + Mathf.Min(doorCount, 1);
                    target = need + 1;
                    return;
                }
                case ObjectiveCondition.ItemTotal:
                    GetItemTotal(o, out current, out target);
                    current = Mathf.Min(current, target);
                    return;
                case ObjectiveCondition.PartsInstalled:
                    GetParts(out current, out target);
                    return;
                default:
                    current = 0;
                    target = 0;
                    return;
            }
        }

        private void Complete(ObjectiveData o, bool allowNotification)
        {
            if (o == null || string.IsNullOrEmpty(o.objectiveId) || !completed.Add(o.objectiveId)) return;
            EventChannel<ObjectiveChangedEvent>.Publish(new ObjectiveChangedEvent(o.objectiveId, true));
            // Tujuan yang belum terlihat selesai tanpa notifikasi (langkah tidak berurutan).
            if (allowNotification && !runEnded && IsVisible(o) && NotificationManager.Instance != null)
                NotificationManager.Instance.ShowFormat("notif.objective_done", NotificationKind.Objective, "title", Localize(o.title));
        }

        private void RefreshAll()
        {
            // 1. Semua tujuan yang belum selesai diperiksa, baik terlihat maupun belum (aturan langkah tidak berurutan).
            for (int i = 0; i < objectives.Length; i++)
            {
                var o = objectives[i];
                if (o == null || completed.Contains(o.objectiveId)) continue;
                if (Evaluate(o)) Complete(o, true);
            }
            // 2.
            EvaluateChapterTriggers();
            // Tujuan baru dari bab yang baru terbuka mungkin sudah terpenuhi.
            for (int i = 0; i < objectives.Length; i++)
            {
                var o = objectives[i];
                if (o == null || completed.Contains(o.objectiveId)) continue;
                if (Evaluate(o)) Complete(o, true);
            }
            // 3. Tujuan pertama menurut order yang terlihat dan belum selesai.
            ObjectiveData active = null;
            for (int i = 0; i < objectives.Length; i++)
            {
                var o = objectives[i];
                if (o == null || completed.Contains(o.objectiveId) || !IsVisible(o)) continue;
                active = o;
                break;
            }
            ActiveObjective = active;
            // 4.
            string id;
            int cur = 0, tgt = 0;
            if (active != null)
            {
                id = active.objectiveId;
                GetProgress(active, out cur, out tgt);
            }
            else if (!bookFound) id = FallbackBookId;
            else if (!unlockedChapters.Contains(3)) id = FallbackBridgeId;
            else id = string.Empty;

            if (id != activeId || cur != activeCurrent || tgt != activeTarget)
            {
                activeId = id;
                activeCurrent = cur;
                activeTarget = tgt;
                EventChannel<ActiveObjectiveChangedEvent>.Publish(new ActiveObjectiveChangedEvent(id, cur, tgt));
            }
            RefreshMarker();
        }

        // ---------------------------------------------------------------- Penanda arah

        private void RefreshMarker()
        {
            bool has = false;
            Vector3 pos = Vector3.zero;
            string label = string.Empty;
            var active = ActiveObjective;

            if (active == null)
            {
                // Tujuan cadangan.
                if (!bookFound) { has = true; pos = bookMarkerPosition; label = GetTitle(FallbackBookId); }
                else if (!unlockedChapters.Contains(3)) { has = true; pos = bridgeMarkerPosition; label = GetTitle(FallbackBridgeId); }
            }
            else if (active.condition == ObjectiveCondition.PartsInstalled)
            {
                // Membawa barang: helipad sudah ditandai HeavyItemSystem, jadi tanpa penanda kedua.
                var heavy = HeavyItemSystem.Instance;
                var player = IslandPlayerController.Instance;
                if (!isCarryingPart && heavy != null && player != null &&
                    heavy.TryGetNearestPart(player.transform.position, out Vector3 partPos))
                {
                    // Hanya suku cadang yang sudah ditemukan, supaya yang belum ditemukan tidak terbongkar.
                    has = true; pos = partPos; label = Localize(active.title);
                }
            }
            else if (active.condition == ObjectiveCondition.BuildCount || active.condition == ObjectiveCondition.ItemTotal)
            {
                if (!bookFound) { has = true; pos = bookMarkerPosition; label = GetTitle(FallbackBookId); }
            }
            else if (active.hasMarker)
            {
                has = true; pos = active.markerPosition; label = Localize(active.title);
            }

            if (markerPublished && has == markerHas && pos == markerPos && label == markerLabel) return;
            markerPublished = true;
            markerHas = has;
            markerPos = pos;
            markerLabel = label;
            EventChannel<ObjectiveMarkerChangedEvent>.Publish(new ObjectiveMarkerChangedEvent(has, pos, label));
        }

        // ---------------------------------------------------------------- Event

        private void HandleDayChanged(DayChangedEvent e) => RefreshAll();

        private void HandleStructureBuilt(StructureBuiltEvent e)
        {
            CountStructure(e.buildableId, 1);
            RefreshAll();
        }

        private void HandleStructureDestroyed(StructureDestroyedEvent e)
        {
            // Tujuan selesai tidak pernah kembali terbuka. Hitungan hanya penting selama obj_base belum selesai, dan
            // dihitung ulang pada frame berikutnya, setelah objek struktur dimatikan.
            if (completed.Contains("obj_base")) return;
            recountPending = true;
            progressDirty = true;
        }

        private void HandleInventoryChanged(InventoryChangedEvent e)
        {
            // Penyegaran dibatasi Update, supaya banyak perubahan kecil tidak menghitung ulang tiap frame.
            progressDirty = true;
        }

        private void HandleBridgeProgress(BridgeProgressChangedEvent e) => progressDirty = true;

        private void HandleRegionUnlocked(RegionUnlockedEvent e) => RefreshAll();

        private void HandleItemCrafted(ItemCraftedEvent e)
        {
            for (int i = 0; i < objectives.Length; i++)
            {
                var o = objectives[i];
                if (o != null && o.condition == ObjectiveCondition.ItemCrafted && string.Equals(o.targetId, e.recipeId, StringComparison.Ordinal))
                    Complete(o, true);
            }
            RefreshAll();
        }

        private void HandleRegion(RegionChangedEvent e)
        {
            var bal = Balance;
            RegionType ch3 = bal != null ? bal.objective.chapter3TriggerRegion : RegionType.Amazon;
            if (e.to == ch3) chapter3RegionReached = true;
            for (int i = 0; i < objectives.Length; i++)
            {
                var o = objectives[i];
                if (o != null && o.condition == ObjectiveCondition.RegionEntered && o.targetRegion == e.to) Complete(o, true);
            }
            RefreshAll();
        }

        private void HandlePartInstalled(HelicopterPartInstalledEvent e) => RefreshAll();

        private void HandleHelicopterReady(HelicopterReadyEvent e) => RefreshAll();

        private void HandleHeavyItemChanged(HeavyItemChangedEvent e)
        {
            isCarryingPart = e.isCarried;
            RefreshMarker();
        }

        private void HandleRunEnded(RunEndedEvent e)
        {
            // Kalah diabaikan. Menang hanya mencatat obj_takeoff tanpa notifikasi, karena alur akhir run sudah berjalan.
            if (e.result != RunResult.Won) return;
            runEnded = true;
            for (int i = 0; i < objectives.Length; i++)
            {
                var o = objectives[i];
                if (o != null && o.condition == ObjectiveCondition.Escaped) Complete(o, false);
            }
        }

        // ---------------------------------------------------------------- Struktur

        private void CountStructure(string buildableId, int delta)
        {
            if (string.IsNullOrEmpty(buildableId)) return;
            string wallPrefix = WallPrefix();
            if (buildableId.StartsWith(wallPrefix, StringComparison.Ordinal)) wallCount += delta;
            else if (buildableId.StartsWith(DoorPrefix, StringComparison.Ordinal)) doorCount += delta;
        }

        private string WallPrefix()
        {
            for (int i = 0; i < objectives.Length; i++)
            {
                var o = objectives[i];
                if (o != null && o.condition == ObjectiveCondition.BuildCount && !string.IsNullOrEmpty(o.targetId)) return o.targetId;
            }
            return DefaultWallPrefix;
        }

        /// <summary>Bangunan dimuat sebelum OnRestoreFinished manager ini (LoadOrder 90 lebih dulu dari 110).</summary>
        private void RecountStructures()
        {
            wallCount = 0;
            doorCount = 0;
            var nodes = FindObjectsByType<StructureNode>(FindObjectsSortMode.None);
            for (int i = 0; i < nodes.Length; i++)
            {
                var n = nodes[i];
                if (n == null || n.Data == null || string.IsNullOrEmpty(n.StructureId)) continue;
                CountStructure(n.Data.id, 1);
            }
        }

        private static string Localize(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            var loc = LocalizationManager.Instance;
            return loc != null ? loc.Get(key) : key;
        }

        // ---------------------------------------------------------------- Save (#80)

        [Serializable]
        private class ObjectiveSaveData
        {
            public bool bookFound;
            public bool chapter3RegionReached;
            public int[] unlockedChapters = Array.Empty<int>();
            public int[] grantedRewards = Array.Empty<int>();
            public string[] completed = Array.Empty<string>();
        }

        public string SaveId => SaveIdConst;
        public int LoadOrder => 110;

        public string CaptureState()
        {
            var done = new string[completed.Count];
            completed.CopyTo(done);
            return JsonUtility.ToJson(new ObjectiveSaveData
            {
                bookFound = bookFound,
                chapter3RegionReached = chapter3RegionReached,
                unlockedChapters = unlockedChapters.ToArray(),
                grantedRewards = grantedRewards.ToArray(),
                completed = done,
            });
        }

        public void RestoreState(string json)
        {
            // Tidak mengirim event. ActiveObjective dihitung ulang di OnRestoreFinished.
            if (string.IsNullOrEmpty(json)) return;
            var d = JsonUtility.FromJson<ObjectiveSaveData>(json);
            if (d == null) return;
            bookFound = d.bookFound;
            chapter3RegionReached = d.chapter3RegionReached;
            unlockedChapters.Clear();
            if (d.unlockedChapters != null) unlockedChapters.AddRange(d.unlockedChapters);
            grantedRewards.Clear();
            if (d.grantedRewards != null) grantedRewards.AddRange(d.grantedRewards);
            completed.Clear();
            if (d.completed != null) for (int i = 0; i < d.completed.Length; i++) if (!string.IsNullOrEmpty(d.completed[i])) completed.Add(d.completed[i]);
        }

        public void OnRestoreFinished()
        {
            // StructureRegistry (LoadOrder 90) sudah selesai. Event sticky dikirim ulang lewat RefreshAll.
            RecountStructures();
            activeId = null;
            markerPublished = false;
            RefreshAll();
            HookDiaryManager();
        }
    }
}
