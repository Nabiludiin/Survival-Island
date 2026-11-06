using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Hasil satu ayunan perbaikan atau satu percobaan upgrade (dibaca HammerEquippable dan HUD).</summary>
    public enum RepairResult : byte
    {
        Repaired, Cooldown, NoTarget, FullHP, UnderAttack, NoMaterial,
        Upgraded, CannotUpgrade, NotLearned, MissingCost, Holding
    }

    /// <summary>
    /// Perbaikan struktur dengan palu dan upgrade kayu ke batu. HP tetap milik BaseHealth (DamagePipeline.md); sistem ini
    /// hanya mengatur syarat, biaya, dan tampilan. Blueprint: raft/BuildingIntegrity.md (#47).
    /// GDD bagian 3: zombie menyerang base. Bagian 6: gempa merusak dinding yang belum di-upgrade, dan pemain perlu cadangan
    /// material untuk perbaikan. Angka perbaikan dan biaya upgrade adalah usulan disetujui developer (#85).
    /// Manager per scene IslandPhase.
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - StructureRegistry (kode Langkah 4) belum punya ReplaceInstance dan Find. Penggantian prefab ditulis di sini
    ///   (ReplaceInstance) memakai Register dan StructureNode.Setup; pencarian memakai TryGetById.
    /// - Field #47 di BuildableData dibaca dari StructureIntegrityInfo di prefab (lihat kelas itu).
    /// - NPCRoster.GetSkillMultiplier (#76) belum ada: pengali Crafter 1.
    /// - Bidikan memakai raycast sendiri dengan repairRange (TryGetAimedStructure memakai maxBuildDistance).
    /// </summary>
    public class BuildingIntegrity : MonoBehaviour
    {
        public const string RepairFxTag = "FX_RepairHit";
        public const string UpgradeFxTag = "FX_UpgradeDust";

        public static BuildingIntegrity Instance { get; private set; }

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip repairClip;
        [SerializeField] private AudioClip upgradeClip;

        private float nextSwingTime;
        private float upgradeTimer;
        private string upgradeTargetId = string.Empty;

        private static IntegrityBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.integrity : null;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Debug.LogError("[BuildingIntegrity] Lebih dari satu instance."); enabled = false; return; }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Bidikan

        /// <summary>Struktur yang dibidik dalam repairRange.</summary>
        public bool TryGetAimedStructure(out StructureNode node, out RaycastHit hit)
        {
            node = null;
            hit = default;
            var b = Bal;
            var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : null;
            if (cam == null || b == null) return false;
            var ray = new Ray(cam.transform.position, cam.transform.forward);
            if (!PhysicsQuery.Raycast(ray, out hit, b.repairRange, PhysicsMasks.BuildSurface)) return false;
            if (hit.collider is TerrainCollider) return false;
            node = hit.collider.GetComponentInParent<StructureNode>();
            return node != null && node.Health != null;
        }

        /// <summary>Isi prompt HUD "Perbaiki (1 Kayu) 62%". HUD memanggil ini setiap 0,1 detik, bukan tiap frame.</summary>
        public bool GetRepairPreview(out string itemId, out int cost, out float hpRatio)
        {
            itemId = string.Empty;
            cost = 0;
            hpRatio = 1f;
            if (!TryGetAimedStructure(out var node, out _)) return false;
            var info = StructureIntegrityInfo.For(node.Data);
            itemId = info != null ? info.repairItemId : "Mat_Wood";
            cost = info != null ? Mathf.Max(1, info.repairCostPerSwing) : 1;
            hpRatio = node.Health.HpRatio;
            return hpRatio < 1f;
        }

        // ---------------------------------------------------------------- Palu perbaikan

        /// <summary>Mode Repair, dipanggil HammerEquippable saat Pressed dan Held.</summary>
        public RepairResult TryRepairSwing()
        {
            var b = Bal;
            if (b == null) return RepairResult.NoTarget;
            // 1. Jeda ayunan (Time.time berhenti saat dijeda).
            if (Time.time < nextSwingTime) return RepairResult.Cooldown;
            // 2.
            if (!TryGetAimedStructure(out var node, out var hit)) return RepairResult.NoTarget;
            var health = node.Health;
            var damageable = health as BaseHealth;
            // 3.
            if (health.HpRatio >= 1f) { Notify("notif.repair.full"); return RepairResult.FullHP; }
            // 4. Usulan 2: perbaikan ditolak selama repairBlockedAfterDamageSeconds setelah menerima damage.
            if (health.LastDamageTime > 0f && Time.time - health.LastDamageTime < b.repairBlockedAfterDamageSeconds)
            {
                Notify("notif.repair.under_attack");
                return RepairResult.UnderAttack;
            }
            // 5. Hanya inventori pemain (peti tidak dihitung).
            var info = StructureIntegrityInfo.For(node.Data);
            string itemId = info != null ? info.repairItemId : "Mat_Wood";
            int cost = info != null ? Mathf.Max(1, info.repairCostPerSwing) : 1;
            var inv = InventoryManager.Instance;
            if (inv == null || !inv.HasItem(itemId, cost)) { NotifyItem("notif.repair.no_material", itemId, cost); return RepairResult.NoMaterial; }
            // 6.
            inv.RemoveItem(itemId, cost);
            // 7. Usulan 1: 15 persen HP maksimum per ayunan, dibatasi maxHP oleh BaseHealth.Repair.
            // Usulan disetujui (#47, #76): Tukang hidup di base membuat perbaikan lebih cepat.
            float crafter = NPCRoster.Instance != null ? NPCRoster.Instance.GetBuilderRepairMultiplier() : 1f;
            float maxHp = node.Data != null ? node.Data.maxHP : 100f;
            if (damageable != null) damageable.Repair(maxHp * b.repairHpRatioPerSwing * crafter);
            // 8.
            WearHammer(b.hammerDecayPerRepairSwing);
            // 9.
            nextSwingTime = Time.time + b.repairSwingSeconds;
            // 10. Animasi ayun dimainkan viewmodel palu. Suara, serpihan, dan Noise Sphere (#49).
            if (audioSource != null && repairClip != null) audioSource.PlayOneShot(repairClip);
            if (VFXManager.Instance != null) VFXManager.Instance.Play(RepairFxTag, hit.point, Quaternion.LookRotation(hit.normal));
            NoiseEmitter.Emit(hit.point, b.noiseRadiusPerSwing, NoiseSourceKind.Tool);
            // 11.
            EventChannel<StructureRepairedEvent>.Publish(new StructureRepairedEvent(node.StructureId, health.HpRatio));
            return RepairResult.Repaired;
        }

        // ---------------------------------------------------------------- Upgrade

        /// <summary>Mode Upgrade: tahan klik kiri selama upgradeHoldSeconds.</summary>
        public RepairResult TickUpgrade(ActionPhase phase)
        {
            var b = Bal;
            if (b == null) return RepairResult.NoTarget;
            if (phase == ActionPhase.Released) { CancelUpgrade(); return RepairResult.NoTarget; }

            if (phase == ActionPhase.Pressed)
            {
                CancelUpgrade();
                if (!TryGetAimedStructure(out var node, out _)) return RepairResult.NoTarget;
                var check = CheckUpgrade(node, true);
                if (check != RepairResult.Holding) return check;
                upgradeTargetId = node.StructureId;
                upgradeTimer = 0f;
                return RepairResult.Holding;
            }

            // Held
            if (string.IsNullOrEmpty(upgradeTargetId)) return RepairResult.NoTarget;
            // Target berubah atau struktur hancur: batal tanpa biaya.
            if (!TryGetAimedStructure(out var aimed, out _) || aimed.StructureId != upgradeTargetId) { CancelUpgrade(); return RepairResult.NoTarget; }
            upgradeTimer += Time.deltaTime;
            if (upgradeTimer < b.upgradeHoldSeconds) return RepairResult.Holding;
            var result = TryUpgrade(aimed) ? RepairResult.Upgraded : CheckUpgrade(aimed, true);
            CancelUpgrade();
            return result;
        }

        private void CancelUpgrade()
        {
            upgradeTimer = 0f;
            upgradeTargetId = string.Empty;
        }

        /// <summary>Tabel "Syarat upgrade". Holding berarti semua syarat lolos.</summary>
        private RepairResult CheckUpgrade(StructureNode node, bool notify)
        {
            var b = Bal;
            var info = node != null ? StructureIntegrityInfo.For(node.Data) : null;
            // 2.
            if (info == null || info.upgradeTo == null) { if (notify) Notify("notif.upgrade.cannot"); return RepairResult.CannotUpgrade; }
            // 3. Usulan 8: unlockNodeId kosong berarti terbuka.
            string unlock = info.upgradeTo.unlockNodeId;
            if (!string.IsNullOrEmpty(unlock) && (TechTreeManager.Instance == null || !TechTreeManager.Instance.IsUnlocked(unlock)))
            { if (notify) Notify("notif.upgrade.not_learned"); return RepairResult.NotLearned; }
            // 4.
            var h = node.Health;
            if (h.LastDamageTime > 0f && Time.time - h.LastDamageTime < b.upgradeBlockedAfterDamageSeconds)
            { if (notify) Notify("notif.repair.under_attack"); return RepairResult.UnderAttack; }
            // 5.
            var inv = InventoryManager.Instance;
            if (inv == null) return RepairResult.MissingCost;
            if (info.upgradeCost != null)
            {
                for (int i = 0; i < info.upgradeCost.Length; i++)
                {
                    var c = info.upgradeCost[i];
                    if (c.amount > 0 && !inv.HasItem(c.itemId, c.amount))
                    { if (notify) NotifyItem("notif.upgrade.need", c.itemId, c.amount); return RepairResult.MissingCost; }
                }
            }
            return RepairResult.Holding;
        }

        private bool TryUpgrade(StructureNode node)
        {
            var b = Bal;
            // 1.
            if (CheckUpgrade(node, false) != RepairResult.Holding) return false;
            var info = StructureIntegrityInfo.For(node.Data);
            var next = info.upgradeTo;
            if (next.prefab == null) { Debug.LogWarning($"[BuildingIntegrity] '{next.id}' tanpa prefab."); return false; }
            // 2.
            var inv = InventoryManager.Instance;
            for (int i = 0; i < info.upgradeCost.Length; i++)
                if (info.upgradeCost[i].amount > 0) inv.RemoveItem(info.upgradeCost[i].itemId, info.upgradeCost[i].amount);
            // 3. Usulan 7: rasio HP tetap, tanpa perbaikan gratis.
            float ratio = node.Health.HpRatio;
            string id = node.StructureId;
            Vector3 pos = node.transform.position;
            // 4.
            var replaced = ReplaceInstance(node, next, ratio);
            if (replaced == null) return false;
            // 5-6.
            WearHammer(b.hammerDecayPerUpgrade);
            if (audioSource != null && upgradeClip != null) audioSource.PlayOneShot(upgradeClip);
            if (VFXManager.Instance != null) VFXManager.Instance.Play(UpgradeFxTag, pos + Vector3.up, Quaternion.identity);
            // 7.
            EventChannel<StructureUpgradedEvent>.Publish(new StructureUpgradedEvent(id, next.id));
            return true;
        }

        /// <summary>
        /// Prefab baru pada posisi dan rotasi yang sama, structureId sama, isi grid dipindahkan, status pintu dipindahkan,
        /// lalu objek lama dihapus di akhir frame tanpa StructureDestroyedEvent (tidak ada runtuhan).
        /// NavMeshObstacle prefab baru aktif pada frame yang sama, jadi lubang di dinding tidak sempat terbuka (#78).
        /// </summary>
        private StructureNode ReplaceInstance(StructureNode old, BuildableData next, float ratio)
        {
            var registry = StructureRegistry.Instance;
            if (registry == null || old == null) return null;
            var t = old.transform;
            var go = Instantiate(next.prefab, t.position, t.rotation, t.parent);
            var health = go.GetComponent<IStructureHealth>();
            if (health == null) { Debug.LogError($"[BuildingIntegrity] Prefab '{next.id}' tanpa BaseHealth."); Destroy(go); return null; }
            var node = go.GetComponent<StructureNode>();
            if (node == null) node = go.AddComponent<StructureNode>();
            // HP dibatasi maxHP baru.
            health.Initialize(next, old.StructureId, next.maxHP * Mathf.Clamp01(ratio));
            node.Setup(next, old.Key, old.BaseY, old.IsGrounded);
            var oldDoor = old.GetComponent<BaseDoor>();
            var newDoor = go.GetComponent<BaseDoor>();
            if (oldDoor != null && newDoor != null) newDoor.CopyStateFrom(oldDoor);
            // Register menimpa entri grid dan ID lama, jadi grafik dukungan tidak berubah.
            registry.Register(node);
            BaseNavigation.ConfigureObstacle(node);
            Destroy(old.gameObject);
            return node;
        }

        // ---------------------------------------------------------------- Bantuan

        private static void WearHammer(float amount)
        {
            var eq = EquipmentManager.Instance;
            var inst = eq != null ? eq.ActiveInstance : null;
            // Palu rusak: alur item hilang di #60 memanggil OnUnequip.
            if (inst != null && amount > 0f) DurabilitySystem.ApplyWear(inst, amount);
        }

        /// <summary>Pesan yang menyebut nama bahan, misalnya "Butuh 10 Batu". Teks memakai placeholder {item} dan {amount}.</summary>
        private static void NotifyItem(string key, string itemId, int amount)
        {
            if (NotificationManager.Instance == null) return;
            var item = ItemDatabase.Get(itemId);
            string nameKey = item != null ? item.NameKey : "item." + itemId + ".name";
            string name = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(nameKey) : itemId;
            NotificationManager.Instance.ShowFormat(key, NotificationKind.Warning, "item", name, "amount", amount.ToString());
        }

        private static void Notify(string key)
        {
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show(key, NotificationKind.Info);
        }
    }
}
