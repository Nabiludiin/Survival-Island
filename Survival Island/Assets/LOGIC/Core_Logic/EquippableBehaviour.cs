using UnityEngine;

namespace SurvivalIsland
{
    public enum ActionPhase { Pressed, Held, Released }

    /// <summary>Kontrak item yang bisa dipegang (EquipmentManager.md, #60).</summary>
    public interface IEquippable
    {
        void OnEquip(ItemInstance instance);  // Item masuk ke tangan, setelah animasi naik mulai
        void OnUnequip();                     // Membatalkan reload, ayunan, dan cast
        void PrimaryAction(ActionPhase phase);
        void SecondaryAction(ActionPhase phase);
        bool CanSwitchNow { get; }            // false pada saat momen damage sedang berjalan
    }

    /// <summary>
    /// Kelas dasar komponen viewmodel item (GunController #58, MeleeCombatSystem, FishingSystem #55, HammerEquippable #79,
    /// TorchController #65, FlashlightController). Fungsi yang tidak dipakai item boleh dibiarkan kosong.
    /// Blueprint: Core_Logic/EquipmentManager.md (#60). Prefab berada di layer PlayerViewmodel tanpa collider.
    /// </summary>
    public class EquippableBehaviour : MonoBehaviour, IEquippable
    {
        [Tooltip("Posisi item di layar (FirstPersonCamera.md, tabel 'Posisi tiap item').")]
        public ViewmodelPose pose = ViewmodelPose.EmptyHands;

        /// <summary>Instance yang sedang dipegang. null untuk tangan kosong.</summary>
        public ItemInstance CurrentInstance { get; private set; }

        public virtual bool CanSwitchNow => true;

        public virtual void OnEquip(ItemInstance instance)
        {
            CurrentInstance = instance;
        }

        public virtual void OnUnequip()
        {
            CurrentInstance = null;
        }

        public virtual void PrimaryAction(ActionPhase phase) { }
        public virtual void SecondaryAction(ActionPhase phase) { }

        /// <summary>Membaca ulang modifier dan skin (#72). Dipanggil EquipmentManager.RefreshEquipped.</summary>
        public virtual void Refresh() { }
    }
}
