using System;
using System.Collections.Generic;
using UnityEngine;

public class Block_Pipe : Block
{
	[SerializeField]
	private SO_PipeColliderInfo pipeColliderInfo;

	[SerializeField]
	private Block_PipeBitmask[] pipeBitmaskConnections;

	private GameObject currentPipeCollider;

	private bool hasPipeSocket;

	public bool HasPipeConnections
	{
		get
		{
			if (pipeBitmaskConnections != null)
			{
				return pipeBitmaskConnections.ContainsItems();
			}
			return false;
		}
	}

	protected override void OnDestroy()
	{
		Block_PipeBitmask[] array;
		if (HasPipeConnections)
		{
			array = pipeBitmaskConnections;
			foreach (Block_PipeBitmask con in array)
			{
				if (con.bitmaskTile != null)
				{
					BitmaskTile bitmaskTile = con.bitmaskTile;
					bitmaskTile.OnMeshChange = (Action)Delegate.Remove(bitmaskTile.OnMeshChange, (Action)(() =>
					{
						OnBitmaskTileChange(con.bitmaskTile);
					}));
					if (Raft_Network.IsHost)
					{
						BitmaskTile bitmaskTile2 = con.bitmaskTile;
						bitmaskTile2.OnCompleteVisual = (Action)Delegate.Remove(bitmaskTile2.OnCompleteVisual, new Action(OnBitmaskTileCompleteVisual));
					}
				}
			}
		}
		if (Raft_Network.IsHost && !GameManager.IsLeavingGame && hasBeenPlaced && HasPipeConnections)
		{
			array = pipeBitmaskConnections;
			foreach (Block_PipeBitmask block_PipeBitmask in array)
			{
				if (block_PipeBitmask.pipe != null && !block_PipeBitmask.pipe.isAboutToBeDestroyed)
				{
					block_PipeBitmask.pipe.OnPipeRemoved();
				}
			}
		}
		array = pipeBitmaskConnections;
		foreach (Block_PipeBitmask block_PipeBitmask2 in array)
		{
			if (block_PipeBitmask2.bitmaskTile != null && !GameManager.IsLeavingGame && hasBeenPlaced)
			{
				TileBitmaskManager.RemoveTile(block_PipeBitmask2.bitmaskTile);
				block_PipeBitmask2.bitmaskTile.RefreshNeighbours();
			}
		}
		base.OnDestroy();
	}

	protected override void Awake()
	{
		base.Awake();
		if (!HasPipeConnections)
		{
			return;
		}
		Block_PipeBitmask[] array = pipeBitmaskConnections;
		foreach (Block_PipeBitmask block_PipeBitmask in array)
		{
			if (block_PipeBitmask.pipe != null && block_PipeBitmask.pipe is PipeSocket)
			{
				hasPipeSocket = true;
				break;
			}
		}
	}

	private void MergeIntoNearbyPipesLate()
	{
		if (!HasPipeConnections)
		{
			return;
		}
		Block_PipeBitmask[] array = pipeBitmaskConnections;
		foreach (Block_PipeBitmask block_PipeBitmask in array)
		{
			if (block_PipeBitmask.pipe != null)
			{
				List<Pipe> nearbyPipes = block_PipeBitmask.pipe.GetNearbyPipes();
				block_PipeBitmask.pipe.MergeIntoNearbyPipeGroup(nearbyPipes);
			}
		}
	}

	private void OnBitmaskTileChange(BitmaskTile bitmaskTile)
	{
		if (!(pipeColliderInfo != null))
		{
			return;
		}
		for (int i = 0; i < pipeColliderInfo.pipeColliderConnections.Length; i++)
		{
			PipeColliderConnection pipeColliderConnection = pipeColliderInfo.pipeColliderConnections[i];
			if (pipeColliderConnection.ContainsBitmaskValue(bitmaskTile.currentBitmaskValue))
			{
				if (currentPipeCollider != null)
				{
					UnityEngine.Object.Destroy(currentPipeCollider);
				}
				currentPipeCollider = UnityEngine.Object.Instantiate(pipeColliderConnection.colliderPrefab, base.transform.position, base.transform.rotation, bitmaskTile.rotationTransform);
				Transform transform = currentPipeCollider.transform;
				Vector3 localPosition = (currentPipeCollider.transform.localEulerAngles = Vector3.zero);
				transform.localPosition = localPosition;
				stableComponent = currentPipeCollider.GetComponent<StableComponent>();
				break;
			}
		}
	}

	private void OnBitmaskTileCompleteVisual()
	{
		Invoke("MergeIntoNearbyPipesLate", 0.1f);
	}

	public override void OnFinishedPlacement()
	{
		base.OnFinishedPlacement();
		if (onoffColliders.ContainsItems() && onoffColliders[0] != null && HasPipeConnections)
		{
			Block_PipeBitmask[] array = pipeBitmaskConnections;
			foreach (Block_PipeBitmask block_PipeBitmask in array)
			{
				if (block_PipeBitmask.pipe != null && !(block_PipeBitmask.pipe is PipeSocket))
				{
					UnityEngine.Object.Destroy(onoffColliders[0].gameObject);
					onoffColliders = null;
				}
			}
		}
		Invoke("OnFinishedPlacementLate", 0.1f);
	}

	private void OnFinishedPlacementLate()
	{
		if (!HasPipeConnections)
		{
			return;
		}
		bool flag = false;
		Block_PipeBitmask[] array = pipeBitmaskConnections;
		foreach (Block_PipeBitmask con in array)
		{
			if (!flag && con.pipe != null && !(con.pipe is PipeSocket) && con.bitmaskTile != null)
			{
				flag = true;
				BitmaskTile bitmaskTile = con.bitmaskTile;
				bitmaskTile.OnMeshChange = (Action)Delegate.Combine(bitmaskTile.OnMeshChange, (Action)(() =>
				{
					OnBitmaskTileChange(con.bitmaskTile);
				}));
				if (Raft_Network.IsHost)
				{
					BitmaskTile bitmaskTile2 = con.bitmaskTile;
					bitmaskTile2.OnCompleteVisual = (Action)Delegate.Combine(bitmaskTile2.OnCompleteVisual, new Action(OnBitmaskTileCompleteVisual));
				}
			}
			if (con.bitmaskTile != null)
			{
				TileBitmaskManager.AddTile(con.bitmaskTile);
				con.bitmaskTile.Refresh(refreshNeighbour: true);
			}
			if (Raft_Network.IsHost && con.pipe != null)
			{
				con.pipe.OnPipePlaced();
			}
		}
	}

	public override bool IsStable()
	{
		if (hasPipeSocket)
		{
			return base.IsStable();
		}
		if (hasBeenPlaced && HasPipeConnections)
		{
			Block_PipeBitmask[] array = pipeBitmaskConnections;
			foreach (Block_PipeBitmask block_PipeBitmask in array)
			{
				if (block_PipeBitmask.pipe != null && block_PipeBitmask.pipe.pipeGroup != null && !block_PipeBitmask.pipe.pipeGroup.IsStable())
				{
					block_PipeBitmask.pipe.pipeGroup.DestroyGroup();
				}
			}
		}
		return true;
	}

	public bool IsPipeStable()
	{
		int? num = null;
		if (ComponentManager<BlockCollisionConsolidator>.Value != null)
		{
			num = ComponentManager<BlockCollisionConsolidator>.Value.TempActivateCellAndNeighbours(transform.position);
		}
		if (stableComponent == null && currentPipeCollider != null)
		{
			stableComponent = currentPipeCollider.GetComponent<StableComponent>();
		}
		bool result = stableComponent != null && stableComponent.IsStable();
		if (num.HasValue)
		{
			ComponentManager<BlockCollisionConsolidator>.Value.RemoveTempActivate(num.Value);
		}
		return result;
	}
}
