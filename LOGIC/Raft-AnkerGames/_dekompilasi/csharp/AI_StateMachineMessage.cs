using System;
using UnityEngine;

public class AI_StateMachineMessage : MonoBehaviour
{
	[Serializable]
	public class StateMachineMessage
	{
		public AIStateEvent eventType;

		public string message;
	}

	[SerializeField]
	private StateMachineMessage[] messages;

	private AI_StateMachine stateMachine;

	private AI_State state;

	private bool initialized;

	private void Awake()
	{
		state = GetComponent<AI_State>();
		stateMachine = GetComponentInParent<AI_StateMachine>();
		if (state != null && stateMachine != null)
		{
			AI_StateMachine aI_StateMachine = stateMachine;
			aI_StateMachine.OnEnterState = (Action<AI_State>)Delegate.Combine(aI_StateMachine.OnEnterState, new Action<AI_State>(OnEnterState));
			AI_StateMachine aI_StateMachine2 = stateMachine;
			aI_StateMachine2.OnExitState = (Action<AI_State>)Delegate.Combine(aI_StateMachine2.OnExitState, new Action<AI_State>(OnExitState));
			initialized = true;
		}
	}

	private void OnEnterState(AI_State newState)
	{
		if (newState == state)
		{
			SendMessages(AIStateEvent.EnterState);
		}
	}

	private void OnExitState(AI_State newState)
	{
		if (newState == state)
		{
			SendMessages(AIStateEvent.ExitState);
		}
	}

	private void SendMessages(AIStateEvent eventType)
	{
		if (stateMachine == null || !messages.ContainsItems())
		{
			return;
		}
		for (int i = 0; i < messages.Length; i++)
		{
			StateMachineMessage stateMachineMessage = messages[i];
			if (stateMachineMessage != null && stateMachineMessage.eventType == eventType)
			{
				stateMachine.OnStateMachineMessage(stateMachineMessage.eventType, stateMachineMessage.message);
			}
		}
	}
}
