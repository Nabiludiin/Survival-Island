using System;

public class QuestInteractable_Balboa_RelayStationButton : QuestInteractable
{
	public static Action OnBalboaRelayStationButtonPressed;

	protected override void OnDestroy()
	{
		OnBalboaRelayStationButtonPressed = null;
		base.OnDestroy();
	}

	public override void Interact(Network_Player player, bool successFull)
	{
		base.Interact(player, successFull);
		if (successFull && OnBalboaRelayStationButtonPressed != null)
		{
			OnBalboaRelayStationButtonPressed();
		}
	}
}
