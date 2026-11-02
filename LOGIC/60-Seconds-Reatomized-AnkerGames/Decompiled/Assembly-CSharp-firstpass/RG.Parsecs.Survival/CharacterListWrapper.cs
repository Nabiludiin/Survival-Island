using System;
using System.Collections.Generic;

namespace RG.Parsecs.Survival;

[Serializable]
internal struct CharacterListWrapper
{
	public List<string> CharactersList;

	public int StartCrewMember;
}
