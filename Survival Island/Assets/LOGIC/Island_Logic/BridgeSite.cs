using System;
using TMPro;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Tempat setoran kayu jembatan gantung di tepi jurang S1 (Region1_Volcano). Menampilkan progres di papan dunia game.
    /// Blueprint: Island_Logic/RegionGateSystem.md bagian "Menyetor kayu" dan "Visual jembatan mengikuti progres" (#68).
    /// GDD bagian 5, Region 2: akses lewat jembatan, syarat kayu GameBalanceConfig.region.bridgeWoodCost (nilai awal 500).
    /// Claude.md Tahap 4: kalau kayu cukup, aktifkan GameObject jembatan. Di sini setoran kumulatif menggantikan cek
    /// "Kayu >= 500" sekaligus, karena 500 kayu memakan 5 slot (CarryCapacity.md).
    ///
    /// Tidak menyimpan data sendiri: status dibaca dari RegionGateSystem. Jembatan tidak punya BaseHealth (usulan 3, tidak bisa dirusak).
    /// </summary>
    public class BridgeSite : MonoBehaviour, IInteractable
    {
        private const string WoodId = "Mat_Wood";

        [Tooltip("Ruas jembatan, aktif berurutan sesuai progres (20 ruas).")]
        [SerializeField] private Transform[] segments = Array.Empty<Transform>();
        private const string WoodDustTag = "VFX_WoodDust";
        private int lastVisible = -1;
        [Tooltip("Collider lantai. Aktif hanya saat jembatan selesai, jadi ruas setengah jadi tidak bisa dipijak.")]
        [SerializeField] private Collider deckCollider;
        [SerializeField] private TMP_Text progressLabel;
        [SerializeField] private GameObject constructionProps;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;
        private static RegionGateSystem Gates => RegionGateSystem.Instance;

        public bool IsAvailable => Gates != null && !Gates.BridgeComplete;
        public float HoldSeconds => Balance != null ? Balance.region.bridgeDepositHoldSeconds : 1f;

        private void OnEnable()
        {
            EventChannel<BridgeProgressChangedEvent>.Raised += HandleProgress;
            EventChannel<RegionUnlockedEvent>.Raised += HandleUnlocked;
            // Region dilepas lalu dimuat lagi: ruas kembali sesuai data.
            RefreshVisual();
        }

        private void OnDisable()
        {
            EventChannel<BridgeProgressChangedEvent>.Raised -= HandleProgress;
            EventChannel<RegionUnlockedEvent>.Raised -= HandleUnlocked;
        }

        private void HandleProgress(BridgeProgressChangedEvent e) => RefreshVisual();

        private void HandleUnlocked(RegionUnlockedEvent e)
        {
            if (e.region == RegionType.Amazon) RefreshVisual();
        }

        // ---------------------------------------------------------------- Interaksi

        public string GetPrompt()
        {
            var g = Gates;
            if (g == null) return string.Empty;
            if (!g.HasBridgeBlueprint) return "Butuh Blueprint Jembatan";
            var inv = InventoryManager.Instance;
            if (inv == null || inv.GetCount(WoodId) <= 0) return $"Butuh kayu ({g.WoodDeposited} / {g.WoodRequired})";
            return $"Setor kayu ({g.WoodDeposited} / {g.WoodRequired})";
        }

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            var g = Gates;
            // 1.
            if (ctx.IsCarryingHeavyItem) return InteractBlockReason.CarryingHeavyItem;
            // 2. Kayu boleh dikumpulkan sebelum Blueprint didapat. Hanya setorannya yang terkunci.
            if (g == null || !g.HasBridgeBlueprint) return InteractBlockReason.Locked;
            // 3.
            if (ctx.Inventory == null || ctx.Inventory.GetCount(WoodId) <= 0) return InteractBlockReason.Locked;
            // 4.
            return InteractBlockReason.None;
        }

        public bool Interact(in InteractContext ctx)
        {
            var g = Gates;
            if (g == null || ctx.Inventory == null) return false;
            // bridgeWoodCost bisa diturunkan DevConsole di bawah woodDeposited.
            g.TryCompleteBridge();
            // 1. Setoran sebagian tetap diterima. Tidak ada kayu yang tersisa di lokasi.
            int n = Mathf.Min(ctx.Inventory.GetCount(WoodId), g.WoodRemaining);
            if (n <= 0) return false;
            // 2-4. Deposit menerbitkan BridgeProgressChangedEvent dan menyelesaikan jembatan saat sisa 0.
            ctx.Inventory.RemoveItem(WoodId, n);
            int accepted = g.Deposit(n);
            return accepted > 0;
        }

        public void OnHoverEnter() { }
        public void OnHoverExit() { }

        // ---------------------------------------------------------------- Visual

        public void RefreshVisual()
        {
            var g = Gates;
            int deposited = g != null ? g.WoodDeposited : 0;
            int required = g != null ? g.WoodRequired : 500;
            bool complete = g != null && g.BridgeComplete;

            // 1. Rasio dibatasi 1.
            float ratio = complete ? 1f : (required > 0 ? Mathf.Clamp01(deposited / (float)required) : 1f);
            int visible = Mathf.FloorToInt(ratio * segments.Length);
            // 2. Ruas baru muncul dengan debu kayu lewat VFXManager (#15). Refresh pertama (load) tanpa debu.
            if (lastVisible >= 0 && visible > lastVisible && VFXManager.Instance != null)
                for (int i = lastVisible; i < visible && i < segments.Length; i++)
                    if (segments[i] != null) VFXManager.Instance.Play(WoodDustTag, segments[i].position, Quaternion.identity);
            lastVisible = visible;
            for (int i = 0; i < segments.Length; i++)
                if (segments[i] != null) segments[i].gameObject.SetActive(i < visible);
            // 3. Papan di dunia game, bukan HUD.
            if (progressLabel != null) progressLabel.text = $"{Mathf.Min(deposited, required)} / {required}";
            // 4.
            if (deckCollider != null) deckCollider.enabled = complete;
            // 5.
            if (constructionProps != null) constructionProps.SetActive(!complete);
        }
    }
}
