using UnityEngine;

public class AI_State_StateRandomizer : AI_State
{
	[SerializeField]
	private Randomizer randomizer;

	[SerializeField]
	private bool allowSameRandomTwice = true;

	private AI_State lastRandomState;

	public override void EnterState()
	{
		base.EnterState();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		int itemCount = randomizer.GetItemCount();
		if (itemCount <= 0)
		{
			Debug.LogError("randomizer does not have states", gameObject);
		}
		AI_State randomItem = randomizer.GetRandomItem<AI_State>();
		if (!allowSameRandomTwice && itemCount > 1 && randomItem == lastRandomState)
		{
			int num = 20;
			while (randomItem == lastRandomState && num > 0)
			{
				randomItem = randomizer.GetRandomItem<AI_State>();
				num--;
			}
		}
		if (randomItem == null)
		{
			stateMachine.ChangeToPreviousState();
			return;
		}
		stateMachine.ChangeState(randomItem);
		lastRandomState = randomItem;
	}
}
