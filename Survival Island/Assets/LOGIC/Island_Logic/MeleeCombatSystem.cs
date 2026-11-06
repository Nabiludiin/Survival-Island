using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pertarungan jarak dekat dengan Pisau, Kapak, dan Beliung lewat raycast pendek dari kamera.
    /// Blueprint: Island_Logic/MeleeCombatSystem.md (#59). GDD bagian 3: Base_Pisau item inti.
    /// Input dari EquipmentManager (PrimaryAction), jadi komponen ini tidak membaca klik kiri.
    /// Damage awal dari ItemData.weaponDamage; jarak dan jeda di prefab viewmodel (data, bukan logika, #84).
    ///
    /// Panen memanggil ResourceNode.ReceiveHit milik Langkah 4, jadi file ini terkompilasi setelah branch Langkah 4 digabung.
    /// Catatan Langkah 5 (TODO): DurabilitySystem.ApplyWear (#113), WeaponStatCalculator (#113), AnimationController (#12),
    /// Fire Axe (#97) belum ada.
    /// </summary>
    public class MeleeCombatSystem : EquippableBehaviour
    {
        [SerializeField] private float fallbackDamage = 25f;   // Dipakai kalau ItemData.weaponDamage 0 (nilai lama blueprint)
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackCooldown = 1f;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip swingClip, surfaceHitClip;

        private float nextAttackTime;
        private bool isInDamageMoment;
        private float meleeDamage;
        private string itemId;

        public override bool CanSwitchNow => !isInDamageMoment;

        public override void OnEquip(ItemInstance instance)
        {
            base.OnEquip(instance);
            itemId = instance != null ? instance.itemId : null;
            var item = itemId != null ? ItemDatabase.Get(itemId) : null;
            meleeDamage = item != null && item.weaponDamage > 0f ? item.weaponDamage : fallbackDamage;
        }

        public override void OnUnequip()
        {
            isInDamageMoment = false;
            base.OnUnequip();
        }

        public override void PrimaryAction(ActionPhase phase)
        {
            if (phase == ActionPhase.Pressed && Time.time >= nextAttackTime) PerformMeleeAttack();
        }

        private void PerformMeleeAttack()
        {
            var bal = GameManager.CurrentBalance;
            var stats = SurvivalStats.Instance;
            // 1. Melee tetap berhasil saat lelah (VitalsRules.md).
            if (stats != null && bal != null) stats.SpendStamina(bal.vitals.meleeStaminaCost, StaminaUse.Melee);
            // 2.
            float cooldown = attackCooldown;
            float damage = meleeDamage;
            if (stats != null && stats.IsExhausted && bal != null)
            {
                cooldown *= bal.vitals.exhaustedMeleeCooldownMultiplier;
                damage *= bal.vitals.exhaustedMeleeDamageMultiplier;
            }
            // 3.
            nextAttackTime = Time.time + cooldown;
            // 4. TODO #12: armsAnim.Play(AnimIds.Swing).
            if (audioSource != null && swingClip != null) audioSource.PlayOneShot(swingClip);
            // 5.
            var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : null;
            if (cam == null) return;
            Transform camT = cam.transform;
            NoiseEmitter.Emit(camT.position, bal != null ? bal.stealth.noiseMelee : 8f, NoiseSourceKind.Melee);
            // 6.
            isInDamageMoment = true;
            if (PhysicsQuery.Raycast(new Ray(camT.position, camT.forward), out var hit, attackRange, PhysicsMasks.Melee))
            {
                // ResourceNode (#73, ditulis Langkah 4): panen. Keausan hanya kalau hasilnya Hit (TODO #113: ApplyWear).
                var node = hit.collider.GetComponentInParent<ResourceNode>();
                if (node != null)
                {
                    var item = itemId != null ? ItemDatabase.Get(itemId) : null;
                    node.ReceiveHit(item != null ? item.toolClass : ToolClass.None, hit.point, true);
                    isInDamageMoment = false;
                    return;
                }
                GameObject player = IslandPlayerController.Instance != null ? IslandPlayerController.Instance.gameObject : gameObject;
                var info = new DamageInfo
                {
                    amount = damage, type = DamageType.Melee, source = player, sourceId = itemId,
                    hitDirection = camT.forward, bodyPart = BodyPart.Torso, byPlayer = true   // hitDirection wajib untuk knockback #95
                };
                bool applied = DamageRouter.ApplyHit(hit, info);
                if (!applied && audioSource != null && surfaceHitClip != null) audioSource.PlayOneShot(surfaceHitClip);
                // TODO #113: DurabilitySystem.ApplyWear setelah damage.
            }
            // 7.
            isInDamageMoment = false;
        }
    }
}
