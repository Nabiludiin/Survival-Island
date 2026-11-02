using System;
using UnityEngine;

namespace UltimateWater;

[Serializable]
[AddComponentMenu("Ultimate Water/Water Volume Add")]
public sealed class WaterVolumeAdd : WaterVolumeBase
{
	protected override void Register(Water water)
	{
		if (water != null)
		{
			water.Volume.AddVolume(this);
		}
	}

	protected override void Unregister(Water water)
	{
		if (water != null)
		{
			water.Volume.RemoveVolume(this);
		}
	}
}
