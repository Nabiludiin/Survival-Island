using System;
using System.Collections.Generic;

namespace RG.Parsecs.Survival;

[Serializable]
public struct ScheduleEventWrapper
{
	public string EventName;

	public List<EventParameterWrapper> CharacterParameter;

	public List<EventParameterWrapper> ItemParameter;
}
