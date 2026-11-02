using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

public class WeatherSystem : MonoBehaviour
{
	[SerializeField]
	[FormerlySerializedAs("water")]
	private Water _Water;

	[SerializeField]
	[FormerlySerializedAs("profile")]
	private WaterProfile _Profile;

	[SerializeField]
	[FormerlySerializedAs("radius")]
	[Tooltip("Describes how big the weather system is. Common values range from 10000 to 150000, assuming that the scene units are used as meters.")]
	private float _Radius = 10000f;

	[Range(0f, 1f)]
	[SerializeField]
	[FormerlySerializedAs("weight")]
	private float _Weight = 1f;

	private WaterWavesSpectrumData _SpectrumData;

	private Vector2 _LastOffset;

	private Vector2 _LastWindDirection;

	private float _LastRadius;

	private float _LastWeight;

	private void Start()
	{
		_SpectrumData = new WaterWavesSpectrumData(_Water, _Water.WindWaves, _Profile.Data.Spectrum);
		LateUpdate();
		_Water.WindWaves.SpectrumResolver.AddSpectrum(_SpectrumData);
	}

	private void OnEnable()
	{
		if (_SpectrumData != null && !_Water.WindWaves.SpectrumResolver.ContainsSpectrum(_SpectrumData))
		{
			_Water.WindWaves.SpectrumResolver.AddSpectrum(_SpectrumData);
		}
	}

	private void OnDisable()
	{
		_Water.WindWaves.SpectrumResolver.RemoveSpectrum(_SpectrumData);
	}

	private void LateUpdate()
	{
		Vector3 vector = _Water.transform.InverseTransformPoint(transform.position);
		Vector2 vector2 = new Vector2(vector.x, vector.z);
		Vector3 forward = transform.forward;
		Vector2 normalized = new Vector2(forward.x, forward.z).normalized;
		if (normalized != _LastWindDirection || vector2 != _LastOffset || _Radius != _LastRadius || _Weight != _LastWeight)
		{
			_SpectrumData.WindDirection = (_LastWindDirection = normalized);
			_SpectrumData.WeatherSystemOffset = (_LastOffset = vector2);
			_SpectrumData.WeatherSystemRadius = (_LastRadius = _Radius);
			_SpectrumData.Weight = (_LastWeight = _Weight);
			_Water.WindWaves.SpectrumResolver.SetDirectionalSpectrumDirty();
		}
	}
}
