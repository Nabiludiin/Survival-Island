using System;
using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;

namespace UltimateWater;

[Serializable]
public class WaterVolume : WaterModule
{
	[Tooltip("Makes water volume be infinite in horizontal directions and infinitely deep. It is still reduced by subtractive colliders tho. Check that if this is an ocean, sea or if this water spans through most of the scene. If you will uncheck this, you will need to add some child colliders to define where water should display.")]
	[SerializeField]
	private bool _Boundless = true;

	private bool _CollidersAdded;

	private Water _Water;

	private readonly List<WaterVolumeAdd> _Volumes = new List<WaterVolumeAdd>();

	private readonly List<WaterVolumeSubtract> _Subtractors = new List<WaterVolumeSubtract>();

	public bool Boundless => _Boundless;

	public bool HasRenderableAdditiveVolumes
	{
		get
		{
			for (int num = _Volumes.Count - 1; num >= 0; num--)
			{
				if (_Volumes[num].RenderMode != WaterVolumeRenderMode.None)
				{
					return true;
				}
			}
			return false;
		}
	}

	public List<WaterVolumeAdd> GetVolumesDirect()
	{
		return _Volumes;
	}

	public List<WaterVolumeSubtract> GetSubtractiveVolumesDirect()
	{
		return _Subtractors;
	}

	public void Dispose()
	{
	}

	public void EnableRenderers(bool forBorderRendering = false)
	{
		for (int i = 0; i < _Volumes.Count; i++)
		{
			_Volumes[i].EnableRenderers(forBorderRendering);
		}
		for (int j = 0; j < _Subtractors.Count; j++)
		{
			_Subtractors[j].EnableRenderers(forBorderRendering);
		}
	}

	public void DisableRenderers()
	{
		for (int i = 0; i < _Volumes.Count; i++)
		{
			_Volumes[i].DisableRenderers();
		}
		for (int j = 0; j < _Subtractors.Count; j++)
		{
			_Subtractors[j].DisableRenderers();
		}
	}

	public bool IsPointInside(Vector3 point, WaterVolumeSubtract[] exclusions, float radius = 0f)
	{
		for (int num = _Subtractors.Count - 1; num >= 0; num--)
		{
			WaterVolumeSubtract waterVolumeSubtract = _Subtractors[num];
			if (waterVolumeSubtract.EnablePhysics && waterVolumeSubtract.IsPointInside(point) && !Contains(exclusions, waterVolumeSubtract))
			{
				return false;
			}
		}
		if (_Boundless)
		{
			return point.y - radius <= _Water.transform.position.y + _Water.MaxVerticalDisplacement;
		}
		for (int num2 = _Volumes.Count - 1; num2 >= 0; num2--)
		{
			WaterVolumeAdd waterVolumeAdd = _Volumes[num2];
			if (waterVolumeAdd.EnablePhysics && waterVolumeAdd.IsPointInside(point))
			{
				return true;
			}
		}
		return false;
	}

	internal override void Start(Water water)
	{
		_Water = water;
	}

	internal override void Enable()
	{
		if (!_CollidersAdded && Application.isPlaying)
		{
			Collider[] componentsInChildren = _Water.GetComponentsInChildren<Collider>(includeInactive: true);
			foreach (Collider collider in componentsInChildren)
			{
				if (collider.GetComponent<WaterVolumeSubtract>() == null)
				{
					WaterVolumeAdd component = collider.GetComponent<WaterVolumeAdd>();
					AddVolume((component != null) ? component : collider.gameObject.AddComponent<WaterVolumeAdd>());
				}
			}
			_CollidersAdded = true;
		}
		EnableRenderers();
	}

	internal override void Disable()
	{
		Dispose();
		DisableRenderers();
	}

	internal void AddVolume(WaterVolumeAdd volume)
	{
		_Volumes.Add(volume);
		volume.AssignTo(_Water);
	}

	internal void RemoveVolume(WaterVolumeAdd volume)
	{
		_Volumes.Remove(volume);
	}

	internal void AddSubtractor(WaterVolumeSubtract volume)
	{
		_Subtractors.Add(volume);
		volume.AssignTo(_Water);
	}

	internal void RemoveSubtractor(WaterVolumeSubtract volume)
	{
		_Subtractors.Remove(volume);
	}

	private static bool Contains(WaterVolumeSubtract[] array, WaterVolumeSubtract element)
	{
		if (array == null)
		{
			return false;
		}
		for (int num = array.Length - 1; num >= 0; num--)
		{
			if (array[num] == element)
			{
				return true;
			}
		}
		return false;
	}

	internal bool IsPointInsideMainVolume(Vector3 point, float radius = 0f)
	{
		if (_Boundless)
		{
			return point.y - radius <= _Water.transform.position.y + _Water.MaxVerticalDisplacement;
		}
		return false;
	}

	internal override void Update()
	{
	}

	internal override void Destroy()
	{
	}

	internal override void Validate()
	{
	}
}
