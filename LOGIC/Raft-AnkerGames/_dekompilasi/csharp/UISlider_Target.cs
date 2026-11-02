using UnityEngine;
using UnityEngine.UI;

public class UISlider_Target : UISlider
{
	[SerializeField]
	private Slider targetSliderComponent;

	public void SetTargetValue(float sliderValue)
	{
		if (targetSliderComponent != null)
		{
			SetSliderValue(targetSliderComponent, sliderValue);
		}
	}
}
