using UnityEngine;
using UnityEngine.UI;

public class SliderFillHelper : MonoBehaviour
{
	private void Awake()
	{
		Slider slider = GetComponent<Slider>();
		if (slider != null)
		{
			slider.onValueChanged.AddListener((float value) =>
			{
				UpdateSlider(slider);
			});
			UpdateSlider(slider);
		}
	}

	private void UpdateSlider(Slider slider)
	{
		if (slider != null)
		{
			slider.fillRect.gameObject.SetActiveSafe(slider.normalizedValue > 0f);
		}
	}
}
