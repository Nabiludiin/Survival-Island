using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>Target serangan bangunan satu zombie (BaseNavigation.md bagian 1).</summary>
    public struct BreachTarget
    {
        public string structureId;
        public IDamageable target;
        public Vector3 slotPosition;
        public int slotIndex;          // -1 berarti posisi tunggu (semua posisi penuh)
    }

    /// <summary>Posisi serangan satu struktur. Dibuat saat struktur pertama kali diserang, lalu dipakai ulang.</summary>
    public class BreachSlots
    {
        public Vector3[] outerPositions;   // Paruh pertama sisi +normal, paruh kedua sisi -normal
        public int[] occupantIds;          // -1 berarti kosong
        public int attackerCount;
        public int maxAttackers;
        public Vector3 center;
        public Vector3 normal;
        public IDamageable target;
    }

    /// <summary>Hasil pemeriksaan jalur zombie saat Chase.</summary>
    public enum BlockedCheck : byte
    {
        None,           // Belum ada hasil baru
        Clear,          // Jalur lengkap
        Breach,         // Target bangunan didapat: masuk AttackBuilding
        Wait,           // Ada kandidat tapi posisi penuh: tunggu di slotPosition, coba lagi
        NoStructure,    // Terhalang tanpa struktur: ikuti AITraversal.GetResponse (#94)
    }

    /// <summary>
    /// Pengubah jalur musuh saat base dibangun, deteksi pemain di dalam base, pembagian posisi serangan dinding dan pintu,
    /// dan pusat base (BaseAnchor). Blueprint: raft/BaseNavigation.md (#78).
    /// GDD bagian 3: Horde Zombie menyerang base saat malam. Bagian 4: base sebagai tempat berlindung.
    /// Pilihan carving, pintu terkunci, definisi dalam base, dan jumlah penyerang adalah usulan disetujui developer (#85).
    /// Manager per scene IslandPhase. Pemeriksaan jalur diantre dengan batas maxPathChecksPerFrame.
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - EnemyAI memanggil NotifySpawned, PollChase, PollAttacking, dan Forget. Status jalur per zombie disimpan di sini,
    ///   bukan di EnemyAI, supaya integrasinya cukup beberapa baris (lihat laporan Grup F).
    /// - StructureRegistry tidak membuka daftar struktur, jadi pusat base dan pengaturan obstacle memakai pemindaian
    ///   FindObjectsByType yang hanya berjalan saat jumlah struktur berubah.
    /// - Cara menghitung pusat base belum diputuskan developer (bagian 5 usulan 7). Sementara: rata-rata posisi struktur.
    /// - RegionStreaming (#88): region anchor wajib dimuat, dan RegionLoadStateChangedEvent Unloading mengosongkan posisi serangan.
    /// </summary>
    public class BaseNavigation : MonoBehaviour
    {
        public static BaseNavigation Instance { get; private set; }

        public bool IsPlayerInsideBase { get; private set; }

        /// <summary>Target markas di pusat base (#108, #50).</summary>
        public BaseAnchor Anchor => baseAnchor;

        private sealed class AgentState
        {
            public EnemyAI enemy;
            public int id;
            public bool queued;
            public bool attacking;
            public float graceUntil;
            public float nextCheckTime;
            public float lastCheckTime;
            public float blockedTimer;
            public BlockedCheck result;
            public BreachTarget breach;
            public bool hasSlot;
            public bool invalidated;
            public bool returnToChase;
            public float retargetUntil;
            public Vector3 attackTargetPos;

            public void Clear()
            {
                enemy = null; id = 0; queued = attacking = hasSlot = invalidated = returnToChase = false;
                graceUntil = nextCheckTime = lastCheckTime = blockedTimer = retargetUntil = 0f;
                result = BlockedCheck.None;
                breach = default;
                attackTargetPos = default;
            }
        }

        private readonly Dictionary<string, BreachSlots> slotsByStructure = new Dictionary<string, BreachSlots>(32, System.StringComparer.Ordinal);
        private readonly Stack<BreachSlots> spareSlots = new Stack<BreachSlots>(16);
        private readonly Dictionary<int, AgentState> agents = new Dictionary<int, AgentState>(64);
        private readonly Stack<AgentState> spareAgents = new Stack<AgentState>(64);
        private readonly Queue<AgentState> pathCheckQueue = new Queue<AgentState>(64);
        private NavMeshPath sharedPath;
        private readonly Collider[] overlapBuffer = new Collider[32];
        private readonly string[] candidateIds = new string[16];
        private readonly IDamageable[] candidateTargets = new IDamageable[16];
        private readonly Collider[] candidateColliders = new Collider[16];
        private readonly float[] candidateDist = new float[16];
        private readonly bool[] candidateIsDoor = new bool[16];

        private BaseAnchor baseAnchor;
        private bool anchorDirty = true;
        private int scannedCount = -1;
        private bool hasAnchor;
        private Vector3 anchorPosition;
        private int carvingCount;
        private Coroutine insideLoop;

        private static readonly Vector3[] FourDirections = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };

        private static BaseNavBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.baseNav : null;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this) { Debug.LogError("[BaseNavigation] Lebih dari satu instance."); enabled = false; return; }
            Instance = this;
            sharedPath = new NavMeshPath();
            // BaseAnchor dibuat sekali di scene, bukan per malam.
            var go = new GameObject("BaseAnchor");
            go.transform.SetParent(transform, false);
            baseAnchor = go.AddComponent<BaseAnchor>();
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<StructureDestroyedEvent>.Raised += HandleStructureDestroyed;
            EventChannel<StructureBuiltEvent>.Raised += HandleStructureBuilt;
            EventChannel<StructureUpgradedEvent>.Raised += HandleStructureUpgraded;
            EventChannel<DoorStateChangedEvent>.Raised += HandleDoorState;
            EventChannel<RegionLoadStateChangedEvent>.Raised += HandleRegionLoadState;   // #88
            insideLoop = StartCoroutine(InsideLoop());
        }

        private void OnDisable()
        {
            EventChannel<StructureDestroyedEvent>.Raised -= HandleStructureDestroyed;
            EventChannel<StructureBuiltEvent>.Raised -= HandleStructureBuilt;
            EventChannel<StructureUpgradedEvent>.Raised -= HandleStructureUpgraded;
            EventChannel<DoorStateChangedEvent>.Raised -= HandleDoorState;
            EventChannel<RegionLoadStateChangedEvent>.Raised -= HandleRegionLoadState;
            if (insideLoop != null) StopCoroutine(insideLoop);
            insideLoop = null;
        }

        // ---------------------------------------------------------------- Antrean pemeriksaan jalur

        private AgentState GetState(EnemyAI e, bool create)
        {
            int id = e.GetInstanceID();
            if (agents.TryGetValue(id, out var s)) return s;
            if (!create) return null;
            s = spareAgents.Count > 0 ? spareAgents.Pop() : new AgentState();
            s.Clear();
            s.enemy = e;
            s.id = id;
            var b = Bal;
            // Langkah 1: zombie baru belum boleh dianggap terhalang selama spawnGraceSeconds.
            s.graceUntil = Time.time + (b != null ? b.spawnGraceSeconds : 2f);
            // Jeda awal acak dari ID supaya pemeriksaan tidak serempak.
            float interval = b != null ? b.pathCheckIntervalSeconds : 1f;
            s.nextCheckTime = s.graceUntil + (id & 15) / 16f * interval;
            s.lastCheckTime = Time.time;
            agents[id] = s;
            return s;
        }

        /// <summary>Dipanggil EnemyAI.OnSpawned: memulai spawnGraceSeconds dari awal.</summary>
        public void NotifySpawned(EnemyAI e)
        {
            if (e == null) return;
            Forget(e);
            GetState(e, true);
        }

        /// <summary>
        /// Dipanggil EnemyAI tiap tick saat Chase dan data.canAttackBuildings. Mengembalikan hasil pemeriksaan terbaru
        /// sekali saja. Breach dan Wait mengisi target.
        /// </summary>
        public BlockedCheck PollChase(EnemyAI e, out BreachTarget target)
        {
            target = default;
            if (e == null) return BlockedCheck.None;
            var s = GetState(e, true);
            if (s.attacking) { ReleaseSlotInternal(s); s.attacking = false; }
            float now = Time.time;
            if (now < s.graceUntil) return BlockedCheck.None;
            if (!s.queued && now >= s.nextCheckTime) { s.queued = true; pathCheckQueue.Enqueue(s); }
            var r = s.result;
            if (r == BlockedCheck.None) return r;
            s.result = BlockedCheck.None;
            target = s.breach;
            return r;
        }

        /// <summary>
        /// Dipanggil EnemyAI tiap tick saat AttackBuilding. targetPos = posisi target gerak (pemain atau BaseAnchor).
        /// true berarti kembali ke Chase (struktur hilang, pintu terbuka, atau jalur ke target sudah lengkap).
        /// </summary>
        public bool PollAttacking(EnemyAI e, Vector3 targetPos)
        {
            if (e == null) return true;
            var s = GetState(e, true);
            s.attacking = true;
            s.attackTargetPos = targetPos;
            if (s.invalidated || s.returnToChase)
            {
                ReleaseSlotInternal(s);
                s.invalidated = s.returnToChase = false;
                s.attacking = false;
                s.blockedTimer = 0f;
                return true;
            }
            // Jalur ke posisi serangan gagal: lepas posisi, lalu coba lagi setelah retargetCooldownSeconds.
            var agent = e.Agent;
            if (s.hasSlot && agent != null && agent.enabled && !agent.pathPending && agent.hasPath && agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                var b = Bal;
                ReleaseSlotInternal(s);
                s.attacking = false;
                s.retargetUntil = Time.time + (b != null ? b.retargetCooldownSeconds : 4f);
                return true;
            }
            if (!s.queued && Time.time >= s.nextCheckTime) { s.queued = true; pathCheckQueue.Enqueue(s); }
            return false;
        }

        /// <summary>Target struktur zombie ini masih sah.</summary>
        public bool IsBreachValid(EnemyAI e)
        {
            var s = e != null ? GetState(e, false) : null;
            return s != null && s.hasSlot && !s.invalidated && s.breach.target != null && s.breach.target.IsAlive;
        }

        /// <summary>Saat keluar dari AttackBuilding, mati, atau OnReturned.</summary>
        public void ReleaseSlot(EnemyAI e)
        {
            var s = e != null ? GetState(e, false) : null;
            if (s != null) ReleaseSlotInternal(s);
        }

        /// <summary>Dipanggil EnemyAI.OnReturned: lepas posisi dan hapus status jalur. Zombie dari pool tidak membawa target lama.</summary>
        public void Forget(EnemyAI e)
        {
            if (e == null) return;
            int id = e.GetInstanceID();
            if (!agents.TryGetValue(id, out var s)) return;
            ReleaseSlotInternal(s);
            agents.Remove(id);
            s.enemy = null;   // Entri di antrean dibuang saat diproses
            spareAgents.Push(s);
        }

        private void Update()
        {
            var b = Bal;
            // Game dijeda: antrean tidak berjalan.
            if (b == null || Time.deltaTime <= 0f) return;
            int max = Mathf.Max(1, b.maxPathChecksPerFrame);
            int done = 0;
            float now = Time.time;
            while (done < max && pathCheckQueue.Count > 0)
            {
                var s = pathCheckQueue.Dequeue();
                if (s.enemy == null || !s.queued) continue;
                s.queued = false;
                if (!s.enemy.isActiveAndEnabled) continue;
                if (ProcessCheck(s, b, now)) done++;
            }
        }

        /// <summary>true kalau pemeriksaan memakai jatah frame.</summary>
        private bool ProcessCheck(AgentState s, BaseNavBalance b, float now)
        {
            var e = s.enemy;
            var agent = e.Agent;
            s.nextCheckTime = now + b.pathCheckIntervalSeconds;
            if (agent == null || !agent.enabled || !agent.isOnNavMesh) return false;

            if (s.attacking)
            {
                // Saat AttackBuilding: CalculatePath dengan sharedPath dan areaMask musuh. PathComplete = kembali Chase.
                NavMesh.CalculatePath(agent.nextPosition, s.attackTargetPos, agent.areaMask, sharedPath);
                if (sharedPath.status == NavMeshPathStatus.PathComplete) s.returnToChase = true;
                s.lastCheckTime = now;
                return true;
            }

            // 3. Jalur masih dihitung: lewati pemeriksaan ini.
            if (agent.pathPending) { s.lastCheckTime = now; return false; }
            float elapsed = now - s.lastCheckTime;
            s.lastCheckTime = now;
            // 4-5. Tanda terhalang dari status jalur, bukan kecepatan (keputusan teknis bagian 5).
            if (agent.pathStatus == NavMeshPathStatus.PathComplete) { s.blockedTimer = 0f; s.result = BlockedCheck.Clear; return true; }
            s.blockedTimer += elapsed;
            // 6.
            if (s.blockedTimer < b.blockedConfirmSeconds) return true;
            if (now < s.retargetUntil) return true;
            var res = AssignBreachTarget(e, out var t);
            if (res == BreachAssign.Assigned)
            {
                s.breach = t;
                s.hasSlot = true;
                s.invalidated = false;
                s.blockedTimer = 0f;
                s.result = BlockedCheck.Breach;
            }
            else if (res == BreachAssign.Waiting)
            {
                s.breach = t;
                s.result = BlockedCheck.Wait;
            }
            else
            {
                // 7. Tanpa struktur: EnemyAI mengikuti AITraversal.GetResponse (#94).
                s.blockedTimer = 0f;
                s.result = BlockedCheck.NoStructure;
            }
            return true;
        }

        // ---------------------------------------------------------------- Memilih dinding atau pintu

        public enum BreachAssign : byte { None, Assigned, Waiting }

        /// <summary>Nama dari blueprint. true hanya kalau posisi serangan didapat.</summary>
        public bool TryAssignBreachTarget(EnemyAI enemy, out BreachTarget target)
        {
            var r = AssignBreachTarget(enemy, out target);
            if (r == BreachAssign.Assigned)
            {
                var s = GetState(enemy, true);
                ReleaseSlotInternal(s);
                s.breach = target;
                s.hasSlot = true;
                s.invalidated = false;
            }
            return r == BreachAssign.Assigned;
        }

        public BreachAssign AssignBreachTarget(EnemyAI enemy, out BreachTarget target)
        {
            target = default;
            var b = Bal;
            if (enemy == null || b == null) return BreachAssign.None;
            var agent = enemy.Agent;
            // 1. Ujung jalur (pathEndPosition tanpa alokasi corners), atau posisi zombie kalau tidak ada jalur.
            Vector3 end = agent != null && agent.enabled && agent.hasPath ? agent.pathEndPosition : enemy.transform.position;
            int count = 0;

            // 2. Garis ke pemain/target: struktur pertama yang menghalangi menjadi kandidat pertama.
            Vector3 aim = enemy.CurrentTarget != null ? enemy.CurrentTarget.AimPoint
                        : PlayerTargetable.Instance != null ? PlayerTargetable.Instance.AimPoint : end;
            Vector3 from = end + Vector3.up;
            Vector3 dir = aim - from;
            float len = dir.magnitude;
            if (len > 0.01f && PhysicsQuery.Raycast(new Ray(from, dir / len), out RaycastHit hit, len, PhysicsMasks.BuildSurface))
                count = AddCandidate(hit.collider, end, count, -1f);

            // 3. Sekitar ujung jalur, urut terdekat.
            int n = PhysicsQuery.OverlapSphere(end, b.targetSearchRadius, PhysicsMasks.AIAttack, overlapBuffer);
            for (int i = 0; i < n && count < candidateIds.Length; i++)
            {
                var c = overlapBuffer[i];
                if (c == null) continue;
                count = AddCandidate(c, end, count, (c.ClosestPoint(end) - end).sqrMagnitude);
            }
            if (count == 0) return BreachAssign.None;

            // 4. Kandidat berurutan: yang pertama punya posisi kosong menang.
            int enemyId = enemy.GetInstanceID();
            for (int i = 0; i < count; i++)
            {
                if (!TryReserveSlot(candidateIds[i], candidateTargets[i], candidateColliders[i], candidateIsDoor[i], enemyId, end, agent, out int slot, out Vector3 pos))
                    continue;
                target = new BreachTarget { structureId = candidateIds[i], target = candidateTargets[i], slotPosition = pos, slotIndex = slot };
                ClearCandidates(count);
                return BreachAssign.Assigned;
            }

            // 5. Semua penuh: posisi tunggu pada waitRingRadius, sudut dari ID musuh.
            var first = candidateColliders[0];
            Vector3 c0 = first != null ? first.bounds.center : end;
            Vector3 away = end - c0; away.y = 0f;
            if (away.sqrMagnitude < 0.01f) away = Vector3.forward;
            float ang = ((enemyId & 1023) / 1023f - 0.5f) * 120f;
            Vector3 wait = c0 + Quaternion.Euler(0f, ang, 0f) * away.normalized * b.waitRingRadius;
            if (NavMesh.SamplePosition(wait, out var wh, 1.5f, agent != null ? agent.areaMask : NavMesh.AllAreas)) wait = wh.position;
            target = new BreachTarget { structureId = candidateIds[0], target = candidateTargets[0], slotPosition = wait, slotIndex = -1 };
            ClearCandidates(count);
            return BreachAssign.Waiting;
        }

        /// <summary>sqrDist -1 = kandidat garis pandang (selalu pertama).</summary>
        private int AddCandidate(Collider c, Vector3 end, int count, float sqrDist)
        {
            // Claude.md Pilar 3: zombie menyerang objek bertag BaseWall (dan BaseDoor, GLOSSARY.md).
            bool isDoor = c.CompareTag("BaseDoor");
            if (!isDoor && !c.CompareTag("BaseWall")) return count;
            var node = c.GetComponentInParent<StructureNode>();
            var dmg = c.GetComponentInParent<IDamageable>();
            if (node == null || dmg == null || !dmg.IsAlive || string.IsNullOrEmpty(node.StructureId)) return count;
            string id = node.StructureId;
            for (int i = 0; i < count; i++) if (candidateIds[i] == id) return count;
            if (count >= candidateIds.Length) return count;
            // Sisip berurutan (kandidat sedikit, tanpa alokasi).
            int at = count;
            while (at > 0 && candidateDist[at - 1] > sqrDist)
            {
                candidateIds[at] = candidateIds[at - 1];
                candidateTargets[at] = candidateTargets[at - 1];
                candidateColliders[at] = candidateColliders[at - 1];
                candidateDist[at] = candidateDist[at - 1];
                candidateIsDoor[at] = candidateIsDoor[at - 1];
                at--;
            }
            candidateIds[at] = id;
            candidateTargets[at] = dmg;
            candidateColliders[at] = c;
            candidateDist[at] = sqrDist;
            candidateIsDoor[at] = isDoor;
            return count + 1;
        }

        private void ClearCandidates(int count)
        {
            for (int i = 0; i < count; i++) { candidateTargets[i] = null; candidateColliders[i] = null; candidateIds[i] = null; }
        }

        // ---------------------------------------------------------------- Membagi posisi serangan

        private BreachSlots GetOrCreateSlots(string id, IDamageable target, Collider col, bool isDoor)
        {
            if (slotsByStructure.TryGetValue(id, out var s)) return s;
            var b = Bal;
            int perSide = isDoor ? b.maxAttackersPerDoor : b.maxAttackersPerWall;
            perSide = Mathf.Max(1, perSide);
            s = spareSlots.Count > 0 ? spareSlots.Pop() : new BreachSlots();
            int total = perSide * 2;
            if (s.outerPositions == null || s.outerPositions.Length != total)
            {
                s.outerPositions = new Vector3[total];
                s.occupantIds = new int[total];
            }
            for (int i = 0; i < total; i++) s.occupantIds[i] = -1;
            s.attackerCount = 0;
            s.maxAttackers = perSide;
            s.target = target;

            // Sumbu panjang struktur dan normalnya dari bounds collider dan transform.
            var tr = col.transform;
            Bounds bounds = col.bounds;
            Vector3 ext = bounds.extents;
            Vector3 right = tr.right; right.y = 0f; right.Normalize();
            Vector3 fwd = tr.forward; fwd.y = 0f; fwd.Normalize();
            float alongRight = Mathf.Abs(ext.x * right.x) + Mathf.Abs(ext.z * right.z);
            float alongFwd = Mathf.Abs(ext.x * fwd.x) + Mathf.Abs(ext.z * fwd.z);
            Vector3 axis = alongRight >= alongFwd ? right : fwd;
            Vector3 normal = alongRight >= alongFwd ? fwd : right;
            float halfThick = Mathf.Min(alongRight, alongFwd);
            s.center = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
            s.normal = normal;
            float standoff = halfThick + b.slotStandoff;
            for (int side = 0; side < 2; side++)
            {
                float sign = side == 0 ? 1f : -1f;
                for (int k = 0; k < perSide; k++)
                {
                    float offset = (k - (perSide - 1) * 0.5f) * b.slotSpacing;
                    s.outerPositions[side * perSide + k] = s.center + normal * (standoff * sign) + axis * offset;
                }
            }
            slotsByStructure[id] = s;
            return s;
        }

        private bool TryReserveSlot(string id, IDamageable target, Collider col, bool isDoor, int enemyId, Vector3 from,
                                    NavMeshAgent agent, out int slot, out Vector3 pos)
        {
            slot = -1;
            pos = default;
            var s = GetOrCreateSlots(id, target, col, isDoor);
            // 1.
            if (s.attackerCount >= s.maxAttackers) return false;
            // 2. Hanya sisi yang menghadap from, supaya zombie di luar tidak berebut titik di dalam base.
            int side = Vector3.Dot(from - s.center, s.normal) >= 0f ? 0 : 1;
            int perSide = s.maxAttackers;
            int mask = agent != null ? agent.areaMask : NavMesh.AllAreas;
            // 3-4. Posisi kosong terdekat yang sah di NavMesh.
            for (int attempt = 0; attempt < perSide; attempt++)
            {
                int best = -1;
                float bestD = float.MaxValue;
                for (int k = 0; k < perSide; k++)
                {
                    int i = side * perSide + k;
                    if (s.occupantIds[i] != -1) continue;
                    float d = (s.outerPositions[i] - from).sqrMagnitude;
                    if (d < bestD) { bestD = d; best = i; }
                }
                if (best < 0) return false;
                if (NavMesh.SamplePosition(s.outerPositions[best], out var hit, 1f, mask))
                {
                    // 5.
                    s.occupantIds[best] = enemyId;
                    s.attackerCount++;
                    slot = best;
                    pos = hit.position;
                    return true;
                }
                s.occupantIds[best] = -2;   // Tidak sah, dicoba titik lain pada putaran ini
            }
            for (int i = 0; i < s.occupantIds.Length; i++) if (s.occupantIds[i] == -2) s.occupantIds[i] = -1;
            return false;
        }

        private void ReleaseSlotInternal(AgentState st)
        {
            if (!st.hasSlot) return;
            st.hasSlot = false;
            var id = st.breach.structureId;
            int slot = st.breach.slotIndex;
            st.breach = default;
            if (string.IsNullOrEmpty(id) || !slotsByStructure.TryGetValue(id, out var s)) return;
            if (slot >= 0 && slot < s.occupantIds.Length && s.occupantIds[slot] == st.id)
            {
                s.occupantIds[slot] = -1;
                s.attackerCount = Mathf.Max(0, s.attackerCount - 1);
            }
        }

        // ---------------------------------------------------------------- Kembali ke Chase

        private void HandleStructureDestroyed(StructureDestroyedEvent e)
        {
            anchorDirty = true;
            if (string.IsNullOrEmpty(e.structureId) || !slotsByStructure.TryGetValue(e.structureId, out var s)) return;
            // Pemetaan struktur ke occupantIds: tidak perlu mencari musuh satu per satu.
            InvalidateOccupants(s);
            slotsByStructure.Remove(e.structureId);
            s.target = null;
            spareSlots.Push(s);
        }

        private readonly List<string> regionSlotKeys = new List<string>(16);

        /// <summary>Region dilepas (#88): posisi serangan struktur di region itu dikosongkan, penyerangnya kembali Chase.</summary>
        private void HandleRegionLoadState(RegionLoadStateChangedEvent e)
        {
            if (e.state != RegionLoadState.Unloading) return;
            anchorDirty = true;
            regionSlotKeys.Clear();
            foreach (var kv in slotsByStructure)
                if (kv.Value != null && RegionStreaming.RegionOf(kv.Value.center) == e.region) regionSlotKeys.Add(kv.Key);
            for (int i = 0; i < regionSlotKeys.Count; i++)
            {
                var s = slotsByStructure[regionSlotKeys[i]];
                InvalidateOccupants(s);
                slotsByStructure.Remove(regionSlotKeys[i]);
                s.target = null;
                spareSlots.Push(s);
            }
            regionSlotKeys.Clear();
        }

        private void HandleDoorState(DoorStateChangedEvent e)
        {
            // Pintu terbuka: penyerangnya kembali Chase dan masuk lewat lubang pintu.
            if (e.state != DoorState.Open || string.IsNullOrEmpty(e.structureId)) return;
            if (slotsByStructure.TryGetValue(e.structureId, out var s)) InvalidateOccupants(s);
        }

        private void InvalidateOccupants(BreachSlots s)
        {
            for (int i = 0; i < s.occupantIds.Length; i++)
            {
                int occ = s.occupantIds[i];
                s.occupantIds[i] = -1;
                if (occ < 0 || !agents.TryGetValue(occ, out var st)) continue;
                st.hasSlot = false;
                st.breach = default;
                st.invalidated = true;
                st.blockedTimer = 0f;
            }
            s.attackerCount = 0;
        }

        private void HandleStructureBuilt(StructureBuiltEvent e)
        {
            anchorDirty = true;
            if (StructureRegistry.Instance != null && StructureRegistry.Instance.TryGetById(e.structureId, out var node)) ConfigureObstacle(node);
        }

        private void HandleStructureUpgraded(StructureUpgradedEvent e)
        {
            anchorDirty = true;
            // structureId tetap, jadi BreachSlots tetap sah (#47). Target IDamageable diganti objek baru.
            if (StructureRegistry.Instance == null || !StructureRegistry.Instance.TryGetById(e.structureId, out var node)) return;
            ConfigureObstacle(node);
            if (slotsByStructure.TryGetValue(e.structureId, out var s)) s.target = node.GetComponent<IDamageable>();
        }

        // ---------------------------------------------------------------- Obstacle per struktur

        /// <summary>Aturan obstacle per struktur (BuildableData.navMode). Carving hanya pada level 0.</summary>
        public static void ConfigureObstacle(StructureNode node)
        {
            if (node == null || node.Data == null) return;
            var obstacles = node.GetComponentsInChildren<NavMeshObstacle>(true);
            var door = node.GetComponent<BaseDoor>();
            for (int i = 0; i < obstacles.Length; i++)
            {
                var o = obstacles[i];
                // Daun pintu diatur BaseDoor sesuai status.
                if (door != null && door.LeafObstacle == o) continue;
                switch (node.Data.navMode)
                {
                    case NavObstacleMode.Carve:
                        o.enabled = true;
                        o.carving = node.Level == 0;   // Lantai atas tidak menyentuh NavMesh di tanah
                        o.carveOnlyStationary = false;  // Upgrade langsung menutup jalur pada frame yang sama
                        break;
                    case NavObstacleMode.Avoid:
                        o.enabled = true;
                        o.carving = false;
                        break;
                    default:
                        o.enabled = false;
                        break;
                }
            }
        }

        // ---------------------------------------------------------------- BaseAnchor

        private void RescanStructures()
        {
            var reg = StructureRegistry.Instance;
            int regCount = reg != null ? reg.Count : 0;
            if (!anchorDirty && regCount == scannedCount) return;
            anchorDirty = false;
            scannedCount = regCount;
            hasAnchor = false;
            carvingCount = 0;
            if (regCount == 0) return;
            // Jarang: hanya saat jumlah struktur berubah.
            var nodes = FindObjectsByType<StructureNode>(FindObjectsSortMode.None);
            Vector3 sum = Vector3.zero;
            int n = 0;
            for (int i = 0; i < nodes.Length; i++)
            {
                var node = nodes[i];
                if (node == null || !node.isActiveAndEnabled || string.IsNullOrEmpty(node.StructureId)) continue;
                ConfigureObstacle(node);
                if (node.Data != null && node.Data.navMode == NavObstacleMode.Carve && node.Level == 0) carvingCount++;
                sum += node.transform.position;
                n++;
            }
            if (n == 0) return;
            // Sementara (usulan 7 menunggu developer): pusat base = rata-rata posisi struktur.
            anchorPosition = sum / n;
            if (NavMesh.SamplePosition(anchorPosition, out var hit, 4f, NavMesh.AllAreas)) anchorPosition = hit.position;
            hasAnchor = true;
            baseAnchor.transform.position = anchorPosition;
        }

        /// <summary>false kalau belum ada struktur. Dipakai #50, #76, dan #108.</summary>
        public bool TryGetBaseAnchor(out Vector3 anchor)
        {
            RescanStructures();
            anchor = anchorPosition;
            return hasAnchor;
        }

        /// <summary>
        /// Syarat 2-4 BaseAnchor.IsTargetable dan pusat serbuan HordeManager: base ada, region anchor dimuat,
        /// dan pemain dalam bloodMoon.baseEngageDistance.
        /// </summary>
        public bool IsAnchorEngaged(out Vector3 anchor)
        {
            if (!TryGetBaseAnchor(out anchor)) return false;
            // Region anchor harus dimuat (#88). Tanpa RegionStreaming selalu dianggap dimuat.
            if (!RegionStreaming.IsRegionLoaded(RegionStreaming.RegionOf(anchor))) return false;
            var bal = GameManager.CurrentBalance;
            var player = PlayerTargetable.Instance;
            if (bal == null || player == null) return false;
            float max = bal.bloodMoon.baseEngageDistance;
            return (player.Position - anchor).sqrMagnitude <= max * max;
        }

        // ---------------------------------------------------------------- Pemain di dalam base

        /// <summary>Usulan 4: minimal insideMinWallHits dari empat arah terhalang dinding atau pintu. Atap tidak wajib.</summary>
        public bool IsInsideBase(Vector3 position)
        {
            var b = Bal;
            if (b == null) return false;
            Vector3 origin = position + Vector3.up;
            int hits = 0;
            for (int i = 0; i < FourDirections.Length; i++)
            {
                if (!PhysicsQuery.Raycast(new Ray(origin, FourDirections[i]), out RaycastHit hit, b.insideRayDistance, PhysicsMasks.BuildSurface)) continue;
                var c = hit.collider;
                if (c.CompareTag("BaseWall") || c.CompareTag("BaseDoor")) hits++;
            }
            return hits >= b.insideMinWallHits;
        }

        private IEnumerator InsideLoop()
        {
            // Waktu berskala: berhenti saat game dijeda.
            var wait = new WaitForSeconds(0.5f);
            float lastInterval = 0.5f;
            if (EventChannel<PlayerBaseStateChangedEvent>.HasLast) IsPlayerInsideBase = EventChannel<PlayerBaseStateChangedEvent>.Last.isInside;
            while (true)
            {
                var b = Bal;
                float interval = b != null ? b.insideCheckSeconds : 0.5f;
                if (!Mathf.Approximately(interval, lastInterval)) { wait = new WaitForSeconds(interval); lastInterval = interval; }
                yield return wait;
                var player = PlayerTargetable.Instance;
                if (player == null) continue;
                bool inside = IsInsideBase(player.Position);
                if (inside == IsPlayerInsideBase) continue;
                IsPlayerInsideBase = inside;
                EventChannel<PlayerBaseStateChangedEvent>.Publish(new PlayerBaseStateChangedEvent(inside));
            }
        }

        // ---------------------------------------------------------------- DevConsole (#56)

        /// <summary>navstat: jumlah obstacle carving dan zombie yang menunggu pemeriksaan jalur.</summary>
        public string DebugNavStat()
        {
            anchorDirty = true;
            RescanStructures();
            return $"carving={carvingCount} antreanJalur={pathCheckQueue.Count} zombieTercatat={agents.Count} strukturDiserang={slotsByStructure.Count}";
        }
    }
}
