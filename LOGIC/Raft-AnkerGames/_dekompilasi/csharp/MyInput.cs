using System.Collections.Generic;
using UnityEngine;

public class MyInput : MonoBehaviour
{
	public static Dictionary<string, Keybind> Keybinds = new Dictionary<string, Keybind>();

	public static bool CanGetInput => CanvasHelper.ActiveMenu != MenuType.PauseMenu;

	public static bool GetButtonDown(string identifier)
	{
		if (!CanGetInput)
		{
			return false;
		}
		Keybind keybind = IdentifierToKeybind(identifier);
		if (keybind == null)
		{
			return false;
		}
		if (keybind.MainKey == KeyCode.None || !Input.GetKeyDown(keybind.MainKey))
		{
			if (keybind.AltKey != KeyCode.None)
			{
				return Input.GetKeyDown(keybind.AltKey);
			}
			return false;
		}
		return true;
	}

	public static bool GetButtonUp(string identifier)
	{
		if (!CanGetInput)
		{
			return false;
		}
		Keybind keybind = IdentifierToKeybind(identifier);
		if (keybind == null)
		{
			return false;
		}
		if (keybind.MainKey == KeyCode.None || !Input.GetKeyUp(keybind.MainKey))
		{
			if (keybind.AltKey != KeyCode.None)
			{
				return Input.GetKeyUp(keybind.AltKey);
			}
			return false;
		}
		return true;
	}

	public static bool GetButton(string identifier)
	{
		if (!CanGetInput)
		{
			return false;
		}
		Keybind keybind = IdentifierToKeybind(identifier);
		if (keybind == null)
		{
			return false;
		}
		if (keybind.MainKey == KeyCode.None || !Input.GetKey(keybind.MainKey))
		{
			if (keybind.AltKey != KeyCode.None)
			{
				return Input.GetKey(keybind.AltKey);
			}
			return false;
		}
		return true;
	}

	public static float GetAxis(string identifier)
	{
		if (!CanGetInput)
		{
			return 0f;
		}
		Keybind keybind = IdentifierToKeybind(identifier);
		if (keybind == null)
		{
			return 0f;
		}
		if (keybind.MainKey != KeyCode.None && Input.GetKey(keybind.MainKey))
		{
			return 1f;
		}
		if (keybind.AltKey != KeyCode.None && Input.GetKey(keybind.AltKey))
		{
			return -1f;
		}
		return 0f;
	}

	private static Keybind IdentifierToKeybind(string identifier)
	{
		if (Keybinds == null || Keybinds.Count <= 0)
		{
			return null;
		}
		if (Keybinds.ContainsKey(identifier))
		{
			return Keybinds[identifier];
		}
		Debug.LogWarning("Can't find keybind with identifier [" + identifier + "]");
		return null;
	}
}
