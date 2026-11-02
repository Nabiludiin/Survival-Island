using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class EndOfDayViewRationToggleController : ToggleController, IRefreshInteractable
{
	[SerializeField]
	private EndOfDayRationingBarView _rationingBar;

	[SerializeField]
	private EndOfDayView _endOfDayView;

	[SerializeField]
	private RationingListenerList _rationingListenerList;

	[SerializeField]
	private IItem _rationingItem;

	public void OnEnable()
	{
		RefreshDisplay();
	}

	protected void Start()
	{
		_rationingListenerList.Register(RefreshDisplay);
	}

	private void OnDestroy()
	{
		_rationingListenerList.Unregister(RefreshDisplay);
	}

	public override void OnToggleValueChangedAction(bool toggleValue)
	{
		_endOfDayView.RationingManager.CharacterRationed(_rationingBar.CharacterIndex, _rationingItem);
		RefreshInteractable();
	}

	private void RefreshDisplay()
	{
		SetToggleWithoutInvokingValueChange(_endOfDayView.RationingManager.RationingData.IsItemRationedForCharacter(_rationingBar.CharacterIndex, _rationingItem.BaseStaticData.ItemId));
	}

	public void RefreshInteractable()
	{
		bool flag = _endOfDayView.RationingManager.IsItemAvailableForRationing(_rationingItem) || _endOfDayView.RationingManager.RationingData.IsItemRationedForCharacter(_rationingBar.CharacterIndex, _rationingItem.BaseStaticData.ItemId);
		bool flag2 = _endOfDayView.RationingManager.CanItemBeRationedToCharacter(_rationingItem, _rationingBar.CharacterIndex);
		_toggle.interactable = flag & flag2;
	}
}
