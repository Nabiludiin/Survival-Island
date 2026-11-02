using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RaycastInteractable))]
public class Bed : Block, IRaycastable
{
	[Header("Animation settings")]
	[SerializeField]
	private PlayerFullBodyAnimation fullbodyAnimation;

	[Header("Respawn settings")]
	[SerializeField]
	private Transform respawnPoint;

	[SerializeField]
	private Transform sleepPoint;

	[SerializeField]
	private BoxCollider interactBox;

	[SerializeField]
	private GameObject sheetSleeping;

	[SerializeField]
	private GameObject sheetNormal;

	[Range(1f, 100f)]
	[SerializeField]
	private float healthPercentage = 50f;

	[Range(1f, 100f)]
	[SerializeField]
	private float hungerPercentage = 50f;

	[Range(1f, 100f)]
	[SerializeField]
	private float thirstPercentage = 50f;

	[Header("Regen settings/night")]
	[Range(0f, 100f)]
	[SerializeField]
	private float healthRegen = 0.6f;

	[Range(0f, 100f)]
	[SerializeField]
	private float hungerDecay = 0.6f;

	[Range(0f, 100f)]
	[SerializeField]
	private float thirstDecay = 0.6f;

	private Network_Player currentPlayer;

	private CanvasHelper canvas;

	private Network_Player localPlayer;

	private bool showText;

	public float HealthRespawnPercentage => healthPercentage / 100f;

	public float HungerRespawnPercentage => hungerPercentage / 100f;

	public float ThirstRespawnPercentage => thirstPercentage / 100f;

	public float HealthRegen => healthRegen;

	public float HungerDecay => hungerDecay;

	public float ThirstDecay => thirstDecay;

	public PlayerFullBodyAnimation FullbodyAnimation => fullbodyAnimation;

	public Transform RespawnPoint => respawnPoint;

	public bool Busy => currentPlayer != null;

	protected override void Start()
	{
		base.Start();
		canvas = ComponentManager<CanvasHelper>.Value;
		localPlayer = ComponentManager<Network_Player>.Value;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		BedManager.RemoveBed(this);
		if (Busy)
		{
			DettachPlayer();
		}
	}

	public override void OnFinishedPlacement()
	{
		base.OnFinishedPlacement();
		BedManager.AddBed(this);
	}

	public void AttachPlayer(Network_Player player)
	{
		if (!Busy)
		{
			SetSheetState(playerSleeping: true);
			currentPlayer = player;
			if (player.IsLocalPlayer)
			{
				PlayerItemManager.IsBusy = true;
				currentPlayer.PersonController.SwitchControllerType(ControllerType.Ground);
				currentPlayer.PersonController.DetachFromLadders();
				currentPlayer.PersonController.IsMovementFree = false;
				currentPlayer.PlayerItemManager.SelectUsable(null);
				currentPlayer.PlayerScript.SetMouseLookScripts(canLook: false);
				currentPlayer.cameraBobAnimator.SetBool("InWater", value: false);
				currentPlayer.cameraBobAnimator.SetFloat("Speed", 0f);
				currentPlayer.PersonController.enabled = false;
				currentPlayer.Stats.buffManager.ClearBuffs();
				PlayerItemManager.IsBusy = true;
			}
			currentPlayer.PersonController.controller.enabled = false;
			currentPlayer.BedComponent.Sleeping = true;
			currentPlayer.Animator.SetAnimation(FullbodyAnimation);
			currentPlayer.transform.SetParent(SingletonGeneric<GameManager>.Singleton.lockedPivot);
			currentPlayer.transform.position = sleepPoint.position;
			currentPlayer.transform.rotation = sleepPoint.rotation;
			currentPlayer.PersonController.ResetPlayerPivotToOriginal();
		}
	}

	public void DettachPlayer()
	{
		if (Busy)
		{
			SetSheetState(playerSleeping: false);
			if (currentPlayer.UsingNoclip)
			{
				currentPlayer.flightCamera.Disable(teleportPlayerToCamera: false);
			}
			currentPlayer.BedComponent.Sleeping = false;
			currentPlayer.Animator.SetAnimation(PlayerFullBodyAnimation.None_0);
			currentPlayer.PlayerScript.incapacitatedCollider.enabled = false;
			currentPlayer.PersonController.IsMovementFree = true;
			currentPlayer.PersonController.enabled = true;
			if (currentPlayer.IsLocalPlayer)
			{
				PlayerItemManager.IsBusy = false;
				currentPlayer.PlayerScript.SetMouseLookScripts(canLook: true);
				currentPlayer.transform.position = RespawnPoint.position;
				currentPlayer.PlayerScript.MakeUnStuck();
				currentPlayer.Inventory.hotbar.ReselectCurrentSlot();
				currentPlayer.PersonController.ResetExternalVelocity();
			}
			currentPlayer.PersonController.controller.enabled = true;
			currentPlayer.transform.localScale = Vector3.one;
			currentPlayer.BedComponent.CurrentBed = null;
			currentPlayer = null;
		}
	}

	public void HandleAsleep()
	{
		if (currentPlayer == null || !currentPlayer.IsLocalPlayer)
		{
			return;
		}
		if (!BedManager.Slumbering && !localPlayer.PlayerScript.waitingForRespawn)
		{
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
			InputAction action2 = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
			showText = true;
			canvas.displayTextManager.ShowText(Helper.GetTerm("Game/StopSleep"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			if (!BedManager.BedTime())
			{
				canvas.displayTextManager.ShowText(Helper.GetTerm("Game/CantSleepDay"), 1, clearAllTexts: false);
			}
			if (!BedManager.AllPlayersSleeping())
			{
				canvas.displayTextManager.ShowText(Helper.GetTerm("Game/CantSleepPlayers"), 2, clearAllTexts: false);
			}
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact") || SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action2, "Cancel"))
			{
				canvas.displayTextManager.HideDisplayTexts();
				showText = false;
				if (Raft_Network.IsHost)
				{
					localPlayer.Network.RPC(new Message_NetworkBehaviour(Messages.Bed_Exit, localPlayer), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					localPlayer.BedComponent.StopSleep();
				}
				else
				{
					localPlayer.SendP2P(new Message_NetworkBehaviour(Messages.Bed_Exit, localPlayer), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
		}
		else
		{
			canvas.displayTextManager.HideDisplayTexts();
			showText = false;
		}
	}

	private void HandleAwake()
	{
		if (Busy)
		{
			showText = false;
			canvas.displayTextManager.HideDisplayTexts();
		}
		else
		{
			if (Busy)
			{
				return;
			}
			if (Helper.LocalPlayerIsWithinDistance(interactBox.transform.TransformPoint(interactBox.center), Player.UseDistance))
			{
				InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
				showText = true;
				canvas.displayTextManager.ShowText(Helper.GetTerm("Game/Sleep"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey, 0, 0, clearAllTexts: false);
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
				{
					canvas.displayTextManager.HideDisplayTexts();
					Message_Bed_Enter message = new Message_Bed_Enter(Messages.Bed_Enter, localPlayer, ObjectIndex);
					if (Raft_Network.IsHost)
					{
						localPlayer.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
						localPlayer.BedComponent.Sleep(this);
					}
					else
					{
						localPlayer.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					}
				}
			}
			else
			{
				canvas.displayTextManager.HideDisplayTexts();
				showText = false;
			}
		}
	}

	private void SetSheetState(bool playerSleeping)
	{
		if (sheetSleeping != null)
		{
			sheetSleeping.SetActiveSafe(playerSleeping);
		}
		if (sheetNormal != null)
		{
			sheetNormal.SetActiveSafe(!playerSleeping);
		}
	}

	public void OnIsRayed()
	{
		if (hasBeenPlaced && !PlayerItemManager.IsBusy && CanvasHelper.ActiveMenu == MenuType.None && localPlayer != null && localPlayer.BedComponent != null && localPlayer.RessurectComponent != null && !localPlayer.BedComponent.Sleeping && !localPlayer.RessurectComponent.IsCarrying && !localPlayer.RessurectComponent.BeingCarried)
		{
			HandleAwake();
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (showText)
		{
			canvas.displayTextManager.HideDisplayTexts();
			showText = false;
		}
	}
}
