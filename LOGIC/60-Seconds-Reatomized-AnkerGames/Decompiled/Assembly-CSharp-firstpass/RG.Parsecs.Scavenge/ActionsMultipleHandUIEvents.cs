using System;
using System.Collections.Generic;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class ActionsMultipleHandUIEvents
{
	public List<HandUIEvents> Events;

	public List<EventAction> Actions;
}
