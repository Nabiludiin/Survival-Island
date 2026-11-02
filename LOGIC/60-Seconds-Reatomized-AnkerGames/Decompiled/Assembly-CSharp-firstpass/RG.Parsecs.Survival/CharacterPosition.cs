using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class CharacterPosition
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private Vector3 _position;

	public Character Character => _character;

	public Vector3 Position => _position;
}
