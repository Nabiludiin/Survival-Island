using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Item yang tergeletak di dunia. Satu pool generik "WorldItem_Generic", mesh dari ItemData.worldMesh.
    /// Blueprint: Core_Logic/ItemUseSystem.md bagian "WorldItem" (#66). Layer WorldItem (PhysicsLayers.md).
    ///
    /// Catatan implementasi (Langkah 3): IInteractable dan InteractContext milik #17 (PlayerInteractor) belum ada di
    /// branch ini. Alur ambil ditulis di TryPickUp(InventoryManager); TODO #17: implementasikan
    /// IInteractable.Interact(in InteractContext ctx) => TryPickUp(ctx.Inventory).
    /// Belum ada: sourceSpawner/sourcePointIndex (LootSpawnerSystem #89) dan homeRegion (RegionType, #88).
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class WorldItem : MonoBehaviour, IPoolable
    {
        public const string PoolTag = "WorldItem_Generic";

        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private MeshCollider meshCollider;
        [SerializeField] private float waterDrag = 8f;   // Hambatan saat masuk air. Data prefab, bukan angka penyetelan run

        private Rigidbody body;
        private Mesh defaultMesh;
        private Material defaultMaterial;
        private float defaultDrag;
        private Vector3 defaultScale;

        public ItemInstance Instance { get; private set; }
        public int Amount { get; private set; }
        public bool Despawns { get; private set; }
        public float despawnAtGameHour = -1f;            // -1 = tidak hilang
        public float SpawnedAtGameHour { get; private set; }

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            if (meshFilter == null) meshFilter = GetComponentInChildren<MeshFilter>();
            if (meshRenderer == null) meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (meshCollider == null) meshCollider = GetComponentInChildren<MeshCollider>();
            defaultMesh = meshFilter != null ? meshFilter.sharedMesh : null;
            defaultMaterial = meshRenderer != null ? meshRenderer.sharedMaterial : null;
            defaultDrag = body.linearDamping;
            defaultScale = transform.localScale;
        }

        // ---------------------------------------------------------------- IPoolable

        public void OnSpawned()
        {
            transform.localScale = defaultScale;
            body.linearVelocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            body.linearDamping = defaultDrag;
            body.isKinematic = false;
        }

        public void OnReturned()
        {
            Instance = null;
            Amount = 0;
            Despawns = false;
            despawnAtGameHour = -1f;
            if (body != null)
            {
                body.linearVelocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
            if (meshFilter != null) meshFilter.sharedMesh = defaultMesh;
            if (meshRenderer != null) meshRenderer.sharedMaterial = defaultMaterial;
        }

        // ---------------------------------------------------------------- Isi

        /// <summary>Dipanggil WorldItemManager setelah spawn. Memasang mesh dan collider dari data.</summary>
        public void Setup(ItemInstance inst, int amount, bool despawns, float despawnAt)
        {
            Instance = inst;
            Amount = amount;
            Despawns = despawns;
            despawnAtGameHour = despawns ? despawnAt : -1f;
            SpawnedAtGameHour = ItemUseSystem.CurrentTotalGameHours();

            var data = inst != null ? ItemDatabase.Get(inst.itemId) : null;
            if (data != null && meshFilter != null && data.worldMesh != null) meshFilter.sharedMesh = data.worldMesh;
            if (data != null && meshRenderer != null && data.worldMaterial != null) meshRenderer.sharedMaterial = data.worldMaterial;
            if (meshCollider != null && meshFilter != null) meshCollider.sharedMesh = meshFilter.sharedMesh;
        }

        /// <summary>Dorongan kecil saat dibuang (ItemUseSystem.md, DropFromSlot langkah 4).</summary>
        public void Push(Vector3 impulse)
        {
            if (body != null && !body.isKinematic) body.AddForce(impulse, ForceMode.Impulse);
        }

        /// <summary>
        /// Ambil item lewat E (#17). Inventori penuh: item tetap di tanah. Sebagian muat: sisanya tetap di tanah.
        /// Semua masuk: kembali ke pool.
        /// </summary>
        public bool TryPickUp(InventoryManager inventory)
        {
            if (inventory == null || Instance == null || Amount <= 0) return false;
            int rest = inventory.AddItem(Instance, Amount);
            if (rest == Amount)
            {
                // TODO #22: notifikasi "Inventori penuh".
                return false;
            }
            if (rest > 0)
            {
                Amount = rest;
                // TODO #89: sourceSpawner?.UpdateRemaining(sourcePointIndex, rest); notifikasi "Sebagian terambil" (#22).
                return true;
            }
            // TODO #89: sourceSpawner?.MarkTaken(sourcePointIndex);
            if (WorldItemManager.Instance != null) WorldItemManager.Instance.Despawn(this);
            else if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(PoolTag, gameObject);
            return true;
        }

        /// <summary>Item yang masuk air kehilangan kecepatan dan tenggelam, lalu dihapus setelah 1 jam game (ItemUseSystem.md).</summary>
        private void OnTriggerEnter(Collider other)
        {
            if (Instance == null || other.gameObject.layer != LayerMask.NameToLayer(PhysicsMasks.WaterLayer)) return;
            body.linearDamping = waterDrag;
            var data = ItemDatabase.Get(Instance.itemId);
            if (data != null && !data.isDroppable) return;   // Item penting tidak hilang (#116)
            float soon = ItemUseSystem.CurrentTotalGameHours() + 1f;
            if (despawnAtGameHour < 0f || despawnAtGameHour > soon)
            {
                Despawns = true;
                despawnAtGameHour = soon;
            }
        }
    }
}
