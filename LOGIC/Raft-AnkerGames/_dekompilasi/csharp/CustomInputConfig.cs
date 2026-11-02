using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class CustomInputConfig : SimpleMonoBehaviourSingleton<CustomInputConfig>
{
	[SerializeField]
	private InputSystemUIInputModule inputModule;

	[SerializeField]
	private PlayerInput playerInput;

	private string currentActionMap = "UI";

	public Action<bool> OnControlsChangedEvent;

	public bool Gamepad => PlayerInput.currentControlScheme == "Gamepad";

	public PlayerInput PlayerInput => playerInput;

	public void OnEnable()
	{
		Input.simulateMouseWithTouches = false;
	}

	public void Update()
	{
		Cursor.visible = !Gamepad && Helper.CursorVisible;
	}

	public void OnControlsChanged(PlayerInput playerInput)
	{
		OnControlsChangedEvent?.Invoke(Gamepad);
		if ((bool)ComponentManager<CanvasHelper>.Value)
		{
			ComponentManager<CanvasHelper>.Value.OnControlsChanged(Gamepad);
		}
	}

	public void EnableInput()
	{
		if (!inputModule.enabled)
		{
			inputModule.SetEnabledSafe(value: true);
			playerInput.SwitchCurrentActionMap(currentActionMap);
		}
	}

	public void DisableInput()
	{
		if (inputModule.enabled)
		{
			inputModule.SetEnabledSafe(value: false);
			playerInput.SwitchCurrentActionMap("NoInput");
		}
	}

	public void SwitchCurrentActionMap(string actionMap)
	{
		if (inputModule.enabled)
		{
			if (currentActionMap != actionMap)
			{
				currentActionMap = actionMap;
				playerInput.SwitchCurrentActionMap(actionMap);
			}
		}
		else
		{
			currentActionMap = actionMap;
		}
	}

	public InputAction GetAction(string uiName, string playerName)
	{
		if (playerInput.currentActionMap.name == "Player")
		{
			return playerInput.actions[playerName];
		}
		return playerInput.actions[uiName];
	}

	public bool WasPressedThisFrame(InputAction action, string key)
	{
		if (Gamepad)
		{
			return action.WasPressedThisFrame();
		}
		return MyInput.GetButtonDown(key);
	}

	public bool WasPressedThisFrame(InputAction action, KeyCode key)
	{
		if (Gamepad)
		{
			return action.WasPressedThisFrame();
		}
		return Input.GetKeyDown(key);
	}

	public bool WasReleasedThisFrame(InputAction action, string key)
	{
		if (Gamepad)
		{
			return action.WasReleasedThisFrame();
		}
		return MyInput.GetButtonUp(key);
	}

	public bool IsPressed(InputAction action, string key)
	{
		if (Gamepad)
		{
			return action.IsPressed();
		}
		return MyInput.GetButton(key);
	}

	public bool IsPressed(InputAction action, KeyCode key)
	{
		if (Gamepad)
		{
			return action.IsPressed();
		}
		return Input.GetKey(key);
	}

	public float GetXAxis(InputAction action, string key)
	{
		if (Gamepad)
		{
			return action.ReadValue<Vector2>().x;
		}
		return Input.GetAxis(key);
	}
}
