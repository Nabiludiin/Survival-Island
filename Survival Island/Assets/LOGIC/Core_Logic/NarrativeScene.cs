using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu adegan cerita sebagai data, jadi menambah adegan tidak mengubah kode.
    /// Blueprint: Core_Logic/NarrativeSequencer.md (#99). Usulan 1 disetujui developer: versi awal berupa rangkaian
    /// teks dan gambar; Timeline bisa menyusul di balik fungsi publik NarrativeSequencer yang sama.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Narrative Scene")]
    public class NarrativeScene : ScriptableObject
    {
        public string sceneId;
        public NarrativeBeat[] beats = new NarrativeBeat[0];
        public AudioClip music;
        [Tooltip("true untuk adegan di tengah permainan (helipad_found): dunia, jam, dan fisika berhenti.")]
        public bool freezeWorld;
        [Tooltip("Boleh dilewati walau belum pernah ditonton.")]
        public bool alwaysSkippable;
    }

    /// <summary>Satu gambar dan teks dalam adegan.</summary>
    [Serializable]
    public class NarrativeBeat
    {
        public Sprite image;           // Boleh null untuk layar hitam
        public string textKey;         // Teks layar, lewat LocalizationManager (#104)
        public string subtitleKey;     // Subtitle untuk voice, lewat SubtitleRequestedEvent
        public AudioClip voice;
        public AudioClip sfx;
        [Tooltip("0 berarti mengikuti panjang voice. Kalau voice juga kosong, dipakai narrative.beatFadeSeconds.")]
        public float seconds;
    }
}
