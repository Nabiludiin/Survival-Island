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
    /// DurabilitySystem.ApplyWear dan WeaponStatCalculator (#113) serta animasi ayun lewat PlayerAnimDriver (#12) sudah dipakai.
    /// Belum ada (TODO): Fire Axe (#97). Keausan Pisau saat bertarung memakai resource.knifeDecayPerCombatHit (keputusan developer).
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
        private ItemInstance instance;

        public override bool CanSwitchNow => !isInDamageMoment;

        public override void OnEquip(ItemInstance instance)
        {
            base.OnEquip(instance);
            this.instance = instance;
            itemId = instance != null ? instance.itemId : null;
            var item = itemId != null ? ItemDatabase.Get(itemId) : null;
            // Damage akhir setelah upgrade dan modifier Lucky Machine (#113).
            meleeDamage = WeaponStatCalculator.GetMeleeDamage(GetBaseDamage(item), instance);
        }

        /// <summary>Damage dasar sebelum upgrade. FistBehaviour menimpanya dengan equipment.fistDamage.</summary>
        protected virtual float GetBaseDamage(ItemData item)
        {
            return item != null && item.weaponDamage > 0f ? item.weaponDamage : fallbackDamage;
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
            // 4. Animasi ayun viewmodel (#12).
            var driver = PlayerAnimDriver.Instance;
            if (driver != null && driver.Arms != null) driver.Arms.Play(AnimIds.Swing);
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
                // ResourceNode (#73, ditulis Langkah 4): panen. Keausan hanya kalau hasilnya Hit (#113).
                var node = hit.collider.GetComponentInParent<ResourceNode>();
                if (node != null)
                {
                    var item = itemId != null ? ItemDatabase.Get(itemId) : null;
                    var tool = item != null ? item.toolClass : ToolClass.None;
                    if (node.ReceiveHit(tool, hit.point, true) == HitResult.Hit) ApplyToolWear(tool, true);
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
                // Keausan setelah damage (#113).
                if (applied) { var weaponItem = itemId != null ? ItemDatabase.Get(itemId) : null; ApplyToolWear(weaponItem != null ? weaponItem.toolClass : ToolClass.None, false); }
            }
            // 7.
            isInDamageMoment = false;
        }

        /// <summary>
        /// DurabilitySystem.md tabel keausan (#113): ayunan Kapak dan Beliung yang kena memakai resource.axeDecayPerUse /
        /// pickaxeDecayPerUse; memanen dengan Pisau memakai resource.knifeDecayPerHarvest, dan tusukan yang kena musuh memakai
        /// resource.knifeDecayPerCombatHit (keputusan developer, sistem pendukung).
        /// </summary>
        private void ApplyToolWear(ToolClass tool, bool harvest)
        {
            var res = GameManager.CurrentBalance != null ? GameManager.CurrentBalance.resource : null;
            if (res == null || instance == null) return;
            float amount = 0f;
            switch (tool)
            {
                case ToolClass.Axe: amount = res.axeDecayPerUse; break;
                case ToolClass.Pickaxe: amount = res.pickaxeDecayPerUse; break;
                case ToolClass.Knife: amount = harvest ? res.knifeDecayPerHarvest : res.knifeDecayPerCombatHit; break;
            }
            if (amount > 0f) DurabilitySystem.ApplyWear(instance, amount);
        }
    }
}
