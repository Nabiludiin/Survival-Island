using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Jenis penyakit sekunder (DiseaseManager.md). DiseaseCure (flag) ada di raft/DiseaseCure.cs.</summary>
    public enum DiseaseType { FoodPoisoning, Venom, Bleeding, WoundInfection }

    /// <summary>Pasangan DiseaseType dan StatusEffectData, diisi di Inspector (DiseaseManager.md "Variabel").</summary>
    [Serializable]
    public class DiseaseEffectTable
    {
        public StatusEffectData foodPoisoning;     // Debuff_FoodPoisoning
        public StatusEffectData venom;             // Debuff_Venom
        public StatusEffectData bleeding;          // Debuff_Bleeding (Permanent, Reject)
        public StatusEffectData woundInfection;    // Debuff_WoundInfection (Permanent, Reject)

        public StatusEffectData Get(DiseaseType t)
        {
            switch (t)
            {
                case DiseaseType.FoodPoisoning: return foodPoisoning;
                case DiseaseType.Venom: return venom;
                case DiseaseType.Bleeding: return bleeding;
                default: return woundInfection;
            }
        }

        public void Set(DiseaseType t, StatusEffectData d)
        {
            switch (t)
            {
                case DiseaseType.FoodPoisoning: foodPoisoning = d; break;
                case DiseaseType.Venom: venom = d; break;
                case DiseaseType.Bleeding: bleeding = d; break;
                default: woundInfection = d; break;
            }
        }
    }

    /// <summary>
    /// Penyakit sekunder di luar infeksi zombie: keracunan makanan, racun ular, pendarahan, dan infeksi luka.
    /// Komponen di pemain Fase 2 (IslandPhase). Blueprint: raft/DiseaseManager.md (#45).
    ///
    /// GDD bagian 3: air kotor berisiko sakit. Keracunan daging mentah 50 persen dari checklist issue. Pendarahan, infeksi luka,
    /// dan obat adalah usulan yang disetujui developer sebagai nilai awal.
    /// Efek pada tubuh (damage, stamina, kecepatan, blok HP regen) dijalankan StatusEffectManager (#20). Kelas ini hanya
    /// menentukan kapan penyakit mulai, berubah, dan sembuh. Tidak mengubah Infection zombie (InfectionSystem #64).
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - Durasi dan damage per detik dari GameBalanceConfig.disease ditulis ke salinan runtime aset StatusEffectData (Awake),
    ///   supaya aset di disk tidak berubah. Pengali stamina dan kecepatan tetap dari aset.
    /// </summary>
    [DisallowMultipleComponent]
    public class DiseaseManager : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "player_disease";

        public static DiseaseManager Instance { get; private set; }

        [SerializeField] private DiseaseEffectTable effects = new DiseaseEffectTable();
        [SerializeField] private SurvivalStats stats;

        private DiseaseCure activeMask = DiseaseCure.None;
        private float bleedingEndGameHour = -1f;
        private readonly DiseaseEffectTable runtime = new DiseaseEffectTable();

        public DiseaseCure ActiveMask => activeMask;
        public bool Has(DiseaseType t) => (activeMask & ToCure(t)) != 0;

        /// <summary>Kedipan hijau aktif untuk FoodPoisoning dan Venom (DiseaseOverlay).</summary>
        public bool IsPoisoned => Has(DiseaseType.FoodPoisoning) || Has(DiseaseType.Venom);

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
            if (stats == null) stats = GetComponent<SurvivalStats>();
            BuildRuntimeEffects();
        }

        private void OnEnable()
        {
            EventChannel<PlayerDamagedEvent>.Raised += HandleDamaged;
            EventChannel<StatusEffectChangedEvent>.Raised += HandleEffectChanged;
            EventChannel<HourChangedEvent>.Raised += HandleHour;
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Register(this);
        }

        private void OnDisable()
        {
            EventChannel<PlayerDamagedEvent>.Raised -= HandleDamaged;
            EventChannel<StatusEffectChangedEvent>.Raised -= HandleEffectChanged;
            EventChannel<HourChangedEvent>.Raised -= HandleHour;
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
            for (int i = 0; i < 4; i++)
            {
                var r = runtime.Get((DiseaseType)i);
                if (r != null) Destroy(r);
            }
        }

        private void BuildRuntimeEffects()
        {
            var b = Balance != null ? Balance.disease : null;
            for (int i = 0; i < 4; i++)
            {
                var t = (DiseaseType)i;
                var src = effects.Get(t);
                if (src == null) { runtime.Set(t, null); continue; }
                var copy = Instantiate(src);
                copy.name = src.name;
                if (b != null)
                {
                    switch (t)
                    {
                        case DiseaseType.FoodPoisoning:
                            copy.duration = b.foodPoisonGameHours;
                            copy.damagePerSecond = b.foodPoisonDamagePerSec;
                            break;
                        case DiseaseType.Venom:
                            copy.duration = b.venomGameHours;
                            copy.damagePerSecond = b.venomDamagePerSec;
                            break;
                        case DiseaseType.Bleeding:
                            // Akhir pendarahan diatur bleedingEndGameHour, bukan durasi efek (Permanent).
                            copy.damagePerSecond = b.bleedingDamagePerSec;
                            break;
                    }
                }
                runtime.Set(t, copy);
            }
        }

        private static DiseaseCure ToCure(DiseaseType t) => (DiseaseCure)(1 << (int)t);

        private string EffectIdOf(DiseaseType t)
        {
            var d = runtime.Get(t);
            return d != null ? d.effectId : string.Empty;
        }

        private bool TryGetDiseaseOfEffect(string effectId, out DiseaseType type)
        {
            for (int i = 0; i < 4; i++)
            {
                var d = runtime.Get((DiseaseType)i);
                if (d != null && string.Equals(d.effectId, effectId, StringComparison.Ordinal))
                {
                    type = (DiseaseType)i;
                    return true;
                }
            }
            type = DiseaseType.FoodPoisoning;
            return false;
        }

        // ---------------------------------------------------------------- Dari makanan

        /// <summary>Dipanggil ItemUseSystem.CompleteUse langkah 7.</summary>
        public void OnConsumed(ItemData item, Freshness freshness)
        {
            if (item == null) return;
            // 1. Obat tidak meracuni.
            if (item.curesDiseases != DiseaseCure.None)
            {
                CureFromItem(item);
                return;
            }
            // 2. Busuk: peluang minimal items.rottenPoisonChance (ItemUseSystem.md).
            float chance = item.poisonChance;
            if (freshness == Freshness.Rotten)
                chance = Mathf.Max(chance, Balance != null ? Balance.items.rottenPoisonChance : 0.8f);
            // 3.
            if (chance <= 0f || RunSeedManager.Instance == null) return;
            // 4. Aliran Gameplay, jadi keluar-masuk tidak mengubah hasil (#89).
            ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Gameplay);
            if (rng.NextFloat01() < chance) Apply(DiseaseType.FoodPoisoning, item.itemID);
        }

        // ---------------------------------------------------------------- Dari serangan

        private void HandleDamaged(PlayerDamagedEvent e)
        {
            // 1. Racun ular dibaca dari DamageType.Poison, sehingga WildlifeAI tidak mengenal sistem ini.
            if (e.info.type == DamageType.Poison)
            {
                Apply(DiseaseType.Venom, e.info.sourceId);
                return;
            }
            // 2. Usulan disetujui #45: 25 persen pada pukulan melee musuh dengan damage minimal 15.
            if (e.info.type != DamageType.Melee || e.info.byPlayer || Has(DiseaseType.Bleeding)) return;
            float minDamage = Balance != null ? Balance.disease.bleedMinDamage : 15f;
            if (e.info.amount < minDamage || RunSeedManager.Instance == null) return;
            float chance = Balance != null ? Balance.disease.bleedChance : 0.25f;
            ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Gameplay);
            if (rng.NextFloat01() < chance) Apply(DiseaseType.Bleeding, e.info.sourceId);
        }

        // ---------------------------------------------------------------- Memberi penyakit

        public void Apply(DiseaseType type, string sourceId)
        {
            // 1.
            if (type == DiseaseType.WoundInfection && Has(DiseaseType.WoundInfection)) return;
            // 2.
            if (type == DiseaseType.Bleeding)
                bleedingEndGameHour = ItemUseSystem.CurrentTotalGameHours() + (Balance != null ? Balance.disease.bleedingGameHours : 6f);
            // 3. Efek sama memakai Refresh di aset, jadi makan daging mentah lagi hanya mengisi durasi.
            var data = runtime.Get(type);
            if (data != null && StatusEffectManager.Instance != null) StatusEffectManager.Instance.Apply(data, sourceId);
            // 4-5.
            activeMask |= ToCure(type);
            EventChannel<DiseaseChangedEvent>.Publish(new DiseaseChangedEvent(type, true));
            Notify(NotificationKeyOf(type));
        }

        private static string NotificationKeyOf(DiseaseType t)
        {
            switch (t)
            {
                case DiseaseType.FoodPoisoning: return "notif.disease_food_poisoning";   // "Kamu keracunan"
                case DiseaseType.Venom: return "notif.disease_venom";                     // "Kamu terkena racun"
                case DiseaseType.Bleeding: return "notif.disease_bleeding";               // "Kamu berdarah"
                default: return "notif.disease_wound_infection";                          // "Lukamu terinfeksi"
            }
        }

        private static void Notify(string key)
        {
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show(key, NotificationKind.Warning);
        }

        // ---------------------------------------------------------------- Akhir penyakit

        private void HandleEffectChanged(StatusEffectChangedEvent e)
        {
            if (e.isActive || !TryGetDiseaseOfEffect(e.effectId, out var type)) return;
            if (!Has(type)) return;
            // 1-2.
            activeMask &= ~ToCure(type);
            if (type == DiseaseType.Bleeding) bleedingEndGameHour = -1f;
            EventChannel<DiseaseChangedEvent>.Publish(new DiseaseChangedEvent(type, false));
        }

        private void HandleHour(HourChangedEvent e)
        {
            // Hanya DiseaseManager yang mengakhiri pendarahan (Debuff_Bleeding = Permanent, Reject).
            if (!Has(DiseaseType.Bleeding) || bleedingEndGameHour < 0f) return;
            if (ItemUseSystem.CurrentTotalGameHours() < bleedingEndGameHour) return;
            // 1. Remove menerbitkan StatusEffectChangedEvent { isActive = false }, yang menghapus bit Bleeding.
            RemoveEffect(DiseaseType.Bleeding);
            // Cadangan kalau StatusEffectManager tidak mengirim event.
            if (Has(DiseaseType.Bleeding))
            {
                activeMask &= ~DiseaseCure.Bleeding;
                EventChannel<DiseaseChangedEvent>.Publish(new DiseaseChangedEvent(DiseaseType.Bleeding, false));
            }
            bleedingEndGameHour = -1f;
            // 2. Usulan disetujui #45: pendarahan yang tidak diobati menjadi infeksi luka.
            Apply(DiseaseType.WoundInfection, "wound");
        }

        private void RemoveEffect(DiseaseType t)
        {
            string id = EffectIdOf(t);
            if (!string.IsNullOrEmpty(id) && StatusEffectManager.Instance != null) StatusEffectManager.Instance.Remove(id);
        }

        // ---------------------------------------------------------------- Obat

        /// <summary>Dipanggil ItemUseSystem.TryUse langkah 2. Obat tanpa penyakit ditolak, kecuali memulihkan HP yang belum penuh.</summary>
        public bool CanUseMedicine(ItemData item)
        {
            if (item == null) return false;
            if ((activeMask & item.curesDiseases) != DiseaseCure.None) return true;
            var s = stats != null ? stats : SurvivalStats.Instance;
            if (item.healAmount > 0f && s != null && s.HP < s.MaxHP) return true;
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.not_needed", NotificationKind.Info);   // "Tidak diperlukan"
            return false;
        }

        private void CureFromItem(ItemData item)
        {
            // Usulan disetujui #45: Norit mengobati keracunan dan racun ular, Antibiotik hanya infeksi luka.
            var cured = activeMask & item.curesDiseases;
            if (cured == DiseaseCure.None) return;
            for (int i = 0; i < 4; i++)
            {
                var t = (DiseaseType)i;
                if ((cured & ToCure(t)) == 0) continue;
                RemoveEffect(t);
                if (t == DiseaseType.Bleeding) bleedingEndGameHour = -1f;
                // Cadangan kalau StatusEffectManager tidak mengirim event akhir.
                if (Has(t))
                {
                    activeMask &= ~ToCure(t);
                    EventChannel<DiseaseChangedEvent>.Publish(new DiseaseChangedEvent(t, false));
                }
            }
        }

        // ---------------------------------------------------------------- ISaveable

        [Serializable]
        private class SaveData
        {
            public int activeMask;
            public float bleedingEndGameHour = -1f;
        }

        public string SaveId => SaveIdConst;
        public int LoadOrder => 70;

        public string CaptureState()
        {
            return JsonUtility.ToJson(new SaveData { activeMask = (int)activeMask, bleedingEndGameHour = bleedingEndGameHour });
        }

        public void RestoreState(string json)
        {
            // Hanya mengisi data. Sisa durasi efek dipulihkan StatusEffectManager; Apply tidak dipanggil ulang.
            if (string.IsNullOrEmpty(json)) return;
            SaveData d = null;
            try { d = JsonUtility.FromJson<SaveData>(json); }
            catch (Exception e) { Debug.LogWarning($"[DiseaseManager] Save tidak terbaca: {e.Message}"); }
            if (d == null) return;
            activeMask = (DiseaseCure)d.activeMask;
            bleedingEndGameHour = d.bleedingEndGameHour;
        }

        public void OnRestoreFinished()
        {
            // Penyakit yang efeknya tidak ikut pulih (aset diganti) dilepas, supaya mask tidak menggantung.
            var sem = StatusEffectManager.Instance;
            if (sem == null) return;
            for (int i = 0; i < 4; i++)
            {
                var t = (DiseaseType)i;
                if (!Has(t)) continue;
                string id = EffectIdOf(t);
                if (!string.IsNullOrEmpty(id) && !sem.Has(id)) activeMask &= ~ToCure(t);
            }
            if (!Has(DiseaseType.Bleeding)) bleedingEndGameHour = -1f;
        }
    }
}
