using System;
using UnityEngine;

public class DetailPlankLength : MonoBehaviour
{
	[Serializable]
	public class DetailLength
	{
		public Mesh mesh;

		public float modelDefaultScale = 1f;

		public float colliderLength = 1f;

		public Interval_Float lengthInterval;
	}

	[SerializeField]
	private Transform scalePivot;

	[SerializeField]
	private MeshFilter meshFilter;

	[SerializeField]
	private GizmoBox secondRequiredCollider;

	[SerializeField]
	private BoxCollider boxCollider;

	[SerializeField]
	private DetailLength[] lengths;

	public void SetLength(float length)
	{
		scalePivot.localScale = new Vector3(1f, 1f, length);
		if (secondRequiredCollider != null)
		{
			secondRequiredCollider.transform.localPosition = new Vector3(0f, 0f, length + secondRequiredCollider.size.z);
		}
		DetailLength[] array = lengths;
		foreach (DetailLength detailLength in array)
		{
			if (length >= detailLength.lengthInterval.minValue && length < detailLength.lengthInterval.maxValue)
			{
				meshFilter.sharedMesh = detailLength.mesh;
				meshFilter.transform.localScale = new Vector3(1f, 1f, detailLength.modelDefaultScale);
				if (boxCollider != null)
				{
					Vector3 center = boxCollider.center;
					center.z = (0f - detailLength.colliderLength) / 2f;
					boxCollider.center = center;
					Vector3 size = boxCollider.size;
					size.z = detailLength.colliderLength;
					boxCollider.size = size;
				}
				break;
			}
		}
	}

	public float GetLength()
	{
		return scalePivot.localScale.z;
	}
}
