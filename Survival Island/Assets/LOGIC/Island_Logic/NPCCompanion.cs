using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>State FSM NPC. Enum resmi GLOSSARY.md (NPCRoster.md, NPCCompanion.md, #76).</summary>
    public enum NPCState { Follow, Attack, Idle, StayAtBase, Work, TakeCover }

    /// <summary>
    /// AI satu NPC teman di pulau: FSM, pemilihan target, damage, dan perintah tombol E.
    /// Blueprint: Island_Logic/NPCCompanion.md dan NPCRoster.md bagian "State" (#76, melengkapi #29, #58, #78, #108, #110).
    /// GDD bagian 3: NPC punya skill khusus, dan pemain melepaskan diri sendiri saat diterkam (NPC tidak menolong).
    /// GDD bagian 4: base sebagai tempat NPC berlindung. Claude.md Pilar 3: semua AI memakai NavMeshAgent.
    /// Perilaku tempur, perintah, dan angka adalah usulan disetujui developer.
    ///
    /// Objek dibuat sekali per run oleh NPCRoster.SpawnRescued, tanpa ObjectPooler. HP dibaca dan ditulis langsung di
    /// NPCRecord (usulan disetujui), jadi damage jarak jauh (#90) tetap benar saat objek dimatikan.
    ///
    /// Catatan implementasi (sistem pendukung, juga di blueprint):
    /// - Musuh tidak memasang ITargetable, jadi target NPC adalah EnemyHealth, dengan titik bidik dari collider musuh.
    /// - Pendaftaran ThreatSystem memakai RegisterCandidate/UnregisterCandidate (API ThreatSystem untuk ITargetable).
    /// - FireSystem.IsNearFire (#97) belum dipakai: API belum memberi titik api untuk menghindar (TODO di Think).
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCCompanion : MonoBehaviour, IInteractable, ITargetable, IDamageable
    {
        private const int EnemyBufferSize = 16;
        private const int CoverSampleCount = 8;

        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private NPCGunUser gunUser;
        [Tooltip("Klip ucapan NPCBarks (#100).")]
        [SerializeField] private AudioSource voice;
        [Tooltip("Titik dada, untuk garis pandang musuh dan tembakan.")]
        [SerializeField] private Transform aimPoint;
        [Tooltip("Mata NPC, asal pemeriksaan garis pandang.")]
        [SerializeField] private Transform eyePoint;
        [SerializeField] private Animator animator;
        [SerializeField] private InteractHighlight highlight;

        private NPCData data;
        private NPCRecord record;
        private EnemyHealth currentTarget;
        private Collider currentTargetCollider;
        private Transform workPoint;
        private Vector3 homePoint;
        private bool hasHome;
        private bool isInsideBase;
        private bool runEnded;
        private bool active;
        private bool isDying;
        private int defaultAreaMask = NavMesh.AllAreas;
        private int doorLinkArea = -1;
        private Coroutine thinkRoutine;
        private readonly Collider[] enemyBuffer = new Collider[EnemyBufferSize];
        private readonly Collider[] doorBuffer = new Collider[8];

        public NPCState State { get; private set; } = NPCState.Follow;
        public NPCCommand Command { get; private set; } = NPCCommand.Follow;
        public NPCData Data => data;
        public NPCRecord Record => record;
        public AudioSource Voice => voice;
        public string NpcId => record != null ? record.npcId : string.Empty;

        private static NpcBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.npc : null;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (agent == null) agent = GetComponent<NavMeshAgent>();
            if (gunUser == null) gunUser = GetComponentInChildren<NPCGunUser>();
            if (voice == null) voice = GetComponentInChildren<AudioSource>();
            if (animator == null) animator = GetComponentInChildren<Animator>();
            if (highlight == null) highlight = GetComponentInChildren<InteractHighlight>();
            // BaseNavigation.md: NPC menyeberang link sendiri, supaya pintu tertutup bisa dibuka lewat RequestNpcPass.
            agent.autoTraverseOffMeshLink = false;
            defaultAreaMask = agent.areaMask;
            doorLinkArea = NavMesh.GetAreaFromName("DoorLink");
            var b = Bal;
            if (b != null) agent.avoidancePriority = b.avoidancePriority;   // #87: NPC memberi jalan ke pemain
        }

        /// <summary>Dipanggil NPCRoster setelah Instantiate. Langkah 1-3 di NPCCompanion.md "Siklus hidup".</summary>
        public void Initialize(NPCData npcData, NPCRecord npcRecord)
        {
            data = npcData;
            record = npcRecord;
            // 1.
            if (record.hp <= 0f && record.isAlive) record.hp = data != null ? data.maxHP : 100f;
            State = record.state;
            Command = record.command;
            // 2. NPC_Pistol (#58). null = tidak menembak.
            if (gunUser != null) gunUser.SetWeapon(data != null ? data.weapon : null);
            // 3. voice dipakai NPCBarks lewat properti Voice.
            if (isActiveAndEnabled) Activate();
        }

        private void OnEnable()
        {
            Activate();
        }

        private void Activate()
        {
            // 1.
            if (active || record == null || !record.isAlive) return;
            active = true;
            // 2. Pool dipakai ulang tidak berlaku (tanpa pool), tetapi objek bisa dinyalakan kembali saat region kembali.
            currentTarget = null;
            currentTargetCollider = null;
            agent.enabled = true;
            WarpTo(record.position);
            transform.rotation = Quaternion.Euler(0f, record.yaw, 0f);
            // 3. ThreatSystem (#108): NPC menjadi kandidat target musuh.
            if (ThreatSystem.Instance != null) ThreatSystem.Instance.RegisterCandidate(this);
            // 4.
            EventChannel<PlayerPinnedEvent>.Raised += HandlePinned;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            if (EventChannel<PlayerPinnedEvent>.HasLast) HandlePinned(EventChannel<PlayerPinnedEvent>.Last);
            // 5.
            if (!runEnded) thinkRoutine = StartCoroutine(ThinkLoop());
        }

        private void OnDisable()
        {
            if (!active) return;
            active = false;
            // 1.
            if (thinkRoutine != null) StopCoroutine(thinkRoutine);
            thinkRoutine = null;
            // 2. Region dilepas: NPC tidak lagi diincar.
            if (ThreatSystem.Instance != null) ThreatSystem.Instance.UnregisterCandidate(this);
            // 3.
            EventChannel<PlayerPinnedEvent>.Raised -= HandlePinned;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            // 4.
            WriteRecord();
        }

        private void OnDestroy()
        {
            if (record != null && record.companion == this) record.companion = null;
        }

        /// <summary>Menulis posisi, arah, state, dan perintah ke NPCRecord. Juga dipanggil NPCRoster sebelum save.</summary>
        public void WriteRecord()
        {
            if (record == null) return;
            record.position = transform.position;
            record.yaw = transform.eulerAngles.y;
            record.state = State;
            record.command = Command;
        }

        /// <summary>Pindah ke titik NavMesh terdekat. Kalau di luar NavMesh, ulangi lewat SamplePosition.</summary>
        public void WarpTo(Vector3 position)
        {
            if (!agent.enabled) return;
            agent.Warp(position);
            if (!agent.isOnNavMesh && NavMesh.SamplePosition(position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
                agent.Warp(hit.position);
        }

        private IEnumerator ThinkLoop()
        {
            var b = Bal;
            float interval = b != null ? Mathf.Max(0.05f, b.thinkSeconds) : 0.25f;
            // Usulan disetujui: jeda acak awal supaya beberapa NPC tidak berpikir pada frame yang sama.
            yield return new WaitForSeconds(Random.Range(0f, interval));
            var wait = new WaitForSeconds(interval);
            while (record != null && record.isAlive && !runEnded)
            {
                Think();
                yield return wait;   // Waktu berskala: berhenti saat game dijeda
            }
            thinkRoutine = null;
        }

        private void Update()
        {
            if (record == null || !record.isAlive || runEnded) return;
            TraverseLink();
            // Regen hanya di base (usulan disetujui 7). Time.deltaTime: berhenti saat jeda, tanpa percepatan saat tidur.
            if (isInsideBase && IsBaseState(State))
            {
                var b = Bal;
                float max = data != null ? data.maxHP : 100f;
                if (b != null && record.hp < max) record.hp = Mathf.Min(record.hp + b.baseRegenPerSec * Time.deltaTime, max);
            }
        }

        public static bool IsBaseState(NPCState s) => s == NPCState.StayAtBase || s == NPCState.Work || s == NPCState.TakeCover;

        // ---------------------------------------------------------------- Think

        private void Think()
        {
            var b = Bal;
            if (b == null || !agent.enabled || !agent.isOnNavMesh) return;
            var nav = BaseNavigation.Instance;
            isInsideBase = nav != null && nav.IsInsideBase(transform.position);

            // 1. TODO(FireSystem): if (FireSystem.Instance.IsNearFire(pos, b.fireAvoidMargin)) cari titik di luar api,
            //    SetDestination ke sana, lalu return (#97). NPC tidak memasang Flammable.

            // 2.
            SelectTarget(b);

            // Base hancur: NPC base kembali Follow.
            if (IsBaseState(State))
            {
                hasHome = nav != null && nav.TryGetBaseAnchor(out homePoint);
                if (!hasHome)
                {
                    Command = NPCCommand.Follow;
                    if (record != null) record.command = Command;
                    SetState(NPCState.Follow);
                }
            }

            // 3. Baris state di NPCRoster.md.
            switch (State)
            {
                case NPCState.Follow: TickFollow(b); break;
                case NPCState.Attack: TickAttack(b); break;
                case NPCState.Idle: TickIdle(); break;
                case NPCState.StayAtBase: TickStayAtBase(b); break;
                case NPCState.Work: TickWork(b); break;
                case NPCState.TakeCover: TickTakeCover(); break;
            }
        }

        private void TickFollow(NpcBalance b)
        {
            if (currentTarget != null && CanShoot)
            {
                SetState(NPCState.Attack);
                TickAttack(b);
                return;
            }
            var player = PlayerTargetable.Instance;
            if (player == null) return;
            Vector3 p = player.Position;
            float dist = Vector3.Distance(transform.position, p);
            var roster = NPCRoster.Instance;
            bool indoor = roster != null && roster.IsPlayerInsideBase;
            float follow = indoor ? b.followIndoorDistance : b.followDistance;

            // RegionStreaming.md: Follow yang tertinggal jauh dan di luar pandangan pindah ke belakang pemain.
            if (dist > b.teleportDistance && !IsVisibleToPlayer())
            {
                Vector3 behind = p - player.transform.forward * Mathf.Max(1f, follow * 0.5f);
                WarpTo(behind);
                return;
            }

            agent.isStopped = false;
            agent.stoppingDistance = follow;
            if (dist > follow) agent.SetDestination(p);
            else if (indoor) KeepAwayFromDoor(b);
        }

        private void TickAttack(NpcBalance b)
        {
            var player = PlayerTargetable.Instance;
            bool leashBroken = player != null && Vector3.Distance(transform.position, player.Position) > b.attackLeashDistance;
            if (currentTarget == null || !CanShoot || leashBroken)
            {
                SetState(StateFromCommand());
                return;
            }
            // Berhenti berjalan selama garis pandang ada.
            agent.isStopped = true;
            ShootAtTarget();
        }

        private void TickIdle()
        {
            // Diam di tempat dari perintah Wait. Tetap menembak dari tempat.
            agent.isStopped = true;
            if (currentTarget != null) ShootAtTarget();
        }

        private void TickStayAtBase(NpcBalance b)
        {
            MoveTo(homePoint, 0.5f, true);
            if (currentTarget != null) ShootAtTarget();
            if (DayPhaseTracker.IsNight)
            {
                SetState(NPCState.TakeCover);
                return;
            }
            // Siang dan ada tempat kerja ke Work. Medis tidak butuh tempat kerja.
            if (data != null && data.skill != NPCSkill.None && (data.skill == NPCSkill.Medic || FindWorkPoint(b)))
                SetState(NPCState.Work);
        }

        private void TickWork(NpcBalance b)
        {
            if (DayPhaseTracker.IsNight || currentTarget != null)
            {
                SetState(NPCState.TakeCover);
                return;
            }
            if (data == null || data.skill == NPCSkill.Medic)
            {
                // Medis tetap di homePoint, karena skillnya hanya bergantung pada hidup.
                MoveTo(homePoint, 0.5f, true);
                return;
            }
            // Tempat kerja hancur ke StayAtBase (juga lewat StructureDestroyedEvent dari NPCRoster).
            if (workPoint == null || !workPoint.gameObject.activeInHierarchy)
            {
                workPoint = null;
                SetState(NPCState.StayAtBase);
                return;
            }
            MoveTo(workPoint.position, 1.5f, true);
        }

        private void TickTakeCover()
        {
            // Tidak keluar pintu: Area DoorLink dimatikan selama TakeCover, jadi pintu tidak terbuka saat malam.
            Vector3 cover = PickCoverPoint();
            MoveTo(cover, 0.5f, false);
            if (currentTarget != null) ShootAtTarget();
        }

        private void MoveTo(Vector3 target, float stopping, bool allowDoorLink)
        {
            agent.areaMask = allowDoorLink || doorLinkArea < 0 ? defaultAreaMask : defaultAreaMask & ~(1 << doorLinkArea);
            agent.isStopped = false;
            agent.stoppingDistance = stopping;
            if ((agent.destination - target).sqrMagnitude > 0.25f) agent.SetDestination(target);
        }

        private NPCState StateFromCommand()
        {
            switch (Command)
            {
                case NPCCommand.Wait: return NPCState.Idle;
                case NPCCommand.ReturnToBase: return DayPhaseTracker.IsNight ? NPCState.TakeCover : NPCState.StayAtBase;
                default: return NPCState.Follow;
            }
        }

        private void SetState(NPCState next)
        {
            if (next == State) return;
            State = next;
            if (record != null) record.state = next;
            if (next != NPCState.TakeCover) agent.areaMask = defaultAreaMask;
            // 4. ThreatWeight ikut properti. Event dan pembaruan skill.
            EventChannel<NPCStateChangedEvent>.Publish(new NPCStateChangedEvent(NpcId, next));
            if (NPCRoster.Instance != null) NPCRoster.Instance.RefreshSkills();
        }

        // ---------------------------------------------------------------- Target

        private bool CanShoot => gunUser != null && gunUser.HasWeapon;

        private void SelectTarget(NpcBalance b)
        {
            Vector3 eye = eyePoint != null ? eyePoint.position : transform.position + Vector3.up * 1.6f;
            var pinned = PlayerPinnedState.Instance;
            IDamageable pinAttacker = pinned != null && pinned.IsPinned && pinned.Attacker != null ? pinned.Attacker.DamageTarget : null;

            // Target lama dipertahankan kalau masih sah, supaya NPC tidak berpindah-pindah.
            if (currentTarget != null && IsValidTarget(currentTarget, currentTargetCollider, eye, b.detectRadius, pinAttacker)) return;
            currentTarget = null;
            currentTargetCollider = null;

            // 1. Layer Enemy, buffer milik NPC (tanpa alokasi).
            int n = PhysicsQuery.OverlapSphere(transform.position, b.detectRadius, PhysicsMasks.EnemyBody, enemyBuffer);
            float best = float.MaxValue;
            for (int i = 0; i < n; i++)
            {
                var c = enemyBuffer[i];
                enemyBuffer[i] = null;
                if (c == null) continue;
                // 2.
                var h = c.GetComponentInParent<EnemyHealth>();
                if (h == null || !IsValidTarget(h, c, eye, b.detectRadius, pinAttacker)) continue;
                // 5. Terdekat.
                float d = (c.bounds.center - transform.position).sqrMagnitude;
                if (d < best) { best = d; currentTarget = h; currentTargetCollider = c; }
            }
        }

        private bool IsValidTarget(EnemyHealth h, Collider c, Vector3 eye, float radius, IDamageable pinAttacker)
        {
            if (h == null || c == null || !h.IsAlive || !h.isActiveAndEnabled) return false;
            // Zombie yang Dormant atau Submerged bukan ancaman aktif (#63).
            var ai = h.GetComponent<EnemyAI>();
            if (ai != null && !ai.IsActiveThreat) return false;
            // 3. Keputusan developer (#110): NPC tidak menolong dan tidak menyerang penyerang selama pemain tertahan.
            if (pinAttacker != null && ReferenceEquals(pinAttacker, h)) return false;
            Vector3 aim = c.bounds.center;
            if ((aim - transform.position).sqrMagnitude > radius * radius) return false;
            // 4. Garis pandang lewat AIVision. Dinding menghalangi tembakan.
            Vector3 dir = aim - eye;
            float dist = dir.magnitude;
            if (dist > 0.01f && PhysicsQuery.Raycast(new Ray(eye, dir / dist), out _, dist, PhysicsMasks.AIVision)) return false;
            return true;
        }

        private void ShootAtTarget()
        {
            if (currentTarget == null || currentTargetCollider == null || !CanShoot) return;
            Vector3 aim = currentTargetCollider.bounds.center;
            Vector3 flat = aim - transform.position;
            flat.y = 0f;
            if (flat.sqrMagnitude > 0.0001f) transform.rotation = Quaternion.LookRotation(flat);
            gunUser.TryShootAt(aim);
        }

        private void HandlePinned(PlayerPinnedEvent e)
        {
            if (!e.isPinned || currentTarget == null) return;
            var pinned = PlayerPinnedState.Instance;
            var attacker = pinned != null && pinned.Attacker != null ? pinned.Attacker.DamageTarget : null;
            if (attacker == null || !ReferenceEquals(attacker, currentTarget)) return;
            // Lepaskan saat itu juga. NPC tidak menambah EscapeProgress (keputusan developer).
            currentTarget = null;
            currentTargetCollider = null;
            if (State == NPCState.Attack) SetState(StateFromCommand());
        }

        private void HandleRunEnded(RunEndedEvent e)
        {
            // Run berakhir: berhenti total, tanpa NPCDiedEvent.
            runEnded = true;
            if (thinkRoutine != null) StopCoroutine(thinkRoutine);
            thinkRoutine = null;
            if (agent.enabled && agent.isOnNavMesh) agent.isStopped = true;
        }

        // ---------------------------------------------------------------- Tempat kerja, berlindung, pintu

        private bool FindWorkPoint(NpcBalance b)
        {
            if (workPoint != null && workPoint.gameObject.activeInHierarchy) return true;
            workPoint = null;
            if (data == null) return false;
            float best = b.workSearchRadius * b.workSearchRadius;
            Vector3 from = hasHome ? homePoint : transform.position;
            // Jarang: hanya saat StayAtBase mencari tempat kerja. Dua NPC boleh memakai tempat yang sama.
            if (data.skill == NPCSkill.Crafter)
            {
                var all = FindObjectsByType<CraftingStation>(FindObjectsSortMode.None);
                for (int i = 0; i < all.Length; i++) Consider(all[i] != null ? all[i].transform : null, from, ref best);
            }
            else if (data.skill == NPCSkill.Cook)
            {
                var all = FindObjectsByType<CampfireManager>(FindObjectsSortMode.None);
                for (int i = 0; i < all.Length; i++) Consider(all[i] != null ? all[i].transform : null, from, ref best);
            }
            return workPoint != null;
        }

        private void Consider(Transform t, Vector3 from, ref float best)
        {
            if (t == null || !t.gameObject.activeInHierarchy) return;
            float d = (t.position - from).sqrMagnitude;
            if (d > best) return;
            best = d;
            workPoint = t;
        }

        /// <summary>Titik di dalam base terjauh dari musuh. Tanpa alokasi: sampel cincin tetap di sekitar homePoint.</summary>
        private Vector3 PickCoverPoint()
        {
            var nav = BaseNavigation.Instance;
            if (!hasHome || nav == null) return transform.position;
            Vector3 threat = currentTargetCollider != null ? currentTargetCollider.bounds.center : transform.position;
            bool hasThreat = currentTargetCollider != null;
            Vector3 bestPoint = homePoint;
            float bestScore = hasThreat ? (homePoint - threat).sqrMagnitude : 0f;
            if (!hasThreat) return homePoint;
            for (int i = 0; i < CoverSampleCount; i++)
            {
                float ang = i * (360f / CoverSampleCount) * Mathf.Deg2Rad;
                Vector3 p = homePoint + new Vector3(Mathf.Cos(ang), 0f, Mathf.Sin(ang)) * 2.5f;
                if (!NavMesh.SamplePosition(p, out NavMeshHit hit, 1f, defaultAreaMask)) continue;
                if (!nav.IsInsideBase(hit.position)) continue;
                float score = (hit.position - threat).sqrMagnitude;
                if (score > bestScore) { bestScore = score; bestPoint = hit.position; }
            }
            return bestPoint;
        }

        /// <summary>NPCRoster.md "Di dalam base": NPC Follow tidak berdiri dekat pintu, supaya tidak menghalangi pemain.</summary>
        private void KeepAwayFromDoor(NpcBalance b)
        {
            int n = PhysicsQuery.OverlapSphere(transform.position, b.doorKeepAwayRadius, PhysicsMasks.BuildSurface, doorBuffer);
            for (int i = 0; i < n; i++)
            {
                var c = doorBuffer[i];
                doorBuffer[i] = null;
                if (c == null || !c.CompareTag("BaseDoor")) continue;
                Vector3 away = transform.position - c.bounds.center;
                away.y = 0f;
                if (away.sqrMagnitude < 0.0001f) away = -transform.forward;
                agent.stoppingDistance = 0.1f;
                agent.SetDestination(transform.position + away.normalized * b.doorKeepAwayRadius);
                for (int j = i + 1; j < n; j++) doorBuffer[j] = null;
                return;
            }
        }

        /// <summary>BaseNavigation.md: menyeberang NavMeshLink sendiri. Pintu tertutup dibuka lewat RequestNpcPass.</summary>
        private void TraverseLink()
        {
            if (!agent.enabled || !agent.isOnOffMeshLink) return;
            var link = agent.currentOffMeshLinkData;
            var owner = link.owner as Component;
            var door = owner != null ? owner.GetComponentInParent<BaseDoor>() : null;
            if (door != null && !door.RequestNpcPass(this)) return;   // Tunggu daun terbuka
            Vector3 end = link.endPos + Vector3.up * agent.baseOffset;
            transform.position = Vector3.MoveTowards(transform.position, end, agent.speed * Time.deltaTime);
            if ((transform.position - end).sqrMagnitude > 0.01f) return;
            agent.CompleteOffMeshLink();
            if (door != null) door.NotifyNpcPassed();
        }

        private bool IsVisibleToPlayer()
        {
            var cam = Camera.main;
            if (cam == null) return false;
            Vector3 v = cam.WorldToViewportPoint(transform.position + Vector3.up);
            return v.z > 0f && v.x >= 0f && v.x <= 1f && v.y >= 0f && v.y <= 1f;
        }

        // ---------------------------------------------------------------- Dari NPCRoster

        /// <summary>NPCRoster.md: NightfallEvent hanya memengaruhi NPC dengan Command ReturnToBase.</summary>
        public void HandleNightfall()
        {
            if (Command != NPCCommand.ReturnToBase || !IsAliveActive) return;
            if (State == NPCState.StayAtBase || State == NPCState.Work) SetState(NPCState.TakeCover);
        }

        /// <summary>Fajar: kembali StayAtBase, lalu Think memilih Work kalau ada tempat kerja.</summary>
        public void HandleDaybreak()
        {
            if (Command != NPCCommand.ReturnToBase || !IsAliveActive) return;
            if (State == NPCState.TakeCover) SetState(NPCState.StayAtBase);
        }

        /// <summary>Tempat kerja hancur: workPoint kosong, StayAtBase, dan skill Crafter atau Cook berhenti.</summary>
        public void HandleStructureDestroyed()
        {
            if (!IsAliveActive || State != NPCState.Work || data == null || data.skill == NPCSkill.Medic) return;
            if (workPoint != null && workPoint.gameObject.activeInHierarchy) return;
            workPoint = null;
            SetState(NPCState.StayAtBase);
        }

        private bool IsAliveActive => record != null && record.isAlive && active;

        // ---------------------------------------------------------------- Perintah

        /// <summary>NPCRoster.md "Perintah pemain". false kalau ReturnToBase ditolak karena belum ada base.</summary>
        public bool IssueCommand(NPCCommand c)
        {
            if (record == null || !record.isAlive) return false;
            if (c == NPCCommand.ReturnToBase)
            {
                var nav = BaseNavigation.Instance;
                if (nav == null || !nav.TryGetBaseAnchor(out homePoint))
                {
                    if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.npc_no_base", NotificationKind.Warning);
                    return false;
                }
                hasHome = true;
            }
            Command = c;
            record.command = c;
            SetState(StateFromCommand());
            return true;
        }

        private NPCCommand NextCommand => Command == NPCCommand.Follow ? NPCCommand.Wait
                                        : Command == NPCCommand.Wait ? NPCCommand.ReturnToBase : NPCCommand.Follow;

        // ---------------------------------------------------------------- IInteractable

        public bool IsAvailable => IsAlive && isActiveAndEnabled;
        public float HoldSeconds => 0f;

        /// <summary>Usulan disetujui: prompt menampilkan perintah berikutnya (Ikuti, Tunggu, Base). Teks dari #104.</summary>
        public string GetPrompt()
        {
            string key = NextCommand == NPCCommand.Follow ? "prompt.npc_follow"
                       : NextCommand == NPCCommand.Wait ? "prompt.npc_wait" : "prompt.npc_base";
            var loc = LocalizationManager.Instance;
            return loc != null ? loc.Get(key) : key;
        }

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            if (!IsAvailable) return InteractBlockReason.Locked;
            // Input terkunci saat diterkam (#110).
            var pinned = PlayerPinnedState.Instance;
            if (pinned != null && pinned.IsPinned) return InteractBlockReason.Locked;
            // Usulan disetujui: perintah ditolak saat bertarung jarak dekat.
            var b = Bal;
            if (State == NPCState.Attack && currentTargetCollider != null && b != null &&
                (currentTargetCollider.bounds.center - transform.position).sqrMagnitude <= b.commandBlockEnemyRadius * b.commandBlockEnemyRadius)
                return InteractBlockReason.Locked;
            return InteractBlockReason.None;
        }

        public void OnHoverEnter()
        {
            if (highlight != null) highlight.SetState(HighlightState.Available);
        }

        public void OnHoverExit()
        {
            if (highlight != null) highlight.SetState(HighlightState.Off);
        }

        public bool Interact(in InteractContext ctx)
        {
            if (CanInteract(ctx) != InteractBlockReason.None) return false;
            return IssueCommand(NextCommand);
        }

        // ---------------------------------------------------------------- ITargetable

        public int TargetId => GetInstanceID();
        public TargetKind Kind => TargetKind.NPC;
        public Vector3 Position => transform.position;
        public Vector3 AimPoint => aimPoint != null ? aimPoint.position : transform.position + Vector3.up * 1.2f;

        public float ThreatWeight
        {
            get
            {
                var b = Bal;
                if (b == null) return State == NPCState.TakeCover ? 0.3f : 0.6f;
                return State == NPCState.TakeCover ? b.coverThreatWeight : b.followThreatWeight;
            }
        }

        public bool IsTargetable => record != null && record.isAlive && isActiveAndEnabled;
        public IDamageable Damageable => this;

        // ---------------------------------------------------------------- IDamageable

        public bool IsAlive => record != null && record.isAlive;

        public void TakeDamage(in DamageInfo info)
        {
            // 1.
            if (!IsAlive || info.amount <= 0f) return;
            // 2. Friendly fire (DamagePipeline.md).
            float amount = info.amount;
            switch (info.type)
            {
                case DamageType.Bullet:
                    // Peluru pemain atau NPC tertahan kapsul NPC. Musuh tidak menembak, jadi semua peluru diabaikan.
                    amount = 0f;
                    break;
                case DamageType.Melee:
                    if (info.byPlayer) amount = 0f;
                    break;
                case DamageType.Explosion:
                case DamageType.Fire:
                    if (info.byPlayer)
                    {
                        var bal = GameManager.CurrentBalance;
                        amount *= bal != null ? bal.damage.playerExplosionSelfMultiplier : 0.5f;
                    }
                    break;
            }
            // 3.
            if (amount <= 0f) return;
            // 4. Keputusan developer: NPC tidak bisa terinfeksi (#117 tidak dipakai). DamageInfo.infection diabaikan.
            record.hp -= amount;
            // 5.
            if (record.hp <= 0f) Die(DeathReasonMap.From(info.type));
        }

        /// <summary>Dipanggil NPCRoster.KillNpc saat objek aktif.</summary>
        internal void DieFromRoster(DeathReason reason) => Die(reason);

        /// <summary>NPCRoster.md "Target musuh dan NPC mati" langkah 1-5.</summary>
        private void Die(DeathReason reason)
        {
            if (isDying || record == null) return;
            isDying = true;
            // 1.
            record.isAlive = false;
            record.hp = 0f;
            WriteRecord();
            // 2. Agen dan AI mati. Tulang ragdoll pindah ke layer Ragdoll.
            if (thinkRoutine != null) StopCoroutine(thinkRoutine);
            thinkRoutine = null;
            if (ThreatSystem.Instance != null) ThreatSystem.Instance.UnregisterCandidate(this);
            if (agent.enabled) agent.enabled = false;
            if (animator != null) animator.enabled = false;
            EnableRagdoll();
            if (highlight != null) highlight.SetState(HighlightState.Off);
            if (voice != null) voice.Stop();
            // 3.
            EventChannel<NPCDiedEvent>.Publish(new NPCDiedEvent(record.npcId, reason));
            // 4. Skill hilang permanen sampai run berakhir.
            if (NPCRoster.Instance != null) NPCRoster.Instance.RefreshSkills();
            // 5. Mayat dihapus setelah corpseSeconds.
            StartCoroutine(CorpseRoutine());
        }

        private void EnableRagdoll()
        {
            int ragdoll = LayerMask.NameToLayer("Ragdoll");
            var bodies = GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < bodies.Length; i++)
            {
                bodies[i].isKinematic = false;
                if (ragdoll >= 0) bodies[i].gameObject.layer = ragdoll;
            }
        }

        private IEnumerator CorpseRoutine()
        {
            var b = Bal;
            yield return new WaitForSeconds(b != null ? b.corpseSeconds : 60f);
            Destroy(gameObject);
        }
    }
}
