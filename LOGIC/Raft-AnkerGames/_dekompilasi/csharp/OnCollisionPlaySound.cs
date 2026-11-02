using System.Collections;
using FMODUnity;
using UnityEngine;

public class OnCollisionPlaySound : MonoBehaviour
{
	public bool active = true;

	public EventReference er_sound;

	public bool cooldown;

	[SerializeField]
	private float cooldownTime = 1f;

	private bool canPlaySound = true;

	private void OnCollisionEnter(Collision collision)
	{
		if (active && canPlaySound)
		{
			RuntimeManager.PlayOneShot(er_sound, transform.position);
			if (cooldown)
			{
				canPlaySound = false;
				StartCoroutine(ResetCooldown());
			}
		}
	}

	private IEnumerator ResetCooldown()
	{
		yield return new WaitForSeconds(cooldownTime);
		canPlaySound = true;
	}
}
