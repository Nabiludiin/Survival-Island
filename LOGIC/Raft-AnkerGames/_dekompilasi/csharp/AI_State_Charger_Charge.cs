using System.Collections;
using UnityEngine;

public class AI_State_Charger_Charge : AI_State
{
	[SerializeField]
	private AI_State nextState;

	[SerializeField]
	private float chargeLoadTime = 4f;

	[SerializeField]
	private float chargeRotateSpeed = 5f;

	[SerializeField]
	private float chargeSpeed = 5f;

	[SerializeField]
	private float chargeDamage = 1f;

	[SerializeField]
	private float maxChargeDistance = 3f;

	[SerializeField]
	private AI_Movement movement;

	private bool charge;

	private AI_StateMachine_Charger charger;

	private Vector3 chargePosition;

	protected override void Awake()
	{
		base.Awake();
		charger = stateMachine as AI_StateMachine_Charger;
	}

	public override void EnterState()
	{
		base.EnterState();
		StartCoroutine(LoadCharge());
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (Raft_Network.IsHost && charge)
		{
			movement.MoveLookObstruction_Translate(stateMachine.transform, chargePosition, chargeSpeed, movement.RotationSpeed * 5f);
			if (Helper.WithinDistance(stateMachine.transform.position, chargePosition, 0.5f))
			{
				charge = false;
				StartCoroutine(EndCharge());
			}
		}
	}

	private IEnumerator EndCharge()
	{
		if (Vector3.Distance(stateMachine.transform.position, charger.target.transform.position) < 2f)
		{
			charger.networkHost.DamageEntity(charger.target.Stats, null, chargeDamage, charger.target.transform.position, charger.transform.transform.position - stateMachine.transform.position, EntityType.Enemy);
		}
		yield return new WaitForSeconds(1f);
		stateMachine.ChangeState(nextState);
	}

	private IEnumerator LoadCharge()
	{
		float loadTimer = 0f;
		while (loadTimer < chargeLoadTime)
		{
			loadTimer += Time.deltaTime;
			chargePosition = charger.target.transform.position + Vector3.up * 0.5f;
			movement.Look(stateMachine.transform, chargePosition, chargeRotateSpeed);
			yield return null;
		}
		if (Vector3.Distance(stateMachine.transform.position, chargePosition) > maxChargeDistance)
		{
			Vector3 normalized = (chargePosition - stateMachine.transform.position).normalized;
			chargePosition = stateMachine.transform.position + normalized * maxChargeDistance;
		}
		charge = true;
	}
}
