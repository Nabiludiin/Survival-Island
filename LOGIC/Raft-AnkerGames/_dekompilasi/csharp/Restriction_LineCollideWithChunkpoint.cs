using UnityEngine;

[CreateAssetMenu(fileName = "Restriction_LineCollideWithChunkpoint", menuName = "Raft/Restriction/LineCollideWithChunkpoint")]
public class Restriction_LineCollideWithChunkpoint : Restriction
{
	public static Vector3 latestValidSpawnPoint;

	public float radiusFromRaft = 300f;

	public int maxCheckIterations = 5;

	public override bool IsRestricted(WorldEvent we)
	{
		return !WorldEventManager.networkHostEntities.GetSpawnPositionDontCollideWithChunkPoint(ref latestValidSpawnPoint, radiusFromRaft, maxCheckIterations);
	}
}
