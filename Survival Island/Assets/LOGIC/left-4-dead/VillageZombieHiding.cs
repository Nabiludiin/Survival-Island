using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>
    /// Pengatur state sembunyi Zombie Desa: SeekHiding, Submerged, dan Emerging. Dipasang di prefab zombie_desa.
    /// Blueprint: left-4-dead/SunlightExposure.md bagian "Zombie Desa" (#63), EnemyAI.SetHidingState (EnemyAI.md).
    /// GDD bagian 3: zombie tidak berkeliaran di luar ruangan saat siang. GDD bagian 5, Region 4: Zombie Desa berendam
    /// di air atau terkubur di lumpur, lalu naik secara massal saat malam dan sangat cepat.
    /// Usulan disetujui developer (SunlightExposure.md bagian 5): mulai sembunyi jam sunlight.seekHidingHour (05:00),
    /// yang terlambat terbakar seperti zombie lain, dan yang bersembunyi tidak terlihat serta tidak bisa diserang.
    ///
    /// Selama SeekHiding, Submerged, dan Emerging, EnemyAI.CrowdTick tidak berjalan, jadi kelas ini sendiri yang
    /// menggerakkan agen ke titik masuk dan memeriksa kedatangan.
    /// Siang/malam dibaca dari HourChangedEvent (DayPhaseTracker), sama seperti SunlightExposure dan SpawnManager.
    /// </summary>
    [RequireComponent(typeof(EnemyAI))]
    public class VillageZombieHiding : MonoBehaviour, IPoolable
    {
        // Jeda pemeriksaan kedatangan saat SeekHiding. Nilai teknis (bukan penyetelan gameplay).
        private const float SeekPollSeconds = 0.25f;
        private const float ArriveSlack = 1f;

        private enum Mode : byte { None, Seeking, Submerged, Emerging }

        [Tooltip("Kapsul gerak di layer Enemy. Kosong = Collider di objek ini.")]
        [SerializeField] private Collider bodyCapsule;
        [Tooltip("Collider hitbox di layer EnemyHitbox. Kosong = semua Collider anak non-trigger selain kapsul.")]
        [SerializeField] private Collider[] hitboxes = new Collider[0];
        [Tooltip("Renderer yang dimatikan saat Submerged. Kosong = semua Renderer anak.")]
        [SerializeField] private Renderer[] renderers = new Renderer[0];
        [Tooltip("Model yang turun dan naik sejauh buryDepth. Harus anak, bukan objek akar (agen).")]
        [SerializeField] private Transform model;
        [SerializeField] private ParticleSystem emergeEffect;   // Cipratan air atau lumpur saat naik
        [SerializeField] private AudioSource emergeAudio;       // Suara naik

        private EnemyAI ai;
        private NavMeshAgent agent;
        private SunlightExposure sun;
        private EnemyInstance instance;
        private EnemyAttack attack;
        private AISensory sensory;
        private VillageBehavior village;
        private Animator animator;
        private bool[] hitboxDefault;
        private bool[] rendererDefault;
        private bool capsuleDefault;
        private Vector3 modelBaseLocal;

        private Mode mode;
        private HidingSpot spot;
        private bool wasNight;
        private bool externallyPlaced;   // Diisi SpawnSubmerged/SpawnEmerging, supaya pemeriksaan awal dilewati
        private Coroutine routine;

        public HidingSpot Spot => spot;
        public bool IsSubmerged => mode == Mode.Submerged;

        private static SunlightBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.sunlight : null;

        private void Awake()
        {
            ai = GetComponent<EnemyAI>();
            agent = GetComponent<NavMeshAgent>();
            sun = GetComponent<SunlightExposure>();
            instance = GetComponent<EnemyInstance>();
            attack = GetComponent<EnemyAttack>();
            sensory = GetComponent<AISensory>();
            village = GetComponent<VillageBehavior>();
            animator = GetComponentInChildren<Animator>();
            if (bodyCapsule == null) bodyCapsule = GetComponent<Collider>();
            if (hitboxes == null || hitboxes.Length == 0)
            {
                var all = GetComponentsInChildren<Collider>(true);
                var list = new System.Collections.Generic.List<Collider>(all.Length);
                for (int i = 0; i < all.Length; i++) if (all[i] != bodyCapsule && !all[i].isTrigger) list.Add(all[i]);
                hitboxes = list.ToArray();
            }
            if (renderers == null || renderers.Length == 0) renderers = GetComponentsInChildren<Renderer>(true);
            if (model == null && animator != null && animator.transform != transform) model = animator.transform;
            if (model == transform) model = null;   // Akar dipegang agen; model akar tidak digeser
            capsuleDefault = bodyCapsule != null && bodyCapsule.enabled;
            hitboxDefault = new bool[hitboxes.Length];
            for (int i = 0; i < hitboxes.Length; i++) hitboxDefault[i] = hitboxes[i] != null && hitboxes[i].enabled;
            rendererDefault = new bool[renderers.Length];
            for (int i = 0; i < renderers.Length; i++) rendererDefault[i] = renderers[i] != null && renderers[i].enabled;
            if (model != null) modelBaseLocal = model.localPosition;
        }

        private void OnEnable()
        {
            EventChannel<HourChangedEvent>.Raised += HandleHour;
        }

        private void OnDisable()
        {
            EventChannel<HourChangedEvent>.Raised -= HandleHour;
        }

        // ---------------------------------------------------------------- Jam

        /// <summary>Jam seekHidingHour sampai siang (05:00-06:00 pada nilai awal).</summary>
        private static bool InHidingWindow(int hour)
        {
            var b = Bal;
            var bal = GameManager.CurrentBalance;
            if (b == null || !DayPhaseTracker.IsNightHour(hour)) return false;
            int dayStart = bal != null ? bal.spawn.dayStartHour : 6;
            return hour >= b.seekHidingHour && hour < dayStart;
        }

        private void HandleHour(HourChangedEvent e)
        {
            bool night = DayPhaseTracker.IsNightHour(e.hour);
            // SunlightExposure.md: SeekHiding dimulai saat HourChangedEvent mencapai seekHidingHour.
            if (InHidingWindow(e.hour) && mode == Mode.None) BeginSeekHiding();
            // Fajar: terlambat bersembunyi.
            if (!night && wasNight) HandleDaybreak();
            // Senja: naik dari tempat sembunyi.
            else if (night && !wasNight && mode == Mode.Submerged) BeginEmergeWithStagger();
            wasNight = night;
        }

        /// <summary>
        /// Terlambat bersembunyi (usulan disetujui developer, SunlightExposure.md bagian 5 no. 3): Zombie Desa yang belum
        /// Submerged saat fajar diperlakukan seperti zombie lain, tempatnya dilepas, lalu terbakar.
        /// </summary>
        private void HandleDaybreak()
        {
            if (mode == Mode.Submerged) return;
            if (mode == Mode.Seeking || mode == Mode.Emerging)
            {
                StopRoutine();
                ReleaseSpot();
                RestoreVisible();
                mode = Mode.None;
                if (ai.CurrentState == AIState.SeekHiding || ai.CurrentState == AIState.Emerging) ai.SetHidingState(AIState.Chase);
                if (agent.enabled && agent.isOnNavMesh) agent.isStopped = false;
            }
            // Tafsiran: "terbakar saat CheckExposure pertama" (skenario tes 8) diterapkan langsung lewat StartBurn,
            // yang tetap memakai jeda acak burnStaggerMaxSeconds, tanpa menunggu batas tempat tertutup.
            if (sun != null && ai.CurrentState != AIState.Dying) sun.StartBurn();
        }

        // ---------------------------------------------------------------- SeekHiding

        private void BeginSeekHiding()
        {
            var b = Bal;
            if (b == null || ai.IsBurning || ai.CurrentState == AIState.Dying || ai.CurrentState == AIState.BurnToAshes) return;
            // 2. Titik terdekat yang masih punya tempat.
            var found = HidingSpot.FindAvailable(transform.position, b.spotSearchRadius);
            // 3. Tidak ada titik: tetap mengejar seperti biasa, lalu kena aturan terlambat.
            if (found == null || !found.Reserve(this)) return;
            spot = found;
            // 1. Lepas target, tinggalkan pemain, hapus boost lewat SetSpeedMultiplier (agent.speed tidak ditulis langsung).
            if (attack != null) attack.CancelAttack();
            if (village != null) village.CancelPounce();
            ai.SetTarget(null);
            ai.SetSpeedMultiplier(SpeedSource.EmergeBoost, 1f);
            mode = Mode.Seeking;
            ai.SetHidingState(AIState.SeekHiding);
            StopRoutine();
            routine = StartCoroutine(SeekRoutine());
        }

        private IEnumerator SeekRoutine()
        {
            var wait = new WaitForSeconds(SeekPollSeconds);
            bool destSet = false;
            while (mode == Mode.Seeking)
            {
                if (spot == null) { AbortSeek(); yield break; }
                // Reaksi kena hit (#95) bisa menimpa state lalu EndReaction mengembalikan Chase: masuk SeekHiding lagi.
                var s = ai.CurrentState;
                if (s == AIState.Dying || s == AIState.BurnToAshes) { ReleaseSpot(); mode = Mode.None; routine = null; yield break; }
                if (s == AIState.Chase || s == AIState.Idle || s == AIState.AttackPlayer || s == AIState.AttackBuilding)
                {
                    if (attack != null) attack.CancelAttack();
                    ai.SetTarget(null);
                    ai.SetHidingState(AIState.SeekHiding);
                    destSet = false;
                }
                if (s != AIState.Stagger && s != AIState.Knockback && agent.enabled && agent.isOnNavMesh)
                {
                    // 4. Pergi ke spot.entryPoint.
                    if (!destSet)
                    {
                        agent.isStopped = false;
                        agent.SetDestination(spot.EntryPosition);
                        destSet = true;
                    }
                    else if (!agent.pathPending)
                    {
                        if (agent.pathStatus == NavMeshPathStatus.PathInvalid) { AbortSeek(); yield break; }
                        if (agent.remainingDistance <= agent.stoppingDistance + ArriveSlack) { routine = null; EnterSubmerged(true); yield break; }
                    }
                }
                yield return wait;
            }
            routine = null;
        }

        /// <summary>Titik tidak terjangkau atau hilang: kembali normal, lalu kena aturan terlambat saat fajar.</summary>
        private void AbortSeek()
        {
            ReleaseSpot();
            mode = Mode.None;
            routine = null;
            if (ai.CurrentState == AIState.SeekHiding) ai.SetHidingState(AIState.Chase);
        }

        // ---------------------------------------------------------------- Submerged

        private void EnterSubmerged(bool animate)
        {
            mode = Mode.Submerged;
            // 1. Agen, kapsul, dan hitbox mati. IsDormantCounted = true lewat SetHidingState.
            if (agent.enabled && agent.isOnNavMesh) agent.ResetPath();
            if (agent.enabled) agent.enabled = false;
            ai.SetHidingState(AIState.Submerged);
            SetColliders(false);
            // 3. Terlindungi dari sinar.
            if (sun != null) sun.IsProtected = true;
            // 2. Bool Dormant (#12), turun sejauh buryDepth, lalu Renderer mati. 4. FSM berhenti (EnemyAI.CrowdTick).
            if (animator != null && animator.isActiveAndEnabled) animator.SetBool(AnimIds.Dormant, true);
            StopRoutine();
            if (animate) routine = StartCoroutine(SubmergeRoutine());
            else { SetModelDepth(1f); SetRenderers(false); }
        }

        private IEnumerator SubmergeRoutine()
        {
            var b = Bal;
            float dur = b != null ? b.submergeSeconds : 2f;
            for (float t = 0f; t < dur; t += Time.deltaTime)
            {
                SetModelDepth(t / dur);
                yield return null;
            }
            SetModelDepth(1f);
            SetRenderers(false);
            routine = null;
        }

        // ---------------------------------------------------------------- Emerging

        private void BeginEmergeWithStagger()
        {
            var b = Bal;
            // 1. Jeda acak sampai emergeStaggerSeconds, dihitung di luar iterator (ref tidak boleh di iterator).
            float delay = 0f;
            int pointIndex = 0;
            if (RunSeedManager.Instance != null)
            {
                ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Enemy);
                if (b != null) delay = rng.Range(0f, b.emergeStaggerSeconds);
                pointIndex = rng.Range(0, Mathf.Max(1, spot != null ? spot.EmergePointCount : 1));
            }
            StopRoutine();
            routine = StartCoroutine(EmergeRoutine(delay, pointIndex));
        }

        private IEnumerator EmergeRoutine(float delay, int pointIndex)
        {
            if (delay > 0f) yield return new WaitForSeconds(delay);
            var b = Bal;
            // 2. Titik naik acak, agent.Warp ke sana, model mulai dari buryDepth di bawahnya.
            Vector3 target = spot != null ? spot.GetEmergePosition(pointIndex) : transform.position;
            float sample = GameManager.CurrentBalance != null ? GameManager.CurrentBalance.spawn.sampleRadius : 6f;
            if (!NavMesh.SamplePosition(target, out var hit, sample, NavMesh.AllAreas))
            {
                routine = null;
                Despawn(DespawnReason.OffNavMesh);
                yield break;
            }
            mode = Mode.Emerging;
            agent.enabled = true;
            agent.Warp(hit.position);
            if (!agent.isOnNavMesh) { routine = null; Despawn(DespawnReason.OffNavMesh); yield break; }
            agent.isStopped = true;
            ai.SetHidingState(AIState.Emerging);
            if (instance != null) instance.IsDormantCounted = true;   // Tetap tidak dihitung sampai langkah 4
            SetModelDepth(1f);
            // 3. Renderer, hitbox, dan kapsul menyala. Dormant = false, trigger Emerge (#12), efek dan suara.
            SetRenderers(true);
            SetColliders(true);
            if (animator != null && animator.isActiveAndEnabled)
            {
                animator.SetBool(AnimIds.Dormant, false);
                animator.SetTrigger(AnimIds.Emerge);
            }
            if (emergeEffect != null) emergeEffect.Play();
            if (emergeAudio != null) emergeAudio.Play();
            float dur = b != null ? b.emergeSeconds : 2f;
            for (float t = 0f; t < dur; t += Time.deltaTime)
            {
                // Dibunuh saat naik (hitbox sudah menyala): mayat milik EnemyHitReaction (#95).
                if (ai.CurrentState != AIState.Emerging) break;
                SetModelDepth(1f - t / dur);
                yield return null;
            }
            SetModelDepth(0f);
            routine = null;
            if (ai.CurrentState != AIState.Emerging)
            {
                // Atau terhuyung (#95): EndReaction mengembalikan Chase tanpa boost.
                ReleaseSpot();
                mode = Mode.None;
                if (sun != null) sun.IsProtected = false;
                if (instance != null) instance.IsDormantCounted = false;
                if (agent.enabled && agent.isOnNavMesh) agent.isStopped = false;
                yield break;
            }
            FinishEmerge();
        }

        private void FinishEmerge()
        {
            var b = Bal;
            // 4. Lepas tempat, IsDormantCounted = false, IsProtected = false.
            ReleaseSpot();
            mode = Mode.None;
            if (sun != null) sun.IsProtected = false;
            if (instance != null) instance.IsDormantCounted = false;
            // 6. Chase dengan boost sementara. EmergeBoost dikalikan sesudah batas maxZombieSpeed (EnemyAI.ApplySpeed).
            ai.SetHidingState(AIState.Chase);
            if (agent.enabled && agent.isOnNavMesh) agent.isStopped = false;
            if (b != null) ai.ApplySpeedModifier(SpeedSource.EmergeBoost, b.emergeBoostMultiplier, b.emergeBoostSeconds);
            // 5. Pemain dalam huntHintRadius x 2: langsung sadar. Lainnya memakai perburuan biasa (#49).
            var player = PlayerTargetable.Instance;
            var bal = GameManager.CurrentBalance;
            if (player != null && sensory != null && bal != null)
            {
                float r = bal.stealth.huntHintRadius * 2f;
                if ((player.Position - transform.position).sqrMagnitude <= r * r) sensory.ForceAware(player.Position);
            }
        }

        // ---------------------------------------------------------------- Dari SpawnManager

        /// <summary>
        /// SpawnManager.PopulateHidingSpots: zombie yang baru keluar dari pool langsung Submerged di titik ini.
        /// Mengembalikan false kalau titik sudah penuh (zombie tetap aktif; pemanggil yang mengembalikannya).
        /// </summary>
        public bool SpawnSubmerged(HidingSpot target)
        {
            if (target == null || !target.Reserve(this)) return false;
            externallyPlaced = true;
            spot = target;
            ai.SetTarget(null);
            EnterSubmerged(false);
            return true;
        }

        /// <summary>
        /// Slot gelombang malam SpawnManager (RequestWave, RequestBurst, RequestAmbush): zombie baru naik dari titik ini.
        /// Tanpa jeda acak, karena slot antrean sudah tersebar per frame. Titik tidak perlu punya tempat kosong,
        /// karena zombie ini tidak menginap.
        /// </summary>
        public void SpawnEmerging(HidingSpot target)
        {
            externallyPlaced = true;
            spot = target;
            if (target != null) target.Reserve(this);   // Dilepas lagi di FinishEmerge (langkah 4)
            int pointIndex = 0;
            if (RunSeedManager.Instance != null && target != null)
            {
                ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Enemy);
                pointIndex = rng.Range(0, Mathf.Max(1, target.EmergePointCount));
            }
            // Mulai tersembunyi, lalu naik.
            if (agent.enabled && agent.isOnNavMesh) agent.ResetPath();
            if (agent.enabled) agent.enabled = false;
            ai.SetHidingState(AIState.Submerged);
            SetColliders(false);
            SetRenderers(false);
            SetModelDepth(1f);
            StopRoutine();
            routine = StartCoroutine(EmergeRoutine(0f, pointIndex));
        }

        /// <summary>HidingSpot dimatikan (region dilepas, #88): rujukan dikosongkan. Zombie dikembalikan DespawnRegion.</summary>
        public void NotifySpotLost(HidingSpot lost)
        {
            if (spot != lost) return;
            spot = null;
        }

        // ---------------------------------------------------------------- Bantuan

        private void ReleaseSpot()
        {
            if (spot != null) spot.Release(this);
            spot = null;
        }

        private void StopRoutine()
        {
            if (routine != null) { StopCoroutine(routine); routine = null; }
        }

        private void SetColliders(bool on)
        {
            if (bodyCapsule != null) bodyCapsule.enabled = on && capsuleDefault;
            for (int i = 0; i < hitboxes.Length; i++) if (hitboxes[i] != null) hitboxes[i].enabled = on && hitboxDefault[i];
        }

        private void SetRenderers(bool on)
        {
            for (int i = 0; i < renderers.Length; i++) if (renderers[i] != null) renderers[i].enabled = on && rendererDefault[i];
        }

        private void RestoreVisible()
        {
            SetColliders(true);
            SetRenderers(true);
            SetModelDepth(0f);
            if (animator != null && animator.isActiveAndEnabled) animator.SetBool(AnimIds.Dormant, false);
            if (sun != null) sun.IsProtected = false;
        }

        /// <summary>0 = di permukaan, 1 = buryDepth di bawah permukaan.</summary>
        private void SetModelDepth(float t01)
        {
            if (model == null) return;
            float depth = spot != null ? spot.buryDepth : 0f;
            Vector3 down = model.parent != null ? model.parent.InverseTransformVector(Vector3.down * depth) : Vector3.down * depth;
            model.localPosition = modelBaseLocal + down * Mathf.Clamp01(t01);
        }

        private void Despawn(DespawnReason reason)
        {
            if (SpawnManager.Instance != null) SpawnManager.Instance.Despawn(gameObject, reason);
            else if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(gameObject);
        }

        // ---------------------------------------------------------------- Pool

        public void OnSpawned()
        {
            StopRoutine();
            mode = Mode.None;
            spot = null;
            externallyPlaced = false;
            if (model != null) model.localPosition = modelBaseLocal;
            SetColliders(true);
            SetRenderers(true);
            wasNight = DayPhaseTracker.IsNight;
            // Zombie yang muncul setelah seekHidingHour membaca HourChangedEvent.Last (SunlightExposure.md). Ditunda satu
            // frame, supaya urutan OnSpawned antar komponen tidak berpengaruh dan SpawnManager sempat memanggil
            // SpawnSubmerged/SpawnEmerging lebih dulu.
            routine = StartCoroutine(InitialCheck());
        }

        private IEnumerator InitialCheck()
        {
            yield return null;
            routine = null;
            if (externallyPlaced || mode != Mode.None) yield break;
            if (InHidingWindow(DayPhaseTracker.CurrentHour)) BeginSeekHiding();
        }

        /// <summary>Lepas tempat, lalu kembalikan Renderer, kapsul, dan hitbox ke nilai awal. EmergeBoost dihapus EnemyAI.OnSpawned.</summary>
        public void OnReturned()
        {
            StopRoutine();
            ReleaseSpot();
            mode = Mode.None;
            externallyPlaced = false;
            if (model != null) model.localPosition = modelBaseLocal;
            SetColliders(true);
            SetRenderers(true);
        }
    }
}
