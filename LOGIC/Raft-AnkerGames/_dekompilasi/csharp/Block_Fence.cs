using System.Collections.Generic;
using UnityEngine;

public class Block_Fence : Block
{
	[SerializeField]
	private int knobPriority;

	[SerializeField]
	private GizmoBox gizmoBoxLeft;

	[SerializeField]
	private GizmoBox gizmoBoxRight;

	public Transform knobLeft;

	public Transform knobRight;

	[SerializeField]
	private Collider currentFenceCollider;

	public override void OnFinishedPlacement()
	{
		base.OnFinishedPlacement();
		if ((!Raft_Network.IsHost || !SaveAndLoad.IsGameLoading) && Raft_Network.WorldHasBeenRecieved)
		{
			RefreshKnob(GetOverlappingFences(gizmoBoxLeft), this, gizmoBoxLeft.transform.position);
			RefreshKnob(GetOverlappingFences(gizmoBoxRight), this, gizmoBoxRight.transform.position);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (hasBeenPlaced && !GameManager.IsLeavingGame)
		{
			List<Block_Fence> overlappingFences = GetOverlappingFences(gizmoBoxRight);
			if (overlappingFences != null)
			{
				overlappingFences.Remove(this);
				RefreshKnob(overlappingFences, overlappingFences[0], gizmoBoxRight.transform.position);
			}
			overlappingFences = GetOverlappingFences(gizmoBoxLeft);
			if (overlappingFences != null)
			{
				overlappingFences.Remove(this);
				RefreshKnob(overlappingFences, overlappingFences[0], gizmoBoxLeft.transform.position);
			}
		}
	}

	private List<Block_Fence> GetOverlappingFences(GizmoBox box)
	{
		IEnumerable<Collider> overlaps = box.GetOverlaps(includeInactiveObjects: false, LayerMasks.MASK_Block, 1f);
		List<Block_Fence> list = null;
		foreach (Collider item in overlaps)
		{
			if (!(item != null) || !item.CompareTag("BlockFence"))
			{
				continue;
			}
			Block_Fence component = item.transform.parent.GetComponent<Block_Fence>();
			if (component != null)
			{
				if (list == null)
				{
					list = new List<Block_Fence>();
				}
				list.Add(component);
			}
		}
		return list;
	}

	private void RefreshKnob(List<Block_Fence> otherFences, Block_Fence newFence, Vector3 knobPosition)
	{
		if (otherFences == null)
		{
			return;
		}
		Block_Fence block_Fence = null;
		foreach (Block_Fence otherFence in otherFences)
		{
			otherFence.SetClosestKnobState(knobPosition, state: false);
			if (otherFence.ShouldBeKnobbed(newFence))
			{
				block_Fence = ((otherFence.knobPriority > newFence.knobPriority) ? otherFence : newFence);
			}
		}
		if (block_Fence != null)
		{
			block_Fence.SetClosestKnobState(knobPosition, state: true);
		}
	}

	private void SetClosestKnobState(Vector3 position, bool state)
	{
		float num = Vector3.Distance(knobLeft.position, position);
		float num2 = Vector3.Distance(knobRight.position, position);
		if (num < num2)
		{
			knobLeft.gameObject.SetActiveSafe(state);
		}
		else
		{
			knobRight.gameObject.SetActiveSafe(state);
		}
	}

	private bool ShouldBeKnobbed(Block_Fence other)
	{
		if (other == null || other == this)
		{
			return false;
		}
		Vector3 vector = transform.InverseTransformPoint(other.transform.position);
		vector.z = (float)Mathf.RoundToInt(vector.z / 0.75f) * 0.75f;
		return vector.z != 0f;
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Block_Fence(RGDType.Block_Fence, this);
	}

	public override RGD_Block GetBlockCreationData()
	{
		return new RGD_Block_Fence(RGDType.Block_Fence, this);
	}
}
