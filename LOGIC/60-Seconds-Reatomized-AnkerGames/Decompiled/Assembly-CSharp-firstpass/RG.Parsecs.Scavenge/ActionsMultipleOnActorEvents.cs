using System;
using System.Collections.Generic;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class ActionsMultipleOnActorEvents
{
	public List<ActorEvents> Events;

	public List<EventAction> Actions;
}
