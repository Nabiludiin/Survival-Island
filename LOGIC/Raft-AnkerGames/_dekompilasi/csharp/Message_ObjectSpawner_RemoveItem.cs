using System;

[Serializable]
public class Message_ObjectSpawner_RemoveItem : Message_NetworkBehaviour
{
	public uint uniqueSpawnerIndex;

	public uint spawnedObjectIndex;

	public Message_ObjectSpawner_RemoveItem(Messages type, MonoBehaviour_Network behaviour, uint uniqueSpawnerIndex, uint spawnedObjectIndex)
		: base(type, behaviour)
	{
		this.uniqueSpawnerIndex = uniqueSpawnerIndex;
		this.spawnedObjectIndex = spawnedObjectIndex;
	}
}
