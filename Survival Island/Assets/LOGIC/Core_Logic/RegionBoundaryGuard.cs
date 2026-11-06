using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pengaman posisi aman terakhir dan pemain tanpa tanah di region yang belum dimuat. Komponen di IslandPhase.
    /// Blueprint: Core_Logic/RegionStreaming.md bagian "Pengaman kalau pemain tiba lebih cepat", langkah 5 (#88).
    /// LastSafePosition dipakai RavineRecoveryVolume dan VerifyPlayerRegion (#68).
    ///
    /// Catatan implementasi (sistem pendukung): IslandPlayerController tidak punya API menahan gravitasi, jadi penahan
    /// memakai PlayerTraversal.SetLocked (mode Locked tidak memakai gravitasi). Kunci hanya dilepas kalau kelas ini yang
    /// memasangnya dan pemain tidak sedang terkunci oleh sistem lain sebelumnya.
    /// </summary>
    public class RegionBoundaryGuard : MonoBehaviour
    {
        public static RegionBoundaryGuard Instance { get; private set; }

        private Vector3 lastSafePosition;
        private bool hasSafePosition;
        private float timer;
        private bool holdingPlayer;

        public Vector3 LastSafePosition => lastSafePosition;
        public bool HasSafePosition => hasSafePosition;

        private static StreamingBalance B => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.streaming : null;

        private void Awake() { Instance = this; }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnDisable() => ReleaseHold();

        private void Update()
        {
            if (Time.deltaTime <= 0f) return;
            var b = B;
            if (b == null) return;
            timer += Time.deltaTime;
            if (timer < b.safePositionIntervalSeconds) return;
            timer = 0f;

            var player = IslandPlayerController.Instance;
            var streaming = RegionStreaming.Instance;
            if (player == null || streaming == null || !streaming.IsReady) return;

            Vector3 p = player.transform.position;
            bool regionLoaded = streaming.IsLoaded(streaming.GetRegionAt(p));
            bool hasGround = PhysicsQuery.Raycast(new Ray(p + Vector3.up * 0.5f, Vector3.down), out _, b.safeGroundProbeDistance + 0.5f, PhysicsMasks.Ground);

            if (regionLoaded && hasGround)
            {
                // Dicatat saat raycast ke Ground mengenai tanah.
                if (player.IsGrounded)
                {
                    lastSafePosition = p;
                    hasSafePosition = true;
                }
                ReleaseHold();
                return;
            }

            if (!regionLoaded && !hasGround && hasSafePosition)
            {
                // Pemain di region yang belum Loaded tanpa tanah: kembali ke posisi aman, gravitasi ditahan sampai region siap.
                player.Teleport(lastSafePosition);
                HoldPlayer(player);
            }
        }

        private void HoldPlayer(IslandPlayerController player)
        {
            if (holdingPlayer) return;
            var traversal = player.GetComponent<PlayerTraversal>();
            if (traversal == null || traversal.Mode == TraversalMode.Locked) return;
            traversal.SetLocked(true);
            holdingPlayer = true;
        }

        private void ReleaseHold()
        {
            if (!holdingPlayer) return;
            holdingPlayer = false;
            var player = IslandPlayerController.Instance;
            var traversal = player != null ? player.GetComponent<PlayerTraversal>() : null;
            if (traversal != null) traversal.SetLocked(false);
        }
    }
}
