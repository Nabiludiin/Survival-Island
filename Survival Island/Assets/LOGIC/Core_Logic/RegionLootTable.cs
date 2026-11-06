using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Lokasi tabel loot. Nilai mengikuti GLOSSARY.md (#103).</summary>
    public enum LootLocation { None, Ship, Volcano, Amazon, Snow, Logistic }

    /// <summary>Jenis titik loot. Flags, nilai mengikuti GLOSSARY.md (#103).</summary>
    [Flags] public enum LootPointKind { House = 1, Chest = 2, SoldierBody = 4, Warehouse = 8, Any = 15 }

    /// <summary>Satu baris LootTable. Menggantikan kelas LootTable lama berisi satu item (#103).</summary>
    [Serializable]
    public struct LootEntry
    {
        public ItemData item;
        [Tooltip("Bobot relatif, bukan persen.")]
        public float weight;
        public int minAmount, maxAmount;
        [Tooltip("Jenis titik yang boleh memuat entri ini.")]
        public LootPointKind kinds;
        [Tooltip("Bobot dianggap 0 sebelum hari ini.")]
        public int minDay;
        [Tooltip("Bobotnya naik mengikuti hari.")]
        public bool isRare;
    }

    /// <summary>Pasangan ItemData dan jumlah untuk isi tetap titik loot (guaranteedItems, GLOSSARY.md #103).</summary>
    [Serializable]
    public struct ItemStack
    {
        public ItemData item;
        public int amount;
        public ItemStack(ItemData item, int amount) { this.item = item; this.amount = amount; }
    }

    /// <summary>
    /// Pemilih isi loot dari aset LootTable. Kelas statis tanpa status; status titik dipegang LootSpawnerSystem dan
    /// LootContainerInteractable. Blueprint: Core_Logic/RegionLootTable.md (#103).
    /// GDD bagian 3 (looting di kapal dan pulau) dan bagian 5 (region). Rumus bobot, skala hari, dan pengali lokasi adalah
    /// usulan yang disetujui developer sebagai nilai awal (#85).
    /// </summary>
    public static class RegionLootTable
    {
        // Buffer bobot yang dipakai ulang (tanpa garbage). Elemen sisa diisi 0, dan PickWeighted mengabaikan bobot 0.
        private static float[] weightBuffer = new float[32];

        /// <summary>
        /// Pilih satu entri dan jumlahnya. false kalau tabel kosong atau semua bobot efektif 0 (titik dibiarkan kosong, #89).
        /// Urutan pemakaian aliran acak: PickWeighted lalu Range jumlah. Mengubah urutan ini mengubah susunan loot per seed.
        /// </summary>
        public static bool TryRoll(LootTable table, LootPointKind kind, int day, ref RandomStream stream,
                                   out ItemData item, out int amount)
        {
            item = null;
            amount = 0;
            if (table == null || table.entries == null || table.entries.Length == 0) return false;

            var entries = table.entries;
            if (weightBuffer.Length < entries.Length) weightBuffer = new float[Mathf.NextPowerOfTwo(entries.Length)];

            // 1. Bobot efektif tiap entri.
            float rareMul = GetRareMultiplier(day);
            for (int i = 0; i < weightBuffer.Length; i++)
            {
                if (i >= entries.Length) { weightBuffer[i] = 0f; continue; }
                weightBuffer[i] = EffectiveWeight(in entries[i], kind, day, rareMul);
            }

            // 2. Pilih menurut bobot.
            int picked = stream.PickWeighted(weightBuffer);
            if (picked < 0 || picked >= entries.Length) return false;

            // 3. Jumlah: acak min..max, kali pengali lokasi dan hari, bulat ke bawah, minimal 1.
            var e = entries[picked];
            int minA = Mathf.Max(1, e.minAmount);
            int maxA = Mathf.Max(minA, e.maxAmount);
            int raw = stream.Range(minA, maxA + 1);
            amount = Mathf.Max(1, Mathf.FloorToInt(raw * GetAmountMul(table.location, day)));
            // Item yang tidak bisa bertumpuk selalu berjumlah 1.
            if (e.item.EffectiveMaxStack <= 1) amount = 1;
            item = e.item;
            return true;
        }

        /// <summary>
        /// amountMul&lt;lokasi&gt; x (1 + amountBonusPer10Days x floor(day / 10)). Ship dan None memakai pengali 1 (RegionLootTable.md).
        /// </summary>
        public static float GetAmountMul(LootLocation loc, int day)
        {
            var bal = GameManager.CurrentBalance;
            if (bal == null || loc == LootLocation.Ship || loc == LootLocation.None) return 1f;
            var l = bal.loot;
            float locMul;
            switch (loc)
            {
                case LootLocation.Volcano: locMul = l.amountMulVolcano; break;
                case LootLocation.Amazon: locMul = l.amountMulAmazon; break;
                case LootLocation.Snow: locMul = l.amountMulSnow; break;     // Region 3, area terjauh
                case LootLocation.Logistic: locMul = l.amountMulLogistic; break;
                default: locMul = 1f; break;
            }
            int tens = Mathf.Max(0, day) / 10;
            return locMul * (1f + l.amountBonusPer10Days * tens);
        }

        /// <summary>Pengali bobot item langka: min(rareWeightMaxMul, 1 + rareWeightPerDay x (hari - 1)).</summary>
        public static float GetRareMultiplier(int day)
        {
            var bal = GameManager.CurrentBalance;
            if (bal == null) return 1f;
            float mul = 1f + bal.loot.rareWeightPerDay * Mathf.Max(0, day - 1);
            return Mathf.Min(bal.loot.rareWeightMaxMul, mul);
        }

        private static float EffectiveWeight(in LootEntry e, LootPointKind kind, int day, float rareMul)
        {
            if (e.item == null || e.weight <= 0f) return 0f;
            if ((e.kinds & kind) == 0) return 0f;
            if (day < e.minDay) return 0f;   // Contoh: Ammo_556 tertahan sebelum hari ke-10
            return e.isRare ? e.weight * rareMul : e.weight;
        }
    }
}
