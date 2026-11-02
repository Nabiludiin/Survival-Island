using System.Collections.Generic;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[CreateAssetMenu(fileName = "ScavangeItem", menuName = "Scavenge/ScavangeItem")]
public class ScavangeItemData : ScriptableObject
{
	public string itemName;

	public IItem survivalItemRef;

	public Sprite handSprite;

	[SerializeField]
	private PrefabRepresentation[] prefabRepresentations;

	[SerializeField]
	private ItemSpawnPreferences ItemLikesToSpawn;

	public int Weight;

	public int ItemSpawnDifficulty
	{
		get
		{
			if (survivalItemRef is ScavengeCharacterItem)
			{
				return 0;
			}
			return ItemLikesToSpawn.GetItemSpawnDifficulty();
		}
	}

	[ContextMenu("PullWeight")]
	public void PrintShips()
	{
		Weight = survivalItemRef.BaseStaticData.BaseWeight;
	}

	public GameObject GetItemRepresentation(ESpawnPointType type)
	{
		List<GameObject> list = new List<GameObject>();
		PrefabRepresentation[] array = prefabRepresentations;
		foreach (PrefabRepresentation prefabRepresentation in array)
		{
			if (prefabRepresentation.type == type)
			{
				list.Add(prefabRepresentation.prefab);
			}
		}
		if (list.Count == 0)
		{
			Debug.LogError("Unable to find " + itemName + "for spawnPoint " + type);
		}
		return list.RandomElement();
	}

	public ItemSpawnPoint GetBestSpawnPointForMe(List<ItemSpawnPoint> spawnPoints)
	{
		Dictionary<ItemSpawnPoint, int> dictionary = new Dictionary<ItemSpawnPoint, int>();
		foreach (ItemSpawnPoint spawnPoint in spawnPoints)
		{
			if (spawnPoint.IsItemAvailable || (survivalItemRef is ScavengeCharacterItem && ShipGenerator.I.roomsWithCharacters.Contains(spawnPoint.GetRoomType())))
			{
				continue;
			}
			int num = 0;
			int num2 = 0;
			foreach (SpawnerPreferences item in ItemLikesToSpawn.PreferableSpawnType)
			{
				if (item.spawnType == spawnPoint.SpawnType)
				{
					num += item.weight;
					break;
				}
			}
			if (num == 0)
			{
				continue;
			}
			foreach (RoomPreferences item2 in ItemLikesToSpawn.PreferableRoom)
			{
				if (item2.roomType == spawnPoint.GetRoomType())
				{
					num2 += item2.weight;
					break;
				}
			}
			if (num2 != 0)
			{
				int value = num + num2;
				dictionary.Add(spawnPoint, value);
			}
		}
		if (dictionary.Count == 0)
		{
			Debug.LogError("Unable to find prefered spawn point for item " + itemName);
			return null;
		}
		return GetRandomWeight(dictionary);
	}

	private ItemSpawnPoint GetRandomWeight(Dictionary<ItemSpawnPoint, int> spawnList)
	{
		int num = 0;
		foreach (KeyValuePair<ItemSpawnPoint, int> spawn in spawnList)
		{
			num += spawn.Value;
		}
		int num2 = Random.Range(0, num);
		ItemSpawnPoint result = null;
		foreach (KeyValuePair<ItemSpawnPoint, int> spawn2 in spawnList)
		{
			if (num2 < spawn2.Value)
			{
				result = spawn2.Key;
				break;
			}
			num2 -= spawn2.Value;
		}
		return result;
	}
}
