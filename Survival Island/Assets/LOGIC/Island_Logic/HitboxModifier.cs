using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Penanda bagian tubuh pada collider tulang musuh (layer EnemyHitbox). Blueprint: Island_Logic/HitboxModifier.md (#59).
    /// Headshot 2x mengikuti Claude.md Pilar 3 dan keputusan #85. Kaki 0,5x dan lengan = Torso adalah usulan disetujui developer.
    /// Komponen ini tidak punya referensi HP dan tidak memanggil TakeDamage. DamageRouter yang memanggil Apply.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class HitboxModifier : MonoBehaviour
    {
        public BodyPart bodyPart = BodyPart.Torso;

        /// <summary>Isi bodyPart, lalu kalikan amount hanya untuk Bullet dan Melee. Field lain tidak diubah.</summary>
        public void Apply(ref DamageInfo info)
        {
            info.bodyPart = bodyPart;
            if (info.type == DamageType.Bullet || info.type == DamageType.Melee)
                info.amount *= GetMultiplier(bodyPart);
        }

        /// <summary>
        /// Pengali dibaca dari config setiap panggilan, supaya perubahan DevConsole langsung berlaku.
        /// Dibatasi Max(0, nilai) supaya angka negatif tidak menyembuhkan musuh.
        /// </summary>
        public static float GetMultiplier(BodyPart part)
        {
            var bal = GameManager.CurrentBalance;
            if (bal == null) return part == BodyPart.Head ? 2f : part == BodyPart.Leg ? 0.5f : 1f;
            var d = bal.damage;
            float v = part == BodyPart.Head ? d.headMultiplier : part == BodyPart.Leg ? d.legMultiplier : d.torsoMultiplier;
            return Mathf.Max(0f, v);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            int layer = LayerMask.NameToLayer(PhysicsMasks.EnemyHitboxLayer);
            if (layer >= 0 && gameObject.layer != layer)
                Debug.LogWarning($"[HitboxModifier] '{name}' bukan di layer EnemyHitbox.", this);
            var col = GetComponent<Collider>();
            if (col != null && col.isTrigger)
                Debug.LogWarning($"[HitboxModifier] Collider '{name}' tidak boleh trigger (#87).", this);
            if (GetComponentInParent<IDamageable>() == null)
                Debug.LogWarning($"[HitboxModifier] '{name}' tidak punya IDamageable di parent.", this);
        }
#endif
    }
}
