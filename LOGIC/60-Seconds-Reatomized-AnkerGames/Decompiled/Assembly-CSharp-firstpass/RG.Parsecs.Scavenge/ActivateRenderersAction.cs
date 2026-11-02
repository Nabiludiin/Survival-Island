using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ActivateRenderersAction : EventAction
{
	public List<Renderer> Objects = new List<Renderer>();

	protected override void Act(EventActionArgs args = null)
	{
		for (int i = 0; i < Objects.Count; i++)
		{
			Objects[i].enabled = true;
		}
	}
}
