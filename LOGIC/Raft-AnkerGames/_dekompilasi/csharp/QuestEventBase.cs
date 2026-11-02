using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class QuestEventBase : MonoBehaviour_ID_Network
{
	[Serializable]
	public class InteractRequirementItem
	{
		public Item_Base inventoryItem;

		public SO_QuestItem questItem;

		public int amount;

		public bool IsQuestItem => questItem != null;

		public string GetRequirementText()
		{
			string text = string.Empty;
			if (questItem != null)
			{
				text = text + amount + " " + questItem.DisplayName;
			}
			if (inventoryItem != null)
			{
				if (questItem != null)
				{
					text += "\n";
				}
				text = text + amount + " " + inventoryItem.settings_Inventory.DisplayName;
			}
			return text;
		}
	}

	public Action<int, bool, Network_Player, bool> OnInteract;

	public Action<int, bool, Network_Player, bool> OnInteractFail;

	public List<InteractRequirementItem> interactRequirement = new List<InteractRequirementItem>();

	public List<InteractRequirementItem> consumedItemsOnInteract = new List<InteractRequirementItem>();

	private Raft_Network _network;

	public int objectStateMaxIndex = 2;

	public int currentObjectStateIndex;

	public bool isStateIndexLooping = true;

	[SerializeField]
	private bool turnOfColliderWhenNoInteractionIsAvailable = true;

	private QuestItemManager questItemManager;

	protected Raft_Network Network
	{
		get
		{
			if (_network == null)
			{
				_network = ComponentManager<Raft_Network>.Value;
			}
			return _network;
		}
	}

	public bool CanStillBeInteractedWith
	{
		get
		{
			if (CurrentObjectStateIndex >= objectStateMaxIndex)
			{
				if (currentObjectStateIndex == objectStateMaxIndex)
				{
					return isStateIndexLooping;
				}
				return false;
			}
			return true;
		}
	}

	public bool IsAtMaxStateIndex => CurrentObjectStateIndex == objectStateMaxIndex;

	public int CurrentObjectStateIndex
	{
		get
		{
			return currentObjectStateIndex;
		}
		set
		{
			if (value < 0)
			{
				currentObjectStateIndex = 0;
			}
			else if (value > objectStateMaxIndex)
			{
				if (isStateIndexLooping)
				{
					currentObjectStateIndex = 0;
				}
				else
				{
					currentObjectStateIndex = objectStateMaxIndex;
				}
			}
			else
			{
				currentObjectStateIndex = value;
			}
		}
	}

	protected virtual void Start()
	{
		questItemManager = ComponentManager<QuestItemManager>.Value;
		_network = ComponentManager<Raft_Network>.Value;
	}

	public virtual void ResetFromCheat()
	{
		CurrentObjectStateIndex = 0;
		Collider component = GetComponent<Collider>();
		if (component != null)
		{
			component.enabled = true;
		}
		SendOnInteractEvent(null);
	}

	public virtual void Interact(Network_Player player, bool successFull)
	{
		if (successFull)
		{
			if (Raft_Network.IsHost)
			{
				CurrentObjectStateIndex++;
			}
			ConsumeRequiredItems(player);
		}
		if (successFull)
		{
			SendOnInteractEvent(player);
		}
		else
		{
			SendOnInteractFailEvent(player);
		}
		RefreshColliderState();
	}

	public virtual bool CanPlayerInteract(Network_Player player, bool overwriteInventoryValidation = false)
	{
		if (player == null)
		{
			return false;
		}
		if (CanStillBeInteractedWith)
		{
			return HasInteractRequirement(player, overwriteInventoryValidation);
		}
		return false;
	}

	public virtual bool HasInteractRequirement(Network_Player player, bool hasInventoryValidation = false)
	{
		if (interactRequirement.Count > 0)
		{
			foreach (InteractRequirementItem item in interactRequirement)
			{
				if (item.questItem != null && !QuestItemManager.HasQuestItem(item.questItem.questItemType, item.amount))
				{
					return false;
				}
				if (hasInventoryValidation || !(item.inventoryItem != null))
				{
					continue;
				}
				if (player == null)
				{
					return false;
				}
				if (player.IsLocalPlayer)
				{
					if (player.Inventory.GetItemCount(item.inventoryItem) < item.amount)
					{
						return false;
					}
				}
				else if (!player.IsLocalPlayer)
				{
					return false;
				}
			}
			return true;
		}
		return true;
	}

	public virtual bool HasInteractRequirementInventoryOnly(Network_Player player)
	{
		if (interactRequirement.Count > 0)
		{
			foreach (InteractRequirementItem item in interactRequirement)
			{
				if (!(item.inventoryItem != null))
				{
					continue;
				}
				if (player == null)
				{
					return false;
				}
				if (player.IsLocalPlayer)
				{
					if (player.Inventory.GetItemCount(item.inventoryItem) < item.amount)
					{
						return false;
					}
				}
				else if (!player.IsLocalPlayer)
				{
					return false;
				}
			}
			return true;
		}
		return true;
	}

	public void SendOnInteractEvent(Network_Player p_player, bool atStart = false)
	{
		OnInteract?.Invoke(CurrentObjectStateIndex, atStart, p_player, arg4: true);
	}

	public void SendOnInteractFailEvent(Network_Player p_player, bool atStart = false)
	{
		OnInteractFail?.Invoke(CurrentObjectStateIndex, atStart, p_player, arg4: false);
	}

	public void RestoreObjectStateIndex(int objectStateIndex)
	{
		CurrentObjectStateIndex = 0;
		for (int i = 1; i <= objectStateIndex; i++)
		{
			CurrentObjectStateIndex = i;
			SendOnInteractEvent(null, atStart: true);
		}
		CurrentObjectStateIndex = objectStateIndex;
		RefreshColliderState();
	}

	public void SetNewObjectStateIndex(int newObjectStateIndex)
	{
		CurrentObjectStateIndex = newObjectStateIndex;
		SendOnInteractEvent(null);
		RefreshColliderState();
	}

	protected virtual void ConsumeRequiredItems(Network_Player player)
	{
		if (Raft_Network.IsHost && consumedItemsOnInteract.ContainsItems())
		{
			foreach (InteractRequirementItem item in consumedItemsOnInteract)
			{
				if (item.questItem != null)
				{
					QuestItemManager.AddQuestItem(item.questItem.questItemType, -item.amount, sendNotification: false);
				}
			}
			questItemManager.SendModifiedQuestItemsMessage();
		}
		if (!consumedItemsOnInteract.ContainsItems())
		{
			return;
		}
		foreach (InteractRequirementItem item2 in consumedItemsOnInteract)
		{
			if (item2.inventoryItem != null && player.IsLocalPlayer)
			{
				player.Inventory.RemoveItem(item2.inventoryItem.UniqueName, item2.amount);
			}
		}
	}

	public virtual void SendOnInteractNetwork(Network_Player interactingPlayer, bool forceSuccess = false)
	{
		ulong steamID = ((interactingPlayer == null) ? ((Network_UserId)0uL) : interactingPlayer.steamID);
		bool flag = HasInteractRequirementInventoryOnly(interactingPlayer);
		Message_QuestEventInteract message_QuestEventInteract = new Message_QuestEventInteract(Messages.QuestEventInteract, Network.NetworkIDManager, ObjectIndex, CurrentObjectStateIndex, forceSuccess, flag, steamID);
		if (Raft_Network.IsHost)
		{
			message_QuestEventInteract.interactSuccess = forceSuccess;
			if (!forceSuccess)
			{
				message_QuestEventInteract.interactSuccess = CanPlayerInteract(interactingPlayer, flag);
			}
			Interact(interactingPlayer, message_QuestEventInteract.interactSuccess);
			message_QuestEventInteract.interactIndex = CurrentObjectStateIndex;
		}
		if (Raft_Network.IsHost)
		{
			Network.RPC(message_QuestEventInteract, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			Network.SendP2P(Network.HostID, message_QuestEventInteract, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.QuestEventInteract)
		{
			Message_QuestEventInteract message_QuestEventInteract = msg as Message_QuestEventInteract;
			if (Raft_Network.IsHost)
			{
				Network_Player playerFromID = Network.GetPlayerFromID(message_QuestEventInteract.SteamID);
				bool flag = CanPlayerInteract(playerFromID, message_QuestEventInteract.hasInventoryRequirement);
				Interact(playerFromID, flag);
				message_QuestEventInteract.interactIndex = CurrentObjectStateIndex;
				message_QuestEventInteract.interactSuccess = flag;
				return true;
			}
			CurrentObjectStateIndex = message_QuestEventInteract.interactIndex;
			Interact(Network.GetPlayerFromID(message_QuestEventInteract.SteamID), message_QuestEventInteract.interactSuccess);
			return false;
		}
		return base.Deserialize(msg, remoteID);
	}

	private void RefreshColliderState()
	{
		if (turnOfColliderWhenNoInteractionIsAvailable)
		{
			Collider component = GetComponent<Collider>();
			if (component != null)
			{
				component.enabled = CanStillBeInteractedWith;
			}
		}
	}

	private void OdinButton_InteractPlayer()
	{
		if (Application.isEditor && !Application.isPlaying)
		{
			Debug.Log("Can't interact: Game needs to be running");
		}
		else
		{
			SendOnInteractNetwork(Network.GetLocalPlayer());
		}
	}

	private void OdinButton_Interact()
	{
		if (Application.isEditor && !Application.isPlaying)
		{
			Debug.Log("Can't interact: Game needs to be running");
		}
		else
		{
			SendOnInteractNetwork(null);
		}
	}

	private void OdinButton_InteractSinglePlayer()
	{
		if (Application.isEditor && !Application.isPlaying)
		{
			Debug.Log("Can't interact: Game needs to be running");
		}
		else
		{
			Interact(null, successFull: true);
		}
	}
}
