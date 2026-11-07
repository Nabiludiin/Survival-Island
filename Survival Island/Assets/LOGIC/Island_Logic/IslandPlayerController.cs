using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Simpanan posisi pemain (WorldStateSave.md: player_transform).</summary>
    [Serializable]
    public struct PlayerTransformSaveData
    {
        public Vector3 position;
        public float yaw;
        public float pitch;
    }

    /// <summary>
    /// Pergerakan Fase 2: jalan, lari, lompat, gravitasi, dan pengali kecepatan. Mode gerak lain milik PlayerTraversal.
    /// Blueprint: Island_Logic/IslandPlayerController.md (#101, melengkapi #18, #49, #69, #102, #111).
    /// GDD bagian 5 Region 4 dan Claude.md Tahap 4: membawa suku cadang = kecepatan setengah, Sprint dan Jump mati.
    /// Mouse look milik FirstPersonCamera (#102). Stamina milik SurvivalStats (#111).
    ///
    /// Catatan implementasi (Langkah 3): HeavyItemSystem.Instance.IsCarrying (Langkah 6) dibaca lewat HeavyItemChangedEvent
    /// sticky, supaya tidak bergantung pada kelas yang belum ada. Teleport memanggil ZoneManager.ResyncAll (ditambah Langkah 6).
    /// Save posisi (player_transform, LoadOrder 80) ditulis di sini sesuai tabel WorldStateSave.md.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class IslandPlayerController : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "player_transform";

        public static IslandPlayerController Instance { get; private set; }

        [SerializeField] private CharacterController controller;
        [SerializeField] private PlayerTraversal traversal;
        [SerializeField] private SurvivalStats stats;

        private Vector3 velocity;
        // Dorongan luar (#101, keputusan developer 7 November 2026): kecepatan awal yang memudar linier ke 0.
        private Vector3 externalStartVelocity;
        private float externalDuration;
        private float externalElapsed;
        private readonly float[] speedModifiers = new float[Enum.GetValues(typeof(SpeedModifierId)).Length];
        private bool carryingHeavy;
        private bool registeredForSave;

        /// <summary>Hasil kali semua pengali aktif, batas bawah 0,1 (PlayerTraversal.md "Pengali kecepatan").</summary>
        public float SpeedMultiplier { get; private set; } = 1f;
        public bool IsSprinting { get; private set; }
        public bool IsGrounded { get; private set; } = true;
        public Vector3 Velocity => velocity;
        public CharacterController Controller => controller;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
            if (controller == null) controller = GetComponent<CharacterController>();
            if (traversal == null) traversal = GetComponent<PlayerTraversal>();
            if (stats == null) stats = GetComponent<SurvivalStats>();
            for (int i = 0; i < speedModifiers.Length; i++) speedModifiers[i] = 1f;
            if (Balance != null) controller.slopeLimit = Balance.traversal.slideMinAngle;
        }

        private void OnEnable()
        {
            EventChannel<HeavyItemChangedEvent>.Raised += HandleHeavyItem;
            EventChannel<PlayerDiedEvent>.Raised += HandlePlayerDied;
            carryingHeavy = EventChannel<HeavyItemChangedEvent>.HasLast && EventChannel<HeavyItemChangedEvent>.Last.isCarried;
            if (WorldStateSave.Instance != null)
            {
                WorldStateSave.Instance.Register(this);
                registeredForSave = true;
            }
        }

        private void OnDisable()
        {
            EventChannel<HeavyItemChangedEvent>.Raised -= HandleHeavyItem;
            EventChannel<PlayerDiedEvent>.Raised -= HandlePlayerDied;
            ClearExternalVelocity();
            if (registeredForSave && WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            registeredForSave = false;
            IsSprinting = false;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void HandleHeavyItem(HeavyItemChangedEvent e) => carryingHeavy = e.isCarried;
        private void HandlePlayerDied(PlayerDiedEvent e) => ClearExternalVelocity();

        // ---------------------------------------------------------------- Gerak

        private void Update()
        {
            var bal = Balance;
            if (bal == null || !controller.enabled) return;
            var t = bal.traversal;

            // 3. Tanah: isGrounded ditambah raycast pendek ke PhysicsMasks.Ground (menggantikan Physics.CheckSphere).
            IsGrounded = CheckGrounded(t);
            Vector3 external = TickExternalVelocity(Time.deltaTime);

            // 1. Mode Climbing, Sliding, Swimming, Locked: skrip ini tidak menggerakkan pemain.
            // Pengecualian: dorongan luar tetap berlaku (misalnya jerat Tribal menarik pemain yang sedang Locked karena ditahan).
            if (traversal != null && !traversal.AllowsGroundMove)
            {
                IsSprinting = false;
                if (traversal.Mode != TraversalMode.Sliding) velocity.y = 0f;
                if (external != Vector3.zero) controller.Move(external * Time.deltaTime);
                return;
            }

            // 2.
            var input = InputManager.Instance;
            Vector2 move = input != null ? Vector2.ClampMagnitude(input.MoveInput, 1f) : Vector2.zero;
            bool crouching = traversal != null && traversal.IsCrouching;

            // 4. Lari: Sprint diabaikan saat jongkok (PlayerTraversal.md) dan saat membawa Heavy Item (Claude.md Tahap 4).
            IsSprinting = false;
            if (input != null && input.SprintHeld && move.y > 0.1f && !carryingHeavy && !crouching && stats != null)
                IsSprinting = stats.SpendStamina(bal.vitals.sprintStaminaPerSec * Time.deltaTime, StaminaUse.Sprint);

            // 5. Kecepatan akhir.
            float speed = (IsSprinting ? t.sprintSpeed : t.walkSpeed) * SpeedMultiplier;
            Vector3 horizontal = (transform.right * move.x + transform.forward * move.y) * speed;

            // 6. Lompat. Heavy Item mematikan lompat (Claude.md Tahap 4). Jongkok berdiri dulu kalau ada ruang.
            if (IsGrounded && input != null && input.JumpPressed && !carryingHeavy && stats != null)
            {
                bool canStand = traversal == null || !crouching || traversal.TryStandForJump();
                if (canStand && stats.SpendStamina(bal.vitals.jumpStaminaCost, StaminaUse.Jump))
                    velocity.y = Mathf.Sqrt(t.jumpHeight * -2f * t.gravity);
            }

            // 7. Gravitasi, lalu Move.
            if (IsGrounded && velocity.y < 0f) velocity.y = -2f;
            velocity.y += t.gravity * Time.deltaTime;
            controller.Move((horizontal + external + Vector3.up * velocity.y) * Time.deltaTime);
        }

        // ---------------------------------------------------------------- Dorongan luar (#101)

        /// <summary>
        /// Keputusan developer (7 November 2026): kecepatan tambahan dari luar (tarikan jerat Tribal, dorongan) yang memudar
        /// linier ke 0 selama durationSeconds, ditambahkan ke controller.Move. Tafsiran: panggilan baru menggantikan dorongan
        /// yang sedang berjalan (tidak ditumpuk), supaya pemanggil per frame tidak membuat kecepatan terus bertambah.
        /// </summary>
        public void AddExternalVelocity(Vector3 extraVelocity, float durationSeconds)
        {
            if (durationSeconds <= 0f) { ClearExternalVelocity(); return; }
            externalStartVelocity = extraVelocity;
            externalDuration = durationSeconds;
            externalElapsed = 0f;
        }

        /// <summary>Hapus dorongan luar. Dipanggil Teleport, saat mati, dan saat pemanggil berhenti menarik.</summary>
        public void ClearExternalVelocity()
        {
            externalStartVelocity = Vector3.zero;
            externalDuration = 0f;
            externalElapsed = 0f;
        }

        private Vector3 TickExternalVelocity(float dt)
        {
            if (externalDuration <= 0f) return Vector3.zero;
            float k = 1f - externalElapsed / externalDuration;
            externalElapsed += dt;
            if (externalElapsed >= externalDuration) ClearExternalVelocity();
            return k > 0f ? externalStartVelocity * k : Vector3.zero;
        }

        private bool CheckGrounded(TraversalBalance t)
        {
            if (controller.isGrounded) return true;
            if (velocity.y > 0.1f) return false;
            Vector3 origin = transform.position + Vector3.up * (controller.radius + 0.05f);
            float dist = controller.radius + 0.05f + t.groundCheckDistance;
            return PhysicsQuery.SphereCast(new Ray(origin, Vector3.down), controller.radius * 0.9f, out _, dist - controller.radius * 0.9f, PhysicsMasks.Ground);
        }

        // ---------------------------------------------------------------- Pengali kecepatan

        public void SetSpeedModifier(SpeedModifierId id, float multiplier)
        {
            speedModifiers[(int)id] = Mathf.Max(0f, multiplier);
            RecalculateMultiplier();
        }

        public void ClearSpeedModifier(SpeedModifierId id)
        {
            speedModifiers[(int)id] = 1f;
            RecalculateMultiplier();
        }

        private void RecalculateMultiplier()
        {
            // Pengali bertumpuk tanpa berkurang berulang. Contoh: barang 0,5 x badai 0,75 = 0,375.
            float m = 1f;
            for (int i = 0; i < speedModifiers.Length; i++) m *= speedModifiers[i];
            SpeedMultiplier = Mathf.Max(0.1f, m);
        }

        // ---------------------------------------------------------------- Teleport

        /// <summary>
        /// Pindah tanpa damage jatuh. Pemanggil seperti ProgressionSafety (#116) lalu memanggil PlayerTraversal.ResetFallTracking.
        /// </summary>
        public void Teleport(Vector3 position)
        {
            bool wasEnabled = controller.enabled;
            controller.enabled = false;
            transform.position = position;
            controller.enabled = wasEnabled;
            velocity = Vector3.zero;
            ClearExternalVelocity();
            if (traversal != null) traversal.ResetFallTracking();
            // Langkah 6 (ZoneManager.md): hitung ulang zona, karena OnTriggerExit tidak dijamin terkirim.
            ZoneManager.ResyncAll(position);
        }

        /// <summary>Dorongan sesaat (lepas dari tangga dengan Space, #101).</summary>
        public void AddImpulse(Vector3 impulse)
        {
            velocity.y += impulse.y;
            if (controller.enabled) controller.Move(new Vector3(impulse.x, 0f, impulse.z) * Time.deltaTime);
        }

        // ---------------------------------------------------------------- Save (#80)

        public string SaveId => SaveIdConst;
        public int LoadOrder => 80;

        public string CaptureState()
        {
            var cam = FirstPersonCamera.Instance;
            return JsonUtility.ToJson(new PlayerTransformSaveData
            {
                position = traversal != null ? traversal.GetSafeSavePosition() : transform.position,
                yaw = cam != null ? cam.Yaw : transform.eulerAngles.y,
                pitch = cam != null ? cam.Pitch : 0f,
            });
        }

        public void RestoreState(string json)
        {
            var d = JsonUtility.FromJson<PlayerTransformSaveData>(json);
            Teleport(d.position);
            if (FirstPersonCamera.Instance != null) FirstPersonCamera.Instance.SetYawPitch(d.yaw, d.pitch);
            else transform.rotation = Quaternion.Euler(0f, d.yaw, 0f);
        }

        public void OnRestoreFinished() { }
    }
}
