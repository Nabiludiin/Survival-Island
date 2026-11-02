using System;
using System.Collections.Generic;
using UnityEngine;

public class AI_Snake : MonoBehaviour
{
	[Serializable]
	public class SnakePart
	{
		public Transform parent;

		public Transform transform;

		public bool position = true;

		public bool rotation = true;

		[HideInInspector]
		public Transform animated;

		[HideInInspector]
		public Vector3 originalOffset;

		[HideInInspector]
		public Quaternion previousRotation;
	}

	public float rotationLerpSpeed = 5f;

	public Transform animatedParent;

	[SerializeField]
	private List<SnakePart> parts = new List<SnakePart>();

	private GameObject partParent;

	private void Start()
	{
		for (int i = 0; i < parts.Count; i++)
		{
			SnakePart snakePart = parts[i];
			snakePart.animated = animatedParent.FindChildRecursively(snakePart.transform.name);
			if (snakePart.parent != null)
			{
				snakePart.originalOffset = snakePart.parent.InverseTransformPoint(snakePart.transform.position);
			}
			if (snakePart.position)
			{
				if (partParent == null)
				{
					partParent = new GameObject(transform.name + "-SnakeParent");
				}
				snakePart.transform.SetParent(partParent.transform);
			}
		}
	}

	private void LateUpdate()
	{
		for (int i = 0; i < parts.Count; i++)
		{
			SnakePart snakePart = parts[i];
			if (!snakePart.position && !snakePart.rotation)
			{
				snakePart.transform.localPosition = snakePart.animated.localPosition;
				snakePart.transform.localRotation = snakePart.animated.localRotation;
			}
			else if (snakePart.parent != null)
			{
				if (snakePart.position)
				{
					snakePart.transform.position = snakePart.parent.TransformPoint(snakePart.originalOffset);
				}
				if (snakePart.rotation)
				{
					Quaternion rotation = Quaternion.Lerp(snakePart.transform.rotation, snakePart.previousRotation, Time.deltaTime * rotationLerpSpeed);
					snakePart.transform.rotation = rotation;
					snakePart.previousRotation = snakePart.parent.rotation * snakePart.animated.localRotation;
				}
			}
		}
	}

	private void OnDestroy()
	{
		if (partParent != null)
		{
			UnityEngine.Object.Destroy(partParent);
		}
	}
}
