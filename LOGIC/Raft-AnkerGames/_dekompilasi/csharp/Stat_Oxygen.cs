using UltimateWater;
using UnityEngine;

public class Stat_Oxygen : Stat
{
	[SerializeField]
	private float oxygenLostPerSecond = 1f;

	[SerializeField]
	private PersonController personController;

	private void Update()
	{
		if (GameModeValueManager.IsPlayerInvurnerable)
		{
			if (Value != Max)
			{
				Value = Max;
			}
		}
		else
		{
			if (personController == null)
			{
				return;
			}
			if (personController.SubmersionState != SubmersionState.Full || personController.HasRaftAsParent || personController.airPillar)
			{
				if (Value != Max)
				{
					Value = Mathf.Lerp(Value, Max, Time.deltaTime);
					if (NormalValue > 0.99f)
					{
						Value = Max;
					}
				}
			}
			else
			{
				float num = ((Stat_WellBeing.Factor == WellBeing.Bad) ? (oxygenLostPerSecond * Stat_WellBeing.oxygenLostMultiplier) : oxygenLostPerSecond);
				Value -= num * Time.deltaTime;
			}
		}
	}

	public void SetOxygenLostPerSecond(float perSecond)
	{
		oxygenLostPerSecond = perSecond;
	}

	public float GetOxygenLostPerSecond()
	{
		return oxygenLostPerSecond;
	}
}
