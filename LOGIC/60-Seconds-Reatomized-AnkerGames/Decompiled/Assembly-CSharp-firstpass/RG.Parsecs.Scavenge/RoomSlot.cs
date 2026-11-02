using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class RoomSlot : MonoBehaviour
{
	public RoomShape RoomShape;

	public GameObject Centre;

	public bool TopWallTransparent;

	public bool BottomWallTransparent;

	public List<RandomPrefabSpawner> TopWalls;

	public List<RandomPrefabSpawner> RightWalls;

	public List<RandomPrefabSpawner> BottomWalls;

	public List<RandomPrefabSpawner> LeftWalls;

	private List<Wall> _topWalls = new List<Wall>();

	private List<Wall> _rightWalls = new List<Wall>();

	private List<Wall> _bottomWalls = new List<Wall>();

	private List<Wall> _leftWalls = new List<Wall>();

	public MeshRenderer Floor;

	private void OnDestroy()
	{
	}

	public void SetInsideMaterial(Material material, Material transparentMaterial)
	{
		SetupWalls(TopWalls, _topWalls);
		SetupWalls(LeftWalls, _leftWalls);
		SetupWalls(BottomWalls, _bottomWalls);
		SetupWalls(RightWalls, _rightWalls);
		if (TopWallTransparent)
		{
			SetCanBeTransparent(_topWalls);
		}
		if (BottomWallTransparent)
		{
			SetCanBeTransparent(_bottomWalls, OnBottomWallTransparent);
			RemoveSides(_bottomWalls);
		}
		SetFrontMaterial(_bottomWalls, material, transparentMaterial);
		SetFrontMaterial(_leftWalls, material, transparentMaterial);
		SetBackMaterial(_topWalls, material, transparentMaterial);
		SetBackMaterial(_rightWalls, material, transparentMaterial);
	}

	private void RemoveSides(List<Wall> walls)
	{
		foreach (Wall wall in walls)
		{
			wall.RemoveSides();
		}
	}

	public void SetFloorMaterial(Material material)
	{
		Floor.sharedMaterial = material;
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

	private void SetCanBeTransparent(List<Wall> walls, Action<Wall> onTransparent = null)
	{
		foreach (Wall wall in walls)
		{
			wall.CanBeTransparent = true;
			if (onTransparent != null)
			{
				wall.OnTransparent.Add(onTransparent);
			}
		}
	}

	private void OnBottomWallTransparent(Wall wall)
	{
		foreach (Wall bottomWall in _bottomWalls)
		{
			if (bottomWall != wall && bottomWall.gameObject.activeInHierarchy)
			{
				bottomWall.Transparency.SetTransparent(events: false);
			}
		}
	}

	private void SetFrontMaterial(List<Wall> walls, Material material, Material transparentMaterial)
	{
		foreach (Wall wall in walls)
		{
			if (wall.CanBeTransparent)
			{
				wall.SetFrontMaterial(transparentMaterial, material);
			}
			else
			{
				wall.SetFrontMaterial(material);
			}
		}
	}

	private void SetBackMaterial(List<Wall> walls, Material material, Material transparentMaterial)
	{
		foreach (Wall wall in walls)
		{
			if (wall.CanBeTransparent)
			{
				wall.SetBackMaterial(transparentMaterial, material);
			}
			else
			{
				wall.SetBackMaterial(material);
			}
		}
	}

	[ContextMenu("EditorRotateWalls")]
	public void EditorRotateWalls()
	{
	}

	[ContextMenu("EditorFindWalls")]
	public void EditorFindWalls()
	{
	}
}
