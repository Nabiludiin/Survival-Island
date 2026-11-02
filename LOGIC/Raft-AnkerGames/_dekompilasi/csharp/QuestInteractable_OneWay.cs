using UnityEngine;
using UnityEngine.InputSystem;

public class QuestInteractable_OneWay : QuestInteractable
{
	[SerializeField]
	public int openDoorIndex = 1;

	[Tooltip("Which local axis does the player have to be on in order to interact.")]
	[SerializeField]
	private Axis correctAxis;

	[SerializeField]
	private Axis byPassRequirementAxis = Axis.None;

	[SerializeField]
	private string cantInteractTerm;

	[SerializeField]
	private string byPassInteractTerm;

	[HideInInspector]
	public bool hasBeenOpened;

	private static Network_Player localPlayer;

	public override void OnIsRayed()
	{
		if (!hasBeenOpened)
		{
			if (!CanStillBeInteractedWith)
			{
				displayText.HideDisplayTexts();
			}
			if (localPlayer == null)
			{
				localPlayer = Network.GetLocalPlayer();
			}
			if (!IsOnCorrectSide(localPlayer, correctAxis) && !IsOnCorrectSide(localPlayer, byPassRequirementAxis))
			{
				displayText.ShowText(Helper.GetTerm(cantInteractTerm));
			}
		}
		base.OnIsRayed();
	}

	public override bool CanPlayerInteract(Network_Player player, bool overwriteInventoryValidation = false)
	{
		if (hasBeenOpened)
		{
			return base.CanPlayerInteract(player, overwriteInventoryValidation);
		}
		if (IsOnCorrectSide(player, correctAxis) || IsOnCorrectSide(player, byPassRequirementAxis))
		{
			return base.CanPlayerInteract(player, overwriteInventoryValidation);
		}
		return false;
	}

	public override bool HasInteractRequirement(Network_Player player, bool overwriteInventoryValidation = false)
	{
		if (hasBeenOpened)
		{
			return true;
		}
		if (IsOnCorrectSide(player, correctAxis))
		{
			return base.HasInteractRequirement(player, overwriteInventoryValidation);
		}
		if (IsOnCorrectSide(player, byPassRequirementAxis))
		{
			return true;
		}
		return false;
	}

	public override void DisplayInteractText(Network_Player player)
	{
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		if (hasBeenOpened)
		{
			displayText.ShowText(Helper.GetTerm(byPassInteractTerm), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		}
		else if (IsOnCorrectSide(player, byPassRequirementAxis))
		{
			displayText.ShowText(Helper.GetTerm(byPassInteractTerm), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		}
		else
		{
			base.DisplayInteractText(player);
		}
	}

	public override void DisplayRequirementText(Network_Player player)
	{
		if (!hasBeenOpened)
		{
			if (!IsOnCorrectSide(player, correctAxis))
			{
				displayText.ShowText(Helper.GetTerm(cantInteractTerm));
			}
			else
			{
				base.DisplayRequirementText(player);
			}
		}
	}

	public override void Interact(Network_Player player, bool successFull)
	{
		base.Interact(player, successFull);
		if (currentObjectStateIndex == openDoorIndex)
		{
			hasBeenOpened = true;
		}
	}

	protected override void ConsumeRequiredItems(Network_Player player)
	{
		if (!hasBeenOpened && IsOnCorrectSide(player, correctAxis))
		{
			base.ConsumeRequiredItems(player);
		}
	}

	private bool IsOnCorrectSide(Network_Player player, Axis axis)
	{
		if (player == null)
		{
			return false;
		}
		Vector3 vector = transform.InverseTransformPoint(player.transform.position);
		switch (axis)
		{
		case Axis.X:
			if (vector.x >= 0f)
			{
				return true;
			}
			break;
		case Axis.Y:
			if (vector.y >= 0f)
			{
				return true;
			}
			break;
		case Axis.Z:
			if (vector.z >= 0f)
			{
				return true;
			}
			break;
		case Axis.NX:
			if (vector.z < 0f)
			{
				return true;
			}
			break;
		case Axis.NY:
			if (vector.z < 0f)
			{
				return true;
			}
			break;
		case Axis.NZ:
			if (vector.z < 0f)
			{
				return true;
			}
			break;
		case Axis.All:
			return true;
		case Axis.None:
			return false;
		}
		return false;
	}
}
