using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SurvivalIsland.EditorTools.Langkah2Setup;

namespace SurvivalIsland.EditorTools
{
    /// <summary>
    /// Penyiapan Unity untuk Langkah 4 (LOGIC/README.md, "Penyiapan di Unity untuk Langkah 4"). Membuat:
    /// - ItemData bahan, alat, obat, tas, dan kit dari resep inti CraftingStation.md (angka dari blueprint saja);
    /// - CraftingRecipe resep inti di Resources/Recipes, BuildableData + prefab struktur greybox, ResourceNodeData;
    /// - viewmodel greybox Kapak, Beliung (MeleeCombatSystem), dan Palu (HammerEquippable);
    /// - di IslandPhase: Terrain greybox (pasir di selatan, rumput di utara) dengan TerrainSurfaceSource yang dibake,
    ///   node pohon, batu, jamur, dan pohon getah, manager Langkah 4, CraftingPanel dan BuildMenu polos;
    /// - InfectionSystem di Player_Island dan ResourceDropGuard di WorldItem_Generic.
    /// Jalankan setelah menu Langkah 3 (menu Langkah 3 membangun ulang IslandPhase dan Player_Island).
    /// </summary>
    public static class Langkah4Setup
    {
        private const string ItemFolder = "Assets/Resources/Items";
        private const string RecipeFolder = "Assets/Resources/Recipes";
        private const string BuildFolder = "Assets/Data/Buildables";
        private const string NodeFolder = "Assets/Data/ResourceNodes";
        private const string SurfaceFolder = "Assets/Data/Surface";
        private const string StructurePrefabFolder = "Assets/Prefabs/Structures";
        private const string NodePrefabFolder = "Assets/Prefabs/ResourceNodes";
        private const string PlayerFolder = "Assets/Prefabs/Player";
        private const string IslandScenePath = "Assets/Scenes/IslandPhase.unity";
        private const string PlayerPath = "Assets/Prefabs/Player/Player_Island.prefab";
        private const string WorldItemPath = "Assets/Prefabs/Items/WorldItem_Generic.prefab";

        // GameBalanceConfig.build.gridSize dan levelHeight (2,5 m), dipakai untuk ukuran prefab greybox.
        private const float Cell = 2.5f, Level = 2.5f;

        private static readonly Dictionary<string, ItemData> items = new Dictionary<string, ItemData>();

        [MenuItem("Survival Island/Setup/Langkah 4 - Survival dan Bangun")]
        public static void Run()
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(PlayerPath) == null)
            {
                Debug.LogError("[Langkah4Setup] Player_Island belum ada. Jalankan menu Langkah 3 dulu.");
                return;
            }
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            foreach (var f in new[] { ItemFolder, RecipeFolder, BuildFolder, NodeFolder, SurfaceFolder, StructurePrefabFolder, NodePrefabFolder })
                EnsureFolder(f);

            CreateMaterials();
            var vmMat = Mat("Greybox_Viewmodel", new Color(0.2f, 0.2f, 0.22f));
            var woodMat = Mat("Greybox_WoodStructure", new Color(0.55f, 0.38f, 0.22f));
            var leafMat = Mat("Greybox_Leaf", new Color(0.2f, 0.5f, 0.2f));
            var rockMat = Mat("Greybox_Rock", new Color(0.45f, 0.45f, 0.47f));
            var herbMat = Mat("Greybox_Herb", new Color(0.8f, 0.75f, 0.55f));
            var ghostOk = Mat("Greybox_GhostValid", new Color(0.2f, 0.9f, 0.3f, 0.4f), unlit: true, transparent: true);
            var ghostBad = Mat("Greybox_GhostInvalid", new Color(0.95f, 0.2f, 0.2f, 0.4f), unlit: true, transparent: true);

            items.Clear();
            CreateItems(vmMat);
            int recipeCount = CreateRecipes();
            var buildables = CreateBuildables(woodMat);
            var nodes = CreateNodeData();

            PatchPlayer();
            PatchWorldItem();
            BuildIslandAdditions(buildables, nodes, ghostOk, ghostBad, woodMat, leafMat, rockMat, herbMat);

            AssetDatabase.SaveAssets();
            Debug.Log($"[Langkah4Setup] Selesai: {items.Count} ItemData, {recipeCount} resep, {buildables.Count} BuildableData, " +
                      $"{nodes.Count} ResourceNodeData, Terrain IslandPhase dengan peta permukaan, dan manager Langkah 4.");
        }

        // ---------------------------------------------------------------- Item

        private static ItemData Item(string id, ItemType type, int maxStack)
        {
            var item = LoadOrCreate<ItemData>($"{ItemFolder}/{id}.asset");
            item.itemID = id;
            item.type = type;
            item.maxStack = maxStack;
            items[id] = item;
            EditorUtility.SetDirty(item);
            return item;
        }

        private static void CreateItems(Material vmMat)
        {
            // Bahan (CarryCapacity.md: kayu dan batu 100, bahan lain 20). ItemType bahan tidak tertulis: nilai awal Material.
            // fuelSeconds: Mat_Wood 60, Mat_Charcoal 120 (CampfireManager.md).
            Item("Mat_Wood", ItemType.Material, 100).fuelSeconds = 60f;
            Item("Mat_Stone", ItemType.Material, 100);
            foreach (var id in new[] { "Mat_Cloth", "Mat_Rope", "Mat_Hide", "Mat_TreeSap", "Mat_Mushroom" }) Item(id, ItemType.Material, 20);
            Item("Mat_Charcoal", ItemType.Material, 20).fuelSeconds = 120f;

            // Alat. ItemType alat tidak tertulis; dipakai Tool (LuckyMachine.md menyebut alat sebagai ItemType.Tool).
            var axe = Item("Tool_Axe", ItemType.Tool, 1);
            axe.handUse = HandUse.TwoHand;
            axe.toolClass = ToolClass.Axe;
            axe.maxDurability = 100f;   // GameBalanceConfig.md: 100 ayunan per kapak; ToolRepairSystem.md contoh maxDurability 100
            Repair(axe, CraftingStationType.WoodBench, ("Mat_Wood", 2), ("Mat_Stone", 1));
            axe.equipPrefab = MeleeViewmodel("Viewmodel_Axe", vmMat, new Vector3(0.05f, 0.5f, 0.05f));

            var pick = Item("Tool_Pickaxe", ItemType.Tool, 1);
            pick.handUse = HandUse.TwoHand;
            pick.toolClass = ToolClass.Pickaxe;
            pick.maxDurability = -1f;   // Tidak ada angka di blueprint (menunggu developer)
            Repair(pick, CraftingStationType.WoodBench, ("Mat_Wood", 2), ("Mat_Stone", 1));
            pick.equipPrefab = MeleeViewmodel("Viewmodel_Pickaxe", vmMat, new Vector3(0.4f, 0.05f, 0.05f));

            var hammer = Item("Tool_Hammer", ItemType.Tool, 1);
            hammer.handUse = HandUse.OneHand;
            hammer.maxDurability = -1f;  // Tidak ada angka di blueprint (menunggu developer)
            Repair(hammer, CraftingStationType.Hand, ("Mat_Wood", 1), ("Mat_Stone", 1));
            hammer.equipPrefab = HammerViewmodel(vmMat);

            // Obor: OffHand, tidak bisa diperbaiki. Viewmodel TorchController dibuat bersama penyiapan cahaya (Langkah 6).
            var torch = Item("Tool_Torch", ItemType.Tool, 1);
            torch.handUse = HandUse.OffHand;
            torch.isRepairable = false;

            Item("Kit_Campfire", ItemType.Material, 1);   // Aksi khusus (ItemUseSystem.md), ItemType tidak tertulis

            var clothes = Item("Gear_WarmClothes", ItemType.Tool, 1);   // WarmthSystem.md: ItemType.Tool
            clothes.isWarmClothing = true;

            Item("Bag_Cloth", ItemType.Material, 1).bagBonusSlots = 5;     // CarryCapacity.md
            Item("Bag_Leather", ItemType.Material, 1).bagBonusSlots = 10;

            // Obat (DiseaseManager.md, ItemUseSystem.md). maxStack obat selain vaksin tidak tertulis: nilai awal 1.
            var norit = Item("Med_Norit", ItemType.Consumable, 1);
            norit.isConsumable = true;
            norit.useSeconds = 2f;
            norit.curesDiseases = DiseaseCure.FoodPoisoning | DiseaseCure.Venom;
            var antibiotic = Item("Med_Antibiotic", ItemType.Consumable, 1);
            antibiotic.isConsumable = true;
            antibiotic.useSeconds = 2f;
            antibiotic.curesDiseases = DiseaseCure.WoundInfection;
            var bandage = AssetDatabase.LoadAssetAtPath<ItemData>($"{ItemFolder}/Med_Bandage.asset");
            if (bandage != null)
            {
                bandage.curesDiseases = DiseaseCure.Bleeding;   // DiseaseManager.md
                EditorUtility.SetDirty(bandage);
                items["Med_Bandage"] = bandage;
            }
            var vaccine = AssetDatabase.LoadAssetAtPath<ItemData>($"{ItemFolder}/Med_Vaccine.asset");
            if (vaccine != null) items["Med_Vaccine"] = vaccine;
        }

        private static void Repair(ItemData item, CraftingStationType station, params (string id, int amount)[] cost)
        {
            // ToolRepairSystem.md: biaya perbaikan per alat.
            item.isRepairable = true;
            item.repairStation = station;
            item.repairCost = new RecipeIngredient[cost.Length];
            for (int i = 0; i < cost.Length; i++)
                item.repairCost[i] = new RecipeIngredient { item = items[cost[i].id], amount = cost[i].amount };
        }

        private static EquippableBehaviour MeleeViewmodel(string name, Material mat, Vector3 headSize)
        {
            var root = new GameObject(name);
            var handle = Box(root.transform, "Handle", new Vector3(0f, 0f, 0.15f), new Vector3(0.04f, 0.04f, 0.5f), mat, PhysicsMasks.PlayerViewmodelLayer);
            Object.DestroyImmediate(handle.GetComponent<Collider>());
            var head = Box(root.transform, "Head", new Vector3(0f, 0f, 0.4f), headSize, mat, PhysicsMasks.PlayerViewmodelLayer);
            Object.DestroyImmediate(head.GetComponent<Collider>());
            var src = root.AddComponent<AudioSource>();
            src.playOnAwake = false;
            var melee = root.AddComponent<MeleeCombatSystem>();
            Set(melee, "audioSource", src);
            SetLayer(root, PhysicsMasks.PlayerViewmodelLayer);
            return SavePrefab(root, $"{PlayerFolder}/{name}.prefab").GetComponent<EquippableBehaviour>();
        }

        private static EquippableBehaviour HammerViewmodel(Material mat)
        {
            var root = new GameObject("Viewmodel_Hammer");
            var handle = Box(root.transform, "Handle", new Vector3(0f, 0f, 0.1f), new Vector3(0.03f, 0.03f, 0.3f), mat, PhysicsMasks.PlayerViewmodelLayer);
            Object.DestroyImmediate(handle.GetComponent<Collider>());
            var head = Box(root.transform, "Head", new Vector3(0f, 0f, 0.25f), new Vector3(0.12f, 0.05f, 0.05f), mat, PhysicsMasks.PlayerViewmodelLayer);
            Object.DestroyImmediate(head.GetComponent<Collider>());
            root.AddComponent<HammerEquippable>();
            SetLayer(root, PhysicsMasks.PlayerViewmodelLayer);
            return SavePrefab(root, $"{PlayerFolder}/Viewmodel_Hammer.prefab").GetComponent<EquippableBehaviour>();
        }

        // ---------------------------------------------------------------- Resep

        private static int CreateRecipes()
        {
            // CraftingStation.md "Resep inti". Semua startsUnlocked = true, techNodeId kosong (TechTreeManager.md).
            var defs = new (string id, string result, float secs, CraftingStationType st, (string, int)[] ing)[]
            {
                ("rec_axe", "Tool_Axe", 4, CraftingStationType.Hand, new[] { ("Mat_Wood", 3), ("Mat_Stone", 2) }),
                ("rec_pickaxe", "Tool_Pickaxe", 4, CraftingStationType.Hand, new[] { ("Mat_Wood", 3), ("Mat_Stone", 3) }),
                ("rec_hammer", "Tool_Hammer", 3, CraftingStationType.Hand, new[] { ("Mat_Wood", 2), ("Mat_Stone", 1) }),
                ("rec_torch", "Tool_Torch", 2, CraftingStationType.Hand, new[] { ("Mat_Wood", 1), ("Mat_Cloth", 1) }),
                ("rec_campfire", "Kit_Campfire", 3, CraftingStationType.Hand, new[] { ("Mat_Wood", 5), ("Mat_Stone", 3) }),
                ("rec_warm_clothes", "Gear_WarmClothes", 15, CraftingStationType.WoodBench, new[] { ("Mat_Hide", 4), ("Mat_Rope", 2) }),
                ("rec_vaccine", "Med_Vaccine", 10, CraftingStationType.MedicalBench, new[] { ("Mat_TreeSap", 2), ("Mat_Mushroom", 2) }),
                ("rec_antibiotic", "Med_Antibiotic", 8, CraftingStationType.MedicalBench, new[] { ("Mat_TreeSap", 1), ("Mat_Mushroom", 1) }),
                ("rec_norit", "Med_Norit", 3, CraftingStationType.Hand, new[] { ("Mat_Charcoal", 2) }),
                ("rec_bandage", "Med_Bandage", 3, CraftingStationType.Hand, new[] { ("Mat_Cloth", 2) }),
                ("rec_bag_cloth", "Bag_Cloth", 5, CraftingStationType.Hand, new[] { ("Mat_Cloth", 6), ("Mat_Rope", 2) }),
                ("rec_bag_leather", "Bag_Leather", 10, CraftingStationType.WoodBench, new[] { ("Mat_Hide", 6), ("Mat_Rope", 4), ("Bag_Cloth", 1) }),
            };
            foreach (var d in defs)
            {
                var r = LoadOrCreate<CraftingRecipe>($"{RecipeFolder}/{d.id}.asset");
                r.recipeId = d.id;
                r.result = items[d.result];
                r.resultAmount = 1;
                r.craftSeconds = d.secs;
                r.station = d.st;
                r.techNodeId = string.Empty;
                r.startsUnlocked = true;
                r.ingredients = new RecipeIngredient[d.ing.Length];
                for (int i = 0; i < d.ing.Length; i++) r.ingredients[i] = new RecipeIngredient { item = items[d.ing[i].Item1], amount = d.ing[i].Item2 };
                EditorUtility.SetDirty(r);
            }
            return defs.Length;
        }

        // ---------------------------------------------------------------- Bangunan

        private static List<BuildableData> CreateBuildables(Material wood)
        {
            // BuildMenu.md tabel struktur awal dan CraftingStation.md (meja). Kategori tidak tertulis untuk struktur dasar:
            // dipakai Structure untuk lantai/dinding/pintu/atap/tangga, Furniture untuk kasur/peti/api unggun/meja.
            // Build_BarbedWire dan Build_Lamp dilewati karena Mat_Scrap belum punya data item.
            var list = new List<BuildableData>
            {
                Buildable("Floor_Wood", BuildCategory.Structure, BuildSlotKind.Cell, SupportRule.GroundOrWall, NavObstacleMode.None, 200, Shape(wood, "Floor_Wood", new Vector3(Cell, 0.2f, Cell), -0.1f), ("Mat_Wood", 10)),
                Buildable("Wall_Wood", BuildCategory.Structure, BuildSlotKind.Edge, SupportRule.FloorOrGround, NavObstacleMode.Carve, 300, Shape(wood, "Wall_Wood", new Vector3(Cell, Level, 0.2f), Level / 2f, "BaseWall"), ("Mat_Wood", 12)),
                Buildable("Door_Wood", BuildCategory.Structure, BuildSlotKind.Edge, SupportRule.FloorOrGround, NavObstacleMode.Carve, 200, Shape(wood, "Door_Wood", new Vector3(Cell * 0.5f, Level * 0.85f, 0.15f), Level * 0.425f, "BaseDoor"), ("Mat_Wood", 10)),
                Buildable("Roof_Wood", BuildCategory.Structure, BuildSlotKind.Cell, SupportRule.WallBelow, NavObstacleMode.None, 150, Shape(wood, "Roof_Wood", new Vector3(Cell, 0.2f, Cell), 0.1f), ("Mat_Wood", 12)),
                Buildable("Stairs_Wood", BuildCategory.Structure, BuildSlotKind.Cell, SupportRule.FloorOrGround, NavObstacleMode.None, 200, StairsShape(wood), ("Mat_Wood", 16)),
                Buildable("Bed_Basic", BuildCategory.Furniture, BuildSlotKind.Cell, SupportRule.FloorOrGround, NavObstacleMode.Avoid, 100, Shape(wood, "Bed_Basic", new Vector3(1f, 0.5f, 2f), 0.25f), ("Mat_Wood", 20)),
                Buildable("Chest_Wood", BuildCategory.Furniture, BuildSlotKind.Cell, SupportRule.FloorOrGround, NavObstacleMode.Avoid, 150, Shape(wood, "Chest_Wood", new Vector3(1f, 0.7f, 0.6f), 0.35f), ("Mat_Wood", 20)),
                Buildable("Build_Campfire", BuildCategory.Furniture, BuildSlotKind.Cell, SupportRule.FloorOrGround, NavObstacleMode.Avoid, 100, Shape(wood, "Build_Campfire", new Vector3(1.2f, 0.3f, 1.2f), 0.15f)),
                Buildable("Build_SpikeTrap", BuildCategory.Defense, BuildSlotKind.Cell, SupportRule.FloorOrGround, NavObstacleMode.None, 60, Shape(wood, "Build_SpikeTrap", new Vector3(2f, 0.3f, 2f), 0.15f), ("Mat_Wood", 6), ("Mat_Stone", 2)),
                Buildable("Build_RainCollector", BuildCategory.Furniture, BuildSlotKind.Cell, SupportRule.FloorOrGround, NavObstacleMode.Avoid, 80, Shape(wood, "Build_RainCollector", new Vector3(1.2f, 1.2f, 1.2f), 0.6f), ("Mat_Wood", 6), ("Mat_Cloth", 2)),
                Buildable("Build_WoodBench", BuildCategory.Furniture, BuildSlotKind.Cell, SupportRule.FloorOrGround, NavObstacleMode.Avoid, 150, Bench(wood, "Build_WoodBench", CraftingStationType.WoodBench), ("Mat_Wood", 15)),
                Buildable("Build_MedicalBench", BuildCategory.Furniture, BuildSlotKind.Cell, SupportRule.FloorOrGround, NavObstacleMode.Avoid, 150, Bench(wood, "Build_MedicalBench", CraftingStationType.MedicalBench), ("Mat_Wood", 15), ("Mat_Stone", 4)),
            };
            return list;
        }

        private static BuildableData Buildable(string id, BuildCategory cat, BuildSlotKind slot, SupportRule support, NavObstacleMode nav, float hp, GameObject prefab, params (string id, int amount)[] cost)
        {
            var data = LoadOrCreate<BuildableData>($"{BuildFolder}/{id}.asset");
            data.id = id;
            data.category = cat;
            data.slotKind = slot;
            data.support = support;
            data.navMode = nav;
            data.maxHP = hp;
            data.gridSize = Vector3Int.one;   // Semua struktur awal satu sel (BuildMenu.md)
            data.isDemolishable = true;
            data.unlockNodeId = string.Empty;
            data.cost = new ItemCost[cost.Length];
            for (int i = 0; i < cost.Length; i++) data.cost[i] = new ItemCost { itemId = cost[i].id, amount = cost[i].amount };
            data.prefab = prefab;
            EditorUtility.SetDirty(data);
            // BaseHealth di prefab memakai data ini (BuildManager.Initialize juga mengisinya saat dibangun).
            var contents = PrefabUtility.LoadPrefabContents(AssetDatabase.GetAssetPath(prefab));
            Set(contents.GetComponent<BaseHealth>(), "data", data);
            PrefabUtility.SaveAsPrefabAsset(contents, AssetDatabase.GetAssetPath(prefab));
            PrefabUtility.UnloadPrefabContents(contents);
            return data;
        }

        private static GameObject Shape(Material mat, string name, Vector3 size, float centerY, string tag = null)
        {
            // README Langkah 4 nomor 5: collider di layer Structure, StructureNode, BaseHealth, pivot di lantai.
            var root = new GameObject(name);
            root.layer = Layer(PhysicsMasks.StructureLayer);
            if (tag != null) root.tag = tag;
            var body = Box(root.transform, "Body", new Vector3(0f, centerY, 0f), size, mat, PhysicsMasks.StructureLayer);
            if (tag != null) body.tag = tag;
            body.AddComponent<SurfaceTag>().type = SurfaceType.Kayu;   // SurfaceDetection.md: bangunan kayu
            root.AddComponent<StructureNode>();
            root.AddComponent<BaseHealth>();
            return SavePrefab(root, $"{StructurePrefabFolder}/{name}.prefab");
        }

        private static GameObject StairsShape(Material mat)
        {
            var root = new GameObject("Stairs_Wood");
            root.layer = Layer(PhysicsMasks.StructureLayer);
            float len = Mathf.Sqrt(Cell * Cell + Level * Level);
            var ramp = Box(root.transform, "Ramp", new Vector3(0f, Level / 2f, 0f), new Vector3(Cell * 0.8f, 0.2f, len), mat, PhysicsMasks.StructureLayer);
            ramp.transform.localRotation = Quaternion.Euler(-Mathf.Atan2(Level, Cell) * Mathf.Rad2Deg, 0f, 0f);
            ramp.AddComponent<SurfaceTag>().type = SurfaceType.Kayu;
            root.AddComponent<StructureNode>();
            root.AddComponent<BaseHealth>();
            return SavePrefab(root, $"{StructurePrefabFolder}/Stairs_Wood.prefab");
        }

        private static GameObject Bench(Material mat, string name, CraftingStationType type)
        {
            // CraftingStation.md: prefab meja mendaftar sebagai stasiun dan membuka panel lewat E. Layer Interactable.
            var root = new GameObject(name);
            root.layer = Layer(PhysicsMasks.StructureLayer);
            var body = Box(root.transform, "Body", new Vector3(0f, 0.45f, 0f), new Vector3(1.6f, 0.9f, 0.8f), mat, PhysicsMasks.InteractableLayer);
            body.AddComponent<SurfaceTag>().type = SurfaceType.Kayu;
            var station = body.AddComponent<CraftingStation>();
            var so = new SerializedObject(station);
            so.FindProperty("stationType").enumValueIndex = (int)type;
            so.FindProperty("centerPoint").objectReferenceValue = root.transform;
            so.ApplyModifiedPropertiesWithoutUndo();
            Set(station, "highlight", AddHighlight(body, PrimitiveType.Cube));
            root.AddComponent<StructureNode>();
            root.AddComponent<BaseHealth>();
            return SavePrefab(root, $"{StructurePrefabFolder}/{name}.prefab");
        }

        // ---------------------------------------------------------------- Node

        private static List<ResourceNodeData> CreateNodeData()
        {
            // ResourceRespawn.md tabel alat dan masa tumbuh, ResourceNode.md (jamur, pohon getah). ID pohon dan batu tidak
            // tertulis di blueprint (hanya contoh Node_Pine): dipakai Node_Tree dan Node_Rock.
            // Jumlah drop pohon dan batu dibaca dari config (treeDropMin/Max, rockDropMin/Max), angka aset diabaikan.
            return new List<ResourceNodeData>
            {
                Node("Node_Tree", NodeType.Tree, 5, items["Mat_Wood"], 1, 2, true,
                    new[] { (ToolClass.None, 1), (ToolClass.Axe, 3), (ToolClass.Pickaxe, 1), (ToolClass.Knife, 1) },
                    new[] { (RegionType.Volcano, 5f), (RegionType.Amazon, 7f), (RegionType.Snow, 10f), (RegionType.Logistic, 8f) }),
                Node("Node_Rock", NodeType.Rock, 6, items["Mat_Stone"], 1, 2, true,
                    new[] { (ToolClass.None, 0), (ToolClass.Axe, 1), (ToolClass.Pickaxe, 3), (ToolClass.Knife, 0) },
                    new[] { (RegionType.Volcano, 10f), (RegionType.Amazon, 10f), (RegionType.Snow, 12f), (RegionType.Logistic, 10f) }),
                Node("Node_Mushroom", NodeType.MedicalHerb, 1, items["Mat_Mushroom"], 1, 2, false,
                    new[] { (ToolClass.None, 1), (ToolClass.Axe, 1), (ToolClass.Pickaxe, 1), (ToolClass.Knife, 1) },
                    new[] { (RegionType.Amazon, 1f) }),
                Node("Node_SapTree", NodeType.MedicalHerb, 2, items["Mat_TreeSap"], 1, 2, true,
                    new[] { (ToolClass.None, 0), (ToolClass.Axe, 0), (ToolClass.Pickaxe, 0), (ToolClass.Knife, 1) },
                    new[] { (RegionType.Amazon, 2f) }),
            };
        }

        private static ResourceNodeData Node(string id, NodeType type, int hits, ItemData drop, int min, int max, bool keepCollider,
            (ToolClass tool, int dmg)[] rules, (RegionType region, float days)[] regrow)
        {
            var d = LoadOrCreate<ResourceNodeData>($"{NodeFolder}/{id}.asset");
            d.nodeTypeId = id;
            d.nodeType = type;
            d.maxHits = hits;
            d.dropItem = drop;
            d.dropMin = min;
            d.dropMax = max;
            d.keepColliderWhenDepleted = keepCollider;
            d.toolRules = new ToolRule[rules.Length];
            for (int i = 0; i < rules.Length; i++) d.toolRules[i] = new ToolRule { tool = rules[i].tool, damage = rules[i].dmg };
            d.regrow = new RegrowRule[regrow.Length];
            for (int i = 0; i < regrow.Length; i++) d.regrow[i] = new RegrowRule { region = regrow[i].region, days = regrow[i].days };
            EditorUtility.SetDirty(d);
            return d;
        }

        // ---------------------------------------------------------------- Prefab lama

        private static void PatchPlayer()
        {
            // README Langkah 4 nomor 3: InfectionSystem di samping SurvivalStats.
            var contents = PrefabUtility.LoadPrefabContents(PlayerPath);
            if (contents.GetComponent<InfectionSystem>() == null)
            {
                var inf = contents.AddComponent<InfectionSystem>();
                Set(inf, "stats", contents.GetComponent<SurvivalStats>());
            }
            PrefabUtility.SaveAsPrefabAsset(contents, PlayerPath);
            PrefabUtility.UnloadPrefabContents(contents);
        }

        private static void PatchWorldItem()
        {
            // README Langkah 4 nomor 6: WorldItem_Generic mendapat ResourceDropGuard.
            var contents = PrefabUtility.LoadPrefabContents(WorldItemPath);
            if (contents.GetComponent<ResourceDropGuard>() == null) contents.AddComponent<ResourceDropGuard>();
            PrefabUtility.SaveAsPrefabAsset(contents, WorldItemPath);
            PrefabUtility.UnloadPrefabContents(contents);
        }

        // ---------------------------------------------------------------- Scene

        private static void BuildIslandAdditions(List<BuildableData> buildables, List<ResourceNodeData> nodes, Material ghostOk, Material ghostBad,
            Material wood, Material leaf, Material rock, Material herb)
        {
            var scene = EditorSceneManager.OpenScene(IslandScenePath, OpenSceneMode.Single);
            foreach (var name in new[] { "IslandTerrain", "ResourceNodes", "Langkah4Systems" })
            {
                var old = GameObject.Find(name);
                if (old != null) Object.DestroyImmediate(old);
            }
            // Lantai kotak dari Langkah 3 diganti Terrain.
            var beach = GameObject.Find("BeachGreybox/Beach");
            if (beach != null) Object.DestroyImmediate(beach);

            BuildTerrain();
            BuildNodes(nodes, wood, leaf, rock, herb);

            var systems = new GameObject("Langkah4Systems").transform;
            Add<DayNightDirector>(systems, "DayNightDirector");
            Add<ResourceRespawn>(systems, "ResourceRespawn");
            Add<CraftingManager>(systems, "CraftingManager");
            var tech = Add<TechTreeManager>(systems, "TechTreeManager");
            SetArray(tech, "nodes", new Object[0]);   // TechTreeManager.md: node masih usulan, boleh kosong
            Add<StructureRegistry>(systems, "StructureRegistry");
            var build = Add<BuildManager>(systems, "BuildManager");
            var structureRoot = new GameObject("Structures").transform;
            structureRoot.SetParent(systems, false);
            SetArray(build, "buildables", buildables.ToArray());
            Set(build, "ghostValidMat", ghostOk);
            Set(build, "ghostInvalidMat", ghostBad);
            Set(build, "structureRoot", structureRoot);

            // Panel di canvas HUD: Crafting dan BuildMenu, keduanya releasesCursor, blocksGameplayInput, closableByEsc, tanpa jeda.
            var canvas = GameObject.Find("IslandCanvas");
            foreach (var name in new[] { "CraftingPanel", "BuildMenu" })
            {
                var old = canvas.transform.Find(name);
                if (old != null) Object.DestroyImmediate(old.gameObject);
            }
            var settings = new UIPanelSettings { pausesTime = false, releasesCursor = true, blocksGameplayInput = true, closableByEsc = true, isLocked = false, rejectsNewPanels = false };
            BuildCraftingPanel(canvas.transform, settings);
            BuildBuildMenu(canvas.transform, settings);

            EditorSceneManager.SaveScene(scene);
        }

        private static T Add<T>(Transform parent, string name) where T : Component
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            return go.AddComponent<T>();
        }

        private static void BuildTerrain()
        {
            // Terrain Region 1 (1000 x 1000 m, X 1000-2000, Z 0-1000, RegionStreaming.md), datar di Y = 0.
            // Lapisan 0 = pasir (pantai selatan tempat mendarat, Z dunia < R1Origin.z - 25), lapisan 1 = rumput.
            var sand = TerrainLayerAsset("TL_Sand", new Color(0.86f, 0.8f, 0.6f));
            var grass = TerrainLayerAsset("TL_Grass", new Color(0.3f, 0.5f, 0.25f));

            string dataPath = $"{SurfaceFolder}/IslandTerrainData.asset";
            AssetDatabase.DeleteAsset(dataPath);
            // Aset dibuat lebih dulu; alphamap yang diisi sebelum CreateAsset tidak ikut tersimpan.
            var data = new TerrainData { heightmapResolution = 129 };
            AssetDatabase.CreateAsset(data, dataPath);
            data.size = new Vector3(IslandLayout.RegionSize, 20f, IslandLayout.RegionSize);
            data.alphamapResolution = 512;
            data.terrainLayers = new[] { sand, grass };
            int res = data.alphamapResolution;
            var alpha = new float[res, res, 2];
            for (int z = 0; z < res; z++)
                for (int x = 0; x < res; x++)
                {
                    float worldZ = IslandLayout.R1Min.z + (z + 0.5f) / res * IslandLayout.RegionSize;
                    float s = Mathf.Clamp01((IslandLayout.R1Origin.z - 25f - worldZ) / 4f + 0.5f);   // Peralihan 4 m di garis pantai
                    alpha[z, x, 0] = s;
                    alpha[z, x, 1] = 1f - s;
                }
            data.SetAlphamaps(0, 0, alpha);
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();

            var go = Terrain.CreateTerrainGameObject(data);
            go.name = "IslandTerrain";
            go.transform.position = IslandLayout.R1Min;
            go.layer = Layer(PhysicsMasks.TerrainLayer);

            // SurfaceDetection.md: TerrainSurfaceSource + SurfaceLayerMap (pasir = Pasir, rumput = Rumput), lalu bake.
            var map = LoadOrCreate<SurfaceLayerMap>($"{SurfaceFolder}/IslandSurfaceLayerMap.asset");
            map.layers = new[] { sand, grass };
            map.types = new[] { SurfaceType.Pasir, SurfaceType.Rumput };
            EditorUtility.SetDirty(map);
            var source = go.AddComponent<TerrainSurfaceSource>();
            source.layerMap = map;
            // SurfaceBakeTool membuat nama unik baru tiap bake; peta lama dihapus supaya tidak ada aset yatim.
            foreach (var guid in AssetDatabase.FindAssets("IslandTerrain_SurfaceMap", new[] { "Assets/Scenes" }))
                AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(guid));
            if (!SurfaceBakeTool.Bake(source)) Debug.LogError("[Langkah4Setup] Bake peta permukaan Terrain gagal.");
        }

        private static TerrainLayer TerrainLayerAsset(string name, Color color)
        {
            string texPath = $"{SurfaceFolder}/{name}_Tex.asset";
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
            if (tex == null)
            {
                tex = new Texture2D(4, 4);
                var px = new Color[16];
                for (int i = 0; i < 16; i++) px[i] = color;
                tex.SetPixels(px);
                tex.Apply();
                AssetDatabase.CreateAsset(tex, texPath);
            }
            var layer = AssetDatabase.LoadAssetAtPath<TerrainLayer>($"{SurfaceFolder}/{name}.terrainlayer");
            if (layer == null)
            {
                layer = new TerrainLayer();
                AssetDatabase.CreateAsset(layer, $"{SurfaceFolder}/{name}.terrainlayer");
            }
            layer.diffuseTexture = tex;
            layer.tileSize = new Vector2(4f, 4f);
            EditorUtility.SetDirty(layer);
            return layer;
        }

        private static void BuildNodes(List<ResourceNodeData> nodes, Material wood, Material leaf, Material rock, Material herb)
        {
            // Node resource (README Langkah 4 nomor 6): ResourceNode + SaveIdTag, layer Obstacle, visual berdiri dan habis.
            // Letak greybox di utara titik mendarat, region Volcano (Region 1 = zona awal).
            var root = new GameObject("ResourceNodes").transform;
            root.position = IslandLayout.R1Origin;
            var tree = nodes[0]; var stone = nodes[1]; var mushroom = nodes[2]; var sap = nodes[3];
            Vector3[] trees = { new Vector3(-6, 0, -18), new Vector3(4, 0, -15), new Vector3(10, 0, -8), new Vector3(-12, 0, -6), new Vector3(0, 0, -4), new Vector3(15, 0, 2) };
            for (int i = 0; i < trees.Length; i++) TreeNode(root, $"Tree_{i:00}", trees[i], tree, wood, leaf, SurfaceType.Kayu);
            Vector3[] rocks = { new Vector3(-4, 0, -22), new Vector3(9, 0, -20), new Vector3(-15, 0, -14) };
            for (int i = 0; i < rocks.Length; i++) RockNode(root, $"Rock_{i:00}", rocks[i], stone, rock);
            HerbNode(root, "Mushroom_00", new Vector3(-2, 0, -12), mushroom, herb);
            HerbNode(root, "Mushroom_01", new Vector3(6, 0, -11), mushroom, herb);
            TreeNode(root, "SapTree_00", new Vector3(-9, 0, -10), sap, wood, herb, SurfaceType.Kayu);
        }

        private static GameObject NodeRoot(Transform parent, string name, Vector3 pos, ResourceNodeData data, out ResourceNode node)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = pos;
            go.layer = Layer(PhysicsMasks.ObstacleLayer);
            node = go.AddComponent<ResourceNode>();
            Set(node, "data", data);
            var so = new SerializedObject(node);
            so.FindProperty("region").enumValueIndex = (int)RegionType.Volcano;
            so.ApplyModifiedPropertiesWithoutUndo();
            var tag = go.AddComponent<SaveIdTag>();
            var tso = new SerializedObject(tag);
            tso.FindProperty("idPrefix").stringValue = "node";
            tso.FindProperty("saveId").stringValue = "node_" + name.ToLowerInvariant();
            tso.ApplyModifiedPropertiesWithoutUndo();
            return go;
        }

        private static void TreeNode(Transform parent, string name, Vector3 pos, ResourceNodeData data, Material trunkMat, Material leafMat, SurfaceType surface)
        {
            var go = NodeRoot(parent, name, pos, data, out var node);
            var standing = Child(go.transform, "Standing", Vector3.zero).gameObject;
            var trunk = Cyl(standing.transform, "Trunk", new Vector3(0, 2f, 0), new Vector3(0.6f, 2f, 0.6f), trunkMat);
            trunk.AddComponent<SurfaceTag>().type = surface;
            var crown = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            crown.name = "Crown";
            Object.DestroyImmediate(crown.GetComponent<Collider>());
            crown.transform.SetParent(standing.transform, false);
            crown.transform.localPosition = new Vector3(0, 4.5f, 0);
            crown.transform.localScale = Vector3.one * 3f;
            crown.GetComponent<MeshRenderer>().sharedMaterial = leafMat;
            var depleted = Child(go.transform, "Depleted", Vector3.zero).gameObject;
            var stump = Cyl(depleted.transform, "Stump", new Vector3(0, 0.3f, 0), new Vector3(0.65f, 0.3f, 0.65f), trunkMat);
            depleted.SetActive(false);
            Wire(node, standing, depleted, trunk.GetComponent<Collider>(), stump.GetComponent<Collider>());
        }

        private static void RockNode(Transform parent, string name, Vector3 pos, ResourceNodeData data, Material mat)
        {
            var go = NodeRoot(parent, name, pos, data, out var node);
            var standing = Child(go.transform, "Standing", Vector3.zero).gameObject;
            var big = Box(standing.transform, "Rock", new Vector3(0, 0.75f, 0), new Vector3(2f, 1.5f, 1.8f), mat, PhysicsMasks.ObstacleLayer);
            big.AddComponent<SurfaceTag>().type = SurfaceType.Batu;
            var depleted = Child(go.transform, "Depleted", Vector3.zero).gameObject;
            var small = Box(depleted.transform, "Rubble", new Vector3(0, 0.2f, 0), new Vector3(1.6f, 0.4f, 1.4f), mat, PhysicsMasks.ObstacleLayer);
            depleted.SetActive(false);
            Wire(node, standing, depleted, big.GetComponent<Collider>(), small.GetComponent<Collider>());
        }

        private static void HerbNode(Transform parent, string name, Vector3 pos, ResourceNodeData data, Material mat)
        {
            var go = NodeRoot(parent, name, pos, data, out var node);
            var standing = Child(go.transform, "Standing", Vector3.zero).gameObject;
            var cap = Box(standing.transform, "Herb", new Vector3(0, 0.2f, 0), new Vector3(0.5f, 0.4f, 0.5f), mat, PhysicsMasks.ObstacleLayer);
            var depleted = Child(go.transform, "Depleted", Vector3.zero).gameObject;
            depleted.SetActive(false);
            Wire(node, standing, depleted, cap.GetComponent<Collider>());
        }

        private static void Wire(ResourceNode node, GameObject standing, GameObject depleted, params Collider[] colliders)
        {
            Set(node, "standingVisual", standing);
            Set(node, "depletedVisual", depleted);
            SetArray(node, "colliders", colliders);
            var src = node.gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.spatialBlend = 1f;
            Set(node, "audioSource", src);
            SetLayer(node.gameObject, PhysicsMasks.ObstacleLayer);
        }

        private static GameObject Cyl(Transform parent, string name, Vector3 pos, Vector3 scale, Material mat)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.name = name;
            go.transform.SetParent(parent, false);
            go.transform.localPosition = pos;
            go.transform.localScale = scale;
            go.GetComponent<MeshRenderer>().sharedMaterial = mat;
            return go;
        }

        // ---------------------------------------------------------------- Panel

        private static void BuildCraftingPanel(Transform canvas, UIPanelSettings settings)
        {
            // CraftingPanel (Langkah 4): baris resep dari rowPrefab (Button + TMP_Text) dan daftar antrean.
            var panel = CreatePanel<CraftingPanel>(canvas, "CraftingPanel", UIPanelId.Crafting, settings);
            Text(panel.transform, "Title", "CRAFTING", 48).rectTransform.anchoredPosition = new Vector2(0, 460);
            var list = ListRoot(panel.transform, "RecipeList", new Vector2(-260, 0));
            var queue = ListRoot(panel.transform, "QueueList", new Vector2(420, 0));
            var row = Button(list, "RecipeRowTemplate", "resep");
            var qrow = Button(queue, "QueueRowTemplate", "antrean");
            row.gameObject.SetActive(false);
            qrow.gameObject.SetActive(false);
            Set(panel, "recipeListRoot", list);
            Set(panel, "rowPrefab", row);
            Set(panel, "queueListRoot", queue);
            Set(panel, "queueRowPrefab", qrow);
            HideAtStart(panel);
        }

        private static void BuildBuildMenu(Transform canvas, UIPanelSettings settings)
        {
            // BuildMenu (Langkah 4): cincin ditampilkan sebagai daftar tombol dari buttonPrefab.
            var panel = CreatePanel<BuildMenu>(canvas, "BuildMenu", UIPanelId.BuildMenu, settings);
            var center = Text(panel.transform, "Center", "", 36);
            center.rectTransform.anchoredPosition = new Vector2(0, 420);
            var main = ListRoot(panel.transform, "MainRing", new Vector2(-260, 0));
            var sub = ListRoot(panel.transform, "SubRing", new Vector2(260, 0));
            var button = Button(main, "ButtonTemplate", "mode");
            button.gameObject.SetActive(false);
            Set(panel, "mainRingRoot", main);
            Set(panel, "subRingRoot", sub);
            Set(panel, "buttonPrefab", button);
            Set(panel, "centerText", center);
            HideAtStart(panel);
        }

        private static Transform ListRoot(Transform parent, string name, Vector2 pos)
        {
            var rt = Rect(parent, name);
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = pos;
            rt.sizeDelta = new Vector2(520, 800);
            var layout = rt.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 8;
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = false;
            layout.childForceExpandHeight = false;
            return rt;
        }

        private static void HideAtStart(FadingUIPanel panel)
        {
            var g = panel.GetComponent<CanvasGroup>();
            g.alpha = 0f;
            g.blocksRaycasts = false;
            g.interactable = false;
            // Objek tetap aktif: CraftingPanel dan BuildMenu mengisi Instance di Awake, dan Awake tidak jalan pada objek nonaktif.
        }

        private static void SetLayer(GameObject go, string layer)
        {
            int l = Layer(layer);
            foreach (var t in go.GetComponentsInChildren<Transform>(true)) t.gameObject.layer = l;
        }
    }
}
