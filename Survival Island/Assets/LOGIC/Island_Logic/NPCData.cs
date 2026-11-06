using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Skill pasif NPC (NPCRoster.md, #76). GDD bagian 3: NPC punya skill khusus (crafting, medis). Koki usulan disetujui.</summary>
    public enum NPCSkill { None, Medic, Crafter, Cook }

    /// <summary>Perintah pemain lewat tombol E (NPCRoster.md, usulan disetujui developer).</summary>
    public enum NPCCommand { Follow, Wait, ReturnToBase }

    /// <summary>
    /// Data tetap satu NPC. Blueprint: Island_Logic/NPCRoster.md bagian 1 (#76).
    /// Daftar awal (usulan disetujui): npc_medic (HP 80, Medic), npc_crafter (HP 120, Crafter), npc_cook (HP 100, Cook),
    /// semuanya memakai NPC_Pistol. Keputusan developer: semua NPC berberat 4 di kapal (#74), jadi satu NPC memenuhi tangan.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/NPC Data")]
    public class NPCData : ScriptableObject
    {
        [Tooltip("Contoh npc_medic. Sama dengan ID di ShipInteractable dan RunLayout.shipNpcIds.")]
        public string npcId;
        [Tooltip("Kunci teks nama (#104), contoh npc.medic.name.")]
        public string displayNameKey;
        [Tooltip("Berat di kapal (#74). Keputusan developer: semua NPC 4.")]
        public int weight = 4;
        public float maxHP = 100f;
        public NPCSkill skill;
        [Tooltip("null berarti NPC tidak menembak, dan state Attack dilewati.")]
        public WeaponData weapon;
        [Tooltip("Prefab dengan NPCCompanion dan NavMeshAgent. Dibuat sekali per run di IslandPhase.")]
        public GameObject prefab;
        [Tooltip("Ucapan NPC (#100). Boleh kosong.")]
        public BarkSet barkSet;
    }
}
