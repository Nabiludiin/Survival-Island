using UnityEngine;

public class TabButtonGameMode : TabButton
{
	[SerializeField]
	private GameMode tabGameMode;

	public GameMode ButtonGameMode => tabGameMode;

	public override void OnPointerClick()
	{
		base.OnPointerClick();
		GameModeValueManager.SelectCurrentGameMode(tabGameMode);
	}
}
