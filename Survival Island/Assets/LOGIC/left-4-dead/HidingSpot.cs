using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Jenis tempat sembunyi Zombie Desa (SunlightExposure.md, #63).</summary>
    public enum HidingKind : byte { Water, Mud }

    /// <summary>
    /// Titik air atau lumpur di scene Region4_Village, tempat Zombie Desa bersembunyi saat siang.
    /// Blueprint: left-4-dead/SunlightExposure.md bagian "HidingSpot" (#63).
    /// GDD bagian 5, Region 4: Zombie Desa berendam di air atau terkubur di lumpur, lalu naik secara massal saat malam.
    /// Usulan disetujui developer (SunlightExposure.md bagian 5 no. 5): kapasitas air 8, lumpur 4, total minimal 30
    /// (ukuran pool zombie_desa). Kapasitas diisi di Inspector per titik.
    /// Daftar statis diisi OnEnable dan dilepas OnDisable, jadi titik di region yang dilepas (#88) tidak pernah dipilih.
    /// </summary>
    public class HidingSpot : MonoBehaviour
    {
        private static readonly List<HidingSpot> all = new List<HidingSpot>(16);

        public HidingKind kind;
        [Tooltip("Air 8, lumpur 4 (usulan disetujui developer).")]
        public int capacity = 8;
        [Tooltip("Di tepi, berada di NavMesh. Tujuan SeekHiding.")]
        public Transform entryPoint;
        [Tooltip("Titik naik saat malam, berada di NavMesh.")]
        public Transform[] emergePoints = new Transform[0];
        [Tooltip("Jarak model turun di bawah permukaan.")]
        public float buryDepth = 2f;
        [Tooltip("Gelembung atau lumpur bergerak. Aktif selama ada penghuni.")]
        public ParticleSystem tellEffect;

        private readonly List<VillageZombieHiding> occupants = new List<VillageZombieHiding>(8);

        public static IReadOnlyList<HidingSpot> All => all;

        /// <summary>Jumlah penghuni titik ini.</summary>
        public int Occupants => occupants.Count;

        public bool HasRoom => occupants.Count < capacity;

        /// <summary>Titik masuk di NavMesh. Tanpa entryPoint, posisi titik ini sendiri.</summary>
        public Vector3 EntryPosition => entryPoint != null ? entryPoint.position : transform.position;

        /// <summary>Jumlah penghuni semua titik (dipakai SunlightExposure.ReportEmergence dan PopulateHidingSpots).</summary>
        public static int TotalOccupants
        {
            get
            {
                int n = 0;
                for (int i = 0; i < all.Count; i++) n += all[i].occupants.Count;
                return n;
            }
        }

        private void OnEnable()
        {
            if (!all.Contains(this)) all.Add(this);
            UpdateTell();
        }

        private void OnDisable()
        {
            all.Remove(this);
            // Region dilepas (#88): penghuni dilepas supaya tidak ada kebocoran. Zombienya sendiri dikembalikan
            // SpawnManager.DespawnRegion; di sini hanya rujukan titiknya yang dikosongkan.
            for (int i = occupants.Count - 1; i >= 0; i--)
                if (occupants[i] != null) occupants[i].NotifySpotLost(this);
            occupants.Clear();
            UpdateTell();
        }

        /// <summary>Titik terdekat yang masih punya tempat, dalam radius (SeekHiding langkah 2).</summary>
        public static HidingSpot FindAvailable(Vector3 from, float radius)
        {
            HidingSpot best = null;
            float bestSqr = radius * radius;
            for (int i = 0; i < all.Count; i++)
            {
                var s = all[i];
                if (!s.HasRoom) continue;
                float sqr = (s.EntryPosition - from).sqrMagnitude;
                if (sqr <= bestSqr) { bestSqr = sqr; best = s; }
            }
            return best;
        }

        /// <summary>Titik terdekat ke pemain dalam spawn.maxDistance, untuk slot gelombang malam (SpawnManager.md).</summary>
        public static HidingSpot FindEmergeSpot(Vector3 player)
        {
            var bal = GameManager.CurrentBalance;
            float max = bal != null ? bal.spawn.maxDistance : 45f;
            HidingSpot best = null;
            float bestSqr = max * max;
            for (int i = 0; i < all.Count; i++)
            {
                var s = all[i];
                float sqr = (s.transform.position - player).sqrMagnitude;
                if (sqr <= bestSqr) { bestSqr = sqr; best = s; }
            }
            return best;
        }

        /// <summary>Titik naik acak. Tanpa emergePoints, titik masuk dipakai.</summary>
        public Vector3 GetEmergePosition(int index)
        {
            if (emergePoints == null || emergePoints.Length == 0) return EntryPosition;
            var p = emergePoints[Mathf.Abs(index) % emergePoints.Length];
            return p != null ? p.position : EntryPosition;
        }

        public int EmergePointCount => emergePoints != null ? emergePoints.Length : 0;

        public bool IsOccupiedBy(VillageZombieHiding z) => occupants.Contains(z);

        public bool Reserve(VillageZombieHiding z)
        {
            if (z == null) return false;
            if (occupants.Contains(z)) return true;
            if (!HasRoom) return false;
            occupants.Add(z);
            UpdateTell();
            return true;
        }

        public void Release(VillageZombieHiding z)
        {
            if (z == null || !occupants.Remove(z)) return;
            UpdateTell();
        }

        // Tanda samar: gelembung di air atau lumpur bergerak, hanya selama ada penghuni (SunlightExposure.md "Terlihat dan diserang").
        private void UpdateTell()
        {
            if (tellEffect == null) return;
            if (occupants.Count > 0 && isActiveAndEnabled) { if (!tellEffect.isPlaying) tellEffect.Play(); }
            else tellEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
