using UnityEngine;

namespace UltimateWater;

[AddComponentMenu("Ultimate Water/Network Synchronization", 2)]
[RequireComponent(typeof(Water))]
public class NetworkWater
{
	private float _Time;

	private void Awake()
	{
	}

	private void Update()
	{
		_Time = Time.time;
	}
}
