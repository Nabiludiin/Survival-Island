using Knife.Tools;
using UnityEngine;

namespace Knife.DeferredDecals.Spawn;

public static class DecalReceiverHelper
{
	public static void SendPhysicsRaycastInfo(RaycastHit hitInfo)
	{
		IDecalReceiver decalReceiver = hitInfo.collider.GetComponent<IDecalReceiver>();
		if (decalReceiver == null)
		{
			decalReceiver = hitInfo.collider.GetComponentInParent<IDecalReceiver>();
		}
		decalReceiver?.HittedByPhysicsRaycast(hitInfo);
	}

	public static void SendParticleCollisionEvent(ParticleCollisionEvent collisionEvent)
	{
		IDecalReceiver decalReceiver = collisionEvent.colliderComponent.GetComponent<IDecalReceiver>();
		if (decalReceiver == null)
		{
			decalReceiver = collisionEvent.colliderComponent.GetComponentInParent<IDecalReceiver>();
		}
		decalReceiver?.HittedByParticle(collisionEvent);
	}

	public static void SendGPURaycastInfo(GPURaycastDecalsTargetInfo hitInfo)
	{
		IDecalReceiver decalReceiver = hitInfo.hittedRenderer.GetComponent<IDecalReceiver>();
		if (decalReceiver == null)
		{
			decalReceiver = hitInfo.hittedRenderer.GetComponentInParent<IDecalReceiver>();
		}
		decalReceiver?.HittedByGPURaycast(hitInfo);
	}

	public static void SendCollision(Collision collision)
	{
		IDecalReceiver decalReceiver = collision.collider.GetComponent<IDecalReceiver>();
		if (decalReceiver == null)
		{
			decalReceiver = collision.collider.GetComponentInParent<IDecalReceiver>();
		}
		decalReceiver?.HittedByPhysicsCollision(collision);
	}
}
