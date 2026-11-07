using UnityEngine;

namespace SurvivalIsland
{
    public enum ClimbKind { Ladder, Rope }

    /// <summary>
    /// Tangga atau tali yang dipanjat dengan E. Blueprint: Island_Logic/PlayerTraversal.md bagian "Tangga dan tali" (#101).
    /// Layer Interactable, collider biasa (bukan trigger).
    ///
    /// Mewarisi IslandInteractable (#17): tombol E lewat PlayerInteractor, CanInteract mengembalikan CarryingHeavyItem saat
    /// membawa Heavy Item (blockedWhileCarrying), dan OnInteract memanggil TryBeginClimb. Objek statis tanpa saveId.
    /// </summary>
    public class ClimbableSurface : IslandInteractable
    {
        public ClimbKind kind = ClimbKind.Ladder;
        public Transform bottomPoint;
        public Transform topPoint;
        public Transform exitBottomPoint;
        public Transform exitTopPoint;
        [Tooltip("Arah pemain saat memanjat. Kosong = transform.forward")]
        public Transform facing;

        public Vector3 Bottom => bottomPoint != null ? bottomPoint.position : transform.position;
        public Vector3 Top => topPoint != null ? topPoint.position : transform.position + Vector3.up;
        public Vector3 ExitBottom => exitBottomPoint != null ? exitBottomPoint.position : Bottom;
        public Vector3 ExitTop => exitTopPoint != null ? exitTopPoint.position : Top;
        public Vector3 Facing => facing != null ? facing.forward : transform.forward;
        public float Length => Mathf.Max(0.01f, Vector3.Distance(Bottom, Top));

        /// <summary>Titik terdekat di jalur panjat, 0 bawah dan 1 atas.</summary>
        public float ClosestT(Vector3 position)
        {
            Vector3 ab = Top - Bottom;
            float sq = ab.sqrMagnitude;
            return sq < 0.0001f ? 0f : Mathf.Clamp01(Vector3.Dot(position - Bottom, ab) / sq);
        }

        private void Reset()
        {
            blockedWhileCarrying = true;   // PlayerTraversal.md: CanInteract = CarryingHeavyItem saat membawa Heavy Item
        }

        /// <summary>Tidak tersedia saat pemain sudah memanjat permukaan ini.</summary>
        public override bool IsAvailable => base.IsAvailable && !IsActiveClimb();

        /// <summary>PlayerTraversal.md: BeginClimb dipanggil ClimbableSurface.OnInteract.</summary>
        protected override bool OnInteract(in InteractContext ctx)
        {
            var traversal = FindTraversal(ctx.Player);
            return TryBeginClimb(traversal);
        }

        private static PlayerTraversal FindTraversal(Transform player)
        {
            if (player != null)
            {
                var t = player.GetComponentInParent<PlayerTraversal>();
                if (t != null) return t;
            }
            var pc = IslandPlayerController.Instance;
            return pc != null ? pc.GetComponent<PlayerTraversal>() : null;
        }

        private bool IsActiveClimb()
        {
            var pc = IslandPlayerController.Instance;
            var t = pc != null ? pc.GetComponent<PlayerTraversal>() : null;
            return t != null && t.ActiveClimb == this;
        }

        /// <summary>Ditolak saat membawa Heavy Item ("Tangan penuh", PlayerTraversal.md).</summary>
        public bool TryBeginClimb(PlayerTraversal traversal)
        {
            if (traversal == null) return false;
            bool heavy = EventChannel<HeavyItemChangedEvent>.HasLast && EventChannel<HeavyItemChangedEvent>.Last.isCarried;
            if (heavy)
            {
                // Notifikasi "Tangan penuh" (#22).
                if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.hands_full", NotificationKind.Warning);
                return false;
            }
            return traversal.BeginClimb(this);
        }
    }
}
