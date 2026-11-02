using System;
using UnityEngine;
using UnityEngine.AI;

public class AI_StateMachine : MonoBehaviour
{
	public Action<AI_State> OnExitState;

	public Action<AI_State> OnEnterState;

	[SerializeField]
	protected AI_State startState;

	public AI_State currentState;

	public AI_State previousState;

	[HideInInspector]
	public AI_Movement movement;

	public Rigidbody rigid;

	public virtual void Awake()
	{
		rigid = GetComponent<Rigidbody>();
		movement = GetComponentInChildren<AI_Movement>();
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Combine(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
	}

	public virtual void Start()
	{
		if (Raft_Network.IsHost)
		{
			Invoke("LateStart", 0.5f);
		}
	}

	public virtual void OnDestroy()
	{
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Remove(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
	}

	private void LateStart()
	{
		if (!SaveAndLoad.IsGameLoading)
		{
			if (currentState == null)
			{
				ChangeState(startState);
			}
		}
		else
		{
			Debug.Log("Aborting statemachine start change because game was loading");
		}
	}

	public void Update()
	{
		UpdateStateMachine();
	}

	protected virtual void UpdateStateMachine()
	{
		if (currentState != null && Raft_Network.IsAllLandmarksLoaded)
		{
			currentState.UpdateState();
		}
	}

	protected virtual void OnWorldShift(Vector3 shift)
	{
		NavMeshAgent agent = movement.agent;
		if (agent != null && agent.enabled)
		{
			agent.enabled = false;
			Invoke("LateShift", 0.1f);
		}
		if (transform.parent == null)
		{
			transform.position -= shift;
		}
		AI_State[] componentsInChildren = GetComponentsInChildren<AI_State>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].OnWorldShift(shift);
		}
	}

	private void LateShift()
	{
		if (movement != null)
		{
			NavMeshAgent agent = movement.agent;
			if (agent != null)
			{
				agent.enabled = true;
			}
		}
	}

	public void ChangeToPreviousState()
	{
		if (Raft_Network.IsHost && previousState != null)
		{
			ChangeState(previousState);
		}
	}

	public void ChangeState(AI_State newState)
	{
		if (currentState != null)
		{
			currentState.ExitState();
			if (OnExitState != null)
			{
				OnExitState(currentState);
			}
		}
		previousState = currentState;
		currentState = newState;
		if (currentState != null)
		{
			currentState.EnterState();
			if (OnEnterState != null)
			{
				OnEnterState(newState);
			}
		}
	}

	public void ChangeState<T>(AI_State newState, params T[] parameters)
	{
		ChangeState(newState);
		if (currentState != null)
		{
			currentState.OnStateChange(parameters);
		}
	}

	public void ChangeState(uint stateIndex)
	{
		AI_State[] componentsInChildren = GetComponentsInChildren<AI_State>();
		if (componentsInChildren == null)
		{
			return;
		}
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].stateIndex == stateIndex)
			{
				ChangeState(componentsInChildren[i]);
				break;
			}
		}
	}

	public virtual void OnStateMachineMessage(AIStateEvent eventType, string message)
	{
	}

	public virtual RGD_StateMachine GetStateMachineData()
	{
		return new RGD_StateMachine(this);
	}

	public virtual void OnStateMachineRestored()
	{
	}
}
