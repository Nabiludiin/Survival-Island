using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    public enum DespawnReason : byte { Killed, Burned, ReturnedToDen, Harvested, Stuck, OffNavMesh, TooFar, RegionUnloaded, Sacrificed, Debug }

    /// <summary>Override spawn dari HordeManager (#50) dan DisasterManager (#70).</summary>
    public struct SpawnOverride
    {
        public RegionSpawnTable table;   // null berarti tabel region
        public bool forceNightTable;
        public bool suspendNormalSpawn;
        public bool suspendAnimals;
        public int capOverride;          // 0 berarti batas biasa
        public int desiredOverride;      // 0 berarti ikut #27
    }

    public struct SpawnOverrideHandle { public int id; }

    /// <summary>
    /// Jenis dan tempat musuh per region, siang dan malam, lewat ObjectPooler. Blueprint: left-4-dead/SpawnManager.md (#62).
    /// GDD bagian 3: siang hewan di luar ruangan, malam zombie. Bagian 5: ancaman tiap region.
    /// Claude.md Tahap 6: semua musuh dari ObjectPooler, satu jalur kembali (Despawn).
    /// Jumlah zombie malam ditentukan AIDirector (#27) lewat RequestWave; hewan siang diisi TickSpawn.
    ///
    /// Catatan Langkah 5 (sistem pendukung; yang belum disambung ditandai TODO):
    /// - RegionStreaming (#88): syarat GetRegionAt/IsLoaded dipakai; RegionLoadStateChangedEvent belum didengar.
    /// - BaseNavigation.IsInsideBase (#78), InteriorZone (#112), dan LairRegistry (#112) sudah dipakai.
    /// - HidingSpot/VillageZombieHiding (#63) dan BushSpawnPoint (#25): Zombie Desa dan Lumut memakai FindSpawnPoint biasa.
    /// - Tidur dari SleepStateChangedEvent (#67). LairClearedEvent (#112) dan PopulateHidingSpots belum dipakai.
    /// - Malam dibaca dari HourChangedEvent (DayPhaseTracker), bukan NightfallEvent (Langkah 4).
    /// </summary>
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Instance { get; private set; }

        [Tooltip("Satu tabel per region, urut RegionType (Volcano, Amazon, Snow, Logistic).")]
        [SerializeField] private RegionSpawnTable[] tables = new RegionSpawnTable[4];

        private readonly List<EnemyInstance> activeEnemies = new List<EnemyInstance>(64);
        public IReadOnlyList<EnemyInstance> ActiveEnemies => activeEnemies;

        private RegionType activeRegion = RegionType.Volcano;
        private bool isNight;
        private bool isSleeping;
        private readonly List<KeyValuePair<int, SpawnOverride>> overrides = new List<KeyValuePair<int, SpawnOverride>>();
        private int nextOverrideId = 1;
        private float[] weightBuffer = new float[16];
        private readonly Collider[] crowdBuffer = new Collider[4];
        private readonly Plane[] frustum = new Plane[6];

        private struct Slot
        {
            public WaveModifiers modifiers;
            public int maxLarge;
            public float specialShare;
            public string forcedEnemyId;
            public Vector3? origin;
            public float radius;               // 0 = jarak spawn biasa
            public RegionSpawnTable table;     // null = tabel region aktif
            public bool usesBudget;
            public bool forceAware;
            public bool ignoreView;
            public int attempts;
        }
        private readonly List<Slot> queue = new List<Slot>(64);
        private int specialsThisWave;
        private int largeThisWave;
        private int failedPoints;

        private Coroutine spawnLoop, watchdogLoop, sweepRoutine;

        private static SpawnBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.spawn : null;

        // ---------------------------------------------------------------- Hitungan

        public int MaxAlive => Bal != null ? Bal.maxZombiesOverride + Bal.maxAnimalsAlive : 58;
        public int CurrentZombieCap
        {
            get
            {
                var b = Bal;
                if (b == null) return 30;
                int cap = b.maxZombiesAlive;
                for (int i = 0; i < overrides.Count; i++) if (overrides[i].Value.capOverride > cap) cap = overrides[i].Value.capOverride;
                return Mathf.Min(cap, b.maxZombiesOverride);
            }
        }

        public int AliveZombies
        {
            get
            {
                int n = 0;
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    var e = activeEnemies[i];
                    if (e != null && e.IsZombie && !e.IsDormantCounted && e.Health.IsAlive) n++;
                }
                return n;
            }
        }

        public int AliveLarge
        {
            get
            {
                int n = 0;
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    var e = activeEnemies[i];
                    if (e != null && e.IsZombie && !e.IsDormantCounted && e.Health.IsAlive && e.Data.isLarge) n++;
                }
                return n;
            }
        }

        public int EngagedEnemyCount
        {
            get
            {
                int n = 0;
                for (int i = 0; i < activeEnemies.Count; i++) if (activeEnemies[i] != null && activeEnemies[i].IsEngaged) n++;
                return n;
            }
        }

        /// <summary>Hewan hidup, tanpa bangkai dan mayat.</summary>
        private int CountAwakeAnimals()
        {
            int n = 0;
            for (int i = 0; i < activeEnemies.Count; i++)
            {
                var e = activeEnemies[i];
                if (e != null && e.IsAnimal && !e.IsCarcass && e.Health.IsAlive) n++;
            }
            return n;
        }

        private int CountAliveOf(string enemyId)
        {
            int n = 0;
            for (int i = 0; i < activeEnemies.Count; i++)
            {
                var e = activeEnemies[i];
                if (e != null && e.Data != null && e.Data.enemyId == enemyId && e.Health.IsAlive) n++;
            }
            return n;
        }

        // ---------------------------------------------------------------- Siklus

        private void Awake()
        {
            if (Instance != null && Instance != this) { Debug.LogError("[SpawnManager] Lebih dari satu instance."); enabled = false; return; }
            Instance = this;
            CorpseBudget.Clear();
#if UNITY_EDITOR
            ValidatePools();
#endif
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<HourChangedEvent>.Raised += HandleHour;
            EventChannel<RegionChangedEvent>.Raised += HandleRegion;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            EventChannel<SleepStateChangedEvent>.Raised += HandleSleep;   // #67
            // Event sticky: setelah load, mulai dari kosong dengan region dan waktu terakhir.
            if (EventChannel<RegionChangedEvent>.HasLast) activeRegion = EventChannel<RegionChangedEvent>.Last.to;
            isNight = DayPhaseTracker.IsNight;
            spawnLoop = StartCoroutine(SpawnLoop());
            watchdogLoop = StartCoroutine(WatchdogLoop());
        }

        private void OnDisable()
        {
            EventChannel<HourChangedEvent>.Raised -= HandleHour;
            EventChannel<RegionChangedEvent>.Raised -= HandleRegion;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            EventChannel<SleepStateChangedEvent>.Raised -= HandleSleep;
            StopAllCoroutines();
            spawnLoop = watchdogLoop = sweepRoutine = null;
        }

        private void HandleHour(HourChangedEvent e)
        {
            bool night = DayPhaseTracker.IsNightHour(e.hour);
            if (night == isNight) return;
            isNight = night;
            if (!night)
            {
                // Fajar: zombie di luar ruangan dibakar SunlightExposure (#63). Mayat zombie langsung memudar (#95).
                CorpseBudget.FadeAllZombies();
            }
            // Malam: hewan pergi sendiri lewat WildlifeAI (FleeToDen).
            ScheduleSweep();
        }

        private void HandleRegion(RegionChangedEvent e)
        {
            // Musuh baru memakai tabel region baru. Musuh lama tetap sampai TooFar (usulan disetujui developer).
            activeRegion = e.to;
        }

        private void HandleRunEnded(RunEndedEvent e)
        {
            StopAllCoroutines();
            spawnLoop = watchdogLoop = sweepRoutine = null;
            queue.Clear();
        }

        /// <summary>SleepSystem (#67): tick tidak memunculkan musuh baru selama tidur. RequestAmbush tetap jalan.</summary>
        public void SetSleeping(bool value) => isSleeping = value;
        private void HandleSleep(SleepStateChangedEvent e) => isSleeping = e.isSleeping;

        private void ScheduleSweep()
        {
            if (sweepRoutine != null) StopCoroutine(sweepRoutine);
            sweepRoutine = StartCoroutine(SweepAfterDelay());
        }

        private IEnumerator SweepAfterDelay()
        {
            var b = Bal;
            yield return new WaitForSeconds(b != null ? b.cleanupSeconds : 30f);
            sweepRoutine = null;
            SweepWrongPhase();
        }

        /// <summary>Kembalikan musuh salah waktu yang di luar pandangan (hewan saat malam, zombie saat siang).</summary>
        private void SweepWrongPhase()
        {
            UpdateFrustum();
            for (int i = activeEnemies.Count - 1; i >= 0; i--)
            {
                var e = activeEnemies[i];
                if (e == null || e.IsDormantCounted || e.IsCarcass || e.IsFromOverride || !e.Health.IsAlive) continue;
                // Zombie sarang di dalam InteriorZone tidak dikembalikan saat siang (#112).
                if (e.AI != null && LairRegistry.IsShelteredLairZombie(e.AI)) continue;
                if (e.AI != null && e.AI.IsBurning) continue;
                bool wrong = isNight ? e.IsAnimal : e.IsZombie;
                if (wrong && !IsVisible(e.transform.position)) Despawn(e.gameObject, DespawnReason.Debug);
            }
        }

        // ---------------------------------------------------------------- Tick hewan siang

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                var b = Bal;
                yield return new WaitForSeconds(b != null ? b.tickSeconds : 2f);
                TickSpawn();
            }
        }

        private bool AnimalsSuspended
        {
            get
            {
                for (int i = 0; i < overrides.Count; i++) if (overrides[i].Value.suspendAnimals || overrides[i].Value.suspendNormalSpawn) return true;
                return false;
            }
        }

        /// <summary>GDD bagian 3: siang hari hewan buas di luar ruangan. Tick ini hanya mengisi hewan.</summary>
        private void TickSpawn()
        {
            var b = Bal;
            var bal = GameManager.CurrentBalance;
            if (b == null || bal == null || isSleeping || isNight || AnimalsSuspended || RunSeedManager.Instance == null) return;
            int deficit = Mathf.Min(b.dayAnimalsAlive, b.maxAnimalsAlive) - CountAwakeAnimals();
            deficit = Mathf.Min(deficit, b.maxSpawnsPerTick);
            if (deficit <= 0) return;
            var table = TableFor(activeRegion);
            if (table == null) return;
            UpdateFrustum();
            ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Enemy);
            int spawned = 0;
            while (spawned < deficit)
            {
                int idx = PickEntry(table.dayEntries, ref rng, int.MaxValue, 1f);
                if (idx < 0) break;
                var entry = table.dayEntries[idx];
                Vector3 point;
                if (!TryTerritoryPoint(entry, ref rng, bal.wildlife.territorySpawnChance, out point)
                    && !FindSpawnPoint(ref rng, null, 0f, false, out point)) break;
                int n = SpawnGroup(entry, point, ref rng);
                if (n <= 0) break;
                spawned += n;
            }
        }

        private bool TryTerritoryPoint(SpawnEntry entry, ref RandomStream rng, float chance, out Vector3 point)
        {
            point = default;
            var b = Bal;
            var player = PlayerTargetable.Instance;
            if (entry.enemy == null || player == null || rng.NextFloat01() >= chance) return false;
            var all = AnimalTerritory.All;
            for (int i = 0; i < all.Count; i++)
            {
                var t = all[i];
                if (!t.Accepts(entry.enemy.enemyId)) continue;
                float d = Vector3.Distance(t.transform.position, player.Position);
                if (d < b.minDistance || d > b.maxDistance) continue;
                if (!NavMesh.SamplePosition(t.transform.position, out var hit, b.sampleRadius, NavMesh.AllAreas)) continue;
                if (IsVisible(hit.position)) continue;
                point = hit.position;
                return true;
            }
            return false;
        }

        private int SpawnGroup(SpawnEntry entry, Vector3 point, ref RandomStream rng)
        {
            var b = Bal;
            int count = Mathf.Max(1, rng.Range(Mathf.Max(1, entry.groupMin), Mathf.Max(1, entry.groupMax) + 1));
            int groupId = rng.Range(1, int.MaxValue);
            int spawned = 0;
            for (int m = 0; m < count; m++)
            {
                if (CountAwakeAnimals() >= b.maxAnimalsAlive) break;
                Vector3 pos = point;
                if (m > 0)
                {
                    Vector3 off = new Vector3(rng.Range(-2f, 2f), 0f, rng.Range(-2f, 2f));
                    if (NavMesh.SamplePosition(point + off, out var hit, 2f, NavMesh.AllAreas)) pos = hit.position;
                }
                var e = SpawnOne(entry.enemy, pos, ref rng);
                if (e == null) break;   // Pool memang punya batas
                e.GroupId = groupId;
                if (e.Wildlife != null) e.Wildlife.SetHome(pos, groupId);
                spawned++;
            }
            return spawned;
        }

        private EnemyInstance SpawnOne(EnemyData data, Vector3 pos, ref RandomStream rng)
        {
            if (data == null || ObjectPooler.Instance == null || !ObjectPooler.Instance.HasPool(data.enemyId)) return null;
            float yaw = rng.Range(0f, 360f);
            var go = ObjectPooler.Instance.SpawnFromPool(data.enemyId, pos, Quaternion.Euler(0f, yaw, 0f));
            if (go == null) return null;   // Pool kosong, atau EnemyAI kembali sendiri karena NavMesh
            var e = go.GetComponent<EnemyInstance>();
            if (e == null) { Debug.LogError($"[SpawnManager] Prefab '{data.enemyId}' tidak punya EnemyInstance.", go); ObjectPooler.Instance.ReturnToPool(go); return null; }
            if (!activeEnemies.Contains(e)) activeEnemies.Add(e);
            EventChannel<EnemySpawnedEvent>.Publish(new EnemySpawnedEvent(data.enemyId, go.transform.position));
            return e;
        }

        // ---------------------------------------------------------------- Gelombang zombie

        /// <summary>Dipanggil AIDirector (#27). Mengembalikan jumlah slot yang diterima.</summary>
        public int RequestWave(in WaveRequest r)
        {
            if (isSleeping) return 0;
            var b = Bal;
            if (b == null) return 0;
            // Sarang yang hancur mengurangi anggaran horde region ini (#112, usulan 3).
            int wanted = Mathf.RoundToInt(r.count * (1f - LairRegistry.GetBudgetReduction(activeRegion)));
            int count = Mathf.Min(wanted, CurrentZombieCap - AliveZombies - queue.Count);
            if (count <= 0) return 0;
            specialsThisWave = 0;
            largeThisWave = AliveLarge;
            for (int i = 0; i < count; i++)
            {
                queue.Add(new Slot
                {
                    modifiers = r.modifiers, maxLarge = r.maxLarge, specialShare = r.specialShare, forcedEnemyId = r.forcedEnemyId,
                    origin = r.origin, usesBudget = true
                });
            }
            return count;
        }

        /// <summary>Penyergapan saat tidur atau pemulihan pertempuran (#67, #80). Memakai tabel malam dan ForceAware.</summary>
        public void RequestAmbush(Vector3 center, int count, float radius)
        {
            int day = GameManager.Instance != null ? GameManager.Instance.currentDay : 1;
            var mods = AIDirector.Instance != null ? AIDirector.Instance.GetPostTargetModifiers(day) : WaveModifiers.One;
            for (int i = 0; i < count; i++)
                queue.Add(new Slot { modifiers = mods, maxLarge = int.MaxValue, specialShare = 1f, origin = center, radius = radius, forceAware = true });
        }

        /// <summary>Spawn bertahap dari tabel tertentu (#50, #71), di luar pandangan, tanpa behindChance.</summary>
        public void RequestBurst(RegionSpawnTable table, int count, Vector3 center, float radius, in WaveModifiers modifiers)
        {
            for (int i = 0; i < count; i++)
                queue.Add(new Slot { modifiers = modifiers, maxLarge = int.MaxValue, specialShare = 1f, origin = center, radius = radius, table = table });
        }

        public void RequestBurst(RegionSpawnTable table, int count, Vector3 center, float radius)
        {
            RequestBurst(table, count, center, radius, WaveModifiers.One);
        }

        private void Update()
        {
            if (queue.Count == 0 || Time.deltaTime <= 0f || RunSeedManager.Instance == null) return;
            var b = Bal;
            if (b == null) return;
            UpdateFrustum();
            ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Enemy);
            int done = 0;
            while (queue.Count > 0 && done < b.maxSpawnsPerFrame)
            {
                var slot = queue[0];
                queue.RemoveAt(0);
                done++;
                if (TrySpawnSlot(slot, ref rng)) continue;
                slot.attempts++;
                if (slot.attempts < 3) queue.Add(slot);   // Dicoba lagi pada frame berikutnya
                else if (slot.usesBudget && AIDirector.Instance != null) AIDirector.Instance.RefundBudget(1);   // Slot gagal 3 kali (SpawnManager.md langkah 7)
            }
        }

        private bool TrySpawnSlot(Slot slot, ref RandomStream rng)
        {
            if (AliveZombies >= CurrentZombieCap) return false;
            var table = slot.table != null ? slot.table : TableFor(activeRegion);
            if (table == null) return false;
            var entries = table.nightEntries;
            int idx = -1;
            if (!string.IsNullOrEmpty(slot.forcedEnemyId))
            {
                for (int i = 0; i < entries.Length; i++) if (entries[i].enemy != null && entries[i].enemy.enemyId == slot.forcedEnemyId) { idx = i; break; }
            }
            if (idx < 0) idx = PickEntry(entries, ref rng, slot.maxLarge, slot.specialShare);
            if (idx < 0) return false;
            var entry = entries[idx];
            // TODO #63/#25: Zombie Desa memakai HidingSpot.FindEmergeSpot, Lumut memakai BushSpawnPoint.
            if (!FindSpawnPoint(ref rng, slot.origin, slot.radius, slot.ignoreView, out var point)) return false;
            var e = SpawnOne(entry.enemy, point, ref rng);
            if (e == null) return false;
            if (entry.isSpecial) specialsThisWave++;
            if (entry.enemy.isLarge) largeThisWave++;
            // 6. Setelah keluar dari pool.
            e.UsesNightBudget = slot.usesBudget;
            e.Health.SetHPMultiplier(slot.modifiers.hp > 0f ? slot.modifiers.hp : 1f);
            if (e.AI != null) e.AI.ApplyWaveModifiers(in slot.modifiers);
            var sensory = e.GetComponent<AISensory>();
            if (slot.forceAware && sensory != null && PlayerTargetable.Instance != null) sensory.ForceAware(PlayerTargetable.Instance.Position);
            return true;
        }

        /// <summary>Bobot 0 untuk entri sebelum minDay, sudah maxAlive, porsi khusus penuh, atau varian besar penuh.</summary>
        private int PickEntry(SpawnEntry[] entries, ref RandomStream rng, int maxLarge, float specialShare)
        {
            if (entries == null || entries.Length == 0) return -1;
            if (weightBuffer.Length < entries.Length) weightBuffer = new float[entries.Length];
            int day = GameManager.Instance != null ? GameManager.Instance.currentDay : 1;
            float total = 0f;
            int waveSize = Mathf.Max(1, queue.Count + specialsThisWave + 1);
            for (int i = 0; i < weightBuffer.Length; i++) weightBuffer[i] = 0f;
            for (int i = 0; i < entries.Length; i++)
            {
                var en = entries[i];
                float w = en.weight;
                if (en.enemy == null || day < en.minDay) w = 0f;
                else if (en.maxAlive > 0 && CountAliveOf(en.enemy.enemyId) >= en.maxAlive) w = 0f;
                else if (en.isSpecial && specialsThisWave >= Mathf.CeilToInt(specialShare * waveSize)) w = 0f;
                else if (en.enemy.isLarge && largeThisWave >= maxLarge) w = 0f;
                weightBuffer[i] = Mathf.Max(0f, w);
                total += weightBuffer[i];
            }
            if (total <= 0f) return -1;
            int pick = rng.PickWeighted(weightBuffer);
            return pick >= 0 && pick < entries.Length ? pick : -1;
        }

        private RegionSpawnTable TableFor(RegionType region)
        {
            for (int i = overrides.Count - 1; i >= 0; i--) if (overrides[i].Value.table != null) return overrides[i].Value.table;
            int r = (int)region;
            return tables != null && r < tables.Length ? tables[r] : null;
        }

        // ---------------------------------------------------------------- Titik spawn

        private void UpdateFrustum()
        {
            var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : Camera.main;
            if (cam != null) GeometryUtility.CalculateFrustumPlanes(cam, frustum);
        }

        private bool IsVisible(Vector3 pos)
        {
            var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : Camera.main;
            if (cam == null) return false;
            if (!GeometryUtility.TestPlanesAABB(frustum, new Bounds(pos + Vector3.up, new Vector3(1f, 2f, 1f)))) return false;
            // Di dalam frustum: tetap tak terlihat kalau garis dari kamera terhalang.
            Vector3 from = cam.transform.position;
            Vector3 to = pos + Vector3.up;
            Vector3 dir = to - from;
            float len = dir.magnitude;
            return !(len > 0.01f && PhysicsQuery.Raycast(new Ray(from, dir / len), out _, len, PhysicsMasks.AIVision));
        }

        private bool FindSpawnPoint(ref RandomStream rng, Vector3? origin, float ringRadius, bool ignoreView, out Vector3 point)
        {
            point = default;
            var b = Bal;
            var player = PlayerTargetable.Instance;
            if (b == null || (player == null && !origin.HasValue)) return false;
            Vector3 center = origin ?? player.Position;
            var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : Camera.main;
            Vector3 back = cam != null ? -cam.transform.forward : Vector3.back;
            back.y = 0f;
            if (back.sqrMagnitude < 0.001f) back = Vector3.back;
            float backYaw = Mathf.Atan2(back.x, back.z) * Mathf.Rad2Deg;

            for (int attempt = 0; attempt < b.attemptsPerSpawn; attempt++)
            {
                // 1. Arah: 75 persen dari belakang pemain (usulan disetujui, CATATAN_MEKANIK.md). Override origin: 360 derajat.
                float yaw = !origin.HasValue && rng.NextFloat01() < b.behindChance
                    ? backYaw + rng.Range(-b.behindHalfAngleDeg, b.behindHalfAngleDeg)
                    : rng.Range(0f, 360f);
                // 2. Jarak.
                float minD = ringRadius > 0f ? ringRadius * 0.5f : b.minDistance;
                float maxD = ringRadius > 0f ? ringRadius : b.maxDistance;
                float dist = rng.Range(minD, maxD);
                Vector3 dir = Quaternion.Euler(0f, yaw, 0f) * Vector3.forward;
                Vector3 candidate = center + dir * dist;
                // 3. NavMesh.
                if (!NavMesh.SamplePosition(candidate, out var hit, b.sampleRadius, NavMesh.AllAreas)) { failedPoints++; continue; }
                // 4. Jarak ulang.
                float d2 = Vector3.Distance(hit.position, center);
                if (d2 < minD * 0.9f || d2 > maxD * 1.1f) { failedPoints++; continue; }
                // 5. Titik harus di region aktif yang sudah dimuat (#88). Tanpa RegionStreaming syarat ini lolos.
                if (RegionStreaming.Instance != null
                    && (RegionStreaming.Instance.GetRegionAt(hit.position) != activeRegion || !RegionStreaming.Instance.IsLoaded(activeRegion)))
                { failedPoints++; continue; }
                // 6. Pandangan: syarat tidak pernah dilonggarkan.
                if (!ignoreView && IsVisible(hit.position)) { failedPoints++; continue; }
                // 7. Tolak di dalam InteriorZone (#112) dan di dalam base (#78).
                if (InteriorZone.Contains(hit.position) || (BaseNavigation.Instance != null && BaseNavigation.Instance.IsInsideBase(hit.position)))
                { failedPoints++; continue; }
                // 8. Sesak.
                if (PhysicsQuery.OverlapSphere(hit.position + Vector3.up, 1f, PhysicsMasks.BuildOverlap, crowdBuffer) > 0) { failedPoints++; continue; }
                point = hit.position;
                return true;
            }
            return false;
        }

        // ---------------------------------------------------------------- Kembali ke pool

        /// <summary>Satu-satunya jalur kembali ke pool di Fase 2. Panggilan kedua diabaikan.</summary>
        public void Despawn(GameObject go, DespawnReason reason)
        {
            if (go == null) return;
            var e = go.GetComponent<EnemyInstance>();
            if (e != null)
            {
                bool wasListed = activeEnemies.Remove(e);
                // Anggaran malam dikembalikan hanya untuk zombie RequestWave yang hilang tanpa dibunuh (#27).
                if (wasListed && e.IsZombie && e.UsesNightBudget && AIDirector.Instance != null
                    && (reason == DespawnReason.Stuck || reason == DespawnReason.OffNavMesh || reason == DespawnReason.TooFar || reason == DespawnReason.RegionUnloaded))
                    AIDirector.Instance.RefundBudget(1);
            }
            var pooled = go.GetComponent<PooledObject>();
            if (pooled != null && pooled.IsInPool) return;
            if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(go);
        }

        /// <summary>#25, #94. Anggaran dikembalikan di dalam Despawn.</summary>
        public void ReturnStuck(EnemyInstance e)
        {
            if (e != null) Despawn(e.gameObject, DespawnReason.Stuck);
        }

        /// <summary>#90: zombie di sekitar base dikembalikan. Mengembalikan jumlahnya.</summary>
        public int ReturnRaidersNear(Vector3 center, float radius)
        {
            int n = 0;
            float r2 = radius * radius;
            for (int i = activeEnemies.Count - 1; i >= 0; i--)
            {
                var e = activeEnemies[i];
                if (e == null || !e.IsZombie || (e.transform.position - center).sqrMagnitude > r2) continue;
                Despawn(e.gameObject, DespawnReason.TooFar);
                n++;
            }
            return n;
        }

        // ---------------------------------------------------------------- Pengaman macet

        private IEnumerator WatchdogLoop()
        {
            while (true)
            {
                var b = Bal;
                float interval = b != null ? b.stuckCheckSeconds : 5f;
                yield return new WaitForSeconds(interval);
                if (b != null) Watchdog(b, interval);
            }
        }

        private void Watchdog(SpawnBalance b, float interval)
        {
            var player = PlayerTargetable.Instance;
            UpdateFrustum();
            for (int i = activeEnemies.Count - 1; i >= 0; i--)
            {
                var e = activeEnemies[i];
                if (e == null) { activeEnemies.RemoveAt(i); continue; }
                if (e.IsDormantCounted || e.IsCarcass || !e.Health.IsAlive) continue;
                if (e.AI != null)
                {
                    var s = e.AI.CurrentState;
                    if (s == AIState.AttackPlayer || s == AIState.AttackBuilding || s == AIState.BurnToAshes || s == AIState.Dying
                        || s == AIState.SeekHiding || s == AIState.Emerging || s == AIState.Dormant) continue;
                }
                if (e.Wildlife != null && e.Wildlife.State == AnimalState.Attack) continue;

                bool visible = IsVisible(e.transform.position);
                if (!e.IsCulled)
                {
                    // 2.
                    if (e.Agent != null && !e.Agent.isOnNavMesh)
                    {
                        e.offMeshTimer += interval;
                        if (!visible || e.offMeshTimer >= b.stuckForceSeconds) { Despawn(e.gameObject, DespawnReason.OffNavMesh); continue; }
                    }
                    else e.offMeshTimer = 0f;
                    // 3-4.
                    float moved = (e.transform.position - e.lastPosition).magnitude;
                    e.lastPosition = e.transform.position;
                    if (moved < b.stuckMoveEpsilon) e.stuckTimer += interval; else e.stuckTimer = 0f;
                    if ((e.stuckTimer >= b.stuckSeconds && !visible) || e.stuckTimer >= b.stuckForceSeconds) { Despawn(e.gameObject, DespawnReason.Stuck); continue; }
                }
                // 5.
                if (player != null && !e.IsEngaged && (e.transform.position - player.Position).sqrMagnitude > b.despawnDistance * b.despawnDistance)
                    Despawn(e.gameObject, DespawnReason.TooFar);
            }
        }

        // ---------------------------------------------------------------- Override

        public SpawnOverrideHandle PushOverride(in SpawnOverride o)
        {
            int id = nextOverrideId++;
            overrides.Add(new KeyValuePair<int, SpawnOverride>(id, o));
            return new SpawnOverrideHandle { id = id };
        }

        public void PopOverride(SpawnOverrideHandle h)
        {
            for (int i = 0; i < overrides.Count; i++) if (overrides[i].Key == h.id) { overrides.RemoveAt(i); return; }
        }

        // ---------------------------------------------------------------- DevConsole (#56)

        /// <summary>Di depan pemain, tanpa syarat pandangan.</summary>
        public void DebugSpawn(string enemyId, int count)
        {
            var player = PlayerTargetable.Instance;
            if (player == null || ObjectPooler.Instance == null || RunSeedManager.Instance == null) return;
            EnemyData data = null;
            for (int t = 0; t < tables.Length && data == null; t++)
            {
                if (tables[t] == null) continue;
                foreach (var en in tables[t].dayEntries) if (en.enemy != null && en.enemy.enemyId == enemyId) { data = en.enemy; break; }
                if (data != null) break;
                foreach (var en in tables[t].nightEntries) if (en.enemy != null && en.enemy.enemyId == enemyId) { data = en.enemy; break; }
            }
            if (data == null) { Debug.LogWarning($"[SpawnManager] enemyId '{enemyId}' tidak ada di tabel mana pun."); return; }
            var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : null;
            Vector3 fwd = cam != null ? cam.transform.forward : player.transform.forward;
            fwd.y = 0f;
            ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Enemy);
            for (int i = 0; i < count; i++)
            {
                Vector3 p = player.Position + fwd.normalized * 8f + new Vector3(rng.Range(-3f, 3f), 0f, rng.Range(-3f, 3f));
                if (!NavMesh.SamplePosition(p, out var hit, 6f, NavMesh.AllAreas)) continue;
                if (SpawnOne(data, hit.position, ref rng) == null) break;
            }
        }

        public void DebugReturnAll()
        {
            for (int i = activeEnemies.Count - 1; i >= 0; i--) if (activeEnemies[i] != null) Despawn(activeEnemies[i].gameObject, DespawnReason.Debug);
            activeEnemies.Clear();
        }

        public int FailedSpawnPoints => failedPoints;

#if UNITY_EDITOR
        private void ValidatePools()
        {
            var pooler = FindAnyObjectByType<ObjectPooler>();
            if (pooler == null || tables == null) return;
            foreach (var t in tables)
            {
                if (t == null) continue;
                Check(pooler, t.dayEntries);
                Check(pooler, t.nightEntries);
            }
        }

        private static void Check(ObjectPooler pooler, SpawnEntry[] entries)
        {
            if (entries == null) return;
            foreach (var en in entries)
            {
                if (en.enemy == null) continue;
                var pool = pooler.pools.Find(p => p.tag == en.enemy.enemyId);
                if (pool == null) Debug.LogWarning($"[SpawnManager] Pool '{en.enemy.enemyId}' belum ada di ObjectPooler.");
                else if (en.maxAlive > 0 && pool.size < en.maxAlive) Debug.LogWarning($"[SpawnManager] Pool '{en.enemy.enemyId}' ({pool.size}) lebih kecil dari maxAlive ({en.maxAlive}).");
            }
        }
#endif
    }
}
