using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Baris UI notifikasi yang dipakai ulang. Blueprint: Core_Logic/NotificationManager.md (#22).
    /// Bind menimpa teks, warna, ikon, alpha, dan posisi setiap kali dipakai, jadi tidak ada sisa pesan lama.
    /// Lebar tetap; teks pindah baris paling banyak dua kali (tiga baris), dan tinggi baris mengikuti isi.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class NotificationRow : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private Image icon;
        [Tooltip("Garis atau latar yang diberi warna jenis. Boleh kosong.")]
        [SerializeField] private Graphic accent;
        [Tooltip("Ruang atas dan bawah di luar tinggi teks (data prefab).")]
        [SerializeField] private float verticalPadding = 8f;

        private CanvasGroup group;
        private RectTransform rect;
        private bool hasPosition;

        public RectTransform Rect => rect != null ? rect : (rect = (RectTransform)transform);

        private CanvasGroup Group => group != null ? group : (group = GetComponent<CanvasGroup>());

        /// <summary>Tinggi baris sesuai isi teks.</summary>
        public float Height => (label != null ? label.preferredHeight : 0f) + verticalPadding;

        public void Bind(string text, in NotificationStyle style)
        {
            if (label != null)
            {
                label.text = text;
                label.maxVisibleLines = 3;
                label.overflowMode = TextOverflowModes.Ellipsis;
            }
            if (icon != null)
            {
                icon.sprite = style.icon;
                icon.enabled = style.icon != null;
                icon.color = style.color;
            }
            if (accent != null) accent.color = style.color;
            if (label != null) label.color = style.color;
        }

        public void SetText(string text)
        {
            if (label != null) label.text = text;
        }

        public void SetAlpha(float alpha) => Group.alpha = alpha;

        public void SetVisible(bool visible)
        {
            if (gameObject.activeSelf != visible) gameObject.SetActive(visible);
            if (!visible) hasPosition = false;
        }

        /// <summary>Geser menuju y target. Baris yang baru tampil langsung di tempat.</summary>
        public void MoveTowards(float targetY, float maxDelta)
        {
            var p = Rect.anchoredPosition;
            p.y = hasPosition ? Mathf.MoveTowards(p.y, targetY, maxDelta) : targetY;
            hasPosition = true;
            Rect.anchoredPosition = p;
        }
    }
}
