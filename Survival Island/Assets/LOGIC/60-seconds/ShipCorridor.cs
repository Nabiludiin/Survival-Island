using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>
    /// Koridor kapal yang bisa runtuh. Blueprint: 60-seconds/ShipHazardManager.md bagian "Koridor runtuh" (#75).
    /// Usulan disetujui (#85): dua titik runtuh, tanda 3 detik, tanpa damage langsung.
    /// Belum diimplementasikan: suara besi melengkung (AudioManager) dan CameraShake.Play(ShipCollapse) (#24).
    /// </summary>
    public class ShipCorridor : MonoBehaviour
    {
        [SerializeField] private string corridorId;
        [Tooltip("Puing, nonaktif sebelum runtuh.")]
        [SerializeField] private GameObject debrisRoot;
        [Tooltip("NavMeshObstacle dengan carving = true, nonaktif sebelum runtuh.")]
        [SerializeField] private NavMeshObstacle navObstacle;
        [Tooltip("Debu, percikan, dan garis merah di lantai.")]
        [SerializeField] private GameObject warningVfx;
        [SerializeField] private Transform exitPoint;
        [Tooltip("Area koridor dalam koordinat dunia.")]
        [SerializeField] private Bounds blockVolume = new Bounds(Vector3.zero, Vector3.one * 2f);
        [Header("Suara (AudioManager)")]
        [Tooltip("Suara besi berderit saat tanda sebelum runtuh.")]
        [SerializeField] private AudioClip warnClip;
        [SerializeField] private AudioClip collapseClip;

        public string CorridorId => corridorId;
        public Bounds BlockVolume => blockVolume;
        public bool IsCollapsed { get; private set; }

        private void Awake()
        {
            if (debrisRoot != null) debrisRoot.SetActive(false);
            if (navObstacle != null) navObstacle.enabled = false;
            if (warningVfx != null) warningVfx.SetActive(false);
        }

        /// <summary>Tanda sebelum runtuh: debu, garis merah, dan suara besi lewat AudioManager.</summary>
        public void Warn()
        {
            if (IsCollapsed) return;
            if (warningVfx != null) warningVfx.SetActive(true);
            AudioManager.Instance?.PlaySFX(warnClip);
        }

        /// <summary>
        /// Runtuh. Pemain di dalam blockVolume dipindah ke exitPoint lewat controller.Move, tanpa damage.
        /// Zombie di dalamnya dikembalikan ke pool oleh ShipHazardManager.
        /// </summary>
        public void Collapse(CharacterController player)
        {
            if (IsCollapsed) return;
            IsCollapsed = true;

            if (player != null && player.enabled && exitPoint != null && blockVolume.Contains(player.transform.position))
                player.Move(exitPoint.position - player.transform.position);

            if (warningVfx != null) warningVfx.SetActive(false);
            if (debrisRoot != null) debrisRoot.SetActive(true);
            if (navObstacle != null) navObstacle.enabled = true;
            // Getaran runtuh (#24) dan suara runtuh.
            CameraShake.Play(CameraShakeKind.ShipCollapse);
            AudioManager.Instance?.PlaySFX(collapseClip);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.3f, 0.2f, 0.4f);
            Gizmos.DrawCube(blockVolume.center, blockVolume.size);
        }
    }
}
