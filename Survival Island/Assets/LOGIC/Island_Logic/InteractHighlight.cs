using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Status outline target interaksi (IslandInteractable.md, "Prompt dan highlight").</summary>
    public enum HighlightState { Off, Available, Blocked }

    /// <summary>
    /// Outline bersama IslandInteractable dan ShipInteractable. Blueprint: Island_Logic/IslandInteractable.md (#17).
    /// Warna dibaca dari GameBalanceConfig.interact. Outline memakai MaterialPropertyBlock pada renderer outline
    /// yang sudah ada di prefab (tidak menambah material ke array), jadi hover tidak mengalokasi.
    ///
    /// Belum diimplementasikan: penanda selain warna untuk aksesibilitas #104 (outline putus-putus, ikon gembok).
    /// Penanda itu bergantung pada shader outline dan HUD yang belum ada.
    /// </summary>
    public class InteractHighlight : MonoBehaviour
    {
        private static readonly int ColorId = Shader.PropertyToID("_BaseColor");
        private static readonly InteractBalance FallbackInteract = new InteractBalance();

        [Tooltip("Renderer khusus outline (mis. mesh hull terbalik). Dimatikan saat Off.")]
        [SerializeField] private Renderer outlineRenderer;

        private MaterialPropertyBlock block;

        public HighlightState State { get; private set; } = HighlightState.Off;

        private void Awake()
        {
            block = new MaterialPropertyBlock();
            ApplyState();
        }

        public void SetState(HighlightState state)
        {
            if (state == State) return;
            State = state;
            ApplyState();
        }

        private void ApplyState()
        {
            if (outlineRenderer == null) return;
            if (State == HighlightState.Off)
            {
                outlineRenderer.enabled = false;
                return;
            }

            var balance = GameManager.CurrentBalance;
            var interact = balance != null ? balance.interact : FallbackInteract;
            if (block == null) block = new MaterialPropertyBlock();
            outlineRenderer.GetPropertyBlock(block);
            block.SetColor(ColorId, State == HighlightState.Available ? interact.availableColor : interact.blockedColor);
            outlineRenderer.SetPropertyBlock(block);
            outlineRenderer.enabled = true;
        }
    }
}
