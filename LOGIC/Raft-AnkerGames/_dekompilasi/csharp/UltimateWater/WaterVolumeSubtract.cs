using UnityEngine;
using UnityEngine.Rendering;

namespace UltimateWater;

[AddComponentMenu("Ultimate Water/Water Volume Subtract")]
public sealed class WaterVolumeSubtract : WaterVolumeBase
{
	protected override CullMode _CullMode => CullMode.Front;

	protected override void Register(Water water)
	{
		if (water != null)
		{
			water.Volume.AddSubtractor(this);
		}
		else if (Application.isPlaying)
		{
			Debug.LogError("Water reference is null when regestering water volume subtractor " + gameObject.name, gameObject);
		}
	}

	protected override void Unregister(Water water)
	{
		if (water != null)
		{
			water.Volume.RemoveSubtractor(this);
		}
	}
}
