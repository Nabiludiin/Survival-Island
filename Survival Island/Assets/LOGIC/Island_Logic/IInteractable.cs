using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Kontrak interaksi tombol E, dipakai ShipInteractable (Fase 1), sekoci (LifeboatExtraction), dan IslandInteractable (Fase 2).
    /// Blueprint: Island_Logic/IslandInteractable.md bagian 1 "Bentuk kode" (#17). Tombol E: keputusan #85 (GLOSSARY.md).
    /// Ditulis di Langkah 2, karena Fase 1 adalah pemakai pertama ("Raycast Interactable Item" di TAHAP 7).
    /// </summary>
    public interface IInteractable
    {
        bool IsAvailable { get; }                       // false kalau objek nonaktif atau sudah dipakai
        float HoldSeconds { get; }                      // 0 berarti langsung
        string GetPrompt();                             // Teks aksi, tanpa nama tombol
        InteractBlockReason CanInteract(in InteractContext ctx);
        void OnHoverEnter();
        void OnHoverExit();
        bool Interact(in InteractContext ctx);          // true kalau berhasil
    }

    /// <summary>Data untuk IInteractable. Struct, dibuat PlayerInteractor tanpa alokasi.</summary>
    public readonly struct InteractContext
    {
        public readonly InventoryManager Inventory;
        public readonly Transform Player;
        public readonly bool IsCarryingHeavyItem;

        public InteractContext(InventoryManager inventory, Transform player, bool isCarryingHeavyItem)
        {
            Inventory = inventory;
            Player = player;
            IsCarryingHeavyItem = isCarryingHeavyItem;
        }
    }

    /// <summary>Alasan interaksi ditolak (GLOSSARY.md, pemilik IslandInteractable #17).</summary>
    public enum InteractBlockReason { None, InventoryFull, HandsFull, MissingTool, CarryingHeavyItem, Locked }
}
