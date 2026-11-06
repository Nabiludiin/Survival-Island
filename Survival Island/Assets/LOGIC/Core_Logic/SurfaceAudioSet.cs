using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Klip langkah dan mendarat per SurfaceType. Blueprint: Core_Logic/FootstepAudio.md bagian 1 (#21).
    /// Satu baris per SurfaceType (Kayu, Logam, Pasir, Rumput, Tanah, Batu, Salju, Lumpur). Usulan teknis.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Surface Audio Set")]
    public class SurfaceAudioSet : ScriptableObject
    {
        public SurfaceAudioEntry[] entries = new SurfaceAudioEntry[0];
    }

    [Serializable]
    public struct SurfaceAudioEntry
    {
        public SurfaceType type;
        [Tooltip("Minimal 3 klip, supaya tidak monoton.")]
        public AudioClip[] steps;
        [Tooltip("Boleh kosong, lalu memakai steps.")]
        public AudioClip[] landings;
        [Tooltip("Nilai awal 1.")]
        public float volume;
    }
}
