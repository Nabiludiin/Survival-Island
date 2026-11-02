using System.Collections;
using System.Collections.Generic;
using RG.Parsecs.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RG.VirtualInput;

public class VirtualInputPanel : MonoBehaviour
{
	[Tooltip("Should buttons on this panel be disabled when new closable panel is open from it")]
	[SerializeField]
	private bool _disableButtonsWhenNewPanelOpen = true;

	public List<VirtualInputButton> ExternalButtons = new List<VirtualInputButton>();

	protected List<VirtualInputButton> _buttons = new List<VirtualInputButton>();

	private bool _interactable;

	public static VirtualInputPanel CurrentPanel { get; protected set; }

	public bool Interactable
	{
		get
		{
			return _interactable;
		}
		set
		{
			if (_disableButtonsWhenNewPanelOpen)
			{
				ChangeNavigationModeInButtons(value);
			}
			_interactable = value;
			if (value && Singleton<VirtualInputManager>.Instance != null && Singleton<VirtualInputManager>.Instance.IsSelectablesMode())
			{
				SelectPriorityButton();
			}
		}
	}

	protected virtual void Awake()
	{
		for (int i = 0; i < ExternalButtons.Count; i++)
		{
			if (ExternalButtons[i] != null)
			{
				ExternalButtons[i].SetupPanel(this);
			}
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

	private void OnDestroy()
	{
		if (CurrentPanel == this)
		{
			CurrentPanel = null;
		}
	}

	public void SelectPriorityButton()
	{
		if (gameObject.activeInHierarchy)
		{
			StartCoroutine(SelectPriorityButtonDelayed());
		}
	}

	public void SelectClosestButton()
	{
		if (gameObject.activeInHierarchy)
		{
			StartCoroutine(SelectClosestButtonDelayed());
		}
	}

	private IEnumerator SelectPriorityButtonDelayed()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if (EventSystem.current == null)
		{
			yield return null;
		}
		VirtualInputButton virtualInputButton = null;
		VirtualInputButton virtualInputButton2 = null;
		for (int i = 0; i < _buttons.Count; i++)
		{
			VirtualInputButton virtualInputButton3 = _buttons[i];
			if (!(virtualInputButton3 == null) && !(virtualInputButton3.Selectable == null))
			{
				if (virtualInputButton3.Selectable.interactable && virtualInputButton3.IsInteractable && virtualInputButton3.gameObject.activeInHierarchy && (virtualInputButton2 == null || virtualInputButton3.SelectionPriority > virtualInputButton2.SelectionPriority))
				{
					virtualInputButton2 = virtualInputButton3;
				}
				if (virtualInputButton3.SelectOnEnable && virtualInputButton3.IsInteractable && virtualInputButton3.Selectable.interactable && virtualInputButton3.gameObject.activeInHierarchy && (virtualInputButton == null || virtualInputButton3.SelectionPriority > virtualInputButton.SelectionPriority))
				{
					virtualInputButton = virtualInputButton3;
				}
			}
		}
		if (virtualInputButton != null)
		{
			virtualInputButton.SelectThisSelectable();
		}
		else if (virtualInputButton2 != null)
		{
			virtualInputButton2.SelectThisSelectable();
		}
	}

	private IEnumerator SelectClosestButtonDelayed()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if (EventSystem.current == null)
		{
			yield return null;
		}
		VirtualInputButton virtualInputButton = null;
		float num = 1000000f;
		for (int i = 0; i < _buttons.Count; i++)
		{
			VirtualInputButton virtualInputButton2 = _buttons[i];
			if (!(virtualInputButton2 == null) && !(virtualInputButton2.Selectable == null) && virtualInputButton2.IsInteractable && virtualInputButton2.Selectable.interactable && virtualInputButton2.gameObject.activeInHierarchy)
			{
				float num2 = Vector3.Distance(_buttons[i].GetPosition(), Singleton<VirtualInputManager>.Instance.GetMousePosition());
				if (num2 < num)
				{
					num = num2;
					virtualInputButton = _buttons[i];
				}
			}
		}
		if (virtualInputButton != null)
		{
			virtualInputButton.SelectThisSelectable();
		}
	}

	public void RegisterButton(VirtualInputButton button)
	{
		_buttons.Add(button);
	}

	internal void UnRegisterButton(VirtualInputButton button)
	{
		_buttons.Remove(button);
	}

	public void ChangeNavigationModeInButtons(bool navigationMode)
	{
		for (int i = 0; i < _buttons.Count; i++)
		{
			_buttons[i].ChangeNavigationMode(navigationMode);
		}
	}
}
