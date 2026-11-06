using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Tampilan hotbar di HUD, bagian HUDManager tanpa UIPanel (bukan panel fokus).
    /// Blueprint: Core_Logic/EquipmentManager.md bagian "Tampilan hotbar di HUD" (#60), Core_Logic/HUDManager.md.
    /// Delapan kotak bernomor, ikon, jumlah tumpukan, bar durabilitas, bingkai kotak aktif (berkedip ke kotak tujuan
    /// selama ganti item), kotak tangan kiri terpisah, dan peredupan saat kunci HeavyItem.
    /// Isi digambar ulang hanya saat HUDManager memanggil Refresh (dari event), bukan setiap frame.
    /// </summary>
    public class HotbarView : MonoBehaviour
    {
        [Serializable]
        public class SlotWidget
        {
            public GameObject root;
            public Image icon;
            public TMP_Text countText;
            public TMP_Text numberText;
            public Image frame;               // Bingkai kotak aktif
            public Image durabilityFill;      // Image.Type Filled, horizontal
        }

        [SerializeField] private SlotWidget[] slots = new SlotWidget[0];
        [SerializeField] private SlotWidget offHandSlot;
        [Tooltip("Diredupkan saat kunci HeavyItem.")]
        [SerializeField] private CanvasGroup group;

        [Header("Tampilan (usulan, tata letak final menunggu developer)")]
        [SerializeField] private float dimmedAlpha = 0.35f;
        [SerializeField] private float blinkHz = 4f;
        [SerializeField] private Color durabilityNormal = new Color(0.4f, 0.9f, 0.4f);
        [SerializeField] private Color durabilityLow = new Color(1f, 0.85f, 0.2f);
        [SerializeField] private Color durabilityBroken = new Color(0.9f, 0.2f, 0.2f);

        private int blinkIndex = -1;
        private float blinkTime;

        private void Awake()
        {
            for (int i = 0; i < slots.Length; i++)
                if (slots[i] != null && slots[i].numberText != null) slots[i].numberText.text = (i + 1).ToString();
        }

        private void OnEnable()
        {
            Refresh();
        }

        /// <summary>Gambar ulang semua kotak dari EquipmentManager dan InventoryManager.</summary>
        public void Refresh()
        {
            var eq = EquipmentManager.Instance;
            var inv = InventoryManager.Instance;
            for (int i = 0; i < slots.Length; i++)
            {
                int id = eq != null ? eq.GetHotbarInstanceId(i) : EquipmentManager.NoSlot;
                DrawSlot(slots[i], inv, id);
                if (slots[i] != null && slots[i].frame != null)
                    slots[i].frame.enabled = eq != null && eq.ActiveHotbarIndex == i;
            }
            DrawSlot(offHandSlot, inv, eq != null ? eq.OffHandInstanceId : EquipmentManager.NoSlot);
            if (offHandSlot != null && offHandSlot.frame != null) offHandSlot.frame.enabled = eq != null && eq.OffHandActive;

            if (group != null)
                group.alpha = eq != null && (eq.Locks & EquipLock.HeavyItem) != 0 ? dimmedAlpha : 1f;
        }

        private void DrawSlot(SlotWidget w, InventoryManager inv, int instanceId)
        {
            if (w == null) return;
            ItemInstance inst = inv != null && instanceId != EquipmentManager.NoSlot ? inv.FindInstance(instanceId) : null;
            ItemData data = inst != null ? ItemDatabase.Get(inst.itemId) : null;

            if (w.icon != null)
            {
                w.icon.enabled = data != null && data.icon != null;
                w.icon.sprite = data != null ? data.icon : null;
            }

            if (w.countText != null)
            {
                int amount = 0;
                if (inst != null)
                {
                    var slot = inv.GetSlot(inv.IndexOfInstance(instanceId));
                    amount = slot != null ? slot.amount : 0;
                }
                w.countText.text = amount > 1 ? amount.ToString() : string.Empty;
            }

            if (w.durabilityFill != null)
            {
                var state = DurabilitySystem.GetState(inst);
                bool show = state != DurabilityState.NoDurability;
                w.durabilityFill.enabled = show;
                if (show)
                {
                    // Rumus rasio hanya di DurabilitySystem.GetRatio (DurabilitySystem.md).
                    w.durabilityFill.fillAmount = DurabilitySystem.GetRatio(inst);
                    w.durabilityFill.color = state == DurabilityState.Normal ? durabilityNormal
                        : state == DurabilityState.Low ? durabilityLow : durabilityBroken;
                }
            }
        }

        /// <summary>Kedip kotak tujuan selama EquipmentManager.State != Idle. Waktu berskala, jadi berhenti saat jeda.</summary>
        private void Update()
        {
            var eq = EquipmentManager.Instance;
            if (eq == null) return;

            if (eq.State == EquipState.Idle)
            {
                if (blinkIndex >= 0)
                {
                    blinkIndex = -1;
                    Refresh();
                }
                return;
            }

            // Kotak tujuan = kotak yang sedang dipilih EquipmentManager.
            int target = eq.ActiveHotbarIndex;
            if (target < 0 || target >= slots.Length || slots[target] == null || slots[target].frame == null) return;
            blinkIndex = target;
            blinkTime += Time.deltaTime;
            slots[target].frame.enabled = Mathf.Repeat(blinkTime * blinkHz, 1f) < 0.5f;
        }
    }
}
