using System;
using System.Collections.Generic;

namespace RG.Parsecs.Survival;

[Serializable]
internal struct EventManagerWrapper
{
	public List<DayEventContentListWrapper> List;

	public int ReportCount;
}
