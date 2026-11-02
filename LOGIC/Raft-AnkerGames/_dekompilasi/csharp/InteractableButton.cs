using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RaycastInteractable))]
public class InteractableButton : MonoBehaviour, IRaycastable
{
	public Action OnButtonPressed;

	[SerializeField]
	private string keybindName = string.Empty;

	[SerializeField]
	public string localizationTerm = string.Empty;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private string triggerParameter = string.Empty;

	[SerializeField]
	[EventRef]
	private string eventRef_Click = string.Empty;

	[SerializeField]
	[EventRef]
	private string eventRef_FailClick = string.Empty;

	protected static DisplayTextManager displayTextManager;

	protected static Network_Player localPlayer;

	protected virtual void Start()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (displayTextManager == null)
		{
			displayTextManager = ComponentManager<DisplayTextManager>.Value;
		}
	}

	private void OnDestroy()
	{
		localPlayer = null;
		displayTextManager = null;
	}

	void IRaycastable.OnIsRayed()
	{
		HandleOnIsRayed();
	}

	void IRaycastable.OnRayEnter()
	{
		if (displayTextManager == null)
		{
			displayTextManager = ComponentManager<DisplayTextManager>.Value;
		}
	}

	void IRaycastable.OnRayExit()
	{
		if (displayTextManager != null)
		{
			displayTextManager.HideDisplayTexts();
		}
	}

	private bool HasValidKeybindName()
	{
		if (keybindName == string.Empty)
		{
			return false;
		}
		if (string.IsNullOrWhiteSpace(keybindName))
		{
			return false;
		}
		return true;
	}

	private void OnInspectorGUILocalizationTerm()
	{
		GUILayout.Label("Term: " + Helper.GetTerm(localizationTerm));
	}

	protected virtual void HandleOnIsRayed()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (displayTextManager != null && !localizationTerm.IsNullOrEmpty())
		{
			ShowOnHoverText(localizationTerm, keybindName);
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, keybindName))
		{
			ButtonPressed();
		}
	}

	protected virtual void ShowOnHoverText(string p_localizationTerm, string p_keybindName)
	{
		string text = Helper.GetTerm(p_localizationTerm);
		if (text.IsNullOrEmpty())
		{
			text = p_localizationTerm;
		}
		if (p_keybindName.IsNullOrEmpty())
		{
			displayTextManager.ShowText(text);
		}
		else
		{
			displayTextManager.ShowText(text, PlayerInput.GetPlayerByIndex(0).actions["Interact"].GetBindingDisplayString(), MyInput.Keybinds[p_keybindName].MainKey);
		}
	}

	public void ButtonPress_Fake(bool hideDisplayText = true)
	{
		ButtonPressed(hideDisplayText);
	}

	protected virtual bool ButtonPressed(bool hideDisplayText = true)
	{
		if (CanButtonBePressed())
		{
			OnButtonPressed.CallSafe();
			if (hideDisplayText && displayTextManager != null && !string.IsNullOrEmpty(localizationTerm))
			{
				displayTextManager.HideDisplayTexts();
			}
			return true;
		}
		return false;
	}

	protected virtual bool CanButtonBePressed()
	{
		return true;
	}

	public void PlayButtonFeedback(bool success)
	{
		PlayAnimation();
		if (success)
		{
			RuntimeManager.PlayOneShot(eventRef_Click, transform.position);
		}
		else
		{
			RuntimeManager.PlayOneShot(eventRef_FailClick, transform.position);
		}
	}

	protected void PlayAnimation()
	{
		if (animator != null && !string.IsNullOrEmpty(triggerParameter))
		{
			animator.SetTrigger(triggerParameter);
		}
	}
}
