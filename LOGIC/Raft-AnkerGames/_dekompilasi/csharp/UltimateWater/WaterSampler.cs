using System;
using UnityEngine;
using UnityEngine.Events;

namespace UltimateWater;

[AddComponentMenu("Ultimate Water/Water Sampler")]
public class WaterSampler : MonoBehaviour
{
	[Serializable]
	public class WaterSubmersionEvent : UnityEvent<SubmersionState>
	{
	}

	public enum SubmersionState
	{
		Under,
		Above
	}

	[Header("References")]
	[SerializeField]
	private Water _Water;

	public float Hysteresis = 0.1f;

	[Header("Events")]
	public WaterSubmersionEvent OnSubmersionStateChanged;

	private WaterSample _Sample;

	private float _PreviousWaterHeight;

	private float _PreviousObjectHeight;

	public float Height { get; private set; }

	public float Velocity { get; private set; }

	public SubmersionState State { get; private set; }

	public bool IsInitialized
	{
		get
		{
			if (_Water != null)
			{
				return _Sample != null;
			}
			return false;
		}
	}

	private void Start()
	{
		if (_Water == null)
		{
			_Water = Utilities.GetWaterReference();
		}
		_Sample = new WaterSample(_Water);
	}

	private void Update()
	{
		Vector3 andReset = _Sample.GetAndReset(transform.position);
		float num = (transform.position.y - _PreviousObjectHeight) / Time.deltaTime;
		float num2 = (andReset.y - _PreviousWaterHeight) / Time.deltaTime;
		Velocity = Mathf.Abs(num - num2);
		Height = transform.position.y - andReset.y;
		if (State != GetState(Height) && Mathf.Abs(Height) > Hysteresis)
		{
			State = ((Height > 0f) ? SubmersionState.Above : SubmersionState.Under);
			OnSubmersionStateChanged.Invoke(State);
		}
		_PreviousObjectHeight = transform.position.y;
		_PreviousWaterHeight = andReset.y;
		transform.rotation = Quaternion.identity;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, 0.2f);
	}

	private void Reset()
	{
		_Water = Utilities.GetWaterReference();
	}

	private static SubmersionState GetState(float height)
	{
		if (!(height > 0f))
		{
			return SubmersionState.Under;
		}
		return SubmersionState.Above;
	}
}
