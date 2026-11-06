using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Akar tunggal semua objek di satu region scene (Region1_Volcano, Region2_Amazon, Region3_Snow, Region4_Village).
    /// Blueprint: Core_Logic/RegionStreaming.md bagian "Aturan region scene" dan "Pengisian" (#88).
    /// Daftar ISaveable dikumpulkan sekali saat region masuk (GetComponentsInChildren), lalu dipakai CaptureRegion.
    /// Awake dan OnEnable hanya mendaftar, tanpa pencarian berat ("Hitch aktivasi").
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class RegionSceneRoot : MonoBehaviour
    {
        private static readonly RegionSceneRoot[] byRegion = new RegionSceneRoot[4];

        [SerializeField] private RegionType region = RegionType.Volcano;

        private readonly List<ISaveable> saveables = new List<ISaveable>(128);
        private bool collected;

        public RegionType Region => region;
        public IReadOnlyList<ISaveable> Saveables => saveables;

        /// <summary>Akar region yang sedang dimuat, atau null.</summary>
        public static RegionSceneRoot Get(RegionType r) => byRegion[(int)r];

        /// <summary>Akar region terdekat di atas transform ini, dipakai objek region untuk membaca homeRegion.</summary>
        public static bool TryGetRegionOf(Component c, out RegionType r)
        {
            r = RegionType.Volcano;
            if (c == null) return false;
            var root = c.GetComponentInParent<RegionSceneRoot>(true);
            if (root == null) return false;
            r = root.region;
            return true;
        }

        private void OnEnable()
        {
            if (byRegion[(int)region] != null && byRegion[(int)region] != this)
                Debug.LogError($"[RegionSceneRoot] Akar ganda untuk {region}.", this);
            byRegion[(int)region] = this;
        }

        private void OnDisable()
        {
            if (byRegion[(int)region] == this) byRegion[(int)region] = null;
        }

        /// <summary>Kumpulkan ISaveable sekali (termasuk objek nonaktif, misalnya tong yang sudah meledak).</summary>
        public void CollectSaveables()
        {
            if (collected) return;
            collected = true;
            saveables.Clear();
            var found = GetComponentsInChildren<ISaveable>(true);
            for (int i = 0; i < found.Length; i++)
            {
                if (found[i] == null || string.IsNullOrEmpty(found[i].SaveId)) continue;
                saveables.Add(found[i]);
            }
        }
    }
}
