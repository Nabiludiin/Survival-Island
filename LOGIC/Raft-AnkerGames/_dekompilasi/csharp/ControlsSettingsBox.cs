using UnityEngine;
using UnityEngine.UI;

public class ControlsSettingsBox : OptionsMenuBox
{
	public static float DefaultMouseSensitivity;

	public static float DefaultLookSensitivity;

	[Header("Components")]
	[SerializeField]
	private Slider sensitivitySlider;

	[SerializeField]
	private Text sensitivityText;

	[SerializeField]
	private Slider cursorSlider;

	[SerializeField]
	private Text cursorText;

	[SerializeField]
	private Slider lookSlider;

	[SerializeField]
	private Text lookText;

	[SerializeField]
	private Slider vibrationSlider;

	[SerializeField]
	private Text vibrationText;

	[SerializeField]
	private Toggle mouseInvertYToggle;

	[SerializeField]
	private Toggle lookInvertYToggle;

	[SerializeField]
	private Toggle toggleRunToggle;

	[SerializeField]
	private Toggle toggleCrouch;

	[SerializeField]
	private ColorBlock colorblock;

	[SerializeField]
	private RectTransform contentBox;

	private KeybindInterface[] keybindInterfaces;

	public float MouseSensitivity
	{
		get
		{
			return sensitivitySlider.value;
		}
		set
		{
			sensitivitySlider.value = value;
		}
	}

	public float CursorSensitivity
	{
		get
		{
			return cursorSlider.value / 10f;
		}
		set
		{
			cursorSlider.value = value * 10f;
		}
	}

	public float LookSensitivity
	{
		get
		{
			return lookSlider.value / 10f;
		}
		set
		{
			lookSlider.SetValueWithoutNotify(value * 10f);
		}
	}

	public bool InvertMouseY => mouseInvertYToggle.isOn;

	public bool InvertLookY => lookInvertYToggle.isOn;

	public bool RunToggle => toggleRunToggle.isOn;

	public bool CrouchToggle => toggleCrouch.isOn;

	public float VibrationStrength => vibrationSlider.value;

	public override BoxVisibility GetBoxVisibility()
	{
		return BoxVisibility.Pc;
	}

	public override void SetFocus(bool gamepad)
	{
		Vector2 sizeDelta = contentBox.sizeDelta;
		sizeDelta.y = (gamepad ? 411 : 2684);
		contentBox.sizeDelta = sizeDelta;
		sensitivitySlider.Select();
	}

	private void Start()
	{
		InitializeKeybindInterfaces();
	}

	public override void Save(RGD_Settings settings)
	{
		base.Save(settings);
		RGD_Settings_Controls controls = settings.controls;
		controls.cursorSensitivity = cursorSlider.value;
		controls.mouseSensitivity = sensitivitySlider.value;
		controls.stickSensitivity = lookSlider.value;
		controls.invertMouseY = mouseInvertYToggle.isOn;
		controls.invertStickY = lookInvertYToggle.isOn;
		controls.toggleRun = toggleRunToggle.isOn;
		controls.toggleCrouch = toggleCrouch.isOn;
		controls.vibrationStrength = vibrationSlider.value;
		controls.keybinds = new Keybind[keybindInterfaces.Length];
		for (int i = 0; i < controls.keybinds.Length; i++)
		{
			KeybindInterface keybindInterface = keybindInterfaces[i];
			controls.keybinds[i] = keybindInterface.Keybind;
		}
	}

	public override void Load(RGD_Settings settings)
	{
		base.Load(settings);
		RGD_Settings_Controls controls = settings.controls;
		mouseInvertYToggle.isOn = controls.invertMouseY;
		lookInvertYToggle.isOn = controls.invertStickY;
		sensitivitySlider.value = controls.mouseSensitivity;
		OnSensitivitySliderChange();
		cursorSlider.value = controls.cursorSensitivity;
		OnCursorSliderChange();
		lookSlider.value = controls.stickSensitivity;
		OnLookSliderChange();
		vibrationSlider.value = controls.vibrationStrength;
		vibrationText.text = vibrationSlider.value.ToString("F1");
		toggleRunToggle.isOn = controls.toggleRun;
		toggleCrouch.isOn = controls.toggleCrouch;
		InitializeKeybindInterfaces();
		MyInput.Keybinds.Clear();
		for (int i = 0; i < keybindInterfaces.Length; i++)
		{
			KeybindInterface keybindInterface = keybindInterfaces[i];
			bool flag = false;
			if (controls.keybinds != null)
			{
				for (int j = 0; j < controls.keybinds.Length; j++)
				{
					Keybind keybind = controls.keybinds[j];
					if (keybindInterface.Identifier == keybind.Identifier)
					{
						flag = true;
						keybindInterface.Set(keybind);
						break;
					}
				}
			}
			if (!flag)
			{
				keybindInterface.SetDefaultKeys();
			}
			MyInput.Keybinds.Add(keybindInterface.Identifier, keybindInterface.Keybind);
		}
	}

	public void SetKeybindsToDefaultValue()
	{
		MyInput.Keybinds.Clear();
		for (int i = 0; i < keybindInterfaces.Length; i++)
		{
			KeybindInterface keybindInterface = keybindInterfaces[i];
			keybindInterface.SetDefaultKeys();
			MyInput.Keybinds.Add(keybindInterface.Identifier, keybindInterface.Keybind);
		}
	}

	private void InitializeKeybindInterfaces()
	{
		keybindInterfaces = GetComponentsInChildren<KeybindInterface>(includeInactive: true);
		for (int i = 0; i < keybindInterfaces.Length; i++)
		{
			keybindInterfaces[i].Initialize(colorblock);
		}
	}

	public void OnSensitivitySliderChange()
	{
		sensitivityText.text = sensitivitySlider.value.ToString("F1");
		DefaultMouseSensitivity = sensitivitySlider.value;
	}

	public void OnCursorSliderChange()
	{
		cursorText.text = (cursorSlider.value / 10f).ToString("F1");
	}

	public void OnLookSliderChange()
	{
		lookText.text = (lookSlider.value / 10f).ToString("F1");
		DefaultLookSensitivity = lookSlider.value / 10f;
	}

	public void OnVibrationSliderChange()
	{
		vibrationText.text = vibrationSlider.value.ToString("F1");
		SimpleMonoBehaviourSingleton<HapticManager>.Instance.PlayOneShot(HapticEvent.Build);
	}
}
