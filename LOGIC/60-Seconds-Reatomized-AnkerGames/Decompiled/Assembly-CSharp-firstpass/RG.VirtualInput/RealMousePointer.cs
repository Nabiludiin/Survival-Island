using System.Collections.Generic;
using RG.Parsecs.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RG.VirtualInput;

public class RealMousePointer : VirtualMousePointer
{
	private const string REAL_MOUSE_SUBMIT_BUTTON_NAME = "ConfirmMouse";

	private const string SCAVENGE_HORIZONTAL_NAME = "MoveHorizontal";

	private const string SCAVENGE_VERTICAL_NAME = "MoveVertical";

	private const string SCAVENGE_ROTATE_NAME = "Rotate";

	private List<RaycastResult> _raycastResultsCache = new List<RaycastResult>();

	protected override void UpdateCursorMovement()
	{
		_cursorObject.transform.position = Input.mousePosition;
		_cursorPosition.x = _cursorObject.transform.position.x;
		_cursorPosition.y = _cursorObject.transform.position.y;
	}

	public override bool ShouldProcess()
	{
		if (Singleton<VirtualInputManager>.Instance.IsRealMouseMode())
		{
			return !Singleton<VirtualInputManager>.Instance.IsGamepadInputBlocked;
		}
		return false;
	}

	public override void Process()
	{
		if (ShouldProcess())
		{
			CheckCamera();
			SendUpdateEventToSelectedObject();
			UpdateCursorMovement();
			ProcessMouseEvent();
		}
	}

	private new void ProcessMouseEvent()
	{
		MouseState mousePointerEventData = GetMousePointerEventData();
		bool pressed = mousePointerEventData.AnyPressesThisFrame();
		bool released = mousePointerEventData.AnyReleasesThisFrame();
		MouseButtonEventData eventData = mousePointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData;
		if (UseMouse(pressed, released, eventData.buttonData))
		{
			ProcessMousePress(eventData);
			ProcessMove(eventData.buttonData);
			ProcessDrag(eventData.buttonData);
			ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData);
			ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
			ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
			ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
			if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				ExecuteEvents.ExecuteHierarchy(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), eventData.buttonData, ExecuteEvents.scrollHandler);
			}
		}
	}

	private static bool UseMouse(bool pressed, bool released, PointerEventData pointerData)
	{
		if ((pressed | released) || pointerData.IsPointerMoving() || pointerData.IsScrolling())
		{
			return true;
		}
		return false;
	}

	private new void ProcessMousePress(MouseButtonEventData data)
	{
		PointerEventData buttonData = data.buttonData;
		GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
		if (data.PressedThisFrame())
		{
			buttonData.eligibleForClick = true;
			buttonData.delta = Vector2.zero;
			buttonData.dragging = false;
			buttonData.useDragThreshold = true;
			buttonData.pressPosition = buttonData.position;
			buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
			DeselectIfSelectionChanged(gameObject, buttonData);
			GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
			if (gameObject2 == null)
			{
				gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
			}
			float unscaledTime = Time.unscaledTime;
			if (gameObject2 == buttonData.lastPress)
			{
				if (unscaledTime - buttonData.clickTime < 0.3f)
				{
					int clickCount = buttonData.clickCount + 1;
					buttonData.clickCount = clickCount;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.clickTime = unscaledTime;
			}
			else
			{
				buttonData.clickCount = 1;
			}
			buttonData.pointerPress = gameObject2;
			buttonData.rawPointerPress = gameObject;
			buttonData.clickTime = unscaledTime;
			buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
			if (buttonData.pointerDrag != null)
			{
				ExecuteEvents.Execute(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
			}
		}
		if (data.ReleasedThisFrame())
		{
			ExecuteEvents.Execute(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
			GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
			if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
			{
				ExecuteEvents.Execute(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
			}
			else if (buttonData.pointerDrag != null)
			{
				ExecuteEvents.ExecuteHierarchy(gameObject, buttonData, ExecuteEvents.dropHandler);
			}
			buttonData.eligibleForClick = false;
			buttonData.pointerPress = null;
			buttonData.rawPointerPress = null;
			if (buttonData.pointerDrag != null && buttonData.dragging)
			{
				ExecuteEvents.Execute(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
			}
			buttonData.dragging = false;
			buttonData.pointerDrag = null;
			if (gameObject != buttonData.pointerEnter)
			{
				HandlePointerExitAndEnter(buttonData, null);
				HandlePointerExitAndEnter(buttonData, gameObject);
			}
		}
	}

	public override bool IsPointerOverGameObject()
	{
		PointerEventData pointerEventData = new PointerEventData(_eventSystem);
		pointerEventData.position = _cursorPosition;
		_eventSystem.RaycastAll(pointerEventData, _raycastResultsCache);
		return _raycastResultsCache.Count > 0;
	}

	protected override string GetSubmitButtonName()
	{
		return "ConfirmMouse";
	}

	public override bool IsAnyInputActive()
	{
		if (_player.GetAxis("MoveCursorHorizontal") == 0f && _player.GetAxis("MoveCursorVertical") == 0f && !_player.GetButtonDown(GetSubmitButtonName()))
		{
			return IsScavengeKeyboardInputActive();
		}
		return true;
	}

	private bool IsScavengeKeyboardInputActive()
	{
		if (_player.GetAxis("MoveHorizontal") == 0f && _player.GetAxis("MoveVertical") == 0f)
		{
			return _player.GetAxis("Rotate") != 0f;
		}
		return true;
	}
}
