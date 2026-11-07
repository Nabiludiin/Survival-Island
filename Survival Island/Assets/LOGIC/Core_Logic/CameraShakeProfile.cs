using System;
using Unity.Cinemachine;
using UnityEngine;

namespace SurvivalIsland
{
    public enum CameraShakeKind : byte
    {
        GunFire, PlayerHit, ZombieBite, Explosion, FireballImpact,
        Earthquake, Pinned, ShipRumble, ShipCollapse, ShipFinalShudder
    }

    /// <summary>Nilai bitmask channel Impulse. Aim menggeser pitch dan yaw; Visual hanya posisi dan roll.</summary>
    public enum ShakeChannel { Aim = 1, Visual = 2 }

    /// <summary>Data satu jenis getaran (CameraShake.md bagian 1). Nilai awal ada di tabel "Preset awal".</summary>
    [Serializable]
    public struct CameraShakePreset
    {
        public CameraShakeKind kind;
        public CinemachineImpulseDefinition.ImpulseShapes shape;
        public float duration;
        public Vector3 direction;       // Arah dasar, kalau pemanggil tidak memberi arah
        public float force;
        public float cooldownSeconds;
        public float falloffRadius;     // 0 berarti tanpa jarak
        public ShakeChannel channel;
    }

    /// <summary>
    /// Semua preset getaran, satu baris per CameraShakeKind. Blueprint: Core_Logic/CameraShake.md (#24).
    /// Usulan disetujui developer (#24 poin 5): semua angka preset disetel lewat playtest.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Camera Shake Profile")]
    public class CameraShakeProfile : ScriptableObject
    {
        public CameraShakePreset[] presets = new CameraShakePreset[0];

        private void Reset()
        {
            // Tabel "Preset awal" di CameraShake.md, diisi saat aset dibuat.
            presets = new[]
            {
                // GunFire: arah dasar sedikit ke atas; sisi kiri atau kanan diacak saat dipicu (tanda x dibalik acak).
                P(CameraShakeKind.GunFire, CinemachineImpulseDefinition.ImpulseShapes.Recoil, 0.15f, 0.4f, 0f, 0f, ShakeChannel.Visual, new Vector3(0.3f, 1f, 0f)),
                P(CameraShakeKind.PlayerHit, CinemachineImpulseDefinition.ImpulseShapes.Bump, 0.2f, 0.5f, 0.15f, 0f, ShakeChannel.Aim),
                P(CameraShakeKind.ZombieBite, CinemachineImpulseDefinition.ImpulseShapes.Bump, 0.35f, 1f, 0.25f, 0f, ShakeChannel.Aim),
                P(CameraShakeKind.Explosion, CinemachineImpulseDefinition.ImpulseShapes.Explosion, 0.8f, 2.5f, 0.1f, 25f, ShakeChannel.Aim),
                P(CameraShakeKind.FireballImpact, CinemachineImpulseDefinition.ImpulseShapes.Explosion, 0.7f, 2f, 0.1f, 40f, ShakeChannel.Aim),
                P(CameraShakeKind.Earthquake, CinemachineImpulseDefinition.ImpulseShapes.Rumble, 3f, 1.2f, 0f, 0f, ShakeChannel.Aim),
                P(CameraShakeKind.Pinned, CinemachineImpulseDefinition.ImpulseShapes.Bump, 0.5f, 1.5f, 0.5f, 0f, ShakeChannel.Aim),
                P(CameraShakeKind.ShipRumble, CinemachineImpulseDefinition.ImpulseShapes.Rumble, 2f, 0.8f, 0f, 0f, ShakeChannel.Aim),
                P(CameraShakeKind.ShipCollapse, CinemachineImpulseDefinition.ImpulseShapes.Explosion, 1f, 2f, 0f, 0f, ShakeChannel.Aim),
                P(CameraShakeKind.ShipFinalShudder, CinemachineImpulseDefinition.ImpulseShapes.Rumble, 2.5f, 2f, 0f, 0f, ShakeChannel.Aim),
            };
        }

        private static CameraShakePreset P(CameraShakeKind k, CinemachineImpulseDefinition.ImpulseShapes shape, float duration,
            float force, float cooldown, float falloff, ShakeChannel channel, Vector3? direction = null)
        {
            return new CameraShakePreset
            {
                kind = k, shape = shape, duration = duration, direction = direction ?? Vector3.down, force = force,
                cooldownSeconds = cooldown, falloffRadius = falloff, channel = channel
            };
        }
    }
}
