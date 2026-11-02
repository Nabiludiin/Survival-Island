public class PlaceableFirework : MonoBehaviour_ID_Network, IBlockPlaced
{
	private Block_Firework fireworkBlock;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkIDManager.RemoveNetworkID(this);
	}

	void IBlockPlaced.OnBlockPlacedInterface()
	{
		fireworkBlock = GetComponent<Block_Firework>();
		NetworkIDManager.AddNetworkID(this, typeof(PlaceableFirework));
	}

	public override RGD Serialize_Save()
	{
		if (fireworkBlock == null)
		{
			fireworkBlock = GetComponent<Block_Firework>();
		}
		return new RGD_Block_Firework(RGDType.Block_Firework, fireworkBlock);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.FireworkBlock_LightFuse:
			return fireworkBlock.LightFuse();
		case Messages.RotationComponentRotate:
		case Messages.RotationComponentSetRotation:
		case Messages.RotationComponentRequestRotation:
			return fireworkBlock.rotationComponent.Deserialize(msg, remoteID);
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
