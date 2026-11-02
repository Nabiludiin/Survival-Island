using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class Influence
{
	[SerializeField]
	private CharacterNeed _need;

	[SerializeField]
	private float _value;

	public CharacterNeed Need => _need;

	public float Value => _value;
}
