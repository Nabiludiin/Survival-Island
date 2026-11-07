using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Objek pool FX_FireBurn: partikel, cahaya (LightSource), dan collider padam untuk satu Flammable.
    /// Blueprint: Island_Logic/FireSystem.md (#97). Efek tidak dijadikan child pemiliknya (Unity melarang ganti parent saat
    /// objek sedang dinonaktifkan), jadi FireSystem menyalin posisi Flammable musuh setiap frame.
    /// </summary>
    public class BurnEffect : MonoBehaviour, IPoolable
    {
        [SerializeField] private LightSource lightSource;
        [Tooltip("Collider kecil layer Interactable untuk FireExtinguishTarget. Dimatikan kalau pemiliknya musuh.")]
        [SerializeField] private Collider extinguishCollider;
        [SerializeField] private ParticleSystem[] particles;

        private Flammable owner;
        private bool released;

        public Flammable Owner => owner;

        internal void Bind(Flammable flammable)
        {
            owner = flammable;
            released = false;
            if (extinguishCollider != null) extinguishCollider.enabled = flammable != null && !flammable.IsEnemy;
            if (lightSource != null) lightSource.IsLit = true;
            if (particles != null)
                for (int i = 0; i < particles.Length; i++) if (particles[i] != null) particles[i].Play(true);
        }

        /// <summary>Kembali ke pool. Aman dipanggil berkali-kali.</summary>
        public void Release()
        {
            if (released) return;
            released = true;
            owner = null;
            if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(gameObject);
            else gameObject.SetActive(false);
        }

        /// <summary>FireExtinguishTarget: padamkan pemilik.</summary>
        internal void ExtinguishOwner()
        {
            if (owner != null) owner.Extinguish();
            else Release();
        }

        public void OnSpawned()
        {
            owner = null;
            released = false;
            if (extinguishCollider != null) extinguishCollider.enabled = true;
        }

        public void OnReturned()
        {
            released = true;
            if (owner != null)
            {
                // Dikembalikan dari luar (misalnya ReturnAllActive saat reset run): pemilik ikut padam tanpa event.
                var o = owner;
                owner = null;
                o.Extinguish(true);
            }
            if (lightSource != null) lightSource.IsLit = false;
            if (particles != null)
                for (int i = 0; i < particles.Length; i++) if (particles[i] != null) particles[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
