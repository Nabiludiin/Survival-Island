using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu pemancar cahaya. Dipakai struktur (Obor Dinding, Lampu), api unggun, obor, senter, dan api (#97).
    /// Blueprint: Island_Logic/LightSourceSystem.md bagian "Variabel LightSource" (#98). Usulan, belum ada di GDD.
    /// Mendaftar ke LightSourceSystem di OnEnable dan keluar di OnDisable. LightSourceSystem memilih sumber mana yang
    /// memakai Light real-time (batas lampu); sisanya tampil lewat emissive.
    /// </summary>
    public class LightSource : MonoBehaviour, IPoolable
    {
        [Range(0f, 1f)] public float intensity01 = 1f;
        public float range = 8f;
        [Tooltip("Ikut pemain (senter, obor pegang). Selalu menang batas lampu dan tetap dihitung di InteriorZone.")]
        public bool isHeld;
        [Tooltip("Senter. Tidak dihitung di GetLightLevelAt dan tidak memakai bayangan.")]
        public bool isFlashlight;
        [Tooltip("Dihitung untuk tarikan cahaya base (Obor Dinding dan Lampu). Api unggun dan obor tidak.")]
        public bool countsForAttraction;
        [Tooltip("Lampu sungguhan. Boleh kosong.")]
        public Light realLight;
        [Tooltip("Tampilan tanpa lampu (bahan emisif). Boleh kosong.")]
        public Renderer emissive;
        [SerializeField] private bool litOnStart;

        [System.NonSerialized] public float lastScore;

        private bool isLit;
        private float baseLightIntensity;
        private float prefabIntensity01;
        private bool prefabLit;
        private bool budgetOn = true;
        private float fade = 1f;
        private float flickerMultiplier = 1f;
        private bool registered;
        private LightShadows prefabShadows;

        public bool IsLit
        {
            get => isLit;
            set
            {
                if (isLit == value) return;
                isLit = value;
                ApplyVisual();
            }
        }

        /// <summary>Lampu real-time sedang dipakai (hasil batas lampu).</summary>
        public bool HasRealtimeBudget => budgetOn;

        public void SetLit(bool on) => IsLit = on;

        /// <summary>Pengali kedip dari pemilik (LightFixture). 1 berarti normal.</summary>
        public void SetFlicker(float multiplier) => flickerMultiplier = Mathf.Clamp01(multiplier);

        private void Awake()
        {
            prefabIntensity01 = intensity01;
            prefabLit = litOnStart;
            isLit = litOnStart;
            if (realLight != null)
            {
                baseLightIntensity = realLight.intensity;
                prefabShadows = realLight.shadows;
            }
            ApplyVisual();
        }

        private void OnEnable()
        {
            registered = LightSourceSystem.Instance != null && LightSourceSystem.Instance.Register(this);
            ApplyVisual();
        }

        private void OnDisable()
        {
            if (registered && LightSourceSystem.Instance != null) LightSourceSystem.Instance.Unregister(this);
            registered = false;
        }

        /// <summary>Dipanggil LightSourceSystem kalau manager muncul setelah sumber ini aktif.</summary>
        internal void MarkRegistered() => registered = true;

        // Kasus khusus: objek pool mendaftar di OnSpawned, keluar di OnReturned, dan kembali ke nilai prefab.
        public void OnSpawned()
        {
            intensity01 = prefabIntensity01;
            isLit = prefabLit;
            flickerMultiplier = 1f;
            budgetOn = true;
            fade = 1f;
            if (realLight != null)
            {
                realLight.intensity = baseLightIntensity;
                realLight.shadows = prefabShadows;
            }
            if (!registered && LightSourceSystem.Instance != null) registered = LightSourceSystem.Instance.Register(this);
            ApplyVisual();
        }

        public void OnReturned()
        {
            isLit = false;
            if (registered && LightSourceSystem.Instance != null) LightSourceSystem.Instance.Unregister(this);
            registered = false;
            ApplyVisual();
        }

        // ---------------------------------------------------------------- Batas lampu (dipanggil LightSourceSystem)

        internal void SetRealtimeBudget(bool on, bool shadows)
        {
            budgetOn = on;
            if (realLight != null) realLight.shadows = shadows ? prefabShadows : LightShadows.None;
            if (on && realLight != null && isLit && !realLight.enabled)
            {
                fade = isHeld ? 1f : 0f;
                realLight.enabled = true;
            }
        }

        private void Update()
        {
            if (realLight == null || isHeld) return;   // Senter dan obor mengatur intensitasnya sendiri
            var b = GameManager.CurrentBalance;
            float fadeSeconds = b != null ? Mathf.Max(0.01f, b.light.lightFadeSeconds) : 0.25f;
            float target = isLit && budgetOn ? 1f : 0f;
            // Peralihan memudar selama 0,25 detik (LightSourceSystem.md, batas lampu langkah 3).
            fade = Mathf.MoveTowards(fade, target, Time.unscaledDeltaTime / fadeSeconds);
            realLight.intensity = baseLightIntensity * fade * flickerMultiplier;
            bool shouldEnable = fade > 0.001f;
            if (realLight.enabled != shouldEnable) realLight.enabled = shouldEnable;
        }

        private void ApplyVisual()
        {
            if (emissive != null) emissive.enabled = isLit;
            if (realLight != null && !isLit && isHeld) realLight.enabled = false;
        }
    }
}
