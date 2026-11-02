using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.Survival;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Menu;

public class CadetIcon : MonoBehaviour
{
	public Image image;

	public TextMeshProUGUI nameText;

	public Character character;

	public GlobalBoolVariable isCharAvailable;

	public CharacterList allCharacters;

	[SerializeField]
	private GlobalIntVariable previousCaptain;

	public void SetValues()
	{
		if (isCharAvailable.Value)
		{
			image.sprite = character.StaticData.Icons.ButtonIcon;
			nameText.text = character.StaticData.Name;
		}
		else
		{
			image.sprite = null;
			nameText.text = "Locked";
		}
	}

	private void Start()
	{
		SetValues();
	}

	public void SetCaptain()
	{
		foreach (Character allCharacter in allCharacters.AllCharacters)
		{
			allCharacter.RuntimeData.IsCaptain = false;
		}
		character.RuntimeData.IsCaptain = true;
		previousCaptain.Value = allCharacters.GetCharacterSet.GetCharacterId(character);
		if (Singleton<HandCursor>.Instance != null)
		{
			Singleton<HandCursor>.Instance.RefreshCaptainCursor();
		}
	}
}
