using System;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    [Serializable]
    public class ZombieLairSaveData
    {
        public string lairId;
        public int remainingCount;
        public bool isCleared;
        public bool isNestDestroyed;
        public float nestHP;
    }

    /// <summary>
    /// Sarang zombie: titik tidur, zombie Dormant, pemicu bangun, batas pengejaran saat siang, pelepasan horde malam,
    /// dan sarang yang bisa dihancurkan. Blueprint: left-4-dead/ZombieLair.md (#112).
    /// GDD bagian 3: tidak ada zombie berkeliaran di luar ruangan saat siang; sebagian zombie diam di ruang gelap (gua,
    /// bunker, gudang) lalu bangun kalau terganggu; horde keluar dari persembunyiannya saat malam.
    /// Usulan disetujui developer (#85): lokasi per region, sarang mengurangi horde, separuh keluar saat malam (kecuali
    /// Region 4), berhenti di pintu saat siang, senter membangunkan setelah 2 detik.
    /// Komponen di objek yang sama dengan InteriorZone.
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - RegionStateStore/RegionSceneRoot (#88) belum ada: status didaftarkan langsung ke WorldStateSave dengan SaveId
    ///   "lair_" + lairId (pola LuckyMachine Langkah 7). Ganti ke RegionStateStore saat #88 ada.
    /// - EnemyAI belum punya field lair dan EnterDormant(lair): rujukan disimpan LairRegistry, dan areaMask diatur di sini.
    /// - SpawnManager belum bisa mengambil alih zombie dari luar: zombie yang dilepas saat malam tetap dari pool sarang dan
    ///   tidak masuk daftar SpawnManager; bagian yang belum muncul diminta lewat SpawnManager.RequestBurst di pintu keluar.
    /// - FlashlightController dan LightSourceSystem (#98) belum ada: cahaya dibaca dari TorchController yang menyala.
    /// </summary>
    [RequireComponent(typeof(InteriorZone))]
    public class ZombieLair : MonoBehaviour, ISaveable
    {
        private enum MemberState : byte { Empty, Dormant, Awake }

        private struct Member
        {
            public EnemyAI ai;
            public EnemyHealth health;
            public int spawnIndex;
            public MemberState state;
            public float lightTimer;
            public float lostTimer;
        }

        [SerializeField] private LairData data;
        [SerializeField] private Transform[] spawnPoints = new Transform[0];
        [SerializeField] private LairNest nest;

        public LairData Data => data;
        public InteriorZone Zone { get; private set; }
        public int RemainingCount => remainingCount;
        public bool IsCleared => isCleared;
        public bool IsNestDestroyed => isNestDestroyed;
        public bool IsSpawned => isSpawned;

        private Member[] members;
        private Action<DamageInfo>[] diedHandlers;
        private Action<DamageInfo>[] damagedHandlers;
        private int remainingCount;
        private bool isCleared;
        private bool isNestDestroyed;
        private bool isSpawned;
        private float senseTimer;
        private int interiorMask = -1;
        private PlayerTraversal playerTraversal;
        private bool initialized;

        private static LairBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.lair : null;
        private static bool IsDay => !DayPhaseTracker.IsNight;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Zone = GetComponent<InteriorZone>();
            if (nest == null) nest = GetComponentInChildren<LairNest>(true);
            int cap = Mathf.Max(spawnPoints != null ? spawnPoints.Length : 0, data != null ? data.dormantCount : 0);
            members = new Member[Mathf.Max(1, cap)];
            diedHandlers = new Action<DamageInfo>[members.Length];
            damagedHandlers = new Action<DamageInfo>[members.Length];
            for (int i = 0; i < members.Length; i++)
            {
                int idx = i;   // Delegate dibuat sekali per slot, bukan per spawn
                diedHandlers[i] = _ => HandleMemberDied(idx);
                damagedHandlers[i] = _ => HandleMemberDamaged(idx);
            }
            int area = NavMesh.GetAreaFromName("Interior");
            interiorMask = area >= 0 ? 1 << area : -1;
            ResetState();
        }

        private void ResetState()
        {
            remainingCount = data != null ? data.dormantCount : 0;
            isCleared = false;
            isNestDestroyed = false;
            if (nest != null)
            {
                bool useNest = data != null && data.hasNest;
                nest.Setup(NestMaxHP, useNest ? NestMaxHP : 0f);
            }
            initialized = true;
        }

        private float NestMaxHP
        {
            get
            {
                var b = Bal;
                if (data != null && data.nestHP > 0f) return data.nestHP;
                return b != null ? b.nestHP : 300f;
            }
        }

        private void OnEnable()
        {
            if (!initialized) ResetState();
            LairRegistry.Register(this);
            EventChannel<DaybreakEvent>.Raised += HandleDaybreak;
            EventChannel<NightfallEvent>.Raised += HandleNightfall;
            EventChannel<NoiseEmittedEvent>.Raised += HandleNoise;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Register(this);
        }

        private void OnDisable()
        {
            EventChannel<DaybreakEvent>.Raised -= HandleDaybreak;
            EventChannel<NightfallEvent>.Raised -= HandleNightfall;
            EventChannel<NoiseEmittedEvent>.Raised -= HandleNoise;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            // Region dilepas: zombie kembali ke pool, status tetap.
            if (isSpawned) Despawn();
            LairRegistry.Unregister(this);
        }

        // ---------------------------------------------------------------- Pemunculan

        /// <summary>Dipanggil LairRegistry saat pemain dalam activationRadius.</summary>
        public void SpawnDormant()
        {
            if (data == null || isSpawned) return;
            isSpawned = true;
            FillMembers();
        }

        private void FillMembers()
        {
            if (data == null || data.occupants == null || data.occupants.Length == 0 || spawnPoints == null) return;
            var pooler = ObjectPooler.Instance;
            if (pooler == null || RunSeedManager.Instance == null) return;
            ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Enemy);
            int alive = CountMembers();
            for (int p = 0; p < spawnPoints.Length && alive < remainingCount; p++)
            {
                if (spawnPoints[p] == null || IsSpawnPointUsed(p)) continue;
                int slot = FreeSlot();
                if (slot < 0) break;
                var occupant = data.occupants[rng.Range(0, data.occupants.Length)];
                if (occupant == null || !pooler.HasPool(occupant.enemyId)) continue;
                var go = pooler.SpawnFromPool(occupant.enemyId, spawnPoints[p].position, spawnPoints[p].rotation);
                if (go == null) break;   // Pool penuh
                var ai = go.GetComponent<EnemyAI>();
                var health = go.GetComponent<EnemyHealth>();
                if (ai == null || health == null || !go.activeInHierarchy) continue;   // EnemyAI kembali sendiri tanpa NavMesh
                members[slot] = new Member { ai = ai, health = health, spawnIndex = p, state = MemberState.Dormant };
                health.Died += diedHandlers[slot];
                health.Damaged += damagedHandlers[slot];
                LairRegistry.MapEnemy(ai, this);
                EnterDormant(ref members[slot]);
                alive++;
            }
        }

        /// <summary>Zombie Dormant kembali ke pool, remainingCount tetap.</summary>
        public void Despawn()
        {
            isSpawned = false;
            for (int i = 0; i < members.Length; i++)
            {
                if (members[i].state == MemberState.Empty) continue;
                var go = members[i].ai != null ? members[i].ai.gameObject : null;
                ReleaseMember(i);
                if (go == null || !go.activeInHierarchy) continue;
                if (SpawnManager.Instance != null) SpawnManager.Instance.Despawn(go, DespawnReason.RegionUnloaded);
                else if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(go);
            }
        }

        private void ReleaseMember(int i)
        {
            ref var m = ref members[i];
            if (m.health != null)
            {
                m.health.Died -= diedHandlers[i];
                m.health.Damaged -= damagedHandlers[i];
            }
            if (m.ai != null)
            {
                LairRegistry.UnmapEnemy(m.ai);
                // OnReturned mengembalikan areaMask bawaan. Zombie yang masih hidup memakai traversalMask penuh.
                if (m.ai.isActiveAndEnabled && m.ai.Agent != null && m.ai.Data != null) m.ai.Agent.areaMask = m.ai.Data.traversalMask;
            }
            m = default;
        }

        private int CountMembers()
        {
            int n = 0;
            for (int i = 0; i < members.Length; i++) if (members[i].state != MemberState.Empty) n++;
            return n;
        }

        private int FreeSlot()
        {
            for (int i = 0; i < members.Length; i++) if (members[i].state == MemberState.Empty) return i;
            return -1;
        }

        private bool IsSpawnPointUsed(int p)
        {
            for (int i = 0; i < members.Length; i++) if (members[i].state != MemberState.Empty && members[i].spawnIndex == p) return true;
            return false;
        }

        // ---------------------------------------------------------------- State Dormant

        private void EnterDormant(ref Member m)
        {
            m.state = MemberState.Dormant;
            m.lightTimer = 0f;
            m.lostTimer = 0f;
            m.ai.EnterDormant();
            ApplyAreaMask(ref m);
        }

        /// <summary>Usulan 5: saat siang hanya Area Interior, jadi jalur berhenti di ambang pintu.</summary>
        private void ApplyAreaMask(ref Member m)
        {
            var agent = m.ai != null ? m.ai.Agent : null;
            if (agent == null || m.ai.Data == null) return;
            bool interiorOnly = IsDay || (data != null && data.region == RegionType.Logistic);
            agent.areaMask = interiorOnly && interiorMask != -1 ? interiorMask : m.ai.Data.traversalMask;
            // NavMesh rusak di pintu: kembalikan ke titik tidur.
            if (agent.enabled && !agent.isOnNavMesh && spawnPoints[m.spawnIndex] != null
                && NavMesh.SamplePosition(spawnPoints[m.spawnIndex].position, out var hit, 2f, agent.areaMask))
                agent.Warp(hit.position);
        }

        private void Wake(int i)
        {
            ref var m = ref members[i];
            if (m.state != MemberState.Dormant || m.ai == null) return;
            m.state = MemberState.Awake;
            m.lostTimer = 0f;
            // Ke Chase; ThreatSystem (#108) memilih target.
            var player = PlayerTargetable.Instance;
            m.ai.WakeUp(player != null ? player.transform : null);
            WakeGroup(m.ai.transform.position);
        }

        /// <summary>Bangunkan zombie lain dalam groupWakeRadius.</summary>
        private void WakeGroup(Vector3 position)
        {
            var b = Bal;
            float r2 = b != null ? b.groupWakeRadius * b.groupWakeRadius : 36f;
            for (int i = 0; i < members.Length; i++)
            {
                if (members[i].state != MemberState.Dormant || members[i].ai == null) continue;
                if ((members[i].ai.transform.position - position).sqrMagnitude <= r2) Wake(i);
            }
        }

        private void WakeAll()
        {
            for (int i = 0; i < members.Length; i++) if (members[i].state == MemberState.Dormant) Wake(i);
        }

        private void HandleMemberDamaged(int i)
        {
            // Pemicu damage: damage apa pun membangunkan.
            if (i >= 0 && i < members.Length && members[i].state == MemberState.Dormant) Wake(i);
        }

        private void HandleMemberDied(int i)
        {
            if (i < 0 || i >= members.Length || members[i].state == MemberState.Empty) return;
            ReleaseMember(i);
            remainingCount = Mathf.Max(0, remainingCount - 1);
            // Lokasi tanpa sarang: bersih saat semua penghuni habis, tanpa pengurangan horde.
            if (remainingCount == 0 && (data == null || !data.hasNest)) isCleared = true;
        }

        // ---------------------------------------------------------------- Pemicu bangun

        private void HandleNoise(NoiseEmittedEvent e)
        {
            if (!isSpawned || e.radius <= 0f) return;
            var b = Bal;
            float radius = e.radius * (b != null ? b.noiseWakeMultiplier : 1f);
            bool fromInside = Zone != null && Zone.ContainsPoint(e.position);
            for (int i = 0; i < members.Length; i++)
            {
                if (members[i].state != MemberState.Dormant || members[i].ai == null) continue;
                Vector3 zp = members[i].ai.transform.position;
                // Suara dari luar zona melewati exitPoints terdekat.
                float d = fromInside ? Vector3.Distance(e.position, zp) : Zone.PathDistanceFromOutside(e.position, zp);
                if (d < radius) Wake(i);
            }
        }

        private void Update()
        {
            var b = Bal;
            if (!isSpawned || b == null || Time.deltaTime <= 0f) return;
            senseTimer -= Time.deltaTime;
            if (senseTimer > 0f) return;
            float dt = b.senseIntervalSeconds;
            senseTimer = dt;
            SenseTick(b, dt);
        }

        private void SenseTick(LairBalance b, float dt)
        {
            var player = PlayerTargetable.Instance;
            if (playerTraversal == null && IslandPlayerController.Instance != null)
                playerTraversal = IslandPlayerController.Instance.GetComponent<PlayerTraversal>();
            bool crouching = playerTraversal != null && playerTraversal.IsCrouching;
            float prox = crouching ? b.crouchProximityWakeRadius : b.proximityWakeRadius;
            var torch = EquipmentManager.Instance != null ? EquipmentManager.Instance.Active as TorchController : null;
            bool lightOn = torch != null && torch.IsLit;   // TODO(FlashlightController): senter (#98)
            Vector3 eye = AISensoryHub.Instance != null ? AISensoryHub.Instance.PlayerEyePosition : (player != null ? player.AimPoint : Vector3.zero);

            for (int i = 0; i < members.Length; i++)
            {
                ref var m = ref members[i];
                if (m.state == MemberState.Empty) continue;
                // Kembali ke pool lewat jalur lain (macet, region): lepas tanpa mengurangi jumlah.
                if (m.ai == null || !m.ai.isActiveAndEnabled) { ReleaseMember(i); continue; }

                if (m.state == MemberState.Dormant)
                {
                    if (player == null) continue;
                    Vector3 zp = m.ai.transform.position;
                    float d = Vector3.Distance(zp, player.Position);
                    // Jarak (PlayerTraversal.IsCrouching).
                    if (d < prox) { Wake(i); continue; }
                    // Cahaya: lightWakeSeconds dalam lightWakeRange dengan garis pandang bebas.
                    if (lightOn && d <= b.lightWakeRange && HasLineOfSight(zp + Vector3.up * 1.6f, eye))
                    {
                        m.lightTimer += dt;
                        if (m.lightTimer >= b.lightWakeSeconds) Wake(i);
                    }
                    else m.lightTimer = 0f;
                    continue;
                }

                // Awake. Terdorong keluar zona saat siang: bukan lagi zombie sarang, SunlightExposure (#63) membakarnya.
                if (IsDay && Zone != null && !Zone.ContainsPoint(m.ai.transform.position))
                {
                    ReleaseMember(i);
                    remainingCount = Mathf.Max(0, remainingCount - 1);
                    continue;
                }
                bool playerInside = Zone != null && Zone.IsPlayerInside;
                bool hasTarget = m.ai.CurrentTarget != null && m.ai.CurrentTarget.IsTargetable;
                bool doorWait = IsDay && !playerInside;
                if (doorWait || !hasTarget) m.lostTimer += dt; else m.lostTimer = 0f;
                // Siang: tunggu doorWaitSeconds di pintu. Tanpa target: returnToDormantSeconds.
                float limit = doorWait ? b.doorWaitSeconds : b.returnToDormantSeconds;
                if (m.lostTimer >= limit) ReturnToSleep(ref m);
            }
        }

        private static bool HasLineOfSight(Vector3 from, Vector3 to)
        {
            Vector3 dir = to - from;
            float len = dir.magnitude;
            return len < 0.01f || !PhysicsQuery.Raycast(new Ray(from, dir / len), out _, len, PhysicsMasks.AIVision);
        }

        /// <summary>Kembali ke titik tidur dalam keadaan Dormant. Pemicu bangun tetap berlaku di jalan.</summary>
        private void ReturnToSleep(ref Member m)
        {
            EnterDormant(ref m);
            var agent = m.ai.Agent;
            var point = spawnPoints[m.spawnIndex];
            if (agent != null && agent.enabled && agent.isOnNavMesh && point != null)
            {
                agent.isStopped = false;
                agent.SetDestination(point.position);
            }
        }

        // ---------------------------------------------------------------- Siang dan malam

        private void HandleDaybreak(DaybreakEvent e)
        {
            // Zombie sarang di dalam zona tidak terbakar dan areaMask kembali Interior. Di luar zona mengikuti #63.
            for (int i = 0; i < members.Length; i++)
            {
                if (members[i].state == MemberState.Empty || members[i].ai == null) continue;
                if (Zone != null && !Zone.ContainsPoint(members[i].ai.transform.position))
                {
                    ReleaseMember(i);
                    remainingCount = Mathf.Max(0, remainingCount - 1);
                    continue;
                }
                ApplyAreaMask(ref members[i]);
            }
            // Usulan 4: sarang terisi ulang sampai dormantCount saat fajar, kecuali isCleared.
            if (!isCleared && data != null)
            {
                remainingCount = data.dormantCount;
                if (isSpawned) FillMembers();
            }
        }

        private void HandleNightfall(NightfallEvent e)
        {
            var b = Bal;
            if (data == null || b == null || !b.enabled || isCleared) return;
            // Usulan 2: Region 4 tetap di dalam ruangan (GDD bagian 5 hanya menyebut Zombie Desa sebagai ancaman malam).
            if (data.region == RegionType.Logistic) return;

            int release = Mathf.RoundToInt(remainingCount * b.nightReleaseRatio);
            if (release <= 0) return;
            int released = 0;
            // Usulan 4 ("keluar dari persembunyiannya"): zombie yang sudah muncul menjadi anggota horde.
            for (int i = 0; i < members.Length && released < release; i++)
            {
                if (members[i].state == MemberState.Empty || members[i].ai == null) continue;
                var ai = members[i].ai;
                var player = PlayerTargetable.Instance;
                ReleaseMember(i);   // Rujukan sarang dikosongkan dan Died dilepas: saat fajar ia zombie horde
                ai.WakeUp(player != null ? player.transform : null);
                released++;
            }
            remainingCount = Mathf.Max(0, remainingCount - release);
            // Sisanya yang belum muncul keluar lewat pintu, lewat SpawnManager (#62).
            int pending = release - released;
            if (pending > 0 && SpawnManager.Instance != null && Zone != null)
            {
                Vector3 exit = Zone.NearestExit(PlayerTargetable.Instance != null ? PlayerTargetable.Instance.Position : transform.position);
                SpawnManager.Instance.RequestBurst(null, pending, exit, 0f);
            }
            // Sisanya tetap Dormant, dengan areaMask penuh saat malam.
            for (int i = 0; i < members.Length; i++)
                if (members[i].state != MemberState.Empty) ApplyAreaMask(ref members[i]);
        }

        private void HandleRunEnded(RunEndedEvent e)
        {
            if (isSpawned) Despawn();
        }

        // ---------------------------------------------------------------- Sarang

        internal void HandleNestDamaged(DamageInfo info)
        {
            if (isSpawned) WakeGroup(nest != null ? nest.transform.position : transform.position);
        }

        internal void HandleNestDestroyed()
        {
            if (isNestDestroyed) return;
            // 1-3.
            isNestDestroyed = true;
            isCleared = true;
            WakeAll();
            EventChannel<LairClearedEvent>.Publish(new LairClearedEvent(data != null ? data.lairId : name, data != null ? data.region : RegionType.Volcano));
            // 4. Pengurangan horde dibaca SpawnManager lewat LairRegistry.GetBudgetReduction.
        }

        // ---------------------------------------------------------------- Save

        public string SaveId => "lair_" + (data != null ? data.lairId : name);
        public int LoadOrder => 140;

        public string CaptureState()
        {
            return JsonUtility.ToJson(new ZombieLairSaveData
            {
                lairId = data != null ? data.lairId : name, remainingCount = remainingCount, isCleared = isCleared,
                isNestDestroyed = isNestDestroyed, nestHP = nest != null ? nest.CurrentHP : 0f
            });
        }

        public void RestoreState(string json)
        {
            var d = string.IsNullOrEmpty(json) ? null : JsonUtility.FromJson<ZombieLairSaveData>(json);
            if (d == null) return;
            if (isSpawned) Despawn();
            remainingCount = Mathf.Max(0, d.remainingCount);
            isCleared = d.isCleared;
            isNestDestroyed = d.isNestDestroyed;
            if (nest != null) nest.Setup(NestMaxHP, d.isNestDestroyed ? 0f : d.nestHP);
            initialized = true;
        }

        // Zombie aktif tidak disimpan: muncul lagi Dormant sesuai remainingCount saat pemain mendekat.
        public void OnRestoreFinished() { }
    }
}
