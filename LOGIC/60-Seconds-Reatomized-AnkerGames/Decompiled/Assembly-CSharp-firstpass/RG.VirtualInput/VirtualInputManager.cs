using System;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RG.VirtualInput;

public class VirtualInputManager : Singleton<VirtualInputManager>
{
	[SerializeField]
	private GameObject _virtualMousePrefab;

	[SerializeField]
	private GlobalIntVariable _controlMode;

	[NonSerialized]
	public VirtualMouseVisualManager VisualManager;

	private VirtualMousePointer _virtualMousePointer;

	private SelectablesInputModule _selectablesCursorTest;

	private RealMousePointer _realMouseMode;

	private IVirtualInputMode _currentInputHandler;

	private Player _player;

	private const int GAMEPAD_PLAYER_INPUT_VALUE = 3;

	private const int MOUSE_MAP_CATEGORY = 6;

	private const int KEYBOARD_MAP_CATEGORY = 7;

	private const int GAMEPAD_MAP_CATEGORY = 8;

	private const int MOUSEKEYBOARD_MAP_CATEGORY = 9;

	private int _beforeGamepadInputValue = 2;

	private bool _isInputBlocked;

	public bool IsGamepadInputBlocked
	{
		get
		{
			return _isInputBlocked;
		}
		set
		{
			_isInputBlocked = value;
			if (_isInputBlocked)
			{
				RemoveSelection();
				return;
			}
			if (IsSelectablesMode())
			{
				_selectablesCursorTest.SelectPriorityButton();
			}
			VirtualInputPausePanel[] array = UnityEngine.Object.FindObjectsOfType<VirtualInputPausePanel>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].GamepadInputUnlocked();
			}
		}
	}

	private void RemoveSelection()
	{
		if (IsPointerMode())
		{
			_virtualMousePointer.HandleMouseExit();
		}
		EventSystem.current.SetSelectedGameObject(null);
	}

	private void Start()
	{
		SpawnVirtualMouse();
		VisualManager.MouseCursorVisible = false;
		_player = ReInput.players.GetPlayer(0);
	}

	public void Update()
	{
		if (!IsGamepadInputBlocked)
		{
			DetectInputModeChange();
		}
	}

	private void SpawnVirtualMouse()
	{
		if (_virtualMousePrefab == null)
		{
			Debug.Log("Virtual mouse prefab is null");
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(_virtualMousePrefab, transform);
		VisualManager = gameObject.GetComponent<VirtualMouseVisualManager>();
		_virtualMousePointer = gameObject.GetComponent<VirtualMousePointer>();
		_selectablesCursorTest = gameObject.GetComponent<SelectablesInputModule>();
		_realMouseMode = gameObject.GetComponent<RealMousePointer>();
		ChangeInputMode(_virtualMousePointer);
		_selectablesCursorTest.enabled = false;
		_realMouseMode.enabled = false;
		EventSystem.current = gameObject.GetComponent<EventSystem>();
		Cursor.visible = false;
	}

	private void ChangeInputMode(IVirtualInputMode inputMode)
	{
		if (_currentInputHandler != null)
		{
			_currentInputHandler.OnFinishMode();
		}
		_currentInputHandler = inputMode;
		_currentInputHandler.OnStartMode();
	}

	public void SetMousePosition(Vector3 pos, bool useTweening)
	{
		if (_currentInputHandler is SelectablesInputModule)
		{
			_selectablesCursorTest.SetMousePosition(pos, useTweening);
		}
	}

	public Vector3 GetMousePosition()
	{
		return _currentInputHandler.GetMousePosition();
	}

	public bool IsPointerMode()
	{
		return _currentInputHandler.GetType() == typeof(VirtualMousePointer);
	}

	public bool IsSelectablesMode()
	{
		return _currentInputHandler is SelectablesInputModule;
	}

	public bool IsRealMouseMode()
	{
		return _currentInputHandler.GetType() == typeof(RealMousePointer);
	}

	public bool IsPointerOverGameObject()
	{
		return _currentInputHandler.IsPointerOverGameObject();
	}

	private void DetectInputModeChange()
	{
		if (_player.controllers.GetLastActiveController() is Joystick)
		{
			if (_virtualMousePointer.IsAnyInputActive())
			{
				if (!IsPointerMode())
				{
					ChangeInputMode(_virtualMousePointer);
					SwitchControlModeToGamepad();
				}
			}
			else if (_selectablesCursorTest.IsAnyInputActive() && !IsSelectablesMode())
			{
				ChangeInputMode(_selectablesCursorTest);
				SwitchControlModeToGamepad();
			}
		}
		else if (!IsRealMouseMode() && _realMouseMode.IsAnyInputActive())
		{
			ChangeInputMode(_realMouseMode);
			SwitchControlModeToMouseKeyboard();
		}
	}

	public bool OverMe(GameObject gameObjectToCheck)
	{
		return _virtualMousePointer.OverMe(gameObjectToCheck);
	}

	public void SetSelectablesMode()
	{
		ChangeInputMode(_selectablesCursorTest);
	}

	public void SetRealMouseMode()
	{
		ChangeInputMode(_realMouseMode);
	}

	public void RefreshSelectablesModeSelection()
	{
		if (IsSelectablesMode())
		{
			_selectablesCursorTest.SelectPriorityButton(selectClosest: true);
		}
	}

	public void SwitchControlModeToGamepad()
	{
		if (_controlMode.Value != 3)
		{
			_beforeGamepadInputValue = _controlMode.Value;
			_controlMode.Value = 3;
			_player.controllers.maps.SetMapsEnabled(state: true, 8);
		}
	}

	public void SwitchControlModeToMouseKeyboard()
	{
		if (_controlMode.Value != _beforeGamepadInputValue)
		{
			_player.controllers.maps.SetMapsEnabled(state: false, 7);
			_player.controllers.maps.SetMapsEnabled(state: false, 9);
			_controlMode.Value = _beforeGamepadInputValue;
			_player.controllers.maps.SetMapsEnabled(state: true, GetMapCategoryFromInputValue(_beforeGamepadInputValue));
			_player.controllers.maps.SetMapsEnabled(state: false, 8);
		}
	}

	private int GetMapCategoryFromInputValue(int inputMode)
	{
		return inputMode switch
		{
			1 => 7, 
			2 => 9, 
			3 => 8, 
			6 => 6, 
			_ => 0, 
		};
	}
}
