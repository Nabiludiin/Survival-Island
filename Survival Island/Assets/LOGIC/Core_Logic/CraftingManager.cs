using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Hasil RequestCraft (GLOSSARY.md, CraftingStation.md).</summary>
    public enum CraftResult { Ok, Locked, NoStation, MissingMaterial, NoSpace, QueueFull }

    /// <summary>Satu job di antrean. Bahan sudah dipotong saat job dibuat.</summary>
    [Serializable]
    public class CraftJob
    {
        public string recipeId;
        public float remainingSeconds;
    }

    [Serializable]
    public struct CraftJobSave
    {
        public string recipeId;
        public float remainingSeconds;
    }

    /// <summary>Isi entri save world_recipes: status tech tree dan antrean (CraftingManager.md).</summary>
    [Serializable]
    public class CraftingSaveData
    {
        public string[] unlockedNodeIds;
        public CraftJobSave[] queue;
    }

    /// <summary>
    /// Manager antrean crafting di IslandPhase. Blueprint: Core_Logic/CraftingManager.md (ringkasan) dan
    /// Core_Logic/CraftingStation.md (aturan lengkap, #77, #80).
    ///
    /// GDD bagian 3: MC memulai dengan formula vaksin, dan NPC mempercepat crafting. GDD bagian 4: meja crafting di base.
    /// Usulan disetujui developer: antrean sampai 5 job, bahan dipotong saat masuk antrean dan kembali penuh saat batal,
    /// job meja berhenti saat pemain menjauh, semua job berhenti 1,5 detik saat diserang.
    ///
    /// Belum ada di Langkah 4 (TODO): notifikasi (#22) "Resep baru" dan "Inventori penuh, hasil jatuh", SFX (#16).
    /// </summary>
    [DisallowMultipleComponent]
    public class CraftingManager : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "world_recipes";

        public static CraftingManager Instance { get; private set; }

#pragma warning disable 0169 // Dipakai saat AudioManager (#16) ada
        [SerializeField] private AudioClip completeClip;
#pragma warning restore 0169

        private readonly List<CraftJob> queue = new List<CraftJob>(5);
        private readonly List<CraftingStation> stations = new List<CraftingStation>(8);
        private int availableMask = StationBit(CraftingStationType.Hand);
        private float npcSpeedMultiplier = 1f;
        private float hitPauseTimer;
        private float checkTimer;
        private bool stopped;

        /// <summary>true selama CraftingPanel terbuka (diatur panel). Memengaruhi batas jarak stasiun.</summary>
        public bool IsPanelOpen { get; set; }

        public IReadOnlyList<CraftJob> Queue => queue;

        /// <summary>Bit per CraftingStationType yang dalam jangkauan. Dibaca ToolRepairSystem (#113).</summary>
        public int AvailableMask => availableMask;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;
        private int MaxQueue => Balance != null ? Mathf.Max(1, Balance.crafting.maxQueue) : 5;

        public static int StationBit(CraftingStationType t) => 1 << (int)t;
        public bool IsStationAvailable(CraftingStationType t) => (availableMask & StationBit(t)) != 0;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Register(this);
            EventChannel<TechNodeUnlockedEvent>.Raised += HandleTechNodeUnlocked;
            EventChannel<PlayerDamagedEvent>.Raised += HandlePlayerDamaged;
            EventChannel<PlayerDiedEvent>.Raised += HandlePlayerDied;
        }

        private void OnDisable()
        {
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            EventChannel<TechNodeUnlockedEvent>.Raised -= HandleTechNodeUnlocked;
            EventChannel<PlayerDamagedEvent>.Raised -= HandlePlayerDamaged;
            EventChannel<PlayerDiedEvent>.Raised -= HandlePlayerDied;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Stasiun

        public void RegisterStation(CraftingStation s)
        {
            if (s != null && !stations.Contains(s)) stations.Add(s);
        }

        public void UnregisterStation(CraftingStation s)
        {
            if (stations.Remove(s)) RefreshAvailability();
        }

        /// <summary>Jenis stasiun dalam jangkauan. Dipanggil tiap stationCheckSeconds selama panel terbuka atau antrean berisi.</summary>
        public void RefreshAvailability()
        {
            int mask = StationBit(CraftingStationType.Hand);
            var player = IslandPlayerController.Instance;
            var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : null;
            if (player != null)
            {
                float range = Balance != null ? Balance.crafting.stationRangeMeters : 4f;
                float grace = Balance != null ? Balance.crafting.leaveGraceMeters : 2f;
                Vector3 p = player.transform.position;
                Vector3 eye = cam != null ? cam.transform.position : p + Vector3.up * 1.6f;

                for (int i = 0; i < stations.Count; i++)
                {
                    var s = stations[i];
                    if (s == null || !s.isActiveAndEnabled) continue;
                    // Batas panel = range. Job yang sudah berjalan boleh sampai range + grace.
                    bool hasJob = HasJobFor(s.StationType);
                    float limit = IsPanelOpen && !hasJob ? range : range + grace;
                    Vector3 center = s.Center;
                    if ((center - p).sqrMagnitude > limit * limit) continue;

                    // Stasiun dihitung kalau tidak terhalang, atau yang kena adalah stasiun itu sendiri.
                    Vector3 dir = center - eye;
                    float dist = dir.magnitude;
                    if (dist > 0.01f && PhysicsQuery.Raycast(new Ray(eye, dir / dist), out RaycastHit hit, dist, PhysicsMasks.InteractBlock))
                    {
                        if (hit.collider.GetComponentInParent<CraftingStation>() != s) continue;
                    }
                    mask |= StationBit(s.StationType);
                }
            }

            if (mask != availableMask)
            {
                availableMask = mask;
                // Panel menggambar ulang lewat event yang sama dengan antrean.
                PublishQueueChanged();
            }
        }

        private bool HasJobFor(CraftingStationType t)
        {
            for (int i = 0; i < queue.Count; i++)
            {
                var r = RecipeDatabase.Get(queue[i].recipeId);
                if (r != null && r.station == t) return true;
            }
            return false;
        }

        // ---------------------------------------------------------------- Status resep

        public bool IsRecipeUnlocked(CraftingRecipe r)
        {
            if (r == null) return false;
            if (r.startsUnlocked) return true;
            return !string.IsNullOrEmpty(r.techNodeId) && TechTreeManager.Instance != null && TechTreeManager.Instance.IsUnlocked(r.techNodeId);
        }

        // ---------------------------------------------------------------- Memulai crafting

        public CraftResult RequestCraft(CraftingRecipe r)
        {
            var inv = InventoryManager.Instance;
            // 1. Resep api unggun milik CampfireManager (#44).
            if (r == null || r.station == CraftingStationType.Campfire) return CraftResult.NoStation;
            if (!IsRecipeUnlocked(r)) return CraftResult.Locked;
            // 2. Stasiun.
            RefreshAvailability();
            if (!IsStationAvailable(r.station)) return CraftResult.NoStation;
            // 3. Antrean.
            if (queue.Count >= MaxQueue) return CraftResult.QueueFull;
            // 4. Bahan.
            if (inv == null) return CraftResult.MissingMaterial;
            if (r.ingredients != null)
                for (int i = 0; i < r.ingredients.Length; i++)
                {
                    var ing = r.ingredients[i];
                    if (ing.item == null || !inv.HasItem(ing.item.itemID, ing.amount)) return CraftResult.MissingMaterial;
                }
            // 5. Ruang lebih dulu (simulasi tanpa mengubah inventori). Memperbaiki bug lama: bahan hilang padahal penuh.
            if (!inv.CanFitAfterRemoval(r.ingredients, r.result, r.resultAmount)) return CraftResult.NoSpace;
            // 6. Baru potong bahan, lalu masukkan job.
            if (r.ingredients != null)
                for (int i = 0; i < r.ingredients.Length; i++)
                    inv.RemoveItem(r.ingredients[i].item.itemID, r.ingredients[i].amount);
            queue.Add(new CraftJob { recipeId = r.recipeId, remainingSeconds = r.craftSeconds });
            // 7.
            PublishQueueChanged();
            return CraftResult.Ok;
        }

        /// <summary>Batalkan job dan kembalikan seluruh bahan. Bahan yang tidak muat menjadi WorldItem.</summary>
        public void CancelJob(int index)
        {
            if (index < 0 || index >= queue.Count) return;
            var job = queue[index];
            queue.RemoveAt(index);
            var r = RecipeDatabase.Get(job.recipeId);
            if (r != null && r.ingredients != null)
                for (int i = 0; i < r.ingredients.Length; i++)
                    GiveOrDrop(r.ingredients[i].item, r.ingredients[i].amount);
            PublishQueueChanged();
        }

        /// <summary>Pengali kecepatan dari NPC tukang (#76). GDD bagian 3: NPC mempercepat crafting.</summary>
        public void SetNpcSpeedMultiplier(float m) => npcSpeedMultiplier = Mathf.Max(0f, m);

        // ---------------------------------------------------------------- Menjalankan antrean

        private void Update()
        {
            // Tombol C (OpenCrafting, #18) membuka panel. Action Map Gameplay mati saat panel lain terbuka.
            var input = InputManager.Instance;
            var panel = CraftingPanel.Instance;
            if (!stopped && input != null && input.OpenCraftingPressed && panel != null && UIFocusStack.Instance != null
                && !UIFocusStack.Instance.Contains(panel))
                UIFocusStack.Instance.Push(panel);

            if (stopped || queue.Count == 0)
            {
                if (IsPanelOpen) TickAvailability();
                return;
            }
            TickAvailability();

            // Game dijeda: Time.deltaTime = 0, antrean berhenti.
            if (hitPauseTimer > 0f) { hitPauseTimer -= Time.deltaTime; return; }

            // Job pertama yang stasiunnya tersedia. Job meja yang jauh dilewati, jadi resep tangan tetap berjalan.
            for (int i = 0; i < queue.Count; i++)
            {
                var job = queue[i];
                var r = RecipeDatabase.Get(job.recipeId);
                if (r == null) continue;
                if (!IsStationAvailable(r.station)) continue;
                job.remainingSeconds -= Time.deltaTime * npcSpeedMultiplier;
                if (job.remainingSeconds <= 0f) CompleteJob(i, r);
                break;
            }
        }

        private void TickAvailability()
        {
            checkTimer -= Time.deltaTime;
            if (checkTimer > 0f) return;
            checkTimer = Balance != null ? Balance.crafting.stationCheckSeconds : 0.5f;
            RefreshAvailability();
        }

        private void CompleteJob(int index, CraftingRecipe r)
        {
            queue.RemoveAt(index);
            // 1-2. Hasil masuk inventori. Kalau penuh lagi, sisanya jatuh di dekat pemain (despawns false). Hasil tidak pernah hilang.
            if (r.result != null) GiveOrDrop(r.result, r.resultAmount);
            // 3. TODO #16: AudioManager.PlaySFX(completeClip).
            EventChannel<ItemCraftedEvent>.Publish(new ItemCraftedEvent(r.recipeId));
            PublishQueueChanged();
        }

        private static void GiveOrDrop(ItemData item, int amount)
        {
            if (item == null || amount <= 0) return;
            var inv = InventoryManager.Instance;
            int left = inv != null ? inv.AddItem(item, amount) : amount;
            if (left <= 0 || WorldItemManager.Instance == null || inv == null) return;
            var player = IslandPlayerController.Instance;
            Vector3 pos = player != null ? player.transform.position + player.transform.forward + Vector3.up : Vector3.zero;
            WorldItemManager.Instance.Spawn(inv.CreateInstance(item), left, pos, false);
            // TODO #22: notifikasi "Inventori penuh, hasil jatuh".
        }

        private void PublishQueueChanged()
        {
            string active = queue.Count > 0 ? queue[0].recipeId : string.Empty;
            EventChannel<CraftQueueChangedEvent>.Publish(new CraftQueueChangedEvent(queue.Count, active));
        }

        // ---------------------------------------------------------------- Event

        private void HandleTechNodeUnlocked(TechNodeUnlockedEvent e)
        {
            // TODO #22: NotificationManager "Resep baru" untuk resep dengan techNodeId == e.nodeId.
        }

        private void HandlePlayerDamaged(PlayerDamagedEvent e)
        {
            hitPauseTimer = Balance != null ? Balance.crafting.hitPauseSeconds : 1.5f;
        }

        private void HandlePlayerDied(PlayerDiedEvent e)
        {
            stopped = true;
        }

        // ---------------------------------------------------------------- Simpan dan muat (#80)

        public string SaveId => SaveIdConst;
        public int LoadOrder => 110;

        public string CaptureState()
        {
            var data = new CraftingSaveData
            {
                unlockedNodeIds = TechTreeManager.Instance != null ? TechTreeManager.Instance.GetUnlockedIds() : Array.Empty<string>(),
                queue = new CraftJobSave[queue.Count],
            };
            for (int i = 0; i < queue.Count; i++)
                data.queue[i] = new CraftJobSave { recipeId = queue[i].recipeId, remainingSeconds = queue[i].remainingSeconds };
            return JsonUtility.ToJson(data);
        }

        public void RestoreState(string json)
        {
            queue.Clear();
            CraftingSaveData data = null;
            try { data = JsonUtility.FromJson<CraftingSaveData>(json); }
            catch (Exception e) { Debug.LogError($"[CraftingManager] Save tidak terbaca, memakai nilai awal: {e.Message}"); }
            if (data == null) return;

            if (TechTreeManager.Instance != null) TechTreeManager.Instance.RestoreUnlocked(data.unlockedNodeIds);
            else Debug.LogError("[CraftingManager] TechTreeManager belum aktif saat RestoreState. Status node dilewati.");

            if (data.queue == null) return;
            for (int i = 0; i < data.queue.Length; i++)
            {
                // Bahan tidak dikembalikan, karena sudah dipotong saat job dibuat.
                if (RecipeDatabase.Get(data.queue[i].recipeId) == null)
                {
                    Debug.LogWarning($"[CraftingManager] Resep di save tidak ditemukan, job dibuang: {data.queue[i].recipeId}");
                    continue;
                }
                if (queue.Count >= MaxQueue)
                {
                    Debug.LogWarning($"[CraftingManager] Antrean melebihi maxQueue, job dibuang: {data.queue[i].recipeId}");
                    continue;
                }
                queue.Add(new CraftJob { recipeId = data.queue[i].recipeId, remainingSeconds = data.queue[i].remainingSeconds });
            }
        }

        public void OnRestoreFinished()
        {
            RefreshAvailability();
            // Hanya event antrean. ItemCraftedEvent dan TechNodeUnlockedEvent tidak dikirim ulang (notifikasi palsu).
            PublishQueueChanged();
        }
    }
}
