using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Data suara penanda satu varian musuh atau hewan (tabel "Suara penanda per varian dan hewan").
    /// Blueprint: Core_Logic/CombatMusicDirector.md bagian 1-2 (#114). GDD bagian 3 dan 5: jenis ancaman per region.
    /// Usulan: pemain bisa mendengar jenis ancaman dan arahnya.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Enemy Cue Set")]
    public class EnemyCueSet : ScriptableObject
    {
        public AudioClip[] idleClips = new AudioClip[0];
        public AudioClip[] alertClips = new AudioClip[0];
        public float idleMinInterval = 5f;
        public float idleMaxInterval = 12f;
        public float audibleRadius = 25f;
        [Tooltip("Nilai besar lebih sulit digantikan saat batas suara penuh.")]
        public int priority = 1;
        [Tooltip("Bobot besar saat menghitung ancaman dekat (Beruang Kutub, Zombie Militer).")]
        public bool isLarge;
    }
}
