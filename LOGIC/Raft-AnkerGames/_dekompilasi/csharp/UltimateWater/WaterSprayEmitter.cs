using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

public sealed class WaterSprayEmitter : MonoBehaviour
{
	[SerializeField]
	[FormerlySerializedAs("water")]
	private Spray _Water;

	[SerializeField]
	[FormerlySerializedAs("emissionRate")]
	private float _EmissionRate = 5f;

	[SerializeField]
	[FormerlySerializedAs("startIntensity")]
	private float _StartIntensity = 1f;

	[SerializeField]
	[FormerlySerializedAs("startVelocity")]
	private float _StartVelocity = 1f;

	[SerializeField]
	[FormerlySerializedAs("lifetime")]
	private float _Lifetime = 4f;

	private float _TotalTime;

	private float _TimeStep;

	private Spray.Particle[] _Particles;

	public float StartVelocity
	{
		get
		{
			return _StartVelocity;
		}
		set
		{
			_StartVelocity = value;
		}
	}

	private void Update()
	{
		int num = 0;
		_TotalTime += Time.deltaTime;
		while (_TotalTime >= _TimeStep && num < _Particles.Length)
		{
			_TotalTime -= _TimeStep;
			_Particles[num].Lifetime = new Vector2(_Lifetime, _Lifetime);
			_Particles[num].MaxIntensity = _StartIntensity;
			_Particles[num].Position = transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
			_Particles[num].Velocity = transform.forward * _StartVelocity;
			_Particles[num++].Offset = Random.Range(0f, 10f);
		}
		if (num != 0)
		{
			_Water.SpawnCustomParticles(_Particles, num);
		}
	}

	private void OnValidate()
	{
		_TimeStep = 1f / _EmissionRate;
	}

	private void Start()
	{
		OnValidate();
		_Particles = new Spray.Particle[Mathf.Max(1, (int)_EmissionRate)];
	}
}
