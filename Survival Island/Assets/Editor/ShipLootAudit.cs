using System.Collections.Generic;
using System.Text;
using UnityEditor;

namespace SurvivalIsland.EditorTools
{
    /// <summary>
    /// Pemeriksa data barang kapal. Blueprint: 60-seconds/ShipInteractable.md bagian "Pemeriksaan data di Editor" (#74, #76).
    /// Memeriksa semua ItemData di tabel loot kapal (LootTable dengan location Ship, yaitu Loot_Ship) dan semua NPCData:
    /// - shipWeight dan NPCData.weight di antara 1 dan ship.handCapacity. Lebih besar berarti tidak pernah bisa diambil.
    /// - shipPickupAmount minimal 1.
    /// - worldMesh tidak kosong. Kosong berarti kubus pengganti tampil di game.
    /// - Kunci nama ada di setiap LocalizationTable (#104). Kalau belum ada tabel, pemeriksaan ini dilewati.
    /// Menu: Survival > Audit > Ship Loot.
    /// </summary>
    public class ShipLootAudit : AuditWindowBase
    {
        protected override string AuditName => "Ship Loot Audit";

        [MenuItem("Survival/Audit/Ship Loot")]
        private static void Open() => OpenAndRun<ShipLootAudit>("Ship Loot Audit");

        protected override int Audit(StringBuilder sb)
        {
            int problems = 0;
            var config = AuditUtil.LoadConfig();
            int capacity = config != null ? config.ship.handCapacity : new ShipBalance().handCapacity;
            if (config == null) sb.AppendLine("Peringatan: GameBalanceConfig tidak ditemukan, memakai handCapacity awal.");

            var tables = AuditUtil.FindAssets<LocalizationTable>();
            var keysPerTable = new List<KeyValuePair<string, HashSet<string>>>();
            foreach (var t in tables)
            {
                var set = new HashSet<string>();
                if (t.rows != null) foreach (var r in t.rows) if (!string.IsNullOrEmpty(r.key)) set.Add(r.key);
                keysPerTable.Add(new KeyValuePair<string, HashSet<string>>(t.localeId, set));
            }

            // 1. Barang di tabel loot kapal.
            var items = new List<ItemData>();
            var lootTables = new List<string>();
            foreach (var table in AuditUtil.FindAssets<LootTable>())
            {
                if (table.location != LootLocation.Ship) continue;
                lootTables.Add(table.name);
                if (table.entries == null) continue;
                foreach (var e in table.entries)
                    if (e.item != null && !items.Contains(e.item)) items.Add(e.item);
            }
            sb.AppendLine($"== Barang kapal ({items.Count} item dari {string.Join(", ", lootTables)}) ==");
            if (lootTables.Count == 0) { sb.AppendLine("Tidak ada LootTable dengan location Ship."); problems++; }
            foreach (var item in items)
            {
                string who = $"{item.itemID} ({item.name})";
                if (item.shipWeight < 1 || item.shipWeight > capacity)
                { sb.AppendLine($"{who}: shipWeight {item.shipWeight} di luar 1-{capacity}, jadi tidak bisa diambil."); problems++; }
                if (item.shipPickupAmount < 1)
                { sb.AppendLine($"{who}: shipPickupAmount {item.shipPickupAmount} kurang dari 1."); problems++; }
                if (item.worldMesh == null)
                { sb.AppendLine($"{who}: worldMesh kosong, kubus pengganti akan tampil."); problems++; }
                problems += CheckKey(sb, who, item.NameKey, keysPerTable);
            }

            // 2. NPC kapal.
            var npcs = AuditUtil.FindAssets<NPCData>();
            sb.AppendLine($"== NPC ({npcs.Count}) ==");
            foreach (var npc in npcs)
            {
                string who = $"{npc.npcId} ({npc.name})";
                if (npc.weight < 1 || npc.weight > capacity)
                { sb.AppendLine($"{who}: weight {npc.weight} di luar 1-{capacity}, jadi tidak bisa diselamatkan."); problems++; }
                if (string.IsNullOrEmpty(npc.displayNameKey))
                { sb.AppendLine($"{who}: displayNameKey kosong."); problems++; }
                else problems += CheckKey(sb, who, npc.displayNameKey, keysPerTable);
            }

            sb.AppendLine(tables.Count == 0
                ? "Catatan: belum ada LocalizationTable, jadi kunci teks tidak diperiksa (#104)."
                : $"Kunci teks diperiksa di {tables.Count} tabel.");
            sb.AppendLine($"Batas tangan: {capacity}.");
            return problems;
        }

        private static int CheckKey(StringBuilder sb, string who, string key, List<KeyValuePair<string, HashSet<string>>> tables)
        {
            int problems = 0;
            foreach (var t in tables)
            {
                if (t.Value.Contains(key)) continue;
                sb.AppendLine($"{who}: kunci {key} tidak ada di tabel {t.Key}.");
                problems++;
            }
            return problems;
        }
    }
}
