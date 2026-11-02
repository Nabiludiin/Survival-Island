using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
	public SO_ColorValue colorValues;

	[SerializeField]
	private Image invertImage;

	[SerializeField]
	private Button button;

	private ColorPicker colorPicker;

	private void OnDisable()
	{
		if (colorPicker != null && colorPicker.SelectedColorButton != this)
		{
			SetSelectedState(state: false);
		}
	}

	public void AttachColorPicker(ColorPicker colorPicker)
	{
		this.colorPicker = colorPicker;
	}

	public void OnPointerEnter()
	{
		if (colorPicker == null)
		{
			colorPicker = GetComponentInParent<ColorPicker>();
		}
		SetSelectedState(state: true);
	}

	public void OnPointerClick()
	{
		if (colorPicker == null)
		{
			colorPicker = GetComponentInParent<ColorPicker>();
		}
		colorPicker.OnButtonClick(this);
		SetSelectedState(state: true);
	}

	public void OnPointerExit()
	{
		if (colorPicker == null)
		{
			colorPicker = GetComponentInParent<ColorPicker>();
		}
		if (colorPicker.SelectedColorButton != this)
		{
			SetSelectedState(state: false);
		}
	}

	public void SetSelectedState(bool state)
	{
		if (!state)
		{
			invertImage.gameObject.SetActive(value: false);
			button.targetGraphic.color = colorValues.buttonColor;
		}
		else
		{
			invertImage.gameObject.SetActive(value: true);
			invertImage.color = colorValues.buttonColor;
			button.targetGraphic.color = Color.white;
		}
	}
}
