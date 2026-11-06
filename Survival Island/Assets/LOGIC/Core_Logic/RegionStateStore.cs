using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Status objek tiap region (SaveId -> JSON), termasuk region yang sedang tidak dimuat.
    /// Blueprint: Core_Logic/RegionStreaming.md bagian "Menyimpan status objek di region yang tidak dimuat" (#88).
    /// Kelas biasa (bukan MonoBehaviour) milik RegionStreaming di IslandPhase. Isinya masuk ke regionStoreJson di file save
    /// (WorldStateSave.md, LoadOrder 20), jadi WorldStateSave yang memanggil ToSaveJson dan LoadFromSaveJson.
    /// </summary>
    public class RegionStateStore
    {
        private const int RegionCount = 4;

        // region -> (SaveId -> JSON)
        private readonly Dictionary<string, string>[] states = new Dictionary<string, string>[RegionCount];

        /// <summary>Jam game saat region terakhir dilepas. -1 kalau belum pernah. Ikut disimpan (RegionStreaming.md bagian 4).</summary>
        private readonly float[] lastUnloadedGameHour = new float[RegionCount];

        public RegionStateStore()
        {
            for (int i = 0; i < RegionCount; i++)
            {
                states[i] = new Dictionary<string, string>(64, StringComparer.Ordinal);
                lastUnloadedGameHour[i] = -1f;
            }
        }

        public void Write(RegionType region, string saveId, string json)
        {
            if (string.IsNullOrEmpty(saveId)) return;
            states[(int)region][saveId] = json ?? string.Empty;
        }

        public bool TryRead(RegionType region, string saveId, out string json)
        {
            json = null;
            if (string.IsNullOrEmpty(saveId)) return false;
            return states[(int)region].TryGetValue(saveId, out json);
        }

        public bool Remove(RegionType region, string saveId) => !string.IsNullOrEmpty(saveId) && states[(int)region].Remove(saveId);

        public int Count(RegionType region) => states[(int)region].Count;

        public float GetLastUnloadedGameHour(RegionType region) => lastUnloadedGameHour[(int)region];
        public void SetLastUnloadedGameHour(RegionType region, float hour) => lastUnloadedGameHour[(int)region] = hour;

        public void Clear()
        {
            for (int i = 0; i < RegionCount; i++)
            {
                states[i].Clear();
                lastUnloadedGameHour[i] = -1f;
            }
        }

        // ---------------------------------------------------------------- Save (JsonUtility tidak bisa Dictionary)

        [Serializable]
        private class EntryRecord
        {
            public int region;
            public string id;
            public string json;
        }

        [Serializable]
        private class StoreRecord
        {
            public int version = 1;
            public float[] lastUnloadedGameHour;
            public EntryRecord[] entries;
        }

        /// <summary>Bentuk daftar untuk regionStoreJson.</summary>
        public string ToSaveJson()
        {
            int total = 0;
            for (int i = 0; i < RegionCount; i++) total += states[i].Count;
            var rec = new StoreRecord
            {
                lastUnloadedGameHour = (float[])lastUnloadedGameHour.Clone(),
                entries = new EntryRecord[total]
            };
            int n = 0;
            for (int i = 0; i < RegionCount; i++)
                foreach (var kv in states[i])
                    rec.entries[n++] = new EntryRecord { region = i, id = kv.Key, json = kv.Value };
            return JsonUtility.ToJson(rec);
        }

        /// <summary>Mengisi ulang dari regionStoreJson. Dipanggil sebelum region dimuat (WorldStateSave.md urutan 2).</summary>
        public void LoadFromSaveJson(string json)
        {
            Clear();
            if (string.IsNullOrEmpty(json)) return;
            StoreRecord rec = null;
            try { rec = JsonUtility.FromJson<StoreRecord>(json); }
            catch (Exception e) { Debug.LogWarning($"[RegionStateStore] Data tidak terbaca: {e.Message}"); }
            if (rec == null) return;
            if (rec.lastUnloadedGameHour != null)
                for (int i = 0; i < RegionCount && i < rec.lastUnloadedGameHour.Length; i++) lastUnloadedGameHour[i] = rec.lastUnloadedGameHour[i];
            if (rec.entries == null) return;
            for (int i = 0; i < rec.entries.Length; i++)
            {
                var e = rec.entries[i];
                if (e == null || e.region < 0 || e.region >= RegionCount || string.IsNullOrEmpty(e.id)) continue;
                states[e.region][e.id] = e.json ?? string.Empty;
            }
        }
    }

    /// <summary>
    /// Satu jalur simpan untuk objek dunia yang bisa berada di region scene (#88, WorldStateSave.md "Pendaftaran").
    /// - Ada RegionStreaming dan objek berada di bawah RegionSceneRoot: status dipegang RegionStateStore lewat
    ///   RegionStreaming.CaptureRegion/RestoreRegion. Objek TIDAK mendaftar ke WorldStateSave (tidak tersimpan dua kali).
    /// - Selain itu (scene uji tanpa RegionStreaming, atau objek di luar region scene): jalur cadangan WorldStateSave.Register.
    /// Keputusan sesi induk: scene uji tanpa RegionStreaming tetap bisa save/load.
    /// </summary>
    public static class RegionSaveRouting
    {
        /// <summary>true kalau status objek ini dipegang RegionStateStore.</summary>
        public static bool IsStoredByRegion(Component owner)
        {
            // GetComponentInParent hanya berjalan naik ke induk, jadi aman di Awake/OnEnable ("Hitch aktivasi").
            return RegionStreaming.Instance != null && owner != null && owner.GetComponentInParent<RegionSceneRoot>(true) != null;
        }

        /// <summary>
        /// Daftarkan ke WorldStateSave hanya kalau objek tidak dipegang RegionStateStore.
        /// Mengembalikan true kalau terdaftar; simpan nilainya untuk Unregister.
        /// </summary>
        public static bool Register(ISaveable saveable, Component owner)
        {
            if (saveable == null || string.IsNullOrEmpty(saveable.SaveId)) return false;
            if (IsStoredByRegion(owner)) return false;
            var save = WorldStateSave.Instance;
            if (save == null) return false;
            save.Register(saveable);
            return true;
        }

        /// <summary>Lepas pendaftaran cadangan kalau tadi terdaftar.</summary>
        public static void Unregister(ISaveable saveable, ref bool registered)
        {
            if (registered && WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(saveable);
            registered = false;
        }
    }
}
