using UnityEngine;

namespace SurvivalIsland
{
    public enum TraversalMode { Ground, Crouching, Climbing, Sliding, Swimming, Locked }

    public enum SpeedModifierId { Surface, Crouch, Water, HeavyItem, Weather, ItemUse, StatusEffect }

    /// <summary>
    /// Jongkok, tangga dan tali, damage jatuh, lereng curam, dan pengaruh permukaan untuk pemain Fase 2.
    /// Blueprint: Island_Logic/PlayerTraversal.md (#101). GDD bagian 3: jongkok mendukung Mengendap (menghindar dari zombie,
    /// kecuali Blood Moon); HP habis karena jatuh tetap Game Over. GDD bagian 5: jurang Region 2, puncak es Region 3,
    /// dan barang berat. Mekanik dan angka adalah usulan yang disetujui developer (#85).
    ///
    /// Prioritas mode: Locked, Swimming, Climbing, Sliding, Crouching, lalu Ground.
    /// Belum ada di Langkah 3 (TODO): SurfaceDetection.GetSurfaceAt (#86, Langkah 4; permukaan tetap Tanah sampai ada),
    /// NoiseEmitter.EmitLanding (#49, Langkah 5).
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerTraversal : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private IslandPlayerController playerController;
        [SerializeField] private SurvivalStats stats;

        // Mode dasar (Ground, Crouching, Climbing, Sliding) dan dua penanda di atasnya (Swimming, Locked).
        private TraversalMode baseMode = TraversalMode.Ground;
        private bool swimming;
        private bool locked;

        private ClimbableSurface activeClimb;
        private float climbT;
        private float climbTransitionTimer;
        private float climbTransitionDuration;
        private Vector3 climbTransitionFrom, climbTransitionTo;
        private bool climbExiting;
        private bool climbExitTop;

        private float fallPeakDownSpeed;
        private bool wasGrounded = true;
        private Vector3 lastGroundedPosition;
        private SurfaceType currentSurface = SurfaceType.Tanah;
        private float surfaceTimer;
        private bool isOverWater;
        private bool carryingHeavy;
        private float targetHeight;

        public TraversalMode Mode
        {
            get
            {
                if (locked) return TraversalMode.Locked;
                if (swimming) return TraversalMode.Swimming;
                return baseMode;
            }
        }

        /// <summary>Dibaca NoiseEmitter (#49) untuk radius langkah.</summary>
        public bool IsCrouching { get; private set; }

        /// <summary>IslandPlayerController hanya menggerakkan pemain pada mode Ground dan Crouching.</summary>
        public bool AllowsGroundMove => Mode == TraversalMode.Ground || Mode == TraversalMode.Crouching;

        public SurfaceType CurrentSurface => currentSurface;
        public ClimbableSurface ActiveClimb => activeClimb;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (controller == null) controller = GetComponent<CharacterController>();
            if (playerController == null) playerController = GetComponent<IslandPlayerController>();
            if (stats == null) stats = GetComponent<SurvivalStats>();
            targetHeight = Balance != null ? Balance.traversal.standHeight : controller.height;
            lastGroundedPosition = transform.position;
        }

        private void OnEnable()
        {
            EventChannel<HeavyItemChangedEvent>.Raised += HandleHeavyItem;
            EventChannel<PlayerDiedEvent>.Raised += HandleDied;
            carryingHeavy = EventChannel<HeavyItemChangedEvent>.HasLast && EventChannel<HeavyItemChangedEvent>.Last.isCarried;
        }

        private void OnDisable()
        {
            EventChannel<HeavyItemChangedEvent>.Raised -= HandleHeavyItem;
            EventChannel<PlayerDiedEvent>.Raised -= HandleDied;
        }

        private void HandleHeavyItem(HeavyItemChangedEvent e) => carryingHeavy = e.isCarried;

        private void HandleDied(PlayerDiedEvent e)
        {
            if (baseMode == TraversalMode.Climbing) EndClimb();
            SetLocked(true);
        }

        // ---------------------------------------------------------------- Update

        private void Update()
        {
            var bal = Balance;
            if (bal == null) return;
            var t = bal.traversal;
            bool grounded = playerController != null && playerController.IsGrounded;
            var mode = Mode;
            bool special = mode == TraversalMode.Climbing || mode == TraversalMode.Swimming || mode == TraversalMode.Locked;

            if (!special) UpdateCrouch(t);
            UpdateHeight(t);
            UpdateSurface(t);
            if (!special) UpdateSlide(t, grounded);
            if (Mode == TraversalMode.Climbing) UpdateClimb(t, bal.vitals);
            UpdateFall(t, grounded);
        }

        // ---------------------------------------------------------------- Mode untuk sistem lain

        /// <summary>Dipakai SwimmingController (#13) dan PlayerPinnedState (#110).</summary>
        public void SetMode(TraversalMode mode)
        {
            switch (mode)
            {
                case TraversalMode.Swimming: swimming = true; break;
                case TraversalMode.Locked: locked = true; break;
                case TraversalMode.Climbing: break;   // Hanya lewat BeginClimb
                default: baseMode = mode; break;
            }
        }

        /// <summary>Kalau Mode == mode, kembali ke Ground. Kalau tidak, tidak berubah.</summary>
        public void ClearMode(TraversalMode mode)
        {
            if (Mode != mode) return;
            if (mode == TraversalMode.Swimming) swimming = false;
            else if (mode == TraversalMode.Locked) locked = false;
            else if (mode == TraversalMode.Climbing) EndClimb();
            else baseMode = TraversalMode.Ground;
        }

        /// <summary>Diterkam (#110) dan mati (#53).</summary>
        public void SetLocked(bool value) => locked = value;

        public void SetOverWater(bool value) => isOverWater = value;

        // ---------------------------------------------------------------- Jongkok

        private void UpdateCrouch(TraversalBalance t)
        {
            var input = InputManager.Instance;
            bool wantCrouch = input != null && input.CrouchHeld && !carryingHeavy;

            if (wantCrouch && !IsCrouching) StartCrouch(t);
            else if (!wantCrouch && IsCrouching && CanStandUp()) StandUp(t);
        }

        private void StartCrouch(TraversalBalance t)
        {
            IsCrouching = true;
            if (baseMode == TraversalMode.Ground) baseMode = TraversalMode.Crouching;
            targetHeight = t.crouchHeight;
            if (playerController != null) playerController.SetSpeedModifier(SpeedModifierId.Crouch, t.crouchSpeedMultiplier);
            if (FirstPersonCamera.Instance != null) FirstPersonCamera.Instance.SetEyeHeight(t.eyeHeightCrouch);
        }

        private void StandUp(TraversalBalance t)
        {
            IsCrouching = false;
            if (baseMode == TraversalMode.Crouching) baseMode = TraversalMode.Ground;
            targetHeight = t.standHeight;
            if (playerController != null) playerController.ClearSpeedModifier(SpeedModifierId.Crouch);
            if (FirstPersonCamera.Instance != null) FirstPersonCamera.Instance.ResetEyeHeight();
        }

        /// <summary>Melompat saat jongkok berdiri dulu, kalau ada ruang. Dipanggil IslandPlayerController.</summary>
        public bool TryStandForJump()
        {
            if (!IsCrouching) return true;
            if (!CanStandUp() || Balance == null) return false;
            StandUp(Balance.traversal);
            return true;
        }

        /// <summary>Pemeriksaan ruang kepala. Kapsul 0,95 x radius supaya tidak mengenai dinding samping.</summary>
        private bool CanStandUp()
        {
            float stand = Balance != null ? Balance.traversal.standHeight : 1.8f;
            Vector3 bottom = transform.position + Vector3.up * (controller.radius + 0.05f);
            Vector3 top = transform.position + Vector3.up * (stand - controller.radius);
            return !PhysicsQuery.CheckCapsule(bottom, top, controller.radius * 0.95f, PhysicsMasks.Ground);
        }

        /// <summary>Tinggi CharacterController menuju target, kaki tetap di tanah.</summary>
        private void UpdateHeight(TraversalBalance t)
        {
            if (Mathf.Approximately(controller.height, targetHeight)) return;
            controller.height = Mathf.MoveTowards(controller.height, targetHeight, t.heightChangeSpeed * Time.deltaTime);
            var c = controller.center;
            c.y = controller.height / 2f;
            controller.center = c;
        }

        // ---------------------------------------------------------------- Tangga dan tali

        /// <summary>Dipanggil ClimbableSurface. Senjata tersimpan (#60), kamera dibatasi (#102).</summary>
        public bool BeginClimb(ClimbableSurface surface)
        {
            if (surface == null || carryingHeavy) return false;
            var mode = Mode;
            if (mode == TraversalMode.Locked || mode == TraversalMode.Swimming || mode == TraversalMode.Climbing) return false;
            if (IsCrouching && !TryStandForJump()) return false;

            baseMode = TraversalMode.Climbing;
            activeClimb = surface;
            climbT = surface.ClosestT(transform.position);
            climbExiting = false;
            if (EquipmentManager.Instance != null) EquipmentManager.Instance.PushLock(EquipLock.Climbing);
            if (FirstPersonCamera.Instance != null) FirstPersonCamera.Instance.EnterClimbView(surface.Facing);
            StartTransition(transform.position, Vector3.Lerp(surface.Bottom, surface.Top, climbT),
                Balance != null ? Balance.traversal.climbEnterSeconds : 0.25f);
            return true;
        }

        private void StartTransition(Vector3 from, Vector3 to, float seconds)
        {
            climbTransitionFrom = from;
            climbTransitionTo = to;
            climbTransitionDuration = Mathf.Max(0.01f, seconds);
            climbTransitionTimer = 0f;
        }

        private bool TickTransition()
        {
            if (climbTransitionTimer >= climbTransitionDuration) return false;
            climbTransitionTimer += Time.deltaTime;
            float k = Mathf.Clamp01(climbTransitionTimer / climbTransitionDuration);
            MoveTo(Vector3.Lerp(climbTransitionFrom, climbTransitionTo, k));
            return climbTransitionTimer < climbTransitionDuration;
        }

        private void MoveTo(Vector3 target)
        {
            if (controller.enabled) controller.Move(target - transform.position);
        }

        private void UpdateClimb(TraversalBalance t, VitalsBalance v)
        {
            // Tangga hancur atau dilepas saat dipakai: kembali ke Ground dan jatuh biasa.
            if (activeClimb == null || !activeClimb.isActiveAndEnabled)
            {
                EndClimb();
                return;
            }
            if (TickTransition()) return;
            if (climbExiting)
            {
                if (climbExitTop) fallPeakDownSpeed = 0f;
                EndClimb();
                return;
            }

            var input = InputManager.Instance;
            if (input != null && input.JumpPressed)
            {
                // Lepas dengan dorongan ke belakang, lalu jatuh biasa.
                Vector3 back = -activeClimb.Facing.normalized * t.climbJumpOffForce;
                EndClimb();
                if (playerController != null) playerController.AddImpulse(back);
                return;
            }

            float axis = input != null ? input.MoveInput.y : 0f;   // W naik, S turun
            float speed = activeClimb.kind == ClimbKind.Ladder ? t.ladderClimbSpeed : t.ropeClimbSpeed;
            if (Mathf.Abs(axis) > 0.1f && stats != null) stats.SpendStamina(v.climbStaminaPerSec * Time.deltaTime, StaminaUse.Climb);
            // Stamina habis berarti pelan, tanpa jatuh (usulan disetujui #85 poin 2).
            if (stats != null && stats.Stamina <= 0f) speed *= t.climbExhaustedMultiplier;

            climbT += axis * speed * Time.deltaTime / activeClimb.Length;
            if (climbT >= 1f)
            {
                climbT = 1f;
                climbExiting = true;
                climbExitTop = true;
                StartTransition(transform.position, activeClimb.ExitTop, t.climbExitSeconds);
                return;
            }
            if (climbT <= 0f)
            {
                climbT = 0f;
                climbExiting = true;
                climbExitTop = false;
                StartTransition(transform.position, activeClimb.ExitBottom, t.climbExitSeconds);
                return;
            }
            MoveTo(Vector3.Lerp(activeClimb.Bottom, activeClimb.Top, climbT));
        }

        public void EndClimb()
        {
            bool wasClimbing = baseMode == TraversalMode.Climbing;
            activeClimb = null;
            climbExiting = false;
            if (baseMode == TraversalMode.Climbing) baseMode = TraversalMode.Ground;
            if (!wasClimbing) return;
            if (EquipmentManager.Instance != null) EquipmentManager.Instance.PopLock(EquipLock.Climbing);
            if (FirstPersonCamera.Instance != null) FirstPersonCamera.Instance.ExitClimbView();
        }

        /// <summary>Dipanggil #110 atau serangan menahan: pemain lepas dari tangga.</summary>
        public void ForceDetach()
        {
            if (baseMode == TraversalMode.Climbing) EndClimb();
        }

        // ---------------------------------------------------------------- Damage jatuh

        private void UpdateFall(TraversalBalance t, bool grounded)
        {
            var mode = Mode;
            if (!grounded && mode != TraversalMode.Climbing && mode != TraversalMode.Swimming)
                fallPeakDownSpeed = Mathf.Max(fallPeakDownSpeed, -controller.velocity.y);

            if (grounded && !wasGrounded) OnLanded(t);
            if (grounded && mode != TraversalMode.Sliding) lastGroundedPosition = transform.position;
            wasGrounded = grounded;
        }

        /// <summary>Damage jatuh berbasis kecepatan, dikirim lewat jalur #59 dengan DamageType.Fall.</summary>
        private void OnLanded(TraversalBalance t)
        {
            float peak = fallPeakDownSpeed;
            fallPeakDownSpeed = 0f;
            // 1. Mendarat di air: tidak ada damage (usulan disetujui #85 poin 3).
            if (isOverWater || swimming) return;

            // 2-4.
            float k = Mathf.InverseLerp(t.fallSafeSpeed, t.fallLethalSpeed, peak);
            if (k > 0f && stats != null)
            {
                float damage = k * 100f * FallMultiplier(t);
                stats.TakeDamage(new DamageInfo
                {
                    amount = damage, type = DamageType.Fall,
                    hitPoint = transform.position, hitDirection = Vector3.down, bodyPart = BodyPart.Leg
                });
            }
            // 5. Dip kecil juga tampil pada pendaratan biasa.
            float impact01 = Mathf.InverseLerp(3f, t.fallLethalSpeed, peak);
            if (impact01 > 0f && FirstPersonCamera.Instance != null) FirstPersonCamera.Instance.PlayLandingDip(impact01);
            // 6. TODO Langkah 5 (#49): NoiseEmitter.EmitLanding(impact01).
        }

        private float FallMultiplier(TraversalBalance t)
        {
            var profiles = t.surfaceProfiles;
            if (profiles != null)
                for (int i = 0; i < profiles.Length; i++)
                    if (profiles[i].surface == currentSurface) return profiles[i].fallDamageMultiplier;
            return 1f;
        }

        /// <summary>Dipanggil setelah teleport dan saat CharacterController dinyalakan kembali (#56, #88, #116).</summary>
        public void ResetFallTracking()
        {
            fallPeakDownSpeed = 0f;
            wasGrounded = true;
        }

        /// <summary>Posisi aman untuk save: titik bawah tangga saat memanjat, posisi tanah terakhir saat melayang atau meluncur.</summary>
        public Vector3 GetSafeSavePosition()
        {
            if (Mode == TraversalMode.Climbing && activeClimb != null) return activeClimb.ExitBottom;
            bool grounded = playerController != null && playerController.IsGrounded;
            if (!grounded || Mode == TraversalMode.Sliding) return lastGroundedPosition;
            return transform.position;
        }

        // ---------------------------------------------------------------- Lereng curam

        private void UpdateSlide(TraversalBalance t, bool grounded)
        {
            if (!grounded)
            {
                if (baseMode == TraversalMode.Sliding) baseMode = IsCrouching ? TraversalMode.Crouching : TraversalMode.Ground;
                return;
            }
            Vector3 origin = transform.position + Vector3.up * 0.2f;
            if (!PhysicsQuery.Raycast(new Ray(origin, Vector3.down), out var hit, 0.5f, PhysicsMasks.Ground))
            {
                if (baseMode == TraversalMode.Sliding) baseMode = TraversalMode.Ground;
                return;
            }
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            if (angle >= t.slideMinAngle)
            {
                baseMode = TraversalMode.Sliding;
                Vector3 dir = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;
                var input = InputManager.Instance;
                float side = input != null ? input.MoveInput.x : 0f;
                Vector3 steer = transform.right * side * t.slideSteerSpeed;
                controller.Move((dir * t.slideSpeed + steer + Vector3.down * 2f) * Time.deltaTime);
            }
            else if (baseMode == TraversalMode.Sliding)
            {
                baseMode = IsCrouching ? TraversalMode.Crouching : TraversalMode.Ground;
            }
        }

        // ---------------------------------------------------------------- Permukaan

        private void UpdateSurface(TraversalBalance t)
        {
            surfaceTimer -= Time.deltaTime;
            if (surfaceTimer > 0f) return;
            surfaceTimer = t.surfaceCheckSeconds;
            // Hook Langkah 4 (#86): permukaan dari SurfaceDetection kalau ada.
            var surface = SurfaceDetection.Instance != null ? SurfaceDetection.Instance.GetSurfaceAt(transform.position) : currentSurface;
            ApplySurface(surface, t);
        }

        private void ApplySurface(SurfaceType surface, TraversalBalance t)
        {
            bool changed = surface != currentSurface || !surfaceApplied;
            currentSurface = surface;
            surfaceApplied = true;
            if (!changed || playerController == null || t.surfaceProfiles == null) return;
            float mult = 1f;
            for (int i = 0; i < t.surfaceProfiles.Length; i++)
                if (t.surfaceProfiles[i].surface == surface) { mult = t.surfaceProfiles[i].speedMultiplier; break; }
            playerController.SetSpeedModifier(SpeedModifierId.Surface, mult);
        }

        private bool surfaceApplied;
    }
}
