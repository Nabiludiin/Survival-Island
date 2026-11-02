using System;
using System.Runtime.Serialization;

[Serializable]
public class RGD_Settings_Controls : SerializableBehaviour
{
	public float cursorSensitivity;

	public float stickSensitivity;

	public float mouseSensitivity;

	public bool invertMouseY;

	public bool invertStickY;

	public bool toggleRun;

	public bool toggleCrouch;

	public float vibrationStrength;

	public Keybind[] keybinds;

	public RGD_Settings_Controls()
	{
		SetDefaults(default);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("CursorSensitivity", cursorSensitivity);
		info.AddValue("StickSensitivity", stickSensitivity);
		info.AddValue("MouseSensitivity", mouseSensitivity);
		info.AddValue("InvertMouseY", invertMouseY);
		info.AddValue("InvertStickY", invertStickY);
		info.AddValue("Keybinds", keybinds);
		info.AddValue("ToggleRun", toggleRun);
		info.AddValue("ToggleCrouch", toggleCrouch);
		info.AddValue("VibrationStrength", vibrationStrength);
	}

	public RGD_Settings_Controls(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "CursorSensitivity":
				cursorSensitivity = (float)item.Value;
				break;
			case "StickSensitivity":
				stickSensitivity = (float)item.Value;
				break;
			case "MouseSensitivity":
				mouseSensitivity = (float)item.Value;
				break;
			case "InvertMouseY":
				invertMouseY = (bool)item.Value;
				break;
			case "InvertStickY":
				invertStickY = (bool)item.Value;
				break;
			case "Keybinds":
				keybinds = (Keybind[])item.Value;
				break;
			case "ToggleRun":
				toggleRun = (bool)item.Value;
				break;
			case "ToggleCrouch":
				toggleCrouch = (bool)item.Value;
				break;
			case "VibrationStrength":
				vibrationStrength = (float)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		cursorSensitivity = 50f;
		stickSensitivity = 50f;
		mouseSensitivity = 1f;
		vibrationStrength = 10f;
		invertMouseY = false;
		invertStickY = false;
		keybinds = null;
		toggleRun = false;
		toggleCrouch = true;
	}
}
