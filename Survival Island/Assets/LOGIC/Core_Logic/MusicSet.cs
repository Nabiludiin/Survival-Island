using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Stem musik Fase 2. Blueprint: Core_Logic/CombatMusicDirector.md bagian 1 (#114).
    /// Usulan disetujui developer: empat stem selaras, jadi semua klip harus punya tempo dan panjang sama.
    /// calmDay dan calmNight menggantikan dayBGM dan nightBGM lama; combatStem menggantikan dangerBGM.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Music Set")]
    public class MusicSet : ScriptableObject
    {
        public AudioClip calmDay;
        public AudioClip calmNight;
        public AudioClip alertStem;
        public AudioClip combatStem;
        public AudioClip peakStem;
    }
}
