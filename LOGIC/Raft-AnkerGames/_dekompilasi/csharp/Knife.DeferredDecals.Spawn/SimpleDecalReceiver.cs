using Knife.Tools;
using UnityEngine;

namespace Knife.DeferredDecals.Spawn;

public class SimpleDecalReceiver : MonoBehaviour, IDecalReceiver
{
	public DecalSpawnController SpawnController;

	public bool ParentOnHit;

	public float Offset = 0.15f;

	[Range(0f, 1f)]
	public float RotationJitter;

	public void HittedByGPURaycast(GPURaycastDecalsTargetInfo hitInfo)
	{
		Decal decal = SpawnController.SpawnDecal();
		Transform decalTransform = decal.transform;
		ApplyDecalTransform(decalTransform, hitInfo);
		OnHitted(decal, hitInfo);
	}

	public void HittedByPhysicsRaycast(RaycastHit hitInfo)
	{
		Decal decal = SpawnController.SpawnDecal();
		Transform decalTransform = decal.transform;
		ApplyDecalTransform(decalTransform, hitInfo);
		OnHitted(decal, hitInfo);
	}

	public void HittedByParticle(ParticleCollisionEvent collisionEvent)
	{
		Decal decal = SpawnController.SpawnDecal();
		Transform decalTransform = decal.transform;
		ApplyDecalTransform(decalTransform, collisionEvent);
		OnHitted(decal, collisionEvent);
	}

	public void HittedByPhysicsCollision(Collision collision)
	{
		Decal decal = SpawnController.SpawnDecal();
		Transform decalTransform = decal.transform;
		ApplyDecalTransform(decalTransform, collision);
		OnHitted(decal, collision);
	}

	private void ApplyDecalTransform(Transform decalTransform, GPURaycastDecalsTargetInfo hitInfo)
	{
		decalTransform.position = hitInfo.position + hitInfo.normal * Offset;
		ApplyDecalRotationByNormal(decalTransform, hitInfo.normal);
		if (ParentOnHit)
		{
			decalTransform.SetParent(hitInfo.hittedRenderer.transform);
		}
	}

	private void ApplyDecalTransform(Transform decalTransform, RaycastHit hitInfo)
	{
		decalTransform.position = hitInfo.point + hitInfo.normal * Offset;
		ApplyDecalRotationByNormal(decalTransform, hitInfo.normal);
		if (ParentOnHit)
		{
			decalTransform.SetParent(hitInfo.transform);
		}
	}

	private void ApplyDecalTransform(Transform decalTransform, ParticleCollisionEvent collisionEvent)
	{
		decalTransform.position = collisionEvent.intersection + collisionEvent.normal * Offset;
		ApplyDecalRotationByNormal(decalTransform, collisionEvent.normal);
		if (ParentOnHit)
		{
			decalTransform.SetParent(collisionEvent.colliderComponent.transform);
		}
	}

	private void ApplyDecalTransform(Transform decalTransform, Collision collision)
	{
		ContactPoint contactPoint = collision.contacts[0];
		decalTransform.position = contactPoint.point + contactPoint.normal * Offset;
		ApplyDecalRotationByNormal(decalTransform, contactPoint.normal);
		if (ParentOnHit)
		{
			decalTransform.SetParent(collision.transform);
		}
	}

	private void ApplyDecalRotationByNormal(Transform decalTransform, Vector3 normal)
	{
		decalTransform.up = normal;
		decalTransform.rotation *= Quaternion.AngleAxis(Random.value * 360f * RotationJitter, Vector3.up);
	}

	protected virtual void OnHitted(Decal decalInstance, GPURaycastDecalsTargetInfo hitInfo)
	{
	}

	protected virtual void OnHitted(Decal decalInstance, RaycastHit hitInfo)
	{
	}

	protected virtual void OnHitted(Decal decalInstance, ParticleCollisionEvent collisionEvent)
	{
	}

	protected virtual void OnHitted(Decal decalInstance, Collision collision)
	{
	}
}
