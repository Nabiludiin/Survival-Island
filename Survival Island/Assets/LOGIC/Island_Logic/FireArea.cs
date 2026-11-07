using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>
    /// Objek pool FX_FireArea: area api di tanah dengan damage per detik. Dibuat FireSystem.SpawnFireArea untuk Molotov (#96)
    /// dan Fireball (#70). Blueprint: Island_Logic/FireSystem.md bagian "FireArea" (#97).
    /// Struktur tidak menerima damage area (supaya tidak dihitung dua kali dengan damage bakar).
    /// </summary>
    public class FireArea : MonoBehaviour, IPoolable
    {
        private static Collider[] buffer = new Collider[32];
        private static readonly List<IDamageable> unique = new List<IDamageable>(16);
        private static readonly List<Flammable> flammables = new List<Flammable>(16);

        [SerializeField] private NavMeshObstacle navObstacle;
        [SerializeField] private LightSource lightSource;
        [SerializeField] private Transform visualScaleRoot;
        [SerializeField] private ParticleSystem[] particles;

        private float radius;
        private float remaining;
        private float damagePerSec;
        private bool byPlayer;
        private bool isRoofed;
        private float tickTimer;
        private bool released = true;
        private RegionType region;

        public float Radius => radius;
        public float Remaining => remaining;
        public bool IsCarving => navObstacle != null && navObstacle.enabled && navObstacle.carving;
        public bool IsActive => !released;

        private static FireBalance B => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.fire : null;

        internal void Setup(float radius, float duration, float damagePerSec, bool byPlayer, bool roofed, bool carve)
        {
            this.radius = Mathf.Max(0.1f, radius);
            remaining = duration;
            this.damagePerSec = damagePerSec;
            this.byPlayer = byPlayer;
            isRoofed = roofed;
            region = RegionStreaming.RegionOf(transform.position);
            tickTimer = 0f;
            released = false;
            if (visualScaleRoot != null) visualScaleRoot.localScale = Vector3.one * this.radius;
            if (navObstacle != null)
            {
                navObstacle.shape = NavMeshObstacleShape.Capsule;
                navObstacle.radius = this.radius;
                navObstacle.carving = carve;
                navObstacle.carveOnlyStationary = true;
                navObstacle.enabled = carve;
            }
            if (lightSource != null) lightSource.IsLit = true;
            if (particles != null)
                for (int i = 0; i < particles.Length; i++) if (particles[i] != null) particles[i].Play(true);
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            if (released || dt <= 0f) return;   // Jeda: timer berhenti
            var b = B;
            remaining -= dt;
            if (remaining <= 0f)
            {
                Release();
                return;
            }
            tickTimer += dt;
            float tick = b != null ? b.areaTickSeconds : 0.5f;
            if (tickTimer < tick) return;
            tickTimer -= tick;
            Tick(tick, b);
        }

        private void Tick(float tick, FireBalance b)
        {
            Vector3 pos = transform.position;
            int count = PhysicsQuery.OverlapSphere(pos, radius, PhysicsMasks.FireReach, buffer);

            // 2. Menyalakan Flammable di dalam area, kecuali saat Storm/Blizzard tak beratap.
            WeatherType w = RegionalWeather.Instance != null ? RegionalWeather.Instance.GetWeather(region) : WeatherType.Clear;
            bool canIgnite = isRoofed || (w != WeatherType.Storm && w != WeatherType.Blizzard);
            if (canIgnite)
            {
                float chance = b != null ? b.areaIgniteChancePerTick : 0.25f;
                flammables.Clear();
                for (int i = 0; i < count; i++)
                {
                    var f = buffer[i] != null ? buffer[i].GetComponentInParent<Flammable>() : null;
                    if (f == null || f.IsBurning || flammables.Contains(f)) continue;
                    flammables.Add(f);
                    f.TryIgnite(chance, byPlayer);
                }
                flammables.Clear();
            }

            // 3. Damage area ke target unik selain BaseHealth.
            PhysicsQuery.CollectUniqueDamageables(buffer, count, unique);
            for (int i = 0; i < unique.Count; i++)
            {
                var t = unique[i];
                if (t == null || !t.IsAlive || t is BaseHealth) continue;
                var info = new DamageInfo
                {
                    amount = damagePerSec * tick,
                    type = DamageType.Fire,
                    sourceId = "fire",
                    hitPoint = pos,
                    hitDirection = Vector3.up,
                    bodyPart = BodyPart.Torso,
                    byPlayer = byPlayer
                };
                t.TakeDamage(info);
            }
            unique.Clear();
            for (int i = 0; i < count; i++) buffer[i] = null;
        }

        /// <summary>Kembali ke pool. Aman dipanggil berkali-kali.</summary>
        public void Release()
        {
            if (released) return;
            released = true;
            if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(gameObject);
            else gameObject.SetActive(false);
        }

        public void OnSpawned()
        {
            released = true;   // Setup mengaktifkan
            remaining = 0f;
            tickTimer = 0f;
            if (navObstacle != null) navObstacle.enabled = false;
        }

        public void OnReturned()
        {
            released = true;
            remaining = 0f;
            tickTimer = 0f;
            if (navObstacle != null) navObstacle.enabled = false;
            if (lightSource != null) lightSource.IsLit = false;
            if (particles != null)
                for (int i = 0; i < particles.Length; i++) if (particles[i] != null) particles[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            FireSystem.Instance?.UnregisterArea(this);
        }
    }
}
