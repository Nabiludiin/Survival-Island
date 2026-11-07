using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Jenis syarat selesai satu tujuan (ObjectiveManager.md bagian 1, #81).</summary>
    public enum ObjectiveCondition { BuildCount, ItemTotal, BridgeBuilt, ItemCrafted, RegionEntered, PartsInstalled, Escaped }

    /// <summary>
    /// Satu tujuan bertahap di HUD. Blueprint: Island_Logic/ObjectiveManager.md bagian 1 dan tabel "Daftar tujuan" (#81).
    /// GDD bagian 1 dan 3 (syarat menang) dan bagian 5 (urutan region dan logistik). Delapan aset awal:
    /// obj_base, obj_wood, obj_bridge, obj_vaccine, obj_region4, obj_region3, obj_parts, obj_takeoff.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Objective Data")]
    public class ObjectiveData : ScriptableObject
    {
        public string objectiveId;
        [Tooltip("Kunci teks judul HUD (#104), contoh objective.obj_base.title.")]
        public string title;
        public int order;
        [Tooltip("0 berarti terlihat sejak awal. Usulan disetujui 3: tujuan tersembunyi sebelum babnya terbuka.")]
        public int revealedByChapter;
        public ObjectiveCondition condition;
        [Tooltip("Item (ItemTotal), resep (ItemCrafted), atau awalan buildableId dinding (BuildCount, nilai awal Wall_).")]
        public string targetId;
        public RegionType targetRegion;
        [Tooltip("0 pada ItemTotal berarti region.bridgeWoodCost dibaca dari GameBalanceConfig saat dihitung.")]
        public int targetAmount;
        public Vector3 markerPosition;
        public bool hasMarker;
    }
}
