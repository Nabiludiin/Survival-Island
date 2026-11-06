using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Menu palu (UIPanelId.BuildMenu): pilihan mode Perbaiki, Bangun Struktur, Bangun Perabot, Upgrade, dan Bongkar.
    /// Waktu tetap berjalan saat menu terbuka (#83). Blueprint: raft/BuildMenu.md "Menu radial saat memegang palu" (#79).
    /// Pengaturan Inspector: pausesTime = false, releasesCursor = true, blocksGameplayInput = true, closableByEsc = true,
    /// toggleAction = UI/ToggleBuildMenu (klik kanan atau B menutup tanpa mengubah pilihan).
    ///
    /// Catatan implementasi (Langkah 4): bentuk radial dua cincin (usulan disetujui #85) belum dibuat. Kedua cincin
    /// ditampilkan sebagai daftar tombol dari buttonPrefab (Button + TMP_Text): cincin utama berisi 5 mode, dan cincin
    /// kedua berisi daftar BuildableData kategori yang dipilih. Nama memakai nameKey apa adanya sampai #104 ada.
    /// </summary>
    public class BuildMenu : FadingUIPanel
    {
        public static BuildMenu Instance { get; private set; }

        [SerializeField] private Transform mainRingRoot;
        [SerializeField] private Transform subRingRoot;
        [SerializeField] private Button buttonPrefab;
        [SerializeField] private TMP_Text centerText;
        [SerializeField] private Color dimColor = new Color(1f, 1f, 1f, 0.45f);

        private enum Ring { Main, Structure, Furniture }

        private Ring ring = Ring.Main;
        private readonly List<Button> mainButtons = new List<Button>(5);
        private readonly List<Button> subButtons = new List<Button>(16);
        private readonly List<BuildableData> subData = new List<BuildableData>(16);
        private readonly StringBuilder sb = new StringBuilder(128);

        private void Awake()
        {
            Instance = this;
        }

        protected override void OnDestroy()
        {
            if (Instance == this) Instance = null;
            base.OnDestroy();
        }

        /// <summary>Dipanggil HammerEquippable (klik kanan) dan BuildManager (tombol B).</summary>
        public static void Open()
        {
            if (Instance == null || UIFocusStack.Instance == null || UIFocusStack.Instance.Contains(Instance)) return;
            UIFocusStack.Instance.Push(Instance);
        }

        protected override void Show()
        {
            ring = Ring.Main;
            BuildMain();
            DrawSub();
            base.Show();
        }

        public override void OnFocusGained()
        {
            // Kursor ke tengah layar.
            if (Mouse.current != null) Mouse.current.WarpCursorPosition(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
        }

        // ---------------------------------------------------------------- Cincin utama

        private void BuildMain()
        {
            if (mainButtons.Count > 0) return;
            // TODO #104: kunci teks.
            AddMain("Perbaiki", () => Choose(HammerMode.Repair, null));
            AddMain("Bangun Struktur", () => { ring = Ring.Structure; DrawSub(); });
            AddMain("Bangun Perabot", () => { ring = Ring.Furniture; DrawSub(); });
            AddMain("Upgrade", () => Choose(HammerMode.Upgrade, null));
            AddMain("Bongkar", () => Choose(HammerMode.Demolish, null));
        }

        private void AddMain(string label, UnityEngine.Events.UnityAction action)
        {
            if (buttonPrefab == null || mainRingRoot == null) return;
            var b = Instantiate(buttonPrefab, mainRingRoot);
            var t = b.GetComponentInChildren<TMP_Text>(true);
            if (t != null) t.text = label;
            b.onClick.AddListener(action);
            mainButtons.Add(b);
        }

        // ---------------------------------------------------------------- Cincin kedua

        private void DrawSub()
        {
            subData.Clear();
            var bm = BuildManager.Instance;
            if (ring != Ring.Main && bm != null && bm.Buildables != null)
            {
                for (int i = 0; i < bm.Buildables.Count; i++)
                {
                    var d = bm.Buildables[i];
                    if (d == null) continue;
                    bool inRing = ring == Ring.Structure ? d.category == BuildCategory.Structure : d.category != BuildCategory.Structure;
                    if (!inRing) continue;
                    // Sektor dengan unlockNodeId yang belum terbuka disembunyikan.
                    if (!string.IsNullOrEmpty(d.unlockNodeId) && (TechTreeManager.Instance == null || !TechTreeManager.Instance.IsUnlocked(d.unlockNodeId))) continue;
                    // Struktur dari item (Build_Campfire) tidak muncul di menu palu.
                    if (d.cost == null || d.cost.Length == 0) continue;
                    subData.Add(d);
                }
            }

            for (int i = 0; i < subData.Count; i++)
            {
                while (subButtons.Count <= i)
                {
                    if (buttonPrefab == null || subRingRoot == null) return;
                    var b = Instantiate(buttonPrefab, subRingRoot);
                    int captured = subButtons.Count;
                    b.onClick.AddListener(() => { if (captured < subData.Count) Choose(HammerMode.Build, subData[captured]); });
                    subButtons.Add(b);
                }
                var d = subData[i];
                var label = subButtons[i].GetComponentInChildren<TMP_Text>(true);
                // Bahan kurang: tetap bisa dipilih, tetapi redup.
                bool enough = HasMaterials(d);
                if (label != null)
                {
                    label.text = d.nameKey;
                    label.color = enough ? Color.white : dimColor;
                }
                subButtons[i].gameObject.SetActive(true);
            }
            for (int i = subData.Count; i < subButtons.Count; i++) subButtons[i].gameObject.SetActive(false);

            if (centerText != null) centerText.text = ring == Ring.Main ? string.Empty : DescribeAll();
        }

        private string DescribeAll()
        {
            // Tengah menu: nama, biaya, dan jumlah bahan yang dimiliki untuk tiap pilihan.
            var inv = InventoryManager.Instance;
            sb.Clear();
            for (int i = 0; i < subData.Count; i++)
            {
                var d = subData[i];
                sb.Append(d.nameKey).Append(": ");
                for (int k = 0; k < d.cost.Length; k++)
                {
                    int have = inv != null ? inv.GetCount(d.cost[k].itemId) : 0;
                    sb.Append(d.cost[k].amount).Append(' ').Append(d.cost[k].itemId).Append(" (").Append(have).Append(')');
                    if (k < d.cost.Length - 1) sb.Append(", ");
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }

        private static bool HasMaterials(BuildableData d)
        {
            var inv = InventoryManager.Instance;
            if (inv == null || d.cost == null) return false;
            for (int i = 0; i < d.cost.Length; i++)
                if (!inv.HasItem(d.cost[i].itemId, d.cost[i].amount)) return false;
            return true;
        }

        private void Choose(HammerMode mode, BuildableData data)
        {
            if (BuildManager.Instance != null) BuildManager.Instance.SelectMode(mode, data);
            // Memilih lalu menutup menu.
            if (UIFocusStack.Instance != null) UIFocusStack.Instance.Close(this, UICloseReason.User);
        }
    }
}
