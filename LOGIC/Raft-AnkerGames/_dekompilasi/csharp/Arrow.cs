using FMODUnity;
using UnityEngine;

public class Arrow : Throwable_Object
{
	[SerializeField]
	private float damage = 1f;

	[SerializeField]
	private TrailRenderer trailRenderer;

	public EventReference er_hitMarker;

	public EventReference er_hitInvurnerable;

	[SerializeField]
	private bool isExpended;

	private int maxInvurnabilityHits = 8;

	private int invurnabilityHits;

	public bool IsExpended => isExpended;

	protected override void OnCollisionEvent(Collision collision, Network_Entity entity)
	{
		if (!isExpended)
		{
			base.OnCollisionEvent(collision, entity);
			if (throwableComponent.playerNetwork.IsLocalPlayer && entity != null && !entity.IsInvurnerable)
			{
				SimpleMonoBehaviourSingleton<HapticManager>.Instance.PlayOneShot(HapticEvent.DamageEnemy);
				ComponentManager<Network_Host>.Value.DamageEntity(entity, collision.transform, damage, collision.contacts[0].point, collision.contacts[0].normal, EntityType.Player);
				isExpended = true;
			}
		}
	}

	public override void HandleCollision(Vector3 collisionPosition, Vector3 normal, ParentType parentType, Network_Entity entity)
	{
		if (entity != null)
		{
			if (!entity.IsInvurnerable)
			{
				base.HandleCollision(collisionPosition, normal, parentType, entity);
			}
			else
			{
				invurnabilityHits++;
				if (invurnabilityHits >= maxInvurnabilityHits)
				{
					base.HandleCollision(collisionPosition, normal, parentType, entity);
				}
			}
		}
		else
		{
			base.HandleCollision(collisionPosition, normal, parentType, entity);
		}
		if (entity != null)
		{
			if (throwableComponent.playerNetwork.IsLocalPlayer)
			{
				if (entity.IsInvurnerable)
				{
					RuntimeManager.PlayOneShot(er_hitInvurnerable);
				}
				else
				{
					RuntimeManager.PlayOneShot(er_hitMarker);
				}
			}
		}
		else
		{
			ParticleManager.PlaySystem("Particle_ProjectileHit", transform.position, Quaternion.LookRotation(normal));
		}
	}

	protected override void Update()
	{
		base.Update();
		if (trailRenderer != null)
		{
			if (!hitSomething)
			{
				trailRenderer.enabled = !underWater;
			}
			else if (trailRenderer.enabled)
			{
				trailRenderer.enabled = false;
			}
		}
	}
}
