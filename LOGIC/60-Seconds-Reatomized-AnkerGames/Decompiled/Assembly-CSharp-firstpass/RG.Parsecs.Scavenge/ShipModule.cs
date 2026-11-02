using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ShipModule : Module
{
	public List<RoomSlot> RoomSlots = new List<RoomSlot>();

	public List<RandomPrefabSpawner> FrontOutsideWalls;

	public List<RandomPrefabSpawner> BackOutsidetWalls;

	public List<Wall> StationWalls;

	public List<SpaceDoorGroup> SpaceDoorsGroups;

	private List<Wall> _frontWalls = new List<Wall>();

	private List<Wall> _backtWalls = new List<Wall>();

	public override void Load()
	{
		RandomlyCloseDoors();
	}

	public void SetOutsideMaterial(Material material)
	{
		SetupWalls(FrontOutsideWalls, _frontWalls);
		SetupWalls(BackOutsidetWalls, _backtWalls);
		_frontWalls.AddRange(StationWalls);
		SetFrontMaterial(_backtWalls, material);
		SetBackMaterial(_frontWalls, material);
	}

	private void SetupWalls(List<RandomPrefabSpawner> spawners, List<Wall> walls)
	{
		walls.Capacity = Mathf.Max(spawners.Count, walls.Capacity);
		foreach (RandomPrefabSpawner spawner in spawners)
		{
			Wall component = spawner.Instance.GetComponent<Wall>();
			if (component != null)
			{
				walls.Add(component);
			}
		}
	}

	private void SetFrontMaterial(List<Wall> walls, Material material)
	{
		foreach (Wall wall in walls)
		{
			wall.SetFrontMaterial(material);
		}
	}

	private void SetBackMaterial(List<Wall> walls, Material material)
	{
		foreach (Wall wall in walls)
		{
			wall.SetBackMaterial(material);
		}
	}

	public override void Unload()
	{
	}

	public void RandomlyCloseDoors()
	{
		SetDoorGroupStatus(DoorOperationalState.Damaged);
	}

	public void ResetDoors()
	{
		foreach (SpaceDoorGroup spaceDoorsGroup in SpaceDoorsGroups)
		{
			spaceDoorsGroup.available = true;
			SpaceDoors[] doors = spaceDoorsGroup.doors;
			for (int i = 0; i < doors.Length; i++)
			{
				doors[i].SetDoorState(DoorOperationalState.Working);
			}
		}
	}

	private void SetDoorGroupStatus(DoorOperationalState state)
	{
		List<SpaceDoorGroup> list = new List<SpaceDoorGroup>();
		foreach (SpaceDoorGroup spaceDoorsGroup in SpaceDoorsGroups)
		{
			if (spaceDoorsGroup.available)
			{
				list.Add(spaceDoorsGroup);
			}
		}
		if (list.Count != 0)
		{
			SpaceDoorGroup spaceDoorGroup = list.RandomElement();
			spaceDoorGroup.available = false;
			SpaceDoors[] doors = spaceDoorGroup.doors;
			for (int i = 0; i < doors.Length; i++)
			{
				doors[i].SetDoorState(state);
			}
		}
	}
}
