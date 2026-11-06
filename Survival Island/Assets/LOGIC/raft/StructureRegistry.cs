using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Satu struktur di save world_structures (BuildMenu.md "Simpan dan muat").</summary>
    [Serializable]
    public class StructureRecord
    {
        public string structureId;
        public string buildableId;
        public Vector3 position;
        public float yaw;
        public float currentHP;
        public string extraJson;   // Dari ISaveable di prefab: isi peti (#46), status pintu (#78)
        // Tambahan Langkah 4: kunci grid tidak bisa dihitung ulang dari posisi saja (tingkat dan sisi WallMount).
        public GridKey key;
        public bool grounded;
    }

    [Serializable]
    public class StructureSaveData
    {
        public StructureRecord[] records;
        public int nextStructureNumber = 1;
    }

    /// <summary>
    /// Daftar struktur, grid slot, grafik dukungan, runtuhan beruntun, dan bongkar. Manager di IslandPhase, ISaveable
    /// world_structures (LoadOrder 90). Blueprint: raft/BuildMenu.md (#79).
    ///
    /// Grafik dukungan: hubungan vertikal biaya 0, horizontal biaya 1 (lantai-lantai, atap-atap). Struktur tegak kalau
    /// ada jalur ke struktur IsGrounded dengan biaya paling banyak maxHorizontalSpan. Runtuhan diproses lewat antrean,
    /// paling banyak maxCollapsePerFrame per frame, tanpa rekursi.
    ///
    /// Catatan implementasi (Langkah 4):
    /// - Struktur ukuran lebih dari satu sel (gridSize > 1) belum didukung; semua struktur awal berukuran 1 sel.
    /// - Isi peti yang ikut keluar saat bongkar (#46) dan RemoteBaseSimulation (#90) belum ada (TODO).
    /// - Durabilitas palu saat bongkar menunggu DurabilitySystem (#113, TODO).
    /// </summary>
    [DisallowMultipleComponent]
    public class StructureRegistry : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "world_structures";
        public const string CollapseVfxPoolTag = "FX_CollapseDebris";

        public static StructureRegistry Instance { get; private set; }

        private readonly Dictionary<GridKey, StructureNode> grid = new Dictionary<GridKey, StructureNode>(256);
        private readonly Dictionary<string, StructureNode> byId = new Dictionary<string, StructureNode>(256, StringComparer.Ordinal);
        private int nextStructureNumber = 1;

        // Buffer yang dipakai ulang supaya BFS tanpa garbage.
        private readonly List<StructureNode> dirtySeeds = new List<StructureNode>(16);
        private readonly List<StructureNode> collapseQueue = new List<StructureNode>(32);
        private readonly HashSet<StructureNode> collapseSet = new HashSet<StructureNode>();
        private readonly HashSet<StructureNode> visited = new HashSet<StructureNode>();
        private readonly List<StructureNode> component = new List<StructureNode>(64);
        private readonly Queue<StructureNode> bfsQueue = new Queue<StructureNode>(64);
        private readonly LinkedList<StructureNode> deque = new LinkedList<StructureNode>();
        private readonly Dictionary<StructureNode, int> dist = new Dictionary<StructureNode, int>(64);
        private readonly NeighborBuffer neighbors = new NeighborBuffer();
        private readonly NeighborBuffer neighbors2 = new NeighborBuffer();
        private static readonly Comparison<StructureNode> HigherLevelFirst = (a, b) => b.Level.CompareTo(a.Level);

        private int vfxThisSecond;
        private float vfxSecondStart;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;
        private int MaxSpan => Balance != null ? Balance.build.maxHorizontalSpan : 2;
        private float HeightSnap => Balance != null ? Balance.build.heightSnap : 0.25f;

        public int Count => byId.Count;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Register(this);
            EventChannel<StructureDestroyedEvent>.Raised += OnStructureRemoved;
        }

        private void OnDisable()
        {
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            EventChannel<StructureDestroyedEvent>.Raised -= OnStructureRemoved;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Daftar dan grid

        /// <summary>ID berbentuk s_000123, tidak pernah dipakai ulang dalam satu run.</summary>
        public string NewStructureId() => "s_" + (nextStructureNumber++).ToString("D6");

        public bool IsOccupied(GridKey key) => grid.TryGetValue(key, out var n) && n != null;

        public bool TryGet(GridKey key, out StructureNode node) => grid.TryGetValue(key, out node) && node != null;

        public bool TryGetById(string id, out StructureNode node) => byId.TryGetValue(id ?? string.Empty, out node) && node != null;

        /// <summary>Mengisi grid dan menghitung jarak dukungan awal.</summary>
        public void Register(StructureNode node)
        {
            if (node == null || string.IsNullOrEmpty(node.StructureId)) return;
            byId[node.StructureId] = node;
            grid[node.Key] = node;
            node.SupportDistance = node.IsGrounded ? 0 : ComputeDistanceFromNeighbors(node.Key, node.BaseY, null);
        }

        private void Unregister(StructureNode node)
        {
            if (node == null) return;
            if (grid.TryGetValue(node.Key, out var n) && n == node) grid.Remove(node.Key);
            if (!string.IsNullOrEmpty(node.StructureId)) byId.Remove(node.StructureId);
        }

        // ---------------------------------------------------------------- Tetangga

        private sealed class NeighborBuffer
        {
            public readonly StructureNode[] nodes = new StructureNode[16];
            public readonly int[] costs = new int[16];
            public int count;
            public void Clear() => count = 0;
            public void Add(StructureNode n, int c)
            {
                if (count >= nodes.Length) return;
                nodes[count] = n;
                costs[count] = c;
                count++;
            }
        }

        private void TryAdd(NeighborBuffer buf, GridKey k, int cost, float baseY, bool checkHeight)
        {
            if (!grid.TryGetValue(k, out var n) || n == null) return;
            // Dua lantai pada tinggi berbeda tidak saling menopang (BuildMenu.md "Kasus Khusus").
            if (checkHeight && Mathf.Abs(n.BaseY - baseY) >= HeightSnap) return;
            buf.Add(n, cost);
        }

        /// <summary>Tetangga sebuah kunci, dihitung dari grid (tidak disimpan).</summary>
        private void CollectNeighbors(GridKey k, float baseY, NeighborBuffer buf)
        {
            buf.Clear();
            int L = k.level;
            switch (k.layer)
            {
                case GridLayer.Floor:
                    for (int d = 0; d < 4; d++)
                    {
                        Dir(d, out int dx, out int dz);
                        TryAdd(buf, new GridKey(k.ix + dx, k.iz + dz, L, GridLayer.Edge), 0, baseY, false);      // Dinding di atas lantai
                        TryAdd(buf, new GridKey(k.ix + dx, k.iz + dz, L - 1, GridLayer.Edge), 0, baseY, false);  // Lantai di atas dinding
                        TryAdd(buf, new GridKey(k.ix + dx * 2, k.iz + dz * 2, L, GridLayer.Floor), 1, baseY, true); // Lantai bersebelahan
                    }
                    TryAdd(buf, k.WithLayer(GridLayer.Object), 0, baseY, false);
                    break;

                case GridLayer.Object:
                    TryAdd(buf, k.WithLayer(GridLayer.Floor), 0, baseY, false);
                    break;

                case GridLayer.Edge:
                {
                    bool xEdge = (k.ix & 1) != 0;   // Sisi timur/barat: sel di kiri dan kanan
                    int ax = xEdge ? 1 : 0, az = xEdge ? 0 : 1;
                    TryAdd(buf, new GridKey(k.ix - ax, k.iz - az, L, GridLayer.Floor), 0, baseY, false);
                    TryAdd(buf, new GridKey(k.ix + ax, k.iz + az, L, GridLayer.Floor), 0, baseY, false);
                    TryAdd(buf, new GridKey(k.ix - ax, k.iz - az, L + 1, GridLayer.Floor), 0, baseY, false);
                    TryAdd(buf, new GridKey(k.ix + ax, k.iz + az, L + 1, GridLayer.Floor), 0, baseY, false);
                    TryAdd(buf, k.WithLevel(L - 1), 0, baseY, false);   // Dinding di atas dinding
                    TryAdd(buf, k.WithLevel(L + 1), 0, baseY, false);
                    TryAdd(buf, k.WithLayer(GridLayer.MountA), 0, baseY, false);
                    TryAdd(buf, k.WithLayer(GridLayer.MountB), 0, baseY, false);
                    break;
                }

                default: // WallMount
                    TryAdd(buf, k.WithLayer(GridLayer.Edge), 0, baseY, false);
                    break;
            }
        }

        private static void Dir(int d, out int dx, out int dz)
        {
            switch (d)
            {
                case 0: dx = 1; dz = 0; break;
                case 1: dx = -1; dz = 0; break;
                case 2: dx = 0; dz = 1; break;
                default: dx = 0; dz = -1; break;
            }
        }

        private int ComputeDistanceFromNeighbors(GridKey key, float baseY, StructureNode exclude)
        {
            CollectNeighbors(key, baseY, neighbors2);
            int best = int.MaxValue;
            for (int i = 0; i < neighbors2.count; i++)
            {
                var n = neighbors2.nodes[i];
                if (n == exclude || n.SupportDistance == int.MaxValue) continue;
                int d = n.SupportDistance + neighbors2.costs[i];
                if (d < best) best = d;
            }
            return best;
        }

        // ---------------------------------------------------------------- Syarat dukungan saat dibangun

        /// <summary>
        /// Baris 11 tabel validasi. Aturan SupportRule menentukan syarat saat dibangun, dan jarak dukungan tetap harus
        /// paling banyak maxHorizontalSpan.
        /// </summary>
        public bool HasSupport(BuildableData data, GridKey key, float baseY, bool grounded)
        {
            if (data == null) return false;
            int span = MaxSpan;
            CollectNeighbors(key, baseY, neighbors);

            bool ruleOk = false;
            int best = grounded ? 0 : int.MaxValue;
            for (int i = 0; i < neighbors.count; i++)
            {
                var n = neighbors.nodes[i];
                int c = neighbors.costs[i];
                if (n.SupportDistance != int.MaxValue && n.SupportDistance + c < best) best = n.SupportDistance + c;
                if (n.SupportDistance == int.MaxValue || n.SupportDistance + c > span) continue;

                var nk = n.Key;
                switch (data.slotKind)
                {
                    case BuildSlotKind.WallMount:
                        // Obor dinding menempel pada dinding di sisi yang sama.
                        ruleOk |= nk.layer == GridLayer.Edge;
                        break;
                    case BuildSlotKind.Edge:
                        // Dinding: lantai di sel sebelah pada tingkat sama, atau dinding di bawahnya.
                        ruleOk |= (nk.layer == GridLayer.Floor && nk.level == key.level)
                                  || (nk.layer == GridLayer.Edge && nk.level == key.level - 1);
                        break;
                    default:
                        switch (data.support)
                        {
                            case SupportRule.FloorOrGround:
                                ruleOk |= nk.layer == GridLayer.Floor && nk.level == key.level;
                                break;
                            case SupportRule.GroundOrWall:
                                ruleOk |= (nk.layer == GridLayer.Edge && nk.level == key.level - 1)
                                          || (nk.layer == GridLayer.Floor && nk.level == key.level);
                                break;
                            case SupportRule.WallBelow:
                                // Atap menempel pada dinding di bawah sisinya, atau bersambung ke atap yang tegak.
                                ruleOk |= (nk.layer == GridLayer.Edge && nk.level == key.level - 1)
                                          || (nk.layer == GridLayer.Floor && nk.level == key.level && n.Data != null && n.Data.support == SupportRule.WallBelow);
                                break;
                        }
                        break;
                }
            }

            // Struktur menapak tanah tingkat 0 selalu boleh, kecuali atap dan WallMount yang selalu butuh penopang.
            bool groundOk = grounded && data.support != SupportRule.WallBelow && data.slotKind != BuildSlotKind.WallMount;
            return (groundOk || ruleOk) && best <= span;
        }

        // ---------------------------------------------------------------- Bidikan

        /// <summary>Struktur yang dibidik kamera, lewat PhysicsMasks.BuildSurface. Hasil Terrain dianggap tanpa target.</summary>
        public bool TryGetAimedStructure(out StructureNode node)
        {
            node = null;
            var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : null;
            if (cam == null) return false;
            float range = Balance != null ? Balance.build.maxBuildDistance : 6f;
            var ray = new Ray(cam.transform.position, cam.transform.forward);
            if (!PhysicsQuery.Raycast(ray, out RaycastHit hit, range, PhysicsMasks.BuildSurface)) return false;
            if (hit.collider is TerrainCollider) return false;
            node = hit.collider.GetComponentInParent<StructureNode>();
            return node != null;
        }

        // ---------------------------------------------------------------- Bongkar

        /// <summary>
        /// Bongkar setelah klik kiri ditahan demolishHoldSeconds. Usulan disetujui: kembali 50 persen dikali rasio HP,
        /// dibulatkan ke bawah, dan ditolak selama 5 detik setelah struktur diserang.
        /// </summary>
        public bool TryDemolish(StructureNode node)
        {
            if (node == null || node.Data == null || node.Health == null) return false;
            var data = node.Data;
            var health = node.Health;
            float blocked = Balance != null ? Balance.build.demolishBlockedAfterDamageSeconds : 5f;
            // 1.
            if (!data.isDemolishable) return false;
            if (health.LastDamageTime > 0f && Time.time - health.LastDamageTime < blocked) return false;

            // 2-3. Pengembalian bahan. Sisa yang tidak muat jatuh di depan pemain.
            float ratio = Balance != null ? Balance.build.demolishRefundRatio : 0.5f;
            var inv = InventoryManager.Instance;
            if (data.cost != null && inv != null)
            {
                for (int i = 0; i < data.cost.Length; i++)
                {
                    int amount = Mathf.FloorToInt(data.cost[i].amount * ratio * health.HpRatio);
                    var item = ItemDatabase.Get(data.cost[i].itemId);
                    if (amount <= 0 || item == null) continue;
                    int left = inv.AddItem(item, amount);
                    if (left > 0 && WorldItemManager.Instance != null)
                    {
                        var player = IslandPlayerController.Instance;
                        Vector3 pos = player != null ? player.transform.position + player.transform.forward + Vector3.up : node.transform.position;
                        WorldItemManager.Instance.Spawn(inv.CreateInstance(item), left, pos, true);
                    }
                }
            }
            // 4. TODO #46: isi peti ikut keluar.
            // 5. BaseHealth.Remove menerbitkan StructureDestroyedEvent, lalu OnStructureRemoved memeriksa tetangga.
            health.Remove();
            return true;
        }

        /// <summary>Jumlah struktur yang akan ikut runtuh kalau node ini hilang (ditampilkan HUD sebelum bongkar).</summary>
        public int PreviewCollapse(StructureNode node)
        {
            if (node == null) return 0;
            CollectNeighbors(node.Key, node.BaseY, neighbors);
            int total = 0;
            visited.Clear();
            for (int i = 0; i < neighbors.count; i++)
            {
                var seed = neighbors.nodes[i];
                if (visited.Contains(seed)) continue;
                total += EvaluateComponent(seed, node, false);
            }
            return total;
        }

        // ---------------------------------------------------------------- Runtuhan

        private void OnStructureRemoved(StructureDestroyedEvent e)
        {
            if (!byId.TryGetValue(e.structureId ?? string.Empty, out var node)) return;
            Unregister(node);
            collapseSet.Remove(node);
            // Tetangga menjadi benih. Serangan, bencana, dan bongkar memakai jalur yang sama.
            CollectNeighbors(node.Key, node.BaseY, neighbors);
            for (int i = 0; i < neighbors.count; i++)
                if (!dirtySeeds.Contains(neighbors.nodes[i])) dirtySeeds.Add(neighbors.nodes[i]);
        }

        private void LateUpdate()
        {
            // Game dijeda: runtuhan yang antre berhenti, lalu lanjut setelah jeda.
            if (Time.timeScale <= 0f) return;

            if (dirtySeeds.Count > 0)
            {
                var seed = dirtySeeds[dirtySeeds.Count - 1];
                dirtySeeds.RemoveAt(dirtySeeds.Count - 1);
                if (seed != null && byId.ContainsKey(seed.StructureId))
                {
                    visited.Clear();
                    EvaluateComponent(seed, null, true);
                    // Benih lain yang sudah tercakup komponen ini dihapus.
                    for (int i = dirtySeeds.Count - 1; i >= 0; i--)
                        if (dirtySeeds[i] == null || visited.Contains(dirtySeeds[i])) dirtySeeds.RemoveAt(i);
                }
            }

            int max = Balance != null ? Mathf.Max(1, Balance.build.maxCollapsePerFrame) : 4;
            for (int n = 0; n < max && collapseQueue.Count > 0; n++)
            {
                var node = collapseQueue[0];
                collapseQueue.RemoveAt(0);
                collapseSet.Remove(node);
                if (node == null || !byId.ContainsKey(node.StructureId)) continue;
                Collapse(node);
            }
        }

        /// <summary>
        /// BFS satu komponen dari seed, lalu BFS 0-1 dari node IsGrounded. Mengembalikan jumlah node yang tidak tegak.
        /// apply = true: jarak disimpan dan node tidak tegak masuk collapseQueue (tingkat tertinggi lebih dulu).
        /// </summary>
        private int EvaluateComponent(StructureNode seed, StructureNode exclude, bool apply)
        {
            component.Clear();
            bfsQueue.Clear();
            if (seed == null || seed == exclude) return 0;
            visited.Add(seed);
            bfsQueue.Enqueue(seed);
            while (bfsQueue.Count > 0)
            {
                var cur = bfsQueue.Dequeue();
                component.Add(cur);
                CollectNeighbors(cur.Key, cur.BaseY, neighbors2);
                for (int i = 0; i < neighbors2.count; i++)
                {
                    var nb = neighbors2.nodes[i];
                    if (nb == exclude || visited.Contains(nb)) continue;
                    visited.Add(nb);
                    bfsQueue.Enqueue(nb);
                }
            }

            dist.Clear();
            deque.Clear();
            for (int i = 0; i < component.Count; i++)
            {
                if (!component[i].IsGrounded) continue;
                dist[component[i]] = 0;
                deque.AddLast(component[i]);
            }
            while (deque.Count > 0)
            {
                var cur = deque.First.Value;
                deque.RemoveFirst();
                int dc = dist[cur];
                CollectNeighbors(cur.Key, cur.BaseY, neighbors2);
                for (int i = 0; i < neighbors2.count; i++)
                {
                    var nb = neighbors2.nodes[i];
                    if (nb == exclude) continue;
                    int nd = dc + neighbors2.costs[i];
                    if (dist.TryGetValue(nb, out int old) && old <= nd) continue;
                    dist[nb] = nd;
                    if (neighbors2.costs[i] == 0) deque.AddFirst(nb); else deque.AddLast(nb);
                }
            }

            int span = MaxSpan;
            int unsupported = 0;
            for (int i = 0; i < component.Count; i++)
            {
                var node = component[i];
                int d = dist.TryGetValue(node, out int v) ? v : int.MaxValue;
                bool upright = d <= span;
                if (!upright) unsupported++;
                if (!apply) continue;
                node.SupportDistance = d;
                if (!upright && collapseSet.Add(node)) collapseQueue.Add(node);
            }
            if (apply && unsupported > 0) collapseQueue.Sort(HigherLevelFirst);
            return unsupported;
        }

        private void Collapse(StructureNode node)
        {
            PlayCollapseVfx(node.transform.position);
            var damageable = node.GetComponent<IDamageable>();
            if (damageable != null && node.Health != null)
            {
                var info = new DamageInfo
                {
                    amount = node.Health.CurrentHP + 1f,
                    type = DamageType.Structural,
                    hitPoint = node.transform.position,
                    hitDirection = Vector3.down,
                    bodyPart = BodyPart.Torso,
                };
                damageable.TakeDamage(in info);
                return;
            }

            // Cadangan kalau prefab belum punya BaseHealth: lepas dari grid lewat jalur event yang sama, lalu hapus.
            Debug.LogWarning($"[StructureRegistry] '{node.name}' tanpa BaseHealth. Dihapus langsung.");
            EventChannel<StructureDestroyedEvent>.Publish(new StructureDestroyedEvent(node.StructureId, node.transform.position));
            Destroy(node.gameObject);
        }

        private void PlayCollapseVfx(Vector3 pos)
        {
            if (Time.time - vfxSecondStart >= 1f) { vfxSecondStart = Time.time; vfxThisSecond = 0; }
            int max = Balance != null ? Balance.build.maxCollapseVfxPerSecond : 8;
            // Melewati batas: struktur tetap hilang tanpa efek.
            if (vfxThisSecond >= max) return;
            var pooler = ObjectPooler.Instance;
            if (pooler == null || !pooler.HasPool(CollapseVfxPoolTag)) return;
            vfxThisSecond++;
            pooler.SpawnFromPool(CollapseVfxPoolTag, pos, Quaternion.identity);
        }

        // ---------------------------------------------------------------- Simpan dan muat (#80)

        public string SaveId => SaveIdConst;
        public int LoadOrder => 90;

        public string CaptureState()
        {
            var records = new List<StructureRecord>(byId.Count);
            foreach (var kv in byId)
            {
                var node = kv.Value;
                if (node == null || node.Data == null) continue;
                var extra = FindPrefabSaveable(node);
                records.Add(new StructureRecord
                {
                    structureId = node.StructureId,
                    buildableId = node.Data.id,
                    position = node.transform.position,
                    yaw = node.transform.eulerAngles.y,
                    currentHP = node.Health != null ? node.Health.CurrentHP : node.Data.maxHP,
                    extraJson = extra != null ? extra.CaptureState() : string.Empty,
                    key = node.Key,
                    grounded = node.IsGrounded,
                });
            }
            return JsonUtility.ToJson(new StructureSaveData { records = records.ToArray(), nextStructureNumber = nextStructureNumber });
        }

        public void RestoreState(string json)
        {
            StructureSaveData data = null;
            try { data = JsonUtility.FromJson<StructureSaveData>(json); }
            catch (Exception e) { Debug.LogError($"[StructureRegistry] Save tidak terbaca: {e.Message}"); }
            if (data == null) return;
            nextStructureNumber = Mathf.Max(1, data.nextStructureNumber);
            if (data.records == null) return;

            // 1. Tingkat rendah ke tinggi.
            Array.Sort(data.records, (a, b) => a.key.level.CompareTo(b.key.level));
            var builder = BuildManager.Instance;
            for (int i = 0; i < data.records.Length; i++)
            {
                var r = data.records[i];
                var bd = builder != null ? builder.FindBuildable(r.buildableId) : null;
                if (bd == null || bd.prefab == null)
                {
                    Debug.LogWarning($"[StructureRegistry] buildableId '{r.buildableId}' tidak ditemukan, struktur {r.structureId} dilewati.");
                    continue;
                }
                // 2. Tanpa validasi dan tanpa mengurangi bahan.
                var rot = Quaternion.Euler(0f, r.yaw, 0f);
                var go = Instantiate(bd.prefab, r.position, rot, builder.GetStructureRoot(r.position));
                var node = go.GetComponent<StructureNode>();
                if (node == null) node = go.AddComponent<StructureNode>();
                // 3.
                var health = go.GetComponent<IStructureHealth>();
                if (health != null) health.Initialize(bd, r.structureId, r.currentHP);
                // 4.
                node.Setup(bd, r.key, r.position.y, r.grounded);
                if (string.IsNullOrEmpty(node.StructureId))
                {
                    Debug.LogWarning($"[StructureRegistry] Prefab '{bd.id}' tanpa BaseHealth, struktur tidak bisa didaftarkan.");
                    continue;
                }
                byId[node.StructureId] = node;
                grid[node.Key] = node;
                // 5.
                var extra = FindPrefabSaveable(node);
                if (extra != null && !string.IsNullOrEmpty(r.extraJson)) extra.RestoreState(r.extraJson);
                // 6. Semua node menjadi benih: bangunan yang kehilangan penopang karena data berubah runtuh setelah load.
                dirtySeeds.Add(node);
            }
        }

        public void OnRestoreFinished()
        {
            foreach (var kv in byId)
            {
                var extra = FindPrefabSaveable(kv.Value);
                if (extra != null) extra.OnRestoreFinished();
            }
        }

        private static ISaveable FindPrefabSaveable(StructureNode node)
        {
            return node != null ? node.GetComponentInChildren<ISaveable>(true) : null;
        }
    }
}
