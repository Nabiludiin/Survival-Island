using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class StarlogComputerController : DisplayView
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

	public void UpdateLampColor()
	{
		if (!_survivalData.StarlogOpened)
		{
			_lampState = SystemView.Lamps.ELampState.YELLOW_BLINKING;
		}
		else
		{
			_lampState = SystemView.Lamps.ELampState.GREEN;
		}
		_lamps.SetLampBasedOnState(_lampState);
	}

	public override void ShowWindowView()
	{
		if (!(_windowView == null) && !Singleton<HandCursor>.Instance.IsPointerOverGameObject())
		{
			_windowView.Show();
		}
	}
}
