using System.Collections.Generic;
using RG.Core.Base;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(fileName = "New CharacterBodyPrefabList", menuName = "60 Parsecs!/Characters/New Character Body Prefab List")]
public class CharacterBodyPrefabList : RGScriptableObject
{
	[SerializeField]
	private List<CharacterPrefabPair> _characterBodyPrefabs;

	public List<CharacterPrefabPair> CharacterBodyPrefabs => _characterBodyPrefabs;
}
