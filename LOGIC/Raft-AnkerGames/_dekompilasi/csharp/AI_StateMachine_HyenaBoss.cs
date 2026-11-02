using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_StateMachine_HyenaBoss : AI_StateMachine_Animal
{
	public AcidPool acidPoolPrefab;

	public float acidPoolDuration;

	public AI_State eatState;

	public AI_State throwState;

	public List<AI_State> statesToIncreaseEatTimer = new List<AI_State>();

	public List<AI_State> canSwitchToEatStates = new List<AI_State>();

	public TimerEventer eatTimer;

	public float damageThreshold = 50f;

	public GameObject playerCollider;

	[HideInInspector]
	private AreaZone zone;

	[HideInInspector]
	public List<AcidPool> spawnedPools = new List<AcidPool>();

	private float damageTaken;

	private float zoneTimer;

	public override void Start()
	{
		base.Start();
		zone = GetBossZone();
	}

	protected override void UpdateStateMachine()
	{
		base.UpdateStateMachine();
		if (!Raft_Network.IsHost || currentState == startState)
		{
			return;
		}
		if (GetBossZone() != null)
		{
			zoneTimer += Time.deltaTime;
			if (zoneTimer >= 10f)
			{
				zoneTimer = 0f;
				if (GetBossZone().GetNormalizedDistanceToPoint(transform.position) <= 0f)
				{
					NavMeshAgent component = GetComponent<NavMeshAgent>();
					if (component != null)
					{
						Vector3 centerOfArea = GetBossZone().GetCenterOfArea();
						centerOfArea.y = GetBossZone().transform.position.y;
						centerOfArea = Helper.GetClosestPointOnNavMesh(centerOfArea, 2f);
						component.Warp(centerOfArea);
					}
				}
			}
		}
		bool flag = false;
		if (statesToIncreaseEatTimer.Contains(currentState))
		{
			eatTimer.DoUpdate(Time.deltaTime);
			if (eatTimer.IsTimeBeyondCooldown() && canSwitchToEatStates.Contains(statesToIncreaseEatTimer[0]))
			{
				flag = true;
			}
		}
		if (damageTaken >= damageThreshold)
		{
			flag = true;
		}
		if (flag && currentState != decayState && currentState != deadState)
		{
			eatTimer.ResetTimer();
			damageTaken = 0f;
			ChangeState(eatState);
		}
	}

	protected override void OnDamageTaken(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageType, bool isLocal)
	{
		if (Raft_Network.IsHost)
		{
			damageTaken += damage;
		}
		if (currentState != deadState && currentState != decayState)
		{
			base.OnDamageTaken(damage, hitPoint, hitNormal, damageType, isLocal);
			if (!networkBehaviour.networkEntity.IsInvurnerable)
			{
				CreateAcidPool(transform.position, 2f);
			}
		}
	}

	protected override void OnDeath(EntityType damageType)
	{
		base.OnDeath(damageType);
		if (Raft_Network.IsHost)
		{
			QuestProgressTracker.FinishQuestRPC(QuestType.Utopia_LarsKilled);
		}
		if (playerCollider != null)
		{
			playerCollider.SetActiveSafe(value: false);
		}
	}

	public AreaZone GetBossZone()
	{
		if (zone == null)
		{
			Transform objectOfInterest = ObjectManager.GetObjectOfInterest(ObjectOfInterestType.Utopia_HyenaBossAreaZone);
			if (objectOfInterest != null)
			{
				zone = objectOfInterest.GetComponent<AreaZone>();
				return zone;
			}
			return null;
		}
		return zone;
	}

	public bool IsValidTarget(Network_Player player)
	{
		if (player == null || player.PlayerScript.IsDead)
		{
			return false;
		}
		if (GetBossZone().GetNormalizedDistanceToPoint(player.transform.position) <= 0f)
		{
			return false;
		}
		return true;
	}

	public AcidPool CreateAcidPool(Vector3 position, float size)
	{
		Vector3 position2 = position;
		if (GetBossZone() != null)
		{
			position2.y = GetBossZone().transform.position.y;
		}
		Vector3 startScale = new Vector3(size, acidPoolPrefab.transform.localScale.y, size);
		Vector3 endScale = new Vector3(1f, startScale.y, 1f);
		float num = acidPoolDuration / 3f;
		AcidPool acidPool = UnityEngine.Object.Instantiate(acidPoolPrefab, position2, Quaternion.identity);
		ParticleSystem acidParticles = acidPool.particleShrink.GetComponentInChildren<ParticleSystem>();
		acidPool.damageAreaShrink.Initialize(startScale, endScale, num * 2f, num);
		acidPool.particleShrink.Initialize(Vector3.one * size, Vector3.one, num * 2f, num);
		ShrinkAndDestroy damageAreaShrink = acidPool.damageAreaShrink;
		damageAreaShrink.OnShrinkComplete = (Action)Delegate.Combine(damageAreaShrink.OnShrinkComplete, (Action)(() =>
		{
			RemovePool(acidPool, acidParticles);
		}));
		spawnedPools.Add(acidPool);
		return acidPool;
	}

	private void RemovePool(AcidPool acidPool, ParticleSystem acidParticles)
	{
		spawnedPools.Remove(acidPool);
		acidParticles.transform.SetParentSafe(null);
		acidParticles.Stop();
		UnityEngine.Object.Destroy(acidParticles.gameObject, 5f);
		UnityEngine.Object.Destroy(acidPool.gameObject, 1f);
	}

	public override RGD_StateMachine GetStateMachineData()
	{
		return new RGD_StateMachine_HyenaBoss(this);
	}
}
