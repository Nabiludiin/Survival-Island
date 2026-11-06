using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Status bakar satu objek: struktur kayu, pohon ResourceNode, zombie, hewan, dan Tong Merah.
    /// Blueprint: Island_Logic/FireSystem.md bagian "Flammable" (#97). GDD bagian 5 (Erupsi menghancurkan area) dan bagian 6
    /// (bencana memaksa perbaikan); aturan api menjalar adalah usulan yang disetujui developer (#85).
    /// Pemain dan NPC tidak punya Flammable (usulan 3).
    /// </summary>
    public class Flammable : MonoBehaviour, IPoolable
    {
        /// <summary>Flammable struktur yang aktif, untuk memulihkan api dari save (world_fire).</summary>
        internal static readonly List<Flammable> Structures = new List<Flammable>(64);

        [SerializeField] private FlammableProfile profile;
        [Tooltip("Titik efek dan pusat jangkauan. Kosong berarti transform ini.")]
        [SerializeField] private Transform firePoint;

        private bool isBurning;
        private float burnRemaining;
        private float reigniteAt;
        private int generation;
        private bool byPlayer;
        private bool isRoofed;
        private float roofTimer;
        private RegionType homeRegion;
        private IDamageable damageable;
        private IStructureHealth structure;
        private ResourceNode resourceNode;
        private EnemyHealth enemyHealth;
        private EnemyAI enemyAI;
        private Collider boundsCollider;
        private BurnEffect activeEffect;

        public FlammableProfile Profile => profile;
        public bool IsBurning => isBurning;
        public float BurnRemaining => burnRemaining;
        public int Generation => generation;
        public bool ByPlayer => byPlayer;
        public bool IsRoofed => isRoofed;
        public RegionType HomeRegion => homeRegion;
        public bool IsEnemy => enemyHealth != null;
        public bool IsStructure => structure != null;
        public string StructureId => structure != null ? structure.StructureId : string.Empty;
        public Vector3 FirePosition => firePoint != null ? firePoint.position : transform.position;
        internal BurnEffect ActiveEffect => activeEffect;

        private static FireBalance B => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.fire : null;

        private void Awake()
        {
            damageable = GetComponentInParent<IDamageable>();
            structure = GetComponentInParent<IStructureHealth>();
            resourceNode = GetComponentInParent<ResourceNode>();
            enemyHealth = GetComponentInParent<EnemyHealth>();
            enemyAI = GetComponentInParent<EnemyAI>();
            boundsCollider = GetComponentInChildren<Collider>();
        }

        private void OnEnable()
        {
            if (structure != null && !Structures.Contains(this)) Structures.Add(this);
        }

        private void OnDisable()
        {
            // Kasus khusus: struktur hancur atau region dilepas memadamkan api tanpa event.
            Extinguish(true);
            Structures.Remove(this);
        }

        // ---------------------------------------------------------------- Menyala

        /// <summary>Lempar peluang menyala. Lihat FireSystem.md "TryIgnite".</summary>
        public bool TryIgnite(float chance, bool byPlayer, int generation = 0)
        {
            var b = B;
            var sys = FireSystem.Instance;
            // 1.
            if (!isActiveAndEnabled || profile == null || isBurning || profile.IsImmune || Time.time < reigniteAt) return false;
            if (sys == null || !sys.CanIgniteMore) return false;   // Batas tercapai: gagal tanpa pesan
            // 2. Usulan 5: batas lompatan vegetasi, supaya satu percikan tidak menghabiskan hutan.
            if (profile.kind == FlammableKind.Vegetation && generation > (b != null ? b.maxVegetationGeneration : 3)) return false;
            if (resourceNode != null && resourceNode.IsDepleted) return false;
            if (enemyHealth != null && !enemyHealth.IsAlive) return false;
            // 3. Aliran Gameplay (#89).
            float p = chance * (1f - profile.fireResistance);
            if (FireSystem.Roll01() >= p) return false;
            // 4-5.
            Ignite(profile.burnSeconds, byPlayer, generation);
            return true;
        }

        /// <summary>Menyala tanpa lempar peluang. Dipakai saat memulihkan api struktur dari save.</summary>
        internal void ForceIgnite(float remaining, bool byPlayer, int generation)
        {
            if (profile == null || isBurning || profile.IsImmune) return;
            Ignite(remaining, byPlayer, generation);
        }

        private void Ignite(float remaining, bool byPlayerValue, int gen)
        {
            isBurning = true;
            burnRemaining = remaining;
            byPlayer = byPlayerValue;
            generation = gen;
            homeRegion = RegionStreaming.RegionOf(FirePosition);
            RecheckRoof();
            activeEffect = FireSystem.Instance != null ? FireSystem.Instance.AcquireBurnEffect(this) : null;
            FireSystem.Instance?.RegisterBurning(this);
            if (structure != null) EventChannel<StructureBurningEvent>.Publish(new StructureBurningEvent(structure.StructureId, true));
            // Damage Fire pertama ke Tong Merah memulai sumbunya (EnvironmentalHazard.md); damage datang lewat tick bakar.
        }

        /// <summary>Padam. silent = tanpa StructureBurningEvent.</summary>
        public void Extinguish(bool silent = false)
        {
            if (!isBurning) return;
            isBurning = false;
            var b = B;
            reigniteAt = Time.time + (b != null ? b.reigniteCooldownSeconds : 20f);
            if (activeEffect != null) activeEffect.Release();
            activeEffect = null;
            FireSystem.Instance?.UnregisterBurning(this);
            if (structure != null && !silent) EventChannel<StructureBurningEvent>.Publish(new StructureBurningEvent(structure.StructureId, false));
        }

        // ---------------------------------------------------------------- Tick (dipanggil FireSystem)

        /// <summary>Damage bakar satu tick, lalu kurangi sisa waktu. Mengembalikan false kalau api padam.</summary>
        internal bool TickBurn(float tickSeconds, WeatherType weather)
        {
            if (!isBurning) return false;
            var b = B;

            // Status tubuh musuh: mati, tenggelam, atau terbakar matahari memadamkan api tanpa menumpuk efek #63.
            if (enemyHealth != null)
            {
                if (!enemyHealth.IsAlive || (enemyAI != null && (enemyAI.CurrentState == AIState.Submerged || enemyAI.CurrentState == AIState.BurnToAshes)))
                {
                    Extinguish(true);
                    return false;
                }
            }

            // Atap dicek ulang setiap roofRecheckSeconds, karena atapnya bisa ikut hancur.
            roofTimer += tickSeconds;
            if (roofTimer >= (b != null ? b.roofRecheckSeconds : 5f)) RecheckRoof();

            // Cuaca tak beratap: Storm dan Blizzard padam pada tick berikutnya (sama dengan CampfireManager, #44).
            if (!isRoofed && (weather == WeatherType.Storm || weather == WeatherType.Blizzard))
            {
                Extinguish();
                return false;
            }

            if (profile.burnDamagePerSec > 0f && damageable != null && damageable.IsAlive)
            {
                var info = new DamageInfo
                {
                    amount = profile.burnDamagePerSec * tickSeconds,
                    type = DamageType.Fire,
                    sourceId = "fire",
                    hitPoint = FirePosition,
                    hitDirection = Vector3.up,
                    bodyPart = BodyPart.Torso,
                    byPlayer = byPlayer
                };
                damageable.TakeDamage(info);
                if (!isBurning) return false;   // Target bisa mematikan api (tong meledak, struktur hancur)
            }

            // Rain: burnRemaining berkurang 2 kali lebih cepat (1 / rainBurnMultiplier).
            float rate = 1f;
            if (!isRoofed && weather == WeatherType.Rain) rate = 1f / Mathf.Max(0.01f, b != null ? b.rainBurnMultiplier : 0.5f);
            burnRemaining -= tickSeconds * rate;
            if (burnRemaining > 0f) return true;

            // Akhir bakar.
            burnRemaining = 0f;
            if (profile.kind == FlammableKind.Vegetation && resourceNode != null && !resourceNode.IsDepleted)
                resourceNode.ForceDeplete(false);
            Extinguish();
            return false;
        }

        /// <summary>Pengali peluang menjalar menurut cuaca. 0 berarti tidak menjalar.</summary>
        internal float SpreadWeatherMultiplier(WeatherType weather)
        {
            if (isRoofed) return 1f;
            var b = B;
            switch (weather)
            {
                case WeatherType.Rain: return b != null ? b.rainSpreadMultiplier : 0.25f;
                case WeatherType.Snow: return b != null ? b.snowSpreadMultiplier : 0.5f;
                case WeatherType.Storm:
                case WeatherType.Blizzard: return 0f;
                default: return 1f;
            }
        }

        private void RecheckRoof()
        {
            roofTimer = 0f;
            var b = B;
            float top = boundsCollider != null ? boundsCollider.bounds.max.y : FirePosition.y;
            Vector3 origin = new Vector3(FirePosition.x, top + 0.2f, FirePosition.z);
            isRoofed = PhysicsQuery.Raycast(new Ray(origin, Vector3.up), out _, b != null ? b.roofCheckDistance : 20f, PhysicsMasks.Sunlight);
        }

        // ---------------------------------------------------------------- Pool (zombie dan hewan)

        public void OnSpawned()
        {
            isBurning = false;
            burnRemaining = 0f;
            reigniteAt = 0f;
            generation = 0;
            byPlayer = false;
            activeEffect = null;
        }

        public void OnReturned()
        {
            Extinguish(true);
        }
    }
}
