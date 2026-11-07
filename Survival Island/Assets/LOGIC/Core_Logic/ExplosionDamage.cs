using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Damage ledakan dengan target unik dan falloff linier. Blueprint: Core_Logic/DamagePipeline.md bagian
    /// "Ledakan dan target unik" (#59). Dipakai EnvironmentalHazard, Fireball (#70), Bom Rakitan (#96), dan Zombie Beku (#25).
    /// - Tanpa damage ganda: mask Explosion memuat kapsul Enemy (bukan EnemyHitbox), dan target disaring lewat parent.
    /// - Bagian tubuh selalu Torso, tanpa pengali headshot. Ledakan tidak mengecek dinding.
    /// </summary>
    public static class ExplosionDamage
    {
        private static Collider[] buffer = new Collider[64];
        private static readonly List<IDamageable> unique = new List<IDamageable>(32);
        private static readonly List<Collider> uniqueColliders = new List<Collider>(32);

        public static void Apply(Vector3 center, float radius, DamageInfo info)
        {
            if (radius <= 0f || info.amount <= 0f) return;
            var bal = GameManager.CurrentBalance;
            int size = bal != null ? Mathf.Max(8, bal.damage.maxExplosionTargets) : 64;
            if (buffer.Length != size) buffer = new Collider[size];
            float minFalloff = bal != null ? bal.damage.explosionMinFalloff : 0.25f;

            int count = PhysicsQuery.OverlapSphere(center, radius, PhysicsMasks.Explosion, buffer);
            PhysicsQuery.CollectUniqueDamageables(buffer, count, unique, uniqueColliders);

            float baseAmount = info.amount;
            for (int i = 0; i < unique.Count; i++)
            {
                var target = unique[i];
                if (target == null || !target.IsAlive) continue;
                Vector3 closest = uniqueColliders[i].ClosestPoint(center);
                Vector3 delta = closest - center;
                float dist = delta.magnitude;
                // Falloff: Lerp(1, explosionMinFalloff, jarak / radius). Usulan disetujui developer (#85).
                float falloff = Mathf.Lerp(1f, minFalloff, Mathf.Clamp01(dist / radius));
                var hitInfo = info;
                hitInfo.amount = baseAmount * falloff;
                hitInfo.hitPoint = closest;
                hitInfo.hitDirection = dist > 0.0001f ? delta / dist : Vector3.up;   // #95: knockback menjauhi pusat
                hitInfo.bodyPart = BodyPart.Torso;
                target.TakeDamage(hitInfo);
            }
            unique.Clear();
            uniqueColliders.Clear();
            // Tong di dalam radius ikut bersumbu, supaya Bom Rakitan (#96) dan Fireball (#70) memicu ledakan berantai.
            // Aman dipanggil dua kali: tong yang sudah bersumbu diabaikan.
            EnvironmentalHazard.NotifyExplosion(center, radius, info.byPlayer);
        }
    }
}
