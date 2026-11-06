using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Panel baca satu temuan lore (UIPanelId.LoreReader). Blueprint: Island_Logic/LoreCollectibles.md bagian "Membaca ulang"
    /// (#100, pengaturan panel #83). Panel ini menjeda game lewat pengaturan di Inspector (pausesTime), bukan di sini.
    /// Note dan Board: teks dengan ScrollRect. Recording: transkrip tampil dan recordingClip diputar pada AudioSource dengan
    /// ignoreListenerPause, jadi tetap bunyi saat jeda. Menutup panel menghentikan suara.
    /// </summary>
    public class LoreReaderPanel : FadingUIPanel
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text bodyText;
        [SerializeField] private ScrollRect scroll;
        [Tooltip("Penanda rekaman, aktif hanya untuk Recording.")]
        [SerializeField] private GameObject recordingIndicator;
        [Tooltip("Sumber suara rekaman. Keluaran ke grup SFX di AudioMixer.")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private Button closeButton;

        private LoreEntry entry;
        private bool listenerAdded;

        private void Awake()
        {
            if (sfxSource != null) sfxSource.ignoreListenerPause = true;
            AddListener();
        }

        private void AddListener()
        {
            if (listenerAdded) return;
            listenerAdded = true;
            if (closeButton != null) closeButton.onClick.AddListener(HandleCloseClicked);
        }

        /// <summary>Isi panel sebelum Push. Dipanggil LoreCollectibles.OpenReader.</summary>
        public void SetEntry(LoreEntry e)
        {
            AddListener();
            entry = e;
            Refresh();
        }

        public override void OnFocusGained()
        {
            Refresh();
            PlayRecording();
        }

        public override void OnClosed(UICloseReason reason)
        {
            if (sfxSource != null) sfxSource.Stop();
        }

        private void Refresh()
        {
            if (entry == null) return;
            var loc = LocalizationManager.Instance;
            if (titleText != null) titleText.text = loc != null ? loc.Get(entry.TitleKey) : entry.TitleKey;
            if (bodyText != null) bodyText.text = loc != null ? loc.Get(entry.BodyKey) : entry.BodyKey;
            if (scroll != null) scroll.verticalNormalizedPosition = 1f;
            if (recordingIndicator != null) recordingIndicator.SetActive(entry.kind == LoreKind.Recording && entry.recordingClip != null);
        }

        private void PlayRecording()
        {
            if (sfxSource == null || entry == null || entry.kind != LoreKind.Recording || entry.recordingClip == null) return;
            if (sfxSource.isPlaying && sfxSource.clip == entry.recordingClip) return;
            sfxSource.clip = entry.recordingClip;
            sfxSource.ignoreListenerPause = true;
            sfxSource.Play();
        }

        private void HandleCloseClicked()
        {
            if (UIFocusStack.Instance != null) UIFocusStack.Instance.Close(this, UICloseReason.User);
        }
    }
}
