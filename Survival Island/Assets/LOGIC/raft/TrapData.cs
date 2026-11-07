using UnityEngine;

namespace SurvivalIsland
{
    public enum TrapKind { Spike, BarbedWire, Campfire }

    /// <summary>
    /// Data satu jenis jebakan. Blueprint: raft/BaseDefense.md (#26). Daftar jebakan dan angkanya usulan disetujui developer.
    /// trapId sama dengan BuildableData.id. Angka damage, jeda, lambat, keausan, dan suara disalin dari GameBalanceConfig.trap
    /// lewat GetStats (Claude.md Tahap 6 poin 8) ke salinan per jebakan, jadi aset tidak diubah saat bermain.
    /// Nilai di aset hanya cadangan kalau config belum ada.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Trap Data")]
    public class TrapData : ScriptableObject
    {
        public string trapId;              // Sama dengan BuildableData.id
        public TrapKind kind;
        public DamageType damageType;      // Melee untuk paku dan kawat, Fire untuk api
        public float damagePerHit;
        public float hitIntervalSeconds;   // Jeda per target
        public float slowMultiplier = 1f;  // 1 berarti tanpa lambat
        public float slowSeconds;
        public float selfDamagePerHit;     // HP jebakan berkurang tiap kena musuh
        public float noiseRadius;
        public string hitVfxPoolTag;       // Harus terdaftar di GameBalanceConfig.vfx.catalog

        /// <summary>Angka efektif: dari GameBalanceConfig.trap sesuai jenis, atau isi aset kalau config tidak ada.</summary>
        public TrapStats GetStats(TrapBalance b)
        {
            var s = new TrapStats
            {
                damagePerHit = damagePerHit, hitIntervalSeconds = hitIntervalSeconds, slowMultiplier = slowMultiplier,
                slowSeconds = slowSeconds, selfDamagePerHit = selfDamagePerHit, noiseRadius = noiseRadius
            };
            if (b == null) return s;
            switch (kind)
            {
                case TrapKind.Spike:
                    s.damagePerHit = b.spikeDamagePerHit;
                    s.hitIntervalSeconds = b.spikeHitIntervalSeconds;
                    s.slowMultiplier = 1f;
                    s.slowSeconds = 0f;
                    s.selfDamagePerHit = b.spikeSelfDamagePerHit;
                    s.noiseRadius = b.spikeNoiseRadius;
                    break;
                case TrapKind.BarbedWire:
                    s.damagePerHit = b.wireDamagePerHit;
                    s.hitIntervalSeconds = b.wireHitIntervalSeconds;
                    s.slowMultiplier = b.wireSlowMultiplier;
                    s.slowSeconds = b.wireSlowSeconds;
                    s.selfDamagePerHit = b.wireSelfDamagePerHit;
                    s.noiseRadius = 0f;
                    break;
                case TrapKind.Campfire:
                    // Api unggun tidak memakai HP sendiri dan tidak membakar lebih cepat.
                    s.damagePerHit = b.campfireDamagePerHit;
                    s.hitIntervalSeconds = b.campfireHitIntervalSeconds;
                    s.slowMultiplier = 1f;
                    s.slowSeconds = 0f;
                    s.selfDamagePerHit = 0f;
                    s.noiseRadius = 0f;
                    break;
            }
            return s;
        }
    }

    /// <summary>Angka efektif satu jebakan (salinan, bukan aset).</summary>
    public struct TrapStats
    {
        public float damagePerHit, hitIntervalSeconds, slowMultiplier, slowSeconds, selfDamagePerHit, noiseRadius;
    }
}
