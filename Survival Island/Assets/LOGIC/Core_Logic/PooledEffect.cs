using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// IPoolable untuk efek sekali muncul (muzzle flash, impact, darah, debu). Berasal dari GunController, dipindah ke
    /// VFXManager. Blueprint: Core_Logic/VFXManager.md bagian "PooledEffect" (#15, #58).
    ///
    /// Efek tidak pernah di-SetParent; mengikuti target lewat follow di LateUpdate, supaya tidak ikut hancur bersama
    /// scene atau ikut masuk pool zombie yang ditempeli.
    /// </summary>
    [DisallowMultipleComponent]
    public class PooledEffect : MonoBehaviour, IPoolable
    {
        private ParticleSystem[] systems;
        private float[] baseRateOverTime;
        private ParticleSystem.Burst[][] baseBursts;
        private Renderer[] renderers;
        private int[] prefabLayers;
        private PooledObject pooled;
        private bool cached;

        private float remaining;
        private bool looping;
        private bool stopping;
        private Transform follow;
        private bool hasFollow;

        /// <summary>Indeks di VFXManager.activeEffects untuk swap-remove. -1 kalau tidak tercatat.</summary>
        internal int managerIndex = -1;

        public string PoolTag => pooled != null ? pooled.PoolTag : string.Empty;

        /// <summary>Bertambah setiap kembali ke pool. Handle lama menjadi tidak valid.</summary>
        public int Generation { get; private set; }

        private void Cache()
        {
            if (cached) return;
            cached = true;
            pooled = GetComponent<PooledObject>();
            systems = GetComponentsInChildren<ParticleSystem>(true);
            baseRateOverTime = new float[systems.Length];
            baseBursts = new ParticleSystem.Burst[systems.Length][];
            for (int i = 0; i < systems.Length; i++)
            {
                var emission = systems[i].emission;
                baseRateOverTime[i] = emission.rateOverTimeMultiplier;
                var bursts = new ParticleSystem.Burst[emission.burstCount];
                emission.GetBursts(bursts);
                baseBursts[i] = bursts;
            }
            renderers = GetComponentsInChildren<Renderer>(true);
            prefabLayers = new int[renderers.Length];
            for (int i = 0; i < renderers.Length; i++) prefabLayers[i] = renderers[i].gameObject.layer;
        }

        public void OnSpawned()
        {
            Cache();
            remaining = 0f;
            looping = false;
            stopping = false;
            follow = null;
            hasFollow = false;

            // Kualitas rendah mengecilkan laju partikel (VFXManager.densityMultiplier).
            float density = VFXManager.Instance != null ? VFXManager.Instance.densityMultiplier : 1f;
            for (int i = 0; i < systems.Length; i++)
            {
                var emission = systems[i].emission;
                emission.rateOverTimeMultiplier = baseRateOverTime[i] * density;
                var bursts = baseBursts[i];
                for (int b = 0; b < bursts.Length; b++)
                {
                    var burst = bursts[b];
                    burst.count = new ParticleSystem.MinMaxCurve(
                        Mathf.Ceil(burst.count.constantMin * density), Mathf.Ceil(burst.count.constantMax * density));
                    emission.SetBurst(b, burst);
                }
            }
            if (systems.Length > 0) systems[0].Play(true);
        }

        /// <summary>Data dari VFXManager.Play, diisi setelah OnSpawned.</summary>
        internal void Setup(float lifetimeSeconds, Transform followTarget, int viewmodelLayer)
        {
            looping = lifetimeSeconds <= 0f;   // 0 berarti loop; pemanggil wajib menghentikannya
            remaining = lifetimeSeconds;
            follow = followTarget;
            hasFollow = followTarget != null;
            if (viewmodelLayer >= 0)
                for (int i = 0; i < renderers.Length; i++) renderers[i].gameObject.layer = viewmodelLayer;
        }

        /// <summary>Berhenti memancar; kembali ke pool setelah partikel habis.</summary>
        internal void StopEmitting()
        {
            if (stopping) return;
            stopping = true;
            if (systems != null && systems.Length > 0) systems[0].Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        private void Update()
        {
            if (stopping)
            {
                if (systems == null || systems.Length == 0 || !systems[0].IsAlive(true)) Release();
                return;
            }
            if (looping) return;
            // Waktu berskala: efek berhenti saat jeda dan lanjut dari posisi yang sama (VFXManager.md, Kasus Khusus).
            remaining -= Time.deltaTime;
            if (remaining <= 0f) Release();
        }

        private void LateUpdate()
        {
            if (!hasFollow) return;
            // Target hilang atau nonaktif (mis. zombie kembali ke pool): lepas, lalu berhenti memancar.
            if (follow == null || !follow.gameObject.activeInHierarchy)
            {
                follow = null;
                hasFollow = false;
                StopEmitting();
                return;
            }
            transform.SetPositionAndRotation(follow.position, follow.rotation);
        }

        private void Release()
        {
            if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(gameObject);
            else gameObject.SetActive(false);
        }

        public void OnReturned()
        {
            if (systems != null)
                for (int i = 0; i < systems.Length; i++) systems[i].Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            follow = null;
            hasFollow = false;
            stopping = false;
            looping = false;
            if (renderers != null)
                for (int i = 0; i < renderers.Length; i++)
                    if (renderers[i] != null) renderers[i].gameObject.layer = prefabLayers[i];
            // Skala prefab dikembalikan ObjectPooler saat masuk pool.
            Generation++;
            if (VFXManager.Instance != null) VFXManager.Instance.NotifyReturned(this);
        }
    }
}
