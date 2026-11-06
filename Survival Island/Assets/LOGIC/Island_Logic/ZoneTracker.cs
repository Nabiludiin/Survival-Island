using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Penghitung zona dan pemilih region aktif untuk semua ZoneManager.
    /// Blueprint: Island_Logic/ZoneManager.md bagian "Isi ZoneTracker", aturan di EnvironmentProfile.md "Zona bertumpuk" (#92).
    /// Usulan disetujui developer: hitungan zona untuk keabsahan, zona terakhir untuk memilih.
    /// Region aktif pindah saat zona lama ditinggalkan, kecuali pemain datang dari celah.
    /// </summary>
    public static class ZoneTracker
    {
        private const int RegionCount = 4;
        private static readonly int[] counts = new int[RegionCount];
        private static RegionType lastEntered = RegionType.Volcano;
        private static int sceneHandle;

        /// <summary>Region aktif. Hanya sah kalau HasActive.</summary>
        public static RegionType Active { get; private set; } = RegionType.Volcano;

        /// <summary>false sampai pemain pertama kali masuk zona dalam scene ini.</summary>
        public static bool HasActive { get; private set; }

        public static int CountOf(RegionType r) => counts[(int)r];

        // Editor dengan Domain Reload dimatikan (GameEventBus.md): hitungan statis dikosongkan setiap Play Mode.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetOnLoad()
        {
            Reset();
            sceneHandle = 0;
        }

        /// <summary>Hitungan milik scene IslandPhase yang sedang berjalan. Scene baru (run baru, load) mengosongkannya.</summary>
        public static void EnsureScene(UnityEngine.SceneManagement.Scene scene)
        {
            int handle = scene.handle;
            if (handle == sceneHandle) return;
            sceneHandle = handle;
            Reset();
        }

        public static void Reset()
        {
            for (int i = 0; i < RegionCount; i++) counts[i] = 0;
            HasActive = false;
            Active = RegionType.Volcano;
            lastEntered = RegionType.Volcano;
        }

        /// <summary>Pemain masuk satu zona. true kalau region aktif berubah (atau event pertama, from == to).</summary>
        public static bool Enter(RegionType r, out RegionType from, out RegionType to)
        {
            from = to = Active;
            // 1.
            counts[(int)r]++;
            lastEntered = r;

            // 2. Event pertama dalam scene. RegionType tidak punya nilai kosong, jadi from == to (usulan 2).
            if (!HasActive)
            {
                HasActive = true;
                Active = r;
                from = to = r;
                return true;
            }

            // 3. Datang dari celah: zona aktif sudah kosong, jadi langsung pindah.
            if (counts[(int)Active] == 0 && r != Active)
            {
                from = Active;
                Active = r;
                to = r;
                return true;
            }

            // 4. Masuk zona tumpukan: region aktif belum pindah.
            return false;
        }

        /// <summary>Pemain keluar dari satu zona. true kalau region aktif berubah.</summary>
        public static bool Exit(RegionType r, out RegionType from, out RegionType to)
        {
            from = to = Active;
            // 1.
            counts[(int)r] = Mathf.Max(0, counts[(int)r] - 1);

            // 2.
            if (!HasActive || r != Active || counts[(int)r] > 0) return false;

            // 3-4. Pengganti: zona terakhir kalau masih berisi pemain, lalu region pertama yang berisi. Tidak ada = celah.
            if (!TryFindReplacement(out RegionType next)) return false;

            // 5.
            from = Active;
            Active = next;
            to = next;
            return true;
        }

        private static bool TryFindReplacement(out RegionType next)
        {
            if (counts[(int)lastEntered] > 0)
            {
                next = lastEntered;
                return true;
            }
            for (int i = 0; i < RegionCount; i++)
            {
                if (counts[i] > 0)
                {
                    next = (RegionType)i;
                    return true;
                }
            }
            next = Active;
            return false;
        }

        // ---------------------------------------------------------------- Dipakai ZoneManager.ResyncAll

        internal static void ClearCounts()
        {
            for (int i = 0; i < RegionCount; i++) counts[i] = 0;
        }

        internal static void AddCount(RegionType r) => counts[(int)r]++;

        /// <summary>Setelah hitungan diisi ulang: pilih region aktif tanpa Enter. true kalau perlu event.</summary>
        internal static bool ResolveAfterResync(RegionType preferred, out RegionType from, out RegionType to)
        {
            from = to = Active;
            if (!HasActive)
            {
                // 3. Belum ada region aktif: pilih zona yang berisi pemain, lalu terbitkan event pertama.
                HasActive = true;
                Active = preferred;
                lastEntered = preferred;
                from = to = preferred;
                return true;
            }
            if (counts[(int)Active] > 0) return false;

            // 4. Zona aktif kosong, pindah ke zona yang berisi pemain.
            from = Active;
            Active = preferred;
            lastEntered = preferred;
            to = preferred;
            return true;
        }
    }
}
