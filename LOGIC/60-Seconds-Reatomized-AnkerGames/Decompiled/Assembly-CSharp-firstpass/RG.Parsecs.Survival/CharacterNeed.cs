using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
[CreateAssetMenu(menuName = "60 Parsecs!/Characters/New Need", fileName = "New Need")]
public class CharacterNeed : CharacterProperty
{
	[SerializeField]
	private bool _canBeHadByCaptain = true;

	public bool CanBeHadByCaptain => _canBeHadByCaptain;
}
