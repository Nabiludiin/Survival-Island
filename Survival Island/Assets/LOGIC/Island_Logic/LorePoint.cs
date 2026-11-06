using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Titik temuan lore tetap di scene region (catatan, rekaman, papan). Blueprint: Island_Logic/LoreCollectibles.md
    /// bagian "Menemukan temuan" (#100), mewarisi IslandInteractable (#17).
    /// HoldSeconds 0 dan blockedWhileCarrying = false, karena membaca tidak butuh tangan kosong.
    /// Objek tetap ada setelah dibaca dan bisa dibaca lagi (prompt.read_again). Tampilan dibaca dari
    /// LoreCollectibles.collectedThisRun, jadi titik ini tidak menyimpan apa pun (saveId dibiarkan kosong).
    /// Titik bersifat tetap dan tidak diacak RunSeedManager (#89).
    /// </summary>
    public class LorePoint : IslandInteractable
    {
        [SerializeField] private LoreEntry entry;

        public LoreEntry Entry => entry;

        protected override void Awake()
        {
            blockedWhileCarrying = false;
            holdSeconds = 0f;
            base.Awake();
        }

        public override bool IsAvailable => base.IsAvailable && entry != null && LoreCollectibles.Instance != null;

        public override string GetPrompt()
        {
            // promptKey tidak dipakai: prompt berganti setelah dibaca.
            var lore = LoreCollectibles.Instance;
            if (entry != null && lore != null && lore.IsCollected(entry.loreId)) return Localize("prompt.read_again");
            return Localize(entry != null && entry.kind == LoreKind.Recording ? "prompt.listen" : "prompt.read");
        }

        protected override bool OnInteract(in InteractContext ctx)
        {
            var lore = LoreCollectibles.Instance;
            if (lore == null || entry == null) return false;
            lore.Collect(entry);
            return true;
        }

        // Region dilepas: tidak ada status sendiri (LoreCollectibles.md Kasus Khusus).
        public override string CaptureState() => "{}";

        public override void RestoreState(string json) { }
    }
}
