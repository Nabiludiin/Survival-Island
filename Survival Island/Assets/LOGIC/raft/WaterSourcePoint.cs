using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Titik air kotor di tepi sungai dan danau. Tahan E memberi satu Drink_DirtyWater. Tidak habis dan tidak disimpan.
    /// Blueprint: raft/HydrationSystem.md bagian "Mengambil air kotor" (#43).
    /// GDD bagian 3: air kotor berisiko sakit (peluang dihitung DiseaseManager #45 lewat ItemData.poisonChance).
    /// Usulan disetujui #43: air laut tidak bisa diminum, jadi titik ini tidak diletakkan di laut.
    ///
    /// Diletakkan di layer Interactable, supaya layer Water tetap trigger dan tidak masuk mask Interact.
    /// IslandInteractable (#17) belum ada, jadi kelas ini menerapkan IInteractable langsung.
    /// </summary>
    public class WaterSourcePoint : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemData dirtyWaterItem;      // Drink_DirtyWater
        [SerializeField] private InteractHighlight highlight;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip scoopClip;

        public bool IsAvailable => true;
        public float HoldSeconds
        {
            get
            {
                var b = GameManager.CurrentBalance;
                return b != null ? b.hydration.dirtyWaterHoldSeconds : 1f;
            }
        }

        public string GetPrompt() { var loc = LocalizationManager.Instance; return loc != null && loc.Has("prompt.water_dirty") ? loc.Get("prompt.water_dirty") : "Ambil air kotor"; }   // #104

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            // blockedWhileCarrying = true.
            return ctx.IsCarryingHeavyItem ? InteractBlockReason.CarryingHeavyItem : InteractBlockReason.None;
        }

        public void OnHoverEnter() { if (highlight != null) highlight.SetState(HighlightState.Available); }
        public void OnHoverExit() { if (highlight != null) highlight.SetState(HighlightState.Off); }

        public bool Interact(in InteractContext ctx)
        {
            if (ctx.Inventory == null || dirtyWaterItem == null) return false;
            // 1-2.
            int left = ctx.Inventory.AddItem(dirtyWaterItem, 1);
            if (left > 0)
            {
                if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.inventory_full", NotificationKind.Warning);
                return false;
            }
            // 3.
            if (audioSource != null && scoopClip != null) audioSource.PlayOneShot(scoopClip);
            return true;
        }
    }
}
