using System.Collections.Generic;
using UnityEngine;

namespace UltimateWater;

[AddComponentMenu("Ultimate Water/Dynamic/Water Force")]
public sealed class WaterForce : MonoBehaviour
{
	public struct Data
	{
		public Vector3 Position;

		public float Force;
	}

	[Tooltip("Force affecting water surface")]
	public float Force = 0.01f;

	[Tooltip("Area of water displacement")]
	public float Radius = 1f;

	private static readonly List<Data> _ForceData = new List<Data>(1);

	private void FixedUpdate()
	{
		Data item = default;
		item.Position = transform.position;
		item.Force = Force * Time.fixedDeltaTime;
		_ForceData.Clear();
		_ForceData.Add(item);
		WaterRipples.AddForce(_ForceData, Radius);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green * 0.8f + Color.gray * 0.2f;
		Gizmos.DrawLine(transform.position + Vector3.up * 2f, transform.position - Vector3.up * 2f);
		Gizmos.DrawLine(transform.position + (Vector3.forward + Vector3.left), transform.position - (Vector3.forward + Vector3.left));
		Gizmos.DrawLine(transform.position + (Vector3.forward - Vector3.left), transform.position - (Vector3.forward - Vector3.left));
	}
}
