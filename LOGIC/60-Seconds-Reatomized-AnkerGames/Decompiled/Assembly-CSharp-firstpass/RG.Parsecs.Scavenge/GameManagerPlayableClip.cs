using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class GameManagerPlayableClip : PlayableAsset, ITimelineClipAsset
{
	public GameManagerPlayableBehaviour template = new GameManagerPlayableBehaviour();

	public ScavangeAction Action;

	public ClipCaps clipCaps => ClipCaps.None;

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<GameManagerPlayableBehaviour> scriptPlayable = ScriptPlayable<GameManagerPlayableBehaviour>.Create(graph, template);
		scriptPlayable.GetBehaviour().SetAction(Action);
		return scriptPlayable;
	}
}
