using System;
using UnityEngine;

[Serializable]
public class ObjectSpawnerAssetSettings
{
	[Space(50f)]
	public bool fallback;

	public Interval_Int playerCount;

	public Interval_Float spawnRateInterval = new Interval_Float(-1f, -1f);

	public Interval_Int spawnAmount;

	public bool PlayerCountWithinRange(int playerCount)
	{
		if (playerCount <= this.playerCount.maxValue)
		{
			return playerCount >= this.playerCount.minValue;
		}
		return false;
	}
}
