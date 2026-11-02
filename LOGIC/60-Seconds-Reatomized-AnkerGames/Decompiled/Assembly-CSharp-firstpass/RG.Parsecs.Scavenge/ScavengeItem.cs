using System.Collections;
using FMODUnity;
using Rewired;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ScavengeItem : MonoBehaviour, IHaveEventHub<ScavengeItem, EventHubArgument<ScavengeItemEvents>, ScavengeItemEvents>, IActorBrain
{
	protected EventHub<ScavengeItem, EventHubArgument<ScavengeItemEvents>, ScavengeItemEvents> _eventsHub = new DictionaryEventHub<ScavengeItem, EventHubArgument<ScavengeItemEvents>, ScavengeItemEvents>(default(ScavengeItemEventsComparer));

	private ScavangeItemData _scavangeItemData;

	public SpriteRenderer outline;

	public string PickUpSoundPath;

	private bool _canBeCollected = true;

	public Transform _positionMarker;

	public virtual EventHub<ScavengeItem, EventHubArgument<ScavengeItemEvents>, ScavengeItemEvents> EventHub => _eventsHub;

	public ScavangeItemData ScavengeItemData
	{
		get
		{
			return _scavangeItemData;
		}
		set
		{
			_scavangeItemData = value;
		}
	}

	public Transform Transform => transform;

	public GameObject GameObject => gameObject;

	public string Name => gameObject.name;

	public Transform PositionMarker => _positionMarker;

	public bool CanBeCollected
	{
		get
		{
			return _canBeCollected;
		}
		set
		{
			_canBeCollected = value;
			if (_canBeCollected)
			{
				outline.enabled = true;
				return;
			}
			outline.enabled = false;
			DeHighLight();
		}
	}

	public Player Player => null;

	protected virtual void Awake()
	{
		GlobalScavengeItemEventsHub.I.Subscribe(this);
	}

	internal void SendEvent(ScavengeItemEvents e)
	{
		_eventsHub.Invoke(this, new EventHubArgument<ScavengeItemEvents>(e));
	}

	public void Highlight(int freeHands)
	{
		if (!(outline == null))
		{
			int weight = _scavangeItemData.Weight;
			if (freeHands < weight)
			{
				outline.color = Color.red;
			}
			else
			{
				outline.color = Color.green;
			}
		}
	}

	public void DeHighLight()
	{
		if (!(outline == null))
		{
			outline.color = Color.white;
		}
	}

	internal void Collect(float pickUpDelay)
	{
		StartCoroutine(DelayedCollect(pickUpDelay));
	}

	public void Restore()
	{
		gameObject.SetActive(value: true);
	}

	private IEnumerator DelayedCollect(float pickUpDelay)
	{
		SendEvent(ScavengeItemEvents.AboutToPickUp);
		if (!string.IsNullOrEmpty(PickUpSoundPath))
		{
			RuntimeManager.PlayOneShot(PickUpSoundPath);
		}
		yield return pickUpDelay;
		SendEvent(ScavengeItemEvents.PickUpSuccess);
		gameObject.SetActive(value: false);
	}
}
