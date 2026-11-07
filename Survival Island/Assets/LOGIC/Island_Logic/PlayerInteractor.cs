using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Raycast interaksi dari tengah kamera, tombol E. Dipakai pemain Fase 1 dan Fase 2.
    /// Blueprint: Island_Logic/IslandInteractable.md bagian 2 (#17). Claude.md Pilar 1: raycast dari tengah kamera
    /// dengan LayerMask Interactable, tekan E untuk mengambil loot atau NPC.
    ///
    /// Sistem pendukung yang disambung:
    /// - HUDManager.ShowInteraction/HideInteraction/SetInteractionProgress (prompt jadi juga tetap ada di CurrentPrompt).
    /// - LocalizationManager kunci prompt.format (#104), cadangan "[{tombol}] {aksi}" di FormatPrompt.
    /// - NotificationManager (#22) dan suara tolak (AudioManager) saat CanInteract menolak.
    /// - Batal tahan saat PlayerDamagedEvent.
    /// IsCarryingHeavyItem dibaca dari HeavyItemSystem.IsCarrying (disambungkan Langkah 6, #69).
    /// </summary>
    public class PlayerInteractor : MonoBehaviour
    {
        private static readonly InteractBalance FallbackInteract = new InteractBalance();

        [Tooltip("Asal raycast: WorldCamera dari rig FirstPersonCamera (#102).")]
        [SerializeField] private Transform cameraTransform;
        [Tooltip("Untuk cek kecepatan pemain saat interaksi tahan. Boleh kosong.")]
        [SerializeField] private CharacterController characterController;
        [Tooltip("Suara saat interaksi ditolak (#22). Boleh kosong.")]
        [SerializeField] private AudioClip rejectClip;

        public IInteractable Current { get; private set; }
        public bool IsEnabled { get; private set; } = true;

        /// <summary>Teks prompt jadi, misalnya "[E] Ambil Makanan Kaleng". Kosong kalau tidak ada target.</summary>
        public string CurrentPrompt { get; private set; } = string.Empty;

        /// <summary>Kemajuan interaksi tahan, 0 sampai 1.</summary>
        public float HoldProgress { get; private set; }

        private float holdTimer;
        private float lostTargetTimer;
        private float scanTimer;
        private string lastActionText;
        private bool requireRelease;   // Setelah interaksi berhasil, tombol harus dilepas dulu (holdTimer direset)

        private static InteractBalance Interact
        {
            get
            {
                var b = GameManager.CurrentBalance;
                return b != null ? b.interact : FallbackInteract;
            }
        }

        private void Awake()
        {
            if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform;
            if (characterController == null) characterController = GetComponent<CharacterController>();
            if (cameraTransform == null) Debug.LogError("[PlayerInteractor] cameraTransform kosong.", this);
        }

        private void OnEnable()
        {
            EventChannel<PlayerDamagedEvent>.Raised += HandleDamaged;
            EventChannel<InputBindingsChangedEvent>.Raised += HandleBindingsChanged;
        }

        private void OnDisable()
        {
            EventChannel<PlayerDamagedEvent>.Raised -= HandleDamaged;
            EventChannel<InputBindingsChangedEvent>.Raised -= HandleBindingsChanged;
            ClearTarget();
        }

        // Kena serangan membatalkan interaksi tahan.
        private void HandleDamaged(PlayerDamagedEvent e)
        {
            if (holdTimer > 0f) ResetHold();
        }

        // Tombol berganti: teks prompt dibangun ulang dengan nama tombol baru.
        private void HandleBindingsChanged(InputBindingsChangedEvent e)
        {
            lastActionText = null;
            RefreshPrompt();
        }

        /// <summary>Notifikasi alasan penolakan (#22) dan suara tolak.</summary>
        private void NotifyBlocked(InteractBlockReason reason)
        {
            if (AudioManager.Instance != null && rejectClip != null) AudioManager.Instance.PlaySFX(rejectClip);
            var nm = NotificationManager.Instance;
            if (nm == null) return;
            switch (reason)
            {
                case InteractBlockReason.InventoryFull: nm.Show("notif.inventory_full", NotificationKind.Warning); break;         // "Inventori penuh"
                case InteractBlockReason.HandsFull:
                case InteractBlockReason.CarryingHeavyItem: nm.Show("notif.hands_full", NotificationKind.Warning); break;         // "Tangan penuh"
                case InteractBlockReason.MissingTool: nm.Show("notif.need_tool", NotificationKind.Warning); break;                // "Butuh alat lain"
                case InteractBlockReason.Locked: nm.Show("notif.locked", NotificationKind.Info); break;                           // "Terkunci"
            }
        }

        /// <summary>Dipanggil ShipPlayerController (level kontrol), #110, dan #101. false membersihkan target dan prompt.</summary>
        public void SetEnabled(bool value)
        {
            IsEnabled = value;
            if (!value) ClearTarget();
        }

        private void Update()
        {
            if (!IsEnabled) return;

            // 1. Panel terbuka: prompt hilang.
            var focus = UIFocusStack.Instance;
            if (focus != null && focus.IsGameplayInputBlocked)
            {
                ClearTarget();
                return;
            }

            // 2. Target hilang atau tidak tersedia (objek pool kembali, region dilepas).
            if (Current != null && !IsStillValid(Current)) ClearTarget();

            // 3. Pindai setiap scanIntervalSeconds.
            var cfg = Interact;
            scanTimer -= Time.deltaTime;
            if (scanTimer <= 0f)
            {
                scanTimer = cfg.scanIntervalSeconds;
                IInteractable found = ScanForTarget(cfg);
                if (found != null)
                {
                    if (!ReferenceEquals(found, Current))
                    {
                        if (Current != null && !(Current is Component old && old == null)) Current.OnHoverExit();
                        Current = found;
                        Current.OnHoverEnter();
                        holdTimer = 0f;
                        lastActionText = null;
                    }
                    lostTargetTimer = 0f;
                }
                else if (Current != null)
                {
                    lostTargetTimer += cfg.scanIntervalSeconds;
                    if (lostTargetTimer > cfg.targetGraceSeconds && holdTimer <= 0f) ClearTarget();
                }
            }

            // 4. Prompt.
            RefreshPrompt();

            // 5. Tombol.
            HandleInput(cfg);
        }

        private IInteractable ScanForTarget(InteractBalance cfg)
        {
            if (cameraTransform == null) return null;
            var ray = new Ray(cameraTransform.position, cameraTransform.forward);
            if (!PhysicsQuery.SphereCast(ray, cfg.sphereRadius, out RaycastHit hit, cfg.range, PhysicsMasks.Interact)) return null;

            // Mask Interact tidak memuat dinding, jadi garis pandang diperiksa terpisah (#87).
            if (PhysicsQuery.Raycast(ray, out RaycastHit _, hit.distance, PhysicsMasks.InteractBlock)) return null;

            var target = hit.collider.GetComponentInParent<IInteractable>();
            return target != null && target.IsAvailable ? target : null;
        }

        private void HandleInput(InteractBalance cfg)
        {
            var input = InputManager.Instance;
            if (Current == null || input == null)
            {
                ResetHold();
                return;
            }

            // Contoh: setor di sekoci lalu tetap menahan E tidak langsung berangkat. Tahan sekali lagi (LifeboatExtraction.md).
            if (requireRelease)
            {
                if (input.InteractHeld) return;
                requireRelease = false;
            }

            float hold = Current.HoldSeconds;
            if (hold <= 0f)
            {
                if (input.InteractPressed) TryInteract(cfg);
                return;
            }

            if (!input.InteractHeld)
            {
                ResetHold();
                return;
            }

            // Interaksi tahan batal kalau pemain berlari atau terlalu jauh.
            // Batal juga saat PlayerDamagedEvent terkirim (HandleDamaged).
            if (characterController != null && characterController.velocity.magnitude > cfg.moveCancelSpeed)
            {
                ResetHold();
                return;
            }

            holdTimer += Time.deltaTime;
            HoldProgress = Mathf.Clamp01(holdTimer / hold);
            if (HUDManager.Instance != null) HUDManager.Instance.SetInteractionProgress(HoldProgress);
            if (holdTimer >= hold)
            {
                TryInteract(cfg);
                ResetHold();
            }
        }

        private void TryInteract(InteractBalance cfg)
        {
            if (Current == null) return;

            // 1. Target yang bertahan karena targetGraceSeconds bisa sudah tertinggal.
            if (Current is Component c && c != null && cameraTransform != null)
            {
                Vector3 closest = c.transform.position;
                var col = c.GetComponentInChildren<Collider>();
                if (col != null) closest = col.ClosestPoint(cameraTransform.position);
                if ((closest - cameraTransform.position).sqrMagnitude > cfg.holdLoseRange * cfg.holdLoseRange)
                {
                    ClearTarget();
                    return;
                }
            }

            // 2. Ditolak: notifikasi sesuai alasan (#22) dan suara tolak.
            var ctx = BuildContext();
            var block = Current.CanInteract(ctx);
            if (block != InteractBlockReason.None) { NotifyBlocked(block); return; }

            // 3-4. Simpan ID sebelum Interact, karena target pool bisa langsung kembali ke pool.
            string targetId = GetTargetId(Current);
            if (Current.Interact(ctx))
            {
                requireRelease = true;
                lastActionText = null;
                RefreshPrompt();
                EventChannel<InteractedEvent>.Publish(new InteractedEvent(targetId));
            }
        }

        private InteractContext BuildContext()
        {
            // Langkah 6 (#69): isCarryingHeavyItem dari HeavyItemSystem.
            bool carrying = HeavyItemSystem.Instance != null && HeavyItemSystem.Instance.IsCarrying;
            return new InteractContext(InventoryManager.Instance, transform, carrying);
        }

        private static string GetTargetId(IInteractable target)
        {
            if (target is ShipInteractable ship) return ship.ItemID;
            return target is Component comp && comp != null ? comp.gameObject.name : string.Empty;
        }

        private static bool IsStillValid(IInteractable target)
        {
            // Objek Unity yang sudah dihancurkan atau nonaktif dianggap hilang.
            if (target is Component comp && (comp == null || !comp.gameObject.activeInHierarchy)) return false;
            return target.IsAvailable;
        }

        private void RefreshPrompt()
        {
            if (Current == null)
            {
                if (CurrentPrompt.Length > 0)
                {
                    CurrentPrompt = string.Empty;
                    lastActionText = null;
                    if (HUDManager.Instance != null) HUDManager.Instance.HideInteraction();
                }
                return;
            }

            // Teks hanya dibangun ulang kalau teks aksi berubah, supaya tidak ada garbage per frame.
            string action = Current.GetPrompt();
            if (ReferenceEquals(action, lastActionText)) return;
            lastActionText = action;
            CurrentPrompt = string.IsNullOrEmpty(action) ? string.Empty : FormatPrompt(action);
            if (HUDManager.Instance != null) HUDManager.Instance.ShowInteraction(CurrentPrompt);
        }

        private static string FormatPrompt(string action)
        {
            // Format dari LocalizationManager kunci prompt.format ("[{input:Interact}] {action}", #104). Cadangan di bawah.
            var loc = LocalizationManager.Instance;
            if (loc != null && loc.Has("prompt.format")) return loc.Format("prompt.format", "action", action);
            string key = InputManager.Instance != null ? InputManager.Instance.GetBindingDisplay(GameInputAction.Interact) : "E";
            return "[" + key + "] " + action;
        }

        private void ResetHold()
        {
            // Cincin HUD hanya dikosongkan kalau sebelumnya terisi, supaya tidak dipanggil tiap frame.
            if (HoldProgress > 0f && HUDManager.Instance != null) HUDManager.Instance.SetInteractionProgress(0f);
            holdTimer = 0f;
            HoldProgress = 0f;
        }

        private void ClearTarget()
        {
            // Objek yang sudah dihancurkan tidak dipanggil. Objek pool yang nonaktif tetap dipanggil, aman.
            if (Current != null && !(Current is Component comp && comp == null)) Current.OnHoverExit();
            Current = null;
            lostTargetTimer = 0f;
            ResetHold();
            RefreshPrompt();
        }
    }
}
