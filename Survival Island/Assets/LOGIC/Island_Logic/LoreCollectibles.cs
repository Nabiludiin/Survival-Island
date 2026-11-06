using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// File lore_archive.json: lore yang pernah ditemukan di run mana pun. Tidak dihapus DeleteRunSave.
    /// Usulan disetujui developer (#100): teks tetap terbuka setelah permadeath, seperti stats.json (#57), karena teks tidak
    /// memberi keuntungan permainan. File rusak dianggap kosong, lalu dicatat.
    /// </summary>
    public static class LoreArchiveFile
    {
        public const string FileName = "lore_archive.json";

        [Serializable]
        private class ArchiveData
        {
            public int version = 1;
            public string[] loreIds = Array.Empty<string>();
        }

        public static string FilePath => Path.Combine(Application.persistentDataPath, FileName);

        public static HashSet<string> Load()
        {
            var set = new HashSet<string>(StringComparer.Ordinal);
            try
            {
                string path = FilePath;
                if (!File.Exists(path)) return set;
                var d = JsonUtility.FromJson<ArchiveData>(File.ReadAllText(path));
                if (d?.loreIds != null)
                    for (int i = 0; i < d.loreIds.Length; i++) if (!string.IsNullOrEmpty(d.loreIds[i])) set.Add(d.loreIds[i]);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[LoreArchiveFile] {FileName} rusak dan dianggap kosong: {ex.Message}");
                set.Clear();
            }
            return set;
        }

        public static void Save(HashSet<string> archive)
        {
            if (archive == null) return;
            try
            {
                var ids = new string[archive.Count];
                archive.CopyTo(ids);
                Array.Sort(ids, StringComparer.Ordinal);
                string path = FilePath;
                string tmp = path + ".tmp";
                File.WriteAllText(tmp, JsonUtility.ToJson(new ArchiveData { loreIds = ids }));
                if (File.Exists(path)) File.Delete(path);
                File.Move(tmp, path);
            }
            catch (Exception ex)
            {
                // Arsip hanya kenyamanan menu. Run tetap berjalan.
                Debug.LogWarning($"[LoreArchiveFile] Gagal menulis {FileName}: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Temuan cerita opsional per region yang bisa dibaca ulang. Blueprint: Island_Logic/LoreCollectibles.md (#100).
    /// GDD bagian 2: pulau bekas eksperimen dan jejak militer. GDD bagian 5: Region 4 pusat lore.
    /// Sebaran, petunjuk, dan arsip lintas run adalah usulan disetujui developer.
    /// Usulan disetujui 2: petunjuk gameplay sangat terbatas, maxPartHintsPerRun (2 log militer Region 4).
    ///
    /// Manager scene IslandPhase (world_lore, LoadOrder 110). Diary utama dan tujuan pemain milik ObjectiveManager.
    ///
    /// Catatan implementasi (sistem pendukung): tab Arsip di DiaryArchivePanel (kode lama) belum ditambahkan. Fungsi
    /// GetKnownEntries, CountEntries, dan OpenReader disediakan untuk tab itu.
    /// </summary>
    public class LoreCollectibles : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "world_lore";

        public static LoreCollectibles Instance { get; private set; }

        [Tooltip("Sekitar 20 aset LoreEntry.")]
        [SerializeField] private LoreEntry[] allEntries = Array.Empty<LoreEntry>();
        [SerializeField] private LoreReaderPanel loreReaderPanel;

        private readonly List<string> collectedThisRun = new List<string>(32);
        private readonly List<string> grantedHints = new List<string>(4);
        private HashSet<string> archive = new HashSet<string>(StringComparer.Ordinal);
        private bool registeredForSave;

        public IReadOnlyList<string> CollectedThisRun => collectedThisRun;

        private static readonly Comparison<LoreEntry> BySortOrder = (a, b) => a.sortOrder.CompareTo(b.sortOrder);

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[LoreCollectibles] Instance ganda di scene IslandPhase.");
                enabled = false;
                return;
            }
            Instance = this;
            archive = LoreArchiveFile.Load();
        }

        private void OnEnable()
        {
            if (WorldStateSave.Instance != null)
            {
                WorldStateSave.Instance.Register(this);
                registeredForSave = true;
            }
        }

        private void OnDisable()
        {
            if (registeredForSave && WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            registeredForSave = false;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Menemukan

        public bool IsCollected(string loreId) => !string.IsNullOrEmpty(loreId) && collectedThisRun.Contains(loreId);

        public bool IsInArchive(string loreId) => !string.IsNullOrEmpty(loreId) && archive.Contains(loreId);

        /// <summary>Dipanggil LorePoint.OnInteract. Membaca ulang tidak mengirim event dan tidak memberi petunjuk baru.</summary>
        public void Collect(LoreEntry e)
        {
            if (e == null || string.IsNullOrEmpty(e.loreId)) return;
            // 1-2.
            bool isNew = !collectedThisRun.Contains(e.loreId);
            if (isNew)
            {
                collectedThisRun.Add(e.loreId);
                GrantHint(e);
            }
            // 3.
            bool firstEver = archive.Add(e.loreId);
            if (firstEver) LoreArchiveFile.Save(archive);
            // 4.
            if (isNew) EventChannel<LoreFoundEvent>.Publish(new LoreFoundEvent(e.loreId, e.region, firstEver));
            // 5. Panel LoreReader menjeda game (UIFocusStack.md).
            OpenReader(e);
        }

        /// <summary>Membuka LoreReader tanpa memberi petunjuk. Dipakai Collect dan tab Arsip.</summary>
        public void OpenReader(LoreEntry e)
        {
            if (e == null || loreReaderPanel == null || UIFocusStack.Instance == null) return;
            loreReaderPanel.SetEntry(e);
            UIFocusStack.Instance.Push(loreReaderPanel);
        }

        private void GrantHint(LoreEntry e)
        {
            if (e.hint == LoreHintKind.None || grantedHints.Contains(e.loreId)) return;
            if (e.hint == LoreHintKind.PartLocation)
            {
                var bal = GameManager.CurrentBalance;
                int max = bal != null ? bal.lore.maxPartHintsPerRun : 2;
                if (CountHints(LoreHintKind.PartLocation) >= max) return;
            }
            grantedHints.Add(e.loreId);
            // HeavyItemSystem (#69) menandai satu suku cadang yang belum ditemukan, indeks terkecil.
            EventChannel<LoreHintGrantedEvent>.Publish(new LoreHintGrantedEvent(e.loreId, e.hint, e.region));
        }

        private int CountHints(LoreHintKind kind)
        {
            int n = 0;
            for (int i = 0; i < grantedHints.Count; i++)
            {
                var entry = FindEntry(grantedHints[i]);
                if (entry != null && entry.hint == kind) n++;
            }
            return n;
        }

        public LoreEntry FindEntry(string loreId)
        {
            for (int i = 0; i < allEntries.Length; i++)
                if (allEntries[i] != null && string.Equals(allEntries[i].loreId, loreId, StringComparison.Ordinal)) return allEntries[i];
            return null;
        }

        // ---------------------------------------------------------------- Menu koleksi

        /// <summary>
        /// Isi tab Arsip satu region: collectedThisRun ditambah archive, urut sortOrder. Temuan yang belum pernah ditemukan
        /// tidak masuk. Mengembalikan jumlah yang dimasukkan ke output.
        /// </summary>
        public int GetKnownEntries(RegionType region, List<LoreEntry> output)
        {
            if (output == null) return 0;
            output.Clear();
            for (int i = 0; i < allEntries.Length; i++)
            {
                var e = allEntries[i];
                if (e == null || e.region != region) continue;
                if (collectedThisRun.Contains(e.loreId) || archive.Contains(e.loreId)) output.Add(e);
            }
            output.Sort(BySortOrder);
            return output.Count;
        }

        /// <summary>Jumlah semua temuan di region, untuk judul kelompok "3/7".</summary>
        public int CountEntries(RegionType region)
        {
            int n = 0;
            for (int i = 0; i < allEntries.Length; i++) if (allEntries[i] != null && allEntries[i].region == region) n++;
            return n;
        }

        // ---------------------------------------------------------------- Save (#80)

        [Serializable]
        private class LoreSaveData
        {
            public string[] collectedThisRun = Array.Empty<string>();
            public string[] grantedHints = Array.Empty<string>();
        }

        public string SaveId => SaveIdConst;
        public int LoadOrder => 110;

        public string CaptureState()
        {
            return JsonUtility.ToJson(new LoreSaveData
            {
                collectedThisRun = collectedThisRun.ToArray(),
                grantedHints = grantedHints.ToArray(),
            });
        }

        public void RestoreState(string json)
        {
            // Tidak menerbitkan LoreFoundEvent lagi.
            collectedThisRun.Clear();
            grantedHints.Clear();
            if (string.IsNullOrEmpty(json)) return;
            var d = JsonUtility.FromJson<LoreSaveData>(json);
            if (d == null) return;
            if (d.collectedThisRun != null) collectedThisRun.AddRange(d.collectedThisRun);
            if (d.grantedHints != null) grantedHints.AddRange(d.grantedHints);
        }

        public void OnRestoreFinished() { }
    }
}
