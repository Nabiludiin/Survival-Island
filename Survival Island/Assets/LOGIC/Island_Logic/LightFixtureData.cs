using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Data bahan bakar struktur cahaya: Build_WallTorch (Obor Dinding) dan Build_Lamp (Lampu).
    /// Blueprint: Island_Logic/LightSourceSystem.md tabel "Struktur baru" (#98). Angka usulan diisi di aset.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Light Fixture Data")]
    public class LightFixtureData : ScriptableObject
    {
        public string buildableId = "Build_WallTorch";
        [Tooltip("Mat_Wood untuk Obor Dinding, Item_Battery untuk Lampu.")]
        public string fuelItemId = "Mat_Wood";
        [Tooltip("Detik setara per item: 300 (obor), 1.200 (lampu).")]
        public float secondsPerItem = 300f;
        [Tooltip("Kapasitas detik setara: 900 (obor), 1.200 (lampu).")]
        public float capacitySeconds = 900f;
        [Tooltip("true untuk Lampu: tidak boros saat Rain dan menyala lagi otomatis setelah badai.")]
        public bool isLamp;
        [Tooltip("Kunci teks prompt saat bahan bakar kosong: Butuh Kayu / Butuh Baterai.")]
        public string needFuelKey = "prompt.need_wood";
    }
}
