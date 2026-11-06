using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Meja crafting (Build_WoodBench, Build_MedicalBench). Mendaftar sebagai stasiun aktif, dan membuka CraftingPanel lewat E.
    /// Blueprint: Core_Logic/CraftingStation.md (#77). GDD bagian 4: meja crafting berada di base.
    ///
    /// Catatan implementasi (Langkah 4): blueprint menulis CraftingStation : IslandInteractable (#17). Kelas dasar itu
    /// belum ada, jadi kelas ini menerapkan IInteractable (Langkah 2) langsung. TODO #17: ganti basis ke IslandInteractable.
    /// Collider interaksi berada di anak layer Interactable yang sedikit menonjol (BuildMenu.md "Layer pada prefab struktur").
    /// </summary>
    public class CraftingStation : MonoBehaviour, IInteractable
    {
        [SerializeField] private CraftingStationType stationType = CraftingStationType.WoodBench;
        [SerializeField] private Transform centerPoint;
        [SerializeField] private string promptKey = "prompt.crafting_station";   // Kunci teks #104: "Buka meja crafting"
        [SerializeField] private InteractHighlight highlight;

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

        // ---------------------------------------------------------------- IInteractable

        public bool IsAvailable => isActiveAndEnabled;
        public float HoldSeconds => 0f;
        public string GetPrompt() => UIManager.Localize(promptKey);

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            // Membawa Heavy Item: E terhalang. Tombol C tetap membuka panel.
            return ctx.IsCarryingHeavyItem ? InteractBlockReason.CarryingHeavyItem : InteractBlockReason.None;
        }

        public void OnHoverEnter() { if (highlight != null) highlight.SetState(HighlightState.Available); }
        public void OnHoverExit() { if (highlight != null) highlight.SetState(HighlightState.Off); }

        public bool Interact(in InteractContext ctx)
        {
            var panel = CraftingPanel.Instance;
            if (panel == null || UIFocusStack.Instance == null) return false;
            return UIFocusStack.Instance.Push(panel);
        }
    }
}
