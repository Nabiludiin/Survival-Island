using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class ItemSlotController : MonoBehaviour
{
	[SerializeField]
	[Header("Data")]
	[Tooltip("List of available items.")]
	private ItemList _itemList;

	[SerializeField]
	[Tooltip("Expedition Data scriptable object holding informations about expedition system.")]
	private ExpeditionData _expeditionData;

	[SerializeField]
	[Tooltip("Suit System level required to have this point available.")]
	private int _requiredSuitLevel;

	[SerializeField]
	[Tooltip("Sprite for no item selected.")]
	private Sprite _noItemSprite;

	[SerializeField]
	[Header("References")]
	[Tooltip("Game Object with elements that are displayed when this item slot is locked because insufficient suit level.")]
	private GameObject _locked;

	[SerializeField]
	[Tooltip("Game Object with elements that are displayed when this item slot is unlocked.")]
	private GameObject _unlocked;

	[SerializeField]
	[Tooltip("Field for displaying item icon.")]
	private Image _itemImage;

	[SerializeField]
	[HideInInspector]
	private int _currentItemIndex = -1;

	[SerializeField]
	[HideInInspector]
	private IItem _item;

	public int RequiredSuitLevel => _requiredSuitLevel;

	public int CurrentItemIndex => _currentItemIndex;

	public void ResetSelectItem()
	{
		UnlockCurrentItem();
		ClearSlot();
	}

	public void SetNextItem()
	{
		UnlockCurrentItem();
		for (int i = _currentItemIndex + 1; i < _itemList.Items.Count; i++)
		{
			Item item = _itemList.Items[i] as Item;
			if ((!(item != null) || !item.RuntimeData.IsDamaged) && _itemList.Items[i].BaseRuntimeData.IsAvailable && !_expeditionData.RuntimeData.PlannedExpeditionData.ExpeditionItems.Contains(_itemList.Items[i]) && _itemList.Items[i].IsLockable())
			{
				_item = _itemList.Items[i];
				_itemImage.sprite = _item.BaseStaticData.IconJournal;
				_currentItemIndex = i;
				AddItemToExpeditionItems(_item);
				_item.Lock();
				return;
			}
		}
		ClearSlot();
	}

	public void SetPreviousItem()
	{
		UnlockCurrentItem();
		if (_currentItemIndex == -1)
		{
			_currentItemIndex = _itemList.Items.Count;
		}
		for (int num = _currentItemIndex - 1; num >= 0; num--)
		{
			Item item = _itemList.Items[num] as Item;
			if ((!(item != null) || !item.RuntimeData.IsDamaged) && _itemList.Items[num].BaseRuntimeData.IsAvailable && !_expeditionData.RuntimeData.PlannedExpeditionData.ExpeditionItems.Contains(_itemList.Items[num]) && _itemList.Items[num].IsLockable())
			{
				_item = _itemList.Items[num];
				_itemImage.sprite = _item.BaseStaticData.IconJournal;
				_currentItemIndex = num;
				AddItemToExpeditionItems(_item);
				_item.Lock();
				return;
			}
		}
		ClearSlot();
	}

	private void AddItemToExpeditionItems(IItem item)
	{
		_expeditionData.RuntimeData.PlannedExpeditionData.ExpeditionItems.Add(item);
	}

	private void RemoveItemFromExpeditionItems(IItem item)
	{
		_expeditionData.RuntimeData.PlannedExpeditionData.ExpeditionItems.Remove(item);
	}

	public void SetLocked(bool locked)
	{
		_unlocked.SetActive(!locked);
		_locked.SetActive(locked);
		if (locked)
		{
			UnlockCurrentItem();
			ClearSlot();
		}
	}

	private void ClearSlot()
	{
		_item = null;
		_itemImage.sprite = _noItemSprite;
		_currentItemIndex = -1;
	}

	private void UnlockCurrentItem()
	{
		if (_item != null)
		{
			RemoveItemFromExpeditionItems(_item);
			_item.Unlock();
		}
	}
}
