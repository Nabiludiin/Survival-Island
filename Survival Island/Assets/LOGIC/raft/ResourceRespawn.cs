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
    /// Belum ada di Langkah 4 (TODO): CoconutPickup dan StonePickup (#43, #116, butuh PickupInteractable #17),
    /// CountActiveStones dan ForceRegrowStones (#116), ResourceAuditWindow (Editor).
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
