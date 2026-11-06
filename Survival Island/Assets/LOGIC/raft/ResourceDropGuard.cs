using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Penjaga item jatuh dari node, dipasang di prefab WorldItem_Generic. Mati kalau tidak di-Arm.
    /// Blueprint: raft/ResourceRespawn.md "Item jatuh" (#73). Usulan disetujui: item yang masuk air kembali ke pangkal node.
    /// </summary>
    public class ResourceDropGuard : MonoBehaviour, IPoolable
    {
        private Vector3 safePoint;
        private float settleTimer;
        private bool armed;
        private Rigidbody body;
        private int waterLayer = -1;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            waterLayer = LayerMask.NameToLayer(PhysicsMasks.WaterLayer);
            enabled = false;
        }

        /// <summary>Dipanggil ResourceNode.SpawnDrops. safePoint berada di pangkal node (selalu di darat).</summary>
        public void Arm(Vector3 point)
        {
            safePoint = point;
            settleTimer = Balance != null ? Balance.resource.dropSettleSeconds : 3f;
            armed = true;
            enabled = true;
        }

        private void Update()
        {
            if (!armed) { enabled = false; return; }
            settleTimer -= Time.deltaTime;
            if (settleTimer > 0f) return;

            // Tertimbun atau menembus Terrain: dikembalikan tanpa query fisika. Pemeriksaan hanya sekali.
            float maxFall = Balance != null ? Balance.resource.dropMaxFallMeters : 4f;
            if (transform.position.y < safePoint.y - maxFall) ReturnToSafePoint();
            Disarm();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!armed || other.gameObject.layer != waterLayer) return;
            ReturnToSafePoint();
        }

        private void ReturnToSafePoint()
        {
            transform.position = safePoint;
            if (body != null && !body.isKinematic)
            {
                body.linearVelocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
        }

        private void Disarm()
        {
            armed = false;
            settleTimer = 0f;
            safePoint = Vector3.zero;
            enabled = false;
        }

        public void OnSpawned() => Disarm();
        public void OnReturned() => Disarm();
    }
}
