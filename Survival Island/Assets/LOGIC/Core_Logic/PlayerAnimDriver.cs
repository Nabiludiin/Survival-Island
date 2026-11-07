using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Pasangan itemId dan set animasi tangan.</summary>
    [Serializable]
    public struct ViewmodelAnimSet
    {
        public string itemId;
        public AnimatorOverrideController overrideController;
    }

    /// <summary>
    /// Pengisi Speed tangan first-person dan pengganti set animasi per item.
    /// Blueprint: Core_Logic/AnimationController.md bagian "PlayerAnimDriver" (#12).
    /// Usulan disetujui developer: model pemain hanya tangan first-person, tanpa tubuh penuh.
    ///
    /// Dipasang di rig tangan viewmodel (#102), bersama AnimationController. Berlaku di Fase 1 dan Fase 2.
    /// GunController, MeleeCombatSystem, dan PlayerShove memanggil animasi lewat PlayerAnimDriver.Instance.Arms.
    /// </summary>
    [RequireComponent(typeof(AnimationController))]
    public class PlayerAnimDriver : MonoBehaviour
    {
        /// <summary>Rig tangan pemain di scene yang aktif. null kalau belum ada.</summary>
        public static PlayerAnimDriver Instance { get; private set; }

        [Tooltip("CharacterController pemain. Kosong: dicari di parent.")]
        [SerializeField] private CharacterController body;
        [Tooltip("Controller dasar AC_PlayerArms (set Empty). Kosong: controller yang terpasang saat Awake.")]
        [SerializeField] private RuntimeAnimatorController emptySet;
        [SerializeField] private ViewmodelAnimSet[] animSets = new ViewmodelAnimSet[0];

        private AnimationController arms;
        private bool isCarrying;
        private bool stopped;

        public AnimationController Arms => arms;

        private void Awake()
        {
            arms = GetComponent<AnimationController>();
            if (body == null) body = GetComponentInParent<CharacterController>();
            if (emptySet == null) emptySet = GetComponent<Animator>().runtimeAnimatorController;
        }

        private void OnEnable()
        {
            if (Instance == null) Instance = this;
            stopped = false;
            EventChannel<EquippedItemChangedEvent>.Raised += HandleEquipped;
            EventChannel<HeavyItemChangedEvent>.Raised += HandleHeavyItem;
            EventChannel<PlayerDiedEvent>.Raised += HandlePlayerDied;
            // Sticky: dibaca saat aktif. Di ShipPhase tanpa EquipmentManager, tangan tetap memakai set Empty.
            if (EventChannel<HeavyItemChangedEvent>.HasLast) isCarrying = EventChannel<HeavyItemChangedEvent>.Last.isCarried;
            if (EventChannel<EquippedItemChangedEvent>.HasLast && EquipmentManager.Instance != null)
                HandleEquipped(EventChannel<EquippedItemChangedEvent>.Last);
            else ApplySet(emptySet);
        }

        private void OnDisable()
        {
            if (Instance == this) Instance = null;
            EventChannel<EquippedItemChangedEvent>.Raised -= HandleEquipped;
            EventChannel<HeavyItemChangedEvent>.Raised -= HandleHeavyItem;
            EventChannel<PlayerDiedEvent>.Raised -= HandlePlayerDied;
        }

        private void LateUpdate()
        {
            if (stopped || body == null || arms == null) return;
            var t = GameManager.CurrentBalance != null ? GameManager.CurrentBalance.traversal : null;
            if (t == null) return;
            // 1-2. Kecepatan datar: 0 diam, 1 jalan (walkSpeed), 2 lari (sprintSpeed).
            Vector3 v = body.velocity;
            float speed = new Vector2(v.x, v.z).magnitude;
            float walk = Mathf.Max(0.01f, t.walkSpeed);
            float value = speed <= walk
                ? speed / walk
                : 1f + (speed - walk) / Mathf.Max(0.01f, t.sprintSpeed - walk);
            // 3. Membawa barang berat pada 2,5 m/s menghasilkan 0,5, sehingga langkah tangan tampak lambat.
            arms.SetLocomotion(value);
        }

        private void HandleEquipped(EquippedItemChangedEvent e)
        {
            var set = emptySet;
            if (!string.IsNullOrEmpty(e.itemId) && animSets != null)
            {
                for (int i = 0; i < animSets.Length; i++)
                {
                    if (animSets[i].itemId == e.itemId && animSets[i].overrideController != null)
                    {
                        set = animSets[i].overrideController;
                        break;
                    }
                }
            }
            ApplySet(set);
            if (arms != null) arms.Play(AnimIds.Equip);
        }

        private void ApplySet(RuntimeAnimatorController set)
        {
            if (arms == null || arms.Animator == null) return;
            if (set != null && arms.Animator.runtimeAnimatorController != set) arms.Animator.runtimeAnimatorController = set;
            // Mengganti controller mereset parameter, jadi Carry ditulis ulang (bagian 4).
            if (arms.Animator.runtimeAnimatorController != null) arms.SetBool(AnimIds.Carry, isCarrying);
        }

        private void HandleHeavyItem(HeavyItemChangedEvent e)
        {
            isCarrying = e.isCarried;
            if (arms != null && arms.Animator != null && arms.Animator.runtimeAnimatorController != null) arms.SetBool(AnimIds.Carry, isCarrying);
        }

        private void HandlePlayerDied(PlayerDiedEvent e) => stopped = true;
    }
}
