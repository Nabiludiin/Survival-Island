using System;

[Serializable]
public class Message_UseShovel : Message_NetworkBehaviour
{
	public Message_UseShovel()
	{
	}

	public Message_UseShovel(Messages p_type, MonoBehaviour_Network p_behaviour)
		: base(p_type, p_behaviour)
	{
	}
}
