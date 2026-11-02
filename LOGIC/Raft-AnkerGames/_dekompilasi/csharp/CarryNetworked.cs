using System;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.Rendering;

public class CarryNetworked : MonoBehaviour_ID_Network
{
	public Action OnDestroyed;

	public Carry carryScript;

	[SerializeField]
	protected Transform objectToParent;

	[SerializeField]
	private AreaZone zoneLimitation;

	[SerializeField]
	private Transform respawnPoint;

	[SerializeField]
	[Range(0f, 1f)]
	private float minValidDropAngle;

	[SerializeField]
	private MeshRenderer ghostOriginalRenderer;

	[SerializeField]
	private Renderer[] shadowCasters;

	[SerializeField]
	private GizmoBox occupyingGizmo;

	[SerializeField]
	protected bool changeScale;

	[SerializeField]
	private Vector3 carryPosOffset = Vector3.zero;

	[SerializeField]
	private Vector3 carryRotOffset = Vector3.zero;

	[SerializeField]
	private Vector3 carryLocalScale = Vector3.one;

	[SerializeField]
	private Vector3 thirdPcarryPosOffset = Vector3.zero;

	[SerializeField]
	private Vector3 thirdPcarryRotOffset = Vector3.zero;

	[SerializeField]
	private Vector3 thirdPcarryLocalScale = Vector3.one;

	public EventReference er_startCarry;

	public EventReference er_stopCarry;

	protected Raft_Network network;

	private bool hasBeenInitialized;

	protected Transform currentGhost;

	protected Transform startingParent;

	protected GizmoBox currentGhostGizmo;

	private Vector3 originalScale;

	public AreaZone ZoneLimitation
	{
		set
		{
			zoneLimitation = value;
		}
	}

	protected virtual void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		originalScale = objectToParent.localScale;
	}

	public void OnPreDestroy()
	{
		if (carryScript != null && carryScript.carryingPlayer != null)
		{
			if (carryScript.carryingPlayer.IsLocalPlayer)
			{
				ListenToCharacterPerspectiveChange(carryScript.carryingPlayer, value: false);
			}
			StopCarry(carryScript.carryingPlayer, carryScript.carryingPlayer.FeetPosition);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (hasBeenInitialized)
		{
			NetworkIDManager.RemoveNetworkID(this, typeof(CarryNetworked));
			if (currentGhost != null)
			{
				UnityEngine.Object.Destroy(currentGhost.gameObject);
			}
		}
		OnDestroyed?.Invoke();
	}

	protected virtual void Update()
	{
		if (Raft_Network.IsHost && zoneLimitation != null && network != null && zoneLimitation.GetNormalizedDistanceToPoint(carryScript.transform.position) <= 0f)
		{
			NetworkIDManager.SendIDBehaviourDead(ObjectIndex, typeof(CarryNetworked), removeInstantly: true);
		}
		else if (carryScript.IsBeingCarried && carryScript.carryingPlayer.IsLocalPlayer)
		{
			HandleGhost();
		}
	}

	protected virtual void OnStartCarry(Network_Player player)
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		Message_NetworkBehaviour_ID_SteamID message = new Message_NetworkBehaviour_ID_SteamID(Messages.StartCarryItem, network.NetworkIDManager, ObjectIndex, player.steamID);
		if (Raft_Network.IsHost)
		{
			if (StartCarry(player))
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	protected virtual void OnStopCarry(Network_Player player, bool force)
	{
		if (force || AllowPlacement())
		{
			Vector3 dropPosition = GetDropPosition(player, force);
			Message_StopCarryPosition message = new Message_StopCarryPosition(Messages.StopCarryItem, network.NetworkIDManager, ObjectIndex, player.steamID, dropPosition);
			if (Raft_Network.IsHost)
			{
				StopCarry(player, dropPosition);
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public void Initialize(uint objectIndex, Transform startingParent)
	{
		if (!hasBeenInitialized)
		{
			hasBeenInitialized = true;
			Carry carry = carryScript;
			carry.OnStartCarry = (Action<Network_Player>)Delegate.Combine(carry.OnStartCarry, new Action<Network_Player>(OnStartCarry));
			Carry carry2 = carryScript;
			carry2.OnStopCarry = (Action<Network_Player, bool>)Delegate.Combine(carry2.OnStopCarry, new Action<Network_Player, bool>(OnStopCarry));
			this.startingParent = startingParent;
			ObjectIndex = objectIndex;
			NetworkIDManager.AddNetworkID(this, typeof(CarryNetworked));
		}
	}

	public virtual RGD_CarryNetwork GetSaveData()
	{
		return new RGD_CarryNetwork(this);
	}

	private void HandleGhost()
	{
		if (ghostOriginalRenderer != null && currentGhost == null)
		{
			GameObject gameObject = new GameObject("ghost");
			currentGhost = gameObject.transform;
			GameObject gameObject2 = new GameObject("ghost");
			MeshRenderer meshRenderer = gameObject2.AddComponent<MeshRenderer>();
			MeshFilter meshFilter = gameObject2.AddComponent<MeshFilter>();
			if (occupyingGizmo != null)
			{
				GizmoBox gizmoBox = gameObject2.AddComponent<GizmoBox>();
				gizmoBox.size = occupyingGizmo.size;
				currentGhostGizmo = gizmoBox;
			}
			meshRenderer.sharedMaterials = ghostOriginalRenderer.sharedMaterials;
			meshFilter.sharedMesh = ghostOriginalRenderer.GetComponent<MeshFilter>().sharedMesh;
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			meshRenderer.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetFloat("_Shimmer", 0f);
			meshRenderer.SetPropertyBlock(materialPropertyBlock);
			gameObject2.transform.SetParentSafe(gameObject.transform);
			gameObject2.transform.localPosition = ghostOriginalRenderer.transform.localPosition;
			gameObject2.transform.localEulerAngles = ghostOriginalRenderer.transform.localEulerAngles;
			gameObject2.transform.localScale = ghostOriginalRenderer.transform.localScale;
		}
		if (currentGhost != null)
		{
			currentGhost.position = GetNewGhostPosition(force: false);
			SetGhostRotation();
			carryScript.AllowDisplayText = currentGhost.position != carryScript.carryingPlayer.FeetPosition;
		}
	}

	protected virtual void SetGhostRotation()
	{
		currentGhost.eulerAngles = Vector3.zero;
	}

	protected virtual Vector3 GetNewGhostPosition(bool force)
	{
		return GetDropPosition(carryScript.carryingPlayer, force);
	}

	protected Vector3 GetDropPosition(Network_Player player, bool force)
	{
		if (player == null)
		{
			return Vector3.zero;
		}
		if (force)
		{
			return player.FeetPosition;
		}
		if (Helper.HitAtCursor(out var hit, Player.UseDistance * 1.3f, LayerMasks.MASK_Obstruction, QueryTriggerInteraction.Ignore) && hit.normal.y >= minValidDropAngle)
		{
			return hit.point;
		}
		return player.FeetPosition;
	}

	public virtual bool StartCarry(Network_Player player)
	{
		if (player == null || carryScript == null || carryScript.carryingPlayer != null)
		{
			return false;
		}
		if (carryScript.body != null)
		{
			carryScript.body.isKinematic = true;
		}
		if (player.IsLocalPlayer)
		{
			SetGhostActiveStatus(active: true);
			SetShadowMode(castShadows: false);
		}
		if (!er_startCarry.IsNull)
		{
			PlayOneShot(er_startCarry, transform.position);
		}
		carryScript.StartCarrying(player);
		SetStartCarriedOffsets(player);
		return true;
	}

	protected bool StopCarry(Network_Player player, Vector3 position)
	{
		if (carryScript == null || carryScript.carryingPlayer == null)
		{
			return false;
		}
		if (carryScript.body != null)
		{
			carryScript.body.isKinematic = false;
		}
		if (player.IsLocalPlayer)
		{
			SetGhostActiveStatus(active: false);
			SetShadowMode(castShadows: true);
		}
		if (!er_stopCarry.IsNull)
		{
			PlayOneShot(er_stopCarry, position);
		}
		carryScript.StopCarrying(player);
		SetStopCarriedOffsets(player);
		if (objectToParent != null)
		{
			objectToParent.transform.SetParentSafe(startingParent);
			objectToParent.localScale = originalScale;
			objectToParent.transform.position = position;
		}
		return true;
	}

	private void SetStartCarriedOffsets(Network_Player player)
	{
		if (player == null || player.rightHandParent == null || player.currentModel == null || objectToParent == null)
		{
			return;
		}
		objectToParent.transform.SetParent(player.rightHandParent);
		Vector3 localPosition = thirdPcarryPosOffset;
		Vector3 localEulerAngles = thirdPcarryRotOffset;
		Vector3 localScale = thirdPcarryLocalScale;
		if (player.IsLocalPlayer)
		{
			ListenToCharacterPerspectiveChange(player, value: true);
			if (!player.currentModel.thirdPersonSettings.ThirdPersonState)
			{
				localPosition = carryPosOffset;
				localEulerAngles = carryRotOffset;
				localScale = carryLocalScale;
			}
		}
		objectToParent.transform.localPosition = localPosition;
		objectToParent.transform.localEulerAngles = localEulerAngles;
		if (changeScale)
		{
			objectToParent.transform.localScale = localScale;
		}
	}

	private void SetStopCarriedOffsets(Network_Player player)
	{
		objectToParent.transform.SetParent(player.transform.parent);
		objectToParent.transform.position = player.transform.position - player.transform.up * 0.9f;
		objectToParent.transform.rotation = Quaternion.Euler(Vector3.zero);
		if (changeScale)
		{
			objectToParent.transform.localScale = Vector3.one;
		}
		if (player.IsLocalPlayer)
		{
			ListenToCharacterPerspectiveChange(player, value: false);
		}
	}

	protected void ListenToCharacterPerspectiveChange(Network_Player playerNetwork, bool value)
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

	protected void PlayOneShot(EventReference eventRef, Vector3 position)
	{
		if (!eventRef.IsNull)
		{
			RuntimeManager.PlayOneShot(eventRef, position);
		}
	}

	private void OnPerspectiveSwap(bool toThirdPerson)
	{
		if (!(objectToParent == null))
		{
			Vector3 localPosition = (toThirdPerson ? thirdPcarryPosOffset : carryPosOffset);
			Vector3 localEulerAngles = (toThirdPerson ? thirdPcarryRotOffset : carryRotOffset);
			Vector3 localScale = (toThirdPerson ? thirdPcarryLocalScale : carryLocalScale);
			objectToParent.transform.localPosition = localPosition;
			objectToParent.transform.localEulerAngles = localEulerAngles;
			if (changeScale)
			{
				objectToParent.transform.localScale = localScale;
			}
		}
	}

	protected bool AllowPlacement()
	{
		if (currentGhostGizmo == null)
		{
			return true;
		}
		if (currentGhostGizmo.IsColliding(includeInactiveObjects: false, LayerMasks.MASK_Players, 1f, QueryTriggerInteraction.Ignore))
		{
			return false;
		}
		return true;
	}

	protected void SetGhostActiveStatus(bool active)
	{
		if (currentGhost != null)
		{
			currentGhost.gameObject.SetActiveSafe(active);
		}
	}

	protected void SetShadowMode(bool castShadows)
	{
		if (shadowCasters == null || shadowCasters.Length == 0)
		{
			return;
		}
		Renderer[] array = shadowCasters;
		foreach (Renderer renderer in array)
		{
			if (renderer != null)
			{
				renderer.shadowCastingMode = (castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off);
			}
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		switch (msg.Type)
		{
		case Messages.StartCarryItem:
		{
			Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID = msg as Message_NetworkBehaviour_ID_SteamID;
			return StartCarry(network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID));
		}
		case Messages.StopCarryItem:
		{
			Message_StopCarryPosition message_StopCarryPosition = msg as Message_StopCarryPosition;
			return StopCarry(network.GetPlayerFromID(message_StopCarryPosition.SteamID), message_StopCarryPosition.Position);
		}
		case Messages.SetIDBehaviourDead:
			if (respawnPoint == null)
			{
				OnPreDestroy();
				UnityEngine.Object.Destroy(gameObject);
			}
			else
			{
				OnPreDestroy();
				transform.position = respawnPoint.position;
				transform.rotation = respawnPoint.rotation;
			}
			return true;
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
