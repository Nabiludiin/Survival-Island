using System;
using System.Collections.Generic;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class ActionsMultipleOnRescuePodEntranceEvents
{
	public List<RescurePodEntranceEvents> Events;

	public List<EventAction> Actions;
}
