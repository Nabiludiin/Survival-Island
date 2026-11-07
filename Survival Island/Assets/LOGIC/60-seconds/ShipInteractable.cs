using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Wujud objek ambil di kapal (GLOSSARY.md).</summary>
    public enum ShipInteractableKind { Item, NPC }

    /// <summary>
    /// Barang dan NPC yang bisa diambil di kapal. Blueprint: 60-seconds/ShipInteractable.md (#17, #18, #74, #104, #118).
    /// GDD bagian 3: Fase 1 mengambil peluru, makanan kaleng, vaksin, dan menyelamatkan NPC.
    /// Keputusan developer: vaksin tetap ada di kapal; satu kotak peluru kapal berisi 12 butir (shipPickupAmount);
    /// semua NPC berberat 4 (NPCData.weight), jadi satu NPC memenuhi tangan.
    ///
    /// Objek dari pool (ShipLoot_Generic / ShipNpc_Generic). Data diisi lewat Setup setelah SpawnFromPool, bukan di OnSpawned.
    /// Layer Interactable, collider bukan trigger (#87).
    ///
    /// Setup(NPCData) (#76), teks prompt lewat LocalizationManager (#104), serta efek VFX_ShipPickup dan suara ambil
    /// (VFXManager, AudioManager) sudah tersambung. Data barang kapal diperiksa alat Editor ShipLootAudit (menu Survival > Audit > Ship Loot).
    /// </summary>
    public class ShipInteractable : MonoBehaviour, IInteractable, IPoolable
    {
        private static readonly ShipBalance FallbackShip = new ShipBalance();
        private static readonly ShipHazardBalance FallbackHazard = new ShipHazardBalance();

        [SerializeField] private string poolTag = "ShipLoot_Generic";
        [SerializeField] private InteractHighlight highlight;
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private BoxCollider boxCollider;
        [Tooltip("Dipakai kalau ItemData.worldMesh kosong.")]
        [SerializeField] private Mesh placeholderMesh;
        [Tooltip("Hanya prefab NPC: diputar ulang di OnReturned.")]
        [SerializeField] private Animator npcAnimator;
        [SerializeField] private AudioClip pickupClip;
        [SerializeField] private AudioClip npcGrabClip;

        /// <summary>Klip ambil, diputar lewat AudioManager di Interact.</summary>
        public AudioClip PickupClip => pickupClip;
        public AudioClip NpcGrabClip => npcGrabClip;

        public string ItemID { get; private set; } = string.Empty;
        public ShipInteractableKind Kind => kind;
        public int WeightCost => weightCost;
        public int PickupAmount => pickupAmount;

        private ShipInteractableKind kind = ShipInteractableKind.Item;
        private int weightCost;
        private int pickupAmount;
        private ItemData itemData;
        private NPCData npcData;
        private bool isSetUp;
        private bool isTaken;
        private bool isHovered;
        private string promptNormal;
        private string promptFull;
        private Vector3 prefabColliderCenter;
        private Vector3 prefabColliderSize;

        private static ShipBalance Ship
        {
            get { var b = GameManager.CurrentBalance; return b != null ? b.ship : FallbackShip; }
        }

        private static ShipHazardBalance Hazard
        {
            get { var b = GameManager.CurrentBalance; return b != null ? b.shipHazard : FallbackHazard; }
        }

        private void Awake()
        {
            if (meshFilter == null) meshFilter = GetComponent<MeshFilter>();
            if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
            if (boxCollider == null) boxCollider = GetComponent<BoxCollider>();
            if (highlight == null) highlight = GetComponent<InteractHighlight>();
            if (boxCollider != null)
            {
                prefabColliderCenter = boxCollider.center;
                prefabColliderSize = boxCollider.size;
            }
        }

        private void OnEnable()
        {
            EventChannel<ShipHandChangedEvent>.Raised += HandleShipHandChanged;
            EventChannel<LocaleChangedEvent>.Raised += HandleLocaleChanged;
        }

        private void OnDisable()
        {
            EventChannel<ShipHandChangedEvent>.Raised -= HandleShipHandChanged;
            EventChannel<LocaleChangedEvent>.Raised -= HandleLocaleChanged;
        }

        // ---------------------------------------------------------------- Pengisian data

        /// <summary>Dipanggil LootSpawnerSystem tepat setelah SpawnFromPool.</summary>
        public void Setup(ItemData item)
        {
            if (item == null)
            {
                Debug.LogWarning("[ShipInteractable] Setup dengan ItemData kosong. Objek dikembalikan ke pool.", this);
                ReturnSelf();
                return;
            }

            kind = ShipInteractableKind.Item;
            itemData = item;
            ItemID = item.itemID;
            // Berat dan jumlah dari data (#118). Minimal 1 supaya data rusak tidak membuat barang gratis.
            weightCost = Mathf.Max(1, item.shipWeight);
            pickupAmount = Mathf.Max(1, item.shipPickupAmount);

            if (meshFilter != null) meshFilter.sharedMesh = item.worldMesh != null ? item.worldMesh : placeholderMesh;
            if (meshRenderer != null && item.worldMaterial != null) meshRenderer.sharedMaterial = item.worldMaterial;

            // Collider mengikuti bounds mesh, ditambah padding, supaya barang kecil mudah dibidik.
            if (boxCollider != null && meshFilter != null && meshFilter.sharedMesh != null)
            {
                Bounds b = meshFilter.sharedMesh.bounds;
                float pad = Ship.pickupColliderPadding;
                boxCollider.center = b.center;
                boxCollider.size = b.size + Vector3.one * (pad * 2f);
            }

            isSetUp = true;
            BuildPromptCache();
        }

        /// <summary>Dipanggil LifeboatExtraction.SpawnShipNpcs tepat setelah SpawnFromPool (ShipInteractable.md, #76).</summary>
        public void Setup(NPCData npc)
        {
            if (npc == null)
            {
                Debug.LogWarning("[ShipInteractable] Setup dengan NPCData kosong. Objek dikembalikan ke pool.", this);
                ReturnSelf();
                return;
            }

            kind = ShipInteractableKind.NPC;
            npcData = npc;
            ItemID = npc.npcId;
            weightCost = Mathf.Max(1, npc.weight);
            pickupAmount = 1;

            isSetUp = true;
            BuildPromptCache();
        }

        private void BuildPromptCache()
        {
            // Teks lewat LocalizationManager (#104). Tanpa LocalizationManager, kunci nama dipakai apa adanya.
            var loc = LocalizationManager.Instance;
            string nameKey = kind == ShipInteractableKind.NPC
                ? (npcData != null ? npcData.displayNameKey : null)
                : (itemData != null ? itemData.displayNameKey : null);
            if (string.IsNullOrEmpty(nameKey)) nameKey = ItemID;
            string promptKey = kind == ShipInteractableKind.NPC ? "prompt.rescue" : "prompt.grab";
            if (loc != null)
            {
                promptNormal = loc.Format(promptKey, "name", loc.Get(nameKey));
                promptFull = loc.Get("prompt.handsFull");
            }
            else
            {
                promptNormal = (kind == ShipInteractableKind.NPC ? "Selamatkan " : "Ambil ") + nameKey;
                promptFull = "Tangan penuh. Setor ke Sekoci";
            }
        }

        // ---------------------------------------------------------------- IInteractable

        public bool IsAvailable => isSetUp && !isTaken && gameObject.activeInHierarchy && !IsSubmergedNpc();

        public float HoldSeconds => 0f;   // Barang dan NPC diambil langsung

        public string GetPrompt() => WouldFit() ? promptNormal : promptFull;

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            if (!IsAvailable) return InteractBlockReason.Locked;
            // Menutup celah pada frame waktu habis atau saat berangkat.
            var flow = LifeboatExtraction.Instance;
            if (flow == null || flow.State != ShipFlowState.Scavenge) return InteractBlockReason.Locked;
            if (ctx.Inventory == null) return InteractBlockReason.Locked;
            if (ctx.Inventory.ShipHandWeight + weightCost > ctx.Inventory.ShipHandCapacity) return InteractBlockReason.HandsFull;
            return InteractBlockReason.None;
        }

        public void OnHoverEnter()
        {
            isHovered = true;
            RefreshHighlight();
        }

        public void OnHoverExit()
        {
            isHovered = false;
            if (highlight != null) highlight.SetState(HighlightState.Off);
        }

        public bool Interact(in InteractContext ctx)
        {
            if (CanInteract(ctx) != InteractBlockReason.None) return false;
            // InventoryManager menolak sendiri kalau tangan penuh, jadi tidak ada barang yang hilang.
            if (!ctx.Inventory.AddShipHandItem(ItemID, weightCost, pickupAmount)) return false;
            isTaken = true;

            // Barang: efek VFX_ShipPickup dan suara ambil. NPC: suara saja, tanpa partikel (ShipInteractable.md).
            if (kind == ShipInteractableKind.NPC) AudioManager.Instance?.PlaySFX(npcGrabClip);
            else
            {
                VFXManager.Instance?.Play("VFX_ShipPickup", transform.position, Quaternion.identity);
                AudioManager.Instance?.PlaySFX(pickupClip);
            }

            // NPC yang dibawa hilang dari dunia. HUD menampilkan ikonnya lewat ShipHandChangedEvent.
            // NPCRescuedEvent baru dikirim saat disetor (LifeboatExtraction.DepositHand).
            ReturnSelf();
            return true;
        }

        // ---------------------------------------------------------------- Highlight

        private bool WouldFit()
        {
            var inv = InventoryManager.Instance;
            return inv != null && inv.ShipHandWeight + weightCost <= inv.ShipHandCapacity;
        }

        private void RefreshHighlight()
        {
            if (!isHovered || highlight == null) return;
            // Merah berarti barang tidak muat (LifeboatExtraction.md).
            highlight.SetState(WouldFit() ? HighlightState.Available : HighlightState.Blocked);
        }

        private void HandleShipHandChanged(ShipHandChangedEvent e) { RefreshHighlight(); }

        private void HandleLocaleChanged(LocaleChangedEvent e)
        {
            if (isSetUp) BuildPromptCache();
        }

        /// <summary>NPC sedalam waterHeadDepth tidak bisa diambil (ShipHazardManager.md). Barang terendam tetap bisa.</summary>
        private bool IsSubmergedNpc()
        {
            if (kind != ShipInteractableKind.NPC) return false;
            var hazard = ShipHazardManager.Instance;
            return hazard != null && hazard.WaterLevel - transform.position.y >= Hazard.waterHeadDepth;
        }

        // ---------------------------------------------------------------- Pool

        public void OnSpawned()
        {
            isTaken = false;
            isHovered = false;
            if (highlight != null) highlight.SetState(HighlightState.Off);
            if (boxCollider != null) boxCollider.enabled = true;
            if (meshRenderer != null) meshRenderer.enabled = true;
            transform.localScale = Vector3.one;
        }

        public void OnReturned()
        {
            itemData = null;
            npcData = null;
            ItemID = string.Empty;
            kind = ShipInteractableKind.Item;
            weightCost = 0;
            pickupAmount = 0;
            isSetUp = false;
            promptNormal = null;
            promptFull = null;
            isHovered = false;
            if (highlight != null) highlight.SetState(HighlightState.Off);
            if (meshFilter != null) meshFilter.sharedMesh = null;
            if (boxCollider != null)
            {
                boxCollider.center = prefabColliderCenter;
                boxCollider.size = prefabColliderSize;
            }
            if (npcAnimator != null) npcAnimator.Rebind();   // Pose panik mulai dari awal
        }

        private void ReturnSelf()
        {
            if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(poolTag, gameObject);
            else gameObject.SetActive(false);
        }
    }
}
