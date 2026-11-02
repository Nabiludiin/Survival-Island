using UnityEngine;

namespace RG.Parsecs.Survival;

public class CraftingSystemView : SystemView
{
	[SerializeField]
	private CraftingData _craftingData;

	public override bool Visible => true;

	public override int SystemLevel => _craftingData.RuntimeData.CraftingLevel;

	public override int MaxSystemLevel => _craftingData.StaticData.MaxLevel;

	public override Lamps.ELampState GetLampState
	{
		get
		{
			if (!_craftingData.RuntimeData.IsActive || EndGameManager.Instance.GetEndGameData().RuntimeData.ShouldEndGame)
			{
				return Lamps.ELampState.RED;
			}
			if (_craftingData.RuntimeData.IsActive && !_craftingData.RuntimeData.IsPlanned && !_craftingData.RuntimeData.IsOngoing)
			{
				if (_craftingData.IsFocusedByDesign)
				{
					return Lamps.ELampState.YELLOW_BLINKING;
				}
				return Lamps.ELampState.YELLOW;
			}
			if (_craftingData.RuntimeData.IsOngoing || _craftingData.RuntimeData.IsPlanned)
			{
				return Lamps.ELampState.GREEN;
			}
			Debug.LogError("We've not set correct lamp state for Crafting System. Fall back to RED lamp color.");
			return Lamps.ELampState.RED;
		}
	}
}
