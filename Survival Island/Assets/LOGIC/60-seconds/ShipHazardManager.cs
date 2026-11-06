using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>
    /// Bahaya nyata di kapal selama Fase 1: air naik, koridor runtuh, zombie, dan kemiringan.
    /// Blueprint: 60-seconds/ShipHazardManager.md (#75).
    /// GDD bagian 3: kapal miring, dipenuhi air dan zombie. Jadwal, jumlah zombie, dan cara menghadapinya
    /// adalah usulan yang disetujui developer sebagai nilai awal (#85).
    ///
    /// Belum diimplementasikan (sistemnya milik Langkah lain, ditandai TODO):
    /// - Perilaku zombie: EnemyAI.SetTarget, AIState.Dormant, bangun dalam zombieAggroRadius, kembali Dormant saat macet,
    ///   gigitan Zombie_Ship_Bite, dan EnemyHealth.Died (Langkah 5). Sekarang kelas ini memunculkan zombie dari pool,
    ///   mengatur agent.speed dengan pengali air, dan mengembalikan zombie yang terendam.
    /// - Suara alarm dan kapal (AudioManager), CameraShake.Play (#24), efek basah layar (#23).
    /// - Alat Editor ShipRouteAudit.
    /// </summary>
    public class ShipHazardManager : MonoBehaviour
    {
        private const string ZombiePoolTag = "zombie_standar";   // Tag pool musuh = enemyId (ObjectPooler.md)
        private static readonly ShipHazardBalance FallbackHazard = new ShipHazardBalance();

        public static ShipHazardManager Instance { get; private set; }

        [SerializeField] private ShipHazardSchedule schedule;
        [SerializeField] private ShipCorridor[] corridors;
        [SerializeField] private ShipZombiePoint[] zombiePoints;
        [Tooltip("Bidang air. Posisi Y mengikuti WaterLevel.")]
        [SerializeField] private Transform waterVisual;
        [Tooltip("Penutup tangga dek bawah. Nonaktif di awal.")]
        [SerializeField] private Collider lowerDeckBarrier;
        [Tooltip("Tinggi lantai dek bawah (Y). Susunan kapal usulan: 0.")]
        [SerializeField] private float lowerDeckFloorY = 0f;
        [Tooltip("Arah turun lantai kapal (dunia, horizontal). Mengikuti kemiringan tetap geometri.")]
        [SerializeField] private Vector3 downhillDirection = Vector3.right;
        [SerializeField] private ShipSwaySource swaySource;

        [Header("Pemain")]
        [SerializeField] private ShipPlayerController playerController;
        [SerializeField] private SurvivalStats stats;

        public bool IsRunning { get; private set; }
        public float WaterLevel { get; private set; }

        /// <summary>Penanda uji: FinalShudder sudah dijalankan (skenario tes 1 dan 16 di ShipTimerManager.md).</summary>
        public bool FinalShudderPlayed { get; private set; }

        private readonly List<GameObject> activeZombies = new List<GameObject>(8);
        private int nextEntryIndex;
        private int swayStage;
        private float zombieTickTimer;
        private bool lastDrowning;
        private bool warnedMissingPool;

        private static ShipHazardBalance Hazard
        {
            get
            {
                var b = GameManager.CurrentBalance;
                return b != null ? b.shipHazard : FallbackHazard;
            }
        }

        private Transform Player => playerController != null ? playerController.transform : null;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("[ShipHazardManager] Sudah ada satu di scene. Komponen ini dihapus.", this);
                Destroy(this);
                return;
            }
            Instance = this;

            if (playerController == null) playerController = FindAnyObjectByType<ShipPlayerController>();
            if (stats == null && playerController != null) stats = playerController.GetComponent<SurvivalStats>();
            if (schedule == null) Debug.LogError("[ShipHazardManager] Jadwal bahaya kosong.", this);

            WaterLevel = FirstWaterLevel();
            MoveWaterVisual();
            if (lowerDeckBarrier != null) lowerDeckBarrier.enabled = false;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable() { EventChannel<RunEndedEvent>.Raised += HandleRunEnded; }
        private void OnDisable() { EventChannel<RunEndedEvent>.Raised -= HandleRunEnded; }

        private void HandleRunEnded(RunEndedEvent e) { StopAll(); }

        // ---------------------------------------------------------------- Mulai dan berhenti

        /// <summary>Dipanggil LifeboatExtraction.PreGameRoutine langkah 7.</summary>
        public void Begin()
        {
            if (IsRunning) return;
            IsRunning = true;
            nextEntryIndex = 0;
            FinalShudderPlayed = false;
            WaterLevel = FirstWaterLevel();
            zombieTickTimer = 0f;
        }

        /// <summary>Dipanggil LifeboatExtraction (hasil) dan saat RunEndedEvent. Air berhenti, zombie berhenti.</summary>
        public void StopAll()
        {
            if (!IsRunning) return;
            IsRunning = false;
            if (playerController != null) playerController.SetShipMoveModifiers(1f, 1f, Vector3.zero);
            SetDrowning(false);
            // TODO Langkah 5: hentikan serangan zombie (EnemyAI ke state diam). Agen dihentikan sementara di sini.
            for (int i = 0; i < activeZombies.Count; i++)
            {
                var z = activeZombies[i];
                if (z == null) continue;
                var agent = z.GetComponent<NavMeshAgent>();
                if (agent != null && agent.isOnNavMesh) agent.isStopped = true;
            }
        }

        // ---------------------------------------------------------------- Per frame

        private void Update()
        {
            if (!IsRunning || schedule == null) return;
            var timer = ShipTimerManager.Instance;
            if (timer == null) return;

            float t = timer.SecondsLeft;
            RunEntriesUpTo(t);
            UpdateWater(t);
            UpdatePlayerEffects();
            TickZombies();
        }

        /// <summary>
        /// Dipanggil ShipTimerManager.ExecuteTimeUp sebelum LifeboatExtraction.HandleTimeUp. Menjalankan sisa jadwal
        /// dengan t = 0 (termasuk FinalShudder), karena kedua jalur hasil memanggil StopAll pada frame yang sama.
        /// </summary>
        public void FlushScheduleAtTimeUp()
        {
            if (!IsRunning || schedule == null) return;
            RunEntriesUpTo(0f);
            UpdateWater(0f);
        }

        private void RunEntriesUpTo(float t)
        {
            var entries = schedule.entries;
            if (entries == null) return;
            // while: tidak ada entri terlewat saat frame panjang, dan tiap entri hanya sekali.
            while (nextEntryIndex < entries.Length && t <= entries[nextEntryIndex].atSecondsLeft)
                Fire(entries[nextEntryIndex++], t);
        }

        private void Fire(in ShipHazardEntry entry, float t)
        {
            switch (entry.kind)
            {
                case ShipHazardKind.AlarmUp:
                    // README 60-seconds: T=45 alarm kapal semakin keras.
                    // TODO (AudioManager belum ditulis): naikkan volume alarm kapal.
                    break;
                case ShipHazardKind.CameraShake:
                    // README 60-seconds: T=30 kamera mulai bergetar.
                    // TODO #24 (CameraShake belum ditulis): CameraShake.Play(CameraShakeKind.<entry.targetId>).
                    break;
                case ShipHazardKind.SpawnZombies:
                    SpawnZombies(entry.targetId, entry.value);
                    break;
                case ShipHazardKind.CollapseWarning:
                    { var c = FindCorridor(entry.targetId); if (c != null) c.Warn(); }
                    break;
                case ShipHazardKind.Collapse:
                    CollapseCorridor(entry.targetId);
                    break;
                case ShipHazardKind.SetSwayStage:
                    swayStage = entry.value;
                    if (swaySource != null) swaySource.SetStage(entry.value);
                    // Tahap 0 adalah keadaan awal, jadi event hanya untuk tahap 1 ke atas (T=45, 30, 15).
                    if (entry.value > 0) EventChannel<ShipHazardStageChangedEvent>.Publish(new ShipHazardStageChangedEvent(entry.value, t));
                    break;
                case ShipHazardKind.FinalShudder:
                    FinalShudderPlayed = true;
                    // TODO #24 (CameraShake belum ditulis): CameraShake.Play(CameraShakeKind.ShipFinalShudder), dan suara air.
                    break;
            }
        }

        // ---------------------------------------------------------------- Air

        private float FirstWaterLevel()
        {
            if (schedule == null || schedule.waterKeyframes == null || schedule.waterKeyframes.Length == 0) return 0f;
            return schedule.waterKeyframes[0].level;
        }

        private void UpdateWater(float t)
        {
            var hz = Hazard;
            float target = schedule.EvaluateWater(t);
            // Air naik bertahap, waterRiseMaxPerSec hanya pengaman.
            WaterLevel = Mathf.MoveTowards(WaterLevel, target, hz.waterRiseMaxPerSec * Time.deltaTime);
            MoveWaterVisual();

            // Penghalang berbasis tinggi air, aktif setelah pemain tidak lagi berada di dek bawah.
            if (lowerDeckBarrier != null && !lowerDeckBarrier.enabled && WaterLevel - lowerDeckFloorY >= hz.waterHeadDepth)
            {
                var p = Player;
                if (p == null || p.position.y > lowerDeckBarrier.bounds.max.y) lowerDeckBarrier.enabled = true;
            }
        }

        private void MoveWaterVisual()
        {
            if (waterVisual == null) return;
            var pos = waterVisual.position;
            pos.y = WaterLevel;
            waterVisual.position = pos;
        }

        /// <summary>Pengali kecepatan dari kedalaman air di atas kaki.</summary>
        private static float WaterMultiplier(float depth, ShipHazardBalance hz)
        {
            if (depth < hz.waterAnkleDepth) return 1f;
            if (depth < hz.waterKneeDepth) return hz.waterAnkleMultiplier;
            if (depth < hz.waterWaistDepth) return hz.waterKneeMultiplier;
            return hz.waterWaistMultiplier;
        }

        private void UpdatePlayerEffects()
        {
            var p = Player;
            if (p == null) return;
            var hz = Hazard;
            float depth = WaterLevel - p.position.y;
            playerController.SetShipMoveModifiers(WaterMultiplier(depth, hz), ComputeTiltMul(hz), ComputeDrift(hz));
            // HP turun lewat SurvivalStats (vitals.drownDamagePerSec, #111) saat kepala terendam.
            SetDrowning(depth >= hz.waterHeadDepth);
        }

        private void SetDrowning(bool value)
        {
            if (value == lastDrowning) return;
            lastDrowning = value;
            if (stats != null) stats.SetDrowning(value);
        }

        private Vector3 Downhill
        {
            get
            {
                var d = new Vector3(downhillDirection.x, 0f, downhillDirection.z);
                return d.sqrMagnitude > 0.0001f ? d.normalized : Vector3.zero;
            }
        }

        /// <summary>Menanjak lebih lambat, menurun lebih cepat: 1 + influence * Dot(moveDir, downhill).</summary>
        private float ComputeTiltMul(ShipHazardBalance hz)
        {
            Vector3 move = playerController.MoveDirectionWorld;   // Dari frame sebelumnya
            return 1f + hz.tiltSpeedInfluence * Vector3.Dot(move, Downhill);
        }

        /// <summary>Geser ke sisi rendah hanya pada tahap sway terakhir (tahap 3, T=15).</summary>
        private Vector3 ComputeDrift(ShipHazardBalance hz)
        {
            return swayStage >= 3 ? Downhill * hz.driftSpeedMax : Vector3.zero;
        }

        // ---------------------------------------------------------------- Koridor

        private ShipCorridor FindCorridor(string id)
        {
            if (corridors == null || string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < corridors.Length; i++)
                if (corridors[i] != null && corridors[i].CorridorId == id) return corridors[i];
            Debug.LogWarning($"[ShipHazardManager] Koridor '{id}' tidak ada di scene.", this);
            return null;
        }

        private void CollapseCorridor(string id)
        {
            var corridor = FindCorridor(id);
            if (corridor == null) return;
            corridor.Collapse(playerController != null ? playerController.Controller : null);

            // Zombie di dalam koridor langsung kembali ke pool.
            var volume = corridor.BlockVolume;
            for (int i = activeZombies.Count - 1; i >= 0; i--)
            {
                var z = activeZombies[i];
                if (z != null && z.activeInHierarchy && volume.Contains(z.transform.position)) ReturnZombie(i);
            }
        }

        // ---------------------------------------------------------------- Zombie

        private void SpawnZombies(string groupId, int count)
        {
            var pooler = ObjectPooler.Instance;
            if (pooler == null || zombiePoints == null || count <= 0) return;
            if (!pooler.HasPool(ZombiePoolTag))
            {
                if (!warnedMissingPool) Debug.LogWarning($"[ShipHazardManager] Pool '{ZombiePoolTag}' belum ada. Zombie kapal dilewati.", this);
                warnedMissingPool = true;
                return;
            }

            var hz = Hazard;
            float minDistSqr = hz.zombieMinSpawnDistance * hz.zombieMinSpawnDistance;
            var p = Player;
            var lifeboat = LifeboatExtraction.Instance != null ? LifeboatExtraction.Instance.SafePoint : null;
            int spawned = 0;

            for (int i = 0; i < zombiePoints.Length && spawned < count; i++)
            {
                var point = zombiePoints[i];
                if (point == null || point.GroupId != groupId) continue;
                if (activeZombies.Count >= hz.zombieMaxAlive) break;
                Vector3 pos = point.transform.position;
                if (p != null && (pos - p.position).sqrMagnitude < minDistSqr) continue;
                if (lifeboat != null && (pos - lifeboat.position).sqrMagnitude < minDistSqr) continue;

                // Pool kosong: aturan pool terbatas (#62), grup dilewati tanpa error.
                var zombie = pooler.SpawnFromPool(ZombiePoolTag, pos, point.transform.rotation);
                if (zombie == null) break;

                var agent = zombie.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    // SpawnFromPool lalu SamplePosition, Warp, dan cek isOnNavMesh (#88).
                    if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 1f, NavMesh.AllAreas)) agent.Warp(hit.position);
                    if (!agent.isOnNavMesh)
                    {
                        pooler.ReturnToPool(ZombiePoolTag, zombie);
                        continue;
                    }
                    agent.speed = hz.zombieMoveSpeed;
                }
                // TODO Langkah 5: EnemyAI.SetTarget(pemain) dan state Dormant/bangun. ThreatSystem tidak ada di ShipPhase (#109).
                activeZombies.Add(zombie);
                spawned++;
            }
        }

        /// <summary>Tiap zombieTickSeconds: kecepatan dikali pengali air, dan zombie terendam kembali ke pool.</summary>
        private void TickZombies()
        {
            zombieTickTimer -= Time.deltaTime;
            if (zombieTickTimer > 0f) return;
            var hz = Hazard;
            zombieTickTimer = hz.zombieTickSeconds;

            for (int i = activeZombies.Count - 1; i >= 0; i--)
            {
                var z = activeZombies[i];
                // Sudah kembali ke pool lewat jalur lain (mati, dll.): hapus dari daftar.
                if (z == null || !z.activeInHierarchy) { activeZombies.RemoveAt(i); continue; }

                float depth = WaterLevel - z.transform.position.y;
                if (depth >= hz.waterHeadDepth) { ReturnZombie(i); continue; }   // Dek bawah tidak penuh mayat

                var agent = z.GetComponent<NavMeshAgent>();
                if (agent != null) agent.speed = hz.zombieMoveSpeed * WaterMultiplier(depth, hz);
            }
        }

        private void ReturnZombie(int index)
        {
            var z = activeZombies[index];
            activeZombies.RemoveAt(index);
            if (z != null && ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(ZombiePoolTag, z);
        }
    }
}
