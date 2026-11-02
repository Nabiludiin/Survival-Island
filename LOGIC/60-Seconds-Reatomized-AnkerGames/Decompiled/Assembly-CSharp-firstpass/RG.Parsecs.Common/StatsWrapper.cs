using System;
using System.Collections.Generic;

namespace RG.Parsecs.Common;

[Serializable]
internal struct StatsWrapper
{
	public string Key;

	public int Value;

	public bool IsList;

	public List<string> List;
}
