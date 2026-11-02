using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EZCameraShake;
using Steamworks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

public class Network_Player : MonoBehaviour_NetworkTick
{
	private bool isLocalPlayer;

	private Raft_Network network;

	[HideInInspector]
	public Network_UserId steamID;

	[Header("Components")]
	public TextMesh playerNameTextMesh;

	[SerializeField]
	private BillboardObject nameBillboard;

	public Transform playerPivot;

	public Transform leftHandParent;

	public Transform rightHandParent;

	public FlightCamera flightCamera;

	[HideInInspector]
	public RGD_Settings_Character characterSettings;

	[HideInInspector]
	public CanvasHelper canvas;

	public Animator cameraBobAnimator;

	[SerializeField]
	private Sprite smallSteamProfile;

	public Behaviour[] localBehaviours;

	public GameObject[] localObjects;

	public CharacterModelModifications currentModel;

	[SerializeField]
	private VolumetricLightRenderer volumetricLightRenderer;

	private static Transform localPlayerCameraTransform;

	private Camera camera;

	private Camera handCamera;

	private Transform cameraTransform;

	private PersonController controller;

	private PlayerAnimator animator;

	private PlayerItemManager playerItemManager;

	private BlockCreator blockCreator;

	private ZiplinePlayer ziplinePlayer;

	private PlayerNetworkManager playerNetworkManager;

	private Player playerScript;

	private BedComponent bedComponent;

	private PlayerStats stats;

	private PlantManager plantManager;

	private StorageManager storageManager;

	private RessurectComponent ressurectComponent;

	private BlueprintComponent blueprintComponent;

	private CarryingComponent carryingComponent;

	private PlayerInventory inventory;

	private Pickup pickupScript;

	private Axe axeScript;

	private Hammer hammerScript;

	private Paddle paddleScript;

	private SharkBaitThrower sharkBaitThrower;

	private PaintBrush paintBrush;

	private PlayerEquipment playerEquipment;

	private ThrowableComponent_Bow bow;

	private Shovel shovel;

	private Firework_Hand fireworkHand;

	private NoteBookUI noteBookUI;

	private MovementDeltaTracker movementDeltaTracker;

	private Unstuck_Tracker unstuckTracker;

	private HazmatSuit hazmatSuit;

	private FishingBaitHandler fishingBaitHandler;

	private PlayerBuffManager buffManager;

	private object profileLock = new object();

	private string avatarUrl;

	private string fecthedRealname;

	private bool avatarUrlUpdated;

	private bool worldHasBeenReceived;

	public string visualName;

	public string realName;

	public bool initialized;

	public SO_CameraShaker shakerAsset;

	public bool IsLocalPlayer => isLocalPlayer;

	public bool UsingNoclip
	{
		get
		{
			if (IsLocalPlayer)
			{
				return flightCamera.enabled;
			}
			return false;
		}
	}

	public Vector3 FeetPosition => transform.position + Vector3.down * 0.8f;

	public Raft_Network Network
	{
		get
		{
			return network;
		}
		set
		{
			network = value;
		}
	}

	public static Transform LocalPlayerCameraTransform
	{
		get
		{
			return localPlayerCameraTransform;
		}
		private set
		{
			localPlayerCameraTransform = value;
		}
	}

	public Camera Camera
	{
		get
		{
			return camera;
		}
		private set
		{
			camera = value;
		}
	}

	public Camera HandCamera
	{
		get
		{
			return handCamera;
		}
		private set
		{
			handCamera = value;
		}
	}

	public Transform CameraTransform
	{
		get
		{
			return cameraTransform;
		}
		private set
		{
			cameraTransform = value;
		}
	}

	public Transform PlayerHeadBone => currentModel.headBone;

	public PersonController PersonController
	{
		get
		{
			return controller;
		}
		private set
		{
			controller = value;
		}
	}

	public PlayerAnimator Animator
	{
		get
		{
			return animator;
		}
		private set
		{
			animator = value;
		}
	}

	public PlayerItemManager PlayerItemManager
	{
		get
		{
			return playerItemManager;
		}
		private set
		{
			playerItemManager = value;
		}
	}

	public BlockCreator BlockCreator
	{
		get
		{
			return blockCreator;
		}
		private set
		{
			blockCreator = value;
		}
	}

	public ZiplinePlayer ZiplinePlayer
	{
		get
		{
			return ziplinePlayer;
		}
		private set
		{
			ziplinePlayer = value;
		}
	}

	public PlayerNetworkManager PlayerNetworkManager
	{
		get
		{
			return playerNetworkManager;
		}
		private set
		{
			playerNetworkManager = value;
		}
	}

	public Player PlayerScript
	{
		get
		{
			return playerScript;
		}
		private set
		{
			playerScript = value;
		}
	}

	public BedComponent BedComponent
	{
		get
		{
			return bedComponent;
		}
		private set
		{
			bedComponent = value;
		}
	}

	public PlayerStats Stats
	{
		get
		{
			return stats;
		}
		private set
		{
			stats = value;
		}
	}

	public PlantManager PlantManager
	{
		get
		{
			return plantManager;
		}
		private set
		{
			plantManager = value;
		}
	}

	public StorageManager StorageManager
	{
		get
		{
			return storageManager;
		}
		private set
		{
			storageManager = value;
		}
	}

	public RessurectComponent RessurectComponent
	{
		get
		{
			return ressurectComponent;
		}
		private set
		{
			ressurectComponent = value;
		}
	}

	public BlueprintComponent BlueprintComponent
	{
		get
		{
			return blueprintComponent;
		}
		private set
		{
			blueprintComponent = value;
		}
	}

	public CarryingComponent CarryingComponent
	{
		get
		{
			return carryingComponent;
		}
		private set
		{
			carryingComponent = value;
		}
	}

	public PlayerInventory Inventory
	{
		get
		{
			return inventory;
		}
		set
		{
			inventory = value;
		}
	}

	public Pickup PickupScript
	{
		get
		{
			return pickupScript;
		}
		private set
		{
			pickupScript = value;
		}
	}

	public Axe AxeScript
	{
		get
		{
			return axeScript;
		}
		private set
		{
			axeScript = value;
		}
	}

	public Hammer HammerScript
	{
		get
		{
			return hammerScript;
		}
		private set
		{
			hammerScript = value;
		}
	}

	public Paddle PaddleScript
	{
		get
		{
			return paddleScript;
		}
		private set
		{
			paddleScript = value;
		}
	}

	public SharkBaitThrower SharkBaitThrower
	{
		get
		{
			return sharkBaitThrower;
		}
		private set
		{
			sharkBaitThrower = value;
		}
	}

	public PaintBrush PaintBrush
	{
		get
		{
			return paintBrush;
		}
		private set
		{
			paintBrush = value;
		}
	}

	public PlayerEquipment PlayerEquipment
	{
		get
		{
			return playerEquipment;
		}
		private set
		{
			playerEquipment = value;
		}
	}

	public ThrowableComponent_Bow Bow
	{
		get
		{
			return bow;
		}
		private set
		{
			bow = value;
		}
	}

	public Shovel ShovelScript
	{
		get
		{
			return shovel;
		}
		private set
		{
			shovel = value;
		}
	}

	public Firework_Hand FireworkHand
	{
		get
		{
			return fireworkHand;
		}
		private set
		{
			fireworkHand = value;
		}
	}

	public NoteBookUI NoteBookUI
	{
		get
		{
			return noteBookUI;
		}
		private set
		{
			noteBookUI = value;
		}
	}

	public MovementDeltaTracker MovementDeltaTracker
	{
		get
		{
			return movementDeltaTracker;
		}
		private set
		{
			movementDeltaTracker = value;
		}
	}

	public Unstuck_Tracker UnstuckTracker
	{
		get
		{
			return unstuckTracker;
		}
		private set
		{
			unstuckTracker = value;
		}
	}

	public HazmatSuit HazmatSuit
	{
		get
		{
			return hazmatSuit;
		}
		private set
		{
			hazmatSuit = value;
		}
	}

	public FishingBaitHandler FishingBaitHandler
	{
		get
		{
			return fishingBaitHandler;
		}
		private set
		{
			fishingBaitHandler = value;
		}
	}

	public PlayerBuffManager BuffManager
	{
		get
		{
			return buffManager;
		}
		private set
		{
			buffManager = value;
		}
	}

	public void SetVisualName(string _visualName, bool worldHasBeenReceived)
	{
		visualName = _visualName;
		network.CheckForNameCollision(this, steamID);
		if (!IsLocalPlayer)
		{
			playerNameTextMesh.text = visualName;
		}
		if (worldHasBeenReceived && characterSettings.Platform != 0)
		{
			ComponentManager<ChatTextFieldController>.Value.CreatePlayerJoinText(this);
		}
		network.OnPlayerJoinOrLeaves?.Invoke();
	}

	public void RetrieveAvatar(ulong platformId, bool _worldHasBeenReceived)
	{
		SetSmallSteamProfile(new CSteamID(platformId));
		network.OnPlayerJoinOrLeaves?.Invoke();
		if (_worldHasBeenReceived)
		{
			ComponentManager<ChatTextFieldController>.Value.CreatePlayerJoinText(this);
		}
	}

	private IEnumerator GetAvatarRoutine()
	{
		UnityWebRequest request = UnityWebRequestTexture.GetTexture(avatarUrl);
		yield return request.SendWebRequest();
		Debug.Log($"Response code {request.responseCode}");
		if (request.result == UnityWebRequest.Result.Success)
		{
			_ = ((DownloadHandlerTexture)request.downloadHandler).texture;
			network.OnPlayerJoinOrLeaves?.Invoke();
		}
	}

	private void InitializeComponents()
	{
		Camera = GetComponentInChildren<Camera>(includeInactive: true);
		CameraTransform = Camera.transform;
		PersonController = GetComponent<PersonController>();
		Animator = GetComponentInChildren<PlayerAnimator>(includeInactive: true);
		PlayerItemManager = GetComponentInChildren<PlayerItemManager>(includeInactive: true);
		BlockCreator = GetComponentInChildren<BlockCreator>(includeInactive: true);
		PlayerNetworkManager = GetComponentInChildren<PlayerNetworkManager>(includeInactive: true);
		BedComponent = GetComponentInChildren<BedComponent>(includeInactive: true);
		StorageManager = GetComponentInChildren<StorageManager>(includeInactive: true);
		PlantManager = GetComponentInChildren<PlantManager>(includeInactive: true);
		RessurectComponent = GetComponentInChildren<RessurectComponent>(includeInactive: true);
		BlueprintComponent = GetComponentInChildren<BlueprintComponent>(includeInactive: true);
		CarryingComponent = GetComponentInChildren<CarryingComponent>(includeInactive: true);
		AxeScript = GetComponentInChildren<Axe>(includeInactive: true);
		HammerScript = GetComponentInChildren<Hammer>(includeInactive: true);
		PaddleScript = GetComponentInChildren<Paddle>(includeInactive: true);
		SharkBaitThrower = GetComponentInChildren<SharkBaitThrower>(includeInactive: true);
		PaintBrush = GetComponentInChildren<PaintBrush>(includeInactive: true);
		PlayerEquipment = GetComponentInChildren<PlayerEquipment>(includeInactive: true);
		ZiplinePlayer = GetComponentInChildren<ZiplinePlayer>(includeInactive: true);
		Bow = GetComponentInChildren<ThrowableComponent_Bow>(includeInactive: true);
		FireworkHand = GetComponentInChildren<Firework_Hand>(includeInactive: true);
		hazmatSuit = GetComponentInChildren<HazmatSuit>(includeInactive: true);
		BuffManager = GetComponentInChildren<PlayerBuffManager>(includeInactive: true);
		PlayerScript = GetComponent<Player>();
		Stats = GetComponent<PlayerStats>();
		PickupScript = GetComponent<Pickup>();
		Inventory = ComponentManager<PlayerInventory>.Value;
		flightCamera = Camera.GetComponent<FlightCamera>();
		noteBookUI = GetComponentInChildren<NoteBookUI>();
		movementDeltaTracker = GetComponent<MovementDeltaTracker>();
		unstuckTracker = GetComponent<Unstuck_Tracker>();
		FishingBaitHandler = GetComponentInChildren<FishingBaitHandler>(includeInactive: true);
		if (IsLocalPlayer)
		{
			Camera.GetComponent<VolumetricLightRenderer>().enabled = true;
			ComponentManager<Player>.Value = GetComponent<Player>();
			ComponentManager<Network_Player>.Value = this;
			ComponentManager<PlayerEquipment>.Value = GetComponentInChildren<PlayerEquipment>();
			LocalPlayerCameraTransform = CameraTransform;
		}
	}

	private void Start()
	{
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Combine(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
		canvas = ComponentManager<CanvasHelper>.Value;
		initialized = true;
	}

	private void Update()
	{
		lock (profileLock)
		{
			if (avatarUrlUpdated)
			{
				realName = fecthedRealname;
				StartCoroutine(GetAvatarRoutine());
				avatarUrlUpdated = false;
			}
		}
		if (IsLocalPlayer)
		{
			UpdateNetworkTick();
			if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L) && Cheat.AllowCheatsForLocalPlayer())
			{
				ComponentManager<Inventory_ResearchTable>.Value.LearnAllRecipesInstantly();
			}
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Remove(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
		NetworkUpdateManager.RemoveBehaviour(this);
		MonoBehaviour_ID_Network[] componentsInChildren = GetComponentsInChildren<MonoBehaviour_ID_Network>(includeInactive: true);
		foreach (MonoBehaviour_ID_Network monoBehaviour_ID_Network in componentsInChildren)
		{
			if (monoBehaviour_ID_Network is MonoBehaviour_Network)
			{
				NetworkUpdateManager.RemoveBehaviour(monoBehaviour_ID_Network as MonoBehaviour_Network);
			}
			else
			{
				NetworkIDManager.RemoveNetworkID(monoBehaviour_ID_Network);
			}
		}
		network = null;
		controller = null;
		playerItemManager = null;
		blockCreator = null;
		ziplinePlayer = null;
		playerNetworkManager = null;
		playerScript = null;
		bedComponent = null;
		stats = null;
		plantManager = null;
		storageManager = null;
		ressurectComponent = null;
		blueprintComponent = null;
		carryingComponent = null;
		inventory = null;
		pickupScript = null;
		axeScript = null;
		hammerScript = null;
		paddleScript = null;
		sharkBaitThrower = null;
		paintBrush = null;
		playerEquipment = null;
		bow = null;
		shovel = null;
		fireworkHand = null;
		noteBookUI = null;
		movementDeltaTracker = null;
		unstuckTracker = null;
		hazmatSuit = null;
		fishingBaitHandler = null;
		buffManager = null;
	}

	public void OnPlayerCreated(Raft_Network network, Network_UserId steamID, RGD_Settings_Character characterSettings)
	{
		isLocalPlayer = steamID == network.LocalSteamID;
		this.network = network;
		this.steamID = steamID;
		if (IsLocalPlayer)
		{
			this.characterSettings = ComponentManager<Settings>.Value.Current.character;
		}
		else
		{
			this.characterSettings = characterSettings;
		}
		InitializeComponents();
		base.transform.name = characterSettings.Name;
		base.transform.SetParent(SingletonGeneric<GameManager>.Singleton.lockedPivot);
		int index = characterSettings.ModelIndex;
		if (characterSettings.ModelIndex < 0 || characterSettings.ModelIndex >= CharacterManager.SO_Characters.Count)
		{
			index = 0;
		}
		currentModel = UnityEngine.Object.Instantiate(CharacterManager.SO_Characters[index].modelPrefab, Animator.transform);
		Transform transform = currentModel.transform;
		Vector3 localPosition = (currentModel.transform.localEulerAngles = Vector3.zero);
		transform.localPosition = localPosition;
		ApplyOutfit(characterSettings.OutfitIndex);
		leftHandParent.SetParent(currentModel.leftHandItemHolder);
		Transform transform2 = leftHandParent;
		localPosition = (leftHandParent.localEulerAngles = Vector3.zero);
		transform2.localPosition = localPosition;
		leftHandParent.gameObject.SetActive(value: true);
		rightHandParent.SetParent(currentModel.rightHandItemHolder);
		Transform transform3 = rightHandParent;
		localPosition = (rightHandParent.localEulerAngles = Vector3.zero);
		transform3.localPosition = localPosition;
		rightHandParent.gameObject.SetActive(value: true);
		for (int i = 0; i < currentModel.entityColliders.Length; i++)
		{
			if (IsLocalPlayer)
			{
				UnityEngine.Object.Destroy(currentModel.entityColliders[i].gameObject);
			}
		}
		CameraTransform.SetParent(currentModel.cameraHolder);
		Transform transform4 = CameraTransform;
		localPosition = (CameraTransform.localEulerAngles = Vector3.zero);
		transform4.localPosition = localPosition;
		Animator.anim.Rebind();
		if (!IsLocalPlayer)
		{
			for (int j = 0; j < localBehaviours.Length; j++)
			{
				if (localBehaviours[j] != null)
				{
					UnityEngine.Object.Destroy(localBehaviours[j]);
				}
			}
			for (int k = 0; k < localObjects.Length; k++)
			{
				GameObject gameObject = localObjects[k];
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
			Camera = null;
			CameraTransform.tag = "Untagged";
			PersonController.enabled = true;
			PersonController.IsMovementFree = true;
			StartCoroutine(SetBillboardTarget(0.5f));
			SetNameTagVisibility(GeneralSettingsBox.ShowNameTag);
		}
		else
		{
			StartCoroutine(ActivateController(0.5f));
			base.gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
			CameraShaker.Instance = GetComponentInChildren<CameraShaker>(includeInactive: true);
			HandCamera = CameraTransform.Find("HandCamera").GetComponent<Camera>();
			UnityEngine.Object.Destroy(playerNameTextMesh.transform.parent.gameObject);
			List<Renderer> list = new List<Renderer>();
			list.AddRange(rightHandParent.GetComponentsInChildren<MeshRenderer>(includeInactive: true));
			list.AddRange(rightHandParent.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true));
			list.AddRange(leftHandParent.GetComponentsInChildren<MeshRenderer>(includeInactive: true));
			list.AddRange(leftHandParent.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true));
			list.AddRange(currentModel.hatParent.GetComponentsInChildren<MeshRenderer>(includeInactive: true));
			list.AddRange(currentModel.hatParent.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true));
			for (int l = 0; l < list.Count; l++)
			{
				list[l].shadowCastingMode = ShadowCastingMode.Off;
			}
		}
		if (Raft_Network.IsHost)
		{
			SetIndexLocalPlayer();
		}
		GetComponent<PlayerFOVManager>().Initialize();
	}

	public void SetSmallSteamProfile(CSteamID _steamID)
	{
		smallSteamProfile = Helper.GetMediumSteamAvatarFromID(_steamID);
	}

	public Sprite GetSmallSteamProfile()
	{
		return smallSteamProfile;
	}

	public void SetIndexLocalPlayer()
	{
		List<MonoBehaviour_ID_Network> list = GetComponentsInChildren<MonoBehaviour_ID_Network>(includeInactive: true).ToList();
		list.Remove(this);
		list.Insert(0, this);
		foreach (MonoBehaviour_ID_Network item in list)
		{
			item.transform.name = item.GetType()?.ToString() + ", " + characterSettings.Name;
			item.ObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
			if (item is MonoBehaviour_Network)
			{
				MonoBehaviour_Network obj = item as MonoBehaviour_Network;
				obj.BehaviourIndex = NetworkUpdateManager.GetUniqueBehaviourIndex();
				NetworkUpdateManager.AddBehaviour(obj);
			}
			else
			{
				NetworkIDManager.AddNetworkID(item);
			}
		}
		Stats.ObjectIndex = ObjectIndex;
	}

	public void SetIndexRemotePlayer(uint objectIndex, uint behaviourIndex)
	{
		ObjectIndex = objectIndex;
		BehaviourIndex = behaviourIndex;
		NetworkUpdateManager.AddBehaviour(this);
		MonoBehaviour_ID_Network[] componentsInChildren = GetComponentsInChildren<MonoBehaviour_ID_Network>(includeInactive: true);
		uint num = objectIndex + 1;
		uint num2 = behaviourIndex + 1;
		MonoBehaviour_ID_Network[] array = componentsInChildren;
		foreach (MonoBehaviour_ID_Network monoBehaviour_ID_Network in array)
		{
			monoBehaviour_ID_Network.transform.name = monoBehaviour_ID_Network.GetType()?.ToString() + ", " + characterSettings.Name;
			if (monoBehaviour_ID_Network != this)
			{
				monoBehaviour_ID_Network.ObjectIndex = num++;
				if (monoBehaviour_ID_Network is MonoBehaviour_Network)
				{
					MonoBehaviour_Network obj = monoBehaviour_ID_Network as MonoBehaviour_Network;
					obj.BehaviourIndex = num2++;
					NetworkUpdateManager.AddBehaviour(obj);
				}
				else
				{
					NetworkIDManager.AddNetworkID(monoBehaviour_ID_Network);
				}
			}
		}
		stats.ObjectIndex = ObjectIndex;
	}

	public void SendP2P(Message message, EP2PSend sendType, NetworkChannel channel)
	{
		Network.SendP2P(Network.HostID, message, sendType, channel);
	}

	public void SetToValidSpawnPoint()
	{
		if (IsLocalPlayer)
		{
			Invoke("SetToValidSpawnPointLate", 3f);
		}
	}

	private void SetToValidSpawnPointLate()
	{
		if (PersonController.controllerType != ControllerType.Water)
		{
			return;
		}
		List<Region> regionsFromPosition = RegionManager.GetRegionsFromPosition(transform.position);
		if (regionsFromPosition != null && regionsFromPosition.Count > 0)
		{
			Debug.Log("Teleporting player to Raft was canceled because player was within a region");
			return;
		}
		float num = Vector3.Distance(transform.position, SingletonGeneric<GameManager>.Singleton.buoyancyPivot.position);
		if (num > 100f)
		{
			Debug.Log("Teleporting player to Raft since distance is greater then 100. Distance = " + num);
			SetToWalkableBlockPosition();
		}
	}

	public void SetToWalkableBlockPosition()
	{
		if (PersonController.controllerType != ControllerType.Ground)
		{
			PersonController.SwitchControllerType(ControllerType.Ground);
		}
		List<Block> placedBlocks = BlockCreator.GetPlacedBlocks();
		for (int i = 0; i < placedBlocks.Count; i++)
		{
			if (placedBlocks[i].IsWalkable())
			{
				transform.position = placedBlocks[i].transform.position + placedBlocks[i].transform.up * 2f;
				break;
			}
		}
		PlayerScript.MakeUnStuck();
	}

	public void SetNameTagVisibility(bool visible)
	{
		if (!IsLocalPlayer && !(playerNameTextMesh == null) && !(playerNameTextMesh.transform.parent == null))
		{
			playerNameTextMesh.transform.parent.gameObject.SetActive(visible);
		}
	}

	public void SetHatParentToCamera(bool value)
	{
		if (!(currentModel == null) && !(currentModel.hatParent == null))
		{
			if (value)
			{
				currentModel.hatParent.SetParentSafe(CameraTransform);
				currentModel.hatParent.localPosition = CameraTransform.InverseTransformPoint(currentModel.headBone.position);
				currentModel.hatParent.localEulerAngles = new Vector3(0f, 0f, -90f);
			}
			else
			{
				currentModel.hatParent.SetParentSafe(currentModel.headBone);
				Transform hatParent = currentModel.hatParent;
				Vector3 localEulerAngles = (currentModel.hatParent.localPosition = Vector3.zero);
				hatParent.localEulerAngles = localEulerAngles;
			}
		}
	}

	public void HandleInitialAnimationValues(Message_Player_Create msg)
	{
		CoroutineManager.Singleton.AddCoroutine(HandleInitialAnimationDelayed(msg, 2f));
	}

	private IEnumerator HandleInitialAnimationDelayed(Message_Player_Create msg, float delay)
	{
		yield return new WaitForSeconds(delay);
		if (!(msg == null) && !(Animator == null) && !(Animator.anim == null))
		{
			Animator.SetAnimation(msg.Anim_UsingInstrument ? PlayerFullBodyAnimation.UsingInstrument_3 : PlayerFullBodyAnimation.None_0);
			Animator.ReselectAnimation();
		}
	}

	public bool ApplyOutfit(int outfitIndex)
	{
		if (currentModel != null && currentModel.ApplyOutfit(outfitIndex))
		{
			if (IsLocalPlayer && !GameManager.QuickStartGame)
			{
				Settings value = ComponentManager<Settings>.Value;
				if (value != null)
				{
					characterSettings.OutfitIndex = outfitIndex;
					value.Save(value.Current);
				}
			}
			return true;
		}
		return false;
	}

	private void OnWorldShift(Vector3 shift)
	{
		if (!PersonController.HasRaftAsParent)
		{
			transform.position -= shift;
			if (!IsLocalPlayer)
			{
				PersonController.SetNetworkPosition(transform.position);
			}
		}
	}

	private IEnumerator ActivateController(float timeDelay)
	{
		while (!Raft_Network.IsAllLandmarksLoaded)
		{
			yield return null;
		}
		yield return new WaitForSeconds(timeDelay);
		PersonController.IsMovementFree = true;
		PersonController.enabled = true;
	}

	private IEnumerator SetBillboardTarget(float timeDelay)
	{
		yield return new WaitForSeconds(timeDelay);
		Network_Player localPlayer = network.GetLocalPlayer();
		if (localPlayer == null)
		{
			Debug.LogWarning("Could not set billboard target because local player was not found", gameObject);
			yield break;
		}
		_ = localPlayer.CameraTransform;
		nameBillboard.SetTarget(Network.GetLocalPlayer().CameraTransform);
	}

	public PickupItem GetMineableItemAtCursor()
	{
		if (Helper.HitAtCursor(out var hit, Player.UseDistance * 1.25f, LayerMasks.MASK_Item) && hit.transform.tag == "PickupHook")
		{
			return hit.transform.GetComponent<PickupItem>();
		}
		return null;
	}

	public override void Serialize_Create()
	{
		AddMessage(new Message_Player_Create(Messages.CreatePlayer, this));
		AddMessage(new Message_Equipment_Create(Messages.Equipment_Create, this, PlayerEquipment));
	}

	public override void Serialize_Update()
	{
		AddMessage(new Message_Player_Update(Messages.Update, this, this));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.Update:
		{
			Message_Player_Update message_Player_Update = msg as Message_Player_Update;
			if (!RessurectComponent.BeingCarried)
			{
				PersonController.SetNetworkProperties(message_Player_Update);
				Animator.SetNetworkProperties(message_Player_Update);
			}
			if (Raft_Network.IsHost)
			{
				network.RPCExclude(message_Player_Update, Target.Other, remoteID, EP2PSend.k_EP2PSendUnreliable, NetworkChannel.Channel_Game);
			}
			return false;
		}
		case Messages.Respawn_Complete:
			PlayerScript.OnRespawnComplete();
			return true;
		case Messages.Respawn_Start:
			if (RessurectComponent.BeingCarried && RessurectComponent.CarriedByPlayer != null)
			{
				RessurectComponent.CarriedByPlayer.RessurectComponent.StopCarryingPlayer(manipulatePosition: false);
			}
			PlayerScript.StartRespawn(BedManager.FindClosestBedToPlayer(this), clearInventory: true);
			return true;
		case Messages.Bed_Enter:
		{
			Message_Bed_Enter message_Bed_Enter = msg as Message_Bed_Enter;
			return BedComponent.Sleep(BedManager.GetBedFromIndex(message_Bed_Enter.bedObjectIndex));
		}
		case Messages.Bed_Exit:
			return BedComponent.StopSleep();
		case Messages.PlayerKilled:
			PlayerScript.Kill();
			return true;
		case Messages.RessurectComponent_StartCarry:
		{
			Message_NetworkBehaviour_SteamID message_NetworkBehaviour_SteamID = msg as Message_NetworkBehaviour_SteamID;
			Network_Player playerFromID = network.GetPlayerFromID(message_NetworkBehaviour_SteamID.SteamID);
			if (playerFromID != null && (!Raft_Network.IsHost || (Raft_Network.IsHost && RessurectComponent.CanCarryPlayer(playerFromID))))
			{
				return RessurectComponent.StartCarryingPlayer(playerFromID);
			}
			return false;
		}
		case Messages.RessurectComponent_StopCarry:
			RessurectComponent.StopCarryingPlayer(manipulatePosition: true);
			return true;
		case Messages.RessurectComponent_PlaceInBed:
		{
			Message_Bed_Enter message_Bed_Enter2 = msg as Message_Bed_Enter;
			return RessurectComponent.PlaceInBed(BedManager.GetBedFromIndex(message_Bed_Enter2.bedObjectIndex));
		}
		case Messages.PaintBlock:
		{
			Message_PaintBlock msgPaint = msg as Message_PaintBlock;
			Block block2 = BlockCreator.GetPlacedBlocks().Find((Block b) => b.ObjectIndex == msgPaint.boi);
			if (block2 != null)
			{
				return PaintBrush.PaintBlock(block2, msgPaint.LocalLockedPivotPosition, ColorPicker.GetColorFromUniqueIndex(msgPaint.ci), ColorPicker.GetColorFromUniqueIndex(msgPaint.pci), msgPaint.ps, msgPaint.pi, !Raft_Network.IsHost);
			}
			break;
		}
		case Messages.Consume:
		{
			Item_Base itemByIndex = ItemManager.GetItemByIndex((msg as Message_Consume).edibleItemIndex);
			Stats.Consume(itemByIndex);
			return true;
		}
		case Messages.AxeHit:
		{
			Message_AxeHit message_AxeHit = msg as Message_AxeHit;
			axeScript.PlayEffect(transform.parent, message_AxeHit.HitPoint, message_AxeHit.HitNormal);
			if (message_AxeHit.treeObjectIndex >= 0)
			{
				PickupItem_Networked networkIDFromObjectIndex = NetworkIDManager.GetNetworkIDFromObjectIndex<PickupItem_Networked>((uint)message_AxeHit.treeObjectIndex);
				if (networkIDFromObjectIndex != null)
				{
					HarvestableTree component = networkIDFromObjectIndex.GetComponent<HarvestableTree>();
					if (component != null)
					{
						return component.Harvest(IsLocalPlayer ? Inventory : null);
					}
				}
				return false;
			}
			return true;
		}
		case Messages.Axe_RemoveBlock:
		{
			Message_BlockCreator_RemoveBlock msgAxeRemove = msg as Message_BlockCreator_RemoveBlock;
			Block block = BlockCreator.GetPlacedBlocks().Find((Block b) => b.ObjectIndex == msgAxeRemove.blockObjectIndex);
			if (block != null)
			{
				return AxeScript.DestroyBlock(block, msgAxeRemove.UpdateBounds);
			}
			return false;
		}
		case Messages.Shark_Bait:
		{
			Message_SharkBait message_SharkBait = msg as Message_SharkBait;
			if (Raft_Network.IsHost)
			{
				SharkBaitThrower.currentSharkBait.ObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
				message_SharkBait.sharkBaitObjectIndex = SharkBaitThrower.currentSharkBait.ObjectIndex;
				BlockCreator.GetPlacedBlocks().Add(sharkBaitThrower.currentSharkBait);
			}
			else
			{
				SharkBaitThrower.currentSharkBait.ObjectIndex = message_SharkBait.sharkBaitObjectIndex;
			}
			BlockCreator.GetPlacedBlocks().Add(SharkBaitThrower.currentSharkBait);
			return true;
		}
		case Messages.Paddle:
		{
			Message_Paddle message_Paddle = msg as Message_Paddle;
			PaddleScript.PaddlePaddle(message_Paddle.Position, message_Paddle.Direction, message_Paddle.paddleForce);
			return true;
		}
		case Messages.Equipment_Create:
			PlayerEquipment.HandleCreateMessage(msg as Message_Equipment_Create);
			break;
		case Messages.Equipment_Equip:
		{
			Message_Equipment message_Equipment = msg as Message_Equipment;
			if (!Network.GetPlayerFromID(message_Equipment.SteamID).IsLocalPlayer)
			{
				return PlayerEquipment.EquipItemNetwork(message_Equipment.equipmentIndex);
			}
			return false;
		}
		case Messages.Equipment_UnEquip:
		{
			Message_Equipment message_Equipment2 = msg as Message_Equipment;
			if (!Network.GetPlayerFromID(message_Equipment2.SteamID).IsLocalPlayer)
			{
				return PlayerEquipment.UnEquipItemNetwork(message_Equipment2.equipmentIndex);
			}
			return false;
		}
		case Messages.Blueprint_Switch:
		{
			Message_Item blueprint = msg as Message_Item;
			blueprintComponent.SetBlueprint(blueprint);
			return true;
		}
		case Messages.Shovel_Use:
			return ShovelScript.ChannelItem();
		case Messages.ResearchBlueprint:
		{
			Message_ResearchTable_ResearchOrLearn message_ResearchTable_ResearchOrLearn2 = msg as Message_ResearchTable_ResearchOrLearn;
			Inventory_ResearchTable value = ComponentManager<Inventory_ResearchTable>.Value;
			if (value != null)
			{
				bool flag = value.ResearchBlueprint(ItemManager.GetItemByIndex(message_ResearchTable_ResearchOrLearn2.itemIndex));
				return Raft_Network.IsHost & flag;
			}
			return false;
		}
		case Messages.ResearchTable_Learn:
		{
			Message_ResearchTable_ResearchOrLearn message_ResearchTable_ResearchOrLearn = msg as Message_ResearchTable_ResearchOrLearn;
			Item_Base itemByIndex2 = ItemManager.GetItemByIndex(message_ResearchTable_ResearchOrLearn.itemIndex);
			if (itemByIndex2 != null)
			{
				Inventory_ResearchTable value = ComponentManager<Inventory_ResearchTable>.Value;
				return value.LearnItem(itemByIndex2, message_ResearchTable_ResearchOrLearn.SteamID);
			}
			return false;
		}
		case Messages.ZiplineAttach:
			if (!isLocalPlayer)
			{
				Message_Zipline_Attach message_Zipline_Attach = msg as Message_Zipline_Attach;
				if (message_Zipline_Attach.rgdMeshPath == null)
				{
					ZiplinePlayer.AttachToZipline(null, message_Zipline_Attach.modelIndex);
				}
				else
				{
					MeshPathConnection meshPathConnection = message_Zipline_Attach.rgdMeshPath.GetMeshPathConnection();
					ZiplinePlayer.AttachToZipline((meshPathConnection == null) ? null : (meshPathConnection.path as MeshPath_Zipline), message_Zipline_Attach.modelIndex);
				}
			}
			return true;
		case Messages.ZiplineDetach:
			if (!isLocalPlayer)
			{
				ZiplinePlayer.DetachFromCurrentZiplineClient();
			}
			return true;
		case Messages.FireworkHand_Launch:
		{
			Message_Firework_Launch msg2 = msg as Message_Firework_Launch;
			FireworkHand.LaunchFirework(msg2);
			return Raft_Network.IsHost;
		}
		case Messages.HazmatSuit:
		{
			Message_HazmatSuit message_HazmatSuit = msg as Message_HazmatSuit;
			if (message_HazmatSuit.equiped)
			{
				hazmatSuit.Equip(message_HazmatSuit.duration);
			}
			else
			{
				HazmatSuit.UnEquip(onBuffExpired: true);
			}
			return true;
		}
		case Messages.Character_ChangeOutfit:
		{
			Message_NetworkBehaviour_Int message_NetworkBehaviour_Int = msg as Message_NetworkBehaviour_Int;
			return ApplyOutfit(message_NetworkBehaviour_Int.IntegerValue);
		}
		}
		return false;
	}
}
