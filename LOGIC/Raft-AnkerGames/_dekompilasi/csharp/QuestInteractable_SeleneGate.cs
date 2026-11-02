using UnityEngine;

public class QuestInteractable_SeleneGate : QuestEventBase, IRaycastable
{
	[SerializeField]
	protected string localizationTerm = string.Empty;

	private DisplayTextManager displayText;

	private void OnInspectorGUILocalizationTerm()
	{
		GUILayout.Label("Term: " + Helper.GetTerm(localizationTerm));
	}

	void IRaycastable.OnIsRayed()
	{
		if (displayText == null)
		{
			displayText = ComponentManager<DisplayTextManager>.Value;
		}
		if (!IsAtMaxStateIndex)
		{
			string term = Helper.GetTerm(localizationTerm);
			if (term.IsNullOrEmpty())
			{
				term = localizationTerm;
			}
			displayText.ShowText(term);
		}
	}

	void IRaycastable.OnRayEnter()
	{
	}

	void IRaycastable.OnRayExit()
	{
		if (displayText == null)
		{
			displayText = ComponentManager<DisplayTextManager>.Value;
		}
		displayText.HideDisplayTexts(0);
	}
}
