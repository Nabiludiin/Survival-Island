using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Kedatangan di pantai selatan Region 1: posisi pemain, item inti, vaksin awal, barang kapal, dan NPC.
    /// Blueprint: 60-seconds/LifeboatExtraction.md bagian "Kedatangan di pantai" (#74). Berada di scene IslandPhase.
    /// Dipanggil RunLifecycle.EnterIslandPhase setelah LoadInitialRegions dan sebelum SaveRequestedEvent.
    ///
    /// Memakai API Langkah 3: SurvivalStats.ApplyArrivalState, FirstPersonCamera.LookAt, ItemDatabase.Get,
    /// dan CarryCapacity.ApplyShipLoot (mengembalikan sisa).
    ///
    /// Belum diimplementasikan (ditandai TODO):
    /// - Sisa barang yang tidak muat sebagai WorldItem di overflowOrigin (WorldItemManager #66, overflow CarryCapacity).
    /// - NPCRoster.SpawnRescued (#76).
    /// </summary>
    public class IslandArrival : MonoBehaviour
    {
        // Item inti selalu ada (GDD bagian 3, GLOSSARY.md). IslandArrival satu-satunya pemilik item inti dan vaksin awal.
        private static readonly string[] BaseItemIds = { "Base_Kail", "Base_Pistol", "Base_Pisau" };
        private const string VaccineItemId = "Med_Vaccine";

        private static readonly InfectionBalance FallbackInfection = new InfectionBalance();

        public static IslandArrival Instance { get; private set; }

        [Tooltip("Posisi awal pemain di pasir dekat bangkai sekoci.")]
        [SerializeField] private Transform landingPoint;
        [Tooltip("Arah pandang awal, menghadap hutan Region 1.")]
        [SerializeField] private Transform lookTarget;
        [Tooltip("Titik asal NPC, berjarak ship.arrivalNpcSpacing.")]
        [SerializeField] private Transform npcOrigin;
        [Tooltip("Titik asal barang sisa, di samping bangkai sekoci.")]
        [SerializeField] private Transform overflowOrigin;

        [Header("Pemain (boleh kosong, dicari otomatis)")]
        [SerializeField] private Transform player;
        [SerializeField] private SurvivalStats playerStats;
        [SerializeField] private CarryCapacity carryCapacity;

        /// <summary>Dipakai RunLifecycle untuk LoadInitialRegions (#88).</summary>
        public Vector3 LandingPosition => landingPoint != null ? landingPoint.position : transform.position;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("[IslandArrival] Sudah ada satu di scene. Komponen ini dihapus.", this);
                Destroy(this);
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public void Apply()
        {
            var gm = GameManager.Instance;
            if (gm == null) return;
            // 1. Jalur ContinueRun tidak melewati fungsi ini, dan barang kapal tidak boleh diterapkan dua kali.
            if (gm.hasArrivedOnIsland) return;

            ResolveReferences();
            var outcome = gm.shipOutcome;

            // 2. Pemain di pantai, menghadap hutan.
            PlacePlayer();

            // 3. HP dan infeksi dibawa dari kapal (usulan disetujui #85), status lain penuh.
            if (playerStats != null && outcome != null) playerStats.ApplyArrivalState(outcome.hpOnExit, outcome.infectionOnExit);

            // 4. Item inti (GDD bagian 3), lalu vaksin awal (#64), sebelum ApplyShipLoot.
            var inv = InventoryManager.Instance;
            if (inv != null)
            {
                for (int i = 0; i < BaseItemIds.Length; i++) AddById(inv, BaseItemIds[i], 1);
                var b = GameManager.CurrentBalance;
                int doses = (b != null ? b.infection : FallbackInfection).startingVaccineDoses;
                if (doses > 0) AddById(inv, VaccineItemId, doses);
            }
            else Debug.LogError("[IslandArrival] InventoryManager tidak ada di IslandPhase.", this);

            // 5. Barang kapal masuk inventori lewat aturan konversi #118.
            if (carryCapacity != null)
            {
                var overflow = carryCapacity.ApplyShipLoot(gm.savedShipLoot);
                // TODO #66 (WorldItemManager belum ditulis): sisa menjadi WorldItem di overflowOrigin (despawns: false),
                //      berjarak carry.overflowSpacing. Sementara hanya dicatat supaya tidak hilang diam-diam.
                if (overflow != null && overflow.Count > 0)
                    Debug.LogWarning($"[IslandArrival] {overflow.Count} jenis barang kapal tidak muat dan belum dimunculkan di pantai.", this);
            }
            else Debug.LogError("[IslandArrival] CarryCapacity tidak ditemukan. Barang kapal tidak diterapkan.", this);

            // 6. TODO #76 (NPCRoster belum ditulis): NPCRoster.Instance.SpawnRescued(outcome.rescuedNpcIds, npcOrigin).

            // 7. Kosongkan savedShipLoot dan tandai sudah tiba (GameManager.MarkArrivedOnIsland).
            gm.MarkArrivedOnIsland();

            // 8. Kebal dari kapal dilepas, lapar dan haus aktif.
            if (playerStats != null)
            {
                playerStats.SetInvulnerable(false);
                playerStats.SetDrainEnabled(true);
            }
        }

        private void ResolveReferences()
        {
            if (player == null)
            {
                var tagged = GameObject.FindGameObjectWithTag("Player");
                if (tagged != null) player = tagged.transform;
            }
            if (playerStats == null && player != null) playerStats = player.GetComponentInChildren<SurvivalStats>();
            if (carryCapacity == null) carryCapacity = CarryCapacity.Instance != null ? CarryCapacity.Instance : FindAnyObjectByType<CarryCapacity>();
        }

        private void PlacePlayer()
        {
            if (player == null || landingPoint == null) return;
            // CharacterController menimpa posisi kalau masih aktif, jadi dimatikan sebentar.
            var cc = player.GetComponent<CharacterController>();
            bool wasEnabled = cc != null && cc.enabled;
            if (cc != null) cc.enabled = false;

            player.position = landingPoint.position;
            if (cc != null) cc.enabled = wasEnabled;

            // Yaw dan pitch disimpan FirstPersonCamera, jadi arah pandang diatur lewat LookAt, bukan rotasi transform.
            if (lookTarget != null)
            {
                if (FirstPersonCamera.Instance != null) FirstPersonCamera.Instance.LookAt(lookTarget.position);
                else
                {
                    Vector3 dir = lookTarget.position - landingPoint.position;
                    dir.y = 0f;
                    if (dir.sqrMagnitude > 0.0001f) player.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
                }
            }
        }

        private void AddById(InventoryManager inv, string itemId, int amount)
        {
            var data = ItemDatabase.Get(itemId);
            if (data == null)
            {
                Debug.LogError($"[IslandArrival] ItemData '{itemId}' tidak ada di ItemDatabase.", this);
                return;
            }
            int left = inv.AddItem(data, amount);
            if (left > 0) Debug.LogWarning($"[IslandArrival] '{itemId}' tidak muat sebanyak {left}.", this);
        }

        private void OnDrawGizmosSelected()
        {
            if (landingPoint != null) { Gizmos.color = Color.cyan; Gizmos.DrawWireSphere(landingPoint.position, 0.5f); }
            if (npcOrigin != null) { Gizmos.color = Color.green; Gizmos.DrawWireSphere(npcOrigin.position, 0.4f); }
            if (overflowOrigin != null) { Gizmos.color = Color.yellow; Gizmos.DrawWireSphere(overflowOrigin.position, 0.4f); }
        }
    }
}
