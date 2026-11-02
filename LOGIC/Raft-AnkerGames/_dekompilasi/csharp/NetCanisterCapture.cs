using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NetCanisterCapture : MonoBehaviour
{
	public LayerMask capturableLayer;

	public GameObject smokeParticleEffect;

	public EventReference er_captureAnimal;

	private bool isNetValid = true;

	private void OnCollisionEnter(Collision other)
	{
		if (!isNetValid)
		{
			return;
		}
		if (Helper.ObjectIsOnLayer(other.gameObject, capturableLayer))
		{
			CaptureAnimal captureAnimal = CheckIfObjectIsCapturableAnimal(other.transform);
			if (captureAnimal.IsNotNull() && captureAnimal.IsCapturable)
			{
				captureAnimal.IsCaptured = true;
				OnCaptureTarget();
			}
		}
		else
		{
			Invoke("DeactiveateCapturablility", 0.5f);
		}
	}

	private void DeactiveateCapturablility()
	{
		isNetValid = false;
	}

	private void OnCaptureTarget()
	{
		Object.Destroy(transform.gameObject);
		RuntimeManager.PlayOneShot(er_captureAnimal, transform.position);
		DisplayParticleEffect();
	}

	private void DisplayParticleEffect()
	{
		if (smokeParticleEffect != null)
		{
			ParticleManager.PlaySystem(smokeParticleEffect, transform.position, Quaternion.identity, null);
		}
	}

	private CaptureAnimal CheckIfObjectIsCapturableAnimal(Transform p_other)
	{
		CaptureAnimal componentInParent = p_other.transform.GetComponentInParent<CaptureAnimal>();
		if (componentInParent != null)
		{
			return componentInParent;
		}
		return null;
	}
}
