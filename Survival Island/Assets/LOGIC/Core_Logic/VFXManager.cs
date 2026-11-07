using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SurvivalIsland
{
    /// <summary>Aturan satu efek di GameBalanceConfig.vfx.catalog (VFXManager.md bagian 1).</summary>
    [Serializable]
    public struct VFXEntry
    {
        public string poolTag;
        public float lifetimeSeconds;     // 0 berarti loop, dan pemanggil wajib menghentikannya
        public int maxActive;
        public float cullDistance;        // 0 berarti selalu tampil
        public float minIntervalSeconds;

        public VFXEntry(string poolTag, float lifetimeSeconds, int maxActive, float cullDistance, float minIntervalSeconds)
        {
            this.poolTag = poolTag;
            this.lifetimeSeconds = lifetimeSeconds;
            this.maxActive = maxActive;
            this.cullDistance = cullDistance;
            this.minIntervalSeconds = minIntervalSeconds;
        }
    }

    /// <summary>Pegangan efek loop tanpa garbage. Tidak valid lagi setelah efeknya kembali ke pool.</summary>
    public readonly struct VFXHandle
    {
        public readonly PooledEffect effect;
        public readonly int generation;

        public VFXHandle(PooledEffect effect, int generation)
        {
            this.effect = effect;
            this.generation = generation;
        }

        public bool IsValid => effect != null && effect.Generation == generation;
    }

    /// <summary>
    /// Satu pintu untuk efek visual sekali muncul: darah, debu, percikan gacha, muzzle flash, dan impact peluru.
    /// Blueprint: Core_Logic/VFXManager.md (#15). Tidak disebut langsung di GDD; mendukung GDD bagian 3 (combat),
    /// bagian 5 (menebang dan menambang), dan bagian 7 (Lucky Machine).
    ///
    /// Urutan bootstrap ke-13, tepat setelah ObjectPooler. Efek berasal dari ObjectPooler (Claude.md Tahap 6 poin 1).
    /// Efek baru dibuang saat batas tercapai; efek yang masih hidup tidak dipakai ulang.
    /// </summary>
    public class VFXManager : MonoBehaviour, IBootstrapService
    {
        public static VFXManager Instance { get; private set; }

        [Tooltip("Tag impact per SurfaceType (urutan enum: Pasir, Rumput, Tanah, Batu, Salju, Lumpur, Kayu, Logam).")]
        // Usulan disetujui developer (#15 poin 2): tiga kelompok impact, bukan satu per SurfaceType.
        [SerializeField] private string[] impactBySurface =
        {
            "VFX_ImpactDust", "VFX_ImpactDust", "VFX_ImpactDust", "VFX_Impact",
            "VFX_ImpactDust", "VFX_ImpactDust", "VFX_ImpactWood", "VFX_Impact"
        };

        [SerializeField] private string defaultImpactTag = "VFX_Impact";
        [SerializeField] private string bloodTag = "VFX_Blood";
        [SerializeField] private string bloodHeavyTag = "VFX_BloodHeavy";
        [SerializeField] private string structureDustTag = "VFX_WoodDust";

        private readonly Dictionary<string, VFXEntry> entries = new Dictionary<string, VFXEntry>(32);
        private readonly Dictionary<string, int> activeCount = new Dictionary<string, int>(32);
        private readonly Dictionary<string, float> lastPlayTime = new Dictionary<string, float>(32);
        private readonly List<PooledEffect> activeEffects = new List<PooledEffect>(128);
        private readonly HashSet<string> warnedTags = new HashSet<string>();

        private Camera cachedCamera;
        private int viewmodelLayer = -1;
        private int enemyHitboxLayer = -1;

        /// <summary>1, mediumQualityDensity, atau lowQualityDensity menurut SettingsManager.qualityPreset.</summary>
        public float densityMultiplier { get; private set; } = 1f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[VFXManager] Instance ganda. Periksa prefab PersistentRoot.");
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<EnemyHitEvent>.Raised += HandleEnemyHit;
            EventChannel<StructureDestroyedEvent>.Raised += HandleStructureDestroyed;
            EventChannel<SettingsChangedEvent>.Raised += HandleSettingsChanged;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            SceneManager.activeSceneChanged += HandleActiveSceneChanged;
            if (EventChannel<SettingsChangedEvent>.HasLast) RefreshDensity();
        }

        private void OnDisable()
        {
            EventChannel<EnemyHitEvent>.Raised -= HandleEnemyHit;
            EventChannel<StructureDestroyedEvent>.Raised -= HandleStructureDestroyed;
            EventChannel<SettingsChangedEvent>.Raised -= HandleSettingsChanged;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            SceneManager.activeSceneChanged -= HandleActiveSceneChanged;
        }

        // ---------------------------------------------------------------- Layanan

        public void Initialize()
        {
            entries.Clear();
            var b = GameManager.CurrentBalance;
            var catalog = b != null ? b.vfx.catalog : null;
            if (catalog != null)
            {
                for (int i = 0; i < catalog.Length; i++)
                {
                    var e = catalog[i];
                    if (string.IsNullOrEmpty(e.poolTag)) continue;
                    entries[e.poolTag] = e;
                    activeCount[e.poolTag] = 0;
                    lastPlayTime[e.poolTag] = -999f;
                    if (ObjectPooler.Instance != null)
                    {
                        if (!ObjectPooler.Instance.HasPool(e.poolTag))
                            Debug.LogWarning($"[VFXManager] Tag '{e.poolTag}' belum terdaftar di ObjectPooler.pools.");
                        else if (ObjectPooler.Instance.GetPoolSize(e.poolTag) < e.maxActive)
                            Debug.LogWarning($"[VFXManager] Isi pool '{e.poolTag}' lebih kecil dari maxActive {e.maxActive}.");
                    }
                }
            }

            viewmodelLayer = LayerMask.NameToLayer(PhysicsMasks.PlayerViewmodelLayer);
            enemyHitboxLayer = LayerMask.NameToLayer(PhysicsMasks.EnemyHitboxLayer);
            RefreshDensity();
        }

        public void ResetForNewRun()
        {
            ReturnAll();
            var keys = ListPool.Keys(lastPlayTime);
            for (int i = 0; i < keys.Count; i++) lastPlayTime[keys[i]] = -999f;
            cachedCamera = null;
        }

        // ---------------------------------------------------------------- Fungsi publik

        public VFXHandle Play(string poolTag, Vector3 pos, Quaternion rot, Transform follow = null, bool viewmodel = false)
        {
            // 1. Tag tak dikenal: peringatan sekali.
            if (string.IsNullOrEmpty(poolTag) || !entries.TryGetValue(poolTag, out var entry))
            {
                if (poolTag != null && warnedTags.Add(poolTag)) Debug.LogWarning($"[VFXManager] Tag efek '{poolTag}' tidak ada di vfx.catalog.");
                return default;
            }

            // 2. Jeda minimum per tag, supaya horde tidak membuat ratusan efek dalam satu frame.
            float now = Time.time;
            if (entry.minIntervalSeconds > 0f && now - lastPlayTime[poolTag] < entry.minIntervalSeconds) return default;

            // 3. Batas jumlah (dikecilkan kualitas). Pool penuh = efek baru dibuang tanpa log.
            int limit = Mathf.CeilToInt(entry.maxActive * densityMultiplier);
            if (activeCount[poolTag] >= limit) return default;

            // 4. Jarak dari kamera. Kamera kosong (loading) = langkah ini dilewati.
            if (entry.cullDistance > 0f)
            {
                var cam = GetCamera();
                if (cam != null && (cam.transform.position - pos).sqrMagnitude > entry.cullDistance * entry.cullDistance) return default;
            }

            // 5.
            var pooler = ObjectPooler.Instance;
            if (pooler == null) return default;
            var effect = pooler.SpawnFromPool<PooledEffect>(poolTag, pos, rot);
            if (effect == null) return default;

            // 6. Efek NPC tetap di layer dunia; hanya efek pemain di viewmodel (GunController).
            effect.Setup(entry.lifetimeSeconds, follow, viewmodel ? viewmodelLayer : -1);

            // 7.
            effect.managerIndex = activeEffects.Count;
            activeEffects.Add(effect);
            activeCount[poolTag]++;
            lastPlayTime[poolTag] = now;
            return new VFXHandle(effect, effect.Generation);
        }

        public VFXHandle PlayImpact(in RaycastHit hit, string overrideTag = null)
        {
            if (hit.collider == null) return default;
            // 1. Darah datang lewat EnemyHitEvent (SurfaceDetection.md), jadi hitbox musuh tidak memunculkan impact.
            if (hit.collider.gameObject.layer == enemyHitboxLayer) return default;

            // 2. Tag khusus senjata, atau tag menurut permukaan. Tidak menembakkan raycast baru.
            string tag = overrideTag;
            if (string.IsNullOrEmpty(tag))
            {
                tag = defaultImpactTag;
                if (SurfaceDetection.Instance != null)
                {
                    int s = (int)SurfaceDetection.Instance.GetSurfaceFromHit(hit);
                    if (impactBySurface != null && s >= 0 && s < impactBySurface.Length && !string.IsNullOrEmpty(impactBySurface[s]))
                        tag = impactBySurface[s];
                }
            }

            // 3.
            Vector3 normal = hit.normal.sqrMagnitude > 0.0001f ? hit.normal : Vector3.up;
            return Play(tag, hit.point, Quaternion.LookRotation(normal));
        }

        /// <summary>Hentikan efek loop. Handle lama (efek sudah kembali ke pool) diabaikan.</summary>
        public void Stop(VFXHandle handle)
        {
            if (handle.IsValid) handle.effect.StopEmitting();
        }

        /// <summary>Kembalikan semua efek aktif. Dipanggil saat RunEndedEvent dan ResetForNewRun.</summary>
        public void ReturnAll()
        {
            var pooler = ObjectPooler.Instance;
            for (int i = activeEffects.Count - 1; i >= 0; i--)
            {
                var e = activeEffects[i];
                if (e == null) continue;
                if (pooler != null) pooler.ReturnToPool(e.gameObject);
            }
            // Efek yang dihancurkan dari luar tidak lewat NotifyReturned, jadi daftar dan hitungan dikosongkan di sini.
            for (int i = 0; i < activeEffects.Count; i++) if (activeEffects[i] != null) activeEffects[i].managerIndex = -1;
            activeEffects.Clear();
            var keys = ListPool.Keys(activeCount);
            for (int i = 0; i < keys.Count; i++) activeCount[keys[i]] = 0;
        }

        /// <summary>Kembalikan efek ke pool sebelum umurnya habis.</summary>
        public void Release(PooledEffect effect)
        {
            if (effect == null) return;
            if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(effect.gameObject);
        }

        /// <summary>Dipanggil PooledEffect.OnReturned, termasuk saat ObjectPooler.ReturnAllActive (#82).</summary>
        internal void NotifyReturned(PooledEffect effect)
        {
            int idx = effect.managerIndex;
            if (idx < 0 || idx >= activeEffects.Count || activeEffects[idx] != effect) return;

            int last = activeEffects.Count - 1;
            var moved = activeEffects[last];
            activeEffects[idx] = moved;
            if (moved != null) moved.managerIndex = idx;
            activeEffects.RemoveAt(last);
            effect.managerIndex = -1;

            string tag = effect.PoolTag;
            if (tag != null && activeCount.TryGetValue(tag, out int c)) activeCount[tag] = Mathf.Max(0, c - 1);
        }

        // ---------------------------------------------------------------- Event

        private void HandleEnemyHit(EnemyHitEvent e)
        {
            // Usulan disetujui developer (#15 poin 1): satu warna darah untuk zombie dan hewan.
            string tag = e.isKill || e.bodyPart == BodyPart.Head ? bloodHeavyTag : bloodTag;
            // Event tidak membawa normal permukaan, jadi efek menghadap kamera.
            var cam = GetCamera();
            Vector3 toCam = cam != null ? cam.transform.position - e.position : Vector3.up;
            Play(tag, e.position, toCam.sqrMagnitude > 0.0001f ? Quaternion.LookRotation(toCam) : Quaternion.identity);
        }

        private void HandleStructureDestroyed(StructureDestroyedEvent e)
        {
            Play(structureDustTag, e.position, Quaternion.identity);
        }

        private void HandleSettingsChanged(SettingsChangedEvent e) => RefreshDensity();

        private void HandleRunEnded(RunEndedEvent e) => ReturnAll();

        private void HandleActiveSceneChanged(Scene from, Scene to) => cachedCamera = null;

        private void RefreshDensity()
        {
            var s = SettingsManager.Instance;
            var b = GameManager.CurrentBalance;
            if (s == null || b == null) { densityMultiplier = 1f; return; }
            switch (s.qualityPreset)
            {
                case QualityPreset.Low: densityMultiplier = b.vfx.lowQualityDensity; break;
                case QualityPreset.Medium: densityMultiplier = b.vfx.mediumQualityDensity; break;
                // Custom tidak disebut blueprint: dianggap kualitas penuh (lihat Catatan Implementasi).
                default: densityMultiplier = 1f; break;
            }
        }

        private Camera GetCamera()
        {
            if (cachedCamera == null) cachedCamera = Camera.main;
            return cachedCamera;
        }

        /// <summary>Daftar kunci sementara tanpa alokasi berulang (mengubah nilai Dictionary saat iterasi dilarang).</summary>
        private static class ListPool
        {
            private static readonly List<string> buffer = new List<string>(32);

            public static List<string> Keys<TValue>(Dictionary<string, TValue> dict)
            {
                buffer.Clear();
                foreach (var k in dict.Keys) buffer.Add(k);
                return buffer;
            }
        }
    }
}
