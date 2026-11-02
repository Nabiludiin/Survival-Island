using System.Collections.Generic;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlueprintComponent : MonoBehaviour, LocalizeEventInterface
{
	[SerializeField]
	private Image blueprintImage;

	[SerializeField]
	private TextMeshProUGUI nameText;

	[SerializeField]
	private TextMeshProUGUI descText;

	[SerializeField]
	private List<BlueprintConnection> blueprintConnections = new List<BlueprintConnection>();

	private Network_Player playerNetwork;

	private BlueprintConnection currentCon;

	public void OnLocalizeEvent()
	{
		if (currentCon != null)
		{
			SetBlueprint(currentCon);
		}
	}

	public void OnSelect()
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (!playerNetwork.IsLocalPlayer || blueprintImage == null)
		{
			return;
		}
		Slot selectedHotbarSlot = playerNetwork.Inventory.GetSelectedHotbarSlot();
		if (!(selectedHotbarSlot != null) || !selectedHotbarSlot.HasValidItemInstance())
		{
			return;
		}
		ItemInstance itemInstance = selectedHotbarSlot.itemInstance;
		BlueprintConnection connectionFromItem = GetConnectionFromItem(itemInstance.baseItem);
		if (connectionFromItem != null && (currentCon == null || connectionFromItem != currentCon))
		{
			Message_Item message = new Message_Item(Messages.Blueprint_Switch, playerNetwork, connectionFromItem.blueprintItem);
			if (Raft_Network.IsHost)
			{
				playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				SetBlueprint(connectionFromItem);
			}
			else
			{
				playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public void SetBlueprint(BlueprintConnection con)
	{
		if (con != null)
		{
			currentCon = con;
			blueprintImage.sprite = con.sprite;
			nameText.text = ((con.actualItem != null) ? con.actualItem.settings_Inventory.DisplayName : string.Empty);
			descText.text = ((con.actualItem != null) ? con.actualItem.settings_Inventory.Description : string.Empty);
		}
	}

	public void SetBlueprint(Message_Item msg)
	{
		BlueprintConnection connectionFromItem = GetConnectionFromItem(ItemManager.GetItemByIndex(msg.itemIndex));
		SetBlueprint(connectionFromItem);
	}

	private BlueprintConnection GetConnectionFromItem(Item_Base item)
	{
		if (item == null)
		{
			return null;
		}
		for (int i = 0; i < blueprintConnections.Count; i++)
		{
			BlueprintConnection blueprintConnection = blueprintConnections[i];
			if (blueprintConnection != null && !(blueprintConnection.blueprintItem == null) && blueprintConnection.blueprintItem.UniqueIndex == item.UniqueIndex)
			{
				return blueprintConnection;
			}
		}
		return null;
	}
}
