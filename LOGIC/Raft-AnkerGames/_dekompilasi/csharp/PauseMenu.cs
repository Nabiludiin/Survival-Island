using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour, LocalizeEventInterface, SceneEventInterface
{
	public GameObject pausePanel;

	public GameObject yesNoPanel;

	public GameObject yesNoPanelUnstuck;

	public GameObject savePanel;

	public GameObject buttonHolderPanel;

	public GameObject unstuckPanel;

	public GameObject hostDisconnectedPanel;

	[SerializeField]
	private GameObject button_help;

	[SerializeField]
	private GameObject button_exit;

	[SerializeField]
	private Button button_save;

	[SerializeField]
	private Text text_save;

	[SerializeField]
	private Text text_hostGameMode;

	[SerializeField]
	private Text text_worldCode;

	[SerializeField]
	private Button button_save_diconnected;

	[SerializeField]
	private Text text_save_diconnected;

	[SerializeField]
	private UiMagnet settingsMagnet;

	[SerializeField]
	private UiMagnet lobbyMagnet;

	[SerializeField]
	private UiMagnet returnToGameMagnet;

	[SerializeField]
	private UiMagnet mainMenuMagnet;

	[SerializeField]
	private UiMagnet exitGameMagnet;

	[SerializeField]
	private GameObject[] keyboardOnly;

	[SerializeField]
	private GameObject[] gamepadOnly;

	[SerializeField]
	private Lobby lobby;

	[SerializeField]
	private InputActionReference inputPause;

	[SerializeField]
	private InputActionReference inputCancel;

	[SerializeField]
	private PopupWindow errorPopup;

	private Raft_Network network;

	private CanvasHelper canvas;

	private Settings settings;

	private SaveAndLoad saveandload;

	private SceneName exitScene;

	private GamepadCursor cursor;

	private bool ignoreCancel;

	public bool HasSaved
	{
		get
		{
			if (button_save.interactable)
			{
				return !button_save_diconnected.interactable;
			}
			return true;
		}
	}

	private void Awake()
	{
		ComponentManager<PauseMenu>.Value = this;
	}

	private void Start()
	{
		pausePanel.SetActive(value: false);
		yesNoPanel.SetActive(value: false);
		yesNoPanelUnstuck.SetActive(value: false);
		savePanel.SetActive(value: false);
		hostDisconnectedPanel.SetActive(value: false);
		saveandload = ComponentManager<SaveAndLoad>.Value;
		network = ComponentManager<Raft_Network>.Value;
		canvas = ComponentManager<CanvasHelper>.Value;
		settings = ComponentManager<Settings>.Value;
		cursor = canvas.GetComponent<GamepadCursor>();
		Settings.OnSettingsClosed = (Action)Delegate.Combine(Settings.OnSettingsClosed, new Action(SettingsClosed));
		Lobby.OnSocialClosed = (Action)Delegate.Combine(Lobby.OnSocialClosed, new Action(LobbyClosed));
		if (!Raft_Network.IsHost)
		{
			button_save.gameObject.SetActive(value: false);
			button_save_diconnected.gameObject.SetActive(value: false);
		}
		CustomInputConfig instance = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance;
		instance.OnControlsChangedEvent = (Action<bool>)Delegate.Combine(instance.OnControlsChangedEvent, new Action<bool>(OnControlsChanged));
		OnControlsChanged(CanvasHelper.Gamepad);
		CanvasHelper.MenuCloseEvent += MenuClosed;
		PopupWindow popupWindow = errorPopup;
		popupWindow.OnClose = (Action)Delegate.Combine(popupWindow.OnClose, new Action(MessageClosed));
	}

	private void MenuClosed(MenuType menuType)
	{
		if (menuType == MenuType.PauseMenu)
		{
			Time.timeScale = 1f;
			cursor.SetLayer(GamepadCursorLayer.Inventory);
		}
	}

	private void SettingsClosed()
	{
		ignoreCancel = true;
		cursor.FocusMagnet(settingsMagnet);
		OnLocalizeEvent();
	}

	private void OnDestroy()
	{
		Settings.OnSettingsClosed = (Action)Delegate.Remove(Settings.OnSettingsClosed, new Action(SettingsClosed));
		Lobby.OnSocialClosed = (Action)Delegate.Remove(Lobby.OnSocialClosed, new Action(LobbyClosed));
		PopupWindow popupWindow = errorPopup;
		popupWindow.OnClose = (Action)Delegate.Remove(popupWindow.OnClose, new Action(MessageClosed));
		if ((bool)SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance)
		{
			CustomInputConfig instance = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance;
			instance.OnControlsChangedEvent = (Action<bool>)Delegate.Remove(instance.OnControlsChangedEvent, new Action<bool>(OnControlsChanged));
		}
	}

	private void OnControlsChanged(bool gamepad)
	{
		GameObject[] array = keyboardOnly;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(!gamepad);
		}
		array = gamepadOnly;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(gamepad);
		}
	}

	private void PauseWithMessage()
	{
		Pause();
		buttonHolderPanel.gameObject.SetActive(value: false);
		errorPopup.gameObject.SetActive(value: true);
	}

	private void MessageClosed()
	{
		canvas.CloseMenu(MenuType.PauseMenu);
	}

	private void Pause()
	{
		if (canvas.OpenMenuCloseOther(MenuType.PauseMenu))
		{
			cursor.SetLayer(GamepadCursorLayer.PauseMenu);
			cursor.FocusMagnet(returnToGameMagnet);
			button_save.interactable = true;
			button_save_diconnected.interactable = true;
			OnLocalizeEvent();
			HandleToggleUnstuckPanel();
			if (Raft_Network.InSinglePlayerMode)
			{
				Time.timeScale = 0f;
			}
		}
	}

	private void Update()
	{
		if (PlayerItemManager.IsBusy || LoadSceneManager.IsLoadingScene)
		{
			return;
		}
		if (canvas == null)
		{
			canvas = ComponentManager<CanvasHelper>.Value;
			return;
		}
		lobbyMagnet.gameObject.SetActive(Raft_Network.CurrentRequestJoinAuthSetting != RequestJoinAuthSetting.ALLOW_NONE);
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			gamepadOnly[0].SetActive(!lobby.IsOpen && !settings.IsOpen);
		}
		if (CanvasHelper.ActiveMenu == MenuType.None)
		{
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputPause.action, KeyCode.Escape) && !canvas.hostDisconnected)
			{
				Pause();
			}
		}
		else if (CanvasHelper.ActiveMenu == MenuType.PauseMenu && !yesNoPanel.activeInHierarchy && !yesNoPanelUnstuck.activeInHierarchy && !savePanel.activeInHierarchy && !settings.IsOpen && !lobby.IsOpen && !ignoreCancel && !errorPopup.gameObject.activeInHierarchy && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputCancel.action, KeyCode.Escape) && !canvas.hostDisconnected)
		{
			canvas.CloseMenu(MenuType.PauseMenu);
		}
		ignoreCancel = false;
	}

	public void OnLocalizeEvent()
	{
		string text = "<UNKNOWN>";
		if (network.HostID == 1uL)
		{
			text = network.GetLocalPlayer().visualName;
		}
		else
		{
			Network_Player playerFromID = network.GetPlayerFromID(network.HostID);
			if (playerFromID != null)
			{
				text = playerFromID.visualName;
			}
		}
		text_save.text = Helper.GetTerm("Menu/SaveWorld");
		text_hostGameMode.text = Helper.GetTerm("Menu/Host") + ": " + text + "\n" + Helper.GetTerm("Difficulty") + ": " + Helper.GetTerm("Difficulty/" + GameManager.GameMode);
		if (Raft_Network.IsHost && GameManager.Crossplay && network.WorldCode != "")
		{
			text_worldCode.gameObject.SetActive(value: true);
			text_worldCode.text = Helper.GetTerm("Menu/WorldCode") + ":" + network.WorldCode;
		}
		else
		{
			text_worldCode.gameObject.SetActive(value: false);
		}
	}

	public void ContinuePlaying()
	{
		canvas.CloseMenu(MenuType.PauseMenu);
	}

	public void SaveGame(bool sendRemoteMessage)
	{
		button_save.interactable = false;
		button_save_diconnected.interactable = false;
		text_save.text = Helper.GetTerm("Menu/WorldSaved");
		text_save_diconnected.text = Helper.GetTerm("Menu/WorldSaved");
		saveandload.SaveGame(sendRemoteMessage);
	}

	public void Button_SaveGame()
	{
		SaveGame(sendRemoteMessage: true);
	}

	public void OpenOptionMenu()
	{
		settings.Toggle();
		unstuckPanel.SetActive(value: false);
	}

	public void OpenLobby()
	{
		lobby.Open();
	}

	public void LobbyClosed()
	{
		ignoreCancel = true;
		cursor.FocusMagnet(lobbyMagnet);
	}

	public void ExitToMainMenuButton()
	{
		exitScene = SceneName.Lobby;
		yesNoPanel.gameObject.SetActive(value: true);
		cursor.FocusMagnet(yesNoPanel.GetComponentInChildren<UiMagnet>());
		buttonHolderPanel.gameObject.SetActive(value: false);
		hostDisconnectedPanel.SetActive(value: false);
	}

	public void ExitApplicationButton()
	{
		exitScene = SceneName.Exit;
		yesNoPanel.gameObject.SetActive(value: true);
		cursor.FocusMagnet(yesNoPanel.GetComponentInChildren<UiMagnet>());
		buttonHolderPanel.gameObject.SetActive(value: false);
		hostDisconnectedPanel.SetActive(value: false);
	}

	public void HandleToggleUnstuckPanel()
	{
		Network_Player value = ComponentManager<Network_Player>.Value;
		if (!(value != null))
		{
			return;
		}
		Unstuck_Tracker unstuckTracker = value.UnstuckTracker;
		if (unstuckTracker != null)
		{
			unstuckPanel.SetActive(unstuckTracker.IsConsideredStuck);
			if (unstuckTracker.IsConsideredStuck)
			{
				cursor.FocusMagnet(unstuckPanel.GetComponentInChildren<UiMagnet>());
			}
		}
	}

	public void UnstuckLocalPlayer()
	{
		Network_Player value = ComponentManager<Network_Player>.Value;
		bool flag = true;
		if (value != null)
		{
			Unstuck_Tracker unstuckTracker = value.UnstuckTracker;
			if (unstuckTracker != null)
			{
				unstuckTracker.Unstuck();
				HandleToggleUnstuckPanel();
			}
			else
			{
				flag = false;
			}
		}
		else
		{
			flag = false;
		}
		if (!flag)
		{
			Debug.LogError("failed in unstucking player... major error in unstuck ");
		}
	}

	public void ShowDisconnectPopup()
	{
		canvas.hostDisconnected = true;
		hostDisconnectedPanel.SetActive(value: true);
		cursor.FocusMagnet(hostDisconnectedPanel.GetComponentInChildren<UiMagnet>());
		canvas.OpenMenuCloseOther(MenuType.PauseMenu);
		buttonHolderPanel.SetActiveSafe(value: false);
		button_save.interactable = true;
		button_save_diconnected.interactable = true;
		Time.timeScale = 0f;
	}

	public void Button_Exit_WithSave()
	{
		Time.timeScale = 1f;
		canvas.hostDisconnected = false;
		hostDisconnectedPanel.SetActive(value: false);
		Helper.LogBuild("Button_Exit_WithSave: " + exitScene);
		network.LeaveGame(DisconnectReason.SelfDisconnected, exitScene, saveGame: true);
	}

	public void Button_Exit_WithoutSave()
	{
		Time.timeScale = 1f;
		canvas.hostDisconnected = false;
		hostDisconnectedPanel.SetActive(value: false);
		Helper.LogBuild("Button_Exit_WithOutSave: " + exitScene);
		network.LeaveGame(DisconnectReason.SelfDisconnected, exitScene, saveGame: false);
	}

	public void Button_YesExit()
	{
		if (!Raft_Network.IsHost)
		{
			SaveGame(sendRemoteMessage: false);
		}
		if (HasSaved)
		{
			Button_Exit_WithoutSave();
			return;
		}
		savePanel.SetActive(value: true);
		cursor.FocusMagnet(savePanel.GetComponentInChildren<UiMagnet>());
		yesNoPanel.SetActive(value: false);
	}

	public void Button_NoExit()
	{
		CloseExitBox();
	}

	public void CloseExitBox()
	{
		yesNoPanel.SetActive(value: false);
		savePanel.SetActive(value: false);
		if (canvas.hostDisconnected)
		{
			hostDisconnectedPanel.SetActive(value: true);
			cursor.FocusMagnet(hostDisconnectedPanel.GetComponentInChildren<UiMagnet>());
			buttonHolderPanel.SetActive(value: false);
		}
		else
		{
			buttonHolderPanel.SetActive(value: true);
			cursor.FocusMagnet((exitScene == SceneName.Lobby) ? mainMenuMagnet : exitGameMagnet);
			hostDisconnectedPanel.SetActive(value: false);
		}
	}

	public void Button_Unstuck()
	{
		yesNoPanelUnstuck.SetActive(value: true);
		cursor.FocusMagnet(yesNoPanelUnstuck.GetComponentInChildren<UiMagnet>());
		buttonHolderPanel.SetActiveSafe(value: false);
	}

	public void Button_YesUnstuck()
	{
		UnstuckLocalPlayer();
		Button_CloseUnstuck();
	}

	public void Button_NoUnstuck()
	{
		Button_CloseUnstuck();
	}

	public void Button_CloseUnstuck()
	{
		yesNoPanelUnstuck.SetActive(value: false);
		buttonHolderPanel.SetActiveSafe(value: true);
		cursor.FocusMagnet(buttonHolderPanel.GetComponentInChildren<UiMagnet>());
	}

	public void MenuClose()
	{
		pausePanel.SetActive(value: false);
		yesNoPanel.SetActive(value: false);
		yesNoPanelUnstuck.SetActive(value: false);
		savePanel.SetActive(value: false);
		canvas.hostDisconnected = false;
		hostDisconnectedPanel.SetActive(value: false);
	}

	public void IgnoreDisconnect()
	{
		hostDisconnectedPanel.SetActive(value: false);
		canvas.CloseMenu(MenuType.PauseMenu);
	}

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			Time.timeScale = 1f;
		}
	}
}
