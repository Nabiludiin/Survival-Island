using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// HP bangunan pemain (dinding, pintu, lantai). Blueprint: Core_Logic/DamagePipeline.md bagian "BaseHealth" (#59, #47, #79).
    /// GDD bagian 4: base sebagai tempat berlindung yang diserang horde. Ada di tiap prefab struktur, tanpa ISaveable:
    /// data struktur disimpan StructureRegistry (#79). Bangunan jarang dibuat dan dihancurkan, jadi tidak memakai ObjectPooler.
    /// IStructureHealth, BuildableData, StructureDamagedEvent, dan StructureDestroyedEvent ditulis Langkah 4.
    /// </summary>
    [DisallowMultipleComponent]
    public class BaseHealth : MonoBehaviour, IDamageable, IStructureHealth
    {
        [SerializeField] private BuildableData data;

        public BuildableData Data => data;
        public string StructureId { get; private set; }
        public float CurrentHP { get; private set; }
        public float MaxHP => data != null ? Mathf.Max(1f, data.maxHP) : 1f;
        public float HpRatio => CurrentHP / MaxHP;
        public float LastDamageTime { get; private set; }
        public bool IsAlive => !destroyed && CurrentHP > 0f;

        /// <summary>Event per objek untuk StructureVisual (#47).</summary>
        public event Action<float> HpRatioChanged;

        // Pengali tambahan per DamageType (BaseDoor Locked, #78). Nilai awal 1.
        private readonly float[] extraMultiplier = CreateMultipliers();
        private bool destroyed;

        private static float[] CreateMultipliers()
        {
            var a = new float[Enum.GetValues(typeof(DamageType)).Length];
            for (int i = 0; i < a.Length; i++) a[i] = 1f;
            return a;
        }

        private void Awake()
        {
            if (CurrentHP <= 0f && data != null) CurrentHP = data.maxHP;
        }

        /// <summary>Isi data, ID, dan HP saat dibangun atau dipulihkan (#47). Tidak bergantung pada Awake.</summary>
        public void Initialize(BuildableData data, string id, float hp)
        {
            this.data = data;
            StructureId = id;
            destroyed = false;
            CurrentHP = Mathf.Clamp(hp, 0f, MaxHP);
            HpRatioChanged?.Invoke(HpRatio);
        }

        /// <summary>
        /// Pengali bawaan (DamagePipeline.md, usulan disetujui developer #85): peluru dan melee pemain tidak merusak bangunan
        /// sendiri (0), ledakan dan Structural penuh (1), api mengikuti BuildableData.resistances (kayu 1, batu 0).
        /// </summary>
        private static float BuiltInMultiplier(DamageType type)
        {
            return type == DamageType.Bullet || type == DamageType.Melee ? 0f : 1f;
        }

        public void TakeDamage(in DamageInfo info)
        {
            if (!IsAlive || info.amount <= 0f) return;
            float mult = BuiltInMultiplier(info.type)
                       * DamageResistanceTable.GetMultiplier(data != null ? data.resistances : null, info.type, info.bodyPart)
                       * extraMultiplier[(int)info.type];
            float final = info.amount * mult;   // Tanpa batas bawah: pengali 0 tetap 0 (bukan musuh)
            if (final <= 0f) return;
            CurrentHP = Mathf.Max(0f, CurrentHP - final);
            LastDamageTime = Time.time;
            // Posisi dipakai SleepSystem (#67) untuk jarak alarm.
            EventChannel<StructureDamagedEvent>.Publish(new StructureDamagedEvent(StructureId, HpRatio, transform.position));
            HpRatioChanged?.Invoke(HpRatio);
            if (CurrentHP <= 0f) DestroyStructure();
        }

        /// <summary>Perbaikan (#47). Tidak lewat TakeDamage.</summary>
        public void Repair(float amount)
        {
            if (destroyed || amount <= 0f) return;
            CurrentHP = Mathf.Min(MaxHP, CurrentHP + amount);
            HpRatioChanged?.Invoke(HpRatio);
        }

        /// <summary>Pengali tambahan per jenis damage, nilai awal 1. Dipakai BaseDoor saat Locked (#78).</summary>
        public void SetDamageMultiplier(DamageType type, float multiplier)
        {
            extraMultiplier[(int)type] = Mathf.Max(0f, multiplier);
        }

        /// <summary>Bongkar bangunan (#79): event hancur, lalu hapus tanpa lewat TakeDamage.</summary>
        public void Remove()
        {
            DestroyStructure();
        }

        private void DestroyStructure()
        {
            if (destroyed) return;
            destroyed = true;
            EventChannel<StructureDestroyedEvent>.Publish(new StructureDestroyedEvent(StructureId, transform.position));
            gameObject.SetActive(false);
            Destroy(gameObject);   // Bangunan tidak di-pool (DamagePipeline.md)
        }
    }
}
