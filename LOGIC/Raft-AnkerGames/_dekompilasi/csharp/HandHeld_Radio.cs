using System.Collections;
using FMODUnity;
using UnityEngine;

public class HandHeld_Radio : MonoBehaviour
{
	public StudioEventEmitter eventEmitter_radioOn;

	public Network_Player player;

	public RadioStation radioStation;

	private Coroutine radioStop;

	public void Start()
	{
		player = ComponentManager<Network_Player>.Value;
		radioStation = Object.FindObjectOfType<RadioStation>();
	}

	public void OnSelect()
	{
		TurnOnRadio();
	}

	public void OnDeSelect()
	{
		TurnOffRadio();
	}

	public void TurnOnRadio()
	{
		if (radioStop != null)
		{
			eventEmitter_radioOn.Stop();
			player.StopCoroutine(radioStop);
		}
		eventEmitter_radioOn.Play();
	}

	public void TurnOffRadio()
	{
		eventEmitter_radioOn.EventInstance.keyOff();
		if (player != null)
		{
			radioStop = player.StartCoroutine(StopRadio(1f));
		}
	}

	private IEnumerator StopRadio(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		eventEmitter_radioOn.Stop();
	}
}
