using UnityEngine;

namespace RG.Parsecs.Survival;

public class ArmorVisibility : ItemVisibility
{
	[SerializeField]
	private SuitSystemView _suitSystemView;

	[SerializeField]
	private SurvivalItemSceneReference[] _itemContainersWithoutSuitSystemOn;

	[SerializeField]
	private GameObject _armorWithSuit;

	[SerializeField]
	private GameObject _armorWithoutSuit;

	public override void RefreshDisplay()
	{
		if (_suitSystemView.Visible)
		{
			_armorWithSuit.SetActive(value: true);
			_armorWithoutSuit.SetActive(value: false);
			base.RefreshDisplay();
		}
		else
		{
			_armorWithSuit.SetActive(value: false);
			_armorWithoutSuit.SetActive(value: true);
			RefreshDisplayWithoutSuitSystemOn();
		}
	}

	private void RefreshDisplayWithoutSuitSystemOn()
	{
		SetItemVisibilityWithoutSuitSystemOn(_item.BaseRuntimeData.IsAvailable);
	}

	private void SetItemVisibilityWithoutSuitSystemOn(bool active)
	{
		Item item = _item as Item;
		bool damaged = false;
		if (item != null)
		{
			damaged = item.RuntimeData.IsDamaged;
		}
		_itemContainersWithoutSuitSystemOn[0].SetActive(!active, damaged: false);
		for (int i = 1; i < _itemContainersWithoutSuitSystemOn.Length; i++)
		{
			_itemContainersWithoutSuitSystemOn[i].SetActive((i == _item.BaseRuntimeData.Level) & active, damaged);
		}
	}
}
