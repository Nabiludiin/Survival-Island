using System;
using UnityEngine;

public class TriggerForcedRegionUpdate : MonoBehaviour
{
	[SerializeField]
	private TriggerBoxAction enterAction;

	[SerializeField]
	private TriggerBoxAction exitAction;

	private void Start()
	{
		TriggerBoxAction triggerBoxAction = enterAction;
		triggerBoxAction.OnTriggerAction = (Action<Collider>)Delegate.Remove(triggerBoxAction.OnTriggerAction, new Action<Collider>(ForceUpdate));
		TriggerBoxAction triggerBoxAction2 = exitAction;
		triggerBoxAction2.OnTriggerAction = (Action<Collider>)Delegate.Remove(triggerBoxAction2.OnTriggerAction, new Action<Collider>(ForceUpdate));
		TriggerBoxAction triggerBoxAction3 = enterAction;
		triggerBoxAction3.OnTriggerAction = (Action<Collider>)Delegate.Combine(triggerBoxAction3.OnTriggerAction, new Action<Collider>(ForceUpdate));
		TriggerBoxAction triggerBoxAction4 = exitAction;
		triggerBoxAction4.OnTriggerAction = (Action<Collider>)Delegate.Combine(triggerBoxAction4.OnTriggerAction, new Action<Collider>(ForceUpdate));
	}

	private void ForceUpdate(Collider collider)
	{
		RegionManager.ForceUpdate();
	}
}
