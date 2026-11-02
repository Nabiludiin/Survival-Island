using System;
using UnityEngine;

[Serializable]
public class QuestInteractable_ComponentData_Particles : QuestInteractable_ComponentData
{
	public ParticleComponentModification componentModification;

	public ParticleSystem particleSystem;

	public bool modifyChildParticleSystems = true;

	public override void OnFinishedInteraction(Network_Player player, bool atStart, bool successfull)
	{
		base.OnFinishedInteraction(player, atStart, successfull);
		if (particleSystem != null)
		{
			switch (componentModification)
			{
			case ParticleComponentModification.Enable_Emission:
			{
				ParticleSystem.EmissionModule emission2 = particleSystem.emission;
				emission2.enabled = true;
				break;
			}
			case ParticleComponentModification.Disable_Emission:
			{
				ParticleSystem.EmissionModule emission = particleSystem.emission;
				emission.enabled = false;
				break;
			}
			case ParticleComponentModification.Play:
				particleSystem.Play(modifyChildParticleSystems);
				break;
			case ParticleComponentModification.Stop:
				particleSystem.Stop(modifyChildParticleSystems);
				break;
			case ParticleComponentModification.Unparent:
				particleSystem.gameObject.transform.SetParentSafe(null);
				break;
			case ParticleComponentModification.PlayAndUnparent:
				particleSystem.Play(modifyChildParticleSystems);
				particleSystem.gameObject.transform.SetParentSafe(null);
				break;
			}
		}
	}
}
