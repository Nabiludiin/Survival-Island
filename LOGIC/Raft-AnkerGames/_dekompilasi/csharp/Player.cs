using System.Collections;
using Steamworks;
using UltimateWater;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, SceneEventInterface
{
	private static float _useDistance;

	public static float UseDistanceDefault = 2.5f;

	public static float UseDistanceThirdperson = 7.5f;

	public static bool LocalPlayerIsDead;

	public bool IsDead;

	public bool waitingForRespawn;

	public bool diedUnderWater;

	public Vector3 stuckSizeCheck;

	public Projector causticsProjector;

	[Header("Components")]
	public MouseLook mouseLookXScript;

	public MouseLook mouseLookYScript;

	public MouseLook mouseLookYCameraScript;

	public Collider characterControllerCollider;

	[SerializeField]
	private Network_Player playerNetwork;

	private PlayerStats playerStats;

	[HideInInspector]
	public Collider incapacitatedCollider;

	[HideInInspector]
	public float respawnTimeStamp;

	private CanvasHelper canvas;

	private SoundManager soundManager;

	private float? fixedHealth;

	private float? fixedHunger;

	private float? fixedThurst;

	[SerializeField]
	private InputActionReference inputInventory;

	[SerializeField]
	private InputActionReference inputCancel;

	private InputAction inputView;

	private static readonly int[] Array = new int[8] { 901, 2, 17, 902, 43, 53, 60, 68 };

	private static int ReadIndex = 0;

	public static float UseDistance
	{
		get
		{
			return _useDistance;
		}
		set
		{
			_useDistance = value;
		}
	}

	private void Awake()
	{
		UseDistance = UseDistanceDefault;
		playerStats = GetComponent<PlayerStats>();
		playerNetwork = GetComponent<Network_Player>();
	}

	private void Start()
	{
		canvas = ComponentManager<CanvasHelper>.Value;
		soundManager = ComponentManager<SoundManager>.Value;
		if (playerNetwork != null && playerNetwork.currentModel != null)
		{
			incapacitatedCollider = playerNetwork.currentModel.incapacitatedCollider;
			incapacitatedCollider.enabled = false;
		}
		if (playerNetwork.IsLocalPlayer)
		{
			InitializeInventory();
			if (causticsProjector != null)
			{
				causticsProjector.enabled = true;
			}
		}
		inputView = PlayerInput.GetPlayerByIndex(0).actions["View"];
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			LocalPlayerIsDead = false;
		}
	}

	private void Update()
	{
		if (playerNetwork == null || !playerNetwork.IsLocalPlayer)
		{
			return;
		}
		if (playerNetwork.transform.position.y <= -100f)
		{
			AchievementHandler.UnlockAchievement(AchievementType.ach_diveDeep);
		}
		if (soundManager != null)
		{
			soundManager.SetOceanPlayerHeight(playerNetwork.transform.position.y);
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputInventory.action, "Inventory") && CanvasHelper.ActiveMenu == MenuType.None)
		{
			canvas.OpenMenuCloseOther(MenuType.Inventory);
		}
		else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputCancel.action, "Inventory") || (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad && MyInput.GetButtonDown("Cancel") && CanvasHelper.ActiveMenu == MenuType.Inventory && playerNetwork.StorageManager.currentStorage == null))
		{
			canvas.CloseMenu(MenuType.Inventory);
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputView, "Thirdperson") && CanvasHelper.ActiveMenu == MenuType.None && !playerNetwork.flightCamera.enabled)
		{
			playerNetwork.currentModel.thirdPersonSettings.ThirdPersonState = !playerNetwork.currentModel.thirdPersonSettings.ThirdPersonState;
		}
		if (MyInput.GetButtonDown("Drop") && !PlayerItemManager.IsBusy && CanvasHelper.ActiveMenu == MenuType.None)
		{
			playerNetwork.Inventory.DropCurrentItem();
		}
		if (MyInput.GetButtonDown("Noclip") && (GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.allowNoclip || Cheat.AllowCheatsForLocalPlayer()) && CanvasHelper.ActiveMenu == MenuType.None && !playerNetwork.currentModel.thirdPersonSettings.ThirdPersonState && !ChatTextFieldController.IsChatWindowSelected)
		{
			playerNetwork.flightCamera.Toggle(disablePlayerMovement: true);
		}
		if (!Cheat.UseCheats)
		{
			return;
		}
		PlayerInput playerByIndex = PlayerInput.GetPlayerByIndex(0);
		if (playerByIndex.currentControlScheme == "Gamepad")
		{
			if (playerByIndex.actions["TabLeft"].ReadValue<float>() != 1f || playerByIndex.actions["TabRight"].ReadValue<float>() != 1f)
			{
				return;
			}
			if (playerByIndex.actions["Jump"].WasPressedThisFrame())
			{
				Cheat.UseGodMode = !Cheat.UseGodMode;
				if (Cheat.UseGodMode)
				{
					playerStats.stat_hunger.Normal.SetToMaxValue();
					playerStats.stat_thirst.Normal.SetToMaxValue();
					playerStats.stat_health.SetToMaxValue();
					playerStats.stat_oxygen.SetToMaxValue();
				}
				ChatManager.LocalDebugChatMessage("Toggling GodMode, = " + Cheat.UseGodMode);
			}
			else if (playerByIndex.actions["Sprint"].WasPressedThisFrame())
			{
				ComponentManager<NoteBook>.Value.UnlockSpecificNoteNetworked(Array[ReadIndex], showNotification: true);
				ReadIndex = (ReadIndex + 1) % Array.Length;
			}
			else if (playerByIndex.actions["Crouch"].WasPressedThisFrame())
			{
				if (!Raft_Network.IsHost)
				{
					ChatManager.LocalDebugChatMessage("Denied: Only Host should tinker with anchoring via hotkey f5");
					return;
				}
				if (Cheat.raft.IsAnchored)
				{
					Cheat.HandleCheatCode("/set anchor false", "/", playerNetwork.steamID);
				}
				else
				{
					Cheat.HandleCheatCode("/set anchor true", "/", playerNetwork.steamID);
				}
				ChatManager.SendDebugChatMessage("Setting Raft Anchoring to :" + Cheat.raft.IsAnchored);
			}
			else if (playerByIndex.actions["Interact"].WasPressedThisFrame())
			{
				if ((GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.allowNoclip || Cheat.AllowCheatsForLocalPlayer()) && CanvasHelper.ActiveMenu == MenuType.None && !playerNetwork.currentModel.thirdPersonSettings.ThirdPersonState && !ChatTextFieldController.IsChatWindowSelected)
				{
					playerNetwork.flightCamera.Toggle(disablePlayerMovement: true);
				}
			}
			else if (playerByIndex.actions["Context"].WasPressedThisFrame())
			{
				ComponentManager<AzureSkyController>.Value.timeOfDay.GotoTime(0f);
			}
			else if (playerByIndex.actions["Inventory"].WasPressedThisFrame())
			{
				ComponentManager<AzureSkyController>.Value.timeOfDay.GotoTime(12f);
			}
			else if (playerByIndex.actions["Fire"].WasPressedThisFrame())
			{
				Cheat.LandmarkTest();
			}
			else if (playerByIndex.actions["Rotate"].WasPressedThisFrame())
			{
				Cheat.HandleCheatCode("/kit playtest", "/", playerNetwork.steamID);
			}
			else if (playerByIndex.actions["Notebook"].WasPressedThisFrame())
			{
				ComponentManager<CanvasHelper>.Value.SetUIState(!ComponentManager<CanvasHelper>.Value.UIState);
			}
			else if (playerByIndex.actions["Remove"].WasPressedThisFrame())
			{
				Cheat.HandleCheatCode("/notebook all", "/", playerNetwork.steamID);
			}
			else if (playerByIndex.actions["SkipDialog"].WasPressedThisFrame())
			{
				Cheat.UnlockAllCrafting = !Cheat.UnlockAllCrafting;
			}
			else if (playerByIndex.actions["Special"].WasPressedThisFrame())
			{
				Cheat.TeleportToRaycast();
			}
			return;
		}
		bool key = Input.GetKey(KeyCode.LeftShift);
		bool key2 = Input.GetKey(KeyCode.LeftControl);
		if (Input.GetKeyDown(KeyCode.F1))
		{
			if (Application.isEditor)
			{
				ChatManager.SendDebugChatMessage("Nuked~");
				Cheat.HandleCheatCode("/nuke", "/", playerNetwork.steamID);
			}
		}
		else if (Input.GetKeyDown(KeyCode.F2))
		{
			Cheat.UseGodMode = !Cheat.UseGodMode;
			if (Cheat.UseGodMode)
			{
				playerStats.stat_hunger.Normal.SetToMaxValue();
				playerStats.stat_thirst.Normal.SetToMaxValue();
				playerStats.stat_health.SetToMaxValue();
				playerStats.stat_oxygen.SetToMaxValue();
			}
			ChatManager.LocalDebugChatMessage("Toggling GodMode, = " + Cheat.UseGodMode);
		}
		else if (Input.GetKeyDown(KeyCode.F3))
		{
			playerNetwork.Stats.stat_health.NormalValue = 1f;
			playerNetwork.Stats.stat_thirst.Normal.NormalValue = 1f;
			playerNetwork.Stats.stat_thirst.Normal.statTarget.NormalValue = 1f;
			playerNetwork.Stats.stat_hunger.Normal.NormalValue = 1f;
			playerNetwork.Stats.stat_hunger.Normal.statTarget.NormalValue = 1f;
			playerNetwork.Stats.stat_oxygen.NormalValue = 1f;
		}
		else if (Input.GetKeyDown(KeyCode.F4))
		{
			if (!key2)
			{
				playerNetwork.Inventory.AddItem("Hammer", 1);
				playerNetwork.Inventory.AddItem("Axe", 1);
				playerNetwork.Inventory.AddItem("Scrap", 10000);
				playerNetwork.Inventory.AddItem("Rope", 10000);
				playerNetwork.Inventory.AddItem("Thatch", 10000);
				playerNetwork.Inventory.AddItem("Plastic", 10000);
				playerNetwork.Inventory.AddItem("Plank", 10000);
				playerNetwork.Inventory.AddItem("MetalIngot", 10000);
				playerNetwork.Inventory.AddItem("Nail", 10000);
			}
			else
			{
				Cheat.HandleCheatCode("/kit paintbrush", "/", playerNetwork.steamID);
			}
		}
		else if (Input.GetKeyDown(KeyCode.F5))
		{
			if (!Raft_Network.IsHost)
			{
				ChatManager.LocalDebugChatMessage("Denied: Only Host should tinker with anchoring via hotkey f5");
				return;
			}
			if (Cheat.raft.IsAnchored)
			{
				Cheat.HandleCheatCode("/set anchor false", "/", playerNetwork.steamID);
			}
			else
			{
				Cheat.HandleCheatCode("/set anchor true", "/", playerNetwork.steamID);
			}
			ChatManager.SendDebugChatMessage("Setting Raft Anchoring to :" + Cheat.raft.IsAnchored);
		}
		else if (Input.GetKeyDown(KeyCode.F9))
		{
			Cheat.ForceReflectionsOff = !Cheat.ForceReflectionsOff;
			ChatManager.LocalDebugChatMessage("Reflections = " + (Cheat.ForceReflectionsOff ? "Off" : "On"));
		}
		else if (Input.GetKeyDown(KeyCode.F11))
		{
			Cheat.HandleCheatCode("/build foundation 10", "/", playerNetwork.steamID);
		}
		if (SingletonGeneric<GameManager>.Singleton != null)
		{
			if (Input.GetKeyDown(KeyCode.F8))
			{
				Cheat.SetTargetTimeScale(Cheat.TargetTimeScale + 5f);
			}
			else if (Input.GetKeyDown(KeyCode.F6))
			{
				Cheat.SetTargetTimeScale(Cheat.TargetTimeScale - 5f);
			}
			else if (Input.GetKeyDown(KeyCode.F7))
			{
				if (Time.timeScale > 0f)
				{
					Time.timeScale = 0f;
				}
				else
				{
					Cheat.SetTargetTimeScale(1f);
					Time.timeScale = 1f;
				}
			}
			else if (Input.GetKey(KeyCode.RightArrow) && Raft_Network.IsHost)
			{
				AzureSkyController value = ComponentManager<AzureSkyController>.Value;
				float longitude = value.timeOfDay.longitude;
				longitude += (float)((!key) ? 1 : 2);
				value.timeOfDay.longitude = ((longitude > 180f) ? (-180f) : longitude);
			}
			else if (Input.GetKey(KeyCode.LeftArrow) && Raft_Network.IsHost)
			{
				AzureSkyController value2 = ComponentManager<AzureSkyController>.Value;
				float longitude2 = value2.timeOfDay.longitude;
				longitude2 -= (float)((!key) ? 1 : 2);
				value2.timeOfDay.longitude = ((longitude2 < -180f) ? 180f : longitude2);
			}
			else if (Input.GetKey(KeyCode.UpArrow) && Raft_Network.IsHost)
			{
				AzureSkyController value3 = ComponentManager<AzureSkyController>.Value;
				float hour = value3.timeOfDay.hour;
				hour = ((!key) ? (hour + 0.05f) : (hour + 0.2f));
				if (hour >= 24f)
				{
					hour -= 24f;
				}
				value3.timeOfDay.GotoTime(hour);
			}
			else if (Input.GetKey(KeyCode.DownArrow) && Raft_Network.IsHost)
			{
				AzureSkyController value4 = ComponentManager<AzureSkyController>.Value;
				float hour2 = value4.timeOfDay.hour;
				hour2 = ((!key) ? (hour2 - 0.05f) : (hour2 - 0.2f));
				if (hour2 <= 0f)
				{
					hour2 += 24f;
				}
				value4.timeOfDay.GotoTime(hour2);
			}
		}
		if (Time.timeScale != 0f)
		{
			Time.timeScale = Helper.Damp(Time.timeScale, Cheat.TargetTimeScale, 0.5f, Time.deltaTime);
		}
		if (CanvasHelper.ActiveMenu == MenuType.None && Input.GetKeyDown(KeyCode.Insert))
		{
			SetMouseLookScripts(!mouseLookXScript.enabled);
			if (mouseLookXScript.enabled)
			{
				Helper.SetCursorVisibleAndLockState(state: false, CursorLockMode.Locked);
			}
			else
			{
				Helper.SetCursorVisibleAndLockState(state: true, CursorLockMode.None);
			}
		}
	}

	public void StartRespawn(Bed bed, bool clearInventory, bool modifyStats = true)
	{
		bool flag = bed.transform.tag == "RespawnPointBed";
		playerNetwork.BedComponent.CurrentBed = bed;
		IsDead = false;
		if (playerNetwork.IsLocalPlayer)
		{
			LocalPlayerIsDead = false;
			if (!flag)
			{
				StartCoroutine(RespawnWait(3f));
			}
			respawnTimeStamp = Time.time;
			playerNetwork.PersonController.CameraSubmersionChanged(SubmersionState.None, setControllerType: true);
			playerNetwork.PersonController.SwitchControllerType(ControllerType.Ground);
			PlayerSpecificVariables playerSpecificVariables = GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables;
			if (playerSpecificVariables.clearInventoryOnSurrender && clearInventory)
			{
				if (playerSpecificVariables.leaveSomeItems)
				{
					playerNetwork.Inventory.ClearInventoryLeaveSome();
				}
				else
				{
					playerNetwork.Inventory.Clear();
				}
			}
			canvas.CloseAllMenus();
			canvas.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Inventory);
			Helper.SetCursorVisibleAndLockState(state: false, CursorLockMode.Locked);
		}
		playerNetwork.Animator.anim.SetBool("Dead", value: false);
		bed.AttachPlayer(playerNetwork);
		incapacitatedCollider.enabled = false;
		characterControllerCollider.enabled = true;
		if (modifyStats)
		{
			playerStats.stat_health.Value = playerStats.stat_health.Max * bed.HealthRespawnPercentage;
			playerStats.stat_BonusHealth.Value = 0f;
			playerStats.stat_BonusHealth.statTarget.Value = 0f;
			playerStats.stat_hunger.Normal.Value = playerStats.stat_hunger.Normal.Max * bed.HungerRespawnPercentage;
			playerStats.stat_hunger.Bonus.Value = 0f;
			playerStats.stat_hunger.Bonus.statTarget.Value = 0f;
			playerStats.stat_thirst.Normal.Value = playerStats.stat_thirst.Normal.Max * bed.ThirstRespawnPercentage;
			playerStats.stat_thirst.Bonus.Value = 0f;
			playerStats.stat_thirst.Bonus.statTarget.Value = 0f;
			fixedHealth = playerStats.stat_health.Value;
			fixedHunger = playerStats.stat_hunger.Normal.Value;
			fixedThurst = playerStats.stat_thirst.Normal.Value;
		}
		playerNetwork.HazmatSuit.UnEquip();
		if (flag)
		{
			if (Raft_Network.IsHost)
			{
				playerNetwork.Network.RPC(new Message_NetworkBehaviour(Messages.Respawn_Complete, playerNetwork), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				OnRespawnComplete();
			}
			else
			{
				playerNetwork.SendP2P(new Message_NetworkBehaviour(Messages.Respawn_Complete, playerNetwork), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			if (playerNetwork.IsLocalPlayer)
			{
				playerNetwork.PlayerScript.MakeUnStuck();
			}
		}
	}

	public void RespawnWithoutBed(bool clearInventory)
	{
		IsDead = false;
		if (playerNetwork.IsLocalPlayer)
		{
			LocalPlayerIsDead = false;
			respawnTimeStamp = Time.time;
			playerNetwork.PersonController.CameraSubmersionChanged(SubmersionState.None, setControllerType: true);
			playerNetwork.PersonController.SwitchControllerType(ControllerType.Ground);
			if (GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.clearInventoryOnSurrender && clearInventory)
			{
				playerNetwork.Inventory.Clear();
			}
			canvas.CloseAllMenus();
			Helper.SetCursorVisibleAndLockState(state: false, CursorLockMode.Locked);
		}
		playerNetwork.Animator.anim.SetBool("Dead", value: false);
		incapacitatedCollider.enabled = false;
		characterControllerCollider.enabled = true;
		playerStats.stat_health.Value = playerStats.stat_health.Max * BedManager.RespawnPointBed.HealthRespawnPercentage;
		playerStats.stat_BonusHealth.Value = 0f;
		playerStats.stat_hunger.Normal.Value = playerStats.stat_hunger.Normal.Max * BedManager.RespawnPointBed.HungerRespawnPercentage;
		playerStats.stat_hunger.Bonus.Value = 0f;
		playerStats.stat_thirst.Normal.Value = playerStats.stat_thirst.Normal.Max * BedManager.RespawnPointBed.ThirstRespawnPercentage;
		playerStats.stat_thirst.Bonus.Value = 0f;
		playerNetwork.HazmatSuit.UnEquip();
		playerNetwork.Inventory.hotbar.ReselectCurrentSlot();
		canvas.displayTextManager.HideDisplayTexts();
		playerNetwork.PersonController.IsMovementFree = true;
		playerNetwork.PersonController.enabled = true;
		if (playerNetwork.IsLocalPlayer)
		{
			PlayerItemManager.IsBusy = false;
			playerNetwork.PlayerScript.SetMouseLookScripts(canLook: true);
			playerNetwork.PlayerScript.MakeUnStuck();
			playerNetwork.Inventory.hotbar.ReselectCurrentSlot();
			playerNetwork.PersonController.ResetExternalVelocity();
		}
		playerNetwork.PersonController.controller.enabled = true;
		playerNetwork.transform.localScale = Vector3.one;
	}

	public void OnRespawnComplete()
	{
		if (playerNetwork.BedComponent.CurrentBed != null)
		{
			playerNetwork.BedComponent.CurrentBed.DettachPlayer();
			if (playerNetwork.IsLocalPlayer)
			{
				playerNetwork.Inventory.hotbar.ReselectCurrentSlot();
				canvas.displayTextManager.HideDisplayTexts();
			}
		}
	}

	public void Kill(bool replicating = false)
	{
		if (IsDead)
		{
			return;
		}
		IsDead = true;
		if (!replicating && Raft_Network.IsHost)
		{
			AnalyticsHelper.CustomEvent(AnalyticsEvent.PLAYER_DEATH);
		}
		if (playerNetwork.BedComponent.Sleeping)
		{
			playerNetwork.BedComponent.StopSleep();
		}
		if (playerNetwork.ZiplinePlayer != null)
		{
			if (playerNetwork.IsLocalPlayer)
			{
				if (playerNetwork.ZiplinePlayer.IsAttachedToZipline)
				{
					playerNetwork.ZiplinePlayer.DetachFromCurrentZipline(keepMomentum: false);
				}
			}
			else
			{
				playerNetwork.ZiplinePlayer.DetachFromCurrentZiplineClient();
			}
		}
		if (playerNetwork.NoteBookUI != null && playerNetwork.NoteBookUI.isDisplayed)
		{
			playerNetwork.NoteBookUI.SetBookActive(value: false);
		}
		diedUnderWater = playerNetwork.transform.position.y < playerNetwork.PersonController.waterFloat.PrevYPos;
		if (playerNetwork.IsLocalPlayer)
		{
			if (playerNetwork.RessurectComponent.IsCarrying)
			{
				Message_NetworkBehaviour message = new Message_NetworkBehaviour(Messages.RessurectComponent_StopCarry, playerNetwork);
				if (Raft_Network.IsHost)
				{
					playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					playerNetwork.RessurectComponent.StopCarryingPlayer(manipulatePosition: true);
				}
				else
				{
					playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
			if (!replicating)
			{
				AchievementHandler.AddPlayeDeath();
				if (playerNetwork.currentModel.thirdPersonSettings.ThirdPersonState)
				{
					playerNetwork.currentModel.thirdPersonSettings.ForceThirdPersonState(thirdPersonState: false);
				}
			}
			SetMouseLookScripts(canLook: false);
			if (canvas == null)
			{
				canvas = ComponentManager<CanvasHelper>.Value;
			}
			canvas.OpenMenuCloseOther(MenuType.DeathMenu, force: true);
			LocalPlayerIsDead = true;
			playerNetwork.PlayerItemManager.SelectUsable(null);
			if (!Application.isEditor)
			{
				ComponentManager<SaveAndLoad>.Value.SaveGame(sendRemoteMessage: true);
			}
			playerNetwork.cameraBobAnimator.SetBool("InWater", value: false);
			playerNetwork.cameraBobAnimator.SetFloat("Speed", 0f);
		}
		incapacitatedCollider.enabled = true;
		playerNetwork.Stats.buffManager.ClearBuffs();
		playerNetwork.Stats.SendOnDeath();
		playerNetwork.Animator.anim.SetBool("Dead", value: true);
		playerNetwork.Animator.anim.SetBool("InAir", value: false);
	}

	public void ResetPlayerState()
	{
		if (playerNetwork.BedComponent.Sleeping)
		{
			playerNetwork.BedComponent.StopSleep();
		}
		if (playerNetwork.ZiplinePlayer != null)
		{
			if (playerNetwork.IsLocalPlayer)
			{
				if (playerNetwork.ZiplinePlayer.IsAttachedToZipline)
				{
					playerNetwork.ZiplinePlayer.DetachFromCurrentZipline(keepMomentum: false);
				}
			}
			else
			{
				playerNetwork.ZiplinePlayer.DetachFromCurrentZiplineClient();
			}
		}
		if (playerNetwork.NoteBookUI != null && playerNetwork.NoteBookUI.isDisplayed)
		{
			playerNetwork.NoteBookUI.SetBookActive(value: false);
		}
		if (playerNetwork.IsLocalPlayer)
		{
			if (playerNetwork.RessurectComponent.IsCarrying)
			{
				Message_NetworkBehaviour message = new Message_NetworkBehaviour(Messages.RessurectComponent_StopCarry, playerNetwork);
				if (Raft_Network.IsHost)
				{
					playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					playerNetwork.RessurectComponent.StopCarryingPlayer(manipulatePosition: true);
				}
				else
				{
					playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
			if (canvas == null)
			{
				canvas = ComponentManager<CanvasHelper>.Value;
			}
			if (playerNetwork.CarryingComponent.IsCarrying)
			{
				playerNetwork.CarryingComponent.CarriedObject.OnStopCarry?.Invoke(playerNetwork, arg2: true);
			}
		}
		playerNetwork.Stats.buffManager.ClearBuffs();
		playerNetwork.Stats.SendOnDeath();
	}

	public bool MouseLookIsActive()
	{
		if (mouseLookXScript.enabled)
		{
			return mouseLookYScript.enabled;
		}
		return false;
	}

	public void SetMouseLookScripts(bool canLook)
	{
		if (mouseLookXScript != null && mouseLookYScript != null)
		{
			MouseLook mouseLook = mouseLookXScript;
			bool flag = (mouseLookYScript.enabled = canLook);
			mouseLook.enabled = flag;
		}
	}

	public void SetLockMouseLook(bool isLocked)
	{
		if (mouseLookXScript != null && mouseLookYScript != null)
		{
			mouseLookXScript.SetLocked(isLocked);
			mouseLookYScript.SetLocked(isLocked);
		}
	}

	public void MakeUnStuck()
	{
		while (StuckTest())
		{
			transform.position += Vector3.up;
		}
	}

	private bool StuckTest()
	{
		return Physics.CheckBox(transform.position, stuckSizeCheck / 2f, transform.rotation, LayerMasks.MASK_GroundMask_Raft, QueryTriggerInteraction.Ignore);
	}

	private void InitializeInventory()
	{
		Object.FindObjectOfType<CanvasHelper>().GetMenu(MenuType.Inventory)?.menuObjects.Add(ComponentManager<PlayerInventory>.Value.gameObject);
	}

	private IEnumerator RespawnWait(float timeDelayBeforePlayerCanMove)
	{
		waitingForRespawn = true;
		float timer = 0f;
		FadePanel fadePanel = SingletonGeneric<GameManager>.Singleton.sleepFadePanel;
		StartCoroutine(fadePanel.FadeToAlpha(1f, 1f));
		while (true)
		{
			playerStats.IsInvurnerable = true;
			if (fixedHealth.HasValue)
			{
				playerStats.stat_health.Value = fixedHealth.Value;
			}
			if (fixedHunger.HasValue)
			{
				playerStats.stat_hunger.Normal.Value = fixedHunger.Value;
			}
			if (fixedThurst.HasValue)
			{
				playerStats.stat_thirst.Normal.Value = fixedThurst.Value;
			}
			timer += Time.deltaTime;
			if (timer >= timeDelayBeforePlayerCanMove)
			{
				canvas.displayTextManager.ShowText(Helper.GetTerm("Game/WakeUp"));
				if (Input.anyKeyDown)
				{
					break;
				}
			}
			yield return null;
		}
		playerStats.IsInvurnerable = false;
		fixedHealth = null;
		fixedHunger = null;
		fixedThurst = null;
		fadePanel.StopAllCoroutines();
		StartCoroutine(fadePanel.FadeToAlpha(0f, 0.5f));
		canvas.displayTextManager.HideDisplayTexts();
		if (Raft_Network.IsHost)
		{
			playerNetwork.Network.RPC(new Message_NetworkBehaviour(Messages.Respawn_Complete, playerNetwork), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			OnRespawnComplete();
		}
		else
		{
			playerNetwork.SendP2P(new Message_NetworkBehaviour(Messages.Respawn_Complete, playerNetwork), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		playerStats.IsInvurnerable = false;
		fixedHealth = null;
		fixedHunger = null;
		fixedThurst = null;
		waitingForRespawn = false;
	}

	public IEnumerator KillLater(float timeDelay, bool replicating, bool useNetwork)
	{
		yield return new WaitForSeconds(timeDelay);
		if (useNetwork)
		{
			Message_NetworkBehaviour message = new Message_NetworkBehaviour(Messages.PlayerKilled, playerNetwork);
			if (Raft_Network.IsHost)
			{
				playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				playerNetwork.PlayerScript.Kill(replicating);
			}
			else
			{
				playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			Kill(replicating);
		}
	}
}
