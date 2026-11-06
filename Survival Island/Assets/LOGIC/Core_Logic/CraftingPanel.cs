using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Panel resep dan antrean (UIPanelId.Crafting). Tidak menjeda game (#83). Dibuka dengan C (CraftingManager membaca
    /// OpenCrafting) atau E di stasiun. Blueprint: Core_Logic/CraftingStation.md "Panel resep" (#77).
    /// Pengaturan Inspector: pausesTime = false, releasesCursor = true, blocksGameplayInput = true, closableByEsc = true,
    /// toggleAction = UI/ToggleCrafting.
    ///
    /// Catatan implementasi (Langkah 4): tampilan dibuat sederhana. Tiap resep satu baris dari rowPrefab (Button + TMP_Text),
    /// dan tiap job satu baris dari queueRowPrefab (Button batal + TMP_Text). Nama memakai displayNameKey apa adanya
    /// sampai LocalizationManager (#104) ada. Tab Perbaiki (#113) belum ada.
    /// </summary>
    public class CraftingPanel : FadingUIPanel
    {
        public static CraftingPanel Instance { get; private set; }

        [SerializeField] private Transform recipeListRoot;
        [SerializeField] private Button rowPrefab;
        [SerializeField] private Transform queueListRoot;
        [SerializeField] private Button queueRowPrefab;
        [SerializeField] private Color missingColor = new Color(0.9f, 0.25f, 0.25f);
        [SerializeField] private Color dimColor = new Color(1f, 1f, 1f, 0.45f);

        private readonly List<Button> rows = new List<Button>();
        private readonly List<CraftingRecipe> rowRecipes = new List<CraftingRecipe>();
        private readonly List<Button> queueRows = new List<Button>();
        private readonly StringBuilder sb = new StringBuilder(128);
        private bool listening;

        private void Awake()
        {
            Instance = this;
        }

        protected override void OnDestroy()
        {
            StopListening();
            if (Instance == this) Instance = null;
            base.OnDestroy();
        }

        protected override void Show()
        {
            if (CraftingManager.Instance != null)
            {
                CraftingManager.Instance.IsPanelOpen = true;
                CraftingManager.Instance.RefreshAvailability();
            }
            StartListening();
            Redraw();
            base.Show();
        }

        protected override void Hide()
        {
            // Saat ditutup antrean tetap berjalan, dan HUD memakai CraftQueueChangedEvent.
            if (CraftingManager.Instance != null) CraftingManager.Instance.IsPanelOpen = false;
            StopListening();
            base.Hide();
        }

        // Panel mendengar event hanya saat terbuka.
        private void StartListening()
        {
            if (listening) return;
            listening = true;
            EventChannel<InventoryChangedEvent>.Raised += HandleInventoryChanged;
            EventChannel<CraftQueueChangedEvent>.Raised += HandleQueueChanged;
            EventChannel<TechNodeUnlockedEvent>.Raised += HandleTechNode;
        }

        private void StopListening()
        {
            if (!listening) return;
            listening = false;
            EventChannel<InventoryChangedEvent>.Raised -= HandleInventoryChanged;
            EventChannel<CraftQueueChangedEvent>.Raised -= HandleQueueChanged;
            EventChannel<TechNodeUnlockedEvent>.Raised -= HandleTechNode;
        }

        private void HandleInventoryChanged(InventoryChangedEvent e) => Redraw();
        private void HandleQueueChanged(CraftQueueChangedEvent e) => Redraw();
        private void HandleTechNode(TechNodeUnlockedEvent e) => Redraw();

        // ---------------------------------------------------------------- Gambar ulang

        private void Redraw()
        {
            DrawRecipes();
            DrawQueue();
        }

        private void DrawRecipes()
        {
            var mgr = CraftingManager.Instance;
            var inv = InventoryManager.Instance;
            var all = RecipeDatabase.All;
            int used = 0;
            for (int i = 0; i < all.Count; i++)
            {
                var r = all[i];
                // Resep api unggun milik CampfirePanel (#44).
                if (r == null || r.station == CraftingStationType.Campfire) continue;

                bool unlocked = mgr != null && mgr.IsRecipeUnlocked(r);
                // Terkunci dan prasyarat belum terbuka: disembunyikan (usulan disetujui).
                bool canSee = unlocked || (TechTreeManager.Instance != null && TechTreeManager.Instance.CanUnlock(r.techNodeId));
                if (!canSee) continue;

                var row = GetRow(used);
                rowRecipes[used] = r;
                used++;

                bool stationOk = mgr != null && mgr.IsStationAvailable(r.station);
                bool materialOk = true;
                sb.Clear();
                sb.Append(unlocked ? r.displayNameKey : "???");
                if (unlocked && r.ingredients != null)
                {
                    sb.Append("  ");
                    for (int k = 0; k < r.ingredients.Length; k++)
                    {
                        var ing = r.ingredients[k];
                        if (ing.item == null) continue;
                        int have = inv != null ? inv.GetCount(ing.item.itemID) : 0;
                        bool enough = have >= ing.amount;
                        materialOk &= enough;
                        // Bahan yang kurang berwarna merah.
                        if (!enough) sb.Append("<color=#").Append(ColorUtility.ToHtmlStringRGB(missingColor)).Append('>');
                        sb.Append(ing.amount).Append(' ').Append(ing.item.itemID).Append(" (").Append(have).Append(')');
                        if (!enough) sb.Append("</color>");
                        if (k < r.ingredients.Length - 1) sb.Append(", ");
                    }
                }
                if (unlocked && !stationOk) sb.Append("  ").Append(StationText(r.station));

                var label = row.GetComponentInChildren<TMP_Text>(true);
                if (label != null)
                {
                    label.text = sb.ToString();
                    label.color = unlocked && stationOk ? Color.white : dimColor;
                }
                row.interactable = unlocked && stationOk && materialOk;
                row.gameObject.SetActive(true);
            }
            for (int i = used; i < rows.Count; i++) rows[i].gameObject.SetActive(false);
        }

        private static string StationText(CraftingStationType t)
        {
            // Kunci teks (#104).
            switch (t)
            {
                case CraftingStationType.WoodBench: return UIManager.Localize("ui.crafting.need_wood_bench");       // "Butuh Meja Kayu"
                case CraftingStationType.MedicalBench: return UIManager.Localize("ui.crafting.need_medical_bench"); // "Butuh Meja Medis"
                default: return string.Empty;
            }
        }

        private Button GetRow(int index)
        {
            while (rows.Count <= index)
            {
                var b = Instantiate(rowPrefab, recipeListRoot);
                int captured = rows.Count;
                b.onClick.AddListener(() => HandleCraftClicked(captured));
                rows.Add(b);
                rowRecipes.Add(null);
            }
            return rows[index];
        }

        private void HandleCraftClicked(int rowIndex)
        {
            if (CraftingManager.Instance == null || rowIndex >= rowRecipes.Count) return;
            var result = CraftingManager.Instance.RequestCraft(rowRecipes[rowIndex]);
            if (result != CraftResult.Ok)
            {
                // Notifikasi alasan penolakan (#22).
                if (NotificationManager.Instance != null) NotificationManager.Instance.Show(RejectKey(result), NotificationKind.Info);
            }
        }

        private static string RejectKey(CraftResult r)
        {
            switch (r)
            {
                case CraftResult.Locked: return "notif.craft.locked";                     // "Resep belum terbuka"
                case CraftResult.NoStation: return "notif.craft.no_station";              // "Butuh meja crafting"
                case CraftResult.MissingMaterial: return "notif.craft.missing_material";  // "Bahan kurang"
                case CraftResult.NoSpace: return "notif.craft.no_space";                  // "Inventori penuh"
                case CraftResult.QueueFull: return "notif.craft.queue_full";              // "Antrean penuh"
                default: return string.Empty;
            }
        }

        private void DrawQueue()
        {
            var mgr = CraftingManager.Instance;
            int count = mgr != null ? mgr.Queue.Count : 0;
            for (int i = 0; i < count; i++)
            {
                while (queueRows.Count <= i)
                {
                    var b = Instantiate(queueRowPrefab, queueListRoot);
                    int captured = queueRows.Count;
                    b.onClick.AddListener(() => { if (CraftingManager.Instance != null) CraftingManager.Instance.CancelJob(captured); });
                    queueRows.Add(b);
                }
                var job = mgr.Queue[i];
                var r = RecipeDatabase.Get(job.recipeId);
                var label = queueRows[i].GetComponentInChildren<TMP_Text>(true);
                if (label != null)
                    label.text = $"{(r != null ? r.displayNameKey : job.recipeId)}  {Mathf.CeilToInt(Mathf.Max(0f, job.remainingSeconds))} dtk  (batal)";
                queueRows[i].gameObject.SetActive(true);
            }
            for (int i = count; i < queueRows.Count; i++) queueRows[i].gameObject.SetActive(false);
        }
    }
}
