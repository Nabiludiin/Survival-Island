using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[CreateAssetMenu(fileName = "RoomsDatabase", menuName = "Scavenge/Rooms Database")]
public class RoomsDatabase : ScriptableObject
{
	public List<RoomData> CommandCentre = new List<RoomData>();

	public List<RoomData> Cafeteria = new List<RoomData>();

	public List<RoomData> Bathroom = new List<RoomData>();

	public List<RoomData> ScienceRoom = new List<RoomData>();

	public List<RoomData> SleepingQuaters = new List<RoomData>();

	public List<RoomData> EngineeringRoom = new List<RoomData>();

	public List<RoomData> Corridor = new List<RoomData>();

	private RandomRoomData GetRandomMatchingShape(List<RoomData> roomList, RoomShape shape, int seed)
	{
		List<RoomData> list = roomList.FindAll((RoomData room) => room.RoomShape == shape);
		if (list == null || list.Count == 0)
		{
			return null;
		}
		RandomRoomData randomRoomData = new RandomRoomData();
		if (seed >= 0)
		{
			randomRoomData.Seed = seed;
			randomRoomData.RoomData = list[seed];
			return randomRoomData;
		}
		randomRoomData.Seed = UnityEngine.Random.Range(0, list.Count);
		randomRoomData.RoomData = list[randomRoomData.Seed];
		return randomRoomData;
	}

	internal RandomRoomData GetRoom(RoomShape roomShape, RoomType roomType, bool verbose = false, int seed = -1)
	{
		RandomRoomData randomRoomData = null;
		randomRoomData = roomType switch
		{
			RoomType.CommandCentre => GetRandomMatchingShape(CommandCentre, roomShape, seed), 
			RoomType.Cafeteria => GetRandomMatchingShape(Cafeteria, roomShape, seed), 
			RoomType.Bathroom => GetRandomMatchingShape(Bathroom, roomShape, seed), 
			RoomType.ScienceRoom => GetRandomMatchingShape(ScienceRoom, roomShape, seed), 
			RoomType.SleepingQuaters => GetRandomMatchingShape(SleepingQuaters, roomShape, seed), 
			RoomType.EngineeringRoom => GetRandomMatchingShape(EngineeringRoom, roomShape, seed), 
			RoomType.Corridor => GetRandomMatchingShape(Corridor, roomShape, seed), 
			_ => throw new NotImplementedException(), 
		};
		if (randomRoomData == null)
		{
			if (verbose)
			{
				Debug.LogErrorFormat("No room found for {0} : {1}.", roomShape, roomType);
			}
			return null;
		}
		if (randomRoomData.RoomData.Scene == null)
		{
			Debug.LogErrorFormat("Looking for {0} found {1}. result.Scene is null.", roomShape, randomRoomData.RoomData.name);
			return null;
		}
		if (string.IsNullOrEmpty(randomRoomData.RoomData.Scene.SceneName))
		{
			Debug.LogErrorFormat("Looking for {0} found {1}. result.SceneSceneName is null or empty.", roomShape, randomRoomData.RoomData.name);
			return null;
		}
		if (randomRoomData.RoomData.RoomShape != roomShape)
		{
			Debug.LogErrorFormat("Looking for {0} found {1}. Check {2} definitions.", roomShape, randomRoomData.RoomData.RoomShape, randomRoomData.RoomData.Scene);
			return null;
		}
		return randomRoomData;
	}

	internal bool CanGenerate(List<RoomSlot> slots, List<RoomType> types)
	{
		for (int i = 0; i < slots.Count; i++)
		{
			if (GetRoom(slots[i].RoomShape, types[i]) == null)
			{
				return false;
			}
		}
		return true;
	}

	internal bool CanGenerate(List<RoomShape> shapes, List<RoomType> types)
	{
		for (int i = 0; i < shapes.Count; i++)
		{
			if (GetRoom(shapes[i], types[i]) == null)
			{
				return false;
			}
		}
		return true;
	}
}
