using RG.Parsecs.Survival;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Scavenge;

public class CrewList : MonoBehaviour
{
	public CharacterList crew;

	public Image[] imageList;

	private void Start()
	{
		RefreshCrewList();
	}

	[ContextMenu("refresh crew")]
	public void RefreshCrewList()
	{
		for (int i = 0; i < imageList.Length; i++)
		{
			if (crew.CharactersInGame.Count <= i)
			{
				imageList[i].gameObject.SetActive(value: false);
				continue;
			}
			imageList[i].gameObject.SetActive(value: true);
			imageList[i].sprite = crew.CharactersInGame[i].StaticData.Icons.ButtonIcon;
		}
	}
}
