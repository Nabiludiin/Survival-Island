using System;
using System.Collections.Generic;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu jenis barang yang dibawa dari Fase 1 ke Fase 2 (GameManager.savedShipLoot).
    /// Bentuk dari 60-seconds/LifeboatExtraction.md bagian 1 (#74). Ditulis di Langkah 1 karena GameManager memakainya.
    /// </summary>
    [Serializable]
    public struct SavedItemData
    {
        public string itemId;
        public int amount;
    }

    /// <summary>
    /// Hasil Fase 1: NPC selamat, korban, HP, infeksi, dan sisa waktu.
    /// Bentuk dari 60-seconds/LifeboatExtraction.md bagian 1 (#74). Diisi LifeboatExtraction (Langkah 2).
    /// </summary>
    [Serializable]
    public class ShipOutcomeData
    {
        public List<string> rescuedNpcIds = new List<string>();
        public List<string> lostNpcIds = new List<string>();
        public float hpOnExit;
        public float infectionOnExit;
        public float secondsLeftOnExit;
        public bool departedEarly;

        /// <summary>Kosongkan isi tanpa membuat objek baru (GameManager.ResetForNewRun).</summary>
        public void Clear()
        {
            rescuedNpcIds.Clear();
            lostNpcIds.Clear();
            hpOnExit = 0f;
            infectionOnExit = 0f;
            secondsLeftOnExit = 0f;
            departedEarly = false;
        }
    }
}
