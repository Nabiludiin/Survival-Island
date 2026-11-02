using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ScreenShakeAction : EventAction
{
	public string ShakePresetName;

	protected override void Act(EventActionArgs args = null)
	{
		if (string.IsNullOrEmpty(ShakePresetName))
		{
			Debug.LogErrorFormat("ShakePresetName not set in: {0}", gameObject.name);
		}
	}
}
