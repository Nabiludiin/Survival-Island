using FMODUnity;
using UnityEngine;

public class Placeable_Interactable_HonkHorn : Placeable_Interactable
{
	[SerializeField]
	private StudioEventEmitter emitter;

	protected override void OnInteract(bool restoring)
	{
		base.OnInteract(restoring);
		if (!restoring)
		{
			emitter.Play();
		}
	}
}
