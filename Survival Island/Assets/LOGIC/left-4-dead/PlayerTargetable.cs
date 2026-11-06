using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// ITargetable milik pemain (ThreatSystem.md, #108). Dipasang di prefab pemain Fase 1 dan Fase 2, bersama SurvivalStats.
    /// ThreatWeight 1. Mendaftar ke ThreatSystem saat aktif.
    /// </summary>
    public class PlayerTargetable : MonoBehaviour, ITargetable
    {
        [SerializeField] private Transform aimPoint;     // Titik dada. Kosong = posisi + 1,2 m
        [SerializeField] private float aimHeight = 1.2f;

        public static PlayerTargetable Instance { get; private set; }

        private SurvivalStats stats;

        public int TargetId => GetInstanceID();
        public TargetKind Kind => TargetKind.Player;
        public Vector3 Position => transform.position;
        public Vector3 AimPoint => aimPoint != null ? aimPoint.position : transform.position + Vector3.up * aimHeight;
        public float ThreatWeight => 1f;
        public bool IsTargetable => isActiveAndEnabled && (stats == null || stats.IsAlive);
        public IDamageable Damageable => stats;

        private void Awake()
        {
            stats = GetComponent<SurvivalStats>();
        }

        private void OnEnable()
        {
            Instance = this;
            if (ThreatSystem.Instance != null) ThreatSystem.Instance.RegisterCandidate(this);
        }

        private void Start()
        {
            // ThreatSystem bisa aktif setelah pemain, jadi daftar ulang di Start (aman dipanggil dua kali).
            if (ThreatSystem.Instance != null) ThreatSystem.Instance.RegisterCandidate(this);
        }

        private void OnDisable()
        {
            if (ThreatSystem.Instance != null) ThreatSystem.Instance.UnregisterCandidate(this);
            if (Instance == this) Instance = null;
        }
    }
}
