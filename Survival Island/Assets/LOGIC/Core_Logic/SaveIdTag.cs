using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// ID stabil untuk objek yang diletakkan tangan di scene region (pohon, batu, peti loot, dll.).
    /// Format: &lt;region&gt;_&lt;jenis&gt;_&lt;8 karakter&gt;, contoh r1_tree_a3f9c21b. Nama dan posisi objek boleh berubah, ID tetap.
    /// Blueprint: Core_Logic/WorldStateSave.md bagian "ID stabil" (#80).
    ///
    /// Belum ditulis di Langkah 1: deteksi ID bentrok saat objek diduplikasi, dan alat Editor SaveIdAuditWindow.
    /// Keduanya alat Editor yang dipakai saat scene region dibuat (#88).
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class SaveIdTag : MonoBehaviour
    {
        // Hanya dibaca di Editor (OnValidate). Tetap diserialisasi di build supaya susunan data sama.
#pragma warning disable 0414
        [Tooltip("Contoh: r1_tree. Dipakai sebagai awalan ID saat ID dibuat otomatis.")]
        [SerializeField] private string idPrefix = "obj";
#pragma warning restore 0414

        [SerializeField] private string saveId;

        public string SaveId => saveId;

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Isi otomatis dari GUID kalau kosong.
            if (string.IsNullOrEmpty(saveId))
                saveId = idPrefix + "_" + UnityEditor.GUID.Generate().ToString().Substring(0, 8);
        }
#endif
    }
}
