using System;
using System.Collections.Generic;

namespace RG.Parsecs.Survival;

[Serializable]
public struct DayEventContentListWrapper
{
	public int PlayerChoice;

	public List<EventContentData.EventContentDataWrapper> DayList;
}
