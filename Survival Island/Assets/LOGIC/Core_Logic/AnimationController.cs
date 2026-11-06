using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Hash parameter Animator, dibuat sekali (tanpa string per frame). Blueprint: Core_Logic/AnimationController.md.</summary>
    public static class AnimIds
    {
        public static readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int Reload = Animator.StringToHash("Reload");
        public static readonly int ReloadSpeed = Animator.StringToHash("ReloadSpeed");
        public static readonly int Fire = Animator.StringToHash("Fire");
        public static readonly int Swing = Animator.StringToHash("Swing");
        // Keputusan developer (sistem pendukung): trigger dorong tangan untuk PlayerShove.
        public static readonly int Shove = Animator.StringToHash("Shove");
        public static readonly int Equip = Animator.StringToHash("Equip");
        public static readonly int CancelAction = Animator.StringToHash("CancelAction");
        public static readonly int Carry = Animator.StringToHash("Carry");
        public static readonly int Stagger = Animator.StringToHash("Stagger");
        public static readonly int Knockback = Animator.StringToHash("Knockback");
        public static readonly int Die = Animator.StringToHash("Die");
        public static readonly int Burn = Animator.StringToHash("Burn");
        public static readonly int Emerge = Animator.StringToHash("Emerge");
        public static readonly int Dormant = Animator.StringToHash("Dormant");
        public static readonly int Alert = Animator.StringToHash("Alert");
        // Nilai AttackData.animTrigger yang dipakai musuh.
        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int Bite = Animator.StringToHash("Bite");
        public static readonly int HeavyAttack = Animator.StringToHash("HeavyAttack");
    }

    /// <summary>
    /// Pembungkus Animator untuk tangan pemain, zombie, hewan, dan NPC.
    /// Blueprint: Core_Logic/AnimationController.md (#12). GDD bagian 3 (zombie terbakar, combat, Diterkam); bentuk animasi usulan.
    ///
    /// Animasi hanya tampilan: timer gameplay tetap di sistem pemiliknya (GunController.reloadEndTime, EnemyAttack.phaseTimer).
    /// Root motion mati, karena gerak milik NavMeshAgent dan CharacterController.
    /// animator.enabled dan cullingMode milik HordeCrowdManager (#93) dan OptimizationManager; kelas ini hanya menyalakan
    /// Animator saat keluar dari pool.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public class AnimationController : MonoBehaviour, IPoolable
    {
        [Tooltip("Kecepatan yang berarti Speed = 1. Musuh: diisi EnemyAnimDriver dari ZombieData.moveSpeed atau AnimalData.chaseSpeed.")]
        public float referenceSpeed = 1f;
        [Tooltip("Tujuan AE_AttackHit. Kosong: dicari di parent.")]
        [SerializeField] private EnemyAttack attackRelay;

        private Animator animator;

        public Animator Animator => animator;

        private static AnimationBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.animation : null;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            animator.applyRootMotion = false;
            if (attackRelay == null) attackRelay = GetComponentInParent<EnemyAttack>();
        }

        /// <summary>Isi Speed dengan peredam. Memakai Time.deltaTime, jadi nilainya tidak berubah saat jeda.</summary>
        public void SetLocomotion(float speed) => SetLocomotion(speed, Time.deltaTime);

        /// <summary>Versi untuk pemanggil yang mengisi Speed bergiliran (EnemyAnimDriver): dt = jeda sejak isian terakhir.</summary>
        public void SetLocomotion(float speed, float dt)
        {
            var b = Bal;
            animator.SetFloat(AnimIds.Speed, speed, b != null ? b.locomotionDampSeconds : 0.1f, dt);
        }

        /// <summary>
        /// Trigger. Tidak memeriksa apakah Animator aktif: trigger pada Animator mati menunggu Update berikutnya (bagian 4).
        /// </summary>
        public void Play(int triggerHash) => animator.SetTrigger(triggerHash);

        /// <summary>Trigger dari AttackData.animTrigger (Attack, Bite, HeavyAttack).</summary>
        public void PlayAttack(string animTrigger)
        {
            if (string.IsNullOrEmpty(animTrigger)) return;
            Play(Animator.StringToHash(animTrigger));
        }

        public void SetBool(int hash, bool value) => animator.SetBool(hash, value);

        /// <summary>
        /// Klip reload mengikuti timer GunController, bukan sebaliknya. Kalau batas kecepatan membuat klip lebih panjang,
        /// GunController tetap selesai tepat waktu dan CancelAction memotong sisa klip.
        /// </summary>
        public void PlayReload(float reloadSeconds, float clipSeconds)
        {
            var b = Bal;
            float min = b != null ? b.minReloadAnimSpeed : 0.5f;
            float max = b != null ? b.maxReloadAnimSpeed : 2f;
            float speed = reloadSeconds > 0f ? clipSeconds / reloadSeconds : max;
            animator.SetFloat(AnimIds.ReloadSpeed, Mathf.Clamp(speed, min, max));
            Play(AnimIds.Reload);
        }

        public void CancelAction()
        {
            animator.ResetTrigger(AnimIds.Reload);
            Play(AnimIds.CancelAction);
        }

        /// <summary>
        /// Animation Event dari klip serangan musuh. Unity hanya mengirim event ke objek yang punya Animator,
        /// sedangkan EnemyAttack bisa berada di parent.
        /// </summary>
        public void AE_AttackHit()
        {
            if (attackRelay != null) attackRelay.AE_AttackHit();
        }

        public void OnSpawned()
        {
            if (animator == null) animator = GetComponent<Animator>();
            // Rebind menghapus trigger, bool Dormant, dan state Burn atau Die yang tersisa (bagian 4).
            animator.enabled = true;
            animator.Rebind();
            animator.Update(0f);
        }

        public void OnReturned() { }
    }
}
