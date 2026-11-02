using System;

namespace UltimateWater;

public static class WaterEvents
{
	public enum GlobalEventType
	{
		OnQualityChanged
	}

	public static void AddListener(Action action, GlobalEventType type)
	{
		if (type == GlobalEventType.OnQualityChanged && !(WaterQualitySettings.Instance == null))
		{
			WaterQualitySettings.Instance.Changed -= action;
			WaterQualitySettings.Instance.Changed += action;
		}
	}

	public static void RemoveListener(Action action, GlobalEventType type)
	{
		if (type == GlobalEventType.OnQualityChanged && !(WaterQualitySettings.Instance == null))
		{
			WaterQualitySettings.Instance.Changed -= action;
		}
	}
}
