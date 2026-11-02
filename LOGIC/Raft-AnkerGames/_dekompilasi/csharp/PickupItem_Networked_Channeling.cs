using UnityEngine;

[RequireComponent(typeof(PickupChanneling))]
public class PickupItem_Networked_Channeling : PickupItem_Networked
{
	private PickupChanneling pickupChannelingScript;

	private void Awake()
	{
		pickupChannelingScript = GetComponent<PickupChanneling>();
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.PickupChannel)
		{
			return pickupChannelingScript.HandleNetworkMessageDeserialze(msg as Message_PickupChannel);
		}
		return base.Deserialize(msg, remoteID);
	}
}
