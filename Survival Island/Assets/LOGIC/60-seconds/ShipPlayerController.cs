using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Level kontrol pemain Fase 1. Pemilik menurut GLOSSARY.md: ShipPlayerController (#74).</summary>
    public enum ShipControlLevel { None, LookOnly, Full }

    /// <summary>
    /// Gerak pemain Fase 1 dan level kontrol. Blueprint: 60-seconds/ShipPlayerController.md (#17, #18, #74, #75, #83, #102).
    /// GDD bagian 1: first-person. GDD bagian 3: kapal miring dan dipenuhi air.
    /// Usulan disetujui (#85): di kapal tidak ada lari, lompat, dan jongkok. Kecepatan jalan tetap ship.moveSpeed (5 m/s).
    ///
    /// Kamera (FirstPersonCamera), interaksi (PlayerInteractor), dan dorongan (ShipShove) berada di komponen lain.
    /// Kursor tidak ditulis di sini, karena diatur UIFocusStack.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class ShipPlayerController : MonoBehaviour
    {
        private static readonly ShipBalance FallbackShip = new ShipBalance();

        [SerializeField] private CharacterController controller;
        [SerializeField] private PlayerInteractor interactor;
        [SerializeField] private FirstPersonCamera fpCamera;
        [SerializeField] private ShipShove shove;

        public ShipControlLevel ControlLevel { get; private set; } = ShipControlLevel.None;

        /// <summary>Arah gerak dunia, panjang 0 sampai 1. Dibaca ShipHazardManager.ComputeTiltMul.</summary>
        public Vector3 MoveDirectionWorld { get; private set; }

        /// <summary>Dibaca ShipHazardManager dan ShipCorridor (pindah pemain lewat controller.Move).</summary>
        public CharacterController Controller => controller;

        private float waterMul = 1f;
        private float tiltMul = 1f;
        private Vector3 drift;
        private float verticalVelocity;

        private static ShipBalance Ship
        {
            get
            {
                var b = GameManager.CurrentBalance;
                return b != null ? b.ship : FallbackShip;
            }
        }

        private void Awake()
        {
            if (controller == null) controller = GetComponent<CharacterController>();
            if (interactor == null) interactor = GetComponent<PlayerInteractor>();
            if (fpCamera == null) fpCamera = GetComponentInChildren<FirstPersonCamera>();
            if (shove == null) shove = GetComponent<ShipShove>();

            if (controller == null) Debug.LogError("[ShipPlayerController] CharacterController kosong.", this);
            if (interactor == null) Debug.LogError("[ShipPlayerController] PlayerInteractor kosong.", this);
            if (fpCamera == null) Debug.LogError("[ShipPlayerController] FirstPersonCamera kosong.", this);
            if (shove == null) Debug.LogError("[ShipPlayerController] ShipShove kosong.", this);
        }

        private void Start()
        {
            // Samakan kamera dan interaksi dengan nilai awal sebelum LifeboatExtraction membuka LookOnly.
            SetControlLevel(ShipControlLevel.None);
        }

        /// <summary>Dipanggil LifeboatExtraction. None: diam. LookOnly: pra-game. Full: selama Scavenge.</summary>
        public void SetControlLevel(ShipControlLevel level)
        {
            ControlLevel = level;
            // Kamera tidak dimatikan, hanya input menengok, supaya sway dan getaran FinalShudder tetap tampil.
            if (fpCamera != null) fpCamera.SetLookEnabled(level != ShipControlLevel.None);
            if (interactor != null) interactor.SetEnabled(level == ShipControlLevel.Full);
            if (level != ShipControlLevel.Full) MoveDirectionWorld = Vector3.zero;
        }

        /// <summary>Dipanggil ShipHazardManager.UpdatePlayerEffects tiap frame. Berlaku pada Update berikutnya.</summary>
        public void SetShipMoveModifiers(float waterMul, float tiltMul, Vector3 drift)
        {
            this.waterMul = Mathf.Clamp01(waterMul);
            this.tiltMul = Mathf.Max(0f, tiltMul);
            this.drift = new Vector3(drift.x, 0f, drift.z);
        }

        private void Update()
        {
            if (controller == null || !controller.enabled) return;
            var ship = Ship;

            // 1. Gravitasi di semua level, supaya pemain tidak melayang.
            if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = ship.groundedStickVelocity;
            else verticalVelocity += ship.gravity * Time.deltaTime;

            // 2. Gerak horizontal hanya pada Full. Geser drift juga hanya pada Full.
            Vector3 horizontal = Vector3.zero;
            var input = InputManager.Instance;
            if (ControlLevel == ShipControlLevel.Full && input != null)
            {
                // ClampMagnitude: gerak diagonal tidak lebih cepat dari moveSpeed.
                Vector2 move = Vector2.ClampMagnitude(input.MoveInput, 1f);
                MoveDirectionWorld = transform.right * move.x + transform.forward * move.y;
                horizontal = MoveDirectionWorld * (ship.moveSpeed * waterMul * tiltMul) + drift;
            }

            // 3. Satu Move per frame, supaya isGrounded akurat.
            controller.Move((horizontal + Vector3.up * verticalVelocity) * Time.deltaTime);

            // 4. Dorong: PrimaryPressed, bukan Held, jadi tidak beruntun (#75).
            if (ControlLevel == ShipControlLevel.Full && input != null && input.PrimaryPressed && shove != null) shove.TryShove();
        }

        private void OnDisable()
        {
            // DeathCinematic.DisableController mematikan kelas ini sebelum CharacterController (#53).
            verticalVelocity = 0f;
            MoveDirectionWorld = Vector3.zero;
            drift = Vector3.zero;
        }
    }
}
