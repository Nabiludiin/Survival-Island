using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Mesin gacha peninggalan peneliti. Blueprint: Island_Logic/LuckyMachine.md (#72, melengkapi #1).
    /// GDD bagian 7: satu mesin di setiap region, dua cara aktivasi (tumbal musuh tertentu di kandang, atau gratis
    /// 30 detik saat bencana besar), hadiah berupa enchant senjata atau alat, skin, buff, dan senjata ajaib.
    /// Claude.md Tahap 5: reward dari array rewards (Gacha RNG).
    ///
    /// Penyimpangan sementara (dicatat di blueprint, "Catatan Implementasi (Langkah 7)"):
    /// - IslandInteractable (#17) belum ditulis, jadi kelas ini menerapkan IInteractable langsung (pola CraftingStation).
    /// - RegionStateStore (#88) belum ditulis, jadi status mesin didaftarkan langsung ke WorldStateSave dengan
    ///   saveId dari SaveIdTag. Ganti ke RegionStateStore saat #88 ada.
    /// </summary>
    [RequireComponent(typeof(SaveIdTag))]
    public class LuckyMachine : MonoBehaviour, IInteractable, ISaveable
    {
        [SerializeField] private LuckyMachineData data;
        [SerializeField] private LuckyMachineCage cage;
        [Tooltip("Titik munculnya hadiah. Kosong berarti cageRewardOffset di depan mesin.")]
        [SerializeField] private Transform rewardPoint;
        [SerializeField] private InteractHighlight highlight;
        [Tooltip("Lampu tanda ada putaran tersimpan.")]
        [SerializeField] private GameObject storedSpinLight;
        [Tooltip("Efek listrik statis saat mode gratis.")]
        [SerializeField] private GameObject freeWindowFx;
        [Tooltip("Pengganti modifier yang lebih lemah dari modifier terpasang (LuckyMachine.md \"Hadiah ganda\"), mis. lucky_ammo.")]
        [SerializeField] private LuckyRewardData duplicateModifierFallback;

        // ---------------------------------------------------------------- Status (tabel "Variabel LuckyMachine")
        private int[] quotaProgress = Array.Empty<int>();
        private int storedSpins;
        private float freeWindowRemaining;
        private float freeWindowEndGameHour = -1f;
        private int freeSpinsUsed;
        private bool isSpinning;
        private CageGateState gateState = CageGateState.Open;

        private float spinLeft;
        private bool spinPaidFromStored;
        private bool spinPaidFromFree;
        private float gateReopenLeft;

        private struct Vacuum
        {
            public EnemyInstance inst;
            public int quotaIndex;
            public float elapsed;
            public Vector3 startPos;
            public Vector3 startScale;
        }
        private readonly List<Vacuum> vacuums = new List<Vacuum>(4);

        private static readonly float[] tierWeights = new float[3];
        private static readonly List<LuckyRewardData> candidates = new List<LuckyRewardData>(16);
        private static readonly List<float> candidateWeights = new List<float>(16);
        private static float[] weightArray = new float[16];

        private SaveIdTag saveTag;

        private LuckyMachineBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.luckyMachine : null;

        // ---------------------------------------------------------------- Properti baca (panel dan HUD)
        public LuckyMachineData Data => data;
        public RegionType Region => data != null ? data.region : RegionType.Volcano;
        public int StoredSpins => storedSpins;
        public float FreeWindowRemaining => freeWindowRemaining;
        public int FreeSpinsUsed => freeSpinsUsed;
        public bool IsSpinning => isSpinning;
        public CageGateState GateState => gateState;
        public int GetQuotaProgress(int index) => index >= 0 && index < quotaProgress.Length ? quotaProgress[index] : 0;
        public bool CanSpinFree => freeWindowRemaining > 0f && Bal != null && freeSpinsUsed < Bal.maxFreeSpinsPerWindow;
        public bool CanSpin => !isSpinning && (CanSpinFree || storedSpins > 0);

        /// <summary>Hadiah terakhir, untuk panel.</summary>
        public LuckyRewardData LastReward { get; private set; }
        public event Action<LuckyMachine> StateChanged;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            saveTag = GetComponent<SaveIdTag>();
            if (cage == null) cage = GetComponentInChildren<LuckyMachineCage>();
            EnsureQuotaArray();
        }

        private void OnEnable()
        {
            EventChannel<MajorDisasterStartedEvent>.Raised += HandleMajorDisaster;
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Register(this);
            ApplyVisuals();
        }

        private void OnDisable()
        {
            EventChannel<MajorDisasterStartedEvent>.Raised -= HandleMajorDisaster;
            // Region dilepas saat vakum: semua vakum langsung selesai, supaya hitungan tidak hilang (bagian 4).
            for (int i = vacuums.Count - 1; i >= 0; i--) FinishSacrifice(vacuums[i]);
            vacuums.Clear();
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
        }

        private void EnsureQuotaArray()
        {
            int n = data != null && data.quotas != null ? data.quotas.Length : 0;
            if (quotaProgress.Length != n) Array.Resize(ref quotaProgress, n);
        }

        // ---------------------------------------------------------------- Kandang dan vakum

        /// <summary>Langkah 2-6 OnTriggerEnter kandang (LuckyMachine.md). true kalau musuh mulai tersedot.</summary>
        public bool TryAcceptSacrifice(EnemyInstance inst)
        {
            if (inst == null || inst.IsSacrificing || data == null) return false;
            var health = inst.Health;
            if (health == null || !health.IsAlive) return false;
            if (inst.AI != null && inst.AI.CurrentState == AIState.BurnToAshes) return false;

            // Kandang hanya menerima enemyId yang ada di kuota mesin ini (keputusan 6).
            int q = data.IndexOfQuota(inst.Data != null ? inst.Data.enemyId : null);
            if (q < 0) return false;

            var bal = Bal;
            if (bal == null) return false;
            if (storedSpins >= bal.maxStoredSpins || gateState == CageGateState.Closed) return false;

            inst.IsSacrificing = true;
            StartVacuum(inst, q);
            return true;
        }

        private void StartVacuum(EnemyInstance inst, int quotaIndex)
        {
            // 1. Kebal, supaya tembakan saat vakum tidak membuat dua jalur kematian.
            inst.Health.SetInvulnerable(true);
            // 2. Matikan AI, agen, dan keluarkan dari daftar target.
            if (inst.AI != null)
            {
                if (ThreatSystem.Instance != null) ThreatSystem.Instance.Unregister(inst.AI);
                inst.AI.enabled = false;
            }
            if (inst.Wildlife != null)
            {
                if (ThreatSystem.Instance != null) ThreatSystem.Instance.Unregister(inst.Wildlife);
                inst.Wildlife.enabled = false;
            }
            if (inst.Agent != null) inst.Agent.enabled = false;

            // 3.
            vacuums.Add(new Vacuum
            {
                inst = inst,
                quotaIndex = quotaIndex,
                elapsed = 0f,
                startPos = inst.transform.position,
                startScale = inst.transform.localScale
            });
            // TODO #15/#114: partikel dan suara vakum lewat VFXManager dan AudioManager.
        }

        private void TickVacuums(float dt, LuckyMachineBalance bal)
        {
            if (vacuums.Count == 0) return;
            float dur = Mathf.Max(0.01f, bal.vacuumSeconds);
            Vector3 target = cage != null ? cage.VacuumPoint : transform.position;
            for (int i = vacuums.Count - 1; i >= 0; i--)
            {
                var v = vacuums[i];
                if (v.inst == null || !v.inst.gameObject.activeInHierarchy) { vacuums.RemoveAt(i); continue; }
                v.elapsed += dt;
                float t = Mathf.Clamp01(v.elapsed / dur);
                v.inst.transform.position = Vector3.Lerp(v.startPos, target, t);
                v.inst.transform.localScale = Vector3.Lerp(v.startScale, v.startScale * bal.vacuumEndScale, t);
                vacuums[i] = v;
                if (t >= 1f)
                {
                    vacuums.RemoveAt(i);
                    FinishSacrifice(v);
                }
            }
        }

        private void FinishSacrifice(Vacuum v)
        {
            var inst = v.inst;
            if (inst == null) return;
            string enemyId = inst.Data != null ? inst.Data.enemyId : string.Empty;

            // 1-2.
            EnsureQuotaArray();
            if (v.quotaIndex >= 0 && v.quotaIndex < quotaProgress.Length) quotaProgress[v.quotaIndex]++;
            EventChannel<EnemySacrificedEvent>.Publish(new EnemySacrificedEvent(enemyId));

            // Kembalikan skala dan komponen sebelum masuk pool. EnemyInstance.OnSpawned (Langkah 5) belum mengembalikan
            // skala dan AI, jadi mesin yang mengembalikannya (aturan Reset Pool, Claude.md Tahap 6).
            inst.transform.localScale = v.startScale;

            // 3. Jalur tunggal kembali ke pool. Tidak ada EnemyKilledEvent, drop, atau RefundBudget.
            if (SpawnManager.Instance != null) SpawnManager.Instance.Despawn(inst.gameObject, DespawnReason.Sacrificed);
            else if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(inst.gameObject);

            // Objek sudah nonaktif, jadi menyalakan komponen tidak memanggil OnEnable sampai spawn berikutnya.
            if (inst.AI != null) inst.AI.enabled = true;
            if (inst.Wildlife != null) inst.Wildlife.enabled = true;
            if (inst.Agent != null) inst.Agent.enabled = true;
            if (inst.Health != null) inst.Health.SetInvulnerable(false);

            // 4. Kuota penuh: 1 putaran tersimpan.
            var bal = Bal;
            if (data != null && v.quotaIndex >= 0 && v.quotaIndex < data.quotas.Length
                && quotaProgress[v.quotaIndex] >= Mathf.Max(1, data.quotas[v.quotaIndex].required))
            {
                quotaProgress[v.quotaIndex] = 0;
                storedSpins++;
                // Notifikasi "Lucky Machine siap diputar" (#22).
                if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.lucky_ready", NotificationKind.Objective);
            }

            // 5. Tabungan penuh: gerbang tertutup sampai pemain memutar (keputusan 5).
            if (bal != null && storedSpins >= bal.maxStoredSpins) SetGate(CageGateState.Closed);
            ApplyVisuals();
            StateChanged?.Invoke(this);
        }

        private void SetGate(CageGateState state)
        {
            gateState = state;
            if (cage != null) cage.SetGate(state);
        }

        // ---------------------------------------------------------------- Mode gratis

        private void HandleMajorDisaster(MajorDisasterStartedEvent e)
        {
            var bal = Bal;
            if (bal == null) return;
            // GDD bagian 7: 30 detik. durationSeconds bencana sengaja tidak dipakai.
            // Bencana kedua saat mode gratis mengisi ulang waktu dan batas.
            freeWindowRemaining = bal.freeWindowSeconds;
            freeSpinsUsed = 0;
            freeWindowEndGameHour = CurrentGameHour() + FreeWindowGameHours(bal.freeWindowSeconds);
            SetGate(CageGateState.Open);
            ApplyVisuals();
            EventChannel<LuckyMachineFreeWindowChangedEvent>.Publish(
                new LuckyMachineFreeWindowChangedEvent(Region, true, bal.freeWindowSeconds));
            StateChanged?.Invoke(this);
        }

        private static float CurrentGameHour() =>
            DayNightDirector.Instance != null ? (float)DayNightDirector.Instance.TotalGameHours : ItemUseSystem.CurrentTotalGameHours();

        private static float FreeWindowGameHours(float seconds)
        {
            var bal = GameManager.CurrentBalance;
            float mult = bal != null ? bal.time.timeMultiplier : 96f;
            return seconds * mult / 3600f;
        }

        private void EndFreeWindow()
        {
            freeWindowRemaining = 0f;
            freeWindowEndGameHour = -1f;
            ApplyVisuals();
            EventChannel<LuckyMachineFreeWindowChangedEvent>.Publish(new LuckyMachineFreeWindowChangedEvent(Region, false, 0f));
            StateChanged?.Invoke(this);
        }

        // ---------------------------------------------------------------- Per frame

        private void Update()
        {
            var bal = Bal;
            if (bal == null) return;
            // Semua memakai Time.deltaTime, jadi berhenti saat jeda. Panel mesin sendiri tidak menjeda.
            float dt = Time.deltaTime;

            TickVacuums(dt, bal);

            if (freeWindowRemaining > 0f)
            {
                freeWindowRemaining -= dt;
                // TODO #114: lima detik terakhir berkedip dan berbunyi.
                if (freeWindowRemaining <= 0f) EndFreeWindow();
            }

            if (gateReopenLeft > 0f)
            {
                gateReopenLeft -= dt;
                if (gateReopenLeft <= 0f && storedSpins < bal.maxStoredSpins) SetGate(CageGateState.Open);
            }

            if (isSpinning)
            {
                spinLeft -= dt;
                if (spinLeft <= 0f) ResolveSpin();
            }
        }

        // ---------------------------------------------------------------- Memutar

        /// <summary>LuckyMachine.md bagian 2 "TrySpin". Dipanggil tombol putar di LuckyMachinePanel.</summary>
        public bool TrySpin()
        {
            var bal = Bal;
            if (bal == null || isSpinning || data == null) return false;

            spinPaidFromFree = false;
            spinPaidFromStored = false;
            if (CanSpinFree)
            {
                freeSpinsUsed++;
                spinPaidFromFree = true;
            }
            else if (storedSpins > 0)
            {
                storedSpins--;
                spinPaidFromStored = true;
                gateReopenLeft = bal.gateReopenSeconds;
            }
            else return false;

            // Putaran yang sudah dimulai tetap selesai, walau waktu gratis habis.
            isSpinning = true;
            spinLeft = bal.spinSeconds;
            ApplyVisuals();
            StateChanged?.Invoke(this);
            return true;
        }

        private void ResolveSpin()
        {
            isSpinning = false;
            spinPaidFromFree = false;
            spinPaidFromStored = false;
            var reward = RollReward();
            if (reward != null) GrantReward(reward);
            LastReward = reward;
            StateChanged?.Invoke(this);
        }

        private LuckyRewardData RollReward()
        {
            var bal = Bal;
            var ledger = LuckyRewardLedger.Instance;
            var seed = RunSeedManager.Instance;
            if (bal == null || data == null || data.rewards == null || seed == null) return null;

            // 1. Aliran berurutan LuckyMachine (#89); status aliran ikut tersimpan, jadi hasil sama setelah load.
            ref var rng = ref seed.GetStream(RandomStreamId.LuckyMachine);

            // 2. Pilih tier (keputusan 7: S 5, A 25, B 70).
            tierWeights[(int)LuckyTier.B] = bal.tierWeightB;
            tierWeights[(int)LuckyTier.A] = bal.tierWeightA;
            tierWeights[(int)LuckyTier.S] = bal.tierWeightS;
            int tier = rng.PickWeighted(tierWeights);
            if (tier < 0) tier = (int)LuckyTier.B;

            // 3-4. Buang hadiah yang tidak sah; kalau tier kosong, turun ke tier di bawahnya.
            for (int t = tier; t >= 0; t--)
            {
                candidates.Clear();
                candidateWeights.Clear();
                for (int i = 0; i < data.rewards.Length; i++)
                {
                    var r = data.rewards[i];
                    if (r == null || (int)r.tier != t || r.weight <= 0f) continue;
                    if (ledger != null && !ledger.IsAvailable(r)) continue;
                    candidates.Add(r);
                    candidateWeights.Add(r.weight);
                }
                if (candidates.Count == 0) continue;

                // 5.
                if (weightArray.Length != candidateWeights.Count) weightArray = new float[candidateWeights.Count];
                candidateWeights.CopyTo(weightArray);
                int pick = rng.PickWeighted(weightArray);
                var chosen = pick >= 0 ? candidates[pick] : candidates[0];
                candidates.Clear();
                return chosen;
            }
            candidates.Clear();
            Debug.LogWarning($"[LuckyMachine] Tidak ada hadiah sah di mesin {name}. Tier B harus berisi hadiah tanpa batas.");
            return null;
        }

        private void GrantReward(LuckyRewardData r)
        {
            bool granted = true;
            switch (r.kind)
            {
                case LuckyRewardKind.SpawnItem: granted = GrantSpawnItem(r); break;
                case LuckyRewardKind.WeaponModifier: granted = GrantModifier(r); break;
                case LuckyRewardKind.Skin: GrantSkin(r); break;
                case LuckyRewardKind.PermanentBuff:
                    // Buff permanen lewat StatusEffectManager (#20). Permanent, Intensify, dan maxStacks diatur di aset StatusEffectData.
                    granted = StatusEffectManager.Instance != null && StatusEffectManager.Instance.Apply(r.buff, "lucky_machine");
                    break;
            }
            if (!granted) return;
            if (LuckyRewardLedger.Instance != null) LuckyRewardLedger.Instance.RecordGrant(r.rewardId);
            EventChannel<LuckyMachineSpinResolvedEvent>.Publish(new LuckyMachineSpinResolvedEvent(r.rewardId));
        }

        private Vector3 RewardPosition()
        {
            if (rewardPoint != null) return rewardPoint.position;
            float off = Bal != null ? Bal.cageRewardOffset : 2f;
            return transform.position + transform.forward * off + Vector3.up * 0.5f;
        }

        private bool GrantSpawnItem(LuckyRewardData r)
        {
            var inv = InventoryManager.Instance;
            var world = WorldItemManager.Instance;
            if (r.item == null || inv == null || world == null) return false;
            var inst = inv.CreateInstance(r.item);
            if (r.modifier != null) ApplyModifier(inst, r.modifier);
            // Tidak hilang, dan tidak bergantung pada inventori penuh.
            world.Spawn(inst, Mathf.Max(1, r.amount), RewardPosition(), false);
            return true;
        }

        private static void ApplyModifier(ItemInstance inst, WeaponModifierData mod)
        {
            inst.luckyModifierId = mod.modifierId;
            if (mod.indestructible) inst.durability = -1f;
        }

        private bool GrantModifier(LuckyRewardData r)
        {
            if (r.modifier == null) return false;
            var target = FindModifierTarget();
            if (target == null)
            {
                // Tidak ada senjata atau alat: hadiah pengganti agar putaran tidak terbuang.
                if (duplicateModifierFallback != null && duplicateModifierFallback != r) GrantReward(duplicateModifierFallback);
                return false;
            }

            // Satu modifier per senjata. Modifier baru menggantikan kalau damage * speed lebih besar.
            var existing = FindModifier(target.luckyModifierId);
            if (existing != null && existing.Strength >= r.modifier.Strength)
            {
                if (duplicateModifierFallback != null && duplicateModifierFallback != r) GrantReward(duplicateModifierFallback);
                return false;
            }
            ApplyModifier(target, r.modifier);
            if (EquipmentManager.Instance != null) EquipmentManager.Instance.RefreshEquipped();
            return true;
        }

        /// <summary>Item yang dipegang kalau senjata atau alat (keputusan 13), lalu Base_Pistol di inventori.</summary>
        private static ItemInstance FindModifierTarget()
        {
            var eq = EquipmentManager.Instance;
            var held = eq != null ? eq.ActiveInstance : null;
            if (held != null)
            {
                var d = ItemDatabase.Get(held.itemId);
                if (d != null && (d.type == ItemType.Weapon || d.type == ItemType.Tool)) return held;
            }
            var inv = InventoryManager.Instance;
            return inv != null ? inv.FindFirstInstance("Base_Pistol") : null;
        }

        private WeaponModifierData FindModifier(string modifierId)
        {
            if (string.IsNullOrEmpty(modifierId) || data == null || data.rewards == null) return null;
            for (int i = 0; i < data.rewards.Length; i++)
            {
                var m = data.rewards[i] != null ? data.rewards[i].modifier : null;
                if (m != null && m.modifierId == modifierId) return m;
            }
            return null;
        }

        private static void GrantSkin(LuckyRewardData r)
        {
            var inv = InventoryManager.Instance;
            var inst = inv != null ? inv.FindFirstInstance(r.skinTargetItemId) : null;
            if (inst != null)
            {
                inst.skinId = r.skinId;
                if (EquipmentManager.Instance != null) EquipmentManager.Instance.RefreshEquipped();
            }
            else if (LuckyRewardLedger.Instance != null)
            {
                // Item belum dimiliki: dipasang saat item didapat.
                LuckyRewardLedger.Instance.AddPendingSkin(r.skinId, r.skinTargetItemId);
            }
        }

        private void ApplyVisuals()
        {
            if (storedSpinLight != null) storedSpinLight.SetActive(storedSpins > 0);
            if (freeWindowFx != null) freeWindowFx.SetActive(freeWindowRemaining > 0f);
            if (cage != null) cage.SetGate(gateState);
        }

        // ---------------------------------------------------------------- IInteractable

        public bool IsAvailable => isActiveAndEnabled;
        public float HoldSeconds => 0f;
        public string GetPrompt() { var loc = LocalizationManager.Instance; return loc != null && loc.Has("prompt.lucky_machine") ? loc.Get("prompt.lucky_machine") : "Lucky Machine"; }   // #104

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            return ctx.IsCarryingHeavyItem ? InteractBlockReason.CarryingHeavyItem : InteractBlockReason.None;
        }

        public void OnHoverEnter() { if (highlight != null) highlight.SetState(HighlightState.Available); }
        public void OnHoverExit() { if (highlight != null) highlight.SetState(HighlightState.Off); }

        /// <summary>Membuka panel LuckyMachine lewat UIFocusStack. Panel tidak menjeda game.</summary>
        public bool Interact(in InteractContext ctx)
        {
            var panel = LuckyMachinePanel.Instance;
            if (panel == null || UIFocusStack.Instance == null) return false;
            panel.Bind(this);
            return UIFocusStack.Instance.Push(panel);
        }

        // ---------------------------------------------------------------- ISaveable

        [Serializable]
        private class SaveData
        {
            public int[] quotaProgress;
            public int storedSpins;
            public float freeWindowEndGameHour;
            public int freeSpinsUsed;
            public CageGateState gateState;
        }

        public string SaveId => saveTag != null ? saveTag.SaveId : name;
        public int LoadOrder => 140;

        public string CaptureState()
        {
            // Putaran belum dianggap dibayar sampai hadiah keluar: save menyimpan nilai sebelum dipotong.
            int stored = storedSpins + (isSpinning && spinPaidFromStored ? 1 : 0);
            int freeUsed = freeSpinsUsed - (isSpinning && spinPaidFromFree ? 1 : 0);
            return JsonUtility.ToJson(new SaveData
            {
                quotaProgress = (int[])quotaProgress.Clone(),
                storedSpins = stored,
                freeWindowEndGameHour = freeWindowRemaining > 0f ? freeWindowEndGameHour : -1f,
                freeSpinsUsed = Mathf.Max(0, freeUsed),
                gateState = gateState
            });
        }

        public void RestoreState(string json)
        {
            var d = string.IsNullOrEmpty(json) ? null : JsonUtility.FromJson<SaveData>(json);
            if (d == null) return;
            EnsureQuotaArray();
            for (int i = 0; i < quotaProgress.Length; i++)
                quotaProgress[i] = d.quotaProgress != null && i < d.quotaProgress.Length ? d.quotaProgress[i] : 0;
            storedSpins = Mathf.Max(0, d.storedSpins);
            freeSpinsUsed = Mathf.Max(0, d.freeSpinsUsed);
            freeWindowEndGameHour = d.freeWindowEndGameHour;
            gateState = d.gateState;
            isSpinning = false;
        }

        public void OnRestoreFinished()
        {
            // Sisa mode gratis dihitung dari jam game saat ini. Kalau sudah lewat, mode gratis berakhir.
            if (freeWindowEndGameHour > 0f)
            {
                float mult = GameManager.CurrentBalance != null ? GameManager.CurrentBalance.time.timeMultiplier : 96f;
                float leftHours = freeWindowEndGameHour - CurrentGameHour();
                freeWindowRemaining = leftHours > 0f && mult > 0f ? leftHours * 3600f / mult : 0f;
                if (freeWindowRemaining <= 0f) freeWindowEndGameHour = -1f;
            }
            else freeWindowRemaining = 0f;
            ApplyVisuals();
        }
    }
}
