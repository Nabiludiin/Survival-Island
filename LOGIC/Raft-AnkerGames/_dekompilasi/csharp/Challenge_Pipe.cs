public class Challenge_Pipe : MonoBehaviour_ID_Network
{
	public RotationComponent rotationComponent;

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		Messages type = msg.Type;
		if ((uint)(type - 216) <= 2u)
		{
			return rotationComponent.Deserialize(msg, remoteID);
		}
		return base.Deserialize(msg, remoteID);
	}
}
