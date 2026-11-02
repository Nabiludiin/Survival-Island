using UnityEngine;
using UnityEngine.UI;

public class SelectedOnlyOnConsole : MonoBehaviour
{
	private Image selected;

	private SelectableMenuItem selectedMenuItem;

	private void Awake()
	{
		selected = GetComponent<Image>();
		selectedMenuItem = GetComponentInParent<SelectableMenuItem>();
	}

	private void Update()
	{
		if (selectedMenuItem != null && selectedMenuItem.IsKeepActive())
		{
			selected.enabled = true;
		}
		else
		{
			selected.enabled = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad;
		}
	}
}
