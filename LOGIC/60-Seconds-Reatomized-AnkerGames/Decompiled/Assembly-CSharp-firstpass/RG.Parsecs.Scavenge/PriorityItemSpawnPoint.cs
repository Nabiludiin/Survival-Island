using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class PriorityItemSpawnPoint : ItemSpawnPoint
{
	public ScavangeItemData SpawnItem;

	public RoomType SpawnRoomType;

	protected override IEnumerator Start()
	{
		yield return null;
		SpriteRenderer component = GetComponent<SpriteRenderer>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	public void AssignItem()
	{
		AssignItem(SpawnItem);
		SetRoomType(SpawnRoomType);
	}
}
