using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

public abstract class SystemView : DisplayView
{
	[Serializable]
	public class Lamps
	{
		[Serializable]
		public class Lamp
		{
			public const string BLINKING_PARAMETER_NAME = "Blink";

			[SerializeField]
			private GameObject _lamp;

			[SerializeField]
			private Animator _animator;

			public void SetLampActive(bool visble, bool blinking = false)
			{
				if (_lamp != null)
				{
					_lamp.SetActive(visble);
				}
				if (_animator != null && _animator.gameObject.activeSelf)
				{
					_animator.SetBool("Blink", blinking);
				}
			}
		}

		public enum ELampState
		{
			NONE,
			RED,
			YELLOW,
			YELLOW_BLINKING,
			GREEN
		}

		[SerializeField]
		private Lamp _green;

		[SerializeField]
		private Lamp _yellow;

		[SerializeField]
		private Lamp _red;

		public void SetLampBasedOnState(ELampState lampState)
		{
			switch (lampState)
			{
			case ELampState.RED:
				_red.SetLampActive(visble: true);
				_green.SetLampActive(visble: false);
				_yellow.SetLampActive(visble: false);
				break;
			case ELampState.YELLOW:
				_green.SetLampActive(visble: false);
				_yellow.SetLampActive(visble: true);
				_red.SetLampActive(visble: false);
				break;
			case ELampState.YELLOW_BLINKING:
				_green.SetLampActive(visble: false);
				_yellow.SetLampActive(visble: true, blinking: true);
				_red.SetLampActive(visble: false);
				break;
			case ELampState.GREEN:
				_yellow.SetLampActive(visble: false);
				_red.SetLampActive(visble: false);
				_green.SetLampActive(visble: true);
				break;
			default:
				throw new ArgumentOutOfRangeException("lampState", lampState, null);
			case ELampState.NONE:
				break;
			}
		}
	}

	[SerializeField]
	private GameObject[] _levels;

	[SerializeField]
	private bool _disableOtherLevelsObjects;

	[SerializeField]
	private EndOfDayListenerList _endOfDayListenerList;

	[SerializeField]
	private ComputerLampsListenerList _computerLampsListenerList;

	[SerializeField]
	private Lamps _lamps;

	public abstract bool Visible { get; }

	public abstract int SystemLevel { get; }

	public abstract int MaxSystemLevel { get; }

	public abstract Lamps.ELampState GetLampState { get; }

	private void Awake()
	{
		_endOfDayListenerList.RegisterOnEndOfDay(RefreshVisiblity, "Visuals", 5, this, forceOrder: true);
		if (_computerLampsListenerList != null)
		{
			_computerLampsListenerList.RegisterOnLampsUpdate(UpdateSystemLights);
		}
		Refresh();
		UpdateSystemLights();
	}

	private void OnDestroy()
	{
		_endOfDayListenerList.UnregisterOnEndOfDay(RefreshVisiblity, "Visuals");
		if (_computerLampsListenerList != null)
		{
			_computerLampsListenerList.UnregisterOnLampsUpdate(UpdateSystemLights);
		}
	}

	private void UpdateSystemLights()
	{
		_lamps.SetLampBasedOnState(GetLampState);
	}

	private void RefreshVisiblity()
	{
		Refresh();
	}

	protected void Refresh()
	{
		if (!Visible)
		{
			for (int i = 0; i < MaxSystemLevel; i++)
			{
				_levels[i].SetActive(value: false);
			}
			return;
		}
		for (int j = 0; j < MaxSystemLevel; j++)
		{
			if (_disableOtherLevelsObjects)
			{
				_levels[j].SetActive(j == SystemLevel - 1);
			}
			else if (!_disableOtherLevelsObjects)
			{
				_levels[j].SetActive(j <= SystemLevel - 1);
			}
		}
	}
}
