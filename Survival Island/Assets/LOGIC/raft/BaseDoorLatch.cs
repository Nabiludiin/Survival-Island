using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Zona gerendel pintu (depan dan belakang). Blueprint: raft/BaseNavigation.md bagian "Pintu" (#78).
    /// Usulan disetujui developer (#85): E pada Closed mengunci, E pada Locked membuka kunci. Tidak tersedia saat Open.
    /// Dipasang pada collider zona gerendel (layer Interactable, non-trigger), anak prefab Door_Wood.
    /// </summary>
    public class BaseDoorLatch : MonoBehaviour, IInteractable
    {
        [SerializeField] private BaseDoor door;
        [SerializeField] private string lockPrompt = "Kunci";
        [SerializeField] private string unlockPrompt = "Buka kunci";

        private void Awake()
        {
            if (door == null) door = GetComponentInParent<BaseDoor>();
        }

        public bool IsAvailable => door != null && door.IsAvailable && door.State != DoorState.Open;
        public float HoldSeconds => 0f;
        public string GetPrompt() => door != null && door.State == DoorState.Locked ? unlockPrompt : lockPrompt;
        public InteractBlockReason CanInteract(in InteractContext ctx) => InteractBlockReason.None;
        public void OnHoverEnter() { }
        public void OnHoverExit() { }
        public bool Interact(in InteractContext ctx) => door != null && door.ToggleLock();
    }
}
