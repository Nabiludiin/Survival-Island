using System;

[Serializable]
public class Message_Shark_Create : Message_NetworkBehaviour_ID
{
	public RGD_Shark rgdShark;

	public Message_Shark_Create(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Shark shark)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		rgdShark = new RGD_Shark(RGDType.Entity_Shark, shark);
	}
}
