using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class RessurectComponent : MonoBehaviour
{
	[SerializeField]
	private Network_Player playerNetwork;

	[SerializeField]
	private Transform firstPersonCarryTransform;

	[SerializeField]
	private Transform thirdPersonCarryTransform;

	[SerializeField]
	private bool isCarrying;

	[SerializeField]
	private bool beingCarried;

	private bool aimingAtDead;

	private DisplayTextManager displayTextManager;

	[SerializeField]
	private Network_Player carriedPlayer;

	[SerializeField]
	private Network_Player carriedByPlayer;

	private Network_Player incapacitatedPlayerAtCursor;

	public bool BeingCarried
	{
		get
		{
			return beingCarried;
		}
		set
		{
			beingCarried = value;
		}
	}

	public bool IsCarrying => isCarrying;

	public Network_Player CarriedPlayer => carriedPlayer;

	public Network_Player CarriedByPlayer
	{
		get
		{
			return carriedByPlayer;
		}
		set
		{
			carriedByPlayer = value;
		}
	}

	public Network_Player IncapacitatedPlayerAtCursor => incapacitatedPlayerAtCursor;

	private void Start()
	{
		displayTextManager = ComponentManager<DisplayTextManager>.Value;
	}

	private void Update()
	{
		if (!IsCarrying)
		{
			CheckForPlayerToCarry();
		}
		else
		{
			HandleCarrying();
		}
	}

	public bool StartCarryingPlayer(Network_Player otherPlayer)
	{
		if (otherPlayer == null)
		{
			Debug.LogWarning("Can't carry a player which is null");
			return false;
		}
		if (playerNetwork.IsLocalPlayer)
		{
			PlayerItemManager.IsBusy = true;
			playerNetwork.currentModel.thirdPersonSettings.ThirdPersonState = false;
			playerNetwork.PlayerItemManager.HideItemInHand();
		}
		otherPlayer.RessurectComponent.carriedByPlayer = playerNetwork;
		carriedPlayer = otherPlayer;
		isCarrying = true;
		playerNetwork.Animator.SetAnimation(PlayerAnimation.Index_7_CarryPlayer);
		otherPlayer.RessurectComponent.beingCarried = true;
		otherPlayer.Animator.anim.SetBool("Carried", value: true);
		otherPlayer.PersonController.enabled = false;
		otherPlayer.PersonController.IsMovementFree = false;
		otherPlayer.PersonController.waterFloat.setToSurface = false;
		Transform parent = (playerNetwork.IsLocalPlayer ? firstPersonCarryTransform : thirdPersonCarryTransform);
		otherPlayer.transform.SetParent(parent);
		otherPlayer.transform.localPosition = Vector3.zero;
		otherPlayer.transform.localEulerAngles = Vector3.zero;
		otherPlayer.transform.localEulerAngles = Vector3.zero;
		otherPlayer.playerPivot.localEulerAngles = Vector3.zero;
		return true;
	}

	public void StopCarryingPlayer(bool manipulatePosition)
	{
		if (playerNetwork.IsLocalPlayer)
		{
			PlayerItemManager.IsBusy = false;
			displayTextManager.HideDisplayTexts();
		}
		Item_Base currentItemInHand = playerNetwork.PlayerItemManager.useItemController.GetCurrentItemInHand();
		if (currentItemInHand != null && currentItemInHand.settings_usable.AnimationOnSelect != PlayerAnimation.None)
		{
			playerNetwork.Animator.SetAnimation(currentItemInHand.settings_usable.AnimationOnSelect);
		}
		else
		{
			playerNetwork.Animator.SetAnimation(PlayerAnimation.Index_0_Idle, overrideIndex: true, triggering: false);
		}
		isCarrying = false;
		playerNetwork.Inventory.hotbar.ReselectCurrentSlot();
		if (!(carriedPlayer != null))
		{
			return;
		}
		carriedPlayer.RessurectComponent.beingCarried = false;
		carriedPlayer.Animator.anim.SetBool("Carried", value: false);
		carriedPlayer.transform.SetParent(playerNetwork.transform.parent);
		if (manipulatePosition)
		{
			if (playerNetwork.transform.parent != null)
			{
				carriedPlayer.transform.position = playerNetwork.transform.position;
				carriedPlayer.PersonController.SetNetworkPosition(playerNetwork.transform.position);
			}
			carriedPlayer.transform.eulerAngles = Vector3.zero;
		}
		carriedPlayer.PersonController.IsMovementFree = true;
		carriedPlayer.PersonController.enabled = true;
		carriedPlayer.PersonController.SwitchControllerType(playerNetwork.PersonController.controllerType);
		carriedPlayer.PersonController.waterFloat.setToSurface = playerNetwork.PersonController.waterFloat.setToSurface;
		if (carriedPlayer.PlayerScript.diedUnderWater && playerNetwork.PersonController.controllerType == ControllerType.Ground)
		{
			carriedPlayer.PlayerScript.diedUnderWater = false;
		}
		if (carriedPlayer.PersonController.controllerType == ControllerType.Water && !carriedPlayer.PersonController.waterFloat.setToSurface)
		{
			carriedPlayer.PlayerScript.diedUnderWater = true;
		}
		carriedPlayer.RessurectComponent.CarriedByPlayer = null;
		carriedPlayer = null;
	}

	public bool PlaceInBed(Bed bed)
	{
		if (bed == null)
		{
			Debug.LogError("Can't placed player in bed because the assigned bed is null");
			return false;
		}
		Network_Player network_Player = carriedPlayer;
		StopCarryingPlayer(manipulatePosition: false);
		network_Player.PlayerScript.StartRespawn(bed, clearInventory: false);
		return true;
	}

	public bool CanCarryPlayer(Network_Player otherPlayer)
	{
		if (IsCarrying)
		{
			return false;
		}
		if (!otherPlayer.PlayerScript.IsDead)
		{
			return false;
		}
		if (otherPlayer.RessurectComponent.BeingCarried)
		{
			return false;
		}
		if (otherPlayer.BedComponent.Sleeping)
		{
			return false;
		}
		return true;
	}

	private void CheckForPlayerToCarry()
	{
		if (!playerNetwork.IsLocalPlayer || CanvasHelper.ActiveMenu != MenuType.None || PlayerItemManager.IsBusy)
		{
			return;
		}
		if (!Helper.HitAtCursor(out var hit, Player.UseDistance, LayerMasks.MASK_RemotePlayer))
		{
			if (aimingAtDead)
			{
				aimingAtDead = false;
				displayTextManager.HideDisplayTexts();
			}
			return;
		}
		incapacitatedPlayerAtCursor = hit.transform.GetComponentInParent<Network_Player>();
		if (incapacitatedPlayerAtCursor == null)
		{
			if (aimingAtDead)
			{
				aimingAtDead = false;
				displayTextManager.HideDisplayTexts();
			}
		}
		else if (CanCarryPlayer(incapacitatedPlayerAtCursor))
		{
			aimingAtDead = true;
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
			displayTextManager.ShowText(Helper.GetTerm("Game/Carry", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				aimingAtDead = false;
				displayTextManager.HideDisplayTexts();
				Message_NetworkBehaviour_SteamID message = new Message_NetworkBehaviour_SteamID(Messages.RessurectComponent_StartCarry, playerNetwork, incapacitatedPlayerAtCursor.steamID);
				if (Raft_Network.IsHost)
				{
					playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					StartCarryingPlayer(incapacitatedPlayerAtCursor);
				}
				else
				{
					playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
		}
		else
		{
			aimingAtDead = false;
			displayTextManager.HideDisplayTexts();
		}
	}

	private void HandleCarrying()
	{
		if (carriedPlayer == null)
		{
			StopCarryingPlayer(manipulatePosition: false);
			return;
		}
		carriedPlayer.Animator.anim.SetBool("InWater", playerNetwork.PersonController.controllerType == ControllerType.Water);
		carriedPlayer.Animator.anim.SetFloat("MoveSpeed", playerNetwork.PersonController.moving ? 1f : 0f);
		if (!playerNetwork.IsLocalPlayer || CanvasHelper.ActiveMenu != MenuType.None || !playerNetwork.IsLocalPlayer)
		{
			return;
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		displayTextManager.ShowText(Helper.GetTerm("DeathMenu/FindBed"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		if (Helper.HitAtCursor(out var hit, Player.UseDistance, LayerMasks.MASK_Block) && hit.transform.tag == "Bed")
		{
			Bed component = hit.transform.GetComponent<Bed>();
			if (!(component != null))
			{
				return;
			}
			if (!component.Busy)
			{
				displayTextManager.ShowText(Helper.GetTerm("Game/PlaceInBed", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
				{
					displayTextManager.HideDisplayTexts();
					Message_Bed_Enter message = new Message_Bed_Enter(Messages.RessurectComponent_PlaceInBed, playerNetwork, component.ObjectIndex);
					if (Raft_Network.IsHost)
					{
						playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
						PlaceInBed(component);
					}
					else
					{
						playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					}
				}
			}
			else
			{
				displayTextManager.HideDisplayTexts();
			}
		}
		else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			displayTextManager.HideDisplayTexts();
			Message_NetworkBehaviour message2 = new Message_NetworkBehaviour(Messages.RessurectComponent_StopCarry, playerNetwork);
			if (Raft_Network.IsHost)
			{
				playerNetwork.Network.RPC(message2, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				StopCarryingPlayer(manipulatePosition: true);
			}
			else
			{
				playerNetwork.SendP2P(message2, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}
}
