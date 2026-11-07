using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Data validasi satu titik NPC di kapal. Indeks sama dengan LifeboatExtraction.shipNpcPoints (titik Transform di ShipPhase).
    /// Posisi tetap di scene; aset ini hanya menyimpan hasil pemeriksaan Editor supaya syarat 5 bisa diperiksa tanpa membuka ShipPhase.
    /// </summary>
    [Serializable]
    public struct ShipNpcPointInfo
    {
        [Tooltip("Nama titik untuk dibaca developer (diisi menu Editor dari nama Transform).")]
        public string label;
        [Tooltip("Ada jalur NavMesh ke sekoci. Diisi menu Editor. Titik false tidak boleh dipakai (syarat 5).")]
        public bool isReachable;
        [Tooltip("Perkiraan waktu jalan ke sekoci (panjang jalur / ship.moveSpeed). 0 = belum diukur, dianggap lolos.")]
        public float walkSecondsToLifeboat;
    }

    /// <summary>Salinan satu titik loot kapal tanpa Transform. Cukup untuk LootSpawnerSystem.RollPoint (syarat 4).</summary>
    [Serializable]
    public class ShipLootPointSource
    {
        public LootPointKind kind = LootPointKind.House;
        [Tooltip("Sama dengan LootPointDef.guaranteedItems. Hanya elemen pertama yang dipakai.")]
        public ItemStack[] guaranteedItems = Array.Empty<ItemStack>();
    }

    /// <summary>Salinan satu LootSpawnerSystem di ShipPhase. Urutan titik harus sama dengan spawner aslinya.</summary>
    [Serializable]
    public class ShipLootSpawnerSource
    {
        public string spawnerId;
        public LootTable table;
        public ShipLootPointSource[] points = Array.Empty<ShipLootPointSource>();
    }

    /// <summary>
    /// Sumber data susunan run. Blueprint: Core_Logic/RunSeedManager.md bagian 1 (#89): PartSpawnPointTable, daftar NPC,
    /// titik NPC di kapal, DisasterData[], dan RunLayout safeLayout. Ditambah salinan spawner loot kapal dan daftar itemID
    /// makanan/peluru/minuman untuk syarat 4 (tafsiran: "daftar spawnerId di RunLayoutSources").
    ///
    /// Keputusan developer (7 November 2026): RunLayoutSources dan RunLayoutValidator ditulis di sistem pendukung.
    /// Dipasang di field RunSeedManager.layoutSources (prefab PersistentRoot).
    /// </summary>
    [CreateAssetMenu(fileName = "RunLayoutSources", menuName = "Survival/Run Layout Sources")]
    public class RunLayoutSources : ScriptableObject
    {
        [Header("Suku cadang (#69)")]
        [Tooltip("Aset yang sama dengan HeavyItemSystem.spawnPoints.")]
        public PartSpawnPointTable partSpawnPoints;

        [Header("NPC kapal (#76)")]
        [Tooltip("Aset NPCData yang sama dengan NPCRoster.database dan LifeboatExtraction.npcDatabase.")]
        public NPCData[] shipNpcs = Array.Empty<NPCData>();
        [Tooltip("Urutan sama dengan LifeboatExtraction.shipNpcPoints. Isi lewat menu konteks saat ShipPhase terbuka.")]
        public ShipNpcPointInfo[] shipNpcPoints = Array.Empty<ShipNpcPointInfo>();

        [Header("Bencana (#70)")]
        [Tooltip("Aset DisasterData (Eruption, Earthquake, TropicalStorm). Kosong = memakai RunSeedManager.disasterData lama.")]
        public DisasterData[] disasterData = Array.Empty<DisasterData>();

        [Header("Bekal kapal, syarat 4")]
        [Tooltip("Salinan semua LootSpawnerSystem di ShipPhase. Isi lewat menu konteks saat ShipPhase terbuka.")]
        public ShipLootSpawnerSource[] shipLootSpawners = Array.Empty<ShipLootSpawnerSource>();
        [Tooltip("itemID yang dihitung sebagai makanan (minShipFoodItems).")]
        public string[] foodItemIds = { "Food_Can" };
        [Tooltip("itemID yang dihitung sebagai peluru (minShipAmmoItems).")]
        public string[] ammoItemIds = { "Ammo_9mm" };
        [Tooltip("itemID yang dihitung sebagai minuman (minShipDrinkItems).")]
        public string[] drinkItemIds = { "Drink_CleanWater" };

        [Header("Cadangan")]
        [Tooltip("Dipakai kalau semua seed gagal. Jadwal bencana boleh kosong: diisi dari seed saat dipakai.")]
        public RunLayout safeLayout = new RunLayout();

        /// <summary>Salinan safeLayout, supaya EnsureDisasterScheduleThrough tidak mengubah aset.</summary>
        public RunLayout CloneSafeLayout()
        {
            var src = safeLayout ?? new RunLayout();
            return new RunLayout
            {
                partPointByPart = Copy(src.partPointByPart),
                shipNpcIds = Copy(src.shipNpcIds),
                shipNpcPointByNpc = Copy(src.shipNpcPointByNpc),
                disasterSchedule = Copy(src.disasterSchedule),
                disasterScheduleThroughDay = src.disasterScheduleThroughDay,
            };
        }

        private static T[] Copy<T>(T[] a) => a != null ? (T[])a.Clone() : Array.Empty<T>();

        public static bool Contains(string[] list, string id)
        {
            if (list == null || string.IsNullOrEmpty(id)) return false;
            for (int i = 0; i < list.Length; i++) if (list[i] == id) return true;
            return false;
        }

#if UNITY_EDITOR
        // ---------------------------------------------------------------- Alat Editor

        /// <summary>
        /// Salin spawner loot kapal dan titik NPC dari scene ShipPhase yang sedang terbuka. Waktu jalan diukur dengan
        /// NavMesh.CalculatePath ke LifeboatExtraction.SafePoint, tanpa obstacle jadwal ShipHazardManager (ShipRouteAudit belum ada).
        /// </summary>
        [ContextMenu("Salin data kapal dari scene ShipPhase yang terbuka")]
        private void SyncFromOpenShipScene()
        {
            var spawners = FindObjectsByType<LootSpawnerSystem>(FindObjectsSortMode.None);
            var list = new System.Collections.Generic.List<ShipLootSpawnerSource>();
            for (int i = 0; i < spawners.Length; i++)
            {
                var sp = spawners[i];
                if (sp == null || string.IsNullOrEmpty(sp.SpawnerId) || !sp.SpawnerId.StartsWith("ship_", StringComparison.Ordinal)) continue;
                var defs = sp.SpawnPointDefs;
                var pts = new ShipLootPointSource[defs.Count];
                for (int k = 0; k < defs.Count; k++)
                    pts[k] = new ShipLootPointSource
                    {
                        kind = defs[k] != null ? defs[k].kind : LootPointKind.House,
                        guaranteedItems = defs[k] != null && defs[k].guaranteedItems != null
                            ? (ItemStack[])defs[k].guaranteedItems.Clone() : Array.Empty<ItemStack>(),
                    };
                list.Add(new ShipLootSpawnerSource { spawnerId = sp.SpawnerId, table = sp.Table, points = pts });
            }
            list.Sort((a, b) => string.CompareOrdinal(a.spawnerId, b.spawnerId));
            shipLootSpawners = list.ToArray();

            var boat = FindFirstObjectByType<LifeboatExtraction>();
            if (boat != null)
            {
                var points = boat.ShipNpcPoints;
                float speed = new ShipBalance().moveSpeed;
                var infos = new ShipNpcPointInfo[points.Count];
                var path = new UnityEngine.AI.NavMeshPath();
                for (int i = 0; i < points.Count; i++)
                {
                    var t = points[i];
                    infos[i].label = t != null ? t.name : "(kosong)";
                    if (t == null || boat.SafePoint == null) continue;
                    bool ok = UnityEngine.AI.NavMesh.CalculatePath(t.position, boat.SafePoint.position, UnityEngine.AI.NavMesh.AllAreas, path)
                              && path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete;
                    infos[i].isReachable = ok;
                    if (ok)
                    {
                        float len = 0f;
                        var c = path.corners;
                        for (int k = 1; k < c.Length; k++) len += Vector3.Distance(c[k - 1], c[k]);
                        infos[i].walkSecondsToLifeboat = speed > 0f ? len / speed : 0f;
                    }
                }
                shipNpcPoints = infos;
            }
            else Debug.LogWarning("[RunLayoutSources] LifeboatExtraction tidak ada di scene terbuka. Titik NPC tidak diubah.", this);

            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log($"[RunLayoutSources] {shipLootSpawners.Length} spawner kapal dan {shipNpcPoints.Length} titik NPC disalin.", this);
        }

        /// <summary>RunSeedAudit (RunSeedManager.md): 1000 seed, alasan gagal terbanyak. Di atas 5 persen = data titik perlu diperbaiki.</summary>
        [ContextMenu("RunSeedAudit: periksa 1000 seed")]
        private void AuditSeeds()
        {
            Debug.Log(RunLayoutValidator.Audit(this, 1000), this);
        }

        [ContextMenu("Periksa safeLayout")]
        private void CheckSafeLayout()
        {
            bool ok = RunLayoutValidator.ValidateSeedIndependent(CloneSafeLayout(), this, out string failure);
            if (ok) Debug.Log("[RunLayoutSources] safeLayout lolos syarat 1, 2, 3, dan 5.", this);
            else Debug.LogError("[RunLayoutSources] safeLayout gagal: " + failure, this);
        }
#endif
    }
}
