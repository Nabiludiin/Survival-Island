using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Gerbang tua di seam S2 (scene IslandPhase) yang memberi alasan terkunci. Objeknya dimatikan RegionGateSystem
    /// lewat GateRule.barriers saat S2 terbuka. Blueprint: Island_Logic/RegionGateSystem.md (#68).
    /// </summary>
    public class GateBlocker : MonoBehaviour, IInteractable
    {
        public bool IsAvailable => isActiveAndEnabled;
        public float HoldSeconds => 0f;
        public string GetPrompt() => "Terkunci. Cari petunjuk di Diary";
        public InteractBlockReason CanInteract(in InteractContext ctx) => InteractBlockReason.Locked;
        public void OnHoverEnter() { }
        public void OnHoverExit() { }
        public bool Interact(in InteractContext ctx) => false;
    }
}
