using System;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Anchor_Throwable : MonoBehaviour
{
	public Anchor_Throwable_Stand anchor_stand;

	[SerializeField]
	private Anchor anchorPrefab;

	public Transform anchorParent;

	public Throwable throwable;

	private Network_Player playerNetwork;

	private CanvasHelper canvas;

	private float holdRemoveValue;

	private bool isShowingText;

	[SerializeField]
	private InputActionReference inputInteract;

	private void Awake()
	{
		if (this.throwable != null)
		{
			this.throwable.setThrowableToSurface = false;
			Throwable throwable = this.throwable;
			throwable.Action_OnThrow = (Action)Delegate.Combine(throwable.Action_OnThrow, new Action(OnThrow));
		}
	}

	private void Start()
	{
		canvas = ComponentManager<CanvasHelper>.Value;
		playerNetwork = GetComponentInParent<Network_Player>();
	}

	private void OnDisable()
	{
		if (playerNetwork != null && playerNetwork.IsLocalPlayer)
		{
			PlayerItemManager.throwableAnchorBusy = false;
		}
	}

	private void OnDestroy()
	{
		if (playerNetwork != null && playerNetwork.IsLocalPlayer)
		{
			PlayerItemManager.throwableAnchorBusy = false;
		}
		if (anchor_stand != null)
		{
			anchor_stand.DisconnectThrowableWithStand();
		}
	}

	private void Update()
	{
		if (!playerNetwork.IsLocalPlayer)
		{
			return;
		}
		playerNetwork.PlayerItemManager.useItemController.GetCurrentItemInHand();
		PlayerItemManager.throwableAnchorBusy = false;
		if (anchor_stand != null)
		{
			PlayerItemManager.IsBusy = true;
			PlayerItemManager.throwableAnchorBusy = true;
			int value = LayerMasks.MASK_Default.value;
			if (playerNetwork.IsLocalPlayer && !playerNetwork.currentModel.thirdPersonSettings.ThirdPersonModel && throwable.InHand)
			{
				value = LayerMasks.MASK_HandCamera.value;
			}
			if (throwable != null && throwable.throwableObject != null)
			{
				throwable.throwableObject.gameObject.SetLayerSafe(value);
			}
		}
		else if (CanvasHelper.ActiveMenu == MenuType.None && !PlayerItemManager.IsBusy)
		{
			if (Helper.HitAtCursor(out var hit, Player.UseDistance, LayerMasks.MASK_Item) && hit.transform.tag == "ThrowableAnchor")
			{
				Anchor_Throwable_Stand component = hit.transform.GetComponent<Anchor_Throwable_Stand>();
				if (component.FullyAnchored)
				{
					canvas.displayTextManager.ShowText(Helper.GetTerm("Game/Remove"), inputInteract.action.GetBindingDisplayString(), MyInput.Keybinds["Remove"].MainKey);
					isShowingText = true;
					if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputInteract, "Remove"))
					{
						holdRemoveValue += Time.deltaTime;
						canvas.SetLoadCircle(holdRemoveValue);
						if (holdRemoveValue >= 1f)
						{
							canvas.SetLoadCircle(state: false);
							canvas.displayTextManager.HideDisplayTexts(0);
							isShowingText = false;
							component.DestroyStand();
						}
					}
					else if (holdRemoveValue > 0f)
					{
						canvas.SetLoadCircle(state: false);
						holdRemoveValue = 0f;
					}
				}
				else if (anchor_stand == null && !component.IsBusy() && !component.HasThrownAnchor)
				{
					canvas.displayTextManager.ShowText(Helper.GetTerm("Game/PickUp"), inputInteract.action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey, 0, 0, clearAllTexts: false);
					isShowingText = true;
					if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputInteract, "Interact"))
					{
						canvas.displayTextManager.HideDisplayTexts();
						isShowingText = false;
						Message_NetworkBehaviour_SteamID message = new Message_NetworkBehaviour_SteamID(Messages.ConnectThrowableAnchorStand, component, playerNetwork.Network.GetIDFromPlayer(playerNetwork));
						if (Raft_Network.IsHost)
						{
							playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
							component.ConnectStandWithThrowable(playerNetwork);
						}
						else
						{
							playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
						}
					}
				}
				else
				{
					isShowingText = false;
					canvas.displayTextManager.HideDisplayTexts(0);
				}
			}
			else
			{
				if (holdRemoveValue > 0f)
				{
					canvas.SetLoadCircle(state: false);
					holdRemoveValue = 0f;
				}
				if (isShowingText)
				{
					isShowingText = false;
					canvas.displayTextManager.HideDisplayTexts(0);
				}
			}
		}
		else if (isShowingText)
		{
			isShowingText = false;
			canvas.displayTextManager.HideDisplayTexts(0);
		}
	}

	private void OnThrow()
	{
		playerNetwork.Animator.anim.SetBool("HookThrow", value: false);
		playerNetwork.Animator.anim.SetBool("HookInHand", value: false);
		if (anchor_stand == null)
		{
			Debug.LogError("Anchor stand is null(missing)", gameObject);
		}
		if (throwable == null)
		{
			Debug.LogError("Throwable is null", gameObject);
		}
		else if (throwable.throwableObject == null)
		{
			Debug.LogError("Throwable object is null", gameObject);
		}
		anchor_stand.OnThrowableThrow(throwable.throwableObject.GetComponent<Anchor>());
		anchor_stand = null;
		Anchor anchor = UnityEngine.Object.Instantiate(anchorPrefab, anchorParent.position, Quaternion.identity, anchorParent);
		throwable.throwableObject = anchor.transform;
		throwable.throwableObjectBody = anchor.transform.GetComponent<Rigidbody>();
		throwable.waterFloat = anchor.transform.GetComponent<WaterFloatSemih2>();
		throwable.CanThrow = true;
		throwable.throwableObjectBody.isKinematic = true;
		throwable.PickupThrowable();
		throwable.ResetCanThrow();
	}
}
