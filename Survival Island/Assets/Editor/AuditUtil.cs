using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SurvivalIsland.EditorTools
{
    /// <summary>
    /// Bantuan bersama alat audit: membaca config, mencari aset, dan memindai scene proyek.
    /// </summary>
    public static class AuditUtil
    {
        public const string ScenesFolder = "Assets/Scenes";
        public const string ConfigPath = "Assets/Data/GameBalanceConfig.asset";

        /// <summary>Scene yang punya pemain dan manager sendiri. Scene lain dianggap bagian pulau (IslandPhase dan region).</summary>
        public static readonly string[] StandaloneScenes = { "Bootstrap", "MainMenu", "ShipPhase" };

        public static GameBalanceConfig LoadConfig()
        {
            var config = AssetDatabase.LoadAssetAtPath<GameBalanceConfig>(ConfigPath);
            if (config != null) return config;
            foreach (var guid in AssetDatabase.FindAssets("t:GameBalanceConfig"))
            {
                config = AssetDatabase.LoadAssetAtPath<GameBalanceConfig>(AssetDatabase.GUIDToAssetPath(guid));
                if (config != null) return config;
            }
            return null;
        }

        public static List<T> FindAssets<T>() where T : UnityEngine.Object
        {
            var list = new List<T>();
            foreach (var guid in AssetDatabase.FindAssets("t:" + typeof(T).Name))
            {
                var a = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
                if (a != null) list.Add(a);
            }
            return list;
        }

        /// <summary>Semua scene di Assets/Scenes. Folder _Recovery tidak ikut.</summary>
        public static List<string> SceneAssetPaths()
        {
            var list = new List<string>();
            if (!Directory.Exists(ScenesFolder)) return list;
            foreach (var p in Directory.GetFiles(ScenesFolder, "*.unity", SearchOption.AllDirectories))
                list.Add(p.Replace('\\', '/'));
            list.Sort(StringComparer.Ordinal);
            return list;
        }

        public static bool IsStandalone(string scenePath)
        {
            string name = Path.GetFileNameWithoutExtension(scenePath);
            return Array.IndexOf(StandaloneScenes, name) >= 0;
        }

        /// <summary>
        /// Panggil visit untuk tiap scene di Assets/Scenes. Scene yang belum terbuka dibuka additive lalu ditutup lagi.
        /// Kalau save = true, scene yang ditandai kotor oleh visit disimpan sebelum ditutup.
        /// Mengembalikan false kalau pengguna membatalkan dialog simpan perubahan.
        /// </summary>
        public static bool ForEachScene(Action<Scene, string> visit, bool save = false)
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return false;
            foreach (var path in SceneAssetPaths())
            {
                var scene = SceneManager.GetSceneByPath(path);
                bool openedHere = !scene.IsValid() || !scene.isLoaded;
                if (openedHere) scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
                try
                {
                    visit(scene, path);
                    if (save && scene.isDirty) EditorSceneManager.SaveScene(scene);
                }
                finally
                {
                    if (openedHere && SceneManager.sceneCount > 1) EditorSceneManager.CloseScene(scene, true);
                }
            }
            return true;
        }

        /// <summary>Semua komponen T di scene, termasuk yang nonaktif.</summary>
        public static List<T> ComponentsInScene<T>(Scene scene)
        {
            var list = new List<T>();
            foreach (var root in scene.GetRootGameObjects())
                list.AddRange(root.GetComponentsInChildren<T>(true));
            return list;
        }

        /// <summary>Jalur hierarki untuk laporan, mis. "IslandPhase/Region1/Pohon (12)".</summary>
        public static string PathOf(Component c)
        {
            if (c == null) return "(null)";
            var t = c.transform;
            string p = t.name;
            while (t.parent != null) { t = t.parent; p = t.name + "/" + p; }
            return c.gameObject.scene.name + "/" + p;
        }
    }
}
