using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>Data untuk memilih ending. Diisi RescueEventManager (#14) lewat PrepareEnding.</summary>
    public struct EndingContext
    {
        public int aliveNpcCount, rescuedCount, lostOnShipCount, day;
    }

    /// <summary>
    /// Pemutar adegan cerita di awal, tengah, dan akhir run, termasuk fitur lewati, subtitle, audio, dan pemuatan scene
    /// di belakang adegan. Blueprint: Core_Logic/NarrativeSequencer.md (#99). GDD bagian 2 (Insiden Kapal), bagian 3
    /// (Fase 1 dan 2), dan bagian 5 Region 3. Isi adegan dan tiga ending adalah usulan yang disetujui developer.
    ///
    /// Singleton persisten (RunLifecycle.md, tabel masa hidup). Tidak mendengar event: adegan dipicu pemanggilan langsung.
    /// Pemutar memakai waktu nyata (Time.unscaledDeltaTime), jadi tetap maju saat freezeWorld.
    /// Tidak mengubah Time.timeScale atau kursor langsung: panel Cinematic (#83) dan SetGameplayTimeScale yang mengaturnya.
    ///
    /// Belum ada (TODO): nama korban dari lostNpcIds pada lifeboat_to_beach (butuh nama NPC dari NPCRoster/NPCData, #76).
    /// </summary>
    public class NarrativeSequencer : MonoBehaviour, IBootstrapService
    {
        public static NarrativeSequencer Instance { get; private set; }

        public const string SeenPrefsPrefix = "narrative.seen.";

        // Daftar adegan (usulan 2 disetujui developer).
        public const string IntroShip = "intro_ship";
        public const string LifeboatToBeach = "lifeboat_to_beach";
        public const string HelipadFound = "helipad_found";
        public const string Takeoff = "takeoff";
        public const string EndingAlone = "ending_alone";
        public const string EndingWithNpc = "ending_with_npc";
        public const string EndingAllSaved = "ending_all_saved";

        // SubtitleRequestedEvent dari adegan memakai prioritas 3 (LocalizationManager.md, tabel sumber subtitle).
        private const int SubtitlePriority = 3;

        [SerializeField] private NarrativeScene[] scenes = new NarrativeScene[0];

        [Header("Tampilan (canvas persisten, di atas lapisan hitam CinematicPanel)")]
        [Tooltip("Kosong = UIManager.Instance.cinematicPanel.")]
        [SerializeField] private CinematicPanel cinematicPanel;
        [SerializeField] private Image beatImage;
        [SerializeField] private CanvasGroup beatImageGroup;
        [Tooltip("Petunjuk 'Space untuk lewati'. Teks memakai token {input:Skip} lewat LocalizedText.")]
        [SerializeField] private GameObject skipHint;
        [Tooltip("Satu AudioSource voice di canvas persisten, keluaran ke grup mixer yang memakai waktu tak terskala.")]
        [SerializeField] private AudioSource voiceSource;

        public bool IsPlaying { get; private set; }

        private NarrativeScene currentScene;
        private bool ownsPanel;
        private bool frozeWorld;
        private EndingContext? pendingEnding;

        private static NarrativeBalance B
        {
            get
            {
                var cfg = GameManager.CurrentBalance;
                return cfg != null ? cfg.narrative : null;
            }
        }

        private CinematicPanel Panel
        {
            get
            {
                if (cinematicPanel == null && UIManager.Instance != null) cinematicPanel = UIManager.Instance.cinematicPanel;
                return cinematicPanel;
            }
        }

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[NarrativeSequencer] Instance ganda. Yang baru dimatikan.");
                enabled = false;
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
            HideVisuals();
        }

        /// <summary>Permadeath: catatan narrative.seen.* tetap. Hanya mengosongkan pendingEnding, IsPlaying, dan ownsPanel.</summary>
        public void ResetForNewRun()
        {
            pendingEnding = null;
            IsPlaying = false;
            ownsPanel = false;
            frozeWorld = false;
            currentScene = null;
            HideVisuals();
        }

        // ---------------------------------------------------------------- Fungsi publik

        /// <summary>Putar satu adegan. Hanya satu adegan pada satu waktu.</summary>
        public IEnumerator Play(string sceneId)
        {
            // 1.
            var scene = Find(sceneId);
            if (scene == null)
            {
                Debug.LogWarning($"[NarrativeSequencer] Adegan '{sceneId}' tidak ada.");
                yield break;
            }

            // 2.
            if (IsPlaying)
            {
                Debug.LogWarning($"[NarrativeSequencer] '{sceneId}' ditolak: '{(currentScene != null ? currentScene.sceneId : "?")}' masih berjalan.");
                yield break;
            }

            var b = B;
            float fade = b != null ? b.beatFadeSeconds : 0f;

            // Batas waktu pengaman: total durasi beat + watchdogExtraSeconds, supaya klip rusak tidak mengunci game.
            float limit = (b != null ? b.watchdogExtraSeconds : 0f);
            var beats = scene.beats ?? new NarrativeBeat[0];
            for (int i = 0; i < beats.Length; i++) limit += BeatDuration(beats[i], fade);
            float startTime = Time.realtimeSinceStartup;

            // 3.
            Begin(scene, b);
            try
            {
                bool canSkip = CanSkip();
                float hintLeft = canSkip && b != null ? b.skipHintSeconds : 0f;
                if (skipHint != null) skipHint.SetActive(hintLeft > 0f);

                // 4.
                bool stop = false;
                for (int i = 0; i < beats.Length && !stop; i++)
                {
                    var beat = beats[i];
                    if (beat == null) continue;
                    StartBeat(beat);

                    float duration = BeatDuration(beat, fade);
                    float elapsed = 0f;
                    while (elapsed < duration)
                    {
                        float dt = Time.unscaledDeltaTime;
                        elapsed += dt;

                        if (beatImageGroup != null && fade > 0f) beatImageGroup.alpha = Mathf.Clamp01(elapsed / fade);

                        if (hintLeft > 0f)
                        {
                            hintLeft -= dt;
                            if (hintLeft <= 0f && skipHint != null) skipHint.SetActive(false);
                        }

                        // Usulan 4: hanya adegan yang pernah ditonton (atau alwaysSkippable) bisa dilewati.
                        var input = InputManager.Instance;
                        if (canSkip && input != null && input.SkipPressed) { stop = true; break; }

                        if (Time.realtimeSinceStartup - startTime > limit)
                        {
                            Debug.LogWarning($"[NarrativeSequencer] '{sceneId}' melewati batas waktu pengaman. Adegan dihentikan.");
                            stop = true;
                            break;
                        }
                        yield return null;
                    }
                }
            }
            finally
            {
                // 5. Semua langkah pembersihan di finally.
                Finish(scene, b);
            }
        }

        /// <summary>
        /// Dipakai RunLifecycle: scene dimuat dengan holdActivation: true selama adegan berjalan.
        /// var load = StartCoroutine(AsyncSceneLoader.LoadSingle(scene, holdActivation: true));
        /// yield return NarrativeSequencer.Instance.PlayDuringLoad(sceneId); yield return load;
        /// </summary>
        public IEnumerator PlayDuringLoad(string sceneId)
        {
            var stack = UIFocusStack.Instance;
            var ui = UIManager.Instance;
            UIPanel loading = ui != null ? ui.loadingPanel : null;

            // 1. Loading menolak panel baru (UIFocusStack.md), jadi ditutup dulu.
            if (stack != null && loading != null && stack.Contains(loading)) stack.Close(loading, UICloseReason.Forced);

            // 2.
            yield return Play(sceneId);

            // 3. Pemain yang melewati adegan melihat bar Loading sampai scene siap.
            var loader = AsyncSceneLoader.Instance;
            if (loader != null && loader.IsBusy && loader.Progress < 1f && stack != null && loading != null) stack.Push(loading);

            // 4.
            if (loader != null && loader.IsBusy) loader.AllowActivation();
        }

        /// <summary>Dipanggil RescueEventManager (#14), tepat sebelum TriggerWinSequence.</summary>
        public void PrepareEnding(EndingContext ctx)
        {
            pendingEnding = ctx;
        }

        /// <summary>Dipanggil RunLifecycle.RunEndSequence untuk Won, tanpa parameter.</summary>
        public IEnumerator PlayEnding()
        {
            // 1.
            yield return Play(Takeoff);

            // 2. Tanpa PrepareEnding (uji lewat konsol), dipakai ending_alone.
            string endingId = pendingEnding.HasValue ? ChooseEnding(pendingEnding.Value) : EndingAlone;
            pendingEnding = null;

            // 3.
            yield return Play(endingId);
        }

        /// <summary>
        /// Pemilihan ending berurutan (usulan 3 disetujui developer). Ending hanya mengubah adegan; syarat menang dan
        /// isi WinScreen tidak berubah (GDD bagian 3).
        /// </summary>
        public static string ChooseEnding(in EndingContext ctx)
        {
            if (ctx.aliveNpcCount == 0) return EndingAlone;
            if (ctx.aliveNpcCount == ctx.rescuedCount && ctx.lostOnShipCount == 0 && ctx.rescuedCount > 0) return EndingAllSaved;
            return EndingWithNpc;
        }

        public bool HasSeen(string sceneId) => !string.IsNullOrEmpty(sceneId) && PlayerPrefs.GetInt(SeenPrefsPrefix + sceneId, 0) == 1;

        // ---------------------------------------------------------------- Langkah adegan

        private void Begin(NarrativeScene s, NarrativeBalance b)
        {
            float musicFade = b != null ? b.musicFadeSeconds : 0f;

            // 1.
            IsPlaying = true;
            currentScene = s;

            // 2. RunLifecycle sudah mendorong Cinematic saat akhir run, jadi ownsPanel = false di sana.
            var stack = UIFocusStack.Instance;
            var panel = Panel;
            ownsPanel = false;
            if (stack != null && panel != null && !stack.Contains(panel)) ownsPanel = stack.Push(panel);
            if (panel != null)
            {
                panel.SetBlack(1f);
                panel.ClearCaption();
            }

            // 3. Usulan 6: helipad_found membekukan dunia, supaya pemain tidak diserang saat kontrolnya terkunci.
            frozeWorld = s.freezeWorld && stack != null;
            if (frozeWorld) stack.SetGameplayTimeScale(0f);

            // 4.
            var audio = AudioManager.Instance;
            if (audio != null)
            {
                audio.SetCinematicMix(true, musicFade);
                if (s.music != null) audio.PlayMusicClip(s.music, musicFade);
            }

            // 5. keepMusic = true kalau adegan tidak punya musik sendiri (#114).
            EventChannel<NarrativeStateChangedEvent>.Publish(new NarrativeStateChangedEvent(s.sceneId, true));
            EventChannel<CinematicStateChangedEvent>.Publish(new CinematicStateChangedEvent(true, s.music == null));

            // TODO(NPCRoster): lifeboat_to_beach menyebut nama korban dari GameManager.shipOutcome.lostNpcIds (#76).
        }

        private void StartBeat(NarrativeBeat beat)
        {
            // Gambar: null = layar hitam.
            if (beatImage != null)
            {
                beatImage.sprite = beat.image;
                beatImage.enabled = beat.image != null;
            }
            if (beatImageGroup != null) beatImageGroup.alpha = 0f;

            // Teks layar lewat LocalizationManager (#104). Kunci mentah dipakai kalau tabel belum ada.
            var panel = Panel;
            if (panel != null)
            {
                if (string.IsNullOrEmpty(beat.textKey)) panel.ClearCaption();
                else panel.ShowCaption(beat.textKey);
            }

            // Subtitle diserahkan ke SubtitlePresenter, yang mengikuti setting subtitle.
            if (!string.IsNullOrEmpty(beat.subtitleKey))
                EventChannel<SubtitleRequestedEvent>.Publish(new SubtitleRequestedEvent(beat.subtitleKey, string.Empty, SubtitlePriority, SubtitleSourceKind.Scene));

            if (voiceSource != null)
            {
                voiceSource.Stop();
                if (beat.voice != null)
                {
                    voiceSource.clip = beat.voice;
                    voiceSource.Play();
                }
            }

            var audio = AudioManager.Instance;
            if (audio != null && beat.sfx != null) audio.PlaySFX(beat.sfx);
        }

        /// <summary>Membalik Begin dengan urutan terbalik.</summary>
        private void Finish(NarrativeScene s, NarrativeBalance b)
        {
            float musicFade = b != null ? b.musicFadeSeconds : 0f;

            if (voiceSource != null) voiceSource.Stop();
            if (skipHint != null) skipHint.SetActive(false);

            // 5.
            EventChannel<CinematicStateChangedEvent>.Publish(new CinematicStateChangedEvent(false, s.music == null));
            EventChannel<NarrativeStateChangedEvent>.Publish(new NarrativeStateChangedEvent(s.sceneId, false));

            // 4. Musik dunia kembali mengikuti siang atau malam.
            var audio = AudioManager.Instance;
            if (audio != null)
            {
                if (s.music != null) audio.PlayMusicClip(null, musicFade);
                audio.SetCinematicMix(false, musicFade);
            }

            // 3.
            var stack = UIFocusStack.Instance;
            if (frozeWorld && stack != null) stack.SetGameplayTimeScale(1f);
            frozeWorld = false;

            // 2. Panel hanya ditutup kalau dibuka oleh sistem ini.
            var panel = Panel;
            HideVisuals();
            if (ownsPanel && stack != null && panel != null) stack.Close(panel, UICloseReason.Forced);
            ownsPanel = false;

            // Catatan adegan di luar data run, jadi tetap ada setelah permadeath (usulan 5, RunLifecycle.md).
            MarkSeen(s.sceneId);

            // 1.
            currentScene = null;
            IsPlaying = false;
        }

        private void HideVisuals()
        {
            if (beatImage != null) { beatImage.sprite = null; beatImage.enabled = false; }
            if (beatImageGroup != null) beatImageGroup.alpha = 0f;
            if (skipHint != null) skipHint.SetActive(false);
            var panel = cinematicPanel;
            if (panel != null) panel.ClearCaption();
        }

        // ---------------------------------------------------------------- Pembantu

        private bool CanSkip()
        {
            return currentScene != null && (currentScene.alwaysSkippable || HasSeen(currentScene.sceneId));
        }

        private static void MarkSeen(string sceneId)
        {
            if (string.IsNullOrEmpty(sceneId)) return;
            PlayerPrefs.SetInt(SeenPrefsPrefix + sceneId, 1);
            PlayerPrefs.Save();
        }

        /// <summary>seconds 0 mengikuti panjang voice. Tanpa voice, dipakai beatFadeSeconds supaya adegan tidak macet.</summary>
        private static float BeatDuration(NarrativeBeat beat, float fade)
        {
            if (beat == null) return 0f;
            if (beat.seconds > 0f) return beat.seconds;
            if (beat.voice != null) return beat.voice.length;
            return fade;
        }

        private NarrativeScene Find(string sceneId)
        {
            if (string.IsNullOrEmpty(sceneId)) return null;
            for (int i = 0; i < scenes.Length; i++)
                if (scenes[i] != null && scenes[i].sceneId == sceneId) return scenes[i];
            return null;
        }
    }
}
