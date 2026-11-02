using System;
using UltimateWater.Internal;
using UnityEngine;

namespace UltimateWater;

public sealed class WaterSample
{
	public enum DisplacementMode
	{
		Height,
		Displacement,
		HeightAndForces
	}

	public enum ComputationsMode
	{
		Normal = 0,
		ForceCompletion = 2
	}

	private readonly Water _Water;

	private float _X;

	private float _Z;

	private float _Time;

	private Vector3 _Displaced;

	private Vector3 _PreviousResult;

	private Vector3 _Forces;

	private Vector3 _PreviousForces;

	private bool _Finished;

	private bool _Enqueued;

	private readonly DisplacementMode _DisplacementMode;

	public bool Finished => _Finished;

	public Vector2 Position => new Vector2(_X, _Z);

	public WaterSample(Water water, DisplacementMode displacementMode = DisplacementMode.Height, float precision = 1f)
	{
		if (water == null)
		{
			throw new ArgumentException("Argument 'water' is null.");
		}
		if (precision <= 0f || precision > 1f)
		{
			throw new ArgumentException("Precision has to be between 0.0 and 1.0.");
		}
		_Water = water;
		_DisplacementMode = displacementMode;
		_PreviousResult.x = float.NaN;
	}

	public void Start(Vector3 origin)
	{
		_Finished = true;
		_PreviousResult = (_Displaced = origin);
		_PreviousForces = (_Forces = default);
		GetAndReset(origin.x, origin.z);
	}

	public void Start(float x, float z)
	{
		_Finished = true;
		_PreviousResult = (_Displaced = new Vector3(x, _Water.transform.position.y, z));
		_PreviousForces = (_Forces = default);
		GetAndReset(x, z);
	}

	public Vector3 GetAndReset(Vector3 origin, ComputationsMode mode = ComputationsMode.Normal)
	{
		return GetAndReset(origin.x, origin.z, mode);
	}

	public Vector3 GetAndReset(float x, float z, ComputationsMode mode = ComputationsMode.Normal)
	{
		Vector3 forces;
		return GetAndReset(x, z, mode, out forces);
	}

	public Vector3 GetAndReset(float x, float z, ComputationsMode mode, out Vector3 forces)
	{
		switch (mode)
		{
		case ComputationsMode.ForceCompletion:
			if (!_Finished)
			{
				_Finished = true;
				ComputationStep(ignoreFinishedFlag: true);
			}
			break;
		case ComputationsMode.Normal:
			if (!_Finished && !float.IsNaN(_PreviousResult.x))
			{
				forces = _PreviousForces;
				return _PreviousResult;
			}
			_PreviousResult = _Displaced;
			_PreviousForces = _Forces;
			break;
		}
		_Finished = true;
		if (!_Enqueued)
		{
			WaterAsynchronousTasks.Instance.AddWaterSampleComputations(this);
			_Enqueued = true;
			if (_Water != null)
			{
				_Water.OnSamplingStarted();
			}
		}
		Vector3 displaced = _Displaced;
		if (_Water != null)
		{
			displaced.y += _Water.transform.position.y;
		}
		forces = _Forces;
		_X = x;
		_Z = z;
		_Displaced.x = x;
		_Displaced.y = 0f;
		_Displaced.z = z;
		_Forces.x = 0f;
		_Forces.y = 0f;
		_Forces.z = 0f;
		if (_Water != null)
		{
			_Time = _Water.Time;
		}
		_Finished = false;
		return displaced;
	}

	public void GetAndResetFast(float x, float z, float time, out Vector3 result, out Vector3 forces)
	{
		if (!_Finished)
		{
			forces = _PreviousForces;
			result = _PreviousResult;
			return;
		}
		_PreviousResult = _Displaced;
		_PreviousForces = _Forces;
		result = _Displaced;
		if (_Water != null)
		{
			result.y += _Water.transform.position.y;
		}
		forces = _Forces;
		_X = x;
		_Z = z;
		_Displaced.x = x;
		_Displaced.y = 0f;
		_Displaced.z = z;
		_Forces.x = 0f;
		_Forces.y = 0f;
		_Forces.z = 0f;
		_Time = time;
		_Finished = false;
	}

	public void GetAndResetFast(float x, float z, float time, out float result, out Vector3 forces)
	{
		if (!_Finished)
		{
			forces = _PreviousForces;
			result = _PreviousResult.y;
			return;
		}
		_PreviousResult = _Displaced;
		_PreviousForces = _Forces;
		result = _Displaced.y + _Water.transform.position.y;
		forces = _Forces;
		_X = x;
		_Z = z;
		_Displaced.x = x;
		_Displaced.y = 0f;
		_Displaced.z = z;
		_Forces.x = 0f;
		_Forces.y = 0f;
		_Forces.z = 0f;
		_Time = time;
		_Finished = false;
	}

	public Vector3 Stop()
	{
		if (_Enqueued)
		{
			if (WaterAsynchronousTasks.HasInstance)
			{
				WaterAsynchronousTasks.Instance.RemoveWaterSampleComputations(this);
			}
			_Enqueued = false;
			if (_Water != null)
			{
				_Water.OnSamplingStopped();
			}
		}
		return _Displaced;
	}

	internal void ComputationStep(bool ignoreFinishedFlag = false)
	{
		if (!(!_Finished | ignoreFinishedFlag))
		{
			return;
		}
		if (_DisplacementMode == DisplacementMode.Height || _DisplacementMode == DisplacementMode.HeightAndForces)
		{
			if (_Water != null)
			{
				_Water.CompensateHorizontalDisplacement(ref _X, ref _Z, _Time);
			}
			if (_DisplacementMode == DisplacementMode.Height)
			{
				if (_Water != null)
				{
					float uncompensatedHeightAt = _Water.GetUncompensatedHeightAt(_X, _Z, _Time);
					_Displaced.y += uncompensatedHeightAt;
				}
			}
			else if (_Water != null)
			{
				Vector4 uncompensatedHeightAndForcesAt = _Water.GetUncompensatedHeightAndForcesAt(_X, _Z, _Time);
				_Displaced.y += uncompensatedHeightAndForcesAt.w;
				_Forces.x += uncompensatedHeightAndForcesAt.x;
				_Forces.y += uncompensatedHeightAndForcesAt.y;
				_Forces.z += uncompensatedHeightAndForcesAt.z;
			}
		}
		else if (_Water != null)
		{
			Vector3 vector = ((_Water.WaterId != -1) ? _Water.GetUncompensatedDisplacementAt(_X, _Z, _Time) : default(Vector3));
			_Displaced += vector;
		}
		_Finished = true;
	}
}
