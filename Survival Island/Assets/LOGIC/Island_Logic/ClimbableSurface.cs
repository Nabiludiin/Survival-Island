using UnityEngine;

namespace SurvivalIsland
{
    public enum ClimbKind { Ladder, Rope }

    /// <summary>
    /// Tangga atau tali yang dipanjat dengan E. Blueprint: Island_Logic/PlayerTraversal.md bagian "Tangga dan tali" (#101).
    /// Layer Interactable, collider biasa (bukan trigger).
    ///
    /// Catatan implementasi (Langkah 3): blueprint menulis ClimbableSurface : IslandInteractable (#17). Kelas dasar itu belum
    /// ada di branch ini, jadi alur interaksi ditulis di TryBeginClimb. TODO #17: ganti basis ke IslandInteractable,
    /// CanInteract mengembalikan CarryingHeavyItem saat membawa Heavy Item, dan OnInteract memanggil TryBeginClimb.
    /// </summary>
    public class ClimbableSurface : MonoBehaviour
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

        /// <summary>Ditolak saat membawa Heavy Item ("Tangan penuh", PlayerTraversal.md).</summary>
        public bool TryBeginClimb(PlayerTraversal traversal)
        {
            if (traversal == null) return false;
            bool heavy = EventChannel<HeavyItemChangedEvent>.HasLast && EventChannel<HeavyItemChangedEvent>.Last.isCarried;
            if (heavy) return false;   // TODO #22: notifikasi "Tangan penuh"
            return traversal.BeginClimb(this);
        }
    }
}
