using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Jenis item. Nilai mengikuti GLOSSARY.md.</summary>
    public enum ItemType { Material, Consumable, Weapon, Tool, KeyItem }

    /// <summary>Pemakaian tangan (EquipmentManager.md, #60).</summary>
    public enum HandUse { OneHand, TwoHand, OffHand }

    /// <summary>Kelas alat panen (ItemData.md, #73). Nilai mengikuti GLOSSARY.md.</summary>
    public enum ToolClass { None, Axe, Pickaxe, Knife }

    /// <summary>
    /// Cetakan data satu jenis item, tanpa logika. Blueprint: Core_Logic/ItemData.md (#66, #58, #60, #65, #73, #104, #118).
    /// GDD bagian 3: item inti Base_Kail, Base_Pistol, Base_Pisau, makanan, dan obat. Field tambahan adalah usulan teknis.
    ///
    /// Field weaponData (WeaponData milik GunController #58) ditambahkan Langkah 5; aset lama tetap aman karena
    /// ScriptableObject menerima field baru.
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "Survival/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Identitas")]
        public string itemID;
        [Tooltip("Kunci teks LocalizationManager (#104). Kosong = item.<itemID>.name")]
        public string displayNameKey;
        public Sprite icon;
        public GameObject prefab;
        public ItemType type = ItemType.Material;

        [Header("Pakai (#66)")]
        public float healAmount;
        public float weaponDamage;                 // Hanya senjata melee. Senjata api memakai weaponData (#58)
        public WeaponData weaponData;              // Ditambahkan Langkah 5 (GunController.md, #58). null = bukan senjata api
        public int maxStack = 1;
        public float hungerDelta;
        public float thirstDelta;
        public float infectionDelta;               // Negatif mengobati (#64)
        public float useSeconds;
        public float restoreSeconds;               // 0 = langsung
        public bool isConsumable;
        public bool isDroppable = true;
        public bool isCoreItem;                    // GLOSSARY: item inti tidak bisa dibuang
        public float maxDurability = -1f;          // -1 = tidak memakai durabilitas
        public float spoilHours;                   // Jam game sampai busuk. 0 = tidak busuk
        [Range(0f, 1f)] public float poisonChance;
        public DiseaseCure curesDiseases = DiseaseCure.None;    // #45
        public float fuelSeconds;                  // #44
        public string appliesEffectId;             // #20

        [Header("Perbaikan (#113)")]
        public bool isRepairable;
        public RecipeIngredient[] repairCost = new RecipeIngredient[0];
        public CraftingStationType repairStation = CraftingStationType.WoodBench;

        [Header("Tampilan dunia (#66)")]
        public Mesh worldMesh;
        public Material worldMaterial;

        [Header("Tangan (#60)")]
        public EquippableBehaviour equipPrefab;    // null = tidak bisa dipegang
        public HandUse handUse = HandUse.OneHand;

        [Header("Kapasitas (#118)")]
        public int bagBonusSlots;                  // Lebih dari 0 = item adalah tas
        public int shipWeight = 1;                 // Berat saat diambil di kapal
        public int shipPickupAmount = 1;           // Jumlah dari satu objek kapal

        [Header("Lain")]
        public ToolClass toolClass = ToolClass.None;   // #73
        public bool isWarmClothing;                     // #65, Gear_WarmClothes

        /// <summary>Kunci teks nama. Kosong memakai pola item.&lt;itemID&gt;.name (#104).</summary>
        public string NameKey => string.IsNullOrEmpty(displayNameKey) ? "item." + itemID + ".name" : displayNameKey;

        /// <summary>Item dengan data unik per instance (durabilitas) tidak bertumpuk (ItemUseSystem.md).</summary>
        public int EffectiveMaxStack => maxDurability >= 0f ? 1 : Mathf.Max(1, maxStack);

#if UNITY_EDITOR
        private void OnValidate()
        {
            // GLOSSARY.md: item inti dan KeyItem tidak bisa dibuang (#116).
            if (isCoreItem || type == ItemType.KeyItem) isDroppable = false;
            if (maxStack < 1) maxStack = 1;
        }
#endif
    }
}
