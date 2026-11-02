using FMODUnity;
using UnityEngine;

public class ParticleCollisionReciever : MonoBehaviour
{
	public ParticleSystem outParticleSystem;

	public float delay = 0.5f;

	public EventReference er_emit;

	private void OnDrawGizmos()
	{
		if (outParticleSystem != null)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine(transform.position, outParticleSystem.transform.position);
			Gizmos.DrawSphere(outParticleSystem.transform.position, 0.05f);
		}
	}

	public virtual void OnParticleCollision(GameObject other)
	{
		if (outParticleSystem != null)
		{
			Invoke("EmitParticleFromOutSystem", delay);
		}
	}

	private void EmitParticleFromOutSystem()
	{
		outParticleSystem.Emit(1);
		RuntimeManager.PlayOneShot(er_emit, outParticleSystem.transform.position);
	}
}
