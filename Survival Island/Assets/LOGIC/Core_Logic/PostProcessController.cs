using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SurvivalIsland
{
    /// <summary>
    /// Efek layar URP dari status pemain dan profil suasana region.
    /// Blueprint: Core_Logic/PostProcessController.md (#23). Mendukung GDD bagian 3 (status dan infeksi) dan bagian 5
    /// (suasana region). Bentuk efeknya adalah usulan teknis.
    ///
    /// Singleton per scene di prefab PostFxRig (ShipPhase dan IslandPhase); hilang bersama scene, jadi tidak ada reset run.
    /// Handler event hanya mengisi target. Volume hanya diubah di Update, tanpa alokasi.
    /// </summary>
    public class PostProcessController : MonoBehaviour
    {
        public static PostProcessController Instance { get; private set; }

        [Header("Volume global (layer Default)")]
        [SerializeField] private Volume regionVolumeA;   // Priority 0
        [SerializeField] private Volume regionVolumeB;   // Priority 0
        [SerializeField] private Volume statusVolume;    // Priority 10, bobot selalu 1

        [Header("Warna efek (dilihat langsung di Editor)")]
        [SerializeField] private Color lowHpColor = new Color(0.6f, 0f, 0f);
        [SerializeField] private Color freezeColor = new Color(0.35f, 0.6f, 1f);
        [SerializeField] private Color breathColor = Color.black;
        [SerializeField] private Color infectionTint = new Color(0.85f, 1f, 0.8f);
        [SerializeField] private Color underwaterTint = new Color(0.55f, 0.8f, 0.9f);

        [Tooltip("Ujung blur saat tidak menyelam, dalam meter (PostProcessController.md: gaussianEnd dari 200).")]
        [SerializeField] private float dofClearEndMeters = 200f;

        private VolumeProfile statusProfile;
        private Vignette vignette;
        private ChromaticAberration chromatic;
        private DepthOfField dof;
        private ColorAdjustments colorAdjust;

        private float hpTarget, hpCurrent;
        private float woundTarget, woundCurrent;
        private float hitPulse;
        private float freezeTarget, freezeCurrent;
        private float breathTarget, breathCurrent;
        private float infectionTarget, infectionCurrent;
        private float underwaterTarget, underwaterCurrent;
        private float sleepTarget, sleepCurrent;
        private float deathTarget, deathCurrent;
        private int activeWounds;
        private bool isPulsing;
        private bool drowningFlag;
        private float swimBreathTarget;
        private bool snapNextFrame = true;

        // Profil region: dua volume crossfade.
        private float weightA, weightB, targetA, targetB, blendSpeed;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;

            // 1. Salinan profil, supaya aset tidak berubah saat Play Mode.
            if (statusVolume != null && statusVolume.sharedProfile != null)
            {
                statusProfile = Instantiate(statusVolume.sharedProfile);
                statusVolume.sharedProfile = statusProfile;
                statusVolume.weight = 1f;

                // 2-3. Ambil komponen sekali dan pastikan parameter yang diubah aktif.
                if (statusProfile.TryGet(out vignette)) { vignette.intensity.overrideState = true; vignette.color.overrideState = true; }
                else WarnMissing(nameof(Vignette));
                if (statusProfile.TryGet(out chromatic)) chromatic.intensity.overrideState = true;
                else WarnMissing(nameof(ChromaticAberration));
                if (statusProfile.TryGet(out dof))
                {
                    // Mode Gaussian, lebih ringan dari Bokeh.
                    dof.mode.overrideState = true;
                    dof.mode.value = DepthOfFieldMode.Gaussian;
                    dof.gaussianStart.overrideState = true;
                    dof.gaussianEnd.overrideState = true;
                }
                else WarnMissing(nameof(DepthOfField));
                if (statusProfile.TryGet(out colorAdjust)) { colorAdjust.saturation.overrideState = true; colorAdjust.colorFilter.overrideState = true; }
                else WarnMissing(nameof(ColorAdjustments));
            }
            else Debug.LogWarning("[PostProcessController] statusVolume atau profilnya kosong.", this);

            weightA = targetA = regionVolumeA != null ? regionVolumeA.weight : 0f;
            weightB = targetB = regionVolumeB != null ? regionVolumeB.weight : 0f;
        }

        private void WarnMissing(string component)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"[PostProcessController] StatusProfile tidak memuat {component}. Efek itu dilewati.", this);
#endif
        }

        private void OnEnable()
        {
            EventChannel<VitalsChangedEvent>.Raised += HandleVitals;
            EventChannel<VitalWarningChangedEvent>.Raised += HandleWarning;
            EventChannel<PlayerDamagedEvent>.Raised += HandleDamaged;
            EventChannel<PlayerDiedEvent>.Raised += HandleDied;
            EventChannel<WarmthChangedEvent>.Raised += HandleWarmth;
            EventChannel<InfectionStageChangedEvent>.Raised += HandleInfectionStage;
            EventChannel<DiseaseChangedEvent>.Raised += HandleDisease;
            EventChannel<SwimStateChangedEvent>.Raised += HandleSwim;     // SwimmingController (#13)
            EventChannel<SleepStateChangedEvent>.Raised += HandleSleep;   // SleepSystem (#67)

            // Nilai sticky: setelah load efek langsung sesuai status, tanpa fade.
            if (EventChannel<VitalsChangedEvent>.HasLast) HandleVitals(EventChannel<VitalsChangedEvent>.Last);
            if (EventChannel<VitalWarningChangedEvent>.HasLast) HandleWarning(EventChannel<VitalWarningChangedEvent>.Last);
            if (EventChannel<WarmthChangedEvent>.HasLast) HandleWarmth(EventChannel<WarmthChangedEvent>.Last);
            if (EventChannel<InfectionStageChangedEvent>.HasLast) HandleInfectionStage(EventChannel<InfectionStageChangedEvent>.Last);
            if (EventChannel<SwimStateChangedEvent>.HasLast) HandleSwim(EventChannel<SwimStateChangedEvent>.Last);
            if (EventChannel<SleepStateChangedEvent>.HasLast) HandleSleep(EventChannel<SleepStateChangedEvent>.Last);
            snapNextFrame = true;
        }

        private void OnDisable()
        {
            EventChannel<VitalsChangedEvent>.Raised -= HandleVitals;
            EventChannel<VitalWarningChangedEvent>.Raised -= HandleWarning;
            EventChannel<PlayerDamagedEvent>.Raised -= HandleDamaged;
            EventChannel<PlayerDiedEvent>.Raised -= HandleDied;
            EventChannel<WarmthChangedEvent>.Raised -= HandleWarmth;
            EventChannel<InfectionStageChangedEvent>.Raised -= HandleInfectionStage;
            EventChannel<DiseaseChangedEvent>.Raised -= HandleDisease;
            EventChannel<SwimStateChangedEvent>.Raised -= HandleSwim;
            EventChannel<SleepStateChangedEvent>.Raised -= HandleSleep;
        }

        private void OnDestroy()
        {
            if (statusProfile != null) Destroy(statusProfile);
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Sumber nilai (hanya target)

        private void HandleVitals(VitalsChangedEvent e)
        {
            var b = GameManager.CurrentBalance;
            var stats = SurvivalStats.Instance;
            if (b == null || stats == null) return;
            // MaxHP dibaca langsung, karena buff Lucky Machine bisa mengubahnya.
            hpTarget = Mathf.InverseLerp(b.postProcess.lowHpStartRatio * stats.MaxHP, 0f, e.hp);
        }

        private void HandleWarning(VitalWarningChangedEvent e)
        {
            // Usulan disetujui developer (#23 poin 3): denyut baru mulai saat tanda LowHP.
            isPulsing = (e.flags & VitalWarningFlags.LowHP) != 0;
            drowningFlag = (e.flags & VitalWarningFlags.Drowning) != 0;
            RefreshBreathTarget();
        }

        private void HandleSwim(SwimStateChangedEvent e)
        {
            // Pandangan air dan vignette napas (#13). Napas mulai terasa di bawah breathStartRatio.
            underwaterTarget = e.isUnderwater ? 1f : 0f;
            var b = GameManager.CurrentBalance;
            float start = b != null ? b.postProcess.breathStartRatio : 0.4f;
            swimBreathTarget = Mathf.InverseLerp(start, 0f, e.breathRatio);
            RefreshBreathTarget();
        }

        private void HandleSleep(SleepStateChangedEvent e)
        {
            sleepTarget = e.isSleeping ? 1f : 0f;   // Layar gelap saat tidur (#67)
        }

        private void RefreshBreathTarget()
        {
            breathTarget = drowningFlag ? 1f : swimBreathTarget;
        }

        private void HandleDamaged(PlayerDamagedEvent e)
        {
            switch (e.info.type)
            {
                // Damage berkala tidak membuat kilas.
                case DamageType.Starvation:
                case DamageType.Dehydration:
                case DamageType.Infection:
                case DamageType.Frost:
                case DamageType.Poison:
                case DamageType.Drown:
                    return;
            }
            var b = GameManager.CurrentBalance;
            if (b != null) hitPulse = b.postProcess.hitPulseAmount;
        }

        private void HandleDisease(DiseaseChangedEvent e)
        {
            if (e.type != DiseaseType.Bleeding && e.type != DiseaseType.WoundInfection) return;
            activeWounds = Mathf.Max(0, activeWounds + (e.isActive ? 1 : -1));
            woundTarget = activeWounds > 0 ? 1f : 0f;
        }

        private void HandleWarmth(WarmthChangedEvent e)
        {
            freezeTarget = e.isFreezing ? 1f : 0f;
        }

        private void HandleInfectionStage(InfectionStageChangedEvent e)
        {
            var b = GameManager.CurrentBalance;
            var vision = b != null ? b.infection.stageVision : null;
            if (vision == null || vision.Length == 0) return;
            // InfectionSystem mengirim tahap 1-4, jadi indeks larik adalah stage - 1 (lihat Catatan Implementasi).
            infectionTarget = vision[Mathf.Clamp(e.stage - 1, 0, vision.Length - 1)];
        }

        private void HandleDied(PlayerDiedEvent e)
        {
            deathTarget = 1f;
        }

        // ---------------------------------------------------------------- Fungsi publik

        /// <summary>Layar bersih untuk adegan (#53, #99). Semua target 0 dan langsung tiba.</summary>
        public void ClearStatusEffects()
        {
            hpTarget = woundTarget = freezeTarget = breathTarget = infectionTarget = 0f;
            underwaterTarget = sleepTarget = deathTarget = 0f;
            hitPulse = 0f;
            isPulsing = false;
            drowningFlag = false;
            swimBreathTarget = 0f;
            snapNextFrame = true;
        }

        /// <summary>
        /// Dipanggil EnvironmentDirector (#92). Profil hanya boleh mengatur Bloom, Tonemapping, White Balance,
        /// postExposure, dan contrast. null = bobot region turun ke 0. blendSeconds &lt;= 0 langsung selesai (load).
        /// </summary>
        public void SetRegionProfile(VolumeProfile profile, float blendSeconds)
        {
            if (regionVolumeA == null || regionVolumeB == null) return;

            if (profile == null)
            {
                targetA = targetB = 0f;
            }
            else if (regionVolumeA.sharedProfile == profile && targetA >= 1f) { /* Sudah menuju profil ini */ }
            else if (regionVolumeB.sharedProfile == profile && targetB >= 1f) { }
            else
            {
                // Volume dengan bobot terkecil diisi profil baru, supaya transisi berlanjut dari bobot saat ini.
                bool useA = weightA <= weightB;
                if (useA) { regionVolumeA.sharedProfile = profile; targetA = 1f; targetB = 0f; }
                else { regionVolumeB.sharedProfile = profile; targetB = 1f; targetA = 0f; }
            }

            if (blendSeconds <= 0f)
            {
                weightA = targetA;
                weightB = targetB;
                ApplyRegionWeights();
                blendSpeed = 0f;
            }
            else blendSpeed = 1f / blendSeconds;
        }

        private void ApplyRegionWeights()
        {
            if (regionVolumeA != null) regionVolumeA.weight = weightA;
            if (regionVolumeB != null) regionVolumeB.weight = weightB;
        }

        // ---------------------------------------------------------------- Per frame

        private void Update()
        {
            var b = GameManager.CurrentBalance;
            if (b == null) return;
            var pp = b.postProcess;
            float dt = Time.deltaTime;   // Efek ikut berhenti saat jeda

            // Region crossfade.
            if (blendSpeed > 0f)
            {
                weightA = Mathf.MoveTowards(weightA, targetA, blendSpeed * dt);
                weightB = Mathf.MoveTowards(weightB, targetB, blendSpeed * dt);
                ApplyRegionWeights();
                if (Mathf.Approximately(weightA, targetA) && Mathf.Approximately(weightB, targetB)) blendSpeed = 0f;
            }

            // 1. Dekatkan nilai ke target.
            float step = pp.changePerSec * dt;
            if (snapNextFrame)
            {
                hpCurrent = hpTarget; woundCurrent = woundTarget; freezeCurrent = freezeTarget;
                breathCurrent = breathTarget; infectionCurrent = infectionTarget; underwaterCurrent = underwaterTarget;
                sleepCurrent = sleepTarget; deathCurrent = deathTarget;
                snapNextFrame = false;
            }
            else
            {
                hpCurrent = Mathf.MoveTowards(hpCurrent, hpTarget, step);
                woundCurrent = Mathf.MoveTowards(woundCurrent, woundTarget, step);
                freezeCurrent = Mathf.MoveTowards(freezeCurrent, freezeTarget, step);
                breathCurrent = Mathf.MoveTowards(breathCurrent, breathTarget, step);
                infectionCurrent = Mathf.MoveTowards(infectionCurrent, infectionTarget, step);
                sleepCurrent = Mathf.MoveTowards(sleepCurrent, sleepTarget, step);
                // Masuk dan keluar air lebih cepat.
                underwaterCurrent = Mathf.MoveTowards(underwaterCurrent, underwaterTarget, pp.underwaterChangePerSec * dt);
                float deathStep = pp.deathDesaturateSeconds > 0f ? dt / pp.deathDesaturateSeconds : 1f;
                deathCurrent = Mathf.MoveTowards(deathCurrent, deathTarget, deathStep);
            }

            // 2. Vignette dengan warna berbobot, supaya beberapa sumber tetap terbaca dan yang kuat dominan.
            if (vignette != null)
            {
                float hp = hpCurrent * pp.lowHpMaxVignette;
                // Pilihan aksesibilitas #104: reduceFlashing mematikan denyut, tepi merah tetap ada.
                bool reduceFlashing = LocalizationManager.Instance != null && LocalizationManager.Instance.Accessibility.reduceFlashing;
                if (isPulsing && !reduceFlashing) hp += Mathf.Sin(Time.time * pp.lowHpPulseHz * Mathf.PI * 2f) * pp.lowHpPulseAmount;
                float red = Mathf.Max(Mathf.Max(hp, woundCurrent * pp.woundVignette) + hitPulse, 0f);
                float blue = freezeCurrent * pp.freezeVignette;
                float dark = Mathf.Max(breathCurrent * pp.breathVignette, sleepCurrent * pp.sleepVignette);

                float total = red + blue + dark;
                vignette.active = total > 0.001f;
                if (vignette.active)
                {
                    vignette.intensity.value = Mathf.Clamp01(Mathf.Max(red, Mathf.Max(blue, dark)));
                    vignette.color.value = (lowHpColor * red + freezeColor * blue + breathColor * dark) / total;
                }
            }

            // 3. Infeksi: Chromatic Aberration bisa dimatikan di #19 (usulan #23 poin 4); infeksi tetap terlihat lewat warna.
            if (chromatic != null)
            {
                float caAllowed = SettingsManager.Instance == null || SettingsManager.Instance.chromaticAberrationEnabled ? 1f : 0f;
                float ca = infectionCurrent * caAllowed;
                chromatic.active = ca > 0.001f;
                if (chromatic.active) chromatic.intensity.value = ca;
            }

            // 4. Warna: saturasi infeksi lalu menuju -100 saat mati; filter putih ke warna infeksi lalu warna air.
            if (colorAdjust != null)
            {
                float saturation = Mathf.Lerp(infectionCurrent * pp.infectionSaturationMin, -100f, deathCurrent);
                Color filter = Color.Lerp(Color.Lerp(Color.white, infectionTint, infectionCurrent), underwaterTint, underwaterCurrent);
                colorAdjust.active = saturation < -0.01f || infectionCurrent > 0.001f || underwaterCurrent > 0.001f;
                if (colorAdjust.active)
                {
                    colorAdjust.saturation.value = saturation;
                    colorAdjust.colorFilter.value = filter;
                }
            }

            // 5. Blur bawah air.
            if (dof != null)
            {
                dof.active = underwaterCurrent > 0.001f;
                if (dof.active)
                {
                    dof.gaussianStart.value = pp.underwaterBlurStart;
                    dof.gaussianEnd.value = Mathf.Lerp(dofClearEndMeters, pp.underwaterBlurEnd, underwaterCurrent);
                }
            }

            // 6. Kilas damage memudar.
            hitPulse = Mathf.MoveTowards(hitPulse, 0f, pp.hitPulseDecayPerSec * dt);
        }

        // TODO(DevConsole): perintah "postfx <efek> <0..1>" untuk memaksa target (#56).
    }
}
