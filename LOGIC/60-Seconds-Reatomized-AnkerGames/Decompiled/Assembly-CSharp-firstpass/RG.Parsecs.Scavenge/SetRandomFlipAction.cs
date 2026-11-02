using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class SetRandomFlipAction : EventAction
{
	public bool flipX;

	public bool flipY;

	public bool flipZ;

	[ContextMenu("test")]
	public void Test()
	{
		Act();
	}

	protected override void Act(EventActionArgs args = null)
	{
		Vector3 eulerAngles = transform.eulerAngles;
		if (flipX && Random.value > 0.5f)
		{
			eulerAngles.x += 180f;
		}
		if (flipY && Random.value > 0.5f)
		{
			eulerAngles.y += 180f;
		}
		if (flipZ && Random.value > 0.5f)
		{
			eulerAngles.z += 180f;
		}
		transform.eulerAngles = eulerAngles;
	}
}
