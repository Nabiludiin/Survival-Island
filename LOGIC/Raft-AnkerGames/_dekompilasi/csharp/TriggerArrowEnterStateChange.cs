using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArrowEnterStateChange : MonoBehaviour
{
	public List<AI_State> validSwitchStates;

	public AI_StateMachine statemachine;

	public TriggerBoxEnterAction triggerBoxEnter;

	public Action OnTriggered;

	private void Awake()
	{
		TriggerBoxEnterAction triggerBoxEnterAction = triggerBoxEnter;
		triggerBoxEnterAction.OnTriggerAction = (Action<Collider>)Delegate.Combine(triggerBoxEnterAction.OnTriggerAction, new Action<Collider>(ActiveArrowCheck));
	}

	private void OnDestroy()
	{
		TriggerBoxEnterAction triggerBoxEnterAction = triggerBoxEnter;
		triggerBoxEnterAction.OnTriggerAction = (Action<Collider>)Delegate.Remove(triggerBoxEnterAction.OnTriggerAction, new Action<Collider>(ActiveArrowCheck));
	}

	private void ActiveArrowCheck(Collider other)
	{
		if (validSwitchStates != null && validSwitchStates.Count > 0 && validSwitchStates.Contains(statemachine.currentState))
		{
			Arrow componentInParent = other.gameObject.GetComponentInParent<Arrow>();
			if (componentInParent != null && !componentInParent.IsExpended)
			{
				OnTriggered?.Invoke();
			}
		}
	}
}
