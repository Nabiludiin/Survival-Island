using System.Collections;
using RG.Parsecs.Common;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Scavenge;

public class HandUI : MonoBehaviour, IHaveEventHub<HandUI, EventHubArgument<HandUIEvents>, HandUIEvents>, IActorBrain
{
	protected EventHub<HandUI, EventHubArgument<HandUIEvents>, HandUIEvents> _eventsHub = new DictionaryEventHub<HandUI, EventHubArgument<HandUIEvents>, HandUIEvents>(default(HandUIEventsComparer));

	[SerializeField]
	private Image _img;

	private WaitForSeconds _delayPerWeight;

	private WaitForSeconds _pickUpDelay;

	public Transform _positionMarker;

	public EventHub<HandUI, EventHubArgument<HandUIEvents>, HandUIEvents> EventHub => _eventsHub;

	public Transform Transform => transform;

	public GameObject GameObject => gameObject;

	public string Name => gameObject.name;

	public Transform PositionMarker => _positionMarker;

	public Player Player => null;

	public void Init(float delayPerWeight, float pickupDelay)
	{
		_delayPerWeight = new WaitForSeconds(delayPerWeight);
		_pickUpDelay = new WaitForSeconds(pickupDelay);
	}

	public void SetHands(ScavangeItemData itemData, int index, Color32 color)
	{
		StartCoroutine(DelayedSetHands(itemData, index, color));
	}

	private IEnumerator DelayedSetHands(ScavangeItemData itemData, int index, Color32 color)
	{
		for (int i = 0; i < index; i++)
		{
			yield return _delayPerWeight;
		}
		_eventsHub.Invoke(this, new EventHubArgument<HandUIEvents>(HandUIEvents.AboutToSetHands));
		yield return _pickUpDelay;
		_eventsHub.Invoke(this, new EventHubArgument<HandUIEvents>(HandUIEvents.SetHands));
		_img.sprite = itemData.handSprite;
		Sprite sprite = ContentManager.GetSprite(itemData.handSprite.name);
		if (sprite != null)
		{
			_img.sprite = sprite;
		}
		_img.color = color;
	}

	public void ClearHands(Sprite hand)
	{
		_eventsHub.Invoke(this, new EventHubArgument<HandUIEvents>(HandUIEvents.ClearHands));
		_img.sprite = hand;
		_img.color = Color.white;
	}

	public void AboutToClearHands(Sprite hand)
	{
		_eventsHub.Invoke(this, new EventHubArgument<HandUIEvents>(HandUIEvents.AboutToClearHands));
	}

	public void Show(int index)
	{
		StartCoroutine(DelayedShow(index));
	}

	public void NotEnoughHands()
	{
		_eventsHub.Invoke(this, new EventHubArgument<HandUIEvents>(HandUIEvents.PickUpFail_NotEnoughHands));
	}

	public void NoItemAvailable()
	{
		_eventsHub.Invoke(this, new EventHubArgument<HandUIEvents>(HandUIEvents.PickUpFail_NoItemAvailable));
	}

	private IEnumerator DelayedShow(int index)
	{
		for (int i = 0; i < index; i++)
		{
			yield return _delayPerWeight;
		}
		_eventsHub.Invoke(this, new EventHubArgument<HandUIEvents>(HandUIEvents.Show));
		_img.enabled = true;
	}

	public void Hide()
	{
		_img.enabled = false;
	}
}
