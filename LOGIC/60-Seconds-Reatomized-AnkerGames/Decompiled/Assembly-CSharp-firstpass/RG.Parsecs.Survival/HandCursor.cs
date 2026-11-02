using System.Collections;
using System.Collections.Generic;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.Scavenge;
using Rewired;
using Rewired.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class HandCursor : Singleton<HandCursor>
{
	[SerializeField]
	private bool _isMouseCursorVisible;

	[SerializeField]
	private Rewired.Components.PlayerMouse _playerMouse;

	private bool _handSpriteEnabled = true;

	private bool _isInputBlocked;

	[SerializeField]
	private RectTransform _rectTransform;

	[SerializeField]
	private Image _image;

	[SerializeField]
	private CharacterSet allCharacters;

	[SerializeField]
	private GlobalIntVariable previousCaptain;

	private Vector2 _sizeOnScreen = Vector2.zero;

	private float _nothingSelectedLimit = 0.15f;

	private float _nothingSelectedTimer;

	private Vector3 previousMousePos = Vector3.zero;

	private Rigidbody2D _previousHoverTarget;

	private List<GamepadPanel> _panelsToAdd = new List<GamepadPanel>();

	private List<GamepadPanel> _panelsToRemove = new List<GamepadPanel>();

	private List<GamepadPanel> _panels = new List<GamepadPanel>();

	public bool MouseCursorVisible
	{
		get
		{
			return _isMouseCursorVisible;
		}
		set
		{
			_isMouseCursorVisible = value;
			if (value && _handSpriteEnabled)
			{
				_image.enabled = value;
			}
			if (!value)
			{
				_image.enabled = value;
			}
			if (value)
			{
				Cursor.lockState = CursorLockMode.None;
			}
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
	}

	public bool HandSpriteVisible
	{
		get
		{
			return _handSpriteEnabled;
		}
		set
		{
			_handSpriteEnabled = value;
			if (value && _isMouseCursorVisible)
			{
				_image.enabled = value;
			}
			if (!value)
			{
				_image.enabled = value;
			}
		}
	}

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
				if (!isPointerMode)
				{
					HandSpriteVisible = false;
				}
			}
			else if (!isPointerMode)
			{
				SelectPriorityButton();
			}
		}
	}

	public bool isGamepadMode { get; protected set; }

	public bool isGamepadPointerMode { get; protected set; }

	public bool isPointerMode
	{
		get
		{
			if (!isGamepadMode)
			{
				return true;
			}
			return isGamepadPointerMode;
		}
	}

	public static Vector2 SizeOnScreen
	{
		get
		{
			if (Singleton<HandCursor>.Instance._sizeOnScreen == Vector2.zero)
			{
				Vector3[] array = new Vector3[4];
				Singleton<HandCursor>.Instance._rectTransform.GetWorldCorners(array);
				Singleton<HandCursor>.Instance._sizeOnScreen = new Vector2(Vector3.Distance(array[1], array[2]), Vector3.Distance(array[0], array[1]));
			}
			return Singleton<HandCursor>.Instance._sizeOnScreen;
		}
	}

	public bool IsPointerOverGameObject()
	{
		if (Singleton<HandCursor>.Instance.isGamepadMode && !Singleton<HandCursor>.Instance.isGamepadPointerMode)
		{
			return false;
		}
		return EventSystem.current.IsPointerOverGameObject();
	}

	internal void DisablePanels()
	{
		for (int i = 0; i < _panels.Count; i++)
		{
			_panels[i].Interactable = false;
		}
		_panels.Clear();
		_panelsToAdd.Clear();
	}

	private new void Awake()
	{
		base.Awake();
		Cursor.visible = false;
	}

	public void RefreshCaptainCursor()
	{
		Character character = null;
		character = ((previousCaptain.Value != -1) ? allCharacters.GetCharacter(previousCaptain.Value) : allCharacters.AllCharacters.RandomElement());
		if (character != null)
		{
			_image.sprite = character.StaticData.Icons.CursorIcon;
		}
	}

	public Vector3 GetMousePosition()
	{
		return _playerMouse.screenPosition;
	}

	private void SelectPriorityButton()
	{
		GamepadPanel[] array = Object.FindObjectsOfType<GamepadPanel>();
		foreach (GamepadPanel gamepadPanel in array)
		{
			if (gamepadPanel.Interactable)
			{
				gamepadPanel.SelectPriorityButton();
				break;
			}
		}
	}

	public void OnScreenPositionChanged(Vector2 screenPosition)
	{
		if (_playerMouse.enabled)
		{
			SetCursorPosition(screenPosition);
		}
	}

	internal void SubscribePlayerMouse(Rewired.Components.PlayerMouse playerMouse)
	{
		unsubscribeFromPlayerMouse();
		_playerMouse = playerMouse;
		_playerMouse.ScreenPositionChangedEvent += OnScreenPositionChanged;
		_playerMouse.ButtonStateChangedEvent += OnButtonStateChangedEvent;
	}

	private void OnButtonStateChangedEvent(int index, bool pressed)
	{
		if (IsGamepadInputBlocked || isGamepadPointerMode || !isGamepadMode)
		{
			return;
		}
		ReInput.players.GetPlayer(0);
		bool flag = false;
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
		{
			iTween.Stop(_rectTransform.gameObject);
			_playerMouse.screenPosition = Input.mousePosition;
			flag = true;
		}
		if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
		{
			StartCoroutine(EnterMouseModeNextFrame());
			flag = true;
		}
		if (!flag && pressed)
		{
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject != null)
			{
				currentSelectedGameObject.SendMessage("OnSubmit", new BaseEventData(EventSystem.current));
			}
		}
	}

	private void unsubscribeFromPlayerMouse()
	{
		if (_playerMouse != null)
		{
			_playerMouse.ScreenPositionChangedEvent -= OnScreenPositionChanged;
			_playerMouse.ButtonStateChangedEvent -= OnButtonStateChangedEvent;
		}
	}

	public void EnterGamepadMode()
	{
		isGamepadMode = true;
		isGamepadPointerMode = false;
	}

	private void Update()
	{
		if (IsGamepadInputBlocked)
		{
			return;
		}
		if (!isGamepadMode)
		{
			Player player = ReInput.players.GetPlayer(0);
			if ((player.GetButton("MouxeX") || player.GetButton("MouseY") || player.GetNegativeButton("MouxeX") || player.GetNegativeButton("MouseY")) && player.controllers.GetLastActiveController() is Joystick)
			{
				isGamepadMode = true;
				Singleton<HandCursor>.Instance.HandSpriteVisible = true;
				EventSystem.current.SetSelectedGameObject(null);
				isGamepadPointerMode = true;
				Debug.Log("Changed to gamepad pointer");
			}
			else if ((player.GetButton("UICancel") || player.GetButton("UIHorizontal") || player.GetButton("UIVertical") || player.GetNegativeButton("UIHorizontal") || player.GetNegativeButton("UIVertical")) && player.controllers.GetLastActiveController() is Joystick)
			{
				isGamepadMode = true;
				isGamepadPointerMode = false;
				SelectPriorityButton();
				Debug.Log("Changed to gamepad");
				return;
			}
		}
		else
		{
			Player player2 = ReInput.players.GetPlayer(0);
			if (!isGamepadPointerMode && EventSystem.current.currentSelectedGameObject == null && (player2.GetButton("UICancel") || player2.GetButton("UIHorizontal") || player2.GetButton("UIVertical") || player2.GetNegativeButton("UIHorizontal") || player2.GetNegativeButton("UIVertical")) && player2.controllers.GetLastActiveController() is Joystick)
			{
				SelectPriorityButton();
			}
			if (!isGamepadPointerMode && (player2.GetButton("MouseX") || player2.GetButton("MouseY") || player2.GetNegativeButton("MouseX") || player2.GetNegativeButton("MouseY")))
			{
				Singleton<HandCursor>.Instance.HandSpriteVisible = true;
				EventSystem.current.SetSelectedGameObject(null);
				isGamepadPointerMode = true;
				Debug.Log("Changed to gamepad pointer");
			}
			if (isGamepadPointerMode)
			{
				if (player2.GetButton("UICancel") || player2.GetButton("UIHorizontal") || player2.GetButton("UIVertical") || player2.GetNegativeButton("UIHorizontal") || player2.GetNegativeButton("UIVertical"))
				{
					HandleGamepadMouseExit();
					isGamepadPointerMode = false;
					SelectPriorityButton();
					Debug.Log("Changed to gamepad");
					return;
				}
				HandleMouseEvents();
			}
			HandlePanelInput(player2);
			if (previousMousePos == Vector3.zero)
			{
				previousMousePos = Input.mousePosition;
			}
			if ((Input.mousePosition - previousMousePos).magnitude > 2f)
			{
				EnterMouseMode();
			}
		}
		previousMousePos = Input.mousePosition;
	}

	public void RemoveSelection()
	{
		HandleGamepadMouseExit();
		if (GamepadPointerInputModule.I != null)
		{
			GamepadPointerInputModule.I.ForceMouseExit();
		}
		EventSystem.current.SetSelectedGameObject(null);
	}

	private IEnumerator EnterMouseModeNextFrame()
	{
		yield return null;
		EnterMouseMode();
	}

	private void EnterMouseMode()
	{
		EventSystem.current.SetSelectedGameObject(null);
		iTween.Stop(_rectTransform.gameObject);
		_playerMouse.screenPosition = Input.mousePosition;
		isGamepadMode = false;
		isGamepadPointerMode = false;
		HandSpriteVisible = true;
		Debug.Log("Changed to mouse");
	}

	private void HandlePanelInput(Player player)
	{
		for (int i = 0; i < _panelsToAdd.Count; i++)
		{
			GamepadPanel item = _panelsToAdd[i];
			if (!_panels.Contains(item))
			{
				_panels.Add(item);
			}
		}
		for (int j = 0; j < _panelsToRemove.Count; j++)
		{
			GamepadPanel item2 = _panelsToRemove[j];
			_panels.Remove(item2);
		}
		if (player.GetButtonDown(9))
		{
			for (int k = 0; k < _panels.Count; k++)
			{
				_panels[k].Cancel();
			}
		}
		if (player.GetButtonDown(20))
		{
			for (int l = 0; l < _panels.Count; l++)
			{
				_panels[l].LB();
			}
		}
		if (player.GetButtonDown(21))
		{
			for (int m = 0; m < _panels.Count; m++)
			{
				_panels[m].RB();
			}
		}
		if (player.GetButtonDown(22))
		{
			for (int n = 0; n < _panels.Count; n++)
			{
				_panels[n].LT();
			}
		}
		if (player.GetButtonDown(23))
		{
			for (int num = 0; num < _panels.Count; num++)
			{
				_panels[num].RT();
			}
		}
	}

	public bool OverMe(GameObject gameObject)
	{
		if (_previousHoverTarget == null)
		{
			return false;
		}
		return _previousHoverTarget.gameObject == gameObject;
	}

	private void HandleGamepadMouseExit()
	{
		if (_previousHoverTarget != null)
		{
			_previousHoverTarget.SendMessage("OnMouseExit");
		}
		_previousHoverTarget = null;
	}

	private void HandleGamepadMouseEnter(Rigidbody2D target)
	{
		HandleGamepadMouseExit();
		_previousHoverTarget = target;
		_previousHoverTarget.SendMessage("OnMouseEnter");
	}

	private void HandleMouseEvents()
	{
		if (_playerMouse == null)
		{
			return;
		}
		Camera main = Camera.main;
		if (main == null)
		{
			return;
		}
		Collider2D collider2D = Physics2D.OverlapPoint(main.ScreenToWorldPoint(_playerMouse.screenPosition));
		if (collider2D != null)
		{
			Rigidbody2D componentInParent = collider2D.GetComponentInParent<Rigidbody2D>();
			if (componentInParent != null)
			{
				if (_playerMouse.leftButton.justPressed)
				{
					componentInParent.SendMessage("OnMouseUpAsButton");
					componentInParent.SendMessage("OnMouseDown");
				}
				if (_playerMouse.leftButton.justReleased)
				{
					componentInParent.SendMessage("OnMouseUp");
				}
				if (_previousHoverTarget != componentInParent)
				{
					HandleGamepadMouseEnter(componentInParent);
				}
				else
				{
					componentInParent.SendMessage("OnMouseOver");
				}
			}
		}
		else
		{
			HandleGamepadMouseExit();
		}
	}

	public void RegisterGamepadPanel(GamepadPanel panel)
	{
		_panelsToAdd.Add(panel);
	}

	public void UnRegisterGamepadPanel(GamepadPanel panel)
	{
		_panelsToRemove.Remove(panel);
	}

	public bool GetMouseButtonUp(int index)
	{
		if (isGamepadPointerMode)
		{
			return index switch
			{
				0 => _playerMouse.leftButton.justPressed, 
				1 => _playerMouse.rightButton.justPressed, 
				_ => false, 
			};
		}
		return index switch
		{
			0 => _playerMouse.leftButton.justReleased, 
			1 => _playerMouse.rightButton.justReleased, 
			_ => false, 
		};
	}

	public void SetMousePosition(Vector3 pos, bool tween = false)
	{
		if (tween)
		{
			iTween.MoveTo(_rectTransform.gameObject, iTween.Hash("position", pos, "time", 0.7f, "ignoretimescale", true));
		}
		else
		{
			_rectTransform.position = pos;
		}
		_playerMouse.screenPosition = pos;
	}

	public void SetCursorPosition(Vector3 pos)
	{
		_rectTransform.position = pos;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		unsubscribeFromPlayerMouse();
	}
}
