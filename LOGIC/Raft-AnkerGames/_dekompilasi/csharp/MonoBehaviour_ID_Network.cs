using System.Collections.Generic;

public class MonoBehaviour_ID_Network : MonoBehaviour_ID
{
	private List<Message> messages = new List<Message>();

	public List<Message> MessagesStack => messages;

	public virtual void Clean()
	{
		MessagesStack.Clear();
	}

	protected void AddMessage(Message_NetworkBehaviour msg)
	{
		if (msg != null)
		{
			MessagesStack.Add(msg);
		}
	}

	protected void AddMessage(List<Message_NetworkBehaviour> msgs)
	{
		if (msgs.ContainsItems())
		{
			MessagesStack.AddRange(msgs);
		}
	}

	public virtual void Serialize_Create()
	{
	}

	public virtual void Serialize_CreateFromIDManager(List<Message_NetworkBehaviour> msgs)
	{
	}

	public virtual bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		return false;
	}
}
