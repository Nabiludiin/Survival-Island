using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.SecondsRemaster.Survival;

[CreateAssetMenu(fileName = "New Character", menuName = "60 Seconds Remaster!/Characters/New Character")]
public class SecondsCharacter : Character
{
	[SerializeField]
	private IconSizeDefinition _iconSizeDefinition;

	public IconSizeDefinition SizeDefinition => _iconSizeDefinition;

	public override bool CanLock()
	{
		if (!CharacterManager.Instance.GetCharacterList().CharactersInGame.Contains(this))
		{
			return false;
		}
		if (base.CanLock() && RuntimeData.IsDrawnOnShip())
		{
			return RuntimeData.IsAlive();
		}
		return false;
	}
}
