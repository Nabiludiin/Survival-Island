using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Peti loot dan mayat tentara. Ditahan 1 detik, lalu isinya keluar ke tanah sebagai WorldItem.
    /// Blueprint: Island_Logic/IslandInteractable.md bagian "Objek milik issue ini" (#17), isi dari RegionLootTable (#103).
    /// GDD bagian 3: looting di pulau. Usulan disetujui developer (#85):
    /// 1. Peti loot mengeluarkan isi ke tanah, bukan membuka panel.
    /// 2. Membuka peti ditahan 1 detik dan batal saat berlari atau diserang (PlayerInteractor).
    /// 4. Membuka peti menimbulkan suara kecil, supaya looting malam punya risiko (#49).
    /// Mayat tentara memakai komponen yang sama dengan pointKind = SoldierBody.
    /// </summary>
    public class LootContainerInteractable : IslandInteractable
    {
        [SerializeField] private LootTable lootTable;
        [Tooltip("Chest atau SoldierBody (#103).")]
        [SerializeField] private LootPointKind pointKind = LootPointKind.Chest;
        [Tooltip("Selalu keluar sebelum isi acak (#103).")]
        [SerializeField] private ItemStack[] guaranteedItems = Array.Empty<ItemStack>();
        [Tooltip("Titik keluarnya item. Kosong berarti di depan peti.")]
        [SerializeField] private Transform[] dropPoints = Array.Empty<Transform>();
        [SerializeField] private NoiseEmitter noiseEmitter;
        [SerializeField] private Animator lidAnimator;
        [SerializeField] private string openTrigger = "Open";
        [Tooltip("Pose terbuka tanpa animasi, untuk load.")]
        [SerializeField] private string openedStateName = "Opened";

        private int dropIndex;

        protected override void Awake()
        {
            if (noiseEmitter == null) noiseEmitter = GetComponentInChildren<NoiseEmitter>();
            base.Awake();
        }

        // Setelah dibuka, prompt tidak muncul lagi. Objek tetap terlihat (bukan Consume).
        public override bool IsAvailable => base.IsAvailable && !isOpened;

        protected override bool OnInteract(in InteractContext ctx)
        {
            // 1.
            isOpened = true;
            if (lidAnimator != null && !string.IsNullOrEmpty(openTrigger)) lidAnimator.SetTrigger(openTrigger);

            // 2. Fork per saveId: isi tetap sama pada seed yang sama dan tidak berubah setelah load (#89).
            var seeds = RunSeedManager.Instance;
            var rng = seeds != null ? seeds.Fork(RandomStreamId.IslandLoot, string.IsNullOrEmpty(saveId) ? name : saveId)
                                    : new RandomStream((ulong)Environment.TickCount);
            var inv = InventoryManager.Instance;
            dropIndex = 0;

            // 3. Isi pasti lebih dulu.
            for (int i = 0; i < guaranteedItems.Length; i++) SpawnDrop(inv, guaranteedItems[i].item, guaranteedItems[i].amount);

            // 4. Jumlah tumpukan sesuai pointKind.
            var bal = GameManager.CurrentBalance;
            int rolls = 0;
            if (bal != null)
            {
                bool body = (pointKind & LootPointKind.SoldierBody) != 0;
                int min = body ? bal.loot.bodyRollsMin : bal.loot.chestRollsMin;
                int max = body ? bal.loot.bodyRollsMax : bal.loot.chestRollsMax;
                rolls = rng.Range(min, Mathf.Max(min, max) + 1);
            }
            int day = GameManager.Instance != null ? GameManager.Instance.currentDay : 1;
            for (int i = 0; i < rolls; i++)
            {
                if (RegionLootTable.TryRoll(lootTable, pointKind, day, ref rng, out ItemData item, out int amount))
                    SpawnDrop(inv, item, amount);
            }

            // 6. Suara kecil lewat NoiseEmitter (#49). Radius dari interact.containerNoiseRadius (nama dan nilai usulan).
            float openNoiseRadius = bal != null ? bal.interact.containerNoiseRadius : 8f;
            if (noiseEmitter != null) noiseEmitter.Emit(openNoiseRadius, NoiseSourceKind.Container);
            else NoiseEmitter.Emit(transform.position, openNoiseRadius, NoiseSourceKind.Container);
            return true;
        }

        /// <summary>5. Tiap isi menjadi WorldItem tanpa kedaluwarsa. Pemain tidak perlu ruang inventori.</summary>
        private void SpawnDrop(InventoryManager inv, ItemData item, int amount)
        {
            if (item == null || amount <= 0 || inv == null || WorldItemManager.Instance == null) return;
            Vector3 pos;
            if (dropPoints != null && dropPoints.Length > 0 && dropPoints[dropIndex % dropPoints.Length] != null)
                pos = dropPoints[dropIndex % dropPoints.Length].position;
            else
                pos = transform.position + transform.forward * 0.8f + Vector3.up * 0.3f;
            dropIndex++;
            // TODO(RegionStreaming): WorldItem hasil peti disimpan di RegionStateStore (#66, #88), jadi peti tidak bisa diulang.
            WorldItemManager.Instance.Spawn(inv.CreateInstance(item), amount, pos, false);
        }

        protected override void ApplyRestoredVisual()
        {
            if (isOpened && lidAnimator != null && !string.IsNullOrEmpty(openedStateName)) lidAnimator.Play(openedStateName, 0, 1f);
        }
    }
}
