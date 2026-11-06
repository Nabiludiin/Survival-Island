using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Senjata api pemain (Base_Pistol, Laser Pistol). Blueprint: left-4-dead/GunController.md (#58).
    /// GDD bagian 3: Pistol item awal dan peluru prioritas loot. Claude.md Pilar 3: Raycast Shooting dengan Muzzle Flash,
    /// headshot 2x. Input datang dari EquipmentManager (PrimaryAction), reload dari InputManager.ReloadPressed.
    /// Isi magazine di ItemInstance.magazineAmmo; cadangan = jumlah ammoItemId di InventoryManager.
    ///
    /// Catatan Langkah 5 (TODO): DurabilitySystem (#113) belum ada, jadi status Broken dan keausan dilewati;
    /// WeaponStatCalculator (#113) belum ada (statistik dari WeaponData); VFXManager (#15) belum ada, jadi muzzle flash
    /// memakai ParticleSystem di viewmodel (tanpa Instantiate); AnimationController (#12) dan SFX belum ada;
    /// NPCGunUser (#76) belum ditulis. Radius suara memakai WeaponData.noiseRadius (lihat GunController.md Catatan Implementasi).
    /// </summary>
    public class GunController : EquippableBehaviour
    {
        [SerializeField] private ParticleSystem muzzleFlash;   // Di layer PlayerViewmodel
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip fireClip, dryClip, reloadClip;

        private readonly GunCore core = new GunCore();
        private WeaponData weapon;
        private ItemInstance instance;
        private bool isReloading;
        private float reloadEndTime;
        private float autoReloadTime = -1f;
        private float nextDryClickTime;
        private bool wasSprinting;

        public bool IsReloading => isReloading;
        public float CurrentSpread => core.CurrentSpread;

        // Tembakan terjadi seketika, jadi pergantian selalu boleh (EquipmentManager.md).
        public override bool CanSwitchNow => true;

        private void OnEnable()
        {
            EventChannel<InventoryChangedEvent>.Raised += HandleInventoryChanged;
            EventChannel<PlayerPinnedEvent>.Raised += HandlePinned;
        }

        private void OnDisable()
        {
            EventChannel<InventoryChangedEvent>.Raised -= HandleInventoryChanged;
            EventChannel<PlayerPinnedEvent>.Raised -= HandlePinned;
        }

        public override void OnEquip(ItemInstance inst)
        {
            base.OnEquip(inst);
            instance = inst;
            var item = inst != null ? ItemDatabase.Get(inst.itemId) : null;
            weapon = item != null ? item.weaponData : null;
            if (weapon == null) { Debug.LogError($"[GunController] Item '{inst?.itemId}' tidak punya WeaponData."); return; }
            RecalculateStats();
            // magazineAmmo -1: isi startingMagazine (Pistol run baru penuh 12, cadangan 0, usulan disetujui #85).
            if (instance != null && instance.magazineAmmo < 0) instance.magazineAmmo = weapon.startingMagazine;
            isReloading = false;
            autoReloadTime = -1f;
            wasSprinting = false;
            PublishAmmo();
        }

        public override void OnUnequip()
        {
            CancelReload();   // Ganti senjata membatalkan reload; peluru tidak berkurang
            instance = null;
            weapon = null;
            base.OnUnequip();
        }

        /// <summary>Statistik dari WeaponData. TODO #113: WeaponStatCalculator.Get (upgrade dan modifier Lucky Machine).</summary>
        public void RecalculateStats()
        {
            if (weapon != null) core.SetStats(WeaponStats.From(weapon));
        }

        public override void PrimaryAction(ActionPhase phase)
        {
            if (weapon == null || instance == null) return;
            // 1. SemiAuto hanya Pressed; Auto juga Held.
            if (phase == ActionPhase.Released) return;
            if (weapon.fireMode == FireMode.SemiAuto && phase != ActionPhase.Pressed) return;
            // 2. TODO #113: DurabilitySystem.GetState(instance) == Broken -> klik kosong + notifikasi "Pistol rusak" (#22).
            // 3.
            if (isReloading) return;
            // 4. Klik kosong tidak mengirim Noise Sphere.
            if (instance.magazineAmmo <= 0) { DryClick(); return; }
            // 5. Ray dari tengah kamera dunia, jadi recoil benar-benar menggeser bidikan.
            var fpc = FirstPersonCamera.Instance;
            var cam = fpc != null ? fpc.WorldCamera : null;
            if (cam == null) return;
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            // 6.
            GameObject shooter = IslandPlayerController.Instance != null ? IslandPlayerController.Instance.gameObject : gameObject;
            if (!core.Fire(in ray, shooter, instance.itemId, true)) return;
            // 7. TODO #113: DurabilitySystem.ApplyWear(instance, wearPerShot).
            instance.magazineAmmo--;
            // 8. Muzzle flash dari partikel viewmodel (Claude.md Pilar 3).
            if (muzzleFlash != null) muzzleFlash.Play(true);
            if (audioSource != null && fireClip != null) audioSource.PlayOneShot(fireClip);
            // 9.
            if (fpc != null) fpc.AddRecoil(weapon.recoilPitch, Random.Range(-weapon.recoilYaw, weapon.recoilYaw));
            // 10. Tembakan memancing zombie (#49).
            float noise = weapon.noiseRadius > 0f ? weapon.noiseRadius : (GameManager.CurrentBalance != null ? GameManager.CurrentBalance.stealth.noiseGunshot : 40f);
            NoiseEmitter.Emit(cam.transform.position, noise, NoiseSourceKind.Gunshot);
            // 11.
            PublishAmmo();
            EventChannel<WeaponFiredEvent>.Publish(new WeaponFiredEvent(instance.itemId, cam.transform.position, true));
            // 12. Reload otomatis.
            if (instance.magazineAmmo == 0 && GetReserve() > 0) ScheduleAutoReload();
        }

        private void DryClick()
        {
            var bal = GameManager.CurrentBalance;
            if (Time.time < nextDryClickTime) return;
            nextDryClickTime = Time.time + (bal != null ? bal.gun.dryClickCooldown : 0.25f);
            if (audioSource != null && dryClip != null) audioSource.PlayOneShot(dryClip);
            // Magazine kosong dan cadangan ada: reload otomatis. Cadangan kosong: TODO HUDManager berkedip.
            if (GetReserve() > 0) ScheduleAutoReload();
        }

        private void ScheduleAutoReload()
        {
            if (autoReloadTime >= 0f || isReloading) return;
            var bal = GameManager.CurrentBalance;
            autoReloadTime = Time.time + (bal != null ? bal.gun.autoReloadDelay : 0.3f);
        }

        private int GetReserve()
        {
            if (weapon == null) return 0;
            if (string.IsNullOrEmpty(weapon.ammoItemId)) return int.MaxValue;   // Laser Pistol
            var inv = InventoryManager.Instance;
            return inv != null ? inv.GetCount(weapon.ammoItemId) : 0;
        }

        public void StartReload()
        {
            // 1.
            if (weapon == null || instance == null || isReloading) return;
            if (instance.magazineAmmo >= core.stats.magazineSize || GetReserve() <= 0) return;
            // 2.
            isReloading = true;
            reloadEndTime = Time.time + core.stats.reloadSeconds;
            autoReloadTime = -1f;
            if (audioSource != null && reloadClip != null) audioSource.PlayOneShot(reloadClip);
        }

        private void FinishReload()
        {
            // Peluru baru berkurang di akhir reload, jadi reload yang batal tidak membuang apa pun.
            int take = Mathf.Min(core.stats.magazineSize - instance.magazineAmmo, GetReserve());
            if (take > 0 && !string.IsNullOrEmpty(weapon.ammoItemId) && InventoryManager.Instance != null)
                InventoryManager.Instance.RemoveItem(weapon.ammoItemId, take);
            instance.magazineAmmo += Mathf.Max(0, take);
            isReloading = false;
            PublishAmmo();
        }

        public void CancelReload()
        {
            isReloading = false;
            autoReloadTime = -1f;
        }

        private void Update()
        {
            if (weapon == null || instance == null) return;
            float dt = Time.deltaTime;
            if (dt <= 0f) return;
            var pc = IslandPlayerController.Instance;
            var bal = GameManager.CurrentBalance;

            // Spread: bergerak di atas movingSpeedThreshold.
            bool moving = false;
            if (pc != null)
            {
                Vector3 v = pc.Velocity; v.y = 0f;
                moving = v.magnitude > (bal != null ? bal.gun.movingSpeedThreshold : 0.5f);
            }
            core.TickSpread(moving, dt);

            // Mulai berlari membatalkan reload (usulan disetujui #85). Kena serangan tidak.
            bool sprinting = pc != null && pc.IsSprinting;
            if (sprinting && !wasSprinting && isReloading) CancelReload();
            wasSprinting = sprinting;

            var input = InputManager.Instance;
            bool locked = EquipmentManager.Instance != null && EquipmentManager.Instance.Locks != EquipLock.None;
            if (!locked && input != null && input.ReloadPressed) StartReload();
            if (!locked && autoReloadTime >= 0f && Time.time >= autoReloadTime) { autoReloadTime = -1f; StartReload(); }
            if (isReloading && Time.time >= reloadEndTime) FinishReload();
        }

        private void HandleInventoryChanged(InventoryChangedEvent e)
        {
            if (weapon != null && e.itemId == weapon.ammoItemId) PublishAmmo();
        }

        private void HandlePinned(PlayerPinnedEvent e)
        {
            if (e.isPinned) CancelReload();
        }

        private void PublishAmmo()
        {
            if (instance == null || weapon == null) return;
            int reserve = GetReserve();
            EventChannel<AmmoChangedEvent>.Publish(new AmmoChangedEvent(instance.magazineAmmo, reserve == int.MaxValue ? -1 : reserve));
        }
    }
}
