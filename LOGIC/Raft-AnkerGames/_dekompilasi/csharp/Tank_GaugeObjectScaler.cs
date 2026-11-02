using System;
using UnityEngine;

public class Tank_GaugeObjectScaler : MonoBehaviour
{
	[SerializeField]
	private Tank tank;

	[SerializeField]
	private ObjectScaler objectScaler;

	private void Awake()
	{
		if (this.tank != null && objectScaler != null)
		{
			Tank tank = this.tank;
			tank.OnTankAmountChange = (Action)Delegate.Combine(tank.OnTankAmountChange, new Action(OnTankAmountChanged));
		}
	}

	private void OnDestroy()
	{
		if (this.tank != null && objectScaler != null)
		{
			Tank tank = this.tank;
			tank.OnTankAmountChange = (Action)Delegate.Remove(tank.OnTankAmountChange, new Action(OnTankAmountChanged));
		}
	}

	private void OnTankAmountChanged()
	{
		objectScaler.SetNormalizedValue(tank.NormalizedTankAmount, instant: false);
	}
}
