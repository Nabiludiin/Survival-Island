using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
	public EventReference field_collide;

	public ParticleSystem particleSystem;

	private void OnParticleCollision(GameObject other)
	{
		if (particleSystem == null)
		{
			Debug.Log("Particle system is not assigned", gameObject);
			return;
		}
		List<ParticleCollisionEvent> list = new List<ParticleCollisionEvent>();
		particleSystem.GetCollisionEvents(other, list);
		foreach (ParticleCollisionEvent item in list)
		{
			Vector3 intersection = item.intersection;
			RuntimeManager.PlayOneShot(field_collide, intersection);
		}
	}
}
