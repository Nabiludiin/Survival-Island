using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class NoteBook : MonoBehaviour_ID_Network
{
	public static List<int> unlockedNoteBookIndexes = new List<int>();

	public static Action<uint> OnIndexUnlock;

	public static List<ChunkPointType> unlockedChunkPointType = new List<ChunkPointType>();

	public Dialogue dialogue;

	private Vector2Int latestNoteIndex = new Vector2Int(0, 0);

	private Raft_Network network;

	private NotificationManager notificationManager;

	private Network_Player localPlayer;

	[SerializeField]
	private InputActionReference inputCancel;

	private void Awake()
	{
		ComponentManager<NoteBook>.Value = this;
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		notificationManager = ComponentManager<NotificationManager>.Value;
		localPlayer = ComponentManager<Network_Player>.Value;
		UnlockFrequency(ChunkPointType.Landmark_Big);
		if (Raft_Network.IsHost)
		{
			ObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
		}
		NetworkIDManager.AddNetworkID(this);
		Dialogue.OnVoiceDataTracked = (Action<SO_VoiceData.Instance_VoiceData>)Delegate.Combine(Dialogue.OnVoiceDataTracked, new Action<SO_VoiceData.Instance_VoiceData>(OnVoiceDataTracked));
	}

	private void Update()
	{
		if (dialogue != null && dialogue.IsVoiceActive && dialogue.currentDialogueInstance.voiceData.canBeCanceled && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputCancel, "NoteBookStop"))
		{
			StopDialogue();
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (unlockedNoteBookIndexes != null)
		{
			unlockedNoteBookIndexes.Clear();
		}
		if (unlockedChunkPointType != null)
		{
			unlockedChunkPointType.Clear();
		}
		OnIndexUnlock = null;
		Dialogue.OnVoiceDataTracked = (Action<SO_VoiceData.Instance_VoiceData>)Delegate.Remove(Dialogue.OnVoiceDataTracked, new Action<SO_VoiceData.Instance_VoiceData>(OnVoiceDataTracked));
		StopDialogue();
		NetworkIDManager.RemoveNetworkID(this);
	}

	private void OnVoiceDataTracked(SO_VoiceData.Instance_VoiceData instance)
	{
		if (dialogue.IsVoiceActive && Dialogue.ActiveTrackedDialogues.Count > 1)
		{
			dialogue.CancelVoice();
		}
	}

	public void PlayDialogue(SO_VoiceActor actor, SO_VoiceData voiceData)
	{
		StopDialogue();
		if (!(dialogue == null))
		{
			dialogue.voiceActor = actor;
			dialogue.PlayVoice(voiceData, null, voiceData.GetVoiceLineFromPlayMode());
		}
	}

	public void StopDialogue()
	{
		if (!(dialogue == null))
		{
			dialogue.CancelVoice();
		}
	}

	public bool IsPlayingVoiceData(SO_VoiceData voiceData)
	{
		if (dialogue != null && dialogue.currentDialogueInstance != null)
		{
			return dialogue.currentDialogueInstance.voiceData == voiceData;
		}
		return false;
	}

	public void UnlockAllNotes()
	{
		if (!(network.GetLocalPlayer().NoteBookUI != null))
		{
			return;
		}
		NoteBookNote[] allNotes = network.GetLocalPlayer().NoteBookUI.GetAllNotes();
		foreach (NoteBookNote noteBookNote in allNotes)
		{
			if (!noteBookNote.isUnlocked)
			{
				UnlockSpecificNoteNetworked(noteBookNote.noteIndex, showNotification: false);
			}
		}
	}

	public void UnlockSpecificNoteNetworked(int noteIndex, bool showNotification)
	{
		Message_NoteBook_SetNoteState message = new Message_NoteBook_SetNoteState(Messages.NoteBook_SetNoteUnlockState, network.NetworkIDManager, ObjectIndex, noteIndex, p_isUnlocked: true, showNotification);
		if (Raft_Network.IsHost)
		{
			if (network.NetworkIDManager != null)
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			UnlockSpecificNoteWithUniqueNoteIndex(noteIndex, toUnlock: true, showNotification);
		}
		else if (network.NetworkIDManager != null)
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public bool UnlockSpecificNoteWithUniqueNoteIndex(int p_noteIndex, bool toUnlock, bool showNotification)
	{
		bool flag = false;
		if (toUnlock)
		{
			if (unlockedNoteBookIndexes.Contains(p_noteIndex))
			{
				return false;
			}
			unlockedNoteBookIndexes.Add(p_noteIndex);
			flag = true;
		}
		else
		{
			unlockedNoteBookIndexes.Remove(p_noteIndex);
			flag = true;
		}
		if (OnIndexUnlock != null)
		{
			OnIndexUnlock((uint)p_noteIndex);
		}
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		NoteBookNote noteBookNote = null;
		if (localPlayer != null)
		{
			noteBookNote = localPlayer.NoteBookUI.GetNoteFromIndex(p_noteIndex);
			if ((noteBookNote != null && noteBookNote.isFrequencyThumbnail) & toUnlock)
			{
				UnlockFrequency(noteBookNote.thumbNailLandmarkType);
			}
		}
		if (showNotification)
		{
			notificationManager.ShowNotification("NoteBookNote");
			if (GeneralSettingsBox.AutoPlayNotes && localPlayer != null && noteBookNote != null && noteBookNote.IsUsingPlayVoiceSystem)
			{
				noteBookNote.Button_PlayAndStop();
			}
		}
		return flag;
	}

	public static void UnlockFrequency(ChunkPointType chunkPointType)
	{
		unlockedChunkPointType.AddUniqueOnly(chunkPointType);
	}

	public static bool HasUnlockedFirstFrequency()
	{
		return unlockedNoteBookIndexes.Contains(901);
	}

	public static bool HasUnlockedFrequencyNoteAtDestination(ChunkPointType destination)
	{
		return destination switch
		{
			ChunkPointType.Landmark_RadioTower => unlockedNoteBookIndexes.Contains(2), 
			ChunkPointType.Landmark_Vasagatan => unlockedNoteBookIndexes.Contains(17), 
			ChunkPointType.Landmark_Balboa => unlockedNoteBookIndexes.Contains(902), 
			ChunkPointType.Landmark_CaravanIsland => unlockedNoteBookIndexes.Contains(43), 
			ChunkPointType.Landmark_Tangaroa => unlockedNoteBookIndexes.Contains(53), 
			ChunkPointType.Landmark_VarunaPoint => unlockedNoteBookIndexes.Contains(60), 
			ChunkPointType.Landmark_Temperance => unlockedNoteBookIndexes.Contains(68), 
			_ => false, 
		};
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.NoteBook_Create:
		{
			Message_NoteBook_Create message_NoteBook_Create = msg as Message_NoteBook_Create;
			ObjectIndex = message_NoteBook_Create.NetworkID_ObjectIndex;
			message_NoteBook_Create.RestoreNoteBook();
			return false;
		}
		case Messages.NoteBook_SetNoteUnlockState:
		{
			Message_NoteBook_SetNoteState message_NoteBook_SetNoteState = msg as Message_NoteBook_SetNoteState;
			return UnlockSpecificNoteWithUniqueNoteIndex(message_NoteBook_SetNoteState.noteIndex, message_NoteBook_SetNoteState.isUnlocked, message_NoteBook_SetNoteState.showNotification);
		}
		case Messages.NoteBook_FlipPage:
		{
			Message_NoteBook_ChangePage message_NoteBook_ChangePage = msg as Message_NoteBook_ChangePage;
			NoteBookUI noteBookUI = network.GetPlayerFromID(message_NoteBook_ChangePage.SteamID).NoteBookUI;
			if (noteBookUI != null)
			{
				noteBookUI.FlipToPageLocally((uint)message_NoteBook_ChangePage.pageIndex);
			}
			return true;
		}
		case Messages.NoteBook_SetBookVisibleState:
		{
			Message_NoteBook_SetBookVisibleState message_NoteBook_SetBookVisibleState = msg as Message_NoteBook_SetBookVisibleState;
			NoteBookUI noteBookUI2 = network.GetPlayerFromID(message_NoteBook_SetBookVisibleState.SteamID).NoteBookUI;
			if (noteBookUI2 != null)
			{
				noteBookUI2.SetBookActive(message_NoteBook_SetBookVisibleState.setEnabled);
			}
			return true;
		}
		case Messages.Dialogue:
		{
			Message_Dialogue message_Dialogue = msg as Message_Dialogue;
			if (msg != null)
			{
				dialogue.PlayVoice(message_Dialogue, network.GetPlayerFromID(message_Dialogue.SteamID));
				return true;
			}
			break;
		}
		}
		return base.Deserialize(msg, remoteID);
	}
}
