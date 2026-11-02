using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBox : MenuBox
{
	[SerializeField]
	private InputField nameInputfield;

	public TabGroup tabGroupCharacter;

	[SerializeField]
	private TabGroup tabGroupColorScheme;

	[SerializeField]
	private List<int> charactersUnlockedFromBeginning;

	[HideInInspector]
	public int lastValidModelIndex;

	private Settings settings;

	private string newName;

	private int modelIndex;

	public void Start()
	{
		settings = ComponentManager<Settings>.Value;
	}

	public override void Open()
	{
		base.Open();
		if (settings == null)
		{
			settings = ComponentManager<Settings>.Value;
		}
		RGD_Settings current = settings.Current;
		nameInputfield.text = current.character.Name;
		modelIndex = current.character.ModelIndex;
		lastValidModelIndex = modelIndex;
		ButtonSound.Mute = true;
		tabGroupCharacter.SelectTab(lastValidModelIndex);
		List<int> allUnlockedCharacters = GetAllUnlockedCharacters();
		TabButton[] tabButtons = tabGroupCharacter.tabButtons;
		foreach (TabButton tabButton in tabButtons)
		{
			tabButton.SetInteractability(allUnlockedCharacters.Contains(tabButton.tabIndex));
		}
		RefreshOutfitSprites();
		if (tabGroupCharacter.SelectedTabButton != null && tabGroupCharacter.SelectedTabButton.IsInteractable)
		{
			tabGroupColorScheme.SelectTab(current.character.OutfitIndex);
			ApplyCurrentColorSchemeToCharacter();
		}
		ButtonSound.Mute = false;
	}

	public override void Close()
	{
		if (modelIndex != lastValidModelIndex)
		{
			modelIndex = lastValidModelIndex;
			if (tabGroupCharacter.SelectedTabButton != null)
			{
				tabGroupCharacter.SelectedTabButton.OnPointerExit(forceExit: true);
			}
			tabGroupCharacter.SelectTab(modelIndex);
		}
		if (settings == null)
		{
			settings = ComponentManager<Settings>.Value;
		}
		RGD_Settings current = settings.Current;
		current.character.Name = nameInputfield.text;
		current.character.Platform = 0;
		current.character.ModelIndex = modelIndex;
		current.character.OutfitIndex = ((tabGroupColorScheme.SelectedTabButton != null) ? tabGroupColorScheme.SelectedTabButton.tabIndex : 0);
		settings.Save(current);
		base.Close();
	}

	public void SelectCharacter(int index)
	{
		modelIndex = index;
		if (GetAllUnlockedCharacters().Contains(index))
		{
			lastValidModelIndex = index;
		}
		ApplyCurrentColorSchemeToCharacter();
		RefreshOutfitSprites();
	}

	private void FloatingInputDismissed(FloatingGamepadTextInputDismissed_t data)
	{
		Debug.LogError("CB FloatingGamepadTextInputDismissed_t");
		nameInputfield.DeactivateInputField();
	}

	public void OnInputNameChanged()
	{
		if (!Input.GetKeyDown(KeyCode.Escape))
		{
			newName = nameInputfield.text;
		}
	}

	public void OnInputNameComplete()
	{
		nameInputfield.text = newName;
	}

	private void ApplyCurrentColorSchemeToCharacter()
	{
		if (tabGroupColorScheme.SelectedTabButton != null && tabGroupColorScheme.SelectedTabButton is TabButton_ColorScheme)
		{
			(tabGroupColorScheme.SelectedTabButton as TabButton_ColorScheme).ApplyColorScheme();
		}
	}

	private void RefreshOutfitSprites()
	{
		SO_Character characterFromIndex = CharacterManager.GetCharacterFromIndex(lastValidModelIndex);
		if (!(characterFromIndex != null))
		{
			return;
		}
		for (int i = 0; i < tabGroupColorScheme.tabButtons.Length; i++)
		{
			TabButton tabButton = tabGroupColorScheme.tabButtons[i];
			if (tabButton == null || tabButton.button == null)
			{
				Debug.Log("Tab button is null in character box colorscheme");
			}
			else if (characterFromIndex.modelPrefab == null || characterFromIndex.modelPrefab.outfits == null || characterFromIndex.modelPrefab.outfits[i] == null)
			{
				Debug.Log("Can't find sprite from soCharacter in character box");
			}
			else
			{
				tabButton.button.image.sprite = characterFromIndex.modelPrefab.outfits[i].sprite;
			}
		}
	}

	private List<int> GetAllUnlockedCharacters()
	{
		List<int> unlockedCharacterIndexes = CharacterManager.GetUnlockedCharacterIndexes();
		unlockedCharacterIndexes.AddRangeUniqueOnly(charactersUnlockedFromBeginning);
		return unlockedCharacterIndexes;
	}
}
