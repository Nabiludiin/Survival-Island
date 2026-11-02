using System.Collections.Generic;
using RG.Parsecs.Common;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RG.VirtualInput;

public class SelectablesInputModule : StandaloneInputModule, IVirtualInputMode
{
	private const string SELECT_VERTICAL_AXIS_NAME = "SelectVertical";

	private const string SELECT_HORIZONTAL_AXIS_NAME = "SelectHorizontal";

	private const string SUBMIT_BUTTON_NAME = "Confirm";

	[SerializeField]
	private EventSystem _eventSystem;

	private PointerEventData _pointerEvent;

	private Vector2 _cursorPosition;

	[SerializeField]
	private GameObject _cursorObject;

	[SerializeField]
	private float _tweeningTime = 0.6f;

	private Player _player;

	private Rigidbody2D _previousMouseHoverTarget;

	private Vector3 _previousFrameTargetPosition = Vector3.zero;

	private Camera _mainCamera;

	protected override void Awake()
	{
		base.Awake();
		_player = ReInput.players.GetPlayer(0);
	}

	protected override void Start()
	{
		base.Start();
		_pointerEvent = new PointerEventData(_eventSystem);
	}

	public void OnFinishMode()
	{
		HandleMouseExit();
		_eventSystem.SetSelectedGameObject(null);
		enabled = false;
	}

	public void OnStartMode()
	{
		enabled = true;
		SelectPriorityButton(selectClosest: true);
	}

	public override void Process()
	{
		if (ShouldProcess())
		{
			SendUpdateEventToSelectedObject();
			UpdateCursorMovement();
			UpdateCursorPosition();
			HandleMouseEvents();
		}
	}

	public void UpdateCursorPosition()
	{
		if (VirtualInputButton.SelectedVirtualInputButton != null)
		{
			if (iTween.Count(_cursorObject) == 0)
			{
				_cursorObject.transform.position = VirtualInputButton.SelectedVirtualInputButton.GetPosition();
			}
			else if (VirtualInputButton.SelectedVirtualInputButton.GetPosition() != _previousFrameTargetPosition)
			{
				SetMousePosition(VirtualInputButton.SelectedVirtualInputButton.GetPosition(), useTweening: false);
			}
			_cursorPosition = _cursorObject.transform.position;
			_pointerEvent.position = _cursorPosition;
		}
	}

	private void UpdateCursorMovement()
	{
		if (VirtualInputButton.SelectedVirtualInputButton == null)
		{
			return;
		}
		Selectable selectable = null;
		if (_player.GetButtonDown("SelectVertical"))
		{
			selectable = VirtualInputButton.SelectedVirtualInputButton.Selectable.FindSelectableOnUp();
		}
		if (_player.GetNegativeButtonDown("SelectVertical"))
		{
			selectable = VirtualInputButton.SelectedVirtualInputButton.Selectable.FindSelectableOnDown();
		}
		if (_player.GetButtonDown("SelectHorizontal"))
		{
			selectable = VirtualInputButton.SelectedVirtualInputButton.Selectable.FindSelectableOnRight();
		}
		if (_player.GetNegativeButtonDown("SelectHorizontal"))
		{
			selectable = VirtualInputButton.SelectedVirtualInputButton.Selectable.FindSelectableOnLeft();
		}
		if (selectable != null)
		{
			VirtualInputButton component = selectable.GetComponent<VirtualInputButton>();
			if ((bool)component && component.IsInteractable)
			{
				selectable.Select();
			}
		}
		if (_player.GetButtonDown("Confirm"))
		{
			_pointerEvent.pointerPress = ExecuteEvents.ExecuteHierarchy(VirtualInputButton.SelectedVirtualInputButton.gameObject, _pointerEvent, ExecuteEvents.submitHandler);
		}
	}

	public void SetMousePosition(Vector3 pos, bool useTweening)
	{
		iTween.Stop(_cursorObject);
		if (useTweening)
		{
			iTween.MoveTo(_cursorObject, iTween.Hash("position", pos, "time", _tweeningTime, "ignoretimescale", true));
			_previousFrameTargetPosition = pos;
		}
		else
		{
			_cursorObject.transform.position = pos;
		}
	}

	public Vector3 GetMousePosition()
	{
		return _cursorObject.transform.position;
	}

	public void SelectPriorityButton(bool selectClosest = false)
	{
		VirtualInputPanel[] array = Object.FindObjectsOfType<VirtualInputPanel>();
		foreach (VirtualInputPanel virtualInputPanel in array)
		{
			if (virtualInputPanel.Interactable)
			{
				if (selectClosest)
				{
					virtualInputPanel.SelectClosestButton();
				}
				else
				{
					virtualInputPanel.SelectPriorityButton();
				}
				break;
			}
		}
	}

	public bool IsPointerOverGameObject()
	{
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(_pointerEvent, list);
		return list.Count > 0;
	}

	public bool IsAnyInputActive()
	{
		if (_player.GetAxis("SelectHorizontal") == 0f)
		{
			return _player.GetAxis("SelectVertical") != 0f;
		}
		return true;
	}

	public bool ShouldProcess()
	{
		if (Singleton<VirtualInputManager>.Instance.IsSelectablesMode())
		{
			return !Singleton<VirtualInputManager>.Instance.IsGamepadInputBlocked;
		}
		return false;
	}

	public void HandleMouseExit()
	{
		if (_previousMouseHoverTarget != null)
		{
			_previousMouseHoverTarget.SendMessage("OnMouseExit");
		}
		_previousMouseHoverTarget = null;
	}

	private void HandleMouseEnter(Rigidbody2D target)
	{
		HandleMouseExit();
		_previousMouseHoverTarget = target;
		_previousMouseHoverTarget.SendMessage("OnMouseEnter");
	}

	private void HandleMouseEvents()
	{
		if (_mainCamera == null)
		{
			_mainCamera = Camera.main;
		}
		if (_mainCamera == null)
		{
			return;
		}
		Collider2D collider2D = Physics2D.OverlapPoint(_mainCamera.ScreenToWorldPoint(new Vector3(_cursorPosition.x, _cursorPosition.y, _mainCamera.transform.position.z * -1f)));
		if (collider2D != null)
		{
			Rigidbody2D componentInParent = collider2D.GetComponentInParent<Rigidbody2D>();
			if (componentInParent != null)
			{
				if (_player.GetButtonDown("Confirm"))
				{
					componentInParent.SendMessage("OnMouseUpAsButton");
					componentInParent.SendMessage("OnMouseDown");
				}
				if (_player.GetButtonUp("Confirm"))
				{
					componentInParent.SendMessage("OnMouseUp");
				}
				if (_previousMouseHoverTarget != componentInParent)
				{
					HandleMouseEnter(componentInParent);
				}
			}
		}
		else
		{
			HandleMouseExit();
		}
	}
}
