using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Suasana satu region: fog, ambient, matahari, skybox, partikel, dan post-processing.
    /// Blueprint: Island_Logic/EnvironmentProfile.md bagian "RegionEnvironmentProfile" (#92).
    /// GDD bagian 5: Region 2 Amazon gelap meski siang (fogDensity tinggi). Nilai awal di tabel "Nilai awal region" (usulan).
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Region Environment Profile")]
    public class RegionEnvironmentProfile : ScriptableObject
    {
        public RegionType region = RegionType.Volcano;

        [Header("Fog (ExponentialSquared, jarak pandang sekitar 2 / fogDensity)")]
        public Color fogColor = new Color32(0xB8, 0xC4, 0xC8, 0xFF);
        public float fogDensity = 0.004f;
        public float nightFogMultiplier = 1f;

        [Header("Ambient dan matahari")]
        public Color ambientColor = new Color32(0x8C, 0x9A, 0xA6, 0xFF);
        [Tooltip("Pengali ambient per jam (0 = tengah malam, 1 = tengah malam berikutnya).")]
        public Gradient ambientByHour = new Gradient();
        public Gradient sunColorByHour = new Gradient();
        public float sunIntensity = 1.1f;
        public AnimationCurve sunIntensityByHour = AnimationCurve.Constant(0f, 1f, 1f);
        [Tooltip("Batas bawah luminansi ambient. 0 = pakai environment.defaultMinAmbientLuminance.")]
        public float minAmbientLuminance;

        [Header("Langit, partikel, dan efek layar")]
        public Texture skybox;
        public string ambientParticleTag = "Fx_AshLight";
        [Tooltip("VolumeProfile URP untuk PostProcessController (#23). Disimpan sebagai Object supaya assembly tidak butuh URP.")]
        public Object postProfile;

        /// <summary>Nilai awal tabel EnvironmentProfile.md untuk region ini.</summary>
        [ContextMenu("Isi nilai awal region")]
        private void FillDefaults()
        {
            switch (region)
            {
                case RegionType.Amazon:
                    fogDensity = 0.03f; fogColor = new Color32(0x2E, 0x40, 0x34, 0xFF); ambientColor = new Color32(0x33, 0x44, 0x3A, 0xFF);
                    sunIntensity = 0.35f; nightFogMultiplier = 0.6f; ambientParticleTag = "Fx_Spores";
                    break;
                case RegionType.Snow:
                    fogDensity = 0.012f; fogColor = new Color32(0xD3, 0xE0, 0xEA, 0xFF); ambientColor = new Color32(0xA9, 0xC0, 0xD6, 0xFF);
                    sunIntensity = 0.9f; nightFogMultiplier = 0.8f; ambientParticleTag = "Fx_SnowDust";
                    break;
                case RegionType.Logistic:
                    fogDensity = 0.02f; fogColor = new Color32(0x4A, 0x4F, 0x4A, 0xFF); ambientColor = new Color32(0x4C, 0x54, 0x50, 0xFF);
                    sunIntensity = 0.5f; nightFogMultiplier = 0.8f; ambientParticleTag = "Fx_Mist";
                    break;
                default:
                    fogDensity = 0.004f; fogColor = new Color32(0xB8, 0xC4, 0xC8, 0xFF); ambientColor = new Color32(0x8C, 0x9A, 0xA6, 0xFF);
                    sunIntensity = 1.1f; nightFogMultiplier = 1f; ambientParticleTag = "Fx_AshLight";
                    break;
            }
        }
    }
}
