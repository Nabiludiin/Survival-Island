using System;
using System.Collections.Generic;
using UnityEngine;

public class GabArchiveUnusedFunctions : MonoBehaviour
{
	[Serializable]
	public class RotationLimitHandler
	{
		public List<RotationLimit> rotationLimits = new List<RotationLimit>();

		public float ApplyLimitations(Axis rotationAxis, float angle)
		{
			foreach (RotationLimit rotationLimit in rotationLimits)
			{
				if (rotationLimit.rotationAxis == rotationAxis)
				{
					return rotationLimit.ClampAngle(angle);
				}
			}
			return angle;
		}

		public RotationLimit GetRotationLimit(Axis rotAxis)
		{
			foreach (RotationLimit rotationLimit in rotationLimits)
			{
				if (rotationLimit.rotationAxis == rotAxis)
				{
					return rotationLimit;
				}
			}
			return null;
		}
	}

	[Serializable]
	public class RotationLimit
	{
		public Axis rotationAxis;

		public float minRotation;

		public float maxRotation;

		public float ClampAngle(float angle)
		{
			return Mathf.Clamp(angle, minRotation, maxRotation);
		}
	}
}
