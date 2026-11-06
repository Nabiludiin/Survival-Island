using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Jenis node panen (GLOSSARY.md).</summary>
    public enum NodeType { Tree, Rock, MedicalHerb }

    /// <summary>Status node. Node habis tidak dihancurkan, hanya berganti status (ResourceRespawn.md).</summary>
    public enum NodeStatus { Standing, Depleted }

    /// <summary>Hasil ReceiveHit. MeleeCombatSystem mengurangi durabilitas hanya saat Hit (GLOSSARY.md, #73).</summary>
    public enum HitResult { Hit, Rejected }

    /// <summary>Damage satu alat ke node. 0 berarti ditolak, tanpa pengurangan durabilitas.</summary>
    [Serializable]
    public struct ToolRule
    {
        public ToolClass tool;
        public int damage;
    }

    /// <summary>Lama tumbuh kembali di satu region, dalam hari game.</summary>
    [Serializable]
    public struct RegrowRule
    {
        public RegionType region;
        public float days;
    }

    /// <summary>Objek yang habis lalu tumbuh kembali berdasarkan jam absolut (ResourceRespawn.md).</summary>
    public interface IRegrowable
    {
        bool IsDepleted { get; }
        float RegrowAtGameHour { get; }
        Vector3 Position { get; }
        void Regrow();
    }

    /// <summary>
    /// Data satu jenis node: pohon, batu besar, jamur, atau pohon getah. Blueprint: raft/ResourceRespawn.md bagian 1 (#73).
    /// Tabel alat dan jadwal tumbuh adalah usulan yang disetujui developer sebagai nilai awal.
    ///
    /// Catatan implementasi (Langkah 4): untuk Tree dan Rock, jumlah drop dibaca dari GameBalanceConfig.resource
    /// (treeDropMin/Max, rockDropMin/Max) karena angka itu tercatat di config. dropMin/dropMax di aset dipakai MedicalHerb.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Resource Node")]
    public class ResourceNodeData : ScriptableObject
    {
        public string nodeTypeId;        // Contoh: Node_Pine
        public NodeType nodeType;
        public int maxHits = 5;
        public ToolRule[] toolRules;
        public ItemData dropItem;
        public int dropMin = 1, dropMax = 2;
        public bool keepColliderWhenDepleted = true;
        public RegrowRule[] regrow;      // Hari per region. Region tanpa aturan: node tidak tumbuh

        /// <summary>Damage alat ini, atau 0 kalau ditolak.</summary>
        public int GetDamage(ToolClass tool)
        {
            if (toolRules == null) return 0;
            for (int i = 0; i < toolRules.Length; i++)
                if (toolRules[i].tool == tool) return Mathf.Max(0, toolRules[i].damage);
            return 0;
        }

        /// <summary>Hari tumbuh di region ini. false kalau tidak ada aturan.</summary>
        public bool TryGetRegrowDays(RegionType region, out float days)
        {
            if (regrow != null)
            {
                for (int i = 0; i < regrow.Length; i++)
                {
                    if (regrow[i].region != region) continue;
                    days = regrow[i].days;
                    return true;
                }
            }
            days = -1f;
            return false;
        }

        /// <summary>Tag efek pukulan per jenis (#15).</summary>
        public string HitVfxTag
        {
            get
            {
                switch (nodeType)
                {
                    case NodeType.Tree: return "VFX_WoodDust";
                    case NodeType.Rock: return "VFX_StoneDust";
                    default: return "VFX_LeafBurst";
                }
            }
        }
    }
}
