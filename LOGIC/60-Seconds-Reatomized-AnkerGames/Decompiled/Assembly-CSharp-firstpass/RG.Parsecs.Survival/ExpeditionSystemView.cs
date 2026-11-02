using UnityEngine;

namespace RG.Parsecs.Survival;

public class ExpeditionSystemView : SystemView
{
	[SerializeField]
	private ExpeditionData _expeditionData;

	public override bool Visible => true;

	public override int SystemLevel => _expeditionData.RuntimeData.ExpeditionSystemLevel;

	public override int MaxSystemLevel => _expeditionData.StaticData.MaxLevel;

	public override Lamps.ELampState GetLampState
	{
		get
		{
			if (!_expeditionData.RuntimeData.IsActive || _expeditionData.RuntimeData.CurrentPlanetDeck == null || EndGameManager.Instance.GetEndGameData().RuntimeData.ShouldEndGame)
			{
				return Lamps.ELampState.RED;
			}
			if (_expeditionData.RuntimeData.IsActive && !_expeditionData.RuntimeData.IsPlanned && !_expeditionData.RuntimeData.IsOngoing)
			{
				if (_expeditionData.IsFocusedByDesign)
				{
					return Lamps.ELampState.YELLOW_BLINKING;
				}
				return Lamps.ELampState.YELLOW;
			}
			if (_expeditionData.RuntimeData.IsOngoing || _expeditionData.RuntimeData.IsPlanned)
			{
				return Lamps.ELampState.GREEN;
			}
			Debug.LogError("We've not set correct lamp state for Expedition System. Fall back to RED lamp color.");
			return Lamps.ELampState.RED;
		}
	}
}
