using FMODUnity;
using UnityEngine;

public class PlaySoundWithInterval : MonoBehaviour
{
	public float soundInterval = 0.5f;

	[SerializeField]
	public StudioEventEmitter plingSoundEmitter;

	private float soundIntervalProgress;

	public void Update()
	{
		soundIntervalProgress += Time.deltaTime;
		if (soundIntervalProgress > soundInterval)
		{
			TriggerSound();
		}
	}

	private void TriggerSound()
	{
		plingSoundEmitter.PlaySafe();
	}
}
