using UnityEngine;

public class StableComponent : MonoBehaviour
{
	public bool requireAll;

	public int requiredHitCount = 1;

	public GizmoBox[] requiredGizmoColliders;

	private void OnValidate()
	{
		if (requiredGizmoColliders == null || requiredGizmoColliders.Length == 0)
		{
			requiredHitCount = 0;
			return;
		}
		if (requireAll)
		{
			requiredHitCount = requiredGizmoColliders.Length;
		}
		if (requiredHitCount > requiredGizmoColliders.Length)
		{
			requiredHitCount = requiredGizmoColliders.Length;
		}
		if (requiredHitCount < 1)
		{
			requiredHitCount = 1;
		}
	}

	public bool IsStable()
	{
		if (requiredHitCount == 0)
		{
			return true;
		}
		int overlapCount = GetOverlapCount();
		if (requireAll)
		{
			return overlapCount == requiredGizmoColliders.Length;
		}
		return overlapCount >= requiredHitCount;
	}

	private int GetOverlapCount()
	{
		int num = 0;
		GizmoBox[] array = requiredGizmoColliders;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].IsColliding(includeInactiveObjects: false, LayerMasks.MASK_Block, 0.99f))
			{
				num++;
			}
		}
		return num;
	}
}
