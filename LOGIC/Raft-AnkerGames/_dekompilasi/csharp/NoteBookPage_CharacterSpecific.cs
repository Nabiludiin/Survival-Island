using UnityEngine;

public class NoteBookPage_CharacterSpecific : NoteBookPage
{
	[SerializeField]
	private ObjectEnabler characterEnabler;

	private Network_Player localPlayer;

	public override void SetPageActive(bool value)
	{
		base.SetPageActive(value);
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (localPlayer != null && localPlayer.characterSettings != null)
		{
			characterEnabler.ActivateIndex(localPlayer.characterSettings.ModelIndex, deactivateOthers: true);
		}
		else
		{
			characterEnabler.DeactivateAllObject();
		}
	}
}
