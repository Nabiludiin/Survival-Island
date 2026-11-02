using System;
using System.Collections;
using RG_GameCamera.Config;
using UnityEngine;

namespace RG_GameCamera.CollisionSystem;

public class TargetCollision
{
	public class RayHitComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
		}
	}

	private Ray ray;

	private RaycastHit[] hits;

	private readonly RayHitComparer rayHitComparer;

	private readonly RG_GameCamera.Config.Config config;

	public TargetCollision(RG_GameCamera.Config.Config config)
	{
		rayHitComparer = new RayHitComparer();
		this.config = config;
	}

	public float CalculateTarget(Vector3 targetHead, Vector3 cameraTarget)
	{
		string ignoreTag = config.GetString("IgnoreCollisionTag");
		string transparentTag = config.GetString("TransparentCollisionTag");
		float num = config.GetFloat("TargetSphereRadius");
		float num2 = 1f;
		Vector3 normalized = (cameraTarget - targetHead).normalized;
		Ray ray = new Ray(targetHead, normalized);
		hits = Physics.RaycastAll(ray, normalized.magnitude + num);
		Array.Sort(hits, rayHitComparer);
		float num3 = float.PositiveInfinity;
		bool flag = false;
		RaycastHit[] array = hits;
		for (int i = 0; i < array.Length; i++)
		{
			RaycastHit raycastHit = array[i];
			ViewCollision.CollisionClass collisionClass = ViewCollision.GetCollisionClass(raycastHit.collider, ignoreTag, transparentTag);
			if (raycastHit.distance < num3 && collisionClass == ViewCollision.CollisionClass.Collision)
			{
				num3 = raycastHit.distance;
				num2 = raycastHit.distance - num;
				flag = true;
				Debug.DrawLine(targetHead, raycastHit.point, Color.yellow);
			}
		}
		if (flag)
		{
			return Mathf.Clamp01(num2 / (targetHead - cameraTarget).magnitude);
		}
		return 1f;
	}
}
