using System;

[Serializable]
public class Message_NetworkResult : Message_NetworkBehaviour
{
	public bool result;

	public Message_NetworkResult(Messages type, MonoBehaviour_Network behaviour, bool result)
		: base(type, behaviour)
	{
		this.result = result;
	}
}
