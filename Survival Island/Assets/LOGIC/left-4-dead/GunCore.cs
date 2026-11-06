using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Logika tembakan bersama pemain dan NPC, tanpa MonoBehaviour. Blueprint: left-4-dead/GunController.md bagian "Tembakan" (#58).
    /// Claude.md Pilar 3: Raycast Shooting, damage berbasis collider (headshot 2x lewat HitboxModifier di DamageRouter).
    /// Peluru tidak punya objek fisik dan tidak menembus (PhysicsLayers.md), jadi tidak ada peluru yang di-pool.
    /// </summary>
    public class GunCore
    {
        public const string DefaultImpactTag = "VFX_Impact";

        public WeaponStats stats;
        public float CurrentSpread { get; private set; }
        private float nextShotTime;

        public void SetStats(in WeaponStats s)
        {
            stats = s;
            CurrentSpread = s.spreadBase;
        }

        public bool CanFire => Time.time >= nextShotTime;

        /// <summary>Satu tembakan. false kalau masih dalam jeda tembak. impactTag = WeaponData.impactTag (#15).</summary>
        public bool Fire(in Ray ray, GameObject shooter, string itemId, bool byPlayer, string impactTag = null)
        {
            // 1-2.
            if (Time.time < nextShotTime) return false;
            nextShotTime = Time.time + stats.secondsBetweenShots;
            // 3. Geser arah acak dalam kerucut spread. UnityEngine.Random: tidak memengaruhi isi dunia.
            Vector3 dir = ray.direction;
            if (CurrentSpread > 0f)
            {
                Vector2 c = Random.insideUnitCircle * Mathf.Tan(CurrentSpread * Mathf.Deg2Rad);
                Vector3 right = Vector3.Cross(dir, Vector3.up);
                if (right.sqrMagnitude < 0.001f) right = Vector3.right;
                right.Normalize();
                Vector3 up = Vector3.Cross(right, dir).normalized;
                dir = (dir + right * c.x + up * c.y).normalized;
            }
            var shot = new Ray(ray.origin, dir);
            bool hitEnemy = false, headshot = false;
            // 4. Mask Shoot tidak memuat Player dan kapsul Enemy; PhysicsQuery mengabaikan trigger zona (#87).
            if (PhysicsQuery.Raycast(shot, out var hit, stats.maxRange, PhysicsMasks.Shoot))
            {
                // 5. Pengali kepala 2x, armor, dan friendly fire dihitung jalur #59.
                var info = new DamageInfo
                {
                    amount = stats.damage, type = DamageType.Bullet, source = shooter, sourceId = itemId,
                    hitDirection = dir, bodyPart = BodyPart.Torso, infection = 0f, byPlayer = byPlayer
                };
                bool applied = DamageRouter.ApplyHit(hit, info);
                if (applied && hit.collider.GetComponentInParent<EnemyHealth>() != null)
                {
                    hitEnemy = true;
                    headshot = hit.collider.TryGetComponent(out HitboxModifier hb) && hb.bodyPart == BodyPart.Head;
                }
                // 6. Efek impact (#15). Tag VFX_Impact bawaan = null, supaya efek mengikuti permukaan.
                if (VFXManager.Instance != null)
                    VFXManager.Instance.PlayImpact(hit, string.IsNullOrEmpty(impactTag) || impactTag == DefaultImpactTag ? null : impactTag);
            }
            // 7.
            CurrentSpread = Mathf.Min(CurrentSpread + stats.spreadPerShot, stats.spreadMax);
            // 8. Sekali per tembakan.
            EventChannel<ShotResolvedEvent>.Publish(new ShotResolvedEvent(itemId, hitEnemy, headshot, byPlayer));
            return true;
        }

        /// <summary>Spread kembali ke dasar (ditambah spreadMoving saat bergerak).</summary>
        public void TickSpread(bool isMoving, float dt)
        {
            float floor = stats.spreadBase + (isMoving ? stats.spreadMoving : 0f);
            if (CurrentSpread < floor) CurrentSpread = floor;
            else CurrentSpread = Mathf.MoveTowards(CurrentSpread, floor, stats.spreadRecoverPerSec * dt);
        }
    }
}
