using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    [Flags]
    public enum EquipLock { None = 0, Menu = 1, HeavyItem = 2, Climbing = 4, Pinned = 8, Swimming = 16, Dead = 32 }

    public enum EquipState { Idle, Lowering, Raising }

    /// <summary>Simpanan hotbar (EquipmentManager.md).</summary>
    [Serializable]
    public struct EquipmentSaveData
    {
        public int[] hotbarInstanceIds;
        public int activeHotbarIndex;
        public int offHandInstanceId;
        public bool offHandActive;
        public bool defaultHotbarBuilt;   // Tambahan Langkah 3: supaya hotbar awal tidak dibangun ulang setelah load
    }

    /// <summary>
    /// Satu item aktif di tangan dan hotbar. Satu-satunya pembaca klik kiri dan klik kanan untuk item.
    /// Blueprint: Core_Logic/EquipmentManager.md (#60). GDD bagian 3 (Base_Kail, Base_Pistol, Base_Pisau), bagian 5 (alat dan obor),
    /// bagian 7 (senjata hadiah). Jumlah slot hotbar dan aturan tangan kiri adalah usulan yang disetujui developer (#85).
    ///
    /// Hotbar berisi RUJUKAN (instanceId), bukan salinan barang. Hanya di IslandPhase; Fase 1 tidak memakai kelas ini (#75).
    /// Belum ada di Langkah 3: cek DurabilitySystem.GetState == Broken (#113), notifikasi #22, suara klik slot kosong.
    /// </summary>
    public class EquipmentManager : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "player_equipment";
        public const int NoSlot = -1;
        private const int NoPending = -2;

        public static EquipmentManager Instance { get; private set; }

        [SerializeField] private EquippableBehaviour fistBehaviour;
        [Tooltip("Dipakai makanan, minuman, atau obat di hotbar yang tidak punya equipPrefab sendiri.")]
        [SerializeField] private ConsumableEquippable consumableBehaviour;
        [SerializeField] private InventoryManager inventory;
        [Header("Suara (#16)")]
        [Tooltip("Klik pada slot hotbar kosong.")]
        [SerializeField] private AudioClip emptySlotClip;
        [Tooltip("Suara patah saat klik kiri dengan item Broken (#113).")]
        [SerializeField] private AudioClip brokenClip;

        private int[] hotbar;
        private int activeHotbarIndex = 1;   // Slot Base_Pistol
        private int offHandInstanceId = NoSlot;
        private bool offHandActive;
        private EquippableBehaviour offHandBehaviour;
        private EquipState state = EquipState.Idle;
        private float switchTimer;
        private float briefLowerTimer;   // LowerBriefly (#96): viewmodel turun tanpa berganti item
        private int pendingIndex = NoPending;
        private int targetIndex = NoSlot;
        private int lastIndex = NoSlot;
        private EquipLock locks = EquipLock.None;
        private bool defaultHotbarBuilt;
        private bool registeredForSave;
        private readonly Dictionary<string, EquippableBehaviour> behaviourCache = new Dictionary<string, EquippableBehaviour>();

        private EquippableBehaviour activeBehaviour;

        public IEquippable Active => activeBehaviour;
        public int ActiveHotbarIndex => activeHotbarIndex;
        public int ActiveInstanceId { get; private set; } = NoSlot;
        /// <summary>instanceId item terakhir yang diturunkan. Dibaca ItemUseSystem untuk membatalkan pemakaian.</summary>
        public int LastUnequippedInstanceId { get; private set; } = NoSlot;
        public EquipState State => state;
        public EquipLock Locks => locks;
        public int OffHandInstanceId => offHandInstanceId;
        public bool OffHandActive => offHandActive;
        public int HotbarSlotCount => hotbar != null ? hotbar.Length : 0;
        public int GetHotbarInstanceId(int index) => hotbar != null && index >= 0 && index < hotbar.Length ? hotbar[index] : NoSlot;
        public ItemInstance ActiveInstance => inventory != null && ActiveInstanceId != NoSlot ? inventory.FindInstance(ActiveInstanceId) : null;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;
        private const EquipLock StashLocks = EquipLock.HeavyItem | EquipLock.Climbing | EquipLock.Swimming;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
            if (inventory == null) inventory = GetComponent<InventoryManager>();
            int count = Balance != null ? Balance.equipment.hotbarSlotCount : 8;
            hotbar = new int[Mathf.Max(1, count)];
            for (int i = 0; i < hotbar.Length; i++) hotbar[i] = NoSlot;
            if (fistBehaviour != null) fistBehaviour.gameObject.SetActive(false);
            if (consumableBehaviour != null) consumableBehaviour.gameObject.SetActive(false);
            SetActiveBehaviour(fistBehaviour, null, false);
        }

        private void OnEnable()
        {
            EventChannel<UIFocusChangedEvent>.Raised += HandleFocus;
            EventChannel<InventoryChangedEvent>.Raised += HandleInventoryChanged;
            EventChannel<HeavyItemChangedEvent>.Raised += HandleHeavyItem;
            EventChannel<PlayerPinnedEvent>.Raised += HandlePinned;
            EventChannel<PlayerDiedEvent>.Raised += HandleDied;
            if (EventChannel<UIFocusChangedEvent>.HasLast && EventChannel<UIFocusChangedEvent>.Last.isGameplayBlocked) locks |= EquipLock.Menu;
            if (WorldStateSave.Instance != null)
            {
                WorldStateSave.Instance.Register(this);
                registeredForSave = true;
            }
        }

        private void OnDisable()
        {
            EventChannel<UIFocusChangedEvent>.Raised -= HandleFocus;
            EventChannel<InventoryChangedEvent>.Raised -= HandleInventoryChanged;
            EventChannel<HeavyItemChangedEvent>.Raised -= HandleHeavyItem;
            EventChannel<PlayerPinnedEvent>.Raised -= HandlePinned;
            EventChannel<PlayerDiedEvent>.Raised -= HandleDied;
            if (registeredForSave && WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            registeredForSave = false;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Input

        private void Update()
        {
            if (state != EquipState.Idle) TickSwitch();   // Tetap selesai walau panel tanpa jeda terbuka
            TickBriefLower();
            if (locks != EquipLock.None) return;          // Kunci Menu mengikuti IsGameplayInputBlocked
            ReadHotbarInput();
            if (state != EquipState.Idle || activeBehaviour == null) return;

            var input = InputManager.Instance;
            if (input == null) return;
            if (input.FlashlightPressed) ToggleOffHand();   // Tombol F (#18), bukan lagi dibaca FlashlightController

            // Item Broken tidak bisa dipakai: klik kiri hanya memberi suara dan notifikasi, item tetap di hotbar (#113).
            if (DurabilitySystem.GetState(ActiveInstance) == DurabilityState.Broken)
            {
                if (input.PrimaryPressed) RejectBroken();
            }
            else if (input.PrimaryPressed) activeBehaviour.PrimaryAction(ActionPhase.Pressed);
            else if (input.PrimaryHeld) activeBehaviour.PrimaryAction(ActionPhase.Held);
            else if (input.PrimaryReleased) activeBehaviour.PrimaryAction(ActionPhase.Released);

            if (input.SecondaryPressed) activeBehaviour.SecondaryAction(ActionPhase.Pressed);
            else if (input.SecondaryHeld) activeBehaviour.SecondaryAction(ActionPhase.Held);
            else if (input.SecondaryReleased) activeBehaviour.SecondaryAction(ActionPhase.Released);
        }

        /// <summary>Klik kiri dengan item Broken (#113): suara patah dan notifikasi. Aksi item tidak dijalankan.</summary>
        private void RejectBroken()
        {
            AudioManager.Instance?.PlaySFX(brokenClip);
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.item_broken", NotificationKind.Warning);   // "Item rusak"
        }

        private void ReadHotbarInput()
        {
            var input = InputManager.Instance;
            if (input == null) return;
            int n = input.HotbarSelectPressed;
            if (n > 0)
            {
                RequestSlot(n - 1);
                return;
            }
            float scroll = input.HotbarScroll;
            if (Mathf.Abs(scroll) < 0.01f) return;
            int dir = scroll > 0f ? -1 : 1;
            int start = activeHotbarIndex < 0 ? (dir > 0 ? -1 : hotbar.Length) : activeHotbarIndex;
            for (int step = 1; step <= hotbar.Length; step++)
            {
                int i = ((start + dir * step) % hotbar.Length + hotbar.Length) % hotbar.Length;
                if (hotbar[i] != NoSlot)
                {
                    if (i != activeHotbarIndex) RequestSlot(i);
                    return;
                }
            }
        }

        // ---------------------------------------------------------------- Berganti item

        /// <summary>Memilih slot hotbar. Slot aktif ditekan lagi = tangan kosong. index -1 = tangan kosong.</summary>
        public void RequestSlot(int index)
        {
            if (locks != EquipLock.None) return;
            if (index >= hotbar.Length) return;
            if (index >= 0 && index == activeHotbarIndex && state == EquipState.Idle) index = NoSlot;
            if (index >= 0 && hotbar[index] == NoSlot)
            {
                // Tombol slot kosong: ditolak dengan suara klik (#21).
                AudioManager.Instance?.PlaySFX(emptySlotClip);
                return;
            }
            if (state != EquipState.Idle)
            {
                pendingIndex = index;
                return;
            }
            if (activeBehaviour != null && !activeBehaviour.CanSwitchNow)
            {
                pendingIndex = index;
                return;
            }
            BeginSwitch(index);
        }

        /// <summary>
        /// Viewmodel item aktif turun sebentar lalu naik lagi, tanpa berganti item (ThrowableSystem.md: saat melempar, #96).
        /// Lama turun memakai equipment.lowerSeconds. Diabaikan saat sedang berganti item atau ada kunci.
        /// </summary>
        public void LowerBriefly()
        {
            if (state != EquipState.Idle || locks != EquipLock.None) return;
            briefLowerTimer = Balance != null ? Balance.equipment.lowerSeconds : 0.15f;
            if (FirstPersonCamera.Instance != null) FirstPersonCamera.Instance.SetViewmodelMode(ViewmodelMode.Lowered);
        }

        private void TickBriefLower()
        {
            if (briefLowerTimer <= 0f) return;
            briefLowerTimer -= Time.deltaTime;
            if (briefLowerTimer > 0f) return;
            briefLowerTimer = 0f;
            // Pergantian item atau kunci simpan/mati yang mulai di tengah jalan sudah mengatur mode viewmodel sendiri.
            // Kunci Menu tidak mengubah viewmodel, jadi tetap dinaikkan.
            if (state == EquipState.Idle && (locks & (StashLocks | EquipLock.Dead)) == 0 && FirstPersonCamera.Instance != null)
                FirstPersonCamera.Instance.SetViewmodelMode(ViewmodelMode.Visible);
        }

        private void BeginSwitch(int index)
        {
            briefLowerTimer = 0f;
            LastUnequippedInstanceId = ActiveInstanceId;
            if (activeBehaviour != null) activeBehaviour.OnUnequip();
            targetIndex = index;
            state = EquipState.Lowering;
            switchTimer = Balance != null ? Balance.equipment.lowerSeconds : 0.15f;
            if (FirstPersonCamera.Instance != null) FirstPersonCamera.Instance.SetViewmodelMode(ViewmodelMode.Lowered);
        }

        private void TickSwitch()
        {
            // CanSwitchNow masih false: coba lagi pada frame berikutnya.
            if (state == EquipState.Idle) return;
            switchTimer -= Time.deltaTime;
            if (switchTimer > 0f) return;

            if (state == EquipState.Lowering)
            {
                activeHotbarIndex = targetIndex;
                var inst = activeHotbarIndex >= 0 && inventory != null ? inventory.FindInstance(hotbar[activeHotbarIndex]) : null;
                var behaviour = inst != null ? GetBehaviourFor(inst) : fistBehaviour;
                if (behaviour == null) { behaviour = fistBehaviour; inst = null; }
                SetActiveBehaviour(behaviour, inst, true);
                ApplyOffHandRule();
                if (FirstPersonCamera.Instance != null) FirstPersonCamera.Instance.SetViewmodelMode(ViewmodelMode.Visible);
                state = EquipState.Raising;
                switchTimer = Balance != null ? Balance.equipment.raiseSeconds : 0.2f;
                return;
            }

            // Raising selesai.
            state = EquipState.Idle;
            EventChannel<EquippedItemChangedEvent>.Publish(new EquippedItemChangedEvent(ActiveItemId()));
            if (pendingIndex != NoPending)
            {
                int next = pendingIndex;
                pendingIndex = NoPending;
                if (next != activeHotbarIndex) RequestSlot(next);
            }
        }

        private string ActiveItemId()
        {
            var inst = ActiveInstance;
            return inst != null ? inst.itemId : string.Empty;
        }

        private void SetActiveBehaviour(EquippableBehaviour behaviour, ItemInstance inst, bool callEquip)
        {
            if (activeBehaviour != null && activeBehaviour != behaviour) activeBehaviour.gameObject.SetActive(false);
            activeBehaviour = behaviour;
            ActiveInstanceId = inst != null ? inst.instanceId : NoSlot;
            if (activeBehaviour == null) return;
            activeBehaviour.gameObject.SetActive(true);
            if (callEquip) activeBehaviour.OnEquip(inst);
            if (FirstPersonCamera.Instance != null) FirstPersonCamera.Instance.SetMainPose(activeBehaviour.pose);
        }

        private EquippableBehaviour GetBehaviourFor(ItemInstance inst)
        {
            var b = GetBehaviour(inst.itemId);
            if (b != null) return b;
            var data = ItemDatabase.Get(inst.itemId);
            return data != null && data.isConsumable ? consumableBehaviour : null;
        }

        /// <summary>Viewmodel dibuat sekali per itemId, lalu dinyalakan dan dimatikan (tidak memakai ObjectPooler).</summary>
        private EquippableBehaviour GetBehaviour(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return null;
            if (behaviourCache.TryGetValue(itemId, out var cached) && cached != null) return cached;
            var data = ItemDatabase.Get(itemId);
            if (data == null || data.equipPrefab == null) return null;
            Transform parent = FirstPersonCamera.Instance != null && FirstPersonCamera.Instance.ViewmodelRoot != null
                ? FirstPersonCamera.Instance.ViewmodelRoot : transform;
            var created = Instantiate(data.equipPrefab, parent, false);
            created.gameObject.SetActive(false);
            behaviourCache[itemId] = created;
            return created;
        }

        /// <summary>Membaca ulang modifier dan skin (#72).</summary>
        public void RefreshEquipped()
        {
            if (activeBehaviour != null) activeBehaviour.Refresh();
            if (offHandBehaviour != null) offHandBehaviour.Refresh();
        }

        // ---------------------------------------------------------------- Hotbar

        /// <summary>Isi awal saat tiba di pulau: Kail, Pistol, Pisau, dengan Pistol aktif (EquipmentManager.md).</summary>
        public void BuildDefaultHotbar()
        {
            if (inventory == null) return;
            AssignIfFound(0, "Base_Kail");
            AssignIfFound(1, "Base_Pistol");
            AssignIfFound(2, "Base_Pisau");
            defaultHotbarBuilt = true;
            if (hotbar[1] != NoSlot)
            {
                activeHotbarIndex = NoSlot;   // Supaya RequestSlot(1) tidak dianggap "tekan slot aktif"
                SetActiveBehaviour(fistBehaviour, null, false);
                RequestSlot(1);
            }
        }

        private void AssignIfFound(int index, string itemId)
        {
            if (index >= hotbar.Length) return;
            var inst = inventory.FindFirstInstance(itemId);
            if (inst != null) SetHotbarSlot(index, inst.instanceId);
        }

        /// <summary>Dipanggil dari aksi item di #66. Item OffHand memakai EquipOffHand. Tas tidak bisa masuk (equipPrefab null).</summary>
        public bool AssignToHotbar(int hotbarIndex, int instanceId)
        {
            if (hotbarIndex < 0 || hotbarIndex >= hotbar.Length || inventory == null) return false;
            var inst = inventory.FindInstance(instanceId);
            var data = inst != null ? ItemDatabase.Get(inst.itemId) : null;
            if (data == null) return false;
            bool holdable = data.equipPrefab != null || data.isConsumable;
            if (!holdable || data.handUse == HandUse.OffHand) return false;

            for (int i = 0; i < hotbar.Length; i++)
                if (i != hotbarIndex && hotbar[i] == instanceId) SetHotbarSlot(i, NoSlot);
            SetHotbarSlot(hotbarIndex, instanceId);

            if (hotbarIndex == activeHotbarIndex && locks == EquipLock.None)
            {
                // Ganti item di tangan.
                int idx = hotbarIndex;
                activeHotbarIndex = NoSlot;
                RequestSlot(idx);
            }
            return true;
        }

        private void SetHotbarSlot(int index, int instanceId)
        {
            hotbar[index] = instanceId;
            string id = string.Empty;
            if (instanceId != NoSlot && inventory != null)
            {
                var inst = inventory.FindInstance(instanceId);
                if (inst != null) id = inst.itemId;
            }
            EventChannel<HotbarChangedEvent>.Publish(new HotbarChangedEvent(index, id));
        }

        // ---------------------------------------------------------------- Tangan kiri

        /// <summary>Senter atau Obor di tangan kiri, lewat aksi "Pegang di Tangan Kiri" (#66).</summary>
        public bool EquipOffHand(int instanceId)
        {
            var inst = inventory != null ? inventory.FindInstance(instanceId) : null;
            var data = inst != null ? ItemDatabase.Get(inst.itemId) : null;
            if (data == null || data.handUse != HandUse.OffHand || data.equipPrefab == null) return false;
            HideOffHand();
            offHandInstanceId = instanceId;
            offHandActive = true;
            ApplyOffHandRule();
            return true;
        }

        /// <summary>Tombol F: nyalakan atau simpan item tangan kiri terakhir.</summary>
        public void ToggleOffHand()
        {
            if (offHandInstanceId == NoSlot) return;
            offHandActive = !offHandActive;
            ApplyOffHandRule();
        }

        /// <summary>
        /// Tangan kiri tampil kalau aktif, item utama bukan TwoHand, dan tidak membawa Heavy Item (kecuali config mengizinkan).
        /// Usulan disetujui #85 poin 3. Durasi bakar obor tetap berjalan saat disimpan (#65, milik TorchController).
        /// </summary>
        private void ApplyOffHandRule()
        {
            var inst = offHandInstanceId != NoSlot && inventory != null ? inventory.FindInstance(offHandInstanceId) : null;
            if (inst == null)
            {
                HideOffHand();
                offHandInstanceId = NoSlot;
                offHandActive = false;
                return;
            }
            var main = ActiveInstance;
            var mainData = main != null ? ItemDatabase.Get(main.itemId) : null;
            bool twoHand = mainData != null && mainData.handUse == HandUse.TwoHand;
            bool heavyBlocks = (locks & EquipLock.HeavyItem) != 0 && !(Balance != null && Balance.equipment.offHandAllowedWhileCarrying);
            bool show = offHandActive && !twoHand && !heavyBlocks && (locks & EquipLock.Dead) == 0;

            var behaviour = GetBehaviour(inst.itemId);
            if (behaviour == null) return;
            if (show)
            {
                if (offHandBehaviour != behaviour) HideOffHand();
                if (offHandBehaviour == null)
                {
                    offHandBehaviour = behaviour;
                    offHandBehaviour.gameObject.SetActive(true);
                    offHandBehaviour.OnEquip(inst);
                }
            }
            else HideOffHand();
        }

        private void HideOffHand()
        {
            if (offHandBehaviour == null) return;
            offHandBehaviour.OnUnequip();
            offHandBehaviour.gameObject.SetActive(false);
            offHandBehaviour = null;
        }

        // ---------------------------------------------------------------- Kunci

        public void PushLock(EquipLock l)
        {
            bool hadStash = (locks & StashLocks) != 0;
            locks |= l;
            if (!hadStash && (l & StashLocks) != 0)
            {
                // Item disimpan, tangan dipakai barang atau tangga.
                lastIndex = activeHotbarIndex;
                LastUnequippedInstanceId = ActiveInstanceId;
                if (activeBehaviour != null) activeBehaviour.OnUnequip();
                SetActiveBehaviour(fistBehaviour, null, false);
                activeHotbarIndex = NoSlot;
                state = EquipState.Idle;
                pendingIndex = NoPending;
                if (fistBehaviour != null) fistBehaviour.gameObject.SetActive(false);
                if (FirstPersonCamera.Instance != null) FirstPersonCamera.Instance.SetViewmodelMode(ViewmodelMode.Hidden);
                EventChannel<EquippedItemChangedEvent>.Publish(new EquippedItemChangedEvent(string.Empty));
            }
            ApplyOffHandRule();
        }

        public void PopLock(EquipLock l)
        {
            bool hadStash = (locks & StashLocks) != 0;
            locks &= ~l;
            bool hasStash = (locks & StashLocks) != 0;
            if (hadStash && !hasStash && (locks & EquipLock.Dead) == 0)
            {
                if (FirstPersonCamera.Instance != null) FirstPersonCamera.Instance.SetViewmodelMode(ViewmodelMode.Visible);
                if (fistBehaviour != null && activeBehaviour == fistBehaviour) fistBehaviour.gameObject.SetActive(true);
                int restore = lastIndex;
                lastIndex = NoSlot;
                if (restore >= 0 && locks == EquipLock.None) RequestSlot(restore);
            }
            ApplyOffHandRule();
        }

        private void HandleFocus(UIFocusChangedEvent e)
        {
            if (e.isGameplayBlocked) locks |= EquipLock.Menu;
            else locks &= ~EquipLock.Menu;
        }

        private void HandleHeavyItem(HeavyItemChangedEvent e)
        {
            if (e.isCarried) PushLock(EquipLock.HeavyItem); else PopLock(EquipLock.HeavyItem);
        }

        private void HandlePinned(PlayerPinnedEvent e)
        {
            if (e.isPinned) PushLock(EquipLock.Pinned); else PopLock(EquipLock.Pinned);
        }

        private void HandleDied(PlayerDiedEvent e)
        {
            PushLock(EquipLock.Dead);
            HideOffHand();
        }

        // ---------------------------------------------------------------- Sinkron dengan inventori

        private void HandleInventoryChanged(InventoryChangedEvent e)
        {
            if (inventory == null) return;

            // Hotbar awal dibangun saat item inti pertama kali ada (IslandArrival.Apply menambah item inti).
            if (!defaultHotbarBuilt && inventory.FindFirstInstance("Base_Pistol") != null) BuildDefaultHotbar();

            for (int i = 0; i < hotbar.Length; i++)
            {
                if (hotbar[i] == NoSlot || inventory.FindInstance(hotbar[i]) != null) continue;
                SetHotbarSlot(i, NoSlot);
                if (i == activeHotbarIndex) HandleActiveItemLost();
            }
            if (offHandInstanceId != NoSlot && inventory.FindInstance(offHandInstanceId) == null) ApplyOffHandRule();
        }

        /// <summary>Item aktif hancur atau hilang: pilih slot terisi berikutnya ke kanan, kalau tidak ada tangan kosong.</summary>
        private void HandleActiveItemLost()
        {
            LastUnequippedInstanceId = ActiveInstanceId;
            bool wasConsumable = activeBehaviour != null && activeBehaviour == consumableBehaviour;
            if (activeBehaviour != null && activeBehaviour != fistBehaviour) activeBehaviour.OnUnequip();
            int from = activeHotbarIndex;
            activeHotbarIndex = NoSlot;
            SetActiveBehaviour(fistBehaviour, null, false);
            state = EquipState.Idle;
            // Notifikasi "Item rusak" (#22), kecuali tumpukan konsumsi yang habis dipakai.
            if (!wasConsumable && NotificationManager.Instance != null)
                NotificationManager.Instance.Show("notif.item_broken", NotificationKind.Warning);
            for (int step = 1; step < hotbar.Length; step++)
            {
                int i = (from + step) % hotbar.Length;
                if (hotbar[i] != NoSlot)
                {
                    if (locks == EquipLock.None) RequestSlot(i);
                    return;
                }
            }
            EventChannel<EquippedItemChangedEvent>.Publish(new EquippedItemChangedEvent(string.Empty));
        }

        // ---------------------------------------------------------------- Save (#80)

        public string SaveId => SaveIdConst;
        public int LoadOrder => 60;

        public string CaptureState()
        {
            return JsonUtility.ToJson(new EquipmentSaveData
            {
                hotbarInstanceIds = (int[])hotbar.Clone(),
                activeHotbarIndex = activeHotbarIndex,
                offHandInstanceId = offHandInstanceId,
                offHandActive = offHandActive,
                defaultHotbarBuilt = defaultHotbarBuilt,
            });
        }

        /// <summary>Setelah InventoryManager (LoadOrder 50). Rujukan yang tidak ditemukan dikosongkan. Tanpa event.</summary>
        public void RestoreState(string json)
        {
            var data = JsonUtility.FromJson<EquipmentSaveData>(json);
            for (int i = 0; i < hotbar.Length; i++)
            {
                int id = data.hotbarInstanceIds != null && i < data.hotbarInstanceIds.Length ? data.hotbarInstanceIds[i] : NoSlot;
                hotbar[i] = id != NoSlot && inventory != null && inventory.FindInstance(id) != null ? id : NoSlot;
            }
            activeHotbarIndex = data.activeHotbarIndex >= 0 && data.activeHotbarIndex < hotbar.Length && hotbar[data.activeHotbarIndex] != NoSlot
                ? data.activeHotbarIndex : NoSlot;
            offHandInstanceId = data.offHandInstanceId;
            offHandActive = data.offHandActive;
            defaultHotbarBuilt = data.defaultHotbarBuilt;
        }

        /// <summary>Item aktif dipasang tanpa animasi, lalu EquippedItemChangedEvent dikirim ulang.</summary>
        public void OnRestoreFinished()
        {
            state = EquipState.Idle;
            pendingIndex = NoPending;
            var inst = activeHotbarIndex >= 0 && inventory != null ? inventory.FindInstance(hotbar[activeHotbarIndex]) : null;
            var behaviour = inst != null ? GetBehaviourFor(inst) : null;
            if (behaviour == null) { behaviour = fistBehaviour; inst = null; }
            SetActiveBehaviour(behaviour, inst, true);
            ApplyOffHandRule();
            for (int i = 0; i < hotbar.Length; i++) SetHotbarSlot(i, hotbar[i]);
            EventChannel<EquippedItemChangedEvent>.Publish(new EquippedItemChangedEvent(ActiveItemId()));
        }
    }
}
