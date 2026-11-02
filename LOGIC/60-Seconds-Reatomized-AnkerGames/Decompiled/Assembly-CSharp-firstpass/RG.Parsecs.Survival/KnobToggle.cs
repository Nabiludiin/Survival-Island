using RG.Parsecs.Common;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class KnobToggle : ToggleController
{
	[SerializeField]
	private KnobController _knobController;

	[SerializeField]
	private GoalsGlowController _goalsGlowController;

	[SerializeField]
	private Slider _slider;

	[SerializeField]
	private int _knobIndex;

	public override void OnToggleValueChangedAction(bool toggleValue)
	{
		if (toggleValue)
		{
			if (_goalsGlowController != null)
			{
				_goalsGlowController.Hide();
			}
			_knobController.OnStarlogButtonPressed(_knobIndex);
			_slider.value = 0f;
		}
	}
}
