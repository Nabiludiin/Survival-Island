using System.Collections;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Scavenge;

public class ScavengeClock : MonoBehaviour, IHaveEventHub<ScavengeClock, EventHubArgument<ClockEvents>, ClockEvents>, IActorBrain
{
	protected EventHub<ScavengeClock, EventHubArgument<ClockEvents>, ClockEvents> _eventsHub = new DictionaryEventHub<ScavengeClock, EventHubArgument<ClockEvents>, ClockEvents>(default(ClockEventsComparer));

	private int secondsLeft;

	public RectTransform handTransform;

	public Image redIndicator;

	public Transform _positionMarker;

	public EventHub<ScavengeClock, EventHubArgument<ClockEvents>, ClockEvents> EventHub => _eventsHub;

	public int currentTime => secondsLeft;

	public Transform Transform => transform;

	public GameObject GameObject => gameObject;

	public string Name => gameObject.name;

	public Transform PositionMarker => _positionMarker;

	public Player Player => null;

	public void Hide()
	{
		gameObject.SetActive(value: false);
	}

	public IEnumerator StartCount(int startPosition, int seconds, bool withRedIndicator)
	{
		secondsLeft = seconds;
		redIndicator.fillAmount = 0f;
		handTransform.eulerAngles = new Vector3(0f, 0f, startPosition * -6);
		gameObject.SetActive(value: true);
		bool tickA = true;
		StopAllCoroutines();
		while (secondsLeft > 0)
		{
			secondsLeft--;
			handTransform.Rotate(new Vector3(0f, 0f, -6f));
			if (withRedIndicator)
			{
				redIndicator.fillAmount += 1f / 60f;
			}
			_eventsHub.Invoke(this, new EventHubArgument<ClockEvents>(ClockEvents.Tick));
			if (startPosition < 0)
			{
				if (tickA)
				{
					_eventsHub.Invoke(this, new EventHubArgument<ClockEvents>(ClockEvents.TickAExploration));
				}
				else
				{
					_eventsHub.Invoke(this, new EventHubArgument<ClockEvents>(ClockEvents.TickBExploration));
				}
				tickA = !tickA;
			}
			yield return new WaitForSeconds(1f);
		}
	}
}
