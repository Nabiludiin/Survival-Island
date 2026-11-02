using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RaycastInteractable))]
public class QuestInteractable : QuestEventBase, IRaycastable
{
	[HideInInspector]
	protected DisplayTextManager displayText;

	[SerializeField]
	private bool allowPlayerRaycastInteract = true;

	protected virtual bool IsPlayerConsideredBusy => PlayerItemManager.IsBusy;

	protected override void Start()
	{
		base.Start();
		displayText = ComponentManager<DisplayTextManager>.Value;
		if (Raft_Network.IsHost)
		{
			SendOnInteractEvent(null, atStart: true);
		}
	}

	public virtual string GetRequirementText()
	{
		string text = " ";
		if (interactRequirement.Count >= 2)
		{
			text += "\n";
		}
		for (int i = 0; i < interactRequirement.Count; i++)
		{
			InteractRequirementItem interactRequirementItem = interactRequirement[i];
			if (interactRequirementItem != null)
			{
				text += interactRequirementItem.GetRequirementText();
				if (i + 1 < interactRequirement.Count)
				{
					text += "\n";
				}
			}
		}
		return text;
	}

	public virtual void OnIsRayed()
	{
		if (!allowPlayerRaycastInteract)
		{
			return;
		}
		if (!CanStillBeInteractedWith || CanvasHelper.ActiveMenu != MenuType.None || IsPlayerConsideredBusy)
		{
			displayText.HideDisplayTexts();
			return;
		}
		if (HasInteractRequirement(Network.GetLocalPlayer()))
		{
			DisplayInteractText(Network.GetLocalPlayer());
		}
		else
		{
			DisplayRequirementText(Network.GetLocalPlayer());
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			SendOnInteractNetwork(Network.GetLocalPlayer());
			if (displayText != null)
			{
				displayText.HideDisplayTexts();
			}
		}
	}

	public virtual void DisplayRequirementText(Network_Player player)
	{
		displayText.ShowText(Helper.GetTerm("Game/Requires") + GetRequirementText());
	}

	public virtual void DisplayInteractText(Network_Player player)
	{
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		displayText.ShowText(Helper.GetTerm("Game/Use") + GetRequirementText(), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (allowPlayerRaycastInteract && displayText != null)
		{
			displayText.HideDisplayTexts();
		}
	}
}
