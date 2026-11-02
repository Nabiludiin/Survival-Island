using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ParticlesClearAction : EventAction
{
	public ParticleSystem ParticleSystem;

	public ParticleSystem[] ParticleSystems;

	protected override void Act(EventActionArgs args = null)
	{
		if (ParticleSystem != null)
		{
			ParticleSystem.Clear(withChildren: true);
		}
		if (ParticleSystems != null)
		{
			for (int i = 0; i < ParticleSystems.Length; i++)
			{
				ParticleSystems[i].Clear(withChildren: true);
			}
		}
	}
}
