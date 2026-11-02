using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class GlobalScavengeManagerEventsHub : ScavengeManager
{
	private EventHub<ScavengeManager, EventHubArgument<ScavengeManagerEvents>, ScavengeManagerEvents> _globalHub = new EventHub<ScavengeManager, EventHubArgument<ScavengeManagerEvents>, ScavengeManagerEvents>(default(ScavengeManagerEventsComparer));

	public static GlobalScavengeManagerEventsHub I { get; protected set; }

	public override EventHub<ScavengeManager, EventHubArgument<ScavengeManagerEvents>, ScavengeManagerEvents> EventHub => _globalHub;

	public void Subscribe(ScavengeManager actor)
	{
		actor.EventHub.Remove(_handler);
		actor.EventHub.Add(_handler);
	}

	private void _handler(ScavengeManager sender, EventHubArgument<ScavengeManagerEvents> args)
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
		GlobalEventHubResolver.I.RegisterEventHub((ScavengeManager)this);
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
