using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class DeactivateRenderersAction : EventAction
{
	public List<Renderer> Objects = new List<Renderer>();

	protected override void Start()
	{
		base.Start();
	}

	protected override void Act(EventActionArgs args = null)
	{
		for (int i = 0; i < Objects.Count; i++)
		{
			Objects[i].enabled = false;
		}
	}
}
