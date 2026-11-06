using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Trigger zona raksasa (BoxCollider IsTrigger, layer ZoneTrigger) yang menentukan region aktif pemain dan
    /// menerbitkan RegionChangedEvent. Blueprint: Island_Logic/ZoneManager.md (#92, melengkapi #16, #65, #88, #111).
    /// GDD bagian 5: empat region. Claude.md Tahap 4: BoxCollider raksasa di layer ZoneTrigger sebagai pengenal zona.
    ///
    /// ZoneManager tidak memeriksa syarat akses (RegionGateSystem), tidak memuat scene (RegionStreaming #88),
    /// dan tidak memberi efek. Frostbite pindah ke WarmthSystem (#65), suasana ke EnvironmentDirector (#92).
    /// Trigger berada di scene IslandPhase, bukan di region scene (#88).
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class ZoneManager : MonoBehaviour
    {
        private static readonly List<ZoneManager> all = new List<ZoneManager>();

        [Tooltip("Region yang diwakili trigger ini.")]
        public RegionType myRegion = RegionType.Volcano;

        [SerializeField] private BoxCollider zoneCollider;

        private bool playerInside;

        /// <summary>Semua trigger aktif (dibaca alat Editor ZoneCoverageAudit, usulan).</summary>
        public static IReadOnlyList<ZoneManager> All => all;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics() => all.Clear();

        private void Awake()
        {
            if (zoneCollider == null) zoneCollider = GetComponent<BoxCollider>();
            zoneCollider.isTrigger = true;
            // Hitungan statis selalu milik scene IslandPhase yang sedang berjalan.
            ZoneTracker.EnsureScene(gameObject.scene);
        }

        private void OnEnable()
        {
            if (!all.Contains(this)) all.Add(this);
        }

        private void OnDisable()
        {
            // Tidak menerbitkan event di sini: OnDisable juga berjalan saat scene IslandPhase dilepas.
            all.Remove(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            // 1. Musuh, NPC, dan Heavy Item juga menyentuh layer ZoneTrigger, tetapi diabaikan.
            if (playerInside || !other.CompareTag("Player")) return;
            // 2-3.
            playerInside = true;
            if (ZoneTracker.Enter(myRegion, out var from, out var to)) Publish(from, to);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!playerInside || !other.CompareTag("Player")) return;
            playerInside = false;
            if (ZoneTracker.Exit(myRegion, out var from, out var to)) Publish(from, to);
        }

        private static void Publish(RegionType from, RegionType to)
        {
            EventChannel<RegionChangedEvent>.Publish(new RegionChangedEvent(from, to));
        }

        /// <summary>
        /// Hitung ulang zona dari posisi pemain, tanpa query fisika. Dipanggil IslandPlayerController.Teleport (#101),
        /// karena OnTriggerExit tidak dijamin terkirim untuk CharacterController yang dimatikan.
        /// </summary>
        public static void ResyncAll(Vector3 playerPosition)
        {
            // 1. Kosongkan hitungan tanpa mengubah Active dan HasActive.
            ZoneTracker.ClearCounts();

            // 2. ClosestPoint mengembalikan titik itu sendiri kalau berada di dalam collider (juga untuk kotak yang diputar).
            bool anyInside = false;
            RegionType firstInside = RegionType.Volcano;
            for (int i = 0; i < all.Count; i++)
            {
                var z = all[i];
                if (z == null || z.zoneCollider == null) continue;
                bool inside = (z.zoneCollider.ClosestPoint(playerPosition) - playerPosition).sqrMagnitude < 0.0001f;
                z.playerInside = inside;
                if (!inside) continue;
                ZoneTracker.AddCount(z.myRegion);
                if (!anyInside)
                {
                    anyInside = true;
                    firstInside = z.myRegion;
                }
            }

            // 5. Tidak berada di zona mana pun: region aktif tetap.
            if (!anyInside) return;

            // 3-4.
            if (ZoneTracker.ResolveAfterResync(firstInside, out var from, out var to)) Publish(from, to);
        }

#if UNITY_EDITOR
        // Warna per region (aturan penempatan trigger).
        private static readonly Color[] gizmoColors =
        {
            new Color(1f, 0.4f, 0.1f, 0.15f),   // Volcano
            new Color(0.1f, 0.8f, 0.2f, 0.15f), // Amazon
            new Color(0.6f, 0.85f, 1f, 0.15f),  // Snow
            new Color(0.7f, 0.7f, 0.5f, 0.15f), // Logistic
        };

        private void OnDrawGizmos()
        {
            var box = zoneCollider != null ? zoneCollider : GetComponent<BoxCollider>();
            if (box == null) return;
            Gizmos.matrix = transform.localToWorldMatrix;
            Color c = gizmoColors[(int)myRegion];
            Gizmos.color = c;
            Gizmos.DrawCube(box.center, box.size);
            c.a = 0.8f;
            Gizmos.color = c;
            Gizmos.DrawWireCube(box.center, box.size);
        }
#endif
    }
}
