using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class SoundPlayer_IntegerConnection : MonoBehaviour
{
	[SerializeField]
	[EventRef]
	private List<string> soundsPlacement;

	[SerializeField]
	[EventRef]
	private List<string> soundsRetrieval;

	public void PlaySoundPlacement(int index, Vector3 position)
	{
		RuntimeManager.PlayOneShot(soundsPlacement[index], position);
	}

	public void PlaySoundRetrieval(int index, Vector3 position)
	{
		RuntimeManager.PlayOneShot(soundsRetrieval[index], position);
	}
}
