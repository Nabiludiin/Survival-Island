using UnityEngine.Playables;

namespace RG.Parsecs.Scavenge;

public class GameManagerPlayableMixerBehaviour : PlayableBehaviour
{
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		ScavengeManager scavengeManager = playerData as ScavengeManager;
		if ((bool)scavengeManager)
		{
			int inputCount = playable.GetInputCount();
			for (int i = 0; i < inputCount; i++)
			{
				((ScriptPlayable<GameManagerPlayableBehaviour>)playable.GetInput(i)).GetBehaviour().Manager = scavengeManager;
			}
		}
	}
}
