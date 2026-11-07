using UnityEngine;

namespace SurvivalIsland
{
    public enum ThrowEffect { Fire, Explosion, NoiseOnly, Lure }

    /// <summary>
    /// Data satu barang lempar. Blueprint: left-4-dead/ThrowableSystem.md (#96).
    /// GDD bagian 3: Molotov, Bom Rakitan, Batu, dan Suar untuk mengendalikan atau memancing kerumunan zombie.
    /// Semua angka berasal dari GameBalanceConfig.throwable. ThrowableSystem membuat salinan aset saat OnEnable lalu
    /// memanggil ApplyBalance, jadi aset asli tidak berubah saat bermain.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Throwable Data")]
    public class ThrowableData : ScriptableObject
    {
        public const string MolotovId = "Throw_Molotov";
        public const string BombId = "Throw_Bomb";
        public const string RockId = "Throw_Rock";
        public const string FlareId = "Throw_Flare";

        public string itemId;             // Item di inventori
        public string poolTag;            // Tag proyektil di ObjectPooler
        public ThrowEffect effect;
        public float throwForce;          // m/s
        public float upwardBias;          // Tambahan arah atas
        public bool detonateOnImpact;     // true berarti langsung saat menyentuh
        public float fuseSeconds;         // Sumbu kalau tidak langsung
        public float radius;
        public float damage;              // Fire: per detik. Explosion: total
        public float durationSeconds;     // Lama api atau pancingan
        public float noiseRadius;         // Radius Noise Sphere
        public float noiseIntervalSeconds;// 0 berarti sekali saja

        /// <summary>Salin angka dari config sesuai itemId (tabel "Daftar barang lempar").</summary>
        public void ApplyBalance(ThrowableBalance b)
        {
            if (b == null) return;
            switch (itemId)
            {
                case MolotovId:
                    effect = ThrowEffect.Fire; throwForce = b.molotovForce; detonateOnImpact = true; fuseSeconds = 0f;
                    radius = b.molotovRadius; damage = b.molotovDamagePerSec; durationSeconds = b.molotovDuration;
                    noiseRadius = b.molotovNoise; noiseIntervalSeconds = 0f;
                    break;
                case BombId:
                    effect = ThrowEffect.Explosion; throwForce = b.bombForce; detonateOnImpact = false; fuseSeconds = b.bombFuse;
                    radius = b.bombRadius; damage = b.bombDamage; durationSeconds = 0f;
                    noiseRadius = b.bombNoise; noiseIntervalSeconds = 0f;
                    break;
                case RockId:
                    effect = ThrowEffect.NoiseOnly; throwForce = b.rockForce; detonateOnImpact = true; fuseSeconds = 0f;
                    radius = 0f; damage = 0f; durationSeconds = 0f;
                    noiseRadius = b.rockNoise; noiseIntervalSeconds = 0f;
                    break;
                case FlareId:
                    effect = ThrowEffect.Lure; throwForce = b.flareForce; detonateOnImpact = false; fuseSeconds = b.flareFuse;
                    radius = 0f; damage = 0f; durationSeconds = b.flareDuration;
                    noiseRadius = b.flareNoise; noiseIntervalSeconds = b.flareNoiseInterval;
                    break;
            }
        }
    }
}
