using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu sel SlotGridView. Meneruskan klik kiri, klik kanan (UI/RightClick lewat modul input UI, #18), seret, dan lepas.
    /// Blueprint: raft/StorageChest.md (#46). File terpisah karena komponen Unity butuh nama file sama dengan kelas.
    /// </summary>
    public class SlotGridViewCell : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private Image freshnessBar;     // Warna kesegaran makanan (ItemUseSystem.md)
        [SerializeField] private Image durabilityBar;
        [SerializeField] private GameObject highlight;
        [SerializeField] private Color freshColor = new Color(0.35f, 0.85f, 0.35f);
        [SerializeField] private Color staleColor = new Color(0.95f, 0.8f, 0.25f);
        [SerializeField] private Color rottenColor = new Color(0.55f, 0.35f, 0.2f);

        private SlotGridView owner;
        private int index;

        internal void Bind(SlotGridView view, int i)
        {
            owner = view;
            index = i;
            SetEmpty();
        }

        public void SetEmpty()
        {
            if (icon != null) { icon.sprite = null; icon.enabled = false; }
            if (amountText != null) amountText.text = string.Empty;
            if (freshnessBar != null) freshnessBar.enabled = false;
            if (durabilityBar != null) durabilityBar.enabled = false;
        }

        public void SetItem(ItemData data, ItemInstance inst, int amount)
        {
            if (icon != null)
            {
                icon.sprite = data != null ? data.icon : null;
                icon.enabled = true;
            }
            if (amountText != null) amountText.text = amount > 1 ? amount.ToString() : string.Empty;
            if (freshnessBar != null)
            {
                bool spoils = data != null && data.spoilHours > 0f;
                freshnessBar.enabled = spoils;
                if (spoils)
                {
                    var f = ItemUseSystem.GetFreshness(inst, data);
                    freshnessBar.color = f == Freshness.Fresh ? freshColor : f == Freshness.Stale ? staleColor : rottenColor;
                }
            }
            if (durabilityBar != null)
            {
                bool uses = data != null && data.maxDurability > 0f && inst != null && inst.durability >= 0f;
                durabilityBar.enabled = uses;
                if (uses) durabilityBar.fillAmount = Mathf.Clamp01(inst.durability / data.maxDurability);
            }
        }

        public void SetHighlight(bool on)
        {
            if (highlight != null) highlight.SetActive(on);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (owner != null && !eventData.dragging) owner.RaiseClicked(index, eventData.button);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (owner != null && eventData.button == PointerEventData.InputButton.Left) owner.RaiseDragBegan(index);
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (owner != null) owner.RaiseDragEnded();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (owner != null) owner.RaiseDroppedOn(index);
        }
    }
}
