using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ColorBar : MonoBehaviour
{
	[SerializeField]
	private Image image;

	[SerializeField]
	private Text costText;

	[SerializeField]
	private float barHeight;

	public void Set(float value, int amountOfColor, int costAmount)
	{
		costText.text = costAmount + "/" + amountOfColor;
		StopAllCoroutines();
		StartCoroutine(FillBar(value));
	}

	private IEnumerator FillBar(float value)
	{
		float distanceToTarget = Mathf.Abs(image.fillAmount - value);
		while (distanceToTarget > 0.01f)
		{
			image.fillAmount = Mathf.Lerp(image.fillAmount, value, Time.deltaTime * 7.5f);
			distanceToTarget = Mathf.Abs(image.fillAmount - value);
			Vector3 localPosition = costText.rectTransform.localPosition;
			localPosition.y = barHeight * image.fillAmount - barHeight / 2f;
			costText.rectTransform.localPosition = localPosition;
			yield return null;
		}
		image.fillAmount = value;
	}
}
