using UnityEngine;

public class Throwable_Stone : Throwable_Object
{
	[SerializeField]
	private float damage;

	[SerializeField]
	private ParticleSystem particlesBreak;

	protected override void OnCollisionEvent(Collision collision, Network_Entity entity)
	{
		base.OnCollisionEvent(collision, entity);
		if (throwableComponent.playerNetwork.IsLocalPlayer && entity != null)
		{
			SimpleMonoBehaviourSingleton<HapticManager>.Instance.PlayOneShot(HapticEvent.DamageEnemy);
			ComponentManager<Network_Host>.Value.DamageEntity(entity, collision.transform, damage, collision.contacts[0].point, collision.contacts[0].normal, EntityType.Player);
		}
	}

	public override void HandleCollision(Vector3 collisionPosition, Vector3 normal, ParentType parentType, Network_Entity entity)
	{
		base.HandleCollision(collisionPosition, normal, parentType, entity);
		ParticleSystem particleSystem = Object.Instantiate(particlesBreak, collisionPosition, Quaternion.LookRotation(normal), null);
		Helper.SetParentFromParentType(particleSystem.transform, parentType);
		Object.Destroy(particleSystem.gameObject, 10f);
		Object.Destroy(gameObject);
	}
}
