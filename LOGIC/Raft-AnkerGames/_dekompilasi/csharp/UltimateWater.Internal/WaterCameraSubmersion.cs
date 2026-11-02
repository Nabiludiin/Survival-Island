using System;
using System.Collections.Generic;
using UnityEngine;

namespace UltimateWater.Internal;

[Serializable]
public class WaterCameraSubmersion
{
	[Range(0f, 2f)]
	[SerializeField]
	private float _Radius = 1f;

	[SerializeField]
	private int _Subdivisions;

	private WaterCamera _Camera;

	private List<WaterSample> _Samples;

	private readonly List<Vector3> _Points = new List<Vector3>();

	public SubmersionState State
	{
		get
		{
			Water containingWater = _Camera._ContainingWater;
			if (containingWater == null)
			{
				return SubmersionState.None;
			}
			if (!containingWater.Volume.Boundless)
			{
				return SubmersionState.Partial;
			}
			return Evaluate();
		}
	}

	public void OnEnable(WaterCamera camera)
	{
		_Camera = camera;
	}

	public void OnDisable()
	{
	}

	public void OnDrawGizmos()
	{
		Camera cameraComponent = _Camera.CameraComponent;
		float num = CalculateNearPlaneHeight(cameraComponent);
		CreatePlanePoints(cameraComponent, _Subdivisions, _Points);
		Gizmos.color = new Color(0f, 1f, 1f, 0.2f);
		for (int i = 0; i < _Points.Count; i++)
		{
			Gizmos.DrawSphere(_Points[i], num * _Radius);
		}
	}

	public void Create()
	{
		if (_Samples != null)
		{
			Destroy();
		}
		Water containingWater = _Camera._ContainingWater;
		if (containingWater != null)
		{
			Camera cameraComponent = _Camera.CameraComponent;
			_Samples = new List<WaterSample>();
			CreatePlanePoints(cameraComponent, _Subdivisions, _Points);
			for (int i = 0; i < _Points.Count; i++)
			{
				WaterSample waterSample = new WaterSample(containingWater, WaterSample.DisplacementMode.Height, 0.4f);
				waterSample.Start(_Points[i]);
				_Samples.Add(waterSample);
			}
		}
	}

	public void Destroy()
	{
		if (_Samples != null)
		{
			foreach (WaterSample sample in _Samples)
			{
				sample.Stop();
			}
			_Samples.Clear();
			_Samples = null;
		}
		_Points.Clear();
	}

	public void OnValidate()
	{
		if (_Subdivisions < 0)
		{
			_Subdivisions = 0;
		}
		if (_Radius < 0f)
		{
			_Radius = 0f;
		}
	}

	private SubmersionState Evaluate()
	{
		int num = 0;
		float num2 = CalculateNearPlaneHeight(_Camera.CameraComponent) * _Radius;
		CreatePlanePoints(_Camera.CameraComponent, _Subdivisions, _Points);
		for (int i = 0; i < _Samples.Count; i++)
		{
			Vector3 origin = _Points[i];
			Vector3 andReset = _Samples[i].GetAndReset(origin);
			if (origin.y + num2 <= andReset.y)
			{
				num++;
			}
			if (origin.y + num2 >= andReset.y && origin.y - num2 <= andReset.y)
			{
				return SubmersionState.Partial;
			}
		}
		if (num == _Samples.Count)
		{
			return SubmersionState.Full;
		}
		if (num == 0)
		{
			return SubmersionState.None;
		}
		return SubmersionState.Partial;
	}

	private static void CreatePlanePoints(Camera camera, int subdivisions, List<Vector3> result)
	{
		result.Clear();
		float num = CalculateNearPlaneHeight(camera);
		float num2 = num * camera.aspect;
		Vector3 nearPlaneCenter = GetNearPlaneCenter(camera);
		if (subdivisions == 0)
		{
			result.Add(nearPlaneCenter);
			return;
		}
		subdivisions--;
		Vector3 vector = 0.5f * num * camera.transform.up;
		Vector3 vector2 = 0.5f * num2 * camera.transform.right;
		Vector3 vector3 = nearPlaneCenter - vector - vector2;
		Vector3 vector4 = nearPlaneCenter + vector - vector2;
		Vector3 vector5 = nearPlaneCenter + vector + vector2;
		Vector3 vector6 = nearPlaneCenter - vector + vector2;
		result.Add(vector3);
		result.Add(vector4);
		result.Add(vector5);
		result.Add(vector6);
		for (int i = 0; i < subdivisions; i++)
		{
			float t = ((float)i + 1f) / ((float)subdivisions + 1f);
			result.Add(Vector3.Lerp(vector3, vector4, t));
			result.Add(Vector3.Lerp(vector4, vector5, t));
			result.Add(Vector3.Lerp(vector5, vector6, t));
			result.Add(Vector3.Lerp(vector6, vector3, t));
		}
	}

	private static Vector3 GetNearPlaneCenter(Camera camera)
	{
		return camera.transform.position + camera.transform.forward * camera.nearClipPlane;
	}

	private static float CalculateNearPlaneHeight(Camera camera)
	{
		return 2f * camera.nearClipPlane * Mathf.Tan(0.5f * camera.fieldOfView * (MathF.PI / 180f));
	}
}
