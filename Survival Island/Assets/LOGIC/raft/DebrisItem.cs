using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu barang hanyut (pool Debris_Generic, layer Interactable). Bergerak lewat Transform di sepanjang DebrisLane,
    /// tanpa Rigidbody. Mesh diganti dari ItemData.worldMesh. Blueprint: raft/DebrisRetrieval.md (#106).
    /// Saat tertahan karena inventori penuh, barang bisa diambil dengan E lewat IInteractable.
    /// Ditembak tidak berpengaruh, karena tidak punya IDamageable.
    /// </summary>
    public class DebrisItem : MonoBehaviour, IInteractable, IPoolable
    {
        public const string PoolTag = "Debris_Generic";

        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private InteractHighlight highlight;

        private Mesh defaultMesh;
        private Material defaultMaterial;
        private float bobPhase;

        public ItemData item;
        public int amount;
        public DebrisLane lane;
        public int segment;
        public float segmentT;
        public bool isHooked;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        private void Awake()
        {
            if (meshFilter == null) meshFilter = GetComponentInChildren<MeshFilter>();
            if (meshRenderer == null) meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (meshFilter != null) defaultMesh = meshFilter.sharedMesh;
            if (meshRenderer != null) defaultMaterial = meshRenderer.sharedMaterial;
        }

        // ---------------------------------------------------------------- IPoolable

        public void OnSpawned()
        {
            item = null;
            amount = 0;
            lane = null;
            segment = 0;
            segmentT = 0f;
            isHooked = false;
            bobPhase = 0f;
            transform.localScale = Vector3.one;
            if (meshFilter != null) meshFilter.sharedMesh = defaultMesh;
            if (meshRenderer != null) meshRenderer.sharedMaterial = defaultMaterial;
            if (highlight != null) highlight.SetState(HighlightState.Off);
        }

        public void OnReturned()
        {
            // Lepas semua rujukan supaya tidak menunjuk jalur lama.
            item = null;
            lane = null;
            isHooked = false;
            amount = 0;
        }

        public void Setup(ItemData data, int count, DebrisLane l, float phase)
        {
            item = data;
            amount = Mathf.Max(1, count);
            lane = l;
            segment = 0;
            segmentT = 0f;
            isHooked = false;
            bobPhase = phase;
            if (data != null)
            {
                if (meshFilter != null && data.worldMesh != null) meshFilter.sharedMesh = data.worldMesh;
                if (meshRenderer != null && data.worldMaterial != null) meshRenderer.sharedMaterial = data.worldMaterial;
            }
            if (lane != null && lane.IsValid) transform.position = lane.points[0];
        }

        // ---------------------------------------------------------------- Hanyut

        private void Update()
        {
            if (isHooked || lane == null || !lane.IsValid) return;
            float speed = Balance != null ? Balance.debris.driftSpeed : 1.2f;
            float move = speed * Time.deltaTime;   // Dijeda: tidak bergerak
            var pts = lane.points;
            // 1. Maju sepanjang titik jalur.
            while (move > 0f && segment < pts.Length - 1)
            {
                float len = Vector3.Distance(pts[segment], pts[segment + 1]);
                if (len < 0.001f) { segment++; segmentT = 0f; continue; }
                float left = (1f - segmentT) * len;
                if (move < left) { segmentT += move / len; move = 0f; }
                else { move -= left; segment++; segmentT = 0f; }
            }
            // 3. Melewati titik terakhir: hilang sendiri.
            if (segment >= pts.Length - 1)
            {
                if (DebrisRetrieval.Instance != null) DebrisRetrieval.Instance.Despawn(this);
                else gameObject.SetActive(false);
                return;
            }
            // 2. Tinggi Y dari titik jalur ditambah Sin kecil (tanpa raycast tiap frame).
            Vector3 p = Vector3.Lerp(pts[segment], pts[segment + 1], segmentT);
            float amp = Balance != null ? Balance.debris.bobAmplitude : 0.08f;
            float freq = Balance != null ? Balance.debris.bobFrequency : 1.2f;
            bobPhase += Time.deltaTime * freq * Mathf.PI * 2f;
            p.y += Mathf.Sin(bobPhase) * amp;
            transform.position = p;
        }

        /// <summary>Kembali hanyut dari titik terdekat pada jalurnya.</summary>
        public void ResumeDrift()
        {
            isHooked = false;
            if (lane != null) lane.FindNearest(transform.position, out segment, out segmentT);
        }

        // ---------------------------------------------------------------- IInteractable

        public bool IsAvailable => isActiveAndEnabled && item != null;
        public float HoldSeconds => 0f;
        public string GetPrompt() => "Ambil";   // TODO(LocalizationManager #104): kunci teks

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            if (ctx.IsCarryingHeavyItem) return InteractBlockReason.CarryingHeavyItem;
            return InteractBlockReason.None;
        }

        public void OnHoverEnter() { if (highlight != null) highlight.SetState(HighlightState.Available); }
        public void OnHoverExit() { if (highlight != null) highlight.SetState(HighlightState.Off); }

        public bool Interact(in InteractContext ctx)
        {
            return DebrisRetrieval.Instance != null && DebrisRetrieval.Instance.TryPickUp(this, ctx.Inventory);
        }
    }
}
