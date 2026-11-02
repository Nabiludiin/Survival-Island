using UnityEngine;

public class Gauge : MonoBehaviour
{
	[SerializeField]
	private Transform fuelGaugeMeter;

	[SerializeField]
	private float pointerMinAngle = 270f;

	[SerializeField]
	private float pointerMaxAngle;

	[SerializeField]
	private Axis rotationAxis = Axis.Z;

	private float fuelGaugeTargetAngle = 122f;

	private float fuelGaugeCurrentAngle;

	private Vector3 startAngle;

	private void Awake()
	{
		startAngle = fuelGaugeMeter.localEulerAngles;
		fuelGaugeCurrentAngle = pointerMinAngle;
		fuelGaugeTargetAngle = fuelGaugeCurrentAngle;
	}

	private void LateUpdate()
	{
		LerpGauge();
	}

	public void MapToGaugeTargetAngle(float toValue)
	{
		SetTargetAngle(Helper.MapValue(0f, 1f, toValue, pointerMinAngle, pointerMaxAngle));
	}

	public void SetTargetAngle(float angle)
	{
		fuelGaugeTargetAngle = angle;
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
}
