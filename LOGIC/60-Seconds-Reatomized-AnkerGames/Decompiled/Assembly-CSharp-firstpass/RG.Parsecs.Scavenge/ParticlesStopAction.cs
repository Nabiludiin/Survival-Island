using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ParticlesStopAction : EventAction
{
	public ParticleSystem ParticleSystem;

	public ParticleSystem[] ParticleSystems;

	public bool Clear;

	protected override void Act(EventActionArgs args = null)
	{
		if (ParticleSystem != null)
		{
			ParticleSystem.Stop(withChildren: true);
		}
		if (Clear && ParticleSystem != null)
		{
			ParticleSystem.Clear(withChildren: true);
		}
		if (ParticleSystems == null)
		{
			return;
		}
		for (int i = 0; i < ParticleSystems.Length; i++)
		{
			ParticleSystems[i].Stop(withChildren: true);
			if (Clear)
			{
				ParticleSystems[i].Clear(withChildren: true);
			}
		}
	}
}
