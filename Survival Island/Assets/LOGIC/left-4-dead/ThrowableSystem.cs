using System;
using UnityEngine;

namespace SurvivalIsland
{
    [Serializable]
    public class ThrowableSaveData
    {
        public int selectedIndex;
    }

    /// <summary>
    /// Barang lempar khas L4D: Molotov, Bom Rakitan, Batu, dan Suar. Blueprint: left-4-dead/ThrowableSystem.md (#96).
    /// GDD bagian 3: barang lempar untuk mengendalikan atau memancing kerumunan zombie. Bagian 7: memancing musuh ke kandang
    /// Lucky Machine. Usulan disetujui developer (#85): ketuk G memilih, tahan G membidik dengan garis perkiraan,
    /// lepas G melempar, klik kanan membatalkan.
    /// Manager di scene IslandPhase (Fase 1 tidak memakainya). ISaveable world_throwables (LoadOrder 60).
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - EquipmentManager belum punya API menurunkan viewmodel sebentar saat melempar (TODO #60).
    /// - Resep (rec_throw_*) adalah aset CraftingRecipe (#77); sakelar enabled = false tidak menyembunyikan resep (TODO #77).
    /// </summary>
    public class ThrowableSystem : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "world_throwables";

        public static ThrowableSystem Instance { get; private set; }

        [Tooltip("Urutan pilihan: Batu, Molotov, Bom Rakitan, Suar (aset ThrowableData).")]
        [SerializeField] private ThrowableData[] throwables = new ThrowableData[0];
        [Tooltip("Titik lepas di tangan. Kosong = depan kamera.")]
        [SerializeField] private Transform handPoint;
        [SerializeField] private LineRenderer arcLine;
        [SerializeField] private Transform landingMarker;

        public ThrowableData Selected => runtime != null && selectedIndex >= 0 && selectedIndex < runtime.Length ? runtime[selectedIndex] : null;

        private ThrowableData[] runtime;
        private int selectedIndex;
        private bool isAiming;
        private bool gHeld;
        private float gHeldTime;
        private float cooldownTimer;
        private Vector3[] arcPoints;

        private static ThrowableBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.throwable : null;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this) { Debug.LogError("[ThrowableSystem] Lebih dari satu instance."); enabled = false; return; }
            Instance = this;
            var b = Bal;
            arcPoints = new Vector3[b != null ? Mathf.Max(2, b.arcPointCount) : 30];   // Dibuat sekali
            if (arcLine != null) { arcLine.positionCount = 0; arcLine.enabled = false; }
            if (landingMarker != null) landingMarker.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (runtime != null) for (int i = 0; i < runtime.Length; i++) if (runtime[i] != null) Destroy(runtime[i]);
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            // Salinan aset: angka dari GameBalanceConfig.throwable disalin ke ThrowableData saat OnEnable.
            var b = Bal;
            if (runtime == null)
            {
                runtime = new ThrowableData[throwables != null ? throwables.Length : 0];
                for (int i = 0; i < runtime.Length; i++) runtime[i] = throwables[i] != null ? Instantiate(throwables[i]) : null;
            }
            for (int i = 0; i < runtime.Length; i++) if (runtime[i] != null) runtime[i].ApplyBalance(b);

            EventChannel<InventoryChangedEvent>.Raised += HandleInventory;
            EventChannel<UIFocusChangedEvent>.Raised += HandleFocus;
            EventChannel<PlayerPinnedEvent>.Raised += HandlePinned;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Register(this);
        }

        private void OnDisable()
        {
            EventChannel<InventoryChangedEvent>.Raised -= HandleInventory;
            EventChannel<UIFocusChangedEvent>.Raised -= HandleFocus;
            EventChannel<PlayerPinnedEvent>.Raised -= HandlePinned;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            CancelAim();
        }

        private void Start()
        {
            PublishSelection();
        }

        // ---------------------------------------------------------------- Input

        private void Update()
        {
            var b = Bal;
            var input = InputManager.Instance;
            if (b == null || input == null || input.Actions == null) return;
            // Game dijeda: Time.deltaTime 0, cooldown dan bidikan berhenti.
            float dt = Time.deltaTime;
            if (dt <= 0f) return;
            if (cooldownTimer > 0f) cooldownTimer -= dt;
            // Sakelar uji enabled = false: Throw diabaikan.
            if (!b.enabled) { CancelAim(); return; }
            // Diterkam, membawa Heavy Item, berenang, memanjat, atau menu: locks tidak None, bidikan batal.
            var eq = EquipmentManager.Instance;
            if (eq != null && eq.Locks != EquipLock.None) { CancelAim(); gHeld = false; return; }

            var g = input.Actions.Gameplay.Throw;
            if (g.WasPressedThisFrame()) { gHeld = true; gHeldTime = 0f; }
            if (gHeld && g.IsPressed())
            {
                gHeldTime += dt;
                if (!isAiming && gHeldTime >= b.holdToAimSeconds) BeginAim();
            }
            if (isAiming && input.SecondaryHeld) { CancelAim(); gHeld = false; return; }
            if (gHeld && g.WasReleasedThisFrame())
            {
                gHeld = false;
                if (isAiming) { Throw(); CancelAim(); }
                else if (gHeldTime < b.holdToAimSeconds) SelectNext();
            }
            if (isAiming) UpdateArc();
        }

        /// <summary>Pindah ke jenis berikutnya yang jumlahnya lebih dari 0.</summary>
        public void SelectNext()
        {
            if (runtime == null || runtime.Length == 0) return;
            var inv = InventoryManager.Instance;
            for (int step = 1; step <= runtime.Length; step++)
            {
                int i = (selectedIndex + step) % runtime.Length;
                if (runtime[i] == null) continue;
                if (inv == null || inv.GetCount(runtime[i].itemId) > 0) { selectedIndex = i; break; }
            }
            PublishSelection();
        }

        private void BeginAim()
        {
            if (Selected == null) return;
            isAiming = true;
            if (arcLine != null) arcLine.enabled = true;
        }

        public void CancelAim()
        {
            isAiming = false;
            if (arcLine != null) { arcLine.enabled = false; arcLine.positionCount = 0; }
            if (landingMarker != null) landingMarker.gameObject.SetActive(false);
        }

        // ---------------------------------------------------------------- Garis perkiraan

        private Vector3 HandPosition(Camera cam) => handPoint != null ? handPoint.position : cam.transform.position + cam.transform.forward * 0.5f;

        private Vector3 InitialVelocity(Camera cam, ThrowableData d)
        {
            return cam.transform.forward * d.throwForce + Vector3.up * d.upwardBias;
        }

        private void UpdateArc()
        {
            var b = Bal;
            var d = Selected;
            var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : null;
            if (d == null || cam == null || b == null) return;
            // 1.
            Vector3 v0 = InitialVelocity(cam, d);
            Vector3 p0 = HandPosition(cam);
            int count = arcPoints.Length;
            int used = count;
            bool landed = false;
            Vector3 landPoint = default, landNormal = Vector3.up;
            arcPoints[0] = p0;
            for (int i = 1; i < count; i++)
            {
                // 2. p = p0 + v0 t + 0,5 g t^2
                float t = i * b.arcTimeStep;
                Vector3 p = p0 + v0 * t + 0.5f * Physics.gravity * t * t;
                // 3. Ruas diperiksa terhadap tanah.
                Vector3 seg = p - arcPoints[i - 1];
                float len = seg.magnitude;
                if (len > 0.0001f && PhysicsQuery.Raycast(new Ray(arcPoints[i - 1], seg / len), out RaycastHit hit, len, PhysicsMasks.Ground))
                {
                    arcPoints[i] = hit.point;
                    used = i + 1;
                    landed = true;
                    landPoint = hit.point;
                    landNormal = hit.normal;
                    break;
                }
                arcPoints[i] = p;
            }
            // 4. Tanpa alokasi: array dipakai ulang.
            if (arcLine != null)
            {
                arcLine.positionCount = used;
                arcLine.SetPositions(arcPoints);
            }
            if (landingMarker != null)
            {
                landingMarker.gameObject.SetActive(landed);
                if (landed) landingMarker.SetPositionAndRotation(landPoint + landNormal * 0.02f, Quaternion.FromToRotation(Vector3.up, landNormal));
            }
        }

        // ---------------------------------------------------------------- Melempar

        private void Throw()
        {
            var b = Bal;
            var d = Selected;
            var inv = InventoryManager.Instance;
            var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : null;
            // 1.
            if (b == null || d == null || inv == null || cam == null || cooldownTimer > 0f || !inv.HasItem(d.itemId, 1)) return;
            var pooler = ObjectPooler.Instance;
            if (pooler == null || !pooler.HasPool(d.poolTag)) { Debug.LogWarning($"[ThrowableSystem] Pool '{d.poolTag}' tidak ada."); return; }
            Vector3 v0 = InitialVelocity(cam, d);
            Vector3 pos = HandPosition(cam);
            // 3. Proyektil dulu, supaya barang tidak hilang kalau pool penuh.
            var projectile = pooler.SpawnFromPool<ThrowableProjectile>(d.poolTag, pos, Quaternion.LookRotation(v0.sqrMagnitude > 0.001f ? v0 : cam.transform.forward));
            if (projectile == null) return;
            // 2.
            inv.RemoveItem(d.itemId, 1);
            // 4.
            var player = IslandPlayerController.Instance;
            var playerCollider = player != null ? player.GetComponent<Collider>() : null;
            projectile.Launch(d, v0, playerCollider);
            cooldownTimer = b.throwCooldownSeconds;
            // TODO(EquipmentManager): turunkan viewmodel item aktif sebentar, lalu naikkan lagi (#60).
        }

        // ---------------------------------------------------------------- Event

        private void HandleInventory(InventoryChangedEvent e)
        {
            if (runtime == null) return;
            for (int i = 0; i < runtime.Length; i++)
            {
                if (runtime[i] == null || runtime[i].itemId != e.itemId) continue;
                // Pilihan kosong: pindah ke jenis lain yang masih ada.
                var sel = Selected;
                if (sel != null && sel.itemId == e.itemId && e.total <= 0) { SelectNext(); return; }
                PublishSelection();
                return;
            }
        }

        private void HandleFocus(UIFocusChangedEvent e)
        {
            if (e.isGameplayBlocked) { CancelAim(); gHeld = false; }
        }

        private void HandlePinned(PlayerPinnedEvent e)
        {
            if (e.isPinned) { CancelAim(); gHeld = false; }
        }

        private void HandleRunEnded(RunEndedEvent e)
        {
            CancelAim();
            ThrowableProjectile.ExtinguishAllLures();
        }

        /// <summary>HUD: ikon dan jumlah dekat tampilan peluru, disembunyikan kalau semua jenis 0 (HUDManager).</summary>
        private void PublishSelection()
        {
            var d = Selected;
            var b = Bal;
            var inv = InventoryManager.Instance;
            if (d == null || (b != null && !b.enabled)) { EventChannel<ThrowableChangedEvent>.Publish(new ThrowableChangedEvent(string.Empty, 0)); return; }
            int count = inv != null ? inv.GetCount(d.itemId) : 0;
            EventChannel<ThrowableChangedEvent>.Publish(new ThrowableChangedEvent(d.itemId, count));
        }

        /// <summary>Jumlah semua jenis barang lempar di inventori (HUD menyembunyikan ikon saat 0).</summary>
        public int TotalCount()
        {
            var inv = InventoryManager.Instance;
            if (inv == null || runtime == null) return 0;
            int n = 0;
            for (int i = 0; i < runtime.Length; i++) if (runtime[i] != null) n += inv.GetCount(runtime[i].itemId);
            return n;
        }

        // ---------------------------------------------------------------- Save (#80)

        public string SaveId => SaveIdConst;
        public int LoadOrder => 60;

        public string CaptureState() => JsonUtility.ToJson(new ThrowableSaveData { selectedIndex = selectedIndex });

        public void RestoreState(string json)
        {
            var d = string.IsNullOrEmpty(json) ? null : JsonUtility.FromJson<ThrowableSaveData>(json);
            if (d == null) return;
            int max = runtime != null ? runtime.Length : 0;
            selectedIndex = max > 0 ? Mathf.Clamp(d.selectedIndex, 0, max - 1) : 0;
        }

        public void OnRestoreFinished() => PublishSelection();
    }
}
