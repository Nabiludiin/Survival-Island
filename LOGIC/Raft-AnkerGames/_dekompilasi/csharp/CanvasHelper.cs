using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CanvasHelper : MonoBehaviour, SceneEventInterface
{
	public delegate void OnMenu(MenuType menuType);

	public static MenuType ActiveMenu;

	[SerializeField]
	private GameMenu[] gameMenus;

	[SerializeField]
	private Canvas[] canvases;

	public GameObject StatSliderParent;

	public GameObject bonusHealthBackground;

	public GameObject bonusHungerBackground;

	public GameObject bonusThirstBackground;

	public UISlider_Stat healthSlider;

	public UISlider_Stat bonusHealthSlider;

	public UISlider_Stat thirstSlider;

	public UISlider_Stat bonusThirstSlider;

	public UISlider_Stat hungerSlider;

	public UISlider_Stat bonusHungerSlider;

	public GameObject fatigueParent;

	public Image fatigueBackground;

	public Image fatigueRadial;

	private Material fatigueBackgroundMaterialInst;

	private Material fatigueMeterMaterialInst;

	public Image removeBlockRadialImage;

	public Animator loadCircleAnimator;

	public Animator aimCircleAnimator;

	public DisplayTextManager displayTextManager;

	public VoiceOverlay voiceOverlay;

	public Image centerAim;

	[SerializeField]
	private Sprite defaultAimSprite;

	[SerializeField]
	private Sprite gatherHookAimSprite;

	[SerializeField]
	private Sprite gatherShovelAimSprite;

	[SerializeField]
	private Sprite gatherShearAimSprite;

	[SerializeField]
	private Sprite gatherBucketAimSprite;

	[SerializeField]
	private Sprite gatherMacheteAimSprite;

	[SerializeField]
	private Sprite gatherWoodAimSprite;

	public GameObject binocularImage;

	public GameObject hazmatSuitImage;

	public GameObject saveGameIcon;

	public Text dropText;

	public FadePanel genericBlackFade;

	public bool hardOverwriteNoteBookUIDisplayed;

	public GameObject notebookHelpHotkeyParent;

	public GameObject craftingMenuHelpHotkeyParent;

	public TextMeshProUGUI notebookHotkeyText;

	public TextMeshProUGUI craftingMenuHotkeyText;

	[NonSerialized]
	public bool hostDisconnected;

	private SoundManager soundManager;

	private Network_Player playerNetwork;

	private bool canOpenMenu = true;

	public bool UIState = true;

	public Vector3 CursorPos
	{
		get
		{
			if (PlayerInput.GetPlayerByIndex(0).currentControlScheme == "Gamepad")
			{
				return GetComponent<GamepadCursor>().CursorPos;
			}
			return Input.mousePosition;
		}
	}

	public static bool Gamepad
	{
		get
		{
			if ((bool)PlayerInput.GetPlayerByIndex(0) && PlayerInput.GetPlayerByIndex(0).currentControlScheme == "Gamepad")
			{
				return true;
			}
			return false;
		}
	}

	public bool CanOpenMenu
	{
		get
		{
			if (!canOpenMenu)
			{
				return false;
			}
			if (playerNetwork == null)
			{
				playerNetwork = ComponentManager<Raft_Network>.Value.GetLocalPlayer();
			}
			if (playerNetwork == null)
			{
				return false;
			}
			if (!playerNetwork.IsLocalPlayer)
			{
				return false;
			}
			if (PlayerItemManager.IsBusy)
			{
				return false;
			}
			if (Player.LocalPlayerIsDead)
			{
				return false;
			}
			if (playerNetwork.RessurectComponent.IsCarrying || playerNetwork.RessurectComponent.BeingCarried)
			{
				return false;
			}
			if (playerNetwork.BedComponent.Sleeping)
			{
				return false;
			}
			return true;
		}
	}

	public static event OnMenu MenuPreCloseEvent;

	public static event OnMenu MenuCloseEvent;

	public static event OnMenu MenuOpenEvent;

	private void Awake()
	{
		ComponentManager<CanvasHelper>.Value = this;
		fatigueBackgroundMaterialInst = UnityEngine.Object.Instantiate(fatigueBackground.material);
		fatigueMeterMaterialInst = UnityEngine.Object.Instantiate(fatigueRadial.material);
		fatigueBackground.material = fatigueBackgroundMaterialInst;
		fatigueRadial.material = fatigueMeterMaterialInst;
		SetLoadCircle(state: false);
		SetFatigueSlider(1f);
		loadCircleAnimator = removeBlockRadialImage.transform.GetComponent<Animator>();
		aimCircleAnimator = centerAim.transform.GetComponent<Animator>();
	}

	private void Start()
	{
		hostDisconnected = false;
		playerNetwork = ComponentManager<Raft_Network>.Value.GetLocalPlayer();
		soundManager = ComponentManager<SoundManager>.Value;
		for (int i = 0; i < gameMenus.Length; i++)
		{
			gameMenus[i].Initialize();
		}
		SetHotkeyElementsText();
		Raft_Network.OnWorldReceivedLate = (Action)Delegate.Remove(Raft_Network.OnWorldReceivedLate, new Action(OnWorldRecievedLate));
		Raft_Network.OnWorldReceivedLate = (Action)Delegate.Combine(Raft_Network.OnWorldReceivedLate, new Action(OnWorldRecievedLate));
		Settings.OnSettingsChanged = (Action)Delegate.Combine(Settings.OnSettingsChanged, new Action(UpdateHelpHotkeyElements));
		ActiveMenu = MenuType.None;
	}

	private void OnDestroy()
	{
		playerNetwork = null;
		soundManager = null;
		Settings.OnSettingsChanged = (Action)Delegate.Remove(Settings.OnSettingsChanged, new Action(UpdateHelpHotkeyElements));
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			UIState = true;
			MenuCloseEvent = null;
			MenuOpenEvent = null;
			ActiveMenu = MenuType.None;
			StopCoroutine("ShowSaveGameIcon");
			saveGameIcon.SetActive(value: false);
		}
	}

	private void OnWorldRecievedLate()
	{
		UpdateHelpHotkeyElements();
	}

	private void Update()
	{
		if (ActiveMenu == MenuType.None && !PlayerItemManager.IsBusy && MyInput.GetButtonDown("UIState"))
		{
			SetUIState(!UIState);
		}
	}

	public GameMenu GetMenu(MenuType menuType)
	{
		GameMenu[] array = gameMenus;
		foreach (GameMenu gameMenu in array)
		{
			if (gameMenu.menuType == menuType)
			{
				return gameMenu;
			}
		}
		return null;
	}

	public bool OpenMenuCloseOther(MenuType menuType, bool force = false)
	{
		if (!force && !CanOpenMenu)
		{
			return false;
		}
		GameMenu[] array = gameMenus;
		foreach (GameMenu gameMenu in array)
		{
			if (gameMenu.IsOpen)
			{
				gameMenu.Close();
				if (MenuCloseEvent != null)
				{
					MenuCloseEvent(gameMenu.menuType);
				}
			}
		}
		return OpenMenu(menuType, force);
	}

	public bool OpenMenu(MenuType menuType, bool force = false)
	{
		if (!force && !CanOpenMenu)
		{
			return false;
		}
		bool flag = false;
		GameMenu[] array = gameMenus;
		foreach (GameMenu gameMenu in array)
		{
			if (gameMenu.menuType == menuType && !gameMenu.IsOpen)
			{
				flag = true;
				gameMenu.Open();
				if (MenuOpenEvent != null)
				{
					MenuOpenEvent(gameMenu.menuType);
				}
			}
		}
		if (flag)
		{
			ActiveMenu = menuType;
			Helper.SetCursorVisibleAndLockState(state: true, CursorLockMode.None);
			SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.SwitchCurrentActionMap("UI");
			if (menuType != MenuType.Piano && !ButtonSound.Mute)
			{
				soundManager.PlayUI_OpenMenu();
			}
			SetUIState(state: true);
		}
		return flag;
	}

	public void CloseMenu(MenuType menuType)
	{
		bool flag = false;
		GameMenu[] array = gameMenus;
		foreach (GameMenu gameMenu in array)
		{
			if (gameMenu.menuType != menuType || !gameMenu.IsOpen)
			{
				continue;
			}
			if (MenuPreCloseEvent != null)
			{
				MenuPreCloseEvent(menuType);
			}
			gameMenu.Close();
			if (!flag)
			{
				flag = true;
				canOpenMenu = false;
				Invoke("ResetCanOpen", 0.2f);
				if (MenuCloseEvent != null)
				{
					MenuCloseEvent(menuType);
				}
				ActiveMenu = MenuType.None;
				Helper.SetCursorVisibleAndLockState(state: false, CursorLockMode.Locked);
				SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.SwitchCurrentActionMap("Player");
			}
		}
		SetUIState(state: true);
	}

	public void CloseAllMenus()
	{
		GameMenu[] array = gameMenus;
		foreach (GameMenu gameMenu in array)
		{
			if (gameMenu.IsOpen)
			{
				gameMenu.Close();
				if (MenuCloseEvent != null)
				{
					MenuCloseEvent(gameMenu.menuType);
				}
			}
		}
		ActiveMenu = MenuType.None;
		Helper.SetCursorVisibleAndLockState(state: false, CursorLockMode.Locked);
		SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.SwitchCurrentActionMap("Player");
		canOpenMenu = false;
		Invoke("ResetCanOpen", 0.2f);
		SetUIState(state: true);
	}

	public void OnControlsChanged(bool gamepad)
	{
		if (ActiveMenu == MenuType.Inventory)
		{
			OpenMenuCloseOther(MenuType.Inventory, force: true);
		}
		SetHotkeyElementsText();
	}

	private void ResetCanOpen()
	{
		canOpenMenu = true;
	}

	public void SetFatigueSlider(float value)
	{
		bool flag = value < 1f;
		fatigueParent.gameObject.SetActiveSafe(flag);
		if (flag)
		{
			fatigueRadial.fillAmount = value;
			if (value > 0f)
			{
				fatigueMeterMaterialInst.SetFloat("_FatigueValue", 1f - value);
				fatigueBackgroundMaterialInst.SetFloat("_FatigueValue", 0f);
			}
			else
			{
				fatigueMeterMaterialInst.SetFloat("_FatigueValue", 1f);
				fatigueBackgroundMaterialInst.SetFloat("_FatigueValue", 1f);
			}
		}
	}

	public void SetLoadCircle(bool state)
	{
		if (removeBlockRadialImage != null)
		{
			removeBlockRadialImage.gameObject.SetActiveSafe(state);
		}
	}

	public void SetLoadCircle(float value)
	{
		if (value > 0f)
		{
			SetLoadCircle(state: true);
			if (removeBlockRadialImage != null)
			{
				removeBlockRadialImage.fillAmount = value;
			}
		}
		else
		{
			SetLoadCircle(state: false);
		}
	}

	public void SetLoadCircleAnimationPulse()
	{
		if (loadCircleAnimator != null)
		{
			loadCircleAnimator.SetTrigger("Pulse");
		}
	}

	public void SetAimCircleAnimationPulse()
	{
		if (aimCircleAnimator != null)
		{
			aimCircleAnimator.SetTrigger("Pulse");
		}
	}

	public void ResetLoadAndAimCircleAnimationPulse()
	{
		if (aimCircleAnimator != null)
		{
			aimCircleAnimator.ResetTrigger("Pulse");
		}
		if (loadCircleAnimator != null)
		{
			loadCircleAnimator.ResetTrigger("Pulse");
		}
	}

	public void SetAimSprite(AimSprite aimSprite)
	{
		switch (aimSprite)
		{
		case AimSprite.Default:
			if (centerAim.sprite != defaultAimSprite)
			{
				centerAim.sprite = defaultAimSprite;
			}
			break;
		case AimSprite.Mineable:
			if (centerAim.sprite != gatherHookAimSprite)
			{
				centerAim.sprite = gatherHookAimSprite;
			}
			break;
		case AimSprite.Shovelable:
			if (centerAim.sprite != gatherShovelAimSprite)
			{
				centerAim.sprite = gatherShovelAimSprite;
			}
			break;
		case AimSprite.Shearable:
			if (centerAim.sprite != gatherShearAimSprite)
			{
				centerAim.sprite = gatherShearAimSprite;
			}
			break;
		case AimSprite.Bucketable:
			if (centerAim.sprite != gatherBucketAimSprite)
			{
				centerAim.sprite = gatherBucketAimSprite;
			}
			break;
		case AimSprite.Macheteable:
			if (centerAim.sprite != gatherMacheteAimSprite)
			{
				centerAim.sprite = gatherMacheteAimSprite;
			}
			break;
		case AimSprite.Choppable:
			if (centerAim.sprite != gatherWoodAimSprite)
			{
				centerAim.sprite = gatherWoodAimSprite;
			}
			break;
		}
	}

	public void SaveGameIcon(float timeOnScreen)
	{
		StopCoroutine("ShowSaveGameIcon");
		StartCoroutine(ShowSaveGameIcon(timeOnScreen));
	}

	private IEnumerator ShowSaveGameIcon(float timeOnScreen)
	{
		saveGameIcon.SetActive(value: true);
		yield return new WaitForSecondsRealtime(timeOnScreen);
		saveGameIcon.SetActive(value: false);
	}

	public void SetUIState(bool state)
	{
		Canvas[] array = canvases;
		foreach (Canvas canvas in array)
		{
			if (canvas != null)
			{
				canvas.enabled = state;
			}
		}
		UIState = state;
	}

	public void UpdateHelpHotkeyElements()
	{
		SetHotKeyElementsDisplayed(GeneralSettingsBox.HelpfulUIVisible);
		SetHotkeyElementsText();
	}

	public void SetHotKeyElementsDisplayed(bool value)
	{
		craftingMenuHelpHotkeyParent.SetActiveSafe(value);
		if (!hardOverwriteNoteBookUIDisplayed)
		{
			notebookHelpHotkeyParent.SetActiveSafe(value);
		}
		else
		{
			notebookHelpHotkeyParent.SetActiveSafe(value: false);
		}
	}

	public void SetHotkeyElementsText()
	{
		craftingMenuHotkeyText.text = (((bool)SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad) ? PlayerInput.GetPlayerByIndex(0).actions["Inventory"].GetBindingDisplayString() : MyInput.Keybinds["Inventory"].MainKey.ToShortString());
		notebookHotkeyText.text = (((bool)SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad) ? PlayerInput.GetPlayerByIndex(0).actions["Notebook"].GetBindingDisplayString() : MyInput.Keybinds["NoteBookToggle"].MainKey.ToShortString());
	}
}
