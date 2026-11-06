using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Suku cadang di dunia (prefab di layer dan tag HeavyItem). Tidak masuk inventori, tidak memakai IDamageable, dan bukan
    /// objek pool. Blueprint: Island_Logic/HeavyItemSystem.md bagian "Mengambil" dan "Pengaman barang hilang" (#69).
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class HeavyItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private Collider mainCollider;

        private int waterLayer = -1;

        public int Index { get; private set; } = -1;
        public Rigidbody Body => body;
        public Collider MainCollider => mainCollider;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        private void Awake()
        {
            if (body == null) body = GetComponent<Rigidbody>();
            if (mainCollider == null) mainCollider = GetComponent<Collider>();
            waterLayer = LayerMask.NameToLayer(PhysicsMasks.WaterLayer);
        }

        /// <summary>Dipanggil HeavyItemSystem saat barang dibuat.</summary>
        public void Bind(int index) => Index = index;

        private bool IsLying =>
            HeavyItemSystem.Instance != null && HeavyItemSystem.Instance.GetState(Index) == PartState.Lying;

        // ---------------------------------------------------------------- Interaksi

        public bool IsAvailable => isActiveAndEnabled && IsLying;
        public float HoldSeconds => Balance != null ? Balance.heavyItem.pickupHoldSeconds : 0.5f;
        public string GetPrompt() => "Angkat";

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            // 1.
            if (ctx.IsCarryingHeavyItem) return InteractBlockReason.CarryingHeavyItem;
            // 2. Mengangkat saat jongkok, memanjat, atau berenang ditolak (PlayerTraversal.md).
            var traversal = ctx.Player != null ? ctx.Player.GetComponent<PlayerTraversal>() : null;
            if (traversal != null && traversal.Mode != TraversalMode.Ground) return InteractBlockReason.HandsFull;
            // 3.
            return InteractBlockReason.None;
        }

        public bool Interact(in InteractContext ctx)
        {
            return HeavyItemSystem.Instance != null && HeavyItemSystem.Instance.PickUp(Index);
        }

        public void OnHoverEnter() { }
        public void OnHoverExit() { }

        // ---------------------------------------------------------------- Air

        private void OnTriggerEnter(Collider other)
        {
            // Barang Carried diabaikan (boleh dibawa melewati air dangkal). Air dalam ditangani DropReason.Water.
            if (other.gameObject.layer != waterLayer || !IsLying) return;
            HeavyItemSystem.Instance.ReturnToValid(Index);
        }
    }
}
