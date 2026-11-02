using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class GlobalActorEventsHub : ActorBrain
{
	private EventHub<ActorBrain, ActorEventsArgs, ActorEvents> _globalHub = new EventHub<ActorBrain, ActorEventsArgs, ActorEvents>(default(ActorEventsComparer));

	public static GlobalActorEventsHub I { get; protected set; }

	public override EventHub<ActorBrain, ActorEventsArgs, ActorEvents> EventHub => _globalHub;

	public void Subscribe(ActorBrain actor)
	{
		actor.EventHub.Remove(_handler);
		actor.EventHub.Add(_handler);
	}

	private void _handler(ActorBrain sender, ActorEventsArgs args)
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
		GlobalEventHubResolver.I.RegisterEventHub((ActorBrain)this);
		I = this;
	}

	protected override void Start()
	{
	}

	protected override void OnDisable()
	{
	}

	protected override void Update()
	{
	}

	public override void ResetActor()
	{
	}

	protected override void OnDestroy()
	{
		if (I == this)
		{
			I = null;
		}
		_globalHub.Clear();
		ActorEventsHub.Clear();
	}
}
