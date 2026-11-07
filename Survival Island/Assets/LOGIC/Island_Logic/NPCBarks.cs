using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Ucapan singkat NPC yang menunjukkan kondisinya tanpa membuka menu. Blueprint: Island_Logic/LoreCollectibles.md
    /// bagian "Ucapan NPC" (#100). Ucapan disalurkan lewat subtitle #104 (SubtitleRequestedEvent, sourceKind Bark).
    /// Usulan disetujui developer: versi awal hanya teks, suara opsional; "lapar" mengomentari lapar PEMAIN, karena
    /// NPCRoster.md menyatakan NPC tidak punya Hunger. Nilai jeda mengikuti GameBalanceConfig.barks.
    ///
    /// Manager scene IslandPhase. TickLoop memeriksa satu NPC per putaran secara bergiliran (biaya kecil), memakai
    /// WaitForSeconds, jadi berhenti saat game dijeda. Jeda ucapan memakai Time.time dan tidak disimpan.
    /// </summary>
    public class NPCBarks : MonoBehaviour
    {
        private const int CategoryCount = 4;

        private class BarkRuntime
        {
            public float lastSpeakTime = -999f;
            public readonly float[] lastCategoryTime = { -999f, -999f, -999f, -999f };
            public readonly int[][] order = new int[CategoryCount][];
            public readonly int[] cursor = new int[CategoryCount];
            public bool wasNear;
            public float leftAt;      // Diisi Time.time saat dibuat: NPC baru tidak langsung menyapa
            public bool pendingGreet;
            public bool pendingZombie;
        }

        public static NPCBarks Instance { get; private set; }

        private readonly Dictionary<string, BarkRuntime> npcStates = new Dictionary<string, BarkRuntime>(4);
        private readonly Collider[] overlapBuffer = new Collider[32];
        private float lastGlobalBarkTime = -999f;
        private int nextIndex;
        private bool playerHungry;
        private bool runEnded;
        private Coroutine tickRoutine;

        private static BarksBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.barks : null;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[NPCBarks] Instance ganda di scene IslandPhase.");
                enabled = false;
                return;
            }
            Instance = this;
        }

        private void OnEnable()
        {
            EventChannel<NightfallEvent>.Raised += HandleNightfall;
            EventChannel<VitalWarningChangedEvent>.Raised += HandleVitalWarning;
            EventChannel<PlayerBaseStateChangedEvent>.Raised += HandlePlayerBaseState;
            EventChannel<NPCDiedEvent>.Raised += HandleNpcDied;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            if (EventChannel<VitalWarningChangedEvent>.HasLast) HandleVitalWarning(EventChannel<VitalWarningChangedEvent>.Last);
            tickRoutine = StartCoroutine(TickLoop());
        }

        private void OnDisable()
        {
            EventChannel<NightfallEvent>.Raised -= HandleNightfall;
            EventChannel<VitalWarningChangedEvent>.Raised -= HandleVitalWarning;
            EventChannel<PlayerBaseStateChangedEvent>.Raised -= HandlePlayerBaseState;
            EventChannel<NPCDiedEvent>.Raised -= HandleNpcDied;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            if (tickRoutine != null) StopCoroutine(tickRoutine);
            tickRoutine = null;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Event

        private void HandleNightfall(NightfallEvent e)
        {
            foreach (var kv in npcStates) kv.Value.pendingZombie = true;
        }

        private void HandleVitalWarning(VitalWarningChangedEvent e)
        {
            playerHungry = (e.flags & (VitalWarningFlags.HungerLow | VitalWarningFlags.Starving)) != 0;
        }

        private void HandlePlayerBaseState(PlayerBaseStateChangedEvent e)
        {
            if (!e.isInside) return;
            foreach (var kv in npcStates) kv.Value.pendingGreet = true;
        }

        private void HandleNpcDied(NPCDiedEvent e)
        {
            // NPC mati di tengah suara: AudioSource berhenti dan BarkRuntime dihapus.
            var roster = NPCRoster.Instance;
            var rec = roster != null ? roster.FindRecord(e.npcId) : null;
            if (rec != null && rec.companion != null && rec.companion.Voice != null) rec.companion.Voice.Stop();
            npcStates.Remove(e.npcId);
        }

        private void HandleRunEnded(RunEndedEvent e) => runEnded = true;

        // ---------------------------------------------------------------- Pemeriksaan berkala

        private IEnumerator TickLoop()
        {
            while (true)
            {
                var b = Bal;
                yield return new WaitForSeconds(b != null ? Mathf.Max(0.1f, b.checkSeconds) : 2f);
                if (b != null) TickOne(b);
            }
        }

        private void TickOne(BarksBalance b)
        {
            var roster = NPCRoster.Instance;
            if (roster == null || roster.Members.Count == 0) return;
            var members = roster.Members;
            if (nextIndex >= members.Count) nextIndex = 0;
            var rec = members[nextIndex];
            nextIndex++;
            // NPC yang dimatikan (#76) tidak berbicara.
            if (rec == null || !rec.isAlive || rec.companion == null || !rec.companion.isActiveAndEnabled) return;
            var npc = rec.companion;
            var rt = GetRuntime(rec.npcId);
            var player = PlayerTargetable.Instance;
            if (player == null) return;

            // Greeting: pemain masuk greetRadius setelah berpisah greetAwaySeconds.
            float dist = Vector3.Distance(npc.Position, player.Position);
            bool near = dist <= b.greetRadius;
            if (near && !rt.wasNear && Time.time - rt.leftAt >= b.greetAwaySeconds) rt.pendingGreet = true;
            if (!near && rt.wasNear) rt.leftAt = Time.time;
            rt.wasNear = near;

            // Pilih pemicu prioritas tertinggi: ZombieIncoming 5, Hurt 4, Hungry 2, Greeting 1.
            BarkCategory? pick = null;
            if (rt.pendingZombie || ZombieNear(npc.Position, b.zombieAlertRadius)) pick = BarkCategory.ZombieIncoming;
            else if (rec.data != null && rec.data.maxHP > 0f && rec.hp / rec.data.maxHP < b.hurtHpRatio) pick = BarkCategory.Hurt;
            else if (playerHungry) pick = BarkCategory.Hungry;
            else if (rt.pendingGreet) pick = BarkCategory.Greeting;
            if (pick == null) return;

            var cat = pick.Value;
            if (cat == BarkCategory.ZombieIncoming) rt.pendingZombie = false;
            if (cat == BarkCategory.Greeting) rt.pendingGreet = false;
            TrySpeak(npc, rec, rt, cat, dist, b);
        }

        /// <summary>Zombie Desa siang hari: hanya zombie aktif, bukan Submerged atau Dormant (#63).</summary>
        private bool ZombieNear(Vector3 pos, float radius)
        {
            int n = PhysicsQuery.OverlapSphere(pos, radius, PhysicsMasks.EnemyBody, overlapBuffer);
            bool found = false;
            for (int i = 0; i < n; i++)
            {
                var c = overlapBuffer[i];
                overlapBuffer[i] = null;
                if (found || c == null) continue;
                var e = c.GetComponentInParent<EnemyInstance>();
                if (e == null || !e.IsZombie) continue;
                if (e.AI != null && !e.AI.IsActiveThreat) continue;
                found = true;
            }
            return found;
        }

        private BarkRuntime GetRuntime(string npcId)
        {
            if (!npcStates.TryGetValue(npcId, out var rt))
            {
                rt = new BarkRuntime { leftAt = Time.time };
                npcStates.Add(npcId, rt);
            }
            return rt;
        }

        // ---------------------------------------------------------------- Berbicara

        private bool TrySpeak(NPCCompanion npc, NPCRecord rec, BarkRuntime rt, BarkCategory c, float distToPlayer, BarksBalance b)
        {
            // 1. Jeda, run berakhir, tidur, atau cinematic.
            var focus = UIFocusStack.Instance;
            if (focus != null && (focus.IsPaused || (focus.TopPanel != null && focus.TopPanel.id == UIPanelId.Cinematic))) return false;
            if (runEnded || (GameManager.Instance != null && GameManager.Instance.IsRunEnding)) return false;
            if (EventChannel<SleepStateChangedEvent>.HasLast && EventChannel<SleepStateChangedEvent>.Last.isSleeping) return false;
            // 2.
            if (distToPlayer > b.hearRadius) return false;
            int ci = (int)c;
            float now = Time.time;
            // 3.
            if (now - rt.lastCategoryTime[ci] < b.categoryCooldownSeconds) return false;
            // 4. ZombieIncoming melewati jeda global dan jeda per NPC.
            if (c != BarkCategory.ZombieIncoming &&
                (now - lastGlobalBarkTime < b.globalCooldownSeconds || now - rt.lastSpeakTime < b.npcCooldownSeconds)) return false;
            // 5.
            var set = rec.data != null ? rec.data.barkSet : null;
            var lines = set != null ? set.Get(c) : null;
            if (lines == null || lines.Length == 0) return false;
            int lineIndex = NextLine(rt, ci, lines.Length);
            var line = lines[lineIndex];
            if (string.IsNullOrEmpty(line.textKey)) return false;
            // 6.
            string speaker = rec.data != null ? rec.data.displayNameKey : string.Empty;
            EventChannel<SubtitleRequestedEvent>.Publish(new SubtitleRequestedEvent(line.textKey, speaker, 1, SubtitleSourceKind.Bark));
            // 7. Suara opsional pada AudioSource milik NPCCompanion.
            if (line.clip != null && npc.Voice != null) npc.Voice.PlayOneShot(line.clip);
            // 8.
            rt.lastCategoryTime[ci] = now;
            rt.lastSpeakTime = now;
            lastGlobalBarkTime = now;
            return true;
        }

        /// <summary>Urutan acak tanpa mengulang sebelum semua terpakai. Baris terakhir tidak menjadi baris pertama berikutnya.</summary>
        private static int NextLine(BarkRuntime rt, int ci, int count)
        {
            var order = rt.order[ci];
            if (order == null || order.Length != count)
            {
                order = new int[count];
                for (int i = 0; i < count; i++) order[i] = i;
                rt.order[ci] = order;
                rt.cursor[ci] = count;   // Paksa acak di bawah
            }
            if (rt.cursor[ci] >= count)
            {
                int last = order[count - 1];
                // UnityEngine.Random: ucapan tidak memengaruhi isi dunia, jadi tidak memakai RunSeedManager.
                for (int i = count - 1; i > 0; i--)
                {
                    int j = Random.Range(0, i + 1);
                    (order[i], order[j]) = (order[j], order[i]);
                }
                if (count > 1 && order[0] == last)
                {
                    int j = Random.Range(1, count);
                    (order[0], order[j]) = (order[j], order[0]);
                }
                rt.cursor[ci] = 0;
            }
            return order[rt.cursor[ci]++];
        }
    }
}
