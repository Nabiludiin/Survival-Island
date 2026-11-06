using System;
using UnityEngine;

namespace SurvivalIsland
{
    public enum SleepBlockReason
    {
        None, NotNight, EnemyNearby, DisasterActive, BloodMoon, Freezing, VitalsTooLow, RecentlyWoken
    }

    public enum SleepWakeReason { None, Daybreak, Manual, Attacked, EnemyNear, Interrupted, Disaster, TideRising, VitalEmpty, BedDestroyed, RunEnded, Freezing }

    /// <summary>
    /// Tidur di Kasur untuk melewati malam dengan jam dunia dipercepat (tanpa Time.timeScale), dengan syarat, risiko,
    /// pemulihan, hukuman di pantai, dan pemicu autosave. Manager di scene IslandPhase. Satu jalur bangun: EndSleep.
    /// Blueprint: Island_Logic/SleepSystem.md (#67). GDD bagian 3 (siklus siang/malam, target 60 hari) dan bagian 4 (fungsi
    /// base, peringatan Diary Bab 1 agar tidak tidur di pantai). Mekanik dan angka adalah usulan disetujui developer (#85).
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - BaseNavigation.IsInsideBase(pos) belum ada; pemain berdiri di kasur, jadi dipakai BaseNavigation.IsPlayerInsideBase.
    /// - Animasi kamera berbaring (#102) belum ada: TODO(FirstPersonCamera).
    /// - Status tidur tidak disimpan (SleepSystem.md bagian 4), jadi kelas ini bukan ISaveable.
    /// </summary>
    public class SleepSystem : MonoBehaviour
    {
        public static SleepSystem Instance { get; private set; }

        [SerializeField] private SleepPanel sleepPanel;
        [SerializeField] private PlayerInteractor playerInteractor;

        private static readonly Collider[] enemyBuffer = new Collider[8];

        private BedInteractable currentBed;
        private float startGameHour;
        private bool inBase;
        private bool onBeach;
        private float interruptAtGameHour = -1f;
        private float lastWokenTime = -999f;
        private float enemyTimer;
        private bool attackedPending;

        public bool IsSleeping { get; private set; }
        public BedInteractable CurrentBed => currentBed;
        public float HoursSlept => IsSleeping ? NowHours - startGameHour : 0f;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;
        private static float NowHours => DayNightDirector.Instance != null ? (float)DayNightDirector.Instance.TotalGameHours : 0f;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake() { Instance = this; }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<DaybreakEvent>.Raised += HandleDaybreak;
            EventChannel<PlayerDamagedEvent>.Raised += HandlePlayerDamaged;
            EventChannel<StructureDamagedEvent>.Raised += HandleStructureDamaged;
            EventChannel<StructureDestroyedEvent>.Raised += HandleStructureDestroyed;
            EventChannel<DisasterStartedEvent>.Raised += HandleDisasterStarted;
            EventChannel<TideChangedEvent>.Raised += HandleTide;
            EventChannel<WarmthChangedEvent>.Raised += HandleWarmth;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
        }

        private void OnDisable()
        {
            EventChannel<DaybreakEvent>.Raised -= HandleDaybreak;
            EventChannel<PlayerDamagedEvent>.Raised -= HandlePlayerDamaged;
            EventChannel<StructureDamagedEvent>.Raised -= HandleStructureDamaged;
            EventChannel<StructureDestroyedEvent>.Raised -= HandleStructureDestroyed;
            EventChannel<DisasterStartedEvent>.Raised -= HandleDisasterStarted;
            EventChannel<TideChangedEvent>.Raised -= HandleTide;
            EventChannel<WarmthChangedEvent>.Raised -= HandleWarmth;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            if (IsSleeping) EndSleep(SleepWakeReason.RunEnded);
        }

        // ---------------------------------------------------------------- Syarat tidur

        /// <summary>Berhenti pada alasan pertama (tabel "Syarat tidur").</summary>
        public SleepBlockReason GetBlockReason()
        {
            var b = Balance;
            if (b == null) return SleepBlockReason.NotNight;
            var s = b.sleep;
            // 1.
            if (DayNightDirector.Instance == null || !DayNightDirector.Instance.IsNight) return SleepBlockReason.NotNight;
            // 2.
            if (Time.time - lastWokenTime < s.retrySeconds) return SleepBlockReason.RecentlyWoken;
            // 3.
            if (DisasterManager.Instance != null && DisasterManager.Instance.IsActive) return SleepBlockReason.DisasterActive;
            // 4.
            if (HordeManager.Instance != null && HordeManager.Instance.IsBloodMoonActive) return SleepBlockReason.BloodMoon;
            // 5.
            if (WarmthSystem.Instance != null && WarmthSystem.Instance.IsFreezing) return SleepBlockReason.Freezing;
            // 6.
            var stats = SurvivalStats.Instance;
            if (stats != null && (stats.Hunger < s.minVitalToSleep || stats.Thirst < s.minVitalToSleep)) return SleepBlockReason.VitalsTooLow;
            // 7.
            var player = IslandPlayerController.Instance;
            if (player != null && HasActiveEnemyWithin(player.transform.position, s.enemyBlockRadius)) return SleepBlockReason.EnemyNearby;
            return SleepBlockReason.None;
        }

        public static string BlockMessageKey(SleepBlockReason r)
        {
            switch (r)
            {
                case SleepBlockReason.NotNight: return "sleep.block.not_night";          // "Hanya bisa tidur saat malam"
                case SleepBlockReason.RecentlyWoken: return "sleep.block.recently_woken"; // "Masih terlalu gelisah"
                case SleepBlockReason.DisasterActive: return "sleep.block.disaster";      // "Tidak aman saat bencana"
                case SleepBlockReason.BloodMoon: return "sleep.block.blood_moon";         // "Malam ini terlalu berbahaya"
                case SleepBlockReason.Freezing: return "sleep.block.freezing";            // "Terlalu dingin untuk tidur"
                case SleepBlockReason.VitalsTooLow: return "sleep.block.vitals";          // "Terlalu lapar atau haus"
                case SleepBlockReason.EnemyNearby: return "sleep.block.enemy";            // "Ada musuh di dekat sini"
                default: return string.Empty;
            }
        }

        public static string BlockMessageFallback(SleepBlockReason r)
        {
            switch (r)
            {
                case SleepBlockReason.NotNight: return "Hanya bisa tidur saat malam";
                case SleepBlockReason.RecentlyWoken: return "Masih terlalu gelisah";
                case SleepBlockReason.DisasterActive: return "Tidak aman saat bencana";
                case SleepBlockReason.BloodMoon: return "Malam ini terlalu berbahaya";
                case SleepBlockReason.Freezing: return "Terlalu dingin untuk tidur";
                case SleepBlockReason.VitalsTooLow: return "Terlalu lapar atau haus";
                case SleepBlockReason.EnemyNearby: return "Ada musuh di dekat sini";
                default: return string.Empty;
            }
        }

        /// <summary>Musuh yang Dormant, Submerged, BurnToAshes, atau mati tidak dihitung (#63).</summary>
        private static bool HasActiveEnemyWithin(Vector3 pos, float radius)
        {
            int n = PhysicsQuery.OverlapSphere(pos, radius, PhysicsMasks.EnemyBody, enemyBuffer);
            bool found = false;
            for (int i = 0; i < n && !found; i++)
            {
                var c = enemyBuffer[i];
                if (c == null) continue;
                var inst = c.GetComponentInParent<EnemyInstance>();
                if (inst == null) continue;
                if (inst.AI != null) found = inst.AI.IsActiveThreat;
                else found = inst.Health != null && inst.Health.IsAlive && !inst.IsCarcass;
            }
            for (int i = 0; i < n; i++) enemyBuffer[i] = null;
            return found;
        }

        // ---------------------------------------------------------------- Mulai tidur

        public bool TrySleep(BedInteractable bed)
        {
            if (IsSleeping || bed == null) return false;
            var reason = GetBlockReason();
            // 1.
            if (reason != SleepBlockReason.None)
            {
                if (NotificationManager.Instance != null) NotificationManager.Instance.Show(BlockMessageKey(reason), NotificationKind.Warning);
                return false;
            }
            // 2.
            return BeginSleep(bed);
        }

        private bool BeginSleep(BedInteractable bed)
        {
            var b = Balance;
            var s = b.sleep;
            var stack = UIFocusStack.Instance;
            if (sleepPanel == null || stack == null) return false;

            // 1.
            currentBed = bed;
            startGameHour = NowHours;
            inBase = BaseNavigation.Instance != null && BaseNavigation.Instance.IsPlayerInsideBase;
            onBeach = SurfaceDetection.Instance != null && SurfaceDetection.Instance.IsNearSand(bed.transform.position, s.beachRadius);
            attackedPending = false;
            // 2.
            RollInterrupt();
            // 6. Panel lebih dulu: kalau ditolak stack, tidur dibatalkan tanpa efek tersisa.
            IsSleeping = true;
            if (!stack.Push(sleepPanel))
            {
                IsSleeping = false;
                currentBed = null;
                return false;
            }
            // 3. Percepatan waktu tanpa Time.timeScale: hanya jam dunia.
            if (DayNightDirector.Instance != null) DayNightDirector.Instance.SetTimeMultiplierOverride(s.timeMultiplierAsleep);
            // 4. Usulan 4: lapar dan haus setengah laju biasa pada jam game yang sama.
            if (SurvivalStats.Instance != null)
                SurvivalStats.Instance.SetDrainMultiplier(s.timeMultiplierAsleep / Mathf.Max(0.0001f, b.time.timeMultiplier) * s.drainRatioAsleep);
            // 5. Pemain tidak dipindahkan (RegionStreaming.md).
            var interactor = Interactor();
            if (interactor != null) interactor.SetEnabled(false);
            if (EquipmentManager.Instance != null) EquipmentManager.Instance.PushLock(EquipLock.Menu);
            // TODO(FirstPersonCamera #102): animasi kamera berbaring.
            enemyTimer = 0f;
            // 7.
            EventChannel<SleepStateChangedEvent>.Publish(new SleepStateChangedEvent(true, SleepWakeReason.None));
            return true;
        }

        private PlayerInteractor Interactor()
        {
            if (playerInteractor == null && IslandPlayerController.Instance != null)
                playerInteractor = IslandPlayerController.Instance.GetComponentInChildren<PlayerInteractor>();
            return playerInteractor;
        }

        // ---------------------------------------------------------------- Selama tidur

        private void Update()
        {
            if (!IsSleeping) return;
            var b = Balance;
            if (b == null) return;
            // 2.
            if (interruptAtGameHour >= 0f && NowHours >= interruptAtGameHour)
            {
                TriggerInterrupt();
                return;
            }
            // 3.
            var stats = SurvivalStats.Instance;
            if (stats != null && (stats.Hunger <= 0f || stats.Thirst <= 0f))
            {
                EndSleep(SleepWakeReason.VitalEmpty);
                return;
            }
            // 4.
            if (Time.deltaTime <= 0f) return;
            enemyTimer += Time.deltaTime;
            if (enemyTimer < b.sleep.enemyCheckSeconds) return;
            enemyTimer = 0f;
            var player = IslandPlayerController.Instance;
            if (player != null && HasActiveEnemyWithin(player.transform.position, b.sleep.enemyWakeRadius)) EndSleep(SleepWakeReason.EnemyNear);
        }

        // ---------------------------------------------------------------- Pemicu bangun

        private void HandleDaybreak(DaybreakEvent e) { if (IsSleeping) EndSleep(SleepWakeReason.Daybreak); }

        private void HandlePlayerDamaged(PlayerDamagedEvent e)
        {
            if (!IsSleeping) return;
            attackedPending = true;
            EndSleep(SleepWakeReason.Attacked);
        }

        private void HandleStructureDamaged(StructureDamagedEvent e)
        {
            if (!IsSleeping || Balance == null) return;
            var player = IslandPlayerController.Instance;
            if (player == null) return;
            float r = Balance.sleep.structureAlarmRadius;
            if ((e.position - player.transform.position).sqrMagnitude <= r * r) EndSleep(SleepWakeReason.EnemyNear);
        }

        private void HandleStructureDestroyed(StructureDestroyedEvent e)
        {
            if (!IsSleeping || currentBed == null) return;
            if (string.Equals(e.structureId, currentBed.StructureId, StringComparison.Ordinal)) EndSleep(SleepWakeReason.BedDestroyed);
        }

        private void HandleDisasterStarted(DisasterStartedEvent e) { if (IsSleeping) EndSleep(SleepWakeReason.Disaster); }

        private void HandleTide(TideChangedEvent e)
        {
            if (!IsSleeping || !onBeach || !e.isHigh) return;
            // Hukuman pantai langkah 3: tersapu air pasang.
            if (SurvivalStats.Instance != null && Balance != null)
                SurvivalStats.Instance.SpendStamina(Balance.sleep.beachTideStaminaLoss, StaminaUse.Swim);
            EndSleep(SleepWakeReason.TideRising);
        }

        private void HandleWarmth(WarmthChangedEvent e)
        {
            if (!IsSleeping || !e.isFreezing) return;
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.sleep_freezing", NotificationKind.Danger);   // "Kedinginan, kamu terbangun"
            EndSleep(SleepWakeReason.Freezing);
        }

        private void HandleRunEnded(RunEndedEvent e) { if (IsSleeping) EndSleep(SleepWakeReason.RunEnded); }

        /// <summary>Dipanggil SleepPanel.OnClosed. Esc = Manual; ditutup sistem lain = RunEnded (atau Attacked kalau baru diserang).</summary>
        internal void HandlePanelClosed(UICloseReason reason)
        {
            if (!IsSleeping) return;
            if (reason == UICloseReason.User) EndSleep(SleepWakeReason.Manual);
            else EndSleep(attackedPending ? SleepWakeReason.Attacked : SleepWakeReason.RunEnded);
        }

        // ---------------------------------------------------------------- Bangun

        /// <summary>Satu-satunya jalur bangun.</summary>
        private void EndSleep(SleepWakeReason reason)
        {
            // 1.
            if (!IsSleeping) return;
            // 2.
            IsSleeping = false;
            // 3.
            if (DayNightDirector.Instance != null) DayNightDirector.Instance.ClearTimeMultiplierOverride();
            if (SurvivalStats.Instance != null) SurvivalStats.Instance.SetDrainMultiplier(1f);
            // 4.
            var interactor = Interactor();
            if (interactor != null) interactor.SetEnabled(true);
            if (EquipmentManager.Instance != null) EquipmentManager.Instance.PopLock(EquipLock.Menu);
            // 5. Close memanggil OnClosed, tetapi langkah 1 mencegah proses berulang.
            var stack = UIFocusStack.Instance;
            if (stack != null && sleepPanel != null && stack.Contains(sleepPanel)) stack.Close(sleepPanel, UICloseReason.Forced);
            // 6.
            if (reason != SleepWakeReason.RunEnded) ApplyRest(reason);
            // 7.
            if (reason != SleepWakeReason.Daybreak && reason != SleepWakeReason.Manual) lastWokenTime = Time.time;
            currentBed = null;
            interruptAtGameHour = -1f;
            attackedPending = false;
            // 8.
            EventChannel<SleepStateChangedEvent>.Publish(new SleepStateChangedEvent(false, reason));
            // 9. Pemeriksaan pertempuran tetap dilakukan WorldStateSave (#80).
            if (reason == SleepWakeReason.Daybreak || reason == SleepWakeReason.Manual)
                EventChannel<SaveRequestedEvent>.Publish(new SaveRequestedEvent(SaveReason.WakeUp));
        }

        // ---------------------------------------------------------------- Pemulihan

        private void ApplyRest(SleepWakeReason reason)
        {
            var b = Balance;
            var stats = SurvivalStats.Instance;
            if (b == null || stats == null) return;
            var s = b.sleep;
            float hours = NowHours - startGameHour;
            // Hukuman pantai langkah 4: tanpa pemulihan.
            if (onBeach) return;
            // Usulan 5: HP hanya kalau lapar dan haus tidak nol, dan infeksi di bawah regenMaxInfection. Infeksi tidak berkurang
            // karena tidur (GDD bagian 3: obatnya tetap Vaksin).
            if (stats.Hunger > 0f && stats.Thirst > 0f && stats.Infection < b.vitals.regenMaxInfection)
            {
                float hp = hours * (inBase ? s.hpPerGameHourInBase : s.hpPerGameHourOutside);
                if (hp > 0f) stats.RestoreHP(hp);
            }
            bool forced = reason == SleepWakeReason.Attacked || reason == SleepWakeReason.Interrupted
                       || reason == SleepWakeReason.TideRising || reason == SleepWakeReason.Freezing;
            if (hours >= s.minHoursForStamina && !forced) stats.RestoreStamina(stats.MaxStamina);
        }

        // ---------------------------------------------------------------- Gangguan malam

        private void RollInterrupt()
        {
            var s = Balance.sleep;
            float chance = onBeach ? s.interruptChanceBeach : inBase ? s.interruptChanceInBase : s.interruptChanceOutside;
            var seeds = RunSeedManager.Instance;
            float roll, progress;
            if (seeds != null)
            {
                ref var rng = ref seeds.GetStream(RandomStreamId.Gameplay);
                roll = rng.NextFloat01();
                if (roll >= chance) { interruptAtGameHour = -1f; return; }
                progress = rng.Range(s.interruptMinProgress, s.interruptMaxProgress);
            }
            else
            {
                roll = UnityEngine.Random.value;
                if (roll >= chance) { interruptAtGameHour = -1f; return; }
                progress = UnityEngine.Random.Range(s.interruptMinProgress, s.interruptMaxProgress);
            }
            interruptAtGameHour = startGameHour + HoursUntilDaybreak() * progress;
        }

        private static float HoursUntilDaybreak()
        {
            float h = DayNightDirector.Instance != null ? DayNightDirector.Instance.CurrentTimeOfDay : 0f;
            if (h >= 18f) return 24f - h + 6f;
            if (h < 6f) return 6f - h;
            return 0f;
        }

        private void TriggerInterrupt()
        {
            var s = Balance.sleep;
            int day = GameManager.Instance != null ? GameManager.Instance.currentDay : 1;
            // 1. Keputusan developer (AIDirector.md): pengali jumlah setelah hari ke-60.
            float mul = AIDirector.Instance != null ? AIDirector.Instance.GetPostTargetCountMultiplier(day) : 1f;
            int count = Mathf.RoundToInt((s.ambushBaseCount + day / 10 * s.ambushCountPerTenDays) * mul);
            // 2.
            var player = IslandPlayerController.Instance;
            if (SpawnManager.Instance != null && player != null)
                SpawnManager.Instance.RequestAmbush(player.transform.position, count, inBase ? s.ambushRadiusInBase : s.ambushRadiusExposed);
            // 3.
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.sleep_interrupted", NotificationKind.Danger);   // "Ada suara di luar"
            EndSleep(SleepWakeReason.Interrupted);
        }

        /// <summary>DevConsole (#56): paksa gangguan sekarang (forceNow) atau matikan gangguan malam ini.</summary>
        public void DebugSetInterrupt(bool forceNow)
        {
            if (!IsSleeping) return;
            interruptAtGameHour = forceNow ? NowHours : -1f;
        }
    }
}
