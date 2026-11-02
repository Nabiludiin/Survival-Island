using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class LoggerTestAction : EventAction
{
	public string DebugTekst;

	protected override void Act(EventActionArgs args = null)
	{
		Debug.Log(DebugTekst);
	}
}
