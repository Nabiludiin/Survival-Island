using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
	public Action OnPickColor;

	public static SO_ColorValue[] Colors;

	[SerializeField]
	private ColorButton colorButtonObject;

	[SerializeField]
	private RectTransform[] colorLayerParents;

	[SerializeField]
	private int pieSliceCount = 8;

	[SerializeField]
	private List<ColorCollection> colorCollections = new List<ColorCollection>();

	[Header("Color buttons")]
	[SerializeField]
	private List<ColorButton> colorButtons = new List<ColorButton>();

	public SO_ColorValue SelectedColorValue { get; private set; }

	public ColorButton SelectedColorButton { get; private set; }

	private void Awake()
	{
		if (Colors == null || Colors.Length == 0)
		{
			Colors = Resources.LoadAll<SO_ColorValue>("Colors");
		}
		SelectedColorValue = Colors[0];
		SelectButtonFromColor(GetColorFromUniqueIndex(0u));
	}

	private void OnValidate()
	{
		while (colorCollections.Count > pieSliceCount)
		{
			colorCollections.RemoveAt(colorCollections.Count - 1);
		}
		while (colorCollections.Count < pieSliceCount)
		{
			colorCollections.Add(new ColorCollection());
		}
		for (int i = 0; i < colorCollections.Count; i++)
		{
			if (colorCollections[i].name == string.Empty)
			{
				colorCollections[i].name = "No name";
			}
			List<SO_ColorValue> colorValues = colorCollections[i].colorValues;
			while (colorValues.Count > colorLayerParents.Length)
			{
				colorValues.RemoveAt(colorValues.Count - 1);
			}
			while (colorValues.Count < colorLayerParents.Length)
			{
				colorValues.Add(new SO_ColorValue());
			}
		}
	}

	private void Odin_CreateButtons()
	{
		for (int i = 0; i < colorLayerParents.Length; i++)
		{
			RectTransform rectTransform = colorLayerParents[i];
			int childCount = rectTransform.childCount;
			for (int j = 0; j < childCount; j++)
			{
				UnityEngine.Object.DestroyImmediate(rectTransform.GetChild(0).gameObject);
			}
		}
		InitializeButtons();
		colorButtons.Clear();
		colorButtons = GetComponentsInChildren<ColorButton>(includeInactive: true).ToList();
	}

	public static SO_ColorValue GetColorFromUniqueIndex(uint uniqueColorIndex)
	{
		if (Colors == null || Colors.Length == 0)
		{
			Colors = Resources.LoadAll<SO_ColorValue>("Colors");
		}
		for (int i = 0; i < Colors.Length; i++)
		{
			if (Colors[i].uniqueColorIndex == uniqueColorIndex)
			{
				return Colors[i];
			}
		}
		return null;
	}

	public void OnButtonClick(ColorButton colorButton)
	{
		if (!(colorButton.colorValues == null))
		{
			if (SelectedColorButton != null && SelectedColorButton != colorButton)
			{
				SelectedColorButton.SetSelectedState(state: false);
			}
			SelectedColorButton = colorButton;
			SelectedColorValue = colorButton.colorValues;
			OnPickColor?.Invoke();
		}
	}

	public void SelectButtonFromColor(SO_ColorValue colorValue)
	{
		foreach (ColorButton colorButton in colorButtons)
		{
			if (colorButton != null && colorButton.colorValues == colorValue)
			{
				colorButton.AttachColorPicker(this);
				colorButton.OnPointerClick();
				break;
			}
		}
	}

	private void InitializeButtons()
	{
		for (int i = 0; i < colorLayerParents.Length; i++)
		{
			int j;
			for (j = 0; j < pieSliceCount; j++)
			{
				RectTransform parent = colorLayerParents[i];
				ColorButton colorButton = UnityEngine.Object.Instantiate(colorButtonObject, parent);
				Image component = colorButton.transform.GetComponent<Image>();
				component.fillAmount = 1f / (float)pieSliceCount;
				ColorCollection colorCollection = colorCollections.Find((ColorCollection c) => c.order == j);
				if (colorCollection.colorValues[i] != null)
				{
					component.color = colorCollection.colorValues[i].buttonColor;
				}
				Vector3 localEulerAngles = colorButton.transform.localEulerAngles;
				localEulerAngles.z = (float)j * (360f / (float)pieSliceCount);
				colorButton.transform.localEulerAngles = localEulerAngles;
				colorButton.colorValues = colorCollection.colorValues[i];
				colorButtons.Add(colorButton);
			}
		}
		foreach (ColorButton colorButton2 in colorButtons)
		{
			colorButton2.AttachColorPicker(this);
		}
	}
}
