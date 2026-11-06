using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Kelompok struktur yang saling berdekatan. Dihitung ulang, tidak disimpan (RemoteBaseSimulation.md, #90).</summary>
    public class BaseCluster
    {
        public string clusterId;        // structureId terkecil di dalamnya
        public Vector3 center;
        public RegionType region;
        public readonly List<StructureNode> nodes = new List<StructureNode>(32);
        public bool isRemote;

        // Hasil pemilahan saat dibangun ulang.
        public readonly List<BaseHealth> walls = new List<BaseHealth>(16);
        public readonly List<BaseHealth> doors = new List<BaseHealth>(4);
        public readonly List<BaseHealth> traps = new List<BaseHealth>(8);
        public readonly List<BaseHealth> inner = new List<BaseHealth>(16);
    }

    /// <summary>Rencana serangan satu cluster untuk satu malam. Disimpan di world_remotebase.</summary>
    [Serializable]
    public class RaidPlan
    {
        public string clusterId;
        public int day;
        public float netDamage;         // Hasil rumus
        public float appliedFraction;   // 0 sampai 1, bagian yang sudah diterapkan
        public float startHour, endHour;
        public bool notified;
        public bool isTakenOver;        // Sudah beralih ke serangan nyata
        public int structuresLost, trapsWorn, npcInjured, npcDead;
        // Tambahan sistem pendukung (Catatan Implementasi di blueprint):
        public Vector3 center;          // Mencari cluster lagi kalau structureId terkecil hancur
        public RegionType region;
        public float wallMaxHpTotal;    // Total maxHP dinding dan pintu saat rencana dibuat (tekanan NPC)
        public float hitCarry;          // Sisa pecahan pukulan yang dibawa ke panggilan berikutnya
        public int wallsLost;           // Dinding atau pintu yang hancur malam ini (isBreached)
    }

    /// <summary>
    /// Nasib base yang ditinggal jauh saat malam, dihitung dari data tanpa zombie sungguhan dan tanpa NavMesh.
    /// Blueprint: Island_Logic/RemoteBaseSimulation.md (#90). GDD bagian 3: horde menyerang base saat malam. Bagian 4: base
    /// melindungi NPC. Bagian 5: bolak-balik Region 4 ke Region 3. Bagian 6: bencana. Aturan, rumus, dan angka adalah usulan
    /// disetujui developer (#85).
    ///
    /// Tiga keadaan: Nyata (pemain dalam returnDistance dan region dimuat, sistem ini diam), Simulasi (malam, pemain di atas
    /// remoteDistance atau region tidak dimuat), Aman (siang, atau pos sementara). Hasil dihitung sekali saat NightfallEvent,
    /// sehingga save dan load tidak mengubahnya, dan hanya selisih yang diterapkan, sehingga damage tidak pernah ganda.
    ///
    /// Catatan implementasi (sistem pendukung, juga di blueprint):
    /// - StructureRegistry tidak punya daftar publik: struktur dikumpulkan lewat FindObjectsByType (termasuk nonaktif) saat
    ///   cluster dibangun ulang (jarang: senja, perubahan struktur, load), pola BaseNavigation.RescanStructures.
    /// - Tier dinding dari StructureIntegrityInfo (#47). Dinding/pintu dikenali dari tag BaseWall/BaseDoor.
    ///   Jebakan = struktur dengan BaseTrap tanpa CampfireManager (Spike dan BarbedWire).
    /// - NPC penjaga memakai StayAtBase, Work, atau TakeCover, karena saat malam NPC base berada di TakeCover.
    /// </summary>
    public class RemoteBaseSimulation : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "world_remotebase";

        public static RemoteBaseSimulation Instance { get; private set; }

        private readonly List<BaseCluster> clusters = new List<BaseCluster>(4);
        private readonly List<RaidPlan> plans = new List<RaidPlan>(2);
        private readonly List<RaidPlan> pendingReports = new List<RaidPlan>(2);
        private readonly List<BaseHealth> targetBuffer = new List<BaseHealth>(32);
        private bool clustersDirty = true;
        private bool isSleeping;
        private bool bloodMoonActive;
        private bool runEnded;
        private bool registeredForSave;
        private Coroutine distanceRoutine;

        public IReadOnlyList<RaidPlan> Plans => plans;
        public IReadOnlyList<BaseCluster> Clusters => clusters;

        private static RemoteBaseBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.remoteBase : null;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[RemoteBaseSimulation] Instance ganda di scene IslandPhase.");
                enabled = false;
                return;
            }
            Instance = this;
        }

        private void OnEnable()
        {
            EventChannel<NightfallEvent>.Raised += HandleNightfall;
            EventChannel<DaybreakEvent>.Raised += HandleDaybreak;
            EventChannel<HourChangedEvent>.Raised += HandleHour;
            EventChannel<StructureBuiltEvent>.Raised += HandleStructureBuilt;
            EventChannel<StructureDestroyedEvent>.Raised += HandleStructureDestroyed;
            EventChannel<BloodMoonStartedEvent>.Raised += HandleBloodMoonStarted;
            EventChannel<BloodMoonEndedEvent>.Raised += HandleBloodMoonEnded;
            EventChannel<SleepStateChangedEvent>.Raised += HandleSleep;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            if (EventChannel<SleepStateChangedEvent>.HasLast) isSleeping = EventChannel<SleepStateChangedEvent>.Last.isSleeping;
            if (WorldStateSave.Instance != null)
            {
                WorldStateSave.Instance.Register(this);
                registeredForSave = true;
            }
            distanceRoutine = StartCoroutine(DistanceLoop());
        }

        private void OnDisable()
        {
            EventChannel<NightfallEvent>.Raised -= HandleNightfall;
            EventChannel<DaybreakEvent>.Raised -= HandleDaybreak;
            EventChannel<HourChangedEvent>.Raised -= HandleHour;
            EventChannel<StructureBuiltEvent>.Raised -= HandleStructureBuilt;
            EventChannel<StructureDestroyedEvent>.Raised -= HandleStructureDestroyed;
            EventChannel<BloodMoonStartedEvent>.Raised -= HandleBloodMoonStarted;
            EventChannel<BloodMoonEndedEvent>.Raised -= HandleBloodMoonEnded;
            EventChannel<SleepStateChangedEvent>.Raised -= HandleSleep;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            if (registeredForSave && WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            registeredForSave = false;
            if (distanceRoutine != null) StopCoroutine(distanceRoutine);
            distanceRoutine = null;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Cluster

        private void EnsureClusters()
        {
            if (clustersDirty) RebuildClusters();
        }

        private void RebuildClusters()
        {
            clustersDirty = false;
            var b = Bal;
            float radius = b != null ? b.clusterRadius : 30f;
            float r2 = radius * radius;

            // Simpan status isRemote lama per clusterId, supaya peralihan tidak berulang setelah pembangunan ulang.
            var oldRemote = new Dictionary<string, bool>(clusters.Count, StringComparer.Ordinal);
            for (int i = 0; i < clusters.Count; i++) oldRemote[clusters[i].clusterId] = clusters[i].isRemote;
            clusters.Clear();

            // Termasuk objek nonaktif di region yang dilepas (#88).
            var nodes = FindObjectsByType<StructureNode>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            var valid = new List<StructureNode>(nodes.Length);
            for (int i = 0; i < nodes.Length; i++)
            {
                var n = nodes[i];
                if (n == null || string.IsNullOrEmpty(n.StructureId)) continue;
                var h = n.GetComponent<BaseHealth>();
                if (h == null || !h.IsAlive) continue;
                valid.Add(n);
            }

            // Gabung tunggal: struktur yang berjarak kurang dari clusterRadius masuk satu cluster.
            var assigned = new bool[valid.Count];
            var queue = new Queue<int>();
            for (int s = 0; s < valid.Count; s++)
            {
                if (assigned[s]) continue;
                var c = new BaseCluster();
                assigned[s] = true;
                queue.Enqueue(s);
                while (queue.Count > 0)
                {
                    int i = queue.Dequeue();
                    c.nodes.Add(valid[i]);
                    Vector3 p = valid[i].transform.position;
                    for (int j = 0; j < valid.Count; j++)
                    {
                        if (assigned[j] || (valid[j].transform.position - p).sqrMagnitude > r2) continue;
                        assigned[j] = true;
                        queue.Enqueue(j);
                    }
                }
                FinishCluster(c);
                c.isRemote = oldRemote.TryGetValue(c.clusterId, out bool rem) ? rem : ComputeRemote(c, false);
                clusters.Add(c);
            }
            // Cluster terbesar lebih dulu (maxRaidedClusters).
            clusters.Sort((x, y) => y.nodes.Count.CompareTo(x.nodes.Count));
        }

        private static void FinishCluster(BaseCluster c)
        {
            Vector3 sum = Vector3.zero;
            string minId = null;
            for (int i = 0; i < c.nodes.Count; i++)
            {
                var n = c.nodes[i];
                sum += n.transform.position;
                if (minId == null || string.CompareOrdinal(n.StructureId, minId) < 0) minId = n.StructureId;
                var h = n.GetComponent<BaseHealth>();
                if (h == null) continue;
                if (IsTagged(n, "BaseDoor")) c.doors.Add(h);
                else if (IsTagged(n, "BaseWall")) c.walls.Add(h);
                else if (n.GetComponent<BaseTrap>() != null && n.GetComponent<CampfireManager>() == null) c.traps.Add(h);
                else c.inner.Add(h);
            }
            c.clusterId = minId ?? string.Empty;
            c.center = c.nodes.Count > 0 ? sum / c.nodes.Count : Vector3.zero;
            c.region = RegionStreaming.RegionOf(c.center);
        }

        private static bool IsTagged(StructureNode n, string tag)
        {
            if (n.CompareTag(tag)) return true;
            var cols = n.GetComponentsInChildren<Collider>(true);
            for (int i = 0; i < cols.Length; i++) if (cols[i].CompareTag(tag)) return true;
            return false;
        }

        private bool IsOutpost(BaseCluster c)
        {
            var b = Bal;
            return c.nodes.Count < (b != null ? b.minStructuresForRaid : 4);
        }

        /// <summary>Histeresis: masuk simulasi di atas remoteDistance, kembali nyata di bawah returnDistance.</summary>
        private bool ComputeRemote(BaseCluster c, bool current)
        {
            var b = Bal;
            var player = PlayerTargetable.Instance;
            if (b == null || player == null) return current;
            bool loaded = RegionStreaming.IsRegionLoaded(c.region);
            float d = Vector3.Distance(player.Position, c.center);
            if (!loaded || d > b.remoteDistance) return true;
            if (d < b.returnDistance) return false;
            return current;
        }

        private BaseCluster FindCluster(RaidPlan p)
        {
            for (int i = 0; i < clusters.Count; i++) if (clusters[i].clusterId == p.clusterId) return clusters[i];
            // structureId terkecil hancur: cari cluster yang pusatnya dekat pusat lama.
            var b = Bal;
            float r = b != null ? b.clusterRadius : 30f;
            for (int i = 0; i < clusters.Count; i++)
            {
                if ((clusters[i].center - p.center).sqrMagnitude > r * r) continue;
                p.clusterId = clusters[i].clusterId;
                return clusters[i];
            }
            return null;
        }

        // ---------------------------------------------------------------- Rumus serangan

        private void HandleNightfall(NightfallEvent e)
        {
            if (runEnded) return;
            var b = Bal;
            if (b == null) return;
            // Sisa rencana kemarin (seharusnya sudah selesai saat fajar) dibuang.
            plans.Clear();
            // 1.
            RebuildClusters();
            // 2.
            if (e.day < b.firstRaidDay) return;
            var seeds = RunSeedManager.Instance;
            if (seeds == null) return;
            ref var rng = ref seeds.GetStream(RandomStreamId.Enemy);
            // 3. Cluster terbesar sampai maxRaidedClusters. Pos sementara tidak diserang.
            int raided = 0;
            for (int i = 0; i < clusters.Count && raided < b.maxRaidedClusters; i++)
            {
                var c = clusters[i];
                if (IsOutpost(c)) continue;
                raided++;
                float chance = Mathf.Min(b.raidChanceMax, b.raidChanceBase + b.raidChancePerDay * e.day);
                if (rng.NextFloat01() >= chance) continue;
                // 4. Semua cluster mendapat rencana, termasuk yang saat ini nyata.
                plans.Add(CreatePlan(c, e.day, b, ref rng));
            }
        }

        private RaidPlan CreatePlan(BaseCluster c, int day, RemoteBaseBalance b, ref RandomStream rng)
        {
            bool bloodMoon = bloodMoonActive || (HordeManager.Instance != null && HordeManager.Instance.IsBloodMoonDay(day));
            float sizeFactor = Mathf.Clamp(c.nodes.Count / Mathf.Max(1f, b.referenceStructures), b.sizeFactorMin, b.sizeFactorMax);
            float power = Mathf.Min(b.maxPower, b.basePower + b.powerPerDay * (day - 1)) * sizeFactor * (bloodMoon ? b.bloodMoonMultiplier : 1f);

            float defense = 0f, wallMax = 0f;
            AddWallDefense(c.walls, b, ref defense, ref wallMax);
            AddWallDefense(c.doors, b, ref defense, ref wallMax);
            for (int i = 0; i < c.traps.Count; i++) if (c.traps[i] != null && c.traps[i].IsAlive) defense += b.scoreTrap;
            defense += b.scoreNpcGuard * CountGuards(c, b);

            float mitigation = Mathf.Min(b.maxMitigation, defense / (defense + Mathf.Max(0.0001f, b.mitigationK)));
            float net = power * (1f - mitigation) * rng.Range(1f - b.varianceFraction, 1f + b.varianceFraction);

            return new RaidPlan
            {
                clusterId = c.clusterId, day = day, netDamage = Mathf.Max(0f, net), appliedFraction = 0f,
                startHour = rng.Range(b.raidStartHourMin, b.raidStartHourMax), endHour = b.raidEndHour,
                center = c.center, region = c.region, wallMaxHpTotal = wallMax,
            };
        }

        private static void AddWallDefense(List<BaseHealth> list, RemoteBaseBalance b, ref float defense, ref float wallMax)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var h = list[i];
                if (h == null || !h.IsAlive) continue;
                var info = StructureIntegrityInfo.For(h.Data);
                float score = info != null && info.tier == StructureTier.Stone ? b.scoreWallStone : b.scoreWallWood;
                defense += score * h.HpRatio;
                wallMax += h.MaxHP;
            }
        }

        // ---------------------------------------------------------------- NPC penjaga

        private int CountGuards(BaseCluster c, RemoteBaseBalance b)
        {
            var roster = NPCRoster.Instance;
            if (roster == null) return 0;
            int n = 0;
            var members = roster.Members;
            for (int i = 0; i < members.Count; i++) if (IsGuard(members[i], c, b)) n++;
            return n;
        }

        private static bool IsGuard(NPCRecord r, BaseCluster c, RemoteBaseBalance b)
        {
            if (r == null || !r.isAlive || r.data == null || !NPCCompanion.IsBaseState(r.state)) return false;
            if (r.data.maxHP > 0f && r.hp / r.data.maxHP <= b.npcGuardMinHpRatio) return false;
            Vector3 pos = r.companion != null && r.companion.isActiveAndEnabled ? r.companion.transform.position : r.position;
            return (pos - c.center).sqrMagnitude <= b.clusterRadius * b.clusterRadius;
        }

        private void ResolveNpcs(RaidPlan p, BaseCluster c, RemoteBaseBalance b, ref RandomStream rng)
        {
            var roster = NPCRoster.Instance;
            if (roster == null || c == null) return;
            // 1.
            float pressure = p.wallMaxHpTotal > 0f ? p.netDamage / p.wallMaxHpTotal : 1f;
            bool isBreached = p.wallsLost > 0;
            // 2. Usulan disetujui 3: peluang NPC mati hanya saat dinding jebol.
            float injuryChance = Mathf.Min(b.npcInjuryChanceMax, pressure) * (isBreached ? 1f : 0.5f);
            var members = roster.Members;
            for (int i = members.Count - 1; i >= 0; i--)
            {
                var r = members[i];
                if (!IsGuard(r, c, b)) continue;
                if (rng.NextFloat01() >= injuryChance) continue;
                // 3.
                p.npcInjured++;
                string id = r.npcId;
                roster.ApplyRemoteDamage(id, b.npcInjuryDamage);
                if (!r.isAlive) { p.npcDead++; continue; }
                // 4. Penjaga bersenjata lebih selamat.
                if (!isBreached) continue;
                float death = b.npcDeathChanceOnBreach * (r.data.weapon != null ? b.npcGuardDeathMultiplier : 1f);
                if (rng.NextFloat01() < death)
                {
                    roster.KillNpc(id, DeathReason.KilledByEnemy);
                    p.npcDead++;
                }
            }
        }

        // ---------------------------------------------------------------- Alur satu malam

        private static float CurrentHour()
        {
            var dn = DayNightDirector.Instance;
            return dn != null ? dn.CurrentTimeOfDay : DayPhaseTracker.CurrentHour;
        }

        /// <summary>0 di startHour sampai 1 di raidEndHour, melewati tengah malam.</summary>
        private static float Fraction(RaidPlan p, float hour)
        {
            float span = Mathf.Repeat(p.endHour - p.startHour, 24f);
            if (span <= 0f) return 1f;
            float elapsed = Mathf.Repeat(hour - p.startHour, 24f);
            if (elapsed <= span) return elapsed / span;
            // Di luar rentang: pagi berarti sudah lewat raidEndHour, sore berarti belum mulai.
            return hour < 12f ? 1f : 0f;
        }

        private void HandleHour(HourChangedEvent e)
        {
            if (runEnded || plans.Count == 0) return;
            var b = Bal;
            if (b == null) return;
            EnsureClusters();
            for (int i = plans.Count - 1; i >= 0; i--)
            {
                var p = plans[i];
                if (p.isTakenOver) continue;
                var c = FindCluster(p);
                if (c == null) { plans.RemoveAt(i); continue; }   // Cluster hilang membuang rencananya
                if (!c.isRemote) continue;
                float f = Fraction(p, e.hour);
                // 1.
                if (f > 0f && !p.notified)
                {
                    p.notified = true;
                    EventChannel<BaseRaidStartedEvent>.Publish(new BaseRaidStartedEvent(p.clusterId, p.region));
                    // Usulan disetujui 6: pemain selalu tahu, walaupun tanpa radio.
                    if (NotificationManager.Instance != null)
                        NotificationManager.Instance.ShowFormat("notif.base_raid_started", NotificationKind.Danger, "region", RegionName(p.region));
                }
                // 2.
                ApplyProgress(p, c, f);
            }
        }

        private void ApplyProgress(RaidPlan p, BaseCluster c, float target)
        {
            target = Mathf.Clamp01(target);
            if (target <= p.appliedFraction) return;
            float amount = p.netDamage * (target - p.appliedFraction);
            p.appliedFraction = target;
            ApplyStructureDamage(c, p, amount);
        }

        private void ApplyStructureDamage(BaseCluster c, RaidPlan p, float amount)
        {
            var b = Bal;
            var seeds = RunSeedManager.Instance;
            if (b == null || c == null || seeds == null || b.hitChunk <= 0f) return;
            ref var rng = ref seeds.GetStream(RandomStreamId.Enemy);
            // 1. Pukulan sebesar hitChunk; sisa pecahan dibawa ke panggilan berikutnya.
            float total = amount + p.hitCarry;
            while (total >= b.hitChunk)
            {
                // 2. Pintu berbobot doorWeight, dinding 1. Struktur dalam baru setelah semua dinding dan pintu hancur.
                bool isWall;
                var target = PickWallTarget(c, b, ref rng, out isWall);
                if (target == null) target = PickAny(c.traps, c.inner, ref rng);
                if (target == null)
                {
                    // Semua habis: sisa dibuang.
                    total = 0f;
                    break;
                }
                // 3. Bisa dipanggil pada objek nonaktif (#59, #88). StructureDestroyedEvent dan runtuhan mengikuti #79.
                var info = new DamageInfo { amount = b.hitChunk, type = DamageType.Structural, bodyPart = BodyPart.Torso, byPlayer = false };
                target.TakeDamage(info);
                if (!target.IsAlive)
                {
                    p.structuresLost++;
                    if (isWall) p.wallsLost++;
                }
                total -= b.hitChunk;
            }
            p.hitCarry = total;
        }

        private static BaseHealth PickWallTarget(BaseCluster c, RemoteBaseBalance b, ref RandomStream rng, out bool isWall)
        {
            isWall = false;
            float sum = 0f;
            for (int i = 0; i < c.walls.Count; i++) if (c.walls[i] != null && c.walls[i].IsAlive) sum += 1f;
            for (int i = 0; i < c.doors.Count; i++) if (c.doors[i] != null && c.doors[i].IsAlive) sum += b.doorWeight;
            if (sum <= 0f) return null;
            float roll = rng.NextFloat01() * sum;
            for (int i = 0; i < c.doors.Count; i++)
            {
                var h = c.doors[i];
                if (h == null || !h.IsAlive) continue;
                roll -= b.doorWeight;
                if (roll <= 0f) { isWall = true; return h; }
            }
            BaseHealth last = null;
            for (int i = 0; i < c.walls.Count; i++)
            {
                var h = c.walls[i];
                if (h == null || !h.IsAlive) continue;
                last = h;
                roll -= 1f;
                if (roll <= 0f) break;
            }
            isWall = last != null;
            return last;
        }

        private BaseHealth PickAny(List<BaseHealth> a, List<BaseHealth> bList, ref RandomStream rng)
        {
            targetBuffer.Clear();
            for (int i = 0; i < a.Count; i++) if (a[i] != null && a[i].IsAlive) targetBuffer.Add(a[i]);
            for (int i = 0; i < bList.Count; i++) if (bList[i] != null && bList[i].IsAlive) targetBuffer.Add(bList[i]);
            if (targetBuffer.Count == 0) return null;
            var pick = targetBuffer[rng.Range(0, targetBuffer.Count)];
            targetBuffer.Clear();
            return pick;
        }

        private void HandleDaybreak(DaybreakEvent e)
        {
            if (runEnded) return;
            EnsureClusters();
            for (int i = plans.Count - 1; i >= 0; i--) ResolvePlan(plans[i]);
            plans.Clear();
        }

        /// <summary>HandleDaybreak langkah 1-5 untuk satu rencana.</summary>
        private void ResolvePlan(RaidPlan p)
        {
            var b = Bal;
            var seeds = RunSeedManager.Instance;
            if (b == null || seeds == null || p.isTakenOver) return;
            var c = FindCluster(p);
            if (c == null) return;
            // 1. Rencana yang belum sempat berjalan pada cluster nyata dibuang.
            if (c.isRemote) ApplyProgress(p, c, 1f);
            else if (p.appliedFraction <= 0f) return;
            ref var rng = ref seeds.GetStream(RandomStreamId.Enemy);
            // 2. Jebakan kehilangan trapWearFraction x maxHP.
            for (int i = 0; i < c.traps.Count; i++)
            {
                var t = c.traps[i];
                if (t == null || !t.IsAlive) continue;
                t.TakeDamage(new DamageInfo { amount = b.trapWearFraction * t.MaxHP, type = DamageType.Structural, bodyPart = BodyPart.Torso, byPlayer = false });
                p.trapsWorn++;
            }
            // 3.
            ResolveNpcs(p, c, b, ref rng);
            // 4.
            EventChannel<BaseRaidResolvedEvent>.Publish(new BaseRaidResolvedEvent(p.netDamage * p.appliedFraction, p.npcInjured));
            // 5. Pemain tidur: ringkasan menunggu bangun.
            if (isSleeping) pendingReports.Add(p);
            else ShowReport(p);
        }

        private static void ShowReport(RaidPlan p)
        {
            var nm = NotificationManager.Instance;
            if (nm == null) return;
            nm.ShowFormat("notif.base_raid_report", NotificationKind.Warning,
                          "lost", p.structuresLost.ToString(), "injured", p.npcInjured.ToString());
        }

        private static string RegionName(RegionType r)
        {
            var loc = LocalizationManager.Instance;
            string key = "region." + r.ToString().ToLowerInvariant() + ".name";
            return loc != null && loc.Has(key) ? loc.Get(key) : r.ToString();
        }

        // ---------------------------------------------------------------- Peralihan saat pemain kembali

        private IEnumerator DistanceLoop()
        {
            while (true)
            {
                var b = Bal;
                // Waktu berskala: simulasi ikut berhenti saat jeda.
                yield return new WaitForSeconds(b != null ? Mathf.Max(0.25f, b.distanceCheckSeconds) : 2f);
                if (b == null || runEnded || !DayPhaseTracker.IsNight) continue;
                EnsureClusters();
                CheckTransitions(b);
            }
        }

        private void CheckTransitions(RemoteBaseBalance b)
        {
            float hour = CurrentHour();
            for (int i = 0; i < clusters.Count; i++)
            {
                var c = clusters[i];
                bool remote = ComputeRemote(c, c.isRemote);
                if (remote == c.isRemote) continue;
                var p = FindPlan(c);
                if (!remote)
                {
                    // Simulasi ke nyata.
                    c.isRemote = false;
                    if (p == null || p.isTakenOver) continue;
                    // 1.
                    ApplyProgress(p, c, Fraction(p, hour));
                    // 2. startHour sudah lewat: pemain menemukan zombie sedang membobol dinding.
                    bool started = p.notified || Fraction(p, hour) > 0f;
                    if (started && p.appliedFraction < 1f)
                    {
                        p.isTakenOver = true;
                        float left = p.netDamage * (1f - p.appliedFraction);
                        int count = Mathf.Min(b.takeoverMaxZombies, Mathf.RoundToInt(left * b.takeoverZombiesPerPower));
                        if (count > 0 && SpawnManager.Instance != null)
                        {
                            var req = new WaveRequest
                            {
                                count = count, modifiers = WaveModifiers.One, reason = "RemoteTakeover", origin = c.center,
                            };
                            SpawnManager.Instance.RequestWave(req);
                        }
                    }
                    // 3. Belum mulai: dibatalkan, karena horde biasa sudah mengikuti pemain.
                    else if (!started) plans.Remove(p);
                }
                else
                {
                    // Nyata ke simulasi. 1.
                    c.isRemote = true;
                    if (SpawnManager.Instance != null) SpawnManager.Instance.ReturnRaidersNear(c.center, b.clusterRadius);
                    // 2. Sisa malam disimulasikan tanpa damage untuk bagian yang sudah lewat.
                    if (p == null) continue;
                    p.isTakenOver = false;
                    p.appliedFraction = Mathf.Max(p.appliedFraction, Fraction(p, hour));
                }
            }
        }

        private RaidPlan FindPlan(BaseCluster c)
        {
            for (int i = 0; i < plans.Count; i++) if (FindCluster(plans[i]) == c) return plans[i];
            return null;
        }

        // ---------------------------------------------------------------- Bencana saat ditinggal

        /// <summary>
        /// Dipanggil DisasterManager (#70) untuk region yang tidak dimuat. Hantaman langsung diterapkan, tidak disimpan.
        /// Usulan disetujui 5: base yang ditinggal tetap terkena bencana; besarnya diputuskan #70.
        /// </summary>
        public void QueueDisasterDamage(RegionType region, DamageType type, float damagePerHit, int hits, bool onlyWood)
        {
            if (runEnded || hits <= 0 || damagePerHit <= 0f) return;
            var seeds = RunSeedManager.Instance;
            if (seeds == null) return;
            EnsureClusters();
            ref var rng = ref seeds.GetStream(RandomStreamId.Disaster);
            var info = new DamageInfo { amount = damagePerHit, type = type, bodyPart = BodyPart.Torso, byPlayer = false };
            // 1. Semua cluster di region, termasuk pos sementara.
            for (int i = 0; i < clusters.Count; i++)
            {
                var c = clusters[i];
                if (c.region != region) continue;
                // 2. onlyWood: buang struktur survivesEarthquake (raft/BuildingIntegrity.md).
                targetBuffer.Clear();
                for (int n = 0; n < c.nodes.Count; n++)
                {
                    var node = c.nodes[n];
                    if (node == null) continue;
                    var h = node.GetComponent<BaseHealth>();
                    if (h == null || !h.IsAlive) continue;
                    if (onlyWood && StructureIntegrityInfo.SurvivesEarthquake(node.Data)) continue;
                    targetBuffer.Add(h);
                }
                // 3. hits sasaran acak lewat aliran Disaster.
                for (int k = 0; k < hits && targetBuffer.Count > 0; k++)
                {
                    var h = targetBuffer[rng.Range(0, targetBuffer.Count)];
                    if (h != null && h.IsAlive) h.TakeDamage(info);
                }
                targetBuffer.Clear();
            }
        }

        // ---------------------------------------------------------------- Event lain

        private void HandleStructureBuilt(StructureBuiltEvent e) => clustersDirty = true;
        private void HandleStructureDestroyed(StructureDestroyedEvent e) => clustersDirty = true;
        private void HandleBloodMoonStarted(BloodMoonStartedEvent e) => bloodMoonActive = true;
        private void HandleBloodMoonEnded(BloodMoonEndedEvent e) => bloodMoonActive = false;

        private void HandleSleep(SleepStateChangedEvent e)
        {
            isSleeping = e.isSleeping;
            if (isSleeping) return;
            // Ringkasan yang tertunda tampil saat bangun.
            for (int i = 0; i < pendingReports.Count; i++) ShowReport(pendingReports[i]);
            pendingReports.Clear();
        }

        private void HandleRunEnded(RunEndedEvent e)
        {
            // Permadeath atau menang: semua rencana dibatalkan.
            runEnded = true;
            plans.Clear();
            pendingReports.Clear();
        }

        // ---------------------------------------------------------------- Save (#80)

        [Serializable]
        private class RemoteBaseSaveData
        {
            public RaidPlan[] plans = Array.Empty<RaidPlan>();
            public RaidPlan[] pendingReports = Array.Empty<RaidPlan>();
        }

        public string SaveId => SaveIdConst;
        public int LoadOrder => 120;

        public string CaptureState()
        {
            return JsonUtility.ToJson(new RemoteBaseSaveData { plans = plans.ToArray(), pendingReports = pendingReports.ToArray() });
        }

        public void RestoreState(string json)
        {
            // Kerusakan yang sudah diterapkan ada di StructureRecord.currentHP, jadi tidak diulang.
            plans.Clear();
            pendingReports.Clear();
            if (string.IsNullOrEmpty(json)) return;
            var d = JsonUtility.FromJson<RemoteBaseSaveData>(json);
            if (d == null) return;
            if (d.plans != null) for (int i = 0; i < d.plans.Length; i++) if (d.plans[i] != null) plans.Add(d.plans[i]);
            if (d.pendingReports != null) for (int i = 0; i < d.pendingReports.Length; i++) if (d.pendingReports[i] != null) pendingReports.Add(d.pendingReports[i]);
        }

        public void OnRestoreFinished()
        {
            RebuildClusters();
            for (int i = 0; i < clusters.Count; i++) clusters[i].isRemote = ComputeRemote(clusters[i], true);
            float hour = CurrentHour();
            bool night = DayPhaseTracker.IsNight;
            for (int i = plans.Count - 1; i >= 0; i--)
            {
                var p = plans[i];
                var c = FindCluster(p);
                if (c == null) { plans.RemoveAt(i); continue; }
                if (!night)
                {
                    // Load saat siang dengan rencana belum selesai: HandleDaybreak sekali untuk rencana itu.
                    ResolvePlan(p);
                    plans.RemoveAt(i);
                    continue;
                }
                // Terapkan sisa yang tertinggal.
                if (c.isRemote && !p.isTakenOver) ApplyProgress(p, c, Fraction(p, hour));
            }
        }
    }
}
