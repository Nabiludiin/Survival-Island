using UnityEngine;

[CreateAssetMenu(fileName = "Restriction_RaftInChunkpoint", menuName = "Raft/Restriction/RaftInChunkpoint")]
public class Restriction_RaftInChunkpoint : Restriction
{
	public override bool IsRestricted(WorldEvent we)
	{
		return ChunkManager.RaftIsInsideChunkPoint;
	}
}
