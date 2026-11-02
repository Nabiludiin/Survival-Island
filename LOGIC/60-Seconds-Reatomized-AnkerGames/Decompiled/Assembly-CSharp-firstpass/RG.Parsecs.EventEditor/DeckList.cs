using System;
using System.Collections.Generic;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Serializable]
[CreateAssetMenu(fileName = "New Deck List", menuName = "60 Parsecs!/Create Deck List")]
public class DeckList : ScriptableObject
{
	[SerializeField]
	private List<Deck> _decks;

	public List<Deck> Decks => _decks;
}
