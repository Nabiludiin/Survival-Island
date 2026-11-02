using UnityEngine;

public class AdvancedCollision : MonoBehaviour
{
	[HideInInspector]
	public BoxCollider[] colliders;

	private void Awake()
	{
		colliders = GetComponents<BoxCollider>();
	}

	public OverlappType IsOverlapping(BoxCollider[] advCollisions, SO_BlockCollisionMask mask, Block parentBlock)
	{
		BoxCollider[] array;
		if (mask == null)
		{
			array = colliders;
			foreach (BoxCollider boxCollider in array)
			{
				if (boxCollider == null)
				{
					continue;
				}
				Collider hitCollider = null;
				if (!Helper.IsColliding(boxCollider, LayerMasks.MASK_BlockCreatorOverlap, advCollisions, out hitCollider))
				{
					continue;
				}
				if (hitCollider == null)
				{
					return OverlappType.Overlapp;
				}
				Block componentInParent = hitCollider.GetComponentInParent<Block>();
				if (componentInParent == null)
				{
					return OverlappType.Overlapp;
				}
				if (componentInParent == parentBlock)
				{
					continue;
				}
				SO_BlockCollisionMask blockCollisionMask = componentInParent.blockCollisionMask;
				if (blockCollisionMask != null && blockCollisionMask.ignoreAll && blockCollisionMask.exceptSelf && parentBlock == componentInParent)
				{
					return OverlappType.Overlapp_Same;
				}
				if (blockCollisionMask == null || !blockCollisionMask.ignoreAll || !blockCollisionMask.IgnoresBlock(parentBlock.buildableItem))
				{
					if (HasSamePosition(componentInParent, parentBlock))
					{
						return OverlappType.Overlapp_Same;
					}
					return OverlappType.Overlapp;
				}
			}
			return OverlappType.None;
		}
		if (mask.ignoreAll && !mask.exceptSelf)
		{
			return OverlappType.None;
		}
		array = colliders;
		foreach (BoxCollider boxCollider2 in array)
		{
			if (boxCollider2 == null)
			{
				continue;
			}
			Collider[] collisions = Helper.GetCollisions(boxCollider2, LayerMasks.MASK_BlockCreatorOverlap, advCollisions);
			foreach (Collider collider in collisions)
			{
				if (collider == null)
				{
					continue;
				}
				Block componentInParent2 = collider.GetComponentInParent<Block>();
				if (componentInParent2 == null)
				{
					return OverlappType.Overlapp;
				}
				if (!(componentInParent2 == parentBlock))
				{
					if (HasSamePosition(componentInParent2, parentBlock))
					{
						return OverlappType.Overlapp_Same;
					}
					if ((!(componentInParent2.blockCollisionMask != null) || (!componentInParent2.blockCollisionMask.ignoreAll && !componentInParent2.blockCollisionMask.IgnoresBlock(parentBlock.buildableItem))) && !mask.IgnoresBlock(componentInParent2.buildableItem))
					{
						return OverlappType.Overlapp;
					}
				}
			}
		}
		return OverlappType.None;
	}

	private bool HasSamePosition(Block block, Block block2)
	{
		return Vector3.Distance(block.transform.localPosition, block2.transform.localPosition) < 0.05f;
	}
}
