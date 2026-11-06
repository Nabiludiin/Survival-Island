using System;

namespace SurvivalIsland
{
    // =====================================================================================
    // Tipe data file save. Blueprint: Core_Logic/WorldStateSave.md bagian 1 "Bentuk file" (#80).
    // Dipakai hanya oleh WorldStateSave dan pemanggilnya (RunLifecycle).
    // =====================================================================================

    /// <summary>Alasan save. Tabel "Kapan tiap alasan dipakai" di WorldStateSave.md.</summary>
    public enum SaveReason { ArrivedOnIsland, WakeUp, Daybreak, QuitToMenu, AppQuit, Manual, BridgeComplete, DisasterEnded, PartInstalled }

    /// <summary>Hasil pemeriksaan file save terakhir. Dibaca MainMenuController.</summary>
    public enum SaveProblem { None, Corrupt, NewerVersion, OldVersionNoMigration }

    /// <summary>Versi format save. Naik setiap format berubah. gameVersion hanya catatan.</summary>
    public static class SaveVersions
    {
        public const int Current = 1;
    }

    /// <summary>Amplop file save: versi, CRC, dan payload dalam bentuk JSON.</summary>
    [Serializable]
    public class RunSaveEnvelope
    {
        public int saveVersion;         // Nilai awal 1
        public string gameVersion;      // Application.version. Hanya catatan
        public uint payloadCrc32;
        public string payloadJson;      // RunSavePayload dalam bentuk JSON
    }

    /// <summary>Isi save run.</summary>
    [Serializable]
    public class RunSavePayload
    {
        public long savedAtUtcTicks;
        public string reason;
        public bool wasInCombat;
        public int engagedEnemyCount;
        public RunSeedSaveData seed;       // Dari RunSeedManager (#89)
        public GameRunSaveData game;       // currentDay, hasEscaped, shipOutcome, dan fase
        public SaveEntry[] entries;        // Semua ISaveable di scene dasar
        public string regionStoreJson;     // Dari RegionStateStore.ToSaveJson() (#88)
    }

    /// <summary>Data satu ISaveable.</summary>
    [Serializable]
    public struct SaveEntry
    {
        public string id;
        public string json;
    }

    /// <summary>
    /// Isi run_end.marker: result, reason, currentDay, dan seed (RunLifecycle.md "Game ditutup paksa saat akhir run").
    /// </summary>
    [Serializable]
    public struct RunEndMarker
    {
        public RunResult result;
        public DeathReason reason;
        public int currentDay;
        public uint seed;
    }

    /// <summary>
    /// Rantai migrasi format save (Migrate_1_to_2, dan seterusnya). Versi 1 belum punya migrasi.
    /// Mengembalikan false kalau tidak ada migrasi dari versi itu.
    /// </summary>
    public static class SaveMigrator
    {
        public static bool TryMigrate(int fromVersion, ref string payloadJson)
        {
            // Belum ada migrasi. Tambahkan langkah per versi di sini saat SaveVersions.Current naik.
            return fromVersion == SaveVersions.Current;
        }
    }

    /// <summary>CRC-32 (polinomial IEEE) untuk memeriksa payload.</summary>
    public static class SaveCrc32
    {
        private static readonly uint[] Table = BuildTable();

        private static uint[] BuildTable()
        {
            var table = new uint[256];
            for (uint i = 0; i < 256; i++)
            {
                uint c = i;
                for (int k = 0; k < 8; k++) c = (c & 1) != 0 ? 0xEDB88320u ^ (c >> 1) : c >> 1;
                table[i] = c;
            }
            return table;
        }

        public static uint Compute(string text)
        {
            if (text == null) return 0;
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            uint crc = 0xFFFFFFFFu;
            for (int i = 0; i < bytes.Length; i++) crc = Table[(crc ^ bytes[i]) & 0xFF] ^ (crc >> 8);
            return crc ^ 0xFFFFFFFFu;
        }
    }
}
