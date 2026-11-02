using System.Collections.Generic;
using RG.Parsecs.Common;
using RG.Parsecs.Survival;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamepadPanel : MonoBehaviour
{
	protected List<GamepadButton> _buttons = new List<GamepadButton>();

	protected HashSet<GameObject> _buttonsObjects = new HashSet<GameObject>();

	public List<GamepadButton> ExternalButtons = new List<GamepadButton>();

	public GamepadPanelShoulderButtons ShoulderButtons;

	public GamepadButton AcceptButton;

	protected bool _interactable = true;

	private List<IRefreshInteractable> _refreshInteractables = new List<IRefreshInteractable>();

	public static GamepadPanel CurrentPanel { get; protected set; }

	public bool Interactable
	{
		get
		{
			return _interactable;
		}
		set
		{
			_interactable = value;
			for (int i = 0; i < _refreshInteractables.Count; i++)
			{
				_ = _refreshInteractables[i];
			}
			for (int j = 0; j < _buttons.Count; j++)
			{
				_ = _buttons[j];
			}
			if (value && Singleton<HandCursor>.Instance.isGamepadMode && !Singleton<HandCursor>.Instance.isGamepadPointerMode && !Singleton<HandCursor>.Instance.IsGamepadInputBlocked)
			{
				SelectPriorityButton();
			}
		}
	}

	protected virtual void Awake()
	{
		for (int i = 0; i < ExternalButtons.Count; i++)
		{
			GamepadButton button = ExternalButtons[i];
			RegisterButton(button, null);
		}
	}

	protected virtual void OnEnable()
	{
		if (CurrentPanel != null)
		{
			CurrentPanel.Interactable = false;
		}
		CurrentPanel = this;
		Interactable = true;
	}

	public void SelectPriorityButton()
	{
		if (EventSystem.current == null || _buttonsObjects.Contains(EventSystem.current.currentSelectedGameObject))
		{
			return;
		}
		GamepadButton gamepadButton = null;
		GamepadButton gamepadButton2 = null;
		for (int i = 0; i < _buttons.Count; i++)
		{
			GamepadButton gamepadButton3 = _buttons[i];
			if (gamepadButton3.Selectable.interactable && gamepadButton3.IsInteractable && gamepadButton3.gameObject.activeInHierarchy && (gamepadButton2 == null || gamepadButton3.SelectionPriority > gamepadButton2.SelectionPriority))
			{
				gamepadButton2 = gamepadButton3;
			}
			if (gamepadButton3.SelectOnEnable && gamepadButton3.IsInteractable && gamepadButton3.Selectable.interactable && gamepadButton3.gameObject.activeInHierarchy && (gamepadButton == null || gamepadButton3.SelectionPriority > gamepadButton.SelectionPriority))
			{
				gamepadButton = gamepadButton3;
			}
		}
		if (gamepadButton != null)
		{
			gamepadButton.SelectThisSelectable();
		}
		else if (gamepadButton2 != null)
		{
			gamepadButton2.SelectThisSelectable();
		}
	}

	public virtual void Hide()
	{
	}

	public void RegisterButton(GamepadButton button, IRefreshInteractable refreshInteractable)
	{
		_buttons.Add(button);
		_buttonsObjects.Add(button.gameObject);
		if (refreshInteractable != null && !_refreshInteractables.Contains(refreshInteractable))
		{
			_refreshInteractables.Add(refreshInteractable);
		}
	}

	internal void UnRegisterButton(GamepadButton button, IRefreshInteractable refreshInteractable)
	{
		_buttons.Remove(button);
		if (button.gameObject != null)
		{
			_buttonsObjects.Remove(button.gameObject);
		}
		if (refreshInteractable != null)
		{
			_refreshInteractables.Remove(refreshInteractable);
		}
	}

	private void OnDestroy()
	{
		_refreshInteractables.Clear();
		_buttons.Clear();
		_buttonsObjects.Clear();
		if (CurrentPanel == this)
		{
			CurrentPanel = null;
		}
	}

	public virtual void Cancel()
	{
	}

	private void ButtonInteraction(Button button)
	{
		if (_interactable && button != null && button.gameObject.activeInHierarchy && button.interactable)
		{
			button.onClick.Invoke();
		}
	}

	public void LB()
	{
		ButtonInteraction(ShoulderButtons.LB);
	}

	public void RB()
	{
		ButtonInteraction(ShoulderButtons.RB);
	}

	public void LT()
	{
		ButtonInteraction(ShoulderButtons.LT);
	}

	public void RT()
	{
		ButtonInteraction(ShoulderButtons.RT);
	}

	internal void SelectAcceptButton()
	{
		if (AcceptButton != null)
		{
			AcceptButton.SelectThisSelectable();
		}
	}
}
