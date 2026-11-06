using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Titik sarang hewan untuk FleeToDen saat malam (WildlifeAI.md).</summary>
    public class DenPoint : MonoBehaviour
    {
        private static readonly List<DenPoint> all = new List<DenPoint>(16);

        private void OnEnable() { if (!all.Contains(this)) all.Add(this); }
        private void OnDisable() { all.Remove(this); }

        public static DenPoint FindNearest(Vector3 from)
        {
            DenPoint best = null;
            float bestSqr = float.MaxValue;
            for (int i = 0; i < all.Count; i++)
            {
                float sqr = (all[i].transform.position - from).sqrMagnitude;
                if (sqr < bestSqr) { bestSqr = sqr; best = all[i]; }
            }
            return best;
        }
    }
}
