using System;
using Steamworks;
using UnityEngine;

[RequireComponent(typeof(Throwable))]
public class SharkBaitThrower : MonoBehaviour
{
	[SerializeField]
	private Item_Base sharkbaitItem;

	[SerializeField]
	private SharkBait sharkBaitPrefab;

	[SerializeField]
	private ParticleSystem waterSplash;

	private Throwable throwable;

	private PlayerInventory inventory;

	[HideInInspector]
	public SharkBait currentSharkBait;

	private PlayerAnimator playerAnimator;

	private Network_Player playerNetwork;

	private void Awake()
	{
		this.throwable = GetComponent<Throwable>();
		Throwable throwable = this.throwable;
		throwable.Action_OnThrow = (Action)Delegate.Combine(throwable.Action_OnThrow, new Action(OnThrow));
		Throwable throwable2 = this.throwable;
		throwable2.Action_OnWaterHit = (Action)Delegate.Combine(throwable2.Action_OnWaterHit, new Action(OnHitWater));
		if (this.throwable != null)
		{
			currentSharkBait = this.throwable.throwableObject.GetComponent<SharkBait>();
		}
	}

	private void Start()
	{
		playerNetwork = GetComponentInParent<Network_Player>();
		playerAnimator = playerNetwork.Animator;
		UseItemController useItemController = playerNetwork.PlayerItemManager.useItemController;
		useItemController.OnSelectItem = (Action<Item_Base>)Delegate.Combine(useItemController.OnSelectItem, new Action<Item_Base>(OnSelectItem));
		UseItemController useItemController2 = playerNetwork.PlayerItemManager.useItemController;
		useItemController2.OnDeselectItem = (Action<Item_Base>)Delegate.Combine(useItemController2.OnDeselectItem, new Action<Item_Base>(OnDeSelectItem));
	}

	private void Update()
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		throwable.enabled = throwable.throwableObject.gameObject.activeInHierarchy;
		if (playerNetwork.IsLocalPlayer && throwable.CanThrow && throwable.enabled && CanvasHelper.ActiveMenu == MenuType.None && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(throwable.ThrowableAction, "LMB"))
		{
			playerNetwork.Animator.anim.SetBool("HookThrow", value: true);
		}
		if (!playerNetwork.IsLocalPlayer && !throwable.InWater && !throwable.InHand)
		{
			throwable.LookForWaterHitRemoteClient();
		}
		if (!playerNetwork.IsLocalPlayer)
		{
			return;
		}
		if (currentSharkBait == null && throwable.throwableObject != null)
		{
			currentSharkBait = throwable.throwableObject.GetComponent<SharkBait>();
		}
		if (currentSharkBait != null)
		{
			int value = LayerMasks.MASK_Default.value;
			if (!playerNetwork.currentModel.thirdPersonSettings.ThirdPersonModel && throwable.InHand)
			{
				value = LayerMasks.MASK_HandCamera.value;
			}
			currentSharkBait.gameObject.SetLayerRecursivly(value);
		}
	}

	public void OnSelectItem(Item_Base item)
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (item.UniqueIndex == sharkbaitItem.UniqueIndex)
		{
			playerNetwork.Animator.anim.SetBool("HookInHand", value: true);
		}
	}

	public void OnDeSelectItem(Item_Base item)
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (item.UniqueIndex == sharkbaitItem.UniqueIndex)
		{
			playerNetwork.Animator.anim.SetBool("HookInHand", value: false);
		}
	}

	private void OnThrow()
	{
		playerAnimator.anim.SetBool("HookThrow", value: false);
		if (playerNetwork.IsLocalPlayer)
		{
			Message_SharkBait message_SharkBait = new Message_SharkBait(Messages.Shark_Bait, playerNetwork);
			if (Raft_Network.IsHost)
			{
				currentSharkBait.ObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
				message_SharkBait.sharkBaitObjectIndex = currentSharkBait.ObjectIndex;
				BlockCreator.GetPlacedBlocks().Add(currentSharkBait);
				currentSharkBait.gameObject.SetLayerRecursivly(LayerMasks.MASK_Default.value);
				playerNetwork.Network.RPC(message_SharkBait, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				playerNetwork.SendP2P(message_SharkBait, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	private void OnHitWater()
	{
		currentSharkBait.OnHitWater();
		waterSplash.transform.eulerAngles = Vector3.zero;
		waterSplash.transform.position = currentSharkBait.transform.position;
		waterSplash.Play();
		currentSharkBait = null;
		CreateNewThrowable();
		if (playerNetwork.IsLocalPlayer)
		{
			playerNetwork.Inventory.RemoveSelectedHotSlotItem(1);
		}
	}

	private void CreateNewThrowable()
	{
		if (currentSharkBait == null)
		{
			SharkBait sharkBait = UnityEngine.Object.Instantiate(sharkBaitPrefab, transform.position, Quaternion.identity, transform);
			throwable.waterFloat = sharkBait.transform.GetComponent<WaterFloatSemih2>();
			throwable.throwableObject = sharkBait.transform;
			throwable.throwableObjectBody = sharkBait.transform.GetComponent<Rigidbody>();
			throwable.CanThrow = true;
			throwable.PickupThrowable(setDirty: false);
			currentSharkBait = sharkBait;
		}
	}
}
