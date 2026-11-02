using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class StartMenuScreen : MonoBehaviour
{
	public JoinGameBox joinGameBox;

	[SerializeField]
	private CharacterBox characterBox;

	[SerializeField]
	private CharacterBox characterBoxConsole;

	[SerializeField]
	private JoinGameBoxConsole joinGameBoxConsole;

	[SerializeField]
	private LoadGameBox loadGameBox;

	[SerializeField]
	private CreditsBox creditsBox;

	[SerializeField]
	private Text versionText;

	[SerializeField]
	private Transform rotationTransform;

	[SerializeField]
	private Selectable settingsButton;

	[SerializeField]
	private Selectable newGameButton;

	[SerializeField]
	private GameObject discordBtn;

	[SerializeField]
	private Text signedInUser;

	[SerializeField]
	private PopupWindow errorPopup;

	[SerializeField]
	private GameObject menuButtons;

	[SerializeField]
	private LocalizationParameters locParams;

	private MenuBox[] menuBoxes;

	private Settings settings;

	private GraphicRaycaster graphicRaycaster;

	public static bool PlayerPrefsInitialized;

	public static UnityAction OnPlayerPrefsInitialized;

	public static bool firstTimeInputActivate;

	private void Awake()
	{
		PlayerPrefsInitialized = true;
		Settings.OnSettingsClosed = (Action)Delegate.Combine(Settings.OnSettingsClosed, new Action(OnSettingsClosed));
	}

	private void Start()
	{
		graphicRaycaster = GetComponent<GraphicRaycaster>();
		settings = ComponentManager<Settings>.Value;
		menuBoxes = GetComponentsInChildren<MenuBox>(includeInactive: true);
		versionText.text = Settings.VersionNumberText;
		CustomInputConfig instance = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance;
		instance.OnControlsChangedEvent = (Action<bool>)Delegate.Combine(instance.OnControlsChangedEvent, new Action<bool>(ControlsChangedEvent));
		Invoke("LateStart", 0.5f);
	}

	private void ControlsChangedEvent(bool gamepad)
	{
		bool flag = settings.IsOpen;
		if (!settings.IsOpen)
		{
			MenuBox[] array = menuBoxes;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].IsOpen)
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			return;
		}
		if (gamepad)
		{
			if (!EventSystem.current.currentSelectedGameObject || !EventSystem.current.currentSelectedGameObject.transform.parent || EventSystem.current.currentSelectedGameObject.transform.parent.name != "MenuButtons")
			{
				newGameButton.Select();
			}
		}
		else
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
	}

	private void DelayedLateStart()
	{
		Invoke("LateStart", 0.5f);
	}

	private void OpenCreditsBox()
	{
		ShowBox(creditsBox);
	}

	private void LateStart()
	{
		menuButtons.SetActive(value: true);
		characterBox.Open();
		characterBox.Close();
		newGameButton.Select();
		string text = ComponentManager<Raft_Network>.Value.PopErrorMessage();
		if (text != "")
		{
			Debug.LogError("Error Popup: " + text);
			errorPopup.gameObject.SetActive(value: true);
			errorPopup.SetMessage(text);
		}
		SimpleMonoBehaviourSingleton<HapticManager>.Instance?.ResetHaptics();
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
	}

	private void OnDestroy()
	{
		Settings.OnSettingsClosed = (Action)Delegate.Remove(Settings.OnSettingsClosed, new Action(OnSettingsClosed));
		Settings settings = this.settings;
		settings.openCredits = (Action)Delegate.Remove(settings.openCredits, new Action(OpenCreditsBox));
		if ((bool)SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance)
		{
			CustomInputConfig instance = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance;
			instance.OnControlsChangedEvent = (Action<bool>)Delegate.Remove(instance.OnControlsChangedEvent, new Action<bool>(ControlsChangedEvent));
		}
	}

	private void Update()
	{
		if (SplashScreen.isFinished && !firstTimeInputActivate)
		{
			SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.SwitchCurrentActionMap("UI");
			SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.EnableInput();
			firstTimeInputActivate = true;
		}
		if (graphicRaycaster != null)
		{
			graphicRaycaster.enabled = !LoadSceneManager.IsLoadingScene;
		}
		if (rotationTransform != null)
		{
			Vector3 localEulerAngles = rotationTransform.localEulerAngles;
			localEulerAngles.y = 0f;
			rotationTransform.localEulerAngles = localEulerAngles;
		}
	}

	public void ExitApplication()
	{
		Application.Quit();
	}

	public void ShowBox(MenuBox box)
	{
		if (box is JoinGameBox)
		{
			box = joinGameBoxConsole;
		}
		if (!box.gameObject.activeInHierarchy)
		{
			HideAllBoxes();
		}
		box.Open();
	}

	private void ShowJoinGameBoxWithoutPremiumCheck(MenuBox box)
	{
		if (!box.gameObject.activeInHierarchy)
		{
			HideAllBoxes();
		}
		box.Open();
	}

	public void ToggleGameObject(GameObject obj)
	{
		obj.SetActive(!obj.activeInHierarchy);
	}

	public void ToggleOptionsMenu()
	{
		HideAllBoxes();
		settings.Toggle();
		settingsButton.GetComponent<SelectableMenuItem>()?.KeepActive();
	}

	private void OnSettingsClosed()
	{
		settingsButton.GetComponent<SelectableMenuItem>()?.ReleaseActive();
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			settingsButton.Select();
		}
		else
		{
			settingsButton.GetComponent<SelectableMenuItem>()?.OnDeselect(null);
		}
	}

	public void OnPasswordBoxClosed()
	{
		newGameButton.Select();
	}

	private void HideAllBoxes()
	{
		MenuBox[] array = menuBoxes;
		foreach (MenuBox menuBox in array)
		{
			if (menuBox.IsOpen)
			{
				menuBox.Close();
			}
		}
		if (settings.IsOpen)
		{
			settings.Close();
		}
	}
}
