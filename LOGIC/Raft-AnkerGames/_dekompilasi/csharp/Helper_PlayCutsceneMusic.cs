using FMODUnity;

public class Helper_PlayCutsceneMusic : Helper_PlayMusic
{
	public override StudioEventEmitter GetEmitterFromSoundManager()
	{
		if (soundManager != null)
		{
			return soundManager.eventEmitter_CutsceneMusic;
		}
		return null;
	}
}
