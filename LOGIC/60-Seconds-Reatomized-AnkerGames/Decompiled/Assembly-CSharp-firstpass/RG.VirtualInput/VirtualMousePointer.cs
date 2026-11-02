using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace RG.VirtualInput;

public class VirtualMousePointer : StandaloneInputModule, IVirtualInputMode
{
	protected const string CURSOR_HORIZONTAL_AXIS_NAME = "MoveCursorHorizontal";

	protected const string CURSOR_VERTICAL_AXIS_NAME = "MoveCursorVertical";

	private const string MOUSE_SUBMIT_BUTTON_NAME = "Confirm";

	[SerializeField]
	private GlobalFloatVariable _pointerSensitivity;

	[SerializeField]
	protected GameObject _cursorObject;

	[SerializeField]
	protected EventSystem _eventSystem;

	private PointerEventData _pointerEvent;

	protected Player _player;

	private Camera _mainCamera;

	protected Vector2 _cursorPosition;

	private Vector2 _xPositionRange;

	private Vector2 _yPositionRange;

	private Vector2 _previousScreenResolution;

	private Rigidbody2D _previousMouseHoverTarget;

	protected override void Awake()
	{
		base.Awake();
		_player = ReInput.players.GetPlayer(0);
		_mainCamera = Camera.main;
	}

	protected override void Start()
	{
		base.Start();
		_pointerEvent = new PointerEventData(_eventSystem);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		SetMousePositionRanges();
	}

	public void OnStartMode()
	{
		enabled = true;
	}

	public void OnFinishMode()
	{
		HandleMouseExit();
		HandlePointerExitAndEnter(_pointerEvent, null);
		_eventSystem.SetSelectedGameObject(null);
		enabled = false;
	}

	protected virtual void UpdateCursorMovement()
	{
		if (_xPositionRange == Vector2.zero || _yPositionRange == Vector2.zero || (float)Screen.width != _previousScreenResolution.x || (float)Screen.height != _previousScreenResolution.y)
		{
			SetMousePositionRanges();
		}
		float axis = _player.GetAxis("MoveCursorHorizontal");
		float axis2 = _player.GetAxis("MoveCursorVertical");
		float x = Mathf.Clamp(_cursorObject.transform.position.x + axis * _pointerSensitivity.Value, _xPositionRange.x, _xPositionRange.y);
		float y = Mathf.Clamp(_cursorObject.transform.position.y + axis2 * _pointerSensitivity.Value, _yPositionRange.x, _yPositionRange.y);
		_cursorObject.transform.position = new Vector3(x, y, 0f);
	}

	public override void Process()
	{
		if (!ShouldProcess())
		{
			return;
		}
		SendUpdateEventToSelectedObject();
		UpdateCursorMovement();
		CheckCamera();
		Vector3 position = _cursorObject.transform.position;
		_cursorPosition.x = position.x;
		_cursorPosition.y = position.y;
		_pointerEvent.position = _cursorPosition;
		_eventSystem.RaycastAll(_pointerEvent, m_RaycastResultCache);
		RaycastResult raycastResult = BaseInputModule.FindFirstRaycast(m_RaycastResultCache);
		_pointerEvent.pointerCurrentRaycast = raycastResult;
		ProcessMove(_pointerEvent);
		ProcessDrag(_pointerEvent);
		_pointerEvent.clickCount = 0;
		if (_player.GetButtonDown(GetSubmitButtonName()))
		{
			_pointerEvent.pressPosition = _cursorPosition;
			_pointerEvent.clickTime = Time.unscaledTime;
			_pointerEvent.pointerPressRaycast = raycastResult;
			_pointerEvent.clickCount = 1;
			_pointerEvent.eligibleForClick = true;
			if (m_RaycastResultCache.Count > 0)
			{
				GameObject gameObject = ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, _pointerEvent, ExecuteEvents.pointerDownHandler);
				if (gameObject == null)
				{
					gameObject = ExecuteEvents.GetEventHandler<IPointerClickHandler>(base.gameObject);
				}
				_pointerEvent.pointerPress = gameObject;
				_pointerEvent.rawPointerPress = raycastResult.gameObject;
				_pointerEvent.pointerDrag = ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, _pointerEvent, ExecuteEvents.pointerDownHandler);
			}
			else
			{
				_pointerEvent.rawPointerPress = null;
				_pointerEvent.pointerPress = null;
			}
		}
		else if (_player.GetButtonUp(GetSubmitButtonName()))
		{
			ExecuteEvents.Execute(_pointerEvent.pointerPress, _pointerEvent, ExecuteEvents.pointerUpHandler);
			GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(raycastResult.gameObject);
			if (_pointerEvent.pointerPress == eventHandler && _pointerEvent.eligibleForClick)
			{
				ExecuteEvents.Execute(_pointerEvent.pointerPress, _pointerEvent, ExecuteEvents.pointerClickHandler);
			}
			_pointerEvent.clickCount = 0;
			_pointerEvent.eligibleForClick = false;
			_pointerEvent.pointerPress = null;
			_pointerEvent.rawPointerPress = null;
			_pointerEvent.dragging = false;
			_pointerEvent.pointerDrag = null;
		}
		if (m_RaycastResultCache.Count > 0)
		{
			_pointerEvent.selectedObject = raycastResult.gameObject;
		}
		else
		{
			_pointerEvent.selectedObject = null;
		}
		HandleMouseEvents();
	}

	protected virtual string GetSubmitButtonName()
	{
		return "Confirm";
	}

	private bool ShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
	{
		if (!useDragThreshold)
		{
			return true;
		}
		return (pressPos - currentPos).sqrMagnitude >= threshold * threshold;
	}

	protected override void ProcessDrag(PointerEventData pointerEvent)
	{
		if (pointerEvent.pointerDrag == null)
		{
			return;
		}
		if (!pointerEvent.dragging && ShouldStartDrag(pointerEvent.pressPosition, pointerEvent.position, eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold))
		{
			ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);
			pointerEvent.dragging = true;
		}
		if (pointerEvent.dragging)
		{
			if (pointerEvent.pointerPress != pointerEvent.pointerDrag)
			{
				ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
				pointerEvent.eligibleForClick = false;
				pointerEvent.pointerPress = null;
				pointerEvent.rawPointerPress = null;
			}
			ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.dragHandler);
		}
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

	protected void HandleMouseEvents()
	{
		if (_mainCamera == null || m_RaycastResultCache.Count > 0)
		{
			return;
		}
		Collider2D collider2D = Physics2D.OverlapPoint(_mainCamera.ScreenToWorldPoint(new Vector3(_cursorPosition.x, _cursorPosition.y, _mainCamera.transform.position.z * -1f)));
		if (collider2D != null)
		{
			Rigidbody2D componentInParent = collider2D.GetComponentInParent<Rigidbody2D>();
			if (componentInParent != null)
			{
				if (_player.GetButtonDown(GetSubmitButtonName()))
				{
					componentInParent.SendMessage("OnMouseUpAsButton");
					componentInParent.SendMessage("OnMouseDown");
				}
				if (_player.GetButtonUp(GetSubmitButtonName()))
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

	protected void CheckCamera()
	{
		if (_mainCamera == null)
		{
			_mainCamera = Camera.main;
		}
	}

	private void SetMousePositionRanges()
	{
		if (!(_mainCamera == null))
		{
			Vector3 vector = _mainCamera.ViewportToScreenPoint(new Vector3(0f, 0f, 0f));
			Vector3 vector2 = _mainCamera.ViewportToScreenPoint(new Vector3(1f, 1f, 1f));
			_xPositionRange = new Vector2(vector.x, vector2.x);
			_yPositionRange = new Vector2(vector.y, vector2.y);
			_previousScreenResolution = new Vector2(Screen.width, Screen.height);
		}
	}

	public Vector3 GetMousePosition()
	{
		return _cursorPosition;
	}

	public virtual bool IsPointerOverGameObject()
	{
		return m_RaycastResultCache.Count > 0;
	}

	public virtual bool IsAnyInputActive()
	{
		if (_player.GetAxis("MoveCursorHorizontal") == 0f)
		{
			return _player.GetAxis("MoveCursorVertical") != 0f;
		}
		return true;
	}

	public bool OverMe(GameObject gameObject)
	{
		if (_previousMouseHoverTarget == null)
		{
			return false;
		}
		return _previousMouseHoverTarget.gameObject == gameObject;
	}

	public virtual bool ShouldProcess()
	{
		if (Singleton<VirtualInputManager>.Instance.IsPointerMode())
		{
			return !Singleton<VirtualInputManager>.Instance.IsGamepadInputBlocked;
		}
		return false;
	}
}
