using UnityEngine;

public class Stat : MonoBehaviour
{
	[SerializeField]
	private float max;

	[SerializeField]
	private float value;

	[SerializeField]
	private float maxValue = 100f;

	[SerializeField]
	private float startValue = 100f;

	public float Max => max;

	public float Value
	{
		get
		{
			return value;
		}
		set
		{
			float diff = value - this.value;
			ChangeValue(diff, value);
		}
	}

	public float NormalValue
	{
		get
		{
			return Value / Max;
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 1f);
			Value = Max * value;
		}
	}

	public bool MeetsWellBeing => NormalValue >= Stat_WellBeing.WellBeingLimit;

	public bool IsZero => NormalValue <= 0f;

	protected virtual void Awake()
	{
		SetMaxValue(maxValue);
		Value = startValue;
	}

	public void SetToMaxValue()
	{
		Value = Max;
	}

	public void SetMaxValue(float newMax)
	{
		max = newMax;
	}

	protected virtual void ChangeValue(float diff, float newValue)
	{
		value = Mathf.Clamp(newValue, 0f, Max);
	}
}
