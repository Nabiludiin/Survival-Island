using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RG.VirtualInput;

[RequireComponent(typeof(Slider))]
public class VirtualInputSlider : VirtualInputButton
{
	private const string SELECT_AXIS_NAME = "SelectHorizontal";

	private const float DEADZONE_VALUE = 0.6f;

	private Slider _slider;

	private Player _player;

	private float _stepValue;

	private float _sliderChange;

	public new void Awake()
	{
		base.Awake();
		_slider = GetComponent<Slider>();
		_player = ReInput.players.GetPlayer(0);
		_stepValue = _slider.maxValue - _slider.minValue;
	}

	private void Update()
	{
		ProcessInput();
	}

	private void ProcessInput()
	{
		if (gameObject == EventSystem.current.currentSelectedGameObject)
		{
			_sliderChange = _player.GetAxis("SelectHorizontal");
			if (Mathf.Abs(_sliderChange) > 0.6f)
			{
				_slider.value += _sliderChange * _stepValue * Time.unscaledDeltaTime;
				_slider.onValueChanged.Invoke(_slider.value);
				_sliderChange = 0f;
			}
		}
	}
}
