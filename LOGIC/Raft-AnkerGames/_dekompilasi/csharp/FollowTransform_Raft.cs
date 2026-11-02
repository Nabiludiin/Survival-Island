using UnityEngine;

public class FollowTransform_Raft : FollowTransform
{
	[SerializeField]
	private Transform raftCenterPivot;

	[SerializeField]
	private Transform spawnerParent;

	protected override void SetPosition(Vector3 position)
	{
		spawnerParent.position = raftCenterPivot.transform.position;
		base.SetPosition(position);
	}
}
