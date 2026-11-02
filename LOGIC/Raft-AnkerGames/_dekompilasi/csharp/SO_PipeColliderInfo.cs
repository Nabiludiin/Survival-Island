using UnityEngine;

[CreateAssetMenu(fileName = "PipeColliderInfo", menuName = "Raft/PipeColliderInfo")]
public class SO_PipeColliderInfo : ScriptableObject
{
	public PipeColliderConnection[] pipeColliderConnections;
}
