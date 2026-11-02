using System;

[Serializable]
public class Message_StoneDrop_Update : Message_NetworkBehaviour
{
	public SerializableVector3 Position;

	public Message_StoneDrop_Update(Messages type, MonoBehaviour_Network behaviour, StoneDrop stoneDrop)
		: base(type, behaviour)
	{
		Position = new SerializableVector3(stoneDrop.transform.position);
	}
}
