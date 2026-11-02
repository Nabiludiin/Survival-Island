using System;
using UnityEngine;

public class AI_Sub : MonoBehaviour
{
	[SerializeField]
	private Transform transformToFollow;

	[SerializeField]
	private Vector3[] offsetArray;

	[SerializeField]
	private float totalArrayTime = 1f;

	[SerializeField]
	private AnimationCurve moveSpeed;

	[SerializeField]
	private AnimationCurve rotateSpeed;

	[SerializeField]
	private bool clampY;

	[SerializeField]
	private float clampedYPos = -1f;

	private float offsetTime;

	private Vector3 localOffset = Vector3.back;

	private Vector3 targetPosition;

	private static Network_Water water;

	private void Start()
	{
		if (water == null)
		{
			water = ComponentManager<Network_Water>.Value;
		}
		transform.SetParent(null);
		offsetTime = UnityEngine.Random.Range(0f, totalArrayTime);
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Combine(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
	}

	private void Update()
	{
		if (transformToFollow == null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		else if (!(water == null) && !(transformToFollow == null))
		{
			float normalizedTime = (water.WaterTime + offsetTime) % totalArrayTime;
			UpdateTargetPositionToCorrectOffset(normalizedTime);
			float time = Vector3.Distance(transform.position, targetPosition);
			float num = moveSpeed.Evaluate(time);
			float num2 = rotateSpeed.Evaluate(time);
			transform.position += transform.forward * num * Time.deltaTime;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position, Vector3.up), num2 * Time.deltaTime);
		}
	}

	private void OnDestroy()
	{
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Remove(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
	}

	private void OnWorldShift(Vector3 shift)
	{
		transform.position -= shift;
	}

	private void UpdateTargetPositionToCorrectOffset(float normalizedTime)
	{
		float num = totalArrayTime / ((float)offsetArray.Length - 1f);
		int num2 = Mathf.CeilToInt(normalizedTime / num);
		if (num2 == 0)
		{
			num2 = 1;
		}
		int num3 = num2 - 1;
		float num4 = (float)num2 * num - (float)num3 * num;
		float t = (normalizedTime - (float)num3 * num) / num4;
		Vector3 a = offsetArray[num3];
		Vector3 b = offsetArray[num2];
		localOffset = Vector3.Lerp(a, b, t);
		targetPosition = transformToFollow.TransformPoint(localOffset);
		if (clampY && targetPosition.y > clampedYPos)
		{
			targetPosition.y = clampedYPos;
		}
	}
}
