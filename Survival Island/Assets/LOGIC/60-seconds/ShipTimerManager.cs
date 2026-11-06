using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Hitung mundur Fase 1 dan tensi (detak jam, vignette UI, getar teks timer).
    /// Blueprint: 60-seconds/ShipTimerManager.md (#74, #82).
    /// GDD bagian 3: Fase 1 berlangsung 60 detik. Keputusan developer: 60 detik adalah batas paling lama,
    /// pemain boleh berangkat lebih awal dengan sekoci (LifeboatExtraction).
    ///
    /// Kelas ini tidak menyimpan inventori, tidak memuat scene, dan tidak memulai dirinya sendiri.
    /// Belum diimplementasikan (sistemnya belum ditulis, ditandai TODO): musik dan sirene lewat AudioManager,
    /// cameraShakeScale dari LocalizationManager (#104), dan perintah konsol shiptime (#56).
    /// </summary>
    public class ShipTimerManager : MonoBehaviour
    {
        private static readonly ShipBalance FallbackShip = new ShipBalance();

        public static ShipTimerManager Instance { get; private set; }

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private RectTransform timerRect;
        [SerializeField] private Image vignetteOverlay;
        [Tooltip("Bentuk naiknya kepanikan. Disimpan di prefab, bukan di logika.")]
        [SerializeField] private AnimationCurve panicCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color panicColor = Color.red;

        [Header("Audio")]
        [Tooltip("Detak jam. loop = true, tidak memakai ignoreListenerPause.")]
        [SerializeField] private AudioSource tickSource;
        [SerializeField] private AudioClip musicClip;
        [SerializeField] private AudioClip timeUpClip;

        /// <summary>Klip untuk AudioManager saat sudah ditulis (TODO di StartCountdown dan ExecuteTimeUp).</summary>
        public AudioClip MusicClip => musicClip;
        public AudioClip TimeUpClip => timeUpClip;

        public float SecondsLeft { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsTimeUp { get; private set; }

        private int lastShownSecond = -1;
        private float panicFactor;
        private Vector2 timerBasePos;

        private static ShipBalance Ship
        {
            get
            {
                var b = GameManager.CurrentBalance;
                return b != null ? b.ship : FallbackShip;
            }
        }

        private static bool RunEnding => GameManager.Instance != null && GameManager.Instance.IsRunEnding;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("[ShipTimerManager] Sudah ada satu di scene. Komponen ini dihapus.", this);
                Destroy(this);
                return;
            }
            Instance = this;
            if (timerRect != null) timerBasePos = timerRect.anchoredPosition;

            // GDD bagian 3: 60 detik (dibaca dari config, bukan ditulis langsung).
            SecondsLeft = Ship.durationSeconds;
            IsRunning = false;
            IsTimeUp = false;
            ShowIdle();
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable() { EventChannel<RunEndedEvent>.Raised += HandleRunEnded; }
        private void OnDisable() { EventChannel<RunEndedEvent>.Raised -= HandleRunEnded; }

        private void HandleRunEnded(RunEndedEvent e) { StopCountdown(); }

        /// <summary>Angka penuh tanpa berjalan, untuk jeda pra-game ("timer menampilkan 60").</summary>
        private void ShowIdle()
        {
            WriteSeconds(Mathf.CeilToInt(SecondsLeft));
            if (timerText != null) timerText.color = normalColor;
            SetVignetteAlpha(0f, force: true);
            if (timerRect != null) timerRect.anchoredPosition = timerBasePos;
            if (tickSource != null) tickSource.Stop();
        }

        /// <summary>Dipanggil LifeboatExtraction.PreGameRoutine langkah 7. Panggilan kedua diabaikan.</summary>
        public void StartCountdown()
        {
            if (IsRunning || IsTimeUp) return;
            if (RunEnding) return;

            var ship = Ship;
            SecondsLeft = ship.durationSeconds;
            IsRunning = true;

            // Usulan disetujui (#85): detak terdengar sejak timer mulai.
            if (tickSource != null)
            {
                tickSource.pitch = ship.tickPitchMin;
                tickSource.volume = ship.tickVolume;
                tickSource.Play();
            }
            // TODO (AudioManager belum ditulis): AudioManager.Instance.PlayMusicClip(musicClip, ship.musicFadeInSeconds).
        }

        /// <summary>Dipanggil LifeboatExtraction.RequestDepart dan saat RunEndedEvent. Aman dipanggil berulang.</summary>
        public void StopCountdown()
        {
            if (!IsRunning) return;
            IsRunning = false;
            if (tickSource != null) tickSource.Stop();
            // TODO (AudioManager belum ditulis): AudioManager.Instance.PlayMusicClip(null, Ship.musicFadeOutSeconds).
            // SecondsLeft tidak direset: LifeboatExtraction.ResolveOutcome membacanya untuk secondsLeftOnExit.
        }

        private void Update()
        {
            if (!IsRunning || IsTimeUp) return;
            if (RunEnding) { StopCountdown(); return; }   // Pengaman kalau RunEndedEvent terlambat

            // Timer gameplay memakai Time.deltaTime, jadi berhenti saat Pause.
            SecondsLeft = Mathf.Max(0f, SecondsLeft - Time.deltaTime);
            UpdateDisplay();
            UpdatePanic();
            if (SecondsLeft <= 0f) ExecuteTimeUp();
        }

        private void UpdateDisplay()
        {
            // Pembulatan ke atas: 60 sampai 1, lalu 0 hanya saat waktu benar-benar habis.
            int shown = Mathf.CeilToInt(SecondsLeft);
            if (shown != lastShownSecond) WriteSeconds(shown);
        }

        private void WriteSeconds(int value)
        {
            // SetText dengan argumen angka tidak membuat string baru.
            if (timerText != null) timerText.SetText("{0}", value);
            lastShownSecond = value;
        }

        private void UpdatePanic()
        {
            var ship = Ship;
            float start = ship.panicStartSeconds;
            float raw = SecondsLeft >= start || start <= 0f ? 0f : 1f - SecondsLeft / start;
            panicFactor = panicCurve != null ? panicCurve.Evaluate(raw) : raw;

            if (tickSource != null) tickSource.pitch = Mathf.Lerp(ship.tickPitchMin, ship.tickPitchMax, panicFactor);
            SetVignetteAlpha(panicFactor * ship.vignetteMaxAlpha, force: false);
            if (timerText != null) timerText.color = Color.Lerp(normalColor, panicColor, panicFactor);

            if (timerRect == null) return;
            // TODO #104 (LocalizationManager belum ditulis): kalikan LocalizationManager.Instance.Accessibility.cameraShakeScale.
            const float shakeScale = 1f;
            float amp = panicFactor * ship.timerShakePixels * shakeScale;
            float t = Time.time * ship.timerShakeFrequency;   // Time.time berhenti saat jeda
            timerRect.anchoredPosition = timerBasePos + new Vector2(
                (Mathf.PerlinNoise(t, 0f) - 0.5f) * 2f * amp,
                (Mathf.PerlinNoise(0f, t) - 0.5f) * 2f * amp);
        }

        private void SetVignetteAlpha(float alpha, bool force)
        {
            if (vignetteOverlay == null) return;
            var c = vignetteOverlay.color;
            // Hanya diubah kalau selisih > 0,005, supaya Canvas tidak dibangun ulang tiap frame.
            if (!force && Mathf.Abs(c.a - alpha) <= 0.005f) return;
            c.a = alpha;
            vignetteOverlay.color = c;
        }

        private void ExecuteTimeUp()
        {
            IsTimeUp = true;
            IsRunning = false;
            SecondsLeft = 0f;
            WriteSeconds(0);

            if (tickSource != null) tickSource.Stop();
            // TODO (AudioManager belum ditulis): AudioManager.Instance.PlaySFX(timeUpClip);
            //      AudioManager.Instance.PlayMusicClip(null, Ship.musicFadeOutSeconds).

            // Urutan wajib: jadwal sisa (termasuk FinalShudder T=0) dijalankan sebelum hasil,
            // karena kedua jalur hasil memanggil ShipHazardManager.StopAll().
            if (ShipHazardManager.Instance != null) ShipHazardManager.Instance.FlushScheduleAtTimeUp();

            if (LifeboatExtraction.Instance != null) LifeboatExtraction.Instance.HandleTimeUp();
            else Debug.LogError("[ShipTimerManager] LifeboatExtraction tidak ada di ShipPhase. Scene wajib punya sekoci.", this);
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        /// <summary>Alat uji (usulan): perintah konsol shiptime &lt;detik&gt; (#56).</summary>
        public void DebugSetSecondsLeft(float value)
        {
            SecondsLeft = Mathf.Clamp(value, 0f, Ship.durationSeconds);
        }
#endif
    }
}
