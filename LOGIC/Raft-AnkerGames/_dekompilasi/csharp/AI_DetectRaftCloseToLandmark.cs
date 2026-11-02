using System.Collections.Generic;
using UnityEngine;

public class AI_DetectRaftCloseToLandmark : MonoBehaviour
{
	[SerializeField]
	private AI_StateMachine stateMachine;

	[SerializeField]
	private TimerEventer checkTimer;

	[SerializeField]
	private AI_State nextState;

	[SerializeField]
	private List<AI_State> statesToCheckIn;

	private void Update()
	{
		if (Raft_Network.IsHost && !(stateMachine == null) && !(nextState == null) && statesToCheckIn.Contains(stateMachine.currentState) && checkTimer.DoUpdate(Time.deltaTime, autoResetTimer: true) && ChunkManager.RaftIsInsideChunkPoint)
		{
			stateMachine.ChangeState(nextState);
		}
	}
}
