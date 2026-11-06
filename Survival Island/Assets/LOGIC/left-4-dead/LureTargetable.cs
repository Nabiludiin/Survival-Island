using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Umpan Suar. Blueprint: left-4-dead/ThrowableSystem.md bagian "Efek pancingan" (#96) dan ThreatSystem.md (#108).
    /// Kind Lure, ThreatWeight 1, Damageable null. Didaftarkan sebagai kandidat ThreatSystem saat Suar menyala, dan dilepas
    /// saat padam. GDD bagian 7: Suar bisa memancing musuh ke kandang Lucky Machine.
    /// </summary>
    public class LureTargetable : MonoBehaviour, ITargetable
    {
        public bool IsActive { get; private set; }

        public int TargetId => GetInstanceID();
        public TargetKind Kind => TargetKind.Lure;
        public Vector3 Position => transform.position;
        public Vector3 AimPoint => transform.position;
        public float ThreatWeight => 1f;
        public bool IsTargetable => IsActive && isActiveAndEnabled;
        public IDamageable Damageable => null;

        public void Activate()
        {
            if (IsActive) return;
            IsActive = true;
            if (ThreatSystem.Instance != null) ThreatSystem.Instance.RegisterCandidate(this);
        }

        public void Deactivate()
        {
            if (!IsActive) return;
            IsActive = false;
            // Musuh memilih target secara normal lagi.
            if (ThreatSystem.Instance != null) ThreatSystem.Instance.UnregisterCandidate(this);
        }

        private void OnDisable() => Deactivate();
    }
}
