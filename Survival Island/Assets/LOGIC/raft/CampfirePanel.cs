using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Panel api unggun (UIPanelId.Campfire). Tidak menjeda game (#83). Satu instance di IslandPhase, diikat ke satu
    /// CampfireManager lewat Bind. Panel hanya memanggil fungsi CampfireManager (aturan UI di GameEventBus.md).
    /// Blueprint: raft/CampfireManager.md bagian "Panel CampfirePanel" (#44).
    /// Pengaturan Inspector: pausesTime = false, releasesCursor = true, blocksGameplayInput = true, closableByEsc = true.
    ///
    /// Catatan implementasi (sistem pendukung): seret-lepas dari grid ke kotak input belum ada. Klik kiri atau klik kanan
    /// item inventori memasukkannya (bahan bakar ke kotak bahan bakar, bahan masak ke baris kosong satu per satu).
    /// </summary>
    public class CampfirePanel : FadingUIPanel
    {
        [Serializable]
        private class RowView
        {
            public Button inputButton;
            public Image inputIcon;
            public Button outputButton;
            public Image outputIcon;
            public Image progressFill;
        }

        public static CampfirePanel Instance { get; private set; }

        [SerializeField] private Image fuelFill;
        [SerializeField] private TMP_Text fuelText;
        [SerializeField] private Button addFuelButton;
        [SerializeField] private Button collectAllButton;
        [SerializeField] private RowView[] rowViews = new RowView[0];
        [SerializeField] private SlotGridView inventoryGrid;
        [SerializeField] private TMP_Text rejectText;
        [SerializeField] private float rejectShowSeconds = 1.5f;
        [SerializeField] private Color cookingColor = new Color(0.95f, 0.8f, 0.2f);
        [SerializeField] private Color doneColor = new Color(0.3f, 0.85f, 0.3f);
        [SerializeField] private Color nearBurntColor = new Color(0.9f, 0.25f, 0.2f);
        [SerializeField] private Color burntColor = new Color(0.1f, 0.1f, 0.1f);
        [SerializeField, Range(0f, 1f)] private float nearBurntRatio = 0.7f;

        private CampfireManager bound;
        private bool listening;
        private bool leftDirty, rightDirty;
        private float rejectHideAt;

        public bool IsBoundTo(CampfireManager c) => c != null && bound == c && IsShown;

        private void Awake()
        {
            Instance = this;
            if (addFuelButton != null) addFuelButton.onClick.AddListener(OnAddFuelClicked);
            if (collectAllButton != null) collectAllButton.onClick.AddListener(OnCollectAllClicked);
            for (int i = 0; i < rowViews.Length; i++)
            {
                int idx = i;
                var rv = rowViews[i];
                if (rv == null) continue;
                if (rv.inputButton != null) rv.inputButton.onClick.AddListener(() => OnInputClicked(idx));
                if (rv.outputButton != null) rv.outputButton.onClick.AddListener(() => OnOutputClicked(idx));
            }
        }

        protected override void OnDestroy()
        {
            StopListening();
            if (Instance == this) Instance = null;
            base.OnDestroy();
        }

        /// <summary>Mengikat panel ke satu api unggun. null melepas ikatan.</summary>
        public void Bind(CampfireManager fire)
        {
            if (bound == fire) return;
            if (bound != null && listening) bound.ContentsChanged -= HandleContentsChanged;
            bound = fire;
            if (bound != null && listening) bound.ContentsChanged += HandleContentsChanged;
            leftDirty = rightDirty = true;
        }

        protected override void Show()
        {
            StartListening();
            leftDirty = rightDirty = true;
            if (rejectText != null) rejectText.gameObject.SetActive(false);
            base.Show();
        }

        protected override void Hide()
        {
            StopListening();
            base.Hide();
        }

        public override void OnClosed(UICloseReason reason)
        {
            // Api tetap berjalan setelah panel tertutup. Panel hanya tampilan.
            Bind(null);
            base.OnClosed(reason);
        }

        private void StartListening()
        {
            if (listening) return;
            listening = true;
            EventChannel<InventoryChangedEvent>.Raised += HandleInventoryChanged;
            if (bound != null) bound.ContentsChanged += HandleContentsChanged;
            if (inventoryGrid != null) inventoryGrid.Clicked += OnInventoryClicked;
        }

        private void StopListening()
        {
            if (!listening) return;
            listening = false;
            EventChannel<InventoryChangedEvent>.Raised -= HandleInventoryChanged;
            if (bound != null) bound.ContentsChanged -= HandleContentsChanged;
            if (inventoryGrid != null) inventoryGrid.Clicked -= OnInventoryClicked;
        }

        private void HandleInventoryChanged(InventoryChangedEvent e) => leftDirty = true;
        private void HandleContentsChanged() => rightDirty = true;

        // ---------------------------------------------------------------- Gambar

        private void LateUpdate()
        {
            if (!listening) return;
            if (rejectText != null && rejectText.gameObject.activeSelf && Time.unscaledTime >= rejectHideAt)
                rejectText.gameObject.SetActive(false);
            if (leftDirty)
            {
                leftDirty = false;
                if (inventoryGrid != null && InventoryManager.Instance != null) inventoryGrid.Draw(InventoryManager.Instance.Slots);
            }
            // Progres berubah tiap tick, jadi sisi api digambar tiap frame selama panel terbuka (murah: 3 baris).
            rightDirty = false;
            DrawFire();
        }

        private void DrawFire()
        {
            if (bound == null) return;
            var b = GameManager.CurrentBalance;
            float maxFuel = b != null ? b.campfire.maxFuelSeconds : 300f;
            if (fuelFill != null) fuelFill.fillAmount = maxFuel > 0f ? Mathf.Clamp01(bound.FuelSeconds / maxFuel) : 0f;
            if (fuelText != null) fuelText.text = Mathf.CeilToInt(bound.FuelSeconds) + " s";

            var rows = bound.Rows;
            for (int i = 0; i < rowViews.Length; i++)
            {
                var rv = rowViews[i];
                if (rv == null) continue;
                var row = rows != null && i < rows.Count ? rows[i] : null;
                SetIcon(rv.inputIcon, row != null ? row.input : null);
                SetIcon(rv.outputIcon, row != null ? row.output : null);
                if (rv.progressFill == null) continue;
                if (row == null || row.state == CookRowState.Empty) { rv.progressFill.fillAmount = 0f; continue; }
                var recipe = RecipeDatabase.Get(row.recipeId) as CookingRecipe;
                switch (row.state)
                {
                    case CookRowState.Cooking:
                        float need = recipe != null ? recipe.EffectiveCookSeconds : 1f;
                        rv.progressFill.fillAmount = Mathf.Clamp01(row.progressSeconds / need);
                        rv.progressFill.color = cookingColor;
                        break;
                    case CookRowState.Done:
                        rv.progressFill.fillAmount = 1f;
                        bool near = recipe != null && recipe.canBurn
                                    && row.overcookSeconds >= recipe.EffectiveBurnAfterSeconds * nearBurntRatio;
                        rv.progressFill.color = near ? nearBurntColor : doneColor;
                        break;
                    default:
                        rv.progressFill.fillAmount = 1f;
                        rv.progressFill.color = burntColor;
                        break;
                }
            }
        }

        private static void SetIcon(Image img, ItemInstance inst)
        {
            if (img == null) return;
            var data = inst != null ? ItemDatabase.Get(inst.itemId) : null;
            img.sprite = data != null ? data.icon : null;
            img.enabled = data != null;
        }

        // ---------------------------------------------------------------- Aksi

        private bool IsTop => UIFocusStack.Instance != null && UIFocusStack.Instance.TopPanel == this;

        private void OnInventoryClicked(int index, PointerEventData.InputButton button)
        {
            if (bound == null || !IsTop) return;
            var inv = InventoryManager.Instance;
            var slot = inv != null ? inv.GetSlot(index) : null;
            if (slot == null || slot.IsEmpty) return;
            var data = ItemDatabase.Get(slot.instance.itemId);
            if (data != null && data.fuelSeconds > 0f && CookingRecipe.FindByInput(data.itemID) == null)
            {
                ShowReject(bound.AddFuel(index, 1));
                return;
            }
            // Klik kanan pada tumpukan mengisi baris kosong satu per satu sampai habis.
            var first = bound.InsertToRow(index);
            if (first != CampfireReject.None)
            {
                // Kayu punya resep arang, jadi klik memasukkannya ke baris. Bahan bakar lewat tombol "Tambah Kayu".
                ShowReject(first);
                return;
            }
            if (button != PointerEventData.InputButton.Right) return;
            int guard = 16;
            while (guard-- > 0)
            {
                var s = inv.GetSlot(index);
                if (s == null || s.IsEmpty || bound.InsertToRow(index) != CampfireReject.None) break;
            }
        }

        private void OnAddFuelClicked()
        {
            if (bound == null || !IsTop) return;
            ShowReject(bound.AddFirstFuel());
        }

        private void OnCollectAllClicked()
        {
            if (bound == null || !IsTop) return;
            bound.CollectAll();
        }

        private void OnInputClicked(int index)
        {
            if (bound == null || !IsTop) return;
            ShowReject(bound.RemoveInput(index));
        }

        private void OnOutputClicked(int index)
        {
            if (bound == null || !IsTop) return;
            ShowReject(bound.CollectRow(index));
        }

        private void ShowReject(CampfireReject r)
        {
            if (r == CampfireReject.None || r == CampfireReject.Empty || rejectText == null) return;
            // TODO(LocalizationManager #104): kunci teks.
            string text;
            switch (r)
            {
                case CampfireReject.CannotCook: text = "Tidak bisa dimasak"; break;
                case CampfireReject.Rotten: text = "Sudah busuk"; break;
                case CampfireReject.NoEmptyRow: text = "Baris masak penuh"; break;
                case CampfireReject.FuelFull: text = "Bahan bakar penuh"; break;
                case CampfireReject.NotFuel: text = "Tidak ada bahan bakar"; break;
                case CampfireReject.NoSpace: text = "Inventori penuh"; break;
                case CampfireReject.TooWindy: text = "Terlalu berangin"; break;
                default: text = string.Empty; break;
            }
            rejectText.text = text;
            rejectText.gameObject.SetActive(true);
            rejectHideAt = Time.unscaledTime + rejectShowSeconds;
        }
    }
}
