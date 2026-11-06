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

        /// <summary>Prompt sesuai state (tabel di blueprint). Teks lewat LocalizationManager (#104), teks bawaan kalau kunci belum ada.</summary>
        public string GetPrompt()
        {
            var m = RescueEventManager.Instance;
            if (m == null) return string.Empty;
            switch (m.State)
            {
                case RescueState.Assembling:
                {
                    var loc = LocalizationManager.Instance;
                    if (loc != null && loc.Has("prompt.cockpit_parts"))
                        return loc.Format("prompt.cockpit_parts", "installed", loc.FormatNumber(m.InstalledCount), "total", loc.FormatNumber(m.TotalParts));
                    return $"Suku cadang kurang ({m.InstalledCount} / {m.TotalParts})";
                }
                case RescueState.Ready: return Text("prompt.cockpit_start", "Nyalakan mesin");
                case RescueState.WarmingUp:
                    int s = Mathf.CeilToInt(m.WarmupLeft);
                    string clock = $"{s / 60}:{s % 60:00}";
                    var l = LocalizationManager.Instance;
                    return l != null && l.Has("prompt.cockpit_warmup") ? l.Format("prompt.cockpit_warmup", "time", clock) : "Mesin memanas: " + clock;
                case RescueState.ReadyToLift: return Text("prompt.cockpit_liftoff", "Lepas landas");
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

        /// <summary>Teks dari LocalizationManager (#104); teks bawaan kalau kunci belum ada di tabel.</summary>
        private static string Text(string key, string fallback)
        {
            var loc = LocalizationManager.Instance;
            return loc != null && loc.Has(key) ? loc.Get(key) : fallback;
        }
    }
}
