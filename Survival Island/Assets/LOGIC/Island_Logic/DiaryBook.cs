namespace SurvivalIsland
{
    /// <summary>
    /// Buku Diary di shelter tua yang hancur. Blueprint: Island_Logic/ObjectiveManager.md (#81) dan IslandInteractable.md
    /// tabel "Jenis objek" (#17). GDD bagian 4: satu buku Diary di shelter tua, Bab 1 sampai 3 terbuka bertahap.
    ///
    /// Pengaman (ProgressionSafety.md, #116): buku tidak punya BaseHealth atau IDamageable, jadi bencana tidak merusaknya,
    /// dan prefab memasang NoBuildVolume supaya bangunan tidak menimbunnya. Buku tetap ada setelah dibaca.
    /// Status buku disimpan ObjectiveManager (world_objectives), jadi saveId objek ini dibiarkan kosong.
    /// </summary>
    public class DiaryBook : IslandInteractable
    {
        protected override bool OnInteract(in InteractContext ctx)
        {
            var om = ObjectiveManager.Instance;
            if (om == null) return false;
            om.InteractBook();
            return true;
        }
    }
}
