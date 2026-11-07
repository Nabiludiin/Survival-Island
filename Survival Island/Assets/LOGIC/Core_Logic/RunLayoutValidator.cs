using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pemeriksa susunan run. Blueprint: Core_Logic/RunSeedManager.md bagian 2 "Membangun susunan run yang valid" (#89).
    /// Dipanggil RunSeedManager.ResolveLayout saat run baru, jadi kodenya runtime. Alat audit hanya di Editor.
    ///
    /// Syarat:
    /// 1. Jumlah titik suku cadang = region.helicopterPartCount, semua berbeda.
    /// 2. Tiap titik sah dan isReachable (pemeriksaan Editor PartSpawnPointTable, termasuk terendam saat pasang #91).
    /// 3. Jarak antar-titik minimal runSeed.minPartPointSpacing.
    /// 4. Loot kapal (Fork ShipLoot lewat LootSpawnerSystem.RollPoint) memuat minShipFoodItems, minShipAmmoItems, minShipDrinkItems.
    /// 5. Titik NPC kapal sah, isReachable, dan waktu jalan ke sekoci paling lama maxShipNpcWalkSeconds (#75).
    /// 6. Bencana besar tidak sebelum firstMajorDisasterDay, dan berjarak minimal minDaysBetweenMajorDisasters.
    ///
    /// Tafsiran: signature blueprint Validate(layout, seed, out failure) ditambah parameter sources, karena kelas ini statis.
    /// </summary>
    public static class RunLayoutValidator
    {
        private static readonly RunSeedBalance FallbackRunSeed = new RunSeedBalance();
        private static readonly RegionBalance FallbackRegion = new RegionBalance();

        private static RunSeedBalance RunSeedBal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.runSeed : FallbackRunSeed;
        private static RegionBalance RegionBal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.region : FallbackRegion;

        /// <summary>Semua syarat. failure berisi alasan gagal pertama (dipakai log dan RunSeedAudit).</summary>
        public static bool Validate(RunLayout layout, uint seed, RunLayoutSources sources, out string failure)
        {
            if (!ValidateSeedIndependent(layout, sources, out failure)) return false;
            if (!CheckShipLoot(seed, sources, out failure)) return false;    // 4
            return CheckDisasters(layout, sources, out failure);             // 6
        }

        /// <summary>Syarat 1, 2, 3, dan 5. Dipakai juga untuk memeriksa safeLayout, yang tidak bergantung pada seed.</summary>
        public static bool ValidateSeedIndependent(RunLayout layout, RunLayoutSources sources, out string failure)
        {
            failure = null;
            if (layout == null) { failure = "layout null"; return false; }
            if (sources == null) { failure = "RunLayoutSources kosong"; return false; }
            if (!CheckParts(layout, sources.partSpawnPoints, out failure)) return false;
            return CheckShipNpcs(layout, sources, out failure);
        }

        // ---------------------------------------------------------------- Syarat 1-3

        private static bool CheckParts(RunLayout layout, PartSpawnPointTable table, out string failure)
        {
            failure = null;
            int count = RegionBal.helicopterPartCount;
            var pts = layout.partPointByPart ?? Array.Empty<int>();

            // 1.
            if (pts.Length != count) { failure = $"syarat 1: {pts.Length} titik suku cadang, butuh {count}"; return false; }
            for (int i = 0; i < pts.Length; i++)
                for (int j = i + 1; j < pts.Length; j++)
                    if (pts[i] == pts[j]) { failure = $"syarat 1: titik {pts[i]} dipakai dua barang"; return false; }

            // 2.
            if (table == null && count > 0) { failure = "syarat 2: PartSpawnPointTable kosong"; return false; }
            for (int i = 0; i < pts.Length; i++)
                if (!table.IsValidIndex(pts[i])) { failure = $"syarat 2: titik {pts[i]} tidak ada atau isReachable = false"; return false; }

            // 3.
            float min = RunSeedBal.minPartPointSpacing;
            for (int i = 0; i < pts.Length; i++)
                for (int j = i + 1; j < pts.Length; j++)
                    if ((table.points[pts[i]].position - table.points[pts[j]].position).sqrMagnitude < min * min)
                    {
                        failure = $"syarat 3: titik {pts[i]} dan {pts[j]} lebih dekat dari {min} m";
                        return false;
                    }
            return true;
        }

        // ---------------------------------------------------------------- Syarat 5

        private static bool CheckShipNpcs(RunLayout layout, RunLayoutSources sources, out string failure)
        {
            failure = null;
            var ids = layout.shipNpcIds ?? Array.Empty<string>();
            var map = layout.shipNpcPointByNpc ?? Array.Empty<int>();
            var points = sources.shipNpcPoints ?? Array.Empty<ShipNpcPointInfo>();

            int expected = RunSeedManager.ShipNpcTargetCount(sources);
            if (ids.Length != expected) { failure = $"syarat 5: {ids.Length} NPC kapal, butuh {expected}"; return false; }
            if (map.Length != ids.Length) { failure = "syarat 5: shipNpcPointByNpc tidak sepanjang shipNpcIds"; return false; }

            float maxWalk = RunSeedBal.maxShipNpcWalkSeconds;
            for (int i = 0; i < ids.Length; i++)
            {
                if (FindNpc(sources, ids[i]) == null) { failure = $"syarat 5: NPC '{ids[i]}' tidak ada di shipNpcs"; return false; }
                int p = map[i];
                if (p < 0 || p >= points.Length) { failure = $"syarat 5: titik NPC {p} tidak ada"; return false; }
                if (!IsShipNpcPointUsable(points[p], maxWalk)) { failure = $"syarat 5: titik NPC {p} tidak bisa dicapai sebelum air naik"; return false; }
                for (int j = 0; j < i; j++)
                {
                    if (map[j] == p) { failure = $"syarat 5: titik NPC {p} dipakai dua NPC"; return false; }
                    if (ids[j] == ids[i]) { failure = $"syarat 5: NPC '{ids[i]}' muncul dua kali"; return false; }
                }
            }
            return true;
        }

        /// <summary>Titik NPC boleh dipakai: isReachable, dan waktu jalan tidak melebihi batas. 0 = belum diukur, lolos.</summary>
        public static bool IsShipNpcPointUsable(in ShipNpcPointInfo p, float maxWalkSeconds)
        {
            return p.isReachable && (p.walkSecondsToLifeboat <= 0f || maxWalkSeconds <= 0f || p.walkSecondsToLifeboat <= maxWalkSeconds);
        }

        private static NPCData FindNpc(RunLayoutSources sources, string id)
        {
            if (sources.shipNpcs == null || string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < sources.shipNpcs.Length; i++)
                if (sources.shipNpcs[i] != null && sources.shipNpcs[i].npcId == id) return sources.shipNpcs[i];
            return null;
        }

        // ---------------------------------------------------------------- Syarat 4

        /// <summary>
        /// Hitung isi kapal persis seperti LootSpawnerSystem di ShipPhase (hari 1, aliran ShipLoot, kunci spawnerId/indeks),
        /// tanpa membuka scene. Tafsiran: satu titik berisi = satu barang, berapa pun shipPickupAmount-nya.
        /// </summary>
        public static void CountShipSupplies(uint seed, RunLayoutSources sources, out int food, out int ammo, out int drink)
        {
            food = ammo = drink = 0;
            if (sources == null || sources.shipLootSpawners == null) return;
            for (int s = 0; s < sources.shipLootSpawners.Length; s++)
            {
                var sp = sources.shipLootSpawners[s];
                if (sp == null || sp.points == null) continue;
                for (int i = 0; i < sp.points.Length; i++)
                {
                    var pt = sp.points[i];
                    if (pt == null) continue;
                    if (!LootSpawnerSystem.RollPointForSeed(pt.kind, pt.guaranteedItems, sp.table, sp.spawnerId, i,
                                                            RandomStreamId.ShipLoot, 1, seed, out string itemId, out _))
                        continue;
                    if (RunLayoutSources.Contains(sources.foodItemIds, itemId)) food++;
                    if (RunLayoutSources.Contains(sources.ammoItemIds, itemId)) ammo++;
                    if (RunLayoutSources.Contains(sources.drinkItemIds, itemId)) drink++;
                }
            }
        }

        private static bool CheckShipLoot(uint seed, RunLayoutSources sources, out string failure)
        {
            failure = null;
            // Tanpa salinan spawner kapal, syarat 4 tidak bisa dihitung. Dilewati dengan peringatan supaya run tetap jalan.
            if (sources.shipLootSpawners == null || sources.shipLootSpawners.Length == 0)
            {
                WarnOnce(ref warnedNoShipLoot, "[RunLayoutValidator] RunLayoutSources.shipLootSpawners kosong. Syarat 4 (bekal kapal) dilewati.");
                return true;
            }
            var bal = RunSeedBal;
            CountShipSupplies(seed, sources, out int food, out int ammo, out int drink);
            if (food < bal.minShipFoodItems) { failure = $"syarat 4: makanan {food} < {bal.minShipFoodItems}"; return false; }
            if (ammo < bal.minShipAmmoItems) { failure = $"syarat 4: peluru {ammo} < {bal.minShipAmmoItems}"; return false; }
            if (drink < bal.minShipDrinkItems) { failure = $"syarat 4: minuman {drink} < {bal.minShipDrinkItems}"; return false; }
            return true;
        }

        // ---------------------------------------------------------------- Syarat 6

        private static bool CheckDisasters(RunLayout layout, RunLayoutSources sources, out string failure)
        {
            failure = null;
            var schedule = layout.disasterSchedule ?? Array.Empty<DisasterScheduleEntry>();
            var data = RunSeedManager.ResolveDisasterData(sources);
            var bal = RunSeedBal;
            int lastMajor = int.MinValue / 2;
            for (int i = 0; i < schedule.Length; i++)
            {
                var e = schedule[i];
                var d = FindDisaster(data, e.type);
                if (d == null || !d.IsMajor(e.scale)) continue;
                if (e.day < bal.firstMajorDisasterDay) { failure = $"syarat 6: bencana besar {e.type} di hari {e.day}"; return false; }
                if (e.day - lastMajor < bal.minDaysBetweenMajorDisasters)
                {
                    failure = $"syarat 6: bencana besar hari {lastMajor} dan {e.day} terlalu dekat";
                    return false;
                }
                lastMajor = e.day;
            }
            return true;
        }

        private static DisasterData FindDisaster(DisasterData[] data, DisasterType type)
        {
            if (data == null) return null;
            for (int i = 0; i < data.Length; i++) if (data[i] != null && data[i].type == type) return data[i];
            return null;
        }

        // ---------------------------------------------------------------- Lain-lain

        private static bool warnedNoShipLoot;

        private static void WarnOnce(ref bool flag, string message)
        {
            if (flag) return;
            flag = true;
            Debug.LogWarning(message);
        }

#if UNITY_EDITOR
        /// <summary>
        /// RunSeedAudit (RunSeedManager.md): bangun dan periksa <paramref name="count"/> seed berurutan, lalu laporkan
        /// jumlah gagal dan alasan terbanyak. Tanpa GameManager (mode Edit) angka config memakai nilai awal kode.
        /// </summary>
        public static string Audit(RunLayoutSources sources, int count)
        {
            if (sources == null) return "[RunSeedAudit] RunLayoutSources kosong.";
            var reasons = new System.Collections.Generic.Dictionary<string, int>();
            int failed = 0;
            for (int i = 1; i <= count; i++)
            {
                uint seed = (uint)i * 2654435761u;
                if (seed == 0) seed = 1;
                var layout = RunSeedManager.BuildLayout(seed, sources, RunSeedManager.ResolveDisasterData(sources));
                if (Validate(layout, seed, sources, out string failure)) continue;
                failed++;
                // Kelompokkan per syarat (teks sebelum titik dua), bukan per titik.
                int colon = failure != null ? failure.IndexOf(':') : -1;
                string key = colon > 0 ? failure.Substring(0, colon) : failure ?? "?";
                reasons.TryGetValue(key, out int n);
                reasons[key] = n + 1;
            }
            var sb = new System.Text.StringBuilder();
            float pct = count > 0 ? 100f * failed / count : 0f;
            sb.Append($"[RunSeedAudit] {failed}/{count} seed gagal ({pct:0.0} persen).");
            if (pct > 5f) sb.Append(" Di atas 5 persen: perbaiki data titik, bukan menaikkan maxSeedRerolls.");
            foreach (var kv in reasons) sb.Append("\n- ").Append(kv.Key).Append(": ").Append(kv.Value);
            return sb.ToString();
        }
#endif
    }
}
