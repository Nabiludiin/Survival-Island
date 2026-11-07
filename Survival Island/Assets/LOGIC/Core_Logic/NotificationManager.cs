using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    public enum NotificationKind : byte { Info, Gain, Warning, Danger, Objective }

    /// <summary>Data satu pesan yang tampil atau menunggu (NotificationManager.md bagian 1).</summary>
    public struct NotificationEntry
    {
        public string text;        // Teks akhir (untuk Gain: nama item, teks dibuat saat tampil)
        public string mergeKey;    // itemId untuk Gain, key untuk yang lain
        public NotificationKind kind;
        public int amount;         // Jumlah Gain, atau hitungan pesan yang sama
        public float remaining;    // Sisa waktu tampil, dalam detik
        public float age;          // Umur sejak terakhir digabung, untuk jendela mergeWindowSeconds
        public float shownTime;    // Lama sudah tampil, untuk fade masuk
    }

    /// <summary>Data satu baris riwayat pesan. hour = -1 di Fase 1 (ditulis "Kapal").</summary>
    public struct NotificationLogEntry
    {
        public string text;
        public NotificationKind kind;
        public int day, hour;
    }

    /// <summary>Warna, ikon, dan klip per NotificationKind. Warna tidak menjadi satu-satunya penanda (ada ikon).</summary>
    [Serializable]
    public struct NotificationStyle
    {
        public Color color;
        public Sprite icon;
        public AudioClip clip;   // Kosong = tanpa suara (Info)
    }

    /// <summary>
    /// Pesan kecil di pojok layar ("+5 Kayu", "Inventori penuh", "Suku cadang ditemukan") dan riwayat pesan satu run.
    /// Blueprint: Core_Logic/NotificationManager.md (#22). Mendukung GDD bagian 3, 5, dan 6; bentuk notifikasi dan log
    /// adalah usulan teknis.
    ///
    /// Urutan bootstrap ke-10, memakai canvas persisten di atas HUD. Baris dibuat sekali di Initialize dan dipakai ulang.
    /// Usulan disetujui developer (#22 poin 1): sistem lain memanggil Show langsung; event hanya untuk tabel di blueprint.
    /// </summary>
    public class NotificationManager : MonoBehaviour, IBootstrapService
    {
        public static NotificationManager Instance { get; private set; }

        [Header("Tampilan (canvas persisten, pojok kanan atas)")]
        [SerializeField] private CanvasGroup canvasGroup;
        [Tooltip("Prefab baris. Disalin sebanyak notification.maxVisible saat Initialize.")]
        [SerializeField] private NotificationRow rowPrefab;
        [SerializeField] private RectTransform rowContainer;
        [Tooltip("Kecepatan geser baris, dalam unit UI per detik (data prefab).")]
        [SerializeField] private float rowMoveSpeed = 600f;
        [Tooltip("Satu per NotificationKind, urut: Info, Gain, Warning, Danger, Objective.")]
        [SerializeField] private NotificationStyle[] kindStyles = new NotificationStyle[5];

        private NotificationRow[] rows = new NotificationRow[0];
        private readonly List<NotificationEntry> visible = new List<NotificationEntry>(8);
        private readonly List<NotificationEntry> queue = new List<NotificationEntry>(16);
        private NotificationLogEntry[] log = new NotificationLogEntry[0];
        private string[] logMergeKeys = new string[0];
        private int logHead, logCount;
        private readonly HashSet<string> announcedParts = new HashSet<string>();
        private bool isHeld;
        private bool initialized;

        private static readonly NotificationBalance DefaultBalance = new NotificationBalance();
        private static NotificationBalance Cfg => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.notification : DefaultBalance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[NotificationManager] Instance ganda. Periksa prefab PersistentRoot.");
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<InventoryChangedEvent>.Raised += HandleInventoryChanged;
            EventChannel<HeavyItemChangedEvent>.Raised += HandleHeavyItemChanged;
            EventChannel<HelicopterPartInstalledEvent>.Raised += HandlePartInstalled;
            EventChannel<HelicopterReadyEvent>.Raised += HandleHelicopterReady;
            EventChannel<RegionUnlockedEvent>.Raised += HandleRegionUnlocked;
            EventChannel<NPCRescuedEvent>.Raised += HandleNPCRescued;
            EventChannel<StructureDestroyedEvent>.Raised += HandleStructureDestroyed;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            EventChannel<DiaryChapterUnlockedEvent>.Raised += HandleDiaryChapter;   // ObjectiveManager (#81)
            EventChannel<NPCDiedEvent>.Raised += HandleNPCDied;                     // NPCRoster/NPCCompanion (#76)
            EventChannel<StructureBurningEvent>.Raised += HandleStructureBurning;   // Flammable (#44)
        }

        private void OnDisable()
        {
            EventChannel<InventoryChangedEvent>.Raised -= HandleInventoryChanged;
            EventChannel<HeavyItemChangedEvent>.Raised -= HandleHeavyItemChanged;
            EventChannel<HelicopterPartInstalledEvent>.Raised -= HandlePartInstalled;
            EventChannel<HelicopterReadyEvent>.Raised -= HandleHelicopterReady;
            EventChannel<RegionUnlockedEvent>.Raised -= HandleRegionUnlocked;
            EventChannel<NPCRescuedEvent>.Raised -= HandleNPCRescued;
            EventChannel<StructureDestroyedEvent>.Raised -= HandleStructureDestroyed;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            EventChannel<DiaryChapterUnlockedEvent>.Raised -= HandleDiaryChapter;
            EventChannel<NPCDiedEvent>.Raised -= HandleNPCDied;
            EventChannel<StructureBurningEvent>.Raised -= HandleStructureBurning;
        }

        // ---------------------------------------------------------------- Layanan

        public void Initialize()
        {
            var c = Cfg;
            int max = Mathf.Max(1, c.maxVisible);
            if (rowPrefab != null)
            {
                // Dibuat satu kali; tidak ada Instantiate saat pesan tampil.
                var parent = rowContainer != null ? rowContainer : (RectTransform)rowPrefab.transform.parent;
                rows = new NotificationRow[max];
                for (int i = 0; i < max; i++)
                {
                    rows[i] = Instantiate(rowPrefab, parent);
                    rows[i].SetVisible(false);
                }
                if (rowPrefab.gameObject.scene.IsValid()) rowPrefab.gameObject.SetActive(false);
            }
            else Debug.LogError("[NotificationManager] Rujukan 'rowPrefab' kosong di prefab PersistentRoot.");

            int cap = Mathf.Max(1, c.logCapacity);
            log = new NotificationLogEntry[cap];
            logMergeKeys = new string[cap];
            logHead = 0;
            logCount = 0;
            initialized = true;
        }

        /// <summary>Permadeath: riwayat dan suku cadang yang sudah diumumkan tidak ikut ke run baru.</summary>
        public void ResetForNewRun()
        {
            ClearDisplay();
            logHead = 0;
            logCount = 0;
            Array.Clear(log, 0, log.Length);
            Array.Clear(logMergeKeys, 0, logMergeKeys.Length);
            announcedParts.Clear();
        }

        // ---------------------------------------------------------------- Fungsi publik

        /// <summary>Pesan dari kunci notif.* (#104). Teks kosong dibuang.</summary>
        public void Show(string key, NotificationKind kind = NotificationKind.Info)
        {
            string text = Localize(key);
            if (string.IsNullOrEmpty(text)) return;
            Enqueue(text, key, kind, 1);
        }

        public void ShowFormat(string key, NotificationKind kind, string name, string value)
        {
            var loc = LocalizationManager.Instance;
            string text = loc != null ? loc.Format(key, name, value) : key;
            if (string.IsNullOrEmpty(text)) return;
            Enqueue(text, key, kind, 1);
        }

        /// <summary>Dua pasangan, misalnya {installed} dan {total} untuk "Suku cadang terpasang 2/5".</summary>
        public void ShowFormat(string key, NotificationKind kind, string name1, string value1, string name2, string value2)
        {
            var loc = LocalizationManager.Instance;
            string text = loc != null ? loc.Format(key, name1, value1, name2, value2) : key;
            if (string.IsNullOrEmpty(text)) return;
            Enqueue(text, key, kind, 1);
        }

        /// <summary>"+{amount} {nama}" dengan nama dari ItemData.displayNameKey (item.&lt;itemId&gt;.name).</summary>
        public void ShowGain(string itemId, int delta)
        {
            if (string.IsNullOrEmpty(itemId) || delta <= 0) return;
            var data = ItemDatabase.Get(itemId);
            if (data == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"[NotificationManager] Item '{itemId}' tidak ada di ItemDatabase.");
#endif
                return;
            }
            string nameKey = !string.IsNullOrEmpty(data.displayNameKey) ? data.displayNameKey : "item." + itemId + ".name";
            Enqueue(Localize(nameKey), itemId, NotificationKind.Gain, delta);
        }

        /// <summary>Salin riwayat ke output dari yang terbaru. Mengembalikan jumlah baris.</summary>
        public int GetLog(List<NotificationLogEntry> output)
        {
            if (output == null) return 0;
            output.Clear();
            for (int i = 0; i < logCount; i++)
            {
                int idx = (logHead - 1 - i + log.Length * 2) % log.Length;
                output.Add(log[idx]);
            }
            return logCount;
        }

        // ---------------------------------------------------------------- Antrean

        private void Enqueue(string text, string mergeKey, NotificationKind kind, int amount)
        {
            var c = Cfg;

            // 2. Gabungkan dengan pesan sejenis yang masih di dalam jendela waktu.
            if (TryMerge(visible, mergeKey, kind, amount, c, true, out var merged) ||
                TryMerge(queue, mergeKey, kind, amount, c, false, out merged))
            {
                WriteLog(merged, true);
                return;
            }

            var entry = new NotificationEntry
            {
                text = text, mergeKey = mergeKey, kind = kind, amount = amount,
                remaining = DurationFor(kind, c), age = 0f, shownTime = 0f
            };

            // 1. Riwayat ditulis untuk semua pesan, termasuk yang nanti dibuang dari antrean.
            WriteLog(entry, false);

            // 3. Tampil langsung kalau ada baris kosong.
            if (initialized && !isHeld && visible.Count < rows.Length)
            {
                ShowEntry(entry);
                return;
            }

            // Danger saat baris penuh: baris Info atau Gain tertua langsung pudar, supaya peringatan tidak menunggu.
            if (kind == NotificationKind.Danger && initialized && !isHeld) ForceFadeOldestMinor(c);

            // 4. Danger dan Objective masuk di depan Info dan Gain.
            if (IsImportant(kind))
            {
                int insertAt = 0;
                while (insertAt < queue.Count && IsImportant(queue[insertAt].kind)) insertAt++;
                queue.Insert(insertAt, entry);
            }
            else queue.Add(entry);

            // 5. Antrean penuh: buang Info atau Gain tertua. Danger dan Objective tidak dibuang.
            int capacity = Mathf.Max(1, c.queueCapacity);
            while (queue.Count > capacity)
            {
                int drop = -1;
                for (int i = 0; i < queue.Count; i++)
                    if (!IsImportant(queue[i].kind)) { drop = i; break; }
                if (drop < 0) break;
                queue.RemoveAt(drop);
            }
        }

        private bool TryMerge(List<NotificationEntry> list, string mergeKey, NotificationKind kind, int amount,
            NotificationBalance c, bool isVisible, out NotificationEntry merged)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var e = list[i];
                if (e.kind != kind || e.mergeKey != mergeKey || e.age >= c.mergeWindowSeconds) continue;
                e.amount += kind == NotificationKind.Gain ? amount : 1;
                e.remaining = DurationFor(kind, c);   // Waktu tampil diulang
                e.age = 0f;
                list[i] = e;
                if (isVisible && i < rows.Length && rows[i] != null) rows[i].SetText(DisplayText(e));
                merged = e;
                return true;
            }
            merged = default;
            return false;
        }

        private void ShowEntry(in NotificationEntry entry)
        {
            int i = visible.Count;
            visible.Add(entry);
            var row = rows[i];
            if (row == null) return;
            var style = StyleFor(entry.kind);
            row.Bind(DisplayText(entry), style);
            row.SetAlpha(0f);
            row.SetVisible(true);
            row.MoveTowards(TargetY(i), float.MaxValue);
            // Gain hanya berbunyi pada baris baru (penggabungan tidak berbunyi).
            if (style.clip != null && AudioManager.Instance != null) AudioManager.Instance.PlaySFX(style.clip);
        }

        private void ForceFadeOldestMinor(NotificationBalance c)
        {
            for (int i = 0; i < visible.Count; i++)
            {
                if (IsImportant(visible[i].kind)) continue;
                var e = visible[i];
                e.remaining = Mathf.Min(e.remaining, c.fadeSeconds);
                visible[i] = e;
                return;
            }
        }

        private static bool IsImportant(NotificationKind k) => k == NotificationKind.Danger || k == NotificationKind.Objective;

        private static float DurationFor(NotificationKind k, NotificationBalance c)
        {
            // Usulan disetujui developer (#22 poin 4): 3 detik untuk pesan biasa, 5 detik untuk pesan penting.
            return k == NotificationKind.Info || k == NotificationKind.Gain ? c.infoSeconds : c.warningSeconds;
        }

        private static string DisplayText(in NotificationEntry e)
        {
            if (e.kind == NotificationKind.Gain) return "+" + e.amount + " " + e.text;
            return e.amount > 1 ? e.text + " x" + e.amount : e.text;
        }

        private NotificationStyle StyleFor(NotificationKind k)
        {
            int i = (int)k;
            return kindStyles != null && i < kindStyles.Length ? kindStyles[i] : new NotificationStyle { color = Color.white };
        }

        // ---------------------------------------------------------------- Riwayat

        private void WriteLog(in NotificationEntry e, bool isMerge)
        {
            if (log.Length == 0) return;
            string text = DisplayText(e);

            // Gain yang digabung menimpa baris log terakhir dengan itemId yang sama, supaya log tidak penuh "+1 Kayu".
            if (isMerge && e.kind == NotificationKind.Gain && logCount > 0)
            {
                int last = (logHead - 1 + log.Length) % log.Length;
                if (logMergeKeys[last] == e.mergeKey && log[last].kind == NotificationKind.Gain)
                {
                    log[last].text = text;
                    return;
                }
            }

            var gm = GameManager.Instance;
            int day = gm != null ? gm.currentDay : 1;
            int hour = -1;   // Fase 1: ditulis "Kapal"
            if (gm != null && gm.currentPhase == GamePhase.IslandSurvival && EventChannel<HourChangedEvent>.HasLast)
                hour = EventChannel<HourChangedEvent>.Last.hour;

            log[logHead] = new NotificationLogEntry { text = text, kind = e.kind, day = day, hour = hour };
            logMergeKeys[logHead] = e.mergeKey;
            logHead = (logHead + 1) % log.Length;
            if (logCount < log.Length) logCount++;
        }

        // ---------------------------------------------------------------- Per frame

        private void Update()
        {
            if (!initialized) return;
            // 1. Ditahan selama jeda atau adegan: waktu tampil tidak berkurang.
            RefreshHeld();
            if (isHeld) return;

            var c = Cfg;
            float dt = Time.deltaTime;   // Berhenti saat Time.timeScale = 0
            float fade = Mathf.Max(0.0001f, c.fadeSeconds);

            for (int i = 0; i < queue.Count; i++)
            {
                var q = queue[i];
                q.age += dt;
                queue[i] = q;
            }

            // 2-3. Kurangi waktu; baris habis dikosongkan, lalu diisi dari antrean.
            for (int i = visible.Count - 1; i >= 0; i--)
            {
                var e = visible[i];
                e.remaining -= dt;
                e.age += dt;
                e.shownTime += dt;
                visible[i] = e;
                if (e.remaining <= 0f) RemoveVisible(i);
            }
            while (visible.Count < rows.Length && queue.Count > 0)
            {
                var next = queue[0];
                queue.RemoveAt(0);
                next.shownTime = 0f;
                ShowEntry(next);
            }

            // 4. Alpha dan posisi tiap baris. Pesan terbaru di bawah (usulan #22 poin 3).
            float y = 0f;
            for (int i = 0; i < visible.Count; i++)
            {
                var row = rows[i];
                if (row == null) continue;
                var e = visible[i];
                float alpha = Mathf.Min(Mathf.Clamp01(e.shownTime / fade), Mathf.Clamp01(e.remaining / fade));
                row.SetAlpha(alpha);
                row.MoveTowards(-y, rowMoveSpeed * dt);
                y += row.Height;
            }
        }

        private float TargetY(int index)
        {
            float y = 0f;
            for (int i = 0; i < index && i < rows.Length; i++) if (rows[i] != null) y += rows[i].Height;
            return -y;
        }

        private void RemoveVisible(int index)
        {
            visible.RemoveAt(index);
            // Baris di bawahnya naik satu: isi ulang baris UI dari data supaya rows[i] selalu sesuai visible[i].
            for (int i = index; i < visible.Count; i++)
            {
                var row = rows[i];
                if (row == null) continue;
                row.Bind(DisplayText(visible[i]), StyleFor(visible[i].kind));
                if (rows[i + 1] != null) row.Rect.anchoredPosition = rows[i + 1].Rect.anchoredPosition;
            }
            if (visible.Count < rows.Length && rows[visible.Count] != null) rows[visible.Count].SetVisible(false);
        }

        /// <summary>
        /// Status ditahan dibaca langsung dari UIFocusStack, karena UIFocusChangedEvent tidak terbit saat Cinematic
        /// didorong di atas panel yang tidak mengubah status jeda (NotificationManager.md, "Tampilan saat ada panel").
        /// </summary>
        private void RefreshHeld()
        {
            var stack = UIFocusStack.Instance;
            bool held = false;
            if (stack != null)
            {
                var top = stack.TopPanel;
                bool cinematic = top != null && top.id == UIPanelId.Cinematic;
                held = stack.IsPaused || cinematic;
            }
            if (held != isHeld)
            {
                isHeld = held;
                if (canvasGroup != null) canvasGroup.alpha = isHeld ? 0f : 1f;
            }
        }

        private void ClearDisplay()
        {
            visible.Clear();
            queue.Clear();
            for (int i = 0; i < rows.Length; i++) if (rows[i] != null) rows[i].SetVisible(false);
        }

        // ---------------------------------------------------------------- Event

        // Event saat Loading diabaikan, supaya pemindahan loot kapal, pemulihan save, dan event sticky yang dikirim ulang
        // tidak membanjiri layar (NotificationManager.md, "Event saat memuat").
        private static bool IsLoading()
        {
            var stack = UIFocusStack.Instance;
            var top = stack != null ? stack.TopPanel : null;
            return top != null && top.id == UIPanelId.Loading;
        }

        private void HandleInventoryChanged(InventoryChangedEvent e)
        {
            // Usulan disetujui developer (#22 poin 5): barang berkurang tidak ditampilkan.
            if (IsLoading() || e.delta <= 0) return;
            ShowGain(e.itemId, e.delta);
        }

        private void HandleHeavyItemChanged(HeavyItemChangedEvent e)
        {
            if (IsLoading() || !e.isCarried || string.IsNullOrEmpty(e.partId)) return;
            // Menjatuhkan lalu mengambil ulang tidak mengulang pesan.
            if (!announcedParts.Add(e.partId)) return;
            var data = ItemDatabase.Get(e.partId);
            string nameKey = data != null && !string.IsNullOrEmpty(data.displayNameKey) ? data.displayNameKey : "item." + e.partId + ".name";
            ShowFormat("notif.part_found", NotificationKind.Objective, "name", Localize(nameKey));
        }

        private void HandlePartInstalled(HelicopterPartInstalledEvent e)
        {
            if (IsLoading()) return;
            ShowFormat("notif.part_installed", NotificationKind.Objective,
                "installed", e.installed.ToString(), "total", e.total.ToString());
        }

        private void HandleHelicopterReady(HelicopterReadyEvent e)
        {
            if (IsLoading()) return;
            Show("notif.helicopter_ready", NotificationKind.Objective);
        }

        private void HandleRegionUnlocked(RegionUnlockedEvent e)
        {
            if (IsLoading()) return;
            Show("notif.region_unlocked", NotificationKind.Objective);
        }

        private void HandleNPCRescued(NPCRescuedEvent e)
        {
            if (IsLoading()) return;
            ShowFormat("notif.npc_rescued", NotificationKind.Info, "name", NpcName(e.npcId));
        }

        private void HandleNPCDied(NPCDiedEvent e)
        {
            if (IsLoading()) return;
            ShowFormat("notif.npc_died", NotificationKind.Danger, "name", NpcName(e.npcId));
        }

        private void HandleDiaryChapter(DiaryChapterUnlockedEvent e)
        {
            if (IsLoading()) return;
            Show("notif.diary_page", NotificationKind.Objective);   // "Halaman baru di Diary"
        }

        private void HandleStructureBurning(StructureBurningEvent e)
        {
            if (IsLoading() || !e.isBurning) return;
            Show("notif.base_burning", NotificationKind.Danger);
        }

        /// <summary>Nama NPC dari NPCData.displayNameKey (#76). Tanpa NPCRoster atau data, npcId dipakai sebagai nama.</summary>
        private static string NpcName(string npcId)
        {
            var data = NPCRoster.Instance != null ? NPCRoster.Instance.GetData(npcId) : null;
            if (data == null || string.IsNullOrEmpty(data.displayNameKey)) return npcId;
            return Localize(data.displayNameKey);
        }

        private void HandleStructureDestroyed(StructureDestroyedEvent e)
        {
            if (IsLoading()) return;
            Show("notif.structure_destroyed", NotificationKind.Warning);
        }

        private void HandleRunEnded(RunEndedEvent e) => ClearDisplay();

        private static string Localize(string key)
        {
            var loc = LocalizationManager.Instance;
            return loc != null ? loc.Get(key) : key;
        }

        // Perintah "notify <jenis> <kunci>" ada di DevConsole.CmdNotify (#56).
    }
}
