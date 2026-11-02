using UnityEngine;

public class AI_State_Dead_Domestic : AI_State_Dead_Ragdoll
{
	private AI_StateMachine_Domestic stateMachineDomestic;

	protected override void Start()
	{
		base.Start();
		stateMachineDomestic = stateMachine as AI_StateMachine_Domestic;
	}

	public override void EnterState()
	{
		base.EnterState();
		if (stateMachineDomestic != null)
		{
			stateMachineDomestic.networkBehaviourDomestic.captureScript.IsCapturable = false;
			stateMachineDomestic.networkBehaviourDomestic.carryScript.AllowCarry = false;
			stateMachineDomestic.networkBehaviourDomestic.carryScript.DisableCollider();
		}
	}

	protected override void OnApplyRagdollForce(Vector3 force)
	{
		if (stateMachine.transform.parent == null)
		{
			base.OnApplyRagdollForce(force);
		}
		else
		{
			ragdollManager.SetRagdollState(state: true);
		}
	}
}
