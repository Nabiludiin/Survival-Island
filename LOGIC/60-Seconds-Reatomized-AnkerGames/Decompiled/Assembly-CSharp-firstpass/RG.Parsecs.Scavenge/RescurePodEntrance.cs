using Rewired;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class RescurePodEntrance : MonoBehaviour, IHaveEventHub<RescurePodEntrance, EventHubArgument<RescurePodEntranceEvents>, RescurePodEntranceEvents>, IActorBrain
{
	protected EventHub<RescurePodEntrance, EventHubArgument<RescurePodEntranceEvents>, RescurePodEntranceEvents> _eventsHub = new DictionaryEventHub<RescurePodEntrance, EventHubArgument<RescurePodEntranceEvents>, RescurePodEntranceEvents>(default(RescurePodEntranceEventsComparer));

	public Transform _positionMarker;

	public EventHub<RescurePodEntrance, EventHubArgument<RescurePodEntranceEvents>, RescurePodEntranceEvents> EventHub => _eventsHub;

	public Transform Transform => transform;

	public GameObject GameObject => gameObject;

	public string Name => gameObject.name;

	public Transform PositionMarker => _positionMarker;

	public Player Player => null;

	private void OnTriggerEnter(Collider player)
	{
		player.GetComponent<PlayerController>().RescuePodEnter();
		SendEvent(RescurePodEntranceEvents.Actor_OnTriggerEnter);
	}

	private void OnTriggerExit(Collider player)
	{
		player.GetComponent<PlayerController>().RescuePodExit();
		SendEvent(RescurePodEntranceEvents.Actor_OnTriggerExit);
	}

	internal void SendEvent(RescurePodEntranceEvents e)
	{
		_eventsHub.Invoke(this, new EventHubArgument<RescurePodEntranceEvents>(e));
	}
}
