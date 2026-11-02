using System;
using System.Collections;
using RG_GameCamera.Config;
using UnityEngine;

namespace RG_GameCamera.CollisionSystem;

public class SphericalCollision : ViewCollision
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

	public SphericalCollision(RG_GameCamera.Config.Config config)
		: base(config)
	{
		rayHitComparer = new RayHitComparer();
	}

	public override float Process(Vector3 cameraTarget, Vector3 dir, float distance)
	{
		float value = distance;
		float min = config.GetFloat("MinDistance");
		float num = config.GetFloat("SphereCastTolerance");
		float num2 = config.GetFloat("SphereCastRadius");
		ray.origin = cameraTarget + dir * num2;
		ray.direction = -dir;
		Collider[] array = Physics.OverlapSphere(ray.origin, num2);
		bool flag = false;
		string ignoreTag = config.GetString("IgnoreCollisionTag");
		string transparentTag = config.GetString("TransparentCollisionTag");
		for (int i = 0; i < array.Length; i++)
		{
			if (ViewCollision.GetCollisionClass(array[i], ignoreTag, transparentTag) == CollisionClass.Collision)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			ray.origin += dir * num2;
			hits = Physics.RaycastAll(ray, distance - num2 + num);
		}
		else
		{
			hits = Physics.SphereCastAll(ray, num2, distance + num2);
		}
		Array.Sort(hits, rayHitComparer);
		float num3 = float.PositiveInfinity;
		RaycastHit[] array2 = hits;
		for (int j = 0; j < array2.Length; j++)
		{
			RaycastHit raycastHit = array2[j];
			CollisionClass collisionClass = ViewCollision.GetCollisionClass(raycastHit.collider, ignoreTag, transparentTag);
			if (raycastHit.distance < num3 && collisionClass == CollisionClass.Collision)
			{
				num3 = raycastHit.distance;
				value = raycastHit.distance - num;
			}
			if (collisionClass == CollisionClass.IgnoreTransparent)
			{
				UpdateTransparency(raycastHit.collider);
			}
		}
		return Mathf.Clamp(value, min, distance);
	}
}
