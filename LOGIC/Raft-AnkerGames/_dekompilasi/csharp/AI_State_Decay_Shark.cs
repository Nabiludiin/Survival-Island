using UnityEngine;

public class AI_State_Decay_Shark : AI_State_Decay
{
	public float decayRespawnTime = 80f;

	private AI_StateMachine_Shark stateMachineShark;

	private Rigidbody rigid;

	protected override void Start()
	{
		stateMachineShark = stateMachine as AI_StateMachine_Shark;
		rigid = stateMachineShark.GetComponent<Rigidbody>();
	}

	public override void EnterState()
	{
		base.EnterState();
		if (stateMachineShark == null)
		{
			stateMachineShark = stateMachine as AI_StateMachine_Shark;
		}
		stateMachineShark.networkBehaviour.pickupItemChanneling.SetPickupColliderState(state: true);
	}

	protected override bool CheckToSpawnOnDecay()
	{
		Network_Host_Entities value = ComponentManager<Network_Host_Entities>.Value;
		int sharkCount = value.SharkCount;
		if (sharkCount == 1)
		{
			value.StartCoroutine(value.CreateShark(decayRespawnTime, Vector3.zero, AI_NetworkBehaviourType.Shark));
		}
		if (SpawnAdditionalShark(sharkCount))
		{
			value.StartCoroutine(value.CreateShark(decayRespawnTime, Vector3.zero, AI_NetworkBehaviourType.Shark));
		}
		return base.CheckToSpawnOnDecay();
	}

	protected override void FinalDecay()
	{
		CheckToSpawnOnDecay();
		base.FinalDecay();
	}

	private bool SpawnAdditionalShark(int sharkCount)
	{
		if (GameModeValueManager.GetCurrentGameModeValue().gameMode == GameMode.Hardcore)
		{
			int num = 5;
			if (Random.Range(0, 10) <= num - sharkCount)
			{
				return true;
			}
			return false;
		}
		return false;
	}
}
