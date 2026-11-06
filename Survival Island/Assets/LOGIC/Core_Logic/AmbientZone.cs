using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Trigger area suara latar (BoxCollider isTrigger di layer ZoneTrigger, di scene IslandPhase, bukan scene region #88).
    /// Blueprint: Core_Logic/AmbientAudio.md bagian 2 "Trigger area" (#52).
    /// Satu area boleh terdiri dari banyak BoxCollider dengan profil sama; AmbientAudio menghitung per profil.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class AmbientZone : MonoBehaviour
    {
        public AmbientProfile profile;

        private int playerInside;   // Jumlah collider pemain yang sedang di dalam (biasanya 0 atau 1)

        private void OnTriggerEnter(Collider other)
        {
            if (profile == null || !other.CompareTag("Player")) return;
            playerInside++;
            if (playerInside == 1 && AmbientAudio.Instance != null) AmbientAudio.Instance.EnterZone(profile);
        }

        private void OnTriggerExit(Collider other)
        {
            if (profile == null || !other.CompareTag("Player") || playerInside <= 0) return;
            playerInside--;
            if (playerInside == 0 && AmbientAudio.Instance != null) AmbientAudio.Instance.ExitZone(profile);
        }

        // Unity tidak mengirim OnTriggerExit saat trigger dimatikan (AmbientAudio.md bagian 2).
        private void OnDisable()
        {
            if (playerInside > 0 && profile != null && AmbientAudio.Instance != null) AmbientAudio.Instance.ExitZone(profile);
            playerInside = 0;
        }
    }
}
