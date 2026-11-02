using UnityEngine;

public class BlockQuad : MonoBehaviour
{
	public SO_BlockQuadType quadType;

	public BlockSurface[] acceptableBuildSides;

	public bool snapToQuadRotation;

	public bool snapToQuadPosition;

	private BoxCollider quadCollider;

	private Block parentBlock;

	public BoxCollider QuadCollider
	{
		get
		{
			if (quadCollider == null)
			{
				quadCollider = transform.GetComponent<BoxCollider>();
			}
			return quadCollider;
		}
	}

	public Block ParentBlock => parentBlock;

	private void OnEnable()
	{
		parentBlock = GetComponentInParent<Block>();
	}

	public bool CanBuildOnSurface(Item_Base blockItem, Vector3 surfaceNormal)
	{
		SurfaceType surfaceType = ConvertNormalToSurfaceType(surfaceNormal, transform);
		BlockSurface[] array = acceptableBuildSides;
		foreach (BlockSurface blockSurface in array)
		{
			if ((blockSurface.surfaceType == SurfaceType.All || blockSurface.surfaceType == surfaceType) && blockItem.settings_buildable.GetBlockPrefab(blockSurface.dpsType) != null)
			{
				return true;
			}
		}
		return false;
	}

	public bool AcceptsDpsType(DPS type)
	{
		BlockSurface[] array = acceptableBuildSides;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].dpsType == type)
			{
				return true;
			}
		}
		return false;
	}

	public bool AcceptsBlock(Item_Base blockItem, Vector3 surfaceNormal)
	{
		if (quadType.AcceptsBlock(blockItem))
		{
			return CanBuildOnSurface(blockItem, surfaceNormal);
		}
		return false;
	}

	public BlockSurface GetSurfaceFromNormal(Vector3 surfaceNormal)
	{
		SurfaceType surfaceType = ConvertNormalToSurfaceType(surfaceNormal, transform);
		BlockSurface[] array = acceptableBuildSides;
		foreach (BlockSurface blockSurface in array)
		{
			if (blockSurface.surfaceType == SurfaceType.All || blockSurface.surfaceType == surfaceType)
			{
				return blockSurface;
			}
		}
		return null;
	}

	public static SurfaceType ConvertNormalToSurfaceType(Vector3 normal, Transform objectTransform)
	{
		if (normal == objectTransform.forward)
		{
			return SurfaceType.Front;
		}
		if (normal == -objectTransform.forward)
		{
			return SurfaceType.Back;
		}
		if (normal == objectTransform.right)
		{
			return SurfaceType.Right;
		}
		if (normal == -objectTransform.right)
		{
			return SurfaceType.Left;
		}
		if (normal == objectTransform.up)
		{
			return SurfaceType.Top;
		}
		if (normal == -objectTransform.up)
		{
			return SurfaceType.Bottom;
		}
		return SurfaceType.None;
	}
}
