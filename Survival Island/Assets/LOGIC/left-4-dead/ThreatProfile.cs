using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Bobot pemilihan target per jenis musuh. Blueprint: left-4-dead/ThreatSystem.md bagian "ThreatProfile" (#108).
    /// Aset Threat_Zombie dan Threat_Animal. Rumus dan bobot adalah usulan yang disetujui developer.
    /// </summary>
    [CreateAssetMenu(fileName = "Threat_Zombie", menuName = "Survival/Threat Profile")]
    public class ThreatProfile : ScriptableObject
    {
        public bool allowPlayer = true;
        public bool allowNpc = true;
        public bool allowBaseAnchor = true;
        public bool allowLure = true;
        public float kindWeightPlayer = 40f;
        public float kindWeightNpc = 30f;
        public float distanceWeight = 30f;
        public float visibleBonus = 25f;
        public float recentDamageBonus = 30f;
        [Tooltip("Jarak terjauh kandidat. Hewan: 0 berarti memakai AnimalData.aggroRadius.")]
        public float senseRange = 60f;
        public bool huntsUnseen = true;
        public bool reactsToLure = true;

        public bool Allows(TargetKind kind)
        {
            switch (kind)
            {
                case TargetKind.Player: return allowPlayer;
                case TargetKind.NPC: return allowNpc;
                case TargetKind.BaseAnchor: return allowBaseAnchor;
                default: return allowLure;
            }
        }

        public float KindWeight(TargetKind kind) => kind == TargetKind.NPC ? kindWeightNpc : kindWeightPlayer;
    }
}
