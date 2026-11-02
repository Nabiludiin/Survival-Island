using System;
using UnityEngine;

public class Tank_Gauge : MonoBehaviour
{
	public Tank connectedTank;

	public Transform fuelGaugeMeter;

	private float fuelGaugeTargetAngle = 122f;

	private float fuelGaugeCurrentAngle;

	[SerializeField]
	private float pointerMinAngle = 270f;

	[SerializeField]
	private float pointerMaxAngle;

	[SerializeField]
	private int previouslySentFuelGaugeValue;

	[SerializeField]
	private Axis rotationAxis = Axis.Z;

	public Action OnGaugeUpdateAction;

	private Vector3 startAngle;

	private void OnValidate()
	{
		if (connectedTank == null)
		{
			connectedTank = GetComponentInChildren<Tank>();
		}
	}

	private void Awake()
	{
		startAngle = fuelGaugeMeter.localEulerAngles;
		fuelGaugeCurrentAngle = pointerMinAngle;
		fuelGaugeTargetAngle = fuelGaugeCurrentAngle;
		Tank tank = connectedTank;
		tank.OnTankAmountChange = (Action)Delegate.Combine(tank.OnTankAmountChange, new Action(UpdateFuelGauge));
	}

	private void LateUpdate()
	{
		LerpGauge();
	}

	private void LerpGauge()
	{
		if (!(fuelGaugeMeter == null) && Mathf.Abs(fuelGaugeCurrentAngle - fuelGaugeTargetAngle) > 0.1f)
		{
			fuelGaugeCurrentAngle = Mathf.Lerp(fuelGaugeCurrentAngle, fuelGaugeTargetAngle, Time.deltaTime * 3f);
			switch (rotationAxis)
			{
			case Axis.X:
				fuelGaugeMeter.localEulerAngles = new Vector3(fuelGaugeCurrentAngle, startAngle.y, startAngle.z);
				break;
			case Axis.Y:
				fuelGaugeMeter.localEulerAngles = new Vector3(startAngle.x, fuelGaugeCurrentAngle, startAngle.z);
				break;
			case Axis.Z:
				fuelGaugeMeter.localEulerAngles = new Vector3(startAngle.x, startAngle.y, fuelGaugeCurrentAngle);
				break;
			}
		}
	}

	private void UpdateFuelGauge()
	{
		SetGaugeTargetAngle(connectedTank.NormalizedTankAmount);
		if (IsCurrentGaugeValueDifferentFromPreviousSentValue(connectedTank.CurrentTankAmount))
		{
			previouslySentFuelGaugeValue = (int)Math.Round(connectedTank.CurrentTankAmount, 0);
			if (Raft_Network.IsHost && OnGaugeUpdateAction != null)
			{
				OnGaugeUpdateAction();
			}
		}
	}

	public bool IsCurrentGaugeValueDifferentFromPreviousSentValue(float currentGaugeValue)
	{
		int num = (int)Math.Round(currentGaugeValue, 0);
		if (previouslySentFuelGaugeValue != num)
		{
			return true;
		}
		return false;
	}

	public void SetGaugeTargetAngle(float toValue)
	{
		fuelGaugeTargetAngle = Helper.MapValue(0f, 1f, toValue, pointerMinAngle, pointerMaxAngle);
	}
}
