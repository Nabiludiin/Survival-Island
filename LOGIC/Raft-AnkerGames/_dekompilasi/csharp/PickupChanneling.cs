using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(YieldHandler))]
[RequireComponent(typeof(RaycastInteractable))]
public class PickupChanneling : MonoBehaviour, IRaycastable
{
	[SerializeField]
	private MonoBehaviour_ID_Network networkBehaviourID;

	[SerializeField]
	[Range(1f, 5f)]
	private int itemsTakenPerTick = 1;

	public bool functionWithoutYield;

	public float pickupTime = 1f;

	public PickupItem pickupItem;

	[SerializeField]
	private Collider pickupCollider;

	[SerializeField]
	private bool collectLastYieldFromPickupScript;

	[SerializeField]
	private bool forcePickupLastYield = true;

	[SerializeField]
	[Tooltip("Child arrows are searched under this, leave null for no arrow check")]
	private Transform arrowParent;

	public bool triggerHandAnimation = true;

	public PlayerAnimation playerAnimation = PlayerAnimation.None;

	protected Network_Player player;

	private int channelStartItemCount;

	private float currentChannelProgress;

	private float lastPickupTime;

	private bool isPickingUp;

	public YieldHandler YieldHandler => pickupItem.yieldHandler;

	public int ItemsRemaining
	{
		get
		{
			if (!(YieldHandler == null))
			{
				return YieldHandler.Yield.Count;
			}
			return 0;
		}
	}

	public bool HasYield
	{
		get
		{
			if (ItemsRemaining <= 0)
			{
				return functionWithoutYield;
			}
			return true;
		}
	}

	private void Awake()
	{
		if (pickupCollider == null)
		{
			pickupCollider = GetComponent<Collider>();
		}
		if (networkBehaviourID == null)
		{
			networkBehaviourID = GetComponentInParent<MonoBehaviour_ID_Network>();
		}
	}

	private void Start()
	{
		player = ComponentManager<Network_Player>.Value;
	}

	private void OnDestroy()
	{
		player = null;
	}

	public void OnRayEnter()
	{
	}

	public void OnIsRayed()
	{
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			InitiateChannel();
		}
		else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(action, "Interact"))
		{
			CancelChannel();
		}
		if (isPickingUp)
		{
			if (HasYield)
			{
				ProgressChannel(Time.deltaTime);
			}
			else
			{
				CancelChannel();
			}
		}
	}

	public void OnRayExit()
	{
		CancelChannel();
	}

	public void RemoveYield(Network_Player player)
	{
		if (player.IsLocalPlayer)
		{
			for (int i = 0; i < itemsTakenPerTick; i++)
			{
				if (ItemsRemaining > 1)
				{
					YieldHandler.CollectYield(player);
					continue;
				}
				if (!collectLastYieldFromPickupScript)
				{
					YieldHandler.CollectYield(player);
				}
				else
				{
					RemoveLastObject(player, forcePickupLastYield);
				}
				UpdateUIChannelRing(0f);
			}
		}
		else
		{
			for (int j = 0; j < itemsTakenPerTick; j++)
			{
				YieldHandler.RemoveYield();
			}
		}
	}

	public void RemoveLastObject(Network_Player player, bool forcePickup)
	{
		if (pickupItem == null)
		{
			Debug.Log("Add pickup item ref on", gameObject);
		}
		player.PickupScript.PickupItemByType(pickupItem, forcePickup);
		PickupChildArrows(player);
	}

	public bool HandleNetworkMessageDeserialze(Message_PickupChannel msg)
	{
		if (Raft_Network.IsHost)
		{
			if (HasYield)
			{
				TakeItemFromYield(player.Network.GetPlayerFromID(msg.SteamID));
				return true;
			}
			return false;
		}
		TakeItemFromYield(player.Network.GetPlayerFromID(msg.SteamID));
		return false;
	}

	public void RestoreYield(int toYieldAmount)
	{
		YieldHandler.RestoreYield(toYieldAmount);
	}

	public void SetPickupColliderState(bool state)
	{
		if (pickupCollider != null)
		{
			pickupCollider.enabled = state;
		}
	}

	protected virtual void InitiateChannel()
	{
		channelStartItemCount = ItemsRemaining;
		if (channelStartItemCount == 0 && functionWithoutYield)
		{
			channelStartItemCount = 1;
		}
		currentChannelProgress = 0f;
		lastPickupTime = 0f;
		isPickingUp = true;
		if (player != null && playerAnimation != PlayerAnimation.None)
		{
			player.PlayerItemManager.HideItemInHand();
			player.Animator.SetAnimation(playerAnimation);
		}
	}

	protected virtual void ProgressChannel(float time)
	{
		currentChannelProgress += time;
		int num = Mathf.Clamp(itemsTakenPerTick, 0, channelStartItemCount);
		float num2 = currentChannelProgress / (pickupTime * ((float)channelStartItemCount / (float)num));
		HandleChannelProgress();
		if (num2 != 0f)
		{
			UpdateUIChannelRing(num2);
		}
	}

	protected virtual void CancelChannel()
	{
		isPickingUp = false;
		currentChannelProgress = 0f;
		lastPickupTime = 0f;
		UpdateUIChannelRing(currentChannelProgress);
		if (ComponentManager<CanvasHelper>.Value != null)
		{
			ComponentManager<CanvasHelper>.Value.ResetLoadAndAimCircleAnimationPulse();
		}
		if (player != null && playerAnimation != PlayerAnimation.None)
		{
			player.Inventory.hotbar.ReselectCurrentSlot();
		}
	}

	private void HandleChannelProgress()
	{
		if (currentChannelProgress - lastPickupTime >= pickupTime)
		{
			lastPickupTime = currentChannelProgress;
			TakeItemNetworked();
		}
	}

	private void TakeItemNetworked()
	{
		Message_PickupChannel message = new Message_PickupChannel(Messages.PickupChannel, player.Network.NetworkIDManager, player.steamID, networkBehaviourID.ObjectIndex);
		if (Raft_Network.IsHost)
		{
			TakeItemFromYield(player);
			player.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			player.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void TakeItemFromYield(Network_Player player)
	{
		RemoveYield(player);
		if (player.IsLocalPlayer)
		{
			if (ComponentManager<CanvasHelper>.Value != null)
			{
				ComponentManager<CanvasHelper>.Value.SetLoadCircleAnimationPulse();
				ComponentManager<CanvasHelper>.Value.SetAimCircleAnimationPulse();
			}
		}
		else
		{
			currentChannelProgress += pickupTime;
			lastPickupTime += pickupTime;
		}
		if (triggerHandAnimation)
		{
			player.Animator.SetAnimation(PlayerAnimation.Trigger_GrabItem);
		}
	}

	private void AddItemToInventory()
	{
		if (player != null && player.Inventory != null && YieldHandler != null)
		{
			YieldHandler.CollectYield(player);
		}
	}

	private void UpdateUIChannelRing(float normalizedFill)
	{
		if (ComponentManager<CanvasHelper>.Value != null)
		{
			ComponentManager<CanvasHelper>.Value.SetLoadCircle(normalizedFill);
		}
	}

	private void PickupChildArrows(Network_Player player)
	{
		if (arrowParent == null || player == null || !player.IsLocalPlayer)
		{
			return;
		}
		Arrow[] componentsInChildren = arrowParent.GetComponentsInChildren<Arrow>();
		if (componentsInChildren == null)
		{
			return;
		}
		Arrow[] array = componentsInChildren;
		foreach (Arrow arrow in array)
		{
			if (!(arrow == null))
			{
				player.PickupScript.PickupItem(arrow.pickupItem.PickupItem, forcePickup: true);
			}
		}
	}
}
