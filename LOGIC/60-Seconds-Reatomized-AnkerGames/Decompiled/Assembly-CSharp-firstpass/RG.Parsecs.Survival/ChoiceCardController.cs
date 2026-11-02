using RG.Parsecs.EventEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class ChoiceCardController : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Image Renderer that will display sprite of the choice e.g. Item sprite or Character sprite.")]
	private Image _imageRenderer;

	[SerializeField]
	[Tooltip("Animator Controller for Show/Hide/Set Disabled card states.")]
	private Animator _animatorController;

	[SerializeField]
	[FormerlySerializedAs("_button")]
	[Tooltip("Button to choose specific item")]
	private Selectable _selectable;

	[SerializeField]
	private PlayerResources _playerResources;

	[SerializeField]
	private int _cardId;

	[SerializeField]
	private ChoiceCardsController _choiceCardsController;

	private EPlayerChoice _choiceType;

	private IItem _item;

	private GameResource _gameResource;

	private bool _useResource;

	private Character _character;

	private BaseActionCondition _actionCondition;

	private CharacterAttribute _characterAttribute;

	public bool Visible { get; set; }

	public Character Character => _character;

	public IItem Item => _item;

	public GameResource GameResource => _gameResource;

	public bool Interactable
	{
		get
		{
			return _selectable.interactable;
		}
		set
		{
			_selectable.interactable = value;
		}
	}

	public bool UseResource => _useResource;

	public bool Enabled { get; private set; }

	public BaseActionCondition ActionCondition => _actionCondition;

	private void OnEnable()
	{
		if (!Visible)
		{
			Hide(animate: false);
		}
		else
		{
			Show(animate: true);
		}
	}

	public void RefreshCard()
	{
		if (_choiceType == EPlayerChoice.ITEM)
		{
			_selectable.interactable = _item.IsLockable() && _item.BaseRuntimeData.IsAvailable;
		}
		else if (_choiceType == EPlayerChoice.CHARACTER)
		{
			_selectable.interactable = _character.CanLock() && !_character.RuntimeData.HasStatusPreventingSelectionInEventChoices();
		}
		else if (_choiceType == EPlayerChoice.RESOURCE && _useResource)
		{
			ChoiceCardController playerChoice = _choiceCardsController.GetPlayerChoice();
			if (playerChoice != null)
			{
				GameResource gameResource = playerChoice.GameResource;
				if (gameResource != null)
				{
					_selectable.interactable = ((gameResource.Resource != _gameResource.Resource) ? _playerResources.CanLock(_gameResource) : _playerResources.CanLock(_gameResource.Resource, _gameResource.Amount - gameResource.Amount));
				}
				else
				{
					_selectable.interactable = _playerResources.CanLock(_gameResource);
				}
			}
			else
			{
				_selectable.interactable = _playerResources.CanLock(_gameResource);
			}
		}
		else if (_choiceType == EPlayerChoice.SPRITE)
		{
			_selectable.interactable = _actionCondition.CanSelect();
		}
		else
		{
			_selectable.interactable = true;
		}
	}

	public void Lock()
	{
		if (_choiceType == EPlayerChoice.ITEM)
		{
			_item.Lock();
		}
		else if (_choiceType == EPlayerChoice.CHARACTER)
		{
			_character.Lock();
		}
		else if (_choiceType == EPlayerChoice.RESOURCE && _useResource)
		{
			_playerResources.Lock(_gameResource);
		}
		else if (_choiceType == EPlayerChoice.SPRITE)
		{
			_actionCondition.OnSelect();
		}
	}

	public void Unlock()
	{
		if (_choiceType == EPlayerChoice.ITEM)
		{
			_item.Unlock();
		}
		else if (_choiceType == EPlayerChoice.CHARACTER)
		{
			_character.Unlock();
		}
		else if (_choiceType == EPlayerChoice.RESOURCE && _useResource)
		{
			_playerResources.Unlock(_gameResource);
		}
		else if (_choiceType == EPlayerChoice.SPRITE)
		{
			_actionCondition.OnDeselect();
		}
		RefreshCard();
	}

	public void DisableCard()
	{
		Visible = false;
		Enabled = false;
		_imageRenderer.enabled = false;
		gameObject.SetActive(value: false);
	}

	public void SetCurrentChoiceType(IItem item)
	{
		Visible = true;
		Enabled = true;
		_choiceType = EPlayerChoice.ITEM;
		_item = item;
		gameObject.SetActive(value: true);
		_imageRenderer.enabled = true;
		_imageRenderer.sprite = item.BaseStaticData.IconJournal;
	}

	public void SetCurrentChoiceType(GameResource resource, bool useResource)
	{
		Visible = true;
		Enabled = true;
		_choiceType = EPlayerChoice.RESOURCE;
		_gameResource = resource;
		_useResource = useResource;
		gameObject.SetActive(value: true);
		_imageRenderer.enabled = true;
		_imageRenderer.sprite = resource.Resource.Icon;
	}

	public void SetCurrentChoiceType(Character character)
	{
		Visible = true;
		Enabled = true;
		_choiceType = EPlayerChoice.CHARACTER;
		_character = character;
		gameObject.SetActive(value: true);
		_imageRenderer.enabled = true;
		_imageRenderer.sprite = character.StaticData.Icons.JournalIcon;
	}

	public void SetCurrentChoiceType(BaseActionCondition actionCondition)
	{
		Visible = true;
		Enabled = true;
		_choiceType = EPlayerChoice.SPRITE;
		_actionCondition = actionCondition;
		gameObject.SetActive(value: true);
		_imageRenderer.enabled = true;
		_imageRenderer.sprite = _actionCondition.Sprite;
	}

	public void SetCurrentChoiceType(CharacterAttribute characterAttribute, Character character)
	{
		Visible = true;
		Enabled = true;
		_choiceType = EPlayerChoice.ATTRIBUTE;
		_characterAttribute = characterAttribute;
		_character = character;
		gameObject.SetActive(value: true);
		_imageRenderer.enabled = true;
		if (character == null)
		{
			_imageRenderer.sprite = _characterAttribute.GetAttributeLevel(CharacterAttribute.CalculateTeamAttributeValue(_characterAttribute.Id)).TeamIcon;
			return;
		}
		CharacterAttributeValue attributeByObject = character.RuntimeData.GetAttributeByObject(characterAttribute);
		_imageRenderer.sprite = _characterAttribute.GetAttributeLevel(attributeByObject.Value).Icon;
	}

	public void SetCurrentChoiceType(EPlayerChoice choice, Sprite sprite = null)
	{
		Visible = true;
		Enabled = true;
		switch (choice)
		{
		case EPlayerChoice.ITEM:
			throw new UnityException("You should use SetCurrentChoice(Item item) method.");
		case EPlayerChoice.CHARACTER:
			throw new UnityException("You should use SetCurrentChoice(Character character) method.");
		case EPlayerChoice.ATTRIBUTE:
			throw new UnityException("You should use SetCurrentChoice(CharacterAttribute characterAttribute) method.");
		case EPlayerChoice.RESOURCE:
			throw new UnityException("You should use SetCurrentChoice(GameResource resource) method.");
		}
		_choiceType = choice;
		gameObject.SetActive(value: true);
		_imageRenderer.enabled = true;
		if (sprite != null)
		{
			_imageRenderer.sprite = sprite;
		}
	}

	public IItem GetItemValue()
	{
		if (_item == null)
		{
			throw new UnityException("This choice item is null. Are you sure this card is ITEM type?");
		}
		return _item;
	}

	public int GetCardId()
	{
		return _cardId;
	}

	public GameResource GetResourceValue()
	{
		if (_gameResource == null)
		{
			throw new UnityException("This choice resource is null. Are you sure this card is RESOURCE type?");
		}
		return _gameResource;
	}

	public Character GetCharacterValue()
	{
		if (_character == null)
		{
			throw new UnityException("This choice character is null. Are you sure this card is CHARACTER type?");
		}
		return _character;
	}

	public CharacterAttribute GetAttributeValue()
	{
		if (_characterAttribute == null)
		{
			throw new UnityException("This choice character is null. Are you sure this card is ATTRIBUTE type?");
		}
		return _characterAttribute;
	}

	public EPlayerChoice GetChoiceType()
	{
		return _choiceType;
	}

	public Sprite GetCurrentImage()
	{
		return _imageRenderer.sprite;
	}

	public void Hide(bool animate)
	{
		Visible = false;
		if (!animate && _animatorController != null)
		{
			_animatorController.Play("Hidden");
		}
		if (_animatorController != null)
		{
			_animatorController.SetBool("Visible", value: false);
		}
	}

	public void Show(bool animate)
	{
		gameObject.SetActive(value: true);
		Visible = true;
		if (!animate && _animatorController != null)
		{
			_animatorController.Play("Visible");
		}
		if (_animatorController != null)
		{
			_animatorController.SetBool("Visible", value: true);
		}
	}
}
