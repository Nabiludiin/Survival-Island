using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Alasan penempatan ditolak, urut sesuai tabel validasi BuildMenu.md (#79).</summary>
    public enum PlacementError
    {
        None, NoSelection, OutOfRange, NoBuildZone, UnknownSurface, Beach, TooSteep,
        MaxLevel, MaxStructures, Overlap, Occupied, NoSupport, NotEnoughMaterial,
        MaxLights   // LightSourceSystem.md: "Terlalu banyak lampu" (#98)
    }

    /// <summary>
    /// Penempatan struktur: hologram, grid, validasi, dan pembuatan objek. Manager di IslandPhase.
    /// Blueprint: raft/BuildManager.md (ringkasan) dan raft/BuildMenu.md (aturan rinci, #59, #79, #86, #88).
    ///
    /// Claude.md Pilar 2: grid snapping saat mode Build aktif, dan RESTRIKSI PANTAI: kalau SurfaceDetection membaca pasir
    /// di bawah hologram, material hologram menjadi merah dan instansiasi dibatalkan.
    /// GDD bagian 4: "Pemain dilarang membangun di atas pasir pantai. Sistem menolak penempatan, dan Diary menjelaskan
    /// bahwa air pasang akan menghanyutkan bangunan di sana."
    ///
    /// Catatan implementasi (Langkah 4):
    /// - Struktur menjadi anak structureRoot region dari RegionStreaming (#88), dengan cadangan structureRoot sendiri.
    /// - Hologram dibuat dari salinan prefab di bawah induk nonaktif, lalu semua Behaviour, Collider, dan Rigidbody dibuang
    ///   sebelum diaktifkan, supaya Awake/OnEnable komponen struktur tidak pernah berjalan pada hologram.
    /// - Kotak overlap dan tapak pantai dihitung dari collider/mesh prefab. Prefab lantai memakai pivot di permukaan atas
    ///   (rok kayu ke bawah), supaya dinding di atasnya tidak dianggap tumpang tindih.
    /// - GhostStatusIcon (#104), batas jebakan (#26) dan lampu (#98), durabilitas palu (#113), dan suara tolak (#16) sudah dipakai.
    ///   Belum ada (TODO): alasan penolakan di HUD.
    /// </summary>
    [DisallowMultipleComponent]
    public class BuildManager : MonoBehaviour
    {
        public static BuildManager Instance { get; private set; }

        [SerializeField] private BuildableData[] buildables;
        [SerializeField] private Material ghostValidMat;
        [SerializeField] private Material ghostInvalidMat;
        [Tooltip("Induk struktur cadangan kalau RegionStreaming (#88) tidak ada atau region tidak punya structureRoot.")]
        [SerializeField] private Transform structureRoot;
        [Tooltip("Ikon status pada hologram (#104). Boleh kosong.")]
        [SerializeField] private GhostStatusIcon ghostStatusIcon;
        [Tooltip("Suara saat penempatan ditolak (#16). Boleh kosong.")]
        [SerializeField] private AudioClip rejectClip;

        public HammerMode Mode { get; private set; } = HammerMode.Repair;
        public BuildableData Selected { get; private set; }
        public PlacementError LastError { get; private set; } = PlacementError.NoSelection;
        public IReadOnlyList<BuildableData> Buildables => buildables;

        private class GhostInfo
        {
            public GameObject go;
            public Renderer[] renderers;
            public Vector3 localCenter;
            public Vector3 halfExtents;
        }

        private readonly Dictionary<string, GhostInfo> ghostCache = new Dictionary<string, GhostInfo>(StringComparer.Ordinal);
        private GhostInfo ghost;
        private int ghostYaw;
        private bool ghostValidShown;
        private bool ghostHasMaterial;
        private Transform ghostStorage;      // Induk nonaktif untuk salinan prefab sebelum dibersihkan
        private readonly Collider[] overlapBuffer = new Collider[16];
        private readonly List<Behaviour> stripBehaviours = new List<Behaviour>(16);

        // Hasil bidikan terakhir.
        private Placement current;
        private int placingInstanceId = -1;  // BeginPlacementFromItem (#44). -1 = memakai cost

        private struct Placement
        {
            public bool valid;          // Raycast mengenai sesuatu
            public Vector3 pos;
            public Quaternion rot;
            public GridKey key;
            public int level;
            public bool onStructure;
            public bool grounded;
            public bool tooSteep;
        }

        private static readonly Vector2Int[] NeighborOffsets =
        {
            new Vector2Int(0, 0),
            new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1),
            new Vector2Int(2, 0), new Vector2Int(-2, 0), new Vector2Int(0, 2), new Vector2Int(0, -2),
        };

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
            var storage = new GameObject("GhostStorage");
            storage.SetActive(false);
            storage.transform.SetParent(transform, false);
            ghostStorage = storage.transform;
        }

        private void OnEnable()
        {
            EventChannel<UIFocusChangedEvent>.Raised += HandleFocusChanged;
        }

        private void OnDisable()
        {
            EventChannel<UIFocusChangedEvent>.Raised -= HandleFocusChanged;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public BuildableData FindBuildable(string id)
        {
            if (buildables == null || string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < buildables.Length; i++)
                if (buildables[i] != null && buildables[i].id == id) return buildables[i];
            return null;
        }

        /// <summary>Induk struktur di posisi ini: structureRoot region dari RegionStreaming (#88), cadangan structureRoot.</summary>
        public Transform GetStructureRoot(Vector3 pos)
        {
            var rs = RegionStreaming.Instance;
            var root = rs != null ? rs.GetStructureRoot(rs.GetRegionAt(pos)) : null;
            return root != null ? root : structureRoot;
        }

        // ---------------------------------------------------------------- Mode

        /// <summary>Atur mode palu dan pilihan, nyalakan atau matikan hologram, lalu terbitkan BuildModeChangedEvent.</summary>
        public void SelectMode(HammerMode mode, BuildableData data)
        {
            Mode = mode;
            Selected = mode == HammerMode.Build ? data : null;
            placingInstanceId = -1;
            SetGhost(Mode == HammerMode.Build ? Selected : null);
            LastError = Selected != null ? PlacementError.None : PlacementError.NoSelection;
            EventChannel<BuildModeChangedEvent>.Publish(new BuildModeChangedEvent(mode, data != null ? data.id : string.Empty));
        }

        /// <summary>
        /// Penempatan dari item, misalnya Kit_Campfire (#44). Palu tidak diperlukan. Item berkurang hanya setelah berhasil.
        /// </summary>
        public void BeginPlacementFromItem(int instanceId, BuildableData data)
        {
            SelectMode(HammerMode.Build, data);
            placingInstanceId = instanceId;
        }

        /// <summary>Sembunyikan hologram tanpa melupakan pilihan (palu diturunkan, OnUnequip).</summary>
        public void HideGhost()
        {
            if (ghost != null && ghost.go != null) ghost.go.SetActive(false);
        }

        /// <summary>Batalkan penempatan dari item (pemain mengganti item).</summary>
        public void CancelItemPlacement()
        {
            if (placingInstanceId < 0) return;
            SelectMode(HammerMode.Repair, null);
        }

        private void HandleFocusChanged(UIFocusChangedEvent e)
        {
            if (UIFocusStack.Instance != null && UIFocusStack.Instance.IsGameplayInputBlocked) HideGhost();
        }

        // ---------------------------------------------------------------- Hologram

        private void Update()
        {
            HandleOpenBuildMenuKey();
            if (Mode != HammerMode.Build || Selected == null) { HideGhost(); return; }
            if (!IsHolding()) { HideGhost(); return; }
            // Panel terbuka atau game dijeda: hologram tersembunyi dan klik tidak diproses.
            if (UIFocusStack.Instance != null && UIFocusStack.Instance.IsGameplayInputBlocked) { HideGhost(); return; }

            // 1. Rotasi per 90 derajat (R hanya dibaca saat mode Build).
            if (InputManager.Instance != null && InputManager.Instance.BuildRotatePressed) ghostYaw = (ghostYaw + 90) % 360;

            // 2-3.
            UpdateGhostPosition();
            LastError = current.valid ? ValidatePlacement(Selected, current) : PlacementError.OutOfRange;

            // 4. Material hanya diganti saat hasilnya berubah.
            if (ghost != null && ghost.go != null)
            {
                ghost.go.SetActive(current.valid);
                bool ok = LastError == PlacementError.None;
                if (!ghostHasMaterial || ok != ghostValidShown) ApplyGhostMaterial(ok);
            }
            // 5-6. Ikon status dan teks build.error.<PlacementError> pada hologram (#104). TODO HUD: alasan di HUD.
            if (ghostStatusIcon != null) ghostStatusIcon.SetStatus(LastError);
        }

        /// <summary>
        /// Tombol B (OpenBuildMenu): palu di hotbar = ganti ke palu lewat RequestSlot lalu buka menu.
        /// Palu tidak ada di hotbar = notifikasi "Butuh palu di hotbar" (#22).
        /// </summary>
        private void HandleOpenBuildMenuKey()
        {
            var input = InputManager.Instance;
            var eq = EquipmentManager.Instance;
            if (input == null || eq == null || !input.OpenBuildMenuPressed) return;
            if (eq.Active is HammerEquippable) { BuildMenu.Open(); return; }

            var inv = InventoryManager.Instance;
            for (int i = 0; i < eq.HotbarSlotCount; i++)
            {
                int id = eq.GetHotbarInstanceId(i);
                if (id == EquipmentManager.NoSlot || inv == null) continue;
                var inst = inv.FindInstance(id);
                var data = inst != null ? ItemDatabase.Get(inst.itemId) : null;
                if (data == null || !(data.equipPrefab is HammerEquippable)) continue;
                eq.RequestSlot(i);
                BuildMenu.Open();
                return;
            }
            // Notifikasi "Butuh palu di hotbar" (#22).
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.need_hammer", NotificationKind.Warning);
        }

        /// <summary>Hologram aktif kalau palu dipegang atau sedang menaruh dari item.</summary>
        private bool IsHolding()
        {
            if (placingInstanceId >= 0) return true;
            var eq = EquipmentManager.Instance;
            return eq != null && eq.Active is HammerEquippable;
        }

        private void UpdateGhostPosition()
        {
            current = default;
            var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : null;
            if (cam == null) return;
            float range = Balance != null ? Balance.build.maxBuildDistance : 6f;

            // Raycast hanya ke Terrain dan Structure, supaya tidak mengenai pemain, trigger, atau musuh.
            var ray = new Ray(cam.transform.position, cam.transform.forward);
            if (!PhysicsQuery.Raycast(ray, out RaycastHit hit, range, PhysicsMasks.BuildSurface)) return;

            float g = Balance != null ? Balance.build.gridSize : 2.5f;
            // Grid berlaku untuk seluruh dunia, jadi semua struktur otomatis sejajar.
            int cx = Mathf.RoundToInt(hit.point.x / g);
            int cz = Mathf.RoundToInt(hit.point.z / g);
            var rot = Quaternion.Euler(0f, ghostYaw, 0f);
            GridKey key = BuildKey(Selected, cx, cz, hit.point, cam.transform.position, g, ref rot);

            Vector3 pos = new Vector3(key.ix * g * 0.5f, 0f, key.iz * g * 0.5f);
            pos.y = ResolveHeight(pos, in hit, out int level, out bool onStructure, out bool grounded, out bool tooSteep);
            key.level = level;

            current.valid = true;
            current.pos = pos;
            current.rot = rot;
            current.key = key;
            current.level = level;
            current.onStructure = onStructure;
            current.grounded = grounded;
            current.tooSteep = tooSteep;

            if (ghost != null && ghost.go != null) ghost.go.transform.SetPositionAndRotation(pos, rot);
        }

        /// <summary>Kunci slot grid dari sel bidikan dan slotKind (BuildMenu.md "Rotasi dan snap").</summary>
        private static GridKey BuildKey(BuildableData data, int cx, int cz, Vector3 aim, Vector3 eye, float g, ref Quaternion rot)
        {
            int ix = cx * 2, iz = cz * 2;
            if (data.slotKind == BuildSlotKind.Cell)
            {
                var layer = data.support == SupportRule.FloorOrGround ? GridLayer.Object : GridLayer.Floor;
                return new GridKey(ix, iz, 0, layer);
            }

            float yaw = rot.eulerAngles.y;
            bool alongX = Mathf.Abs(Mathf.DeltaAngle(yaw, 0f)) < 45f || Mathf.Abs(Mathf.DeltaAngle(yaw, 180f)) < 45f;
            if (alongX)
            {
                // Sejajar sumbu X: sisi utara atau selatan, mana yang lebih dekat ke titik bidik.
                iz += aim.z >= cz * g ? 1 : -1;
            }
            else
            {
                // Sejajar sumbu Z: sisi timur atau barat.
                ix += aim.x >= cx * g ? 1 : -1;
            }

            if (data.slotKind == BuildSlotKind.Edge) return new GridKey(ix, iz, 0, GridLayer.Edge);

            // WallMount: menempel pada sisi dinding yang menghadap pemain, dan menghadap ke arah pemain.
            Vector3 edgePos = new Vector3(ix * g * 0.5f, 0f, iz * g * 0.5f);
            float side = alongX ? eye.z - edgePos.z : eye.x - edgePos.x;
            var mountLayer = side >= 0f ? GridLayer.MountA : GridLayer.MountB;
            Vector3 face = alongX ? new Vector3(0f, 0f, Mathf.Sign(side)) : new Vector3(Mathf.Sign(side), 0f, 0f);
            rot = Quaternion.LookRotation(face, Vector3.up);
            return new GridKey(ix, iz, 0, mountLayer);
        }

        /// <summary>
        /// Tinggi dasar dan tingkat (BuildMenu.md "Aturan tinggi"): membidik struktur, lantai tetangga, lalu tanah.
        /// snapPos.y = hit.point.y dari blueprint lama tidak dipakai.
        /// </summary>
        private float ResolveHeight(Vector3 cell, in RaycastHit hit, out int level, out bool onStructure, out bool grounded, out bool tooSteep)
        {
            var b = Balance != null ? Balance.build : null;
            float levelHeight = b != null ? b.levelHeight : 2.5f;
            float snap = b != null ? b.heightSnap : 0.25f;
            float maxDelta = b != null ? b.maxFoundationHeightDelta : 1.25f;
            level = 0;
            onStructure = false;
            grounded = false;
            tooSteep = false;
            var registry = StructureRegistry.Instance;

            // 1. Membidik struktur.
            var aimed = hit.collider != null && !(hit.collider is TerrainCollider) ? hit.collider.GetComponentInParent<StructureNode>() : null;
            if (aimed != null)
            {
                onStructure = true;
                float y = aimed.BaseY;
                level = aimed.Level;
                // Lantai dan atap di atas dinding berada satu tingkat di atasnya. Dinding di atas lantai: tingkat sama.
                if (aimed.Key.layer == GridLayer.Edge && hit.point.y > aimed.BaseY + levelHeight * 0.5f)
                {
                    y += levelHeight;
                    level += 1;
                }
                return y;
            }

            // Ukur tanah pada lima titik tapak.
            float half = (Balance != null ? Balance.build.gridSize : 2.5f) * 0.5f;
            float probeH = b != null ? b.groundProbeHeight : 5f;
            float minY = float.MaxValue, maxY = float.MinValue;
            int hits = 0;
            for (int i = 0; i < 5; i++)
            {
                Vector3 p = cell;
                if (i == 1) p += new Vector3(half, 0f, half);
                else if (i == 2) p += new Vector3(-half, 0f, half);
                else if (i == 3) p += new Vector3(half, 0f, -half);
                else if (i == 4) p += new Vector3(-half, 0f, -half);
                var ray = new Ray(new Vector3(p.x, hit.point.y + probeH, p.z), Vector3.down);
                if (!PhysicsQuery.Raycast(ray, out RaycastHit g, probeH * 2f, PhysicsMasks.Ground)) continue;
                hits++;
                if (g.point.y < minY) minY = g.point.y;
                if (g.point.y > maxY) maxY = g.point.y;
            }
            if (hits == 0) { minY = maxY = hit.point.y; }

            // 2. Ada lantai tetangga di tingkat 0: pakai tingginya, supaya baris lantai tetap rata di tanah miring.
            if (registry != null)
            {
                // Koordinat grid digandakan (lihat GridKey). Sel: lantai di sel sama atau 4 sel tetangga (jarak 2).
                // Sisi: lantai di dua sel yang mengapit sisi itu (jarak 1); filter IsCell membuang kunci yang bukan sel.
                int ix = Mathf.RoundToInt(cell.x / half), iz = Mathf.RoundToInt(cell.z / half);
                for (int d = 0; d < NeighborOffsets.Length; d++)
                {
                    var k = new GridKey(ix + NeighborOffsets[d].x, iz + NeighborOffsets[d].y, 0, GridLayer.Floor);
                    if (!k.IsCell || !registry.TryGet(k, out var floor)) continue;
                    // Lantai yang mengambang terlalu tinggi tetap boleh, tetapi tidak dihitung menapak tanah.
                    grounded = floor.BaseY - minY <= maxDelta;
                    return floor.BaseY;
                }
            }

            // 3. Di tanah: tinggi tertinggi dibulatkan heightSnap, tingkat 0.
            tooSteep = maxY - minY > maxDelta;
            grounded = hits > 0;
            return Mathf.Round(maxY / snap) * snap;
        }

        // ---------------------------------------------------------------- Validasi

        /// <summary>Berhenti pada kesalahan pertama, sesuai urutan tabel validasi BuildMenu.md.</summary>
        private PlacementError ValidatePlacement(BuildableData data, in Placement p)
        {
            var b = Balance != null ? Balance.build : null;
            // 1.
            if (data == null || data.prefab == null) return PlacementError.NoSelection;
            // 2.
            var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : null;
            float range = b != null ? b.maxBuildDistance : 6f;
            if (cam == null || (p.pos - cam.transform.position).sqrMagnitude > range * range) return PlacementError.OutOfRange;
            // 3. Seam, jembatan, helipad, buku Diary.
            if (NoBuildVolume.IsBlocked(p.pos)) return PlacementError.NoBuildZone;

            var info = GetGhostInfo(data);
            Vector3 center = p.pos + p.rot * info.localCenter;
            Vector3 flatHalf = new Vector3(info.halfExtents.x, 0f, info.halfExtents.z);

            // 4. Permukaan harus terbaca, kecuali tapak bertumpu pada struktur.
            var surface = SurfaceDetection.Instance;
            if (!p.onStructure && (surface == null || !surface.TryGetSurfaceAt(p.pos, out _))) return PlacementError.UnknownSurface;
            // 5. GDD bagian 4: larangan pasir pantai. Pasir dibaca lewat XZ, jadi batu di atas pasir tetap ditolak.
            if (surface != null && surface.IsBeachFootprint(center, flatHalf, p.rot)) return PlacementError.Beach;
            // 6.
            if (p.tooSteep) return PlacementError.TooSteep;
            // 7.
            int maxLevels = b != null ? b.maxLevels : 3;
            if (p.level >= maxLevels) return PlacementError.MaxLevel;
            // 8. BaseDefense.md (#26): Defense di atas trap.maxTrapsPerRegion ditolak dengan MaxStructures. Api unggun tidak dihitung.
            if (data.category == BuildCategory.Defense && data.prefab != null)
            {
                var trapBal = GameManager.CurrentBalance != null ? GameManager.CurrentBalance.trap : null;
                var trap = data.prefab.GetComponent<BaseTrap>();
                if (trapBal != null && trap != null && trap.Kind != TrapKind.Campfire
                    && BaseTrap.CountInRegion(RegionStreaming.RegionOf(p.pos)) >= trapBal.maxTrapsPerRegion)
                    return PlacementError.MaxStructures;
            }
            // LightSourceSystem.md (#98): struktur cahaya baru di atas light.maxLightStructures ditolak.
            if (data.prefab != null && data.prefab.GetComponent<LightFixture>() != null && !LightSourceSystem.CanBuildLightStructure())
                return PlacementError.MaxLights;
            var registry = StructureRegistry.Instance;
            int maxStructures = b != null ? b.maxStructures : 400;
            if (registry != null && registry.Count >= maxStructures) return PlacementError.MaxStructures;
            // 9. Overlap dengan kotak yang dikecilkan, supaya struktur yang bersentuhan tetap sah.
            float shrink = b != null ? b.overlapShrink : 0.1f;
            Vector3 half = new Vector3(
                Mathf.Max(0.01f, info.halfExtents.x - shrink),
                Mathf.Max(0.01f, info.halfExtents.y - shrink),
                Mathf.Max(0.01f, info.halfExtents.z - shrink));
            int count = PhysicsQuery.OverlapBox(center, half, p.rot, PhysicsMasks.BuildOverlap, overlapBuffer);
            if (count > 0)
            {
                int playerLayer = LayerMask.NameToLayer(PhysicsMasks.PlayerLayer);
                int enemyLayer = LayerMask.NameToLayer(PhysicsMasks.EnemyLayer);
                int npcLayer = LayerMask.NameToLayer(PhysicsMasks.NPCLayer);
                for (int i = 0; i < count; i++)
                {
                    int l = overlapBuffer[i].gameObject.layer;
                    if (l == playerLayer || l == enemyLayer || l == npcLayer) return PlacementError.Occupied;
                }
                return PlacementError.Overlap;
            }
            // 10. Satu slot satu struktur.
            if (registry != null && registry.IsOccupied(p.key)) return PlacementError.Overlap;
            // 11.
            if (registry != null && !registry.HasSupport(data, p.key, p.pos.y, p.grounded && p.level == 0 && !p.onStructure))
                return PlacementError.NoSupport;
            // 12. Bahan paling akhir, supaya alasan yang tampil adalah hal yang perlu diubah lebih dulu.
            if (!HasMaterials(data)) return PlacementError.NotEnoughMaterial;
            return PlacementError.None;
        }

        private bool HasMaterials(BuildableData data)
        {
            var inv = InventoryManager.Instance;
            if (inv == null) return false;
            if (placingInstanceId >= 0) return inv.FindInstance(placingInstanceId) != null;
            if (data.cost == null) return true;
            for (int i = 0; i < data.cost.Length; i++)
                if (!inv.HasItem(data.cost[i].itemId, data.cost[i].amount)) return false;
            return true;
        }

        // ---------------------------------------------------------------- Membuat struktur

        /// <summary>Klik kiri palu dalam mode Build. Validasi diulang, karena musuh bisa masuk setelah hologram diperiksa.</summary>
        public bool TryExecuteBuild()
        {
            if (Mode != HammerMode.Build || Selected == null) return false;
            if (UIFocusStack.Instance != null && UIFocusStack.Instance.IsGameplayInputBlocked) return false;
            UpdateGhostPosition();
            // 1.
            LastError = current.valid ? ValidatePlacement(Selected, current) : PlacementError.OutOfRange;
            if (LastError != PlacementError.None)
            {
                // Suara tolak (#16).
                if (AudioManager.Instance != null && rejectClip != null) AudioManager.Instance.PlaySFX(rejectClip);
                return false;
            }

            var data = Selected;
            var inv = InventoryManager.Instance;
            // 2. Kurangi biaya, atau item untuk penempatan dari item (#44).
            if (placingInstanceId >= 0) inv.RemoveInstance(placingInstanceId);
            else if (data.cost != null)
                for (int i = 0; i < data.cost.Length; i++) inv.RemoveItem(data.cost[i].itemId, data.cost[i].amount);
            // 3. Durabilitas palu -= build.hammerDecayPerBuild (#113), hanya saat memakai palu (bukan penempatan dari item).
            if (placingInstanceId < 0 && EquipmentManager.Instance != null && EquipmentManager.Instance.Active is HammerEquippable)
                DurabilitySystem.ApplyWear(EquipmentManager.Instance.ActiveInstance, Balance != null ? Balance.build.hammerDecayPerBuild : 1f);

            // 4-5. Struktur memakai Instantiate, bukan pool, karena jarang dibuat dan hidup lama (DamagePipeline.md).
            var registry = StructureRegistry.Instance;
            string id = registry != null ? registry.NewStructureId() : Guid.NewGuid().ToString("N");
            var go = Instantiate(data.prefab, current.pos, current.rot, GetStructureRoot(current.pos));

            // 6. BaseHealth sudah ada di prefab.
            var health = go.GetComponent<IStructureHealth>();
            if (health != null) health.Initialize(data, id, data.maxHP);
            else Debug.LogWarning($"[BuildManager] Prefab '{data.id}' belum punya BaseHealth (Langkah 5).");

            // 7. Grid dan isGrounded.
            var node = go.GetComponent<StructureNode>();
            if (node == null) node = go.AddComponent<StructureNode>();
            bool grounded = current.grounded && current.level == 0 && !current.onStructure && data.support != SupportRule.WallBelow;
            node.Setup(data, current.key, current.pos.y, grounded);
            if (registry != null) registry.Register(node);

            // 8.
            EventChannel<StructureBuiltEvent>.Publish(new StructureBuiltEvent(id, data.id, current.pos));

            // Penempatan dari item selesai sekali pakai.
            if (placingInstanceId >= 0) SelectMode(HammerMode.Repair, null);
            return true;
        }

        // ---------------------------------------------------------------- Hologram cache

        private void SetGhost(BuildableData data)
        {
            if (ghost != null && ghost.go != null) ghost.go.SetActive(false);
            ghost = data != null ? GetGhostInfo(data) : null;
            ghostHasMaterial = false;
        }

        /// <summary>Hologram per ID, dibuat sekali lalu dinyalakan dan dimatikan.</summary>
        private GhostInfo GetGhostInfo(BuildableData data)
        {
            if (ghostCache.TryGetValue(data.id ?? string.Empty, out var info) && info.go != null) return info;

            // Salinan di bawah induk nonaktif: Awake komponen struktur tidak berjalan.
            var go = Instantiate(data.prefab, ghostStorage);
            go.name = "Ghost_" + data.id;
            info = new GhostInfo { go = go };
            ComputeLocalBounds(go.transform, out info.localCenter, out info.halfExtents);

            // Buang semua yang punya perilaku atau fisika, lalu pindahkan ke layer StructureGhost.
            // Behaviour lebih dulu, karena script bisa mewajibkan (RequireComponent) collider atau Rigidbody.
            stripBehaviours.Clear();
            go.GetComponentsInChildren(true, stripBehaviours);
            for (int i = stripBehaviours.Count - 1; i >= 0; i--)
                if (stripBehaviours[i] != null) DestroyImmediate(stripBehaviours[i]);
            foreach (var rb in go.GetComponentsInChildren<Rigidbody>(true)) DestroyImmediate(rb);
            foreach (var c in go.GetComponentsInChildren<Collider>(true)) DestroyImmediate(c);
            int ghostLayer = LayerMask.NameToLayer(PhysicsMasks.StructureGhostLayer);
            if (ghostLayer >= 0)
                foreach (var t in go.GetComponentsInChildren<Transform>(true)) t.gameObject.layer = ghostLayer;

            info.renderers = go.GetComponentsInChildren<Renderer>(true);
            go.SetActive(false);
            go.transform.SetParent(transform, false);
            ghostCache[data.id ?? string.Empty] = info;
            return info;
        }

        private void ApplyGhostMaterial(bool valid)
        {
            ghostValidShown = valid;
            ghostHasMaterial = true;
            var mat = valid ? ghostValidMat : ghostInvalidMat;
            if (ghost == null || ghost.renderers == null || mat == null) return;
            for (int i = 0; i < ghost.renderers.Length; i++)
            {
                var r = ghost.renderers[i];
                if (r == null) continue;
                var mats = r.sharedMaterials;
                for (int m = 0; m < mats.Length; m++) mats[m] = mat;
                r.sharedMaterials = mats;
            }
        }

        /// <summary>Kotak lokal dari BoxCollider/MeshCollider, atau MeshFilter kalau tidak ada collider. Bekerja pada objek nonaktif.</summary>
        private static void ComputeLocalBounds(Transform root, out Vector3 center, out Vector3 halfExtents)
        {
            bool has = false;
            var bounds = new Bounds();
            var toRoot = root.worldToLocalMatrix;

            foreach (var col in root.GetComponentsInChildren<Collider>(true))
            {
                Bounds local;
                if (col is BoxCollider box) local = new Bounds(box.center, box.size);
                else if (col is MeshCollider mc && mc.sharedMesh != null) local = mc.sharedMesh.bounds;
                else if (col is SphereCollider sc) local = new Bounds(sc.center, Vector3.one * sc.radius * 2f);
                else if (col is CapsuleCollider cc) local = new Bounds(cc.center, new Vector3(cc.radius * 2f, cc.height, cc.radius * 2f));
                else continue;
                Encapsulate(ref bounds, ref has, local, toRoot * col.transform.localToWorldMatrix);
            }
            if (!has)
            {
                foreach (var mf in root.GetComponentsInChildren<MeshFilter>(true))
                    if (mf.sharedMesh != null) Encapsulate(ref bounds, ref has, mf.sharedMesh.bounds, toRoot * mf.transform.localToWorldMatrix);
            }
            if (!has) bounds = new Bounds(Vector3.up * 0.5f, Vector3.one);
            center = bounds.center;
            halfExtents = bounds.extents;
        }

        private static void Encapsulate(ref Bounds total, ref bool has, Bounds local, Matrix4x4 m)
        {
            Vector3 c = local.center, e = local.extents;
            for (int i = 0; i < 8; i++)
            {
                var corner = new Vector3(
                    c.x + ((i & 1) != 0 ? e.x : -e.x),
                    c.y + ((i & 2) != 0 ? e.y : -e.y),
                    c.z + ((i & 4) != 0 ? e.z : -e.z));
                Vector3 p = m.MultiplyPoint3x4(corner);
                if (!has) { total = new Bounds(p, Vector3.zero); has = true; }
                else total.Encapsulate(p);
            }
        }
    }
}
