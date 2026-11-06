using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SurvivalIsland
{
    /// <summary>Daftar panel resmi. Blueprint: Core_Logic/UIFocusStack.md bagian 1 (#83).</summary>
    public enum UIPanelId
    {
        MainMenu, Loading, Cinematic, Pause, Settings, ConfirmDialog, RunSummary,
        Diary, DiaryArchive, LoreReader, Inventory, Crafting, Storage, Campfire, BuildMenu,
        LuckyMachine, GameOver, WinScreen, DevConsole, Stats,
        Sleep   // Ditambahkan untuk SleepPanel (SleepSystem.md, #67). Nilai lama tidak diubah (urutan serialisasi).
    }

    /// <summary>Alasan panel tertutup.</summary>
    public enum UICloseReason { User, Forced, RunEnded, SceneUnloaded }

    /// <summary>
    /// Pengaturan tiap panel. Nilai per panel mengikuti tabel "Pengaturan tiap panel" di UIFocusStack.md,
    /// diisi di Inspector prefab panel.
    /// </summary>
    [Serializable]
    public struct UIPanelSettings
    {
        public bool pausesTime;          // Membuat Time.timeScale = 0
        public bool releasesCursor;      // Melepas dan menampilkan kursor
        public bool blocksGameplayInput; // Mematikan Action Map Gameplay (#18)
        public bool closableByEsc;       // Esc boleh menutup panel ini
        public bool isLocked;            // Tidak ikut tertutup oleh CloseAllUnlocked
        public bool rejectsNewPanels;    // Selama di puncak, panel lain tidak bisa masuk
    }

    /// <summary>
    /// Dasar semua panel UI. Panel hanya meminta dibuka atau ditutup lewat UIFocusStack, dan tidak mengubah
    /// Time.timeScale, kursor, atau audio sendiri. Blueprint: Core_Logic/UIFocusStack.md (#83).
    ///
    /// Catatan implementasi: Show dan Hide tetap protected abstract seperti blueprint. UIFocusStack memanggilnya
    /// lewat InvokeShow dan InvokeHide (internal), karena kelas lain tidak bisa memanggil anggota protected.
    /// </summary>
    public abstract class UIPanel : MonoBehaviour
    {
        public UIPanelId id;
        public UIPanelSettings settings;

        [Tooltip("Tombol yang juga menutup panel (UI/Toggle...). Boleh kosong.")]
        public InputActionReference toggleAction;

        protected abstract void Show();
        protected abstract void Hide();

        /// <summary>Panel menjadi puncak stack.</summary>
        public virtual void OnFocusGained() { }

        /// <summary>Panel tertutup panel baru.</summary>
        public virtual void OnFocusLost() { }

        /// <summary>Panel tertutup. Tempat mengembalikan barang yang sedang dipegang, dan sejenisnya.</summary>
        public virtual void OnClosed(UICloseReason reason) { }

        internal void InvokeShow() => Show();
        internal void InvokeHide() => Hide();

        /// <summary>
        /// Panel scene yang hilang saat scene diganti keluar dari stack dengan alasan SceneUnloaded.
        /// OnDisable sengaja tidak dipakai, karena reset (#82) mematikan sebentar panel persisten seperti Loading.
        /// </summary>
        protected virtual void OnDestroy()
        {
            var stack = UIFocusStack.Instance;
            if (stack != null && stack.Contains(this))
                stack.Close(this, UICloseReason.SceneUnloaded);
        }
    }
}
