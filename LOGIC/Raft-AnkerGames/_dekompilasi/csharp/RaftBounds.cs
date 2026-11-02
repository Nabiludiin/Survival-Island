using System;
using System.Collections.Generic;
using UnityEngine;

public class RaftBounds : MonoBehaviour
{
	public Action OnBoundsUpdate;

	public Action<Block, bool> OnAddWalkableBlock;

	public Action<List<Block>, bool> OnRemoveWalkableBlocks;

	public Action<int> OnFoundationCountChange;

	private float width;

	private float length;

	private int foundationCount;

	[SerializeField]
	public List<Block> walkableBlocks = new List<Block>();

	public Vector3 Left => transform.position - RightDir * HalfWidth;

	public Vector3 Right => transform.position + RightDir * HalfWidth;

	public Vector3 Top => transform.position + ForwardDir * HalfLength;

	public Vector3 Bottom => transform.position - ForwardDir * HalfLength;

	public Vector3 TopLeft => Left + ForwardDir * HalfLength;

	public Vector3 TopRight => Right + ForwardDir * HalfLength;

	public Vector3 BottomLeft => Left - ForwardDir * HalfLength;

	public Vector3 BottomRight => Right - ForwardDir * HalfLength;

	public Vector3 ForwardDir => transform.forward;

	public Vector3 RightDir => transform.right;

	public Vector3 UpDir => transform.up;

	public Vector3 Center => transform.position;

	public float Width => width;

	public float Length => length;

	public float HalfWidth => Width / 2f;

	public float HalfLength => Length / 2f;

	public int WalkableBlocksCount => walkableBlocks.Count;

	public int FoundationCount
	{
		get
		{
			return foundationCount;
		}
		set
		{
			foundationCount = value;
			OnFoundationCountChange?.Invoke(foundationCount);
		}
	}

	private void Awake()
	{
		ComponentManager<RaftBounds>.Value = this;
		BlockCreator.PlaceBlockCallStack = (Action<Block, Network_Player, bool, int>)Delegate.Combine(BlockCreator.PlaceBlockCallStack, new Action<Block, Network_Player, bool, int>(OnCreateBlock));
	}

	private void Start()
	{
		if (SingletonGeneric<GameManager>.Singleton == null)
		{
			UpdateBounds();
		}
	}

	private void OnDestroy()
	{
		BlockCreator.PlaceBlockCallStack = (Action<Block, Network_Player, bool, int>)Delegate.Remove(BlockCreator.PlaceBlockCallStack, new Action<Block, Network_Player, bool, int>(OnCreateBlock));
	}

	private void OnDrawGizmosSelected()
	{
		float radius = 0.1f;
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(Center, radius);
		Gizmos.color = new Color(1f, 0.4f, 0f, 1f);
		Gizmos.DrawSphere(Left, radius);
		Gizmos.DrawSphere(Right, radius);
		Gizmos.DrawSphere(Top, radius);
		Gizmos.DrawSphere(Bottom, radius);
		Gizmos.DrawSphere(TopLeft, radius);
		Gizmos.DrawSphere(TopRight, radius);
		Gizmos.DrawSphere(BottomLeft, radius);
		Gizmos.DrawSphere(BottomRight, radius);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(BottomLeft, TopLeft);
		Gizmos.DrawLine(TopLeft, TopRight);
		Gizmos.DrawLine(TopRight, BottomRight);
		Gizmos.DrawLine(BottomRight, BottomLeft);
	}

	public void Initialize()
	{
		List<Block> placedBlocks = BlockCreator.GetPlacedBlocks();
		for (int i = 0; i < placedBlocks.Count; i++)
		{
			Block block = placedBlocks[i];
			if (block != null && block.IsWalkable())
			{
				AddWalkableBlock(block, replicating: false, updateBounds: false);
			}
		}
		UpdateBounds();
	}

	public void ClearWalkableBlocks()
	{
		if (walkableBlocks != null)
		{
			walkableBlocks.Clear();
			FoundationCount = 0;
		}
	}

	public void AddWalkableBlock(Block block, bool replicating, bool updateBounds)
	{
		if (!walkableBlocks.Contains(block))
		{
			walkableBlocks.Add(block);
			if (Block.IsBlockIndexFoundation(block.buildableItem.UniqueIndex))
			{
				FoundationCount++;
			}
			if (updateBounds)
			{
				UpdateBounds();
			}
			if (OnAddWalkableBlock != null)
			{
				OnAddWalkableBlock(block, replicating);
			}
		}
	}

	public void UpdateBounds()
	{
		if (walkableBlocks.Count == 0)
		{
			return;
		}
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		float num3 = float.MinValue;
		float num4 = float.MinValue;
		Transform transform = null;
		for (int i = 0; i < walkableBlocks.Count; i++)
		{
			Block block = walkableBlocks[i];
			if (block == null)
			{
				if (Block.IsBlockIndexFoundation(block.buildableItem.UniqueIndex))
				{
					FoundationCount--;
				}
				walkableBlocks.RemoveAt(i);
				i--;
				continue;
			}
			if (transform == null)
			{
				transform = block.transform.parent;
			}
			Vector3 localPosition = block.transform.localPosition;
			if (localPosition.x < num)
			{
				num = localPosition.x;
			}
			if (localPosition.x > num3)
			{
				num3 = localPosition.x;
			}
			if (localPosition.z < num2)
			{
				num2 = localPosition.z;
			}
			if (localPosition.z > num4)
			{
				num4 = localPosition.z;
			}
		}
		width = Mathf.Abs(num3 - num) + 1.5f;
		length = Mathf.Abs(num4 - num2) + 1.5f;
		Vector3 position = new Vector3((num + num3) / 2f, 0f, (num2 + num4) / 2f);
		base.transform.position = transform.TransformPoint(position);
		if (OnBoundsUpdate != null)
		{
			OnBoundsUpdate();
		}
	}

	private bool BlockIndexCountsAsWeight(int index)
	{
		switch (index)
		{
		case 2:
		case 189:
		case 382:
		case 383:
		case 387:
			return true;
		default:
			return false;
		}
	}

	private void OnCreateBlock(Block block, Network_Player playerNetwork, bool replicating, int hotSlotIndex)
	{
		if (!(block == null) && block.IsWalkable())
		{
			AddWalkableBlock(block, replicating: false, updateBounds: true);
		}
	}

	public void RemovedWalkableBlocks(List<Block> removedBlocks)
	{
		if (!removedBlocks.ContainsItems())
		{
			return;
		}
		bool flag = false;
		_ = Time.realtimeSinceStartup;
		List<Block> list = new List<Block>();
		for (int i = 0; i < removedBlocks.Count; i++)
		{
			Block block = removedBlocks[i];
			if (walkableBlocks.Contains(block))
			{
				list.Add(block);
				walkableBlocks.Remove(block);
				flag = true;
				if (Block.IsBlockIndexFoundation(block.buildableItem.UniqueIndex))
				{
					FoundationCount--;
				}
			}
		}
		if (flag)
		{
			UpdateBounds();
			if (OnRemoveWalkableBlocks != null)
			{
				OnRemoveWalkableBlocks(list, arg2: false);
			}
		}
	}
}
