using FMODUnity;
using UnityEngine;

public class PickupItem_Networked_DigPile : PickupItem_Networked
{
	private enum OnDigUpTrickOrTreatType
	{
		none,
		spawn
	}

	[SerializeField]
	private OnDigUpTrickOrTreatType trickOrTreatType = OnDigUpTrickOrTreatType.spawn;

	public GameObject spawnableObject;

	public EventReference er_spawnBug;

	public override void HandleOnPickedUp()
	{
		base.HandleOnPickedUp();
		OnDigUpTrickOrTreatType onDigUpTrickOrTreatType = trickOrTreatType;
		if (onDigUpTrickOrTreatType != OnDigUpTrickOrTreatType.none && onDigUpTrickOrTreatType == OnDigUpTrickOrTreatType.spawn)
		{
			Object.Instantiate(spawnableObject, transform.position, Quaternion.identity, null);
			RuntimeManager.PlayOneShot(er_spawnBug, transform.position);
		}
	}
}
