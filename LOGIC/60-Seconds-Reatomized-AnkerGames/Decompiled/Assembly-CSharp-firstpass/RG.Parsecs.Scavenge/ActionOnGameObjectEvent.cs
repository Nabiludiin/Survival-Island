using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ActionOnGameObjectEvent : MonoBehaviour
{
	public List<EventAction> Actions;

	public GameObjectEvents Event;

	protected bool _hasStarted;

	protected void OnEvent(GameObject actor, GameObjectEventArgs eargs)
	{
		if (Event != eargs.Event)
		{
			return;
		}
		for (int i = 0; i < Actions.Count; i++)
		{
			EventAction eventAction = Actions[i];
			if (eventAction == null)
			{
				Debug.LogErrorFormat("Action is null in ActionOnGameObjectEvent: {0}.", gameObject.name);
			}
			else
			{
				eventAction.Invoke(new EventActionArgs
				{
					Object = actor.gameObject
				});
			}
		}
	}

	protected void Awake()
	{
		OnEvent(gameObject, new GameObjectEventArgs(GameObjectEvents.Awaken));
	}

	protected void Start()
	{
		OnEvent(gameObject, new GameObjectEventArgs(GameObjectEvents.Started));
		_hasStarted = true;
		OnStartAndEnable();
	}

	protected void OnEnable()
	{
		OnEvent(gameObject, new GameObjectEventArgs(GameObjectEvents.Enabled));
		if (_hasStarted)
		{
			OnStartAndEnable();
		}
	}

	protected void OnStartAndEnable()
	{
		OnEvent(gameObject, new GameObjectEventArgs(GameObjectEvents.StartedOrEnabled));
	}

	protected void OnDisable()
	{
		OnEvent(gameObject, new GameObjectEventArgs(GameObjectEvents.Disabled));
	}

	protected void OnDestroy()
	{
		OnEvent(gameObject, new GameObjectEventArgs(GameObjectEvents.Destroyed));
	}
}
