namespace SurvivalIsland
{
    /// <summary>
    /// Tangan kosong (EquipmentManager.md, usulan disetujui #85 poin 7: klik kiri menjadi pukulan lemah).
    /// Pukulan memakai jalur MeleeCombatSystem (stamina, raycast Melee, damage lewat DamageRouter).
    /// Keputusan developer (sistem pendukung): damage dasar pukulan = equipment.fistDamage (nilai awal 5).
    /// Tanpa item, jadi tidak ada keausan, dan ResourceNode menolak pukulan tanpa alat.
    /// </summary>
    public class FistBehaviour : MeleeCombatSystem
    {
        protected override float GetBaseDamage(ItemData item)
        {
            var bal = GameManager.CurrentBalance;
            return bal != null ? bal.equipment.fistDamage : base.GetBaseDamage(item);
        }
    }
}
