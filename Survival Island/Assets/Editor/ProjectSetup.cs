#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SurvivalIsland.EditorTools
{
    /// <summary>
    /// Menu satu klik untuk pengaturan proyek yang tidak tersimpan di kode gameplay.
    /// Aman dijalankan berulang: aset yang sudah ada dipakai ulang.
    /// - URP: `PostProcessController` dan `EnvironmentProfile` memakai Universal RP.
    /// - Input: `InputManager` (#18) memakai Input System baru. Diatur ke "Both" supaya
    ///   paket pihak ketiga yang masih memakai Input lama tetap jalan.
    /// </summary>
    internal static class ProjectSetup
    {
        private const string SettingsFolder = "Assets/Settings";
        private const string PipelinePath = SettingsFolder + "/SurvivalIsland_URP.asset";
        private const string RendererPath = SettingsFolder + "/SurvivalIsland_URP_Renderer.asset";

        // activeInputHandler di ProjectSettings: 0 = Input lama, 1 = Input System, 2 = keduanya.
        private const int InputHandlerBoth = 2;

        [MenuItem("Survival Island/Setup/Terapkan Pengaturan Proyek")]
        public static void ApplyAll()
        {
            bool restartNeeded = ApplyInputHandler();
            ApplyUrp();
            ApplyCollisionMatrix();
            AssetDatabase.SaveAssets();
            Debug.Log("[ProjectSetup] Pengaturan proyek diterapkan." +
                      (restartNeeded ? " Input handler berubah: restart Unity supaya berlaku." : string.Empty));
        }

        [MenuItem("Survival Island/Setup/Pasang URP")]
        public static void ApplyUrp()
        {
            if (!Directory.Exists(SettingsFolder))
            {
                AssetDatabase.CreateFolder("Assets", "Settings");
            }

            var pipeline = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(PipelinePath);
            if (pipeline == null)
            {
                var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(RendererPath);
                if (rendererData == null)
                {
                    rendererData = ScriptableObject.CreateInstance<UniversalRendererData>();
                    AssetDatabase.CreateAsset(rendererData, RendererPath);
                }

                pipeline = UniversalRenderPipelineAsset.Create(rendererData);
                AssetDatabase.CreateAsset(pipeline, PipelinePath);
            }

            GraphicsSettings.defaultRenderPipeline = pipeline;
            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                QualitySettings.SetQualityLevel(i, false);
                QualitySettings.renderPipeline = pipeline;
            }

            Debug.Log("[ProjectSetup] URP dipasang: " + PipelinePath);
        }

        // Blueprint Core_Logic/PhysicsLayers.md, tabel "Layer Collision Matrix" (#87).
        // Setiap baris: layer -> layer yang bertabrakan dengannya. Dibuat simetris saat diterapkan.
        private static readonly string[][] CollisionRows =
        {
            new[] { "Player", "Enemy", "NPC", "Terrain", "Structure", "Obstacle", "Projectile", "ZoneTrigger", "Water" },
            new[] { "PlayerViewmodel" },
            new[] { "Enemy", "Player", "Enemy", "NPC", "Terrain", "Structure", "Obstacle", "Projectile", "ZoneTrigger", "Water" },
            new[] { "EnemyHitbox" },
            new[] { "NPC", "Player", "Enemy", "NPC", "Terrain", "Structure", "Obstacle", "Projectile", "ZoneTrigger", "Water" },
            new[] { "Interactable", "Terrain", "Structure", "Obstacle" },
            new[] { "WorldItem", "Terrain", "Structure", "Obstacle", "Water" },
            new[] { "Terrain", "Player", "Enemy", "NPC", "Interactable", "WorldItem", "Terrain", "Structure", "Obstacle", "Projectile", "Ragdoll", "HeavyItem" },
            new[] { "Structure", "Player", "Enemy", "NPC", "Interactable", "WorldItem", "Terrain", "Structure", "Obstacle", "Projectile", "Ragdoll", "HeavyItem" },
            new[] { "StructureGhost" },
            new[] { "Obstacle", "Player", "Enemy", "NPC", "Interactable", "WorldItem", "Terrain", "Structure", "Obstacle", "Projectile", "Ragdoll", "HeavyItem" },
            new[] { "Water", "Player", "Enemy", "NPC", "WorldItem", "HeavyItem" },
            new[] { "ZoneTrigger", "Player", "Enemy", "NPC", "HeavyItem" },
            new[] { "Projectile", "Player", "Enemy", "NPC", "Terrain", "Structure", "Obstacle" },
            new[] { "Ragdoll", "Terrain", "Structure", "Obstacle" },
            new[] { "HeavyItem", "Terrain", "Structure", "Obstacle", "Water", "ZoneTrigger" },
        };

        [MenuItem("Survival Island/Setup/Matriks Tabrakan")]
        public static void ApplyCollisionMatrix()
        {
            var layers = new System.Collections.Generic.List<int>();
            foreach (var row in CollisionRows)
            {
                int layer = LayerMask.NameToLayer(row[0]);
                if (layer < 0)
                {
                    Debug.LogError("[ProjectSetup] Layer belum ada: " + row[0] + ". Buat dulu di Tags and Layers.");
                    return;
                }
                layers.Add(layer);
            }

            // Mulai dari tidak bertabrakan sama sekali di antara layer resmi.
            foreach (int a in layers)
            {
                foreach (int b in layers)
                {
                    Physics.IgnoreLayerCollision(a, b, true);
                }
            }

            int pairs = 0;
            foreach (var row in CollisionRows)
            {
                int a = LayerMask.NameToLayer(row[0]);
                for (int i = 1; i < row.Length; i++)
                {
                    Physics.IgnoreLayerCollision(a, LayerMask.NameToLayer(row[i]), false);
                    pairs++;
                }
            }

            Debug.Log("[ProjectSetup] Matriks tabrakan diterapkan untuk " + layers.Count + " layer resmi (" + pairs + " pasangan).");
        }

        [MenuItem("Survival Island/Setup/Input System (Both)")]
        public static bool ApplyInputHandler()
        {
            var assets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset");
            if (assets == null || assets.Length == 0)
            {
                Debug.LogError("[ProjectSetup] ProjectSettings.asset tidak ditemukan.");
                return false;
            }

            var so = new SerializedObject(assets[0]);
            var prop = so.FindProperty("activeInputHandler");
            if (prop == null)
            {
                Debug.LogError("[ProjectSetup] Properti activeInputHandler tidak ditemukan.");
                return false;
            }

            if (prop.intValue == InputHandlerBoth)
            {
                return false;
            }

            prop.intValue = InputHandlerBoth;
            so.ApplyModifiedPropertiesWithoutUndo();
            Debug.Log("[ProjectSetup] Active Input Handling diatur ke Both.");
            return true;
        }
    }
}
#endif
