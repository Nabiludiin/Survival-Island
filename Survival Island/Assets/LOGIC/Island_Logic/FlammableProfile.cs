using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Jenis bahan bakar. Menentukan aturan batas lompatan dan "Akhir bakar" (FireSystem.md).</summary>
    public enum FlammableKind { WoodStructure, Stone, Vegetation, Zombie, Animal, Barrel }

    /// <summary>
    /// Ketahanan, durasi, dan peluang menjalar per material. Aset: Flame_WoodStructure, Flame_Stone, Flame_Vegetation,
    /// Flame_Zombie, Flame_Animal, Flame_Barrel. Blueprint: Island_Logic/FireSystem.md bagian "Data FlammableProfile" (#97).
    /// Angka di tabel blueprint adalah usulan dan diisi di aset, bukan di kode.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Flammable Profile")]
    public class FlammableProfile : ScriptableObject
    {
        public FlammableKind kind = FlammableKind.WoodStructure;
        [Tooltip("1 berarti kebal (Flame_Stone).")]
        [Range(0f, 1f)] public float fireResistance = 0.3f;
        public float burnSeconds = 30f;
        public float burnDamagePerSec = 6f;
        [Range(0f, 1f)] public float spreadChancePerTick = 0.05f;
        public float spreadRadius = 3f;

        public bool IsImmune => fireResistance >= 1f;
    }
}
