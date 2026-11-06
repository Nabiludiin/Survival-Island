using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Jenis kejadian di jadwal bahaya kapal. Blueprint: 60-seconds/ShipHazardManager.md bagian 1 (#75).</summary>
    public enum ShipHazardKind { AlarmUp, CameraShake, SpawnZombies, CollapseWarning, Collapse, SetSwayStage, FinalShudder }

    /// <summary>Satu entri jadwal. Diurutkan dari waktu sisa terbesar.</summary>
    [Serializable]
    public struct ShipHazardEntry
    {
        public float atSecondsLeft;
        public ShipHazardKind kind;
        public string targetId;     // ID koridor atau grup zombie
        public int value;           // Tahap sway atau jumlah zombie
    }

    /// <summary>Tinggi air dunia pada waktu sisa tertentu.</summary>
    [Serializable]
    public struct WaterKeyframe
    {
        public float atSecondsLeft;
        public float level;
    }

    /// <summary>
    /// Jadwal bahaya kapal sebagai data (keputusan teknis ShipHazardManager.md: jadwal berupa data).
    /// Isi awal (usulan disetujui #85) mengikuti tabel "Isi jadwal awal" di ShipHazardManager.md dan diisi di aset.
    /// Kelas ScriptableObject berada di file sendiri karena Unity mewajibkan nama file sama dengan nama kelas aset.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Ship Hazard Schedule")]
    public class ShipHazardSchedule : ScriptableObject
    {
        public ShipHazardEntry[] entries = Array.Empty<ShipHazardEntry>();       // Urut dari waktu sisa terbesar
        // Keyframe awal (usulan #85): (60, -1), (30, 0), (15, 1,1), (5, 4), (0, 4,6).
        public WaterKeyframe[] waterKeyframes =
        {
            new WaterKeyframe { atSecondsLeft = 60f, level = -1f },
            new WaterKeyframe { atSecondsLeft = 30f, level = 0f },
            new WaterKeyframe { atSecondsLeft = 15f, level = 1.1f },
            new WaterKeyframe { atSecondsLeft = 5f, level = 4f },
            new WaterKeyframe { atSecondsLeft = 0f, level = 4.6f },
        };

        /// <summary>Tinggi air target pada waktu sisa t, interpolasi linier antar-keyframe.</summary>
        public float EvaluateWater(float t)
        {
            if (waterKeyframes == null || waterKeyframes.Length == 0) return 0f;
            if (t >= waterKeyframes[0].atSecondsLeft) return waterKeyframes[0].level;
            for (int i = 1; i < waterKeyframes.Length; i++)
            {
                var a = waterKeyframes[i - 1];
                var b = waterKeyframes[i];
                if (t >= b.atSecondsLeft)
                {
                    float span = a.atSecondsLeft - b.atSecondsLeft;
                    float k = span > 0f ? (a.atSecondsLeft - t) / span : 1f;
                    return Mathf.Lerp(a.level, b.level, k);
                }
            }
            return waterKeyframes[waterKeyframes.Length - 1].level;
        }

#if UNITY_EDITOR
        /// <summary>Isi jadwal awal dari tabel ShipHazardManager.md. Menu konteks aset di Inspector.</summary>
        [ContextMenu("Isi jadwal awal (ShipHazardManager.md)")]
        private void FillDefaultSchedule()
        {
            entries = new[]
            {
                E(60f, ShipHazardKind.SetSwayStage, null, 0),
                E(60f, ShipHazardKind.SpawnZombies, "lower_start", 3),
                E(60f, ShipHazardKind.SpawnZombies, "mid_start", 1),
                E(45f, ShipHazardKind.AlarmUp, null, 0),
                E(45f, ShipHazardKind.SetSwayStage, null, 1),
                E(45f, ShipHazardKind.SpawnZombies, "mid_45", 2),
                E(33f, ShipHazardKind.CollapseWarning, "Corridor_MidPort", 0),
                E(30f, ShipHazardKind.CameraShake, "ShipRumble", 0),
                E(30f, ShipHazardKind.SetSwayStage, null, 2),
                E(30f, ShipHazardKind.Collapse, "Corridor_MidPort", 0),
                E(30f, ShipHazardKind.SpawnZombies, "lower_30", 2),
                E(18f, ShipHazardKind.CollapseWarning, "Stair_LowerAft", 0),
                E(15f, ShipHazardKind.SetSwayStage, null, 3),
                E(15f, ShipHazardKind.Collapse, "Stair_LowerAft", 0),
                E(15f, ShipHazardKind.SpawnZombies, "mid_15", 2),
                E(0f, ShipHazardKind.FinalShudder, null, 0),
            };
            UnityEditor.EditorUtility.SetDirty(this);
        }

        private static ShipHazardEntry E(float t, ShipHazardKind kind, string id, int value) =>
            new ShipHazardEntry { atSecondsLeft = t, kind = kind, targetId = id, value = value };
#endif
    }
}
