using System.Collections.Generic;

public class TabGroup_Characters : TabGroup
{
	private void Start()
	{
		List<SO_Character> sO_Characters = CharacterManager.SO_Characters;
		TabButton[] array = tabButtons;
		foreach (TabButton tabButton in array)
		{
			foreach (SO_Character item in sO_Characters)
			{
				if (item.index == tabButton.tabIndex)
				{
					tabButton.text.text = item.displayName;
					break;
				}
			}
		}
	}
}
