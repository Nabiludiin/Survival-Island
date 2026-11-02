using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace RG.Parsecs.Scavenge;

[TrackColor(0.855f, 0.8623f, 0.87f)]
[TrackClipType(typeof(GameManagerPlayableClip))]
[TrackBindingType(typeof(ScavengeManager))]
public class GameManagerPlayableTrack : TrackAsset
{
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		return ScriptPlayable<GameManagerPlayableMixerBehaviour>.Create(graph, inputCount);
	}
}
