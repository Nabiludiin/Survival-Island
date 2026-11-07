using System;
using UnityEngine;

namespace SurvivalIsland
{
    public enum StructureTier { Wood, Stone }

    /// <summary>Satu tahap tampilan rusak (BuildingIntegrity.md "Tambahan pada BuildableData").</summary>
    [Serializable]
    public struct DamageVisualStage
    {
        public float maxHpRatio;       // Tahap ini berlaku kalau HpRatio <= nilai ini
        public Material material;      // Material bersama. Tanpa salinan per objek
        public Mesh mesh;              // Boleh null, artinya memakai mesh asli
        public string enterFxPoolTag;  // Efek saat masuk tahap ini. Boleh kosong
    }

    /// <summary>
    /// Data perbaikan, upgrade, dan tampilan rusak satu jenis struktur. Blueprint: raft/BuildingIntegrity.md (#47),
    /// tabel "Tambahan pada BuildableData" dan "Data bangunan".
    ///
    /// Catatan implementasi (sistem pendukung): BuildableData (kode Langkah 4) bukan partial dan belum boleh diedit, jadi
    /// field tambahan #47 ditaruh di komponen ini pada prefab struktur (BuildableData.prefab). Kalau BuildableData nanti
    /// diberi field ini, For() cukup diganti membaca BuildableData langsung.
    /// GDD bagian 6: gempa merusak base yang dindingnya belum di-upgrade (survivesEarthquake).
    /// </summary>
    [DisallowMultipleComponent]
    public class StructureIntegrityInfo : MonoBehaviour
    {
        public StructureTier tier = StructureTier.Wood;
        [Tooltip("Hasil upgrade. Kosong berarti tidak bisa di-upgrade (lantai, atap, tangga, dan tier batu).")]
        public BuildableData upgradeTo;
        public ItemCost[] upgradeCost = new ItemCost[0];
        public string repairItemId = "Mat_Wood";
        public int repairCostPerSwing = 1;
        [Tooltip("GDD bagian 6: dinding batu lolos dari damage gempa.")]
        public bool survivesEarthquake;
        [Tooltip("Urut dari maxHpRatio terbesar: Utuh 1, Retak 0,66, Hancur sebagian 0,33.")]
        public DamageVisualStage[] damageVisuals = new DamageVisualStage[0];

        /// <summary>Data #47 untuk satu BuildableData, dibaca dari prefab-nya. null kalau prefab belum memasang komponen ini.</summary>
        public static StructureIntegrityInfo For(BuildableData data)
        {
            return data != null && data.prefab != null ? data.prefab.GetComponent<StructureIntegrityInfo>() : null;
        }

        /// <summary>Dipakai DisasterManager (#70) dan RemoteBaseSimulation (#90) saat gempa.</summary>
        public static bool SurvivesEarthquake(BuildableData data)
        {
            var info = For(data);
            return info != null && info.survivesEarthquake;
        }
    }
}
