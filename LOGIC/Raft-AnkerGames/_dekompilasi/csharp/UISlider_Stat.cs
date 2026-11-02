using UnityEngine;
using UnityEngine.UI;

public class UISlider_Stat : UISlider_Target
{
	public RectTransform sliderTransform;

	[SerializeField]
	private Image blinkImage;

	[SerializeField]
	private Image sliderFill;

	[SerializeField]
	private Image targetSliderFill;

	[SerializeField]
	private float sineHeight = 1f;

	[SerializeField]
	private float sineFrequency = 1f;

	[SerializeField]
	private float blinkFadeSpeed = 0.5f;

	private MaterialPropertyBlock blinkMaterialBlock;

	private float blinkAmount;

	protected override void Awake()
	{
		base.Awake();
		enabled = true;
	}

	protected override void Update()
	{
		base.Update();
		if (blinkImage == null)
		{
			enabled = false;
		}
		else if (NormalizedValue < Stat_WellBeing.WellBeingLimit)
		{
			if (blinkAmount != 1f)
			{
				blinkAmount = Mathf.MoveTowards(blinkAmount, 1f, Time.deltaTime * blinkFadeSpeed);
				if (blinkAmount > 0.95f)
				{
					blinkAmount = 1f;
				}
			}
			HandleBlinking();
		}
		else if (blinkAmount > 0f)
		{
			blinkAmount = Mathf.MoveTowards(blinkAmount, 0f, Time.deltaTime * blinkFadeSpeed * 2f);
			if (blinkAmount < 0.05f)
			{
				blinkAmount = 0f;
				blinkImage.color = new Color(1f, 1f, 1f, 0f);
				SetSliderAlpha(1f);
			}
			else
			{
				HandleBlinking();
			}
		}
	}

	private void HandleBlinking()
	{
		float value = Mathf.Sin(Time.timeSinceLevelLoad * sineFrequency) * sineHeight;
		float alpha = Helper.MapValue(-1f, 1f, value, 0f, 1f) * blinkAmount;
		SetAlpha(alpha);
	}

	private void SetAlpha(float alpha)
	{
		blinkImage.color = new Color(1f, 1f, 1f, alpha);
		SetSliderAlpha(alpha);
	}

	private void SetSliderAlpha(float alpha)
	{
		if (sliderFill != null)
		{
			Color color = sliderFill.color;
			color.a = alpha;
			sliderFill.color = color;
		}
		if (targetSliderFill != null)
		{
			Color color2 = targetSliderFill.color;
			color2.a = alpha;
			targetSliderFill.color = color2;
		}
	}
}
