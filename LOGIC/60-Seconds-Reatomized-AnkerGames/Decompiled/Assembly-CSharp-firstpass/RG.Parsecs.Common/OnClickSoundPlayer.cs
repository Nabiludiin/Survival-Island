using UnityEngine;

namespace RG.Parsecs.Common;

public abstract class OnClickSoundPlayer : MonoBehaviour
{
	[Tooltip("The sound event set in this SoundSlot will be played back on click by this script. If null is set, the sound will not be played.")]
	[SerializeField]
	protected SoundSlot _soundSlot;

	public SoundSlot SoundSlot
	{
		get
		{
			return _soundSlot;
		}
		set
		{
			_soundSlot = value;
		}
	}
}
