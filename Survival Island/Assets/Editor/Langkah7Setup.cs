using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static SurvivalIsland.EditorTools.Langkah2Setup;

namespace SurvivalIsland.EditorTools
{
    /// <summary>
    /// Penyiapan Unity untuk Langkah 7 (LOGIC/README.md, "Penyiapan di Unity untuk Langkah 7"). Membuat:
    /// - DisasterData Eruption/Earthquake/TropicalStorm (DisasterManager.md) di RunSeedManager dan DisasterManager,
    ///   pool Hazard_Fireball dan Hazard_ImpactMarker;
    /// - Lucky Machine: 4 LuckyMachineData (kuota LuckyMachine.md), 9 LuckyRewardData, 3 WeaponModifierData,
    ///   2 buff permanen, item Reward_LaserPistol (+WeaponData) dan Reward_FireAxe, mesin greybox per region;
    /// - Diary: 3 DiaryEntry, 8 ObjectiveData (ObjectiveManager.md), DiaryBook greybox, DiaryManager, ObjectiveManager,
    ///   DiaryPanel, DiaryArchivePanel, LuckyMachinePanel;
    /// - helikopter di helipad (HelicopterFrame, PartInstallZone, CockpitInteractable), RescueEventManager;
    /// - DeathCinematic di kedua prefab pemain.
    /// Jalankan setelah menu Langkah 6.
    /// </summary>
    public static class Langkah7Setup
    {
        private const string DisasterFolder = "Assets/Data/Disasters";
        private const string LuckyFolder = "Assets/Data/LuckyMachine";
        private const string ModifierFolder = "Assets/Resources/WeaponModifiers";
        private const string DiaryFolder = "Assets/Data/Diary";
        private const string ItemFolder = "Assets/Resources/Items";
        private const string WeaponFolder = "Assets/Data/Weapons";
        private const string HazardFolder = "Assets/Prefabs/Hazards";
        private const string PersistentRootPath = "Assets/Prefabs/PersistentRoot.prefab";
        private const string IslandPlayerPath = "Assets/Prefabs/Player/Player_Island.prefab";
        private const string ShipPlayerPath = "Assets/Prefabs/Ship/Player_Ship.prefab";
        private const string IslandScenePath = "Assets/Scenes/IslandPhase.unity";

        [MenuItem("Survival Island/Setup/Langkah 7 - Event dan Akhir")]
        public static void Run()
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(IslandPlayerPath) == null)
            {
                Debug.LogError("[Langkah7Setup] Player_Island belum ada. Jalankan menu Langkah 3 sampai 6 dulu.");
                return;
            }
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            foreach (var f in new[] { DisasterFolder, LuckyFolder, ModifierFolder, DiaryFolder, WeaponFolder, HazardFolder }) EnsureFolder(f);
            CreateMaterials();

            var disasters = CreateDisasters();
            var hazards = BuildHazardPrefabs();
            PatchPersistentRoot(disasters, hazards);
            var machines = CreateLuckyData();
            var (chapters, objectives) = CreateDiaryData();
            PatchPlayers();
            EnsureExecutionOrder();
            BuildScene(disasters, machines, chapters, objectives);

            AssetDatabase.SaveAssets();
            Debug.Log("[Langkah7Setup] Selesai: 3 DisasterData, pool hazard, 4 Lucky Machine, 9 hadiah, 3 modifier, 2 buff, item hadiah, " +
                      "3 bab Diary, 8 tujuan, panel Diary/Arsip/Lucky, helikopter di helipad, RescueEventManager, dan DeathCinematic.");
        }

        // ---------------------------------------------------------------- Bencana

        private static DisasterData[] CreateDisasters()
        {
            // DisasterManager.md tabel bencana. Klip suara belum ada (opsional).
            DisasterData D(DisasterType t, float chance, int cooldown, int minDay, int majorMin, float warn, float active, float active2, float after)
            {
                var d = LoadOrCreate<DisasterData>($"{DisasterFolder}/Disaster_{t}.asset");
                d.type = t; d.dailyChance = chance; d.cooldownDays = cooldown; d.minDay = minDay; d.majorMinScale = majorMin;
                d.scale2Chance = 0.3f; d.warningSeconds = warn; d.activeSeconds = active; d.activeSecondsScale2 = active2;
                d.aftermathSeconds = after; d.startHourMin = 8; d.startHourMax = 14;
                EditorUtility.SetDirty(d);
                return d;
            }
            return new[]
            {
                D(DisasterType.Eruption, 0.05f, 10, 5, 1, 30, 120, 180, 45),
                D(DisasterType.Earthquake, 0.06f, 6, 3, 2, 5, 25, 40, 15),
                D(DisasterType.TropicalStorm, 0.07f, 6, 2, 2, 20, 240, 300, 30),
            };
        }

        private static (GameObject fireball, GameObject marker) BuildHazardPrefabs()
        {
            // Fireball: layer Projectile, komponen Fireball. Penanda dampak: diameter dasar 1 m (HazardImpactMarker menskala sendiri).
            var fb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            fb.name = "Hazard_Fireball";
            fb.layer = Layer(PhysicsMasks.ProjectileLayer);
            fb.transform.localScale = Vector3.one * 1.5f;
            Object.DestroyImmediate(fb.GetComponent<Collider>());
            fb.GetComponent<MeshRenderer>().sharedMaterial = Mat("Greybox_Fireball", new Color(1f, 0.35f, 0.05f), unlit: true);
            fb.AddComponent<Fireball>();
            var fbPrefab = SavePrefab(fb, $"{HazardFolder}/Hazard_Fireball.prefab");

            var mk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            mk.name = "Hazard_ImpactMarker";
            Object.DestroyImmediate(mk.GetComponent<Collider>());
            mk.transform.localScale = new Vector3(1f, 0.01f, 1f);
            mk.GetComponent<MeshRenderer>().sharedMaterial = Mat("Greybox_ImpactMarker", new Color(1f, 0.2f, 0.1f, 0.5f), unlit: true, transparent: true);
            mk.AddComponent<HazardImpactMarker>();
            var mkPrefab = SavePrefab(mk, $"{HazardFolder}/Hazard_ImpactMarker.prefab");
            return (fbPrefab, mkPrefab);
        }

        private static void PatchPersistentRoot(DisasterData[] disasters, (GameObject fireball, GameObject marker) hazards)
        {
            // README Langkah 7 nomor 1 dan 3: RunSeedManager.disasterData dan pool hazard (ObjectPooler.md: 8 dan 8).
            var contents = PrefabUtility.LoadPrefabContents(PersistentRootPath);
            try
            {
                var seed = contents.GetComponentInChildren<RunSeedManager>(true);
                SetArray(seed, "disasterData", disasters);
                var pooler = contents.GetComponentInChildren<ObjectPooler>(true);
                Upsert(pooler.pools, "Hazard_Fireball", hazards.fireball, 8);
                Upsert(pooler.pools, "Hazard_ImpactMarker", hazards.marker, 8);
                PrefabUtility.SaveAsPrefabAsset(contents, PersistentRootPath);
            }
            finally { PrefabUtility.UnloadPrefabContents(contents); }
        }

        // ---------------------------------------------------------------- Lucky Machine

        private static LuckyMachineData[] CreateLuckyData()
        {
            // Modifier (LuckyMachine.md), di Resources/WeaponModifiers (WeaponStatCalculator).
            WeaponModifierData Mod(string id, float dmg, float spd, bool indestructible)
            {
                var m = LoadOrCreate<WeaponModifierData>($"{ModifierFolder}/{id}.asset");
                m.modifierId = id; m.damageMultiplier = dmg; m.speedMultiplier = spd; m.indestructible = indestructible;
                EditorUtility.SetDirty(m);
                return m;
            }
            var modDamage = Mod("mod_damage_25", 1.25f, 1f, false);
            var modSpeed = Mod("mod_speed_15", 1f, 1.15f, false);
            var modIndestructible = Mod("mod_indestructible", 1f, 1f, true);

            // Buff permanen: Permanent, Intensify, maxStacks 3 (LuckyMachine.md).
            StatusEffectData Buff(string id, float hp, float stamina)
            {
                var b = LoadOrCreate<StatusEffectData>($"{LuckyFolder}/{id}.asset");
                b.effectId = id; b.isBuff = true; b.durationUnit = EffectDurationUnit.Permanent; b.stackRule = EffectStackRule.Intensify;
                b.maxStacks = 3; b.maxHpMultiplier = hp; b.maxStaminaMultiplier = stamina;
                EditorUtility.SetDirty(b);
                return b;
            }
            var buffHp = Buff("Buff_LuckyHP", 1.1f, 1f);
            var buffStamina = Buff("Buff_LuckyStamina", 1f, 1.1f);

            // Item hadiah. Reward_LaserPistol: WeaponData dari GunController.md. Reward_FireAxe: maxDurability -1, Axe, TwoHand.
            var wd = LoadOrCreate<WeaponData>($"{WeaponFolder}/WD_Reward_LaserPistol.asset");
            wd.fireMode = FireMode.SemiAuto; wd.damage = 60f; wd.secondsBetweenShots = 0.35f; wd.magazineSize = 8; wd.startingMagazine = 8;
            wd.ammoItemId = string.Empty; wd.reloadSeconds = 3f; wd.maxRange = 80f; wd.spreadBase = 0.2f; wd.spreadMoving = 1f;
            wd.spreadPerShot = 0.3f; wd.spreadMax = 3f; wd.spreadRecoverPerSec = 6f; wd.recoilPitch = 0.6f; wd.recoilYaw = 0.2f;
            wd.noiseRadius = 20f; wd.wearPerShot = 0f; wd.muzzleFlashTag = "VFX_LaserMuzzle"; wd.impactTag = "VFX_LaserImpact"; wd.cameraShakeScale = 1f;
            EditorUtility.SetDirty(wd);
            var laser = LoadOrCreate<ItemData>($"{ItemFolder}/Reward_LaserPistol.asset");
            laser.itemID = "Reward_LaserPistol"; laser.type = ItemType.Weapon; laser.handUse = HandUse.OneHand; laser.weaponData = wd;
            laser.maxStack = 1; laser.maxDurability = -1f;
            var pistolVm = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Viewmodel_Pistol.prefab");
            if (pistolVm != null) laser.equipPrefab = pistolVm.GetComponent<EquippableBehaviour>();
            EditorUtility.SetDirty(laser);
            var axe = LoadOrCreate<ItemData>($"{ItemFolder}/Reward_FireAxe.asset");
            axe.itemID = "Reward_FireAxe"; axe.type = ItemType.Tool; axe.handUse = HandUse.TwoHand; axe.toolClass = ToolClass.Axe;
            axe.maxStack = 1; axe.maxDurability = -1f;   // weaponDamage tidak tertulis: 0 = cadangan 25 di MeleeCombatSystem
            var axeVm = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Viewmodel_Axe.prefab");
            if (axeVm != null) axe.equipPrefab = axeVm.GetComponent<EquippableBehaviour>();
            EditorUtility.SetDirty(axe);

            // Hadiah (LuckyMachine.md tabel). Bobot per hadiah tidak tertulis: nilai awal 1 (bobot tier di config).
            LuckyRewardData R(string id, LuckyRewardKind kind, LuckyTier tier, bool unique = false, int maxGrants = 0)
            {
                var r = LoadOrCreate<LuckyRewardData>($"{LuckyFolder}/{id}.asset");
                r.rewardId = id; r.kind = kind; r.tier = tier; r.isUnique = unique; r.maxGrants = maxGrants; r.weight = 1f;
                r.modifier = null; r.buff = null; r.item = null; r.amount = 1; r.skinId = null; r.skinTargetItemId = null;
                EditorUtility.SetDirty(r);
                return r;
            }
            var rLaser = R("lucky_laser", LuckyRewardKind.SpawnItem, LuckyTier.S, true); rLaser.item = laser; rLaser.modifier = modIndestructible;
            var rAxe = R("lucky_fireaxe", LuckyRewardKind.SpawnItem, LuckyTier.S, true); rAxe.item = axe;
            var rHp = R("lucky_hp", LuckyRewardKind.PermanentBuff, LuckyTier.A, false, 3); rHp.buff = buffHp;
            var rStamina = R("lucky_stamina", LuckyRewardKind.PermanentBuff, LuckyTier.A, false, 3); rStamina.buff = buffStamina;
            var rDamage = R("lucky_mod_damage", LuckyRewardKind.WeaponModifier, LuckyTier.A); rDamage.modifier = modDamage;
            var rSpeed = R("lucky_mod_speed", LuckyRewardKind.WeaponModifier, LuckyTier.B); rSpeed.modifier = modSpeed;
            var rSkin = R("lucky_skin_gold", LuckyRewardKind.Skin, LuckyTier.B, true); rSkin.skinId = "skin_gold"; rSkin.skinTargetItemId = "Base_Pistol";
            var rAmmo = R("lucky_ammo", LuckyRewardKind.SpawnItem, LuckyTier.B); rAmmo.item = AssetDatabase.LoadAssetAtPath<ItemData>($"{ItemFolder}/Ammo_9mm.asset"); rAmmo.amount = 20;
            var rWood = R("lucky_wood", LuckyRewardKind.SpawnItem, LuckyTier.B); rWood.item = AssetDatabase.LoadAssetAtPath<ItemData>($"{ItemFolder}/Mat_Wood.asset"); rWood.amount = 30;
            var rewards = new[] { rLaser, rAxe, rHp, rStamina, rDamage, rSpeed, rSkin, rAmmo, rWood };

            // Kuota per region (LuckyMachine.md). Semua mesin memakai daftar hadiah yang sama.
            LuckyMachineData M(RegionType region, string zombie, int zn, string animal, int an)
            {
                var m = LoadOrCreate<LuckyMachineData>($"{LuckyFolder}/LuckyMachine_{region}.asset");
                m.region = region;
                m.quotas = new[] { new SacrificeQuota { enemyId = zombie, required = zn }, new SacrificeQuota { enemyId = animal, required = an } };
                m.rewards = rewards;
                EditorUtility.SetDirty(m);
                return m;
            }
            return new[]
            {
                M(RegionType.Volcano, "zombie_standar", 8, "hewan_serigala", 3),
                M(RegionType.Amazon, "zombie_tribal", 5, "hewan_jaguar", 2),
                M(RegionType.Snow, "zombie_militer", 5, "hewan_beruang", 3),
                M(RegionType.Logistic, "zombie_desa", 5, "hewan_anjing", 4),
            };
        }

        // ---------------------------------------------------------------- Diary dan tujuan

        private static (DiaryEntry[] chapters, ObjectiveData[] objectives) CreateDiaryData()
        {
            // ObjectiveManager.md: bab 1-3 (kunci teks diary.chN.title/body; isi teks belum ditulis di dokumen).
            // Bab 1 memberi Kit_Campfire x1; bab 2 dan 3 membuka tujuan (RevealObjectives).
            DiaryEntry C(int n, DiaryReward[] rewards)
            {
                var c = LoadOrCreate<DiaryEntry>($"{DiaryFolder}/Diary_Ch{n}.asset");
                c.chapter = n; c.titleKey = $"diary.ch{n}.title"; c.bodyKey = $"diary.ch{n}.body"; c.rewards = rewards;
                EditorUtility.SetDirty(c);
                return c;
            }
            var kit = AssetDatabase.LoadAssetAtPath<ItemData>($"{ItemFolder}/Kit_Campfire.asset");
            var chapters = new[]
            {
                C(1, new[] { new DiaryReward { kind = DiaryRewardKind.GiveItem, item = kit, amount = 1 } }),
                C(2, new[] { new DiaryReward { kind = DiaryRewardKind.RevealObjectives } }),
                C(3, new[] { new DiaryReward { kind = DiaryRewardKind.RevealObjectives } }),
            };

            // ObjectiveManager.md tabel 8 tujuan. Penanda memakai seam dan helipad dari IslandLayout (RegionStreaming.md).
            ObjectiveData O(string id, int order, int reveal, ObjectiveCondition cond, string target, RegionType region, int amount, bool marker, Vector3 pos)
            {
                var o = LoadOrCreate<ObjectiveData>($"{DiaryFolder}/{id}.asset");
                o.objectiveId = id; o.title = $"objective.{id}.title"; o.order = order; o.revealedByChapter = reveal; o.condition = cond;
                o.targetId = target; o.targetRegion = region; o.targetAmount = amount; o.hasMarker = marker; o.markerPosition = pos;
                EditorUtility.SetDirty(o);
                return o;
            }
            var bridge = new Vector3(1500f, 0f, 997f);
            var objectives = new[]
            {
                O("obj_base", 1, 0, ObjectiveCondition.BuildCount, "Wall_", RegionType.Volcano, 4, false, Vector3.zero),
                O("obj_wood", 2, 2, ObjectiveCondition.ItemTotal, "Mat_Wood", RegionType.Volcano, 0, false, Vector3.zero),
                O("obj_bridge", 3, 2, ObjectiveCondition.BridgeBuilt, null, RegionType.Amazon, 0, true, bridge),
                O("obj_vaccine", 4, 2, ObjectiveCondition.ItemCrafted, "rec_vaccine", RegionType.Amazon, 0, false, Vector3.zero),
                O("obj_region4", 5, 3, ObjectiveCondition.RegionEntered, null, RegionType.Logistic, 0, true, IslandLayout.S2),
                O("obj_region3", 6, 3, ObjectiveCondition.RegionEntered, null, RegionType.Snow, 0, true, IslandLayout.S3),
                O("obj_parts", 7, 3, ObjectiveCondition.PartsInstalled, null, RegionType.Logistic, 0, false, Vector3.zero),
                O("obj_takeoff", 8, 3, ObjectiveCondition.Escaped, null, RegionType.Snow, 0, true, IslandLayout.Helipad),
            };
            return (chapters, objectives);
        }

        private static void EnsureExecutionOrder()
        {
            // HelicopterFrame mendaftar ke RescueEventManager.Instance di OnEnable. Menurut blueprint rangka ada di scene
            // Region3_Snow yang dimuat setelah IslandPhase; di scene greybox tunggal keduanya aktif bersamaan, jadi
            // RescueEventManager diberi urutan eksekusi lebih awal supaya Awake-nya mengisi Instance lebih dulu.
            foreach (var guid in AssetDatabase.FindAssets("RescueEventManager t:MonoScript"))
            {
                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(guid));
                if (script != null && script.GetClass() == typeof(RescueEventManager) && MonoImporter.GetExecutionOrder(script) != -50)
                    MonoImporter.SetExecutionOrder(script, -50);
            }
        }

        // ---------------------------------------------------------------- Pemain

        private static void PatchPlayers()
        {
            // README Langkah 7 nomor 6: DeathCinematic dengan cameraRig. Tanpa PlayerBodyRig semua gaya menjadi Blackout.
            foreach (var path in new[] { IslandPlayerPath, ShipPlayerPath })
            {
                var p = PrefabUtility.LoadPrefabContents(path);
                var dc = p.GetComponent<DeathCinematic>();
                if (dc == null) dc = p.AddComponent<DeathCinematic>();
                Set(dc, "cameraRig", p.transform.Find("CameraRig"));
                PrefabUtility.SaveAsPrefabAsset(p, path);
                PrefabUtility.UnloadPrefabContents(p);
            }
        }

        // ---------------------------------------------------------------- Scene

        private static void BuildScene(DisasterData[] disasters, LuckyMachineData[] machines, DiaryEntry[] chapters, ObjectiveData[] objectives)
        {
            var scene = EditorSceneManager.OpenScene(IslandScenePath, OpenSceneMode.Single);
            foreach (var n in new[] { "Langkah7World", "Langkah7Systems" })
            {
                var old = GameObject.Find(n);
                if (old != null) Object.DestroyImmediate(old);
            }
            var world = new GameObject("Langkah7World").transform;

            // Lucky Machine per region. Letak R3 dari LuckyMachine.md (350, 2050); R1, R2, R4 belum ditentukan dokumen
            // ("ditentukan saat layout #88"): greybox dekat titik awal tiap region.
            BuildMachine(world, machines[0], new Vector3(1530f, 0f, 120f));
            BuildMachine(world, machines[1], new Vector3(1500f, 0f, 1200f));
            BuildMachine(world, machines[2], new Vector3(350f, 0f, 2050f));
            BuildMachine(world, machines[3], new Vector3(600f, 0f, 1750f));

            // Buku Diary di Region 1. Letak shelter tidak tertulis: greybox dekat pantai pendaratan.
            var book = Box(world, "DiaryBook", IslandLayout.R1Origin + new Vector3(4f, 0.4f, -32f), new Vector3(0.4f, 0.1f, 0.3f), Mat("Greybox_Book", new Color(0.4f, 0.2f, 0.1f)), PhysicsMasks.InteractableLayer);
            var bookComp = book.AddComponent<DiaryBook>();
            var bso = new SerializedObject(bookComp);
            bso.FindProperty("saveId").stringValue = "diary_book_r1";
            bso.FindProperty("highlight").objectReferenceValue = AddHighlight(book, PrimitiveType.Cube);
            bso.ApplyModifiedPropertiesWithoutUndo();

            BuildHelicopter(world);

            var systems = new GameObject("Langkah7Systems").transform;
            var dm = AddTo<DisasterManager>(systems, "DisasterManager");
            SetArray(dm, "disasterData", disasters);
            AddTo<LuckyRewardLedger>(systems, "LuckyRewardLedger");
            AddTo<RescueEventManager>(systems, "RescueEventManager");
            var om = AddTo<ObjectiveManager>(systems, "ObjectiveManager");
            SetArray(om, "chapters", chapters);
            SetArray(om, "objectives", objectives);
            var oso = new SerializedObject(om);
            oso.FindProperty("bookMarkerPosition").vector3Value = book.transform.position;
            oso.FindProperty("bridgeMarkerPosition").vector3Value = new Vector3(1500f, 0f, 997f);
            oso.ApplyModifiedPropertiesWithoutUndo();
            var diary = AddTo<DiaryManager>(systems, "DiaryManager");

            var canvas = GameObject.Find("IslandCanvas").transform;
            foreach (var n in new[] { "DiaryPanel", "DiaryArchivePanel", "LuckyMachinePanel" })
            {
                var old = canvas.Find(n);
                if (old != null) Object.DestroyImmediate(old.gameObject);
            }
            // UIFocusStack.md: Diary dan Arsip menjeda; LuckyMachine tidak menjeda (GDD bagian 7, jatah 30 detik saat bencana).
            var pause = new UIPanelSettings { pausesTime = true, releasesCursor = true, blocksGameplayInput = true, closableByEsc = true };
            var noPause = new UIPanelSettings { pausesTime = false, releasesCursor = true, blocksGameplayInput = true, closableByEsc = true };
            var diaryPanel = BuildDiaryPanel(canvas, pause);
            var archive = BuildArchivePanel(canvas, pause, chapters);
            BuildLuckyPanel(canvas, noPause);
            Set(diary, "diaryPanel", diaryPanel);
            Set(diary, "archivePanel", archive);

            EditorSceneManager.SaveScene(scene);
        }

        private static void BuildMachine(Transform world, LuckyMachineData data, Vector3 pos)
        {
            // LuckyMachine.md: mesin layer Interactable + SaveIdTag; kandang anak dengan BoxCollider trigger 6 x 4 x 4 m di
            // layer ZoneTrigger, Rigidbody kinematic, gerbang NavMeshObstacle carve, dan vacuumPoint.
            var root = new GameObject("LuckyMachine_" + data.region);
            root.transform.SetParent(world, false);
            root.transform.position = pos;
            var body = Box(root.transform, "Machine", new Vector3(0f, 1.2f, 0f), new Vector3(1.4f, 2.4f, 1f), Mat("Greybox_LuckyMachine", new Color(0.9f, 0.75f, 0.1f)), PhysicsMasks.InteractableLayer);
            var tag = body.AddComponent<SaveIdTag>();
            var tso = new SerializedObject(tag);
            tso.FindProperty("idPrefix").stringValue = "lucky";
            tso.FindProperty("saveId").stringValue = "lucky_" + data.region.ToString().ToLowerInvariant();
            tso.ApplyModifiedPropertiesWithoutUndo();
            var machine = body.AddComponent<LuckyMachine>();
            var light = Box(body.transform, "StoredSpinLight", new Vector3(0f, 0.6f, -0.01f), new Vector3(0.3f, 0.1f, 1.05f), Mat("Greybox_SpinLight", new Color(0.2f, 1f, 0.3f), unlit: true), PhysicsMasks.InteractableLayer);
            Object.DestroyImmediate(light.GetComponent<Collider>());
            light.SetActive(false);

            var cageGo = new GameObject("Cage");
            cageGo.transform.SetParent(root.transform, false);
            cageGo.transform.localPosition = new Vector3(0f, 2f, -4f);
            cageGo.layer = Layer(PhysicsMasks.ZoneTriggerLayer);
            var cageCol = cageGo.AddComponent<BoxCollider>();
            cageCol.isTrigger = true;
            cageCol.size = new Vector3(6f, 4f, 4f);
            var rb = cageGo.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            var cage = cageGo.AddComponent<LuckyMachineCage>();
            var gate = Box(cageGo.transform, "GateClosed", new Vector3(0f, 0f, -2f), new Vector3(6f, 4f, 0.2f), Mat("Greybox_Gate", new Color(0.6f, 0.2f, 0.2f)), PhysicsMasks.ObstacleLayer);
            var obs = gate.AddComponent<NavMeshObstacle>();
            obs.carving = true;
            obs.shape = NavMeshObstacleShape.Box;
            obs.size = Vector3.one;
            gate.SetActive(false);
            var vacuum = Child(root.transform, "VacuumPoint", new Vector3(0f, 1f, -0.8f));
            Set(cage, "machine", machine);
            Set(cage, "gateClosedObject", gate);
            Set(cage, "vacuumPoint", vacuum);

            Set(machine, "data", data);
            Set(machine, "cage", cage);
            Set(machine, "rewardPoint", Child(root.transform, "RewardPoint", new Vector3(1.5f, 0.5f, 1f)));
            Set(machine, "highlight", AddHighlight(body, PrimitiveType.Cube));
            Set(machine, "storedSpinLight", light);
            Set(machine, "duplicateModifierFallback", AssetDatabase.LoadAssetAtPath<LuckyRewardData>($"{LuckyFolder}/lucky_ammo.asset"));
        }

        private static void BuildHelicopter(Transform world)
        {
            // RescueEventManager.md: HelicopterFrame di helipad dengan visual per partId, anak PartInstallZone (trigger
            // ZoneTrigger, radius rescue.partInstallRadius 4), dan CockpitInteractable (layer Interactable).
            var root = new GameObject("HelicopterFrame");
            root.transform.SetParent(world, false);
            root.transform.position = IslandLayout.Helipad + new Vector3(0f, 0.2f, 0f);
            var hull = Box(root.transform, "Hull", new Vector3(0f, 1.2f, 0f), new Vector3(2.4f, 2f, 6f), Mat("Greybox_Heli", new Color(0.3f, 0.4f, 0.3f)), PhysicsMasks.ObstacleLayer);
            var frame = root.AddComponent<HelicopterFrame>();

            var visuals = new (string id, Vector3 pos, Vector3 size)[]
            {
                ("Part_Rotor", new Vector3(0f, 2.4f, 0f), new Vector3(8f, 0.1f, 0.3f)),
                ("Part_Engine", new Vector3(0f, 2.3f, -1f), new Vector3(1.2f, 0.8f, 1f)),
                ("Part_FuelTank", new Vector3(1.4f, 0.8f, 0f), new Vector3(0.6f, 0.6f, 1.6f)),
                ("Part_TailRotor", new Vector3(0.3f, 1.8f, -4.2f), new Vector3(0.1f, 1.4f, 0.2f)),
                ("Part_Avionics", new Vector3(0f, 1.6f, 2.6f), new Vector3(1f, 0.4f, 0.3f)),
            };
            var so = new SerializedObject(frame);
            var arr = so.FindProperty("partVisuals");
            arr.arraySize = visuals.Length;
            for (int i = 0; i < visuals.Length; i++)
            {
                var v = Box(root.transform, "Visual_" + visuals[i].id, visuals[i].pos, visuals[i].size, Mat("Greybox_" + visuals[i].id, new Color(0.4f, 0.4f, 0.45f)), PhysicsMasks.ObstacleLayer);
                Object.DestroyImmediate(v.GetComponent<Collider>());
                v.SetActive(false);
                var el = arr.GetArrayElementAtIndex(i);
                el.FindPropertyRelative("partId").stringValue = visuals[i].id;
                el.FindPropertyRelative("visual").objectReferenceValue = v;
            }

            var zoneGo = new GameObject("PartInstallZone");
            zoneGo.transform.SetParent(root.transform, false);
            zoneGo.layer = Layer(PhysicsMasks.ZoneTriggerLayer);
            var sphere = zoneGo.AddComponent<SphereCollider>();
            sphere.isTrigger = true;
            sphere.radius = 4f;
            var zone = zoneGo.AddComponent<PartInstallZone>();

            var cockpit = Box(root.transform, "Cockpit", new Vector3(1.3f, 1.2f, 1.8f), new Vector3(0.3f, 1.2f, 1.2f), Mat("Greybox_Cockpit", new Color(0.2f, 0.5f, 0.8f)), PhysicsMasks.InteractableLayer);
            var ci = cockpit.AddComponent<CockpitInteractable>();
            Set(ci, "highlight", AddHighlight(cockpit, PrimitiveType.Cube));

            so.FindProperty("installZone").objectReferenceValue = zone;
            so.FindProperty("cockpit").objectReferenceValue = cockpit.transform;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        // ---------------------------------------------------------------- Panel

        private static DiaryPanel BuildDiaryPanel(Transform canvas, UIPanelSettings s)
        {
            var panel = CreatePanel<DiaryPanel>(canvas, "DiaryPanel", UIPanelId.Diary, s);
            var title = Text(panel.transform, "Title", "", 48);
            title.rectTransform.anchoredPosition = new Vector2(0, 400);
            var scrollGo = Rect(panel.transform, "Scroll");
            scrollGo.anchorMin = scrollGo.anchorMax = new Vector2(0.5f, 0.5f);
            scrollGo.sizeDelta = new Vector2(1100, 650);
            scrollGo.gameObject.AddComponent<RectMask2D>();
            var content = Text(scrollGo, "Content", "", 30);
            content.alignment = TextAlignmentOptions.TopLeft;
            var crt = content.rectTransform;
            crt.anchorMin = new Vector2(0f, 1f); crt.anchorMax = new Vector2(1f, 1f); crt.pivot = new Vector2(0.5f, 1f);
            crt.sizeDelta = new Vector2(0, 1600);
            var scroll = scrollGo.gameObject.AddComponent<ScrollRect>();
            scroll.content = crt;
            scroll.horizontal = false;
            var close = Button(panel.transform, "CloseButton", "Tutup");
            ((RectTransform)close.transform).anchoredPosition = new Vector2(0, -420);
            Set(panel, "titleText", title);
            Set(panel, "contentText", content);
            Set(panel, "closeButton", close);
            Set(panel, "scroll", scroll);
            HideAtStart(panel);
            return panel;
        }

        private static DiaryArchivePanel BuildArchivePanel(Transform canvas, UIPanelSettings s, DiaryEntry[] chapters)
        {
            var panel = CreatePanel<DiaryArchivePanel>(canvas, "DiaryArchivePanel", UIPanelId.DiaryArchive, s);
            var list = VList(panel.transform);
            Text(list, "Title", "ARSIP DIARY", 48);
            var buttons = new Object[chapters.Length];
            for (int i = 0; i < chapters.Length; i++) buttons[i] = Button(list, $"Chapter{i + 1}Button", $"Bab {i + 1}");
            Set(panel, "emptyText", Text(list, "Empty", "", 28));
            Set(panel, "closeButton", Button(list, "CloseButton", "Tutup"));
            SetArray(panel, "chapters", chapters);
            SetArray(panel, "chapterButtons", buttons);
            HideAtStart(panel);
            return panel;
        }

        private static void BuildLuckyPanel(Transform canvas, UIPanelSettings s)
        {
            var panel = CreatePanel<LuckyMachinePanel>(canvas, "LuckyMachinePanel", UIPanelId.LuckyMachine, s);
            var list = VList(panel.transform);
            Text(list, "Title", "LUCKY MACHINE", 48);
            Set(panel, "quotaText", Text(list, "Quota", "", 30));
            Set(panel, "spinsText", Text(list, "Spins", "", 30));
            Set(panel, "freeWindowText", Text(list, "FreeWindow", "", 30));
            Set(panel, "resultText", Text(list, "Result", "", 30));
            Set(panel, "spinButton", Button(list, "SpinButton", "Putar"));
            Set(panel, "closeButton", Button(list, "CloseButton", "Tutup"));
            HideAtStart(panel);
        }

        private static void HideAtStart(FadingUIPanel panel)
        {
            // Panel tetap aktif supaya Awake mengisi Instance (README Langkah 7 nomor 2), hanya tak terlihat.
            var g = panel.GetComponent<CanvasGroup>();
            g.alpha = 0f;
            g.blocksRaycasts = false;
            g.interactable = false;
        }

        private static T AddTo<T>(Transform parent, string name) where T : Component
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            return go.AddComponent<T>();
        }
    }
}
