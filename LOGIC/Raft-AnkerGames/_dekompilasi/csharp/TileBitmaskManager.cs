using System.Collections.Generic;
using UnityEngine;

public class TileBitmaskManager : MonoBehaviour, SceneEventInterface
{
	private static Dictionary<TileBitmaskType, List<BitmaskTile>> tileDictionary = new Dictionary<TileBitmaskType, List<BitmaskTile>>();

	private static SO_TileBitmask[] bitmaskAssets;

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			tileDictionary.Clear();
		}
	}

	public void Awake()
	{
		LoadBitmaskAssets();
	}

	public static void AddTile(BitmaskTile tile)
	{
		if (!(tile == null))
		{
			List<BitmaskTile> list = null;
			if (tileDictionary.ContainsKey(tile.BitmaskType))
			{
				list = tileDictionary[tile.BitmaskType];
			}
			else
			{
				list = new List<BitmaskTile>();
				tileDictionary.Add(tile.BitmaskType, list);
			}
			list.Add(tile);
		}
	}

	public static void RemoveTile(BitmaskTile tile)
	{
		if (!(tile == null) && tileDictionary.ContainsKey(tile.BitmaskType))
		{
			tileDictionary[tile.BitmaskType].Remove(tile);
		}
	}

	public static BitmaskTile TileExistAtPosition(TileBitmaskType type, Vector3 localPosition)
	{
		if (tileDictionary.ContainsKey(type))
		{
			BlockCreator.SnapBuildPosition(ref localPosition);
			List<BitmaskTile> list = tileDictionary[type];
			for (int i = 0; i < list.Count; i++)
			{
				Vector3 position = BlockCreator.WorldToLocalLockedPivot(list[i].transform.position);
				BlockCreator.SnapBuildPosition(ref position);
				if (position == localPosition)
				{
					return list[i];
				}
			}
		}
		return null;
	}

	public static TileBitmaskConnection GetBitmaskConnectionFromBitmaskValue(TileBitmaskType type, int bitmaskValue)
	{
		if (bitmaskAssets == null)
		{
			LoadBitmaskAssets();
		}
		if (bitmaskAssets != null)
		{
			for (int i = 0; i < bitmaskAssets.Length; i++)
			{
				SO_TileBitmask sO_TileBitmask = bitmaskAssets[i];
				if (sO_TileBitmask.bitmaskType == type)
				{
					return sO_TileBitmask.GetBitmaskConnectionFromBitmaskValue(bitmaskValue);
				}
			}
		}
		else
		{
			Debug.LogError("Could not load bitmask mesh with type " + type.ToString() + " because bitmaskassets failed to load");
		}
		return null;
	}

	private static void LoadBitmaskAssets()
	{
		bitmaskAssets = Resources.LoadAll<SO_TileBitmask>("TileBitmaskAssets");
	}

	public void RefreshBitmasktilesInScene()
	{
		LoadBitmaskAssets();
		tileDictionary.Clear();
		BitmaskTile[] array = Object.FindObjectsOfType<BitmaskTile>();
		for (int i = 0; i < array.Length; i++)
		{
			AddTile(array[i]);
		}
		for (int j = 0; j < array.Length; j++)
		{
			array[j].Refresh(refreshNeighbour: false);
		}
	}
}
