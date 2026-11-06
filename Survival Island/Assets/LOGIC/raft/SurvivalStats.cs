using System;
using UnityEngine;

namespace SurvivalIsland
{
    public enum StaminaUse { Sprint, Jump, Melee, Swim, Climb, Struggle }

    public enum VitalPenaltySource { Hunger, Thirst, Infection, Cold, Heavy, StatusEffect }

    [Flags]
    public enum VitalWarningFlags
    {
        None = 0, LowHP = 1, Starving = 2, Dehydrated = 4, Freezing = 8,
        Drowning = 16, Exhausted = 32, InfectionHigh = 64, HungerLow = 128, ThirstLow = 256
    }

    /// <summary>Simpanan status tubuh (VitalsRules.md, "Save dan load").</summary>
    [Serializable]
    public struct SurvivalStatsSaveData
    {
        public float hp, stamina, hunger, thirst, infection;
        public bool isExhausted;
        public float pendingHunger, pendingThirst, hungerRestoreRate, thirstRestoreRate;
    }

    /// <summary>
    /// Pemilik satu-satunya HP, Stamina, Hunger, Thirst, dan Infection (GLOSSARY.md).
    /// Blueprint: raft/SurvivalStats.md (#59, #82, #111), aturan rinci raft/VitalsRules.md, jalur damage Core_Logic/DamagePipeline.md.
    /// GDD bagian 3: lapar, haus, infeksi, dan Game Over saat HP habis. GDD bagian 5: frostbite menguras stamina dulu, lalu HP.
    /// Angka laju dan urutan prioritas adalah usulan yang disetujui developer (#85), dibaca dari GameBalanceConfig.vitals.
    ///
    /// Friendly fire (#59) dan grace PlayerPinnedState (#110) sudah dipakai di TakeDamage. Adegan mati milik
    /// DeathCinematic (#53, dijalankan RunLifecycle). Pemilik penalti Infection dan StatusEffect (InfectionSystem #64, StatusEffectManager #20)
    /// mengisi lewat SetPenalty.
    /// </summary>
    public class SurvivalStats : MonoBehaviour, IDamageable, ISaveable
    {
        public const string SaveIdConst = "player_vitals";
        private const int PenaltyCount = 6;

        public static SurvivalStats Instance { get; private set; }

        [Header("Maksimum dasar")]
        [SerializeField] private float baseMaxHP = 100f;
        [SerializeField] private float baseMaxStamina = 100f;
        [SerializeField] private float baseMaxHunger = 100f;
        [SerializeField] private float baseMaxThirst = 100f;
        [SerializeField] private float baseMaxInfection = 100f;

        public float HP { get; private set; }
        public float Stamina { get; private set; }
        public float Hunger { get; private set; }
        public float Thirst { get; private set; }
        public float Infection { get; private set; }

        public float MaxHP => baseMaxHP * hpBonus;
        public float MaxStamina => baseMaxStamina;
        public float MaxHunger => baseMaxHunger;
        public float MaxThirst => baseMaxThirst;
        public float MaxInfection => baseMaxInfection;

        /// <summary>Batas stamina yang berlaku (MaxStamina x maxStaminaMultiplier), termasuk buff #20 di atas 1.</summary>
        public float EffectiveMaxStamina => MaxStamina * maxStaminaMultiplier;

        public bool IsAlive => !isDead;
        public bool IsDead => isDead;
        public bool IsExhausted { get; private set; }
        public bool IsFreezing => isFreezing;
        public bool IsDrowning => isDrowning;
        public DamageType LastDamageType => lastDamageType;
        public VitalWarningFlags WarningFlags => warningFlags;

        private bool isDead;
        private bool isRunEnded;
        private DamageType lastDamageType = DamageType.Melee;
        private float lastDamageTime = -999f;
        private float lastStaminaUseTime = -999f;
        private int sprintFrame = -10;
        private int exertFrame = -10;

        private bool isFreezing;
        private bool isDrowning;
        private bool carryingHeavy;
        private bool drainEnabled = true;
        private float drainMultiplier = 1f;
        private bool invulnerable;
        private bool hpRegenBlocked;
        private float hpBonus = 1f;

        // Penalti per sumber (VitalsRules.md "Penalti tiap status").
        private readonly float[] penaltyMaxStamina = { 1f, 1f, 1f, 1f, 1f, 1f };
        private readonly float[] penaltyRegen = { 1f, 1f, 1f, 1f, 1f, 1f };
        private float maxStaminaMultiplier = 1f;
        private float staminaRegenMultiplier = 1f;

        // Pemulihan bertahap (QueueRestore, #66).
        private float pendingHunger, pendingThirst, hungerRestoreRate, thirstRestoreRate;

        private float publishTimer;
        private VitalWarningFlags warningFlags;
        private bool warningsPublished;
        private bool registeredForSave;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
            ResetToFull();
        }

        private void OnEnable()
        {
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            EventChannel<WarmthChangedEvent>.Raised += HandleWarmth;
            EventChannel<HeavyItemChangedEvent>.Raised += HandleHeavyItem;
            if (EventChannel<WarmthChangedEvent>.HasLast) isFreezing = EventChannel<WarmthChangedEvent>.Last.isFreezing;
            if (EventChannel<HeavyItemChangedEvent>.HasLast) carryingHeavy = EventChannel<HeavyItemChangedEvent>.Last.isCarried;

            // Fase 1 tidak punya save (#82).
            if (GameManager.Instance != null && GameManager.Instance.currentPhase == GamePhase.IslandSurvival
                && WorldStateSave.Instance != null)
            {
                WorldStateSave.Instance.Register(this);
                registeredForSave = true;
            }
        }

        private void OnDisable()
        {
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            EventChannel<WarmthChangedEvent>.Raised -= HandleWarmth;
            EventChannel<HeavyItemChangedEvent>.Raised -= HandleHeavyItem;
            if (registeredForSave && WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            registeredForSave = false;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        /// <summary>Status awal run: semua penuh, Infection 0 (VitalsRules.md, permadeath).</summary>
        public void ResetToFull()
        {
            HP = MaxHP;
            Stamina = MaxStamina;
            Hunger = MaxHunger;
            Thirst = MaxThirst;
            Infection = 0f;
            IsExhausted = false;
            pendingHunger = pendingThirst = hungerRestoreRate = thirstRestoreRate = 0f;
        }

        /// <summary>
        /// Dipanggil IslandArrival.Apply (#74) langkah 3: HP dan Infection dibawa dari kapal, Stamina, Hunger, dan Thirst penuh
        /// (usulan disetujui VitalsRules.md poin 8). Catatan implementasi (Langkah 3): nama fungsi adalah usulan; blueprint
        /// hanya menulis "HP = shipOutcome.hpOnExit, Infection = shipOutcome.infectionOnExit".
        /// </summary>
        public void ApplyArrivalState(float hp, float infection)
        {
            ResetToFull();
            HP = Mathf.Clamp(hp, 1f, MaxHP);
            Infection = Mathf.Clamp(infection, 0f, MaxInfection);
            lastDamageTime = Time.time;
            PublishVitals();
        }

        // ---------------------------------------------------------------- Update

        private void Update()
        {
            if (isDead || isRunEnded) return;
            var b = Balance != null ? Balance.vitals : null;
            if (b == null) return;
            float dt = Time.deltaTime;

            TickDrains(dt, b);
            TickRestore(dt);
            UpdateInternalPenalties(b);
            TickStamina(dt, b);
            TickHP(dt, b);
            if (isDead) return;

            publishTimer -= dt;
            if (publishTimer <= 0f)
            {
                publishTimer = b.vitalsPublishIntervalSeconds;
                PublishVitals();
            }
        }

        private void TickDrains(float dt, VitalsBalance b)
        {
            if (!drainEnabled) return;
            var diff = Difficulty;
            float hungerMult = diff != null ? diff.hungerMultiplier : 1f;
            float thirstMult = diff != null ? diff.thirstMultiplier : 1f;

            Hunger = Mathf.Clamp(Hunger - b.hungerDrainPerSec * drainMultiplier * hungerMult * dt, 0f, MaxHunger);

            float thirstRate = b.thirstDrainPerSec * drainMultiplier * thirstMult;
            float activity = 1f;
            // isSprinting dan isExerting diisi otomatis oleh SpendStamina pada frame ini atau frame sebelumnya.
            if (Time.frameCount - sprintFrame <= 1) activity = Mathf.Max(activity, b.sprintThirstMultiplier);
            if (Time.frameCount - exertFrame <= 1)
                activity = Mathf.Max(activity, Balance != null ? Balance.hydration.exertionThirstMultiplier : 1f);
            Thirst = Mathf.Clamp(Thirst - thirstRate * activity * dt, 0f, MaxThirst);
        }

        private void TickRestore(float dt)
        {
            if (pendingHunger > 0f)
            {
                float add = Mathf.Min(pendingHunger, hungerRestoreRate * dt);
                pendingHunger -= add;
                Hunger = Mathf.Min(MaxHunger, Hunger + add);
            }
            if (pendingThirst > 0f)
            {
                float add = Mathf.Min(pendingThirst, thirstRestoreRate * dt);
                pendingThirst -= add;
                Thirst = Mathf.Min(MaxThirst, Thirst + add);
            }
        }

        /// <summary>Penalti yang diisi SurvivalStats sendiri: lapar, haus, dingin, dan beban.</summary>
        private void UpdateInternalPenalties(VitalsBalance b)
        {
            Set(VitalPenaltySource.Hunger, Hunger < b.lowVitalThreshold ? b.lowHungerMaxStaminaMultiplier : 1f, 1f);
            // Haus: regenerasi setengah saat rendah, 0 saat habis (#43).
            // Nilai saat habis dari hydration.dehydratedStaminaRegenMultiplier (HydrationSystem.md, nilai awal 0).
            float dehydratedRegen = Balance != null ? Balance.hydration.dehydratedStaminaRegenMultiplier : 0f;
            float thirstRegen = Thirst <= 0f ? dehydratedRegen : (Thirst < b.lowVitalThreshold ? b.lowThirstRegenMultiplier : 1f);
            Set(VitalPenaltySource.Thirst, 1f, thirstRegen);
            Set(VitalPenaltySource.Cold, 1f, isFreezing ? 0f : 1f);
            Set(VitalPenaltySource.Heavy, 1f, carryingHeavy ? b.heavyStaminaRegenMultiplier : 1f);
            RecalculatePenalties();

            void Set(VitalPenaltySource s, float max, float regen)
            {
                penaltyMaxStamina[(int)s] = max;
                penaltyRegen[(int)s] = regen;
            }
        }

        /// <summary>
        /// maxStaminaMultiplier = (terkecil dari sumber selain StatusEffect) x nilai StatusEffect, supaya buff di atas 1
        /// tidak hilang (#20). regenMultiplier = hasil kali semua sumber.
        /// </summary>
        private void RecalculatePenalties()
        {
            float min = 1f;
            float regen = 1f;
            for (int i = 0; i < PenaltyCount; i++)
            {
                regen *= penaltyRegen[i];
                if (i == (int)VitalPenaltySource.StatusEffect) continue;
                min = Mathf.Min(min, penaltyMaxStamina[i]);
            }
            maxStaminaMultiplier = min * penaltyMaxStamina[(int)VitalPenaltySource.StatusEffect];
            staminaRegenMultiplier = regen;
        }

        private void TickStamina(float dt, VitalsBalance b)
        {
            // 1. GDD bagian 5, Region 3: dingin menguras stamina dulu.
            if (isFreezing)
            {
                Stamina = Mathf.Max(0f, Stamina - b.frostbiteStaminaDrainPerSec * dt);
                lastStaminaUseTime = Time.time;
            }
            else if (Time.time - lastStaminaUseTime >= b.staminaRegenDelaySeconds)
            {
                // 2-3.
                Stamina += b.staminaRegenPerSec * staminaRegenMultiplier * dt;
            }
            // 4. Batas turun langsung memotong stamina; tidak kembali otomatis saat batas naik.
            Stamina = Mathf.Clamp(Stamina, 0f, EffectiveMaxStamina);
            // 5. Keluar dari kelelahan.
            if (IsExhausted && Stamina >= b.exhaustedRecoverThreshold) IsExhausted = false;
        }

        /// <summary>
        /// Tabel prioritas HP (VitalsRules.md): hanya baris pertama yang cocok yang berlaku, kerusakan kondisi tidak bertumpuk
        /// (usulan disetujui #85, pola Raft di CATATAN_MEKANIK.md).
        /// </summary>
        private void TickHP(float dt, VitalsBalance b)
        {
            if (isDrowning) ApplyVitalDamage(b.drownDamagePerSec * dt, DamageType.Drown);
            // GDD bagian 5, Region 3: HP turun setelah stamina habis karena dingin.
            else if (isFreezing && Stamina <= 0f) ApplyVitalDamage(b.frostbiteDamagePerSec * dt, DamageType.Frost);
            // GDD bagian 3: HP habis karena lapar atau haus.
            else if (Hunger <= 0f) ApplyVitalDamage(b.starvationDamagePerSec * dt, DamageType.Starvation);
            else if (Thirst <= 0f) ApplyVitalDamage(b.dehydrationDamagePerSec * dt, DamageType.Dehydration);
            // Keputusan developer: infeksi tinggi (termasuk 100) menurunkan HP bertahap, tidak membunuh seketika.
            else if (Infection >= b.infectionDamageThreshold) ApplyVitalDamage(b.infectionDamagePerSec * dt, DamageType.Infection);
            else if (CanRegenHP(b)) HP = Mathf.Min(HP + b.regenPerSec * dt, MaxHP);
        }

        private bool CanRegenHP(VitalsBalance b)
        {
            return Hunger >= b.regenMinHunger
                && Thirst >= b.regenMinThirst
                && Infection < b.regenMaxInfection
                && Time.time - lastDamageTime >= b.regenDelayAfterDamageSeconds
                && !hpRegenBlocked;
        }

        private static DifficultyProfile Difficulty
        {
            get
            {
                // Catatan implementasi (Langkah 3): profil kesulitan aktif belum punya aturan pilih. Profil pertama dipakai.
                var bal = Balance;
                return bal != null && bal.difficulties != null && bal.difficulties.Length > 0 ? bal.difficulties[0] : null;
            }
        }

        // ---------------------------------------------------------------- Damage (#59)

        /// <summary>Damage dari luar, mengikuti DamagePipeline.md "Menerima damage di pemain".</summary>
        public void TakeDamage(in DamageInfo info)
        {
            // 1. SurvivalStats.md: abaikan setelah mati atau setelah run berakhir (#82).
            if (isDead || isRunEnded) return;
            float amount = info.amount;
            // 2. Friendly fire (DamagePipeline.md, usulan #59): peluru (pemain atau NPC) dan melee pemain tidak melukai pemain;
            //    ledakan dan api buatan pemain dikali playerExplosionSelfMultiplier.
            if (info.type == DamageType.Bullet && (info.byPlayer || (info.source != null && info.source.GetComponentInParent<NPCCompanion>() != null))) amount = 0f;
            else if (info.type == DamageType.Melee && info.byPlayer) amount = 0f;
            else if ((info.type == DamageType.Explosion || info.type == DamageType.Fire) && info.byPlayer)
                amount *= Balance != null ? Balance.damage.playerExplosionSelfMultiplier : 0.5f;
            // 3. Hook Langkah 5 (#110): grace dan pengali musuh lain saat tertahan.
            amount = PlayerPinnedState.ModifyIncomingEnemyDamage(in info, amount);
            // 4. Serangan musuh dikali DifficultyProfile.enemyDamageMultiplier.
            bool fromEnemy = !info.byPlayer && (info.type == DamageType.Melee || info.type == DamageType.Bullet || info.type == DamageType.Structural);
            if (fromEnemy && Difficulty != null) amount *= Difficulty.enemyDamageMultiplier;
            if (amount <= 0f && info.infection <= 0f) return;

            // 5.
            ReduceHP(amount, info.type);
            // 6. GDD bagian 3: serangan zombie menambah infeksi.
            if (!isDead && info.infection > 0f) AddInfection(info.infection);
            // 7.
            lastDamageTime = Time.time;
            // 8.
            EventChannel<PlayerDamagedEvent>.Publish(new PlayerDamagedEvent(info, HP));
        }

        /// <summary>Damage kondisi tanpa PlayerDamagedEvent (supaya indikator arah #51 tidak muncul tiap frame).</summary>
        public void ApplyVitalDamage(float amount, DamageType type)
        {
            if (isDead || isRunEnded) return;
            ReduceHP(amount, type);
        }

        private void ReduceHP(float amount, DamageType type)
        {
            if (amount <= 0f) return;
            lastDamageType = type;
            HP -= amount;
            if (invulnerable && HP < 1f) HP = 1f;
            if (HP <= 0f)
            {
                HP = 0f;
                TriggerDeath(DeathReasonMap.From(type));
            }
        }

        /// <summary>Idempoten (#82): damage mematikan kedua pada frame yang sama diabaikan.</summary>
        private void TriggerDeath(DeathReason reason)
        {
            if (isDead) return;
            isDead = true;
            // Matikan gerak. Urutan jatuh dijalankan DeathCinematic lewat RunLifecycle setelah PlayerDiedEvent (#53).
            var traversal = GetComponent<PlayerTraversal>();
            if (traversal != null) traversal.SetLocked(true);
            var controller = GetComponent<IslandPlayerController>();
            if (controller != null) controller.enabled = false;
            PublishVitals();
            // Hanya RunLifecycle yang menghapus save dan memuat scene (#82). PlayerPrefs.DeleteAll() dilarang.
            EventChannel<PlayerDiedEvent>.Publish(new PlayerDiedEvent(reason));
        }

        // ---------------------------------------------------------------- Pemulihan (#66, #67)

        public void RestoreHP(float amount)
        {
            if (isDead || amount <= 0f) return;
            HP = Mathf.Min(MaxHP, HP + amount);
        }

        public void RestoreHunger(float amount)
        {
            if (isDead || amount <= 0f) return;
            Hunger = Mathf.Min(MaxHunger, Hunger + amount);
        }

        public void RestoreThirst(float amount)
        {
            if (isDead || amount <= 0f) return;
            Thirst = Mathf.Min(MaxThirst, Thirst + amount);
        }

        public void RestoreStamina(float amount)
        {
            if (isDead || amount <= 0f) return;
            Stamina = Mathf.Min(EffectiveMaxStamina, Stamina + amount);
        }

        /// <summary>
        /// Pemulihan bertahap: makanan menaikkan target, nilai mengejarnya selama seconds (CATATAN_MEKANIK bagian 2, Raft).
        /// Tidak batal saat diserang.
        /// </summary>
        public void QueueRestore(float hunger, float thirst, float seconds)
        {
            if (isDead) return;
            seconds = Mathf.Max(0.01f, seconds);
            if (hunger > 0f)
            {
                pendingHunger += hunger;
                hungerRestoreRate = pendingHunger / seconds;
            }
            if (thirst > 0f)
            {
                pendingThirst += thirst;
                thirstRestoreRate = pendingThirst / seconds;
            }
        }

        public void AddInfection(float amount)
        {
            if (isDead || amount <= 0f) return;
            Infection = Mathf.Min(MaxInfection, Infection + amount);
            if (invulnerable && Infection > MaxInfection - 1f) Infection = MaxInfection - 1f;
        }

        public void ReduceInfection(float amount)
        {
            if (amount <= 0f) return;
            Infection = Mathf.Max(0f, Infection - amount);
        }

        // ---------------------------------------------------------------- Stamina

        public bool SpendStamina(float amount, StaminaUse use)
        {
            if (isDead) return false;
            // 1. Lari dan lompat ditolak saat lelah.
            if ((use == StaminaUse.Sprint || use == StaminaUse.Jump) && IsExhausted) return false;
            // 2.
            if (use == StaminaUse.Jump && Stamina < amount) return false;
            if (use == StaminaUse.Struggle && Stamina <= 0f) return false;
            // 3.
            Stamina = Mathf.Max(0f, Stamina - Mathf.Max(0f, amount));
            lastStaminaUseTime = Time.time;
            if (use == StaminaUse.Sprint) sprintFrame = Time.frameCount;
            else if (use == StaminaUse.Swim || use == StaminaUse.Climb) exertFrame = Time.frameCount;
            // 4.
            if (Stamina <= 0f) IsExhausted = true;
            return true;
        }

        /// <summary>Pemeriksaan tanpa mengurangi. Melee tetap boleh saat lelah, dengan pengali (MeleeCombatSystem).</summary>
        public bool CanUseStamina(StaminaUse use)
        {
            if (isDead) return false;
            switch (use)
            {
                case StaminaUse.Sprint: return !IsExhausted && Stamina > 0f;
                case StaminaUse.Jump:
                    return !IsExhausted && Balance != null && Stamina >= Balance.vitals.jumpStaminaCost;
                case StaminaUse.Struggle: return Stamina > 0f;
                default: return true;
            }
        }

        // ---------------------------------------------------------------- Kondisi dari sistem lain

        public void SetDrowning(bool value) => isDrowning = value;

        public void SetPenalty(VitalPenaltySource source, float maxStaminaMultiplierValue, float staminaRegenMultiplierValue)
        {
            int i = (int)source;
            penaltyMaxStamina[i] = Mathf.Max(0f, maxStaminaMultiplierValue);
            penaltyRegen[i] = Mathf.Max(0f, staminaRegenMultiplierValue);
            RecalculatePenalties();
        }

        /// <summary>Pengali naik MaxHP dari buff Permanent Lucky Machine (#20, #72). HP saat ini tidak ikut naik.</summary>
        public void SetMaxBonus(float hpMultiplier)
        {
            hpBonus = Mathf.Max(0.01f, hpMultiplier);
            HP = Mathf.Min(HP, MaxHP);
        }

        public void SetHPRegenBlocked(bool value) => hpRegenBlocked = value;
        public void SetDrainMultiplier(float value) => drainMultiplier = Mathf.Max(0f, value);
        /// <summary>Fase 1: lapar dan haus mati (LifeboatExtraction.PreGameRoutine, #74).</summary>
        public void SetDrainEnabled(bool value) => drainEnabled = value;

        /// <summary>HP tertahan di 1 dan infeksi di 99 (DevConsole #56, akhir Fase 1 #74).</summary>
        public void SetInvulnerable(bool value)
        {
            invulnerable = value;
            if (invulnerable)
            {
                HP = Mathf.Max(HP, 1f);
                Infection = Mathf.Min(Infection, MaxInfection - 1f);
            }
        }

        // ---------------------------------------------------------------- Event

        private void HandleRunEnded(RunEndedEvent e)
        {
            // Menang saat kondisi kritis: lapar, haus, dan damage berhenti (#82).
            isRunEnded = true;
        }

        private void HandleWarmth(WarmthChangedEvent e) => isFreezing = e.isFreezing;
        private void HandleHeavyItem(HeavyItemChangedEvent e) => carryingHeavy = e.isCarried;

        private void PublishVitals()
        {
            EventChannel<VitalsChangedEvent>.Publish(new VitalsChangedEvent
            {
                hp = HP, stamina = Stamina, hunger = Hunger, thirst = Thirst, infection = Infection
            });
            RefreshWarnings();
        }

        /// <summary>VitalWarningChangedEvent hanya saat kombinasi berubah (sticky).</summary>
        private void RefreshWarnings()
        {
            var b = Balance != null ? Balance.vitals : null;
            float low = b != null ? b.lowVitalThreshold : 25f;
            var f = VitalWarningFlags.None;
            if (MaxHP > 0f && HP / MaxHP < (b != null ? b.lowHPWarningRatio : 0.25f)) f |= VitalWarningFlags.LowHP;
            if (Hunger < low) f |= VitalWarningFlags.HungerLow;
            if (Thirst < low) f |= VitalWarningFlags.ThirstLow;
            if (Hunger <= 0f) f |= VitalWarningFlags.Starving;
            if (Thirst <= 0f) f |= VitalWarningFlags.Dehydrated;
            if (isFreezing) f |= VitalWarningFlags.Freezing;
            if (isDrowning) f |= VitalWarningFlags.Drowning;
            if (IsExhausted) f |= VitalWarningFlags.Exhausted;
            if (Infection >= (b != null ? b.infectionDamageThreshold : 75f)) f |= VitalWarningFlags.InfectionHigh;

            if (warningsPublished && f == warningFlags) return;
            warningFlags = f;
            warningsPublished = true;
            EventChannel<VitalWarningChangedEvent>.Publish(new VitalWarningChangedEvent(f));
        }

        // ---------------------------------------------------------------- Save (#80)

        public string SaveId => SaveIdConst;
        public int LoadOrder => 70;

        public string CaptureState()
        {
            return JsonUtility.ToJson(new SurvivalStatsSaveData
            {
                hp = HP, stamina = Stamina, hunger = Hunger, thirst = Thirst, infection = Infection,
                isExhausted = IsExhausted,
                pendingHunger = pendingHunger, pendingThirst = pendingThirst,
                hungerRestoreRate = hungerRestoreRate, thirstRestoreRate = thirstRestoreRate,
            });
        }

        /// <summary>isFreezing dan isDrowning dihitung ulang. lastDamageTime = saat dimuat, jadi jeda pulih penuh.</summary>
        public void RestoreState(string json)
        {
            var d = JsonUtility.FromJson<SurvivalStatsSaveData>(json);
            HP = Mathf.Clamp(d.hp, 0f, MaxHP);
            Stamina = Mathf.Clamp(d.stamina, 0f, MaxStamina);
            Hunger = Mathf.Clamp(d.hunger, 0f, MaxHunger);
            Thirst = Mathf.Clamp(d.thirst, 0f, MaxThirst);
            Infection = Mathf.Clamp(d.infection, 0f, MaxInfection);
            IsExhausted = d.isExhausted;
            pendingHunger = Mathf.Max(0f, d.pendingHunger);
            pendingThirst = Mathf.Max(0f, d.pendingThirst);
            hungerRestoreRate = d.hungerRestoreRate;
            thirstRestoreRate = d.thirstRestoreRate;
            lastDamageTime = Time.time;
            isDrowning = false;
        }

        public void OnRestoreFinished()
        {
            warningsPublished = false;
            PublishVitals();
        }

        // ---------------------------------------------------------------- DevConsole (#56)

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        /// <summary>Perintah setvital. Nama: hp, stamina, hunger, thirst, infection.</summary>
        public void DebugSetVital(string name, float value)
        {
            switch ((name ?? string.Empty).ToLowerInvariant())
            {
                case "hp": HP = Mathf.Clamp(value, 0f, MaxHP); break;
                case "stamina": Stamina = Mathf.Clamp(value, 0f, EffectiveMaxStamina); break;
                case "hunger": Hunger = Mathf.Clamp(value, 0f, MaxHunger); break;
                case "thirst": Thirst = Mathf.Clamp(value, 0f, MaxThirst); break;
                case "infection": Infection = Mathf.Clamp(value, 0f, MaxInfection); break;
                default: Debug.LogWarning($"[SurvivalStats] Status '{name}' tidak dikenal."); return;
            }
            PublishVitals();
            if (HP <= 0f) TriggerDeath(DeathReasonMap.From(lastDamageType));
        }

        public void DebugKill(DeathReason reason)
        {
            HP = 0f;
            TriggerDeath(reason);
        }
#endif
    }
}
