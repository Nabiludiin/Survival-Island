using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class PlaySoundAction : EventAction
{
	public List<AudioSource> Sounds;

	protected override void Start()
	{
		base.Start();
	}

	protected override void Act(EventActionArgs args = null)
	{
		if (Sounds.Count == 0)
		{
			Debug.LogError("No sound effects present");
		}
		else
		{
			Sounds.RandomElement().Play();
		}
	}
}
