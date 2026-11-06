using System;

namespace SurvivalIsland
{
    /// <summary>Tingkat kesegaran makanan (ItemUseSystem.md, #66). Nilai mengikuti GLOSSARY.md.</summary>
    public enum Freshness { Fresh, Stale, Rotten }

    /// <summary>
    /// Data satu item di slot, peti, atau dunia. Blueprint: Core_Logic/ItemUseSystem.md bagian 1 "ItemInstance" (#66).
    /// Isi field mengikuti GLOSSARY.md. Disimpan utuh di save (#80).
    /// </summary>
    [Serializable]
    public class ItemInstance
    {
        public int instanceId;          // Unik dalam satu run, dari InventoryManager.nextInstanceId
        public string itemId;
        public float durability = -1f;  // -1 kalau tidak dipakai
        public int magazineAmmo = -1;   // -1 kalau bukan senjata api (#58)
        public float createdGameHour = -1f; // Jam game total saat dibuat. -1 kalau tidak busuk
        public int upgradeMask;         // Dicadangkan untuk #113
        public int repairCount;         // #113
        public string luckyModifierId;  // #113, #72
        public string skinId;           // #72

        public ItemInstance Clone()
        {
            return (ItemInstance)MemberwiseClone();
        }
    }

    /// <summary>Satu slot inventori atau peti. instance null berarti kosong (ItemUseSystem.md).</summary>
    [Serializable]
    public class InventorySlot
    {
        public ItemInstance instance;
        public int amount;

        public bool IsEmpty => instance == null || amount <= 0;

        public void Clear()
        {
            instance = null;
            amount = 0;
        }
    }
}
