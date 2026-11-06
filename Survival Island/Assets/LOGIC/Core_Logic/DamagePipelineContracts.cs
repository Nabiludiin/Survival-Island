using UnityEngine;

namespace SurvivalIsland
{
    // =====================================================================================
    // Kontrak jalur damage. Pemilik resmi: DamagePipeline (#59, Langkah 5), blueprint Core_Logic/DamagePipeline.md.
    // Ditulis di Langkah 3 karena SurvivalStats (IDamageable) dan PlayerTraversal (damage jatuh) sudah memakainya.
    // Langkah 5 TIDAK mendeklarasikan ulang tipe di file ini. EnemyHealth, BaseHealth, DamageRouter, ExplosionDamage,
    // dan PhysicsQuery.CollectUniqueDamageables ditulis Langkah 5 di file terpisah.
    // =====================================================================================

    /// <summary>Jenis damage. Nilai mengikuti GLOSSARY.md.</summary>
    public enum DamageType
    {
        Bullet, Melee, Fire, Explosion, Frost, Starvation, Dehydration, Infection, Structural, Fall, Drown, Poison
    }

    /// <summary>Bagian tubuh yang kena. Tidak punya nilai kosong, jadi damage tanpa hitbox memakai Torso (DamagePipeline.md).</summary>
    public enum BodyPart { Head, Torso, Leg }

    /// <summary>Satu jalur damage untuk pemain, musuh, NPC, dan bangunan (DamagePipeline.md).</summary>
    public interface IDamageable
    {
        bool IsAlive { get; }
        void TakeDamage(in DamageInfo info);
    }

    /// <summary>Isi satu damage. Struct dikirim lewat in, supaya tanpa garbage.</summary>
    public struct DamageInfo
    {
        public float amount;          // Damage dasar, sudah termasuk pengali bagian tubuh setelah melewati router
        public DamageType type;
        public GameObject source;     // Boleh null untuk lingkungan. Hanya sah pada frame yang sama
        public string sourceId;       // EnemyData.enemyId atau itemId
        public Vector3 hitPoint;
        public Vector3 hitDirection;  // Arah gerak serangan, dari sumber ke target
        public BodyPart bodyPart;     // Nilai awal Torso untuk damage tanpa hitbox
        public float infection;       // Tambahan infeksi. 0 kalau bukan serangan zombie
        public bool byPlayer;         // Berasal dari pemain
    }

    /// <summary>
    /// Pemetaan DamageType terakhir ke DeathReason (DamagePipeline.md, "Pencatatan DeathReason").
    /// GDD bagian 3: Game Over saat HP habis karena serangan, lapar, haus, atau infeksi.
    /// </summary>
    public static class DeathReasonMap
    {
        public static DeathReason From(DamageType type)
        {
            switch (type)
            {
                case DamageType.Bullet:
                case DamageType.Melee:
                case DamageType.Structural: return DeathReason.KilledByEnemy;
                case DamageType.Fire: return DeathReason.Fire;
                case DamageType.Explosion: return DeathReason.Explosion;
                case DamageType.Frost: return DeathReason.Frostbite;
                case DamageType.Starvation: return DeathReason.Starvation;
                case DamageType.Dehydration: return DeathReason.Dehydration;
                // GDD bagian 3 + keputusan developer: infeksi hanya membunuh saat HP habis, alasannya Zombified
                case DamageType.Infection: return DeathReason.Zombified;
                case DamageType.Fall: return DeathReason.Fall;
                case DamageType.Drown: return DeathReason.Drowned;
                case DamageType.Poison: return DeathReason.Poison;
                default: return DeathReason.KilledByEnemy;
            }
        }
    }
}
