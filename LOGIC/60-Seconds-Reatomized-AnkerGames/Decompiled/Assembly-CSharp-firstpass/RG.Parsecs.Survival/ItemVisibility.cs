using RG.VirtualInput;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class ItemVisibility : MonoBehaviour
{
	[SerializeField]
	private EndOfDayListenerList _endOfDay;

	[SerializeField]
	protected IItem _item;

	[SerializeField]
	protected SurvivalItemSceneReference[] _itemContainers;

	protected VirtualInputButton _virtualButton;

	private void Start()
	{
		_endOfDay.RegisterOnEndOfDay(RefreshVisibility, "Visuals", 4, this, forceOrder: true);
		SetItemActive(_item.BaseRuntimeData.IsAvailable);
		RefreshDisplay();
	}

	public void RefreshVisibility()
	{
		RefreshDisplay();
	}

	public virtual void RefreshDisplay()
	{
		SetItemActive(_item.BaseRuntimeData.IsAvailable);
	}

	private void SetItemActive(bool active)
	{
		Item item = _item as Item;
		bool damaged = false;
		if (item != null)
		{
			damaged = item.RuntimeData.IsDamaged;
		}
		for (int i = 0; i < _itemContainers.Length; i++)
		{
			_itemContainers[i].SetActive((i + 1 == _item.BaseRuntimeData.Level) & active, damaged);
		}
		if (_virtualButton == null)
		{
			_virtualButton = GetComponentInChildren<VirtualInputButton>();
		}
		_virtualButton.Selectable.interactable = active;
	}

	private void OnDestroy()
	{
		_endOfDay.UnregisterOnEndOfDay(RefreshVisibility, "Visuals");
	}
}
