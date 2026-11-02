using System.Collections.Generic;
using UnityEngine;

public class AI_Component_SeePlayerSwitchState : AI_Component
{
	[SerializeField]
	private AI_State detectPlayerState;

	[SerializeField]
	private float detectPlayerRadius = 5f;

	[SerializeField]
	private float detectInterval = 0.5f;

	[SerializeField]
	private bool useRaycast;

	[SerializeField]
	private LayerMask obstructionMask;

	[SerializeField]
	private float detectionRayCastRadius;

	[SerializeField]
	private bool closesPlayer;

	public bool retrievePlayerReferenceToSwitchedState;

	public bool requiresPlayerToBeValidWaterTarget;

	public QueryTriggerInteraction raycastQuery = QueryTriggerInteraction.Ignore;

	protected AI_NetworkBehaviour behaviour;

	private float detectTimer;

	private bool isThisTamed;

	private Network_Player calculationPlayer;

	public void Start()
	{
		behaviour = GetComponentInParent<AI_NetworkBehaviour>();
		if (behaviour != null)
		{
			EntityVariables difficultyEntityVariableFromAINetworkBehaviourType = GameModeValueManager.GetDifficultyEntityVariableFromAINetworkBehaviourType(behaviour.behaviourType);
			if (difficultyEntityVariableFromAINetworkBehaviourType != null)
			{
				isThisTamed = difficultyEntityVariableFromAINetworkBehaviourType.isTame;
			}
		}
	}

	protected override void OnUpdateComponent()
	{
		if (!Raft_Network.IsHost || isThisTamed)
		{
			return;
		}
		base.OnUpdateComponent();
		if (!(detectPlayerState != null))
		{
			return;
		}
		if (detectInterval > 0f)
		{
			detectTimer += Time.deltaTime;
		}
		if (!(detectInterval <= 0f) && !(detectTimer >= detectInterval))
		{
			return;
		}
		detectTimer = 0f;
		if (retrievePlayerReferenceToSwitchedState)
		{
			Network_Player nearbyPlayer = GetNearbyPlayer(closesPlayer);
			if ((!requiresPlayerToBeValidWaterTarget || Helper.IsValidWaterTarget(nearbyPlayer)) && nearbyPlayer != null)
			{
				OnSeePlayerSwitchState(nearbyPlayer);
			}
		}
		else if (PlayerIsNearby(out calculationPlayer) && (!(calculationPlayer != null) || !requiresPlayerToBeValidWaterTarget || Helper.IsValidWaterTarget(calculationPlayer)))
		{
			OnSeePlayerSwitchState(null);
		}
	}

	protected virtual void OnSeePlayerSwitchState(Network_Player playerReference)
	{
		if (playerReference == null)
		{
			SwitchState(detectPlayerState);
			return;
		}
		SwitchState<Network_Player>(detectPlayerState, playerReference);
	}

	private bool PlayerIsNearby(out Network_Player outPlayer)
	{
		List<Network_Player> list = Helper.PlayersWithinRange(transform.position, detectPlayerRadius, includeDeadPlayers: false);
		if (list.ContainsItems())
		{
			foreach (Network_Player item in list)
			{
				if (!(item == null) && !item.PlayerScript.IsDead && (connectedState.stateMachine.movement.MovementType != MovementType.NavmeshAgent || connectedState.stateMachine.movement.IsValidNavmeshPath(item)))
				{
					if (!useRaycast)
					{
						outPlayer = item;
						return true;
					}
					bool flag = ((detectionRayCastRadius == 0f) ? Helper.HasClearVisionAndWithinRange(transform.position, item.transform.position, LayerMasks.MASK_Obstruction, detectPlayerRadius) : Helper.HasClearVision(transform.position, item.transform.position, detectionRayCastRadius, LayerMasks.MASK_Obstruction));
					if (flag)
					{
						outPlayer = item;
						return flag;
					}
				}
			}
		}
		outPlayer = null;
		return false;
	}

	private Network_Player GetNearbyPlayer(bool closest)
	{
		List<Network_Player> list = Helper.PlayersWithinRange(transform.position, detectPlayerRadius, includeDeadPlayers: false);
		Network_Player result = null;
		if (list.ContainsItems())
		{
			float num = float.MaxValue;
			Vector3 position = connectedState.stateMachine.transform.position;
			foreach (Network_Player item in list)
			{
				if (item == null || item.PlayerScript.IsDead || (connectedState.stateMachine.movement.MovementType == MovementType.NavmeshAgent && !connectedState.stateMachine.movement.IsValidNavmeshPath(item)))
				{
					continue;
				}
				bool flag = false;
				if (useRaycast)
				{
					if (Helper.HasVisionToPlayer(item, transform.position, useFeetPosition: false, !useRaycast, raycastQuery))
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					if (!closest)
					{
						return item;
					}
					float num2 = Vector3.Distance(position, item.transform.position);
					if (num2 < num)
					{
						num = num2;
						result = item;
					}
				}
			}
		}
		return result;
	}
}
