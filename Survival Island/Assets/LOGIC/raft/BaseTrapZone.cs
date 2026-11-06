using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Anak TriggerZone jebakan (layer ZoneTrigger, Rigidbody kinematic, BoxCollider trigger). Meneruskan OnTriggerEnter dan
    /// OnTriggerExit kapsul Enemy ke BaseTrap di root. Blueprint: raft/BaseDefense.md bagian "Susunan prefab" (#26).
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class BaseTrapZone : MonoBehaviour
    {
        [SerializeField] private BaseTrap trap;

        private void Awake()
        {
            if (trap == null) trap = GetComponentInParent<BaseTrap>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (trap != null) trap.HandleEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (trap != null) trap.HandleExit(other);
        }
    }
}
