using UnityEngine;

namespace RG.Parsecs.Common;

public class OnClickSoundSlotSetter : MonoBehaviour
{
	[SerializeField]
	private OnClickSoundPlayer _soundPlayer;

	[SerializeField]
	private SoundSlot _soundSlot;

	private void Start()
	{
		FindSoundPlayerInParents();
	}

	private void OnEnable()
	{
		if (_soundPlayer != null)
		{
			_soundPlayer.SoundSlot = _soundSlot;
		}
	}

	private void FindSoundPlayerInParents()
	{
		if (!(_soundPlayer == null))
		{
			return;
		}
		Transform parent = transform;
		while (_soundPlayer == null && parent != null)
		{
			_soundPlayer = parent.GetComponent<OnClickSoundPlayer>();
			if (_soundPlayer == null)
			{
				parent = parent.parent;
			}
		}
		if (_soundPlayer == null)
		{
			Debug.LogWarning("No OnClickSoundPlayer found in parents of " + transform.name);
		}
	}
}
