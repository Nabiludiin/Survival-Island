using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Alur infeksi zombie: tahap 1-4, penalti stamina, dan efek vaksin. Komponen di pemain Fase 2.
    /// Blueprint: raft/InfectionSystem.md (#64).
    ///
    /// GDD bagian 3: infeksi naik karena serangan zombie, vaksin sangat sedikit, dan pemain mati saat HP habis.
    /// SurvivalStats tetap satu-satunya pemilik nilai Infection (GLOSSARY.md). Kelas ini hanya membaca nilai,
    /// menentukan tahap, memasang penalti lewat SetPenalty, dan menerapkan vaksin.
    /// HP tahap 3 dan 4 dihitung SurvivalStats sendiri (VitalsRules.md baris HP 5), bukan di sini.
    ///
    /// Belum ada di Langkah 4 (TODO): notifikasi (#22), pengali NPC medis (NPCRoster #76), efek layar (#23 membaca event).
    /// </summary>
    [DisallowMultipleComponent]
    public class InfectionSystem : MonoBehaviour
    {
        public static InfectionSystem Instance { get; private set; }

        [SerializeField] private SurvivalStats stats;

        public int CurrentStage { get; private set; } = 1;
        private bool warnedCritical;
        private bool stopped;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        private void Awake()
        {
            Instance = this;
            if (stats == null) stats = GetComponent<SurvivalStats>();
        }

        private void OnEnable()
        {
            EventChannel<VitalsChangedEvent>.Raised += HandleVitals;
            EventChannel<HourChangedEvent>.Raised += HandleHour;
            EventChannel<PlayerDiedEvent>.Raised += HandlePlayerDied;
        }

        private void OnDisable()
        {
            EventChannel<VitalsChangedEvent>.Raised -= HandleVitals;
            EventChannel<HourChangedEvent>.Raised -= HandleHour;
            EventChannel<PlayerDiedEvent>.Raised -= HandlePlayerDied;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void Start()
        {
            // Pindah dari Fase 1: infeksi terbawa lewat shipOutcome, jadi tahap langsung dihitung (Kasus Khusus).
            // Penalti tahap 1 juga dipasang supaya nilai lama tidak tertinggal.
            if (stats != null) ApplyStage(GetStage(stats.Infection), true);
        }

        // ---------------------------------------------------------------- Tahap

        private void HandleVitals(VitalsChangedEvent e)
        {
            if (stopped) return;
            int stage = GetStage(e.infection);
            if (stage != CurrentStage) ApplyStage(stage, false);

            float critical = Balance != null ? Balance.infection.criticalWarningAt : 90f;
            if (e.infection >= critical && !warnedCritical)
            {
                warnedCritical = true;
                // TODO #22: NotificationManager "Infeksi kritis, segera suntik vaksin".
            }
            else if (e.infection < critical)
            {
                warnedCritical = false;
            }
        }

        /// <summary>Tahap dari nilai infeksi. Batas dari config (stage4At = vitals.infectionDamageThreshold).</summary>
        public int GetStage(float value)
        {
            var b = Balance != null ? Balance.infection : null;
            float s4 = b != null ? b.stage4At : 75f;
            float s3 = b != null ? b.stage3At : 50f;
            float s2 = b != null ? b.stage2At : 25f;
            if (value >= s4) return 4;
            if (value >= s3) return 3;
            if (value >= s2) return 2;
            return 1;
        }

        private void ApplyStage(int stage, bool force)
        {
            int previous = CurrentStage;
            CurrentStage = stage;

            if (stats != null)
            {
                float maxStamina = ReadStageValue(Balance != null ? Balance.infection.stageMaxStamina : null, stage);
                float regen = ReadStageValue(Balance != null ? Balance.infection.stageStaminaRegen : null, stage);
                stats.SetPenalty(VitalPenaltySource.Infection, maxStamina, regen);
            }

            EventChannel<InfectionStageChangedEvent>.Publish(new InfectionStageChangedEvent(stage));
            if (!force && stage > previous)
            {
                // TODO #22: notifikasi tahap, contoh "Mulai demam" untuk tahap 2.
            }
        }

        private static float ReadStageValue(float[] values, int stage)
        {
            int i = stage - 1;
            if (values == null || i < 0 || i >= values.Length) return 1f;
            return values[i];
        }

        // ---------------------------------------------------------------- Naik sendiri (nilai awal 0 = mati)

        private void HandleHour(HourChangedEvent e)
        {
            if (stopped || stats == null || Balance == null) return;
            // Usulan disetujui: hanya naik saat diserang. Aktif kalau selfProgressPerGameHour > 0.
            float perHour = Balance.infection.selfProgressPerGameHour;
            if (perHour > 0f && stats.Infection >= Balance.infection.selfProgressMinInfection)
                stats.AddInfection(perHour);
        }

        // ---------------------------------------------------------------- Vaksin (alur ItemUseSystem #66)

        /// <summary>Dipanggil ItemUseSystem.TryUse langkah 2 untuk item dengan infectionDelta negatif.</summary>
        public bool CanUseMedicine(ItemData item)
        {
            if (stats == null || item == null) return false;
            if (stats.Infection <= 0f)
            {
                // TODO #22: notifikasi "Tidak terinfeksi".
                return false;
            }
            return true;
        }

        /// <summary>Dipanggil ItemUseSystem.CompleteUse langkah 6. GDD bagian 3: vaksin menurunkan infeksi.</summary>
        public void ApplyMedicine(ItemData item)
        {
            if (stats == null || item == null || stats.IsDead) return;
            float amount = -item.infectionDelta;
            if (amount <= 0f) return;

            // TODO #76: amount *= NPCRoster.Instance.GetSkillMultiplier(NPCSkill.Medic) (medicVaccineMultiplier saat NPC medis hidup).
            stats.ReduceInfection(amount);
            EventChannel<VaccineUsedEvent>.Publish(new VaccineUsedEvent(amount, stats.Infection));

            // Tahap langsung turun tanpa menunggu VitalsChangedEvent berikutnya.
            int stage = GetStage(stats.Infection);
            if (stage != CurrentStage) ApplyStage(stage, false);
            if (stats.Infection < (Balance != null ? Balance.infection.criticalWarningAt : 90f)) warnedCritical = false;
        }

        private void HandlePlayerDied(PlayerDiedEvent e)
        {
            stopped = true;
        }
    }
}
