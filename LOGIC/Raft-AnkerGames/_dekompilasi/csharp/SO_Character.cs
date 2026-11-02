using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Raft/Character", order = 1)]
public class SO_Character : ScriptableObject
{
	public int index;

	public string displayName;

	public Sprite sprite;

	public CharacterModelModifications modelPrefab;
}
