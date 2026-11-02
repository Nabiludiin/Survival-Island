using RG.Parsecs.EventEditor;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class ChoiceCardsController : MonoBehaviour, IRefreshInteractable
{
	[SerializeField]
	[Tooltip("References to all choice cards in Event View.")]
	private ChoiceCardController[] _choiceCards;

	[SerializeField]
	[Tooltip("Reference to image that will be used to display current choice.")]
	private Image _currentChoiceComputer;

	[SerializeField]
	[Tooltip("Reference to animator that is used to hide lever on scene.")]
	private Animator _leverPanelAnimator;

	[SerializeField]
	[Tooltip("Reference to animator that is used to hide lever on scene.")]
	private Animator _choiceCardAnimation;

	[SerializeField]
	[Tooltip("Reference to the accept button that will be used to confirm the choice.")]
	private Button _acceptButton;

	[SerializeField]
	[Tooltip("Reference to Survival Data ScriptableObject.")]
	private SurvivalData _data;

	[SerializeField]
	private bool _useGamepadButton = true;

	private int _currentAmountOfEnabledCards;

	private int _currentChoiceNumber;

	private ChoiceCardController _currentChoice;

	public void SetSpriteCards(BaseActionCondition choice1, BaseActionCondition choice2, BaseActionCondition choice3, BaseActionCondition choice4)
	{
		if (!choice1 && !choice2 && !choice3 && !choice4)
		{
			Debug.LogWarning("You're not setting any sprite choice in Choose Sprite type event. Fallbacks to no choice event");
			SetAllCardsDisabled();
			return;
		}
		_currentAmountOfEnabledCards = 0;
		for (int num = _choiceCards.Length - 1; num >= 0; num--)
		{
			if ((bool)choice1)
			{
				_choiceCards[num].SetCurrentChoiceType(choice1);
				_currentAmountOfEnabledCards++;
				choice1 = null;
			}
			else if ((bool)choice2)
			{
				_choiceCards[num].SetCurrentChoiceType(choice2);
				_currentAmountOfEnabledCards++;
				choice2 = null;
			}
			else if ((bool)choice3)
			{
				_choiceCards[num].SetCurrentChoiceType(choice3);
				_currentAmountOfEnabledCards++;
				choice3 = null;
			}
			else if ((bool)choice4)
			{
				_choiceCards[num].SetCurrentChoiceType(choice4);
				_currentAmountOfEnabledCards++;
				choice4 = null;
			}
			else
			{
				_choiceCards[num].DisableCard();
			}
		}
		if (_acceptButton != null)
		{
			_acceptButton.interactable = false;
		}
	}

	public void SetYesNoCards(Sprite yesIcon, Sprite noIcon)
	{
		_choiceCards[0].DisableCard();
		_choiceCards[1].DisableCard();
		_choiceCards[2].SetCurrentChoiceType(EPlayerChoice.YES, yesIcon);
		_choiceCards[3].SetCurrentChoiceType(EPlayerChoice.NO, noIcon);
		_currentAmountOfEnabledCards = 2;
		if (_acceptButton != null)
		{
			_acceptButton.interactable = false;
		}
	}

	public void SetCharacterCards(Character character, CharacterAttribute attribute1, CharacterAttribute attribute2 = null, CharacterAttribute attribute3 = null, CharacterAttribute attribute4 = null)
	{
		if (!attribute1 && !attribute2 && !attribute3 && !attribute4)
		{
			Debug.LogWarning("You're not setting any character in Choose Character type event. Fallbacks to no character event");
			SetAllCardsDisabled();
			return;
		}
		_currentAmountOfEnabledCards = 0;
		for (int num = _choiceCards.Length - 1; num >= 0; num--)
		{
			if ((bool)attribute4)
			{
				_choiceCards[num].SetCurrentChoiceType(attribute4, character);
				_currentAmountOfEnabledCards++;
				attribute4 = null;
			}
			else if ((bool)attribute3)
			{
				_choiceCards[num].SetCurrentChoiceType(attribute3, character);
				_currentAmountOfEnabledCards++;
				attribute3 = null;
			}
			else if ((bool)attribute2)
			{
				_choiceCards[num].SetCurrentChoiceType(attribute2, character);
				_currentAmountOfEnabledCards++;
				attribute2 = null;
			}
			else if ((bool)attribute1)
			{
				_choiceCards[num].SetCurrentChoiceType(attribute1, character);
				_currentAmountOfEnabledCards++;
				attribute1 = null;
			}
			else
			{
				_choiceCards[num].DisableCard();
			}
		}
		if (_acceptButton != null)
		{
			_acceptButton.interactable = false;
		}
	}

	public void SetItemCards(IItem item1, IItem item2 = null, IItem item3 = null, IItem item4 = null, bool useCardAsNoChoice = true)
	{
		if (!item1 && !item2 && !item3 && !item4)
		{
			Debug.LogWarning("You're not setting any item in Choose Item type event. Fallbacks to no choice event");
			SetAllCardsDisabled();
			return;
		}
		if ((item4 == null) & useCardAsNoChoice)
		{
			_choiceCards[3].SetCurrentChoiceType(EPlayerChoice.NO_CHOICE, ParsecsEventManager.NoChoiceIcon);
			_currentAmountOfEnabledCards = 1;
		}
		for (int num = _choiceCards.Length - 1 - _currentAmountOfEnabledCards; num >= 0; num--)
		{
			if ((bool)item4)
			{
				_choiceCards[num].SetCurrentChoiceType(item4);
				_currentAmountOfEnabledCards++;
				item4 = null;
			}
			else if ((bool)item3)
			{
				_choiceCards[num].SetCurrentChoiceType(item3);
				_currentAmountOfEnabledCards++;
				item3 = null;
			}
			else if ((bool)item2)
			{
				_choiceCards[num].SetCurrentChoiceType(item2);
				_currentAmountOfEnabledCards++;
				item2 = null;
			}
			else if ((bool)item1)
			{
				_choiceCards[num].SetCurrentChoiceType(item1);
				_currentAmountOfEnabledCards++;
				item1 = null;
			}
			else
			{
				_choiceCards[num].DisableCard();
			}
		}
		if (_acceptButton != null)
		{
			_acceptButton.interactable = false;
		}
	}

	public void SetResourceCards(bool useResource, GameResource resource1, GameResource resource2 = null, GameResource resource3 = null, bool useCardAsNoChoice = true)
	{
		GameResource[] array = new GameResource[3] { resource1, resource2, resource3 };
		if (resource1 == null && resource2 == null && resource3 == null)
		{
			Debug.LogWarning("You're not setting any resource in Choose Resource type event. Fallbacks to no choice event");
			SetAllCardsDisabled();
			return;
		}
		int num = _choiceCards.Length - 1;
		if (useCardAsNoChoice)
		{
			_choiceCards[3].SetCurrentChoiceType(EPlayerChoice.NO_CHOICE, ParsecsEventManager.NoChoiceIcon);
			_currentAmountOfEnabledCards = 1;
			num--;
		}
		for (int num2 = num; num2 >= 0; num2--)
		{
			if (array[num2] != null && array[num2].Resource != null)
			{
				_choiceCards[num2].SetCurrentChoiceType(array[num2], useResource);
				_currentAmountOfEnabledCards++;
			}
			else
			{
				_choiceCards[num2].DisableCard();
			}
		}
		if (_acceptButton != null)
		{
			_acceptButton.interactable = false;
		}
	}

	public void SetCharacterCards(Character character1, Character character2 = null, Character character3 = null, Character character4 = null)
	{
		if (!character1 && !character2 && !character3 && !character4)
		{
			Debug.LogWarning("You're not setting any character in Choose Character type event. Fallbacks to no character event");
			SetAllCardsDisabled();
			return;
		}
		_currentAmountOfEnabledCards = 0;
		for (int num = _choiceCards.Length - 1; num >= 0; num--)
		{
			if ((bool)character4)
			{
				_choiceCards[num].SetCurrentChoiceType(character4);
				_currentAmountOfEnabledCards++;
				character4 = null;
			}
			else if ((bool)character3)
			{
				_choiceCards[num].SetCurrentChoiceType(character3);
				_currentAmountOfEnabledCards++;
				character3 = null;
			}
			else if ((bool)character2)
			{
				_choiceCards[num].SetCurrentChoiceType(character2);
				_currentAmountOfEnabledCards++;
				character2 = null;
			}
			else if ((bool)character1)
			{
				_choiceCards[num].SetCurrentChoiceType(character1);
				_currentAmountOfEnabledCards++;
				character1 = null;
			}
			else
			{
				_choiceCards[num].DisableCard();
			}
		}
		if (_acceptButton != null)
		{
			_acceptButton.interactable = false;
		}
	}

	public void SetAllCardsDisabled()
	{
		if (_currentChoice != null)
		{
			if (_currentChoice.Character != null)
			{
				_currentChoice.Character.Unlock();
			}
			if (_currentChoice.Item != null)
			{
				_currentChoice.Item.Unlock();
			}
			if (_currentChoice.ActionCondition != null)
			{
				_currentChoice.ActionCondition.OnDeselect();
			}
			_currentChoice = null;
		}
		_currentAmountOfEnabledCards = 0;
		if (_acceptButton != null)
		{
			_acceptButton.interactable = true;
		}
		for (int i = 0; i < _choiceCards.Length; i++)
		{
			_choiceCards[i].DisableCard();
		}
	}

	public void SetCurrentChoice(ChoiceCardController choiceCard)
	{
		if (_currentChoice == choiceCard)
		{
			return;
		}
		if (_acceptButton != null)
		{
			_acceptButton.interactable = true;
		}
		if (_currentChoice != null)
		{
			_currentChoice.Unlock();
		}
		if (choiceCard != null)
		{
			choiceCard.Hide(animate: true);
		}
		if (_currentChoiceComputer != null)
		{
			_currentChoiceComputer.enabled = true;
			if (choiceCard != null)
			{
				_currentChoiceComputer.sprite = choiceCard.GetCurrentImage();
			}
		}
		_currentChoice = choiceCard;
		if (_currentChoice != null)
		{
			_currentChoice.Lock();
		}
		if (_choiceCardAnimation != null)
		{
			_choiceCardAnimation.SetTrigger("Spin");
		}
		for (int i = 0; i < _choiceCards.Length; i++)
		{
			if (_choiceCards[i].Enabled)
			{
				if (choiceCard == _choiceCards[i])
				{
					_currentChoiceNumber = i;
				}
				else if (choiceCard != _choiceCards[i] && !_choiceCards[i].Visible)
				{
					_choiceCards[i].Show(animate: true);
				}
			}
		}
	}

	public bool AcceptChoice()
	{
		if (_currentChoice == null && _currentAmountOfEnabledCards != 0)
		{
			return false;
		}
		_data.DailyEventResolved = true;
		if (_leverPanelAnimator != null)
		{
			_leverPanelAnimator.SetBool("Open", value: true);
		}
		if (_currentChoice == null && _currentAmountOfEnabledCards == 0)
		{
			return true;
		}
		ParsecsEventManager.SetDaysChoiceForStarlog(_currentChoiceNumber);
		return true;
	}

	public ChoiceCardController GetPlayerChoice()
	{
		return _currentChoice;
	}

	public void ResetCardsController()
	{
		SetAllCardsDisabled();
		if (_currentChoiceComputer != null)
		{
			_currentChoiceComputer.enabled = false;
			_currentChoiceComputer.sprite = null;
		}
		_data.DailyEventResolved = false;
	}

	public void RefreshCardsController()
	{
		for (int i = 0; i < _choiceCards.Length; i++)
		{
			if (_choiceCards[i].gameObject.activeSelf)
			{
				_choiceCards[i].RefreshCard();
			}
		}
	}

	public void RefreshInteractable()
	{
		for (int i = 0; i < _choiceCards.Length; i++)
		{
			if (_choiceCards[i].gameObject.activeSelf)
			{
				_choiceCards[i].RefreshCard();
			}
		}
		if (_currentChoice != null && _useGamepadButton && _acceptButton != null)
		{
			_acceptButton.interactable = true;
		}
	}
}
