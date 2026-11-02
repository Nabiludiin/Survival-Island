using UnityEngine;

public class ExclamationNotification : MonoBehaviour
{
	[SerializeField]
	private float maxSizeDistance = 1f;

	[SerializeField]
	private float minSizeDistance = 3f;

	[SerializeField]
	private float lerpSpeed = 5f;

	private Transform cameraTransform;

	private Vector3 localScaleStart;

	private void Awake()
	{
		localScaleStart = transform.localScale;
	}

	private void Update()
	{
		if (cameraTransform == null)
		{
			Network_Player value = ComponentManager<Network_Player>.Value;
			if (value != null)
			{
				cameraTransform = value.CameraTransform;
			}
			return;
		}
		float num = Vector3.Distance(transform.position, cameraTransform.position);
		float num2 = 1f;
		if (num > minSizeDistance)
		{
			num2 = 0f;
		}
		else
		{
			num2 = ((!(num < maxSizeDistance)) ? Helper.NormalizeValue(num, minSizeDistance, maxSizeDistance) : 1f);
		}
		Vector3 vector = localScaleStart * num2;
		if (lerpSpeed < 0f)
		{
			transform.localScale = vector;
		}
		else
		{
			transform.localScale = Vector3.Lerp(transform.localScale, vector, Time.deltaTime * lerpSpeed);
		}
	}
}
