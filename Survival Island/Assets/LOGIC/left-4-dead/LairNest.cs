using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Sarang yang bisa dihancurkan, anak ZombieLair. Blueprint: left-4-dead/ZombieLair.md bagian "Menghancurkan sarang" (#112).
    /// Usulan 3 disetujui developer: sarang hancur mengurangi horde region. Damage apa pun diterima (sesuai IDamageable).
    /// </summary>
    public class LairNest : MonoBehaviour, IDamageable
    {
        [SerializeField] private ZombieLair lair;

        public float CurrentHP { get; private set; }
        public float MaxHP { get; private set; }
        public bool IsAlive => CurrentHP > 0f;

        private void Awake()
        {
            if (lair == null) lair = GetComponentInParent<ZombieLair>();
        }

        /// <summary>Diisi ZombieLair saat Awake dan setelah load.</summary>
        public void Setup(float maxHp, float currentHp)
        {
            MaxHP = Mathf.Max(1f, maxHp);
            CurrentHP = Mathf.Clamp(currentHp, 0f, MaxHP);
            gameObject.SetActive(CurrentHP > 0f);
        }

        public void TakeDamage(in DamageInfo info)
        {
            if (!IsAlive || info.amount <= 0f) return;
            CurrentHP = Mathf.Max(0f, CurrentHP - info.amount);
            if (lair != null) lair.HandleNestDamaged(info);
            if (CurrentHP > 0f) return;
            if (lair != null) lair.HandleNestDestroyed();
            gameObject.SetActive(false);
        }
    }
}
