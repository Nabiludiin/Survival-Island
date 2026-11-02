using UnityEngine;
using UnityEngine.UI;

public class PrevColorButton : MonoBehaviour
{
	[HideInInspector]
	public SO_ColorValue primaryColor;

	[HideInInspector]
	public SO_ColorValue secondaryColor;

	[HideInInspector]
	public SO_Pattern pattern;

	[SerializeField]
	private GameObject visualParent;

	[SerializeField]
	private Image primaryImage;

	[SerializeField]
	private Image secondaryImage;

	[SerializeField]
	private Image doubleImage;

	[SerializeField]
	private Image patternImage;

	public void SetColors(SO_ColorValue primary, SO_ColorValue secondary, SO_Pattern pattern)
	{
		if (!visualParent.activeSelf)
		{
			visualParent.SetActiveSafe(value: true);
		}
		primaryColor = primary;
		secondaryColor = secondary;
		this.pattern = pattern;
		bool flag = primary != null && secondary != null && primary == secondary;
		primaryImage.enabled = primary != null && !flag;
		if (primaryImage.enabled)
		{
			primaryImage.color = ((primary.uniqueColorIndex == 0) ? Color.clear : primary.buttonColor);
		}
		secondaryImage.enabled = secondary != null && !flag;
		if (secondaryImage.enabled)
		{
			secondaryImage.color = ((secondary.uniqueColorIndex == 0) ? Color.clear : secondaryColor.buttonColor);
		}
		patternImage.enabled = pattern != null && !flag;
		if (patternImage.enabled)
		{
			patternImage.sprite = pattern.buttonSprite;
		}
		doubleImage.enabled = flag;
		if (doubleImage.enabled)
		{
			doubleImage.color = primary.buttonColor;
		}
	}
}
