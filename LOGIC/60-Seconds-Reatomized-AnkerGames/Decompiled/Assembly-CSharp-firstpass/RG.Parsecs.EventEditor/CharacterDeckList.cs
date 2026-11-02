using System;
using System.Collections.Generic;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Serializable]
[CreateAssetMenu(fileName = "New Character Deck List", menuName = "60 Parsecs!/Create Character Deck List")]
public class CharacterDeckList : ScriptableObject
{
	[SerializeField]
	private List<CharacterDeck> _decks;

	public List<CharacterDeck> Decks => _decks;
}
