using System.Collections.Generic;
using UnityEngine;

namespace Knife.DeferredDecals.Spawn;

[RequireComponent(typeof(ParticleSystem))]
public class SpawnDecalOnParticle : MonoBehaviour
{
	public SpawnControl SpawnControl;

	public DecalSpawnController SpawnController;

	public float Offset = 0.1f;

	public bool ParentOnCollide;

	[Range(0f, 1f)]
	public float RotationJitter;

	private ParticleSystem attachedParticleSystem;

	private List<ParticleCollisionEvent> collisions;

	private void Awake()
	{
		attachedParticleSystem = GetComponent<ParticleSystem>();
		collisions = new List<ParticleCollisionEvent>();
	}

	private void OnParticleCollision(GameObject other)
	{
		collisions.Clear();
		int collisionEvents = attachedParticleSystem.GetCollisionEvents(other, collisions);
		if (SpawnControl == SpawnControl.ByReceiver)
		{
			for (int i = 0; i < collisionEvents; i++)
			{
				DecalReceiverHelper.SendParticleCollisionEvent(collisions[i]);
			}
		}
		else
		{
			if (SpawnControl != SpawnControl.BySpawnController)
			{
				return;
			}
			for (int j = 0; j < collisionEvents; j++)
			{
				Vector3 intersection = collisions[j].intersection;
				Vector3 normal = collisions[j].normal;
				Decal decal = SpawnController.SpawnDecal();
				decal.transform.position = intersection + normal * Offset;
				decal.transform.up = normal;
				decal.transform.rotation *= Quaternion.AngleAxis(Random.value * 360f * RotationJitter, Vector3.up);
				if (ParentOnCollide)
				{
					decal.transform.SetParent(collisions[j].colliderComponent.transform);
				}
			}
		}
	}
}
