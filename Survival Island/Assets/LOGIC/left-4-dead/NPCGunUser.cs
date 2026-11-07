using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pembungkus tembakan NPC di prefab NPCCompanion. Blueprint: left-4-dead/GunController.md bagian "Senjata lain" (NPC)
    /// dan Island_Logic/NPCCompanion.md (#58, #76). Claude.md Pilar 3: Raycast Shooting lewat GunCore yang sama dengan pemain.
    /// Peluru tidak memakai inventori dan tidak mengirim AmmoChangedEvent. byPlayer = false, jadi tembakan NPC tidak melepas
    /// terkaman (keputusan developer, PlayerPinnedState.md).
    ///
    /// Catatan implementasi (sistem pendukung): musuh tidak memasang ITargetable (ThreatSystem.md hanya untuk target musuh),
    /// jadi fungsi menerima titik bidik (Vector3), bukan ITargetable. NPCCompanion mengisi AimPoint dari collider musuh.
    /// </summary>
    public class NPCGunUser : MonoBehaviour
    {
        [Tooltip("Ujung laras. Harus berada di dalam kapsul NPC, supaya raycast tidak mengenai kapsulnya sendiri.")]
        [SerializeField] private Transform muzzlePoint;
        [SerializeField] private NoiseEmitter noiseEmitter;

        private readonly GunCore core = new GunCore();
        private WeaponData weapon;
        private int magazine;
        private float reloadDoneTime = -1f;

        public bool HasWeapon => weapon != null;
        public bool IsReloading => reloadDoneTime >= 0f;

        private void Awake()
        {
            if (muzzlePoint == null) muzzlePoint = transform;
            if (noiseEmitter == null) noiseEmitter = GetComponentInChildren<NoiseEmitter>();
        }

        private void OnEnable()
        {
            // GunController.md Kasus Khusus: magazine diisi ulang saat NPC aktif kembali.
            if (weapon != null) magazine = weapon.magazineSize;
            reloadDoneTime = -1f;
        }

        /// <summary>Dipanggil NPCCompanion.Initialize dengan NPCData.weapon. null berarti NPC tidak menembak.</summary>
        public void SetWeapon(WeaponData w)
        {
            weapon = w;
            if (w == null) return;
            core.SetStats(WeaponStats.From(w));
            magazine = w.magazineSize;
            reloadDoneTime = -1f;
        }

        /// <summary>Satu tembakan ke titik bidik. false kalau tanpa senjata, reload, jeda tembak, atau sudut terlalu besar.</summary>
        public bool TryShootAt(Vector3 aimPoint)
        {
            if (weapon == null) return false;
            // 5. Reload tanpa inventori. Waktu berskala, jadi berhenti saat jeda.
            if (reloadDoneTime >= 0f)
            {
                if (Time.time < reloadDoneTime) return false;
                magazine = weapon.magazineSize;
                reloadDoneTime = -1f;
            }
            if (!core.CanFire) return false;

            Vector3 origin = muzzlePoint.position;
            Vector3 dir = aimPoint - origin;
            if (dir.sqrMagnitude < 0.0001f) return false;
            dir.Normalize();

            // 1. Tolak kalau sudut ke target di atas npcFireMaxAngle (dihitung dari arah hadap mendatar NPC).
            var bal = GameManager.CurrentBalance;
            float maxAngle = bal != null ? bal.gun.npcFireMaxAngle : 10f;
            Vector3 fwd = transform.forward; fwd.y = 0f;
            Vector3 flat = dir; flat.y = 0f;
            if (fwd.sqrMagnitude > 0.0001f && flat.sqrMagnitude > 0.0001f && Vector3.Angle(fwd, flat) > maxAngle) return false;

            // 2-3. Ray dari laras ke AimPoint, byPlayer = false.
            if (!core.Fire(new Ray(origin, dir), gameObject, weapon.name, false, weapon.impactTag)) return false;

            // Efek NPC tetap di layer dunia (GunController.md).
            if (VFXManager.Instance != null && !string.IsNullOrEmpty(weapon.muzzleFlashTag))
                VFXManager.Instance.Play(weapon.muzzleFlashTag, origin, Quaternion.LookRotation(dir), muzzlePoint);

            // 4. Suara lewat NoiseEmitter (#49).
            if (noiseEmitter != null) noiseEmitter.Emit(weapon.noiseRadius, NoiseSourceKind.Gunshot);
            else NoiseEmitter.Emit(origin, weapon.noiseRadius, NoiseSourceKind.Gunshot);

            magazine--;
            if (magazine <= 0) reloadDoneTime = Time.time + weapon.reloadSeconds;
            return true;
        }
    }
}
