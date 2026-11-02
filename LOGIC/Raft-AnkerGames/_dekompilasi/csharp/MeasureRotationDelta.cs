using System.Collections.Generic;
using UnityEngine;

public class MeasureRotationDelta : MonoBehaviour
{
	[SerializeField]
	private Vector3 normalizedRotationDelta;

	[SerializeField]
	private Vector3 rotationDelta;

	[SerializeField]
	private bool invertX;

	[SerializeField]
	private bool invertY;

	[SerializeField]
	private bool invertZ;

	[SerializeField]
	private float maxRotationDelta = 1f;

	[SerializeField]
	private float minimumRotationDelta = 0.25f;

	[SerializeField]
	private Transform trackedTransform;

	private List<Vector3> previousRotations = new List<Vector3>();

	[SerializeField]
	private int savedRotationCount = 3;

	public Vector3 NormalizedRotationDelta
	{
		get
		{
			return normalizedRotationDelta;
		}
		private set
		{
			normalizedRotationDelta = value;
		}
	}

	public Vector3 RotationDelta => rotationDelta;

	private Vector3 GetAverageRotationVector()
	{
		Vector3 zero = Vector3.zero;
		if (previousRotations.Count == 0)
		{
			return trackedTransform.eulerAngles;
		}
		if (previousRotations.Count == 1)
		{
			return previousRotations[0];
		}
		for (int num = previousRotations.Count - 1; num >= 0; num--)
		{
			zero += previousRotations[num];
		}
		return zero / previousRotations.Count;
	}

	public void SetTrackedTransform(Transform p_trans)
	{
		trackedTransform = p_trans;
	}

	private void OnValidate()
	{
		if (trackedTransform == null)
		{
			trackedTransform = transform;
		}
	}

	protected virtual void LateUpdate()
	{
		if (trackedTransform == null)
		{
			return;
		}
		rotationDelta = trackedTransform.eulerAngles - GetAverageRotationVector();
		if (invertX)
		{
			rotationDelta.x *= -1f;
		}
		if (invertY)
		{
			rotationDelta.y *= -1f;
		}
		if (invertZ)
		{
			rotationDelta.z *= -1f;
		}
		float num = Mathf.Abs(rotationDelta.x);
		float num2 = Mathf.Abs(rotationDelta.y);
		float num3 = Mathf.Abs(rotationDelta.z);
		if (num < minimumRotationDelta)
		{
			num = (rotationDelta.x = 0f);
		}
		if (num2 < minimumRotationDelta)
		{
			num2 = (rotationDelta.y = 0f);
		}
		if (num3 < minimumRotationDelta)
		{
			num3 = (rotationDelta.z = 0f);
		}
		NormalizedRotationDelta = new Vector3(Mathf.Clamp01(num / maxRotationDelta), Mathf.Clamp01(num2 / maxRotationDelta), Mathf.Clamp01(num3 / maxRotationDelta));
		if (rotationDelta.x < 0f)
		{
			normalizedRotationDelta.x *= -1f;
		}
		if (rotationDelta.y < 0f)
		{
			normalizedRotationDelta.y *= -1f;
		}
		if (rotationDelta.z < 0f)
		{
			normalizedRotationDelta.z *= -1f;
		}
		previousRotations.Add(trackedTransform.eulerAngles);
		for (int num4 = previousRotations.Count - 1; num4 >= 0; num4--)
		{
			if (previousRotations.Count > savedRotationCount)
			{
				previousRotations.RemoveAt(0);
			}
		}
	}
}
