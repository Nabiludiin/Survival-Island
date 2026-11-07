using System.Collections.Generic;
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

        /// <summary>Kandang yang aktif di scene. Dibaca IsInsideAnyCage tanpa query fisika.</summary>
        private static readonly List<LuckyMachineCage> active = new List<LuckyMachineCage>(4);
        private Collider area;

        private void Awake()
        {
            if (machine == null) machine = GetComponentInParent<LuckyMachine>();
            area = GetComponent<Collider>();
        }

        private void OnEnable() { if (!active.Contains(this)) active.Add(this); }
        private void OnDisable() { active.Remove(this); }

        /// <summary>
        /// true kalau posisi berada di dalam trigger salah satu kandang. Dipakai SpecialZombieAI.TickLargeStuck: musuh boleh
        /// diam di dalam kandang, jadi pengaman macet dilewati di sana (SpecialZombieAI.md, #72).
        /// </summary>
        public static bool IsInsideAnyCage(Vector3 position)
        {
            for (int i = 0; i < active.Count; i++)
            {
                var c = active[i];
                if (c == null || c.area == null || !c.area.enabled) continue;
                if (c.area.bounds.Contains(position) && (c.area.ClosestPoint(position) - position).sqrMagnitude < 0.0001f) return true;
            }
            return false;
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
