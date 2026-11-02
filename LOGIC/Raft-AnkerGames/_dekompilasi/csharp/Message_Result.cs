using System;

[Serializable]
public class Message_Result : Message
{
	public bool result;

	public Message_Result(Messages type, bool result)
	{
		Type = type;
		this.result = result;
	}
}
