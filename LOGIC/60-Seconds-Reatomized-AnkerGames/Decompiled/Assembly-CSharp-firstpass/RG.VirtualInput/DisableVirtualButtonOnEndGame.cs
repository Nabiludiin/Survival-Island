using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.VirtualInput;

public class DisableVirtualButtonOnEndGame : MonoBehaviour
{
	[SerializeField]
	private VirtualInputButton _virtualButton;

	[SerializeField]
	private EndGameData _endGameData;

	[SerializeField]
	private EndOfDayListenerList _endOfDayListener;

	public void Setup()
	{
		if (_endGameData.RuntimeData.ShouldEndGame)
		{
			_virtualButton.Selectable.interactable = false;
		}
	}

	private void Awake()
	{
		_endOfDayListener.RegisterOnEndOfDay(Setup, "Visuals", 10, this, forceOrder: true);
	}

	private void OnDestroy()
	{
		_endOfDayListener.UnregisterOnEndOfDay(Setup, "Visuals");
	}
}
