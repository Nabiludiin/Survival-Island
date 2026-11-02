using UnityEngine;

public class TabButton_ColorScheme : TabButton
{
	[SerializeField]
	private CharacterBox characterBox;

	public override void OnPointerClick()
	{
		base.OnPointerClick();
		ApplyColorScheme();
	}

	public void ApplyColorScheme()
	{
		SO_Character characterFromIndex = CharacterManager.GetCharacterFromIndex(characterBox.lastValidModelIndex);
		if (characterFromIndex != null)
		{
			TabButton_Character tabButton_Character = characterBox.tabGroupCharacter.tabButtons[characterBox.lastValidModelIndex] as TabButton_Character;
			if (tabButton_Character != null && tabButton_Character.IsInteractable)
			{
				characterFromIndex.modelPrefab.outfits[tabIndex]?.ApplyOutfit(tabButton_Character.bodyMesh, null, tabButton_Character.hairMeshes);
			}
		}
	}
}
