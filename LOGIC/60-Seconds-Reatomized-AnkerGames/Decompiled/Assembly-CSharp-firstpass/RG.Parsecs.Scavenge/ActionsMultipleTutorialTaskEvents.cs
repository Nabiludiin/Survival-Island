using System;
using System.Collections.Generic;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class ActionsMultipleTutorialTaskEvents
{
	public List<TutorialTaskEvents> Events;

	public List<EventAction> Actions;
}
