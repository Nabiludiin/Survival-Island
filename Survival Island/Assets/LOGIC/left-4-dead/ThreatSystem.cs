using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Data ThreatSystem per musuh. Blueprint: ThreatSystem.md bagian "Variabel per musuh" (#108).
    /// Catatan Langkah 5: ditulis sebagai class (satu objek per musuh, dibuat sekali di Awake) supaya bisa dibagi lewat
    /// IThreatAgent tanpa ref. Tidak ada alokasi per tick.
    /// </summary>
    public class ThreatState
    {
        public ITargetable current;
        public float heldUntil;
        public float lastSeenTime;
        public Vector3 lastSeenPosition;
        public ITargetable aggroTarget;
        public float aggroUntil;
        public int unreachableId = -1;
        public float unreachableUntil;
        public float nextEvalTime;

        public void Reset(float nextEval)
        {
            current = null; heldUntil = 0f; lastSeenTime = 0f; lastSeenPosition = Vector3.zero;
            aggroTarget = null; aggroUntil = 0f; unreachableId = -1; unreachableUntil = 0f; nextEvalTime = nextEval;
        }
    }

    /// <summary>Musuh yang dinilai ThreatSystem: EnemyAI dan WildlifeAI.</summary>
    public interface IThreatAgent
    {
        ThreatState Threat { get; }
        Vector3 Position { get; }
        ThreatProfile Profile { get; }
        float SenseRange { get; }
        bool CanBeEvaluated { get; }                 // false saat Stagger, Dying, Dormant, dan lainnya
        AISensory Sensory { get; }                   // null untuk hewan
        bool IsWithinTerritory(Vector3 position);    // Zombie selalu true
        void SetTarget(ITargetable target);
    }

    /// <summary>
    /// Satu-satunya penulis target musuh di Fase 2. Blueprint: left-4-dead/ThreatSystem.md (#108).
    /// GDD bagian 3: hewan menjaga teritori, zombie memburu MC dan menyerang base. Rumus dan bobot adalah usulan disetujui developer.
    /// Tidak ada di ShipPhase; zombie kapal memakai EnemyAI.SetTarget langsung.
    /// Override Lure (#96) memakai SenseRange sebagai radius umpan. Override BaseAnchor saat Blood Moon (#50) membaca
    /// HordeManager.IsBloodMoonActive. Batas pengincar NPC (maxChasersPerNpc) dipakai di Score.
    /// </summary>
    public class ThreatSystem : MonoBehaviour
    {
        public static ThreatSystem Instance { get; private set; }

        private readonly List<ITargetable> candidates = new List<ITargetable>(16);
        private readonly List<int> chaserCounts = new List<int>(16);
        private readonly List<IThreatAgent> agents = new List<IThreatAgent>(64);
        private int cursor;

        private static ThreatBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.threat : null;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Debug.LogError("[ThreatSystem] Lebih dari satu instance di scene."); enabled = false; return; }
            Instance = this;
        }

        private void OnEnable()
        {
            EventChannel<PlayerDiedEvent>.Raised += HandlePlayerDied;
            if (PlayerTargetable.Instance != null) RegisterCandidate(PlayerTargetable.Instance);
        }

        private void OnDisable()
        {
            EventChannel<PlayerDiedEvent>.Raised -= HandlePlayerDied;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Pendaftaran

        public void RegisterCandidate(ITargetable t)
        {
            if (t == null || candidates.Contains(t)) return;
            var b = Bal;
            if (b != null && candidates.Count >= b.maxCandidates) { Debug.LogWarning("[ThreatSystem] Kandidat penuh (maxCandidates)."); return; }
            candidates.Add(t);
            chaserCounts.Add(0);
        }

        public void UnregisterCandidate(ITargetable t)
        {
            int i = candidates.IndexOf(t);
            if (i < 0) return;
            candidates.RemoveAt(i);
            chaserCounts.RemoveAt(i);
            for (int a = 0; a < agents.Count; a++)
            {
                var s = agents[a].Threat;
                if (s.current == t) { s.current = null; agents[a].SetTarget(null); }
                if (s.aggroTarget == t) s.aggroTarget = null;
            }
        }

        public void Register(IThreatAgent agent)
        {
            if (agent == null || agents.Contains(agent)) return;
            agents.Add(agent);
        }

        /// <summary>Aman dipanggil dua kali (LuckyMachine #72 dan EnemyAI.OnReturned).</summary>
        public void Unregister(IThreatAgent agent)
        {
            int i = agents.IndexOf(agent);
            if (i < 0) return;
            SetCurrent(agent, null);
            agent.Threat.aggroTarget = null;
            int last = agents.Count - 1;
            agents[i] = agents[last];
            agents.RemoveAt(last);
            if (cursor > agents.Count) cursor = 0;
        }

        // ---------------------------------------------------------------- Masukan dari sistem lain

        /// <summary>Dipanggil EnemyAI/WildlifeAI saat dilukai pemain atau NPC. Override 1.</summary>
        public void ReportAttacker(IThreatAgent agent, ITargetable attacker)
        {
            var b = Bal;
            if (agent == null || attacker == null || b == null) return;
            agent.Threat.aggroTarget = attacker;
            agent.Threat.aggroUntil = Time.time + b.aggroOverrideSeconds;
            agent.Threat.nextEvalTime = 0f;   // Dinilai pada giliran berikutnya
        }

        /// <summary>Target saat ini tidak terjangkau tanpa struktur (#78/#94): dicoret sementara.</summary>
        public void ReportUnreachable(IThreatAgent agent)
        {
            var b = Bal;
            if (agent == null || b == null || agent.Threat.current == null) return;
            agent.Threat.unreachableId = agent.Threat.current.TargetId;
            agent.Threat.unreachableUntil = Time.time + b.unreachableBanSeconds;
            SetCurrent(agent, null);
            agent.Threat.nextEvalTime = 0f;
        }

        /// <summary>Minta penilaian ulang segera (SpecialZombieAI pengaman macet, #25).</summary>
        public void RequestReevaluate(IThreatAgent agent)
        {
            if (agent != null) agent.Threat.nextEvalTime = 0f;
        }

        /// <summary>Jumlah pengincar target (ThreatSystem.md "Membagi target").</summary>
        public int GetChaserCount(ITargetable t)
        {
            int i = candidates.IndexOf(t);
            return i >= 0 ? chaserCounts[i] : 0;
        }

        // ---------------------------------------------------------------- Penilaian

        private void Update()
        {
            var b = Bal;
            if (b == null || agents.Count == 0) return;
            float now = Time.time;
            int processed = 0;
            int checkedCount = 0;
            while (processed < b.maxSelectionsPerFrame && checkedCount < agents.Count)
            {
                if (cursor >= agents.Count) cursor = 0;
                var a = agents[cursor];
                cursor++;
                checkedCount++;
                if (a == null || now < a.Threat.nextEvalTime) continue;
                Evaluate(a, b, now);
                processed++;
            }
        }

        private void Evaluate(IThreatAgent a, ThreatBalance b, float now)
        {
            var s = a.Threat;
            float dist = s.current != null ? Vector3.Distance(a.Position, s.current.Position) : 0f;
            s.nextEvalTime = now + (dist > b.farDistance ? b.farReselectSeconds : b.reselectSeconds);

            // 1. State reaksi, sembunyi, dan Dormant dilewati.
            if (!a.CanBeEvaluated) return;

            // 2. Target tidak sah dilepas tanpa menunggu waktu minimum.
            if (s.current != null && !s.current.IsTargetable) SetCurrent(a, null);

            // Garis pandang dibaca dari AISensory (tanpa raycast di sini).
            var sensory = a.Sensory;
            if (s.current != null && sensory != null && sensory.CanSee(s.current))
            {
                s.lastSeenTime = now;
                s.lastSeenPosition = s.current.Position;
            }
            if (s.unreachableId >= 0 && now >= s.unreachableUntil) s.unreachableId = -1;

            // 3. Override 1: penyerang terakhir. Mengabaikan heldUntil dan switchMargin.
            if (s.aggroTarget != null && now < s.aggroUntil && s.aggroTarget.IsTargetable && a.IsWithinTerritory(s.aggroTarget.Position))
            {
                if (s.current != s.aggroTarget) SetCurrent(a, s.aggroTarget);
                return;
            }
            // Override 2: umpan Suar terdekat dalam SenseRange, untuk profil reactsToLure (#96).
            var lureProfile = a.Profile;
            if (lureProfile == null || lureProfile.reactsToLure)
            {
                ITargetable lure = null;
                float lureSqr = a.SenseRange * a.SenseRange;
                for (int i = 0; i < candidates.Count; i++)
                {
                    var t = candidates[i];
                    if (t == null || t.Kind != TargetKind.Lure || !t.IsTargetable) continue;
                    float sqr = (t.Position - a.Position).sqrMagnitude;
                    if (sqr <= lureSqr) { lureSqr = sqr; lure = t; }
                }
                if (lure != null)
                {
                    if (s.current != lure) SetCurrent(a, lure);
                    return;
                }
            }
            // Override 3: Blood Moon, zombie menuju BaseAnchor (#50). Pemain/NPC terlihat dalam bloodMoonPlayerRange tetap
            // diserang lewat skor biasa. Tanpa base, BaseAnchor tidak sah dan zombie memburu pemain.
            if (a is EnemyAI && HordeManager.Instance != null && HordeManager.Instance.IsBloodMoonActive)
            {
                ITargetable anchor = null;
                bool closeVisible = false;
                float closeSqr = b.bloodMoonPlayerRange * b.bloodMoonPlayerRange;
                for (int i = 0; i < candidates.Count; i++)
                {
                    var t = candidates[i];
                    if (t == null || !t.IsTargetable) continue;
                    if (t.Kind == TargetKind.BaseAnchor) anchor = t;
                    else if ((t.Kind == TargetKind.Player || t.Kind == TargetKind.NPC)
                             && (t.Position - a.Position).sqrMagnitude <= closeSqr && sensory != null && sensory.CanSee(t))
                        closeVisible = true;
                }
                if (anchor != null && !closeVisible)
                {
                    if (s.current != anchor) SetCurrent(a, anchor);
                    return;
                }
            }

            // 4. Skor biasa.
            var profile = a.Profile;
            ITargetable best = null;
            float bestScore = float.MinValue;
            float currentScore = float.MinValue;
            for (int i = 0; i < candidates.Count; i++)
            {
                var t = candidates[i];
                float score = Score(a, profile, t, i, b);
                if (float.IsNegativeInfinity(score)) continue;
                if (t == s.current) currentScore = score;
                if (score > bestScore) { bestScore = score; best = t; }
            }

            // Target hilang terlalu lama: profil tanpa huntsUnseen melepas target (hewan).
            bool hunts = profile == null || profile.huntsUnseen;
            if (s.current != null && !hunts && s.lastSeenTime > 0f && now - s.lastSeenTime > b.loseSightSeconds)
            {
                SetCurrent(a, null);
                return;
            }

            if (best == null) return;
            // 5. Pencegah berganti.
            if (s.current == null || (now >= s.heldUntil && bestScore > currentScore + b.switchMargin))
            {
                if (best != s.current)
                {
                    SetCurrent(a, best);
                    s.heldUntil = now + b.minTargetHoldSeconds;
                    if (sensory != null && sensory.CanSee(best)) { s.lastSeenTime = now; s.lastSeenPosition = best.Position; }
                }
            }
        }

        private float Score(IThreatAgent a, ThreatProfile p, ITargetable t, int index, ThreatBalance b)
        {
            if (t == null || !t.IsTargetable) return float.NegativeInfinity;
            if (p != null && !p.Allows(t.Kind)) return float.NegativeInfinity;
            if (t.TargetId == a.Threat.unreachableId) return float.NegativeInfinity;
            if (!a.IsWithinTerritory(t.Position)) return float.NegativeInfinity;
            float range = a.SenseRange;
            float sqr = (t.Position - a.Position).sqrMagnitude;
            if (sqr > range * range) return float.NegativeInfinity;
            int chasers = chaserCounts[index] - (a.Threat.current == t ? 1 : 0);
            if (t.Kind == TargetKind.NPC && chasers >= b.maxChasersPerNpc) return float.NegativeInfinity;

            float kindWeight = p != null ? p.KindWeight(t.Kind) : 40f;
            float distWeight = p != null ? p.distanceWeight : 30f;
            float visBonus = p != null ? p.visibleBonus : 25f;
            float dmgBonus = p != null ? p.recentDamageBonus : 30f;
            float dist = Mathf.Sqrt(sqr);
            float score = kindWeight * t.ThreatWeight + distWeight * (1f - dist / Mathf.Max(0.01f, range));
            if (a.Sensory != null && a.Sensory.CanSee(t)) score += visBonus;
            if (a.Threat.aggroTarget == t && Time.time < a.Threat.aggroUntil + b.aggroOverrideSeconds) score += dmgBonus;
            score -= b.crowdPenalty * Mathf.Max(0, chasers);
            return score;
        }

        private void SetCurrent(IThreatAgent a, ITargetable t)
        {
            var s = a.Threat;
            if (s.current == t) return;
            if (s.current != null)
            {
                int i = candidates.IndexOf(s.current);
                if (i >= 0 && chaserCounts[i] > 0) chaserCounts[i]--;
            }
            s.current = t;
            if (t != null)
            {
                int i = candidates.IndexOf(t);
                if (i >= 0) chaserCounts[i]++;
            }
            a.SetTarget(t);
        }

        private void HandlePlayerDied(PlayerDiedEvent e)
        {
            for (int i = 0; i < agents.Count; i++) SetCurrent(agents[i], null);
        }
    }
}
