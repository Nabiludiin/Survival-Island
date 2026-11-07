using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Isi suara latar satu area (ambient_pantai, ambient_hutan, ambient_volcano, ...).
    /// Blueprint: Core_Logic/AmbientAudio.md bagian 1 (#52). Pemetaan suara per area adalah usulan yang disetujui developer.
    /// GDD bagian 5 (suasana tiap region) dan bagian 6 (pantai), secara tidak langsung.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Ambient Profile")]
    public class AmbientProfile : ScriptableObject
    {
        /// <summary>Jumlah loop maksimal per profil (jumlah AudioSource per deck).</summary>
        public const int MaxLayers = 3;

        public string profileId;          // ambient_pantai, ambient_hutan, ...
        [Tooltip("Nilai besar menang saat area bertumpuk. Pantai 20, hutan 10, region 0.")]
        public int priority;
        [Tooltip("Maksimal 3 loop.")]
        public AmbientLayer[] layers = new AmbientLayer[0];
        [Tooltip("Suara acak 3D, misalnya camar.")]
        public AmbientOneShot[] oneShots = new AmbientOneShot[0];

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (layers != null && layers.Length > MaxLayers)
                Debug.LogWarning($"[AmbientProfile] '{name}' punya {layers.Length} layer. Hanya {MaxLayers} pertama yang diputar.", this);
        }
#endif
    }

    /// <summary>Satu loop suara latar.</summary>
    [Serializable]
    public class AmbientLayer
    {
        public AudioClip clip;
        public float dayVolume = 1f;
        public float nightVolume = 1f;
        [Tooltip("Volume mengikuti TideChangedEvent.level.")]
        public bool followsTide;
        [Tooltip("Hilang saat Rain, Storm, atau Blizzard.")]
        public bool mutedByBadWeather;
        [Tooltip("Hilang saat bencana dan Blood Moon.")]
        public bool mutedByThreatEvent;
    }

    /// <summary>Suara acak yang muncul di sekitar pemain.</summary>
    [Serializable]
    public class AmbientOneShot
    {
        public AudioClip[] clips = new AudioClip[0];
        public float minInterval = 8f;
        public float maxInterval = 20f;
        [Tooltip("Jarak munculnya suara dari pemain.")]
        public float radius = 25f;
        public bool dayOnly;
    }
}
