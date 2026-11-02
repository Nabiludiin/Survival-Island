public class Placeable_Tank : MonoBehaviour_ID_Network
{
	public Tank tank;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkIDManager.RemoveNetworkID(this);
	}

	public void OnBlockPlaced()
	{
		NetworkIDManager.AddNetworkID(this);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		Messages type = msg.Type;
		if ((uint)(type - 172) <= 1u)
		{
			Message_Tank msgTank = msg as Message_Tank;
			if (msg.Type == Messages.Tank)
			{
				return tank.OnTankModifyMessage(msgTank);
			}
			if (msg.Type == Messages.Tank_SetValue)
			{
				tank.OnTankSetValueMessage(msgTank);
			}
		}
		return base.Deserialize(msg, remoteID);
	}
}
