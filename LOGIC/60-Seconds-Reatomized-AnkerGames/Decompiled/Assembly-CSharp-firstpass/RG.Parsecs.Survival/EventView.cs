using System.Collections.Generic;
using RG.Parsecs.EventEditor;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class EventView : WindowView
{
	[SerializeField]
	[Tooltip("Controller of choice cards.")]
	private ChoiceCardsController _choiceCardsController;

	[SerializeField]
	[Tooltip("Transform in which all event content will be held.")]
	private Transform _contentHolder;

	[SerializeField]
	private Slider _scrollSlider;

	[SerializeField]
	private GameObject RedLightEndOfDay;

	[SerializeField]
	private GameObject YellowLightOfEndDay;

	[SerializeField]
	private GameObject _yellowLightEvent;

	[SerializeField]
	private GameObject _greenLightEvent;

	[SerializeField]
	private GameObject _eventPaper;

	private List<EventContent> _currentContent = new List<EventContent>();

	private ChoiceCardController _yesterdayPlayerChoice;

	public void OnAcceptButtonPressed()
	{
		HideWindow();
	}

	private void HideWindow()
	{
		if (_choiceCardsController.AcceptChoice())
		{
			YellowLightOfEndDay.SetActive(value: true);
			RedLightEndOfDay.SetActive(value: false);
			_greenLightEvent.SetActive(value: true);
			_yellowLightEvent.SetActive(value: false);
			if (_choiceCardsController.GetPlayerChoice() != null)
			{
				_eventPaper.SetActive(value: true);
			}
		}
		Hide();
		_scrollSlider.value = 0f;
	}

	public void OnCloseWindowButtonPressed()
	{
		HideWindow();
	}

	protected override void OnClickAway()
	{
		HideWindow();
	}

	public void SetPlayerChoice()
	{
		_yesterdayPlayerChoice = _choiceCardsController.GetPlayerChoice();
	}

	public void PrepareEventView()
	{
		_scrollSlider.value = 0f;
		_choiceCardsController.ResetCardsController();
		foreach (EventContent item in _currentContent)
		{
			Object.Destroy(item.gameObject);
		}
		_currentContent.Clear();
		YellowLightOfEndDay.SetActive(value: false);
		RedLightEndOfDay.SetActive(value: true);
		_greenLightEvent.SetActive(value: false);
		_yellowLightEvent.SetActive(value: true);
		_eventPaper.SetActive(value: false);
	}

	public void AddContent(EventContent content)
	{
		content.SetParent(_contentHolder);
		_currentContent.Add(content);
	}

	public ChoiceCardController GetPlayerChoice()
	{
		return _yesterdayPlayerChoice;
	}

	public void SetChoiceContent()
	{
		_choiceCardsController.SetYesNoCards(ParsecsEventManager.YesIcon, ParsecsEventManager.NoIcon);
	}

	public void SetChoiceContent(Character character, CharacterAttribute attribute1, CharacterAttribute attribute2 = null, CharacterAttribute attribute3 = null, CharacterAttribute attribute4 = null)
	{
		_choiceCardsController.SetCharacterCards(character, attribute1, attribute2, attribute3, attribute4);
	}

	public void SetChoiceContent(BaseActionCondition choice1, BaseActionCondition choice2, BaseActionCondition choice3, BaseActionCondition choice4)
	{
		_choiceCardsController.SetSpriteCards(choice1, choice2, choice3, choice4);
	}

	public void SetChoiceContent(bool useResource, GameResource resource1, GameResource resource2 = null, GameResource resource3 = null)
	{
		_choiceCardsController.SetResourceCards(useResource, resource1, resource2, resource3);
	}

	public void SetChoiceContent(IItem item1, IItem item2 = null, IItem item3 = null, IItem item4 = null)
	{
		_choiceCardsController.SetItemCards(item1, item2, item3, item4);
	}

	public void SetChoiceContent(Character character1, Character character2 = null, Character character3 = null, Character character4 = null)
	{
		_choiceCardsController.SetCharacterCards(character1, character2, character3, character4);
	}

	public override void OnShow()
	{
		_choiceCardsController.RefreshCardsController();
	}
}
