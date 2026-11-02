using System;
using UnityEngine;
using UnityEngine.Playables;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class GameManagerPlayableBehaviour : PlayableBehaviour
{
	public ScavengeManager Manager;

	public ScavangeAction _action;

	public void SetAction(ScavangeAction action)
	{
		_action = action;
	}

	public override void OnGraphStart(Playable playable)
	{
	}

	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		if (Application.isPlaying)
		{
			switch (_action)
			{
			case ScavangeAction.START:
				Manager.StartGameplay();
				break;
			case ScavangeAction.PAUSE:
				Manager.PauseGameplay();
				break;
			case ScavangeAction.UNPAUSE:
				Manager.UnpauseGameplay();
				break;
			case ScavangeAction.SHOW_SUMMARY:
				Manager.ShowSummary();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		base.OnBehaviourPlay(playable, info);
	}
}
