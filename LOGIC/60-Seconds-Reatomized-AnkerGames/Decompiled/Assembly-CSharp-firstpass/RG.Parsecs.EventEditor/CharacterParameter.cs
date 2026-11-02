using System;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Serializable]
public class CharacterParameter
{
	[Tooltip("Name of the parameter")]
	public string Name;

	[Tooltip("Character-type parameter value")]
	public Character Character;
}
