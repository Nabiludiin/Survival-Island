using System.Collections;
using RG.VirtualInput;

namespace RG.Parsecs.Common;

public class OnSpriteClickedSoundPlayer : OnClickSoundPlayer
{
	private void OnMouseUpAsButton()
	{
		if (!Singleton<VirtualInputManager>.Instance.IsPointerOverGameObject() && _soundSlot != null && !string.IsNullOrEmpty(_soundSlot.SoundEventName))
		{
			Singleton<GameManager>.Instance.PlaySoundInvoke(PlaySound());
		}
	}

	public void InvokePlaySound()
	{
		if (_soundSlot != null && !string.IsNullOrEmpty(_soundSlot.SoundEventName))
		{
			Singleton<GameManager>.Instance.PlaySoundInvoke(PlaySound());
		}
	}

	public IEnumerator PlaySound()
	{
		AudioManager.PlaySoundAndReturnInstance(_soundSlot.SoundEventName);
		yield return null;
	}
}
