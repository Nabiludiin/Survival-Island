using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class TeleportAction : EventAction
{
	public ActorBrain Actor;

	public Transform Target;

	public Vector3 TargetPosition;

	protected override void Act(EventActionArgs args = null)
	{
		ActorBrain actorBrain = Actor;
		if (actorBrain == null)
		{
			if (args == null)
			{
				return;
			}
			actorBrain = args.Actor as ActorBrain;
		}
		Vector3 position = ((Target != null) ? Target.position : TargetPosition);
		actorBrain.Transform.position = position;
	}
}
