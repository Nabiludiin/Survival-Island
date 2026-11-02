using System;
using UnityEngine;

namespace UltimateWater;

[Serializable]
public class WaterProfile : ScriptableObject
{
	[SerializeField]
	private WaterProfileData _Data = new WaterProfileData();

	public WaterProfileData Data => _Data;
}
