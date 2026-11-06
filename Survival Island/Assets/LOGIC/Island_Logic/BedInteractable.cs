using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Interaksi E pada prefab Bed_Basic (struktur, 20 Mat_Wood, 100 HP). Tahan 0 detik, ditolak saat membawa Heavy Item.
    /// Blueprint: Island_Logic/SleepSystem.md bagian "Kasur sebagai struktur" (#67). IInteractable langsung
    /// (IslandInteractable #17 belum ada). Kasur boleh dibangun di luar base; syarat base dihitung saat tidur.
    /// </summary>
    public class BedInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractHighlight highlight;

        public string StructureId
        {
            get
            {
                var h = GetComponentInParent<IStructureHealth>();
                return h != null ? h.StructureId : string.Empty;
            }
        }

        public bool IsAvailable => isActiveAndEnabled && SleepSystem.Instance != null && !SleepSystem.Instance.IsSleeping;
        public float HoldSeconds => 0f;

        public string GetPrompt()
        {
            var sys = SleepSystem.Instance;
            if (sys == null) return "Tidur";
            var reason = sys.GetBlockReason();
            string text = reason == SleepBlockReason.None ? Text("prompt.sleep", "Tidur") : Text(SleepSystem.BlockMessageKey(reason), SleepSystem.BlockMessageFallback(reason));
            // GDD bagian 4 (Diary Bab 1): peringatan sebelum pemain menekan E.
            var b = GameManager.CurrentBalance;
            if (b != null && SurfaceDetection.Instance != null && SurfaceDetection.Instance.IsNearSand(transform.position, b.sleep.beachRadius))
                text += " (" + Text("prompt.sleep_beach_warning", "Terlalu dekat pantai") + ")";
            return text;
        }

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            // blockedWhileCarrying = true.
            if (ctx.IsCarryingHeavyItem) return InteractBlockReason.CarryingHeavyItem;
            var sys = SleepSystem.Instance;
            if (sys == null || sys.GetBlockReason() != SleepBlockReason.None) return InteractBlockReason.Locked;
            return InteractBlockReason.None;
        }

        public void OnHoverEnter() { if (highlight != null) highlight.SetState(HighlightState.Available); }
        public void OnHoverExit() { if (highlight != null) highlight.SetState(HighlightState.Off); }

        public bool Interact(in InteractContext ctx) => SleepSystem.Instance != null && SleepSystem.Instance.TrySleep(this);

        private static string Text(string key, string fallback)
        {
            var loc = LocalizationManager.Instance;
            if (loc == null || string.IsNullOrEmpty(key)) return fallback;
            string s = loc.Get(key);
            return string.IsNullOrEmpty(s) || s == key ? fallback : s;
        }
    }
}
