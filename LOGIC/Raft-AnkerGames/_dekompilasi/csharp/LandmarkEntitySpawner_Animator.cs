using FMODUnity;
using UnityEngine;

public class LandmarkEntitySpawner_Animator : LandmarkEntitySpawner
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private AnimatorPropertySetting_Legit animProperties;

	public EventReference er_openSpawner;

	public EventReference er_closeSpawner;

	[SerializeField]
	private float durationTillClose = 2.1f;

	private Coroutine animLerpRoutine;

	public override void OnEntityConnectedToSpawner()
	{
		base.OnEntityConnectedToSpawner();
		if (animator != null)
		{
			animProperties.ApplyDataToAnimator(animator, animLerpRoutine);
			if (!er_openSpawner.IsNull)
			{
				RuntimeManager.PlayOneShot(er_openSpawner, transform.position);
			}
			if (!er_closeSpawner.IsNull)
			{
				Invoke("PlayCloseSound", durationTillClose);
			}
		}
	}

	private void PlayCloseSound()
	{
		RuntimeManager.PlayOneShot(er_closeSpawner, transform.position);
	}
}
