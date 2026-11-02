using System;
using UnityEngine;
using UnityEngine.UI;

public class DeathMenu : SingletonGeneric<DeathMenu>
{
	[SerializeField]
	private FadePanel fadePanel;

	[SerializeField]
	private GameObject respawnButton;

	[SerializeField]
	private GameObject yesnopanel;

	[SerializeField]
	private GameObject yesnopanelUnstuck;

	[SerializeField]
	private GameObject unstuckPanel;

	[SerializeField]
	private GameObject loseItemsText;

	[SerializeField]
	private GameObject exitButton;

	[SerializeField]
	private Text worldCode;

	private Raft_Network network;

	private Network_Player localPlayer;

	private GamepadCursor cursor;

	[SerializeField]
	private GameObject[] gamepadObjects;

	[SerializeField]
	private GameObject[] keyboardObjects;

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		cursor = ComponentManager<CanvasHelper>.Value.GetComponent<GamepadCursor>();
		CustomInputConfig instance = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance;
		instance.OnControlsChangedEvent = (Action<bool>)Delegate.Combine(instance.OnControlsChangedEvent, new Action<bool>(ControlsChangedEvent));
		ControlsChangedEvent(SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad);
	}

	private void OnDestroy()
	{
		if ((bool)SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance)
		{
			CustomInputConfig instance = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance;
			instance.OnControlsChangedEvent = (Action<bool>)Delegate.Remove(instance.OnControlsChangedEvent, new Action<bool>(ControlsChangedEvent));
		}
	}

	private void ControlsChangedEvent(bool gamepad)
	{
		GameObject[] array = gamepadObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(gamepad);
		}
		array = keyboardObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(!gamepad);
		}
	}

	public void MenuOpen()
	{
		cursor.SetLayer(GamepadCursorLayer.PauseMenu);
		cursor.FocusMagnet(respawnButton.GetComponent<UiMagnet>());
		cursor.EnableCursor(_useCursor: false);
		cursor.RecenterCursor();
		fadePanel.SetAlpha(0f);
		StartCoroutine(fadePanel.FadeToAlpha(1f, 0.25f));
		unstuckPanel.SetActiveSafe(value: false);
		if (GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.canSurrender)
		{
			respawnButton.gameObject.SetActive(value: true);
		}
		else
		{
			respawnButton.gameObject.SetActive(value: false);
		}
		loseItemsText.SetActiveSafe(GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.leaveSomeItems);
		if (Raft_Network.IsHost && GameManager.Crossplay && network.WorldCode != "")
		{
			worldCode.gameObject.SetActive(value: true);
			worldCode.text = Helper.GetTerm("Menu/WorldCode") + ":" + network.WorldCode;
		}
		else
		{
			worldCode.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (fadePanel.canvasGroup.gameObject.activeInHierarchy && fadePanel.canvasGroup.alpha > 0.75f)
		{
			cursor.EnableCursor(_useCursor: true);
			Helper.SetCursorVisibleAndLockState(state: true, CursorLockMode.None);
			SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.SwitchCurrentActionMap("UI");
			if (localPlayer == null)
			{
				localPlayer = ComponentManager<Network_Player>.Value;
			}
			unstuckPanel.SetActiveSafe(localPlayer != null && localPlayer.UnstuckTracker.IsConsideredStuckWhileDead);
		}
	}

	public void Button_YesExit()
	{
		yesnopanel.SetActive(value: false);
		network.LeaveGame(DisconnectReason.SelfDisconnected, SceneName.Lobby, saveGame: true);
	}

	public void Button_NoExit()
	{
		cursor.SetLayer(GamepadCursorLayer.PauseMenu);
		cursor.FocusMagnet(exitButton.GetComponent<UiMagnet>());
		yesnopanel.SetActive(value: false);
	}

	public void Button_ExitMainMenu()
	{
		yesnopanel.SetActive(value: true);
		cursor.SetLayer(GamepadCursorLayer.PopupWindows);
		cursor.FocusMagnet(yesnopanel.GetComponentInChildren<UiMagnet>());
	}

	public void Button_Unstuck()
	{
		yesnopanelUnstuck.SetActive(value: true);
		cursor.SetLayer(GamepadCursorLayer.PopupWindows);
		cursor.FocusMagnet(yesnopanelUnstuck.GetComponentInChildren<UiMagnet>());
	}

	public void Button_YesUnstuck()
	{
		PauseMenu value = ComponentManager<PauseMenu>.Value;
		if (value != null)
		{
			value.UnstuckLocalPlayer();
		}
		Button_CloseUnstuck();
	}

	public void Button_NoUnstuck()
	{
		Button_CloseUnstuck();
	}

	public void Button_CloseUnstuck()
	{
		cursor.SetLayer(GamepadCursorLayer.PauseMenu);
		cursor.FocusMagnet(unstuckPanel.GetComponentInChildren<UiMagnet>());
		yesnopanelUnstuck.SetActiveSafe(value: false);
	}
}
