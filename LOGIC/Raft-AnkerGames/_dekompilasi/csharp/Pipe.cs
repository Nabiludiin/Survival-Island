using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
	[SerializeField]
	private BitmaskTile bitmaskTile;

	public Block_Pipe block_Pipe;

	[HideInInspector]
	public bool isAboutToBeDestroyed;

	[HideInInspector]
	public PipeGroup pipeGroup;

	[HideInInspector]
	public Vector3 snappedBuildingPosition;

	public void OnPipePlaced()
	{
		snappedBuildingPosition = transform.position;
		BlockCreator.SnapBuildPositionWorldToLocalLockedPivot(ref snappedBuildingPosition);
		PipeGroupManager.TrackPipe(snappedBuildingPosition, this);
		gameObject.SetActiveSafe(value: true);
		List<Pipe> nearbyPipes = GetNearbyPipes();
		MergeIntoNearbyPipeGroup(nearbyPipes);
	}

	public void OnPipeRemoved()
	{
		PipeGroupManager.UntrackPipe(snappedBuildingPosition);
		gameObject.SetActiveSafe(value: false);
		DisconnectFromPipeGroup();
	}

	public void MergeIntoNearbyPipeGroup(List<Pipe> nearbyPipes)
	{
		List<PipeGroup> list = new List<PipeGroup>();
		if (nearbyPipes.ContainsItems())
		{
			foreach (Pipe nearbyPipe in nearbyPipes)
			{
				PipeGroup pipeGroup = nearbyPipe.pipeGroup;
				if (pipeGroup != null)
				{
					list.AddUniqueOnly(pipeGroup);
				}
			}
		}
		if (list.ContainsItems())
		{
			if (list.Count == 1)
			{
				list[0].AddPipe(this);
				return;
			}
			PipeGroup pipeGroup2 = list[0];
			pipeGroup2.AddPipe(this);
			for (int i = 1; i < list.Count; i++)
			{
				List<Pipe> pipesCopy = list[i].GetPipesCopy();
				PipeGroupManager.RemovePipeGroup(list[i]);
				pipeGroup2.AddPipes(pipesCopy);
			}
		}
		else
		{
			PipeGroupManager.CreateNewPipeGroup().AddPipe(this);
		}
	}

	public List<Pipe> GetNearbyPipes()
	{
		List<Pipe> list = new List<Pipe>();
		Pipe pipe = PipeGroupManager.GetPipeFromPosition(snappedBuildingPosition + Vector3.forward * 1.5f);
		Pipe pipe2 = PipeGroupManager.GetPipeFromPosition(snappedBuildingPosition - Vector3.forward * 1.5f);
		Pipe pipe3 = PipeGroupManager.GetPipeFromPosition(snappedBuildingPosition - Vector3.right * 1.5f);
		Pipe pipe4 = PipeGroupManager.GetPipeFromPosition(snappedBuildingPosition + Vector3.right * 1.5f);
		Pipe pipe5 = PipeGroupManager.GetPipeFromPosition(snappedBuildingPosition + Vector3.up * 1.21f);
		Pipe pipe6 = PipeGroupManager.GetPipeFromPosition(snappedBuildingPosition - Vector3.up * 1.21f);
		if (bitmaskTile != null)
		{
			BitmaskTile[] neighbours = bitmaskTile.GetNeighbours();
			bitmaskTile.ExcludeOutOfSightNeighbours(ref neighbours);
			if (pipe != null && pipe.bitmaskTile != null && neighbours[0] == null)
			{
				pipe = null;
			}
			if (pipe2 != null && pipe2.bitmaskTile != null && neighbours[3] == null)
			{
				pipe2 = null;
			}
			if (pipe3 != null && pipe3.bitmaskTile != null && neighbours[1] == null)
			{
				pipe3 = null;
			}
			if (pipe4 != null && pipe4.bitmaskTile != null && neighbours[2] == null)
			{
				pipe4 = null;
			}
			if (pipe5 != null && pipe5.bitmaskTile != null && neighbours[4] == null)
			{
				pipe5 = null;
			}
			if (pipe6 != null && pipe6.bitmaskTile != null && neighbours[5] == null)
			{
				pipe6 = null;
			}
		}
		list.AddUniqueOnly(pipe);
		list.AddUniqueOnly(pipe2);
		list.AddUniqueOnly(pipe3);
		list.AddUniqueOnly(pipe4);
		list.AddUniqueOnly(pipe5);
		list.AddUniqueOnly(pipe6);
		if (!list.ContainsItems())
		{
			return null;
		}
		return list;
	}

	private void FloodFill(List<Pipe> pipesInFloodFill)
	{
		if (pipesInFloodFill == null)
		{
			pipesInFloodFill = new List<Pipe>();
		}
		if (pipesInFloodFill.Contains(this))
		{
			return;
		}
		pipesInFloodFill.Add(this);
		List<Pipe> nearbyPipes = GetNearbyPipes();
		if (nearbyPipes.ContainsItems())
		{
			for (int i = 0; i < nearbyPipes.Count; i++)
			{
				nearbyPipes[i].FloodFill(pipesInFloodFill);
			}
		}
	}

	private void DisconnectFromPipeGroup()
	{
		List<Pipe> list = null;
		if (this.pipeGroup != null)
		{
			list = this.pipeGroup.GetPipesCopy();
			list.Remove(this);
		}
		PipeGroupManager.RemovePipeGroup(this.pipeGroup);
		if (list == null || !list.ContainsItems())
		{
			return;
		}
		List<PipeGroup> list2 = new List<PipeGroup>();
		for (int i = 0; i < list.Count; i++)
		{
			Pipe pipe = list[i];
			if (pipe.pipeGroup == null)
			{
				List<Pipe> list3 = new List<Pipe>();
				pipe.FloodFill(list3);
				PipeGroup pipeGroup = PipeGroupManager.CreateNewPipeGroup();
				pipeGroup.AddPipes(list3);
				list2.Add(pipeGroup);
			}
		}
		if (!list2.ContainsItems())
		{
			return;
		}
		for (int j = 0; j < list2.Count; j++)
		{
			PipeGroup pipeGroup2 = list2[j];
			if (!pipeGroup2.IsStable())
			{
				pipeGroup2.DestroyGroup();
			}
		}
	}

	private void LogNearbyPipes()
	{
		Helper.ClearConsole();
		List<Pipe> nearbyPipes = GetNearbyPipes();
		if (nearbyPipes == null)
		{
			Debug.Log("Could not find any nearby pipes");
			return;
		}
		foreach (Pipe item in nearbyPipes)
		{
			Debug.Log("Connected to: " + item.gameObject.name);
			Debug.DrawRay(transform.position, item.transform.position - transform.position, Color.green, 5f);
		}
	}

	private void LogFloodFill()
	{
		Helper.ClearConsole();
		List<Pipe> list = new List<Pipe>();
		FloodFill(list);
		if (list.ContainsItems())
		{
			for (int i = 0; i < list.Count; i++)
			{
				Debug.Log("Flood filled to: " + list[i].gameObject.name);
				Debug.DrawRay(transform.position, list[i].transform.position - transform.position, Color.green, 5f);
			}
		}
		else
		{
			Debug.Log("Could not find any pipes in floodfill");
		}
	}

	private void SimulatePipePlaced()
	{
		OnPipePlaced();
	}

	private void SimulatePipeRemoved()
	{
		OnPipeRemoved();
	}
}
