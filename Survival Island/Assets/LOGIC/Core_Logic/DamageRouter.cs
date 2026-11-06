using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu jalan masuk damage untuk semua senjata raycast dan melee. Blueprint: Core_Logic/DamagePipeline.md
    /// bagian "Mencari target lewat parent" (#59). Collider yang kena sering kepala atau kaki, jadi IDamageable dicari lewat parent.
    /// Hasil yang dijamin: raycast dan melee selalu melewati HitboxModifier, dan pengali headshot tidak ditulis dua kali.
    /// </summary>
    public static class DamageRouter
    {
        /// <summary>Terapkan damage ke target hit. false kalau tidak ada IDamageable hidup.</summary>
        public static bool ApplyHit(in RaycastHit hit, DamageInfo info)
        {
            return ApplyHit(hit.collider, hit.point, info);
        }

        /// <summary>Versi tanpa RaycastHit, dipakai overlap (Shove, serangan musuh).</summary>
        public static bool ApplyHit(Collider collider, Vector3 point, DamageInfo info)
        {
            if (collider == null) return false;
            var target = collider.GetComponentInParent<IDamageable>();
            if (target == null || !target.IsAlive) return false;
            info.hitPoint = point;
            // HitboxModifier mengisi bodyPart dan pengali bagian tubuh (headshot 2x, #85).
            // Collider tanpa HitboxModifier tetap kena dengan Torso dan pengali 1 (HitboxModifier.md "Kasus Khusus").
            if (collider.TryGetComponent(out HitboxModifier hitbox)) hitbox.Apply(ref info);
            target.TakeDamage(info);
            return true;
        }
    }
}
