using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// HP musuh (zombie dan hewan). Blueprint: Core_Logic/DamagePipeline.md bagian "EnemyHealth" (#59).
    /// GDD bagian 5: armor Zombie Militer dihitung lewat data.resistances (DamageResistanceTable), sesudah pengali bagian tubuh.
    /// EnemyHealth TIDAK mengembalikan dirinya ke pool. Mayat dipegang EnemyHitReaction (#95), bangkai hewan AnimalCarcass (#61),
    /// dan zombie terbakar SunlightExposure (#63).
    /// </summary>
    [DisallowMultipleComponent]
    public class EnemyHealth : MonoBehaviour, IDamageable, IPoolable
    {
        [SerializeField] private EnemyData data;

        public EnemyData Data => data;
        public float CurrentHP { get; private set; }
        public float MaxHP => data != null ? data.maxHP * hpMultiplier : 0f;
        public bool IsAlive { get; private set; } = true;
        public bool IsInvulnerable { get; private set; }

        /// <summary>Event per objek untuk komponen di prefab yang sama (EnemyAI, EnemyHitReaction). Bukan event global.</summary>
        public event Action<DamageInfo> Damaged;
        public event Action<DamageInfo> Died;

        private float hpMultiplier = 1f;

        private void Awake()
        {
            if (data == null) Debug.LogError($"[EnemyHealth] '{name}' tidak punya EnemyData.");
            CurrentHP = data != null ? data.maxHP : 1f;
        }

        public void TakeDamage(in DamageInfo info)
        {
            // 1. Penjaga kematian ganda.
            if (!IsAlive || IsInvulnerable || data == null) return;
            if (info.amount <= 0f) return;
            // 2. Armor dari data target.
            float final = DamageResistanceTable.Apply(data.resistances, info);
            if (final <= 0f) return;
            // 3-4. IsAlive dimatikan lebih dulu, supaya damage pada frame yang sama tidak membunuh dua kali.
            CurrentHP -= final;
            bool isKill = CurrentHP <= 0f;
            if (isKill) { CurrentHP = 0f; IsAlive = false; }
            // 5.
            EventChannel<EnemyHitEvent>.Publish(new EnemyHitEvent(info.hitPoint, info.bodyPart, isKill, info.byPlayer));
            // 6.
            Damaged?.Invoke(info);
            // 7.
            if (isKill)
            {
                Died?.Invoke(info);
                EventChannel<EnemyKilledEvent>.Publish(new EnemyKilledEvent(data.enemyId, transform.position, info.type, info.byPlayer));
            }
        }

        /// <summary>
        /// Pemulihan HP hewan saat kembali ke teritori (WildlifeAI.md). Tambahan Langkah 5 di luar tabel DamagePipeline.md:
        /// pemulihan tidak lewat TakeDamage (damage negatif diabaikan), jadi butuh fungsi sendiri.
        /// </summary>
        public void Heal(float amount)
        {
            if (!IsAlive || amount <= 0f) return;
            CurrentHP = Mathf.Min(MaxHP, CurrentHP + amount);
        }

        /// <summary>Lucky Machine (#72) menahan semua damage selama vakum.</summary>
        public void SetInvulnerable(bool value) => IsInvulnerable = value;

        /// <summary>Diisi SpawnManager setelah objek keluar dari pool (#62, #27). Mengisi ulang HP.</summary>
        public void SetHPMultiplier(float value)
        {
            hpMultiplier = value > 0f ? value : 1f;
            if (data != null) CurrentHP = data.maxHP * hpMultiplier;
        }

        public void OnSpawned()
        {
            IsAlive = true;
            CurrentHP = data != null ? data.maxHP * hpMultiplier : 1f;
            IsInvulnerable = false;
        }

        public void OnReturned()
        {
            hpMultiplier = 1f;
        }
    }
}
