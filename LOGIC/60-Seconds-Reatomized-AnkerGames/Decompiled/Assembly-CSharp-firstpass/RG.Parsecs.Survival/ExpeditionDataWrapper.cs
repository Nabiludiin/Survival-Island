using System;
using System.Collections.Generic;

namespace RG.Parsecs.Survival;

[Serializable]
internal struct ExpeditionDataWrapper
{
	public string Destination;

	public string Character;

	public List<string> Items;
}
