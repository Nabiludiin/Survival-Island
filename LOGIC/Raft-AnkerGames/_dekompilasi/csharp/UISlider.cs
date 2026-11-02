using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour
{
	[SerializeField]
	private bool updateText;

	[SerializeField]
	private bool showPercent;

	[SerializeField]
	private int decimalCount;

	[SerializeField]
	protected Slider sliderComponent;

	[SerializeField]
	private Text sliderTextComponent;

	private string format;

	public Slider Slider => sliderComponent;

	public float NormalizedValue => sliderComponent.normalizedValue;

	public float Value => sliderComponent.value;

	public float MaxValue => sliderComponent.maxValue;

	public Slider.SliderEvent SliderEvent => sliderComponent.onValueChanged;

	protected virtual void Awake()
	{
		format = "F" + decimalCount;
		if (!updateText || sliderTextComponent == null)
		{
			enabled = false;
		}
	}

	protected virtual void OnEnable()
	{
		if (updateText && sliderTextComponent != null)
		{
			enabled = true;
		}
	}

	protected virtual void Update()
	{
		if (!gameObject.activeInHierarchy)
		{
			enabled = false;
		}
		SetText(sliderTextComponent, showPercent ? ((NormalizedValue * 100f).ToString(format) + "%") : Value.ToString(format));
	}

	public void SetValue(float sliderValue, string sliderText = "")
	{
		SetSliderValue(sliderComponent, sliderValue, sliderTextComponent, sliderText);
	}

	public void SetValue(float sliderValue)
	{
		SetSliderValue(sliderComponent, sliderValue);
	}

	protected void SetSliderValue(Slider sliderComponent, float sliderValue, Text textComponent = null, string sliderText = "")
	{
		if (!(sliderComponent == null) && sliderComponent.value != sliderValue)
		{
			sliderComponent.value = sliderValue;
			SetText(textComponent, sliderText);
		}
	}

	private void SetText(Text textComponent, string text)
	{
		if (textComponent != null && !string.IsNullOrEmpty(text) && !text.Equals(textComponent.text))
		{
			textComponent.text = text;
		}
	}
}
