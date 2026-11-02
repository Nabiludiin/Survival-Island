using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class SerializableBehaviour : ISerializable
{
	public SerializableBehaviour()
	{
	}

	public virtual void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
	}

	public SerializableBehaviour(SerializationInfo info, StreamingContext sc)
	{
	}

	protected virtual void SetDefaults(StreamingContext sc)
	{
	}

	public virtual void SerializeFast(FastBufferWriter writer)
	{
		NetworkDebug.AddBehaviour(GetType().Name);
	}

	public virtual void DeserializeFast(FastBufferReader reader)
	{
	}
}
