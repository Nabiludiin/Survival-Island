using System.Collections.Generic;
using RG_GameCamera.Config;
using UnityEngine;

namespace RG_GameCamera.CollisionSystem;

public class VolumetricCollision : ViewCollision
{
	public class RayHitComparer : IComparer<RaycastHit>
	{
		public int Compare(RaycastHit x, RaycastHit y)
		{
			return x.distance.CompareTo(y.distance);
		}
	}

	private readonly List<RaycastHit> hits;

	private readonly Ray[] rays;

	private readonly RayHitComparer rayHitComparer;

	public VolumetricCollision(RG_GameCamera.Config.Config config)
		: base(config)
	{
		rayHitComparer = new RayHitComparer();
		hits = new List<RaycastHit>(40);
		rays = new Ray[10];
		for (int i = 0; i < rays.Length; i++)
		{
			rays[i] = new Ray(Vector3.zero, Vector3.zero);
		}
	}

	public override float Process(Vector3 cameraTarget, Vector3 dir, float distance)
	{
		float value = distance;
		float num = config.GetFloat("RaycastTolerance");
		float min = config.GetFloat("MinDistance");
		Vector2 vector = config.GetVector2("ConeRadius");
		float num2 = config.GetFloat("ConeSegments");
		Vector3 vector2 = cameraTarget - dir * distance;
		Vector3 vector3 = Vector3.Cross(dir, Vector3.up);
		Vector3 vector4 = Vector3.zero;
		for (int i = 0; (float)i < num2; i++)
		{
			Quaternion quaternion = Quaternion.AngleAxis((float)i / num2 * 360f, dir);
			Vector3 vector5 = cameraTarget + quaternion * (vector3 * vector.x);
			Vector3 vector6 = vector2 + quaternion * (vector3 * vector.y);
			vector4 = vector6 - vector5;
			rays[i].origin = vector5;
			rays[i].direction = vector6 - vector5;
		}
		float magnitude = vector4.magnitude;
		hits.Clear();
		Ray[] array = rays;
		foreach (Ray ray in array)
		{
			hits.AddRange(Physics.RaycastAll(ray, magnitude + num));
		}
		hits.Sort(rayHitComparer);
		float num3 = float.PositiveInfinity;
		string ignoreTag = config.GetString("IgnoreCollisionTag");
		string transparentTag = config.GetString("TransparentCollisionTag");
		foreach (RaycastHit hit in hits)
		{
			CollisionClass collisionClass = ViewCollision.GetCollisionClass(hit.collider, ignoreTag, transparentTag);
			if (hit.distance < num3 && collisionClass == CollisionClass.Collision)
			{
				num3 = hit.distance;
				value = hit.distance - num;
			}
			if (collisionClass == CollisionClass.IgnoreTransparent)
			{
				UpdateTransparency(hit.collider);
			}
		}
		return Mathf.Clamp(value, min, distance);
	}
}
