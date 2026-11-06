using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Target markas di pusat base. Blueprint: raft/BaseNavigation.md bagian "BaseAnchor" (#78, #108, #50).
    /// GDD bagian 3: saat Blood Moon horde menyerbu base. Dibuat BaseNavigation sekali, posisinya diperbarui saat base berubah.
    /// Kind BaseAnchor, Damageable null, ThreatWeight 1. Didaftarkan ke ThreatSystem sebagai kandidat; IsTargetable hanya
    /// true saat Blood Moon, base ada, region dimuat, dan pemain dalam bloodMoon.baseEngageDistance.
    /// </summary>
    public class BaseAnchor : MonoBehaviour, ITargetable
    {
        private bool registered;

        public int TargetId => GetInstanceID();
        public TargetKind Kind => TargetKind.BaseAnchor;
        public Vector3 Position => transform.position;
        public Vector3 AimPoint => transform.position + Vector3.up;
        public float ThreatWeight => 1f;
        public IDamageable Damageable => null;

        public bool IsTargetable
        {
            get
            {
                // 1. Blood Moon sedang berjalan.
                if (HordeManager.Instance == null || !HordeManager.Instance.IsActive) return false;
                // 2-4. Base ada, region dimuat, pemain dekat (tanpa base, zombie memburu pemain, ThreatSystem.md).
                var nav = BaseNavigation.Instance;
                return nav != null && nav.IsAnchorEngaged(out _);
            }
        }

        private void Update()
        {
            // ThreatSystem bisa aktif setelah BaseNavigation, jadi pendaftaran dicoba sampai berhasil.
            if (registered || ThreatSystem.Instance == null) return;
            ThreatSystem.Instance.RegisterCandidate(this);
            registered = true;
            enabled = false;   // Tetap terdaftar sampai OnDestroy (IsTargetable menyaring)
        }

        private void OnDestroy()
        {
            if (registered && ThreatSystem.Instance != null) ThreatSystem.Instance.UnregisterCandidate(this);
            registered = false;
        }
    }
}
