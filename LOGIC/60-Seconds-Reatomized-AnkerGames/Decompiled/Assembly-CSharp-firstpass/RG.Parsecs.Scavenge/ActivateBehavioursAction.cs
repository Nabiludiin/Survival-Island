using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ActivateBehavioursAction : EventAction
{
	public List<MonoBehaviour> Objects = new List<MonoBehaviour>();

	protected override void Act(EventActionArgs args = null)
	{
		for (int i = 0; i < Objects.Count; i++)
		{
			if (!(Objects[i] == null))
			{
				Objects[i].enabled = true;
			}
		}
	}
}
