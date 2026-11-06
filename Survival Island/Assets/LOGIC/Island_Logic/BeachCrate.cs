using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu kargo pantai (pool Supply_BeachCrate, layer Interactable). Tahan E beachSupply.openHoldSeconds untuk membuka.
    /// Blueprint: Island_Logic/BeachSupplyManager.md bagian "Membuka kargo" (#71, #17). Status isOpened dipegang
    /// BeachSupplyManager, jadi kargo ini tidak menyimpan apa pun sendiri. IInteractable langsung (IslandInteractable #17 belum ada).
    /// </summary>
    public class BeachCrate : MonoBehaviour, IInteractable, IPoolable
    {
        [SerializeField] private NoiseEmitter noiseEmitter;
        [SerializeField] private InteractHighlight highlight;
        [Tooltip("Tutup kargo. Diputar saat terbuka (tanpa Animator).")]
        [SerializeField] private Transform lid;
        [SerializeField] private Vector3 lidOpenEuler = new Vector3(-110f, 0f, 0f);
        [SerializeField] private Animator animator;
        [SerializeField] private string openTrigger = "Open";

        private BeachSupplyManager owner;
        private int index = -1;
        private Quaternion lidClosed;
        private bool lidCached;

        public int Index => index;

        internal void Bind(BeachSupplyManager manager, int crateIndex, bool opened)
        {
            owner = manager;
            index = crateIndex;
            ApplyOpened(opened, false);
        }

        internal void ApplyOpened(bool opened, bool animate)
        {
            if (lid != null)
            {
                if (!lidCached) { lidClosed = lid.localRotation; lidCached = true; }
                lid.localRotation = opened ? lidClosed * Quaternion.Euler(lidOpenEuler) : lidClosed;
            }
            if (animate && animator != null && !string.IsNullOrEmpty(openTrigger)) animator.SetTrigger(openTrigger);
        }

        public bool IsAvailable => isActiveAndEnabled && owner != null && index >= 0 && !owner.IsCrateOpened(index);

        public float HoldSeconds
        {
            get
            {
                var b = GameManager.CurrentBalance;
                return b != null ? b.beachSupply.openHoldSeconds : 1.5f;
            }
        }

        public string GetPrompt() => "Buka Kargo";   // TODO(LocalizationManager #104): kunci teks
        public InteractBlockReason CanInteract(in InteractContext ctx) => ctx.IsCarryingHeavyItem ? InteractBlockReason.CarryingHeavyItem : InteractBlockReason.None;
        public void OnHoverEnter() { if (highlight != null) highlight.SetState(HighlightState.Available); }
        public void OnHoverExit() { if (highlight != null) highlight.SetState(HighlightState.Off); }

        public bool Interact(in InteractContext ctx)
        {
            if (!IsAvailable) return false;
            if (!owner.OpenCrate(index, transform.position)) return false;
            ApplyOpened(true, true);
            // Suara saat dibuka memanggil zombie (#49).
            var b = GameManager.CurrentBalance;
            float r = b != null ? b.beachSupply.openNoiseRadius : 10f;
            if (noiseEmitter != null) noiseEmitter.Emit(r, NoiseSourceKind.Container);
            else NoiseEmitter.Emit(transform.position, r, NoiseSourceKind.Container);
            return true;
        }

        public void OnSpawned()
        {
            owner = null;
            index = -1;
            ApplyOpened(false, false);
        }

        public void OnReturned()
        {
            owner = null;
            index = -1;
            if (highlight != null) highlight.SetState(HighlightState.Off);
        }
    }
}
