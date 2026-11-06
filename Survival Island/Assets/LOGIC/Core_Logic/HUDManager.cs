using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Informasi real-time di layar: status tubuh, peluru, hotbar, sisa hari, suku cadang, mesin helikopter, mode gratis
    /// Lucky Machine, suhu, antrean crafting, efek status, prompt interaksi, dan penanda Fase 1.
    /// Blueprint: Core_Logic/HUDManager.md (#18, #17, #51, #58, #60, #74, #81, #96, #111). GDD bagian 3 (status karakter dan
    /// timer 60 detik Fase 1). Tata letak dan warna tanda peringatan adalah usulan, menunggu developer.
    ///
    /// Komponen scene, ada di ShipPhase dan IslandPhase (#82). Awake hanya mengisi Instance. HUD hanya mendengar event
    /// atau membaca data, tidak mengirim event. Tidak disimpan: nilai kembali lewat event sticky di OnEnable/OnRestoreFinished.
    ///
    /// Belum tersambung karena sistem pengirimnya belum ada (TODO di kode): ThrowableChangedEvent (#96),
    /// ActiveObjectiveChangedEvent (#81), SwimStateChangedEvent (#13), RegionStreaming.IsStalled (#88).
    /// </summary>
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager Instance { get; private set; }

        [Serializable]
        public struct WarningIcon
        {
            public VitalWarningFlags flag;
            public GameObject icon;
        }

        [Header("Status pemain")]
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider staminaBar;
        [SerializeField] private Slider hungerBar;
        [SerializeField] private Slider thirstBar;
        [SerializeField] private Slider infectionBar;
        [SerializeField] private Image infectionFill;
        [SerializeField] private WarningIcon[] warningIcons = new WarningIcon[0];

        [Header("Senjata dan interaksi")]
        [SerializeField] private TMP_Text ammoText;
        [SerializeField] private TMP_Text interactionPromptText;
        [SerializeField] private Image interactionProgress;
        [Tooltip("MarkerRoot milik CombatFeedbackUI (#51) dijadikan anak objek ini, supaya ikut tersembunyi.")]
        [SerializeField] private RectTransform crosshairRoot;
        [SerializeField] private HotbarView hotbarView;
        [SerializeField] private Image throwableIcon;
        [SerializeField] private TMP_Text throwableCount;

        [Header("Tujuan dan helikopter")]
        [SerializeField] private TMP_Text objectiveTitle;
        [SerializeField] private TMP_Text objectiveProgress;
        [SerializeField] private TMP_Text daysLeftText;
        [SerializeField] private TMP_Text partsText;
        [SerializeField] private TMP_Text engineText;
        [SerializeField] private TMP_Text luckyFreeText;

        [Header("Lingkungan dan sistem lain")]
        [SerializeField] private Slider breathBar;
        [SerializeField] private Slider warmthBar;
        [SerializeField] private Image warmthFill;
        [SerializeField] private Image craftQueueIcon;
        [SerializeField] private StatusEffectBar statusEffectBar;
        [SerializeField] private TMP_Text loadingAreaText;

        [Header("Fase 1 (hanya ShipPhase, #74)")]
        [SerializeField] private TMP_Text shipHandWeight;
        [SerializeField] private GameObject lifeboatMarker;
        [SerializeField] private GameObject safeZoneLine;

        [Header("Tampilan (usulan, menunggu developer)")]
        [SerializeField] private Color infectionNormal = new Color(0.55f, 0.75f, 0.3f);
        [SerializeField] private Color infectionCritical = new Color(0.9f, 0.15f, 0.15f);
        [Tooltip("HUDManager.md: bar infeksi lebih merah di atas 80.")]
        [SerializeField] private float infectionRedFrom = 80f;
        [SerializeField] private Color warmthNormal = new Color(1f, 0.75f, 0.35f);
        [SerializeField] private Color warmthCold = new Color(0.55f, 0.8f, 1f);
        [SerializeField] private Color warmthFreezing = new Color(0.1f, 0.25f, 0.8f);
        [Tooltip("WarmthSystem.md: bar tampil kalau Warmth di bawah nilai ini atau di Region 3.")]
        [SerializeField] private float warmthFull = 100f;
        [SerializeField] private float warmthColdAt = 50f;
        [SerializeField] private float blinkHz = 3f;

        private const float SlowTickInterval = 0.25f;   // Blueprint: 4 kali per detik

        private bool isShipPhase;
        private bool ammoEmptyBlink;
        private float blinkTime;
        private float slowTick;

        private float luckyFreeRemaining;
        private int luckyShownSecond = -1;
        private float engineRemaining;
        private bool engineWarming;
        private int engineShownSecond = -1;

        private float warmth = 100f;
        private bool isFreezing;
        private bool inSnow;

        private int lastDay = -1;
        private bool dayLimitPassed;
        private int partsInstalled = -1, partsTotal;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<VitalsChangedEvent>.Raised += HandleVitals;
            EventChannel<VitalWarningChangedEvent>.Raised += HandleWarnings;
            EventChannel<AmmoChangedEvent>.Raised += HandleAmmo;
            EventChannel<HotbarChangedEvent>.Raised += HandleHotbar;
            EventChannel<EquippedItemChangedEvent>.Raised += HandleEquipped;
            EventChannel<InventoryChangedEvent>.Raised += HandleInventory;
            EventChannel<HeavyItemChangedEvent>.Raised += HandleHeavyItem;
            EventChannel<DurabilityStateChangedEvent>.Raised += HandleDurability;
            EventChannel<InputBindingsChangedEvent>.Raised += HandleBindings;
            EventChannel<DayChangedEvent>.Raised += HandleDayChanged;
            EventChannel<DayLimitPassedEvent>.Raised += HandleDayLimit;
            EventChannel<ShipHandChangedEvent>.Raised += HandleShipHand;
            EventChannel<HelicopterPartInstalledEvent>.Raised += HandlePart;
            EventChannel<HelicopterEngineChangedEvent>.Raised += HandleEngine;
            EventChannel<LuckyMachineFreeWindowChangedEvent>.Raised += HandleLuckyFree;
            EventChannel<WarmthChangedEvent>.Raised += HandleWarmth;
            EventChannel<RegionChangedEvent>.Raised += HandleRegion;
            EventChannel<CraftQueueChangedEvent>.Raised += HandleCraftQueue;
            EventChannel<StatusEffectChangedEvent>.Raised += HandleStatusEffect;
            EventChannel<UIFocusChangedEvent>.Raised += HandleFocus;
            EventChannel<LocaleChangedEvent>.Raised += HandleLocale;
            // TODO(ThrowableSystem #96): EventChannel<ThrowableChangedEvent>.Raised += HandleThrowable (sticky, baca HasLast/Last).
            // TODO(ObjectiveManager #81): EventChannel<ActiveObjectiveChangedEvent>.Raised += HandleObjective, sorotan
            //     selama objective.hudFlashSeconds (sticky, baca HasLast/Last).
            // TODO(SwimmingController #13): EventChannel<SwimStateChangedEvent>.Raised += HandleSwim; breathBar tampil
            //     selama breathRatio < 1 (sticky, baca HasLast/Last).

            isShipPhase = GameManager.Instance != null && GameManager.Instance.currentPhase == GamePhase.SinkingShip;
            InitialState();
        }

        private void OnDisable()
        {
            EventChannel<VitalsChangedEvent>.Raised -= HandleVitals;
            EventChannel<VitalWarningChangedEvent>.Raised -= HandleWarnings;
            EventChannel<AmmoChangedEvent>.Raised -= HandleAmmo;
            EventChannel<HotbarChangedEvent>.Raised -= HandleHotbar;
            EventChannel<EquippedItemChangedEvent>.Raised -= HandleEquipped;
            EventChannel<InventoryChangedEvent>.Raised -= HandleInventory;
            EventChannel<HeavyItemChangedEvent>.Raised -= HandleHeavyItem;
            EventChannel<DurabilityStateChangedEvent>.Raised -= HandleDurability;
            EventChannel<InputBindingsChangedEvent>.Raised -= HandleBindings;
            EventChannel<DayChangedEvent>.Raised -= HandleDayChanged;
            EventChannel<DayLimitPassedEvent>.Raised -= HandleDayLimit;
            EventChannel<ShipHandChangedEvent>.Raised -= HandleShipHand;
            EventChannel<HelicopterPartInstalledEvent>.Raised -= HandlePart;
            EventChannel<HelicopterEngineChangedEvent>.Raised -= HandleEngine;
            EventChannel<LuckyMachineFreeWindowChangedEvent>.Raised -= HandleLuckyFree;
            EventChannel<WarmthChangedEvent>.Raised -= HandleWarmth;
            EventChannel<RegionChangedEvent>.Raised -= HandleRegion;
            EventChannel<CraftQueueChangedEvent>.Raised -= HandleCraftQueue;
            EventChannel<StatusEffectChangedEvent>.Raised -= HandleStatusEffect;
            EventChannel<UIFocusChangedEvent>.Raised -= HandleFocus;
            EventChannel<LocaleChangedEvent>.Raised -= HandleLocale;
        }

        /// <summary>Aktif terlambat: baca event sticky lewat HasLast dan Last.</summary>
        private void InitialState()
        {
            // Komponen Fase 1 hanya aktif di ShipPhase; komponen pulau disembunyikan di kapal.
            SetActive(shipHandWeight, isShipPhase);
            SetActive(lifeboatMarker, false);
            SetActive(safeZoneLine, false);
            SetActive(daysLeftText, !isShipPhase);
            SetActive(partsText, false);
            SetActive(engineText, false);
            SetActive(luckyFreeText, false);
            SetActive(breathBar, false);
            SetActive(warmthBar, false);
            SetActive(throwableIcon, false);
            SetActive(throwableCount, false);
            SetActive(objectiveTitle, false);
            SetActive(objectiveProgress, false);
            SetActive(loadingAreaText, false);
            if (craftQueueIcon != null) craftQueueIcon.enabled = false;
            HideInteraction();

            if (EventChannel<VitalsChangedEvent>.HasLast) HandleVitals(EventChannel<VitalsChangedEvent>.Last);
            HandleWarnings(EventChannel<VitalWarningChangedEvent>.HasLast ? EventChannel<VitalWarningChangedEvent>.Last : default);
            SetActive(ammoText, false);
            if (EventChannel<EquippedItemChangedEvent>.HasLast) HandleEquipped(EventChannel<EquippedItemChangedEvent>.Last);
            if (EventChannel<AmmoChangedEvent>.HasLast)
            {
                var a = EventChannel<AmmoChangedEvent>.Last;
                UpdateAmmo(a.inMagazine, a.reserve);
            }
            if (EventChannel<ShipHandChangedEvent>.HasLast) HandleShipHand(EventChannel<ShipHandChangedEvent>.Last);

            if (!isShipPhase)
            {
                dayLimitPassed = GameManager.Instance != null && GameManager.Instance.IsPastTargetDay;
                if (EventChannel<DayChangedEvent>.HasLast) HandleDayChanged(EventChannel<DayChangedEvent>.Last);
                else RefreshDaysText();
                if (EventChannel<HelicopterPartInstalledEvent>.HasLast) HandlePart(EventChannel<HelicopterPartInstalledEvent>.Last);
                if (EventChannel<HelicopterEngineChangedEvent>.HasLast) HandleEngine(EventChannel<HelicopterEngineChangedEvent>.Last);
                if (EventChannel<LuckyMachineFreeWindowChangedEvent>.HasLast) HandleLuckyFree(EventChannel<LuckyMachineFreeWindowChangedEvent>.Last);
                if (EventChannel<RegionChangedEvent>.HasLast) inSnow = EventChannel<RegionChangedEvent>.Last.to == RegionType.Snow;
                if (EventChannel<WarmthChangedEvent>.HasLast) HandleWarmth(EventChannel<WarmthChangedEvent>.Last);
                else RefreshWarmth();
            }

            HandleFocus(EventChannel<UIFocusChangedEvent>.HasLast ? EventChannel<UIFocusChangedEvent>.Last : default);
            if (hotbarView != null) hotbarView.Refresh();
            if (statusEffectBar != null) statusEffectBar.Refresh();
        }

        // ---------------------------------------------------------------- Status pemain (#111)

        private void HandleVitals(VitalsChangedEvent e)
        {
            var st = SurvivalStats.Instance;
            SetBar(healthBar, e.hp, st != null ? st.MaxHP : 100f);
            SetBar(staminaBar, e.stamina, st != null ? st.MaxStamina : 100f);
            SetBar(hungerBar, e.hunger, st != null ? st.MaxHunger : 100f);
            SetBar(thirstBar, e.thirst, st != null ? st.MaxThirst : 100f);
            SetBar(infectionBar, e.infection, st != null ? st.MaxInfection : 100f);
            if (infectionFill != null) infectionFill.color = e.infection > infectionRedFrom ? infectionCritical : infectionNormal;
        }

        private void HandleWarnings(VitalWarningChangedEvent e)
        {
            for (int i = 0; i < warningIcons.Length; i++)
                if (warningIcons[i].icon != null)
                    warningIcons[i].icon.SetActive((e.flags & warningIcons[i].flag) != 0);
        }

        private static void SetBar(Slider bar, float value, float max)
        {
            if (bar == null) return;
            bar.minValue = 0f;
            bar.maxValue = Mathf.Max(1f, max);
            bar.value = value;
        }

        // ---------------------------------------------------------------- Senjata (#58)

        private void HandleAmmo(AmmoChangedEvent e) => UpdateAmmo(e.inMagazine, e.reserve);

        /// <summary>Tulis inMagazine / reserve. Saat keduanya 0, teks berkedip (GunController.md).</summary>
        public void UpdateAmmo(int inMagazine, int reserve)
        {
            if (ammoText == null) return;
            ammoText.text = inMagazine + " / " + reserve;
            ammoEmptyBlink = inMagazine <= 0 && reserve <= 0;
            if (!ammoEmptyBlink) ammoText.alpha = 1f;
        }

        private void HandleEquipped(EquippedItemChangedEvent e)
        {
            // Catatan implementasi: teks peluru hanya tampil saat memegang senjata api (ItemData.weaponData terisi).
            var data = ItemDatabase.Get(e.itemId);
            SetActive(ammoText, data != null && data.weaponData != null);
            if (hotbarView != null) hotbarView.Refresh();
        }

        private void HandleHotbar(HotbarChangedEvent e) { if (hotbarView != null) hotbarView.Refresh(); }
        private void HandleInventory(InventoryChangedEvent e) { if (hotbarView != null) hotbarView.Refresh(); }
        private void HandleHeavyItem(HeavyItemChangedEvent e) { if (hotbarView != null) hotbarView.Refresh(); }
        private void HandleDurability(DurabilityStateChangedEvent e) { if (hotbarView != null) hotbarView.Refresh(); }

        // ---------------------------------------------------------------- Interaksi (#17)

        /// <summary>Teks jadi dari PlayerInteractor (sudah memakai InputManager.GetBindingDisplay, bukan tombol tetap).</summary>
        public void ShowInteraction(string message)
        {
            if (interactionPromptText == null) return;
            bool has = !string.IsNullOrEmpty(message);
            interactionPromptText.text = has ? message : string.Empty;
            interactionPromptText.gameObject.SetActive(has);
        }

        public void HideInteraction()
        {
            if (interactionPromptText != null)
            {
                interactionPromptText.text = string.Empty;
                interactionPromptText.gameObject.SetActive(false);
            }
            SetInteractionProgress(0f);
        }

        /// <summary>Cincin interaksi tahan, 0 sampai 1.</summary>
        public void SetInteractionProgress(float value)
        {
            if (interactionProgress == null) return;
            float v = Mathf.Clamp01(value);
            interactionProgress.fillAmount = v;
            interactionProgress.enabled = v > 0f;
        }

        private void HandleBindings(InputBindingsChangedEvent e)
        {
            // TODO(PlayerInteractor #17): minta ulang teks prompt yang sedang tampil. PlayerInteractor menyimpan cache
            // lastActionText, jadi perlu membuang cache itu saat InputBindingsChangedEvent lalu memanggil ShowInteraction lagi.
        }

        // ---------------------------------------------------------------- Hari (#81)

        private void HandleDayChanged(DayChangedEvent e)
        {
            // Pendengar tampilan: isReplay tidak diabaikan (HUDManager.md).
            lastDay = e.newDay;
            if (e.newDay > GameManager.TargetDay) dayLimitPassed = true;
            RefreshDaysText();
        }

        private void HandleDayLimit(DayLimitPassedEvent e)
        {
            dayLimitPassed = true;
            RefreshDaysText();
        }

        private void RefreshDaysText()
        {
            if (daysLeftText == null || isShipPhase) return;
            int day = lastDay > 0 ? lastDay : (GameManager.Instance != null ? GameManager.Instance.currentDay : 1);
            // GDD bagian 3: target hari ke-60. Rumus sisa hari dari ObjectiveManager.md, tanpa angka negatif.
            if (dayLimitPassed || day > GameManager.TargetDay)
                daysLeftText.text = Text("hud.dayLimitPassed", "Target hari ke-60 terlewat");
            else
                daysLeftText.text = Text("hud.daysLeft", "Sisa hari: {n}", "n", (GameManager.TargetDay - day + 1).ToString());
        }

        // ---------------------------------------------------------------- Fase 1 (#74)

        private void HandleShipHand(ShipHandChangedEvent e)
        {
            if (!isShipPhase) return;
            if (shipHandWeight != null) shipHandWeight.text = e.weight + " / " + e.capacity;
            // Penanda sekoci tampil selama pemain membawa barang.
            SetActive(lifeboatMarker, e.weight > 0);
        }

        // ---------------------------------------------------------------- Helikopter (#14, #69)

        private void HandlePart(HelicopterPartInstalledEvent e)
        {
            partsInstalled = e.installed;
            partsTotal = e.total;
            RefreshPartsText();
        }

        private void RefreshPartsText()
        {
            if (partsText == null || partsInstalled < 0) return;
            SetActive(partsText, true);
            partsText.text = Text("hud.parts", "Suku cadang {installed} / {total}",
                "installed", partsInstalled.ToString(), "total", partsTotal.ToString());
        }

        private void HandleEngine(HelicopterEngineChangedEvent e)
        {
            engineWarming = e.state == RescueState.WarmingUp;
            engineRemaining = engineWarming ? Mathf.Max(0f, e.secondsLeft) : 0f;
            engineShownSecond = -1;
            SetActive(engineText, engineWarming);
            if (engineWarming) RefreshEngineText();
        }

        private void RefreshEngineText()
        {
            int sec = Mathf.CeilToInt(engineRemaining);
            if (engineText == null || sec == engineShownSecond) return;
            engineShownSecond = sec;
            engineText.text = Text("hud.engineWarmup", "Mesin memanas {time}", "time", Clock(sec));
        }

        // ---------------------------------------------------------------- Lucky Machine (#72)

        private void HandleLuckyFree(LuckyMachineFreeWindowChangedEvent e)
        {
            // Dua mesin aktif bersamaan tetap satu hitung mundur, karena dimulai dari bencana yang sama.
            if (e.isActive) luckyFreeRemaining = Mathf.Max(luckyFreeRemaining, e.seconds);
            luckyShownSecond = -1;
            RefreshLuckyText();
        }

        private void RefreshLuckyText()
        {
            bool show = luckyFreeRemaining > 0f;
            SetActive(luckyFreeText, show);
            if (!show || luckyFreeText == null) return;
            int sec = Mathf.CeilToInt(luckyFreeRemaining);
            if (sec == luckyShownSecond) return;
            luckyShownSecond = sec;
            luckyFreeText.text = Text("hud.luckyFree", "Putaran gratis {time}", "time", Clock(sec));
        }

        // ---------------------------------------------------------------- Suhu (#65)

        private void HandleWarmth(WarmthChangedEvent e)
        {
            warmth = e.warmth;
            isFreezing = e.isFreezing;
            RefreshWarmth();
        }

        private void HandleRegion(RegionChangedEvent e)
        {
            inSnow = e.to == RegionType.Snow;
            RefreshWarmth();
        }

        private void RefreshWarmth()
        {
            if (warmthBar == null || isShipPhase) return;
            bool show = warmth < warmthFull || inSnow;
            SetActive(warmthBar, show);
            if (!show) return;
            SetBar(warmthBar, warmth, warmthFull);
            if (warmthFill != null && !isFreezing)
                warmthFill.color = warmth <= warmthColdAt ? warmthCold : warmthNormal;
        }

        // ---------------------------------------------------------------- Lain-lain

        private void HandleCraftQueue(CraftQueueChangedEvent e)
        {
            if (craftQueueIcon != null) craftQueueIcon.enabled = e.queueCount > 0;
        }

        private void HandleStatusEffect(StatusEffectChangedEvent e)
        {
            if (statusEffectBar != null) statusEffectBar.Refresh();
        }

        private void HandleFocus(UIFocusChangedEvent e)
        {
            // #83: crosshair (dan MarkerRoot #51 sebagai anaknya) tersembunyi selama gameplay ditahan panel.
            if (crosshairRoot != null) crosshairRoot.gameObject.SetActive(!e.isGameplayBlocked);
        }

        private void HandleLocale(LocaleChangedEvent e)
        {
            RefreshDaysText();
            RefreshPartsText();
            engineShownSecond = -1;
            if (engineWarming) RefreshEngineText();
            luckyShownSecond = -1;
            RefreshLuckyText();
        }

        // ---------------------------------------------------------------- Waktu tampilan

        private void Update()
        {
            // Waktu berskala: sorotan, kedip, dan hitung mundur berhenti saat jeda (HUDManager.md, #72).
            float dt = Time.deltaTime;
            blinkTime += dt;
            bool blinkOn = Mathf.Repeat(blinkTime * blinkHz, 1f) < 0.5f;

            if (ammoEmptyBlink && ammoText != null) ammoText.alpha = blinkOn ? 1f : 0.25f;

            if (luckyFreeRemaining > 0f)
            {
                luckyFreeRemaining -= dt;
                RefreshLuckyText();
            }

            if (engineWarming && engineRemaining > 0f)
            {
                engineRemaining = Mathf.Max(0f, engineRemaining - dt);
                RefreshEngineText();
            }

            // WarmthSystem.md: biru tua berkedip saat isFreezing.
            if (isFreezing && warmthFill != null && warmthBar != null && warmthBar.gameObject.activeSelf)
                warmthFill.color = blinkOn ? warmthFreezing : warmthCold;

            slowTick += dt;
            if (slowTick >= SlowTickInterval)
            {
                slowTick = 0f;
                SlowTick();
            }
        }

        /// <summary>4 kali per detik: status yang tidak punya event.</summary>
        private void SlowTick()
        {
            if (isShipPhase)
            {
                // Garis area aman tampil selama tahap Scavenge (#74).
                var lb = LifeboatExtraction.Instance;
                SetActive(safeZoneLine, lb != null && lb.State == ShipFlowState.Scavenge);
            }
            // TODO(RegionStreaming #88): SetActive(loadingAreaText, RegionStreaming.Instance != null && RegionStreaming.Instance.IsStalled);
        }

        // ---------------------------------------------------------------- Bantuan

        private static void SetActive(Component c, bool value)
        {
            if (c != null && c.gameObject.activeSelf != value) c.gameObject.SetActive(value);
        }

        private static void SetActive(GameObject go, bool value)
        {
            if (go != null && go.activeSelf != value) go.SetActive(value);
        }

        private static string Clock(int totalSeconds)
        {
            int s = Mathf.Max(0, totalSeconds);
            return (s / 60) + ":" + (s % 60).ToString("00");
        }

        // Teks lewat LocalizationManager (#104). Kalau kunci belum ada di tabel, pakai teks Indonesia cadangan.
        private static string Text(string key, string fallback)
        {
            var loc = LocalizationManager.Instance;
            return loc != null && loc.Has(key) ? loc.Get(key) : fallback;
        }

        private static string Text(string key, string fallback, string name, string value)
        {
            var loc = LocalizationManager.Instance;
            if (loc != null && loc.Has(key)) return loc.Format(key, name, value);
            return fallback.Replace("{" + name + "}", value);
        }

        private static string Text(string key, string fallback, string name1, string value1, string name2, string value2)
        {
            var loc = LocalizationManager.Instance;
            if (loc != null && loc.Has(key)) return loc.Format(key, name1, value1, name2, value2);
            return fallback.Replace("{" + name1 + "}", value1).Replace("{" + name2 + "}", value2);
        }
    }
}
