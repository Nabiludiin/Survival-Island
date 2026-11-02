using UnityEngine;

public class AI_State_Indexer : AI_State
{
	[SerializeField]
	private AI_State[] nextStates;

	[SerializeField]
	private bool randomizeFirstIndex = true;

	private int currentIndex = -1;

	protected override void Start()
	{
		base.Start();
		if (randomizeFirstIndex)
		{
			currentIndex = -1;
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		if (Raft_Network.IsHost)
		{
			if (currentIndex < 0)
			{
				currentIndex = Random.Range(0, nextStates.Length);
			}
			else
			{
				currentIndex.IncrementCycle(1, 0, nextStates.Length - 1);
			}
			AI_State aI_State = nextStates[currentIndex];
			if (aI_State != null)
			{
				stateMachine.ChangeState(aI_State);
			}
			else
			{
				Debug.Log("Next state was null " + stateMachine.name, gameObject);
			}
		}
	}
}
