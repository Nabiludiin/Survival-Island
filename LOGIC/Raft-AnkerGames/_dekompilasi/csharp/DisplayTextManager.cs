using UnityEngine;

public class DisplayTextManager : MonoBehaviour
{
	[SerializeField]
	private DisplayText[] displayTexts;

	[SerializeField]
	private GameObject displayTextBottomParent;

	private void Awake()
	{
		ComponentManager<DisplayTextManager>.Value = this;
		HideDisplayTexts();
	}

	private void Update()
	{
		if (displayTextBottomParent.activeSelf == Dialogue_UI.IsShowingDialogue)
		{
			displayTextBottomParent.SetActiveSafe(!Dialogue_UI.IsShowingDialogue);
		}
		bool flag = CanvasHelper.ActiveMenu != MenuType.None || GameManager.IsLeavingGame;
		DisplayText[] array = displayTexts;
		foreach (DisplayText displayText in array)
		{
			if (displayText.hide | flag)
			{
				displayText.HardHide();
			}
		}
	}

	public void ShowText(string text, int displayIndex = 0, bool clearAllTexts = true, int priority = 0)
	{
		if (clearAllTexts)
		{
			HideDisplayTexts();
		}
		displayTexts[displayIndex].Show(text, priority);
	}

	public void ShowText(string text, KeyCode key, int displayIndex = 0, int priority = 0, bool clearAllTexts = true)
	{
		if (clearAllTexts)
		{
			HideDisplayTexts();
		}
		displayTexts[displayIndex].Show(text, key, priority);
	}

	public void ShowText(string text, string binding, KeyCode key, int displayIndex = 0, int priority = 0, bool clearAllTexts = true)
	{
		if (clearAllTexts)
		{
			HideDisplayTexts();
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			displayTexts[displayIndex].ShowConsoleBinding(text, binding, priority);
		}
		else
		{
			displayTexts[displayIndex].Show(text, key, priority);
		}
	}

	public void ShowTextConsole(string text, string binding, int displayIndex = 0, int priority = 0, bool clearAllTexts = true)
	{
		if (clearAllTexts)
		{
			HideDisplayTexts();
		}
		displayTexts[displayIndex].ShowConsoleBinding(text, binding, priority);
	}

	public void ShowText(string text, string binding1, string binding2, KeyCode key, int displayIndex = 0, int priority = 0, bool clearAllTexts = true)
	{
		if (clearAllTexts)
		{
			HideDisplayTexts();
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			displayTexts[displayIndex].ShowConsoleBinding2(text, binding1, binding2, priority);
		}
		else
		{
			displayTexts[displayIndex].Show(text, key, priority);
		}
	}

	public void ShowTextConsole(string text, string binding1, string binding2, int displayIndex = 0, int priority = 0, bool clearAllTexts = true)
	{
		if (clearAllTexts)
		{
			HideDisplayTexts();
		}
		displayTexts[displayIndex].ShowConsoleBinding2(text, binding1, binding2, priority);
	}

	public void HideDisplayTexts()
	{
		DisplayText[] array = displayTexts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Hide();
		}
	}

	public void HideDisplayTexts(int displayIndex)
	{
		displayTexts[displayIndex].Hide();
	}
}
