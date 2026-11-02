using UnityEngine;

public class AI_StateMachine_BugSwarm : AI_StateMachine_Animal
{
	public ParticleController particleController;

	protected override void OnDeath(EntityType damageType)
	{
		base.OnDeath(damageType);
		particleController.particleParent.transform.SetParentSafe(null);
		particleController.PlayParticles();
		Object.Destroy(particleController.particleParent.gameObject, 2f);
	}
}
