using FMODUnity;

namespace RG.Parsecs.Scavenge;

public class PlayFMODSoundAction : EventAction
{
	public string Path;

	protected override void Act(EventActionArgs args = null)
	{
		if (!string.IsNullOrEmpty(Path))
		{
			RuntimeManager.PlayOneShot(Path);
		}
	}
}
