using System;
using UnityEngine;

namespace UltimateWater.Internal;

[Serializable]
public sealed class WaterUvAnimator : WaterModule
{
	private NormalMapAnimation _NormalMapAnimation1 = new NormalMapAnimation(1f, -10f, 1f, new Vector2(1f, 1f));

	private NormalMapAnimation _NormalMapAnimation2 = new NormalMapAnimation(-0.55f, 20f, 0.74f, new Vector2(1.5f, 1.5f));

	private float _WindOffset1X;

	private float _WindOffset1Y;

	private float _WindOffset2X;

	private float _WindOffset2Y;

	private Vector2 _WindSpeed1;

	private Vector2 _WindSpeed2;

	private Vector2 _WindSpeed;

	private Water _Water;

	private WindWaves _WindWaves;

	private bool _HasWindWaves;

	private Vector4 _UvTransform1;

	private Vector4 _UvTransform2;

	private bool _WindVectorsDirty = true;

	private float _LastTime;

	public Vector2 WindOffset => new Vector2(_WindOffset1X, _WindOffset1Y);

	public NormalMapAnimation NormalMapAnimation1
	{
		get
		{
			return _NormalMapAnimation1;
		}
		set
		{
			_NormalMapAnimation1 = value;
			_WindVectorsDirty = true;
			_UvTransform1.x = _NormalMapAnimation1.Tiling.x;
			_UvTransform1.y = _NormalMapAnimation1.Tiling.y;
		}
	}

	public NormalMapAnimation NormalMapAnimation2
	{
		get
		{
			return _NormalMapAnimation2;
		}
		set
		{
			_NormalMapAnimation2 = value;
			_WindVectorsDirty = true;
			_UvTransform2.x = _NormalMapAnimation2.Tiling.x;
			_UvTransform2.y = _NormalMapAnimation2.Tiling.y;
		}
	}

	internal override void Start(Water water)
	{
		_Water = water;
		_WindWaves = water.WindWaves;
		_HasWindWaves = _WindWaves != null;
	}

	internal override void Update()
	{
		float time = _Water.Time;
		float num = time - _LastTime;
		_LastTime = time;
		if (_WindVectorsDirty || HasWindSpeedChanged())
		{
			PrecomputeWindVectors();
			_WindVectorsDirty = false;
		}
		_WindOffset1X += _WindSpeed1.x * num;
		_WindOffset1Y += _WindSpeed1.y * num;
		_WindOffset2X += _WindSpeed2.x * num;
		_WindOffset2Y += _WindSpeed2.y * num;
		_UvTransform1.z = (0f - _WindOffset1X) * _UvTransform1.x;
		_UvTransform1.w = (0f - _WindOffset1Y) * _UvTransform1.y;
		_UvTransform2.z = (0f - _WindOffset2X) * _UvTransform2.x;
		_UvTransform2.w = (0f - _WindOffset2Y) * _UvTransform2.y;
		MaterialPropertyBlock propertyBlock = _Water.Renderer.PropertyBlock;
		propertyBlock.SetVector(ShaderVariables.BumpMapST, _UvTransform1);
		propertyBlock.SetVector(ShaderVariables.DetailAlbedoMapST, _UvTransform2);
	}

	private void PrecomputeWindVectors()
	{
		_WindSpeed = GetWindSpeed();
		_WindSpeed1 = FastMath.Rotate(_WindSpeed, _NormalMapAnimation1.Deviation * (MathF.PI / 180f)) * (_NormalMapAnimation1.Speed * 0.001365f);
		_WindSpeed2 = FastMath.Rotate(_WindSpeed, _NormalMapAnimation2.Deviation * (MathF.PI / 180f)) * (_NormalMapAnimation2.Speed * 0.00084f);
	}

	private Vector2 GetWindSpeed()
	{
		if (!_HasWindWaves)
		{
			return new Vector2(1f, 0f);
		}
		return _WindWaves.WindSpeed;
	}

	private bool HasWindSpeedChanged()
	{
		if (_HasWindWaves)
		{
			return _WindWaves.WindSpeedChanged;
		}
		return false;
	}
}
