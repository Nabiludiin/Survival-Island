using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Daftar sarang aktif dan pemetaan zombie ke sarangnya, untuk pencarian tanpa alokasi.
    /// Blueprint: left-4-dead/ZombieLair.md bagian "Pemunculan" (#112). Manager di IslandPhase.
    /// Tick tiap lair.registryTickSeconds (1 detik) memeriksa jarak pemain ke tiap sarang di region yang dimuat.
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - EnemyAI (kode Langkah 5) belum punya field lair. Rujukan zombie ke sarang disimpan di sini (GetLair, IsLairZombie).
    /// - RegionStreaming (#88) belum ada: semua sarang aktif di scene dianggap berada di region yang dimuat.
    /// </summary>
    public class LairRegistry : MonoBehaviour
    {
        public static LairRegistry Instance { get; private set; }

        private static readonly List<ZombieLair> lairs = new List<ZombieLair>(16);
        private static readonly Dictionary<int, ZombieLair> byEnemy = new Dictionary<int, ZombieLair>(64);
        private float tickTimer;

        private static LairBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.lair : null;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            lairs.Clear();
            byEnemy.Clear();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Debug.LogError("[LairRegistry] Lebih dari satu instance."); enabled = false; return; }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Daftar

        public static void Register(ZombieLair lair)
        {
            if (lair != null && !lairs.Contains(lair)) lairs.Add(lair);
        }

        public static void Unregister(ZombieLair lair)
        {
            lairs.Remove(lair);
        }

        public static IReadOnlyList<ZombieLair> All => lairs;

        internal static void MapEnemy(EnemyAI ai, ZombieLair lair)
        {
            if (ai != null) byEnemy[ai.GetInstanceID()] = lair;
        }

        internal static void UnmapEnemy(EnemyAI ai)
        {
            if (ai != null) byEnemy.Remove(ai.GetInstanceID());
        }

        /// <summary>Pengganti EnemyAI.lair: sarang zombie ini, atau null untuk zombie horde.</summary>
        public static ZombieLair GetLair(EnemyAI ai)
        {
            return ai != null && byEnemy.TryGetValue(ai.GetInstanceID(), out var l) ? l : null;
        }

        public static bool IsLairZombie(EnemyAI ai) => GetLair(ai) != null;

        /// <summary>
        /// Zombie sarang di dalam InteriorZone terlindung dari matahari (#63). Dipakai SunlightExposure.CheckExposure.
        /// </summary>
        public static bool IsShelteredLairZombie(EnemyAI ai)
        {
            var l = GetLair(ai);
            return l != null && l.Zone != null && l.Zone.ContainsPoint(ai.transform.position);
        }

        /// <summary>
        /// Usulan 3: pengurangan anggaran horde region dari sarang yang hancur, clearedBudgetReduction per sarang,
        /// dibatasi maxBudgetReduction. Dihitung ulang dari status sarang, jadi tetap benar setelah load. Dipakai SpawnManager (#62).
        /// </summary>
        public static float GetBudgetReduction(RegionType region)
        {
            var b = Bal;
            if (b == null || !b.enabled) return 0f;
            int destroyed = 0;
            for (int i = 0; i < lairs.Count; i++)
            {
                var l = lairs[i];
                if (l != null && l.Data != null && l.Data.region == region && l.Data.hasNest && l.IsNestDestroyed) destroyed++;
            }
            return Mathf.Min(b.maxBudgetReduction, destroyed * b.clearedBudgetReduction);
        }

        // ---------------------------------------------------------------- Tick

        private void Update()
        {
            var b = Bal;
            if (b == null || Time.deltaTime <= 0f) return;
            // Game dijeda: Time.deltaTime 0, jadi berhenti.
            tickTimer -= Time.deltaTime;
            if (tickTimer > 0f) return;
            tickTimer = b.registryTickSeconds;
            var player = PlayerTargetable.Instance;
            if (player == null) return;
            Vector3 p = player.Position;
            float act2 = b.activationRadius * b.activationRadius;
            float deact2 = b.deactivationRadius * b.deactivationRadius;
            for (int i = lairs.Count - 1; i >= 0; i--)
            {
                var l = lairs[i];
                if (l == null) { lairs.RemoveAt(i); continue; }
                float d2 = (l.transform.position - p).sqrMagnitude;
                // 1. Sakelar uji lair.enabled = false: tidak ada zombie sarang.
                if (b.enabled && d2 < act2 && !l.IsSpawned && l.RemainingCount > 0) l.SpawnDormant();
                // 2. Zombie kembali ke pool, remainingCount tetap.
                else if (l.IsSpawned && (d2 > deact2 || !b.enabled)) l.Despawn();
            }
        }
    }
}
