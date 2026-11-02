using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.UI;

public class AzureUIController : MonoBehaviour
{
	public AzureSkyController skyController;

	public Slider slider;

	public Image transitionBar;

	private Vector3 m_scale;

	private void Update()
	{
		skyController.timeOfDay.hour = slider.value;
		if ((bool)transitionBar)
		{
			m_scale = new Vector3(skyController.weatherTransitionTime, 1f, 1f);
			transitionBar.rectTransform.localScale = m_scale;
		}
	}
}
