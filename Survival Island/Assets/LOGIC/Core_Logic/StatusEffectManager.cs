using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Satuan durasi efek. Nilai mengikuti GLOSSARY.md (#20).</summary>
    public enum EffectDurationUnit { Seconds, GameHours, Permanent }

    /// <summary>Aturan saat efek yang sama diberikan lagi. Nilai mengikuti GLOSSARY.md (#20).</summary>
    public enum EffectStackRule { Refresh, Extend, Intensify, Reject }

    /// <summary>Satu efek yang sedang berjalan (StatusEffectManager.md bagian 1).</summary>
    public struct ActiveEffect
    {
        public StatusEffectData data;
        public string sourceId;          // itemId, jenis penyakit, atau sistem pemanggil
        public int stacks;
        public float remainingSeconds;   // Dipakai saat Seconds
        public float endGameHour;        // Dipakai saat GameHours
    }

    /// <summary>
    /// Pengatur buff dan debuff pada pemain Fase 2. Satu daftar efek aktif, satu aturan tumpuk, dan satu tempat menggabungkan
    /// pengali ke SurvivalStats dan kecepatan gerak. Blueprint: Core_Logic/StatusEffectManager.md (#20).
    /// GDD bagian 7: buff HP dan Stamina permanen dari Lucky Machine. Efek sementara (Buff_Supplement) adalah usulan
    /// yang disetujui developer sebagai nilai awal.
    ///
    /// Komponen scene IslandPhase, di objek pemain. Awake hanya mengisi Instance. SaveId player_effects, LoadOrder 70.
    /// Tampilan ikon ada di StatusEffectBar (bagian HUDManager).
    /// </summary>
    public class StatusEffectManager : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "player_effects";
        public const int SaveDataVersion = 1;
        public const string SupplementId = "Buff_Supplement";

        public static StatusEffectManager Instance { get; private set; }

        [SerializeField] private StatusEffectDatabase database;

        private List<ActiveEffect> active = new List<ActiveEffect>(8);
        private float cachedSpeed = 1f, cachedMaxStamina = 1f, cachedRegen = 1f, cachedMaxHp = 1f;
        private bool cachedBlockRegen;
        private bool stopped;
        private bool registeredForSave;

        private SurvivalStats stats;
        private IslandPlayerController controller;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        public IReadOnlyList<ActiveEffect> Active => active;
        public StatusEffectDatabase Database => database;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
            int cap = Balance != null ? Mathf.Max(1, Balance.statusEffects.maxActive) : 8;
            active = new List<ActiveEffect>(cap);
        }

        private void OnEnable()
        {
            EventChannel<PlayerDiedEvent>.Raised += HandlePlayerDied;
            if (GameManager.Instance != null && GameManager.Instance.currentPhase == GamePhase.IslandSurvival
                && WorldStateSave.Instance != null)
            {
                WorldStateSave.Instance.Register(this);
                registeredForSave = true;
            }
        }

        private void OnDisable()
        {
            EventChannel<PlayerDiedEvent>.Raised -= HandlePlayerDied;
            if (registeredForSave && WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            registeredForSave = false;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void HandlePlayerDied(PlayerDiedEvent e)
        {
            // Mati saat efek berjalan: hitungan dan damage berhenti.
            stopped = true;
        }

        // ---------------------------------------------------------------- Memberi efek

        /// <summary>Pasang efek. false kalau ditolak (Reject, daftar penuh, atau data kosong).</summary>
        public bool Apply(StatusEffectData data, string sourceId)
        {
            if (data == null || string.IsNullOrEmpty(data.effectId)) return false;
            if (stopped) return false;

            // 1-2. Efek yang sama sudah ada: aturan tumpuk.
            int idx = IndexOf(data.effectId);
            if (idx >= 0)
            {
                var e = active[idx];
                switch (data.stackRule)
                {
                    case EffectStackRule.Refresh:
                        ResetDuration(ref e);
                        break;
                    case EffectStackRule.Extend:
                        if (data.durationUnit == EffectDurationUnit.Seconds) e.remainingSeconds += GetDuration(data);
                        else if (data.durationUnit == EffectDurationUnit.GameHours) e.endGameHour += GetDuration(data);
                        break;
                    case EffectStackRule.Intensify:
                        e.stacks = Mathf.Min(Mathf.Max(1, data.maxStacks), e.stacks + 1);
                        ResetDuration(ref e);
                        break;
                    case EffectStackRule.Reject:
                        return false;
                }
                e.sourceId = sourceId;
                active[idx] = e;
                Recalculate();
                EventChannel<StatusEffectChangedEvent>.Publish(new StatusEffectChangedEvent(data.effectId, true, e.stacks));
                return true;
            }

            // 3. Daftar penuh. Debuff wajib selalu masuk dengan menggantikan buff yang paling cepat habis.
            int max = Balance != null ? Mathf.Max(1, Balance.statusEffects.maxActive) : 8;
            if (active.Count >= max)
            {
                int victim = data.isBuff ? -1 : ShortestBuffIndex();
                if (victim < 0)
                {
                    Debug.LogWarning($"[StatusEffectManager] Daftar efek penuh ({max}). {data.effectId} ditolak.", this);
                    return false;
                }
                string removedId = active[victim].data.effectId;
                active.RemoveAt(victim);
                EventChannel<StatusEffectChangedEvent>.Publish(new StatusEffectChangedEvent(removedId, false, 0));
            }

            // 4. Efek baru.
            var fresh = new ActiveEffect { data = data, sourceId = sourceId, stacks = 1 };
            ResetDuration(ref fresh);
            active.Add(fresh);

            // 5.
            Recalculate();
            EventChannel<StatusEffectChangedEvent>.Publish(new StatusEffectChangedEvent(data.effectId, true, 1));
            return true;
        }

        /// <summary>Dipanggil ItemUseSystem.CompleteUse kalau ItemData.appliesEffectId terisi (#66).</summary>
        public bool ApplyById(string effectId, string sourceId)
        {
            var data = database != null ? database.Get(effectId) : null;
            if (data == null)
            {
                Debug.LogWarning($"[StatusEffectManager] effectId {effectId} tidak ada di StatusEffectDatabase.", this);
                return false;
            }
            return Apply(data, sourceId);
        }

        public bool Remove(string effectId)
        {
            int idx = IndexOf(effectId);
            if (idx < 0) return false;
            active.RemoveAt(idx);
            Recalculate();
            EventChannel<StatusEffectChangedEvent>.Publish(new StatusEffectChangedEvent(effectId, false, 0));
            return true;
        }

        public bool Has(string effectId) => IndexOf(effectId) >= 0;

        /// <summary>Sisa detik nyata efek, untuk lingkaran HUD. Permanent = PositiveInfinity, tidak ada = 0.</summary>
        public float GetRemainingSeconds(in ActiveEffect e)
        {
            if (e.data == null) return 0f;
            switch (e.data.durationUnit)
            {
                case EffectDurationUnit.Seconds: return Mathf.Max(0f, e.remainingSeconds);
                case EffectDurationUnit.GameHours: return Mathf.Max(0f, e.endGameHour - NowHours()) * SecondsPerGameHour();
                default: return float.PositiveInfinity;
            }
        }

        /// <summary>Durasi penuh dalam detik nyata, untuk rasio lingkaran HUD. Permanent = PositiveInfinity.</summary>
        public float GetFullSeconds(StatusEffectData data)
        {
            if (data == null) return 0f;
            switch (data.durationUnit)
            {
                case EffectDurationUnit.Seconds: return GetDuration(data);
                case EffectDurationUnit.GameHours: return GetDuration(data) * SecondsPerGameHour();
                default: return float.PositiveInfinity;
            }
        }

        private int IndexOf(string effectId)
        {
            if (string.IsNullOrEmpty(effectId)) return -1;
            for (int i = 0; i < active.Count; i++)
                if (active[i].data != null && active[i].data.effectId == effectId) return i;
            return -1;
        }

        private int ShortestBuffIndex()
        {
            int best = -1;
            float bestLeft = float.PositiveInfinity;
            for (int i = 0; i < active.Count; i++)
            {
                if (active[i].data == null || !active[i].data.isBuff) continue;
                float left = GetRemainingSeconds(active[i]);
                if (best < 0 || left < bestLeft)
                {
                    best = i;
                    bestLeft = left;
                }
            }
            return best;
        }

        private void ResetDuration(ref ActiveEffect e)
        {
            float d = GetDuration(e.data);
            if (e.data.durationUnit == EffectDurationUnit.Seconds) e.remainingSeconds = d;
            else if (e.data.durationUnit == EffectDurationUnit.GameHours) e.endGameHour = NowHours() + d;
        }

        /// <summary>Buff_Supplement memakai angka di GameBalanceConfig (statusEffects.supplementSeconds). Efek lain dari asetnya.</summary>
        private static float GetDuration(StatusEffectData d)
        {
            if (d.effectId == SupplementId && Balance != null) return Balance.statusEffects.supplementSeconds;
            return d.duration;
        }

        private static float GetSpeedMultiplier(StatusEffectData d)
        {
            if (d.effectId == SupplementId && Balance != null) return Balance.statusEffects.supplementSpeedMultiplier;
            return d.moveSpeedMultiplier;
        }

        private static float NowHours() => ItemUseSystem.CurrentTotalGameHours();

        private static float SecondsPerGameHour()
        {
            float mul = Balance != null ? Balance.time.timeMultiplier : 96f;
            return mul > 0f ? 3600f / mul : 0f;
        }

        // ---------------------------------------------------------------- Waktu

        private void Update()
        {
            if (stopped || active.Count == 0) return;

            // 1. Jeda game: deltaTime 0 dan TotalGameHours berhenti.
            float dt = Time.deltaTime;
            if (dt <= 0f) return;

            float now = NowHours();
            bool removedAny = false;
            for (int i = active.Count - 1; i >= 0; i--)
            {
                var e = active[i];
                if (e.data == null)
                {
                    active.RemoveAt(i);
                    removedAny = true;
                    continue;
                }

                // 3. Damage kondisi tanpa PlayerDamagedEvent (tidak membatalkan pemakaian obat, tidak memunculkan indikator #51).
                if (e.data.damagePerSecond > 0f)
                {
                    var s = Stats();
                    if (s != null) s.ApplyVitalDamage(e.data.damagePerSecond * e.stacks * dt, e.data.damageType);
                    if (stopped) return;   // Damage tadi mematikan pemain
                }

                // 2.
                bool expired = false;
                switch (e.data.durationUnit)
                {
                    case EffectDurationUnit.Seconds:
                        e.remainingSeconds -= dt;
                        expired = e.remainingSeconds <= 0f;
                        if (!expired) active[i] = e;
                        break;
                    case EffectDurationUnit.GameHours:
                        expired = now >= e.endGameHour;
                        break;
                }
                if (expired)
                {
                    active.RemoveAt(i);
                    removedAny = true;
                    EventChannel<StatusEffectChangedEvent>.Publish(new StatusEffectChangedEvent(e.data.effectId, false, 0));
                }
            }

            // 4.
            if (removedAny) Recalculate();
        }

        // ---------------------------------------------------------------- Menggabungkan pengali

        /// <summary>Semua efek aktif dikalikan, jadi urutan pemberian tidak mengubah hasil. Tanpa alokasi memori.</summary>
        private void Recalculate()
        {
            float speed = 1f, maxStamina = 1f, maxHp = 1f, regen = 1f;
            bool blockRegen = false;
            for (int i = 0; i < active.Count; i++)
            {
                var e = active[i];
                if (e.data == null) continue;
                float sp = GetSpeedMultiplier(e.data);
                for (int s = 0; s < e.stacks; s++)
                {
                    speed *= sp;
                    maxStamina *= e.data.maxStaminaMultiplier;
                    maxHp *= e.data.maxHpMultiplier;
                    regen *= e.data.staminaRegenMultiplier;
                }
                blockRegen |= e.data.blocksHPRegen;
            }

            var st = Stats();
            if (st != null)
            {
                if (!Mathf.Approximately(maxStamina, cachedMaxStamina) || !Mathf.Approximately(regen, cachedRegen))
                    st.SetPenalty(VitalPenaltySource.StatusEffect, maxStamina, regen);
                if (blockRegen != cachedBlockRegen) st.SetHPRegenBlocked(blockRegen);
                // #72: bonus HP maksimum. Bonus stamina hanya lewat SetPenalty di atas.
                if (!Mathf.Approximately(maxHp, cachedMaxHp)) st.SetMaxBonus(maxHp);
                cachedMaxStamina = maxStamina;
                cachedRegen = regen;
                cachedBlockRegen = blockRegen;
                cachedMaxHp = maxHp;
            }

            var pc = Controller();
            if (pc != null)
            {
                // PlayerTraversal.md: dikalikan dengan pengali lain (Heavy Item 0,5 x suplemen 1,2 = 0,6).
                if (!Mathf.Approximately(speed, cachedSpeed)) pc.SetSpeedModifier(SpeedModifierId.StatusEffect, speed);
                cachedSpeed = speed;
            }
        }

        private SurvivalStats Stats()
        {
            if (stats == null) stats = GetComponent<SurvivalStats>();
            if (stats == null) stats = SurvivalStats.Instance;
            return stats;
        }

        private IslandPlayerController Controller()
        {
            if (controller == null) controller = GetComponent<IslandPlayerController>();
            if (controller == null) controller = IslandPlayerController.Instance;
            return controller;
        }

        // ---------------------------------------------------------------- ISaveable

        [Serializable]
        private struct EffectRecord
        {
            public string effectId;
            public string sourceId;
            public int stacks;
            public float remainingSeconds;
            public float endGameHour;
        }

        [Serializable]
        private class EffectsSaveData
        {
            public int version = SaveDataVersion;
            public EffectRecord[] effects = Array.Empty<EffectRecord>();
        }

        public string SaveId => SaveIdConst;
        public int LoadOrder => 70;

        public string CaptureState()
        {
            var data = new EffectsSaveData { effects = new EffectRecord[active.Count] };
            for (int i = 0; i < active.Count; i++)
            {
                var e = active[i];
                data.effects[i] = new EffectRecord
                {
                    effectId = e.data != null ? e.data.effectId : string.Empty,
                    sourceId = e.sourceId,
                    stacks = e.stacks,
                    remainingSeconds = e.remainingSeconds,
                    endGameHour = e.endGameHour
                };
            }
            return JsonUtility.ToJson(data);
        }

        /// <summary>Berjalan setelah DayNightDirector (LoadOrder 10). Tidak mengirim event.</summary>
        public void RestoreState(string json)
        {
            active.Clear();
            EffectsSaveData data = null;
            try { data = JsonUtility.FromJson<EffectsSaveData>(json); }
            catch (Exception ex) { Debug.LogWarning($"[StatusEffectManager] Data efek rusak: {ex.Message}", this); }
            if (data != null && data.effects != null)
            {
                for (int i = 0; i < data.effects.Length; i++)
                {
                    var r = data.effects[i];
                    var d = database != null ? database.Get(r.effectId) : null;
                    if (d == null)
                    {
                        Debug.LogWarning($"[StatusEffectManager] effectId {r.effectId} dari save tidak ditemukan. Dibuang.", this);
                        continue;
                    }
                    active.Add(new ActiveEffect
                    {
                        data = d, sourceId = r.sourceId, stacks = Mathf.Clamp(r.stacks, 1, Mathf.Max(1, d.maxStacks)),
                        remainingSeconds = r.remainingSeconds, endGameHour = r.endGameHour
                    });
                }
            }
            // Efek GameHours yang sudah lewat dihapus pada Update pertama.
            Recalculate();
        }

        /// <summary>Menyegarkan tampilan: kirim keadaan tiap efek supaya StatusEffectBar menggambar ulang.</summary>
        public void OnRestoreFinished()
        {
            for (int i = 0; i < active.Count; i++)
                if (active[i].data != null)
                    EventChannel<StatusEffectChangedEvent>.Publish(new StatusEffectChangedEvent(active[i].data.effectId, true, active[i].stacks));
        }
    }
}
