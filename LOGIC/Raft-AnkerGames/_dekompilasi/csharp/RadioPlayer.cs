using System;
using FMOD.Studio;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class RadioPlayer : MonoBehaviour_ID_Network, IRaycastable
{
	[Serializable]
	public class CassetteConnection
	{
		public int cassetteIndex;

		public Item_Base itemBase;

		public Mesh casetteMesh;

		public Material material;

		public StudioEventEmitter cassetteEmitter;

		public int timelinePosition;

		public void SetEmitterToCurrentTimeline()
		{
			if (cassetteEmitter != null)
			{
				cassetteEmitter.EventInstance.setTimelinePosition(timelinePosition);
			}
		}

		public int GetTimelinePosition()
		{
			int position = 0;
			if (Raft_Network.IsHost)
			{
				cassetteEmitter.EventInstance.getPlaybackState(out var state);
				switch (state)
				{
				case PLAYBACK_STATE.PLAYING:
				case PLAYBACK_STATE.STARTING:
					cassetteEmitter.EventInstance.getTimelinePosition(out position);
					if (position == 0)
					{
						position = timelinePosition;
					}
					break;
				case PLAYBACK_STATE.SUSTAINING:
				case PLAYBACK_STATE.STOPPED:
				case PLAYBACK_STATE.STOPPING:
					position = timelinePosition;
					break;
				}
			}
			else
			{
				position = timelinePosition;
			}
			return position;
		}

		public void SaveTimelinePosition()
		{
			int num = GetTimelinePosition();
			timelinePosition = num;
		}
	}

	[SerializeField]
	private MeshRenderer renderer;

	[SerializeField]
	private MeshFilter cassetteMeshFilter;

	[SerializeField]
	private MeshRenderer cassetteMeshRenderer;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private CassetteConnection[] cassetteConnections;

	[SerializeField]
	private bool radioIsOn;

	[SerializeField]
	private int cassetteIndex;

	[SerializeField]
	private InteractableButton_Networked cassetteButton;

	[SerializeField]
	private StudioEventEmitter eventEmitter_radioOn;

	private static RadioStation radioStation;

	private static Network_Player localPlayer;

	private DisplayTextManager displayText;

	private bool isPlaced;

	public bool HasCassette => CassetteIndex > 0;

	public int CassetteIndex
	{
		get
		{
			return cassetteIndex;
		}
		private set
		{
			cassetteIndex = value;
			if (cassetteIndex > 0)
			{
				eventEmitter_radioOn.SetParameter("casette", 6f);
			}
			else
			{
				eventEmitter_radioOn.SetParameter("casette", 0f);
			}
		}
	}

	public void Start()
	{
		if (radioStation == null)
		{
			radioStation = ComponentManager<RadioStation>.Value;
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (isPlaced)
		{
			TurnOffRadio();
			NetworkIDManager.RemoveNetworkID(this);
		}
	}

	public void RestoreRadio(RGD_RadioPlayer rgdRadio)
	{
		radioIsOn = rgdRadio.onState;
		CassetteIndex = rgdRadio.cassetteIndex;
		if (CassetteIndex > 0)
		{
			CassetteConnection cassetConnectionIndexFromItem = GetCassetConnectionIndexFromItem(GetItemFromCassetIndex(CassetteIndex));
			if (cassetConnectionIndexFromItem != null)
			{
				if (cassetteMeshFilter != null)
				{
					cassetteMeshFilter.sharedMesh = cassetConnectionIndexFromItem.casetteMesh;
					cassetteMeshRenderer.sharedMaterial = cassetConnectionIndexFromItem.material;
				}
				cassetConnectionIndexFromItem.timelinePosition = rgdRadio.timelinePosition;
				cassetConnectionIndexFromItem.cassetteEmitter.EventInstance.setTimelinePosition(cassetConnectionIndexFromItem.timelinePosition);
			}
			animator?.SetTrigger("InsertCassette");
		}
		else
		{
			animator?.SetTrigger("TakeCassette");
		}
		CheckRestoreActivation();
		UpdateRadiomesh();
	}

	public void CheckRestoreActivation()
	{
		if (radioIsOn)
		{
			TurnOnRadio();
		}
		else
		{
			TurnOffRadio();
		}
	}

	public void ToggleRadio()
	{
		radioIsOn = !radioIsOn;
		if (radioIsOn)
		{
			TurnOnRadio();
		}
		else
		{
			TurnOffRadio();
		}
		UpdateRadiomesh();
	}

	public void TurnOnRadio()
	{
		if (CassetteIndex == 0)
		{
			eventEmitter_radioOn.Play();
		}
		else
		{
			CassetteConnection cassetConnectionIndexFromIndex = GetCassetConnectionIndexFromIndex(CassetteIndex);
			if (cassetConnectionIndexFromIndex != null)
			{
				cassetConnectionIndexFromIndex.cassetteEmitter.Play();
				cassetConnectionIndexFromIndex.SetEmitterToCurrentTimeline();
			}
		}
		animator.SetBool("On", value: true);
	}

	public void TurnOffRadio()
	{
		animator.SetBool("On", value: false);
		eventEmitter_radioOn.EventInstance.keyOff();
		CassetteConnection[] array = cassetteConnections;
		foreach (CassetteConnection cassetteConnection in array)
		{
			if (cassetteConnection != null)
			{
				if (Raft_Network.IsHost && CassetteIndex == cassetteConnection.cassetteIndex)
				{
					cassetteConnection.SaveTimelinePosition();
				}
				cassetteConnection.cassetteEmitter.Stop();
			}
		}
	}

	public void OnIsRayed()
	{
		if (isPlaced)
		{
			if (localPlayer == null)
			{
				localPlayer = ComponentManager<Network_Player>.Value;
			}
			HandleTurningOnAndOff();
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (displayText != null)
		{
			displayText.HideDisplayTexts();
		}
	}

	private void HandleTurningOnAndOff()
	{
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		if (radioIsOn)
		{
			displayText.ShowText(Helper.GetTerm("Game/TurnOff"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		}
		else
		{
			displayText.ShowText(Helper.GetTerm("Game/TurnOn"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		}
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			return;
		}
		int timelinePosition = 0;
		if (Raft_Network.IsHost)
		{
			CassetteConnection cassetConnectionIndexFromIndex = GetCassetConnectionIndexFromIndex(CassetteIndex);
			if (cassetConnectionIndexFromIndex != null)
			{
				timelinePosition = cassetConnectionIndexFromIndex.timelinePosition;
			}
		}
		Message_RadioPlayer_Toggle message = new Message_RadioPlayer_Toggle(Messages.Radio_Toggle, localPlayer.Network.NetworkIDManager, ObjectIndex, timelinePosition);
		if (Raft_Network.IsHost)
		{
			localPlayer.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			ToggleRadio();
		}
		else
		{
			localPlayer.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		displayText.HideDisplayTexts();
	}

	private void UpdateRadiomesh()
	{
		if (radioIsOn)
		{
			renderer.material.EnableKeyword("_EMISSION");
		}
		else
		{
			renderer.material.DisableKeyword("_EMISSION");
		}
	}

	private void OnBlockPlaced()
	{
		isPlaced = true;
		localPlayer = ComponentManager<Network_Player>.Value;
		displayText = ComponentManager<DisplayTextManager>.Value;
		NetworkIDManager.AddNetworkID(this);
		UpdateRadiomesh();
		ToggleRadio();
	}

	private bool InsertCassette(Network_Player interactingPlayer, int cassetteIndex)
	{
		if (HasCassette)
		{
			return false;
		}
		CassetteConnection cassetConnectionIndexFromIndex = GetCassetConnectionIndexFromIndex(cassetteIndex);
		if (cassetConnectionIndexFromIndex == null)
		{
			return false;
		}
		if (interactingPlayer.IsLocalPlayer)
		{
			interactingPlayer.Animator.SetAnimation(PlayerAnimation.Trigger_Plant);
			interactingPlayer.Inventory.RemoveItem(cassetConnectionIndexFromIndex.itemBase.UniqueName, 1);
		}
		CassetteIndex = cassetConnectionIndexFromIndex.cassetteIndex;
		if (cassetteMeshFilter != null)
		{
			cassetteMeshFilter.sharedMesh = cassetConnectionIndexFromIndex.casetteMesh;
			cassetteMeshRenderer.sharedMaterial = cassetConnectionIndexFromIndex.material;
		}
		if (radioIsOn)
		{
			eventEmitter_radioOn.EventInstance.keyOff();
			cassetConnectionIndexFromIndex.cassetteEmitter.Play();
		}
		cassetConnectionIndexFromIndex.timelinePosition = 0;
		cassetConnectionIndexFromIndex.cassetteEmitter.EventInstance.setTimelinePosition(0);
		animator?.SetTrigger("InsertCassette");
		return true;
	}

	private bool TakeCassette(Network_Player interactingPlayer)
	{
		if (!HasCassette)
		{
			return false;
		}
		Item_Base itemFromCassetIndex = GetItemFromCassetIndex(CassetteIndex);
		if (itemFromCassetIndex != null)
		{
			CassetteConnection cassetConnectionIndexFromItem = GetCassetConnectionIndexFromItem(itemFromCassetIndex);
			if (radioIsOn)
			{
				eventEmitter_radioOn.Play();
			}
			cassetConnectionIndexFromItem?.cassetteEmitter.Stop();
			if (interactingPlayer.IsLocalPlayer)
			{
				interactingPlayer.Animator.SetAnimation(PlayerAnimation.Trigger_GrabItem);
				interactingPlayer.Inventory.AddItem(itemFromCassetIndex.UniqueName, 1);
			}
			CassetteIndex = 0;
			animator?.SetTrigger("TakeCassette");
			return true;
		}
		return false;
	}

	public Item_Base GetItemFromCassetIndex(int cassetteIndex)
	{
		CassetteConnection[] array = cassetteConnections;
		foreach (CassetteConnection cassetteConnection in array)
		{
			if (cassetteConnection.cassetteIndex == cassetteIndex)
			{
				return cassetteConnection.itemBase;
			}
		}
		return null;
	}

	public CassetteConnection GetCassetConnectionIndexFromIndex(int cassetteIndex)
	{
		CassetteConnection[] array = cassetteConnections;
		foreach (CassetteConnection cassetteConnection in array)
		{
			if (cassetteConnection.cassetteIndex == cassetteIndex)
			{
				return cassetteConnection;
			}
		}
		return null;
	}

	public CassetteConnection GetCassetConnectionIndexFromItem(Item_Base item)
	{
		if (item == null)
		{
			return null;
		}
		CassetteConnection[] array = cassetteConnections;
		foreach (CassetteConnection cassetteConnection in array)
		{
			if (cassetteConnection.itemBase.UniqueIndex == item.UniqueIndex)
			{
				return cassetteConnection;
			}
		}
		return null;
	}

	public bool IsItemACassette(Item_Base item)
	{
		return GetCassetConnectionIndexFromItem(item) != null;
	}

	public override RGD Serialize_Save()
	{
		if (isPlaced)
		{
			return new RGD_Block_Radio(RGDType.Block_Radio, GetComponent<Block>(), this);
		}
		return base.Serialize_Save();
	}

	public RGD_RadioPlayer GetRGDRadioPlayer()
	{
		RGD_RadioPlayer rGD_RadioPlayer = new RGD_RadioPlayer();
		rGD_RadioPlayer.onState = radioIsOn;
		rGD_RadioPlayer.cassetteIndex = CassetteIndex;
		rGD_RadioPlayer.objectIndex = ObjectIndex;
		CassetteConnection cassetConnectionIndexFromIndex = GetCassetConnectionIndexFromIndex(CassetteIndex);
		if (cassetConnectionIndexFromIndex != null)
		{
			rGD_RadioPlayer.timelinePosition = cassetConnectionIndexFromIndex.GetTimelinePosition();
		}
		return rGD_RadioPlayer;
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.Radio_Toggle:
		{
			Message_RadioPlayer_Toggle message_RadioPlayer_Toggle = msg as Message_RadioPlayer_Toggle;
			CassetteConnection cassetConnectionIndexFromIndex = GetCassetConnectionIndexFromIndex(CassetteIndex);
			if (!Raft_Network.IsHost && cassetConnectionIndexFromIndex != null)
			{
				cassetConnectionIndexFromIndex.cassetteEmitter.EventInstance.setTimelinePosition(message_RadioPlayer_Toggle.timelinePosition);
				cassetConnectionIndexFromIndex.timelinePosition = message_RadioPlayer_Toggle.timelinePosition;
			}
			ToggleRadio();
			if (Raft_Network.IsHost && cassetConnectionIndexFromIndex != null)
			{
				message_RadioPlayer_Toggle.timelinePosition = cassetConnectionIndexFromIndex.GetTimelinePosition();
			}
			return true;
		}
		case Messages.InteractableButtonPress_Int:
		{
			Message_InteractableButtonPress_Integer message_InteractableButtonPress_Integer = msg as Message_InteractableButtonPress_Integer;
			Network_Player playerFromID = localPlayer.Network.GetPlayerFromID(message_InteractableButtonPress_Integer.SteamID);
			cassetteButton.PlayButtonFeedback(message_InteractableButtonPress_Integer.success);
			if (playerFromID != null)
			{
				if (HasCassette)
				{
					return TakeCassette(playerFromID);
				}
				return InsertCassette(playerFromID, message_InteractableButtonPress_Integer.integer);
			}
			return false;
		}
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
