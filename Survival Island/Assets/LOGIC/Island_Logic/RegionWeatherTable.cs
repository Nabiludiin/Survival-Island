using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Satu bobot peluang cuaca.</summary>
    [Serializable]
    public struct WeatherWeight
    {
        public WeatherType weather;
        public float weight;
        public WeatherWeight(WeatherType weather, float weight) { this.weather = weather; this.weight = weight; }
    }

    /// <summary>
    /// Tabel peluang cuaca satu region. Blueprint: Island_Logic/RegionalWeather.md bagian "Tabel cuaca dan peluang" (#115).
    /// Nilai awal adalah usulan yang disetujui developer. Isi lewat menu konteks "Isi nilai awal region".
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Regional Weather Table")]
    public class RegionWeatherTable : ScriptableObject
    {
        public RegionType region = RegionType.Volcano;
        public WeatherWeight[] weights = Array.Empty<WeatherWeight>();

        private const int WeatherCount = 6;

        /// <summary>Bobot sebagai array per WeatherType, untuk RandomStream.PickWeighted. Diisi ke buffer pemanggil.</summary>
        public void FillWeights(float[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++) buffer[i] = 0f;
            if (weights == null) return;
            for (int i = 0; i < weights.Length; i++)
            {
                int idx = (int)weights[i].weather;
                if (idx >= 0 && idx < buffer.Length) buffer[idx] += Mathf.Max(0f, weights[i].weight);
            }
        }

        /// <summary>Nilai awal tabel RegionalWeather.md untuk region ini.</summary>
        public static float[] DefaultWeights(RegionType r)
        {
            // Urutan Clear, Rain, Fog, Storm, Snow, Blizzard.
            switch (r)
            {
                case RegionType.Amazon:   return new float[] { 25, 50, 15, 10, 0, 0 };   // Hutan tropis lebat
                case RegionType.Snow:     return new float[] { 30, 0, 5, 0, 45, 20 };    // Tidak ada hujan tropis di puncak es
                case RegionType.Logistic: return new float[] { 35, 25, 35, 5, 0, 0 };    // Lembah gelap yang lembab
                default:                  return new float[] { 50, 25, 15, 10, 0, 0 };   // Volcano
            }
        }

        [ContextMenu("Isi nilai awal region")]
        private void FillDefaults()
        {
            var w = DefaultWeights(region);
            weights = new WeatherWeight[WeatherCount];
            for (int i = 0; i < WeatherCount; i++) weights[i] = new WeatherWeight((WeatherType)i, w[i]);
        }
    }
}
