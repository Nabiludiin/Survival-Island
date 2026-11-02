using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.SecondsRemaster.Menu;

public class FlashesSettingController : MonoBehaviour
{
	[SerializeField]
	private Animator _knobAnimator;

	[SerializeField]
	private GlobalBoolVariable _flashesDisabled;

	private static readonly int Right = Animator.StringToHash("Right");

	private const string NO_KNOB_STATE = "Value_Knob_Left";

	private const string YES_KNOB_STATE = "Value_Knob_Right";

	private const string RIGHT_PARAM_NAME = "Right";

	private void OnEnable()
	{
		_knobAnimator.Play(_flashesDisabled.Value ? "Value_Knob_Right" : "Value_Knob_Left");
		_knobAnimator.SetBool(Right, _flashesDisabled.Value);
	}

	public void SwitchFlashes()
	{
		SetFlashes(!_flashesDisabled.Value);
	}

	public void SetFlashes(bool value)
	{
		_flashesDisabled.Value = value;
		_knobAnimator.SetBool(Right, value);
	}
}
