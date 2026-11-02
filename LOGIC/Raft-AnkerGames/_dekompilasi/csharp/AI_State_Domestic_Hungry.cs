using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI_State_Domestic_Hungry : AI_State
{
	[SerializeField]
	private AI_State_Domestic_Eat eatState;

	[SerializeField]
	private AI_State_Domestic_Walk defaultNoFoodStrafeState;

	[SerializeField]
	private float minDistanceToEat = 2f;

	[SerializeField]
	private float originFoodCheckCooldown = 1f;

	[SerializeField]
	private float foodCheckDistance = 5f;

	[SerializeField]
	private LayerMask foodObstructionLayer;

	[SerializeField]
	private float reEnterStateCooldown = 6f;

	public Transform lineCastTransform;

	private AI_StateMachine_Domestic stateMachine_Domestic;

	private Plant targetFood;

	private float foodCheckCooldown;

	public Transform TargetPoint
	{
		get
		{
			if (targetFood != null)
			{
				return targetFood.transform;
			}
			return null;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		stateMachine_Domestic = stateMachine as AI_StateMachine_Domestic;
	}

	public override void EnterState()
	{
		base.EnterState();
		if (Raft_Network.IsHost && stateMachine_Domestic != null)
		{
			stateMachine_Domestic.networkBehaviourDomestic.CooldownToHungerState = reEnterStateCooldown;
			CheckForViableFood();
		}
		stateMachine.movement.SetMovementType(MovementType.CharacterController);
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		foodCheckCooldown -= Time.deltaTime;
		if (foodCheckCooldown <= 0f)
		{
			CheckForViableFood();
		}
		Transform targetPoint = TargetPoint;
		if (targetPoint == null || !stateMachine.movement.hasRaftGroundUnderneath)
		{
			stateMachine_Domestic.ChangeState(defaultNoFoodStrafeState);
			return;
		}
		MoveTowardsFood(targetPoint);
		if (IsFoodInRange())
		{
			eatState.plantToEat = targetFood;
			stateMachine_Domestic.ChangeState(eatState);
		}
	}

	private void CheckForViableFood()
	{
		targetFood = GetNearbyGrassPlant();
		foodCheckCooldown = originFoodCheckCooldown;
	}

	private Plant GetNearbyGrassPlant()
	{
		Collider[] array = Physics.OverlapSphere(stateMachine_Domestic.transform.position, foodCheckDistance, LayerMasks.MASK_Item, QueryTriggerInteraction.Collide);
		if (array.Length == 0)
		{
			return null;
		}
		List<Cropplot> list = new List<Cropplot>();
		List<Cropplot> list2 = new List<Cropplot>();
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			if (!collider.transform.CompareTag("Cropplot"))
			{
				continue;
			}
			Cropplot component = collider.GetComponent<Cropplot>();
			if (component != null)
			{
				if (IsFoodWithinClearSight(component.transform))
				{
					list.Add(component);
				}
				else
				{
					list2.Add(component);
				}
			}
		}
		list = list.OrderBy((Cropplot x) => Vector3.Distance(stateMachine_Domestic.transform.position, x.transform.position)).ToList();
		if (list.Count > 0)
		{
			for (int num = 0; num < list.Count; num++)
			{
				Plant plant = FindGrassOnCropplot(list[num]);
				if (plant != null)
				{
					return plant;
				}
			}
		}
		list2 = list2.OrderBy((Cropplot x) => Vector3.Distance(stateMachine_Domestic.transform.position, x.transform.position)).ToList();
		if (list2.Count > 0)
		{
			for (int num2 = 0; num2 < list2.Count; num2++)
			{
				Plant plant2 = FindGrassOnCropplot(list2[num2]);
				if (plant2 != null)
				{
					return plant2;
				}
			}
		}
		return null;
	}

	private void MoveTowardsFood(Transform target)
	{
		Vector3 position = target.position;
		Vector3 normalized = (target.position - stateMachine.transform.position).normalized;
		if (Physics.Linecast(target.transform.position + Vector3.up * 0.5f, lineCastTransform.position, foodObstructionLayer, QueryTriggerInteraction.Ignore))
		{
			Debug.DrawLine(target.transform.position + Vector3.up * 0.5f, lineCastTransform.position, Color.red);
			stateMachine.movement.MoveLookObstruction_CharacterController(stateMachine.movement.model, stateMachine.movement.model.forward, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed);
		}
		else
		{
			Debug.DrawLine(target.transform.position + Vector3.up * 0.5f, lineCastTransform.position, Color.green);
			stateMachine.movement.MoveDirection_CharacterController(normalized, stateMachine.movement.MovementSpeed);
		}
		Vector3.Distance(stateMachine.transform.position, position);
	}

	private Plant FindGrassOnCropplot(Cropplot cropplot)
	{
		foreach (PlantationSlot slot in cropplot.GetSlots())
		{
			if (slot.plant != null && slot.plant.item.UniqueIndex == eatState.itemToEat.UniqueIndex && slot.plant.FullyGrown() && IsFoodReachableY(slot.transform))
			{
				return slot.plant;
			}
		}
		return null;
	}

	private bool IsFoodReachableY(Transform food)
	{
		if (food.transform.position.y - stateMachine_Domestic.transform.position.y >= 1.21f)
		{
			return false;
		}
		return true;
	}

	private bool IsFoodInRange()
	{
		Transform targetPoint = TargetPoint;
		if (targetPoint == null)
		{
			return false;
		}
		if (Vector3.Distance(stateMachine_Domestic.transform.position, targetPoint.position) <= minDistanceToEat)
		{
			return true;
		}
		return false;
	}

	private bool IsFoodWithinClearSight(Transform target)
	{
		if (Physics.Linecast(target.transform.position, stateMachine_Domestic.transform.position, foodObstructionLayer, QueryTriggerInteraction.Ignore))
		{
			return false;
		}
		return true;
	}
}
