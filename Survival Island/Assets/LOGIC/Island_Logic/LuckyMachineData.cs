using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Jenis hadiah Lucky Machine (LuckyMachine.md bagian 1).</summary>
    public enum LuckyRewardKind { WeaponModifier, Skin, PermanentBuff, SpawnItem }

    /// <summary>Tier hadiah. Peluang S/A/B dari GameBalanceConfig.luckyMachine.tierWeight*.</summary>
    public enum LuckyTier { B, A, S }

    /// <summary>Status gerbang kandang.</summary>
    public enum CageGateState { Open, Closed }

    /// <summary>Satu baris kuota tumbal. Tiap baris dihitung terpisah dan memberi 1 putaran.</summary>
    [Serializable]
    public class SacrificeQuota
    {
        public string enemyId;   // EnemyData.enemyId
        public int required = 5; // Jumlah untuk 1 putaran
    }

    /// <summary>
    /// Data satu mesin: region, kuota musuh, dan tabel hadiah. Satu aset per mesin.
    /// Blueprint: Island_Logic/LuckyMachine.md bagian 1 (#72). GDD bagian 7: satu mesin di setiap region,
    /// contoh kuota 5 Zombie Militer atau 3 Beruang Kutub (mesin Region 3).
    /// Kuota Region 1, 2, dan 4 serta tabel hadiah bersama disetujui developer sebagai nilai awal.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Lucky Machine Data")]
    public class LuckyMachineData : ScriptableObject
    {
        public RegionType region;
        public SacrificeQuota[] quotas = Array.Empty<SacrificeQuota>();

        [Tooltip("Keempat mesin memakai daftar yang sama sebagai nilai awal (keputusan 11).")]
        public LuckyRewardData[] rewards = Array.Empty<LuckyRewardData>();

        /// <summary>Indeks baris kuota untuk enemyId, atau -1 kalau musuh ini bukan tumbal mesin ini.</summary>
        public int IndexOfQuota(string enemyId)
        {
            if (string.IsNullOrEmpty(enemyId) || quotas == null) return -1;
            for (int i = 0; i < quotas.Length; i++)
                if (quotas[i] != null && quotas[i].enemyId == enemyId) return i;
            return -1;
        }
    }
}
