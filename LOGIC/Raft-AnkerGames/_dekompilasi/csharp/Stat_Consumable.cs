using UnityEngine;

public class Stat_Consumable : Stat_Target
{
	private bool _consuming;

	[Header("Consumable settings")]
	public bool autoHandle = true;

	[SerializeField]
	private float lostPerSecondDefault = 0.11f;

	[SerializeField]
	private float consumeTime = 20f;

	[HideInInspector]
	public float timer;

	[HideInInspector]
	public float startConsumeValue;

	[SerializeField]
	private YieldManipulator rawManipulator;

	[SerializeField]
	private YieldManipulator cookedManipulator;

	public bool Consuming
	{
		get
		{
			return _consuming;
		}
		set
		{
			_consuming = value;
		}
	}

	public float LostPerSecond => lostPerSecondDefault * GameModeValueManager.GetCurrentGameModeValue().nourishmentVariables.thirstDecrementRateMultiplier;

	private void Update()
	{
		if (!GameModeValueManager.IsPlayerInvurnerable && autoHandle)
		{
			HandleValue();
		}
	}

	private void OnValidate()
	{
		rawManipulator.finalYield = rawManipulator.baseYield * rawManipulator.GetValue(rawManipulator.cravePercent);
		cookedManipulator.finalYield = cookedManipulator.baseYield * cookedManipulator.GetValue(cookedManipulator.cravePercent);
	}

	public void HandleValue()
	{
		if (Consuming)
		{
			timer += Time.deltaTime / consumeTime;
			Value = Mathf.Lerp(startConsumeValue, statTarget.Value, timer);
			if (Value == statTarget.Value)
			{
				Consuming = false;
			}
		}
		else
		{
			float lostPerSecond = LostPerSecond;
			Value -= Time.deltaTime * lostPerSecond;
			statTarget.Value = Value;
		}
	}

	protected override void ChangeValue(float diff, float newValue)
	{
		base.ChangeValue(diff, newValue);
		if (diff < 0f)
		{
			statTarget.Value += diff;
			if (Consuming)
			{
				startConsumeValue += diff;
			}
		}
	}

	public void Consume(float yield)
	{
		if (yield > 0f)
		{
			Consuming = true;
			startConsumeValue = Value;
			timer = 0f;
			statTarget.Value += yield;
		}
		else
		{
			Value -= yield;
			statTarget.Value = Value;
		}
	}

	public void ConsumeWithManipulatorCurve(float yield, bool isRaw)
	{
		if (yield < 0f)
		{
			Value += yield;
			statTarget.Value = Value;
			return;
		}
		Consuming = true;
		startConsumeValue = Value;
		timer = 0f;
		float num = (isRaw ? rawManipulator.GetValue(statTarget.NormalValue) : cookedManipulator.GetValue(statTarget.NormalValue));
		statTarget.Value += yield * num;
	}
}
