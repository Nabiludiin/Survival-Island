using System;
using System.Collections.Generic;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class ActionsMultipleScavengeItemEvents
{
	public List<ScavengeItemEvents> Events;

	public List<EventAction> Actions;
}
