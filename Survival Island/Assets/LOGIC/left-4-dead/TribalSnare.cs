using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Jerat Zombie Tribal, objek pool di layer Projectile (SpecialZombieAI.md, #25). Bergerak lurus tanpa fisika;
    /// tabrakan memakai PhysicsQuery.SphereCast dengan mask pemain dan penghalang pandang.
    /// </summary>
    public class TribalSnare : MonoBehaviour, IPoolable
    {
        [SerializeField] private float radius = 0.3f;

        private TribalBehavior owner;
        private Vector3 direction;
        private float speed;
        private float remaining;
        private bool flying;
        private int hitMask;

        public void Launch(TribalBehavior owner, Vector3 direction, float speed, float maxDistance)
        {
            this.owner = owner;
            this.direction = direction;
            this.speed = speed;
            remaining = maxDistance;
            flying = true;
            hitMask = LayerMask.GetMask(PhysicsMasks.PlayerLayer) | PhysicsMasks.AIVision;
        }

        /// <summary>Pemilik mati atau kembali ke pool: jerat tidak lagi melapor.</summary>
        public void Detach()
        {
            owner = null;
            Finish();
        }

        private void Update()
        {
            if (!flying) return;
            float step = speed * Time.deltaTime;
            if (step <= 0f) return;
            var ray = new Ray(transform.position, direction);
            if (PhysicsQuery.SphereCast(ray, radius, out var hit, step, hitMask))
            {
                bool hitPlayer = hit.collider.GetComponentInParent<PlayerTargetable>() != null;
                var o = owner;
                owner = null;
                Finish();
                if (o != null) o.OnSnareResult(hitPlayer);
                return;
            }
            transform.position += direction * step;
            remaining -= step;
            if (remaining <= 0f)
            {
                var o = owner;
                owner = null;
                Finish();
                if (o != null) o.OnSnareResult(false);
            }
        }

        private void Finish()
        {
            flying = false;
            if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(gameObject);
        }

        public void OnSpawned() { flying = false; owner = null; }
        public void OnReturned() { flying = false; owner = null; }
    }
}
