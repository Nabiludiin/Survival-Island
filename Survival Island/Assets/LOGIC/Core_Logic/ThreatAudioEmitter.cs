using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    public enum ThreatCueKind { Idle, Alert }

    /// <summary>
    /// Suara penanda 3D satu musuh atau hewan. Tidak punya Update: CombatMusicDirector memanggil Tick bergiliran.
    /// Blueprint: Core_Logic/CombatMusicDirector.md bagian "Variabel ThreatAudioEmitter" dan "Suara penanda" (#114).
    /// GDD bagian 3 (ancaman siang dan malam): pemain bisa mendengar jenis ancaman dan arahnya. Usulan.
    ///
    /// Dipasang di prefab musuh (objek pool). Sumber 3D keluar ke grup mixer EnemyCue.
    /// </summary>
    [DisallowMultipleComponent]
    public class ThreatAudioEmitter : MonoBehaviour, IPoolable
    {
        /// <summary>Daftar musuh aktif, tanpa query fisika. Urutan tidak dijamin (swap-remove).</summary>
        public static readonly List<ThreatAudioEmitter> Active = new List<ThreatAudioEmitter>(64);

        [SerializeField] private EnemyCueSet cues;
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioLowPassFilter lowpass;

        public const float OpenCutoffHz = 22000f;

        public EnemyCueSet Cues => cues;
        public AudioSource Source => source;
        public AudioLowPassFilter Lowpass => lowpass;

        // Diatur CombatMusicDirector (slot suara).
        internal int activeIndex = -1;
        internal bool hasVoice;
        internal bool voiceFading;
        internal float voiceFadeSpeed;
        internal int voicePriority;
        internal bool voiceIsTell;
        internal AudioClip voiceClip;
        internal float voiceStartTime;
        internal float nextOcclusionTime;

        private float nextIdleTime;
        private bool lastEngaged;

        private EnemyAI ai;
        private WildlifeAI wildlife;
        private EnemyHealth health;
        private bool cached;

        /// <summary>EnemyData.enemyId, dipakai batas suara per varian.</summary>
        public string EnemyId => health != null && health.Data != null ? health.Data.enemyId : string.Empty;

        /// <summary>
        /// EnemyAI: Chase, AttackPlayer, AttackBuilding. WildlifeAI: Chase, Attack.
        /// TargetUnreachable tidak dihitung, agar hewan di seberang jurang tidak mengubah musik.
        /// </summary>
        public bool IsEngaged
        {
            get
            {
                if (ai != null)
                {
                    var s = ai.CurrentState;
                    return s == AIState.Chase || s == AIState.AttackPlayer || s == AIState.AttackBuilding;
                }
                if (wildlife != null)
                {
                    var s = wildlife.State;
                    return s == AnimalState.Chase || s == AnimalState.Attack;
                }
                return false;
            }
        }

        /// <summary>Tanpa suara saat Submerged, Dormant, atau BurnToAshes (Tick langkah 1).</summary>
        public bool IsSilent
        {
            get
            {
                if (ai == null) return false;
                var s = ai.CurrentState;
                return s == AIState.Submerged || s == AIState.Dormant || s == AIState.BurnToAshes || s == AIState.Dying;
            }
        }

        private void Awake() => Cache();

        private void Cache()
        {
            if (cached) return;
            cached = true;
            ai = GetComponentInParent<EnemyAI>();
            wildlife = GetComponentInParent<WildlifeAI>();
            health = GetComponentInParent<EnemyHealth>();
            if (source == null) source = GetComponent<AudioSource>();
            if (source == null) source = gameObject.AddComponent<AudioSource>();
            if (lowpass == null) lowpass = GetComponent<AudioLowPassFilter>();
            if (lowpass == null) lowpass = gameObject.AddComponent<AudioLowPassFilter>();
            ConfigureSource();
        }

        // Pengaturan sumber 3D (bagian "Arah suara 3D").
        private void ConfigureSource()
        {
            var b = GameManager.CurrentBalance != null ? GameManager.CurrentBalance.combatMusic : null;
            source.playOnAwake = false;
            source.loop = false;
            source.spatialBlend = 1f;
            source.rolloffMode = AudioRolloffMode.Logarithmic;
            source.minDistance = b != null ? b.cueMinDistance : 2f;
            source.maxDistance = cues != null ? cues.audibleRadius : 25f;
            source.dopplerLevel = 0f;
            source.spread = b != null ? b.cueSpread : 60f;
            // Volume pemain berlaku lewat grup mixer EnemyCue, bukan AudioSource.volume (bagian 4).
            if (CombatMusicDirector.Instance != null && CombatMusicDirector.Instance.EnemyCueGroup != null)
                source.outputAudioMixerGroup = CombatMusicDirector.Instance.EnemyCueGroup;
        }

        public void OnSpawned()
        {
            Cache();
            ConfigureSource();
            // Bagian 4 "Pakai ulang dari pool".
            lastEngaged = false;
            nextIdleTime = Time.time + NextIdleDelay();
            lowpass.cutoffFrequency = OpenCutoffHz;
            source.Stop();
            source.clip = null;
            source.volume = 1f;
            hasVoice = false;
            voiceFading = false;
            voiceClip = null;
            nextOcclusionTime = 0f;
            if (activeIndex < 0)
            {
                activeIndex = Active.Count;
                Active.Add(this);
            }
        }

        public void OnReturned()
        {
            // Objek dimatikan setelah ini, jadi fade 0,05 detik tidak bisa berjalan; suara dihentikan langsung.
            if (source != null) { source.Stop(); source.clip = null; }
            if (CombatMusicDirector.Instance != null) CombatMusicDirector.Instance.ReleaseVoice(this);
            hasVoice = false;
            voiceFading = false;
            RemoveFromActive();
        }

        internal void RemoveFromActive()
        {
            if (activeIndex < 0 || activeIndex >= Active.Count || Active[activeIndex] != this) { activeIndex = -1; return; }
            int last = Active.Count - 1;
            var moved = Active[last];
            Active[activeIndex] = moved;
            moved.activeIndex = activeIndex;
            Active.RemoveAt(last);
            activeIndex = -1;
        }

        // Suara tampilan saja, jadi memakai UnityEngine.Random biasa tanpa seed (#89).
        private float NextIdleDelay()
        {
            if (cues == null) return 10f;
            return Random.Range(cues.idleMinInterval, Mathf.Max(cues.idleMinInterval, cues.idleMaxInterval));
        }

        /// <summary>Dipanggil CombatMusicDirector, maksimal emitterTicksPerFrame emitter per frame.</summary>
        internal void Tick(float now, Vector3 listenerPos, CombatMusicDirector director)
        {
            if (cues == null) return;
            // 1. Diam saat Submerged, Dormant, atau BurnToAshes.
            // Zombie Lumut: napas dalam 6 m saat Dormant milik prefab #25, bukan komponen ini.
            if (IsSilent) { lastEngaged = false; return; }

            // 2. Baru masuk Chase atau menyerang: suara Alert.
            bool engaged = IsEngaged;
            if (engaged && !lastEngaged) director.RequestCue(this, ThreatCueKind.Alert, listenerPos);
            lastEngaged = engaged;

            // 3. Suara Idle terjadwal, hanya di dalam audibleRadius.
            if (now >= nextIdleTime)
            {
                nextIdleTime = now + NextIdleDelay();
                float r = cues.audibleRadius;
                if ((transform.position - listenerPos).sqrMagnitude <= r * r) director.RequestCue(this, ThreatCueKind.Idle, listenerPos);
            }
        }

        /// <summary>
        /// Suara serangan dari EnemyAttack.telegraphClip (#109), MossBehavior, dan WildlifeAI, lewat sumber 3D dan batas suara
        /// yang sama. Memakai priority + tellPriorityBonus, dan tidak dibatasi maxSameVariantVoices.
        /// </summary>
        public void PlayTell(AudioClip clip)
        {
            if (clip == null) return;
            var d = CombatMusicDirector.Instance;
            if (d == null)
            {
                if (source != null && isActiveAndEnabled) source.PlayOneShot(clip);
                return;
            }
            d.RequestTell(this, clip);
        }
    }
}
