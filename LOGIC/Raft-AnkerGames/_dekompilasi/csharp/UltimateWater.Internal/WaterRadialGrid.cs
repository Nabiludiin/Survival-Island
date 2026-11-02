using System;
using System.Collections.Generic;
using UltimateWater.Utils;
using UnityEngine;

namespace UltimateWater.Internal;

[Serializable]
public class WaterRadialGrid : WaterPrimitiveBase
{
	private static float _WidthCorrection;

	private static readonly int[][] _HorizontalVerticesToVertexCount;

	static WaterRadialGrid()
	{
		_WidthCorrection = 5f;
		_HorizontalVerticesToVertexCount = new int[17][];
		for (int i = 0; i < _HorizontalVerticesToVertexCount.Length; i++)
		{
			_HorizontalVerticesToVertexCount[i] = new int[600];
			float num = ((float)(i + 1) * 5f - 1f) * (MathF.PI / 180f);
			Vector2 normalized = new Vector2(Mathf.Sin(0f - num), Mathf.Cos(0f - num)).normalized;
			for (int j = 0; j < 600; j++)
			{
				int num2 = j + 2;
				float f = num * (2f / (float)(num2 - 1) - 1f);
				Vector2 normalized2 = new Vector2(Mathf.Sin(f), Mathf.Cos(f)).normalized;
				float num3 = 1f;
				int num4 = 0;
				float num5 = Vector2.Distance(normalized, normalized2);
				while (num3 >= 0.005f && !((double)num3 < 0.01))
				{
					num4++;
					num3 -= num5 * num3;
				}
				num4 += 2;
				_HorizontalVerticesToVertexCount[i][j] = num2 * num4;
			}
		}
	}

	public override Mesh[] GetTransformedMeshes(Camera camera, out Matrix4x4 matrix, int vertexCount, bool volume)
	{
		int num2;
		if (camera != null)
		{
			float num = 2f * Mathf.Atan(Mathf.Tan(camera.fieldOfView * 0.5f * (MathF.PI / 180f)) * camera.aspect) * 57.29578f;
			num2 = (camera.orthographic ? 14 : Mathf.CeilToInt(num * 0.5f * 0.2f - 0.8f));
			if (num2 >= _HorizontalVerticesToVertexCount.Length)
			{
				num2 = _HorizontalVerticesToVertexCount.Length - 1;
			}
			matrix = GetMatrix(camera, ((float)(num2 + 1) * 5f - 1f) * (MathF.PI / 180f));
		}
		else
		{
			matrix = Matrix4x4.identity;
			num2 = 14;
		}
		int num3 = vertexCount | (num2 << 26);
		if (volume)
		{
			num3 = -num3;
		}
		if (!_Cache.TryGetValue(num3, out var value))
		{
			value = (_Cache[num3] = new CachedMeshSet(CreateMeshes(vertexCount, volume, num2)));
		}
		else
		{
			value.Update();
		}
		return value.Meshes;
	}

	protected override Mesh[] CreateMeshes(int vertexCount, bool volume)
	{
		throw new NotImplementedException();
	}

	protected Mesh[] CreateMeshes(int vertexCount, bool volume, int fovIndex)
	{
		int[] array = _HorizontalVerticesToVertexCount[fovIndex];
		int num = array.Length;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] > vertexCount)
			{
				num = i + 1;
				break;
			}
		}
		int num2 = Mathf.FloorToInt((float)vertexCount / (float)num);
		int num3 = num2;
		List<Mesh> list = new List<Mesh>();
		List<Vector3> list2 = new List<Vector3>();
		List<int> list3 = new List<int>();
		int num4 = 0;
		int num5 = 0;
		float num6 = ((float)(fovIndex + 1) * 5f - 1f) * (MathF.PI / 180f);
		Vector2[] array2 = new Vector2[num];
		for (int j = 0; j < num; j++)
		{
			float num7 = (float)j / (float)(num - 1) * 2f - 1f;
			num7 = ((num7 >= 0f) ? (1f + Mathf.Sin((num7 - 1f) * MathF.PI * 0.5f)) : (-1f - Mathf.Sin((num7 - 1f) * MathF.PI * 0.5f)));
			num7 *= num6;
			array2[j] = new Vector2(Mathf.Sin(num7), Mathf.Cos(num7)).normalized;
		}
		float num8 = 1f;
		float num9 = Vector2.Distance(array2[0], array2[1]);
		if (volume)
		{
			while (num8 > 0.4f)
			{
				num8 -= num9 * num8;
				num3--;
			}
		}
		bool flag = false;
		for (int k = 0; k < num3; k++)
		{
			if ((double)num8 < 0.01)
			{
				flag = true;
			}
			for (int l = 0; l < num; l++)
			{
				Vector2 vector = array2[l] * num8;
				if (flag)
				{
					list2.Add(new Vector3(0f, 0f, 0f));
				}
				else if (k == num3 - 1)
				{
					list2.Add(new Vector3(0f, 0f, 0f));
				}
				else if (k > 1 || !volume)
				{
					list2.Add(new Vector3(vector.x, 0f, vector.y));
				}
				else if (k == 1)
				{
					vector = array2[l] * (1f - num9);
					list2.Add(new Vector3(vector.x * 10f, -0.9f, vector.y) * 0.5f);
				}
				else
				{
					vector = array2[l];
					list2.Add(new Vector3(vector.x * 10f, -0.9f, vector.y * -10f) * 0.5f);
				}
				if (l != 0 && k != 0 && num4 > num)
				{
					list3.Add(num4);
					list3.Add(num4 - 1);
					list3.Add(num4 - num - 1);
					list3.Add(num4 - num);
				}
				num4++;
			}
			if (flag)
			{
				break;
			}
			num8 -= num9 * num8;
		}
		if (num4 != 0)
		{
			Mesh item = CreateMesh32bit(list2.ToArray(), list3.ToArray(), $"Radial Grid {num}x{num2} - {num5:00}");
			list.Add(item);
		}
		return list.ToArray();
	}

	protected Matrix4x4 GetMatrix(Camera camera, float fov)
	{
		float y = _Water.transform.position.y;
		Vector3 vector = UltimateWater.Utils.Math.ViewportWaterPerpendicular(camera);
		Vector3 vector2 = UltimateWater.Utils.Math.ViewportWaterRight(camera);
		float num;
		if (vector.IsNaN() || vector2.IsNaN())
		{
			num = 2f * camera.worldToCameraMatrix.MultiplyPoint(new Vector3(0f, y, 0f)).magnitude;
		}
		else
		{
			Vector3 vector3 = UltimateWater.Utils.Math.RaycastPlane(camera, y, vector - vector2);
			num = UltimateWater.Utils.Math.RaycastPlane(camera, y, vector + vector2).x - vector3.x;
			if (num < 0f)
			{
				num = 0f - num;
			}
		}
		float f = Mathf.Clamp01(Vector3.Dot(camera.transform.forward, Vector3.down) - 0.5f);
		float value = _WidthCorrection * Mathf.Pow(f, 1f) / camera.aspect;
		num *= 1f + Mathf.Clamp(value, 0f, float.MaxValue);
		float num2 = camera.farClipPlane;
		if (WaterProjectSettings.Instance.ClipWaterCameraRange)
		{
			num2 = Mathf.Min(camera.farClipPlane, WaterProjectSettings.Instance.CameraClipRange);
		}
		Vector3 position = camera.transform.position;
		float num3 = Mathf.Tan(fov * 0.5f);
		float num4 = 0f - (num + Mathf.Max(0f, num2 * 0.005f - camera.nearClipPlane) + (_Water.MaxHorizontalDisplacement + _Water.MaxVerticalDisplacement) / num3);
		if (camera.orthographic)
		{
			num4 -= camera.orthographicSize * 3.2f;
		}
		float y2 = camera.transform.forward.y;
		Vector3 vector4 = ((y2 < -0.98f || y2 > 0.98f) ? (-camera.transform.up) : camera.transform.forward);
		float num5 = Mathf.Sqrt(vector4.x * vector4.x + vector4.z * vector4.z);
		vector4.x /= num5;
		vector4.z /= num5;
		float num6 = num2 - num4;
		return Matrix4x4.TRS(new Vector3(position.x + vector4.x * num4, y, position.z + vector4.z * num4), Quaternion.AngleAxis(Mathf.Atan2(vector4.x, vector4.z) * 57.29578f, Vector3.up), new Vector3(num6, num6, num6));
	}

	protected override Matrix4x4 GetMatrix(Camera camera)
	{
		throw new InvalidOperationException();
	}
}
