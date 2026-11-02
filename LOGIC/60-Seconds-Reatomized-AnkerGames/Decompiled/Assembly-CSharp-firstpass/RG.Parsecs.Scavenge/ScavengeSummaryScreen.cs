using System.Collections.Generic;
using RG.Parsecs.Common;
using RG.Parsecs.Survival;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RG.Parsecs.Scavenge;

public class ScavengeSummaryScreen : MonoBehaviour
{
	public Image[] itemIcons;

	public ScavangeItemData[] ScavangeItemDatas;

	public ScavangeItemData soupEntry;

	public PlayerResources playerResources;

	public TextMeshProUGUI soupNumber;

	public List<SummaryScreenResourceIcon> resourceIcons;

	public CharacterSet availableCharacters;

	public CharacterList crew;

	public Image captainIcon;

	public Image[] crewIcons;

	public Selectable SurviveButton;

	public Selectable ReplayButton;

	public ScavengeManager scavengeManager;

	private void Start()
	{
		if (scavengeManager.GameWon)
		{
			SurviveButton.gameObject.SetActive(value: true);
			SurviveButton.Select();
		}
		else
		{
			SurviveButton.gameObject.SetActive(value: false);
			ReplayButton.Select();
		}
		if (itemIcons.Length != ScavangeItemDatas.Length)
		{
			Debug.LogError("SummaryScreen items number missmatch icons number");
		}
		float amount = (soupEntry.survivalItemRef.BaseRuntimeData as ConsumableRuntimeData).Amount;
		soupNumber.text = amount.ToString();
		for (int i = 0; i < ScavangeItemDatas.Length; i++)
		{
			if (ScavangeItemDatas[i].survivalItemRef.BaseRuntimeData.IsAvailable)
			{
				itemIcons[i].sprite = ScavangeItemDatas[i].handSprite;
			}
		}
		for (int j = 0; j < resourceIcons.Count; j++)
		{
			GameResource gameResource = playerResources.RuntimeResources[j];
			resourceIcons[j].icon.sprite = gameResource.Resource.Icon;
			resourceIcons[j].amount.text = gameResource.Amount.ToString();
		}
		Character[] allCharacters = availableCharacters.AllCharacters;
		foreach (Character character in allCharacters)
		{
			if (character.RuntimeData.IsCaptain)
			{
				captainIcon.sprite = character.StaticData.Icons.ButtonIcon;
				break;
			}
		}
		int num = 0;
		foreach (Character item in crew.CharactersInGame)
		{
			if (!item.RuntimeData.IsCaptain && num < crewIcons.Length)
			{
				crewIcons[num].sprite = item.StaticData.Icons.ButtonIcon;
				num++;
			}
		}
	}

	public void OnSurvive()
	{
		List<Scene> scavangeScenes = ShipGenerator.I.GetScavangeScenes();
		Singleton<GameManager>.Instance.StartSurvival(scavangeScenes);
	}

	public void OnRestart()
	{
		List<Scene> scavangeScenes = ShipGenerator.I.GetScavangeScenes();
		Singleton<GameManager>.Instance.StartScavenge(scavangeScenes);
	}

	public void OnMenu()
	{
		List<Scene> scavangeScenes = ShipGenerator.I.GetScavangeScenes();
		Singleton<GameManager>.Instance.LoadMenu(scavangeScenes);
	}
}
