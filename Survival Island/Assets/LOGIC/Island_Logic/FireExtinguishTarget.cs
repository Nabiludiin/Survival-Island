using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Memadamkan api dengan tahan E dan 1 air. Ada di FX_FireBurn dan FX_FireArea, pada collider kecil layer Interactable.
    /// Blueprint: Island_Logic/FireSystem.md bagian "Memadamkan" (#97). Memakai IInteractable langsung, karena api tidak
    /// punya SaveId sendiri. Usulan 1 disetujui developer: tahan 1 detik, air kotor dipakai lebih dulu.
    /// </summary>
    public class FireExtinguishTarget : MonoBehaviour, IInteractable
    {
        public const string DirtyWaterId = "Drink_DirtyWater";
        public const string CleanWaterId = "Drink_CleanWater";

        [SerializeField] private BurnEffect burnEffect;
        [SerializeField] private FireArea fireArea;
        [SerializeField] private InteractHighlight highlight;

        private void Awake()
        {
            if (burnEffect == null) burnEffect = GetComponentInParent<BurnEffect>();
            if (fireArea == null) fireArea = GetComponentInParent<FireArea>();
        }

        public bool IsAvailable => isActiveAndEnabled && (burnEffect != null ? burnEffect.Owner != null : fireArea != null && fireArea.IsActive);

        public float HoldSeconds
        {
            get
            {
                var b = GameManager.CurrentBalance;
                return b != null ? b.fire.extinguishHoldSeconds : 1f;
            }
        }

        public string GetPrompt() => Text("prompt.fire_extinguish", "Padamkan (1 air)");

        public InteractBlockReason CanInteract(in InteractContext ctx) => InteractBlockReason.None;

        public void OnHoverEnter() { if (highlight != null) highlight.SetState(HighlightState.Available); }
        public void OnHoverExit() { if (highlight != null) highlight.SetState(HighlightState.Off); }

        public bool Interact(in InteractContext ctx)
        {
            var inv = ctx.Inventory != null ? ctx.Inventory : InventoryManager.Instance;
            if (inv == null) return false;
            // Air kotor lebih dulu, lalu air bersih.
            if (inv.HasItem(DirtyWaterId, 1)) inv.RemoveItem(DirtyWaterId, 1);
            else if (inv.HasItem(CleanWaterId, 1)) inv.RemoveItem(CleanWaterId, 1);
            else
            {
                if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.fire_need_water", NotificationKind.Warning);   // "Butuh air"
                return false;
            }
            if (burnEffect != null) burnEffect.ExtinguishOwner();
            else if (fireArea != null) fireArea.Release();
            return true;
        }

        private static string Text(string key, string fallback)
        {
            var loc = LocalizationManager.Instance;
            if (loc == null) return fallback;
            string s = loc.Get(key);
            return string.IsNullOrEmpty(s) || s == key ? fallback : s;
        }
    }
}
