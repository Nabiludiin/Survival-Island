using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Batu lemparan zombie (pool Thrown_Rock, layer Projectile). Blueprint: left-4-dead/AITraversal.md bagian
    /// "Respons bertahap" RockThrow (#94). Usulan disetujui developer: semua zombie boleh melempar batu, supaya pemain di
    /// atas batu tidak aman selamanya. Kena pemain atau struktur, lalu kembali ke pool.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class ThrownRock : MonoBehaviour, IPoolable
    {
        public const string PoolTag = "Thrown_Rock";

        [SerializeField] private Collider col;

        private Rigidbody rb;
        private Collider ignored;
        private float damage;
        private float infection;
        private string sourceId;
        private float life;
        private bool active;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (col == null) col = GetComponent<Collider>();
        }

        public void OnSpawned()
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            life = 0f;
            active = false;
            damage = infection = 0f;
        }

        public void OnReturned()
        {
            if (ignored != null && col != null) Physics.IgnoreCollision(col, ignored, false);
            ignored = null;
            active = false;
        }

        public void Launch(Vector3 velocity, float damage, float infection, string sourceId, Collider thrower)
        {
            this.damage = damage;
            this.infection = infection;
            this.sourceId = sourceId;
            if (thrower != null && col != null) { Physics.IgnoreCollision(col, thrower, true); ignored = thrower; }
            rb.linearVelocity = velocity;
            active = true;
        }

        private void Update()
        {
            if (!active || Time.deltaTime <= 0f) return;
            life += Time.deltaTime;
            var b = GameManager.CurrentBalance;
            if (life >= (b != null ? b.traversal.rockLifetimeSeconds : 6f)) ReturnSelf();
        }

        private void OnCollisionEnter(Collision c)
        {
            if (!active) return;
            var target = c.collider.GetComponentInParent<IDamageable>();
            if (target != null && target.IsAlive && !(target is EnemyHealth))
            {
                var contact = c.contactCount > 0 ? c.GetContact(0).point : transform.position;
                // Bangunan hanya menerima Structural (pengali bawaan BaseHealth). Pemain dan NPC menerima Melee.
                bool structure = target is BaseHealth;
                var info = new DamageInfo
                {
                    amount = damage, type = structure ? DamageType.Structural : DamageType.Melee, source = gameObject, sourceId = sourceId,
                    hitPoint = contact, hitDirection = rb.linearVelocity.sqrMagnitude > 0.01f ? rb.linearVelocity.normalized : transform.forward,
                    bodyPart = BodyPart.Torso, infection = infection, byPlayer = false
                };
                target.TakeDamage(in info);
            }
            ReturnSelf();
        }

        private void ReturnSelf()
        {
            if (!active) return;
            active = false;
            if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(gameObject);
            else gameObject.SetActive(false);
        }
    }
}
