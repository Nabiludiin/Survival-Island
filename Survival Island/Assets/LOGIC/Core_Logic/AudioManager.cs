using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace SurvivalIsland
{
    /// <summary>
    /// Pemilik AudioMixer utama, volume, efek suara global, dan musik adegan.
    /// Blueprint: Core_Logic/AudioManager.md (#82, #83, #19, #52, #99, #114). Usulan teknis, belum ada di GDD;
    /// mendukung suasana siang dan malam di GDD bagian 3.
    ///
    /// Urutan bootstrap ke-4. Musik siang, malam, dan pertempuran milik CombatMusicDirector (#114), bukan kelas ini.
    /// Kelas ini tidak menulis PlayerPrefs; volume disimpan SettingsManager (#19).
    /// </summary>
    public class AudioManager : MonoBehaviour, IBootstrapService
    {
        public static AudioManager Instance { get; private set; }

        [Header("Mixer (grup Music, Ambient > AmbientA/AmbientB, EnemyCue, Warning, SFX)")]
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioMixerSnapshot normalSnapshot;
        [SerializeField] private AudioMixerSnapshot pausedSnapshot;
        [Tooltip("Snapshot suara dunia turun untuk adegan #99. Boleh kosong: SetCinematicMix lalu tidak berbuat apa-apa.")]
        [SerializeField] private AudioMixerSnapshot cinematicSnapshot;

        [Header("Parameter volume terekspos (kosong kalau tidak dipakai)")]
        [SerializeField] private string masterVolumeParam = "Master_Vol";
        [SerializeField] private string musicVolumeParam = "Music_Vol";
        [SerializeField] private string sfxVolumeParam = "SFX_Vol";

        [Header("Sumber")]
        [Tooltip("Musik adegan (#99). ignoreListenerPause = true.")]
        [SerializeField] private AudioSource bgmSource;
        [Tooltip("Suara UI dan global. ignoreListenerPause = true.")]
        [SerializeField] private AudioSource sfxSource;
        [Tooltip("Keluar ke grup Warning (#52).")]
        [SerializeField] private AudioSource warningSource;

        public float masterVolume { get; private set; } = 1f;
        public float bgmVolume { get; private set; } = 1f;
        public float sfxVolume { get; private set; } = 1f;

        /// <summary>Mixer yang dipakai AmbientAudio (#52) dan CombatMusicDirector (#114).</summary>
        public AudioMixer Mixer => mixer;

        private bool mixerAppliesVolume;   // true kalau parameter volume ada di mixer
        private bool isPausedMix, isCinematicMix;
        private Coroutine musicRoutine;
        private float musicFade = 1f;      // 0..1, posisi fade musik adegan

        private const float MinDecibel = -80f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[AudioManager] Instance ganda. Periksa prefab PersistentRoot.");
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Layanan

        public void Initialize()
        {
            if (mixer != null)
            {
                // Snapshot tetap berpindah saat Time.timeScale = 0 (AudioManager.md, Kasus Khusus).
                mixer.updateMode = AudioMixerUpdateMode.UnscaledTime;
                mixerAppliesVolume = !string.IsNullOrEmpty(masterVolumeParam) && mixer.GetFloat(masterVolumeParam, out _);
            }
            else Debug.LogError("[AudioManager] Rujukan 'mixer' kosong di prefab PersistentRoot.");

            // UI dan musik adegan tetap terdengar saat AudioListener.pause aktif (#83).
            if (bgmSource != null) bgmSource.ignoreListenerPause = true;
            if (sfxSource != null) sfxSource.ignoreListenerPause = true;

            var s = SettingsManager.Instance;
            if (s != null) UpdateVolume(s.masterVolume, s.bgmVolume, s.sfxVolume);
            else UpdateVolume(masterVolume, bgmVolume, sfxVolume);
        }

        /// <summary>Kembali ke snapshot Normal. Volume tidak berubah (disimpan SettingsManager, bukan save run).</summary>
        public void ResetForNewRun()
        {
            isPausedMix = false;
            isCinematicMix = false;
            if (normalSnapshot != null) normalSnapshot.TransitionTo(0f);
        }

        // ---------------------------------------------------------------- Volume

        /// <summary>Hanya menerapkan volume. Tidak menulis PlayerPrefs (milik #19). PlayerPrefs.DeleteAll dilarang (#82).</summary>
        public void UpdateVolume(float master, float bgm, float sfx)
        {
            masterVolume = Mathf.Clamp01(master);
            bgmVolume = Mathf.Clamp01(bgm);
            sfxVolume = Mathf.Clamp01(sfx);

            if (mixerAppliesVolume)
            {
                // Volume 0 tetap diputar pada -80 dB, sehingga posisi crossfade tidak rusak (SettingsManager.md).
                SetMixerVolume(masterVolumeParam, masterVolume);
                SetMixerVolume(musicVolumeParam, bgmVolume);
                SetMixerVolume(sfxVolumeParam, sfxVolume);
            }
            ApplyMusicSourceVolume();
        }

        private void SetMixerVolume(string param, float linear)
        {
            if (mixer == null || string.IsNullOrEmpty(param)) return;
            float db = linear <= 0.0001f ? MinDecibel : Mathf.Max(MinDecibel, 20f * Mathf.Log10(linear));
            mixer.SetFloat(param, db);
        }

        // Pengali di sumber hanya dipakai kalau mixer tidak punya parameter volume, supaya volume tidak dikali dua kali.
        private float SfxScale => mixerAppliesVolume ? 1f : sfxVolume * masterVolume;
        private float MusicScale => mixerAppliesVolume ? 1f : bgmVolume * masterVolume;

        private void ApplyMusicSourceVolume()
        {
            if (bgmSource != null) bgmSource.volume = musicFade * MusicScale;
        }

        // ---------------------------------------------------------------- Suara

        public void PlaySFX(AudioClip clip, float volumeMultiplier = 1f)
        {
            if (clip == null || sfxSource == null) return;
            sfxSource.PlayOneShot(clip, volumeMultiplier * SfxScale);
        }

        /// <summary>Putar klip lewat grup Warning (#52).</summary>
        public void PlayWarning(AudioClip clip)
        {
            if (clip == null || warningSource == null) return;
            warningSource.PlayOneShot(clip, SfxScale);
        }

        /// <summary>Dipanggil RunLifecycle saat reset. Sistem yang belum dipasang di scene dilewati.</summary>
        public void StopWorldAudio()
        {
            AmbientAudio.Instance?.StopImmediate();          // Suara latar (#52)
            CombatMusicDirector.Instance?.StopImmediate();   // Musik pertempuran dan suara musuh (#114)
            if (musicRoutine != null)
            {
                StopCoroutine(musicRoutine);
                musicRoutine = null;
            }
            if (bgmSource != null) { bgmSource.Stop(); bgmSource.clip = null; }
            if (warningSource != null) warningSource.Stop();
            PooledAudioSource.StopAll();   // Sumber 3D sekali putar (#52) kembali ke pool
        }

        // ---------------------------------------------------------------- Campuran

        /// <summary>Dipanggil UIFocusStack (#83). Waktu transisi: GameBalanceConfig.ui.pauseAudioFadeSeconds.</summary>
        public void SetPausedMix(bool paused)
        {
            if (isPausedMix == paused) return;
            isPausedMix = paused;
            var b = GameManager.CurrentBalance;
            ApplySnapshot(b != null ? b.ui.pauseAudioFadeSeconds : 0f);
        }

        /// <summary>Turunkan atau kembalikan suara dunia untuk adegan #99.</summary>
        public void SetCinematicMix(bool active, float fadeSeconds)
        {
            if (isCinematicMix == active) return;
            isCinematicMix = active;
            ApplySnapshot(fadeSeconds);
        }

        // Jeda menang atas adegan, karena panel Pause menutup adegan yang sedang berjalan.
        private void ApplySnapshot(float seconds)
        {
            AudioMixerSnapshot target = normalSnapshot;
            if (isPausedMix && pausedSnapshot != null) target = pausedSnapshot;
            else if (isCinematicMix && cinematicSnapshot != null) target = cinematicSnapshot;
            if (target != null) target.TransitionTo(Mathf.Max(0f, seconds));
        }

        // ---------------------------------------------------------------- Musik adegan

        /// <summary>Putar musik adegan (#99). clip == null memudarkan musik yang berjalan. Fade memakai waktu tak terskala.</summary>
        public void PlayMusicClip(AudioClip clip, float fadeSeconds)
        {
            if (bgmSource == null) return;
            if (musicRoutine != null) StopCoroutine(musicRoutine);
            if (!isActiveAndEnabled)
            {
                ApplyMusicImmediate(clip);
                return;
            }
            musicRoutine = StartCoroutine(MusicRoutine(clip, Mathf.Max(0f, fadeSeconds)));
        }

        private void ApplyMusicImmediate(AudioClip clip)
        {
            musicRoutine = null;
            bgmSource.Stop();
            bgmSource.clip = clip;
            musicFade = clip != null ? 1f : 0f;
            ApplyMusicSourceVolume();
            if (clip != null) bgmSource.Play();
        }

        private IEnumerator MusicRoutine(AudioClip clip, float fadeSeconds)
        {
            // Pudarkan musik lama dari posisi fade saat ini.
            if (bgmSource.isPlaying && bgmSource.clip != clip)
            {
                yield return FadeMusic(0f, fadeSeconds);
                bgmSource.Stop();
            }

            if (clip == null)
            {
                bgmSource.clip = null;
                musicRoutine = null;
                yield break;
            }

            if (bgmSource.clip != clip || !bgmSource.isPlaying)
            {
                bgmSource.clip = clip;
                musicFade = fadeSeconds > 0f ? 0f : 1f;
                ApplyMusicSourceVolume();
                bgmSource.Play();
            }
            yield return FadeMusic(1f, fadeSeconds);
            musicRoutine = null;
        }

        private IEnumerator FadeMusic(float target, float seconds)
        {
            if (seconds <= 0f)
            {
                musicFade = target;
                ApplyMusicSourceVolume();
                yield break;
            }
            float speed = 1f / seconds;
            while (!Mathf.Approximately(musicFade, target))
            {
                musicFade = Mathf.MoveTowards(musicFade, target, speed * Time.unscaledDeltaTime);
                ApplyMusicSourceVolume();
                yield return null;
            }
        }
    }
}
