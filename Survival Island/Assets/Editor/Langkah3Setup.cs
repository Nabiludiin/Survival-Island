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
    /// Penyiapan Unity untuk Langkah 3 (LOGIC/README.md, "Penyiapan di Unity untuk Langkah 3") dan nomor 4 penyiapan
    /// Langkah 2 (IslandArrival). Membuat:
    /// - ItemData item inti Base_Kail, Base_Pistol, Base_Pisau (isCoreItem) dan WeaponData Base_Pistol (GunController.md);
    /// - viewmodel greybox Pistol dan Pisau, prefab WorldItem dan pool WorldItem_Generic (ObjectPooler.md);
    /// - prefab Player_Island (README Langkah 3 nomor 4);
    /// - scene IslandPhase greybox: pantai datar, IslandArrival, WorldItemManager, pemain, dan HUD polos (+ hotbar).
    /// Pantai hanyalah lantai sementara, bukan peta pulau. Menu boleh dijalankan ulang, setelah menu Langkah 1 dan 2.
    /// </summary>
    public static class Langkah3Setup
    {
        private const string ItemFolder = "Assets/Resources/Items";
        private const string WeaponFolder = "Assets/Data/Weapons";
        private const string PlayerFolder = "Assets/Prefabs/Player";
        private const string ItemPrefabFolder = "Assets/Prefabs/Items";
        private const string PersistentRootPath = "Assets/Prefabs/PersistentRoot.prefab";
        private const string IslandScenePath = "Assets/Scenes/IslandPhase.unity";

        [MenuItem("Survival Island/Setup/Langkah 3 - IslandPhase")]
        public static void Run()
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(PersistentRootPath) == null)
            {
                Debug.LogError("[Langkah3Setup] PersistentRoot belum ada. Jalankan menu Langkah 1 dulu.");
                return;
            }
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            foreach (var f in new[] { ItemFolder, WeaponFolder, PlayerFolder, ItemPrefabFolder, "Assets/Prefabs/Ship/Materials" })
                EnsureFolder(f);

            CreateMaterials();
            var vmMat = Mat("Greybox_Viewmodel", new Color(0.2f, 0.2f, 0.22f));
            var itemMat = Mat("Greybox_WorldItem", new Color(0.85f, 0.65f, 0.2f));
            var sandMat = Mat("Greybox_Sand", new Color(0.86f, 0.8f, 0.6f));

            var pistolVm = BuildPistolViewmodel(vmMat);
            var knifeVm = BuildKnifeViewmodel(vmMat);
            int coreCount = CreateCoreItems(pistolVm, knifeVm);

            var worldItem = BuildWorldItemPrefab(itemMat);
            RegisterWorldItemPool(worldItem);

            var player = BuildPlayerPrefab();
            BuildIslandScene(player, sandMat);

            AssetDatabase.SaveAssets();
            Debug.Log($"[Langkah3Setup] Selesai: {coreCount} item inti, WeaponData Base_Pistol, WorldItem + pool, Player_Island, dan scene IslandPhase.");
        }

        // ---------------------------------------------------------------- Item inti

        private static int CreateCoreItems(GameObject pistolVm, GameObject knifeVm)
        {
            // GunController.md tabel Base_Pistol: semua angka sama dengan nilai awal WeaponData, ditulis eksplisit di sini.
            var wd = LoadOrCreate<WeaponData>($"{WeaponFolder}/WD_Base_Pistol.asset");
            wd.fireMode = FireMode.SemiAuto;
            wd.damage = 30f;
            wd.secondsBetweenShots = 0.25f;
            wd.magazineSize = 12;
            wd.startingMagazine = 12;
            wd.ammoItemId = "Ammo_9mm";
            wd.reloadSeconds = 1.6f;
            wd.maxRange = 60f;
            wd.spreadBase = 0.4f;
            wd.spreadMoving = 1.5f;
            wd.spreadPerShot = 0.6f;
            wd.spreadMax = 4f;
            wd.spreadRecoverPerSec = 6f;
            wd.recoilPitch = 1.2f;
            wd.recoilYaw = 0.4f;
            wd.noiseRadius = 35f;
            wd.wearPerShot = 0.25f;
            wd.muzzleFlashTag = "VFX_Muzzle";
            wd.impactTag = "VFX_Impact";
            wd.cameraShakeScale = 0.6f;
            EditorUtility.SetDirty(wd);

            // ItemUseSystem.md / GLOSSARY.md: item inti isCoreItem = true, isDroppable = false.
            // EquipmentManager.md: Kail TwoHand, Pistol dan Pisau OneHand. ItemData.md: Pisau toolClass Knife.
            // maxDurability item inti tidak ada di blueprint, jadi tetap -1 (menunggu developer).
            // Kail belum punya viewmodel: FishingSystem butuh data ikan (Langkah 4).
            var kail = Core("Base_Kail", ItemType.Tool, HandUse.TwoHand, null);
            var pistol = Core("Base_Pistol", ItemType.Weapon, HandUse.OneHand, pistolVm.GetComponent<EquippableBehaviour>());
            pistol.weaponData = wd;
            var pisau = Core("Base_Pisau", ItemType.Weapon, HandUse.OneHand, knifeVm.GetComponent<EquippableBehaviour>());
            pisau.toolClass = ToolClass.Knife;   // weaponDamage 0 = cadangan 25 di MeleeCombatSystem (MeleeCombatSystem.md)
            foreach (var i in new[] { kail, pistol, pisau }) EditorUtility.SetDirty(i);
            return 3;
        }

        private static ItemData Core(string id, ItemType type, HandUse hand, EquippableBehaviour prefab)
        {
            var item = LoadOrCreate<ItemData>($"{ItemFolder}/{id}.asset");
            item.itemID = id;
            item.type = type;
            item.handUse = hand;
            item.equipPrefab = prefab;
            item.isCoreItem = true;
            item.isDroppable = false;
            item.maxStack = 1;
            item.maxDurability = -1f;
            return item;
        }

        // ---------------------------------------------------------------- Viewmodel

        private static GameObject BuildPistolViewmodel(Material mat)
        {
            // FirstPersonCamera.md: pose viewmodel Base_Pistol.
            var root = new GameObject("Viewmodel_Pistol");
            var body = Box(root.transform, "Body", new Vector3(0f, 0f, 0.05f), new Vector3(0.05f, 0.1f, 0.2f), mat, PhysicsMasks.PlayerViewmodelLayer);
            Object.DestroyImmediate(body.GetComponent<Collider>());
            var muzzle = Child(root.transform, "Muzzle", new Vector3(0f, 0.03f, 0.16f));
            var src = root.AddComponent<AudioSource>();
            src.playOnAwake = false;
            var gun = root.AddComponent<GunController>();
            gun.pose = new ViewmodelPose(new Vector3(0.18f, -0.16f, 0.35f), new Vector3(0f, -2f, 0f), 1f, 1f);
            Set(gun, "muzzlePoint", muzzle);
            Set(gun, "audioSource", src);
            SetLayerRecursive(root, PhysicsMasks.PlayerViewmodelLayer);
            return SavePrefab(root, $"{PlayerFolder}/Viewmodel_Pistol.prefab");
        }

        private static GameObject BuildKnifeViewmodel(Material mat)
        {
            // FirstPersonCamera.md: pose viewmodel Base_Pisau.
            var root = new GameObject("Viewmodel_Pisau");
            var blade = Box(root.transform, "Blade", new Vector3(0f, 0f, 0.08f), new Vector3(0.02f, 0.04f, 0.22f), mat, PhysicsMasks.PlayerViewmodelLayer);
            Object.DestroyImmediate(blade.GetComponent<Collider>());
            var src = root.AddComponent<AudioSource>();
            src.playOnAwake = false;
            var melee = root.AddComponent<MeleeCombatSystem>();
            melee.pose = new ViewmodelPose(new Vector3(0.22f, -0.2f, 0.38f), new Vector3(15f, -20f, 0f), 1.2f, 1f);
            Set(melee, "audioSource", src);
            SetLayerRecursive(root, PhysicsMasks.PlayerViewmodelLayer);
            return SavePrefab(root, $"{PlayerFolder}/Viewmodel_Pisau.prefab");
        }

        // ---------------------------------------------------------------- WorldItem

        private static GameObject BuildWorldItemPrefab(Material mat)
        {
            // README Langkah 3 nomor 5: layer WorldItem, Rigidbody, MeshFilter, MeshRenderer, MeshCollider convex.
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "WorldItem_Generic";
            Object.DestroyImmediate(go.GetComponent<BoxCollider>());
            go.layer = Layer(PhysicsMasks.WorldItemLayer);
            go.transform.localScale = Vector3.one * 0.35f;
            go.GetComponent<MeshRenderer>().sharedMaterial = mat;
            var col = go.AddComponent<MeshCollider>();
            col.convex = true;
            col.sharedMesh = go.GetComponent<MeshFilter>().sharedMesh;
            go.AddComponent<Rigidbody>();
            var wi = go.AddComponent<WorldItem>();
            Set(wi, "meshFilter", go.GetComponent<MeshFilter>());
            Set(wi, "meshRenderer", go.GetComponent<MeshRenderer>());
            Set(wi, "meshCollider", col);
            return SavePrefab(go, $"{ItemPrefabFolder}/WorldItem_Generic.prefab");
        }

        private static void RegisterWorldItemPool(GameObject prefab)
        {
            // ObjectPooler.md: WorldItem_Generic 128, maxSize 512, Grow (usulan di sana).
            var contents = PrefabUtility.LoadPrefabContents(PersistentRootPath);
            try
            {
                var pooler = contents.GetComponentInChildren<ObjectPooler>(true);
                Upsert(pooler.pools, WorldItem.PoolTag, prefab, 128);
                var pool = pooler.pools.Find(p => p.tag == WorldItem.PoolTag);
                pool.overflow = PoolOverflow.Grow;
                pool.maxSize = 512;
                PrefabUtility.SaveAsPrefabAsset(contents, PersistentRootPath);
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(contents);
            }
        }

        // ---------------------------------------------------------------- Pemain

        private static GameObject BuildPlayerPrefab()
        {
            // README Langkah 3 nomor 4. CharacterController sama dengan Player_Ship.
            var root = new GameObject("Player_Island");
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
            var shake = fp.transform.Find("CameraRig").gameObject.AddComponent<CameraShake>();
            Set(shake, "fpCamera", fp);

            var stats = root.AddComponent<SurvivalStats>();
            var controller = root.AddComponent<IslandPlayerController>();
            var traversal = root.AddComponent<PlayerTraversal>();
            Set(controller, "controller", cc);
            Set(controller, "traversal", traversal);
            Set(controller, "stats", stats);
            Set(traversal, "controller", cc);
            Set(traversal, "playerController", controller);
            Set(traversal, "stats", stats);

            var inventory = root.AddComponent<InventoryManager>();
            root.AddComponent<CarryCapacity>();
            var use = root.AddComponent<ItemUseSystem>();
            Set(use, "inventory", inventory);
            Set(use, "stats", stats);
            Set(use, "controller", controller);
            Set(use, "traversal", traversal);

            // Tangan kosong dan pemakaian konsumsi: anak ViewmodelRoot, nonaktif sampai dipakai EquipmentManager.
            var vmRoot = fp.transform.Find("CameraRig/PitchPivot/OffsetPivot/ViewmodelCamera/ViewmodelRoot");
            var fistGo = Child(vmRoot, "Fist", new Vector3(0.2f, -0.2f, 0.4f)).gameObject;
            var fistSrc = fistGo.AddComponent<AudioSource>();
            fistSrc.playOnAwake = false;
            var fist = fistGo.AddComponent<FistBehaviour>();
            Set(fist, "audioSource", fistSrc);
            var consumable = Child(vmRoot, "Consumable", new Vector3(0.2f, -0.2f, 0.4f)).gameObject.AddComponent<ConsumableEquippable>();
            SetLayerRecursive(vmRoot.gameObject, PhysicsMasks.PlayerViewmodelLayer);

            var equipment = root.AddComponent<EquipmentManager>();
            Set(equipment, "fistBehaviour", fist);
            Set(equipment, "consumableBehaviour", consumable);
            Set(equipment, "inventory", inventory);

            var interactor = root.AddComponent<PlayerInteractor>();
            Set(interactor, "cameraTransform", worldCamGo.transform);
            Set(interactor, "characterController", cc);

            return SavePrefab(root, $"{PlayerFolder}/Player_Island.prefab");
        }

        // ---------------------------------------------------------------- Scene

        private static void BuildIslandScene(GameObject playerPrefab, Material sandMat)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var sun = new GameObject("Directional Light").AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.intensity = 1.1f;
            sun.shadows = LightShadows.Soft;
            sun.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            // Pantai greybox: lantai datar layer Terrain, beberapa batu layer Obstacle.
            var world = new GameObject("BeachGreybox").transform;
            world.position = IslandLayout.R1Origin;   // Region 1 di peta usulan RegionStreaming.md
            Box(world, "Beach", new Vector3(0f, -0.5f, 0f), new Vector3(120f, 1f, 120f), sandMat, PhysicsMasks.TerrainLayer);
            Box(world, "Rock_A", new Vector3(8f, 0.75f, 12f), new Vector3(3f, 1.5f, 2f), wallMat, PhysicsMasks.ObstacleLayer);
            Box(world, "Rock_B", new Vector3(-10f, 1f, 18f), new Vector3(2f, 2f, 3f), wallMat, PhysicsMasks.ObstacleLayer);

            // IslandArrival di pantai selatan (README Langkah 2 nomor 4): landingPoint, lookTarget, npcOrigin, overflowOrigin.
            var arrivalGo = new GameObject("IslandArrival");
            arrivalGo.transform.position = IslandLayout.R1Origin + new Vector3(0f, 0f, -40f);
            var arrival = arrivalGo.AddComponent<IslandArrival>();
            Set(arrival, "landingPoint", Child(arrivalGo.transform, "LandingPoint", new Vector3(0f, 0.05f, 0f)));
            Set(arrival, "lookTarget", Child(arrivalGo.transform, "LookTarget", new Vector3(0f, 1.6f, 30f)));
            Set(arrival, "npcOrigin", Child(arrivalGo.transform, "NpcOrigin", new Vector3(3f, 0f, 1f)));
            Set(arrival, "overflowOrigin", Child(arrivalGo.transform, "OverflowOrigin", new Vector3(-3f, 0.3f, 1f)));

            new GameObject("WorldItemManager").AddComponent<WorldItemManager>();

            var player = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab, scene);
            player.transform.position = arrivalGo.transform.position + Vector3.up * 0.05f;

            var canvas = CreateCanvas("IslandCanvas", 10);
            BuildHud(canvas.transform);

            EditorSceneManager.SaveScene(scene, IslandScenePath);
        }

        private static void BuildHud(Transform canvas)
        {
            var hudRoot = Rect(canvas, "HUD");
            Stretch(hudRoot);
            var hud = hudRoot.gameObject.AddComponent<HUDManager>();

            var vitals = Rect(hudRoot, "Vitals");
            vitals.anchorMin = vitals.anchorMax = vitals.pivot = new Vector2(0f, 0f);
            vitals.anchoredPosition = new Vector2(32, 32);
            vitals.sizeDelta = new Vector2(360, 150);
            var vl = vitals.gameObject.AddComponent<VerticalLayoutGroup>();
            vl.spacing = 6;
            vl.childControlHeight = false;
            vl.childForceExpandHeight = false;
            Set(hud, "healthBar", Bar(vitals, "HealthBar", new Color(0.8f, 0.2f, 0.2f)));
            Set(hud, "staminaBar", Bar(vitals, "StaminaBar", new Color(0.9f, 0.85f, 0.3f)));
            Set(hud, "hungerBar", Bar(vitals, "HungerBar", new Color(0.85f, 0.5f, 0.2f)));
            Set(hud, "thirstBar", Bar(vitals, "ThirstBar", new Color(0.3f, 0.6f, 0.95f)));
            var infection = Bar(vitals, "InfectionBar", new Color(0.55f, 0.75f, 0.3f));
            infection.value = 0f;
            Set(hud, "infectionBar", infection);
            Set(hud, "infectionFill", infection.fillRect.GetComponent<Image>());

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

            var ammo = Text(hudRoot, "Ammo", "", 34);
            var art = (RectTransform)ammo.transform;
            art.anchorMin = art.anchorMax = art.pivot = new Vector2(1f, 0f);
            art.anchoredPosition = new Vector2(-32, 32);
            art.sizeDelta = new Vector2(300, 48);
            ammo.alignment = TextAlignmentOptions.Right;
            Set(hud, "ammoText", ammo);

            Set(hud, "hotbarView", BuildHotbar(hudRoot));
        }

        private static HotbarView BuildHotbar(Transform hudRoot)
        {
            // HotbarView: 8 slot (equipment.hotbarSlotCount) + 1 slot tangan kiri. Tata letak final menunggu developer.
            var bar = Rect(hudRoot, "Hotbar");
            bar.anchorMin = bar.anchorMax = bar.pivot = new Vector2(0.5f, 0f);
            bar.anchoredPosition = new Vector2(0, 24);
            bar.sizeDelta = new Vector2(9 * 84, 80);
            var layout = bar.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 6;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = layout.childControlHeight = false;
            layout.childForceExpandWidth = layout.childForceExpandHeight = false;
            var group = bar.gameObject.AddComponent<CanvasGroup>();
            var view = bar.gameObject.AddComponent<HotbarView>();

            var so = new SerializedObject(view);
            var slots = so.FindProperty("slots");
            slots.arraySize = 8;
            for (int i = 0; i < 8; i++) FillSlot(slots.GetArrayElementAtIndex(i), Slot(bar, $"Slot_{i + 1}"));
            FillSlot(so.FindProperty("offHandSlot"), Slot(bar, "Slot_OffHand"));
            so.FindProperty("group").objectReferenceValue = group;
            so.ApplyModifiedPropertiesWithoutUndo();
            return view;
        }

        private struct SlotParts { public GameObject root; public Image icon, frame, durability; public TMP_Text count, number; }

        private static SlotParts Slot(Transform parent, string name)
        {
            var rt = Rect(parent, name);
            rt.sizeDelta = new Vector2(78, 78);
            var frame = rt.gameObject.AddComponent<Image>();
            frame.color = new Color(0f, 0f, 0f, 0.5f);
            var icon = Rect(rt, "Icon");
            Stretch(icon);
            icon.offsetMin = new Vector2(8, 8);
            icon.offsetMax = new Vector2(-8, -8);
            var iconImg = icon.gameObject.AddComponent<Image>();
            iconImg.color = new Color(1f, 1f, 1f, 0.9f);
            var count = Text(rt, "Count", "", 20);
            var crt = (RectTransform)count.transform;
            Stretch(crt);
            count.alignment = TextAlignmentOptions.BottomRight;
            var number = Text(rt, "Number", "", 16);
            var nrt = (RectTransform)number.transform;
            Stretch(nrt);
            number.alignment = TextAlignmentOptions.TopLeft;
            var dur = Rect(rt, "Durability");
            dur.anchorMin = new Vector2(0f, 0f);
            dur.anchorMax = new Vector2(1f, 0f);
            dur.pivot = new Vector2(0.5f, 0f);
            dur.sizeDelta = new Vector2(0, 5);
            var durImg = dur.gameObject.AddComponent<Image>();
            durImg.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            durImg.type = Image.Type.Filled;
            durImg.fillMethod = Image.FillMethod.Horizontal;
            return new SlotParts { root = rt.gameObject, icon = iconImg, frame = frame, durability = durImg, count = count, number = number };
        }

        private static void FillSlot(SerializedProperty p, SlotParts s)
        {
            p.FindPropertyRelative("root").objectReferenceValue = s.root;
            p.FindPropertyRelative("icon").objectReferenceValue = s.icon;
            p.FindPropertyRelative("countText").objectReferenceValue = s.count;
            p.FindPropertyRelative("numberText").objectReferenceValue = s.number;
            p.FindPropertyRelative("frame").objectReferenceValue = s.frame;
            p.FindPropertyRelative("durabilityFill").objectReferenceValue = s.durability;
        }

        private static void SetLayerRecursive(GameObject go, string layer)
        {
            int l = Layer(layer);
            foreach (var t in go.GetComponentsInChildren<Transform>(true)) t.gameObject.layer = l;
        }
    }
}
