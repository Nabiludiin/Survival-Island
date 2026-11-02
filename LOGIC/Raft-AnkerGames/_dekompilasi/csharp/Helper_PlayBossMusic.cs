using FMODUnity;

public class Helper_PlayBossMusic : Helper_PlayMusic
{
	public override StudioEventEmitter GetEmitterFromSoundManager()
	{
		if (soundManager != null)
		{
			return soundManager.eventEmitter_BossMusic;
		}
		return null;
	}
}
