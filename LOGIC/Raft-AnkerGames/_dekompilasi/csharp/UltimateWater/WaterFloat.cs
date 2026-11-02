using UnityEngine;

namespace UltimateWater;

public sealed class WaterFloat : MonoBehaviour
{
	public enum DisplacementMode
	{
		Height,
		Displacement
	}

	[SerializeField]
	private DisplacementMode _DisplacementMode = DisplacementMode.Displacement;

	[SerializeField]
	private float _HeightBonus;

	[Range(0.04f, 1f)]
	[SerializeField]
	private float _Precision = 0.2f;

	[SerializeField]
	private Water _Water;

	private Vector3 _InitialPosition;

	private Vector3 _PreviousPosition;

	private WaterSample _Sample;

	private void Start()
	{
		if (_Water == null)
		{
			_Water = Utilities.GetWaterReference();
		}
		if (!_Water.IsNullReference(this))
		{
			_InitialPosition = transform.position;
			_PreviousPosition = _InitialPosition;
			_Sample = new WaterSample(_Water, (WaterSample.DisplacementMode)_DisplacementMode, _Precision);
			_Sample.Start(transform.position);
		}
	}

	private void OnDisable()
	{
		if (_Sample != null)
		{
			_Sample.Stop();
		}
	}

	private void LateUpdate()
	{
		_InitialPosition += transform.position - _PreviousPosition;
		Vector3 andReset = _Sample.GetAndReset(_InitialPosition.x, _InitialPosition.z, WaterSample.ComputationsMode.ForceCompletion);
		andReset.y += _HeightBonus;
		transform.position = andReset;
		_PreviousPosition = andReset;
	}

	private void Reset()
	{
		_Water = Utilities.GetWaterReference();
	}
}
