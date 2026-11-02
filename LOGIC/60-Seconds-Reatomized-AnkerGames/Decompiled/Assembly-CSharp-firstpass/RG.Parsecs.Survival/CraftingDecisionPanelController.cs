using UnityEngine;

namespace RG.Parsecs.Survival;

public class CraftingDecisionPanelController : MonoBehaviour
{
	[SerializeField]
	private CraftingData _craftingData;

	[SerializeField]
	private RepairedCraftingPanelController _repairedCraftingPanelController;

	[SerializeField]
	private CraftingActionToggleController[] _toggles;

	public void Show()
	{
		for (int i = 0; i < _toggles.Length; i++)
		{
			_toggles[i].Show();
		}
	}

	public void SetPlannedCraftingAction(EPlannedCraftingAction plannedCraftingAction)
	{
		_craftingData.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction = plannedCraftingAction;
	}

	public void SetAllTogglesOff(bool isVisible)
	{
		for (int i = 0; i < _toggles.Length; i++)
		{
			_toggles[i].Interactable = isVisible;
			_toggles[i].IsOn = false;
		}
	}
}
