using FMOD.Studio;
using UnityEngine;

namespace RG.Parsecs.Common;

public class OnUIClickedSoundPlayer : OnClickSoundPlayer
{
	[SerializeField]
	private bool activateOnStart = true;

	private bool active;

	private EventInstance _playEvent;

	public EventInstance FModEvent => _playEvent;

	public virtual void PlaySound()
	{
		if (_soundSlot != null && (active || activateOnStart) && !string.IsNullOrEmpty(_soundSlot.SoundEventName))
		{
			_playEvent = AudioManager.PlaySoundAndReturnInstance(_soundSlot.SoundEventName);
		}
	}

	public void Activate()
	{
		active = true;
	}
}
