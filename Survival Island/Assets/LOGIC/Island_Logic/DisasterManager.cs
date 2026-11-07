using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Tahap bencana (DisasterManager.md bagian 1).</summary>
    public enum DisasterPhase { Idle, Warning, Active, Aftermath }

    /// <summary>
    /// Menjalankan bencana terjadwal (Erupsi, Gempa Bumi, Badai Tropis) lewat tahap Warning, Active, dan Aftermath.
    /// Blueprint: Island_Logic/DisasterManager.md (#70). GDD bagian 5 (Erupsi di Region 1), bagian 6 (bencana acak,
    /// gempa merusak dinding yang belum di-upgrade), dan bagian 7 (mesin gratis saat bencana besar).
    ///
    /// Manager di scene IslandPhase, ISaveable world_disasters (LoadOrder 130). Hanya satu bencana aktif.
    /// Jadwal dibaca dari RunSeedManager.CurrentLayout.disasterSchedule; tidak ada RNG gameplay di sini selain titik
    /// Fireball, struktur badai, dan pohon tumbang yang memakai aliran tampilan (UnityEngine.Random).
    /// </summary>
    public class DisasterManager : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "world_disasters";

        public static DisasterManager Instance { get; private set; }

        [Tooltip("Aset DisasterData yang sama dengan RunSeedManager.disasterData. Dipakai untuk durasi tahap.")]
        [SerializeField] private DisasterData[] disasterData = Array.Empty<DisasterData>();

        // ---------------------------------------------------------------- Status (API di DisasterManager.md bagian 2)
        public DisasterPhase Phase => phase;
        public bool IsActive => phase != DisasterPhase.Idle;
        public DisasterType ActiveType => current.type;
        public int ActiveScale => current.scale;

        private DisasterPhase phase = DisasterPhase.Idle;
        private DisasterScheduleEntry current;
        private bool currentFromSchedule;
        private int nextIndex;
        private float phaseTimeLeft;
        private float activeDuration;
        private float activeElapsed;
        private int pulsesDone;

        private float fireballTimer;
        private float stormTimer;
        private float treeTimer;
        private ResourceNode pendingTree;
        private float pendingTreeTimer;

        private RegionType playerRegion = RegionType.Volcano;   // GDD bagian 5: Region 1 zona awal
        private bool spawnOverrideActive;
        private SpawnOverrideHandle spawnOverride;
        private WeatherOverrideHandle weatherOverride;   // Badai Tropis (#115)

        private readonly Queue<BaseHealth> quakeQueue = new Queue<BaseHealth>(64);
        private float quakeRatioPerPulse;
        private readonly List<BaseHealth> structureBuffer = new List<BaseHealth>(64);

        private GameBalanceConfig Balance => GameManager.CurrentBalance;
        private DisasterBalance Bal => Balance != null ? Balance.disaster : null;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[DisasterManager] Instance ganda di scene IslandPhase.");
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
            EventChannel<HourChangedEvent>.Raised += HandleHour;
            EventChannel<DayChangedEvent>.Raised += HandleDayChanged;
            EventChannel<RegionChangedEvent>.Raised += HandleRegionChanged;
            EventChannel<BloodMoonStartedEvent>.Raised += HandleBloodMoonStarted;
            if (EventChannel<RegionChangedEvent>.HasLast) playerRegion = EventChannel<RegionChangedEvent>.Last.to;
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Register(this);
        }

        private void OnDisable()
        {
            EventChannel<HourChangedEvent>.Raised -= HandleHour;
            EventChannel<DayChangedEvent>.Raised -= HandleDayChanged;
            EventChannel<RegionChangedEvent>.Raised -= HandleRegionChanged;
            EventChannel<BloodMoonStartedEvent>.Raised -= HandleBloodMoonStarted;
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            ReleaseOverrides();
        }

        // ---------------------------------------------------------------- Jadwal

        private static int CurrentDay => GameManager.Instance != null ? GameManager.Instance.currentDay : 1;

        private void HandleDayChanged(DayChangedEvent e)
        {
            // Jadwal berlanjut tanpa batas, termasuk setelah hari ke-60 (keputusan 6).
            if (RunSeedManager.Instance != null) RunSeedManager.Instance.EnsureDisasterScheduleThrough(e.newDay);
        }

        private void HandleHour(HourChangedEvent e)
        {
            if (phase != DisasterPhase.Idle) return;
            var seed = RunSeedManager.Instance;
            if (seed == null || seed.CurrentLayout == null) return;
            var schedule = seed.CurrentLayout.disasterSchedule;
            if (schedule == null) return;

            int day = CurrentDay;
            float hourNow = DayNightDirector.Instance != null ? DayNightDirector.Instance.CurrentTimeOfDay : e.hour;

            // Entri yang harinya sudah lewat (mis. setday) tidak dijalankan.
            while (nextIndex < schedule.Length && schedule[nextIndex].day < day) nextIndex++;
            if (nextIndex >= schedule.Length) return;

            var entry = schedule[nextIndex];
            if (entry.day == day && hourNow >= entry.startHour) Begin(entry, true);
        }

        /// <summary>Perintah DevConsole "disaster &lt;tipe&gt; [skala]" (#56). Ditolak kalau ada bencana aktif.</summary>
        public bool ForceStart(DisasterType type, int scale)
        {
            if (phase != DisasterPhase.Idle) return false;
            Begin(new DisasterScheduleEntry { day = CurrentDay, type = type, scale = Mathf.Clamp(scale, 1, 2), startHour = 0f }, false);
            return true;
        }

        private DisasterData GetData(DisasterType type)
        {
            if (disasterData == null) return null;
            for (int i = 0; i < disasterData.Length; i++)
                if (disasterData[i] != null && disasterData[i].type == type) return disasterData[i];
            return null;
        }

        // ---------------------------------------------------------------- Tahap

        private void Begin(DisasterScheduleEntry entry, bool fromSchedule)
        {
            var data = GetData(entry.type);
            if (data == null)
            {
                Debug.LogError($"[DisasterManager] DisasterData untuk {entry.type} belum diisi. Bencana dilewati.");
                if (fromSchedule) nextIndex++;
                return;
            }

            current = entry;
            currentFromSchedule = fromSchedule;
            pulsesDone = 0;
            activeElapsed = 0f;
            float minMajor = Balance != null ? Balance.luckyMachine.freeWindowSeconds : 30f;
            activeDuration = data.GetActiveSeconds(entry.scale, minMajor);
            EnterWarning(data);
        }

        private void EnterWarning(DisasterData data)
        {
            phase = DisasterPhase.Warning;
            phaseTimeLeft = data.warningSeconds;
            // Dikirim sejak Warning, supaya SleepSystem membangunkan pemain sebelum efek dimulai.
            EventChannel<DisasterStartedEvent>.Publish(new DisasterStartedEvent(current.type, current.scale));
            // Notifikasi (#22) dan subtitle peringatan (#104) dengan kunci yang sama, lalu warningClip (#114).
            string key = WarningKey(current.type);
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show(key, NotificationKind.Danger);
            EventChannel<SubtitleRequestedEvent>.Publish(new SubtitleRequestedEvent(key, null, 2, SubtitleSourceKind.Warning));
            if (AudioManager.Instance != null && data.warningClip != null) AudioManager.Instance.PlayWarning(data.warningClip);
            // Erupsi memulai langit merah (#92).
            ApplyEruptionSky();
        }

        /// <summary>Kunci teks peringatan per bencana (tabel "Notifikasi peringatan" di DisasterManager.md).</summary>
        private static string WarningKey(DisasterType type)
        {
            switch (type)
            {
                case DisasterType.Eruption: return "notif.disaster_eruption";            // "Gunung berapi bergemuruh. Cari perlindungan"
                case DisasterType.Earthquake: return "notif.disaster_earthquake";        // "Tanah bergetar"
                case DisasterType.TropicalStorm: return "notif.disaster_storm";          // "Angin menguat. Badai mendekat"
                default: return string.Empty;
            }
        }

        /// <summary>Lapisan langit merah erupsi (#92): bobot 1 di Region 1, environment.eruptionRemoteWeight di region lain.</summary>
        private void ApplyEruptionSky()
        {
            var env = EnvironmentDirector.Instance;
            if (env == null || phase == DisasterPhase.Idle || current.type != DisasterType.Eruption) return;
            float remote = Balance != null ? Balance.environment.eruptionRemoteWeight : 0.35f;
            env.PushEventLayer(EnvEventId.Eruption, playerRegion == RegionType.Volcano ? 1f : remote);
        }

        private void EnterActive(DisasterData data, bool isRestore)
        {
            phase = DisasterPhase.Active;
            if (!isRestore)
            {
                phaseTimeLeft = activeDuration;
                activeElapsed = 0f;
            }
            fireballTimer = 0f;
            stormTimer = 0f;
            treeTimer = Bal != null ? Bal.treeFallSeconds : 45f;

            ApplyOverrides();

            // GDD bagian 7: bencana besar membuka mode gratis Lucky Machine. Tidak dikirim ulang setelah load (bagian 4).
            if (!isRestore && data.IsMajor(current.scale))
                EventChannel<MajorDisasterStartedEvent>.Publish(new MajorDisasterStartedEvent(current.type, activeDuration));

            if (!isRestore && current.type == DisasterType.Eruption && Bal != null)
            {
                // Pemain di luar Region 1: base di sana dihantam lewat RemoteBaseSimulation (#90).
                if (playerRegion != RegionType.Volcano && RemoteBaseSimulation.Instance != null)
                    RemoteBaseSimulation.Instance.QueueDisasterDamage(RegionType.Volcano, DamageType.Explosion, Bal.fireballStructureDamage,
                                                                     Bal.eruptionOffscreenHits * current.scale, false);
            }
        }

        private void EnterAftermath(DisasterData data)
        {
            phase = DisasterPhase.Aftermath;
            phaseTimeLeft = data != null ? data.aftermathSeconds : 0f;
            ReleaseOverrides();
            quakeQueue.Clear();
            pendingTree = null;
            // Notifikasi "Periksa kerusakan base" (#22). Langit kembali normal lewat ReleaseOverrides (#92).
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.disaster_aftermath", NotificationKind.Warning);
        }

        private void FinishDisaster()
        {
            var type = current.type;
            phase = DisasterPhase.Idle;
            if (currentFromSchedule) nextIndex++;
            currentFromSchedule = false;
            EventChannel<DisasterEndedEvent>.Publish(new DisasterEndedEvent(type));
            EventChannel<SaveRequestedEvent>.Publish(new SaveRequestedEvent(SaveReason.DisasterEnded));
        }

        /// <summary>Akhiri bencana tanpa Aftermath (Blood Moon menang, DisasterManager.md "Prioritas").</summary>
        public void EndNow()
        {
            if (phase == DisasterPhase.Idle) return;
            ReleaseOverrides();
            quakeQueue.Clear();
            pendingTree = null;
            FinishDisaster();
        }

        private void HandleBloodMoonStarted(BloodMoonStartedEvent e)
        {
            EndNow();
        }

        private void HandleRegionChanged(RegionChangedEvent e)
        {
            // Bobot langit dan Fireball ikut berubah di tengah bencana (tabel "Cakupan").
            playerRegion = e.to;
            // Bobot lapisan Eruption ikut region pemain (#92).
            if (phase == DisasterPhase.Warning || phase == DisasterPhase.Active) ApplyEruptionSky();
        }

        private void ApplyOverrides()
        {
            if (current.type == DisasterType.Eruption && !spawnOverrideActive && SpawnManager.Instance != null)
            {
                // Hewan buas lari dari erupsi.
                spawnOverride = SpawnManager.Instance.PushOverride(new SpawnOverride { suspendAnimals = true });
                spawnOverrideActive = true;
            }
            // Badai Tropis (#115): Storm di semua region, Blizzard di Region 3. Ini juga menaikkan air (#91) dan memadamkan api (#44).
            if (current.type == DisasterType.TropicalStorm && !weatherOverride.IsValid && RegionalWeather.Instance != null)
            {
                byte all = (byte)((1 << (int)RegionType.Volcano) | (1 << (int)RegionType.Amazon) | (1 << (int)RegionType.Snow) | (1 << (int)RegionType.Logistic));
                weatherOverride = RegionalWeather.Instance.PushOverride(new WeatherOverride(all, WeatherType.Storm, WeatherType.Blizzard));
            }
            // Langit merah erupsi, termasuk setelah load (#92).
            ApplyEruptionSky();
        }

        private void ReleaseOverrides()
        {
            if (spawnOverrideActive)
            {
                if (SpawnManager.Instance != null) SpawnManager.Instance.PopOverride(spawnOverride);
                spawnOverrideActive = false;
            }
            // Lepas override cuaca badai (#115) dan langit merah (#92).
            if (weatherOverride.IsValid)
            {
                if (RegionalWeather.Instance != null) RegionalWeather.Instance.PopOverride(weatherOverride);
                weatherOverride = default;
            }
            if (current.type == DisasterType.Eruption && EnvironmentDirector.Instance != null)
                EnvironmentDirector.Instance.PopEventLayer(EnvEventId.Eruption);
        }

        // ---------------------------------------------------------------- Per frame

        private void Update()
        {
            if (phase == DisasterPhase.Idle) return;
            // Semua timer memakai Time.deltaTime, jadi berhenti saat game dijeda (bagian 4).
            float dt = Time.deltaTime;
            var data = GetData(current.type);

            ProcessQuakeQueue();
            TickPendingTree(dt);

            phaseTimeLeft -= dt;
            switch (phase)
            {
                case DisasterPhase.Warning:
                    if (phaseTimeLeft <= 0f) EnterActive(data, false);
                    break;
                case DisasterPhase.Active:
                    activeElapsed += dt;
                    TickActive(dt);
                    if (phaseTimeLeft <= 0f) EnterAftermath(data);
                    break;
                case DisasterPhase.Aftermath:
                    if (phaseTimeLeft <= 0f) FinishDisaster();
                    break;
            }
        }

        private void TickActive(float dt)
        {
            var b = Bal;
            if (b == null) return;
            switch (current.type)
            {
                case DisasterType.Eruption:
                    // Fireball hanya di Region 1 (keputusan 1).
                    if (playerRegion != RegionType.Volcano) break;
                    fireballTimer -= dt;
                    if (fireballTimer <= 0f)
                    {
                        fireballTimer = current.scale >= 2 ? b.fireballIntervalSecondsScale2 : b.fireballIntervalSeconds;
                        SpawnFireball(b);
                    }
                    break;

                case DisasterType.Earthquake:
                    TickQuake(b);
                    break;

                case DisasterType.TropicalStorm:
                    stormTimer -= dt;
                    if (stormTimer <= 0f)
                    {
                        stormTimer = b.stormPulseSeconds;
                        StormPulse(b);
                    }
                    treeTimer -= dt;
                    if (treeTimer <= 0f)
                    {
                        treeTimer = b.treeFallSeconds;
                        // Tidak ada pohon tumbang di Region 3 (salju).
                        if (playerRegion != RegionType.Snow) StartTreeFall(b);
                    }
                    break;
            }
        }

        // ---------------------------------------------------------------- Erupsi

        private static Transform PlayerTransform => IslandPlayerController.Instance != null ? IslandPlayerController.Instance.transform : null;

        private void SpawnFireball(DisasterBalance b)
        {
            var pooler = ObjectPooler.Instance;
            var player = PlayerTransform;
            if (pooler == null || player == null || !pooler.HasPool(Fireball.PoolTag)) return;

            // 1. Titik acak di cincin. Aliran tampilan, karena titik ini tidak perlu sama antar-load.
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            float dist = UnityEngine.Random.Range(b.fireballRingMin, b.fireballRingMax);
            Vector3 flat = player.position + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * dist;

            // 2. Raycast ke bawah dengan PhysicsMasks.Ground (memuat Structure, jadi bisa jatuh di atap).
            Vector3 top = flat + Vector3.up * b.fireballDropHeight;
            if (!PhysicsQuery.Raycast(new Ray(top, Vector3.down), out var hit, b.fireballDropHeight * 2f, PhysicsMasks.Ground)) return;

            // 3. Penanda, lalu Fireball dari langit.
            HazardImpactMarker marker = null;
            if (pooler.HasPool(HazardImpactMarker.PoolTag))
            {
                marker = pooler.SpawnFromPool<HazardImpactMarker>(HazardImpactMarker.PoolTag, hit.point + hit.normal * 0.05f,
                    Quaternion.FromToRotation(Vector3.up, hit.normal));
                if (marker != null) marker.Setup(b.fireballRadius, 0f);
            }
            Vector3 start = hit.point + Vector3.up * b.fireballDropHeight
                          + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * (b.fireballDropHeight * 0.3f);
            var ball = pooler.SpawnFromPool<Fireball>(Fireball.PoolTag, start, Quaternion.identity);
            if (ball != null) ball.Launch(start, hit.point, b.fireballMarkerSeconds, marker);
            else if (marker != null) marker.ReturnNow();
        }

        // ---------------------------------------------------------------- Gempa

        private void TickQuake(DisasterBalance b)
        {
            int pulses = Mathf.Max(1, b.quakePulses);
            if (pulsesDone >= pulses || activeDuration <= 0f) return;
            // Denyut pada 20, 50, dan 80 persen durasi Active (untuk 3 denyut); dibagi rata untuk jumlah lain.
            float frac = pulses == 1 ? 0.5f : 0.2f + 0.6f * pulsesDone / (pulses - 1);
            if (activeElapsed < activeDuration * frac) return;
            QuakePulse(b, pulses);
            pulsesDone++;
        }

        private void QuakePulse(DisasterBalance b, int pulses)
        {
            // Getaran kamera gempa (#24), skala mengikuti skala bencana, dan suara gemuruh (#114) dari DisasterData.pulseClip.
            CameraShake.Play(CameraShakeKind.Earthquake, current.scale);
            var data = GetData(current.type);
            if (data != null && data.pulseClip != null) AudioManager.Instance?.PlaySFX(data.pulseClip);
            float ratio = current.scale >= 2 ? b.quakeDamageRatioScale2 : b.quakeDamageRatio;
            quakeRatioPerPulse = ratio / pulses;

            CollectStructures();
            for (int i = 0; i < structureBuffer.Count; i++)
                if (!SurvivesEarthquake(structureBuffer[i])) quakeQueue.Enqueue(structureBuffer[i]);
            structureBuffer.Clear();

            if (pulsesDone == 0)
            {
                // Region yang tidak dimuat menerima damage gempa lewat RemoteBaseSimulation (#90).
                var remote = RemoteBaseSimulation.Instance;
                if (remote != null && RegionStreaming.Instance != null)
                {
                    for (int r = 0; r <= (int)RegionType.Logistic; r++)
                    {
                        var region = (RegionType)r;
                        if (RegionStreaming.IsRegionLoaded(region)) continue;
                        remote.QueueDisasterDamage(region, DamageType.Structural, b.quakeRemoteDamagePerHit, b.quakeRemoteHits * current.scale, true);
                    }
                }
            }
        }

        private void ProcessQuakeQueue()
        {
            if (quakeQueue.Count == 0) return;
            int max = Bal != null ? Mathf.Max(1, Bal.maxStructuresPerFrame) : 16;
            for (int i = 0; i < max && quakeQueue.Count > 0; i++)
            {
                var s = quakeQueue.Dequeue();
                if (s == null || !s.IsAlive) continue;
                var info = new DamageInfo
                {
                    type = DamageType.Structural,
                    sourceId = "Earthquake",
                    amount = s.MaxHP * quakeRatioPerPulse,
                    hitPoint = s.transform.position,
                    hitDirection = Vector3.up,
                    bodyPart = BodyPart.Torso
                };
                s.TakeDamage(info);
            }
        }

        /// <summary>
        /// Penyimpangan sementara: BuildableData belum punya survivesEarthquake (#47, BuildingIntegrity belum ditulis).
        /// Sampai field itu ada, struktur dengan id berisi "Stone" dianggap tahan gempa (Wall_Stone, Door_Stone).
        /// </summary>
        private static bool SurvivesEarthquake(BaseHealth s)
        {
            return s != null && s.Data != null && !string.IsNullOrEmpty(s.Data.id)
                   && s.Data.id.IndexOf("Stone", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Penyimpangan sementara: StructureRegistry belum punya CopyLoadedStructures (#79). Dipakai FindObjectsByType,
        /// yang hanya dipanggil sekali per denyut gempa atau badai (bukan per frame).
        /// </summary>
        private void CollectStructures()
        {
            structureBuffer.Clear();
            var found = FindObjectsByType<BaseHealth>(FindObjectsSortMode.None);
            for (int i = 0; i < found.Length; i++)
                if (found[i] != null && found[i].IsAlive) structureBuffer.Add(found[i]);
        }

        // ---------------------------------------------------------------- Badai Tropis

        private void StormPulse(DisasterBalance b)
        {
            CollectStructures();
            // Hanya struktur kayu. Struktur Stone tidak terkena.
            for (int i = structureBuffer.Count - 1; i >= 0; i--)
                if (SurvivesEarthquake(structureBuffer[i])) structureBuffer.RemoveAt(i);

            int hits = Mathf.Max(1, current.scale);
            for (int h = 0; h < hits && structureBuffer.Count > 0; h++)
            {
                int pick = UnityEngine.Random.Range(0, structureBuffer.Count);
                var s = structureBuffer[pick];
                structureBuffer.RemoveAt(pick);
                var info = new DamageInfo
                {
                    type = DamageType.Structural,
                    sourceId = "TropicalStorm",
                    amount = b.stormStructureDamage,
                    hitPoint = s.transform.position,
                    hitDirection = Vector3.forward,
                    bodyPart = BodyPart.Torso
                };
                s.TakeDamage(info);
            }
            structureBuffer.Clear();
        }

        private void StartTreeFall(DisasterBalance b)
        {
            if (pendingTree != null) return;
            var player = PlayerTransform;
            if (player == null) return;
            var tree = Fireball.FindNearestTree(player.position, b.treeFallRingMax, b.treeFallRingMin);
            if (tree == null) return;

            pendingTree = tree;
            pendingTreeTimer = b.treeFallMarkerSeconds;

            var pooler = ObjectPooler.Instance;
            if (pooler != null && pooler.HasPool(HazardImpactMarker.PoolTag))
            {
                // Penanda di depan pohon, ke arah pemain.
                Vector3 toPlayer = player.position - tree.transform.position;
                toPlayer.y = 0f;
                Vector3 pos = tree.transform.position + (toPlayer.sqrMagnitude > 0.01f ? toPlayer.normalized : Vector3.forward) * b.treeFallRadius;
                var marker = pooler.SpawnFromPool<HazardImpactMarker>(HazardImpactMarker.PoolTag, pos, Quaternion.identity);
                if (marker != null) marker.Setup(b.treeFallRadius, b.treeFallMarkerSeconds);
            }
        }

        private void TickPendingTree(float dt)
        {
            if (pendingTree == null) return;
            pendingTreeTimer -= dt;
            if (pendingTreeTimer > 0f) return;

            var b = Bal;
            var tree = pendingTree;
            pendingTree = null;
            if (b == null || tree == null || tree.IsDepleted) return;

            // Kayu bisa dipungut, lalu makhluk dan bangunan di sekitar menerima damage.
            Vector3 pos = tree.transform.position;
            tree.ForceDeplete(true);
            Fireball.ApplyBlastDamage(pos, b.treeFallRadius, b.treeFallDamage, b.treeFallDamage, b.treeFallStructureDamage,
                                      "TreeFall", false);
            // Efek pohon tumbang lewat VFXManager (#15).
            if (VFXManager.Instance != null) VFXManager.Instance.Play("Fx_TreeFall", pos, Quaternion.identity);
        }

        // ---------------------------------------------------------------- ISaveable (bagian 4 "Save saat bencana")

        [Serializable]
        private class SaveData
        {
            public int nextIndex;
            public DisasterPhase phase;
            public float phaseTimeLeft;
            public int pulsesDone;
            public DisasterType type;
            public int scale;
            public bool fromSchedule;
            public float activeDuration;
            public float activeElapsed;
        }

        public string SaveId => SaveIdConst;
        public int LoadOrder => 130;

        public string CaptureState()
        {
            // Fireball yang sedang terbang tidak disimpan.
            return JsonUtility.ToJson(new SaveData
            {
                nextIndex = nextIndex,
                phase = phase,
                phaseTimeLeft = phaseTimeLeft,
                pulsesDone = pulsesDone,
                type = current.type,
                scale = current.scale,
                fromSchedule = currentFromSchedule,
                activeDuration = activeDuration,
                activeElapsed = activeElapsed
            });
        }

        public void RestoreState(string json)
        {
            var d = string.IsNullOrEmpty(json) ? null : JsonUtility.FromJson<SaveData>(json);
            if (d == null) return;
            nextIndex = Mathf.Max(0, d.nextIndex);
            phase = d.phase;
            phaseTimeLeft = d.phaseTimeLeft;
            pulsesDone = d.pulsesDone;
            current = new DisasterScheduleEntry { day = CurrentDay, type = d.type, scale = d.scale };
            currentFromSchedule = d.fromSchedule;
            activeDuration = d.activeDuration;
            activeElapsed = d.activeElapsed;
        }

        public void OnRestoreFinished()
        {
            if (RunSeedManager.Instance != null) RunSeedManager.Instance.EnsureDisasterScheduleThrough(CurrentDay);
            // Pasang lagi override cuaca, spawn, dan langit. Damage yang sudah terjadi tidak diulang karena pulsesDone.
            // MajorDisasterStartedEvent tidak dikirim ulang (LuckyMachine menyimpan jendelanya sendiri).
            if (phase == DisasterPhase.Active)
            {
                float left = phaseTimeLeft;
                EnterActive(GetData(current.type), true);
                phaseTimeLeft = left;
            }
        }
    }
}
