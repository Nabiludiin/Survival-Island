using System;
using UnityEngine;

namespace SurvivalIsland
{
    public enum BuildCategory { Structure, Furniture, Defense }

    /// <summary>Cell: isi sel. Edge: sisi sel, seperti dinding. WallMount: menempel pada dinding (#98).</summary>
    public enum BuildSlotKind { Cell, Edge, WallMount }

    public enum SupportRule { GroundOrWall, FloorOrGround, WallBelow }

    /// <summary>Cara struktur memengaruhi NavMesh. Rincian di raft/BaseNavigation.md (#78).</summary>
    public enum NavObstacleMode { None, Carve, Avoid }

    /// <summary>Mode palu. Mode awal Repair, supaya klik kiri tidak membuang bahan (usulan disetujui #85).</summary>
    public enum HammerMode { Repair, Build, Upgrade, Demolish }

    [Serializable]
    public struct ItemCost
    {
        public string itemId;
        public int amount;
    }

    /// <summary>
    /// Data satu jenis struktur. Blueprint: raft/BuildMenu.md "BuildableData" (#79). Biaya dan HP awal mengikuti tabel
    /// "Daftar struktur awal" (usulan disetujui developer, #85). Field perbaikan dan upgrade (#47) belum ditambahkan.
    /// resistances memakai DamageResistance milik DamagePipeline (Langkah 5).
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Buildable Data")]
    public class BuildableData : ScriptableObject
    {
        public string id;
        public string nameKey;                  // Kunci teks untuk #104
        public Sprite icon;
        public BuildCategory category;
        public GameObject prefab;
        public ItemCost[] cost;
        public float maxHP = 100f;              // Dibaca BaseHealth (#59)
        public DamageResistance[] resistances;  // Mengikuti DamagePipeline.md
        public Vector3Int gridSize = Vector3Int.one;   // Ukuran dalam sel. Y dalam tingkat
        public BuildSlotKind slotKind;
        public SupportRule support;
        public NavObstacleMode navMode;
        public bool isDemolishable = true;
        public string unlockNodeId;             // Kosong berarti terbuka sejak awal (TechTreeManager)
    }
}
