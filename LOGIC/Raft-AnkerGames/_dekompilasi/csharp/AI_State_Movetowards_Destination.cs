using UnityEngine;

public class AI_State_Movetowards_Destination : AI_State
{
	[SerializeField]
	private float distanceToReach = 2f;

	protected Vector3 destinationPoint;

	public override void UpdateState()
	{
		base.UpdateState();
	}

	public override void ExitState()
	{
		base.ExitState();
	}

	public override void EnterState()
	{
		base.EnterState();
	}

	public override void OnStateChange<T>(params T[] parameters)
	{
		base.OnStateChange(parameters);
		destinationPoint = (Vector3)(object)parameters[0];
	}

	protected virtual void MoveTowardsDestinationPoint()
	{
	}

	protected virtual bool IsInRangeOfDestinationPoint()
	{
		if (Vector3.Distance(stateMachine.transform.position, destinationPoint) <= distanceToReach)
		{
			return true;
		}
		return false;
	}
}
