using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Common;

public class OnUIToggleClickedSoundPlayer : OnUIClickedSoundPlayer
{
	[SerializeField]
	private Toggle _toggle;

	public override void PlaySound()
	{
		if (_toggle.isOn)
		{
			base.PlaySound();
		}
	}
}
