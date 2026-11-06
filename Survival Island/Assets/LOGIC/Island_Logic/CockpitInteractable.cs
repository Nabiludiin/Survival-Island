using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Kokpit helikopter: menyalakan mesin dan lepas landas dengan E. Blueprint: Island_Logic/RescueEventManager.md
    /// bagian 2 "Urutan lepas landas" (#14). GDD bagian 3: menang saat helikopter lepas landas.
    /// IslandInteractable (#17) belum ditulis, jadi kelas ini menerapkan IInteractable langsung (pola CraftingStation).
    /// </summary>
    public class CockpitInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractHighlight highlight;

        public bool IsAvailable => isActiveAndEnabled && RescueEventManager.Instance != null && !RescueEventManager.Instance.IsDeparted;
        public float HoldSeconds => 0f;

        /// <summary>Prompt sesuai state (tabel di blueprint). TODO #104: teks lewat LocalizationManager.</summary>
        public string GetPrompt()
        {
            var m = RescueEventManager.Instance;
            if (m == null) return string.Empty;
            switch (m.State)
            {
                case RescueState.Assembling: return $"Suku cadang kurang ({m.InstalledCount} / {m.TotalParts})";
                case RescueState.Ready: return "Nyalakan mesin";
                case RescueState.WarmingUp:
                    int s = Mathf.CeilToInt(m.WarmupLeft);
                    return $"Mesin memanas: {s / 60}:{s % 60:00}";
                case RescueState.ReadyToLift: return "Lepas landas";
                default: return string.Empty;
            }
        }

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            var m = RescueEventManager.Instance;
            if (m == null) return InteractBlockReason.Locked;
            if (ctx.IsCarryingHeavyItem) return InteractBlockReason.CarryingHeavyItem;
            return m.State == RescueState.Ready || m.State == RescueState.ReadyToLift ? InteractBlockReason.None : InteractBlockReason.Locked;
        }

        public void OnHoverEnter() { if (highlight != null) highlight.SetState(HighlightState.Available); }
        public void OnHoverExit() { if (highlight != null) highlight.SetState(HighlightState.Off); }

        public bool Interact(in InteractContext ctx)
        {
            var m = RescueEventManager.Instance;
            if (m == null) return false;
            if (m.State == RescueState.Ready) return m.StartEngine();
            if (m.State == RescueState.ReadyToLift) return m.LiftOff();
            return false;
        }
    }
}
