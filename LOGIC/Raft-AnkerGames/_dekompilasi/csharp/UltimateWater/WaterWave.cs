using System;
using UnityEngine;

namespace UltimateWater;

public struct WaterWave : IComparable<WaterWave>
{
	private readonly ushort _U;

	private readonly ushort _V;

	private readonly float _Kx;

	private readonly float _Kz;

	private readonly float _DotOffset;

	internal readonly float _Nkx;

	internal readonly float _Nky;

	internal readonly float _W;

	internal readonly byte _ScaleIndex;

	internal float _Amplitude;

	internal float _CPUPriority;

	internal float _Offset;

	public float K => Mathf.Sqrt(_Kx * _Kx + _Kz * _Kz);

	public int CompareTo(WaterWave other)
	{
		return other._CPUPriority.CompareTo(_CPUPriority);
	}

	public float GetHeightAt(float x, float z, float t)
	{
		float num = _Kx * x + _Kz * z;
		return _Amplitude * Mathf.Sin(num + t * _W + _Offset);
	}

	public void GetForceAndHeightAt(float x, float z, float t, ref Vector4 result)
	{
		int num = (int)((_Kx * x + _Kz * z + t * _W + _Offset) * 325.949f) & 0x7FF;
		float num2 = FastMath.Sines[num];
		float num3 = FastMath.Cosines[num];
		float num4 = _Amplitude * num2;
		float num5 = _Amplitude * num3;
		result.x += _Nkx * num4;
		result.z += _Nky * num4;
		result.y += num5;
		result.w += num4;
	}

	public Vector2 GetRawHorizontalDisplacementAt(float x, float z, float t)
	{
		float num = _Kx * x + _Kz * z;
		float num2 = _Amplitude * Mathf.Cos(num + t * _W + _Offset);
		return new Vector2(_Nkx * num2, _Nky * num2);
	}

	public Vector3 GetDisplacementAt(float x, float z, float t)
	{
		FastMath.SinCos2048(_Kx * x + _Kz * z + t * _W + _Offset, out var s, out var c);
		c *= _Amplitude;
		return new Vector3(_Nkx * c, s * _Amplitude, _Nky * c);
	}

	public WaterWave(byte scaleIndex, float offsetX, float offsetZ, ushort u, ushort v, float kx, float kz, float k, float w, float amplitude)
	{
		_ScaleIndex = scaleIndex;
		_DotOffset = offsetX * kx + offsetZ * kz;
		_U = u;
		_V = v;
		_Kx = kx;
		_Kz = kz;
		_Nkx = ((k != 0f) ? (kx / k) : 0.707107f);
		_Nky = ((k != 0f) ? (kz / k) : 0.707107f);
		_Amplitude = 2f * amplitude;
		_Offset = 0f;
		_W = w;
		_CPUPriority = ((amplitude >= 0f) ? amplitude : (0f - amplitude));
	}

	public void UpdateSpectralValues(Vector3[][] spectrum, Vector2 windDirection, float directionalityInv, int resolution, float horizontalScale)
	{
		Vector3 vector = spectrum[_ScaleIndex][_U * resolution + _V];
		float num = windDirection.x * _Nkx + windDirection.y * _Nky;
		float num2 = Mathf.Acos(num * 0.999f);
		float num3 = Mathf.Sqrt(1f + vector.z * Mathf.Cos(2f * num2));
		if (num < 0f)
		{
			num3 *= directionalityInv;
		}
		float num4 = vector.x * num3;
		float num5 = vector.y * num3;
		_Amplitude = 2f * Mathf.Sqrt(num4 * num4 + num5 * num5);
		_Offset = Mathf.Atan2(Mathf.Abs(num4), Mathf.Abs(num5));
		if (num5 > 0f)
		{
			_Amplitude = 0f - _Amplitude;
			_Offset = 0f - _Offset;
		}
		if (num4 < 0f)
		{
			_Offset = 0f - _Offset;
		}
		_Offset += _DotOffset;
		_CPUPriority = ((_Amplitude >= 0f) ? _Amplitude : (0f - _Amplitude));
	}
}
