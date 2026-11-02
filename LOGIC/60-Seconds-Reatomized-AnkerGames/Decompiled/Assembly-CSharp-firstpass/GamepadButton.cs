using System.Collections;
using RG.Parsecs.Common;
using RG.Parsecs.Survival;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class GamepadButton : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler, ISubmitHandler
{
	private WindowView view;

	[SerializeField]
	private GameObject CenterOfMass;

	[SerializeField]
	private bool ConvertPositionToScreen;

	[SerializeField]
	private bool RegisterAutomatically = true;

	private IRefreshInteractable refreshInteractable;

	public RefreshInteractableMode RefreshInteractableMode;

	public int SelectionPriority;

	[SerializeField]
	private GamepadButton SelectOnSubmit;

	[SerializeField]
	private bool SelectPanelAcceptButtonOnSubmit;

	[SerializeField]
	[Tooltip("If null: searches in parent hierarchy.")]
	private GameObject RefreshInteractableSource;

	public bool SelectOnEnable;

	public bool HideCursorOnSelect;

	public bool DontRefreshInteractable;

	private RectTransform _rect;

	private OnUIClickedSoundPlayer _uiSoundPlayer;

	private OnSpriteClickedSoundPlayer _spriteSoundPlayer;

	private TooltipController _tooltipController;

	private Transform _positionTransform;

	private Toggle _toggle;

	private static GamepadButton SelectedGamepadButton;

	[Tooltip("Use Unity UI Selectable system to detect where player can click by game pad")]
	[SerializeField]
	private bool _useSelectable = true;

	private bool _isInteractable;

	private Navigation.Mode _navigationMode;

	public Selectable Selectable { get; private set; }

	public GamepadPanel Panel { get; private set; }

	public bool IsInteractable
	{
		get
		{
			if (!_useSelectable)
			{
				if (_isInteractable)
				{
					return Selectable.interactable;
				}
				return false;
			}
			return true;
		}
	}

	public void SetInteractable()
	{
		if (!_useSelectable)
		{
			_isInteractable = true;
		}
	}

	private void Awake()
	{
		_uiSoundPlayer = GetComponent<OnUIClickedSoundPlayer>() ?? transform.parent.GetComponent<OnUIClickedSoundPlayer>();
		_spriteSoundPlayer = GetComponent<OnSpriteClickedSoundPlayer>() ?? transform.parent.GetComponent<OnSpriteClickedSoundPlayer>();
		_tooltipController = GetComponent<TooltipController>() ?? transform.parent.GetComponent<TooltipController>();
		_toggle = GetComponent<Toggle>() ?? transform.parent.GetComponent<Toggle>();
		if (CenterOfMass == null)
		{
			_rect = GetComponent<RectTransform>();
			_positionTransform = transform;
		}
		else
		{
			_rect = CenterOfMass.GetComponent<RectTransform>();
			_positionTransform = CenterOfMass.transform;
		}
		BindSelectable();
		_navigationMode = Selectable.navigation.mode;
		if (RefreshInteractableSource != null)
		{
			refreshInteractable = RefreshInteractableSource.GetComponent<IRefreshInteractable>();
		}
		else
		{
			refreshInteractable = GetComponentInParent<IRefreshInteractable>();
		}
		if (RegisterAutomatically)
		{
			Panel = GetComponentInParent<GamepadPanel>();
			if (Panel == null)
			{
				Debug.LogError("GamepadButton without GamepadPanel");
			}
			Panel.RegisterButton(this, refreshInteractable);
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

	private void OnEnable()
	{
		if (SelectOnEnable && Singleton<HandCursor>.Instance.isGamepadMode && !Singleton<HandCursor>.Instance.isGamepadPointerMode && !(SelectedGamepadButton == this) && (!(SelectedGamepadButton != null) || SelectionPriority > SelectedGamepadButton.SelectionPriority))
		{
			SelectThisSelectable();
		}
	}

	private void OnDisable()
	{
		if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
		{
			EventSystem.current.SetSelectedGameObject(null);
			SelectedGamepadButton = null;
		}
	}

	public void RefreshInteractable(bool value)
	{
		if (RefreshInteractableMode == RefreshInteractableMode.UseParenteRefreshInteractable)
		{
			if ((!value || refreshInteractable == null) && !DontRefreshInteractable && BindSelectable())
			{
				if (_useSelectable)
				{
					Selectable.interactable = value;
				}
				else
				{
					ChangeNavigationMode(value);
				}
			}
		}
		else if (BindSelectable())
		{
			if (_useSelectable)
			{
				Selectable.interactable = value;
			}
			else
			{
				ChangeNavigationMode(value);
			}
		}
	}

	private void ChangeNavigationMode(bool value)
	{
		_isInteractable = value;
		Navigation navigation = Selectable.navigation;
		navigation.mode = (value ? _navigationMode : Navigation.Mode.None);
		Selectable.navigation = navigation;
	}

	public void SelectThisSelectable()
	{
		if (!Singleton<HandCursor>.Instance.IsGamepadInputBlocked)
		{
			SelectedGamepadButton = this;
			StartCoroutine(SelectAsSoonAsPossible());
		}
	}

	private IEnumerator SelectAsSoonAsPossible()
	{
		if (!(EventSystem.current.currentSelectedGameObject == gameObject))
		{
			yield return null;
			EventSystem.current.SetSelectedGameObject(null);
			yield return new WaitForSecondsRealtime(0.1f);
			EventSystem.current.SetSelectedGameObject(Selectable.gameObject);
			SelectedGamepadButton = this;
		}
	}

	public void OnSelect(BaseEventData eventData)
	{
		if (Singleton<HandCursor>.Instance.isGamepadMode && !Singleton<HandCursor>.Instance.isGamepadPointerMode)
		{
			bool tween = true;
			if (HideCursorOnSelect)
			{
				Singleton<HandCursor>.Instance.HandSpriteVisible = false;
			}
			else
			{
				Singleton<HandCursor>.Instance.HandSpriteVisible = true;
			}
			Vector3 position = GetPosition();
			Singleton<HandCursor>.Instance.SetMousePosition(position, tween);
			SelectedGamepadButton = this;
			if (_tooltipController != null)
			{
				_tooltipController.SendMessage("GamepadEnter");
			}
		}
	}

	private Vector3 GetPosition()
	{
		Vector3 vector = _positionTransform.position;
		if (ConvertPositionToScreen || _rect == null)
		{
			vector = Camera.main.WorldToScreenPoint(vector);
		}
		return vector;
	}

	public void OnDeselect(BaseEventData eventData)
	{
		if (_tooltipController != null)
		{
			_tooltipController.SendMessage("GamepadExit");
		}
		if (SelectedGamepadButton == this)
		{
			SelectedGamepadButton = null;
		}
	}

	public void OnSubmit(BaseEventData eventData)
	{
		if (Singleton<HandCursor>.Instance.IsGamepadInputBlocked)
		{
			return;
		}
		if (_uiSoundPlayer != null)
		{
			_uiSoundPlayer.PlaySound();
		}
		if (_spriteSoundPlayer != null)
		{
			_spriteSoundPlayer.InvokePlaySound();
		}
		DisplayView componentInParent = GetComponentInParent<DisplayView>();
		if (componentInParent != null)
		{
			componentInParent.ShowWindowView();
		}
		else
		{
			DisplayViewController componentInParent2 = GetComponentInParent<DisplayViewController>();
			if (componentInParent2 != null)
			{
				Panel.Hide();
				componentInParent2.ShowWindowView();
			}
			else
			{
				CharacterSlot componentInParent3 = GetComponentInParent<CharacterSlot>();
				if (componentInParent3 != null)
				{
					componentInParent3.ShowRationing();
				}
			}
		}
		if (Singleton<HandCursor>.Instance.isGamepadMode && !Singleton<HandCursor>.Instance.isGamepadPointerMode && (!(_toggle != null) || _toggle.isOn))
		{
			if (SelectOnSubmit != null)
			{
				SelectOnSubmit.SelectThisSelectable();
			}
			else if (SelectPanelAcceptButtonOnSubmit)
			{
				Panel.SelectAcceptButton();
			}
		}
	}

	private void OnDestroy()
	{
		if (Panel != null)
		{
			Panel.UnRegisterButton(this, refreshInteractable);
		}
		Panel = null;
		Selectable = null;
		refreshInteractable = null;
		if (SelectedGamepadButton == this)
		{
			SelectedGamepadButton = null;
		}
	}
}
