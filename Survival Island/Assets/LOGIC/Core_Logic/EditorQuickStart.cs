#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SurvivalIsland
{
    /// <summary>
    /// Alat Editor untuk Play langsung dari scene selain Bootstrap. Membuat PersistentRoot, menjalankan Initialize,
    /// lalu memulai run tes tanpa menu. Tidak ikut di build rilis.
    /// Blueprint: Core_Logic/RunLifecycle.md bagian "Tes dari scene lain di Editor" (#82).
    ///
    /// Prefab dicari lewat AssetDatabase dengan nama "PersistentRoot", jadi lokasi prefab bebas.
    /// </summary>
    public static class EditorQuickStart
    {
        private const string PersistentRootPrefabName = "PersistentRoot";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == RunLifecycle.BootstrapScene || RunLifecycle.Instance != null) return;

            var prefab = FindPersistentRootPrefab();
            if (prefab == null)
            {
                Debug.LogWarning($"[EditorQuickStart] Prefab '{PersistentRootPrefabName}' tidak ditemukan. Scene berjalan tanpa manager persisten.");
                return;
            }

            var root = Object.Instantiate(prefab);
            root.name = prefab.name;

            if (RunLifecycle.Instance == null)
            {
                Debug.LogError("[EditorQuickStart] Prefab PersistentRoot tidak memuat RunLifecycle.");
                return;
            }
            RunLifecycle.Instance.BeginEditorQuickStart(sceneName);
        }

        private static GameObject FindPersistentRootPrefab()
        {
            var guids = AssetDatabase.FindAssets(PersistentRootPrefabName + " t:Prefab");
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (go != null && go.name == PersistentRootPrefabName) return go;
            }
            return null;
        }
    }
}
#endif
