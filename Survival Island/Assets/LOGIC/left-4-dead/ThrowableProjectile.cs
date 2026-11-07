using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Proyektil barang lempar dari ObjectPooler (layer Projectile, Rigidbody, Collider). Blueprint: left-4-dead/
    /// ThrowableSystem.md bagian "Proyektil" (#96). GDD bagian 3: Molotov, Bom Rakitan, Batu, dan Suar.
    ///
    /// Catatan implementasi (sistem pendukung): Molotov memakai FireSystem.SpawnFireArea (#97). Kalau FireSystem tidak ada
    /// di scene (Kasus Khusus "#97 belum tersedia"), Molotov memakai DamagePipeline langsung: tick damage Fire setiap
    /// throwable.fireTickSeconds (1 detik) lewat OverlapSphere sampai durationSeconds habis.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class ThrowableProjectile : MonoBehaviour, IPoolable
    {
        private const float WaterCheckRadius = 0.2f;   // Physics.CheckSphere(pos, 0,2, Water) di blueprint

        [SerializeField] private Collider col;
        [SerializeField] private Light lureLight;
        [SerializeField] private ParticleSystem lureParticles;
        [SerializeField] private ParticleSystem fireParticles;
        [SerializeField] private GameObject visual;
        [Tooltip("Efek ledakan dari VFXManager (pool). Kosong = tanpa efek.")]
        [SerializeField] private string detonateFxPoolTag;
        [SerializeField] private NoiseEmitter noiseEmitter;
        [SerializeField] private LureTargetable lure;

        private enum Phase : byte { Idle, Flying, Burning, Luring, Done }

        private static readonly List<ThrowableProjectile> activeLures = new List<ThrowableProjectile>(4);
        private static readonly Collider[] fireBuffer = new Collider[32];
        private static readonly List<IDamageable> fireUnique = new List<IDamageable>(16);
        private static readonly List<Collider> fireUniqueColliders = new List<Collider>(16);

        private Rigidbody rb;
        private ThrowableData data;
        private Collider ignoredPlayer;
        private Phase phase;
        private float fuseTimer;
        private float lifeTimer;
        private float effectTimer;
        private float tickTimer;
        private float noiseTimer;
        private bool hasFuse;

        private static ThrowableBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.throwable : null;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics() => activeLures.Clear();

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (col == null) col = GetComponent<Collider>();
            if (lure == null) lure = GetComponent<LureTargetable>();
            if (noiseEmitter == null) noiseEmitter = GetComponent<NoiseEmitter>();
        }

        // ---------------------------------------------------------------- Pool

        public void OnSpawned()
        {
            // Kecepatan Rigidbody 0, kinematic mati, Light dan partikel mati, sumbu dan umur direset.
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            if (col != null) col.enabled = true;
            if (visual != null) visual.SetActive(true);
            SetLure(false);
            if (fireParticles != null) fireParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            phase = Phase.Idle;
            fuseTimer = lifeTimer = effectTimer = tickTimer = noiseTimer = 0f;
            hasFuse = false;
            data = null;
        }

        public void OnReturned()
        {
            // Pengabaian tabrakan tidak terbawa ke pemakaian berikutnya.
            if (ignoredPlayer != null && col != null) Physics.IgnoreCollision(col, ignoredPlayer, false);
            ignoredPlayer = null;
            SetLure(false);
            activeLures.Remove(this);
            phase = Phase.Done;
            data = null;
        }

        // ---------------------------------------------------------------- Lempar

        public void Launch(ThrowableData d, Vector3 velocity, Collider playerCollider)
        {
            data = d;
            if (playerCollider != null && col != null)
            {
                Physics.IgnoreCollision(col, playerCollider, true);
                ignoredPlayer = playerCollider;
            }
            rb.isKinematic = false;
            rb.linearVelocity = velocity;
            phase = Phase.Flying;
            hasFuse = d != null && !d.detonateOnImpact;
            fuseTimer = d != null ? d.fuseSeconds : 0f;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (phase != Phase.Flying || data == null) return;
            // Langsung saat menyentuh. Selain itu fisika memantulkannya.
            if (data.detonateOnImpact) Detonate();
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            if (dt <= 0f || phase == Phase.Idle || phase == Phase.Done) return;
            var b = Bal;
            lifeTimer += dt;
            // Proyektil jatuh ke region yang dilepas atau tersangkut: kembali ke pool.
            if (b != null && lifeTimer > b.maxLifetimeSeconds) { ReturnSelf(); return; }

            switch (phase)
            {
                case Phase.Flying:
                    if (!hasFuse) return;
                    fuseTimer -= dt;
                    if (fuseTimer <= 0f) Detonate();
                    break;
                case Phase.Burning:
                    TickFire(b, dt);
                    break;
                case Phase.Luring:
                    TickLure(dt);
                    break;
            }
        }

        // ---------------------------------------------------------------- Ledak

        private void Detonate()
        {
            if (data == null) { ReturnSelf(); return; }
            Vector3 pos = transform.position;
            // Kasus Khusus "Jatuh ke air": cari trigger air (pengecualian PhysicsLayers.md). Molotov dan Suar padam.
            int waterLayer = LayerMask.NameToLayer(PhysicsMasks.WaterLayer);
            bool inWater = waterLayer >= 0 && Physics.CheckSphere(pos, WaterCheckRadius, 1 << waterLayer, QueryTriggerInteraction.Collide);

            switch (data.effect)
            {
                case ThrowEffect.Fire:
                    if (inWater) { ReturnSelf(); return; }
                    EmitNoise(data.noiseRadius);
                    // Area api Molotov lewat FireSystem (#97), lalu proyektil kembali ke pool.
                    if (FireSystem.Instance != null
                        && FireSystem.Instance.SpawnFireArea(pos, data.radius, data.durationSeconds, data.damage, true) != null)
                    {
                        ReturnSelf();
                        return;
                    }
                    // Cadangan tanpa FireSystem (scene uji): area api di proyektil ini.
                    StartStationary(Phase.Burning);
                    if (fireParticles != null) fireParticles.Play(true);
                    tickTimer = 0f;   // Tick pertama langsung
                    break;

                case ThrowEffect.Explosion:
                    // Bom tetap meledak di air. byPlayer: pemain dan NPC menerima playerExplosionSelfMultiplier (DamagePipeline.md).
                    var info = new DamageInfo
                    {
                        amount = data.damage, type = DamageType.Explosion, source = gameObject, sourceId = data.itemId,
                        hitPoint = pos, hitDirection = Vector3.up, bodyPart = BodyPart.Torso, byPlayer = true
                    };
                    ExplosionDamage.Apply(pos, data.radius, info);
                    if (!string.IsNullOrEmpty(detonateFxPoolTag) && VFXManager.Instance != null)
                        VFXManager.Instance.Play(detonateFxPoolTag, pos, Quaternion.identity);
                    CameraShake.PlayAt(CameraShakeKind.Explosion, pos);
                    EmitNoise(data.noiseRadius);
                    ReturnSelf();
                    break;

                case ThrowEffect.NoiseOnly:
                    EmitNoise(data.noiseRadius);
                    ReturnSelf();
                    break;

                case ThrowEffect.Lure:
                    if (inWater) { ReturnSelf(); return; }
                    StartStationary(Phase.Luring);
                    SetLure(true);
                    RegisterLure();
                    noiseTimer = 0f;   // Suara pertama langsung
                    break;
            }
        }

        private void StartStationary(Phase next)
        {
            phase = next;
            effectTimer = data.durationSeconds;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            if (visual != null && next == Phase.Burning) visual.SetActive(false);
        }

        private void TickFire(ThrowableBalance b, float dt)
        {
            effectTimer -= dt;
            if (effectTimer <= 0f) { ReturnSelf(); return; }
            tickTimer -= dt;
            if (tickTimer > 0f) return;
            float interval = b != null ? Mathf.Max(0.1f, b.fireTickSeconds) : 1f;
            tickTimer = interval;
            Vector3 pos = transform.position;
            int n = PhysicsQuery.OverlapSphere(pos, data.radius, PhysicsMasks.Explosion, fireBuffer);
            PhysicsQuery.CollectUniqueDamageables(fireBuffer, n, fireUnique, fireUniqueColliders);
            for (int i = 0; i < fireUnique.Count; i++)
            {
                var t = fireUnique[i];
                if (t == null || !t.IsAlive) continue;
                Vector3 hit = fireUniqueColliders[i].ClosestPoint(pos);
                // Damage per detik dikali jeda tick. Kebal api lewat EnemyData.resistances / BuildableData.resistances.
                var info = new DamageInfo
                {
                    amount = data.damage * interval, type = DamageType.Fire, source = gameObject, sourceId = data.itemId,
                    hitPoint = hit, hitDirection = (hit - pos).normalized, bodyPart = BodyPart.Torso, byPlayer = true
                };
                t.TakeDamage(in info);
            }
            fireUnique.Clear();
            fireUniqueColliders.Clear();
        }

        private void TickLure(float dt)
        {
            effectTimer -= dt;
            if (effectTimer <= 0f) { ReturnSelf(); return; }
            if (data.noiseIntervalSeconds <= 0f) { if (noiseTimer <= 0f) { EmitNoise(data.noiseRadius); noiseTimer = float.MaxValue; } return; }
            noiseTimer -= dt;
            if (noiseTimer > 0f) return;
            noiseTimer = data.noiseIntervalSeconds;
            EmitNoise(data.noiseRadius);
        }

        private void RegisterLure()
        {
            var b = Bal;
            int max = b != null ? Mathf.Max(1, b.maxActiveLures) : 2;
            // Suar kelebihan memadamkan Suar paling lama.
            while (activeLures.Count >= max)
            {
                var oldest = activeLures[0];
                activeLures.RemoveAt(0);
                if (oldest != null) oldest.ReturnSelf();
            }
            activeLures.Add(this);
            if (lure != null) lure.Activate();
        }

        private void SetLure(bool on)
        {
            if (lureLight != null) lureLight.enabled = on;
            if (lureParticles != null) { if (on) lureParticles.Play(true); else lureParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); }
            if (!on && lure != null) lure.Deactivate();
        }

        private void EmitNoise(float radius)
        {
            if (radius <= 0f) return;
            if (noiseEmitter != null) noiseEmitter.Emit(radius, NoiseSourceKind.Throwable);
            else NoiseEmitter.Emit(transform.position, radius, NoiseSourceKind.Throwable);
        }

        private void ReturnSelf()
        {
            if (phase == Phase.Done) return;
            phase = Phase.Done;
            SetLure(false);
            activeLures.Remove(this);
            if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(gameObject);
            else gameObject.SetActive(false);
        }

        /// <summary>Permadeath dan RunEndedEvent: padamkan semua Suar.</summary>
        public static void ExtinguishAllLures()
        {
            for (int i = activeLures.Count - 1; i >= 0; i--) if (activeLures[i] != null) activeLures[i].ReturnSelf();
            activeLures.Clear();
        }
    }
}
