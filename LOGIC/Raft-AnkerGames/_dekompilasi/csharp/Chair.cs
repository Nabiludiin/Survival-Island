using UnityEngine;

public class Chair : MonoBehaviour_ID_Network, IBlockPlaced
{
	[SerializeField]
	private PlayerSeat multiplePlayerSeat;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkIDManager.RemoveNetworkID(this, typeof(Chair));
	}

	void IBlockPlaced.OnBlockPlacedInterface()
	{
		NetworkIDManager.AddNetworkID(this, typeof(Chair));
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Block(RGDType.Block, GetComponent<Block>());
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		Messages type = msg.Type;
		if ((uint)(type - 260) <= 1u)
		{
			return multiplePlayerSeat.HandleDeserialize(msg, remoteID);
		}
		return false;
	}
}
