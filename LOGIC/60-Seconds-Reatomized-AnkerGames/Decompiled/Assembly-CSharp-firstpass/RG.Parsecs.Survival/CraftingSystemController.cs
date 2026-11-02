using I2.Loc;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class CraftingSystemController : CraftingCraftableController
{
	[SerializeField]
	private GameObject _systemLabel;

	[SerializeField]
	private ShipSystem _shipSystem;

	[SerializeField]
	private LocalizedString _onExpeditionTerm;

	[SerializeField]
	private ExpeditionData _expeditionData;

	[SerializeField]
	private bool _ignoreSystemLevel;

	private bool _invokeOnValueChange = true;

	public override void SetIsOnWithoutInvokingOnValueChange(bool isOn)
	{
		_invokeOnValueChange = false;
		IsOn = isOn;
		_invokeOnValueChange = true;
	}

	protected override Sprite SetSprite()
	{
		_systemLabel.SetActive(value: true);
		return _shipSystem.Icon;
	}

	protected override void OnShow(OngoingCraftingActionData plannedCraftingAction)
	{
		if (plannedCraftingAction.PlannedCraftingAction != EPlannedCraftingAction.UPGRADE || !_shipSystem.IsUpgradable || !_shipSystem.IsAvailable || ((_shipSystem is ExpeditionData || _shipSystem is SuitData) && (_expeditionData.RuntimeData.IsOngoing || _expeditionData.RuntimeData.IsPlanned || _expeditionData.RuntimeData.CurrentPlanetDeck == null)))
		{
			gameObject.SetActive(value: false);
			return;
		}
		CraftingOperation upgradeCostForCurrentLevel = _shipSystem.GetUpgradeCostForCurrentLevel();
		if (upgradeCostForCurrentLevel == null)
		{
			return;
		}
		bool flag = CraftingData.SystemLevel >= upgradeCostForCurrentLevel.RequiredSystemLevel && (!upgradeCostForCurrentLevel.AvailableOnlyOnRequiredSystemLevel || CraftingData.SystemLevel == upgradeCostForCurrentLevel.RequiredSystemLevel);
		gameObject.SetActive(flag || _ignoreSystemLevel);
		if (flag || _ignoreSystemLevel)
		{
			if (!_shipSystem.AvailableInCraftingDuringExpedition && _expeditionData.RuntimeData.IsOngoing)
			{
				Togglable = false;
				DisabledReason = _onExpeditionTerm;
			}
			else if (_ignoreSystemLevel)
			{
				_systemLabel.SetActive(value: false);
				Togglable = false;
				DisabledReason = $"{LevelTerm} {upgradeCostForCurrentLevel.RequiredSystemLevel}";
			}
			else if (AvailableResources.CanLockForCraftingUI(upgradeCostForCurrentLevel.Resources))
			{
				_systemLabel.SetActive(value: true);
				Togglable = true;
			}
			else
			{
				_systemLabel.SetActive(value: false);
				Togglable = false;
				DisabledReason = NoResourcesTerm;
			}
		}
	}

	public override void SetCraftingElement(bool isOn)
	{
		if (_invokeOnValueChange)
		{
			if (isOn)
			{
				CraftingContentPanelController.SetSystemForCraftingAction(_shipSystem);
			}
			else
			{
				CraftingContentPanelController.SetSystemForCraftingAction(null);
			}
		}
	}
}
