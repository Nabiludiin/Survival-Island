using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Titik karang kecil di Region 1 yang hanya bisa dipanen saat surut (sekitar 06:00 dan 18:00), sekali per hari.
    /// Blueprint: Island_Logic/TideManager.md bagian "Area saat surut" (#91). Usulan 5 disetujui: hadiah kecil tanpa barang
    /// wajib. Objek region: status harvestedToday disimpan lewat RegionStateStore dengan SaveIdTag.
    /// IInteractable langsung (IslandInteractable #17 belum ada).
    /// </summary>
    public class LowTideSpot : MonoBehaviour, IInteractable, ISaveable
    {
        public const string RewardItemId = "Food_RawFish";

        [SerializeField] private SaveIdTag saveTag;
        [SerializeField] private InteractHighlight highlight;

        private bool harvestedToday;
        private bool registeredForSave;

        private void Awake()
        {
            if (saveTag == null) saveTag = GetComponent<SaveIdTag>();
            // Objek region: RegionStateStore (#88). Tanpa RegionStreaming (scene uji): jalur cadangan WorldStateSave.
            registeredForSave = RegionSaveRouting.Register(this, this);
        }

        private void OnDestroy() { RegionSaveRouting.Unregister(this, ref registeredForSave); }

        private void OnEnable() { EventChannel<DayChangedEvent>.Raised += HandleDayChanged; }
        private void OnDisable() { EventChannel<DayChangedEvent>.Raised -= HandleDayChanged; }

        private void HandleDayChanged(DayChangedEvent e)
        {
            // Kiriman ulang setelah load (isReplay) diabaikan, supaya keluar lalu lanjutkan tidak membuka panen lagi.
            if (e.isReplay) return;
            harvestedToday = false;
        }

        public bool IsAvailable => isActiveAndEnabled && !harvestedToday && TideManager.Instance != null && TideManager.Instance.IsLow;
        public float HoldSeconds => 0f;
        public string GetPrompt() { var loc = LocalizationManager.Instance; return loc != null && loc.Has("prompt.low_tide") ? loc.Get("prompt.low_tide") : "Ambil hasil karang"; }   // #104
        public InteractBlockReason CanInteract(in InteractContext ctx) => ctx.IsCarryingHeavyItem ? InteractBlockReason.CarryingHeavyItem : InteractBlockReason.None;
        public void OnHoverEnter() { if (highlight != null) highlight.SetState(HighlightState.Available); }
        public void OnHoverExit() { if (highlight != null) highlight.SetState(HighlightState.Off); }

        public bool Interact(in InteractContext ctx)
        {
            if (!IsAvailable) return false;
            var item = ItemDatabase.Get(RewardItemId);
            if (item == null) return false;
            harvestedToday = true;
            var inv = ctx.Inventory != null ? ctx.Inventory : InventoryManager.Instance;
            int left = inv != null ? inv.AddItem(item, 1) : 1;
            // Inventori penuh: hasil jatuh sebagai WorldItem.
            if (left > 0 && WorldItemManager.Instance != null && inv != null)
                WorldItemManager.Instance.Spawn(inv.CreateInstance(item), left, transform.position + Vector3.up * 0.3f, true);
            return true;
        }

        [Serializable] private class SaveData { public bool harvestedToday; }

        public string SaveId => saveTag != null ? saveTag.SaveId : string.Empty;
        public int LoadOrder => 0;
        public string CaptureState() => JsonUtility.ToJson(new SaveData { harvestedToday = harvestedToday });

        public void RestoreState(string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            try { harvestedToday = JsonUtility.FromJson<SaveData>(json).harvestedToday; }
            catch (Exception e) { Debug.LogWarning($"[LowTideSpot] Save tidak terbaca: {e.Message}"); }
        }

        public void OnRestoreFinished() { }
    }
}
