using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Manager di IslandPhase yang menumbuhkan kembali node habis berdasarkan jam absolut.
    /// Blueprint: raft/ResourceRespawn.md (#73).
    ///
    /// GDD bagian 3: melewati hari ke-60 bukan Game Over, jadi resource tidak boleh habis permanen.
    /// Usulan disetujui: node hanya tumbuh kalau pemain lebih dari regrowMinPlayerDistance dan radius
    /// regrowClearRadius bebas bangunan, pemain, NPC, dan Heavy Item (PhysicsMasks.RegrowBlock).
    ///
    /// CoconutPickup dan StonePickup (dasar RegrowablePickup) mendaftar lewat MarkDepleted seperti node.
    /// CountActiveStones dan ForceRegrowStones (#116) menghitung semua PickupInteractable dengan item Mat_Stone, termasuk
    /// StonePickup, supaya batu kecil lama di scene tetap terhitung. Belum ada (TODO): ResourceAuditWindow (Editor).
    /// </summary>
    [DisallowMultipleComponent]
    public class ResourceRespawn : MonoBehaviour
    {
        public static ResourceRespawn Instance { get; private set; }

        private readonly List<IRegrowable> depleted = new List<IRegrowable>(64);
        private readonly Collider[] overlapBuffer = new Collider[8];
        private Coroutine checkRoutine;
        private bool checkPending;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            EventChannel<HourChangedEvent>.Raised += HandleHour;
        }

        private void OnDisable()
        {
            EventChannel<HourChangedEvent>.Raised -= HandleHour;
            checkRoutine = null;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        /// <summary>Node habis mendaftar. Pemeriksaan dimulai frame berikutnya, jadi waktu yang sudah lewat langsung dikejar.</summary>
        public void MarkDepleted(IRegrowable node)
        {
            if (node == null || depleted.Contains(node)) return;
            depleted.Add(node);
            StartCheck();
        }

        /// <summary>Node keluar daftar (region dilepas atau node dimatikan).</summary>
        public void Forget(IRegrowable node)
        {
            depleted.Remove(node);
        }

        // ---------------------------------------------------------------- Batu kecil (#116)

        /// <summary>itemID batu kecil. Sama dengan ProgressionSafety.StoneId.</summary>
        private const string StoneItemId = "Mat_Stone";

        /// <summary>PickupInteractable batu kecil (Mat_Stone). Juga dipakai perintah clearstones (ProgressionSafety).</summary>
        public static bool IsSmallStone(PickupInteractable p) => p != null && p.Item != null && p.Item.itemID == StoneItemId;

        /// <summary>Batu kecil yang masih bisa diambil di region itu (ResourceRespawn.md, dibaca ProgressionSafety #116).</summary>
        public int CountActiveStones(RegionType region)
        {
            var all = FindObjectsByType<PickupInteractable>(FindObjectsSortMode.None);
            int n = 0;
            for (int i = 0; i < all.Length; i++)
                if (IsSmallStone(all[i]) && all[i].IsAvailable && RegionStreaming.RegionOf(all[i].transform.position) == region) n++;
            return n;
        }

        /// <summary>
        /// Pengaman ProgressionSafety (#116): tumbuhkan lagi batu kecil yang sudah diambil di region itu sampai jumlah
        /// aktif mencapai minimum. Mengembalikan jumlah batu yang ditumbuhkan. Tidak memunculkan objek baru.
        /// </summary>
        public int ForceRegrowStones(RegionType region, int minimum)
        {
            int need = minimum - CountActiveStones(region);
            if (need <= 0) return 0;
            var all = FindObjectsByType<PickupInteractable>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            int grown = 0;
            for (int i = 0; i < all.Length && grown < need; i++)
            {
                var p = all[i];
                if (!IsSmallStone(p) || !p.IsConsumed) continue;
                if (RegionStreaming.RegionOf(p.transform.position) != region) continue;
                p.Regrow();
                grown++;
            }
            return grown;
        }

        private void HandleHour(HourChangedEvent e)
        {
            StartCheck();
        }

        private void StartCheck()
        {
            if (!isActiveAndEnabled) return;
            if (checkRoutine != null) { checkPending = true; return; }
            checkRoutine = StartCoroutine(CheckRoutine());
        }

        private IEnumerator CheckRoutine()
        {
            do
            {
                checkPending = false;
                // Satu frame jeda, supaya MarkDepleted dari RestoreState selesai dulu.
                yield return null;

                int perFrame = Balance != null ? Mathf.Max(1, Balance.resource.regrowChecksPerFrame) : 20;
                float now = ItemUseSystem.CurrentTotalGameHours();
                int checkedThisFrame = 0;

                for (int i = depleted.Count - 1; i >= 0; i--)
                {
                    if (i >= depleted.Count) continue;
                    var node = depleted[i];
                    if (node == null || !node.IsDepleted) { depleted.RemoveAt(i); continue; }

                    // Tanpa jadwal atau belum waktunya: lewati.
                    if (node.RegrowAtGameHour >= 0f && now >= node.RegrowAtGameHour && CanRegrow(node))
                    {
                        node.Regrow();
                        depleted.RemoveAt(i);
                    }

                    if (++checkedThisFrame >= perFrame)
                    {
                        checkedThisFrame = 0;
                        yield return null;
                        now = ItemUseSystem.CurrentTotalGameHours();
                    }
                }
            }
            while (checkPending);
            checkRoutine = null;
        }

        private bool CanRegrow(IRegrowable node)
        {
            Vector3 pos = node.Position;
            var player = IslandPlayerController.Instance;
            if (player != null)
            {
                float min = Balance != null ? Balance.resource.regrowMinPlayerDistance : 25f;
                if ((player.transform.position - pos).sqrMagnitude < min * min) return false;
            }
            float radius = Balance != null ? Balance.resource.regrowClearRadius : 3f;
            // Bangunan di atas jamur yang habis menahan node sampai dibongkar.
            return PhysicsQuery.OverlapSphere(pos, radius, PhysicsMasks.RegrowBlock, overlapBuffer) == 0;
        }
    }
}
