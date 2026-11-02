using RG.Core.Base;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class EndOfDayRationingBarView : RGMonoBehaviour
{
	[SerializeField]
	private CharacterList _characterList;

	[SerializeField]
	private int _characterIndex;

	[SerializeField]
	private Image _characterIcon;

	[SerializeField]
	private GameObject _rations;

	public int CharacterIndex => _characterIndex;

	protected override void CustomAwake()
	{
		RefreshRationingBar();
	}

	public void RefreshRationingBar()
	{
		if (_characterList.CharactersInGame.Count > _characterIndex)
		{
			_rations.SetActive(value: true);
			_characterIcon.gameObject.SetActive(value: true);
			_characterIcon.sprite = _characterList.CharactersInGame[_characterIndex].StaticData.Icons.JournalIcon;
		}
		else
		{
			_rations.SetActive(value: false);
			_characterIcon.gameObject.SetActive(value: false);
		}
	}
}
