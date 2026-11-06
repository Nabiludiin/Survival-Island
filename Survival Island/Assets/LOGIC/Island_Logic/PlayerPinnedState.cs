using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Alasan lepas dari terkaman. AttackerDamaged hanya dari damage pemain (keputusan developer).</summary>
    public enum PinReleaseReason { Escaped, EscapedWithKnife, AttackerDamaged, AttackerGone, TimedOut, PlayerDied }

    /// <summary>Musuh yang bisa menahan pemain: Jaguar (WildlifeAI) dan Zombie Tribal (TribalBehavior).</summary>
    public interface IPinAttacker
    {
        string AttackerId { get; }
        Transform LookTarget { get; }
        bool IsPinValid { get; }
        void OnPinReleased(PinReleaseReason reason);
        IDamageable DamageTarget { get; }
    }

    /// <summary>
    /// Pemain tertahan musuh dan cara lepas sendiri. Blueprint: Island_Logic/PlayerPinnedState.md (#110).
    /// GDD bagian 3 (Diterkam): Jaguar dan Zombie Tribal bisa menahan pemain, dan pemain harus melepaskan diri sendiri,
    /// misalnya dengan Pisau. Tanpa bantuan NPC (keputusan developer). Claude.md Pilar 3: PlayerPinnedState.
    /// Keausan Pisau lewat DurabilitySystem (#113) sudah dipakai. Belum diimplementasikan (TODO): mode tahan tombol
    /// (#19, butuh StruggleHeld di InputManager), CameraShake Pinned (#24), animasi tusuk.
    /// </summary>
    public class PlayerPinnedState : MonoBehaviour
    {
        public const string KnifeItemId = "Base_Pisau";

        public static PlayerPinnedState Instance { get; private set; }

        public bool IsPinned { get; private set; }
        public IPinAttacker Attacker { get; private set; }
        public float EscapeProgress { get; private set; }

        private float pinTimer;
        private float knifeCooldownTimer;
        private float graceUntil;
        private float repinImmuneUntil;
        private float lastPressTime = -10f;

        private SurvivalStats stats;
        private PlayerTraversal traversal;
        private PlayerInteractor interactor;

        private static PinnedBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.pinned : null;

        private void Awake()
        {
            stats = GetComponent<SurvivalStats>();
            traversal = GetComponent<PlayerTraversal>();
            interactor = GetComponentInChildren<PlayerInteractor>();
        }

        private void OnEnable()
        {
            Instance = this;
            EventChannel<PlayerDiedEvent>.Raised += HandlePlayerDied;
        }

        private void OnDisable()
        {
            EventChannel<PlayerDiedEvent>.Raised -= HandlePlayerDied;
            if (IsPinned) Release(PinReleaseReason.AttackerGone);
            if (Instance == this) Instance = null;
        }

        /// <summary>Dipanggil penyerang setelah serangannya mengenai pemain. false = penyerang menjalankan jeda sendiri.</summary>
        public bool TryPin(IPinAttacker attacker)
        {
            var b = Bal;
            // 1.
            if (b == null || !b.enabled || attacker == null || IsPinned || Time.time < repinImmuneUntil) return false;
            if (stats != null && !stats.IsAlive) return false;
            if (traversal != null && traversal.Mode == TraversalMode.Swimming) return false;
            // Tolak saat panel Cinematic aktif (#99).
            var ui = UIManager.Instance;
            if (ui != null && ui.cinematicPanel != null && UIFocusStack.Instance != null && UIFocusStack.Instance.Contains(ui.cinematicPanel)) return false;
            // 2.
            Attacker = attacker;
            IsPinned = true;
            EscapeProgress = 0f;
            pinTimer = 0f;
            knifeCooldownTimer = 0f;
            // 3-4.
            if (traversal != null) { traversal.ForceDetach(); traversal.SetLocked(true); }
            if (interactor != null) interactor.SetEnabled(false);
            if (FirstPersonCamera.Instance != null) FirstPersonCamera.Instance.SetForcedLook(attacker.LookTarget);
            // Musuh lain yang boleh memukul dibatasi (#109).
            if (AttackCoordinator.Instance != null && PlayerTargetable.Instance != null)
                AttackCoordinator.Instance.SetAttackerLimit(PlayerTargetable.Instance, b.maxOtherAttackers);
            // 5. EquipmentManager memasang EquipLock.Pinned, UIFocusStack menutup panel, HeavyItemSystem menjatuhkan barang.
            EventChannel<PlayerPinnedEvent>.Publish(new PlayerPinnedEvent(attacker.AttackerId, true));
            return true;
        }

        private void Update()
        {
            if (!IsPinned) return;
            var b = Bal;
            float dt = Time.deltaTime;
            if (b == null || dt <= 0f) return;

            // 1-2.
            if (Attacker == null || !Attacker.IsPinValid) { Release(PinReleaseReason.AttackerGone); return; }
            pinTimer += dt;
            if (pinTimer >= b.maxPinSeconds) { Release(PinReleaseReason.TimedOut); return; }
            if (knifeCooldownTimer > 0f) knifeCooldownTimer -= dt;

            // 3.
            float mult = EscapeMultiplier(b);
            var input = InputManager.Instance;
            bool pressed = input != null && input.StrugglePressed;

            // 4. Menekan berulang. Tekanan lebih cepat dari mashMinPressInterval diabaikan.
            if (pressed && Time.time - lastPressTime >= b.mashMinPressInterval)
            {
                lastPressTime = Time.time;
                EscapeProgress += b.mashProgressPerPress * mult;
                if (stats != null) stats.SpendStamina(b.struggleStaminaPerPress, StaminaUse.Struggle);   // Boleh gagal
            }
            else if (!pressed)
            {
                EscapeProgress = Mathf.Max(0f, EscapeProgress - b.mashDecayPerSec * dt);
            }

            // Pisau: klik kiri, tanpa harus memegang Pisau, selama ada di inventori.
            if (input != null && input.PrimaryPressed) TryKnifeStab(b, mult);
            if (!IsPinned) return;

            // 5. Kemajuan hanya dari pemain sendiri.
            if (EscapeProgress >= 1f) Release(PinReleaseReason.Escaped);
        }

        private void TryKnifeStab(PinnedBalance b, float mult)
        {
            if (knifeCooldownTimer > 0f) return;
            var inv = InventoryManager.Instance;
            if (inv == null || !inv.HasItem(KnifeItemId, 1)) return;
            // Pisau Broken tidak bisa dipakai menusuk (#113).
            var knife = inv.FindFirstInstance(KnifeItemId);
            if (DurabilitySystem.GetState(knife) == DurabilityState.Broken) return;
            var target = Attacker.DamageTarget;
            if (target != null)
            {
                Vector3 attackerPos = Attacker.LookTarget != null ? Attacker.LookTarget.position : transform.position;
                Vector3 camPos = FirstPersonCamera.Instance != null && FirstPersonCamera.Instance.WorldCamera != null
                    ? FirstPersonCamera.Instance.WorldCamera.transform.position : transform.position;
                var info = new DamageInfo
                {
                    amount = b.knifeDamage, type = DamageType.Melee, source = gameObject, sourceId = KnifeItemId,
                    hitPoint = attackerPos, hitDirection = (attackerPos - camPos).normalized, bodyPart = BodyPart.Torso, byPlayer = true
                };
                // Tusukan dikirim langsung ke penyerang, tanpa raycast dan tanpa pengali bagian tubuh (HitboxModifier.md).
                target.TakeDamage(info);
            }
            knifeCooldownTimer = b.knifeCooldownSeconds;
            // Keausan pisau per tusukan (#113).
            DurabilitySystem.ApplyWear(knife, b.knifeDurabilityCost);
            if (!IsPinned) return;   // Penyerang sudah melepas lewat ReleaseByAttacker
            EscapeProgress += b.knifeProgress * mult;
            if (EscapeProgress >= 1f) Release(PinReleaseReason.EscapedWithKnife);
        }

        private float EscapeMultiplier(PinnedBalance b)
        {
            if (stats == null) return 1f;
            float s = stats.EffectiveMaxStamina > 0f ? stats.Stamina / stats.EffectiveMaxStamina : 0f;
            float h = stats.MaxHP > 0f ? stats.HP / stats.MaxHP : 0f;
            float i = stats.MaxInfection > 0f ? stats.Infection / stats.MaxInfection : 0f;
            float mult = Mathf.Lerp(b.staminaEmptyMultiplier, 1f, Mathf.Clamp01(s))
                       * Mathf.Lerp(b.hpEmptyMultiplier, 1f, Mathf.Clamp01(h))
                       * Mathf.Lerp(1f, b.infectionFullMultiplier, Mathf.Clamp01(i));
            return Mathf.Max(mult, b.minEscapeMultiplier);
        }

        public void Release(PinReleaseReason reason)
        {
            // 1-2.
            if (!IsPinned) return;
            var attacker = Attacker;
            Attacker = null;
            IsPinned = false;
            var b = Bal;
            // 3.
            if (traversal != null) traversal.SetLocked(false);
            if (interactor != null) interactor.SetEnabled(true);
            if (FirstPersonCamera.Instance != null) FirstPersonCamera.Instance.SetForcedLook(null);
            if (AttackCoordinator.Instance != null && PlayerTargetable.Instance != null)
                AttackCoordinator.Instance.SetAttackerLimit(PlayerTargetable.Instance, -1);
            // 4.
            if (reason != PinReleaseReason.PlayerDied && b != null)
            {
                graceUntil = Time.time + b.postEscapeGraceSeconds;
                repinImmuneUntil = Time.time + b.repinImmuneSeconds;
                attacker?.OnPinReleased(reason);
            }
            // 5.
            EventChannel<PlayerPinnedEvent>.Publish(new PlayerPinnedEvent(attacker != null ? attacker.AttackerId : string.Empty, false));
        }

        /// <summary>Dipanggil penyerang yang mati, kembali ke pool, atau menerima damage melebihi batas. Penyerang lain diabaikan.</summary>
        public void ReleaseByAttacker(IPinAttacker attacker, PinReleaseReason reason)
        {
            if (!IsPinned || attacker == null || attacker != Attacker) return;
            Release(reason);
        }

        /// <summary>
        /// Hook kecil untuk SurvivalStats.TakeDamage langkah 3 (DamagePipeline.md): damage musuh dibuang selama graceUntil,
        /// dan musuh selain Attacker saat IsPinned dikali otherAttackerDamageMultiplier. Damage lingkungan tidak berubah.
        /// </summary>
        public static float ModifyIncomingEnemyDamage(in DamageInfo info, float amount)
        {
            var p = Instance;
            var b = Bal;
            if (p == null || b == null || info.byPlayer || info.source == null) return amount;
            bool fromEnemy = info.source.GetComponent<EnemyHealth>() != null;
            if (!fromEnemy) return amount;
            if (Time.time < p.graceUntil) return 0f;
            if (p.IsPinned && p.Attacker is Component c && c.gameObject != info.source) return amount * b.otherAttackerDamageMultiplier;
            return amount;
        }

        private void HandlePlayerDied(PlayerDiedEvent e)
        {
            Release(PinReleaseReason.PlayerDied);
        }
    }
}
