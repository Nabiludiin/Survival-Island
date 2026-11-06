using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Kategori ucapan NPC (LoreCollectibles.md bagian 1, #100).</summary>
    public enum BarkCategory { Greeting, Hurt, Hungry, ZombieIncoming }

    /// <summary>Satu baris ucapan. clip boleh kosong: versi awal hanya teks (usulan disetujui #100).</summary>
    [Serializable]
    public struct BarkLine
    {
        public string textKey;   // bark.common.<category>.<n> atau bark.<npcId>.<category>.<n> (#104)
        public AudioClip clip;
    }

    /// <summary>Kumpulan ucapan satu NPC. Blueprint: Island_Logic/LoreCollectibles.md bagian 1 (#100). Dirujuk NPCData.barkSet.</summary>
    [CreateAssetMenu(menuName = "Survival/Bark Set")]
    public class BarkSet : ScriptableObject
    {
        public BarkLine[] greeting = Array.Empty<BarkLine>();
        public BarkLine[] hurt = Array.Empty<BarkLine>();
        public BarkLine[] hungry = Array.Empty<BarkLine>();
        public BarkLine[] zombieIncoming = Array.Empty<BarkLine>();

        public BarkLine[] Get(BarkCategory c)
        {
            switch (c)
            {
                case BarkCategory.Greeting: return greeting;
                case BarkCategory.Hurt: return hurt;
                case BarkCategory.Hungry: return hungry;
                default: return zombieIncoming;
            }
        }
    }
}
