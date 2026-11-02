using UnityEngine;

public class AI_State_BiteBait : AI_State
{
	private SharkBait targetBait;

	[SerializeField]
	private float biteRange = 3f;

	[SerializeField]
	private int biteDamage;

	[SerializeField]
	private float biteCooldown = 1f;

	[SerializeField]
	private float biteProgress;

	public override void UpdateState()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		base.UpdateState();
		if (Vector3.Distance(stateMachine.transform.position, targetBait.transform.position) < biteRange)
		{
			biteProgress += Time.deltaTime;
			if (biteProgress >= biteCooldown)
			{
				BiteBait(targetBait);
			}
		}
		else
		{
			MoveTowardsBait(targetBait);
		}
	}

	public override void OnStateChange<T>(params T[] parameters)
	{
		base.OnStateChange(parameters);
		targetBait = parameters[0] as SharkBait;
	}

	protected virtual void MoveTowardsBait(SharkBait targetBait)
	{
		stateMachine.movement.GoldenRuleMovement(targetBait.transform.position, stateMachine.rigid, stateMachine.movement.MovementSpeed);
	}

	protected virtual void BiteBait(SharkBait targetBait)
	{
	}

	private void AttackBait(SharkBait p_targetBait)
	{
		p_targetBait.Damage(biteDamage);
		p_targetBait.isAttacked = true;
	}

	private void InitiateAttack()
	{
		stateMachine.transform.SetParent(targetBait.transform);
	}

	private void ReleaseBait(SharkBait p_targetBait)
	{
		p_targetBait.isAttacked = false;
	}

	public override RGD_AI_State SaveState()
	{
		return base.SaveState();
	}
}
