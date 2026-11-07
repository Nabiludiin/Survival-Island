using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using static SurvivalIsland.EditorTools.Langkah2Setup;

namespace SurvivalIsland.EditorTools
{
    /// <summary>
    /// Penyiapan Unity untuk Langkah 5 (LOGIC/README.md, "Penyiapan di Unity untuk Langkah 5"). Membuat:
    /// - AttackData, ZombieData (6 varian), AnimalData (8 spesies), ThreatProfile, LootTable drop zombie,
    ///   RegionSpawnTable 4 region, dan item Food_RawMeat (hasil menguliti), semua dengan angka blueprint;
    /// - prefab greybox semua musuh (zombie_standar milik menu ini, menu Langkah 2 tidak menimpanya lagi) + Tribal_Snare;
    /// - pool semua tag musuh di PersistentRoot (ObjectPooler.md);
    /// - komponen pemain (PlayerTargetable, PlayerPinnedState, PlayerShove, NoiseEmitter);
    /// - IslandPhase: SpawnManager, AITraversal, AIDirector, ThreatSystem, AISensoryHub, HordeCrowdManager,
    ///   AttackCoordinator, dan NavMeshSurface pulau yang dibake. ShipPhase: HordeCrowdManager dan AttackCoordinator.
    /// Jalankan setelah menu Langkah 4.
    /// </summary>
    public static class Langkah5Setup
    {
        private const string EnemyFolder = "Assets/Data/Enemies";
        private const string AttackFolder = "Assets/Data/Enemies/Attacks";
        private const string LootFolder = "Assets/Data/Loot";
        private const string SpawnFolder = "Assets/Data/Spawn";
        private const string EnemyPrefabFolder = "Assets/Prefabs/Enemies";
        private const string ItemFolder = "Assets/Resources/Items";
        private const string PersistentRootPath = "Assets/Prefabs/PersistentRoot.prefab";
        private const string IslandPlayerPath = "Assets/Prefabs/Player/Player_Island.prefab";
        private const string ShipPlayerPath = "Assets/Prefabs/Ship/Player_Ship.prefab";
        private const string IslandScenePath = "Assets/Scenes/IslandPhase.unity";
        private const string ShipScenePath = "Assets/Scenes/ShipPhase.unity";
        private const string IslandNavPath = "Assets/Scenes/IslandPhase_NavMesh.asset";

        private static readonly Dictionary<string, AttackData> attacks = new Dictionary<string, AttackData>();

        [MenuItem("Survival Island/Setup/Langkah 5 - Musuh dan Senjata")]
        public static void Run()
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(IslandPlayerPath) == null)
            {
                Debug.LogError("[Langkah5Setup] Player_Island belum ada. Jalankan menu Langkah 3 dan 4 dulu.");
                return;
            }
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            foreach (var f in new[] { EnemyFolder, AttackFolder, LootFolder, SpawnFolder, EnemyPrefabFolder }) EnsureFolder(f);
            CreateMaterials();
            attacks.Clear();

            CreateAttacks();
            var threatZombie = CreateThreat("Threat_Zombie", false);
            var threatAnimal = CreateThreat("Threat_Animal", true);
            var drops = CreateDropTables();
            var zombies = CreateZombies(threatZombie, drops);
            var animals = CreateAnimals(threatAnimal);
            var tables = CreateSpawnTables(zombies, animals);

            var pools = new List<(string tag, GameObject prefab, int size)>();
            foreach (var z in zombies.Values) pools.Add((z.enemyId, BuildZombiePrefab(z), ZombiePoolSize(z.enemyId)));
            foreach (var a in animals.Values) pools.Add((a.enemyId, BuildAnimalPrefab(a), AnimalPoolSize(a.enemyId)));
            pools.Add(("Tribal_Snare", BuildSnarePrefab(), 6));   // ObjectPooler.md: Tribal_Snare 6 (usulan)
            RegisterPools(pools);

            PatchPlayers();
            SetupIsland(tables);
            SetupShip();

            AssetDatabase.SaveAssets();
            Debug.Log($"[Langkah5Setup] Selesai: {attacks.Count} AttackData, {zombies.Count} zombie, {animals.Count} hewan, 4 tabel spawn, " +
                      $"{pools.Count} pool, komponen pemain, manager AI IslandPhase + NavMesh, dan ShipPhase.");
        }

        // ---------------------------------------------------------------- Serangan

        private static AttackData Attack(string id, float damage, float infection, AttackData copyFrom = null)
        {
            var a = LoadOrCreate<AttackData>($"{AttackFolder}/{id}.asset");
            if (copyFrom != null) EditorUtility.CopySerialized(copyFrom, a);
            a.name = id;
            a.attackId = id;
            a.damage = damage;
            a.infection = infection;
            attacks[id] = a;
            EditorUtility.SetDirty(a);
            return a;
        }

        private static void CreateAttacks()
        {
            // EnemyAttack.md tabel aset lengkap.
            var std = Attack("Zombie_Standard_Hit", 15, 5);
            Fill(std, 10, 2f, 0.9f, 1f, 120f, 0.5f, 0.6f, 0.15f, 1.2f, 0.8f, true, "Attack");
            var bite = Attack("Zombie_Ship_Bite", 15, 8);
            Fill(bite, 0, 2f, 0.9f, 1f, 120f, 0.5f, 0.6f, 0.15f, 1.2f, 0.8f, true, "Bite");
            var heavy = Attack("Zombie_Heavy_Hit", 30, 10);
            Fill(heavy, 30, 2.5f, 1.2f, 1.2f, 150f, 0.9f, 0.5f, 0.25f, 1.5f, 1f, false, "HeavyAttack");
            // EnemyAttack.md: varian lain menyalin Zombie_Standard_Hit, lalu damage dan infeksi sendiri.
            Attack("Zombie_Tribal_Hit", 10, 6, std);
            Attack("Zombie_Tribal_Snare", 5, 6, std);
            Attack("Zombie_Lumut_Lunge", 30, 8, std);
            Attack("Zombie_Beku_Hit", 10, 5, std);
            Attack("Zombie_Desa_Pounce", 12, 6, std);
            Attack("Zombie_Rock_Throw", 10, 0, std).windupSeconds = 1f;

            // Hewan (WildlifeAI.md): hanya damage yang tertulis, infeksi 0 (EnemyAttack.md). Nama aset tidak tertulis di
            // blueprint: dipakai Animal_<id>. Field lain memakai nilai awal AttackData.
            foreach (var (id, dmg) in new[] { ("hewan_serigala", 12f), ("hewan_babi", 18f), ("hewan_jaguar", 30f), ("hewan_ular", 6f),
                                              ("hewan_beruang", 35f), ("hewan_serigala_salju", 14f), ("hewan_anjing", 10f), ("hewan_gagak", 5f) })
                Attack("Animal_" + id, dmg, 0f);
            var snake = attacks["Animal_hewan_ular"];
            snake.damageType = DamageType.Poison;   // WildlifeAI.md: gigitan berbisa
            snake.range = 1.5f;
        }

        private static void Fill(AttackData a, float structure, float range, float radius, float offset, float arc, float windup,
            float tracking, float active, float recHit, float recMiss, bool interrupt, string trigger)
        {
            a.structureDamage = structure; a.range = range; a.hitRadius = radius; a.hitOffset = offset; a.arcDegrees = arc;
            a.windupSeconds = windup; a.trackingRatio = tracking; a.activeSeconds = active; a.recoveryAfterHit = recHit;
            a.recoveryAfterMiss = recMiss; a.canBeInterrupted = interrupt; a.usesToken = true; a.animTrigger = trigger;
        }

        private static ThreatProfile CreateThreat(string id, bool animal)
        {
            // ThreatSystem.md tabel profil.
            var t = LoadOrCreate<ThreatProfile>($"{EnemyFolder}/{id}.asset");
            t.allowPlayer = true; t.allowNpc = true;
            t.allowBaseAnchor = !animal; t.allowLure = !animal;
            t.kindWeightPlayer = 40f; t.kindWeightNpc = animal ? 40f : 30f;
            t.distanceWeight = 30f; t.visibleBonus = 25f; t.recentDamageBonus = animal ? 40f : 30f;
            t.senseRange = animal ? 0f : 60f;   // 0 = dari aggroRadius (WildlifeAI)
            t.huntsUnseen = !animal; t.reactsToLure = !animal;
            EditorUtility.SetDirty(t);
            return t;
        }

        // ---------------------------------------------------------------- Drop

        private static Dictionary<string, LootTable> CreateDropTables()
        {
            // RegionLootTable.md tabel drop zombie.
            ItemData I(string id) => ItemDatabaseEditor(id);
            var d = new Dictionary<string, LootTable>
            {
                ["Drop_Zombie_Standard"] = Loot("Drop_Zombie_Standard", (I("Ammo_9mm"), 1, 3, 70), (I("Mat_Cloth"), 1, 1, 30)),
                ["Drop_Zombie_Tribal"] = Loot("Drop_Zombie_Tribal", (I("Mat_TreeSap"), 1, 1, 50), (I("Mat_Mushroom"), 1, 1, 50)),
                ["Drop_Zombie_Moss"] = Loot("Drop_Zombie_Moss", (I("Mat_TreeSap"), 1, 1, 50), (I("Mat_Mushroom"), 1, 1, 50)),
                ["Drop_Zombie_Military"] = Loot("Drop_Zombie_Military", (I("Ammo_9mm"), 3, 6, 100)),
                ["Drop_Zombie_Frozen"] = Loot("Drop_Zombie_Frozen", (I("Mat_Cloth"), 1, 2, 100)),
                ["Drop_Zombie_Village"] = Loot("Drop_Zombie_Village", (I("Ammo_9mm"), 1, 3, 100)),
            };
            return d;
        }

        private static ItemData ItemDatabaseEditor(string id) => AssetDatabase.LoadAssetAtPath<ItemData>($"{ItemFolder}/{id}.asset");

        private static LootTable Loot(string id, params (ItemData item, int min, int max, float weight)[] rows)
        {
            var t = LoadOrCreate<LootTable>($"{LootFolder}/{id}.asset");
            t.tableId = id;
            t.location = LootLocation.None;
            t.entries = new LootEntry[rows.Length];
            for (int i = 0; i < rows.Length; i++)
                t.entries[i] = new LootEntry { item = rows[i].item, minAmount = rows[i].min, maxAmount = rows[i].max, weight = rows[i].weight, kinds = LootPointKind.Any };
            EditorUtility.SetDirty(t);
            return t;
        }

        // ---------------------------------------------------------------- Zombie

        private static Dictionary<string, ZombieData> CreateZombies(ThreatProfile threat, Dictionary<string, LootTable> drops)
        {
            // SpecialZombieAI.md tabel varian, EnemyHitReaction.md (poise, drop), SunlightExposure.md (hidesFromSun).
            // traversalMask tetap AllAreas: area NavMesh traversal (#94) belum dibuat di proyek.
            var z = new Dictionary<string, ZombieData>();
            z["zombie_standar"] = Zombie("zombie_standar", 50, 4, false, false, "Zombie_Standard_Hit", null, 25, true, 1f, drops["Drop_Zombie_Standard"], 0.08f, false, threat);
            z["zombie_tribal"] = Zombie("zombie_tribal", 80, 4, false, false, "Zombie_Tribal_Hit", "Zombie_Tribal_Snare", 30, true, 1f, drops["Drop_Zombie_Tribal"], 0.15f, false, threat);
            z["zombie_lumut"] = Zombie("zombie_lumut", 120, 5.5f, false, false, "Zombie_Lumut_Lunge", null, 45, true, 0.8f, drops["Drop_Zombie_Moss"], 0.15f, false, threat);
            z["zombie_militer"] = Zombie("zombie_militer", 300, 3, true, true, "Zombie_Heavy_Hit", "Zombie_Rock_Throw", 90, false, 0f, drops["Drop_Zombie_Military"], 0.4f, false, threat);
            z["zombie_beku"] = Zombie("zombie_beku", 60, 3.5f, false, false, "Zombie_Beku_Hit", null, 25, true, 1f, drops["Drop_Zombie_Frozen"], 0.08f, false, threat);
            z["zombie_desa"] = Zombie("zombie_desa", 60, 7.5f, false, false, "Zombie_Desa_Pounce", null, 20, true, 1.2f, drops["Drop_Zombie_Village"], 0.1f, true, threat);
            // Resistansi militer (SpecialZombieAI.md / DamagePipeline.md).
            z["zombie_militer"].resistances = new[]
            {
                new DamageResistance { type = DamageType.Bullet, bodyPart = BodyPart.Torso, multiplier = 0.35f },
                new DamageResistance { type = DamageType.Bullet, bodyPart = BodyPart.Leg, multiplier = 0.5f },
                new DamageResistance { type = DamageType.Bullet, bodyPart = BodyPart.Head, multiplier = 1f },
                new DamageResistance { type = DamageType.Melee, anyBodyPart = true, multiplier = 0.8f },
                new DamageResistance { type = DamageType.Explosion, anyBodyPart = true, multiplier = 1f },
                new DamageResistance { type = DamageType.Fire, anyBodyPart = true, multiplier = 1f },
            };
            return z;
        }

        private static ZombieData Zombie(string id, float hp, float speed, bool large, bool trapImmune, string primary, string secondary,
            float poise, bool knock, float knockMul, LootTable drop, float dropChance, bool hidesFromSun, ThreatProfile threat)
        {
            var d = LoadOrCreate<ZombieData>($"{EnemyFolder}/{id}.asset");
            d.enemyId = id; d.maxHP = hp; d.moveSpeed = speed; d.isLarge = large; d.immuneToTrapSlow = trapImmune;
            d.primaryAttack = attacks[primary]; d.secondaryAttack = secondary != null ? attacks[secondary] : null;
            d.poise = poise; d.canBeKnockedBack = knock; d.knockbackMultiplier = knockMul;
            d.dropTable = drop; d.dropChance = dropChance; d.hidesFromSun = hidesFromSun; d.threatProfile = threat;
            d.canAttackBuildings = true;
            d.traversalMask = NavMesh.AllAreas;
            if (d.resistances == null) d.resistances = new DamageResistance[0];
            EditorUtility.SetDirty(d);
            return d;
        }

        private static int ZombiePoolSize(string id) => id == "zombie_standar" ? 50 : id == "zombie_desa" ? 30 : 12;

        // ---------------------------------------------------------------- Hewan

        private static Dictionary<string, AnimalData> CreateAnimals(ThreatProfile threat)
        {
            // Food_RawMeat: ItemUseSystem.md (lapar 15, pakai 2 detik, busuk 12 jam, poisonChance 0,5), tumpukan makanan 5.
            var meat = LoadOrCreate<ItemData>($"{ItemFolder}/Food_RawMeat.asset");
            meat.itemID = "Food_RawMeat"; meat.type = ItemType.Consumable; meat.isConsumable = true; meat.maxStack = 5;
            meat.hungerDelta = 15f; meat.useSeconds = 2f; meat.spoilHours = 12f; meat.poisonChance = 0.5f; meat.restoreSeconds = 20f;
            EditorUtility.SetDirty(meat);
            var hide = ItemDatabaseEditor("Mat_Hide");

            // WildlifeAI.md tabel spesies: HP, kecepatan, aggro, leash, jenis serangan, daging/kulit.
            var a = new Dictionary<string, AnimalData>();
            void Add(string id, float hp, float wander, float chase, float aggro, float leash, AnimalAttackKind kind, int meatN, int hideN)
            {
                var d = LoadOrCreate<AnimalData>($"{EnemyFolder}/{id}.asset");
                d.enemyId = id; d.maxHP = hp; d.wanderSpeed = wander; d.chaseSpeed = chase; d.aggroRadius = aggro; d.leashDistance = leash;
                d.attackKind = kind; d.primaryAttack = attacks["Animal_" + id]; d.threatProfile = threat;
                d.traversalMask = NavMesh.AllAreas;
                var h = new List<HarvestEntry>();
                if (meatN > 0) h.Add(new HarvestEntry { item = meat, min = meatN, max = meatN });
                if (hideN > 0) h.Add(new HarvestEntry { item = hide, min = hideN, max = hideN });
                d.harvest = h.ToArray();
                d.leavesCarcass = h.Count > 0;
                if (d.resistances == null) d.resistances = new DamageResistance[0];
                EditorUtility.SetDirty(d);
                a[id] = d;
            }
            Add("hewan_serigala", 60, 2, 7, 18, 35, AnimalAttackKind.Bite, 2, 1);
            Add("hewan_babi", 90, 2, 6.5f, 12, 20, AnimalAttackKind.Charge, 3, 1);
            Add("hewan_jaguar", 110, 2.5f, 8.5f, 14, 15, AnimalAttackKind.Pounce, 2, 2);
            Add("hewan_ular", 25, 0.8f, 3, 5, 8, AnimalAttackKind.VenomBite, 1, 0);
            Add("hewan_beruang", 400, 2, 6, 20, 25, AnimalAttackKind.Bite, 6, 4);
            Add("hewan_serigala_salju", 75, 2, 7.5f, 20, 35, AnimalAttackKind.Bite, 2, 2);
            Add("hewan_anjing", 45, 2.5f, 7.5f, 16, 30, AnimalAttackKind.Bite, 1, 1);
            Add("hewan_gagak", 15, 3, 6, 15, 25, AnimalAttackKind.FlockDive, 0, 0);
            // EnemyHitReaction.md: beruang poise 120, tanpa knockback.
            var bear = a["hewan_beruang"];
            bear.poise = 120f; bear.canBeKnockedBack = false; bear.knockbackMultiplier = 0f;
            return a;
        }

        private static int AnimalPoolSize(string id) =>
            id == "hewan_gagak" ? 16 : (id == "hewan_serigala" || id == "hewan_serigala_salju" || id == "hewan_anjing") ? 8 : 4;

        // ---------------------------------------------------------------- Tabel spawn

        private static RegionSpawnTable[] CreateSpawnTables(Dictionary<string, ZombieData> z, Dictionary<string, AnimalData> a)
        {
            // SpawnManager.md tabel per region; campCounterEnemyId dari AITraversal.md. minDay, maxAlive, isSpecial tidak tertulis.
            SpawnEntry E(EnemyData d, float w, int gmin = 1, int gmax = 1, bool bush = false) =>
                new SpawnEntry { enemy = d, weight = w, groupMin = gmin, groupMax = gmax, useBushPoints = bush };
            var t = new[]
            {
                Table("Spawn_Volcano", RegionType.Volcano, "zombie_standar",
                    new[] { E(a["hewan_serigala"], 60, 2, 3), E(a["hewan_babi"], 40, 1, 2) },
                    new[] { E(z["zombie_standar"], 100) }),
                Table("Spawn_Amazon", RegionType.Amazon, "zombie_tribal",
                    new[] { E(a["hewan_jaguar"], 40), E(a["hewan_ular"], 60) },
                    new[] { E(z["zombie_standar"], 50), E(z["zombie_tribal"], 30), E(z["zombie_lumut"], 20, 1, 1, true) }),
                Table("Spawn_Snow", RegionType.Snow, "zombie_militer",
                    new[] { E(a["hewan_serigala_salju"], 70, 2, 3), E(a["hewan_beruang"], 30) },
                    new[] { E(z["zombie_standar"], 40), E(z["zombie_militer"], 30), E(z["zombie_beku"], 30) }),
                Table("Spawn_Logistic", RegionType.Logistic, "zombie_desa",
                    new[] { E(a["hewan_anjing"], 60, 2, 4), E(a["hewan_gagak"], 40, 5, 8) },
                    new[] { E(z["zombie_desa"], 100) }),
            };
            return t;
        }

        private static RegionSpawnTable Table(string name, RegionType region, string counter, SpawnEntry[] day, SpawnEntry[] night)
        {
            var t = LoadOrCreate<RegionSpawnTable>($"{SpawnFolder}/{name}.asset");
            t.region = region; t.dayEntries = day; t.nightEntries = night; t.campCounterEnemyId = counter;
            EditorUtility.SetDirty(t);
            return t;
        }

        // ---------------------------------------------------------------- Prefab musuh

        private static GameObject EnemyBase(EnemyData data, string tag, Color color, float height, float radius)
        {
            // README Langkah 5 nomor 3-4: kapsul di layer Enemy, NavMeshAgent, EnemyHealth, EnemyInstance, collider hitbox.
            var go = new GameObject(data.enemyId);
            go.layer = Layer(PhysicsMasks.EnemyLayer);
            go.tag = tag;
            var body = go.AddComponent<CapsuleCollider>();
            body.height = height;
            body.radius = radius;
            body.center = new Vector3(0f, height / 2f, 0f);
            var agent = go.AddComponent<NavMeshAgent>();
            agent.radius = radius;
            agent.height = height;
            agent.speed = data.moveSpeed;
            var health = go.AddComponent<EnemyHealth>();
            Set(health, "data", data);
            go.AddComponent<EnemyInstance>();

            var visual = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            visual.name = "Body";
            Object.DestroyImmediate(visual.GetComponent<Collider>());
            visual.transform.SetParent(go.transform, false);
            visual.transform.localPosition = new Vector3(0f, height / 2f, 0f);
            visual.transform.localScale = new Vector3(radius * 2f, height / 2f, radius * 2f);
            var mat = Mat("Greybox_" + data.enemyId, color);
            var r = visual.GetComponent<MeshRenderer>();
            r.sharedMaterial = mat;

            // Hitbox (HitboxModifier.md): Head, Torso, Leg di layer EnemyHitbox, bukan trigger.
            Hitbox(go.transform, "Hitbox_Head", BodyPart.Head, new Vector3(0f, height * 0.9f, 0f), Vector3.one * radius * 0.9f);
            Hitbox(go.transform, "Hitbox_Torso", BodyPart.Torso, new Vector3(0f, height * 0.55f, 0f), new Vector3(radius * 1.8f, height * 0.45f, radius * 1.2f));
            Hitbox(go.transform, "Hitbox_Legs", BodyPart.Leg, new Vector3(0f, height * 0.18f, 0f), new Vector3(radius * 1.4f, height * 0.35f, radius));

            var hit = go.AddComponent<EnemyHitReaction>();
            Set(hit, "bodyCapsule", body);
            SetArray(hit, "renderers", new Object[] { r });
            return go;
        }

        private static void Hitbox(Transform parent, string name, BodyPart part, Vector3 pos, Vector3 size)
        {
            var go = new GameObject(name);
            go.layer = Layer(PhysicsMasks.EnemyHitboxLayer);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = pos;
            go.AddComponent<BoxCollider>().size = size;
            go.AddComponent<HitboxModifier>().bodyPart = part;
        }

        private static GameObject BuildZombiePrefab(ZombieData data)
        {
            Color c = data.enemyId switch
            {
                "zombie_tribal" => new Color(0.55f, 0.35f, 0.2f),
                "zombie_lumut" => new Color(0.2f, 0.45f, 0.15f),
                "zombie_militer" => new Color(0.3f, 0.35f, 0.25f),
                "zombie_beku" => new Color(0.6f, 0.8f, 0.95f),
                "zombie_desa" => new Color(0.5f, 0.45f, 0.4f),
                _ => new Color(0.35f, 0.55f, 0.3f),
            };
            float h = data.isLarge ? 2.4f : 1.8f, rad = data.isLarge ? 0.55f : 0.35f;
            var go = EnemyBase(data, "Zombie", c, h, rad);
            var eye = Child(go.transform, "Eye", new Vector3(0f, h * 0.9f, 0.1f));
            var sensory = go.AddComponent<AISensory>();
            Set(sensory, "eyePoint", eye);
            go.AddComponent<EnemyAttack>();
            go.AddComponent<SunlightExposure>();
            go.AddComponent<TraversalAgent>();
            if (data.secondaryAttack != null && data.secondaryAttack.attackId == "Zombie_Rock_Throw")
                Set(go.GetComponent<TraversalAgent>(), "rockAttack", data.secondaryAttack);
            go.AddComponent<EnemyAI>();
            switch (data.enemyId)
            {
                case "zombie_tribal":
                    var tribal = go.AddComponent<TribalBehavior>();
                    Set(tribal, "throwPoint", Child(go.transform, "ThrowPoint", new Vector3(0.3f, h * 0.8f, 0.3f)));
                    Set(tribal, "headPoint", eye);
                    break;
                case "zombie_lumut": go.AddComponent<MossBehavior>(); break;
                case "zombie_militer": go.AddComponent<MilitaryBehavior>(); break;
                case "zombie_beku":
                    var frozen = go.AddComponent<FrozenBehavior>();
                    Set(frozen, "flashRenderer", go.transform.Find("Body").GetComponent<Renderer>());
                    go.AddComponent<NoiseEmitter>();   // SpecialZombieAI.md: Beku memakai NoiseEmitter
                    break;
                case "zombie_desa":
                    go.AddComponent<VillageBehavior>();
                    go.AddComponent<VillageZombieHiding>();   // Rujukan kosong memakai cadangan otomatis (SunlightExposure.md)
                    break;
            }
            return SavePrefab(go, $"{EnemyPrefabFolder}/{data.enemyId}.prefab");
        }

        private static GameObject BuildAnimalPrefab(AnimalData data)
        {
            bool crow = data.enemyId == "hewan_gagak";
            bool small = crow || data.enemyId == "hewan_ular";
            float h = data.enemyId == "hewan_beruang" ? 1.6f : small ? 0.4f : 1f;
            float rad = data.enemyId == "hewan_beruang" ? 0.7f : small ? 0.2f : 0.4f;
            Color c = crow ? Color.black : data.enemyId == "hewan_ular" ? new Color(0.3f, 0.5f, 0.2f)
                : data.enemyId == "hewan_jaguar" ? new Color(0.85f, 0.65f, 0.25f) : data.enemyId == "hewan_beruang" ? Color.white
                : data.enemyId == "hewan_babi" ? new Color(0.45f, 0.3f, 0.25f) : new Color(0.5f, 0.5f, 0.52f);
            var go = EnemyBase(data, "Animal", c, h, rad);
            go.AddComponent<EnemyAttack>();
            var wild = go.AddComponent<WildlifeAI>();
            Set(wild, "headPoint", Child(go.transform, "Head", new Vector3(0f, h * 0.8f, rad)));
            if (data.leavesCarcass)
            {
                // AnimalCarcass: collider anak di layer Interactable untuk menguliti.
                var carcassCol = new GameObject("CarcassInteract");
                carcassCol.layer = Layer(PhysicsMasks.InteractableLayer);
                carcassCol.transform.SetParent(go.transform, false);
                carcassCol.transform.localPosition = new Vector3(0f, h / 2f, 0f);
                var bc = carcassCol.AddComponent<BoxCollider>();
                bc.size = new Vector3(rad * 2f, h, rad * 3f);
                bc.enabled = false;
                var carcass = go.AddComponent<AnimalCarcass>();
                Set(carcass, "interactCollider", bc);
                SetArray(carcass, "renderers", new Object[] { go.transform.Find("Body").GetComponent<Renderer>() });
            }
            return SavePrefab(go, $"{EnemyPrefabFolder}/{data.enemyId}.prefab");
        }

        private static GameObject BuildSnarePrefab()
        {
            // Jerat Tribal: layer Projectile, komponen TribalSnare (README Langkah 5 nomor 5).
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "Tribal_Snare";
            go.transform.localScale = Vector3.one * 0.3f;
            go.layer = Layer(PhysicsMasks.ProjectileLayer);
            go.GetComponent<Collider>().isTrigger = true;
            go.GetComponent<MeshRenderer>().sharedMaterial = Mat("Greybox_Snare", new Color(0.6f, 0.5f, 0.3f));
            go.AddComponent<TribalSnare>();
            return SavePrefab(go, $"{EnemyPrefabFolder}/Tribal_Snare.prefab");
        }

        private static void RegisterPools(List<(string tag, GameObject prefab, int size)> pools)
        {
            var contents = PrefabUtility.LoadPrefabContents(PersistentRootPath);
            try
            {
                var pooler = contents.GetComponentInChildren<ObjectPooler>(true);
                foreach (var p in pools) Upsert(pooler.pools, p.tag, p.prefab, p.size);
                PrefabUtility.SaveAsPrefabAsset(contents, PersistentRootPath);
            }
            finally { PrefabUtility.UnloadPrefabContents(contents); }
        }

        // ---------------------------------------------------------------- Pemain dan scene

        private static void PatchPlayers()
        {
            // README Langkah 5 nomor 2.
            var island = PrefabUtility.LoadPrefabContents(IslandPlayerPath);
            var target = Ensure<PlayerTargetable>(island);
            Ensure<PlayerPinnedState>(island);
            Ensure<PlayerShove>(island);
            var noise = Ensure<NoiseEmitter>(island);
            var so = new SerializedObject(noise);
            so.FindProperty("isPlayerMovement").boolValue = true;
            so.ApplyModifiedPropertiesWithoutUndo();
            PrefabUtility.SaveAsPrefabAsset(island, IslandPlayerPath);
            PrefabUtility.UnloadPrefabContents(island);

            var ship = PrefabUtility.LoadPrefabContents(ShipPlayerPath);
            Ensure<PlayerTargetable>(ship);
            PrefabUtility.SaveAsPrefabAsset(ship, ShipPlayerPath);
            PrefabUtility.UnloadPrefabContents(ship);
        }

        private static T Ensure<T>(GameObject go) where T : Component
        {
            var c = go.GetComponent<T>();
            return c != null ? c : go.AddComponent<T>();
        }

        private static void SetupIsland(RegionSpawnTable[] tables)
        {
            var scene = EditorSceneManager.OpenScene(IslandScenePath, OpenSceneMode.Single);
            var old = GameObject.Find("Langkah5Systems");
            if (old != null) Object.DestroyImmediate(old);
            var root = new GameObject("Langkah5Systems").transform;
            var spawn = Add<SpawnManager>(root, "SpawnManager");
            SetArray(spawn, "tables", tables);
            var traversal = Add<AITraversal>(root, "AITraversal");
            SetArray(traversal, "tablesByRegion", tables);
            Add<AIDirector>(root, "AIDirector");
            Add<ThreatSystem>(root, "ThreatSystem");
            Add<AISensoryHub>(root, "AISensoryHub");
            Add<HordeCrowdManager>(root, "HordeCrowdManager");
            Add<AttackCoordinator>(root, "AttackCoordinator");

            // NavMesh pulau: satu NavMeshSurface di akar, mengumpulkan Terrain, node, dan batu (layer Terrain + Obstacle).
            var navGo = new GameObject("IslandNavMesh");
            navGo.transform.SetParent(root, false);
            var surface = navGo.AddComponent<NavMeshSurface>();
            surface.collectObjects = CollectObjects.All;
            surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            surface.layerMask = (1 << Layer(PhysicsMasks.TerrainLayer)) | (1 << Layer(PhysicsMasks.ObstacleLayer));
            surface.BuildNavMesh();
            if (surface.navMeshData != null)
            {
                AssetDatabase.DeleteAsset(IslandNavPath);
                AssetDatabase.CreateAsset(surface.navMeshData, IslandNavPath);
            }
            else Debug.LogError("[Langkah5Setup] Bake NavMesh pulau gagal.");
            EditorSceneManager.SaveScene(scene);
        }

        private static void SetupShip()
        {
            // README Langkah 5 nomor 1: ShipPhase cukup HordeCrowdManager dan AttackCoordinator.
            var scene = EditorSceneManager.OpenScene(ShipScenePath, OpenSceneMode.Single);
            var old = GameObject.Find("Langkah5Systems");
            if (old != null) Object.DestroyImmediate(old);
            var root = new GameObject("Langkah5Systems").transform;
            Add<HordeCrowdManager>(root, "HordeCrowdManager");
            Add<AttackCoordinator>(root, "AttackCoordinator");
            EditorSceneManager.SaveScene(scene);
        }

        private static T Add<T>(Transform parent, string name) where T : Component
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            return go.AddComponent<T>();
        }
    }
}
