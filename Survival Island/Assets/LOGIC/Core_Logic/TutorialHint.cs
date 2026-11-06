using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Data satu pop-up panduan. Aset disimpan di Resources/TutorialHints. Blueprint: Core_Logic/TutorialManager.md (#16).
    /// Daftar pop-up adalah usulan yang disetujui developer sebagai nilai awal; bisa dikurangi tanpa mengubah kode.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Tutorial Hint")]
    public class TutorialHint : ScriptableObject
    {
        public string hintId;              // Contoh: "first_inventory"
        public string textKey;             // Kunci LocalizationManager (#104)
        [TextArea] public string fallbackText;   // Dipakai kalau kunci tidak ada
        public int priority;               // Makin besar makin dahulu
        public bool allowDuringDanger;     // Boleh tampil saat pemain baru terkena serangan
    }
}
