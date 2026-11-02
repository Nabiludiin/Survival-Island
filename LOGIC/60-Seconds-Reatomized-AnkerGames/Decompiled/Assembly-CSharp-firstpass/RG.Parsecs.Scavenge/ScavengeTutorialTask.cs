using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[Serializable]
public abstract class ScavengeTutorialTask : MonoBehaviour, IHaveEventHub<ScavengeTutorialTask, EventHubArgument<TutorialTaskEvents>, TutorialTaskEvents>, IActorBrain
{
	public Transform _positionMarker;

	protected EventHub<ScavengeTutorialTask, EventHubArgument<TutorialTaskEvents>, TutorialTaskEvents> _eventsHub = new DictionaryEventHub<ScavengeTutorialTask, EventHubArgument<TutorialTaskEvents>, TutorialTaskEvents>(default(TutorialTaskEventsComparer));

	public GenericEvent OnSuccess = new GenericEvent();

	public List<ScavengeTutorialHint> Hints;

	protected HintAnnouncer _hintAnnouncer;

	private float _counter;

	private int _hintIndex;

	public virtual EventHub<ScavengeTutorialTask, EventHubArgument<TutorialTaskEvents>, TutorialTaskEvents> EventHub => _eventsHub;

	public Transform Transform => transform;

	public GameObject GameObject => gameObject;

	public string Name => gameObject.name;

	public Transform PositionMarker => _positionMarker;

	public Player Player => null;

	protected void InvokeSuccess()
	{
		OnSuccess.Invoke();
		_eventsHub.Invoke(this, new EventHubArgument<TutorialTaskEvents>(TutorialTaskEvents.Finished));
	}

	public virtual void TaskStart(ScavengeManager manager, HintAnnouncer hintAnnouncer)
	{
		_hintAnnouncer = hintAnnouncer;
		_counter = 0f;
		_hintIndex = 0;
		_eventsHub.Invoke(this, new EventHubArgument<TutorialTaskEvents>(TutorialTaskEvents.Started));
	}

	public virtual void TaskUpdate()
	{
		if (_hintIndex < Hints.Count)
		{
			_counter += Time.deltaTime;
			ScavengeTutorialHint scavengeTutorialHint = Hints[_hintIndex];
			if (_counter > scavengeTutorialHint.Time)
			{
				_hintAnnouncer.Announce(scavengeTutorialHint);
				_hintIndex++;
				_counter -= scavengeTutorialHint.Time;
			}
		}
	}
}
