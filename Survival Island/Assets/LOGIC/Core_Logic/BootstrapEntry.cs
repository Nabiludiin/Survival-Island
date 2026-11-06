using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Komponen scene Bootstrap. Membuat PersistentRoot hanya kalau RunLifecycle.Instance masih null,
    /// jadi Bootstrap aman dimuat ulang sebagai scene transit kosong saat reset.
    /// Blueprint: Core_Logic/RunLifecycle.md bagian "Susunan scene Bootstrap" (#82).
    /// </summary>
    public sealed class BootstrapEntry : MonoBehaviour
    {
        [Tooltip("Prefab PersistentRoot (memuat RunLifecycle dan anak Managers).")]
        [SerializeField] private GameObject persistentRootPrefab;

        private void Awake()
        {
            if (RunLifecycle.Instance != null) return;

            if (persistentRootPrefab == null)
            {
                Debug.LogError("[BootstrapEntry] persistentRootPrefab kosong.");
                return;
            }
            var root = Instantiate(persistentRootPrefab);
            root.name = persistentRootPrefab.name;
        }
    }
}
