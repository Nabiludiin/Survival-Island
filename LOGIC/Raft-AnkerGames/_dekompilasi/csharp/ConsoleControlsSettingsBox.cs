using UnityEngine;
using UnityEngine.UI;

public class ConsoleControlsSettingsBox : OptionsMenuBox
{
	public float DefaultLookSensitivity;

	[SerializeField]
	private Slider cursorSlider;

	[SerializeField]
	private Text cursorText;

	[SerializeField]
	private Slider sensitivitySlider;

	[SerializeField]
	private Text sensitivityText;

	[SerializeField]
	private Toggle lookInvertYToggle;

	[SerializeField]
	private Toggle toggleRunToggle;

	[SerializeField]
	private Toggle toggleCrouch;

	public float CursorSensitivity
	{
		get
		{
			return cursorSlider.value;
		}
		set
		{
			cursorSlider.value = value;
		}
	}

	public float LookSensitivity
	{
		get
		{
			return sensitivitySlider.value;
		}
		set
		{
			sensitivitySlider.SetValueWithoutNotify(value);
		}
	}

	public bool InvertLookY => lookInvertYToggle.isOn;

	public bool RunToggle => toggleRunToggle.isOn;

	public bool CrouchToggle => toggleCrouch.isOn;

	public override void SetFocus(bool gamepad)
	{
		cursorSlider.Select();
	}

	public override BoxVisibility GetBoxVisibility()
	{
		return BoxVisibility.Console;
	}

	public override void SettingStart()
	{
	}

	public override void Save(RGD_Settings settings)
	{
		base.Save(settings);
		settings.controls.cursorSensitivity = cursorSlider.value;
		settings.controls.stickSensitivity = sensitivitySlider.value;
		settings.controls.invertStickY = lookInvertYToggle.isOn;
		settings.controls.toggleRun = toggleRunToggle.isOn;
		settings.controls.toggleCrouch = toggleCrouch.isOn;
	}

	public override void Load(RGD_Settings settings)
	{
		base.Load(settings);
		cursorSlider.value = settings.controls.cursorSensitivity;
		OnCursorSliderChange();
		sensitivitySlider.value = settings.controls.stickSensitivity;
		OnSensitivitySliderChange();
		lookInvertYToggle.isOn = settings.controls.invertStickY;
		toggleRunToggle.isOn = settings.controls.toggleRun;
		toggleCrouch.isOn = settings.controls.toggleCrouch;
	}

	public void OnSensitivitySliderChange()
	{
		sensitivityText.text = sensitivitySlider.value.ToString("F1");
		DefaultLookSensitivity = sensitivitySlider.value;
	}

	public void OnCursorSliderChange()
	{
		cursorText.text = cursorSlider.value.ToString("F1");
	}
}
