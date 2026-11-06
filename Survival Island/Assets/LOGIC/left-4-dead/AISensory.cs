using System;
using UnityEngine;

namespace SurvivalIsland
{
    public enum AwarenessLevel : byte { Unaware, Suspicious, Aware }
    public enum NoiseSourceKind : byte { Movement, Landing, Gunshot, Melee, Tool, Container, Decoy, Structure, Throwable }

    /// <summary>
    /// Pandangan dan pendengaran satu zombie. Blueprint: left-4-dead/AISensory.md (#49).
    /// GDD bagian 3 (Mengendap): zombie mendeteksi pemain lewat pandangan dan suara, sehingga pemain bisa menghindar
    /// dengan mengendap, kecuali saat Blood Moon. Tick dibagi AISensoryHub, jadi tidak ada Update di sini.
    /// </summary>
    public class AISensory : MonoBehaviour, IPoolable
    {
        [SerializeField] private Transform eyePoint;          // Kosong = posisi + 1,6 m
        [SerializeField] private float eyeHeight = 1.6f;

        public float Detection01 { get; private set; }
        public AwarenessLevel Level { get; private set; } = AwarenessLevel.Unaware;
        public Vector3 LastStimulusPosition { get; private set; }
        public bool CanSeePlayer { get; private set; }
        public event Action<AwarenessLevel> LevelChanged;

        internal float lastTickTime;
        internal int hubIndex = -1;

        private bool canSeeCurrentTarget;
        private float sinceLastStimulus;
        private ZombieData zombieData;
        private EnemyAI ai;

        private Vector3 Eye => eyePoint != null ? eyePoint.position : transform.position + Vector3.up * eyeHeight;

        private void Awake()
        {
            ai = GetComponent<EnemyAI>();
            var health = GetComponent<EnemyHealth>();
            zombieData = health != null ? health.Data as ZombieData : null;
            LastStimulusPosition = transform.position;
        }

        private void OnEnable()
        {
            EventChannel<NoiseEmittedEvent>.Raised += HandleNoise;
            if (AISensoryHub.Instance != null) AISensoryHub.Instance.Register(this);
        }

        private void OnDisable()
        {
            EventChannel<NoiseEmittedEvent>.Raised -= HandleNoise;
            if (AISensoryHub.Instance != null) AISensoryHub.Instance.Unregister(this);
        }

        private bool Skip => ai != null && (ai.CurrentState == AIState.Submerged || ai.CurrentState == AIState.BurnToAshes
                                         || ai.CurrentState == AIState.Dying);

        /// <summary>Dipanggil AISensoryHub dengan selisih waktu sejak tick terakhir.</summary>
        public void Tick(float dt)
        {
            var bal = GameManager.CurrentBalance;
            var hub = AISensoryHub.Instance;
            var player = PlayerTargetable.Instance;
            if (bal == null || hub == null || player == null) return;
            if (Skip || (ai != null && ai.CurrentState == AIState.Dormant)) return;   // Dormant tetap mendengar suara (#112)
            var s = bal.stealth;

            // 1. Blood Moon atau saklar tes: aggro penuh (Claude.md Pilar 3).
            if (hub.IgnoreStealth || !s.enabled)
            {
                Detection01 = 1f;
                LastStimulusPosition = player.Position;
                sinceLastStimulus = 0f;
                CanSeePlayer = true;
                UpdateLevel(s);
                return;
            }

            // 2-5. Kerucut pandang dan garis pandang.
            float sightMult = zombieData != null ? zombieData.sightMultiplier : 1f;
            float range = s.sightRange * sightMult * hub.PlayerVisibility * hub.WeatherSight;
            Vector3 toPlayer = player.Position - transform.position;
            float sqr = toPlayer.sqrMagnitude;
            bool seen = false;
            if (sqr <= range * range)
            {
                float dist = Mathf.Sqrt(sqr);
                bool inCone = dist <= s.closeSenseRange || Vector3.Angle(transform.forward, toPlayer) <= s.sightAngleDeg * 0.5f;
                if (inCone) seen = HasLineOfSight(hub.PlayerEyePosition) || HasLineOfSight(player.AimPoint);
                if (seen)
                {
                    // 6. Laju isi lebih cepat di jarak dekat.
                    float rate = Mathf.Lerp(s.sightGainMin, s.sightGainMax, 1f - dist / Mathf.Max(0.01f, range)) * hub.PlayerVisibility;
                    Detection01 = Mathf.Min(1f, Detection01 + rate * dt);
                    LastStimulusPosition = player.Position;
                    sinceLastStimulus = 0f;
                }
            }
            CanSeePlayer = seen;

            // 7. Turun tanpa rangsangan, kecuali Aware yang masih dalam loseAwareSeconds.
            if (!seen)
            {
                sinceLastStimulus += dt;
                if (!(Level == AwarenessLevel.Aware && sinceLastStimulus <= s.loseAwareSeconds))
                    Detection01 = Mathf.Max(0f, Detection01 - s.decayPerSec * dt);
            }

            // 8. Garis pandang ke target aktif yang bukan pemain.
            var target = ai != null ? ai.CurrentTarget : null;
            canSeeCurrentTarget = target != null && target.Kind != TargetKind.Player && HasLineOfSight(target.AimPoint);

            UpdateLevel(s);
        }

        private bool HasLineOfSight(Vector3 point)
        {
            Vector3 from = Eye;
            Vector3 dir = point - from;
            float len = dir.magnitude;
            if (len < 0.01f) return true;
            return !PhysicsQuery.Raycast(new Ray(from, dir / len), out _, len, PhysicsMasks.AIVision);
        }

        /// <summary>Tanpa raycast: hasil tersimpan untuk pemain dan target aktif, false untuk lainnya (#108).</summary>
        public bool CanSee(ITargetable t)
        {
            if (t == null) return false;
            if (t.Kind == TargetKind.Player) return CanSeePlayer;
            return ai != null && ai.CurrentTarget == t && canSeeCurrentTarget;
        }

        private void HandleNoise(NoiseEmittedEvent e)
        {
            var bal = GameManager.CurrentBalance;
            if (bal == null || Skip) return;
            var s = bal.stealth;
            var hub = AISensoryHub.Instance;
            float hearMult = zombieData != null ? zombieData.hearingMultiplier : 1f;
            float r = e.radius * hearMult * (hub != null ? hub.WeatherHearing : 1f);
            float d = Vector3.Distance(transform.position, e.position);
            if (d > r) return;
            // 2. Raycast hanya di lapisan luar, supaya 50 zombie tidak menembak ray tiap langkah pemain.
            if (d > r * s.wallMuffle && !HasLineOfSight(e.position + Vector3.up * 0.5f)) return;
            // 3.
            LastStimulusPosition = e.position;
            sinceLastStimulus = 0f;
            // 4-6.
            if (e.sourceKind == NoiseSourceKind.Gunshot) Detection01 = 1f;
            else if (e.sourceKind == NoiseSourceKind.Decoy) Detection01 = Mathf.Max(Detection01, s.suspiciousThreshold);
            else Detection01 = Mathf.Min(1f, Detection01 + s.hearGain * (1f - d / Mathf.Max(0.01f, r)));
            if (e.sourceKind == NoiseSourceKind.Decoy && Detection01 >= 1f) Detection01 = 0.99f;
            UpdateLevel(s);
        }

        /// <summary>Paksa Aware (RequestAmbush, kebangkitan Zombie Desa, dilukai).</summary>
        public void ForceAware(Vector3 stimulus)
        {
            Detection01 = 1f;
            LastStimulusPosition = stimulus;
            sinceLastStimulus = 0f;
            var bal = GameManager.CurrentBalance;
            if (bal != null) UpdateLevel(bal.stealth);
        }

        private void UpdateLevel(StealthBalance s)
        {
            var prev = Level;
            if (Detection01 >= 1f) Level = AwarenessLevel.Aware;
            else if (Level == AwarenessLevel.Aware)
            {
                if (sinceLastStimulus > s.loseAwareSeconds) { Level = AwarenessLevel.Suspicious; Detection01 = 0.6f; }
            }
            else if (Level == AwarenessLevel.Unaware && Detection01 >= s.suspiciousThreshold) Level = AwarenessLevel.Suspicious;
            else if (Level == AwarenessLevel.Suspicious && Detection01 < s.suspiciousThreshold) Level = AwarenessLevel.Unaware;
            if (Level != prev) LevelChanged?.Invoke(Level);
        }

        public void OnSpawned()
        {
            Detection01 = 0f;
            Level = AwarenessLevel.Unaware;
            CanSeePlayer = false;
            canSeeCurrentTarget = false;
            sinceLastStimulus = 0f;
            LastStimulusPosition = transform.position;
        }

        public void OnReturned() { }
    }
}
