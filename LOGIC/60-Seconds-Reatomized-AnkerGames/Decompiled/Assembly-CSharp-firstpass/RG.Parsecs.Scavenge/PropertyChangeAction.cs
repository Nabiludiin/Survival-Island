using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class PropertyChangeAction : EventAction
{
	public bool FireActions;

	public List<EventAction> Actions;

	protected override void Act(EventActionArgs args = null)
	{
		if (Actions.Count == 0)
		{
			Debug.LogError("No actions defined.");
			return;
		}
		for (int i = 0; i < Actions.Count; i++)
		{
			Actions[i].Invoke(args);
		}
	}

	private void Update()
	{
		if (FireActions)
		{
			FireActions = false;
			Act();
			Debug.Log("dupa");
		}
	}
}
