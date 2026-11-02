using System.Collections;
using RG.Parsecs.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RG.VirtualInput;

public class VirtualInputButton : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler, ISubmitHandler
{
	public static VirtualInputButton SelectedVirtualInputButton;

	public int SelectionPriority;

	public bool SelectOnEnable;

	[SerializeField]
	private GameObject _centerOfMass;

	[SerializeField]
	private bool _convertPositionToScreen;

	[SerializeField]
	private bool _registerAutomatically = true;

	[SerializeField]
	private UnityEvent _onSelectEvent;

	[SerializeField]
	private UnityEvent _onDeselectEvent;

	[SerializeField]
	private bool _useTweening = true;

	[SerializeField]
	private bool _lookForOtherButtonOnDeselect = true;

	private TooltipController _tooltipController;

	private Transform _positionTransform;

	private RectTransform _rect;

	private Navigation.Mode _navigationMode;

	private bool _isInteractable;

	public Selectable Selectable { get; private set; }

	public VirtualInputPanel Panel { get; set; }

	public bool IsInteractable
	{
		get
		{
			if (_isInteractable)
			{
				return Selectable.interactable;
			}
			return false;
		}
	}

	public void SetInteractable()
	{
		_isInteractable = true;
	}

	public void Awake()
	{
		BindSelectable();
		_navigationMode = Selectable.navigation.mode;
		_tooltipController = GetComponent<TooltipController>() ?? transform.parent.GetComponent<TooltipController>();
		SetPositionTransform();
		if (_registerAutomatically)
		{
			SetupPanel(GetComponentInParent<VirtualInputPanel>());
		}
	}

	public void SetupPanel(VirtualInputPanel panel)
	{
		Panel = panel;
		if (Panel != null)
		{
			Panel.RegisterButton(this);
			if (Panel.Interactable && !_isInteractable)
			{
				ChangeNavigationMode(value: true);
			}
		}
	}

	private void OnEnable()
	{
		if (SelectOnEnable && Singleton<VirtualInputManager>.Instance.IsSelectablesMode() && !(SelectedVirtualInputButton == this) && IsInteractable && (!(SelectedVirtualInputButton != null) || SelectionPriority > SelectedVirtualInputButton.SelectionPriority))
		{
			SelectThisSelectable();
		}
	}

	private void OnDisable()
	{
		DeselectThisSelectable();
	}

	private void OnDestroy()
	{
		if (Panel != null)
		{
			Panel.UnRegisterButton(this);
		}
		Panel = null;
		Selectable = null;
		if (SelectedVirtualInputButton == this)
		{
			SelectedVirtualInputButton = null;
		}
	}

	private bool BindSelectable()
	{
		if (Selectable == null)
		{
			Selectable = GetComponent<Selectable>();
			if (Selectable == null)
			{
				Debug.LogError("Selectable is null. GamepadButton needs to be a sibling to the Selectable because of OnSelect events that drive cursor position.");
				return false;
			}
		}
		return true;
	}

	private void SetPositionTransform()
	{
		if (_centerOfMass == null)
		{
			_rect = GetComponent<RectTransform>();
			_positionTransform = transform;
		}
		else
		{
			_rect = _centerOfMass.GetComponent<RectTransform>();
			_positionTransform = _centerOfMass.transform;
		}
	}

	public void SetPositionTransform(Transform newTransform)
	{
		_rect = newTransform.GetComponent<RectTransform>();
		_positionTransform = newTransform;
	}

	public void SelectThisSelectable()
	{
		if (!Singleton<VirtualInputManager>.Instance.IsGamepadInputBlocked)
		{
			EventSystem.current.SetSelectedGameObject(Selectable.gameObject);
		}
	}

	private void DeselectThisSelectable()
	{
		if (SelectedVirtualInputButton == this && EventSystem.current != null)
		{
			EventSystem.current.SetSelectedGameObject(null);
			OnDeselect();
		}
	}

	public void OnSelect(BaseEventData eventData)
	{
		Singleton<VirtualInputManager>.Instance.SetMousePosition(GetPosition(), _useTweening);
		SelectedVirtualInputButton = this;
		if (_tooltipController != null)
		{
			_tooltipController.SendMessage("GamepadEnter");
		}
		if (_onSelectEvent != null)
		{
			_onSelectEvent.Invoke();
		}
	}

	public Vector3 GetPosition()
	{
		Vector3 vector = _positionTransform.position;
		if (_convertPositionToScreen || _rect == null)
		{
			vector = Camera.main.WorldToScreenPoint(vector);
		}
		return vector;
	}

	public void OnDeselect(BaseEventData eventData)
	{
		OnDeselect();
	}

	private void OnDeselect()
	{
		if (_tooltipController != null)
		{
			_tooltipController.SendMessage("GamepadExit");
		}
		if (SelectedVirtualInputButton == this)
		{
			SelectedVirtualInputButton = null;
			if (Panel != null && _lookForOtherButtonOnDeselect)
			{
				Singleton<VirtualInputManager>.Instance.StartCoroutine(CheckIfSelectedNewButton());
			}
		}
		if (_onDeselectEvent != null)
		{
			_onDeselectEvent.Invoke();
		}
	}

	private IEnumerator CheckIfSelectedNewButton()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if (SelectedVirtualInputButton == null && Singleton<VirtualInputManager>.Instance.IsSelectablesMode() && Panel != null)
		{
			Panel.SelectPriorityButton();
		}
	}

	public void OnSubmit(BaseEventData eventData)
	{
	}

	public void ChangeNavigationMode(bool value)
	{
		_isInteractable = value;
		if (!(Selectable == null))
		{
			Navigation navigation = Selectable.navigation;
			navigation.mode = (value ? _navigationMode : Navigation.Mode.None);
			Selectable.navigation = navigation;
		}
	}
}
