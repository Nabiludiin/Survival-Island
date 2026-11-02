using System;
using UnityEngine;

public class OlofDuckArrow : MonoBehaviour
{
	[SerializeField]
	private TriggerBoxEnterAction triggerBoxEnter;

	[SerializeField]
	private Animator animator;

	private void Awake()
	{
		TriggerBoxEnterAction triggerBoxEnterAction = triggerBoxEnter;
		triggerBoxEnterAction.OnTriggerAction = (Action<Collider>)Delegate.Combine(triggerBoxEnterAction.OnTriggerAction, new Action<Collider>(OnTriggerAction));
	}

	private void OnTriggerAction(Collider collider)
	{
		animator.SetTrigger("Crouch");
	}
}
