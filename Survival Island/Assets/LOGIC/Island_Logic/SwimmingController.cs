using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    public enum WaterMode { None, Wading, Swimming }

    /// <summary>
    /// Pemain Fase 2 di laut, sungai, atau danau: mengarungi, berenang, menyelam, kehabisan napas, dan tenggelam.
    /// Blueprint: Island_Logic/WaterSwimmingController.md (#13). GDD bagian 3 (HP habis menyebabkan mati) dan bagian 5
    /// (air di Region 4). Mekanik berenang adalah usulan yang disetujui developer (#85).
    ///
    /// Tidak menduplikasi gerak darat: PlayerTraversal memegang mode Swimming dan pengali kecepatan dibaca dari
    /// IslandPlayerController. Saat Swimming, IslandPlayerController berhenti menggerakkan pemain, lalu kelas ini yang
    /// memanggil CharacterController.Move. Satu-satunya pemilik SpeedModifierId.Water.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class SwimmingController : MonoBehaviour
    {
        public static SwimmingController Instance { get; private set; }

        [SerializeField] private CharacterController controller;
        [SerializeField] private IslandPlayerController playerController;
        [SerializeField] private PlayerTraversal traversal;
        [SerializeField] private SurvivalStats stats;
        [Tooltip("Titik mata (kamera dunia). Kosong berarti Camera.main.")]
        [SerializeField] private Transform eye;
        [SerializeField] private string splashVfxTag = "VFX_WaterSplash";

        private static readonly Collider[] overlapBuffer = new Collider[8];
        private readonly List<WaterVolume> touching = new List<WaterVolume>(4);

        private WaterVolume currentWater;
        private float breathSeconds;
        private bool isSinking;
        private float waterCheckTimer;
        private float publishTimer;
        private bool exhaustedNotified;

        public WaterMode Mode { get; private set; } = WaterMode.None;
        public WaterVolume CurrentWater => currentWater;
        public bool IsUnderwater { get; private set; }
        public bool IsSinking => isSinking;
        public float BreathRatio => MaxBreath > 0f ? Mathf.Clamp01(breathSeconds / MaxBreath) : 1f;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;
        private static float MaxBreath => Balance != null ? Balance.swimming.maxBreathSeconds : 30f;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
            if (controller == null) controller = GetComponent<CharacterController>();
            if (playerController == null) playerController = GetComponent<IslandPlayerController>();
            if (traversal == null) traversal = GetComponent<PlayerTraversal>();
            if (stats == null) stats = GetComponent<SurvivalStats>();
            breathSeconds = MaxBreath;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable() { EventChannel<RunEndedEvent>.Raised += HandleRunEnded; }

        private void OnDisable()
        {
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            SetMode(WaterMode.None);
        }

        private void Start()
        {
            // Kasus khusus "Load di dalam air": OnTriggerEnter belum tentu berjalan.
            RefillFromOverlap();
            breathSeconds = MaxBreath;
            PublishState();
        }

        private void HandleRunEnded(RunEndedEvent e)
        {
            SetMode(WaterMode.None);
            touching.Clear();
            currentWater = null;
        }

        private void RefillFromOverlap()
        {
            int n = PhysicsQuery.OverlapWater(transform.position, 0.5f, overlapBuffer);
            for (int i = 0; i < n; i++)
            {
                var v = overlapBuffer[i] != null ? overlapBuffer[i].GetComponent<WaterVolume>() : null;
                overlapBuffer[i] = null;
                if (v != null && !touching.Contains(v)) touching.Add(v);
            }
            PickCurrentWater();
        }

        // ---------------------------------------------------------------- Deteksi air

        private void OnTriggerEnter(Collider other)
        {
            var v = other.GetComponent<WaterVolume>();
            if (v != null && !touching.Contains(v)) touching.Add(v);
        }

        private void OnTriggerExit(Collider other)
        {
            var v = other.GetComponent<WaterVolume>();
            if (v != null) touching.Remove(v);
        }

        /// <summary>Volume dengan SurfaceY tertinggi (muara sungai dan laut). Volume nonaktif dibuang (region dilepas).</summary>
        private void PickCurrentWater()
        {
            currentWater = null;
            float best = float.MinValue;
            for (int i = touching.Count - 1; i >= 0; i--)
            {
                var v = touching[i];
                if (v == null || !v.IsActive) { touching.RemoveAt(i); continue; }
                if (v.SurfaceY > best) { best = v.SurfaceY; currentWater = v; }
            }
        }

        private float FeetY => transform.position.y + controller.center.y - controller.height * 0.5f;

        private Vector3 EyePosition
        {
            get
            {
                if (eye == null && Camera.main != null) eye = Camera.main.transform;
                return eye != null ? eye.position : transform.position + Vector3.up * controller.height * 0.9f;
            }
        }

        // ---------------------------------------------------------------- Update

        private void Update()
        {
            var bal = Balance;
            if (bal == null) return;
            float dt = Time.deltaTime;
            if (dt <= 0f) return;   // Jeda: stamina, napas, dan gerak berhenti
            var sw = bal.swimming;

            waterCheckTimer += dt;
            if (waterCheckTimer >= sw.waterCheckSeconds)
            {
                waterCheckTimer = 0f;
                UpdateMode(sw);
                if (Mode != WaterMode.Swimming && playerController != null && !playerController.IsGrounded) UpdateLandingProbe(sw);
            }

            if (Mode == WaterMode.Swimming)
            {
                // Diterkam (#110): mode Locked lebih tinggi dari Swimming, jadi gerak berhenti, napas tetap berjalan.
                if (traversal == null || traversal.Mode == TraversalMode.Swimming) TickSwim(bal, dt);
                else if (currentWater != null) TickBreath(sw, dt);
            }
            else if (IsUnderwater || breathSeconds < MaxBreath)
            {
                if (currentWater != null) TickBreath(sw, dt);
                else RecoverBreath(sw, dt);
            }
        }

        private void UpdateMode(SwimmingBalance sw)
        {
            PickCurrentWater();
            // 1.
            if (currentWater == null) { SetMode(WaterMode.None); return; }
            // 2.
            float depth = currentWater.SurfaceY - FeetY;
            // 3-5. Selisih swimEnterDepth dan swimExitDepth mencegah status berganti-ganti.
            WaterMode next;
            if (depth >= sw.swimEnterDepth) next = WaterMode.Swimming;
            else if (Mode == WaterMode.Swimming && depth > sw.swimExitDepth) next = WaterMode.Swimming;
            else if (depth >= sw.wadeEnterDepth) next = WaterMode.Wading;
            else next = WaterMode.None;
            SetMode(next);
        }

        private void SetMode(WaterMode next)
        {
            if (next == Mode) return;
            var prev = Mode;
            Mode = next;
            var sw = Balance != null ? Balance.swimming : null;

            if (prev == WaterMode.Swimming)
            {
                // Keluar dari Swimming.
                if (traversal != null) traversal.ClearMode(TraversalMode.Swimming);
                if (EquipmentManager.Instance != null) EquipmentManager.Instance.PopLock(EquipLock.Swimming);
                isSinking = false;
                exhaustedNotified = false;
                if (stats != null) stats.SetDrowning(false);
            }

            switch (next)
            {
                case WaterMode.Wading:
                    // Jalan biasa; lari dan lompat tetap mengikuti SurvivalStats.
                    if (playerController != null) playerController.SetSpeedModifier(SpeedModifierId.Water, sw != null ? sw.wadeSpeedMultiplier : 0.7f);
                    break;
                case WaterMode.Swimming:
                    if (traversal != null) traversal.SetMode(TraversalMode.Swimming);
                    if (playerController != null) playerController.SetSpeedModifier(SpeedModifierId.Water, sw != null ? sw.swimSpeedMultiplier : 0.5f);
                    // Item disimpan saat berenang (EquipmentManager.md).
                    if (EquipmentManager.Instance != null) EquipmentManager.Instance.PushLock(EquipLock.Swimming);
                    if (VFXManager.Instance != null && !string.IsNullOrEmpty(splashVfxTag))
                        VFXManager.Instance.Play(splashVfxTag, new Vector3(transform.position.x, currentWater != null ? currentWater.SurfaceY : transform.position.y, transform.position.z), Quaternion.identity);
                    break;
                default:
                    if (playerController != null) playerController.ClearSpeedModifier(SpeedModifierId.Water);
                    break;
            }
            PublishState();
        }

        // ---------------------------------------------------------------- Gerak renang

        private void TickSwim(GameBalanceConfig bal, float dt)
        {
            var sw = bal.swimming;
            var input = InputManager.Instance;
            Vector2 move = input != null ? Vector2.ClampMagnitude(input.MoveInput, 1f) : Vector2.zero;

            // 1. Tidak ada lari saat berenang.
            float speed = bal.traversal.walkSpeed * (playerController != null ? playerController.SpeedMultiplier : 1f);
            // 2. Arah mendatar dari input dan arah hadap (yaw) pemain.
            Vector3 horizontal = (transform.right * move.x + transform.forward * move.y) * speed;

            // 3. Vertikal.
            float y;
            if (isSinking) y = -sw.sinkSpeed;
            else if (input != null && input.CrouchHeld) y = -sw.verticalSwimSpeed;   // Menyelam
            else
            {
                // Naik sendiri dan mengapung: mata menuju SurfaceY + surfaceEyeOffset.
                float targetEye = currentWater != null ? currentWater.SurfaceY + sw.surfaceEyeOffset : EyePosition.y;
                y = Mathf.Clamp((targetEye - EyePosition.y) / dt, -sw.verticalSwimSpeed, sw.verticalSwimSpeed);
            }

            // 4.
            if (controller.enabled) controller.Move((horizontal + Vector3.up * y) * dt);

            // 5.
            TickStamina(bal, dt, move.sqrMagnitude > 0.01f);
            TickBreath(sw, dt);
        }

        private void TickStamina(GameBalanceConfig bal, float dt, bool moving)
        {
            if (stats == null) return;
            // 1. Usulan 3 disetujui: tidak ada regenerasi saat berenang, termasuk saat mengapung.
            float cost = (moving ? bal.vitals.swimStaminaPerSec : bal.swimming.idleStaminaPerSec) * dt;
            // 2.
            stats.SpendStamina(cost, StaminaUse.Swim);
            // 3.
            if (stats.Stamina <= 0f && !isSinking)
            {
                isSinking = true;
                if (!exhaustedNotified && NotificationManager.Instance != null)
                {
                    exhaustedNotified = true;
                    NotificationManager.Instance.Show("notif.swim_exhausted", NotificationKind.Danger);   // "Kehabisan tenaga"
                }
            }
        }

        // ---------------------------------------------------------------- Napas dan tenggelam

        private void TickBreath(SwimmingBalance sw, float dt)
        {
            bool wasUnder = IsUnderwater;
            // 1.
            IsUnderwater = currentWater != null && EyePosition.y < currentWater.SurfaceY;
            // 2-3.
            if (IsUnderwater) breathSeconds -= dt * (isSinking ? sw.sinkingBreathMultiplier : 1f);
            else breathSeconds = Mathf.Min(MaxBreath, breathSeconds + sw.breathRecoverPerSec * dt);
            if (breathSeconds < 0f) breathSeconds = 0f;
            // 4. SurvivalStats menghitung damage Drown (vitals.drownDamagePerSec), alasan Drowned (VitalsRules.md).
            if (stats != null) stats.SetDrowning(IsUnderwater && breathSeconds <= 0f);
            // 5.
            PublishThrottled(sw, dt, wasUnder != IsUnderwater);
        }

        private void RecoverBreath(SwimmingBalance sw, float dt)
        {
            bool wasUnder = IsUnderwater;
            IsUnderwater = false;
            breathSeconds = Mathf.Min(MaxBreath, breathSeconds + sw.breathRecoverPerSec * dt);
            if (stats != null && stats.IsDrowning) stats.SetDrowning(false);
            PublishThrottled(sw, dt, wasUnder);
        }

        private void PublishThrottled(SwimmingBalance sw, float dt, bool force)
        {
            publishTimer += dt;
            float interval = 1f / Mathf.Max(0.1f, sw.breathPublishHz);
            if (force || (BreathRatio < 1f && publishTimer >= interval) || (BreathRatio >= 1f && publishTimer >= interval && EventChannel<SwimStateChangedEvent>.HasLast && EventChannel<SwimStateChangedEvent>.Last.breathRatio < 1f))
            {
                publishTimer = 0f;
                PublishState();
            }
        }

        private void PublishState()
        {
            EventChannel<SwimStateChangedEvent>.Publish(new SwimStateChangedEvent(Mode == WaterMode.Swimming, IsUnderwater, BreathRatio));
        }

        // ---------------------------------------------------------------- Jatuh ke air

        private void UpdateLandingProbe(SwimmingBalance sw)
        {
            if (traversal == null) return;
            // 1. Pengecualian query trigger (#87): air memang trigger, jadi RaycastWater memakai Collide.
            if (!PhysicsQuery.RaycastWater(transform.position, Vector3.down, sw.landingProbeDistance, out var waterHit))
            {
                traversal.SetOverWater(false);
                return;
            }
            // 2. Tanah di bawah titik air.
            float depth = sw.landingProbeDistance;
            if (PhysicsQuery.Raycast(new Ray(waterHit.point, Vector3.down), out var groundHit, sw.landingProbeDistance, PhysicsMasks.Ground))
                depth = waterHit.point.y - groundHit.point.y;
            // 3. Air dangkal tidak menyelamatkan dari damage jatuh.
            traversal.SetOverWater(depth >= sw.swimEnterDepth);
        }
    }
}
