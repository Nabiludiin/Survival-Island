using System.Collections.Generic;
using UnityEngine;

namespace UltimateWater;

public class ParticleController : MonoBehaviour
{
	public enum EmissionType
	{
		OnWaterEnter,
		OnWaterExit,
		OnWaterEnterAndExit
	}

	[SerializeField]
	private List<ParticleSystem> _Particles;

	[Space]
	public WaterSampler Sampler;

	[Space]
	public EmissionType Type = EmissionType.OnWaterEnterAndExit;

	[Space]
	public float Emission = 1f;

	public float Speed = 1f;

	[Tooltip("How fast the emission decreases")]
	public float Decrease = 1f;

	private readonly ParticleModifier _Modifier = new ParticleModifier();

	private float _CurrentEmission;

	private void Awake()
	{
		_CurrentEmission = Emission;
		_Modifier.Initialize(_Particles.ToArray());
		_Modifier.Speed = Speed;
		_Modifier.Active = false;
	}

	private void OnEnable()
	{
		Sampler.OnSubmersionStateChanged.AddListener(OnChange);
	}

	private void OnDisable()
	{
		Sampler.OnSubmersionStateChanged.RemoveListener(OnChange);
	}

	private void LateUpdate()
	{
		_Modifier.Emission = ((_CurrentEmission > 0f) ? _CurrentEmission : 0f);
		if (_CurrentEmission > 0f)
		{
			_CurrentEmission -= Time.deltaTime * Decrease;
			if (_CurrentEmission <= 0f)
			{
				_Modifier.Active = false;
			}
		}
	}

	private void OnValidate()
	{
		if (Application.isPlaying && _Modifier.IsInitialized)
		{
			_Modifier.Speed = Speed;
		}
	}

	private void Reset()
	{
		Sampler = GetComponent<WaterSampler>();
	}

	private void OnChange(WaterSampler.SubmersionState state)
	{
		bool flag = false;
		switch (Type)
		{
		case EmissionType.OnWaterEnter:
			flag = state == WaterSampler.SubmersionState.Under;
			break;
		case EmissionType.OnWaterExit:
			flag = state == WaterSampler.SubmersionState.Above;
			break;
		case EmissionType.OnWaterEnterAndExit:
			flag = true;
			break;
		}
		if (flag)
		{
			_Modifier.Active = true;
			_Modifier.Emission = (_CurrentEmission = Emission);
		}
	}
}
