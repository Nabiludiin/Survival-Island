using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Ikon status pada hologram bangunan: centang saat valid, silang dan teks build.error.&lt;PlacementError&gt; saat ditolak.
    /// Blueprint: Core_Logic/LocalizationManager.md bagian "Penanda selain warna" (#104, #79).
    /// Satu Canvas world-space pada hologram. Hanya diganti saat PlacementError berubah. Pola ini selalu aktif.
    /// </summary>
    public class GhostStatusIcon : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Sprite validSprite;     // Centang
        [SerializeField] private Sprite invalidSprite;   // Silang
        [SerializeField] private TMP_Text reasonText;    // Boleh kosong kalau alasan tampil di HUD

        private PlacementError shown = (PlacementError)(-1);

        private void OnEnable()
        {
            EventChannel<LocaleChangedEvent>.Raised += HandleLocale;
        }

        private void OnDisable()
        {
            EventChannel<LocaleChangedEvent>.Raised -= HandleLocale;
        }

        /// <summary>Dipanggil BuildManager tiap frame; isi hanya berubah saat error berbeda.</summary>
        public void SetStatus(PlacementError error)
        {
            if (error == shown) return;
            shown = error;
            Apply();
        }

        private void Apply()
        {
            bool valid = shown == PlacementError.None;
            if (icon != null) icon.sprite = valid ? validSprite : invalidSprite;
            if (reasonText != null)
            {
                var loc = LocalizationManager.Instance;
                reasonText.text = valid || loc == null ? string.Empty : loc.Get("build.error." + shown);
            }
        }

        private void HandleLocale(LocaleChangedEvent e)
        {
            if ((int)shown >= 0) Apply();
        }
    }
}
