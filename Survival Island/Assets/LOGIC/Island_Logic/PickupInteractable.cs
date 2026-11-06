using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Benda kecil yang langsung diambil: batu kecil, ranting, dan buah. Blueprint: Island_Logic/IslandInteractable.md
    /// bagian "Objek milik issue ini" (#17). GDD bagian 3: looting di pulau. Usulan disetujui 2: benda kecil langsung terambil.
    /// Batu kecil adalah sumber batu pertama tanpa alat (ProgressionSafety.md, #116). Jadwal tumbuh lagi diatur #73.
    /// </summary>
    public class PickupInteractable : IslandInteractable
    {
        [SerializeField] private ItemData item;
        [Min(1)] [SerializeField] private int amount = 1;

        private int startAmount;

        public ItemData Item => item;
        public int Amount => amount;

        protected override void Awake()
        {
            startAmount = amount;
            base.Awake();
        }

        public override bool IsAvailable => base.IsAvailable && item != null && amount > 0;

        protected override InteractBlockReason CanInteractExtra(in InteractContext ctx)
        {
            // Pesan sama dengan WorldItem: inventori penuh kalau satu pun tidak muat.
            var inv = ctx.Inventory != null ? ctx.Inventory : InventoryManager.Instance;
            if (inv == null) return InteractBlockReason.Locked;
            return inv.CanFitAfterRemoval(null, item, 1) ? InteractBlockReason.None : InteractBlockReason.InventoryFull;
        }

        protected override bool OnInteract(in InteractContext ctx)
        {
            var inv = ctx.Inventory != null ? ctx.Inventory : InventoryManager.Instance;
            if (inv == null) return false;
            int left = inv.AddItem(item, amount);
            if (left > 0)
            {
                // Ada sisa: objek tetap ada dengan jumlah sisa.
                bool tookSome = left < amount;
                amount = left;
                if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.inventory_full", NotificationKind.Warning);
                return tookSome;
            }
            amount = 0;
            Consume();
            return true;
        }

        /// <summary>
        /// Dipakai ResourceRespawn (#73) saat batu kecil tumbuh lagi, termasuk ForceRegrowStones (#116).
        /// Objek kembali aktif dengan jumlah awal.
        /// </summary>
        public void Regrow()
        {
            isConsumed = false;
            amount = startAmount;
            gameObject.SetActive(true);
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        /// <summary>DevConsole clearstones (ProgressionSafety.md, #56): tandai sebagai diambil tanpa masuk inventori.</summary>
        public void DebugConsume()
        {
            if (isConsumed) return;
            amount = 0;
            Consume();
        }
#endif

        protected override int SavedAmount => amount;

        protected override void ApplySavedAmount(int saved) => amount = Mathf.Max(0, saved);
    }
}
