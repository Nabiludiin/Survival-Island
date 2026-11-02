using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class HazmatSuitLocker : MonoBehaviour, IRaycastable
{
	[SerializeField]
	private Animator lockerAnimator;

	[SerializeField]
	private float channelTime = 1f;

	[SerializeField]
	private AnimatorMessageForwarder animatorMessage;

	public EventReference er_open;

	public EventReference er_close;

	private float currentChannelProgress;

	private CanvasHelper canvasHelper;

	private bool used;

	private static DisplayTextManager displayText;

	private static Network_Player localPlayer;

	private void Start()
	{
		canvasHelper = ComponentManager<CanvasHelper>.Value;
		localPlayer = ComponentManager<Network_Player>.Value;
		if (animatorMessage != null)
		{
			AnimatorMessageForwarder animatorMessageForwarder = animatorMessage;
			animatorMessageForwarder.OnAnimatorMessage = (Action<string>)Delegate.Combine(animatorMessageForwarder.OnAnimatorMessage, new Action<string>(OnAnimatorMessage));
		}
	}

	private void OnAnimatorMessage(string msg)
	{
		if (!(msg == "Open"))
		{
			if (msg == "Close")
			{
				RuntimeManager.PlayOneShot(er_close, transform.position);
			}
		}
		else
		{
			RuntimeManager.PlayOneShot(er_open, transform.position);
		}
	}

	void IRaycastable.OnIsRayed()
	{
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		if (!used)
		{
			if (displayText == null)
			{
				displayText = ComponentManager<DisplayTextManager>.Value;
			}
			if (localPlayer != null && !localPlayer.PlayerScript.IsDead && CanvasHelper.ActiveMenu == MenuType.None)
			{
				displayText.ShowText(Helper.GetTerm("Game/HazmatSuit"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(action, "Interact"))
				{
					ProgressChannel(Time.deltaTime);
				}
				else
				{
					CancelChannel();
				}
			}
		}
		else if (localPlayer != null && !localPlayer.PlayerScript.IsDead && CanvasHelper.ActiveMenu == MenuType.None && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(action, "Interact"))
		{
			ResetUsed();
		}
	}

	void IRaycastable.OnRayEnter()
	{
		ResetUsed();
	}

	void IRaycastable.OnRayExit()
	{
		if (displayText == null)
		{
			displayText = ComponentManager<DisplayTextManager>.Value;
		}
		displayText.HideDisplayTexts();
		CancelChannel();
		ResetUsed();
	}

	private void ProgressChannel(float time)
	{
		currentChannelProgress += time;
		float num = currentChannelProgress / channelTime;
		lockerAnimator.SetBool("Opening", value: true);
		if (num != 0f)
		{
			UpdateUIChannelRing(num);
		}
		if (currentChannelProgress >= channelTime)
		{
			localPlayer.HazmatSuit.EquipNetworked();
			displayText.HideDisplayTexts();
			used = true;
			CancelChannel();
		}
	}

	private void CancelChannel()
	{
		currentChannelProgress = 0f;
		UpdateUIChannelRing(currentChannelProgress);
		if (canvasHelper != null)
		{
			canvasHelper.ResetLoadAndAimCircleAnimationPulse();
		}
		lockerAnimator.SetBool("Opening", value: false);
	}

	private void UpdateUIChannelRing(float normalizedFill)
	{
		if (canvasHelper != null)
		{
			canvasHelper.SetLoadCircle(normalizedFill);
		}
	}

	private void ResetUsed()
	{
		used = false;
	}
}
