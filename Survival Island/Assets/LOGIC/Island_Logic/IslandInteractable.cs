using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Isi save satu objek interaksi pulau (IslandInteractable.md Kasus Khusus "Save dan load", #17).</summary>
    [Serializable]
    public class IslandInteractableSaveData
    {
        public string saveId;
        public bool isConsumed;
        public bool isOpened;
        public int amount = -1;   // -1 berarti tidak dipakai jenis objek ini
    }

    /// <summary>
    /// Kelas dasar objek tombol E di pulau (Fase 2). Blueprint: Island_Logic/IslandInteractable.md (#17).
    /// GDD bagian 3 (looting), 4 (Diary), 7 (Lucky Machine). Tombol E: keputusan #85 (GLOSSARY.md).
    /// IInteractable, InteractContext, PlayerInteractor, dan InteractHighlight sudah ditulis Langkah 2 dan tidak diulang.
    ///
    /// Aturan bersama: objek dengan blockedWhileCarrying menolak dengan CarryingHeavyItem (usulan disetujui 3: saat
    /// membawa Heavy Item hanya pintu yang boleh dipakai). Highlight Available/Blocked mengikuti CanInteract.
    ///
    /// Catatan implementasi (sistem pendukung, juga di blueprint):
    /// - RegionStreaming/RegionStateStore (#88) belum ada. Objek dengan saveId mendaftar sendiri ke WorldStateSave sebagai
    ///   jalur cadangan (pola LootSpawnerSystem). Pendaftaran di Awake dan dilepas di OnDestroy, bukan OnEnable/OnDisable,
    ///   supaya objek yang sudah dipakai (dimatikan) tetap tersimpan sebagai isConsumed. saveId kosong = tidak disimpan.
    /// - PlayerInteractor belum mengisi InteractContext.IsCarryingHeavyItem (#69), jadi HeavyItemSystem.IsCarrying juga dibaca.
    /// </summary>
    public abstract class IslandInteractable : MonoBehaviour, IInteractable, ISaveable
    {
        [Tooltip("Kunci teks aksi untuk #104, tanpa nama tombol. Contoh prompt.pickup_stone.")]
        [SerializeField] protected string promptKey;
        [Tooltip("0 berarti langsung. Interaksi tahan batal saat berlari atau diserang (PlayerInteractor).")]
        [SerializeField] protected float holdSeconds;
        [SerializeField] protected bool blockedWhileCarrying = true;
        [Tooltip("ID stabil dari #80, contoh r1_pickup_0001. Kosong berarti status tidak disimpan.")]
        [SerializeField] protected string saveId;
        [SerializeField] protected InteractHighlight highlight;

        protected bool isConsumed;
        protected bool isOpened;
        private bool isHovered;
        private bool registeredForSave;

        public bool IsConsumed => isConsumed;
        public bool IsOpened => isOpened;

        /// <summary>Aksi khusus tiap jenis objek. true kalau berhasil.</summary>
        protected abstract bool OnInteract(in InteractContext ctx);

        // ---------------------------------------------------------------- Siklus hidup

        protected virtual void Awake()
        {
            if (highlight == null) highlight = GetComponentInChildren<InteractHighlight>();
            // TODO(RegionStreaming): objek region disimpan lewat RegionStateStore (#88), bukan WorldStateSave.Register.
            if (!string.IsNullOrEmpty(saveId) && WorldStateSave.Instance != null)
            {
                WorldStateSave.Instance.Register(this);
                registeredForSave = true;
            }
        }

        protected virtual void OnDestroy()
        {
            if (registeredForSave && WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            registeredForSave = false;
        }

        // ---------------------------------------------------------------- IInteractable

        public virtual bool IsAvailable => isActiveAndEnabled && !isConsumed;

        public virtual float HoldSeconds => holdSeconds;

        public virtual string GetPrompt() => Localize(promptKey);

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            if (!IsAvailable) return InteractBlockReason.Locked;
            if (blockedWhileCarrying && IsCarrying(ctx)) return InteractBlockReason.CarryingHeavyItem;
            return CanInteractExtra(ctx);
        }

        /// <summary>Syarat tambahan jenis objek (misalnya inventori penuh). Nilai awal None.</summary>
        protected virtual InteractBlockReason CanInteractExtra(in InteractContext ctx) => InteractBlockReason.None;

        public virtual void OnHoverEnter()
        {
            isHovered = true;
            RefreshHighlight();
        }

        public virtual void OnHoverExit()
        {
            isHovered = false;
            if (highlight != null) highlight.SetState(HighlightState.Off);
        }

        public bool Interact(in InteractContext ctx)
        {
            if (CanInteract(ctx) != InteractBlockReason.None) return false;
            bool ok = OnInteract(ctx);
            RefreshHighlight();
            return ok;
        }

        // ---------------------------------------------------------------- Bantuan

        protected static bool IsCarrying(in InteractContext ctx)
        {
            if (ctx.IsCarryingHeavyItem) return true;
            var heavy = HeavyItemSystem.Instance;
            return heavy != null && heavy.IsCarrying;
        }

        protected static string Localize(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            var loc = LocalizationManager.Instance;
            return loc != null ? loc.Get(key) : key;
        }

        /// <summary>Konteks tanpa PlayerInteractor, untuk warna highlight saat hover.</summary>
        protected static InteractContext BuildContext()
        {
            var player = IslandPlayerController.Instance;
            var heavy = HeavyItemSystem.Instance;
            return new InteractContext(InventoryManager.Instance, player != null ? player.transform : null,
                                       heavy != null && heavy.IsCarrying);
        }

        /// <summary>Kuning muda kalau CanInteract None, merah kalau ditolak (tabel InteractHighlight).</summary>
        protected void RefreshHighlight()
        {
            if (highlight == null) return;
            if (!isHovered || !IsAvailable)
            {
                highlight.SetState(HighlightState.Off);
                return;
            }
            highlight.SetState(CanInteract(BuildContext()) == InteractBlockReason.None ? HighlightState.Available : HighlightState.Blocked);
        }

        /// <summary>Objek habis dipakai: catat, matikan highlight, lalu matikan objek.</summary>
        protected void Consume()
        {
            isConsumed = true;
            isHovered = false;
            if (highlight != null) highlight.SetState(HighlightState.Off);
            gameObject.SetActive(false);
        }

        // ---------------------------------------------------------------- Save (#80, #88)

        public string SaveId => saveId;
        public int LoadOrder => 0;   // Objek region memakai 0

        /// <summary>Jumlah sisa untuk save. -1 kalau tidak dipakai.</summary>
        protected virtual int SavedAmount => -1;

        /// <summary>Isi jumlah sisa dari save.</summary>
        protected virtual void ApplySavedAmount(int amount) { }

        /// <summary>Tampilan setelah RestoreState (tutup peti terbuka, dan sejenisnya). Tidak mengirim event.</summary>
        protected virtual void ApplyRestoredVisual() { }

        public virtual string CaptureState()
        {
            return JsonUtility.ToJson(new IslandInteractableSaveData
            {
                saveId = saveId, isConsumed = isConsumed, isOpened = isOpened, amount = SavedAmount,
            });
        }

        public virtual void RestoreState(string json)
        {
            // Hanya mengisi data. Objek yang sudah dipakai dimatikan.
            if (string.IsNullOrEmpty(json)) return;
            var d = JsonUtility.FromJson<IslandInteractableSaveData>(json);
            if (d == null) return;
            isConsumed = d.isConsumed;
            isOpened = d.isOpened;
            if (d.amount >= 0) ApplySavedAmount(d.amount);
            ApplyRestoredVisual();
            if (isConsumed) gameObject.SetActive(false);
        }

        public virtual void OnRestoreFinished() { }
    }
}
