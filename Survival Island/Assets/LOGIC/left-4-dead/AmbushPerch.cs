using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Penanda pohon untuk sergapan Jaguar di Region 2, dengan satu penghuni. Penanda scene, tanpa FSM sendiri.
    /// Blueprint: left-4-dead/WildlifeAI.md "Terkaman Jaguar (Pounce)" (#61).
    /// GDD bagian 3 (Diterkam) dan bagian 5 (Region 2, Jaguar). Pola sergapan dari Hunter di REFERENSI_MUSUH_L4D.md.
    /// Daftar statis diisi OnEnable dan dilepas OnDisable. Penghuni dilepas saat titik dimatikan (region dilepas, #88).
    /// </summary>
    public class AmbushPerch : MonoBehaviour
    {
        private static readonly List<AmbushPerch> all = new List<AmbushPerch>(16);

        [Tooltip("Kaki pohon, berada di NavMesh. Jaguar berjalan ke sini sebelum naik.")]
        public Transform basePoint;
        [Tooltip("Posisi dan arah Jaguar di cabang.")]
        public Transform perchPoint;
        [Tooltip("Daun bergoyang saat tanda terkaman. Boleh kosong.")]
        public ParticleSystem leafShake;

        private WildlifeAI occupant;

        public static IReadOnlyList<AmbushPerch> All => all;
        public bool IsFree => occupant == null || !occupant.isActiveAndEnabled;
        public Vector3 BasePosition => basePoint != null ? basePoint.position : transform.position;
        public Vector3 PerchPosition => perchPoint != null ? perchPoint.position : transform.position;
        public Quaternion PerchRotation => perchPoint != null ? perchPoint.rotation : transform.rotation;

        private void OnEnable() { if (!all.Contains(this)) all.Add(this); }

        private void OnDisable()
        {
            all.Remove(this);
            occupant = null;
        }

        /// <summary>Perch kosong terdekat dalam radius dari center. Perch dipakai Jaguar lain: dilewati.</summary>
        public static AmbushPerch FindNearestFree(Vector3 from, Vector3 center, float radius)
        {
            AmbushPerch best = null;
            float bestSqr = float.MaxValue;
            float r2 = radius * radius;
            for (int i = 0; i < all.Count; i++)
            {
                var p = all[i];
                if (!p.IsFree || (p.BasePosition - center).sqrMagnitude > r2) continue;
                float sqr = (p.BasePosition - from).sqrMagnitude;
                if (sqr < bestSqr) { bestSqr = sqr; best = p; }
            }
            return best;
        }

        public bool TryOccupy(WildlifeAI jaguar)
        {
            if (jaguar == null) return false;
            if (occupant == jaguar) return true;
            if (!IsFree) return false;
            occupant = jaguar;
            return true;
        }

        public void Release(WildlifeAI jaguar)
        {
            if (occupant == jaguar) occupant = null;
            if (leafShake != null) leafShake.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        /// <summary>Tanda visual terkaman: daun bergoyang (WildlifeAI.md).</summary>
        public void PlayTell()
        {
            if (leafShake != null) leafShake.Play();
        }
    }
}
