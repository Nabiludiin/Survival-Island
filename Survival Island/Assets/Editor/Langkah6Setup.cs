using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static SurvivalIsland.EditorTools.Langkah2Setup;

namespace SurvivalIsland.EditorTools
{
    /// <summary>
    /// Penyiapan Unity untuk Langkah 6 (LOGIC/README.md, "Penyiapan di Unity untuk Langkah 6"). Membuat:
    /// - Terrain greybox Region 2 (Amazon), 3 (Salju), dan 4 (Desa) di koordinat peta usulan RegionStreaming.md,
    ///   tebing pembatas, jurang S1 dengan BridgeSite, celah S2 dan S3 dengan GateBarrier/GateBlocker, dan helipad greybox;
    /// - 4 trigger ZoneManager (layer ZoneTrigger, tumpang tindih 10 m di seam);
    /// - RegionalWeather + 4 tabel, WeatherManager, EnvironmentDirector + 4 profil, RegionGateSystem, HeavyItemSystem
    ///   + 5 HeavyItemData + PartSpawnPointTable (10 titik di Region 4);
    /// - pool Fx_* cuaca dan suasana, WarmthSystem dan CarryAnchor di Player_Island, viewmodel Obor (TorchController);
    /// - NavMesh pulau dibake ulang untuk seluruh peta.
    /// Jalankan setelah menu Langkah 5.
    /// </summary>
    public static class Langkah6Setup
    {
        private const string RegionFolder = "Assets/Data/Region";
        private const string SurfaceFolder = "Assets/Data/Surface";
        private const string PartPrefabFolder = "Assets/Prefabs/HeavyItems";
        private const string FxFolder = "Assets/Prefabs/Fx";
        private const string PlayerFolder = "Assets/Prefabs/Player";
        private const string PersistentRootPath = "Assets/Prefabs/PersistentRoot.prefab";
        private const string IslandPlayerPath = "Assets/Prefabs/Player/Player_Island.prefab";
        private const string IslandScenePath = "Assets/Scenes/IslandPhase.unity";
        private const string IslandNavPath = "Assets/Scenes/IslandPhase_NavMesh.asset";

        private const float WallHeight = 40f, ZoneBottom = -60f, ZoneTop = 400f, ZoneOverlap = 10f;

        [MenuItem("Survival Island/Setup/Langkah 6 - Region dan Cuaca")]
        public static void Run()
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(IslandPlayerPath) == null)
            {
                Debug.LogError("[Langkah6Setup] Player_Island belum ada. Jalankan menu Langkah 3 sampai 5 dulu.");
                return;
            }
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            foreach (var f in new[] { RegionFolder, SurfaceFolder, PartPrefabFolder, FxFolder }) EnsureFolder(f);
            CreateMaterials();

            var weatherTables = CreateWeatherTables();
            var envProfiles = CreateEnvProfiles();
            var parts = CreateHeavyItems();
            var spawnTable = CreatePartSpawnTable();
            RegisterFxPools();
            var torch = BuildTorchViewmodel();
            var torchItem = AssetDatabase.LoadAssetAtPath<ItemData>("Assets/Resources/Items/Tool_Torch.asset");
            if (torchItem != null) { torchItem.equipPrefab = torch; EditorUtility.SetDirty(torchItem); }
            PatchPlayer();

            BuildScene(weatherTables, envProfiles, parts, spawnTable);

            AssetDatabase.SaveAssets();
            Debug.Log("[Langkah6Setup] Selesai: Region 2-4 greybox, jurang S1 + BridgeSite, gerbang S2/S3, 4 zona, cuaca, suasana, " +
                      "5 suku cadang + 10 titik, pool Fx, WarmthSystem, Obor, dan NavMesh seluruh pulau.");
        }

        // ---------------------------------------------------------------- Data

        private static RegionWeatherTable[] CreateWeatherTables()
        {
            // RegionalWeather.md: isi lewat menu konteks "Isi nilai awal region" setelah region diisi.
            var list = new List<RegionWeatherTable>();
            foreach (RegionType r in new[] { RegionType.Volcano, RegionType.Amazon, RegionType.Snow, RegionType.Logistic })
            {
                var t = LoadOrCreate<RegionWeatherTable>($"{RegionFolder}/Weather_{r}.asset");
                t.region = r;
                Invoke(t, "FillDefaults");
                EditorUtility.SetDirty(t);
                list.Add(t);
            }
            return list.ToArray();
        }

        private static RegionEnvironmentProfile[] CreateEnvProfiles()
        {
            // EnvironmentProfile.md: menu konteks mengisi kabut, ambient, matahari, dan partikel suasana.
            // Kurva per jam, skybox, dan postProfile belum ada nilainya di dokumen (tetap nilai awal).
            // Urutan profiles mengikuti enum RegionType: Volcano, Amazon, Snow, Logistic.
            var list = new List<RegionEnvironmentProfile>();
            foreach (RegionType r in new[] { RegionType.Volcano, RegionType.Amazon, RegionType.Snow, RegionType.Logistic })
            {
                var p = LoadOrCreate<RegionEnvironmentProfile>($"{RegionFolder}/Env_{r}.asset");
                p.region = r;
                Invoke(p, "FillDefaults");
                EditorUtility.SetDirty(p);
                list.Add(p);
            }
            return list.ToArray();
        }

        private static void Invoke(Object target, string method)
        {
            var m = target.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (m != null) m.Invoke(target, null);
            else Debug.LogError($"[Langkah6Setup] Menu konteks {method} tidak ada di {target.GetType().Name}.");
        }

        private static HeavyItemData[] CreateHeavyItems()
        {
            // HeavyItemSystem.md tabel suku cadang: Rotor 80, Engine 150, FuelTank 100 (GDD), TailRotor 40, Avionics 30 (usulan).
            // Prefab: layer dan tag HeavyItem, HeavyItem, Rigidbody, collider bukan trigger (README Langkah 6 nomor 5).
            var defs = new (string id, float kg, Vector3 size, Color c)[]
            {
                ("Part_Rotor", 80f, new Vector3(3f, 0.2f, 0.4f), new Color(0.3f, 0.3f, 0.35f)),
                ("Part_Engine", 150f, new Vector3(1.2f, 0.9f, 1f), new Color(0.4f, 0.25f, 0.2f)),
                ("Part_FuelTank", 100f, new Vector3(0.8f, 0.8f, 1.4f), new Color(0.7f, 0.15f, 0.1f)),
                ("Part_TailRotor", 40f, new Vector3(1.4f, 0.15f, 0.3f), new Color(0.35f, 0.35f, 0.4f)),
                ("Part_Avionics", 30f, new Vector3(0.6f, 0.4f, 0.4f), new Color(0.2f, 0.4f, 0.3f)),
            };
            var list = new List<HeavyItemData>();
            foreach (var d in defs)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.name = d.id;
                go.transform.localScale = d.size;
                go.layer = Layer(PhysicsMasks.HeavyItemLayer);
                go.tag = "HeavyItem";
                go.GetComponent<MeshRenderer>().sharedMaterial = Mat("Greybox_" + d.id, d.c);
                go.AddComponent<Rigidbody>().mass = d.kg;
                go.AddComponent<HeavyItem>();
                var prefab = SavePrefab(go, $"{PartPrefabFolder}/{d.id}.prefab");

                var data = LoadOrCreate<HeavyItemData>($"{RegionFolder}/{d.id}.asset");
                data.partId = d.id;
                data.weightKg = d.kg;
                data.prefab = prefab;
                data.speedMultiplierOverride = 0f;   // 0 = heavyItem.speedMultiplier 0,5 (GDD bagian 5 Region 4)
                EditorUtility.SetDirty(data);
                list.Add(data);
            }
            return list.ToArray();
        }

        private static PartSpawnPointTable CreatePartSpawnTable()
        {
            // HeavyItemSystem.md: semua suku cadang di Region 4, minimal 10 titik untuk 5 barang, jarak minimal 30 m
            // (runSeed.minPartPointSpacing). Letak titik tidak tertulis: greybox di sekitar pusat desa (500, 1750).
            var t = LoadOrCreate<PartSpawnPointTable>($"{RegionFolder}/PartSpawnPoints_Village.asset");
            var pts = new List<PartSpawnPoint>();
            Vector3 center = new Vector3(500f, 0.6f, 1600f);
            for (int i = 0; i < 10; i++)
            {
                float x = center.x - 150f + (i % 5) * 75f;
                float z = center.z + (i / 5) * 120f;
                pts.Add(new PartSpawnPoint { position = new Vector3(x, 0.6f, z), eulerRotation = Vector3.zero, isReachable = true });
            }
            t.points = pts.ToArray();
            EditorUtility.SetDirty(t);
            return t;
        }

        private static void RegisterFxPools()
        {
            // ObjectPooler.md: Fx_Snow dan Fx_Blizzard 2 per tag. Tag suasana lain tidak punya ukuran tertulis; dipakai 2
            // (satu aktif dan satu memudar, EnvironmentDirector.md). Prefab: ParticleSystem di akar, tanpa PooledEffect.
            var tags = new (string tag, Color c)[]
            {
                ("Fx_Snow", Color.white), ("Fx_Blizzard", new Color(0.9f, 0.95f, 1f)), ("Fx_AshLight", new Color(0.3f, 0.3f, 0.3f)),
                ("Fx_Spores", new Color(0.8f, 0.9f, 0.5f)), ("Fx_SnowDust", Color.white), ("Fx_Mist", new Color(0.8f, 0.85f, 0.9f)),
            };
            var contents = PrefabUtility.LoadPrefabContents(PersistentRootPath);
            try
            {
                var pooler = contents.GetComponentInChildren<ObjectPooler>(true);
                foreach (var (tag, c) in tags) Upsert(pooler.pools, tag, BuildFxPrefab(tag, c), 2);
                PrefabUtility.SaveAsPrefabAsset(contents, PersistentRootPath);
            }
            finally { PrefabUtility.UnloadPrefabContents(contents); }
        }

        private static GameObject BuildFxPrefab(string tag, Color color)
        {
            var go = new GameObject(tag);
            var ps = go.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.loop = true;
            main.playOnAwake = true;
            main.startColor = color;
            main.startSize = 0.08f;
            main.startLifetime = 4f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(30f, 1f, 30f);
            var r = go.GetComponent<ParticleSystemRenderer>();
            r.sharedMaterial = Mat("Greybox_FxParticle", Color.white, unlit: true, transparent: true);
            return SavePrefab(go, $"{FxFolder}/{tag}.prefab");
        }

        private static EquippableBehaviour BuildTorchViewmodel()
        {
            // TorchController: Light dan ParticleSystem (README Langkah 6 nomor 3). Jangkauan cahaya tidak tertulis: nilai
            // awal komponen Light Unity.
            var root = new GameObject("Viewmodel_Torch");
            var stick = Box(root.transform, "Stick", new Vector3(-0.2f, -0.2f, 0.4f), new Vector3(0.04f, 0.04f, 0.4f), Mat("Greybox_Viewmodel", new Color(0.2f, 0.2f, 0.22f)), PhysicsMasks.PlayerViewmodelLayer);
            Object.DestroyImmediate(stick.GetComponent<Collider>());
            var flameGo = new GameObject("Flame");
            flameGo.transform.SetParent(root.transform, false);
            flameGo.transform.localPosition = new Vector3(-0.2f, -0.15f, 0.6f);
            var flame = flameGo.AddComponent<ParticleSystem>();
            var main = flame.main;
            main.startColor = new Color(1f, 0.6f, 0.2f);
            main.startSize = 0.1f;
            main.startLifetime = 0.5f;
            flameGo.GetComponent<ParticleSystemRenderer>().sharedMaterial = Mat("Greybox_FxParticle", Color.white, unlit: true, transparent: true);
            var light = flameGo.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = new Color(1f, 0.7f, 0.4f);
            var torch = root.AddComponent<TorchController>();
            Set(torch, "torchLight", light);
            Set(torch, "flame", flame);
            foreach (var t in root.GetComponentsInChildren<Transform>(true)) t.gameObject.layer = Layer(PhysicsMasks.PlayerViewmodelLayer);
            return SavePrefab(root, $"{PlayerFolder}/Viewmodel_Torch.prefab").GetComponent<EquippableBehaviour>();
        }

        private static void PatchPlayer()
        {
            // README Langkah 6 nomor 3: WarmthSystem di samping SurvivalStats. CarryAnchor di depan kamera untuk HeavyItemSystem.
            var p = PrefabUtility.LoadPrefabContents(IslandPlayerPath);
            if (p.GetComponent<WarmthSystem>() == null) p.AddComponent<WarmthSystem>();
            var offset = p.transform.Find("CameraRig/PitchPivot/OffsetPivot");
            if (offset != null && offset.Find("CarryAnchor") == null) Child(offset, "CarryAnchor", new Vector3(0f, -0.5f, 1.4f));
            PrefabUtility.SaveAsPrefabAsset(p, IslandPlayerPath);
            PrefabUtility.UnloadPrefabContents(p);
        }

        // ---------------------------------------------------------------- Scene

        private static void BuildScene(RegionWeatherTable[] weather, RegionEnvironmentProfile[] env, HeavyItemData[] parts, PartSpawnPointTable spawn)
        {
            var scene = EditorSceneManager.OpenScene(IslandScenePath, OpenSceneMode.Single);
            foreach (var n in new[] { "Langkah6World", "Langkah6Systems" })
            {
                var old = GameObject.Find(n);
                if (old != null) Object.DestroyImmediate(old);
            }
            var world = new GameObject("Langkah6World").transform;

            // Terrain Region 2, 3, 4 (datar). Region 2 dimulai setelah jurang S1 (lebar 30 m).
            var r2 = RegionTerrain(world, "Terrain_R2_Amazon", IslandLayout.R2Min + new Vector3(0f, 0f, IslandLayout.RavineWidth), IslandLayout.RegionSize - IslandLayout.RavineWidth,
                "TL_Jungle", new Color(0.12f, 0.35f, 0.12f), SurfaceType.Tanah);
            RegionTerrain(world, "Terrain_R4_Village", IslandLayout.R4Min, IslandLayout.RegionSize, "TL_Dirt", new Color(0.5f, 0.42f, 0.3f), SurfaceType.Tanah);
            RegionTerrain(world, "Terrain_R3_Snow", IslandLayout.R3Min, IslandLayout.RegionSize, "TL_Snow", new Color(0.92f, 0.94f, 0.97f), SurfaceType.Salju);
            // Region 1 dari menu Langkah 4 berakhir di Z 1000; tepi utaranya menjadi bibir jurang.

            var cliff = Mat("Greybox_Cliff", new Color(0.35f, 0.33f, 0.32f));
            BuildCliffs(world, cliff);
            var bridge = BuildRavineAndBridge(world, cliff);
            var s2Barrier = Box(world, "GateBarrier_S2", IslandLayout.S2 + new Vector3(0f, WallHeight / 2f, 0f), new Vector3(2f, WallHeight, 30f), Mat("Greybox_Gate", new Color(0.6f, 0.2f, 0.2f)), PhysicsMasks.ObstacleLayer);
            var s2Blocker = Box(world, "GateBlocker_S2", IslandLayout.S2 + new Vector3(4f, 1.5f, 0f), new Vector3(1f, 3f, 6f), Mat("Greybox_Gate", new Color(0.6f, 0.2f, 0.2f)), PhysicsMasks.InteractableLayer);
            s2Blocker.AddComponent<GateBlocker>();
            var s3Barrier = Box(world, "GateBarrier_S3", IslandLayout.S3 + new Vector3(0f, WallHeight / 2f, 0f), new Vector3(30f, WallHeight, 2f), Mat("Greybox_Gate", new Color(0.6f, 0.2f, 0.2f)), PhysicsMasks.ObstacleLayer);

            // Helipad greybox (Region 3). Komponen helikopter dipasang menu Langkah 7.
            var pad = Box(world, "Helipad", IslandLayout.Helipad + new Vector3(0f, 0.1f, 0f), new Vector3(20f, 0.2f, 20f), Mat("Greybox_Helipad", new Color(0.3f, 0.3f, 0.32f)), PhysicsMasks.TerrainLayer);
            pad.AddComponent<SurfaceTag>().type = SurfaceType.Logam;
            var helipadNoBuild = new GameObject("NoBuild_Helipad");
            helipadNoBuild.transform.SetParent(world, false);
            helipadNoBuild.transform.position = IslandLayout.Helipad;
            var nb = helipadNoBuild.AddComponent<NoBuildVolume>();
            SetVector(nb, "size", new Vector3(30f, 20f, 30f));

            BuildZones(world);

            // Sistem.
            var systems = new GameObject("Langkah6Systems").transform;
            var rw = AddTo<RegionalWeather>(systems, "RegionalWeather");
            SetArray(rw, "tables", weather);

            var wmGo = new GameObject("WeatherManager");
            wmGo.transform.SetParent(systems, false);
            var rain = FxChild(wmGo.transform, "Rain", new Color(0.7f, 0.8f, 1f));
            var leaves = FxChild(wmGo.transform, "StormLeaves", new Color(0.3f, 0.5f, 0.2f));
            var deckA = wmGo.AddComponent<AudioSource>(); deckA.loop = true; deckA.playOnAwake = false; deckA.spatialBlend = 0f;
            var deckB = wmGo.AddComponent<AudioSource>(); deckB.loop = true; deckB.playOnAwake = false; deckB.spatialBlend = 0f;
            var wm = wmGo.AddComponent<WeatherManager>();
            Set(wm, "rainParticles", rain);
            Set(wm, "stormLeavesParticles", leaves);
            Set(wm, "deckA", deckA);
            Set(wm, "deckB", deckB);

            var ed = AddTo<EnvironmentDirector>(systems, "EnvironmentDirector");
            SetArray(ed, "profiles", env);
            var sun = Object.FindAnyObjectByType<Light>();
            foreach (var l in Object.FindObjectsByType<Light>(FindObjectsSortMode.None)) if (l.type == LightType.Directional) sun = l;
            Set(ed, "sun", sun);

            var gates = AddTo<RegionGateSystem>(systems, "RegionGateSystem");
            var so = new SerializedObject(gates);
            var rules = so.FindProperty("rules");
            rules.arraySize = 3;
            // RegionGateSystem.md: S1 Bridge -> Amazon, S2 DiaryChapter 3 -> Logistic, S3 FollowsPrevious -> Snow.
            Rule(rules.GetArrayElementAtIndex(0), "S1", RegionType.Amazon, GateKind.Bridge, 0, bridge.barrier);
            Rule(rules.GetArrayElementAtIndex(1), "S2", RegionType.Logistic, GateKind.DiaryChapter, 3, s2Barrier, s2Blocker);
            Rule(rules.GetArrayElementAtIndex(2), "S3", RegionType.Snow, GateKind.FollowsPrevious, 0, s3Barrier);
            so.ApplyModifiedPropertiesWithoutUndo();

            var heavy = AddTo<HeavyItemSystem>(systems, "HeavyItemSystem");
            SetArray(heavy, "parts", parts);
            Set(heavy, "spawnPoints", spawn);
            var player = GameObject.FindWithTag("Player");
            var anchor = player != null ? player.transform.Find("CameraRig/PitchPivot/OffsetPivot/CarryAnchor") : null;
            Set(heavy, "carryAnchor", anchor);

            RebakeNavMesh();
            EditorSceneManager.SaveScene(scene);
        }

        private static Terrain RegionTerrain(Transform parent, string name, Vector3 min, float lengthZ, string layerName, Color color, SurfaceType surface)
        {
            string dataPath = $"{SurfaceFolder}/{name}Data.asset";
            AssetDatabase.DeleteAsset(dataPath);
            var data = new TerrainData { heightmapResolution = 129 };
            AssetDatabase.CreateAsset(data, dataPath);
            data.size = new Vector3(IslandLayout.RegionSize, 20f, lengthZ);
            data.alphamapResolution = 128;
            var layer = TerrainLayerAsset(layerName, color);
            data.terrainLayers = new[] { layer };
            EditorUtility.SetDirty(data);

            var go = Terrain.CreateTerrainGameObject(data);
            go.name = name;
            go.transform.SetParent(parent, true);
            go.transform.position = min;
            go.layer = Layer(PhysicsMasks.TerrainLayer);

            // SurfaceDetection.md: satu lapisan per region, dibake ke peta permukaan.
            var map = LoadOrCreate<SurfaceLayerMap>($"{SurfaceFolder}/{name}_LayerMap.asset");
            map.layers = new[] { layer };
            map.types = new[] { surface };
            EditorUtility.SetDirty(map);
            var src = go.AddComponent<TerrainSurfaceSource>();
            src.layerMap = map;
            foreach (var guid in AssetDatabase.FindAssets(name + "_SurfaceMap", new[] { "Assets/Scenes" }))
                AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(guid));
            if (!SurfaceBakeTool.Bake(src)) Debug.LogError($"[Langkah6Setup] Bake permukaan {name} gagal.");
            return go.GetComponent<Terrain>();
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

        private static void BuildCliffs(Transform world, Material mat)
        {
            // RegionStreaming.md: tepi region selain seam ditutup tebing, gunung, atau laut. Sudut (0-1000, 0-1000) tebing.
            var root = new GameObject("Cliffs").transform;
            root.SetParent(world, false);
            float h = WallHeight, t = 4f;
            void Wall(string n, Vector3 a, Vector3 b)
            {
                Vector3 c = (a + b) / 2f; c.y = h / 2f;
                Vector3 size = new Vector3(Mathf.Max(t, Mathf.Abs(b.x - a.x)), h, Mathf.Max(t, Mathf.Abs(b.z - a.z)));
                var w = Box(root, n, c, size, mat, PhysicsMasks.ObstacleLayer);
                w.AddComponent<SurfaceTag>().type = SurfaceType.Batu;
            }
            // Region 1: selatan (laut), timur, barat (sudut tebing). Utara = jurang S1.
            Wall("R1_South", new Vector3(1000, 0, -2), new Vector3(2000, 0, -2));
            Wall("R1_East", new Vector3(2002, 0, 0), new Vector3(2002, 0, 1000));
            Wall("R1_West", new Vector3(998, 0, 0), new Vector3(998, 0, 1000));
            // Region 2: timur, utara. Barat = batas dengan Region 4, terbuka hanya di S2 (lebar 30 m).
            Wall("R2_East", new Vector3(2002, 0, 1000), new Vector3(2002, 0, 2000));
            Wall("R2_North", new Vector3(1000, 0, 2002), new Vector3(2000, 0, 2002));
            Wall("R2R4_SouthOfS2", new Vector3(1000, 0, 1000), new Vector3(1000, 0, 1485));
            Wall("R2R4_NorthOfS2", new Vector3(1000, 0, 1515), new Vector3(1000, 0, 2000));
            // Region 4: selatan (sudut tebing), barat. Utara = batas dengan Region 3, terbuka hanya di S3.
            Wall("R4_South", new Vector3(0, 0, 998), new Vector3(1000, 0, 998));
            Wall("R4_West", new Vector3(-2, 0, 1000), new Vector3(-2, 0, 2000));
            Wall("R4R3_WestOfS3", new Vector3(0, 0, 2000), new Vector3(485, 0, 2000));
            Wall("R4R3_EastOfS3", new Vector3(515, 0, 2000), new Vector3(1000, 0, 2000));
            // Region 3: barat, utara, timur.
            Wall("R3_West", new Vector3(-2, 0, 2000), new Vector3(-2, 0, 3000));
            Wall("R3_North", new Vector3(0, 0, 3002), new Vector3(1000, 0, 3002));
            Wall("R3_East", new Vector3(1002, 0, 2000), new Vector3(1002, 0, 3000));
        }

        private struct BridgeParts { public GameObject barrier; }

        private static BridgeParts BuildRavineAndBridge(Transform world, Material cliff)
        {
            // Jurang S1 (RegionGateSystem.md): lebar 30 m sepanjang batas Region 1-2, dasar jurang di bawah lantai,
            // RavineRecoveryVolume 5 m di atas dasar, GateBarrier 30 m di sisi seberang, BridgeSite di tepi Region 1.
            var root = new GameObject("Ravine_S1").transform;
            root.SetParent(world, false);
            float floorY = -20f, w = IslandLayout.RavineWidth, zMid = 1000f + w / 2f;
            Box(root, "RavineFloor", new Vector3(1500f, floorY - 0.5f, zMid), new Vector3(1000f, 1f, w), cliff, PhysicsMasks.TerrainLayer);
            var rec = new GameObject("RavineRecoveryVolume");
            rec.transform.SetParent(root, false);
            rec.transform.position = new Vector3(1500f, floorY + 5f, zMid);
            rec.layer = Layer(PhysicsMasks.ZoneTriggerLayer);
            var recCol = rec.AddComponent<BoxCollider>();
            recCol.isTrigger = true;
            recCol.size = new Vector3(1000f, 2f, w);
            rec.AddComponent<RavineRecoveryVolume>();

            // GateBarrier di sisi Region 2, menutup ujung jembatan sampai seam terbuka. Tinggi 30 m.
            var barrier = Box(root, "GateBarrier_S1", new Vector3(1500f, 15f, 1000f + w + 1f), new Vector3(8f, 30f, 1f), Mat("Greybox_Gate", new Color(0.6f, 0.2f, 0.2f)), PhysicsMasks.ObstacleLayer);

            // BridgeSite: 20 ruas melintasi jurang, deck collider layer Structure, papan progres TMP.
            var site = new GameObject("BridgeSite");
            site.transform.SetParent(root, false);
            site.transform.position = new Vector3(1500f, 0f, 997f);
            site.layer = Layer(PhysicsMasks.InteractableLayer);
            var siteCol = site.AddComponent<BoxCollider>();
            siteCol.size = new Vector3(6f, 2f, 2f);
            siteCol.center = new Vector3(0f, 1f, 0f);
            var wood = Mat("Greybox_WoodStructure", new Color(0.55f, 0.38f, 0.22f));
            var segs = new List<Object>();
            float segLen = (w + 6f) / 20f;
            for (int i = 0; i < 20; i++)
            {
                var seg = Box(site.transform, $"Segment_{i:00}", new Vector3(0f, -0.1f, 3f + segLen * (i + 0.5f)), new Vector3(4f, 0.2f, segLen * 0.95f), wood, PhysicsMasks.StructureLayer);
                Object.DestroyImmediate(seg.GetComponent<Collider>());
                seg.SetActive(false);
                segs.Add(seg.transform);
            }
            var deck = Box(site.transform, "DeckCollider", new Vector3(0f, -0.1f, 3f + (w + 6f) / 2f), new Vector3(4f, 0.2f, w + 6f), wood, PhysicsMasks.StructureLayer);
            deck.GetComponent<MeshRenderer>().enabled = false;
            var deckCol = deck.GetComponent<Collider>();
            deckCol.enabled = false;
            var props = Box(site.transform, "ConstructionProps", new Vector3(3.5f, 0.5f, 0f), new Vector3(2f, 1f, 1f), wood, PhysicsMasks.ObstacleLayer);
            var labelGo = new GameObject("ProgressLabel");
            labelGo.transform.SetParent(site.transform, false);
            labelGo.transform.localPosition = new Vector3(0f, 2.5f, 0f);
            labelGo.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            var label = labelGo.AddComponent<TextMeshPro>();
            label.fontSize = 6;
            label.alignment = TextAlignmentOptions.Center;
            label.rectTransform.sizeDelta = new Vector2(8f, 2f);
            var bs = site.AddComponent<BridgeSite>();
            SetArray(bs, "segments", segs.ToArray());
            Set(bs, "deckCollider", deckCol);
            Set(bs, "progressLabel", label);
            Set(bs, "constructionProps", props);

            // NoBuildVolume di sekitar BridgeSite dan bibir jurang.
            var nbGo = new GameObject("NoBuild_Bridge");
            nbGo.transform.SetParent(root, false);
            nbGo.transform.position = new Vector3(1500f, 0f, 1000f);
            SetVector(nbGo.AddComponent<NoBuildVolume>(), "size", new Vector3(40f, 20f, 60f));
            return new BridgeParts { barrier = barrier };
        }

        private static void BuildZones(Transform world)
        {
            // ZoneManager.md: BoxCollider raksasa IsTrigger di layer ZoneTrigger, satu per region, tumpang tindih 10 m di seam,
            // dari dasar laut sampai puncak.
            var root = new GameObject("Zones").transform;
            root.SetParent(world, false);
            void Zone(string n, RegionType r, Vector3 min)
            {
                var go = new GameObject(n);
                go.transform.SetParent(root, false);
                go.layer = Layer(PhysicsMasks.ZoneTriggerLayer);
                float S = IslandLayout.RegionSize + ZoneOverlap * 2f;
                go.transform.position = min + new Vector3(IslandLayout.RegionSize / 2f, (ZoneBottom + ZoneTop) / 2f, IslandLayout.RegionSize / 2f);
                var box = go.AddComponent<BoxCollider>();
                box.isTrigger = true;
                box.size = new Vector3(S, ZoneTop - ZoneBottom, S);
                var zm = go.AddComponent<ZoneManager>();
                var so = new SerializedObject(zm);
                so.FindProperty("myRegion").enumValueIndex = (int)r;
                so.ApplyModifiedPropertiesWithoutUndo();
            }
            Zone("Zone_R1_Volcano", RegionType.Volcano, IslandLayout.R1Min);
            Zone("Zone_R2_Amazon", RegionType.Amazon, IslandLayout.R2Min);
            Zone("Zone_R4_Village", RegionType.Logistic, IslandLayout.R4Min);
            Zone("Zone_R3_Snow", RegionType.Snow, IslandLayout.R3Min);
        }

        private static void Rule(SerializedProperty p, string seam, RegionType unlocks, GateKind kind, int chapter, params GameObject[] barriers)
        {
            p.FindPropertyRelative("seamId").stringValue = seam;
            p.FindPropertyRelative("unlocks").enumValueIndex = (int)unlocks;
            p.FindPropertyRelative("kind").enumValueIndex = (int)kind;
            p.FindPropertyRelative("diaryChapter").intValue = chapter;
            var b = p.FindPropertyRelative("barriers");
            b.arraySize = barriers.Length;
            for (int i = 0; i < barriers.Length; i++) b.GetArrayElementAtIndex(i).objectReferenceValue = barriers[i];
        }

        private static ParticleSystem FxChild(Transform parent, string name, Color color)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var ps = go.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.playOnAwake = false;
            main.startColor = color;
            main.startSize = 0.05f;
            var em = ps.emission;
            em.rateOverTime = 0f;
            go.GetComponent<ParticleSystemRenderer>().sharedMaterial = Mat("Greybox_FxParticle", Color.white, unlit: true, transparent: true);
            ps.Stop();
            return ps;
        }

        private static T AddTo<T>(Transform parent, string name) where T : Component
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            return go.AddComponent<T>();
        }

        private static void RebakeNavMesh()
        {
            // NavMesh dari menu Langkah 5 dibake ulang supaya mencakup Region 2-4.
            var surface = Object.FindAnyObjectByType<NavMeshSurface>();
            if (surface == null) { Debug.LogError("[Langkah6Setup] NavMeshSurface pulau tidak ada. Jalankan menu Langkah 5."); return; }
            surface.BuildNavMesh();
            if (surface.navMeshData == null) { Debug.LogError("[Langkah6Setup] Bake NavMesh gagal."); return; }
            AssetDatabase.DeleteAsset(IslandNavPath);
            AssetDatabase.CreateAsset(surface.navMeshData, IslandNavPath);
        }
    }
}
