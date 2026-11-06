namespace SurvivalIsland
{
    /// <summary>
    /// Makanan, minuman, atau obat di hotbar: klik kiri memakai lewat ItemUseSystem (#66), klik kanan membatalkan.
    /// Blueprint: Core_Logic/EquipmentManager.md, tabel "Daftar item yang bisa dipegang" (usulan disetujui #85 poin 6).
    /// </summary>
    public class ConsumableEquippable : EquippableBehaviour
    {
        public override void PrimaryAction(ActionPhase phase)
        {
            if (phase != ActionPhase.Pressed || CurrentInstance == null) return;
            var use = ItemUseSystem.Instance;
            var inv = InventoryManager.Instance;
            if (use == null || inv == null || use.IsUsing) return;
            int slot = inv.IndexOfInstance(CurrentInstance.instanceId);
            if (slot >= 0) use.TryUse(slot);
        }

        public override void SecondaryAction(ActionPhase phase)
        {
            if (phase == ActionPhase.Pressed && ItemUseSystem.Instance != null) ItemUseSystem.Instance.CancelUse("secondary");
        }

        public override void OnUnequip()
        {
            // Pemakaian konsumsi batal tanpa mengurangi item (EquipmentManager.md, tabel pembatalan).
            if (ItemUseSystem.Instance != null) ItemUseSystem.Instance.CancelUse("unequip");
            base.OnUnequip();
        }
    }
}
