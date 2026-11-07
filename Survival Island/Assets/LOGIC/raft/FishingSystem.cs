using TMPro;
using UnityEngine;

namespace SurvivalIsland
{
    public enum FishingState { Idle, Charging, LureFlying, Waiting, BiteWindow, Reeling, Intercepted, Recovering }
    public enum FishingEndReason { Caught, FishEscaped, LineBroken, Cancelled, Interrupted, WaterGone, TooFar, InterceptedByTarget }

    /// <summary>
    /// Sistem lain yang bisa mengait target dengan lemparan Kail (DebrisRetrieval #106). Tanpa ini, FishingSystem tetap utuh.
    /// </summary>
    public interface IKailCastInterceptor
    {
        bool IsTargetAt(Vector3 landing);                        // Untuk teks penanda
        bool TryIntercept(Vector3 landing, FishingSystem kail);  // true berarti kail mengait target lain
        void TickIntercept(bool isHeld, float dt);               // Tiap frame selama Intercepted
        void CancelIntercept();                                  // Saat sesi dibatalkan dari sisi Kail
    }

    /// <summary>
    /// Perilaku klik kiri Base_Kail: lempar ke air, tunggu gigitan acak, lalu tarik lewat Tension Bar. Klik kanan batal.
    /// Komponen viewmodel Base_Kail (EquipmentManager.md). Blueprint: raft/FishingSystem.md (#55).
    ///
    /// GDD bagian 3: lapar diisi dengan memancing memakai Kail. GDD bagian 4 (Diary Bab 1): memancing di tebing karang.
    /// Tension Bar dan semua angka adalah usulan yang disetujui developer sebagai nilai awal.
    /// Semua timer memakai Time.deltaTime (detik nyata), jadi berhenti saat dijeda.
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - Interceptor statis (RegisterInterceptor), karena DebrisRetrieval adalah singleton scene dan viewmodel Kail bisa dibuat
    ///   setelahnya.
    /// - SetInterceptTarget(Transform) tambahan supaya benang tergambar ke barang yang dikait (DebrisRetrieval.md langkah 3).
    /// - lureRegion: RegionStreaming.GetRegionAt (#88) belum ada, memakai ZoneTracker.Active.
    /// </summary>
    public class FishingSystem : EquippableBehaviour
    {
        private static IKailCastInterceptor interceptor;

        [SerializeField] private ItemData rawFishItem;             // Food_RawFish
        [SerializeField] private Transform rodTip;
        [SerializeField] private Transform lure;                    // Pelampung, objek milik viewmodel (tidak dipool)
        [SerializeField] private LineRenderer line;
        [SerializeField] private Transform landingMarker;
        [SerializeField] private TMP_Text landingLabel;
        [SerializeField] private Color validColor = new Color(0.3f, 0.9f, 0.4f);
        [SerializeField] private Color invalidColor = new Color(0.9f, 0.3f, 0.3f);
        [SerializeField] private Color targetColor = new Color(0.95f, 0.8f, 0.3f);
        [SerializeField] private float lureArcHeight = 3f;
        [SerializeField] private float biteSinkDepth = 0.15f;
        [SerializeField] private string splashVfxTag = "VFX_WaterSplash";
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip castClip, biteClip, catchClip, lineBreakClip;

        public FishingState State { get; private set; } = FishingState.Idle;
        public float Tension { get; private set; }
        public float CatchProgress { get; private set; }

        private float chargeTimer;
        private Vector3 lureLanding;
        private Vector3 lureStart;
        private WaterBody lureWater;
        private float biteTimer, stateTimer;
        private float surgeTimer, surgeLeft;
        private float slackTimer;
        private bool holdThisFrame;
        private float waterRecheckTimer;
        private Transform interceptTarget;
        private bool listening;
        private Transform lureHome;          // Induk asal pelampung di viewmodel
        private Vector3 lureHomeLocalPos;
        private Quaternion lureHomeLocalRot;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        public static void RegisterInterceptor(IKailCastInterceptor i) => interceptor = i;

        public static void UnregisterInterceptor(IKailCastInterceptor i)
        {
            if (interceptor == i) interceptor = null;
        }

        /// <summary>Kail berada di air (atau sedang mengait target).</summary>
        public bool IsLureInWater => State == FishingState.Waiting || State == FishingState.BiteWindow
                                     || State == FishingState.Reeling || State == FishingState.Intercepted;

        // ---------------------------------------------------------------- Siklus hidup

        private void OnEnable()
        {
            if (listening) return;
            listening = true;
            EventChannel<PlayerDamagedEvent>.Raised += HandleDamaged;
        }

        private void OnDisable()
        {
            if (listening) EventChannel<PlayerDamagedEvent>.Raised -= HandleDamaged;
            listening = false;
            if (State != FishingState.Idle && State != FishingState.Recovering) Cancel(FishingEndReason.Cancelled);
        }

        private void OnDestroy()
        {
            // Viewmodel dihapus saat pelampung terlepas: pelampung ikut dihapus supaya tidak tertinggal di dunia.
            if (lure != null && lure.parent == null && lureHome != null) Destroy(lure.gameObject);
        }

        public override void OnEquip(ItemInstance instance)
        {
            base.OnEquip(instance);
            // Viewmodel dipakai ulang: semua nilai ke awal, kail dan benang disembunyikan.
            ResetSession();
            State = FishingState.Idle;
        }

        public override void OnUnequip()
        {
            // Tabel pembatalan EquipmentManager.md: selain Idle, langsung berakhir dengan Cancelled.
            if (State != FishingState.Idle && State != FishingState.Recovering) Cancel(FishingEndReason.Cancelled);
            ResetSession();
            if (State != FishingState.Idle) SetState(FishingState.Idle, FishingEndReason.Cancelled);
            base.OnUnequip();
        }

        public override bool CanSwitchNow => true;

        private void ResetSession()
        {
            chargeTimer = 0f;
            biteTimer = stateTimer = 0f;
            surgeTimer = surgeLeft = 0f;
            slackTimer = 0f;
            holdThisFrame = false;
            waterRecheckTimer = 0f;
            Tension = 0f;
            CatchProgress = 0f;
            lureWater = null;
            interceptTarget = null;
            SetLureVisible(false);
            if (landingMarker != null) landingMarker.gameObject.SetActive(false);
        }

        // ---------------------------------------------------------------- Input (dari EquipmentManager)

        public override void PrimaryAction(ActionPhase phase)
        {
            switch (State)
            {
                case FishingState.Idle:
                    if (phase == ActionPhase.Pressed)
                    {
                        chargeTimer = 0f;
                        SetState(FishingState.Charging, FishingEndReason.Cancelled);
                    }
                    break;
                case FishingState.Charging:
                    if (phase == ActionPhase.Held) chargeTimer += Time.deltaTime;
                    else if (phase == ActionPhase.Released) Cast();
                    break;
                case FishingState.Waiting:
                    // Menarik kail kosong.
                    if (phase == ActionPhase.Pressed) Cancel(FishingEndReason.Cancelled);
                    break;
                case FishingState.BiteWindow:
                    if (phase == ActionPhase.Pressed) BeginReeling();
                    break;
                case FishingState.Reeling:
                case FishingState.Intercepted:
                    if (phase != ActionPhase.Released) holdThisFrame = true;
                    break;
            }
        }

        public override void SecondaryAction(ActionPhase phase)
        {
            if (phase != ActionPhase.Pressed) return;
            if (State == FishingState.Idle || State == FishingState.Recovering) return;
            Cancel(FishingEndReason.Cancelled);
        }

        // ---------------------------------------------------------------- Update

        private void Update()
        {
            float dt = Time.deltaTime;
            switch (State)
            {
                case FishingState.Charging:
                    UpdateMarker();
                    break;

                case FishingState.LureFlying:
                    stateTimer += dt;
                    float flight = Balance != null ? Mathf.Max(0.01f, Balance.fishing.lureFlightSeconds) : 0.8f;
                    float t = Mathf.Clamp01(stateTimer / flight);
                    if (lure != null)
                        lure.position = Vector3.Lerp(lureStart, lureLanding, t) + Vector3.up * (lureArcHeight * 4f * t * (1f - t));
                    if (t >= 1f) OnLureLanded();
                    break;

                case FishingState.Waiting:
                    biteTimer -= dt;
                    if (!RecheckWater(dt)) return;
                    if (biteTimer <= 0f)
                    {
                        // Pelampung tenggelam, suara berbunyi.
                        if (lure != null) lure.position = lureLanding + Vector3.down * biteSinkDepth;
                        Play(biteClip);
                        stateTimer = 0f;
                        SetState(FishingState.BiteWindow, FishingEndReason.Cancelled);
                    }
                    break;

                case FishingState.BiteWindow:
                    stateTimer += dt;
                    if (!RecheckWater(dt)) return;
                    if (stateTimer >= (Balance != null ? Balance.fishing.biteWindowSeconds : 2f))
                    {
                        // Ikan pergi: tunggu lagi tanpa melempar ulang.
                        Notify("notif.fish_escaped");   // "Ikan lolos"
                        if (lure != null) lure.position = lureLanding;
                        RollBiteDelay();
                        SetState(FishingState.Waiting, FishingEndReason.Cancelled);
                    }
                    break;

                case FishingState.Reeling:
                    TickReeling(dt);
                    break;

                case FishingState.Intercepted:
                    bool held = holdThisFrame;
                    holdThisFrame = false;
                    if (interceptor != null) interceptor.TickIntercept(held, dt);
                    else End(FishingEndReason.InterceptedByTarget);
                    break;

                case FishingState.Recovering:
                    stateTimer -= dt;
                    if (stateTimer <= 0f)
                    {
                        ResetSession();
                        SetState(FishingState.Idle, FishingEndReason.Cancelled);
                    }
                    break;
            }
        }

        private void LateUpdate()
        {
            bool inWater = State == FishingState.LureFlying || IsLureInWater;
            if (State == FishingState.Intercepted && interceptTarget != null && lure != null) lure.position = interceptTarget.position;
            if (line != null)
            {
                line.enabled = inWater && rodTip != null && lure != null;
                if (line.enabled)
                {
                    line.positionCount = 2;
                    line.SetPosition(0, rodTip.position);
                    line.SetPosition(1, lure.position);
                }
            }
            // Pemain menjauh melewati leashDistance: kail ditarik otomatis. Tarikan barang memakai jarak DebrisRetrieval.
            if (IsLureInWater && State != FishingState.Intercepted)
            {
                var player = PlayerTransform;
                float leash = Balance != null ? Balance.fishing.leashDistance : 32f;
                if (player != null && (player.position - lureLanding).sqrMagnitude > leash * leash) Cancel(FishingEndReason.TooFar);
            }
        }

        // ---------------------------------------------------------------- Casting

        private void UpdateMarker()
        {
            if (landingMarker == null) return;
            bool ok = ComputeLanding(out var point, out _);
            landingMarker.gameObject.SetActive(true);
            landingMarker.position = point;
            bool target = ok && interceptor != null && interceptor.IsTargetAt(point);
            if (landingLabel != null)
            {
                // Teks dari LocalizationManager (#104).
                landingLabel.text = target ? Text("fishing.marker.reel", "Tarik") : ok ? Text("fishing.marker.cast", "Pancing") : Text("fishing.marker.no_water", "Tidak ada air");
                landingLabel.color = target ? targetColor : ok ? validColor : invalidColor;
            }
        }

        private bool ComputeLanding(out Vector3 point, out WaterBody water)
        {
            water = null;
            var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : null;
            Transform camT = cam != null ? cam.transform : transform;
            var b = Balance != null ? Balance.fishing : null;
            // 1.
            float charge = b != null ? Mathf.Max(0.01f, b.castChargeSeconds) : 1.2f;
            float t = Mathf.Clamp01(chargeTimer / charge);
            float dist = Mathf.Lerp(b != null ? b.castMinDistance : 6f, b != null ? b.castMaxDistance : 25f, t);
            // 2.
            Vector3 flat = camT.forward;
            flat.y = 0f;
            if (flat.sqrMagnitude < 0.0001f) flat = transform.forward;
            flat.Normalize();
            Vector3 start = camT.position + flat * dist + Vector3.up * 2f;
            point = start + Vector3.down * 2f;
            // 3. Pengecualian trigger: collider Water adalah trigger (PhysicsLayers.md). Mask tanpa ZoneTrigger.
            float depth = b != null ? b.castProbeDepth : 40f;
            if (!PhysicsQuery.RaycastIncludingTriggers(new Ray(start, Vector3.down), out var hit, depth, PhysicsMasks.CastLanding))
                return false;
            point = hit.point;
            // 4. Hasil terdekat harus berlayer Water dan punya WaterBody.
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer(PhysicsMasks.WaterLayer)) return false;
            water = hit.collider.GetComponentInParent<WaterBody>();
            if (water == null) return false;
            // 5. Garis pandang dari kamera.
            Vector3 toPoint = hit.point - camT.position;
            float len = toPoint.magnitude;
            if (len > 0.01f && PhysicsQuery.Raycast(new Ray(camT.position, toPoint / len), out _, len - 0.05f, PhysicsMasks.InteractBlock))
            {
                water = null;
                return false;
            }
            return true;
        }

        private void Cast()
        {
            if (landingMarker != null) landingMarker.gameObject.SetActive(false);
            // 1. Tidak ada air: kembali ke Idle tanpa jeda.
            if (!ComputeLanding(out var point, out var water))
            {
                Notify("notif.no_water");   // "Tidak ada air"
                chargeTimer = 0f;
                SetState(FishingState.Idle, FishingEndReason.Cancelled);
                return;
            }
            // 2. Kail bergerak parabola dengan Transform, bukan Rigidbody.
            lureLanding = point;
            lureWater = water;
            lureStart = rodTip != null ? rodTip.position : transform.position;
            stateTimer = 0f;
            SetLureVisible(true);
            if (lure != null) lure.position = lureStart;
            Play(castClip);
            SetState(FishingState.LureFlying, FishingEndReason.Cancelled);
        }

        private void OnLureLanded()
        {
            // 3. Percikan.
            if (VFXManager.Instance != null && !string.IsNullOrEmpty(splashVfxTag))
                VFXManager.Instance.Play(splashVfxTag, lureLanding, Quaternion.identity);
            if (interceptor != null && interceptor.TryIntercept(lureLanding, this))
            {
                holdThisFrame = false;
                SetState(FishingState.Intercepted, FishingEndReason.Cancelled);
                return;
            }
            // 4.
            RollBiteDelay();
            waterRecheckTimer = 0f;
            SetState(FishingState.Waiting, FishingEndReason.Cancelled);
        }

        private void RollBiteDelay()
        {
            var b = Balance != null ? Balance.fishing : null;
            float min = b != null ? b.biteDelayMin : 12f;
            float max = b != null ? b.biteDelayMax : 30f;
            float delay;
            if (RunSeedManager.Instance != null)
            {
                ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Gameplay);
                delay = rng.Range(min, max);
            }
            else delay = Random.Range(min, max);
            delay *= lureWater != null ? lureWater.biteDelayMultiplier : 1f;
            // Region titik kail dari RegionStreaming (#88), cadangan ZoneTracker.Active.
            RegionType lureRegion = RegionStreaming.RegionOf(lureLanding);
            if (RegionalWeather.Instance != null && RegionalWeather.Instance.GetWeather(lureRegion) == WeatherType.Rain)
                delay *= b != null ? b.rainBiteMultiplier : 0.85f;
            biteTimer = delay;
        }

        /// <summary>Cek ulang air di bawah kail tiap waterRecheckSeconds. false kalau sesi berakhir (air surut, region dilepas).</summary>
        private bool RecheckWater(float dt)
        {
            waterRecheckTimer -= dt;
            if (waterRecheckTimer > 0f) return true;
            waterRecheckTimer = Balance != null ? Balance.fishing.waterRecheckSeconds : 1f;
            float depth = Balance != null ? Balance.fishing.castProbeDepth : 40f;
            Vector3 start = lureLanding + Vector3.up * 2f;
            bool ok = PhysicsQuery.RaycastIncludingTriggers(new Ray(start, Vector3.down), out var hit, depth, PhysicsMasks.CastLanding)
                      && hit.collider.gameObject.layer == LayerMask.NameToLayer(PhysicsMasks.WaterLayer)
                      && hit.collider.GetComponentInParent<WaterBody>() != null;
            if (ok) return true;
            Cancel(FishingEndReason.WaterGone);
            return false;
        }

        // ---------------------------------------------------------------- Tension Bar

        private void BeginReeling()
        {
            var b = Balance != null ? Balance.fishing : null;
            Tension = b != null ? b.tensionStart : 0.35f;
            CatchProgress = 0f;
            slackTimer = 0f;
            surgeLeft = 0f;
            surgeTimer = NextSurgeInterval();
            holdThisFrame = false;
            SetState(FishingState.Reeling, FishingEndReason.Cancelled);
        }

        private void TickReeling(float dt)
        {
            var b = Balance != null ? Balance.fishing : null;
            if (b == null) { holdThisFrame = false; return; }
            float rise = holdThisFrame ? b.tensionRisePerSec : -b.tensionFallPerSec;
            if (surgeLeft > 0f) { rise += b.surgeRisePerSec; surgeLeft -= dt; }
            else if ((surgeTimer -= dt) <= 0f) { surgeLeft = b.surgeSeconds; surgeTimer = NextSurgeInterval(); }

            Tension = Mathf.Clamp01(Tension + rise * dt);
            if (holdThisFrame) CatchProgress = Mathf.Min(1f, CatchProgress + b.progressPerSec * dt);
            slackTimer = Tension <= 0f ? slackTimer + dt : 0f;
            // Kail tidak mengubah Stamina pemain.
            holdThisFrame = false;

            if (Tension >= 1f) End(FishingEndReason.LineBroken);
            else if (CatchProgress >= 1f) End(FishingEndReason.Caught);
            else if (slackTimer >= b.slackEscapeSeconds) End(FishingEndReason.FishEscaped);
        }

        private float NextSurgeInterval()
        {
            var b = Balance != null ? Balance.fishing : null;
            float min = b != null ? b.surgeIntervalMin : 1.5f;
            float max = b != null ? b.surgeIntervalMax : 3.5f;
            if (RunSeedManager.Instance == null) return Random.Range(min, max);
            ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Gameplay);
            return rng.Range(min, max);
        }

        // ---------------------------------------------------------------- Mengakhiri sesi

        /// <summary>Dipanggil interceptor (DebrisRetrieval) saat tarikannya selesai.</summary>
        public void EndIntercept()
        {
            if (State == FishingState.Intercepted) End(FishingEndReason.InterceptedByTarget);
        }

        /// <summary>Benang tergambar ke target yang dikait interceptor. null = kembali ke titik jatuh.</summary>
        public void SetInterceptTarget(Transform target) => interceptTarget = target;

        public void Cancel(FishingEndReason reason)
        {
            if (State == FishingState.Idle || State == FishingState.Recovering) return;
            if (State == FishingState.Intercepted && interceptor != null) interceptor.CancelIntercept();
            End(reason);
        }

        private void End(FishingEndReason reason)
        {
            var b = Balance != null ? Balance.fishing : null;
            float recover = b != null ? b.recoverSeconds : 1f;
            switch (reason)
            {
                case FishingEndReason.Caught:
                    GiveFish();
                    Play(catchClip);
                    break;
                case FishingEndReason.LineBroken:
                    // Usulan disetujui #55: ikan hilang dan jeda 5 detik; Kail item inti, tidak rusak.
                    recover = b != null ? b.lineBreakRecoverSeconds : 5f;
                    Play(lineBreakClip);
                    Notify("notif.line_broken");   // "Benang putus"
                    break;
            }
            interceptTarget = null;
            SetLureVisible(false);
            if (landingMarker != null) landingMarker.gameObject.SetActive(false);
            Tension = 0f;
            stateTimer = recover;
            SetState(FishingState.Recovering, reason);
        }

        private void GiveFish()
        {
            if (rawFishItem == null) return;
            var inv = InventoryManager.Instance;
            int left = inv != null ? inv.AddItem(rawFishItem, 1) : 1;
            // Inventori penuh: ikan jatuh sebagai WorldItem di kaki pemain.
            if (left > 0 && inv != null && WorldItemManager.Instance != null)
            {
                var player = PlayerTransform;
                Vector3 pos = player != null ? player.position + Vector3.up * 0.3f : transform.position;
                WorldItemManager.Instance.Spawn(inv.CreateInstance(rawFishItem), left, pos, true);
            }
            EventChannel<FishCaughtEvent>.Publish(new FishCaughtEvent(rawFishItem.itemID));
        }

        private void SetState(FishingState s, FishingEndReason reason)
        {
            State = s;
            EventChannel<FishingStateChangedEvent>.Publish(new FishingStateChangedEvent(s, reason));
        }

        // ---------------------------------------------------------------- Serangan

        private void HandleDamaged(PlayerDamagedEvent e)
        {
            if (!IsLureInWater) return;
            // Usulan disetujui #55: serangan langsung membatalkan; damage berkala (lapar, haus, racun, dll.) tidak.
            var t = e.info.type;
            if (t == DamageType.Melee || t == DamageType.Bullet || t == DamageType.Fire || t == DamageType.Explosion)
                Cancel(FishingEndReason.Interrupted);
        }

        // ---------------------------------------------------------------- Bantuan

        private static Transform PlayerTransform => IslandPlayerController.Instance != null ? IslandPlayerController.Instance.transform : null;

        private void SetLureVisible(bool on)
        {
            if (lure != null)
            {
                if (on && lure.parent != null)
                {
                    // Pelampung dilepas dari viewmodel supaya diam di air saat pemain bergerak.
                    lureHome = lure.parent;
                    lureHomeLocalPos = lure.localPosition;
                    lureHomeLocalRot = lure.localRotation;
                    lure.SetParent(null, true);
                }
                else if (!on && lureHome != null)
                {
                    lure.SetParent(lureHome, false);
                    lure.localPosition = lureHomeLocalPos;
                    lure.localRotation = lureHomeLocalRot;
                }
                lure.gameObject.SetActive(on);
            }
            if (line != null && !on) line.enabled = false;
        }

        private void Play(AudioClip clip)
        {
            if (audioSource != null && clip != null) audioSource.PlayOneShot(clip);
        }

        private static void Notify(string key)
        {
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show(key, NotificationKind.Info);
        }

        /// <summary>Teks dari LocalizationManager (#104); teks bawaan kalau kunci belum ada di tabel.</summary>
        private static string Text(string key, string fallback)
        {
            var loc = LocalizationManager.Instance;
            return loc != null && loc.Has(key) ? loc.Get(key) : fallback;
        }
    }
}
