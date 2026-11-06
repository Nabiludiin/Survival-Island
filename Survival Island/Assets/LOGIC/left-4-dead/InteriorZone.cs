using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Batas area tertutup yang gelap (gua, bunker, gudang). Blueprint: left-4-dead/ZombieLair.md bagian "Masuk dan keluar
    /// zona" (#112). GDD bagian 3: zombie terlindung dari sinar matahari di ruang gelap.
    /// Objek di scene region, layer ZoneTrigger, BoxCollider trigger. Prefab juga memasang NoBuildVolume, sehingga
    /// BuildManager menolak bangunan di dalamnya (NoBuildZone).
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class InteriorZone : MonoBehaviour
    {
        private static readonly List<InteriorZone> active = new List<InteriorZone>(16);
        public static IReadOnlyList<InteriorZone> Active => active;

        [SerializeField] private string zoneId;
        [SerializeField] private bool isDark = true;
        [Tooltip("Ambang pintu keluar. Dipakai jalur suara dari luar dan zombie yang dilepas saat malam.")]
        [SerializeField] private Transform[] exitPoints = new Transform[0];
        [Tooltip("Profil cahaya dan kabut di dalam zona (dibaca EnvironmentDirector, #92).")]
        [SerializeField] private RegionEnvironmentProfile lightProfile;

        public string ZoneId => string.IsNullOrEmpty(zoneId) ? name : zoneId;
        public bool IsDark => isDark;
        public Transform[] ExitPoints => exitPoints;
        public RegionEnvironmentProfile LightProfile => lightProfile;
        public bool IsPlayerInside { get; private set; }
        public BoxCollider Volume { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics() => active.Clear();

        private void Awake()
        {
            Volume = GetComponent<BoxCollider>();
            Volume.isTrigger = true;
        }

        private void OnEnable()
        {
            if (!active.Contains(this)) active.Add(this);
        }

        private void OnDisable()
        {
            active.Remove(this);
            if (IsPlayerInside) SetInside(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) SetInside(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) SetInside(false);
        }

        private void SetInside(bool inside)
        {
            if (IsPlayerInside == inside) return;
            IsPlayerInside = inside;
            EventChannel<InteriorZoneChangedEvent>.Publish(new InteriorZoneChangedEvent(ZoneId, inside, isDark));
        }

        /// <summary>Titik ini di dalam zona ini.</summary>
        public bool ContainsPoint(Vector3 pos) => Volume != null && Volume.bounds.Contains(pos);

        /// <summary>
        /// Titik ini di dalam salah satu InteriorZone aktif. Dipakai SunlightExposure.CheckExposure dan penolakan titik di
        /// SpawnManager.FindSpawnPoint (#63, #62).
        /// </summary>
        public static bool Contains(Vector3 pos)
        {
            for (int i = 0; i < active.Count; i++) if (active[i].ContainsPoint(pos)) return true;
            return false;
        }

        /// <summary>Jarak suara dari luar: ke exitPoints terdekat, lalu ke target (ZombieLair.md tabel pemicu).</summary>
        public float PathDistanceFromOutside(Vector3 source, Vector3 target)
        {
            if (exitPoints == null || exitPoints.Length == 0) return Vector3.Distance(source, target);
            float best = float.MaxValue;
            for (int i = 0; i < exitPoints.Length; i++)
            {
                if (exitPoints[i] == null) continue;
                Vector3 e = exitPoints[i].position;
                float d = Vector3.Distance(source, e) + Vector3.Distance(e, target);
                if (d < best) best = d;
            }
            return best == float.MaxValue ? Vector3.Distance(source, target) : best;
        }

        /// <summary>Ambang pintu terdekat dari titik ini, atau posisi zona kalau tidak ada.</summary>
        public Vector3 NearestExit(Vector3 from)
        {
            Vector3 best = transform.position;
            float bestD = float.MaxValue;
            if (exitPoints == null) return best;
            for (int i = 0; i < exitPoints.Length; i++)
            {
                if (exitPoints[i] == null) continue;
                float d = (exitPoints[i].position - from).sqrMagnitude;
                if (d < bestD) { bestD = d; best = exitPoints[i].position; }
            }
            return best;
        }
    }
}
