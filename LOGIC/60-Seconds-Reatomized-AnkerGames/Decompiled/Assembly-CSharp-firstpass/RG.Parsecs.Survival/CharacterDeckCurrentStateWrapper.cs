using System;

namespace RG.Parsecs.Survival;

[Serializable]
internal struct CharacterDeckCurrentStateWrapper
{
	public DeckState CurrentState;

	public bool IsOnHold;
}
