public class AI_State_MoveToInterest : AI_State_MoveToPoint
{
	protected InterestPoint interest;

	public AI_State failState;

	private bool hasValidPoint;

	public override void OnStateChange<T>(params T[] parameters)
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		if (parameters.Length != 0)
		{
			interest = (InterestPoint)(object)parameters[0];
			if (interest != null)
			{
				targetPoint = interest.GetPoint();
				hasValidPoint = true;
			}
		}
		else
		{
			hasValidPoint = false;
		}
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (Raft_Network.IsHost && !hasValidPoint)
		{
			stateMachine.ChangeState(failState);
		}
	}

	protected override void ReachedPoint()
	{
		if (Raft_Network.IsHost)
		{
			stateMachine.ChangeState<InterestPoint>(reachPointState, interest);
		}
	}
}
