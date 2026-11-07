using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Data satu jenis bencana: peluang harian, cooldown, hari minimum, skala, dan durasi tiap tahap.
    /// Blueprint: Island_Logic/DisasterManager.md bagian 1 (#70). Nilai awal ada di tabel "Nilai awal DisasterData".
    /// Dibaca DisasterScheduler (jadwal dari seed) dan DisasterManager (durasi tahap).
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Disaster Data")]
    public class DisasterData : ScriptableObject
    {
        public DisasterType type;

        [Tooltip("Peluang bencana ini muncul pada satu hari yang memenuhi syarat.")]
        [Range(0f, 1f)] public float dailyChance = 0.05f;
        public int cooldownDays = 6;
        public int minDay = 2;

        [Tooltip("Skala minimum yang dianggap bencana besar. 1 berarti selalu besar (Erupsi).")]
        public int majorMinScale = 2;
        [Range(0f, 1f)] public float scale2Chance = 0.3f;

        public float warningSeconds = 20f;
        public float activeSeconds = 120f;
        public float activeSecondsScale2 = 180f;
        public float aftermathSeconds = 30f;

        public float startHourMin = 8f;
        public float startHourMax = 14f;

        public AudioClip warningClip;
        public AudioClip activeLoop;
        [Tooltip("Gempa: suara gemuruh tiap denyut (DisasterManager.md \"Gempa\" langkah 1). Boleh kosong.")]
        public AudioClip pulseClip;

        public bool IsMajor(int scale) => scale >= majorMinScale;

        /// <summary>
        /// Durasi Active. Bencana besar selalu minimal freeWindowSeconds, supaya mode gratis Lucky Machine (30 detik, GDD bagian 7)
        /// tidak lebih panjang dari bencananya (DisasterManager.md bagian 2 "API").
        /// </summary>
        public float GetActiveSeconds(int scale, float minMajorSeconds)
        {
            float s = scale >= 2 ? activeSecondsScale2 : activeSeconds;
            return IsMajor(scale) ? Mathf.Max(s, minMajorSeconds) : s;
        }
    }
}
