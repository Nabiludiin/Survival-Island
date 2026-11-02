public class AI_State_Chase_CancelOnSeen : AI_State_Chase
{
	public AI_State seenState;

	public override void UpdateState()
	{
		if (!IsInBackOfTarget())
		{
			stateMachine.ChangeState(seenState);
		}
		else
		{
			base.UpdateState();
		}
	}

	private bool IsInBackOfTarget()
	{
		if (target == null)
		{
			return false;
		}
		if (target.transform.InverseTransformPoint(stateMachine.transform.position).z < 0f)
		{
			return true;
		}
		return false;
	}
}
