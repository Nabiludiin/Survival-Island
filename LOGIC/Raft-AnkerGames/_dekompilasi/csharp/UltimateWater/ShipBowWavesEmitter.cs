using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

public class ShipBowWavesEmitter : MonoBehaviour
{
	[SerializeField]
	[FormerlySerializedAs("gpuParticleSystem")]
	private WaveParticlesSystemGPU _GPUParticleSystem;

	[SerializeField]
	[FormerlySerializedAs("unityParticleSystem")]
	private ParticleSystem _UnityParticleSystem;

	[SerializeField]
	[FormerlySerializedAs("waveSpeed")]
	[Range(0.02f, 0.98f)]
	private float _WaveSpeed = 0.5f;

	[SerializeField]
	[FormerlySerializedAs("amplitude")]
	private float _Amplitude = 0.5f;

	[SerializeField]
	[FormerlySerializedAs("wavelength")]
	private float _Wavelength = 6f;

	[SerializeField]
	[FormerlySerializedAs("lifetime")]
	private float _Lifetime = 50f;

	[SerializeField]
	[FormerlySerializedAs("foam")]
	private float _Foam = 1f;

	[SerializeField]
	[FormerlySerializedAs("maxShipSpeed")]
	private float _MaxShipSpeed = 16.5f;

	[SerializeField]
	[FormerlySerializedAs("leftRightSpace")]
	private float _LeftRightSpace = 1f;

	[Range(0f, 1f)]
	[SerializeField]
	[FormerlySerializedAs("trailCalming")]
	private float _TrailCalming = 1f;

	[Range(0f, 8f)]
	[SerializeField]
	[FormerlySerializedAs("trailFoam")]
	private float _TrailFoam = 1f;

	[Header("Advanced")]
	[Tooltip("Use for submarines. Allows emission to be moved to exposed ship parts during submerge process and completely disabled after complete submarge.")]
	[SerializeField]
	[FormerlySerializedAs("advancedEmissionPositioning")]
	private bool _AdvancedEmissionPositioning;

	[Tooltip("Required if 'advancedEmissionPositioning' is enabled. Allows emitter to determine an emission point on that collider.")]
	[SerializeField]
	[FormerlySerializedAs("shipCollider")]
	private Collider _ShipCollider;

	[SerializeField]
	[FormerlySerializedAs("advancedEmissionOffset")]
	private float _AdvancedEmissionOffset = 2f;

	[SerializeField]
	[FormerlySerializedAs("minTextureIndex")]
	private int _MinTextureIndex;

	[SerializeField]
	[FormerlySerializedAs("maxTextureIndex")]
	private int _MaxTextureIndex = 4;

	[SerializeField]
	[FormerlySerializedAs("emissionSpacing")]
	[Range(0.1f, 1f)]
	private float _EmissionSpacing = 0.45f;

	private Vector2 _PreviousFrameBowPosition;

	private float _TotalBowDeltaMagnitude;

	private float _LastBowEmitTime;

	private float _AngleSin;

	private float _AngleCos;

	private float _Space;

	private bool _UseBuiltinParticleSystem;

	private Water _WaterComponent;

	private void Start()
	{
		if (_GPUParticleSystem == null)
		{
			_GPUParticleSystem = Object.FindObjectOfType<WaveParticlesSystemGPU>();
		}
		_UseBuiltinParticleSystem = _UnityParticleSystem == null;
		_WaterComponent = _GPUParticleSystem.GetComponent<Water>();
		OnValidate();
		Vector2 previousFrameBowPosition = GetVector2(transform.position) + _WaterComponent.SurfaceOffset;
		_PreviousFrameBowPosition = previousFrameBowPosition;
	}

	private void OnValidate()
	{
		_Space = _Wavelength * _EmissionSpacing;
		float f = Mathf.Acos(_WaveSpeed);
		_AngleSin = Mathf.Sin(f);
		_AngleCos = Mathf.Cos(f);
	}

	private void LateUpdate()
	{
		Vector2 vector = GetVector2(transform.position);
		Vector2 vector2 = vector + _WaterComponent.SurfaceOffset;
		Vector2 vector3 = vector2 - _PreviousFrameBowPosition;
		Vector2 normalized = GetVector2(transform.forward).normalized;
		_PreviousFrameBowPosition = vector2;
		float num = vector3.x * normalized.x + vector3.y * normalized.y;
		if (num < 0f)
		{
			return;
		}
		float num2 = num;
		_TotalBowDeltaMagnitude += num2;
		if (!(_TotalBowDeltaMagnitude >= _Space))
		{
			return;
		}
		float time = Time.time;
		float num3 = time - _LastBowEmitTime;
		_LastBowEmitTime = time;
		float num4 = _TotalBowDeltaMagnitude / num3;
		if (num4 >= _MaxShipSpeed)
		{
			num4 = _MaxShipSpeed;
		}
		float num5 = _WaveSpeed * num4;
		Vector2 vector4 = new Vector2(normalized.x * _AngleCos - normalized.y * _AngleSin, normalized.x * _AngleSin + normalized.y * _AngleCos);
		Vector2 vector5 = new Vector2(normalized.x * _AngleCos + normalized.y * _AngleSin, normalized.y * _AngleCos - normalized.x * _AngleSin);
		Vector2 normalized2 = GetVector2(transform.right).normalized;
		do
		{
			_TotalBowDeltaMagnitude -= _Space;
			if (_AdvancedEmissionPositioning)
			{
				float y = _WaterComponent.transform.position.y + _WaterComponent.GetHeightAt(vector.x, vector.y, time);
				if (!_ShipCollider.Raycast(new Ray(new Vector3(vector.x, y, vector.y), new Vector3(0f - normalized.x, 0f, 0f - normalized.y)), out var hitInfo, 100f))
				{
					break;
				}
				vector = GetVector2(hitInfo.point) + normalized * _AdvancedEmissionOffset;
			}
			Vector2 horizontalDisplacementAt = _WaterComponent.GetHorizontalDisplacementAt(vector.x, vector.y, time);
			vector.x -= horizontalDisplacementAt.x;
			vector.y -= horizontalDisplacementAt.y;
			if (_UseBuiltinParticleSystem)
			{
				_GPUParticleSystem.EmitParticle(new WaveParticlesSystemGPU.ParticleData
				{
					Position = vector + normalized2 * _LeftRightSpace,
					Direction = vector5 * num5,
					Amplitude = _Amplitude,
					Wavelength = _Wavelength,
					InitialLifetime = _Lifetime,
					Lifetime = _Lifetime,
					Foam = _Foam,
					UvOffsetPack = (float)Random.Range(0, _GPUParticleSystem.FoamAtlasHeight) / (float)_GPUParticleSystem.FoamAtlasHeight * 16f + (float)Random.Range(_MinTextureIndex, _MaxTextureIndex) / (float)_GPUParticleSystem.FoamAtlasWidth,
					TrailCalming = _TrailCalming,
					TrailFoam = _TrailFoam
				});
				_GPUParticleSystem.EmitParticle(new WaveParticlesSystemGPU.ParticleData
				{
					Position = vector + normalized2 * (0f - _LeftRightSpace),
					Direction = vector4 * num5,
					Amplitude = _Amplitude,
					Wavelength = _Wavelength,
					InitialLifetime = _Lifetime,
					Lifetime = _Lifetime,
					Foam = _Foam,
					UvOffsetPack = (float)Random.Range(0, _GPUParticleSystem.FoamAtlasHeight) / (float)_GPUParticleSystem.FoamAtlasHeight * 16f + (float)Random.Range(_MinTextureIndex, _MaxTextureIndex) / (float)_GPUParticleSystem.FoamAtlasWidth,
					TrailCalming = _TrailCalming,
					TrailFoam = _TrailFoam
				});
			}
			else
			{
				ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
				{
					position = new Vector3(vector.x + normalized2.x * _LeftRightSpace, _WaterComponent.transform.position.y, vector.y + normalized2.y * _LeftRightSpace),
					velocity = new Vector3(vector5.x, 0f, vector5.y) * num5
				};
				_UnityParticleSystem.Emit(emitParams, 1);
				emitParams.position = new Vector3(vector.x + normalized2.x * (0f - _LeftRightSpace), emitParams.position.y, vector.y + normalized2.y * (0f - _LeftRightSpace));
				emitParams.velocity = new Vector3(vector4.x, 0f, vector4.y) * num5;
				_UnityParticleSystem.Emit(emitParams, 1);
			}
		}
		while (_TotalBowDeltaMagnitude >= _Space);
	}

	private static Vector2 GetVector2(Vector3 vector3)
	{
		return new Vector2(vector3.x, vector3.z);
	}
}
