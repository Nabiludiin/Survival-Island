namespace SurvivalIsland
{
    /// <summary>
    /// Aliran acak per sistem. Urutan nilai tidak boleh diubah, karena status aliran berurutan disimpan menurut urutan ini.
    /// Blueprint: Core_Logic/RunSeedManager.md bagian 1 (#89).
    /// </summary>
    public enum RandomStreamId
    {
        ShipLoot, IslandLoot, PartLocation, Weather, Disaster, NPC,
        BeachSupply, Enemy, Gameplay, LuckyMachine,
        SupplyDrop   // Dicadangkan. #54 tidak dipakai, tetapi nilai tetap ada supaya urutan aliran tidak bergeser
    }

    /// <summary>
    /// Pembangkit acak SplitMix64 yang statusnya bisa disimpan (System.Random tidak bisa).
    /// Blueprint: Core_Logic/RunSeedManager.md bagian 1 (#89). Struct, jadi Fork dan salinan tidak mengalokasikan memori.
    /// </summary>
    public struct RandomStream
    {
        private ulong state;

        public RandomStream(ulong seed) { state = seed; }

        public ulong State => state;

        /// <summary>SplitMix64.</summary>
        public ulong NextULong()
        {
            ulong z = state += 0x9E3779B97F4A7C15UL;
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
            return z ^ (z >> 31);
        }

        /// <summary>0 sampai kurang dari 1. Memakai 24 bit atas, sesuai presisi float.</summary>
        public float NextFloat01()
        {
            return (NextULong() >> 40) * (1f / 16777216f);
        }

        /// <summary>Bilangan bulat di [minInclusive, maxExclusive). Mengembalikan minInclusive kalau rentang kosong.</summary>
        public int Range(int minInclusive, int maxExclusive)
        {
            if (maxExclusive <= minInclusive) return minInclusive;
            ulong span = (ulong)((long)maxExclusive - minInclusive);
            return (int)((long)minInclusive + (long)(NextULong() % span));
        }

        /// <summary>Bilangan pecahan di [min, max).</summary>
        public float Range(float min, float max)
        {
            return min + (max - min) * NextFloat01();
        }

        /// <summary>Indeks terpilih menurut bobot. -1 kalau total bobot 0 (bobot negatif dianggap 0).</summary>
        public int PickWeighted(float[] weights)
        {
            if (weights == null || weights.Length == 0) return -1;

            float total = 0f;
            for (int i = 0; i < weights.Length; i++)
                if (weights[i] > 0f) total += weights[i];
            if (total <= 0f) return -1;

            float roll = NextFloat01() * total;
            int last = -1;
            for (int i = 0; i < weights.Length; i++)
            {
                if (weights[i] <= 0f) continue;
                last = i;
                if (roll < weights[i]) return i;
                roll -= weights[i];
            }
            return last;   // Pengaman pembulatan float
        }

        /// <summary>Fisher-Yates.</summary>
        public void Shuffle(int[] indices)
        {
            if (indices == null) return;
            for (int i = indices.Length - 1; i > 0; i--)
            {
                int j = Range(0, i + 1);
                int tmp = indices[i];
                indices[i] = indices[j];
                indices[j] = tmp;
            }
        }
    }
}
