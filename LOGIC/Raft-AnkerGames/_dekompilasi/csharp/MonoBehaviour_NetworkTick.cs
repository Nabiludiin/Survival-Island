using UnityEngine;

public class MonoBehaviour_NetworkTick : MonoBehaviour_Network
{
	[Range(0f, 50f)]
	public int ticksPerSecond = 10;

	private float tickTimer;

	public virtual void UpdateNetworkTick()
	{
		if (ticksPerSecond > 0 && !IsDirty)
		{
			tickTimer += Time.deltaTime;
			if (tickTimer >= 1f / (float)ticksPerSecond)
			{
				tickTimer -= 1f / (float)ticksPerSecond;
				Tick();
			}
		}
	}

	public virtual void Tick()
	{
		SetDirty();
	}
}
