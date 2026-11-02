using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using I2.Loc;
using Steamworks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Settings : MenuBox, SceneEventInterface
{
	public static int AppBuildID;

	public static int SaveVersion = 10199006;

	public static Action OnSettingsChanged;

	public static Action OnSettingsClosed;

	public static bool HelperLogToEditor = false;

	[SerializeField]
	private bool helperLogToEditor;

	public Action openCredits;

	public ControlsSettingsBox controls;

	public GraphicsSettingsBox graphicsBox;

	public ConsoleControlsSettingsBox consoleControls;

	public ConsoleGraphicsSettingsBox consoleGraphicsBox;

	public AudioSettingsBox audioBox;

	[SerializeField]
	private GameObject optionsCanvas;

	[SerializeField]
	private RectTransform panel;

	[SerializeField]
	private RectTransform mainMenuPos;

	[SerializeField]
	private RectTransform inGamePos;

	[SerializeField]
	private TabGroup tabGroup;

	private static bool initialized = false;

	private OptionsMenuBox[] optionMenuboxes;

	private RGD_Settings settings;

	[SerializeField]
	private EventTrigger[] dropdownTriggers;

	public static string VersionNumberText => "Version " + Application.version + " (" + AppBuildID + ")";

	private string SavePath => SaveAndLoad.GetSettingsPath() + "Settings.rgd";

	public override bool IsOpen => optionsCanvas.activeInHierarchy;

	public RGD_Settings Current
	{
		get
		{
			if (settings == null)
			{
				settings = Load();
			}
			return settings;
		}
	}

	protected override void OnControlChanged(bool gamepad)
	{
		base.OnControlChanged(gamepad);
		EventTrigger[] array = dropdownTriggers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = gamepad;
		}
		if (tabGroup.SelectedTabButton != null && IsOpen)
		{
			tabGroup.SelectedTabButton.Tab.GetComponent<OptionsMenuBox>().SetFocus(gamepad);
		}
	}

	private void Awake()
	{
		if (initialized)
		{
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		initialized = true;
		DontDestroyOnLoadManager.AddGameObject(gameObject);
		ComponentManager<Settings>.Value = this;
		optionMenuboxes = GetComponentsInChildren<OptionsMenuBox>(includeInactive: true);
		LocalizationManager.OnLocalizeEvent += OnLocalize;
		HelperLogToEditor = helperLogToEditor;
	}

	public void Start()
	{
		AppBuildID = SteamApps.GetAppBuildId();
		StartBoxes();
		LoadAllBoxes();
	}

	private void InitialLoad()
	{
		StartBoxes();
		LoadAllBoxes();
	}

	private void CheckCommandLine()
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		for (int i = 0; i < commandLineArgs.Length; i++)
		{
			_ = commandLineArgs[i] == "-screen-fullscreen";
		}
	}

	public override void Update()
	{
		base.Update();
		OptionsMenuBox[] array = optionMenuboxes;
		foreach (OptionsMenuBox optionsMenuBox in array)
		{
			if (optionsMenuBox.GetBoxVisibility() == BoxVisibility.Pc || optionsMenuBox.GetBoxVisibility() == BoxVisibility.Both)
			{
				optionsMenuBox.SettingsUpdate();
			}
		}
	}

	private void LocalizeItemsDirectly()
	{
		foreach (Item_Base allItem in ItemManager.GetAllItems())
		{
			ItemInstance_Inventory settings_Inventory = allItem.settings_Inventory;
			string term = Helper.GetTerm(settings_Inventory.LocalizationTerm);
			if (!string.IsNullOrEmpty(term) && term.Contains('@'))
			{
				string[] array = term.Split('@');
				settings_Inventory.DisplayName = array[0];
				settings_Inventory.Description = array[1];
			}
		}
	}

	private IEnumerator LocalizeItems()
	{
		yield return ItemManager.PreloadAllItems();
		foreach (Item_Base allItem in ItemManager.GetAllItems())
		{
			ItemInstance_Inventory settings_Inventory = allItem.settings_Inventory;
			string term = Helper.GetTerm(settings_Inventory.LocalizationTerm);
			if (!string.IsNullOrEmpty(term) && term.Contains('@'))
			{
				string[] array = term.Split('@');
				settings_Inventory.DisplayName = array[0];
				settings_Inventory.Description = array[1];
			}
		}
	}

	private void OnLocalize()
	{
		LocalizeItemsDirectly();
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			OnSettingsChanged = null;
		}
	}

	public override void Open()
	{
		base.Open();
		if (!optionsCanvas.activeInHierarchy)
		{
			optionsCanvas.SetActive(value: true);
			LoadAllBoxes();
			SetCorrectPosition();
		}
		if (tabGroup.SelectedTabButton != null)
		{
			tabGroup.SelectedTabButton.Tab.GetComponent<OptionsMenuBox>().SetFocus(SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad);
		}
	}

	public void OpenCredits()
	{
		openCredits?.Invoke();
	}

	public override void Close()
	{
		if (optionsCanvas.activeInHierarchy)
		{
			optionsCanvas.SetActive(value: false);
			SaveAll();
		}
		OnSettingsClosed?.Invoke();
		base.Close();
	}

	public override void Toggle()
	{
		if (optionsCanvas.activeInHierarchy)
		{
			Close();
		}
		else
		{
			Open();
		}
	}

	public void ToggleMenuBox(MenuBox menubox)
	{
		menubox.Toggle();
	}

	private void StartBoxes()
	{
		OptionsMenuBox[] array = optionMenuboxes;
		foreach (OptionsMenuBox optionsMenuBox in array)
		{
			if (optionsMenuBox.GetBoxVisibility() == BoxVisibility.Pc || optionsMenuBox.GetBoxVisibility() == BoxVisibility.Both)
			{
				optionsMenuBox.SettingStart();
			}
		}
	}

	private void SetCorrectPosition()
	{
		if (Raft_Network.InMenuScene)
		{
			panel.localPosition = mainMenuPos.localPosition;
		}
		else
		{
			panel.localPosition = inGamePos.localPosition;
		}
	}

	public void ForceSaveAudio()
	{
		if (settings == null)
		{
			Debug.Log("Attempting to save with no settings, will default the settings");
			settings = Current;
		}
		audioBox.Save(settings);
		Save(settings);
	}

	private void SaveAll()
	{
		if (settings == null)
		{
			Debug.Log("Attempting to save with no settings, will default the settings");
			settings = Current;
		}
		OptionsMenuBox[] array = optionMenuboxes;
		foreach (OptionsMenuBox optionsMenuBox in array)
		{
			if (optionsMenuBox.GetBoxVisibility() == BoxVisibility.Pc || optionsMenuBox.GetBoxVisibility() == BoxVisibility.Both)
			{
				optionsMenuBox.Save(settings);
			}
		}
		Save(settings);
		OnSettingsChanged?.Invoke();
	}

	public void Save(RGD_Settings settings)
	{
		Debug.Log("Saving settings " + SavePath);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = FileManager.CreateFile(SavePath);
		if (fileStream != null)
		{
			binaryFormatter.Serialize(fileStream, settings);
			fileStream.Close();
		}
		else
		{
			Debug.Log("Unable to save settings file");
		}
	}

	private void LoadAllBoxes()
	{
		Load();
		OptionsMenuBox[] array = optionMenuboxes;
		foreach (OptionsMenuBox optionsMenuBox in array)
		{
			if (optionsMenuBox.GetBoxVisibility() == BoxVisibility.Pc || optionsMenuBox.GetBoxVisibility() == BoxVisibility.Both)
			{
				optionsMenuBox.Load(Current);
			}
		}
	}

	public void LoadAsync()
	{
	}

	public RGD_Settings Load()
	{
		Debug.Log("Loading settings " + SavePath);
		if (File.Exists(SavePath))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = FileManager.OpenFile(SavePath, FileMode.Open);
			if (fileStream == null)
			{
				Helper.LogBuild("Unable to open settings file, does it have read&write permissions?");
			}
			else
			{
				settings = null;
				try
				{
					settings = (RGD_Settings)binaryFormatter.Deserialize(fileStream);
				}
				catch (Exception message)
				{
					Debug.Log(message);
					fileStream.Close();
				}
				fileStream.Close();
			}
		}
		if (settings == null)
		{
			settings = new RGD_Settings();
		}
		return settings;
	}
}
