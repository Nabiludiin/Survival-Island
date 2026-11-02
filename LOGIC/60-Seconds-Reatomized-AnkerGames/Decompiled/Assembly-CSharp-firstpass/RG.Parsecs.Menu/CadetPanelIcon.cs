using System.Collections.Generic;
using I2.Loc;
using RG.Parsecs.Common;
using RG.Parsecs.Survival;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Menu;

public class CadetPanelIcon : MonoBehaviour
{
	[TextArea]
	public string descriptionTemplate;

	public LocalizedString personalGoalText;

	public Image posture;

	public Image faceBlack;

	public Image faceColor;

	public TextMeshProUGUI description;

	public CadetPanelCharacter cadet;

	public bool useToCycle;

	public CadetPanelCharacter[] cadetList;

	private int currentCadet = -1;

	public Image[] stats;

	public TextMeshProUGUI[] statsLabels;

	private void OnEnable()
	{
		if (useToCycle)
		{
			for (int i = 0; i < cadetList.Length; i++)
			{
				if (cadetList[i].character.RuntimeData.IsCaptain)
				{
					currentCadet = i;
					break;
				}
			}
			Cycle(0);
		}
		if (faceBlack != null)
		{
			faceBlack.sprite = cadet.faceBlack;
		}
		if (faceColor != null)
		{
			faceColor.sprite = cadet.faceColor;
		}
	}

	public void Cycle(int dir)
	{
		currentCadet += dir;
		if (currentCadet >= cadetList.Length)
		{
			currentCadet = 0;
		}
		if (currentCadet < 0)
		{
			currentCadet = cadetList.Length - 1;
		}
		cadet = cadetList[currentCadet];
		Clicked();
	}

	public void Clicked()
	{
		if (posture != null)
		{
			posture.sprite = cadet.posture;
		}
		if (description != null)
		{
			description.text = string.Format(descriptionTemplate, cadet.character.StaticData.FullName, cadet.bio, personalGoalText, cadet.personalGoal);
		}
		if (stats.Length != 0)
		{
			List<CharacterAttributeValue> mutableAttributes = cadet.character.StaticData.GetMutableAttributes();
			int num = 0;
			foreach (CharacterAttributeValue item in mutableAttributes)
			{
				if (stats.Length > num)
				{
					stats[num].sprite = item.Attribute.GetAttributeLevel(item.Value).Icon;
					statsLabels[num].text = item.Attribute.Name;
					num++;
				}
			}
		}
		SetCaptain();
	}

	public void SetCaptain()
	{
		foreach (Character allCharacter in cadet.allCharacters.AllCharacters)
		{
			allCharacter.RuntimeData.IsCaptain = false;
		}
		Character character = cadet.character;
		character.RuntimeData.IsCaptain = true;
		cadet.previousCaptain.Value = cadet.allCharacters.GetCharacterSet.GetCharacterId(character);
		if (Singleton<HandCursor>.Instance != null)
		{
			Singleton<HandCursor>.Instance.RefreshCaptainCursor();
		}
	}
}
