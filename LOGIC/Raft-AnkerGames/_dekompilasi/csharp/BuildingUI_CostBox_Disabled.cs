using UnityEngine;
using UnityEngine.UI;

public class BuildingUI_CostBox_Disabled : BuildingUI_CostBox
{
	[SerializeField]
	private Image backgroundImage;

	[SerializeField]
	private Sprite normalSprite;

	[SerializeField]
	private Sprite disabledSprite;

	[SerializeField]
	private CanvasGroup canvasGroup;

	public override void Refresh()
	{
		base.Refresh();
		if (MeetsRequirements())
		{
			SetEnabled();
		}
		else
		{
			SetDisabled();
		}
	}

	public void SetDisabled()
	{
		if (backgroundImage != null)
		{
			backgroundImage.sprite = disabledSprite;
			Color color = backgroundImage.color;
			color.a = 0.2f;
			backgroundImage.color = color;
		}
		if (canvasGroup != null)
		{
			canvasGroup.alpha = 0.5f;
		}
	}

	public void SetEnabled()
	{
		if (backgroundImage != null)
		{
			backgroundImage.sprite = normalSprite;
			Color color = backgroundImage.color;
			color.a = 1f;
			backgroundImage.color = color;
		}
		if (canvasGroup != null)
		{
			canvasGroup.alpha = 1f;
		}
	}
}
