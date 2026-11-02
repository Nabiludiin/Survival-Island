using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ColorCounter
{
	public Text text;

	public CanvasGroup canvasgroup;

	public Animator animator;

	[Range(0f, 1f)]
	public float disabledAlpha = 0.2f;

	public Color normalTextColor;

	public Color disabledTextColor;

	public void Select(int colorCost, int inventoryCount)
	{
		animator.SetBool("Selected", colorCost > 0);
		if (GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.unlimitedResources)
		{
			inventoryCount = colorCost;
		}
		if (colorCost == 0)
		{
			text.gameObject.SetActive(value: false);
			canvasgroup.alpha = disabledAlpha;
			return;
		}
		text.gameObject.SetActive(value: true);
		text.color = ((inventoryCount >= colorCost) ? normalTextColor : disabledTextColor);
		text.text = inventoryCount + "/" + colorCost;
		canvasgroup.alpha = 1f;
	}
}
