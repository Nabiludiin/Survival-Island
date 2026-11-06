using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu-satunya pemanggil SceneManager.LoadSceneAsync dan UnloadSceneAsync.
    /// Blueprint: Core_Logic/AsyncSceneLoader.md (#82, #88, #99). Mendukung transisi Fase 1 ke Fase 2 (GDD bagian 1)
    /// dan kembali ke kapal saat permadeath (GDD bagian 3).
    ///
    /// Urutan bootstrap ke-14. Tidak memegang UI dan tidak menulis Time.timeScale (milik UIFocusStack).
    /// </summary>
    public class AsyncSceneLoader : MonoBehaviour, IBootstrapService
    {
        public static AsyncSceneLoader Instance { get; private set; }

        /// <summary>Kemajuan 0 sampai 1 untuk pemuatan scene tunggal. Dibaca panel Loading.</summary>
        public float Progress { get; private set; }

        /// <summary>Ada pemuatan scene tunggal yang berjalan.</summary>
        public bool IsBusy { get; private set; }

        /// <summary>Hasil LoadSingle terakhir. Dibaca RunLifecycle setelah yield return (usulan AsyncSceneLoader.md).</summary>
        public bool LastLoadSucceeded { get; private set; } = true;

        private bool activationAllowed;
        private AsyncOperation singleOperation;
        private int additiveInFlight;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        // AsyncSceneLoader.md bagian 4: peringatan kalau holdActivation menunggu lebih dari 60 detik.
        // Ini batas diagnostik pengembangan, bukan angka permainan, jadi tidak masuk GameBalanceConfig.
        private const float HoldActivationWarnSeconds = 60f;
#endif

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[AsyncSceneLoader] Instance ganda. Periksa prefab PersistentRoot.");
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public void Initialize()
        {
            Instance = this;
        }

        public void ResetForNewRun()
        {
            // IsBusy tidak disentuh, karena reset sendiri sedang memakai LoadSingle("Bootstrap").
            Progress = 0f;
            activationAllowed = false;
        }

        /// <summary>
        /// Muat scene tunggal. Langkah 1 sampai 6 berjalan sebelum yield pertama, jadi AllowActivation yang dipanggil
        /// setelah StartCoroutine(LoadSingle(...)) tidak tertimpa.
        /// Hasil dibaca dari LastLoadSucceeded setelah coroutine selesai.
        /// </summary>
        public IEnumerator LoadSingle(string sceneName, bool holdActivation = false)
        {
            // 1. Tolak permintaan saat sibuk.
            if (IsBusy)
            {
                Debug.LogError($"[AsyncSceneLoader] LoadSingle('{sceneName}') ditolak: pemuatan lain masih berjalan.");
                LastLoadSucceeded = false;
                yield break;
            }

            // 2. Scene harus ada di Build Settings.
            if (!Application.CanStreamedLevelBeLoaded(sceneName))
            {
                Debug.LogError($"[AsyncSceneLoader] Scene '{sceneName}' tidak ada di Build Settings.");
                LastLoadSucceeded = false;
                yield break;
            }

            // 3. Mulai.
            IsBusy = true;
            Progress = 0f;
            activationAllowed = false;
            float startTime = Time.realtimeSinceStartup;

            // 4. Selesaikan operasi additive yang sudah berjalan dulu, supaya scene region tidak aktif setelah scene tunggal baru.
            // Yield hanya terjadi kalau memang ada operasi additive. activationAllowed sudah di-reset di langkah 3,
            // jadi AllowActivation yang datang selama penantian ini tetap berlaku.
            while (additiveInFlight > 0)
                yield return null;

            // 5-6. Mulai operasi, aktivasi ditahan.
            singleOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            if (singleOperation == null)
            {
                Debug.LogError($"[AsyncSceneLoader] LoadSceneAsync('{sceneName}') mengembalikan null.");
                IsBusy = false;
                LastLoadSucceeded = false;
                yield break;
            }
            singleOperation.allowSceneActivation = false;

            float showSeconds = MinShowSeconds();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            bool warnedHold = false;
#endif
            // 7-8. Isi Progress sambil menunggu: progress >= 0,9, waktu tampil minimum (tak terskala), dan izin aktivasi.
            while (true)
            {
                // progress berhenti di 0,9 selama scene belum diaktifkan, jadi diskalakan ke 0..1.
                Progress = Mathf.Clamp01(singleOperation.progress / 0.9f);

                float elapsed = Time.realtimeSinceStartup - startTime;
                bool loaded = singleOperation.progress >= 0.9f;
                bool shownLongEnough = elapsed >= showSeconds;
                bool activationOk = !holdActivation || activationAllowed;
                if (loaded && shownLongEnough && activationOk) break;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (holdActivation && !activationAllowed && !warnedHold && elapsed > HoldActivationWarnSeconds)
                {
                    warnedHold = true;
                    Debug.LogWarning($"[AsyncSceneLoader] '{sceneName}' menunggu AllowActivation lebih dari {HoldActivationWarnSeconds} detik.");
                }
#endif
                yield return null;
            }

            // 9. Aktifkan.
            singleOperation.allowSceneActivation = true;
            Progress = 1f;
            while (!singleOperation.isDone)
                yield return null;

            // 10. Selesai.
            singleOperation = null;
            IsBusy = false;
            LastLoadSucceeded = true;
        }

        /// <summary>Muat scene region additive (#88). Mengembalikan null kalau ditolak.</summary>
        public AsyncOperation LoadAdditive(string sceneName)
        {
            if (IsBusy)
            {
                Debug.LogWarning($"[AsyncSceneLoader] LoadAdditive('{sceneName}') ditolak: LoadSingle sedang berjalan.");
                return null;
            }
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                Debug.LogWarning($"[AsyncSceneLoader] Scene '{sceneName}' sudah dimuat.");
                return null;
            }

            var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            if (op == null) return null;

            additiveInFlight++;
            // Method kelas, bukan lambda (aturan GameEventBus.md).
            op.completed += HandleAdditiveDone;
            return op;
        }

        /// <summary>Lepas scene region additive (#88). Boleh dipanggil saat IsBusy.</summary>
        public AsyncOperation UnloadAdditive(string sceneName)
        {
            if (!SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                Debug.LogWarning($"[AsyncSceneLoader] UnloadAdditive('{sceneName}'): scene tidak sedang dimuat.");
                return null;
            }

            var op = SceneManager.UnloadSceneAsync(sceneName);
            if (op == null) return null;

            additiveInFlight++;
            op.completed += HandleAdditiveDone;
            return op;
        }

        /// <summary>Izinkan scene aktif. Dipanggil NarrativeSequencer (#99) setelah adegan selesai.</summary>
        public void AllowActivation()
        {
            if (!IsBusy)
            {
                Debug.LogWarning("[AsyncSceneLoader] AllowActivation dipanggil saat tidak ada pemuatan.");
                return;
            }
            activationAllowed = true;
        }

        private void HandleAdditiveDone(AsyncOperation op)
        {
            op.completed -= HandleAdditiveDone;
            if (additiveInFlight > 0) additiveInFlight--;
        }

        private static float MinShowSeconds()
        {
            var gm = GameManager.Instance;
            return gm != null && gm.Balance != null ? gm.Balance.run.sceneLoadMinShowSeconds : 0f;
        }
    }
}
