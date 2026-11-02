using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class RationingViewRationToggleController : ToggleController, IRefreshInteractable
{
	[SerializeField]
	private RationingView _rationingView;

	[SerializeField]
	private IItem _rationingItem;

	[SerializeField]
	private bool _rationToAll;

	public void OnShow()
	{
		bool flag = false;
		bool flag2 = false;
		if (_rationToAll)
		{
			flag = _rationingView.CanRationItem(_rationingItem);
			flag2 = _rationingView.CanBeRationedToAll(_rationingItem);
		}
		else
		{
			flag = _rationingView.CanRationItem(_rationingItem);
			flag2 = _rationingView.CanBeRationedToCharacter(_rationingItem);
		}
		_toggle.interactable = flag & flag2;
		SetToggleWithoutInvokingValueChange(_rationToAll ? _rationingView.WasRationedToAll(_rationingItem) : _rationingView.IsItemRationedForThisCharacter(_rationingItem));
	}

	public override void OnToggleValueChangedAction(bool toggleValue)
	{
		if (_rationToAll)
		{
			_rationingView.RationToAll(_rationingItem);
		}
		else
		{
			_rationingView.RationItem(_rationingItem);
		}
	}

	public void RefreshInteractable()
	{
	}
}
