using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace UltimateWater;

public sealed class WaterVolumeProbe : MonoBehaviour
{
	[SerializeField]
	[FormerlySerializedAs("enter")]
	private UnityEvent _Enter;

	[SerializeField]
	[FormerlySerializedAs("leave")]
	private UnityEvent _Leave;

	private Water _CurrentWater;

	private Transform _Target;

	private bool _Targetted;

	private WaterVolumeSubtract[] _Exclusions;

	private float _Size;

	public Water CurrentWater => _CurrentWater;

	public UnityEvent Enter
	{
		get
		{
			if (_Enter == null)
			{
				return _Enter = new UnityEvent();
			}
			return _Enter;
		}
	}

	public UnityEvent Leave
	{
		get
		{
			if (_Leave == null)
			{
				return _Leave = new UnityEvent();
			}
			return _Leave;
		}
	}

	public static WaterVolumeProbe CreateProbe(Transform target, float size = 0f)
	{
		GameObject gameObject = new GameObject("Water Volume Probe");
		gameObject.hideFlags = HideFlags.HideAndDontSave;
		gameObject.transform.position = target.position;
		gameObject.layer = WaterProjectSettings.Instance.WaterCollidersLayer;
		SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
		sphereCollider.radius = size;
		sphereCollider.isTrigger = true;
		Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
		rigidbody.isKinematic = true;
		rigidbody.mass = 1E-07f;
		WaterVolumeProbe waterVolumeProbe = gameObject.AddComponent<WaterVolumeProbe>();
		waterVolumeProbe._Target = target;
		waterVolumeProbe._Targetted = true;
		waterVolumeProbe._Size = size;
		waterVolumeProbe._Exclusions = target.GetComponentsInChildren<WaterVolumeSubtract>(includeInactive: true);
		return waterVolumeProbe;
	}

	private void Start()
	{
		ScanWaters();
	}

	private void FixedUpdate()
	{
		if (_Targetted)
		{
			if (_Target == null)
			{
				Object.Destroy(gameObject);
				return;
			}
			transform.position = _Target.position;
		}
		if (_CurrentWater != null && _CurrentWater.Volume.Boundless)
		{
			if (!_CurrentWater.Volume.IsPointInsideMainVolume(transform.position) && !_CurrentWater.Volume.IsPointInside(transform.position, _Exclusions, _Size))
			{
				LeaveCurrentWater();
			}
		}
		else if (_CurrentWater == null)
		{
			ScanBoundlessWaters();
		}
	}

	private void OnDestroy()
	{
		_CurrentWater = null;
		if (_Enter != null)
		{
			_Enter.RemoveAllListeners();
			_Enter = null;
		}
		if (_Leave != null)
		{
			_Leave.RemoveAllListeners();
			_Leave = null;
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (_CurrentWater != null)
		{
			WaterVolumeBase waterVolume = WaterVolumeBase.GetWaterVolume<WaterVolumeSubtract>(other);
			if (waterVolume != null && waterVolume.EnablePhysics)
			{
				LeaveCurrentWater();
			}
		}
		else
		{
			WaterVolumeBase waterVolume2 = WaterVolumeBase.GetWaterVolume<WaterVolumeAdd>(other);
			if (waterVolume2 != null && waterVolume2.EnablePhysics)
			{
				EnterWater(waterVolume2.Water);
			}
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (_CurrentWater == null)
		{
			WaterVolumeBase waterVolume = WaterVolumeBase.GetWaterVolume<WaterVolumeSubtract>(other);
			if (waterVolume != null && waterVolume.EnablePhysics)
			{
				ScanWaters();
			}
		}
		else
		{
			WaterVolumeBase waterVolume2 = WaterVolumeBase.GetWaterVolume<WaterVolumeAdd>(other);
			if (waterVolume2 != null && waterVolume2.Water == _CurrentWater && waterVolume2.EnablePhysics)
			{
				LeaveCurrentWater();
			}
		}
	}

	[ContextMenu("Refresh Probe")]
	private void ScanWaters()
	{
		Vector3 position = transform.position;
		List<Water> waters = ApplicationSingleton<WaterSystem>.Instance.Waters;
		int count = waters.Count;
		for (int i = 0; i < count; i++)
		{
			if (waters[i].Volume.IsPointInside(position, _Exclusions, _Size))
			{
				EnterWater(waters[i]);
				return;
			}
		}
		LeaveCurrentWater();
	}

	private void ScanBoundlessWaters()
	{
		Vector3 position = transform.position;
		List<Water> boundlessWaters = ApplicationSingleton<WaterSystem>.Instance.BoundlessWaters;
		int count = boundlessWaters.Count;
		for (int i = 0; i < count; i++)
		{
			Water water = boundlessWaters[i];
			if (water.Volume.IsPointInsideMainVolume(position) && water.Volume.IsPointInside(position, _Exclusions, _Size))
			{
				EnterWater(water);
				break;
			}
		}
	}

	private void EnterWater(Water water)
	{
		if (!(_CurrentWater == water))
		{
			if (_CurrentWater != null)
			{
				LeaveCurrentWater();
			}
			_CurrentWater = water;
			if (_Enter != null)
			{
				_Enter.Invoke();
			}
		}
	}

	private void LeaveCurrentWater()
	{
		if (_CurrentWater != null)
		{
			if (_Leave != null)
			{
				_Leave.Invoke();
			}
			_CurrentWater = null;
		}
	}
}
