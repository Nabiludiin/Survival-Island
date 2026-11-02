using System;

namespace RG.Parsecs.Survival;

internal class DeckData : IComparable<DeckData>
{
	public Deck Deck;

	public float Probability;

	public int CompareTo(DeckData other)
	{
		return Probability.CompareTo(other.Probability);
	}
}
