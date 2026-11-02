using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ActivateGameObjectsAction : EventAction
{
	public bool ActOnTriggeringObject;

	public List<GameObject> Objects = new List<GameObject>();

	protected override void Act(EventActionArgs args = null)
	{
		for (int i = 0; i < Objects.Count; i++)
		{
			if (!(Objects[i] == null))
			{
				Objects[i].SetActive(value: true);
			}
		}
		if (ActOnTriggeringObject && args != null)
		{
			if (args.Actor != null)
			{
				args.Actor.GameObject.SetActive(value: true);
			}
			args.Object.SetActive(value: true);
		}
	}
}
