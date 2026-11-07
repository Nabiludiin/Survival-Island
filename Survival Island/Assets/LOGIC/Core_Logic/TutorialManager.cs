using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pop-up panduan pendek yang muncul saat pemain pertama kali melakukan atau mengalami sesuatu.
    /// Blueprint: Core_Logic/TutorialManager.md (#16). GDD bagian 4: Diary tetap guidebook utama; pop-up ini hanya
    /// pelengkap kontrol dasar dan tidak mengulang isi lore atau resep vaksin dari Diary.
    ///
    /// Manager persisten (IBootstrapService, urutan bootstrap 11). Awake hanya mengisi Instance. Semua pemicu berasal
    /// dari event, jadi sistem gameplay tidak mengenal sistem ini. Tidak menghentikan waktu dan tidak masuk UIFocusStack.
    /// Data "sudah dibaca" disimpan di PlayerPrefs (settings.tutorial.seen), terpisah dari save run.
    /// </summary>
    public class TutorialManager : MonoBehaviour, IBootstrapService
    {
        public static TutorialManager Instance { get; private set; }

        public const string SeenPrefsKey = "settings.tutorial.seen";
        public const string ResourcesFolder = "TutorialHints";

        // ID pop-up (tabel "Kapan pop-up muncul"). Usulan disetujui developer sebagai nilai awal.
        private const string HintShipLoot = "ship_loot";
        private const string HintIslandVitals = "island_vitals";
        private const string HintInventory = "first_inventory";
        private const string HintTree = "first_tree";
        private const string HintRock = "first_rock";
        private const string HintCrafting = "first_crafting";
        private const string HintBuild = "first_build";
        private const string HintKail = "first_kail";
        private const string HintPistol = "first_pistol";
        private const string HintLowHunger = "low_hunger";
        private const string HintLowThirst = "low_thirst";
        private const string HintInfection = "first_infection";
        private const string HintNight = "first_night";
        private const string HintDay = "first_day";
        private const string HintDiary = "first_diary";
        private const string HintSupply = "first_supply";
        private const string HintSnow = "region_snow";
        private const string HintLogistic = "region_logistic";
        private const string HintHeavy = "first_heavy";
        private const string HintStealth = "first_stealth";
        private const string HintBloodMoon = "blood_moon";
        private const string HintThrowable = "first_throwable";

        private const string ActionTokenPrefix = "{action:";

        [Tooltip("Kotak teks di canvas persisten, di atas HUD dan di bawah panel menjeda.")]
        [SerializeField] private TutorialHintPanel hintPanel;

        private readonly Dictionary<string, TutorialHint> hints = new Dictionary<string, TutorialHint>();
        private readonly HashSet<string> seen = new HashSet<string>();
        private readonly List<TutorialHint> queue = new List<TutorialHint>(4);
        private readonly StringBuilder builder = new StringBuilder(128);
        private TutorialHint current;
        private float showTimer;
        private float shownSeconds;
        private float gapTimer;
        private float dangerTimer;
        private bool isPausedUI;
        private bool initialized;

        private static TutorialBalance B
        {
            get
            {
                var cfg = GameManager.CurrentBalance;
                return cfg != null ? cfg.tutorial : null;
            }
        }

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[TutorialManager] Instance ganda. Yang baru dimatikan.");
                enabled = false;
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<PhaseChangedEvent>.Raised += HandlePhaseChanged;
            EventChannel<UIPanelOpenedEvent>.Raised += HandlePanelOpened;
            EventChannel<ResourceNodeHitEvent>.Raised += HandleNodeHit;
            EventChannel<BuildModeChangedEvent>.Raised += HandleBuildMode;
            EventChannel<EquippedItemChangedEvent>.Raised += HandleEquipped;
            EventChannel<VitalsChangedEvent>.Raised += HandleVitals;
            EventChannel<NightfallEvent>.Raised += HandleNightfall;
            EventChannel<DaybreakEvent>.Raised += HandleDaybreak;
            EventChannel<DiaryChapterUnlockedEvent>.Raised += HandleDiaryChapter;
            EventChannel<BeachSupplyArrivedEvent>.Raised += HandleSupplyArrived;
            EventChannel<RegionChangedEvent>.Raised += HandleRegionChanged;
            EventChannel<HeavyItemChangedEvent>.Raised += HandleHeavyItem;
            EventChannel<BloodMoonWarningEvent>.Raised += HandleBloodMoonWarning;
            EventChannel<PlayerDamagedEvent>.Raised += HandlePlayerDamaged;
            EventChannel<UIFocusChangedEvent>.Raised += HandleFocusChanged;
            EventChannel<InputBindingsChangedEvent>.Raised += HandleBindingsChanged;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;

            if (EventChannel<UIFocusChangedEvent>.HasLast) HandleFocusChanged(EventChannel<UIFocusChangedEvent>.Last);
        }

        private void OnDisable()
        {
            EventChannel<PhaseChangedEvent>.Raised -= HandlePhaseChanged;
            EventChannel<UIPanelOpenedEvent>.Raised -= HandlePanelOpened;
            EventChannel<ResourceNodeHitEvent>.Raised -= HandleNodeHit;
            EventChannel<BuildModeChangedEvent>.Raised -= HandleBuildMode;
            EventChannel<EquippedItemChangedEvent>.Raised -= HandleEquipped;
            EventChannel<VitalsChangedEvent>.Raised -= HandleVitals;
            EventChannel<NightfallEvent>.Raised -= HandleNightfall;
            EventChannel<DaybreakEvent>.Raised -= HandleDaybreak;
            EventChannel<DiaryChapterUnlockedEvent>.Raised -= HandleDiaryChapter;
            EventChannel<BeachSupplyArrivedEvent>.Raised -= HandleSupplyArrived;
            EventChannel<RegionChangedEvent>.Raised -= HandleRegionChanged;
            EventChannel<HeavyItemChangedEvent>.Raised -= HandleHeavyItem;
            EventChannel<BloodMoonWarningEvent>.Raised -= HandleBloodMoonWarning;
            EventChannel<PlayerDamagedEvent>.Raised -= HandlePlayerDamaged;
            EventChannel<UIFocusChangedEvent>.Raised -= HandleFocusChanged;
            EventChannel<InputBindingsChangedEvent>.Raised -= HandleBindingsChanged;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
        }

        /// <summary>Muat semua TutorialHint, lalu baca daftar yang sudah dibaca dari PlayerPrefs.</summary>
        public void Initialize()
        {
            hints.Clear();
            var assets = Resources.LoadAll<TutorialHint>(ResourcesFolder);
            for (int i = 0; i < assets.Length; i++)
            {
                var h = assets[i];
                if (h == null || string.IsNullOrEmpty(h.hintId)) continue;
                if (hints.ContainsKey(h.hintId))
                {
                    Debug.LogWarning($"[TutorialManager] hintId ganda: {h.hintId}. Aset {h.name} diabaikan.");
                    continue;
                }
                hints.Add(h.hintId, h);
            }

            // Format: daftar hintId dipisah koma. ID yang tidak dikenal tetap disimpan tetapi tidak berpengaruh.
            seen.Clear();
            string raw = PlayerPrefs.GetString(SeenPrefsKey, string.Empty);
            if (!string.IsNullOrEmpty(raw))
            {
                var parts = raw.Split(',');
                for (int i = 0; i < parts.Length; i++)
                {
                    string id = parts[i].Trim();
                    if (id.Length > 0) seen.Add(id);
                }
            }

            ClearQueue();
            initialized = true;
        }

        /// <summary>Permadeath: kosongkan antrean dan pop-up aktif. Isi seen TIDAK dihapus (usulan 1 disetujui developer).</summary>
        public void ResetForNewRun()
        {
            ClearQueue();
            dangerTimer = 0f;
        }

        // ---------------------------------------------------------------- Fungsi publik

        /// <summary>Minta pop-up ditampilkan. Diabaikan kalau tutorial mati, sudah dibaca, sudah antre, atau sedang tampil.</summary>
        public void Request(string hintId)
        {
            if (!initialized || string.IsNullOrEmpty(hintId)) return;

            // 1. Usulan 4: pilihan matikan tutorial di Settings.
            var settings = SettingsManager.Instance;
            if (settings != null && !settings.tutorialEnabled) return;

            // 2.
            if (seen.Contains(hintId)) return;
            if (current != null && current.hintId == hintId) return;
            for (int i = 0; i < queue.Count; i++) if (queue[i].hintId == hintId) return;
            if (!hints.TryGetValue(hintId, out var hint)) return;

            var b = B;
            int limit = b != null ? Mathf.Max(1, b.queueLimit) : 4;

            // 3. Antrean penuh: buang prioritas terendah (bisa muncul lagi di pemicu berikutnya).
            if (queue.Count >= limit)
            {
                var lowest = queue[queue.Count - 1];
                if (lowest.priority >= hint.priority) return;
                queue.RemoveAt(queue.Count - 1);
            }

            // 4. Sisip menurut prioritas, tertinggi di depan.
            int pos = queue.Count;
            while (pos > 0 && queue[pos - 1].priority < hint.priority) pos--;
            queue.Insert(pos, hint);
        }

        /// <summary>Kosongkan semua tanda sudah dibaca. Dipanggil tombol "Ulang tutorial" di Settings (#19).</summary>
        public void ResetSeen()
        {
            seen.Clear();
            PlayerPrefs.DeleteKey(SeenPrefsKey);   // PlayerPrefs.DeleteAll() dilarang (#82)
            PlayerPrefs.Save();
        }

        public bool HasSeen(string hintId) => seen.Contains(hintId);

        // ---------------------------------------------------------------- Per frame

        private void Update()
        {
            var b = B;
            if (b == null) return;
            float dt = Time.deltaTime;   // Game dijeda: semua timer berhenti (bagian 4)

            // 1.
            if (current != null)
            {
                if (isPausedUI) return;
                showTimer -= dt;
                shownSeconds += dt;
                if (showTimer <= 0f) Finish();
                return;
            }

            // 2.
            gapTimer -= dt;
            dangerTimer -= dt;

            // 3. Pop-up baru tidak dimulai saat jeda.
            if (gapTimer > 0f || isPausedUI || queue.Count == 0) return;
            for (int i = 0; i < queue.Count; i++)
            {
                var h = queue[i];
                if (dangerTimer > 0f && !h.allowDuringDanger) continue;
                queue.RemoveAt(i);
                ShowNext(h, b);
                return;
            }
        }

        private void ShowNext(TutorialHint h, TutorialBalance b)
        {
            current = h;
            showTimer = b.displaySeconds;
            shownSeconds = 0f;
            if (hintPanel != null)
            {
                hintPanel.Show(BuildText(h));
                hintPanel.SetSuspended(isPausedUI);
            }
        }

        private void Finish()
        {
            var b = B;
            if (hintPanel != null) hintPanel.Hide();
            gapTimer = b != null ? b.minGapSeconds : 0f;

            // Dipotong sebelum dibaca: tidak ditandai, jadi muncul lagi lain kali.
            if (current != null && b != null && shownSeconds >= b.minSeenSeconds) MarkSeen(current.hintId);
            current = null;
        }

        private void MarkSeen(string id)
        {
            if (!seen.Add(id)) return;
            builder.Clear();
            foreach (var s in seen)
            {
                if (builder.Length > 0) builder.Append(',');
                builder.Append(s);
            }
            PlayerPrefs.SetString(SeenPrefsKey, builder.ToString());
            PlayerPrefs.Save();   // Paling banyak sekitar 20 kali sepanjang umur game
        }

        private void ClearQueue()
        {
            queue.Clear();
            current = null;
            showTimer = 0f;
            shownSeconds = 0f;
            gapTimer = 0f;
            if (hintPanel != null) hintPanel.Hide();
        }

        // ---------------------------------------------------------------- Teks

        private string BuildText(TutorialHint h)
        {
            var loc = LocalizationManager.Instance;
            string text = loc != null && !string.IsNullOrEmpty(h.textKey) && loc.Has(h.textKey) ? loc.Get(h.textKey) : h.fallbackText;
            return ResolveActionTokens(text ?? string.Empty);
        }

        /// <summary>
        /// Ganti token {action:Interact} dengan nama tombol aktif (#18). Teks tombol tertulis langsung dilarang.
        /// Token {input:...} milik LocalizationManager sudah diganti oleh Get.
        /// </summary>
        private string ResolveActionTokens(string text)
        {
            if (text.IndexOf(ActionTokenPrefix, StringComparison.Ordinal) < 0) return text;
            var input = InputManager.Instance;
            builder.Clear();
            int i = 0;
            while (i < text.Length)
            {
                int start = text.IndexOf(ActionTokenPrefix, i, StringComparison.Ordinal);
                int end = start < 0 ? -1 : text.IndexOf('}', start);
                if (start < 0 || end < 0) { builder.Append(text, i, text.Length - i); break; }
                builder.Append(text, i, start - i);
                int nameStart = start + ActionTokenPrefix.Length;
                string name = text.Substring(nameStart, end - nameStart);
                if (input != null && Enum.TryParse(name, out GameInputAction action)) builder.Append(input.GetBindingDisplay(action));
                else builder.Append(name);
                i = end + 1;
            }
            return builder.ToString();
        }

        // ---------------------------------------------------------------- Pemicu

        private void HandlePhaseChanged(PhaseChangedEvent e)
        {
            // Fase 1 yang singkat: hanya ship_loot yang muncul di kapal.
            Request(e.phase == GamePhase.SinkingShip ? HintShipLoot : HintIslandVitals);
        }

        private void HandlePanelOpened(UIPanelOpenedEvent e)
        {
            if (e.panelId == UIPanelId.Inventory) Request(HintInventory);
            else if (e.panelId == UIPanelId.Crafting) Request(HintCrafting);
        }

        private void HandleNodeHit(ResourceNodeHitEvent e)
        {
            if (!e.byPlayer) return;
            if (e.nodeType == NodeType.Tree) Request(HintTree);
            else if (e.nodeType == NodeType.Rock) Request(HintRock);
        }

        private void HandleBuildMode(BuildModeChangedEvent e)
        {
            // GDD bagian 4: tidak bisa membangun di pasir pantai.
            if (e.mode == HammerMode.Build) Request(HintBuild);
        }

        private void HandleEquipped(EquippedItemChangedEvent e)
        {
            string id = e.itemId;
            if (string.IsNullOrEmpty(id)) return;
            if (id == "Base_Kail") Request(HintKail);
            else if (id == "Base_Pistol") Request(HintPistol);
            else if (id == "Throw_Molotov" || id == "Throw_Bomb" || id == "Throw_Rock" || id == "Throw_Flare") Request(HintThrowable);
        }

        private void HandleVitals(VitalsChangedEvent e)
        {
            var b = B;
            if (b == null) return;
            if (e.hunger < b.lowVitalThreshold) Request(HintLowHunger);
            if (e.thirst < b.lowVitalThreshold) Request(HintLowThirst);
            if (e.infection > 0f) Request(HintInfection);
        }

        private void HandleNightfall(NightfallEvent e)
        {
            // GDD bagian 3: malam = horde zombie. first_stealth muncul pada malam setelah first_night tampil.
            if (seen.Contains(HintNight)) Request(HintStealth);
            else Request(HintNight);
        }

        private void HandleDaybreak(DaybreakEvent e)
        {
            // Pagi pertama setelah first_night tampil (GDD bagian 3: zombie terbakar matahari).
            if (seen.Contains(HintNight)) Request(HintDay);
        }

        private void HandleDiaryChapter(DiaryChapterUnlockedEvent e) => Request(HintDiary);

        private void HandleSupplyArrived(BeachSupplyArrivedEvent e) => Request(HintSupply);

        private void HandleRegionChanged(RegionChangedEvent e)
        {
            if (e.to == RegionType.Snow) Request(HintSnow);
            else if (e.to == RegionType.Logistic) Request(HintLogistic);
        }

        private void HandleHeavyItem(HeavyItemChangedEvent e)
        {
            if (e.isCarried) Request(HintHeavy);
        }

        private void HandleBloodMoonWarning(BloodMoonWarningEvent e) => Request(HintBloodMoon);

        // ---------------------------------------------------------------- Status

        private void HandlePlayerDamaged(PlayerDamagedEvent e)
        {
            var b = B;
            if (b != null) dangerTimer = b.dangerHoldSeconds;
        }

        private void HandleFocusChanged(UIFocusChangedEvent e)
        {
            // Saat jeda, panel disembunyikan sementara dan muncul lagi dengan sisa waktu yang sama.
            isPausedUI = e.isPaused;
            if (hintPanel != null) hintPanel.SetSuspended(isPausedUI);
        }

        private void HandleBindingsChanged(InputBindingsChangedEvent e)
        {
            if (current != null && hintPanel != null) hintPanel.SetText(BuildText(current));
        }

        private void HandleRunEnded(RunEndedEvent e)
        {
            // Pop-up yang terpotong sebelum minSeenSeconds tidak ditandai.
            if (current != null) Finish();
            queue.Clear();
        }
    }
}
