using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

public class Bobber : MonoBehaviour
{
	public Action OnFishGrabBait;

	public Action OnFishEscape;

	[SerializeField]
	private Interval_Float waitTime;

	[SerializeField]
	private Interval_Float fishFightTime;

	[SerializeField]
	private RandomDropper randomDropper;

	[SerializeField]
	private WaterFloatSemih2 waterFloat;

	[SerializeField]
	private StudioEventEmitter eventEmitter_FishOnHook;

	[SerializeField]
	private StudioEventEmitter eventEmitter_FishOnHookPole;

	private Throwable rodThrowable;

	private bool active;

	private bool fishIsOnHook;

	private bool fishEscaped;

	public bool FishIsOnHook => fishIsOnHook;

	private void Awake()
	{
		rodThrowable = transform.GetComponentInParent<Throwable>();
	}

	private void OnValidate()
	{
		if (waterFloat == null)
		{
			waterFloat = GetComponentInParent<WaterFloatSemih2>();
		}
	}

	public void StartBobbing()
	{
		if (!active)
		{
			rodThrowable.throwableObjectBody.isKinematic = true;
			active = true;
			fishEscaped = false;
			StartCoroutine(FishOnHook(waitTime.GetRandomValue()));
			eventEmitter_FishOnHook.Stop();
			eventEmitter_FishOnHookPole.Stop();
		}
	}

	public void StopBobbing()
	{
		active = false;
		fishIsOnHook = false;
		StopAllCoroutines();
		if (!fishEscaped)
		{
			if (eventEmitter_FishOnHook != null)
			{
				eventEmitter_FishOnHook.Stop();
			}
			if (eventEmitter_FishOnHookPole != null)
			{
				eventEmitter_FishOnHookPole.Stop();
			}
		}
	}

	private IEnumerator FishOnHook(float timeDelay)
	{
		yield return new WaitForSeconds(timeDelay);
		SimpleMonoBehaviourSingleton<HapticManager>.Instance?.PlayOneShot(HapticEvent.FishOnHook);
		waterFloat.setToSurface = false;
		fishIsOnHook = true;
		StartCoroutine(Flee());
		ParticleManager.PlaySystem("WaterSplash_Hook", transform.position + Vector3.up * 0.1f);
		eventEmitter_FishOnHook.Play();
		eventEmitter_FishOnHookPole.Play();
		StartCoroutine(FishEscaped(fishFightTime.GetRandomValue()));
		if (OnFishGrabBait != null)
		{
			OnFishGrabBait();
		}
	}

	private IEnumerator FishEscaped(float timeDelay)
	{
		yield return new WaitForSeconds(timeDelay);
		active = false;
		fishEscaped = true;
		eventEmitter_FishOnHook.EventInstance.keyOff();
		eventEmitter_FishOnHookPole.EventInstance.keyOff();
		if (OnFishEscape != null)
		{
			OnFishEscape();
		}
	}

	private IEnumerator Flee()
	{
		Vector3 fleeDir = UnityEngine.Random.insideUnitSphere;
		fleeDir.y = -1f * UnityEngine.Random.Range(1f, 2f);
		fleeDir.x *= UnityEngine.Random.Range(1f, 3f);
		fleeDir.z *= UnityEngine.Random.Range(1f, 3f);
		Vector3 targetDir = fleeDir;
		float timer = UnityEngine.Random.value;
		while (fishIsOnHook)
		{
			timer -= Time.deltaTime;
			if (timer <= 0f)
			{
				timer = UnityEngine.Random.value;
				targetDir = UnityEngine.Random.insideUnitSphere;
				targetDir.y = -1f * UnityEngine.Random.Range(1f, 2f);
				targetDir.x *= UnityEngine.Random.Range(1f, 3f);
				targetDir.z *= UnityEngine.Random.Range(1f, 3f);
			}
			fleeDir = Vector3.Lerp(fleeDir, targetDir, Time.deltaTime * 3f);
			transform.position += fleeDir * Time.deltaTime;
			yield return null;
		}
	}
}
