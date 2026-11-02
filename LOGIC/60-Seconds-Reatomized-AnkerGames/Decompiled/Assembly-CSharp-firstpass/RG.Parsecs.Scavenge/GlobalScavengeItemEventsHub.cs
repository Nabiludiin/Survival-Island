using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class GlobalScavengeItemEventsHub : ScavengeItem
{
	private EventHub<ScavengeItem, EventHubArgument<ScavengeItemEvents>, ScavengeItemEvents> _globalHub = new EventHub<ScavengeItem, EventHubArgument<ScavengeItemEvents>, ScavengeItemEvents>(default(ScavengeItemEventsComparer));

	public static GlobalScavengeItemEventsHub I { get; protected set; }

	public override EventHub<ScavengeItem, EventHubArgument<ScavengeItemEvents>, ScavengeItemEvents> EventHub => _globalHub;

	public void Subscribe(ScavengeItem actor)
	{
		actor.EventHub.Remove(_handler);
		actor.EventHub.Add(_handler);
	}

	private void _handler(ScavengeItem sender, EventHubArgument<ScavengeItemEvents> args)
	{
		_globalHub.Invoke(sender, args);
	}

	protected override void Awake()
	{
		if (I != null)
		{
			Debug.LogError("More than one instance of GlobalActorEventsHub in the scene");
			return;
		}
		GlobalEventHubResolver.I.RegisterEventHub((ScavengeItem)this);
		I = this;
	}

	protected void OnDestroy()
	{
		if (I == this)
		{
			I = null;
		}
		_globalHub.Clear();
		_eventsHub.Clear();
	}
}
