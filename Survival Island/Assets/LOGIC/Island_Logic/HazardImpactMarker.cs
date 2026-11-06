using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Lingkaran penanda titik jatuh (pool Hazard_ImpactMarker). Blueprint: Island_Logic/DisasterManager.md bagian 1 "Pool" (#70).
    /// Dipakai Fireball (erupsi) dan pohon tumbang (badai). Kembali ke pool sendiri setelah lifeSeconds,
    /// atau lewat ReturnNow saat Fireball mendarat.
    /// </summary>
    public class HazardImpactMarker : MonoBehaviour, IPoolable
    {
        public const string PoolTag = "Hazard_ImpactMarker";

        private float lifeLeft;
        private bool returned;
        private Vector3 baseScale = Vector3.one;
        private bool scaleCached;

        private void Awake()
        {
            CacheScale();
        }

        private void CacheScale()
        {
            if (scaleCached) return;
            baseScale = transform.localScale;
            scaleCached = true;
        }

        /// <summary>Isi setelah spawn. lifeSeconds &lt;= 0 berarti menunggu ReturnNow.</summary>
        public void Setup(float radius, float lifeSeconds)
        {
            CacheScale();
            transform.localScale = new Vector3(baseScale.x * radius * 2f, baseScale.y, baseScale.z * radius * 2f);
            lifeLeft = lifeSeconds;
        }

        private void Update()
        {
            if (lifeLeft <= 0f) return;
            // Waktu gameplay: penanda berhenti saat jeda (DisasterManager.md bagian 4).
            lifeLeft -= Time.deltaTime;
            if (lifeLeft <= 0f) ReturnNow();
        }

        public void ReturnNow()
        {
            if (returned) return;
            returned = true;
            if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(gameObject);
            else gameObject.SetActive(false);
        }

        // ---------------------------------------------------------------- IPoolable

        public void OnSpawned()
        {
            CacheScale();
            returned = false;
            lifeLeft = 0f;
            transform.localScale = baseScale;
        }

        public void OnReturned()
        {
            returned = true;
            lifeLeft = 0f;
        }
    }
}
