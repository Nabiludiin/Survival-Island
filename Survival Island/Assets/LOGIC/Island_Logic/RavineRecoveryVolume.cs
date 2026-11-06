using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Trigger di dasar jurang S1 (layer ZoneTrigger), 5 meter di atas dasar, sehingga damage mendarat (#101) tidak ikut.
    /// Pemain yang jatuh dikembalikan ke posisi aman terakhir dan menerima damage Fall sebesar region.ravineFallDamage.
    /// Blueprint: Island_Logic/RegionGateSystem.md bagian "Mencegah melewati jurang" lapis 5 (#68).
    /// TODO #88: posisi aman dari RegionBoundaryGuard.LastSafePosition. Sementara dari RegionGateSystem.LastSafePosition.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class RavineRecoveryVolume : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var gates = RegionGateSystem.Instance;
            var player = IslandPlayerController.Instance;
            var bal = GameManager.CurrentBalance;
            if (gates == null || player == null || !gates.HasSafePosition) return;

            player.Teleport(gates.LastSafePosition);
            var stats = SurvivalStats.Instance;
            if (stats != null && bal != null)
            {
                var info = new DamageInfo
                {
                    amount = bal.region.ravineFallDamage,
                    type = DamageType.Fall,
                    source = gameObject,
                    sourceId = "ravine",
                    hitPoint = player.transform.position,
                    hitDirection = Vector3.down,
                    bodyPart = BodyPart.Torso,
                };
                stats.TakeDamage(in info);
            }
        }
    }
}
