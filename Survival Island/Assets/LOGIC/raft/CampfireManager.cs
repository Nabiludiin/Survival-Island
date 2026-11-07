using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    public enum CookRowState { Empty, Cooking, Done, Burnt }

    /// <summary>Satu baris masak berisi satu item (CampfireManager.md "Baris masak").</summary>
    [Serializable]
    public class CookRow
    {
        public ItemInstance input;     // Bahan mentah, satu item
        public ItemInstance output;    // Hasil, satu item
        public string recipeId;
        public float progressSeconds;  // Waktu masak yang sudah berjalan
        public float overcookSeconds;  // Waktu sejak matang
        public CookRowState state;

        public void Clear()
        {
            input = null;
            output = null;
            recipeId = string.Empty;
            progressSeconds = 0f;
            overcookSeconds = 0f;
            state = CookRowState.Empty;
        }
    }

    /// <summary>Alasan penolakan aksi panel api unggun, untuk teks kotak berkedip (CampfirePanel).</summary>
    public enum CampfireReject { None, CannotCook, Rotten, NoEmptyRow, FuelFull, NotFuel, NoSpace, TooWindy, Empty }

    /// <summary>
    /// Satu api unggun (prefab Build_Campfire): panel masak, waktu matang, bahan bakar, gosong, dan rebus air.
    /// Tidak ada manager global, jadi pemain boleh punya beberapa api unggun. Blueprint: raft/CampfireManager.md (#44).
    ///
    /// GDD bagian 4 (Diary Bab 1: cara membuat api unggun) dan bagian 3 (lapar dan risiko sakit).
    /// Waktu masak 30 detik dari checklist issue. Bahan bakar, gosong, dan rebus air adalah usulan yang disetujui developer.
    ///
    /// Semua waktu dihitung dari selisih jam game (Simulate), sehingga satu rumus berlaku saat bermain, dijeda, tidur, dan
    /// saat region kembali dimuat.
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - IslandInteractable (#17) belum ada, jadi kelas ini menerapkan IInteractable langsung.
    /// - homeRegion: diisi RegionStreaming.RegionOf (#88) saat dibangun dan disimpan.
    /// - LightSource (#98), NPCRoster (#76), dan BaseTrap (#26) sudah disambung. Api unggun tidak menjalar (FireSystem.md keputusan 2, #97).
    /// </summary>
    public class CampfireManager : MonoBehaviour, IInteractable, ISaveable
    {
        /// <summary>Api unggun yang menyala. Dibaca WarmthSystem (#65) dengan campfire.warmthRadius.</summary>
        public static readonly List<CampfireManager> ActiveLit = new List<CampfireManager>(8);

        /// <summary>Ada api unggun menyala dalam campfire.warmthRadius dari pos (dipakai WarmthSystem #65).</summary>
        public static bool IsNearLit(Vector3 pos)
        {
            var b = GameManager.CurrentBalance;
            float r = b != null ? b.campfire.warmthRadius : 5f;
            float r2 = r * r;
            for (int i = 0; i < ActiveLit.Count; i++)
            {
                var c = ActiveLit[i];
                if (c != null && c.IsLit && (c.transform.position - pos).sqrMagnitude <= r2) return true;
            }
            return false;
        }

        [SerializeField] private ItemData burntFallback;           // Food_Burnt kalau resep tidak mengisi burntResult
        [SerializeField] private Transform skyProbe;                // Titik awal raycast atap
        [SerializeField] private Transform fireVfxPoint;
        [SerializeField] private string fireVfxTag = "VFX_Campfire";
        [SerializeField] private GameObject fireVisual;             // Cadangan tampilan kalau VFXManager belum ada
        [SerializeField] private Light fireLight;
        [Tooltip("LightSource api unggun (#98), countsForAttraction = false. Kosong = dicari di anak.")]
        [SerializeField] private LightSource lightSource;
        [SerializeField] private AudioSource loopAudio;             // Suara api (loop)
        [SerializeField] private AudioSource oneShotAudio;
        [SerializeField] private AudioClip igniteClip;
        [SerializeField] private AudioClip extinguishClip;
        [SerializeField] private InteractHighlight highlight;

        private CookRow[] rows;
        private float fuelSeconds;
        private float lastTickGameHour = -1f;
        private float tickTimer;
        private RegionType homeRegion;
        private bool homeRegionSet;
        private bool isRoofed;
        private VFXHandle fireHandle;
        private bool destroyedHandled;

        public bool IsLit { get; private set; }
        public float FuelSeconds => fuelSeconds;
        public IReadOnlyList<CookRow> Rows => rows;
        public bool IsRoofed => isRoofed;

        /// <summary>Isi berubah. Didengar CampfirePanel saat terbuka (event per objek, bukan event global).</summary>
        public event Action ContentsChanged;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;
        private static float MaxFuel => Balance != null ? Balance.campfire.maxFuelSeconds : 300f;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            EnsureRows();
            if (lightSource == null) lightSource = GetComponentInChildren<LightSource>(true);
        }

        private void Start()
        {
            if (lastTickGameHour < 0f) lastTickGameHour = NowGameHours();
            EnsureHomeRegion();
            RefreshSkyExposure();
            ApplyLitVisual();
        }

        private void OnEnable()
        {
            EventChannel<StructureBuiltEvent>.Raised += HandleStructureBuilt;
            EventChannel<StructureDestroyedEvent>.Raised += HandleStructureDestroyed;
            // Region kembali dimuat: satu kali Simulate dengan selisih jam game.
            if (lastTickGameHour >= 0f)
            {
                RefreshSkyExposure();
                Simulate();
            }
            if (IsLit && !ActiveLit.Contains(this)) ActiveLit.Add(this);
            ApplyLitVisual();
        }

        private void OnDisable()
        {
            EventChannel<StructureBuiltEvent>.Raised -= HandleStructureBuilt;
            EventChannel<StructureDestroyedEvent>.Raised -= HandleStructureDestroyed;
            ActiveLit.Remove(this);
            StopFireVfx();
        }

        private void EnsureRows()
        {
            int count = Balance != null ? Mathf.Max(1, Balance.campfire.cookRowCount) : 3;
            if (rows != null && rows.Length == count) return;
            var old = rows;
            rows = new CookRow[count];
            for (int i = 0; i < count; i++)
            {
                rows[i] = old != null && i < old.Length && old[i] != null ? old[i] : new CookRow();
                if (rows[i].state == CookRowState.Empty) rows[i].Clear();
            }
        }

        private void EnsureHomeRegion()
        {
            if (homeRegionSet) return;
            // Region rumah dari RegionStreaming (#88), cadangan ZoneTracker.Active.
            homeRegion = RegionStreaming.RegionOf(transform.position);
            homeRegionSet = true;
        }

        private static float NowGameHours()
        {
            return DayNightDirector.Instance != null ? (float)DayNightDirector.Instance.TotalGameHours : ItemUseSystem.CurrentTotalGameHours();
        }

        private string StructureId
        {
            get
            {
                var h = GetComponent<IStructureHealth>();
                return h != null ? h.StructureId : string.Empty;
            }
        }

        // ---------------------------------------------------------------- Tick waktu

        private void Update()
        {
            tickTimer += Time.deltaTime;
            float interval = Balance != null ? Balance.campfire.tickIntervalSeconds : 0.25f;
            if (tickTimer < interval) return;
            tickTimer = 0f;
            Simulate();
        }

        private void Simulate()
        {
            float now = NowGameHours();
            float multiplier = Balance != null ? Balance.time.timeMultiplier : 96f;
            // Detik setara: sama dengan detik nyata saat bermain normal, lebih besar saat tidur (#67).
            float sim = (now - lastTickGameHour) * 3600f / Mathf.Max(0.0001f, multiplier);
            lastTickGameHour = now;
            if (sim <= 0f) return;   // Game dijeda: jam game berhenti
            bool changed = TickFire(sim);
            changed |= TickRows(sim);
            if (changed) ContentsChanged?.Invoke();
        }

        private bool TickFire(float sim)
        {
            // 1.
            if (!IsLit) return false;
            // 2.
            WeatherType w = RegionalWeather.Instance != null ? RegionalWeather.Instance.GetWeather(homeRegion) : WeatherType.Clear;
            bool stormKills = Balance == null || Balance.campfire.stormExtinguishes;
            // 3. Usulan disetujui #44: Storm dan Blizzard memadamkan api tak beratap. Sisa bahan bakar tetap ada.
            if (!isRoofed && stormKills && (w == WeatherType.Storm || w == WeatherType.Blizzard))
            {
                Extinguish("notif.campfire_storm");   // "Api padam karena badai"
                return true;
            }
            // 4. Rain menghabiskan bahan bakar 1,5 kali kalau tidak beratap.
            float mult = !isRoofed && w == WeatherType.Rain ? (Balance != null ? Balance.campfire.rainFuelMultiplier : 1.5f) : 1f;
            fuelSeconds -= sim * mult;
            // 5. Checklist #44: api mati kalau kayu habis.
            if (fuelSeconds <= 0f)
            {
                fuelSeconds = 0f;
                Extinguish("notif.campfire_out");   // "Api padam"
            }
            return true;
        }

        private bool TickRows(float sim)
        {
            bool changed = false;
            for (int i = 0; i < rows.Length; i++)
            {
                var row = rows[i];
                var recipe = RecipeDatabase.Get(row.recipeId) as CookingRecipe;
                switch (row.state)
                {
                    case CookRowState.Cooking:
                        // Api mati: progres ditahan, tidak hilang.
                        if (!IsLit || recipe == null) break;
                        // Usulan disetujui 3 (#76): Koki yang Work mempercepat masak.
                        row.progressSeconds += sim * (NPCRoster.Instance != null ? NPCRoster.Instance.GetSkillMultiplier(NPCSkill.Cook) : 1f);
                        changed = true;
                        if (row.progressSeconds >= recipe.EffectiveCookSeconds) CompleteRow(row, recipe);
                        break;

                    case CookRowState.Done:
                        // Api mati tidak membuat gosong.
                        if (!IsLit || recipe == null || !recipe.canBurn) break;
                        row.overcookSeconds += sim;
                        changed = true;
                        if (row.overcookSeconds >= recipe.EffectiveBurnAfterSeconds) BurnRow(row, recipe);
                        break;
                }
            }
            return changed;
        }

        private void CompleteRow(CookRow row, CookingRecipe recipe)
        {
            row.output = CreateResult(recipe.result);
            row.input = null;
            row.overcookSeconds = 0f;
            row.state = CookRowState.Done;
            EventChannel<ItemCookedEvent>.Publish(new ItemCookedEvent(row.recipeId, false));
            NotifyCookedIfNear();
        }

        private void BurnRow(CookRow row, CookingRecipe recipe)
        {
            var burnt = recipe.burntResult != null ? recipe.burntResult : burntFallback;
            if (burnt != null) row.output = CreateResult(burnt);
            row.state = CookRowState.Burnt;
            EventChannel<ItemCookedEvent>.Publish(new ItemCookedEvent(row.recipeId, true));
        }

        /// <summary>ItemInstance baru dengan createdGameHour = sekarang (ItemUseSystem.md: hasil matang mulai segar).</summary>
        private static ItemInstance CreateResult(ItemData item)
        {
            if (item == null) return null;
            var inv = InventoryManager.Instance;
            if (inv != null) return inv.CreateInstance(item);
            return new ItemInstance
            {
                itemId = item.itemID,
                durability = item.maxDurability,
                createdGameHour = item.spoilHours > 0f ? ItemUseSystem.CurrentTotalGameHours() : -1f,
            };
        }

        private void NotifyCookedIfNear()
        {
            if (CampfirePanel.Instance != null && CampfirePanel.Instance.IsBoundTo(this)) return;
            var player = IslandPlayerController.Instance;
            if (player == null || NotificationManager.Instance == null) return;
            float d = Balance != null ? Balance.campfire.cookedNotifyDistance : 15f;
            if ((player.transform.position - transform.position).sqrMagnitude > d * d) return;
            NotificationManager.Instance.Show("notif.food_cooked", NotificationKind.Info);   // "Makanan matang"
        }

        // ---------------------------------------------------------------- Fungsi utama (dipanggil CampfirePanel)

        /// <summary>Memasukkan satu item dari slot inventori ke baris Empty.</summary>
        public CampfireReject InsertToRow(int inventorySlot)
        {
            var inv = InventoryManager.Instance;
            var slot = inv != null ? inv.GetSlot(inventorySlot) : null;
            if (slot == null || slot.IsEmpty) return CampfireReject.Empty;
            // 1.
            var recipe = CookingRecipe.FindByInput(slot.instance.itemId);
            if (recipe == null) return CampfireReject.CannotCook;
            // 2. Usulan disetujui #44: makanan busuk tidak bisa dimasak.
            var data = ItemDatabase.Get(slot.instance.itemId);
            if (ItemUseSystem.GetFreshness(slot.instance, data) == Freshness.Rotten) return CampfireReject.Rotten;
            // 3.
            CookRow row = null;
            for (int i = 0; i < rows.Length; i++)
                if (rows[i].state == CookRowState.Empty) { row = rows[i]; break; }
            if (row == null) return CampfireReject.NoEmptyRow;
            // 4. Baris memuat satu item.
            var single = slot.instance.Clone();
            if (slot.amount > 1) single.instanceId = inv.AllocateInstanceId();
            inv.RemoveFromSlot(inventorySlot, 1);
            row.input = single;
            row.output = null;
            row.recipeId = recipe.recipeId;
            row.state = CookRowState.Cooking;
            row.progressSeconds = 0f;
            row.overcookSeconds = 0f;
            ContentsChanged?.Invoke();
            return CampfireReject.None;
        }

        /// <summary>Menambah bahan bakar dari slot. Usulan disetujui #44: menambah kayu otomatis menyalakan api tanpa alat.</summary>
        public CampfireReject AddFuel(int inventorySlot, int amount)
        {
            var inv = InventoryManager.Instance;
            var slot = inv != null ? inv.GetSlot(inventorySlot) : null;
            if (slot == null || slot.IsEmpty || amount <= 0) return CampfireReject.Empty;
            var data = ItemDatabase.Get(slot.instance.itemId);
            // 1.
            if (data == null || data.fuelSeconds <= 0f) return CampfireReject.NotFuel;
            if (fuelSeconds >= MaxFuel) return CampfireReject.FuelFull;
            // 2.
            amount = Mathf.Min(amount, slot.amount);
            inv.RemoveFromSlot(inventorySlot, amount);
            fuelSeconds = Mathf.Min(MaxFuel, fuelSeconds + data.fuelSeconds * amount);
            // 3.
            var result = CampfireReject.None;
            if (!IsLit) result = Ignite();
            ContentsChanged?.Invoke();
            return result;
        }

        /// <summary>Tombol "Tambah Kayu": mencari slot bahan bakar pertama di inventori.</summary>
        public CampfireReject AddFirstFuel()
        {
            var inv = InventoryManager.Instance;
            if (inv == null) return CampfireReject.Empty;
            for (int i = 0; i < inv.SlotCount; i++)
            {
                var s = inv.GetSlot(i);
                if (s == null || s.IsEmpty) continue;
                var d = ItemDatabase.Get(s.instance.itemId);
                if (d != null && d.fuelSeconds > 0f) return AddFuel(i, 1);
            }
            return CampfireReject.NotFuel;
        }

        public CampfireReject CollectRow(int index)
        {
            if (index < 0 || index >= rows.Length) return CampfireReject.Empty;
            var row = rows[index];
            if (row.output == null || (row.state != CookRowState.Done && row.state != CookRowState.Burnt)) return CampfireReject.Empty;
            var inv = InventoryManager.Instance;
            if (inv == null) return CampfireReject.NoSpace;
            // 1-2. Inventori penuh: hasil tetap di baris, dan tetap bisa gosong.
            int left = inv.AddItem(row.output, 1);
            if (left > 0)
            {
                if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.inventory_full", NotificationKind.Warning);
                return CampfireReject.NoSpace;
            }
            // 3.
            row.Clear();
            ContentsChanged?.Invoke();
            return CampfireReject.None;
        }

        /// <summary>Mengeluarkan bahan yang masih dimasak. Progres hilang, bahan kembali.</summary>
        public CampfireReject RemoveInput(int index)
        {
            if (index < 0 || index >= rows.Length) return CampfireReject.Empty;
            var row = rows[index];
            if (row.state != CookRowState.Cooking || row.input == null) return CampfireReject.Empty;
            var inv = InventoryManager.Instance;
            if (inv == null || inv.AddItem(row.input, 1) > 0)
            {
                if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.inventory_full", NotificationKind.Warning);
                return CampfireReject.NoSpace;
            }
            row.Clear();
            ContentsChanged?.Invoke();
            return CampfireReject.None;
        }

        public void CollectAll()
        {
            for (int i = 0; i < rows.Length; i++)
                if (rows[i].output != null) CollectRow(i);
        }

        // ---------------------------------------------------------------- Api

        private CampfireReject Ignite()
        {
            WeatherType w = RegionalWeather.Instance != null ? RegionalWeather.Instance.GetWeather(homeRegion) : WeatherType.Clear;
            bool stormKills = Balance == null || Balance.campfire.stormExtinguishes;
            if (!isRoofed && stormKills && (w == WeatherType.Storm || w == WeatherType.Blizzard))
            {
                if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.campfire_too_windy", NotificationKind.Info);   // "Terlalu berangin"
                return CampfireReject.TooWindy;
            }
            if (fuelSeconds <= 0f) return CampfireReject.NotFuel;
            IsLit = true;
            lastTickGameHour = NowGameHours();
            if (!ActiveLit.Contains(this)) ActiveLit.Add(this);
            if (oneShotAudio != null && igniteClip != null) oneShotAudio.PlayOneShot(igniteClip);
            ApplyLitVisual();
            return CampfireReject.None;
        }

        private void Extinguish(string notificationKey)
        {
            if (!IsLit) return;
            IsLit = false;
            ActiveLit.Remove(this);
            if (oneShotAudio != null && extinguishClip != null && isActiveAndEnabled) oneShotAudio.PlayOneShot(extinguishClip);
            ApplyLitVisual();
            if (!string.IsNullOrEmpty(notificationKey) && NotificationManager.Instance != null && CampfirePanel.Instance != null
                && CampfirePanel.Instance.IsBoundTo(this))
                NotificationManager.Instance.Show(notificationKey, NotificationKind.Info);
        }

        private void ApplyLitVisual()
        {
            bool on = IsLit && isActiveAndEnabled;
            if (fireLight != null) fireLight.enabled = on;
            if (loopAudio != null)
            {
                if (on && !loopAudio.isPlaying) loopAudio.Play();
                else if (!on && loopAudio.isPlaying) loopAudio.Stop();
            }
            // LightSource ikut menyala (#98). Api unggun tidak dihitung dalam tarikan cahaya (countsForAttraction = false).
            if (lightSource != null) lightSource.SetLit(on);
            // BaseTrap TrapKind.Campfire sendiri memeriksa IsLit (#26).
            if (on) StartFireVfx(); else StopFireVfx();
        }

        private void StartFireVfx()
        {
            if (fireHandle.IsValid) return;
            var vfx = VFXManager.Instance;
            Vector3 p = fireVfxPoint != null ? fireVfxPoint.position : transform.position;
            if (vfx != null && !string.IsNullOrEmpty(fireVfxTag)) fireHandle = vfx.Play(fireVfxTag, p, Quaternion.identity, fireVfxPoint != null ? fireVfxPoint : transform);
            if (!fireHandle.IsValid && fireVisual != null) fireVisual.SetActive(true);
        }

        private void StopFireVfx()
        {
            if (fireHandle.IsValid && VFXManager.Instance != null) VFXManager.Instance.Stop(fireHandle);
            fireHandle = default;
            if (fireVisual != null) fireVisual.SetActive(false);
        }

        // ---------------------------------------------------------------- Atap

        private void RefreshSkyExposure()
        {
            float dist = Balance != null ? Balance.campfire.skyCheckDistance : 20f;
            Vector3 origin = skyProbe != null ? skyProbe.position : transform.position + Vector3.up * 2f;
            isRoofed = PhysicsQuery.Raycast(new Ray(origin, Vector3.up), out _, dist, PhysicsMasks.Sunlight);
        }

        private void HandleStructureBuilt(StructureBuiltEvent e) => RecheckIfNear(e.position);

        private void RecheckIfNear(Vector3 pos)
        {
            float r = Balance != null ? Balance.campfire.structureRecheckRadius : 6f;
            if ((pos - transform.position).sqrMagnitude <= r * r) RefreshSkyExposure();
        }

        // ---------------------------------------------------------------- Hancur

        private void HandleStructureDestroyed(StructureDestroyedEvent e)
        {
            string id = StructureId;
            if (string.IsNullOrEmpty(id) || !string.Equals(e.structureId, id, StringComparison.Ordinal))
            {
                RecheckIfNear(e.position);
                return;
            }
            if (destroyedHandled) return;
            destroyedHandled = true;
            Extinguish(null);
            // Tutup panel kalau terikat ke api ini.
            var panel = CampfirePanel.Instance;
            if (panel != null && panel.IsBoundTo(this) && UIFocusStack.Instance != null)
                UIFocusStack.Instance.Close(panel, UICloseReason.Forced);
            // Isi keluar sebagai WorldItem yang hilang otomatis (pool WorldItem_Generic).
            var wim = WorldItemManager.Instance;
            for (int i = 0; i < rows.Length; i++)
            {
                var row = rows[i];
                if (wim != null)
                {
                    Vector3 p = transform.position + Vector3.up * 0.5f + new Vector3((i - 1) * 0.3f, 0f, 0f);
                    if (row.input != null) wim.Spawn(row.input, 1, p, true);
                    if (row.output != null) wim.Spawn(row.output, 1, p, true);
                }
                row.Clear();
            }
            // FireSystem.md (#97) keputusan 2: api unggun tidak menjalar, jadi hancur saat menyala tidak memicu FireArea.
        }

        // ---------------------------------------------------------------- IInteractable

        public bool IsAvailable => isActiveAndEnabled;
        public float HoldSeconds => 0f;
        public string GetPrompt() { var loc = LocalizationManager.Instance; return loc != null && loc.Has("prompt.campfire") ? loc.Get("prompt.campfire") : "Api unggun"; }   // #104

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            return ctx.IsCarryingHeavyItem ? InteractBlockReason.CarryingHeavyItem : InteractBlockReason.None;
        }

        public void OnHoverEnter() { if (highlight != null) highlight.SetState(HighlightState.Available); }
        public void OnHoverExit() { if (highlight != null) highlight.SetState(HighlightState.Off); }

        public bool Interact(in InteractContext ctx)
        {
            var panel = CampfirePanel.Instance;
            var stack = UIFocusStack.Instance;
            if (panel == null || stack == null) return false;
            // Satu panel: Bind mengganti api yang terikat.
            panel.Bind(this);
            if (stack.Push(panel)) return true;
            panel.Bind(null);
            return false;
        }

        // ---------------------------------------------------------------- ISaveable (lewat StructureRecord.extraJson)

        [Serializable]
        private class RowRecord
        {
            public bool hasInput, hasOutput;
            public ItemInstance input;
            public ItemInstance output;
            public string recipeId;
            public float progressSeconds;
            public float overcookSeconds;
            public int state;
        }

        [Serializable]
        private class SaveData
        {
            public float fuelSeconds;
            public bool isLit;
            public float lastTickGameHour;
            public int homeRegion;
            public RowRecord[] rows;
        }

        public string SaveId => StructureId;
        public int LoadOrder => 0;

        public string CaptureState()
        {
            EnsureRows();
            var d = new SaveData
            {
                fuelSeconds = fuelSeconds,
                isLit = IsLit,
                lastTickGameHour = lastTickGameHour,
                homeRegion = (int)homeRegion,
                rows = new RowRecord[rows.Length],
            };
            for (int i = 0; i < rows.Length; i++)
            {
                var r = rows[i];
                // JsonUtility tidak mempertahankan null, jadi ada penanda hasInput/hasOutput.
                d.rows[i] = new RowRecord
                {
                    hasInput = r.input != null,
                    hasOutput = r.output != null,
                    input = r.input,
                    output = r.output,
                    recipeId = r.recipeId,
                    progressSeconds = r.progressSeconds,
                    overcookSeconds = r.overcookSeconds,
                    state = (int)r.state,
                };
            }
            return JsonUtility.ToJson(d);
        }

        public void RestoreState(string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            SaveData d = null;
            try { d = JsonUtility.FromJson<SaveData>(json); }
            catch (Exception e) { Debug.LogWarning($"[CampfireManager] Save tidak terbaca: {e.Message}"); }
            if (d == null) return;
            EnsureRows();
            fuelSeconds = Mathf.Clamp(d.fuelSeconds, 0f, MaxFuel);
            IsLit = d.isLit && fuelSeconds > 0f;
            // Jam game disimpan bersama, jadi Simulate tidak mengejar waktu setelah load.
            lastTickGameHour = d.lastTickGameHour;
            homeRegion = (RegionType)d.homeRegion;
            homeRegionSet = true;
            for (int i = 0; i < rows.Length; i++)
            {
                rows[i].Clear();
                if (d.rows == null || i >= d.rows.Length || d.rows[i] == null) continue;
                var r = d.rows[i];
                rows[i].input = r.hasInput ? r.input : null;
                rows[i].output = r.hasOutput ? r.output : null;
                rows[i].recipeId = r.recipeId;
                rows[i].progressSeconds = r.progressSeconds;
                rows[i].overcookSeconds = r.overcookSeconds;
                rows[i].state = (CookRowState)r.state;
            }
        }

        public void OnRestoreFinished()
        {
            RefreshSkyExposure();
            if (IsLit && isActiveAndEnabled && !ActiveLit.Contains(this)) ActiveLit.Add(this);
            ApplyLitVisual();
            ContentsChanged?.Invoke();
        }
    }
}
