using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Hasil pemeriksaan perbaikan atau upgrade. Catatan implementasi: blueprint menyebut nama RepairCheck tanpa daftar nilai;
    /// nilai di bawah adalah alasan penolakan yang tertulis di ToolRepairSystem.md bagian 2.
    /// </summary>
    public enum RepairCheck
    {
        Ok, ItemMissing, NotRepairable, NotDamaged, NoStation, RepairLimit, MissingMaterial, Busy,
        UpgradeNotApplicable, UpgradeInstalled, UpgradeLimit, UpgradeLocked
    }

    /// <summary>
    /// Perbaikan alat dan senjata serta upgrade kecil per item. Blueprint: Core_Logic/ToolRepairSystem.md (#113).
    /// GDD bagian 7: Fire Axe tidak bisa rusak. Perbaikan dan upgrade adalah usulan disetujui developer (#113).
    /// Item inti (Base_Pistol, Base_Kail, Base_Pisau) mengikuti aturan item inti GLOSSARY.md: bisa rusak, tidak pernah hancur,
    /// dan maksimumnya tidak turun.
    ///
    /// Komponen scene IslandPhase. Satu pekerjaan sekaligus, dijalankan dari tab Perbaiki di CraftingPanel (#77).
    /// Tidak punya ISaveable: currentJob tidak disimpan, jadi pekerjaan yang berjalan batal tanpa biaya.
    /// </summary>
    public class ToolRepairSystem : MonoBehaviour
    {
        public static ToolRepairSystem Instance { get; private set; }

        private enum JobKind { None, Repair, Upgrade }

        private struct Job
        {
            public JobKind kind;
            public int instanceId;
            public ItemUpgradeData upgrade;
            public float remaining;
            public float total;
        }

        [Header("Suara (AudioManager)")]
        [SerializeField] private AudioClip repairDoneClip;
        [SerializeField] private AudioClip upgradeDoneClip;

        private Job currentJob;
        private bool stopped;
        private readonly List<RecipeIngredient> costBuffer = new List<RecipeIngredient>(4);

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        public bool IsBusy => currentJob.kind != JobKind.None;
        public int CurrentInstanceId => IsBusy ? currentJob.instanceId : -1;
        public float Progress => IsBusy && currentJob.total > 0f ? 1f - currentJob.remaining / currentJob.total : 0f;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            EventChannel<PlayerDamagedEvent>.Raised += HandlePlayerDamaged;
            EventChannel<UIFocusChangedEvent>.Raised += HandleFocusChanged;
            EventChannel<PlayerDiedEvent>.Raised += HandlePlayerDied;
        }

        private void OnDisable()
        {
            EventChannel<PlayerDamagedEvent>.Raised -= HandlePlayerDamaged;
            EventChannel<UIFocusChangedEvent>.Raised -= HandleFocusChanged;
            EventChannel<PlayerDiedEvent>.Raised -= HandlePlayerDied;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void HandlePlayerDamaged(PlayerDamagedEvent e) => Cancel();
        private void HandlePlayerDied(PlayerDiedEvent e) { stopped = true; Cancel(); }

        private void HandleFocusChanged(UIFocusChangedEvent e)
        {
            // Panel tertutup: tidak ada panel yang menahan gameplay lagi.
            if (!e.isGameplayBlocked) Cancel();
        }

        /// <summary>Batalkan pekerjaan tanpa biaya (bahan belum dipotong).</summary>
        public void Cancel()
        {
            currentJob = default;
        }

        // ---------------------------------------------------------------- Memperbaiki

        public RepairCheck CanRepair(ItemInstance inst)
        {
            if (inst == null) return RepairCheck.ItemMissing;
            var data = ItemDatabase.Get(inst.itemId);
            // Fire Axe (maxDurability -1) dan item mod_indestructible tidak tampil di tab Perbaiki.
            if (data == null || !data.isRepairable || data.maxDurability < 0f || inst.durability < 0f) return RepairCheck.NotRepairable;
            float max = DurabilitySystem.GetEffectiveMax(inst, data);
            if (inst.durability >= max) return RepairCheck.NotDamaged;
            if (!IsStationAvailable(data.repairStation)) return RepairCheck.NoStation;
            int maxRepairs = Balance != null ? Balance.repair.maxRepairs : 6;
            if (!data.isCoreItem && inst.repairCount >= maxRepairs) return RepairCheck.RepairLimit;
            GetRepairCost(inst, costBuffer);
            if (!HasMaterials(costBuffer)) return RepairCheck.MissingMaterial;
            return RepairCheck.Ok;
        }

        /// <summary>
        /// Biaya mengikuti kerusakan: Ceil(cost x (1 - durability / effectiveMax)), minimum 1 tiap bahan.
        /// Hasil ditulis ke output (dikosongkan dulu), tanpa alokasi baru.
        /// </summary>
        public void GetRepairCost(ItemInstance inst, List<RecipeIngredient> output)
        {
            output.Clear();
            if (inst == null) return;
            var data = ItemDatabase.Get(inst.itemId);
            if (data == null || data.repairCost == null) return;
            float max = DurabilitySystem.GetEffectiveMax(inst, data);
            float damaged = max > 0f ? Mathf.Clamp01(1f - inst.durability / max) : 1f;
            for (int i = 0; i < data.repairCost.Length; i++)
            {
                var c = data.repairCost[i];
                if (c.item == null || c.amount <= 0) continue;
                output.Add(new RecipeIngredient { item = c.item, amount = Mathf.Max(1, Mathf.CeilToInt(c.amount * damaged)) });
            }
        }

        public bool StartRepair(int instanceId)
        {
            // 1.
            if (stopped || IsBusy) return false;
            var inv = InventoryManager.Instance;
            if (inv == null || CanRepair(inv.FindInstance(instanceId)) != RepairCheck.Ok) return false;
            // 2. Bahan belum dipotong.
            float secs = Balance != null ? Balance.repair.repairSeconds : 3f;
            currentJob = new Job { kind = JobKind.Repair, instanceId = instanceId, remaining = secs, total = secs };
            return true;
        }

        // ---------------------------------------------------------------- Upgrade

        public RepairCheck CanUpgrade(ItemInstance inst, ItemUpgradeData upgrade)
        {
            if (inst == null) return RepairCheck.ItemMissing;
            if (upgrade == null) return RepairCheck.UpgradeNotApplicable;
            var data = ItemDatabase.Get(inst.itemId);
            if (data == null || !upgrade.AppliesTo(data.type)) return RepairCheck.UpgradeNotApplicable;
            // Upg_Magazine butuh WeaponData (#58). Upg_Durability butuh durabilitas (Fire Axe tidak).
            if (upgrade.kind == UpgradeKind.MagazineSize && data.weaponData == null) return RepairCheck.UpgradeNotApplicable;
            if (upgrade.kind == UpgradeKind.Durability && (data.maxDurability < 0f || inst.durability < 0f)) return RepairCheck.UpgradeNotApplicable;
            // Sekali per jenis, maksimum upgrade.maxPerItem.
            if ((inst.upgradeMask & upgrade.Mask) != 0 || WeaponStatCalculator.HasUpgrade(inst, upgrade.kind)) return RepairCheck.UpgradeInstalled;
            int maxPer = Balance != null ? Balance.upgrade.maxPerItem : 2;
            if (WeaponStatCalculator.CountUpgrades(inst) >= maxPer) return RepairCheck.UpgradeLimit;
            if (!string.IsNullOrEmpty(upgrade.techNodeId)
                && (TechTreeManager.Instance == null || !TechTreeManager.Instance.IsUnlocked(upgrade.techNodeId)))
                return RepairCheck.UpgradeLocked;
            if (!IsStationAvailable(upgrade.station)) return RepairCheck.NoStation;
            if (!HasMaterials(upgrade.cost)) return RepairCheck.MissingMaterial;
            return RepairCheck.Ok;
        }

        public bool StartUpgrade(int instanceId, ItemUpgradeData upgrade)
        {
            if (stopped || IsBusy) return false;
            var inv = InventoryManager.Instance;
            if (inv == null || CanUpgrade(inv.FindInstance(instanceId), upgrade) != RepairCheck.Ok) return false;
            float secs = Balance != null ? Balance.repair.upgradeSeconds : 6f;
            currentJob = new Job { kind = JobKind.Upgrade, instanceId = instanceId, upgrade = upgrade, remaining = secs, total = secs };
            return true;
        }

        // ---------------------------------------------------------------- Waktu

        private void Update()
        {
            if (!IsBusy || stopped) return;

            // Stasiun di luar jangkauan atau panel ditutup: batal tanpa biaya.
            var inv = InventoryManager.Instance;
            var inst = inv != null ? inv.FindInstance(currentJob.instanceId) : null;
            if (inst == null)
            {
                Cancel();
                return;
            }
            if (!IsStationAvailable(JobStation(inst)) || (CraftingManager.Instance != null && !CraftingManager.Instance.IsPanelOpen))
            {
                Cancel();
                return;
            }

            // 3. Game dijeda: deltaTime 0, timer berhenti.
            currentJob.remaining -= Time.deltaTime;
            if (currentJob.remaining > 0f) return;

            var job = currentJob;
            currentJob = default;
            if (job.kind == JobKind.Repair) CompleteRepair(inst);
            else if (job.kind == JobKind.Upgrade) CompleteUpgrade(inst, job.upgrade);
        }

        private CraftingStationType JobStation(ItemInstance inst)
        {
            if (currentJob.kind == JobKind.Upgrade && currentJob.upgrade != null) return currentJob.upgrade.station;
            var data = ItemDatabase.Get(inst.itemId);
            return data != null ? data.repairStation : CraftingStationType.Hand;
        }

        private void CompleteRepair(ItemInstance inst)
        {
            var data = ItemDatabase.Get(inst.itemId);
            if (data == null) return;

            // 4. Periksa ulang bahan, lalu potong.
            GetRepairCost(inst, costBuffer);
            if (!HasMaterials(costBuffer)) return;
            RemoveMaterials(costBuffer);

            // 5. Item inti tidak menambah repairCount (maksimumnya tidak turun).
            if (!data.isCoreItem) inst.repairCount++;

            // 6. Perbaikan selalu penuh terhadap maksimum yang baru.
            inst.durability = DurabilitySystem.GetEffectiveMax(inst, data);

            // 7.
            EventChannel<DurabilityStateChangedEvent>.Publish(
                new DurabilityStateChangedEvent(inst.instanceId, inst.itemId, DurabilitySystem.GetState(inst)));
            AudioManager.Instance?.PlaySFX(repairDoneClip);   // SFX perbaikan selesai
        }

        private void CompleteUpgrade(ItemInstance inst, ItemUpgradeData upgrade)
        {
            if (upgrade == null) return;
            if (!HasMaterials(upgrade.cost)) return;
            var data = ItemDatabase.Get(inst.itemId);
            float oldMax = data != null ? DurabilitySystem.GetEffectiveMax(inst, data) : -1f;

            // 1.
            RemoveMaterials(upgrade.cost);
            inst.upgradeMask |= upgrade.Mask;

            // 2. Durability: tambahkan selisih maksimum. 3. MagazineSize: magazineAmmo tidak berubah.
            if (upgrade.kind == UpgradeKind.Durability && data != null && inst.durability >= 0f && oldMax >= 0f)
            {
                float newMax = DurabilitySystem.GetEffectiveMax(inst, data);
                inst.durability = Mathf.Min(newMax, inst.durability + (newMax - oldMax));
            }

            // 4. Hitung ulang statistik kalau item sedang dipegang.
            var eq = EquipmentManager.Instance;
            if (eq != null && eq.ActiveInstanceId == inst.instanceId)
            {
                if (eq.Active is GunController gun) gun.RecalculateStats();
                // TODO(MeleeCombatSystem): MeleeCombatSystem.RecalculateStats() belum ada; melee membaca damage saat dipasang.
            }
            if (upgrade.kind == UpgradeKind.Durability)
                EventChannel<DurabilityStateChangedEvent>.Publish(
                    new DurabilityStateChangedEvent(inst.instanceId, inst.itemId, DurabilitySystem.GetState(inst)));
            AudioManager.Instance?.PlaySFX(upgradeDoneClip);   // SFX upgrade selesai
        }

        // ---------------------------------------------------------------- Bantuan

        private static bool IsStationAvailable(CraftingStationType t)
        {
            if (t == CraftingStationType.Hand) return true;
            return CraftingManager.Instance != null && CraftingManager.Instance.IsStationAvailable(t);
        }

        private static bool HasMaterials(IReadOnlyList<RecipeIngredient> cost)
        {
            var inv = InventoryManager.Instance;
            if (inv == null) return false;
            if (cost == null) return true;
            for (int i = 0; i < cost.Count; i++)
                if (cost[i].item != null && cost[i].amount > 0 && !inv.HasItem(cost[i].item.itemID, cost[i].amount)) return false;
            return true;
        }

        private static void RemoveMaterials(IReadOnlyList<RecipeIngredient> cost)
        {
            var inv = InventoryManager.Instance;
            if (inv == null || cost == null) return;
            for (int i = 0; i < cost.Count; i++)
                if (cost[i].item != null && cost[i].amount > 0) inv.RemoveItem(cost[i].item.itemID, cost[i].amount);
        }
    }
}
