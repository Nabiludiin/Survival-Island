using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Alur memakai, membuang, dan membusuknya item. Blueprint: Core_Logic/ItemUseSystem.md (#66).
    /// GDD bagian 3: lapar, haus, vaksin, dan item inti yang tidak bisa dibuang. Pemulihan bertahap, pembusukan,
    /// dan pembatalan pemakaian adalah usulan yang disetujui developer (#85).
    ///
    /// Tersambung: InfectionSystem.CanUseMedicine/ApplyMedicine (#64, fallback ke SurvivalStats), DiseaseManager (#45),
    /// StatusEffectManager.ApplyById (#20), dan NotificationManager (#22). Panel InventoryPanel (UI #66/#83) di luar kelas ini.
    /// </summary>
    public class ItemUseSystem : MonoBehaviour
    {
        public static ItemUseSystem Instance { get; private set; }

        [SerializeField] private InventoryManager inventory;
        [SerializeField] private SurvivalStats stats;
        [SerializeField] private IslandPlayerController controller;
        [SerializeField] private PlayerTraversal traversal;

        public bool IsUsing { get; private set; }
        private int useSlotIndex = -1;
        private int useInstanceId = -1;
        private float useTimer;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        // ---------------------------------------------------------------- Jam game

        /// <summary>
        /// Jam game total sejak run dimulai. Catatan implementasi (Langkah 3): DayNightDirector (Langkah 4) belum ada,
        /// jadi nilai dihitung dari GameManager.currentDay dan HourChangedEvent sticky, dengan ketelitian 1 jam:
        /// (hari - 1) * 24 + jam. Rumus sama dengan DayNightDirector.TotalGameHours (DayNightDirector.md), nilai awal jam 6.
        /// Langkah 4 boleh mengganti isi fungsi ini dengan DayNightDirector.Instance.TotalGameHours.
        /// </summary>
        public static float CurrentTotalGameHours()
        {
            // Hook Langkah 4: pakai jam DayNightDirector kalau ada (ketelitian penuh). Rumus lama tetap sebagai cadangan.
            if (DayNightDirector.Instance != null) return (float)DayNightDirector.Instance.TotalGameHours;
            int day = GameManager.Instance != null ? GameManager.Instance.currentDay : 1;
            int hour = EventChannel<HourChangedEvent>.HasLast ? EventChannel<HourChangedEvent>.Last.hour : 6;
            return (day - 1) * 24f + hour;
        }

        /// <summary>Kesegaran dari jam game, tanpa penghitung per item (ItemUseSystem.md "Pembusukan").</summary>
        public static Freshness GetFreshness(ItemInstance inst, ItemData data)
        {
            if (inst == null || data == null || data.spoilHours <= 0f || inst.createdGameHour < 0f) return Freshness.Fresh;
            float age = CurrentTotalGameHours() - inst.createdGameHour;
            if (age >= data.spoilHours) return Freshness.Rotten;
            float staleRatio = Balance != null ? Balance.items.staleRatio : 0.5f;
            return age >= data.spoilHours * staleRatio ? Freshness.Stale : Freshness.Fresh;
        }

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
            if (inventory == null) inventory = GetComponent<InventoryManager>();
            if (stats == null) stats = GetComponent<SurvivalStats>();
            if (controller == null) controller = GetComponent<IslandPlayerController>();
            if (traversal == null) traversal = GetComponent<PlayerTraversal>();
        }

        private void OnEnable()
        {
            EventChannel<PlayerDamagedEvent>.Raised += HandleDamaged;
            EventChannel<EquippedItemChangedEvent>.Raised += HandleEquippedChanged;
            EventChannel<HeavyItemChangedEvent>.Raised += HandleHeavyItem;
            EventChannel<PlayerPinnedEvent>.Raised += HandlePinned;
            EventChannel<PlayerDiedEvent>.Raised += HandleDied;
        }

        private void OnDisable()
        {
            EventChannel<PlayerDamagedEvent>.Raised -= HandleDamaged;
            EventChannel<EquippedItemChangedEvent>.Raised -= HandleEquippedChanged;
            EventChannel<HeavyItemChangedEvent>.Raised -= HandleHeavyItem;
            EventChannel<PlayerPinnedEvent>.Raised -= HandlePinned;
            EventChannel<PlayerDiedEvent>.Raised -= HandleDied;
            if (IsUsing) CancelUse("disabled");
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void Update()
        {
            if (!IsUsing) return;

            // Pemicu batal: mulai lari (InputManager.SprintHeld) dan memanjat (#101).
            var input = InputManager.Instance;
            if (input != null && input.SprintHeld) { CancelUse("sprint"); return; }
            if (traversal != null && traversal.Mode == TraversalMode.Climbing) { CancelUse("climb"); return; }

            // Panel Inventory tidak menjeda (#83), jadi waktu pakai tetap berjalan. Pause membuat deltaTime 0.
            useTimer -= Time.deltaTime;
            if (useTimer <= 0f) CompleteUse();
        }

        // ---------------------------------------------------------------- Memakai

        /// <summary>Mulai memakai item di slot. Mengembalikan false kalau ditolak.</summary>
        public bool TryUse(int slotIndex)
        {
            if (IsUsing || inventory == null || stats == null) return false;
            var slot = inventory.GetSlot(slotIndex);
            if (slot == null || slot.IsEmpty) return false;
            var data = ItemDatabase.Get(slot.instance.itemId);
            if (data == null || !data.isConsumable) return false;

            // Aturan "sudah penuh" hanya untuk item yang memulihkan lapar atau haus (ItemUseSystem.md langkah 2).
            if (data.hungerDelta > 0f || data.thirstDelta > 0f)
            {
                bool hungerFull = data.hungerDelta <= 0f || stats.Hunger >= stats.MaxHunger;
                bool thirstFull = data.thirstDelta <= 0f || stats.Thirst >= stats.MaxThirst;
                if (hungerFull && thirstFull)
                {
                    if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.already_full", NotificationKind.Info);   // "Sudah kenyang" (#22)
                    return false;
                }
            }
            // Hook Langkah 4 (#64): vaksin ditolak saat tidak terinfeksi.
            if (data.infectionDelta < 0f && InfectionSystem.Instance != null && !InfectionSystem.Instance.CanUseMedicine(data)) return false;
            // Obat penyakit dan obat HP ditolak kalau tidak diperlukan (#45). CanUseMedicine menampilkan notifikasinya.
            if ((data.curesDiseases != DiseaseCure.None || data.healAmount > 0f) && DiseaseManager.Instance != null
                && !DiseaseManager.Instance.CanUseMedicine(data)) return false;

            useSlotIndex = slotIndex;
            useInstanceId = slot.instance.instanceId;
            useTimer = data.useSeconds;
            IsUsing = true;
            if (controller != null)
                controller.SetSpeedModifier(SpeedModifierId.ItemUse, Balance != null ? Balance.items.useMoveSpeedMultiplier : 0.5f);

            if (data.useSeconds <= 0f) CompleteUse();
            return true;
        }

        /// <summary>Batal tanpa mengurangi item (usulan disetujui #85).</summary>
        public void CancelUse(string reason)
        {
            if (!IsUsing) return;
            EndUse();
        }

        private void CompleteUse()
        {
            var slot = inventory.GetSlot(useSlotIndex);
            // Slot berubah selama dipakai: batal (ItemUseSystem.md "Kasus Khusus").
            if (slot == null || slot.IsEmpty || slot.instance.instanceId != useInstanceId)
            {
                EndUse();
                return;
            }
            var inst = slot.instance;
            var data = ItemDatabase.Get(inst.itemId);
            if (data == null) { EndUse(); return; }

            var freshness = GetFreshness(inst, data);
            float mult = 1f;
            if (Balance != null)
            {
                if (freshness == Freshness.Stale) mult = Balance.items.staleHungerMultiplier;
                else if (freshness == Freshness.Rotten) mult = Balance.items.rottenHungerMultiplier;
            }
            float hunger = Mathf.Max(0f, data.hungerDelta) * mult;
            float thirst = Mathf.Max(0f, data.thirstDelta) * mult;

            // Pemulihan bertahap ala Raft (CATATAN_MEKANIK bagian 2): makanan dan minuman memakai restoreSeconds.
            if (hunger > 0f || thirst > 0f)
            {
                if (data.restoreSeconds > 0f) stats.QueueRestore(hunger, thirst, data.restoreSeconds);
                else { stats.RestoreHunger(hunger); stats.RestoreThirst(thirst); }
            }
            // HP dari obat langsung.
            if (data.healAmount > 0f) stats.RestoreHP(data.healAmount);
            // GDD bagian 3: vaksin menurunkan infeksi. Hook Langkah 4 (#64): lewat InfectionSystem kalau ada.
            if (data.infectionDelta < 0f)
            {
                if (InfectionSystem.Instance != null) InfectionSystem.Instance.ApplyMedicine(data);
                else stats.ReduceInfection(-data.infectionDelta);
            }
            else if (data.infectionDelta > 0f) stats.AddInfection(data.infectionDelta);
            // Peluang keracunan, atau penyembuhan penyakit untuk obat (#45).
            DiseaseManager.Instance?.OnConsumed(data, freshness);

            string itemId = inst.itemId;
            string effectId = data.appliesEffectId;
            inventory.RemoveFromSlot(useSlotIndex, 1);
            // Efek status dari item (#20).
            if (!string.IsNullOrEmpty(effectId)) StatusEffectManager.Instance?.ApplyById(effectId, itemId);
            EventChannel<ItemConsumedEvent>.Publish(new ItemConsumedEvent(itemId, freshness));
            EndUse();
        }

        private void EndUse()
        {
            IsUsing = false;
            useSlotIndex = -1;
            useInstanceId = -1;
            useTimer = 0f;
            if (controller != null) controller.ClearSpeedModifier(SpeedModifierId.ItemUse);
        }

        // ---------------------------------------------------------------- Membuang

        /// <summary>
        /// Membuang item sebagai WorldItem di depan kamera. Item inti dan item tidak bisa dibuang selalu ditolak (GLOSSARY.md).
        /// </summary>
        public bool DropFromSlot(int slotIndex, int amount)
        {
            var slot = inventory != null ? inventory.GetSlot(slotIndex) : null;
            if (slot == null || slot.IsEmpty || amount <= 0) return false;
            var data = ItemDatabase.Get(slot.instance.itemId);
            if (data == null || !data.isDroppable || data.isCoreItem)
            {
                if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.cannot_drop", NotificationKind.Info);   // "Tidak bisa dibuang" (#22)
                return false;
            }
            var manager = WorldItemManager.Instance;
            if (manager == null) return false;

            amount = Mathf.Min(amount, slot.amount);
            Transform view = FirstPersonCamera.Instance != null && FirstPersonCamera.Instance.WorldCamera != null
                ? FirstPersonCamera.Instance.WorldCamera.transform : transform;
            float dist = Balance != null ? Balance.items.dropForwardDistance : 1f;
            Vector3 pos = view.position + view.forward * dist;
            if (PhysicsQuery.Raycast(new Ray(view.position, view.forward), out var hit, dist, PhysicsMasks.InteractBlock))
                pos = hit.point - view.forward * 0.1f;

            var copy = slot.instance.Clone();
            if (amount < slot.amount) copy.instanceId = inventory.AllocateInstanceId();
            var item = manager.Spawn(copy, amount, pos, true);
            if (item == null) return false;
            inventory.RemoveFromSlot(slotIndex, amount);
            item.Push(view.forward * (Balance != null ? Balance.items.dropPushForce : 1.5f));
            return true;
        }

        // ---------------------------------------------------------------- Pemicu batal

        private void HandleDamaged(PlayerDamagedEvent e) { if (IsUsing) CancelUse("damaged"); }

        private void HandleEquippedChanged(EquippedItemChangedEvent e)
        {
            // Batal hanya kalau item yang dipakai adalah item di tangan.
            if (!IsUsing || EquipmentManager.Instance == null) return;
            if (EquipmentManager.Instance.LastUnequippedInstanceId == useInstanceId) CancelUse("switch");
        }

        private void HandleHeavyItem(HeavyItemChangedEvent e) { if (e.isCarried && IsUsing) CancelUse("heavy"); }
        private void HandlePinned(PlayerPinnedEvent e) { if (e.isPinned && IsUsing) CancelUse("pinned"); }
        private void HandleDied(PlayerDiedEvent e) { if (IsUsing) CancelUse("died"); }
    }
}
