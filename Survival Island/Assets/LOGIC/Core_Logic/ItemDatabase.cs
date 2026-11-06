using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pencarian ItemData berdasarkan itemId dan alias (GLOSSARY.md, ItemUseSystem.md: "Dictionary yang dibangun sekali").
    ///
    /// Catatan implementasi (Langkah 3, usulan teknis): ItemDatabase belum punya blueprint sendiri. Aset ItemData dimuat
    /// sekali dari folder Resources/Items saat pertama dipakai. Alias = bagian ID setelah garis bawah, huruf kecil
    /// (contoh "wood" menjadi "Mat_Wood"), dipakai DevConsole (#56) lewat ResolveAlias.
    /// </summary>
    public static class ItemDatabase
    {
        public const string ResourcesFolder = "Items";

        private static Dictionary<string, ItemData> byId;
        private static Dictionary<string, string> aliasToId;

        /// <summary>Mencari data item. Mengembalikan null kalau tidak ada.</summary>
        public static ItemData Get(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return null;
            EnsureLoaded();
            return byId.TryGetValue(itemId, out var data) ? data : null;
        }

        /// <summary>Mengubah alias pendek menjadi itemId. ID yang sudah benar dikembalikan apa adanya.</summary>
        public static string ResolveAlias(string idOrAlias)
        {
            if (string.IsNullOrEmpty(idOrAlias)) return idOrAlias;
            EnsureLoaded();
            if (byId.ContainsKey(idOrAlias)) return idOrAlias;
            return aliasToId.TryGetValue(idOrAlias.ToLowerInvariant(), out var id) ? id : idOrAlias;
        }

        /// <summary>Semua ID terdaftar (untuk Tab-complete DevConsole).</summary>
        public static IEnumerable<string> AllIds
        {
            get { EnsureLoaded(); return byId.Keys; }
        }

        /// <summary>Mengganti isi database secara manual (tes atau pemuatan lewat Addressables nanti).</summary>
        public static void Register(IEnumerable<ItemData> items)
        {
            byId = new Dictionary<string, ItemData>(StringComparer.Ordinal);
            aliasToId = new Dictionary<string, string>(StringComparer.Ordinal);
            if (items == null) return;
            foreach (var item in items) Add(item);
        }

        private static void EnsureLoaded()
        {
            if (byId != null) return;
            Register(Resources.LoadAll<ItemData>(ResourcesFolder));
        }

        private static void Add(ItemData item)
        {
            if (item == null || string.IsNullOrEmpty(item.itemID)) return;
            if (byId.ContainsKey(item.itemID))
            {
                Debug.LogWarning($"[ItemDatabase] itemID ganda: {item.itemID}. Aset {item.name} diabaikan.");
                return;
            }
            byId.Add(item.itemID, item);

            int underscore = item.itemID.IndexOf('_');
            string alias = (underscore >= 0 ? item.itemID.Substring(underscore + 1) : item.itemID).ToLowerInvariant();
            if (!aliasToId.ContainsKey(alias)) aliasToId.Add(alias, item.itemID);
        }
    }
}
