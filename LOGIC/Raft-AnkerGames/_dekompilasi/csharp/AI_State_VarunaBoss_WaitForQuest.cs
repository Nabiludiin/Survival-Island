using UnityEngine;

public class AI_State_VarunaBoss_WaitForQuest : AI_State_WaitForQuest
{
	public float introSeePlayerCooldown = 8f;

	private AI_StateMachine_Boss_Varuna bossSM;

	protected override void Start()
	{
		base.Start();
		bossSM = stateMachine as AI_StateMachine_Boss_Varuna;
		if (bossSM.networkBehaviour.ConnectedToSpawner)
		{
			stateMachine.transform.rotation = Quaternion.LookRotation(bossSM.networkBehaviour.connectedSpawner.transform.forward);
		}
	}

	protected override void SwapState()
	{
		if (Raft_Network.IsHost)
		{
			bossSM.waypointCirculation.confirmedCirculationID = ObjectOfInterestType.Varuna_Boss_Floor_1;
			bossSM.waypointCirculation.UpdateCirculationID(ObjectOfInterestType.Varuna_Boss_Floor_1);
		}
		bossSM.seePlayer_component.SetCooldownTimeStamp(introSeePlayerCooldown);
		base.SwapState();
	}
}
