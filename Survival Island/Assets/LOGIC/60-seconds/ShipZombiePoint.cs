using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Titik muncul zombie kapal untuk satu grup jadwal. Blueprint: 60-seconds/ShipHazardManager.md bagian "Zombie di kapal" (#75).
    /// groupId sama dengan targetId entri SpawnZombies di ShipHazardSchedule.
    /// </summary>
    public class ShipZombiePoint : MonoBehaviour
    {
        [SerializeField] private string groupId;

        public string GroupId => groupId;

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.4f, 0.8f, 0.3f, 0.8f);
            Gizmos.DrawWireSphere(transform.position, 0.4f);
        }
    }
}
