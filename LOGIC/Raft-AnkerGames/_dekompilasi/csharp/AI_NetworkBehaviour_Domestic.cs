using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using UnityEngine;

public class AI_NetworkBehaviour_Domestic : AI_NetworkBehaviour_Animal
{
	private DomesticStateType _domesticState;

	public RareMaterial rareMaterialScript;

	[SerializeField]
	protected Vector3 carryPosOffset;

	[SerializeField]
	protected Vector3 carryRotOffset = Vector3.zero;

	[SerializeField]
	protected Vector3 thirdPcarryPosOffset;

	[SerializeField]
	protected Vector3 thirdPcarryRotOffset = Vector3.zero;

	[SerializeField]
	private AI_State defaultRaftState;

	[SerializeField]
	private AI_State defaultLandmarkState;

	[SerializeField]
	private AI_State capturedState;

	[SerializeField]
	private AI_State carriedState;

	[SerializeField]
	private AI_State hungryState;

	[SerializeField]
	private AI_State eatState;

	[SerializeField]
	private AI_State_Domestic_Water waterState;

	[SerializeField]
	public TextWriterObject nameTag;

	private bool isNetworkPositionLocal;

	[HideInInspector]
	public Carry carryScript;

	[HideInInspector]
	public CaptureAnimal captureScript;

	[HideInInspector]
	public Transform lockedPivot;

	protected AI_StateMachine_Domestic stateMachine_Domestic;

	private AI_Movement movement;

	private float _hungerTimer;

	private float totalStarveTime = 60f;

	private float backToHungerStateCooldown;

	private bool hasGivenAchievment;

	public bool IsTamed { get; set; }

	public bool IsDomestic
	{
		get
		{
			if (!IsStarving)
			{
				return IsTamed;
			}
			return false;
		}
	}

	public bool IsStarving => HungerTimer <= 0f;

	public float HungerTimer
	{
		get
		{
			return _hungerTimer;
		}
		set
		{
			_hungerTimer = value;
		}
	}

	public DomesticStateType DomesticState
	{
		get
		{
			return _domesticState;
		}
		set
		{
			_domesticState = value;
			RefreshDomesticState();
		}
	}

	public float CooldownToHungerState
	{
		get
		{
			return backToHungerStateCooldown;
		}
		set
		{
			backToHungerStateCooldown = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		movement = GetComponentInChildren<AI_Movement>();
		stateMachine_Domestic = GetComponent<AI_StateMachine_Domestic>();
		InitializeCarryScript();
		InitializeCaptureScript();
	}

	protected override void Start()
	{
		base.Start();
		if (SingletonGeneric<GameManager>.Singleton != null)
		{
			lockedPivot = SingletonGeneric<GameManager>.Singleton.lockedPivot;
		}
		totalStarveTime = GameModeValueManager.GetCurrentGameModeValue().domesticVariables.starveTime;
		if (Raft_Network.IsHost && !Raft_Network.InMenuScene && !hasBeenLoaded)
		{
			if (rareMaterialScript != null)
			{
				rareMaterialScript.AssignRandomRareMaterial();
				Network.RPC(new Message_RareMaterial(Messages.RareMaterial, Network.NetworkIDManager, ObjectIndex, rareMaterialScript), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			HungerTimer = GameModeValueManager.GetCurrentGameModeValue().domesticVariables.startHunger;
		}
		if (!hasBeenLoaded && !IsTamed)
		{
			SetNameTagEnabled(value: false);
		}
		else if (IsTamed)
		{
			SetNameTagEnabled(value: true);
		}
		if (Raft_Network.InMenuScene)
		{
			DomesticState = DomesticStateType.Raft;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (networkEntity.IsDead)
		{
			return;
		}
		if (transform.parent == lockedPivot)
		{
			transform.gameObject.layer = LayerMasks.MASK_DomesticRaftCollision;
		}
		else
		{
			transform.gameObject.layer = LayerMasks.MASK_DomesticLandmarkCollision;
		}
		if (DomesticState == DomesticStateType.Raft && !carryScript.IsBeingCarried && stateMachine_Domestic.movement.MovementType == MovementType.CharacterController)
		{
			if (transform.SetParentSafe(lockedPivot))
			{
				Debug.Log("Setting domestic behaviour parent to locked pivot since it is in DomesticStateType=Raft");
			}
			Vector3 localEulerAngles = stateMachine_Domestic.transform.localEulerAngles;
			localEulerAngles.x = (localEulerAngles.z = 0f);
			stateMachine_Domestic.transform.localEulerAngles = Vector3.zero;
			if (movement.model != null)
			{
				localEulerAngles = movement.model.localEulerAngles;
				localEulerAngles.x = (localEulerAngles.z = 0f);
				movement.model.localEulerAngles = localEulerAngles;
			}
			float magnitude = stateMachine_Domestic.movement.characterController.velocity.magnitude;
			magnitude = Helper.MapValue(0f, stateMachine_Domestic.movement.MovementSpeed, magnitude, 0f, 1f);
			if (!Raft_Network.IsHost)
			{
				magnitude = stateMachine_Domestic.movement.MovementSpeed;
			}
			if (magnitude <= 0.15f)
			{
				magnitude = 0f;
			}
			float current = stateMachine_Domestic.animController.GetFloat("Speed");
			stateMachine_Domestic.animController.SetFloat("Speed", Mathf.MoveTowards(current, Mathf.Clamp01(magnitude), Time.deltaTime * 7f));
		}
	}

	protected override void UpdateHost()
	{
		if (!Raft_Network.InMenuScene)
		{
			base.UpdateHost();
			if (IsTamed)
			{
				HandleHunger();
				HandleStarving();
			}
		}
	}

	protected override void UpdateRemoteclient()
	{
		if (!carryScript.IsBeingCarried)
		{
			if (isNetworkPositionLocal)
			{
				stateMachine_Domestic.transform.SetParentSafe(lockedPivot);
			}
			else
			{
				stateMachine_Domestic.transform.SetParentSafe(null);
			}
			LerpMovement(networkPosition, isNetworkPositionLocal);
			LerpRotation(networkEuler);
		}
	}

	protected void SetObjectLayerDependingOnState(DomesticStateType type)
	{
		switch (type)
		{
		case DomesticStateType.Raft:
			transform.gameObject.layer = LayerMasks.MASK_DomesticRaftCollision;
			break;
		case DomesticStateType.Landmark:
		case DomesticStateType.Water:
			transform.gameObject.layer = LayerMasks.MASK_DomesticLandmarkCollision;
			break;
		}
	}

	private void OnCaptured()
	{
		if (DomesticState == DomesticStateType.Water)
		{
			return;
		}
		Message_AnimalCaptured message = new Message_AnimalCaptured(Messages.AnimalCaptured, Network.NetworkIDManager, ObjectIndex, p_isAnimalCaptured: true);
		if (Raft_Network.IsHost)
		{
			Captured();
			if (Network != null)
			{
				Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else if (Network != null)
		{
			Network.SendP2P(Network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void OnStartCarry(Network_Player player)
	{
		Message_NetworkBehaviour_ID_SteamID message = new Message_NetworkBehaviour_ID_SteamID(Messages.StartCarryAIBehaviour, Network.NetworkIDManager, ObjectIndex, player.steamID);
		if (Raft_Network.IsHost)
		{
			StartCarry(player);
			if (Network != null)
			{
				Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else if (Network != null)
		{
			Network.SendP2P(Network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void OnStopCarry(Network_Player player, bool force)
	{
		Message_NetworkBehaviour_ID_SteamID message = new Message_NetworkBehaviour_ID_SteamID(Messages.StopCarryAIBehaviour, Network.NetworkIDManager, ObjectIndex, player.steamID);
		if (Raft_Network.IsHost)
		{
			StopCarry(player);
			if (Network != null)
			{
				Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else if (Network != null)
		{
			Network.SendP2P(Network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	protected virtual void QuickTame()
	{
		IsTamed = true;
		carryScript.EnableCollider();
		DomesticState = DomesticStateType.Raft;
		SetNameTagEnabled(value: true);
		if (Raft_Network.IsHost)
		{
			Message_NetworkBehaviour_ID message = new Message_NetworkBehaviour_ID(Messages.QuickTameAnimal, Network.NetworkIDManager, ObjectIndex);
			Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public void QuickTameLate()
	{
		StartCoroutine(QuickTameDelayed());
	}

	private IEnumerator QuickTameDelayed()
	{
		yield return new WaitForSeconds(0.05f);
		QuickTame();
	}

	public override void SetNetworkIndexes(uint objectIndex, uint entityObjectIndex)
	{
		base.SetNetworkIndexes(objectIndex, entityObjectIndex);
		nameTag.Initialize(networkEntity);
	}

	public virtual void SetNameTagEnabled(bool value)
	{
		if (nameTag != null)
		{
			nameTag.gameObject.SetActiveSafe(value);
		}
	}

	protected virtual void Captured()
	{
		if (!(stateMachine_Domestic != null))
		{
			return;
		}
		stateMachine_Domestic.ChangeState(capturedState);
		RuntimeManager.PlayOneShot(stateMachine_Domestic.er_distressed, transform.position);
		if (!hasGivenAchievment)
		{
			hasGivenAchievment = true;
			AchievementHandler.AddCapturedAnimal();
			if (rareMaterialScript != null && rareMaterialScript.randomizer.lastRandomizedIndex != 0)
			{
				AchievementHandler.UnlockAchievement(AchievementType.ach_captureRareAnimal);
			}
		}
	}

	private IEnumerator StartCarryDelayed(Network_Player player, float delay)
	{
		yield return new WaitForSeconds(delay);
		StartCarry(player);
	}

	protected virtual bool StartCarry(Network_Player player)
	{
		if (carryScript == null || carryScript.carryingPlayer != null)
		{
			return false;
		}
		if (movement != null)
		{
			movement.SetMovementType(MovementType.None);
			movement.waterFloat.DeActivate();
		}
		if (carryScript != null)
		{
			SetStartCarriedOffsets(player);
			carryScript.StartCarrying(player);
		}
		if (stateMachine_Domestic != null)
		{
			stateMachine_Domestic.ChangeState(carriedState);
		}
		if (captureScript != null)
		{
			captureScript.IsCaptured = false;
			captureScript.IsCapturable = false;
		}
		SetNameTagEnabled(value: true);
		return true;
	}

	protected virtual bool StopCarry(Network_Player player)
	{
		if (carryScript == null || carryScript.carryingPlayer == null)
		{
			return false;
		}
		if (carryScript != null)
		{
			carryScript.StopCarrying(player);
			SetStopCarriedOffsets(player);
		}
		AI_State_CirculateSpawn[] componentsInChildren = GetComponentsInChildren<AI_State_CirculateSpawn>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetNewSpawnPoint(stateMachine_Domestic.transform.position);
		}
		if (captureScript != null)
		{
			captureScript.IsCapturable = true;
		}
		SetDomesticStateDependingOnPlayerState(player);
		movement.useGravity = true;
		return true;
	}

	protected virtual void SetStartCarriedOffsets(Network_Player player)
	{
		if (player == null || player.rightHandParent == null || player.currentModel == null || stateMachine_Domestic == null)
		{
			return;
		}
		stateMachine_Domestic.transform.SetParent(player.rightHandParent);
		if (player.IsLocalPlayer)
		{
			ListenToCharacterPerspectiveChange(player, value: true);
			if (player.currentModel.thirdPersonSettings.ThirdPersonState)
			{
				stateMachine_Domestic.transform.localPosition = thirdPcarryPosOffset;
				stateMachine_Domestic.transform.localEulerAngles = thirdPcarryRotOffset;
			}
			else
			{
				stateMachine_Domestic.transform.localPosition = carryPosOffset;
				stateMachine_Domestic.transform.localEulerAngles = carryRotOffset;
			}
		}
		else
		{
			stateMachine_Domestic.transform.localPosition = thirdPcarryPosOffset;
			stateMachine_Domestic.transform.localEulerAngles = thirdPcarryRotOffset;
		}
		stateMachine_Domestic.movement.model.localEulerAngles = Vector3.zero;
	}

	protected virtual void SetStopCarriedOffsets(Network_Player player)
	{
		stateMachine_Domestic.transform.SetParent(player.transform.parent);
		stateMachine_Domestic.transform.position = player.transform.position - player.transform.up * 0.9f;
		stateMachine_Domestic.transform.rotation = Quaternion.Euler(Vector3.zero);
		stateMachine_Domestic.transform.localScale = new Vector3(scaledSize, scaledSize, scaledSize);
		if (player.IsLocalPlayer)
		{
			ListenToCharacterPerspectiveChange(player, value: false);
		}
	}

	private void ListenToCharacterPerspectiveChange(Network_Player playerNetwork, bool value)
	{
		if (value)
		{
			ThirdPerson thirdPersonSettings = playerNetwork.currentModel.thirdPersonSettings;
			thirdPersonSettings.OnThirdpersonModelChange = (Action<bool>)Delegate.Combine(thirdPersonSettings.OnThirdpersonModelChange, new Action<bool>(OnPerspectiveSwap));
		}
		else
		{
			ThirdPerson thirdPersonSettings2 = playerNetwork.currentModel.thirdPersonSettings;
			thirdPersonSettings2.OnThirdpersonModelChange = (Action<bool>)Delegate.Remove(thirdPersonSettings2.OnThirdpersonModelChange, new Action<bool>(OnPerspectiveSwap));
		}
	}

	private void OnPerspectiveSwap(bool toThirdPerson)
	{
		if (toThirdPerson)
		{
			stateMachine_Domestic.transform.localPosition = thirdPcarryPosOffset;
			stateMachine_Domestic.transform.localEulerAngles = thirdPcarryRotOffset;
		}
		else
		{
			stateMachine_Domestic.transform.localPosition = carryPosOffset;
			stateMachine_Domestic.transform.localEulerAngles = carryRotOffset;
		}
	}

	private void HandleHunger()
	{
		if (captureScript.IsCaptured || networkEntity.IsDead || carryScript.IsBeingCarried)
		{
			return;
		}
		HungerTimer = Mathf.Clamp(HungerTimer - Time.deltaTime, 0f, float.MaxValue);
		if (IsStarving && stateMachine_Domestic.currentState != hungryState && stateMachine_Domestic.currentState != eatState && stateMachine_Domestic.currentState == defaultRaftState)
		{
			backToHungerStateCooldown -= Time.deltaTime;
			if (backToHungerStateCooldown <= 0f && stateMachine_Domestic.currentState != waterState)
			{
				stateMachine_Domestic.ChangeState(hungryState);
			}
		}
	}

	private void HandleStarving()
	{
		stateMachine_Domestic.animController.SetBool("Starving", IsStarving);
	}

	private void InitializeCaptureScript()
	{
		captureScript = GetComponentInChildren<CaptureAnimal>();
		if (captureScript != null)
		{
			CaptureAnimal captureAnimal = captureScript;
			captureAnimal.OnIsCaptured = (Action)Delegate.Combine(captureAnimal.OnIsCaptured, new Action(OnCaptured));
		}
		else
		{
			Debug.LogError("Can't find 'CaptureAnimal Script' on self or children of " + gameObject.name);
		}
	}

	private void InitializeCarryScript()
	{
		carryScript = GetComponentInChildren<Carry>();
		if (carryScript != null)
		{
			carryScript.DisableCollider();
			Carry carry = carryScript;
			carry.OnStartCarry = (Action<Network_Player>)Delegate.Combine(carry.OnStartCarry, new Action<Network_Player>(OnStartCarry));
			Carry carry2 = carryScript;
			carry2.OnStopCarry = (Action<Network_Player, bool>)Delegate.Combine(carry2.OnStopCarry, new Action<Network_Player, bool>(OnStopCarry));
		}
		else
		{
			Debug.LogError("Can't find 'Carry Script' on self or children of " + gameObject.name);
		}
	}

	private void SetDomesticStateDependingOnPlayerState(Network_Player player)
	{
		if (player.PersonController.controllerType == ControllerType.Water)
		{
			DomesticState = DomesticStateType.Water;
		}
		else if (player.PersonController.HasRaftAsParent)
		{
			DomesticState = DomesticStateType.Raft;
		}
		else if (!player.PersonController.controller.isGrounded)
		{
			if (Physics.Linecast(transform.position, new Vector3(transform.position.x, -0.5f, transform.position.z), out var hitInfo, LayerMasks.MASK_GroundMask, QueryTriggerInteraction.Ignore))
			{
				if (Helper.ObjectIsOnLayerParentedToRaft(hitInfo.transform.gameObject, LayerMasks.MASK_GroundMask_Raft))
				{
					DomesticState = DomesticStateType.Raft;
				}
				else if (Helper.ObjectIsOnLayer(hitInfo.transform.gameObject, LayerMasks.MASK_GroundMask_NonRaft))
				{
					DomesticState = DomesticStateType.Landmark;
				}
			}
			else
			{
				DomesticState = DomesticStateType.Water;
			}
		}
		else
		{
			DomesticState = DomesticStateType.Landmark;
		}
	}

	private void RefreshDomesticState(bool changeAIState = true)
	{
		if (lockedPivot == null && SingletonGeneric<GameManager>.Singleton != null)
		{
			lockedPivot = SingletonGeneric<GameManager>.Singleton.lockedPivot;
		}
		switch (DomesticState)
		{
		case DomesticStateType.Landmark:
			if (changeAIState && stateMachine_Domestic.currentState != defaultLandmarkState)
			{
				stateMachine_Domestic.ChangeState(defaultLandmarkState);
			}
			movement.useGravity = true;
			stateMachine_Domestic.transform.SetParentSafe(null);
			break;
		case DomesticStateType.Raft:
		{
			if (changeAIState && stateMachine_Domestic.currentState != defaultRaftState)
			{
				stateMachine_Domestic.ChangeState(defaultRaftState);
			}
			movement.useGravity = true;
			if (Raft_Network.InMenuScene)
			{
				break;
			}
			if (Physics.Raycast(stateMachine_Domestic.transform.position + Vector3.up * 0.5f, Vector3.down, out var hitInfo, 50f, LayerMasks.MASK_GroundMask, QueryTriggerInteraction.Ignore))
			{
				if (Helper.ObjectIsOnLayerParentedToRaft(hitInfo.transform.gameObject, LayerMasks.MASK_GroundMask_Raft))
				{
					stateMachine_Domestic.transform.SetParentSafe(lockedPivot);
				}
				else if (Helper.ObjectIsOnLayer(hitInfo.transform.gameObject, LayerMasks.MASK_GroundMask_NonRaft))
				{
					stateMachine_Domestic.transform.SetParentSafe(null);
				}
			}
			else
			{
				stateMachine_Domestic.transform.SetParentSafe(lockedPivot);
			}
			break;
		}
		case DomesticStateType.Water:
			if (stateMachine_Domestic.currentState != waterState)
			{
				stateMachine_Domestic.ChangeState(waterState);
			}
			break;
		}
	}

	public void SetToMoveState()
	{
		stateMachine_Domestic.currentState = defaultRaftState;
	}

	public override RGD Serialize_Save()
	{
		if (!ConnectedToSpawner && !networkEntity.IsDead)
		{
			return new RGD_Domestic(this);
		}
		return null;
	}

	public override RGD_AI_NetworkBehaviour Create_SaveData()
	{
		if (ConnectedToSpawner && !networkEntity.IsDead)
		{
			return new RGD_Domestic(this);
		}
		return null;
	}

	public override void OnRestoreFinished(RGD_AI_NetworkBehaviour rgdNetworkBehaviour)
	{
		base.OnRestoreFinished(rgdNetworkBehaviour);
		RGD_Domestic rGD_Domestic = rgdNetworkBehaviour as RGD_Domestic;
		if (rGD_Domestic != null)
		{
			rGD_Domestic.RestoreDomesticBehaviour(this);
			_domesticState = rGD_Domestic.domesticState;
			if (DomesticState == DomesticStateType.Raft)
			{
				movement.useGravity = true;
			}
			if (!IsTamed)
			{
				SetNameTagEnabled(value: false);
			}
		}
		if (stateMachine_Domestic.currentState == carriedState)
		{
			carryScript.AbortCarry();
			DomesticState = DomesticStateType.Raft;
		}
	}

	public override void Serialize_CreateFromIDManager(List<Message_NetworkBehaviour> msgs)
	{
		base.Serialize_CreateFromIDManager(msgs);
		if (!ConnectedToSpawner)
		{
			msgs.Add(new Message_Domestic_Create(Messages.CreateDomestic, Network.NetworkIDManager, ObjectIndex, this, carryScript.carryingPlayer ? carryScript.carryingPlayer.steamID : ((Network_UserId)0uL)));
			if (rareMaterialScript != null)
			{
				msgs.Add(new Message_RareMaterial(Messages.RareMaterial, Network.NetworkIDManager, ObjectIndex, rareMaterialScript));
			}
		}
	}

	public override void Serialize_Update()
	{
		base.Serialize_Update();
	}

	public override void Serialize_Update_Animal()
	{
		AddMessage(new Message_Domestic_Update(Messages.AINetworkBehaviourUpdate_Domestic, Network.NetworkIDManager, stateMachine_Domestic, ObjectIndex));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.CreateDomestic:
		{
			Message_Domestic_Create message_Domestic_Create = msg as Message_Domestic_Create;
			if (message_Domestic_Create.isTamed)
			{
				QuickTame();
				Network_UserId steamID = message_Domestic_Create.SteamID;
				if (steamID.IsValid())
				{
					Network_Player playerFromID = ComponentManager<Raft_Network>.Value.GetPlayerFromID(steamID);
					if ((bool)playerFromID)
					{
						StartCoroutine(StartCarryDelayed(playerFromID, 0.25f));
					}
				}
			}
			else
			{
				SetNameTagEnabled(value: false);
			}
			return false;
		}
		case Messages.RareMaterial:
			(msg as Message_RareMaterial).RestoreMaterial(rareMaterialScript);
			return true;
		case Messages.AINetworkBehaviourUpdate_Domestic:
		{
			Message_Domestic_Update message_Domestic_Update = msg as Message_Domestic_Update;
			isNetworkPositionLocal = message_Domestic_Update.ParentedToRaft;
			break;
		}
		case Messages.AnimalCaptured:
			Captured();
			return true;
		case Messages.StartCarryAIBehaviour:
		{
			Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID2 = msg as Message_NetworkBehaviour_ID_SteamID;
			return StartCarry(Network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID2.SteamID));
		}
		case Messages.StopCarryAIBehaviour:
		{
			Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID = msg as Message_NetworkBehaviour_ID_SteamID;
			return StopCarry(Network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID));
		}
		case Messages.QuickTameAnimal:
			QuickTame();
			return false;
		}
		return base.Deserialize(msg, remoteID);
	}
}
