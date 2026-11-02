using UnityEngine;
using UnityEngine.AzureSky;

public class Clock : MonoBehaviour
{
	[SerializeField]
	private Transform pointer;

	private static AzureSkyController skyController;

	private void Start()
	{
		if (skyController == null)
		{
			skyController = ComponentManager<AzureSkyController>.Value;
		}
	}

	private void Update()
	{
		Vector3 localEulerAngles = pointer.localEulerAngles;
		localEulerAngles.z = GetTimeRotation();
		pointer.localEulerAngles = localEulerAngles;
	}

	private float GetTimeRotation()
	{
		if (skyController == null)
		{
			return 0f;
		}
		float num = skyController.timeOfDay.hour;
		if (num > 12f)
		{
			num -= 12f;
		}
		return num * 30f;
	}
}
