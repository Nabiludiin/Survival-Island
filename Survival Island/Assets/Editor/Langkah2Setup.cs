using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TMPro;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SurvivalIsland.EditorTools
{
    /// <summary>
    /// Penyiapan Unity untuk Langkah 2 (LOGIC/README.md, "Penyiapan di Unity untuk Langkah 2"), ditambah pemain
    /// Langkah 3 dan sistem pendukung yang dipakai di kapal. Membuat:
    /// - aset ItemData isi Loot_Ship di Assets/Resources/Items (nilai dari ItemUseSystem.md, ShipInteractable.md,
    ///   CarryCapacity.md, RegionLootTable.md), LootTable Loot_Ship, jadwal bahaya kapal, ZombieData zombie_standar;
    /// - prefab Player_Ship (rig FirstPersonCamera), ShipLoot_Generic, ShipNpc_Generic, zombie_standar;
    /// - pool ketiga prefab itu di ObjectPooler prefab PersistentRoot (ObjectPooler.md);
    /// - scene ShipPhase: geladak greybox tiga dek (ShipHazardManager.md "Susunan kapal"), NavMeshSurface,
    ///   ShipTimerManager + HUD, ShipHazardManager, sekoci (LifeboatExtraction + RunSummaryPanel), LootSpawnerSystem.
    /// Bentuk geladak dan letak titik adalah greybox sementara, bukan desain level final.
    /// Menu boleh dijalankan ulang. Jalankan setelah menu Langkah 1.
    /// </summary>
    public static class Langkah2Setup
    {
        private const string ItemFolder = "Assets/Resources/Items";
        private const string ShipDataFolder = "Assets/Data/Ship";
        private const string EnemyDataFolder = "Assets/Data/Enemies";
        private const string ShipPrefabFolder = "Assets/Prefabs/Ship";
        private const string EnemyPrefabFolder = "Assets/Prefabs/Enemies";
        private const string MaterialFolder = "Assets/Prefabs/Ship/Materials";
        private const string PersistentRootPath = "Assets/Prefabs/PersistentRoot.prefab";
        private const string ShipScenePath = "Assets/Scenes/ShipPhase.unity";
        private const string NavMeshDataPath = "Assets/Scenes/ShipPhase_NavMesh.asset";

        // LootSpawnerSystem.md: ID stabil "ship_loot_<8 karakter>". Tetap sama saat menu dijalankan ulang.
        private const string ShipSpawnerId = "ship_loot_7d3a91c4";

        // ShipHazardManager.md "Susunan kapal": lantai dek bawah, tengah, dan atas.
        private const float LowerY = 0f, MidY = 4f, UpperY = 8f;
        private const float HalfWidth = 7f, HalfLength = 20f, FloorThickness = 0.4f;

        internal static Material floorMat, wallMat, debrisMat, lootMat, npcMat, zombieMat, outlineMat, waterMat, boatMat;

        [MenuItem("Survival Island/Setup/Langkah 2 - ShipPhase")]
        public static void Run()
        {
            if (TMP_Settings.defaultFontAsset == null)
            {
                Debug.LogError("[Langkah2Setup] TMP Essential Resources belum diimpor.");
                return;
            }
            if (AssetDatabase.LoadAssetAtPath<GameObject>(PersistentRootPath) == null)
            {
                Debug.LogError("[Langkah2Setup] PersistentRoot belum ada. Jalankan menu Langkah 1 dulu.");
                return;
            }
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            foreach (var f in new[] { ItemFolder, ShipDataFolder, EnemyDataFolder, ShipPrefabFolder, EnemyPrefabFolder, MaterialFolder })
                EnsureFolder(f);

            CreateMaterials();
            var items = CreateShipItems();
            var lootTable = CreateLootTable(items);
            var schedule = CreateSchedule();
            var zombieData = CreateZombieData();

            var lootPrefab = BuildShipLootPrefab();
            var npcPrefab = BuildShipNpcPrefab();
            var zombiePrefab = BuildZombiePrefab(zombieData);
            var playerPrefab = BuildPlayerPrefab();

            int lootPointCount = BuildShipScene(playerPrefab, lootTable, schedule);
            RegisterPools(lootPrefab, lootPointCount, npcPrefab, zombiePrefab);

            AssetDatabase.SaveAssets();
            Debug.Log($"[Langkah2Setup] Selesai: {items.Count} ItemData, Loot_Ship, jadwal bahaya, zombie_standar, Player_Ship, " +
                      $"scene ShipPhase ({lootPointCount} titik loot), dan pool kapal di PersistentRoot.");
        }

        // ---------------------------------------------------------------- Data

        private struct ItemDef
        {
            public string id;
            public ItemType type;
            public int maxStack, shipPickup;
            public float hunger, thirst, heal, infection, use, restore;
            public bool consumable;
        }

        private static List<ItemData> CreateShipItems()
        {
            // Hanya isi Loot_Ship (RegionLootTable.md). Angka:
            // - lapar, haus, waktu pakai: ItemUseSystem.md "Contoh isi ItemData konsumsi" dan "Isi tambahan obat";
            // - restoreSeconds makanan dan minuman = items.defaultRestoreSeconds 20 (ItemUseSystem.md bagian 2);
            // - maxStack: CarryCapacity.md "Aturan konversi" (peluru 60, lainnya 5) dan ItemUseSystem.md (baterai 5);
            // - shipWeight 1 dan shipPickupAmount: ShipInteractable.md "Berat dan jumlah per barang kapal".
            // Med_Bandage tidak punya maxStack di blueprint, jadi memakai nilai awal ItemData (1).
            var defs = new[]
            {
                new ItemDef { id = "Food_Can", type = ItemType.Consumable, maxStack = 5, shipPickup = 2, hunger = 33, thirst = 5, use = 3, restore = 20, consumable = true },
                new ItemDef { id = "Ammo_9mm", type = ItemType.Material, maxStack = 60, shipPickup = 12 },
                new ItemDef { id = "Drink_CleanWater", type = ItemType.Consumable, maxStack = 5, shipPickup = 2, thirst = 25, use = 2, restore = 20, consumable = true },
                new ItemDef { id = "Med_Bandage", type = ItemType.Consumable, maxStack = 1, shipPickup = 1, heal = 5, use = 3, consumable = true },
                new ItemDef { id = "Med_Medkit", type = ItemType.Consumable, maxStack = 5, shipPickup = 1, heal = 40, use = 5, consumable = true },
                new ItemDef { id = "Med_Vaccine", type = ItemType.Consumable, maxStack = 5, shipPickup = 1, infection = -50, use = 4, consumable = true },
                new ItemDef { id = "Item_Battery", type = ItemType.Consumable, maxStack = 5, shipPickup = 1, consumable = true },
            };

            var list = new List<ItemData>();
            foreach (var d in defs)
            {
                string path = $"{ItemFolder}/{d.id}.asset";
                var item = LoadOrCreate<ItemData>(path);
                item.itemID = d.id;
                item.type = d.type;
                item.maxStack = d.maxStack;
                item.hungerDelta = d.hunger;
                item.thirstDelta = d.thirst;
                item.healAmount = d.heal;
                item.infectionDelta = d.infection;
                item.useSeconds = d.use;
                item.restoreSeconds = d.restore;
                item.isConsumable = d.consumable;
                item.isDroppable = true;
                item.shipWeight = 1;
                item.shipPickupAmount = d.shipPickup;
                EditorUtility.SetDirty(item);
                list.Add(item);
            }
            return list;
        }

        private static LootTable CreateLootTable(List<ItemData> items)
        {
            // RegionLootTable.md, Loot_Ship: jumlah 1 (jumlah objek dibaca dari shipPickupAmount), bobot, semua Any.
            // Tanda * di tabel = isRare (Med_Medkit, Med_Vaccine).
            var table = LoadOrCreate<LootTable>($"{ShipDataFolder}/Loot_Ship.asset");
            table.tableId = "Loot_Ship";
            table.location = LootLocation.Ship;
            float[] weights = { 30, 25, 20, 12, 5, 4, 5 };
            bool[] rare = { false, false, false, false, true, true, false };
            table.entries = new LootEntry[items.Count];
            for (int i = 0; i < items.Count; i++)
                table.entries[i] = new LootEntry
                {
                    item = items[i], weight = weights[i], minAmount = 1, maxAmount = 1,
                    kinds = LootPointKind.Any, minDay = 0, isRare = rare[i]
                };
            EditorUtility.SetDirty(table);
            return table;
        }

        private static ShipHazardSchedule CreateSchedule()
        {
            var schedule = LoadOrCreate<ShipHazardSchedule>($"{ShipDataFolder}/ShipHazardSchedule.asset");
            // Sama dengan menu konteks "Isi jadwal awal (ShipHazardManager.md)" di Inspector aset.
            var fill = typeof(ShipHazardSchedule).GetMethod("FillDefaultSchedule", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fill != null) fill.Invoke(schedule, null);
            else Debug.LogError("[Langkah2Setup] Menu konteks FillDefaultSchedule tidak ditemukan.");
            EditorUtility.SetDirty(schedule);
            return schedule;
        }

        private static ZombieData CreateZombieData()
        {
            // SpecialZombieAI.md: zombie_standar HP 50, kecepatan 4, damage 15, infeksi 5 (sama dengan nilai awal kelas).
            var attack = LoadOrCreate<AttackData>($"{EnemyDataFolder}/Zombie_Standard_Hit.asset");
            attack.attackId = "Zombie_Standard_Hit";
            EditorUtility.SetDirty(attack);

            var data = LoadOrCreate<ZombieData>($"{EnemyDataFolder}/zombie_standar.asset");
            data.enemyId = "zombie_standar";
            data.maxHP = 50f;
            data.moveSpeed = 4f;
            data.primaryAttack = attack;
            data.canAttackBuildings = true;
            EditorUtility.SetDirty(data);
            return data;
        }

        // ---------------------------------------------------------------- Material

        internal static void CreateMaterials()
        {
            floorMat = Mat("Greybox_Floor", new Color(0.45f, 0.42f, 0.38f));
            wallMat = Mat("Greybox_Wall", new Color(0.3f, 0.33f, 0.36f));
            debrisMat = Mat("Greybox_Debris", new Color(0.35f, 0.2f, 0.12f));
            lootMat = Mat("Greybox_Loot", new Color(0.85f, 0.65f, 0.2f));
            npcMat = Mat("Greybox_Npc", new Color(0.2f, 0.5f, 0.85f));
            zombieMat = Mat("Greybox_Zombie", new Color(0.35f, 0.55f, 0.3f));
            boatMat = Mat("Greybox_Lifeboat", new Color(0.9f, 0.4f, 0.1f));
            outlineMat = Mat("Greybox_Outline", new Color(1f, 1f, 1f, 0.35f), unlit: true, transparent: true);
            waterMat = Mat("Greybox_Water", new Color(0.1f, 0.3f, 0.45f, 0.6f), unlit: false, transparent: true);
        }

        internal static Material Mat(string name, Color color, bool unlit = false, bool transparent = false)
        {
            string path = $"{MaterialFolder}/{name}.mat";
            var shader = Shader.Find(unlit ? "Universal Render Pipeline/Unlit" : "Universal Render Pipeline/Lit");
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null)
            {
                mat = new Material(shader);
                AssetDatabase.CreateAsset(mat, path);
            }
            mat.shader = shader;
            mat.SetColor("_BaseColor", color);
            if (transparent)
            {
                mat.SetFloat("_Surface", 1f);
                mat.SetFloat("_Blend", 0f);
                mat.SetOverrideTag("RenderType", "Transparent");
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            }
            EditorUtility.SetDirty(mat);
            return mat;
        }

        // ---------------------------------------------------------------- Prefab

        private static GameObject BuildShipLootPrefab()
        {
            // ShipInteractable.md: MeshFilter, MeshRenderer, BoxCollider bukan trigger, InteractHighlight, layer Interactable.
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "ShipLoot_Generic";
            go.layer = Layer(PhysicsMasks.InteractableLayer);
            go.transform.localScale = Vector3.one * 0.45f;
            go.GetComponent<MeshRenderer>().sharedMaterial = lootMat;
            var highlight = AddHighlight(go, PrimitiveType.Cube);
            var si = go.AddComponent<ShipInteractable>();
            Set(si, "poolTag", "ShipLoot_Generic");
            Set(si, "highlight", highlight);
            Set(si, "meshFilter", go.GetComponent<MeshFilter>());
            Set(si, "meshRenderer", go.GetComponent<MeshRenderer>());
            Set(si, "boxCollider", go.GetComponent<BoxCollider>());
            Set(si, "placeholderMesh", go.GetComponent<MeshFilter>().sharedMesh);
            return SavePrefab(go, $"{ShipPrefabFolder}/ShipLoot_Generic.prefab");
        }

        private static GameObject BuildShipNpcPrefab()
        {
            // ShipInteractable.md: tubuh NPC sederhana, CapsuleCollider, InteractHighlight, tanpa NavMeshAgent. Layer NPC.
            var go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            go.name = "ShipNpc_Generic";
            go.layer = Layer(PhysicsMasks.NPCLayer);
            go.tag = "NPC";
            go.GetComponent<MeshRenderer>().sharedMaterial = npcMat;
            var highlight = AddHighlight(go, PrimitiveType.Capsule);
            var si = go.AddComponent<ShipInteractable>();
            Set(si, "poolTag", "ShipNpc_Generic");
            Set(si, "highlight", highlight);
            return SavePrefab(go, $"{ShipPrefabFolder}/ShipNpc_Generic.prefab");
        }

        private static GameObject BuildZombiePrefab(ZombieData data)
        {
            // Zombie standar greybox: NavMeshAgent wajib (Claude.md Pilar 3), tag Zombie, layer Enemy, hitbox EnemyHitbox.
            var go = new GameObject("zombie_standar");
            go.layer = Layer(PhysicsMasks.EnemyLayer);
            go.tag = "Zombie";
            var body = go.AddComponent<CapsuleCollider>();
            body.height = 1.8f;
            body.radius = 0.35f;
            body.center = new Vector3(0f, 0.9f, 0f);
            var agent = go.AddComponent<NavMeshAgent>();
            agent.radius = 0.35f;
            agent.height = 1.8f;
            agent.speed = data.moveSpeed;
            var health = go.AddComponent<EnemyHealth>();
            Set(health, "data", data);
            go.AddComponent<EnemyAttack>();
            go.AddComponent<AISensory>();
            go.AddComponent<EnemyInstance>();
            go.AddComponent<EnemyAI>();

            var visual = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            visual.name = "Body";
            Object.DestroyImmediate(visual.GetComponent<Collider>());
            visual.transform.SetParent(go.transform, false);
            visual.transform.localPosition = new Vector3(0f, 0.9f, 0f);
            visual.transform.localScale = new Vector3(0.7f, 0.9f, 0.7f);
            visual.layer = go.layer;
            visual.GetComponent<MeshRenderer>().sharedMaterial = zombieMat;

            AddHitbox(go.transform, "Hitbox_Torso", BodyPart.Torso, new Vector3(0f, 1.0f, 0f), new Vector3(0.6f, 0.9f, 0.4f));
            AddHitbox(go.transform, "Hitbox_Head", BodyPart.Head, new Vector3(0f, 1.65f, 0f), new Vector3(0.3f, 0.3f, 0.3f));
            AddHitbox(go.transform, "Hitbox_Legs", BodyPart.Leg, new Vector3(0f, 0.3f, 0f), new Vector3(0.5f, 0.6f, 0.35f));
            return SavePrefab(go, $"{EnemyPrefabFolder}/zombie_standar.prefab");
        }

        private static void AddHitbox(Transform parent, string name, BodyPart part, Vector3 center, Vector3 size)
        {
            var go = new GameObject(name);
            go.layer = Layer(PhysicsMasks.EnemyHitboxLayer);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = center;
            go.AddComponent<BoxCollider>().size = size;
            go.AddComponent<HitboxModifier>().bodyPart = part;
        }

        private static GameObject BuildPlayerPrefab()
        {
            // README Langkah 2 nomor 2: layer dan tag Player, CharacterController (1,8 / 0,35 / 0,3 / 45 / 0,08).
            var root = new GameObject("Player_Ship");
            root.layer = Layer(PhysicsMasks.PlayerLayer);
            root.tag = "Player";
            var cc = root.AddComponent<CharacterController>();
            cc.height = 1.8f;
            cc.radius = 0.35f;
            cc.stepOffset = 0.3f;
            cc.slopeLimit = 45f;
            cc.skinWidth = 0.08f;
            cc.center = new Vector3(0f, 0.9f, 0f);

            var fp = BuildFirstPersonRig(root, out var worldCamGo);
            var rig = fp.transform.Find("CameraRig");

            // Getaran kamera (#24) di rig, sistem pendukung "Komponen scene". Profil getaran belum dibuat.
            var shake = rig.gameObject.AddComponent<CameraShake>();
            Set(shake, "fpCamera", fp);

            var interactor = root.AddComponent<PlayerInteractor>();
            Set(interactor, "cameraTransform", worldCamGo.transform);
            Set(interactor, "characterController", cc);
            var shove = root.AddComponent<ShipShove>();
            root.AddComponent<SurvivalStats>();
            // Tangan kapal (ShipHandItems) ada di InventoryManager, yang berada di ShipPhase dan IslandPhase (InventoryManager.md).
            root.AddComponent<InventoryManager>();
            root.AddComponent<CarryCapacity>();

            var ship = root.AddComponent<ShipPlayerController>();
            Set(ship, "controller", cc);
            Set(ship, "interactor", interactor);
            Set(ship, "fpCamera", fp);
            Set(ship, "shove", shove);
            return SavePrefab(root, $"{ShipPrefabFolder}/Player_Ship.prefab");
        }

        /// <summary>
        /// Rig FirstPersonCamera (README Langkah 3 nomor 4): CameraRig > PitchPivot > OffsetPivot > WorldCamera
        /// dan ViewmodelCamera > ViewmodelRoot. ViewmodelCamera menjadi Overlay di Camera Stack URP.
        /// </summary>
        internal static FirstPersonCamera BuildFirstPersonRig(GameObject root, out GameObject worldCamGo)
        {
            var rig = Child(root.transform, "CameraRig", new Vector3(0f, 1.6f, 0f));
            var pitch = Child(rig, "PitchPivot", Vector3.zero);
            var offset = Child(pitch, "OffsetPivot", Vector3.zero);
            worldCamGo = Child(offset, "WorldCamera", Vector3.zero).gameObject;
            worldCamGo.tag = "MainCamera";
            var worldCam = worldCamGo.AddComponent<Camera>();
            worldCam.nearClipPlane = 0.05f;
            worldCam.fieldOfView = 70f;
            worldCam.cullingMask = ~(1 << Layer(PhysicsMasks.PlayerViewmodelLayer));
            worldCamGo.AddComponent<AudioListener>();
            var worldData = worldCam.GetUniversalAdditionalCameraData();

            var vmCamGo = Child(offset, "ViewmodelCamera", Vector3.zero).gameObject;
            var vmCam = vmCamGo.AddComponent<Camera>();
            vmCam.nearClipPlane = 0.01f;
            vmCam.fieldOfView = 60f;
            vmCam.cullingMask = 1 << Layer(PhysicsMasks.PlayerViewmodelLayer);
            var vmData = vmCam.GetUniversalAdditionalCameraData();
            vmData.renderType = CameraRenderType.Overlay;
            worldData.cameraStack.Clear();
            worldData.cameraStack.Add(vmCam);
            var vmRoot = Child(vmCamGo.transform, "ViewmodelRoot", Vector3.zero);
            vmRoot.gameObject.layer = Layer(PhysicsMasks.PlayerViewmodelLayer);

            var fp = root.AddComponent<FirstPersonCamera>();
            Set(fp, "playerBody", root.transform);
            Set(fp, "cameraRig", rig);
            Set(fp, "pitchPivot", pitch);
            Set(fp, "offsetPivot", offset);
            Set(fp, "worldCamera", worldCam);
            Set(fp, "viewmodelCamera", vmCam);
            Set(fp, "viewmodelRoot", vmRoot);
            return fp;
        }

        internal static InteractHighlight AddHighlight(GameObject go, PrimitiveType shape)
        {
            // InteractHighlight mewarnai _BaseColor renderer garis luar dan mematikannya saat Off.
            var outline = GameObject.CreatePrimitive(shape);
            outline.name = "Outline";
            Object.DestroyImmediate(outline.GetComponent<Collider>());
            outline.transform.SetParent(go.transform, false);
            outline.transform.localScale = Vector3.one * 1.12f;
            outline.layer = go.layer;
            var r = outline.GetComponent<MeshRenderer>();
            r.sharedMaterial = outlineMat;
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            r.enabled = false;
            var h = go.AddComponent<InteractHighlight>();
            Set(h, "outlineRenderer", r);
            return h;
        }

        // ---------------------------------------------------------------- Pool

        private static void RegisterPools(GameObject loot, int lootSize, GameObject npc, GameObject zombie)
        {
            // ObjectPooler.md: ShipLoot_Generic = jumlah titik loot kapal, ShipNpc_Generic 3, zombie_standar 50. Semua Fixed.
            var contents = PrefabUtility.LoadPrefabContents(PersistentRootPath);
            try
            {
                var pooler = contents.GetComponentInChildren<ObjectPooler>(true);
                Upsert(pooler.pools, "ShipLoot_Generic", loot, lootSize);
                Upsert(pooler.pools, "ShipNpc_Generic", npc, 3);
                Upsert(pooler.pools, "zombie_standar", zombie, 50);
                PrefabUtility.SaveAsPrefabAsset(contents, PersistentRootPath);
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(contents);
            }
        }

        internal static void Upsert(List<Pool> pools, string tag, GameObject prefab, int size)
        {
            var pool = pools.Find(p => p.tag == tag);
            if (pool == null)
            {
                pool = new Pool { tag = tag };
                pools.Add(pool);
            }
            pool.prefab = prefab;
            pool.size = size;
            pool.overflow = PoolOverflow.Fixed;
            pool.maxSize = 0;
        }

        // ---------------------------------------------------------------- Scene

        private static int BuildShipScene(GameObject playerPrefab, LootTable lootTable, ShipHazardSchedule schedule)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var sun = new GameObject("Directional Light").AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.intensity = 0.8f;
            sun.color = new Color(0.75f, 0.8f, 0.95f);
            sun.shadows = LightShadows.Soft;
            sun.transform.rotation = Quaternion.Euler(40f, -30f, 0f);

            // Kemiringan tetap (ShipHazardManager.md "NavMesh pada scene miring", shipHazard.staticTiltDegrees 4):
            // geometri diputar sekali di Editor, sisi +X lebih rendah (downhillDirection = kanan).
            var ship = new GameObject("Ship").transform;
            var cfg = AssetDatabase.LoadAssetAtPath<GameBalanceConfig>("Assets/Data/GameBalanceConfig.asset");
            float tilt = cfg != null ? cfg.shipHazard.staticTiltDegrees : 4f;
            ship.rotation = Quaternion.Euler(0f, 0f, -tilt);

            var geo = Child(ship, "Geometry", Vector3.zero);
            BuildDecks(geo);

            // Koridor runtuh (ShipHazardManager.md): Corridor_MidPort di dek tengah sisi kiri, Stair_LowerAft di tangga belakang.
            var corridors = new List<ShipCorridor>
            {
                BuildCorridor(ship, "Corridor_MidPort", new Vector3(-5f, MidY, 4f), new Vector3(4f, 3f, 3f), new Vector3(-1f, MidY, 4f)),
                BuildCorridor(ship, "Stair_LowerAft", new Vector3(-5f, LowerY, -10f), new Vector3(4f, 4f, 8f), new Vector3(-5f, MidY, -17f)),
            };

            // Penutup tangga dek bawah (lowerDeckBarrier): mulai nonaktif, menutup tangga depan dari atas.
            var barrierGo = Box(ship, "LowerDeckBarrier", new Vector3(-5f, MidY + 1f, 14.3f), new Vector3(4f, 2f, 0.3f), wallMat, PhysicsMasks.ObstacleLayer);
            var barrier = barrierGo.GetComponent<Collider>();
            barrierGo.GetComponent<MeshRenderer>().enabled = false;
            barrier.enabled = false;

            // Titik zombie per grup jadwal: lower_start 3, mid_start 1, mid_45 2 (wakeOnAlarm, README putaran kedua),
            // lower_30 2, mid_15 2.
            var zp = Child(ship, "ZombiePoints", Vector3.zero);
            var zombiePoints = new List<ShipZombiePoint>
            {
                ZPoint(zp, "lower_start", new Vector3(3f, LowerY, -6f), false),
                ZPoint(zp, "lower_start", new Vector3(-1f, LowerY, 8f), false),
                ZPoint(zp, "lower_start", new Vector3(4f, LowerY, 14f), false),
                ZPoint(zp, "mid_start", new Vector3(4f, MidY, -14f), false),
                ZPoint(zp, "mid_45", new Vector3(-4f, MidY, -2f), true),
                ZPoint(zp, "mid_45", new Vector3(2f, MidY, 15f), true),
                ZPoint(zp, "lower_30", new Vector3(0f, LowerY, -10f), false),
                ZPoint(zp, "lower_30", new Vector3(2f, LowerY, 4f), false),
                ZPoint(zp, "mid_15", new Vector3(-1f, MidY, 12f), false),
                ZPoint(zp, "mid_15", new Vector3(3f, MidY, -9f), false),
            };

            // Titik loot: dek bawah paling banyak (gudang), dek tengah sedang, dek atas sedikit.
            var lp = Child(ship, "LootPoints", Vector3.zero);
            var lootPoints = new List<Transform>();
            Vector3[] lootPos =
            {
                new Vector3(-2f, LowerY, -14f), new Vector3(5f, LowerY, -12f), new Vector3(-3f, LowerY, -3f),
                new Vector3(5f, LowerY, 2f), new Vector3(-2f, LowerY, 10f), new Vector3(5f, LowerY, 17f),
                new Vector3(-6f, MidY, -2f), new Vector3(5f, MidY, -15f), new Vector3(-1f, MidY, 9f), new Vector3(6f, MidY, 10f),
                new Vector3(-6f, UpperY, -15f), new Vector3(0f, UpperY, -8f),
            };
            for (int i = 0; i < lootPos.Length; i++)
                lootPoints.Add(Child(lp, $"LootPoint_{i:00}", lootPos[i] + Vector3.up * 0.3f));

            // Titik NPC kapal (3, RunLayoutSources / LifeboatExtraction.shipNpcPoints).
            var np = Child(ship, "ShipNpcPoints", Vector3.zero);
            var npcPoints = new[]
            {
                Child(np, "NpcPoint_Lower", new Vector3(2f, LowerY + 0.9f, -2f)),
                Child(np, "NpcPoint_MidCabin", new Vector3(-5f, MidY + 0.9f, -17f)),
                Child(np, "NpcPoint_MidFore", new Vector3(5f, MidY + 0.9f, 17f)),
            };

            // Sekoci di dek atas (ShipHazardManager.md, #74).
            var boat = Box(ship, "Lifeboat", new Vector3(-4.5f, UpperY + 0.6f, 15f), new Vector3(2.2f, 1.2f, 4.5f), boatMat, PhysicsMasks.InteractableLayer);
            var boatHighlight = AddHighlight(boat, PrimitiveType.Cube);
            var safePoint = Child(ship, "LifeboatSafePoint", new Vector3(-2f, UpperY, 15f));

            // NavMeshSurface satu untuk seluruh kapal, dibake setelah kemiringan tetap.
            var surface = ship.gameObject.AddComponent<NavMeshSurface>();
            surface.collectObjects = CollectObjects.Children;
            surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            surface.layerMask = (1 << Layer(PhysicsMasks.TerrainLayer)) | (1 << Layer(PhysicsMasks.ObstacleLayer));

            // Air: satu bidang datar untuk seluruh kapal, tidak ikut miring.
            var water = GameObject.CreatePrimitive(PrimitiveType.Plane);
            water.name = "WaterVisual";
            Object.DestroyImmediate(water.GetComponent<Collider>());
            water.layer = Layer(PhysicsMasks.WaterLayer);
            water.transform.position = new Vector3(0f, -1f, 0f);
            water.transform.localScale = new Vector3(8f, 1f, 8f);
            water.GetComponent<MeshRenderer>().sharedMaterial = waterMat;

            // Pemain di dek tengah (titik awal).
            var player = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab, scene);
            player.transform.position = ship.TransformPoint(new Vector3(0f, MidY + 0.1f, 0f));
            player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            var shipController = player.GetComponent<ShipPlayerController>();
            var stats = player.GetComponent<SurvivalStats>();

            // UI kapal.
            var canvas = CreateCanvas("ShipCanvas", 10);
            var runSummary = BuildRunSummary(canvas.transform);
            BuildHud(canvas.transform, player);
            BuildTimer(canvas.transform);
            runSummary.transform.SetAsLastSibling();   // Ringkasan digambar di atas HUD dan timer

            // Sistem scene.
            var systems = new GameObject("ShipSystems").transform;

            var hazardGo = new GameObject("ShipHazardManager");
            hazardGo.transform.SetParent(systems, false);
            var sway = hazardGo.AddComponent<ShipSwaySource>();
            var alarm = hazardGo.AddComponent<AudioSource>();
            alarm.playOnAwake = false;
            alarm.loop = true;
            var hazard = hazardGo.AddComponent<ShipHazardManager>();
            Set(hazard, "schedule", schedule);
            SetArray(hazard, "corridors", corridors.ToArray());
            SetArray(hazard, "zombiePoints", zombiePoints.ToArray());
            Set(hazard, "waterVisual", water.transform);
            Set(hazard, "lowerDeckBarrier", barrier);
            SetFloat(hazard, "lowerDeckFloorY", LowerY);
            SetVector(hazard, "downhillDirection", Vector3.right);
            Set(hazard, "swaySource", sway);
            Set(hazard, "alarmSource", alarm);
            Set(hazard, "playerController", shipController);
            Set(hazard, "stats", stats);

            var extraction = boat.AddComponent<LifeboatExtraction>();
            Set(extraction, "safePoint", safePoint);
            SetArray(extraction, "shipNpcPoints", npcPoints);
            Set(extraction, "runSummaryPanel", runSummary);
            Set(extraction, "highlight", boatHighlight);
            Set(extraction, "playerController", shipController);
            Set(extraction, "playerStats", stats);

            var lootGo = new GameObject("LootSpawner_Ship");
            lootGo.transform.SetParent(systems, false);
            var spawner = lootGo.AddComponent<LootSpawnerSystem>();
            Set(spawner, "spawnerId", ShipSpawnerId);
            Set(spawner, "table", lootTable);
            var so = new SerializedObject(spawner);
            var arr = so.FindProperty("spawnPoints");
            arr.arraySize = lootPoints.Count;
            for (int i = 0; i < lootPoints.Count; i++)
            {
                var el = arr.GetArrayElementAtIndex(i);
                el.FindPropertyRelative("point").objectReferenceValue = lootPoints[i];
                el.FindPropertyRelative("kind").intValue = (int)LootPointKind.House;
                el.FindPropertyRelative("guaranteedItems").arraySize = 0;
            }
            so.ApplyModifiedPropertiesWithoutUndo();

            // Bake NavMesh lalu simpan datanya sebagai aset di samping scene.
            surface.BuildNavMesh();
            if (surface.navMeshData != null)
            {
                AssetDatabase.DeleteAsset(NavMeshDataPath);
                AssetDatabase.CreateAsset(surface.navMeshData, NavMeshDataPath);
            }
            else Debug.LogError("[Langkah2Setup] Bake NavMesh kapal gagal.");

            EditorSceneManager.SaveScene(scene, ShipScenePath);
            return lootPoints.Count;
        }

        private static void BuildDecks(Transform geo)
        {
            float w = HalfWidth, l = HalfLength, t = FloorThickness;

            // Dek bawah: lantai penuh.
            Box(geo, "LowerDeck", new Vector3(0f, LowerY - t / 2f, 0f), new Vector3(w * 2f, t, l * 2f), floorMat, PhysicsMasks.TerrainLayer);

            // Dek tengah: lantai utama x[-3,7] penuh, jalur kiri x[-7,-3] dengan celah tangga z[-14,-6] dan z[6,14].
            Box(geo, "MidDeck_Main", new Vector3(2f, MidY - t / 2f, 0f), new Vector3(10f, t, l * 2f), floorMat, PhysicsMasks.TerrainLayer);
            Box(geo, "MidDeck_PortAft", new Vector3(-5f, MidY - t / 2f, -17f), new Vector3(4f, t, 6f), floorMat, PhysicsMasks.TerrainLayer);
            Box(geo, "MidDeck_PortMid", new Vector3(-5f, MidY - t / 2f, 0f), new Vector3(4f, t, 12f), floorMat, PhysicsMasks.TerrainLayer);
            Box(geo, "MidDeck_PortFore", new Vector3(-5f, MidY - t / 2f, 17f), new Vector3(4f, t, 6f), floorMat, PhysicsMasks.TerrainLayer);
            // Tangga dek bawah ke dek tengah (ramp 8 m, kemiringan sekitar 27 derajat): belakang (Aft) dan depan (Fore).
            Ramp(geo, "Stair_LowerAft", new Vector3(-5f, LowerY, -6f), new Vector3(-5f, MidY, -14f));
            Ramp(geo, "Stair_LowerFore", new Vector3(-5f, LowerY, 6f), new Vector3(-5f, MidY, 14f));

            // Dek atas: lantai x[-7,3] penuh, jalur kanan x[3,7] kecuali celah z[-4,4] untuk tangga dek tengah ke dek atas.
            Box(geo, "UpperDeck_Main", new Vector3(-2f, UpperY - t / 2f, 0f), new Vector3(10f, t, l * 2f), floorMat, PhysicsMasks.TerrainLayer);
            Box(geo, "UpperDeck_StarboardAft", new Vector3(5f, UpperY - t / 2f, -12f), new Vector3(4f, t, 16f), floorMat, PhysicsMasks.TerrainLayer);
            Box(geo, "UpperDeck_StarboardFore", new Vector3(5f, UpperY - t / 2f, 12f), new Vector3(4f, t, 16f), floorMat, PhysicsMasks.TerrainLayer);
            Ramp(geo, "Stair_MidUpper", new Vector3(5f, MidY, -4f), new Vector3(5f, UpperY, 4f));

            // Lambung dan pagar sampai 1 m di atas dek atas.
            float wallTop = UpperY + 1f, wallH = wallTop - LowerY;
            Box(geo, "Hull_Port", new Vector3(-w - 0.25f, LowerY + wallH / 2f, 0f), new Vector3(0.5f, wallH, l * 2f), wallMat, PhysicsMasks.ObstacleLayer);
            Box(geo, "Hull_Starboard", new Vector3(w + 0.25f, LowerY + wallH / 2f, 0f), new Vector3(0.5f, wallH, l * 2f), wallMat, PhysicsMasks.ObstacleLayer);
            Box(geo, "Hull_Aft", new Vector3(0f, LowerY + wallH / 2f, -l - 0.25f), new Vector3(w * 2f + 1f, wallH, 0.5f), wallMat, PhysicsMasks.ObstacleLayer);
            Box(geo, "Hull_Fore", new Vector3(0f, LowerY + wallH / 2f, l + 0.25f), new Vector3(w * 2f + 1f, wallH, 0.5f), wallMat, PhysicsMasks.ObstacleLayer);
        }

        private static void Ramp(Transform parent, string name, Vector3 bottom, Vector3 top)
        {
            Vector3 dir = top - bottom;
            float len = dir.magnitude;
            var go = Box(parent, name, (bottom + top) / 2f, new Vector3(4f, FloorThickness, len), floorMat, PhysicsMasks.TerrainLayer);
            go.transform.localRotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
            go.transform.localPosition -= go.transform.localRotation * Vector3.up * (FloorThickness / 2f);
        }

        private static ShipCorridor BuildCorridor(Transform ship, string id, Vector3 center, Vector3 size, Vector3 exit)
        {
            var root = Child(ship, id, center);
            var corridor = root.gameObject.AddComponent<ShipCorridor>();
            var debris = Box(root, "Debris", new Vector3(0f, size.y / 2f, 0f), size, debrisMat, PhysicsMasks.ObstacleLayer);
            var obstacle = debris.AddComponent<NavMeshObstacle>();
            obstacle.carving = true;
            obstacle.shape = NavMeshObstacleShape.Box;
            obstacle.size = Vector3.one;
            var warning = Box(root, "Warning", new Vector3(0f, 0.05f, 0f), new Vector3(size.x, 0.05f, size.z), outlineMat, PhysicsMasks.ObstacleLayer);
            Object.DestroyImmediate(warning.GetComponent<Collider>());
            var exitPoint = Child(ship, id + "_Exit", exit);

            // blockVolume dibaca di ruang dunia (ShipCorridor.Collapse memakai posisi pemain langsung).
            var col = debris.GetComponent<BoxCollider>();
            Physics.SyncTransforms();
            var bounds = col.bounds;

            Set(corridor, "corridorId", id);
            Set(corridor, "debrisRoot", debris);
            Set(corridor, "navObstacle", obstacle);
            Set(corridor, "warningVfx", warning);
            Set(corridor, "exitPoint", exitPoint);
            var so = new SerializedObject(corridor);
            so.FindProperty("blockVolume").boundsValue = bounds;
            so.ApplyModifiedPropertiesWithoutUndo();
            return corridor;
        }

        private static ShipZombiePoint ZPoint(Transform parent, string group, Vector3 pos, bool wakeOnAlarm)
        {
            var t = Child(parent, $"ZombiePoint_{group}_{parent.childCount}", pos);
            var p = t.gameObject.AddComponent<ShipZombiePoint>();
            Set(p, "groupId", group);
            var so = new SerializedObject(p);
            so.FindProperty("wakeOnAlarm").boolValue = wakeOnAlarm;
            so.ApplyModifiedPropertiesWithoutUndo();
            return p;
        }

        // ---------------------------------------------------------------- UI kapal

        private static RunSummaryPanel BuildRunSummary(Transform canvas)
        {
            // UIFocusStack.md: RunSummary menjeda, melepas kursor, memblokir input, tidak ditutup Esc, terkunci.
            var panel = CreatePanel<RunSummaryPanel>(canvas, "RunSummaryPanel", UIPanelId.RunSummary,
                new UIPanelSettings { pausesTime = true, releasesCursor = true, blocksGameplayInput = true, closableByEsc = false, isLocked = true, rejectsNewPanels = false });
            var list = VList(panel.transform);
            Set(panel, "titleText", Text(list, "Title", "SELAMAT DARI KAPAL", 56));
            Set(panel, "itemsText", Text(list, "Items", "", 30));
            Set(panel, "rescuedText", Text(list, "Rescued", "", 30));
            var lost = Text(list, "Lost", "", 30);
            Set(panel, "lostSection", lost.gameObject);
            Set(panel, "lostText", lost);
            Set(panel, "conditionText", Text(list, "Condition", "", 30));
            var time = Text(list, "Time", "", 30);
            Set(panel, "timeSection", time.gameObject);
            Set(panel, "timeText", time);
            Set(panel, "continueButton", Button(list, "ContinueButton", "Lanjut ke Pulau"));
            var group = panel.GetComponent<CanvasGroup>();
            group.alpha = 0f;
            group.blocksRaycasts = false;
            group.interactable = false;
            panel.gameObject.SetActive(false);
            return panel;
        }

        private static void BuildHud(Transform canvas, GameObject player)
        {
            // HUDManager polos untuk kapal: vital, prompt interaksi, crosshair, berat tangan, penanda dan isi sekoci.
            var hudRoot = Rect(canvas, "HUD");
            Stretch(hudRoot);
            var hud = hudRoot.gameObject.AddComponent<HUDManager>();

            var vitals = Rect(hudRoot, "Vitals");
            vitals.anchorMin = vitals.anchorMax = vitals.pivot = new Vector2(0f, 0f);
            vitals.anchoredPosition = new Vector2(32, 32);
            vitals.sizeDelta = new Vector2(360, 80);
            var vl = vitals.gameObject.AddComponent<VerticalLayoutGroup>();
            vl.spacing = 8;
            vl.childControlHeight = false;
            vl.childForceExpandHeight = false;
            Set(hud, "healthBar", Bar(vitals, "HealthBar", new Color(0.8f, 0.2f, 0.2f)));
            Set(hud, "staminaBar", Bar(vitals, "StaminaBar", new Color(0.9f, 0.85f, 0.3f)));

            var cross = Rect(hudRoot, "Crosshair");
            cross.anchorMin = cross.anchorMax = new Vector2(0.5f, 0.5f);
            cross.sizeDelta = new Vector2(6, 6);
            cross.gameObject.AddComponent<Image>().color = Color.white;
            Set(hud, "crosshairRoot", cross);

            var prompt = Text(hudRoot, "InteractionPrompt", "", 30);
            var prt = (RectTransform)prompt.transform;
            prt.anchorMin = prt.anchorMax = new Vector2(0.5f, 0.5f);
            prt.anchoredPosition = new Vector2(0, -70);
            prt.sizeDelta = new Vector2(900, 44);
            Set(hud, "interactionPromptText", prompt);

            var progress = Rect(hudRoot, "InteractionProgress");
            progress.anchorMin = progress.anchorMax = new Vector2(0.5f, 0.5f);
            progress.sizeDelta = new Vector2(48, 48);
            var pimg = progress.gameObject.AddComponent<Image>();
            pimg.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
            pimg.type = Image.Type.Filled;
            pimg.fillMethod = Image.FillMethod.Radial360;
            pimg.fillAmount = 0f;
            pimg.color = new Color(1f, 1f, 1f, 0.6f);
            Set(hud, "interactionProgress", pimg);

            var hand = Text(hudRoot, "ShipHandWeight", "", 30);
            var hrt = (RectTransform)hand.transform;
            hrt.anchorMin = hrt.anchorMax = hrt.pivot = new Vector2(1f, 0f);
            hrt.anchoredPosition = new Vector2(-32, 32);
            hrt.sizeDelta = new Vector2(400, 44);
            hand.alignment = TextAlignmentOptions.Right;
            Set(hud, "shipHandWeight", hand);

            var contents = Text(hudRoot, "LifeboatContents", "", 26);
            var crt = (RectTransform)contents.transform;
            crt.anchorMin = crt.anchorMax = crt.pivot = new Vector2(1f, 0f);
            crt.anchoredPosition = new Vector2(-32, 80);
            crt.sizeDelta = new Vector2(500, 40);
            contents.alignment = TextAlignmentOptions.Right;
            Set(hud, "lifeboatContentsText", contents);

            var marker = Text(hudRoot, "LifeboatMarker", "SEKOCI: DEK ATAS", 30);
            var mrt = (RectTransform)marker.transform;
            mrt.anchorMin = mrt.anchorMax = new Vector2(0.5f, 1f);
            mrt.anchoredPosition = new Vector2(0, -150);
            marker.color = new Color(1f, 0.6f, 0.2f);
            Set(hud, "lifeboatMarker", marker.gameObject);

            var safe = Text(hudRoot, "SafeZoneLine", "Masuk area aman sekoci!", 30);
            var srt = (RectTransform)safe.transform;
            srt.anchorMin = srt.anchorMax = new Vector2(0.5f, 1f);
            srt.anchoredPosition = new Vector2(0, -195);
            safe.color = new Color(1f, 0.3f, 0.3f);
            Set(hud, "safeZoneLine", safe.gameObject);
        }

        private static void BuildTimer(Transform canvas)
        {
            // ShipTimerManager: teks timer TMP, Image vignette, AudioSource detak (README Langkah 2 nomor 1).
            var vignette = Rect(canvas, "Vignette");
            Stretch(vignette);
            vignette.SetAsFirstSibling();
            var vimg = vignette.gameObject.AddComponent<Image>();
            vimg.color = new Color(0.6f, 0f, 0f, 0f);
            vimg.raycastTarget = false;

            var go = Rect(canvas, "ShipTimer");
            go.anchorMin = go.anchorMax = go.pivot = new Vector2(0.5f, 1f);
            go.anchoredPosition = new Vector2(0, -32);
            go.sizeDelta = new Vector2(300, 90);
            var text = go.gameObject.AddComponent<TextMeshProUGUI>();
            text.text = "60";
            text.fontSize = 72;
            text.alignment = TextAlignmentOptions.Center;
            text.raycastTarget = false;
            var tick = go.gameObject.AddComponent<AudioSource>();
            tick.playOnAwake = false;
            var timer = go.gameObject.AddComponent<ShipTimerManager>();
            Set(timer, "timerText", text);
            Set(timer, "timerRect", go);
            Set(timer, "vignetteOverlay", vimg);
            Set(timer, "tickSource", tick);
        }

        // ---------------------------------------------------------------- Bahan

        internal static GameObject Box(Transform parent, string name, Vector3 localCenter, Vector3 size, Material mat, string layer)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localCenter;
            go.transform.localScale = size;
            go.layer = Layer(layer);
            go.GetComponent<MeshRenderer>().sharedMaterial = mat;
            return go;
        }

        internal static Transform Child(Transform parent, string name, Vector3 localPos)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPos;
            return go.transform;
        }

        internal static Canvas CreateCanvas(string name, int order)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.layer = LayerMask.NameToLayer("UI");
            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = order;
            var scaler = go.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            go.AddComponent<GraphicRaycaster>();
            return canvas;
        }

        internal static T CreatePanel<T>(Transform canvas, string name, UIPanelId id, UIPanelSettings settings) where T : UIPanel
        {
            var rt = Rect(canvas, name);
            Stretch(rt);
            rt.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.85f);
            var panel = rt.gameObject.AddComponent<T>();
            panel.id = id;
            panel.settings = settings;
            return panel;
        }

        internal static Transform VList(Transform parent)
        {
            var rt = Rect(parent, "Content");
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(900, 800);
            var layout = rt.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.spacing = 14;
            layout.childControlWidth = true;
            layout.childControlHeight = false;
            layout.childForceExpandHeight = false;
            return rt;
        }

        internal static TMP_Text Text(Transform parent, string name, string text, float size)
        {
            var rt = Rect(parent, name);
            rt.sizeDelta = new Vector2(900, size * 1.4f);
            var tmp = rt.gameObject.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = size;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.white;
            tmp.raycastTarget = false;
            return tmp;
        }

        internal static Button Button(Transform parent, string name, string label)
        {
            var rt = Rect(parent, name);
            rt.sizeDelta = new Vector2(500, 72);
            var image = rt.gameObject.AddComponent<Image>();
            image.color = new Color(0.18f, 0.22f, 0.2f, 1f);
            var button = rt.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            var t = Text(rt, "Label", label, 34);
            Stretch((RectTransform)t.transform);
            return button;
        }

        internal static Slider Bar(Transform parent, string name, Color color)
        {
            var rt = Rect(parent, name);
            rt.sizeDelta = new Vector2(360, 20);
            rt.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.5f);
            var fillArea = Rect(rt, "Fill Area");
            Stretch(fillArea);
            var fill = Rect(fillArea, "Fill");
            Stretch(fill);
            fill.gameObject.AddComponent<Image>().color = color;
            var slider = rt.gameObject.AddComponent<Slider>();
            slider.fillRect = fill;
            slider.interactable = false;
            slider.transition = Selectable.Transition.None;
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = 1f;
            return slider;
        }

        internal static RectTransform Rect(Transform parent, string name)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            go.layer = LayerMask.NameToLayer("UI");
            return (RectTransform)go.transform;
        }

        internal static void Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
        }

        // ---------------------------------------------------------------- Utilitas

        internal static int Layer(string name)
        {
            int l = LayerMask.NameToLayer(name);
            if (l < 0)
            {
                Debug.LogError($"[Langkah2Setup] Layer '{name}' belum ada (PhysicsLayers.md).");
                return 0;
            }
            return l;
        }

        internal static T LoadOrCreate<T>(string path) where T : ScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null) return asset;
            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }

        internal static GameObject SavePrefab(GameObject go, string path)
        {
            var prefab = PrefabUtility.SaveAsPrefabAsset(go, path);
            Object.DestroyImmediate(go);
            return prefab;
        }

        private static SerializedProperty Prop(Object target, string field, out SerializedObject so)
        {
            so = new SerializedObject(target);
            var prop = so.FindProperty(field);
            if (prop == null) Debug.LogError($"[Langkah2Setup] Field '{field}' tidak ada di {target.GetType().Name}.");
            return prop;
        }

        internal static void Set(Object target, string field, Object value)
        {
            var prop = Prop(target, field, out var so);
            if (prop == null) return;
            prop.objectReferenceValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        internal static void Set(Object target, string field, string value)
        {
            var prop = Prop(target, field, out var so);
            if (prop == null) return;
            prop.stringValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        internal static void SetFloat(Object target, string field, float value)
        {
            var prop = Prop(target, field, out var so);
            if (prop == null) return;
            prop.floatValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        internal static void SetVector(Object target, string field, Vector3 value)
        {
            var prop = Prop(target, field, out var so);
            if (prop == null) return;
            prop.vector3Value = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        internal static void SetArray(Object target, string field, Object[] values)
        {
            var prop = Prop(target, field, out var so);
            if (prop == null) return;
            prop.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++) prop.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        internal static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            string parent = Path.GetDirectoryName(path).Replace('\\', '/');
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, Path.GetFileName(path));
        }
    }
}
