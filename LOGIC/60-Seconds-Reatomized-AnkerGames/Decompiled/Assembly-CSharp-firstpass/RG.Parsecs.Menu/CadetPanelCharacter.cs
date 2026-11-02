using I2.Loc;
using RG.Parsecs.EventEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Menu;

[CreateAssetMenu(fileName = "Character", menuName = "RGCore/Menu/CadetCharacter")]
public class CadetPanelCharacter : ScriptableObject
{
	public LocalizedString bio;

	public LocalizedString personalGoal;

	public Sprite faceBlack;

	public Sprite faceColor;

	public Sprite posture;

	public Character character;

	public CharacterList allCharacters;

	public GlobalIntVariable previousCaptain;
}
