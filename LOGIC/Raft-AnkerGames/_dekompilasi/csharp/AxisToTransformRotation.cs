using System;
using System.Collections.Generic;
using UnityEngine;

public class AxisToTransformRotation : MonoBehaviour
{
	[Serializable]
	public class AxisToRotationData
	{
		public Axis axis = Axis.None;

		public Vector3 targetRotation = Vector3.zero;

		public bool IsAxis(Axis axis)
		{
			return this.axis == axis;
		}
	}

	public List<AxisToRotationData> axiesToRotations;

	public Vector3 GetAxisRotation(out bool success, Axis axis)
	{
		AxisToRotationData axisRotationData = GetAxisRotationData(axis);
		success = axisRotationData != null;
		return axisRotationData?.targetRotation ?? Vector3.zero;
	}

	public AxisToRotationData GetAxisRotationData(Axis axis)
	{
		foreach (AxisToRotationData axiesToRotation in axiesToRotations)
		{
			if (axiesToRotation.IsAxis(axis))
			{
				return axiesToRotation;
			}
		}
		return null;
	}
}
