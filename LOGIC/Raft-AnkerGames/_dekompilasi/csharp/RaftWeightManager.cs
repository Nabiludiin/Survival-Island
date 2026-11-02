using UnityEngine;

public class RaftWeightManager : MonoBehaviour
{
	private static RaftBounds raftBounds;

	public static int TotallWeight => raftBounds.WalkableBlocksCount;

	public static int FoundationWeight => Mathf.Clamp(raftBounds.FoundationCount, 0, 600);

	private void Start()
	{
		if (raftBounds == null)
		{
			raftBounds = ComponentManager<RaftBounds>.Value;
		}
	}
}
