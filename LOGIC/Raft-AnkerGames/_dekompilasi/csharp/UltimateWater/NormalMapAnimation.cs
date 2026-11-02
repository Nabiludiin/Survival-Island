using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

[Serializable]
public struct NormalMapAnimation
{
	[SerializeField]
	[FormerlySerializedAs("speed")]
	private float _Speed;

	[Tooltip("Angular deviation from the wind direction.")]
	[SerializeField]
	[FormerlySerializedAs("deviation")]
	private float _Deviation;

	[SerializeField]
	[FormerlySerializedAs("intensity")]
	[Range(0f, 4f)]
	private float _Intensity;

	[SerializeField]
	[FormerlySerializedAs("tiling")]
	private Vector2 _Tiling;

	public float Speed => _Speed;

	public float Deviation => _Deviation;

	public float Intensity => _Intensity;

	public Vector2 Tiling => _Tiling;

	public NormalMapAnimation(float speed, float deviation, float intensity, Vector2 tiling)
	{
		_Speed = speed;
		_Deviation = deviation;
		_Intensity = intensity;
		_Tiling = tiling;
	}

	public static NormalMapAnimation operator *(NormalMapAnimation a, float w)
	{
		return new NormalMapAnimation(a._Speed * w, a._Deviation * w, a._Intensity * w, a._Tiling * w);
	}

	public static NormalMapAnimation operator +(NormalMapAnimation a, NormalMapAnimation b)
	{
		return new NormalMapAnimation(a._Speed + b._Speed, a._Deviation + b._Deviation, a._Intensity + b._Intensity, a._Tiling + b._Tiling);
	}
}
