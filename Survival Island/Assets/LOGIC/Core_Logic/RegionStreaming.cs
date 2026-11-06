using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace SurvivalIsland
{
    public enum RegionLoadState { Unloaded, Loading, Loaded, Unloading }

    /// <summary>Data satu region (RegionStreaming.md bagian "Data dan variabel"). Diisi di Inspector.</summary>
    [Serializable]
    public class RegionRecord
    {
        public RegionType region;
        public string sceneName;                // Region1_Volcano, Region2_Amazon, Region3_Snow, Region4_Village (GLOSSARY.md)
        public Bounds bounds;
        public GameObject farMesh;
        public Transform structureRoot;         // Akar bangunan pemain di region ini (di IslandPhase)
        [NonSerialized] public RegionLoadState state;
        public float lastUnloadedGameHour = -1f; // Disalin dari dan ke RegionStateStore
    }

    /// <summary>Sambungan dua region: S1, S2, S3 (RegionStreaming.md "Peta konsep").</summary>
    [Serializable]
    public class RegionSeam
    {
        public string seamId;
        public RegionType a, b;
        public Vector3 position;
        public SeamBarrier barrier;
        public NavMeshLink[] links;             // Satu per agent type
    }

    /// <summary>
    /// Pemuatan 4 region secara additive berdasarkan jarak pemain ke seam. Manager di scene IslandPhase.
    /// Blueprint: Core_Logic/RegionStreaming.md (#88). GDD bagian 5: 4 Region, jembatan, dan perjalanan Region 4 ke Region 3.
    /// Struktur scene, jarak, dan batas memori adalah usulan teknis yang disetujui developer (#85).
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - Data region dan seam (koordinat Bounds, posisi seam, link) diisi di Inspector. Layout peta usulan ada di blueprint;
    ///   scene region belum dibuat, jadi tidak ada angka koordinat di kode.
    /// - Panel Loading saat tertahan lama dicari sekali dengan FindFirstObjectByType (RunLifecycle tidak membuka API).
    /// - WorldItemManager.CaptureRegion/RestoreRegion (#66) belum ada: TODO di CaptureRegion dan RestoreRegion.
    /// - Perintah streamstat dan streamdelay (#56) milik DevConsole; kelas ini membuka DebugExtraDelaySeconds dan DebugStatLine.
    /// </summary>
    public class RegionStreaming : MonoBehaviour
    {
        public static RegionStreaming Instance { get; private set; }

        [SerializeField] private RegionRecord[] regions = new RegionRecord[4];
        [SerializeField] private RegionSeam[] seams = new RegionSeam[3];

        public RegionStateStore Store { get; private set; } = new RegionStateStore();
        public bool IsReady { get; private set; }
        public bool IsStalled { get; private set; }

        /// <summary>Penundaan buatan sebelum aktivasi, untuk perintah streamdelay di DevConsole (#56).</summary>
        public float DebugExtraDelaySeconds { get; set; }

        private struct PendingOp
        {
            public RegionType region;
            public bool load;
            public bool urgent;
        }

        private readonly List<PendingOp> queue = new List<PendingOp>(8);
        private readonly float[] lastLoadSeconds = new float[4];
        private Coroutine evaluationRoutine;
        private Coroutine workerRoutine;
        private bool runEnded;
        private float lastUnusedAssetsTime = -999f;
        private float stallTimer;
        private UIPanel stallPanel;
        private bool stallPanelPushed;
        private int urgentRequests;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;
        private static readonly StreamingBalance FallbackStreaming = new StreamingBalance();
        private static StreamingBalance B => Balance != null ? Balance.streaming : FallbackStreaming;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
            for (int i = 0; i < regions.Length; i++)
                if (regions[i] != null) regions[i].state = RegionLoadState.Unloaded;
        }

        private void OnEnable()
        {
            EventChannel<RegionUnlockedEvent>.Raised += HandleRegionUnlocked;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
        }

        private void OnDisable()
        {
            EventChannel<RegionUnlockedEvent>.Raised -= HandleRegionUnlocked;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void HandleRegionUnlocked(RegionUnlockedEvent e)
        {
            // Periksa ulang jarak dan link.
            RefreshLinks();
            EvaluateOnce();
        }

        private void HandleRunEnded(RunEndedEvent e)
        {
            // Run berakhir: hentikan antrean. Operasi yang berjalan dibiarkan selesai (scene akan diganti saat reset).
            runEnded = true;
            queue.Clear();
            if (evaluationRoutine != null) StopCoroutine(evaluationRoutine);
            evaluationRoutine = null;
        }

        // ---------------------------------------------------------------- Fungsi publik

        /// <summary>Region dari Bounds (bidang XZ), tanpa trigger. Kalau di luar semua Bounds, region terdekat.</summary>
        public RegionType GetRegionAt(Vector3 position)
        {
            float best = float.MaxValue;
            RegionType bestRegion = RegionType.Volcano;
            for (int i = 0; i < regions.Length; i++)
            {
                var r = regions[i];
                if (r == null) continue;
                var b = r.bounds;
                if (position.x >= b.min.x && position.x <= b.max.x && position.z >= b.min.z && position.z <= b.max.z) return r.region;
                Vector3 c = b.center;
                float d = (position.x - c.x) * (position.x - c.x) + (position.z - c.z) * (position.z - c.z);
                if (d < best) { best = d; bestRegion = r.region; }
            }
            return bestRegion;
        }

        /// <summary>Region di posisi ini, atau ZoneTracker.Active kalau RegionStreaming tidak ada (scene uji).</summary>
        public static RegionType RegionOf(Vector3 position)
        {
            return Instance != null ? Instance.GetRegionAt(position) : ZoneTracker.Active;
        }

        public bool IsLoaded(RegionType region)
        {
            var r = Find(region);
            return r != null && r.state == RegionLoadState.Loaded;
        }

        /// <summary>true kalau region dimuat, atau RegionStreaming tidak ada (scene uji dengan satu scene).</summary>
        public static bool IsRegionLoaded(RegionType region) => Instance == null || Instance.IsLoaded(region);

        public RegionLoadState GetState(RegionType region)
        {
            var r = Find(region);
            return r != null ? r.state : RegionLoadState.Unloaded;
        }

        public Transform GetStructureRoot(RegionType region)
        {
            var r = Find(region);
            return r != null ? r.structureRoot : null;
        }

        /// <summary>Waktu game sejak region dilepas. 0 kalau region dimuat atau belum pernah dilepas.</summary>
        public float GetElapsedGameHours(RegionType region)
        {
            var r = Find(region);
            if (r == null || r.state == RegionLoadState.Loaded || r.lastUnloadedGameHour < 0f) return 0f;
            return Mathf.Max(0f, NowGameHours() - r.lastUnloadedGameHour);
        }

        /// <summary>Ambil status region aktif sebelum save (WorldStateSave.md langkah 4).</summary>
        public void CaptureLoadedRegions()
        {
            for (int i = 0; i < regions.Length; i++)
            {
                var r = regions[i];
                if (r != null && r.state == RegionLoadState.Loaded) CaptureRegion(r.region);
            }
            SyncUnloadHoursToStore();
        }

        /// <summary>Teks satu baris untuk perintah streamstat (#56).</summary>
        public string DebugStatLine(RegionType region)
        {
            var r = Find(region);
            if (r == null) return region + ": tidak ada data";
            return $"{region}: {r.state}, muat terakhir {lastLoadSeconds[(int)region]:0.0} dtk (batas {B.regionLoadTimeLimitSeconds}), objek tersimpan {Store.Count(region)}";
        }

        // ---------------------------------------------------------------- Pemuatan awal

        /// <summary>Dipanggil RunLifecycle (#82) saat panel Loading terbuka.</summary>
        public IEnumerator LoadInitialRegions(Vector3 playerPosition)
        {
            runEnded = false;
            // Store sudah diisi WorldStateSave lebih dulu kalau melanjutkan save (RegionStreaming.md bagian 4).
            SyncUnloadHoursFromStore();

            // 1.
            RegionType start = GetRegionAt(playerPosition);
            // 2.
            Application.backgroundLoadingPriority = ThreadPriority.High;
            // 3.
            yield return LoadRegion(Find(start));
            for (int i = 0; i < seams.Length; i++)
            {
                var s = seams[i];
                if (s == null || (s.a != start && s.b != start)) continue;
                RegionType other = s.a == start ? s.b : s.a;
                if (!IsUnlocked(other)) continue;
                if (Vector3.Distance(playerPosition, s.position) <= B.loadDistance && CountLoadedOrLoading() < B.maxLoadedRegions)
                    yield return LoadRegion(Find(other));
            }
            // 4. RestoreRegion sudah dijalankan di LoadRegion.
            // 5.
            Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
            IsReady = true;
            RefreshBarriersAndLinks();
            if (evaluationRoutine == null) evaluationRoutine = StartCoroutine(EvaluationLoop());
        }

        // ---------------------------------------------------------------- Pemuatan berdasarkan jarak

        private IEnumerator EvaluationLoop()
        {
            while (!runEnded)
            {
                yield return new WaitForSecondsRealtime(Mathf.Max(0.05f, B.checkIntervalSeconds));
                var stack = UIFocusStack.Instance;
                // Game dijeda: tidak memulai operasi baru (kecuali panel Loading milik kita sendiri).
                if (stack != null && stack.IsPaused && !stallPanelPushed) continue;
                EvaluateOnce();
                UpdateStall(B.checkIntervalSeconds);
            }
        }

        private void EvaluateOnce()
        {
            if (!IsReady || runEnded) return;
            var player = IslandPlayerController.Instance;
            if (player == null) return;
            Vector3 p = player.transform.position;
            var b = B;
            RegionType here = GetRegionAt(p);

            // 1. Region tempat pemain berdiri harus Loaded. Kalau belum, pemuatan darurat.
            var hereRec = Find(here);
            if (hereRec != null && hereRec.state != RegionLoadState.Loaded) RequestLoad(here, true);

            bool anyUrgent = false;
            // 2.
            for (int i = 0; i < seams.Length; i++)
            {
                var s = seams[i];
                if (s == null || (s.a != here && s.b != here)) continue;
                RegionType other = s.a == here ? s.b : s.a;
                if (!IsUnlocked(other)) continue;   // RegionGateSystem (#68)
                float d = Vector3.Distance(p, s.position);
                if (d <= b.loadDistance) RequestLoad(other, d <= b.urgentDistance);
                // 4. Prioritas naik kalau region seberang masih Loading saat pemain sudah dekat.
                if (d <= b.urgentDistance && GetState(other) == RegionLoadState.Loading) anyUrgent = true;
            }

            // 3. Region Loaded selain region pemain dilepas kalau jauh dari seam terdekat yang menyentuh region pemain.
            for (int i = 0; i < regions.Length; i++)
            {
                var r = regions[i];
                if (r == null || r.region == here || r.state != RegionLoadState.Loaded) continue;
                float nearest = NearestSeamDistance(p, r.region, here);
                if (nearest >= b.unloadDistance) RequestUnload(r.region);
            }

            Application.backgroundLoadingPriority = anyUrgent || urgentRequests > 0 ? ThreadPriority.High : ThreadPriority.BelowNormal;
        }

        /// <summary>Jarak pemain ke seam terdekat antara region itu dan region pemain. Tanpa seam bersama: tak hingga.</summary>
        private float NearestSeamDistance(Vector3 p, RegionType region, RegionType here)
        {
            float best = float.MaxValue;
            for (int i = 0; i < seams.Length; i++)
            {
                var s = seams[i];
                if (s == null) continue;
                bool touches = (s.a == region && s.b == here) || (s.b == region && s.a == here);
                if (!touches) continue;
                best = Mathf.Min(best, Vector3.Distance(p, s.position));
            }
            return best;
        }

        // ---------------------------------------------------------------- Antrean

        /// <summary>Diabaikan kalau state bukan Unloaded (atau sedang dilepas: disimpan lalu dijalankan setelah selesai).</summary>
        public void RequestLoad(RegionType region, bool urgent = false)
        {
            if (runEnded) return;
            var r = Find(region);
            if (r == null || string.IsNullOrEmpty(r.sceneName)) return;
            if (r.state == RegionLoadState.Loaded)
            {
                // Ada permintaan lepas yang belum jalan: batalkan saja.
                for (int i = queue.Count - 1; i >= 0; i--)
                    if (queue[i].region == region && !queue[i].load) queue.RemoveAt(i);
                return;
            }
            if (r.state == RegionLoadState.Loading) return;
            for (int i = 0; i < queue.Count; i++)
            {
                if (queue[i].region != region || !queue[i].load) continue;
                // Naikkan prioritas permintaan yang sudah ada.
                if (urgent && !queue[i].urgent) { queue.RemoveAt(i); break; }
                return;
            }
            var op = new PendingOp { region = region, load = true, urgent = urgent };
            // Region yang dituju pemain didahulukan.
            if (urgent) queue.Insert(0, op); else queue.Add(op);
            EnsureWorker();
        }

        public void RequestUnload(RegionType region)
        {
            if (runEnded) return;
            var r = Find(region);
            if (r == null || r.state != RegionLoadState.Loaded) return;
            for (int i = 0; i < queue.Count; i++)
                if (queue[i].region == region && !queue[i].load) return;
            queue.Add(new PendingOp { region = region, load = false });
            EnsureWorker();
        }

        private void EnsureWorker()
        {
            if (workerRoutine == null) workerRoutine = StartCoroutine(Worker());
        }

        /// <summary>Satu operasi sekali jalan.</summary>
        private IEnumerator Worker()
        {
            while (queue.Count > 0 && !runEnded)
            {
                // Pilih operasi pertama yang boleh jalan. Muat menunggu kalau batas region tercapai (pengaman maxLoadedRegions).
                int index = -1;
                for (int i = 0; i < queue.Count; i++)
                {
                    if (!queue[i].load || CountLoadedOrLoading() < Mathf.Max(1, B.maxLoadedRegions)) { index = i; break; }
                }
                if (index < 0)
                {
                    yield return null;   // Menunggu region lama dilepas oleh EvaluateOnce
                    continue;
                }
                var op = queue[index];
                queue.RemoveAt(index);
                var r = Find(op.region);
                if (r == null) continue;
                if (op.urgent) urgentRequests++;
                if (op.load && r.state == RegionLoadState.Unloaded) yield return LoadRegion(r);
                else if (!op.load && r.state == RegionLoadState.Loaded) yield return UnloadRegion(r);
                if (op.urgent) urgentRequests--;
            }
            workerRoutine = null;
        }

        private int CountLoadedOrLoading()
        {
            int n = 0;
            for (int i = 0; i < regions.Length; i++)
                if (regions[i] != null && (regions[i].state == RegionLoadState.Loaded || regions[i].state == RegionLoadState.Loading)) n++;
            return n;
        }

        // ---------------------------------------------------------------- Memuat satu region

        private IEnumerator LoadRegion(RegionRecord r)
        {
            if (r == null || r.state != RegionLoadState.Unloaded) yield break;
            var loader = AsyncSceneLoader.Instance;
            if (loader == null) yield break;
            float startTime = Time.realtimeSinceStartup;

            // 1.
            SetState(r, RegionLoadState.Loading);

            // 2. Kasus khusus "Scene gagal dimuat": catat, biarkan SeamBarrier aktif, coba lagi setelah 5 detik.
            AsyncOperation op = loader.LoadAdditive(r.sceneName);
            while (op == null)
            {
                Debug.LogError($"[RegionStreaming] Gagal memuat {r.sceneName}. Dicoba lagi.");
                yield return new WaitForSecondsRealtime(B.loadRetrySeconds);
                if (runEnded) { SetState(r, RegionLoadState.Unloaded); yield break; }
                op = loader.LoadAdditive(r.sceneName);
            }
            while (!op.isDone) yield return null;
            if (DebugExtraDelaySeconds > 0f) yield return new WaitForSecondsRealtime(DebugExtraDelaySeconds);

            // 3. Objek mengambil status dari Store.
            RestoreRegion(r.region);

            // 4.
            if (r.structureRoot != null) r.structureRoot.gameObject.SetActive(true);
            if (r.farMesh != null) r.farMesh.SetActive(false);

            // 5.
            LightProbes.TetrahedralizeAsync();

            // 6.
            lastLoadSeconds[(int)r.region] = Time.realtimeSinceStartup - startTime;
            if (lastLoadSeconds[(int)r.region] > B.regionLoadTimeLimitSeconds)
                Debug.LogWarning($"[RegionStreaming] {r.sceneName} dimuat {lastLoadSeconds[(int)r.region]:0.0} dtk, melewati batas {B.regionLoadTimeLimitSeconds}.");
            SetState(r, RegionLoadState.Loaded);
            RefreshBarriersAndLinks();
        }

        // ---------------------------------------------------------------- Melepas satu region

        private IEnumerator UnloadRegion(RegionRecord r)
        {
            if (r == null || r.state != RegionLoadState.Loaded) yield break;
            var loader = AsyncSceneLoader.Instance;
            if (loader == null) yield break;

            // 1. SpawnManager, NPCRoster, HeavyItemSystem, dan WorldItemManager mengurus objeknya pada frame ini.
            SetState(r, RegionLoadState.Unloading);
            // 2.
            yield return null;
            // 3.
            CaptureRegion(r.region);
            r.lastUnloadedGameHour = NowGameHours();
            Store.SetLastUnloadedGameHour(r.region, r.lastUnloadedGameHour);
            // 4.
            if (r.farMesh != null) r.farMesh.SetActive(true);
            if (r.structureRoot != null) r.structureRoot.gameObject.SetActive(false);
            RefreshBarriersAndLinks();
            // 5.
            var op = loader.UnloadAdditive(r.sceneName);
            if (op != null) while (!op.isDone) yield return null;
            // 6.
            SetState(r, RegionLoadState.Unloaded);
            RefreshBarriersAndLinks();
            // 7.
            if (Time.realtimeSinceStartup - lastUnusedAssetsTime >= B.unusedAssetsMinIntervalSeconds)
            {
                lastUnusedAssetsTime = Time.realtimeSinceStartup;
                Resources.UnloadUnusedAssets();
            }
        }

        private void SetState(RegionRecord r, RegionLoadState s)
        {
            r.state = s;
            EventChannel<RegionLoadStateChangedEvent>.Publish(new RegionLoadStateChangedEvent(r.region, s));
        }

        // ---------------------------------------------------------------- Status objek region

        /// <summary>Tulis status semua ISaveable di region ini ke Store.</summary>
        public void CaptureRegion(RegionType region)
        {
            var root = RegionSceneRoot.Get(region);
            if (root == null) return;
            root.CollectSaveables();
            var list = root.Saveables;
            for (int i = 0; i < list.Count; i++)
            {
                var s = list[i];
                if (s == null) continue;
                try { Store.Write(region, s.SaveId, s.CaptureState()); }
                catch (Exception ex) { Debug.LogException(ex); }
            }
            // TODO(WorldItemManager #66): WorldItemManager.Instance.CaptureRegion(region, Store) mencatat WorldItem lepas.
        }

        /// <summary>Isi status objek region dari Store, lalu OnRestoreFinished. Pencarian ISaveable hanya sekali.</summary>
        public void RestoreRegion(RegionType region)
        {
            var root = RegionSceneRoot.Get(region);
            if (root == null)
            {
                Debug.LogWarning($"[RegionStreaming] RegionSceneRoot untuk {region} tidak ditemukan.");
                return;
            }
            root.CollectSaveables();
            var list = root.Saveables;
            for (int i = 0; i < list.Count; i++)
            {
                var s = list[i];
                if (s == null || !Store.TryRead(region, s.SaveId, out var json)) continue;
                try { s.RestoreState(json); }
                catch (Exception ex) { Debug.LogException(ex); }
            }
            for (int i = 0; i < list.Count; i++)
            {
                try { list[i]?.OnRestoreFinished(); }
                catch (Exception ex) { Debug.LogException(ex); }
            }
            // TODO(WorldItemManager #66): WorldItemManager.Instance.RestoreRegion(region, Store).
        }

        // ---------------------------------------------------------------- Seam dan NavMeshLink

        private void RefreshBarriersAndLinks()
        {
            for (int i = 0; i < seams.Length; i++)
            {
                var s = seams[i];
                if (s == null) continue;
                bool bothLoaded = IsLoaded(s.a) && IsLoaded(s.b);
                // SeamBarrier aktif selama salah satu sisi belum Loaded.
                if (s.barrier != null) s.barrier.SetBlocking(!bothLoaded);
            }
            RefreshLinks();
        }

        /// <summary>Link hanya aktif kalau kedua region Loaded dan seam sudah terbuka (#68; S1 ikut status jembatan).</summary>
        private void RefreshLinks()
        {
            for (int i = 0; i < seams.Length; i++)
            {
                var s = seams[i];
                if (s == null || s.links == null) continue;
                bool on = IsLoaded(s.a) && IsLoaded(s.b) && IsUnlocked(s.a) && IsUnlocked(s.b);
                for (int k = 0; k < s.links.Length; k++)
                {
                    var link = s.links[k];
                    if (link == null) continue;
                    if (on) link.UpdateLink();
                    link.enabled = on;
                }
            }
        }

        private static bool IsUnlocked(RegionType r)
        {
            // Region 1 selalu terbuka. Tanpa RegionGateSystem (scene uji), semua dianggap terbuka.
            if (r == RegionType.Volcano) return true;
            return RegionGateSystem.Instance == null || RegionGateSystem.Instance.IsUnlocked(r);
        }

        // ---------------------------------------------------------------- Pemain tertahan

        private void UpdateStall(float dt)
        {
            var player = IslandPlayerController.Instance;
            var b = B;
            bool waiting = false;
            if (player != null)
            {
                Vector3 p = player.transform.position;
                RegionType here = GetRegionAt(p);
                for (int i = 0; i < seams.Length; i++)
                {
                    var s = seams[i];
                    if (s == null || (s.a != here && s.b != here)) continue;
                    RegionType other = s.a == here ? s.b : s.a;
                    if (!IsUnlocked(other) || IsLoaded(other)) continue;
                    // Dekat SeamBarrier aktif dan hampir diam: pemain tertahan.
                    if (s.barrier != null && s.barrier.IsBlocking && Vector3.Distance(p, s.position) <= b.urgentDistance
                        && player.Velocity.sqrMagnitude <= b.stallSpeedThreshold * b.stallSpeedThreshold)
                        waiting = true;
                }
                if (!IsLoaded(here)) waiting = true;
            }

            stallTimer = waiting ? stallTimer + dt : 0f;
            // 3. Pesan "Memuat area" di HUD (HUDManager membaca IsStalled).
            IsStalled = stallTimer >= b.stallNoticeSeconds;
            // 4. Usulan disetujui #85: panel Loading menjeda waktu setelah 10 detik.
            if (stallTimer >= b.stallPanelSeconds && !stallPanelPushed) PushStallPanel();
            else if (!waiting && stallPanelPushed) CloseStallPanel();
        }

        private void PushStallPanel()
        {
            var stack = UIFocusStack.Instance;
            if (stack == null) return;
            if (stallPanel == null) stallPanel = FindFirstObjectByType<LoadingPanel>(FindObjectsInactive.Include);
            if (stallPanel == null) return;
            stallPanelPushed = stack.Push(stallPanel);
        }

        private void CloseStallPanel()
        {
            stallPanelPushed = false;
            if (stallPanel != null && UIFocusStack.Instance != null) UIFocusStack.Instance.Close(stallPanel, UICloseReason.Forced);
        }

        // ---------------------------------------------------------------- Bantuan

        private RegionRecord Find(RegionType region)
        {
            for (int i = 0; i < regions.Length; i++)
                if (regions[i] != null && regions[i].region == region) return regions[i];
            return null;
        }

        private void SyncUnloadHoursFromStore()
        {
            for (int i = 0; i < regions.Length; i++)
                if (regions[i] != null) regions[i].lastUnloadedGameHour = Store.GetLastUnloadedGameHour(regions[i].region);
        }

        private void SyncUnloadHoursToStore()
        {
            for (int i = 0; i < regions.Length; i++)
                if (regions[i] != null) Store.SetLastUnloadedGameHour(regions[i].region, regions[i].lastUnloadedGameHour);
        }

        private static float NowGameHours() => DayNightDirector.Instance != null ? (float)DayNightDirector.Instance.TotalGameHours : 0f;
    }
}
