using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class PoolableParticleBurstAction : EventAction
{
	public string PoolName;

	public EffectTarget EffectTarget;

	protected override void Act(EventActionArgs args = null)
	{
		Vector3 position = default;
		switch (EffectTarget)
		{
		case EffectTarget.Action:
			position = transform.position;
			break;
		case EffectTarget.Actor:
			if (args == null)
			{
				Debug.LogErrorFormat("PoolableParticleBurstAction EffectTarget:Actor but args are null");
				return;
			}
			position = args.Actor.GetPosition();
			break;
		case EffectTarget.Target:
			if (args == null)
			{
				Debug.LogErrorFormat("PoolableParticleBurstAction EffectTarget:Target but args are null");
				return;
			}
			if (args.Target == null)
			{
				Debug.LogErrorFormat("PoolableParticleBurstAction EffectTarget:Target but Target is null");
				return;
			}
			position = args.Target.GetPosition();
			break;
		}
		ParticleSystemPool.I.GetObjectForType(PoolName).PlayAtPosition(position);
	}
}
