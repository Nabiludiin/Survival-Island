using UnityEngine;

public class RaycastInteractable_DisplayText : RaycastInteractable, IRaycastable
{
	[SerializeField]
	public string localizationTerm = string.Empty;

	private static DisplayTextManager displayTextManager;

	protected bool isDisplayingText;

	void IRaycastable.OnIsRayed()
	{
		OnIsRayed();
	}

	void IRaycastable.OnRayEnter()
	{
		OnRayEnter();
	}

	void IRaycastable.OnRayExit()
	{
		OnRayExit();
	}

	private void OnInspectorGUILocalizationTerm()
	{
		GUILayout.Label("Term: " + Helper.GetTerm(localizationTerm));
	}

	protected virtual void OnIsRayed()
	{
		if (displayTextManager != null)
		{
			displayTextManager.ShowText(Helper.GetTerm(localizationTerm));
			isDisplayingText = true;
		}
	}

	protected virtual void OnRayEnter()
	{
		if (displayTextManager == null)
		{
			displayTextManager = ComponentManager<DisplayTextManager>.Value;
		}
	}

	protected virtual void OnRayExit()
	{
		HideDisplayText();
	}

	protected void HideDisplayText()
	{
		if (displayTextManager != null)
		{
			displayTextManager.HideDisplayTexts(0);
			isDisplayingText = false;
		}
	}
}
