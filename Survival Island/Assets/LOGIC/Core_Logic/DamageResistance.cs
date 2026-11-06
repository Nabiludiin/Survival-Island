using System;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu baris armor per jenis damage dan bagian tubuh. Blueprint: Core_Logic/DamagePipeline.md bagian "Armor" (#59).
    /// Dipakai EnemyData.resistances dan nanti BuildableData.resistances (#79).
    /// </summary>
    [Serializable]
    public struct DamageResistance
    {
        public DamageType type;
        public BodyPart bodyPart;
        public bool anyBodyPart;
        public float multiplier;      // 1 berarti tanpa pengurangan
    }

    /// <summary>
    /// Fungsi armor bersama. Daftar pendek dicari berurutan, tanpa Dictionary (DamagePipeline.md).
    /// Urutan hitung: senjata -> HitboxModifier -> armor di sini -> SurvivalStats (pengali kesulitan).
    /// </summary>
    public static class DamageResistanceTable
    {
        /// <summary>Pengali armor tanpa menghitung damage (#95). Baris pertama yang cocok menang. Tanpa baris = 1.</summary>
        public static float GetMultiplier(DamageResistance[] resistances, DamageType type, BodyPart bodyPart)
        {
            if (resistances == null) return 1f;
            for (int i = 0; i < resistances.Length; i++)
            {
                var r = resistances[i];
                if (r.type != type) continue;
                if (!r.anyBodyPart && r.bodyPart != bodyPart) continue;
                return r.multiplier < 0f ? 0f : r.multiplier;
            }
            return 1f;
        }

        /// <summary>
        /// Damage setelah armor, dengan batas bawah minDamageAfterArmor untuk musuh, supaya tembakan tidak pernah 0.
        /// Pengali 0 (misalnya bangunan terhadap peluru) tetap menghasilkan 0.
        /// </summary>
        public static float Apply(DamageResistance[] resistances, in DamageInfo info)
        {
            if (info.amount <= 0f) return 0f;
            float mult = GetMultiplier(resistances, info.type, info.bodyPart);
            if (mult <= 0f) return 0f;
            float result = info.amount * mult;
            var bal = GameManager.CurrentBalance;
            float min = bal != null ? bal.damage.minDamageAfterArmor : 1f;
            return result < min ? min : result;
        }
    }
}
