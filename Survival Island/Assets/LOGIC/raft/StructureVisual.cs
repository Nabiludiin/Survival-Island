using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Tampilan struktur rusak: sharedMaterial per tahap dan mesh khusus pada tahap terakhir. Collider tidak berubah, jadi
    /// NavMesh dan peluru tetap sama. Blueprint: raft/BuildingIntegrity.md bagian "Tampilan rusak" (#47).
    /// Mendengar BaseHealth.HpRatioChanged (event per objek), bukan StructureDamagedEvent global, supaya satu pukulan
    /// tidak membangunkan 400 struktur.
    /// </summary>
    [RequireComponent(typeof(BaseHealth))]
    public class StructureVisual : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private MeshFilter meshFilter;

        private BaseHealth health;
        private StructureIntegrityInfo info;
        private Material originalMaterial;
        private Mesh originalMesh;
        private int currentStage = -1;

        private void Awake()
        {
            health = GetComponent<BaseHealth>();
            info = GetComponent<StructureIntegrityInfo>();
            if (meshRenderer == null) meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (meshFilter == null && meshRenderer != null) meshFilter = meshRenderer.GetComponent<MeshFilter>();
            if (meshRenderer != null) originalMaterial = meshRenderer.sharedMaterial;
            if (meshFilter != null) originalMesh = meshFilter.sharedMesh;
        }

        private void OnEnable()
        {
            health.HpRatioChanged += HandleRatio;
            // Setelah load: tahap dihitung tanpa efek.
            Apply(health.HpRatio, false);
        }

        private void OnDisable()
        {
            health.HpRatioChanged -= HandleRatio;
        }

        private void HandleRatio(float ratio)
        {
            // Efek hanya saat turun tahap karena damage pada frame ini (bukan Initialize saat load atau perbaikan).
            bool fromDamage = health.LastDamageTime > 0f && Mathf.Approximately(health.LastDamageTime, Time.time);
            Apply(ratio, fromDamage);
        }

        /// <summary>Tahap dengan maxHpRatio terkecil yang masih >= ratio. Tahap sama: selesai.</summary>
        public void Apply(float ratio, bool playFx)
        {
            var stages = info != null ? info.damageVisuals : null;
            if (stages == null || stages.Length == 0) return;
            int stage = -1;
            float best = float.MaxValue;
            for (int i = 0; i < stages.Length; i++)
            {
                float m = stages[i].maxHpRatio;
                if (m >= ratio - 0.0001f && m < best) { best = m; stage = i; }
            }
            if (stage < 0) stage = 0;
            if (stage == currentStage) return;
            bool worse = currentStage >= 0 && stages[stage].maxHpRatio < stages[currentStage].maxHpRatio;
            currentStage = stage;

            var s = stages[stage];
            if (meshRenderer != null) meshRenderer.sharedMaterial = s.material != null ? s.material : originalMaterial;
            if (meshFilter != null) meshFilter.sharedMesh = s.mesh != null ? s.mesh : originalMesh;
            // Batas efek runtuhan (maxCollapseVfxPerSecond, #79) dipegang VFXManager lewat pool.
            if (playFx && worse && !string.IsNullOrEmpty(s.enterFxPoolTag) && VFXManager.Instance != null)
                VFXManager.Instance.Play(s.enterFxPoolTag, transform.position + Vector3.up, Quaternion.identity);
        }
    }
}
