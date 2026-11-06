using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Kandang vakum Lucky Machine. Blueprint: Island_Logic/LuckyMachine.md bagian 2 "Kandang" (#72).
    /// Claude.md Tahap 5, Trigger 1: OnTriggerEnter di kandang mengecek musuh tertentu sesuai kuota region mesin.
    ///
    /// Penyiapan prefab: BoxCollider trigger 6 x 4 x 4 m di layer ZoneTrigger, Rigidbody kinematic supaya
    /// OnTriggerEnter terpanggil, dan gerbang dengan NavMeshObstacle (carve). Kapsul Enemy bertabrakan dengan
    /// ZoneTrigger, sedangkan EnemyHitbox dan Ragdoll tidak, jadi satu musuh hanya memberi satu kejadian masuk.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class LuckyMachineCage : MonoBehaviour
    {
        [SerializeField] private LuckyMachine machine;

        [Tooltip("Objek gerbang (NavMeshObstacle carve). Aktif saat gerbang tertutup.")]
        [SerializeField] private GameObject gateClosedObject;

        [Tooltip("Titik tujuan vakum di dalam mesin.")]
        [SerializeField] private Transform vacuumPoint;

        public Vector3 VacuumPoint => vacuumPoint != null ? vacuumPoint.position : transform.position;

        private void Awake()
        {
            if (machine == null) machine = GetComponentInParent<LuckyMachine>();
        }

        private void OnTriggerEnter(Collider other)
        {
            // 1. Hanya tag Zombie atau Animal (PhysicsLayers.md). Tag Player diabaikan.
            if (machine == null || other == null) return;
            if (!other.CompareTag("Zombie") && !other.CompareTag("Animal")) return;

            // 2-6 diperiksa mesin, karena kuota dan gerbang milik mesin.
            var inst = other.GetComponentInParent<EnemyInstance>();
            if (inst != null) machine.TryAcceptSacrifice(inst);
        }

        /// <summary>Dipanggil LuckyMachine saat gateState berubah.</summary>
        public void SetGate(CageGateState state)
        {
            // Gerbang tertutup tidak menjebak pemain, karena kandang berbentuk lorong dua sisi.
            if (gateClosedObject != null) gateClosedObject.SetActive(state == CageGateState.Closed);
        }
    }
}
