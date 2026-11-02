using System;
using UnityEngine;

public class QuestEventColliderTrigger : QuestEventBase
{
	public TriggerBoxAction triggerBoxEnterAction;

	public TriggerBoxAction triggerBoxExitAction;

	[SerializeField]
	private bool onlyTriggerOnce = true;

	private bool hasBeenTriggered;

	private void Awake()
	{
		if (Raft_Network.IsHost)
		{
			if (triggerBoxEnterAction != null)
			{
				TriggerBoxAction triggerBoxAction = triggerBoxEnterAction;
				triggerBoxAction.OnTriggerAction = (Action<Collider>)Delegate.Combine(triggerBoxAction.OnTriggerAction, new Action<Collider>(SendQuestEventTriggerViaCollider));
			}
			if (triggerBoxExitAction != null)
			{
				TriggerBoxAction triggerBoxAction2 = triggerBoxExitAction;
				triggerBoxAction2.OnTriggerAction = (Action<Collider>)Delegate.Combine(triggerBoxAction2.OnTriggerAction, new Action<Collider>(SendQuestEventTriggerViaCollider));
			}
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Raft_Network.IsHost)
		{
			if (triggerBoxEnterAction != null)
			{
				TriggerBoxAction triggerBoxAction = triggerBoxEnterAction;
				triggerBoxAction.OnTriggerAction = (Action<Collider>)Delegate.Remove(triggerBoxAction.OnTriggerAction, new Action<Collider>(SendQuestEventTriggerViaCollider));
			}
			if (triggerBoxExitAction != null)
			{
				TriggerBoxAction triggerBoxAction2 = triggerBoxExitAction;
				triggerBoxAction2.OnTriggerAction = (Action<Collider>)Delegate.Remove(triggerBoxAction2.OnTriggerAction, new Action<Collider>(SendQuestEventTriggerViaCollider));
			}
		}
	}

	private void SendQuestEventTriggerViaCollider(Collider colli)
	{
		if (!onlyTriggerOnce || !hasBeenTriggered)
		{
			hasBeenTriggered = true;
			SendOnInteractNetwork(colli.GetComponent<Network_Player>());
		}
	}

	public override bool CanPlayerInteract(Network_Player player, bool overwriteInventoryValidation = false)
	{
		return CanStillBeInteractedWith;
	}
}
