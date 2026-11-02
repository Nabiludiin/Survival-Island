using System;
using System.Collections.Generic;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class ItemSpawnPreferences
{
	public List<RoomPreferences> PreferableRoom = new List<RoomPreferences>();

	public List<SpawnerPreferences> PreferableSpawnType = new List<SpawnerPreferences>();

	public int GetItemSpawnDifficulty()
	{
		int num = 0;
		foreach (RoomPreferences item in PreferableRoom)
		{
			if (item.weight != 0)
			{
				num++;
			}
		}
		foreach (SpawnerPreferences item2 in PreferableSpawnType)
		{
			if (item2.weight != 0)
			{
				num++;
			}
		}
		return num;
	}
}
