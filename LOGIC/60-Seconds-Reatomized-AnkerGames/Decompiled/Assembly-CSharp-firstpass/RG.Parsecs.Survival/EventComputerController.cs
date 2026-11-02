using UnityEngine;

namespace RG.Parsecs.Survival;

public class EventComputerController : DisplayView
{
	[SerializeField]
	private SurvivalData _survivalData;

	[SerializeField]
	private SystemView.Lamps _lamps;

	[SerializeField]
	private SystemView.Lamps.ELampState _lampState;

	[SerializeField]
	private ComputerLampsListenerList _computerLampsListenerList;

	private void Awake()
	{
		_computerLampsListenerList.RegisterOnLampsUpdate(UpdateLampColor);
		UpdateLampColor();
	}

	private void OnDestroy()
	{
		_computerLampsListenerList.UnregisterOnLampsUpdate(UpdateLampColor);
	}

	public override void ShowWindowView()
	{
		if (_survivalData.StarlogOpenedOnFirstDay)
		{
			base.ShowWindowView();
		}
	}

	public void UpdateLampColor()
	{
		if (EndGameManager.Instance.GetEndGameData().RuntimeData.ShouldEndGame)
		{
			_lampState = SystemView.Lamps.ELampState.RED;
		}
		else if (_survivalData.DailyEventResolved)
		{
			_lampState = SystemView.Lamps.ELampState.GREEN;
		}
		else if (!_survivalData.StarlogOpenedOnFirstDay)
		{
			_lampState = SystemView.Lamps.ELampState.RED;
		}
		else if (!_survivalData.DailyEventResolved)
		{
			_lampState = SystemView.Lamps.ELampState.YELLOW_BLINKING;
		}
		_lamps.SetLampBasedOnState(_lampState);
	}
}
