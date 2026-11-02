using System;
using Unity.Netcode;

[Serializable]
public class Message_InteractableButtonPress : Message_NetworkBehaviour_ID_SteamID
{
	public bool success;

	public int buttonIndex;

	protected bool Equals(Message_InteractableButtonPress other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other) && success == other.success)
		{
			return buttonIndex == other.buttonIndex;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((Message_InteractableButtonPress)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), success, buttonIndex);
	}

	public static bool operator ==(Message_InteractableButtonPress left, Message_InteractableButtonPress right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_InteractableButtonPress left, Message_InteractableButtonPress right)
	{
		return !object.Equals(left, right);
	}

	public Message_InteractableButtonPress()
	{
	}

	public Message_InteractableButtonPress(MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, int buttonIndex, Network_Player interactingPlayer, Messages msg = Messages.InteractableButtonPress)
		: base(msg, behaviour, networkID_ObjectIndex, (interactingPlayer == null) ? ((Network_UserId)0uL) : interactingPlayer.steamID)
	{
		this.buttonIndex = buttonIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in success, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in buttonIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out success, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out buttonIndex, default(FastBufferWriter.ForPrimitives));
	}
}
