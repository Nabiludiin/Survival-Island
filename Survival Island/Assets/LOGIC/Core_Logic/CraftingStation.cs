using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Meja crafting (Build_WoodBench, Build_MedicalBench). Mendaftar sebagai stasiun aktif, dan membuka CraftingPanel lewat E.
    /// Blueprint: Core_Logic/CraftingStation.md (#77). GDD bagian 4: meja crafting berada di base.
    ///
    /// Mewarisi IslandInteractable (#17): highlight, penolakan CarryingHeavyItem (blockedWhileCarrying), dan prompt dari
    /// promptKey diatur kelas dasar. saveId dibiarkan kosong, karena meja adalah struktur yang disimpan StructureRegistry.
    /// Collider interaksi berada di anak layer Interactable yang sedikit menonjol (BuildMenu.md "Layer pada prefab struktur").
    /// </summary>
    public class CraftingStation : IslandInteractable
    {
        private const string DefaultPromptKey = "prompt.crafting_station";   // Kunci teks #104: "Buka meja crafting"

        [SerializeField] private CraftingStationType stationType = CraftingStationType.WoodBench;
        [SerializeField] private Transform centerPoint;

        public CraftingStationType StationType => stationType;
        public Vector3 Center => centerPoint != null ? centerPoint.position : transform.position + Vector3.up * 0.8f;

        private void OnEnable()
        {
            if (CraftingManager.Instance != null) CraftingManager.Instance.RegisterStation(this);
        }

        private void Start()
        {
            // Meja yang ada di scene sebelum manager Awake didaftarkan di sini.
            if (CraftingManager.Instance != null) CraftingManager.Instance.RegisterStation(this);
        }

        private void OnDisable()
        {
            // Meja hancur: job terhenti sampai ada meja sejenis (CraftingStation.md "Kasus Khusus").
            if (CraftingManager.Instance != null) CraftingManager.Instance.UnregisterStation(this);
        }

        // ---------------------------------------------------------------- IslandInteractable

        private void Reset()
        {
            promptKey = DefaultPromptKey;
            blockedWhileCarrying = true;   // Membawa Heavy Item: E terhalang. Tombol C tetap membuka panel.
        }

        public override string GetPrompt() => Localize(string.IsNullOrEmpty(promptKey) ? DefaultPromptKey : promptKey);

        protected override bool OnInteract(in InteractContext ctx)
        {
            var panel = CraftingPanel.Instance;
            if (panel == null || UIFocusStack.Instance == null) return false;
            return UIFocusStack.Instance.Push(panel);
        }
    }
}
