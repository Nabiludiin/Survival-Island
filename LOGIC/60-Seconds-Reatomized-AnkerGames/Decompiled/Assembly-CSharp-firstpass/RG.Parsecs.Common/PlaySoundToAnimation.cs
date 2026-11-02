using FMODUnity;
using UnityEngine;

namespace RG.Parsecs.Common;

public class PlaySoundToAnimation : MonoBehaviour
{
	[EventRef]
	[SerializeField]
	private string _fmodEvent;

	public void PlaySound()
	{
		if (!string.IsNullOrEmpty(_fmodEvent))
		{
			AudioManager.PlaySound(_fmodEvent);
		}
	}

	public void PlaySoundFromString(string fmodEvent)
	{
		if (!string.IsNullOrEmpty(fmodEvent))
		{
			AudioManager.PlaySound(fmodEvent);
		}
	}
}
