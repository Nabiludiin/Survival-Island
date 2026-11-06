using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Item yang tergeletak di dunia. Satu pool generik "WorldItem_Generic", mesh dari ItemData.worldMesh.
    /// Blueprint: Core_Logic/ItemUseSystem.md bagian "WorldItem" (#66). Layer WorldItem (PhysicsLayers.md).
    ///
    /// IInteractable (#17): Interact memanggil TryPickUp(ctx.Inventory), tanpa tahan (IslandInteractable.md, "Item jatuh").
    /// sourceSpawner/sourcePointIndex (LootSpawnerSystem #89): diisi spawner, dikosongkan OnReturned. homeRegion (#88):
    /// diisi WorldItemManager dari RegionStreaming.RegionOf saat muncul dan saat dicatat.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class WorldItem : MonoBehaviour, IPoolable, IInteractable
    {
        public const string PromptKey = "prompt.pickup";   // Kunci teks #104: "Ambil"

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

        /// <summary>Sumber loot (#89). null kalau item dijatuhkan, tumpah dari peti, atau jatuh dari musuh.</summary>
        public LootSpawnerSystem sourceSpawner;
        public int sourcePointIndex = -1;
        /// <summary>Region tempat item berada (ItemUseSystem.md, #88).</summary>
        public RegionType homeRegion;

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
            // Rujukan lama tidak boleh memanggil MarkTaken pada spawner yang salah (LootSpawnerSystem.md Kasus Khusus).
            sourceSpawner = null;
            sourcePointIndex = -1;
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

        /// <summary>Pindahkan item diam di posisi baru (pulih dari RegionStateStore, atau dipindah air pasang #91).</summary>
        public void PlaceAt(Vector3 pos, Quaternion rot)
        {
            transform.SetPositionAndRotation(pos, rot);
            if (body == null) return;
            body.position = pos;
            body.rotation = rot;
            if (!body.isKinematic)
            {
                body.linearVelocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
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
                // Item tetap di tanah (#22): "Inventori penuh".
                if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.inventory_full", NotificationKind.Warning);
                return false;
            }
            if (rest > 0)
            {
                Amount = rest;
                // Tanpa ini, load memulihkan jumlah awal dan barang berlipat (ItemUseSystem.md langkah 3).
                if (sourceSpawner != null) sourceSpawner.UpdateRemaining(sourcePointIndex, rest);
                if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.partial_pickup", NotificationKind.Info);   // "Sebagian terambil" (#22)
                return true;
            }
            if (sourceSpawner != null) sourceSpawner.MarkTaken(sourcePointIndex);
            if (WorldItemManager.Instance != null) WorldItemManager.Instance.Despawn(this);
            else if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(PoolTag, gameObject);
            return true;
        }

        // ---------------------------------------------------------------- IInteractable (#17)

        public bool IsAvailable => isActiveAndEnabled && Instance != null && Amount > 0;

        public float HoldSeconds => 0f;

        public string GetPrompt()
        {
            var loc = LocalizationManager.Instance;
            return loc != null ? loc.Get(PromptKey) : PromptKey;
        }

        public InteractBlockReason CanInteract(in InteractContext ctx) =>
            IsAvailable ? InteractBlockReason.None : InteractBlockReason.Locked;

        public void OnHoverEnter() { }

        public void OnHoverExit() { }

        public bool Interact(in InteractContext ctx)
        {
            var inv = ctx.Inventory != null ? ctx.Inventory : InventoryManager.Instance;
            return TryPickUp(inv);
        }

        /// <summary>Item yang masuk air kehilangan kecepatan dan tenggelam, lalu dihapus setelah 1 jam game (ItemUseSystem.md).</summary>
        private void OnTriggerEnter(Collider other)
        {
            if (Instance == null || other.gameObject.layer != LayerMask.NameToLayer(PhysicsMasks.WaterLayer)) return;
            EnterWater();
        }

        /// <summary>Aturan air (#66). Dipakai juga WorldItemManager.SweepBelow untuk barang biasa saat air pasang (#91).</summary>
        public void EnterWater()
        {
            if (Instance == null) return;
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
