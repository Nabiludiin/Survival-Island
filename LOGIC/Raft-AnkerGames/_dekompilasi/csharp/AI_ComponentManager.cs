using System;
using UnityEngine;

public class AI_ComponentManager : MonoBehaviour
{
	[SerializeField]
	private bool listenToDamageTaken;

	[SerializeField]
	private Network_Entity networkEntity;

	[SerializeField]
	private INTERFACE_AI_Component_DamageTakenEvent[] components_damageTakenEvent;

	private AI_Component[] components_all;

	private AI_StateMachine stateMachine;

	private void Awake()
	{
		stateMachine = GetComponent<AI_StateMachine>();
		AI_StateMachine aI_StateMachine = stateMachine;
		aI_StateMachine.OnEnterState = (Action<AI_State>)Delegate.Combine(aI_StateMachine.OnEnterState, new Action<AI_State>(OnEnterStateEvent));
		AI_StateMachine aI_StateMachine2 = stateMachine;
		aI_StateMachine2.OnExitState = (Action<AI_State>)Delegate.Combine(aI_StateMachine2.OnExitState, new Action<AI_State>(OnExitStateEvent));
		InitializeComponents();
	}

	private void OnEnterStateEvent(AI_State state)
	{
		if (!(state != null))
		{
			return;
		}
		for (int i = 0; i < components_all.Length; i++)
		{
			AI_Component aI_Component = components_all[i];
			if (aI_Component.connectedState == state)
			{
				aI_Component.OnEnterState();
				break;
			}
		}
	}

	private void OnExitStateEvent(AI_State state)
	{
		if (!(state != null))
		{
			return;
		}
		for (int i = 0; i < components_all.Length; i++)
		{
			AI_Component aI_Component = components_all[i];
			if (aI_Component.connectedState == state)
			{
				aI_Component.OnExitState();
				break;
			}
		}
	}

	private void OnDamageTakenEvent(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageInflictorEntityType, bool isLocal)
	{
		INTERFACE_AI_Component_DamageTakenEvent[] array = components_damageTakenEvent;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].OnDamageTakenEvent(damage, hitPoint, hitNormal, damageInflictorEntityType, isLocal);
		}
	}

	private void InitializeComponents()
	{
		components_all = GetComponentsInChildren<AI_Component>(includeInactive: true);
		if (listenToDamageTaken && networkEntity != null)
		{
			components_damageTakenEvent = GetComponentsInChildren<INTERFACE_AI_Component_DamageTakenEvent>(includeInactive: true);
			if (components_damageTakenEvent.ContainsItems())
			{
				networkEntity.OnDamageTakenEvent += OnDamageTakenEvent;
			}
		}
	}
}
