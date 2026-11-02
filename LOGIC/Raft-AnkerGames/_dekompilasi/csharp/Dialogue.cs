using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour, IRaycastable
{
	[SerializeField]
	private MonoBehaviour_ID_Network networkBehaviourID;

	public static Action<SO_VoiceData.Instance_VoiceData> OnVoiceDataTracked;

	public static Action<SO_VoiceData.Instance_VoiceData> OnVoiceDataUnTracked;

	public Action<SO_VoiceData.Instance_VoiceData> OnVoiceDataPlayed;

	public Action<SO_VoiceLine> OnVoiceLinePlayed;

	public Action<SO_VoiceData.Instance_VoiceData> OnVoiceDataCancelled;

	public static List<SO_VoiceData.Instance_VoiceData> ActiveTrackedDialogues = new List<SO_VoiceData.Instance_VoiceData>();

	public SO_VoiceActor voiceActor;

	public List<SO_VoiceData> voiceDatas;

	public SO_VoiceData.Instance_VoiceData currentDialogueInstance;

	[SerializeField]
	private bool playRandomVoiceData;

	[SerializeField]
	private bool lookAtPlayerWhenTalking;

	[SerializeField]
	private Transform rotateParent;

	[SerializeField]
	private float rotateSpeed = 1f;

	[SerializeField]
	private float subtitleDistance = 50f;

	[HideInInspector]
	public bool allowTalkingAnimator = true;

	public AnimatorController animatorController;

	protected static Network_Player localPlayer;

	private List<SO_VoiceData.Instance_VoiceData> voiceDataInstances = new List<SO_VoiceData.Instance_VoiceData>();

	private DisplayTextManager displayTextManager;

	private int nextPlayableDataIndex;

	public MonoBehaviour_ID_Network NetworkMessageReciever => networkBehaviourID;

	public bool IsVoiceActive => currentDialogueInstance != null;

	public float SubtitleDistance => subtitleDistance;

	public static bool IsDialogueActive(SO_VoiceData voiceData)
	{
		foreach (SO_VoiceData.Instance_VoiceData activeTrackedDialogue in ActiveTrackedDialogues)
		{
			if (activeTrackedDialogue.voiceData == voiceData)
			{
				return true;
			}
		}
		return false;
	}

	public static SO_VoiceData.Instance_VoiceData GetLocalPlayersPrioritizedDialogue()
	{
		SO_VoiceData.Instance_VoiceData instance_VoiceData = null;
		float num = float.MaxValue;
		foreach (SO_VoiceData.Instance_VoiceData activeTrackedDialogue in ActiveTrackedDialogues)
		{
			if (activeTrackedDialogue == null || activeTrackedDialogue.AttachedDialogue == null || !activeTrackedDialogue.isActive)
			{
				continue;
			}
			float num2 = 0f;
			if (!activeTrackedDialogue.voiceData.twoDimensional)
			{
				num2 = Vector3.Distance(activeTrackedDialogue.AttachedDialogue.transform.position, localPlayer.transform.position);
				if (num2 > activeTrackedDialogue.AttachedDialogue.SubtitleDistance || (num2 > num && instance_VoiceData != null && instance_VoiceData.voiceData.priority >= activeTrackedDialogue.voiceData.priority))
				{
					continue;
				}
				num = num2;
			}
			if (instance_VoiceData == null || (instance_VoiceData.voiceData.priority <= activeTrackedDialogue.voiceData.priority && (!(instance_VoiceData.startedPlayer == localPlayer) || !(activeTrackedDialogue.startedPlayer != localPlayer))))
			{
				instance_VoiceData = activeTrackedDialogue;
			}
		}
		return instance_VoiceData;
	}

	public static bool IsPlaying3DDialogue()
	{
		foreach (SO_VoiceData.Instance_VoiceData activeTrackedDialogue in ActiveTrackedDialogues)
		{
			if (activeTrackedDialogue != null && !activeTrackedDialogue.voiceData.twoDimensional)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsPlaying2DDialogue()
	{
		foreach (SO_VoiceData.Instance_VoiceData activeTrackedDialogue in ActiveTrackedDialogues)
		{
			if (activeTrackedDialogue != null && activeTrackedDialogue.voiceData.twoDimensional)
			{
				return true;
			}
		}
		return false;
	}

	public static void TrackDialogue(SO_VoiceData.Instance_VoiceData instance)
	{
		ActiveTrackedDialogues.Add(instance);
		OnVoiceDataTracked?.Invoke(instance);
	}

	public static void UnTrackDialogue(SO_VoiceData.Instance_VoiceData instance)
	{
		ActiveTrackedDialogues.Remove(instance);
		OnVoiceDataUnTracked?.Invoke(instance);
	}

	private void Odin_PlayVoiceData()
	{
		PlayVoice(voiceDatas[0], null);
	}

	public void OnIsRayed()
	{
		if (currentDialogueInstance != null && currentDialogueInstance.isActive)
		{
			if (displayTextManager != null)
			{
				displayTextManager.HideDisplayTexts();
			}
			return;
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		if (displayTextManager != null)
		{
			LocalizationParameters.itemX = ((voiceActor != null) ? voiceActor.displayName : string.Empty);
			displayTextManager.ShowText(Helper.GetTerm("Game/TalkToX", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			if (displayTextManager != null)
			{
				displayTextManager.HideDisplayTexts();
			}
			if (voiceDatas != null)
			{
				PlayNextVoice_Networked(localPlayer);
			}
			else
			{
				Debug.LogError("No voicedata is assigned", gameObject);
			}
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (displayTextManager != null)
		{
			displayTextManager.HideDisplayTexts();
		}
	}

	public void OnDestroy()
	{
		if (currentDialogueInstance != null && !GameManager.IsQuittingGame)
		{
			currentDialogueInstance.Cancel();
		}
	}

	public virtual void Awake()
	{
	}

	public virtual void Start()
	{
		displayTextManager = ComponentManager<DisplayTextManager>.Value;
		localPlayer = ComponentManager<Network_Player>.Value;
	}

	public virtual void Update()
	{
		HandleTalkingAnimator();
		if (!lookAtPlayerWhenTalking)
		{
			return;
		}
		if (IsVoiceActive)
		{
			Network_Player startedPlayer = currentDialogueInstance.startedPlayer;
			if (startedPlayer != null)
			{
				Vector3 forward = startedPlayer.transform.position - transform.position;
				forward.y = 0f;
				rotateParent.rotation = Quaternion.Lerp(rotateParent.rotation, Quaternion.LookRotation(forward, Vector3.up), Time.deltaTime * rotateSpeed);
			}
		}
		else
		{
			Vector3 forward2 = ((rotateParent.parent != null) ? rotateParent.parent.forward : rotateParent.forward);
			rotateParent.rotation = Quaternion.Lerp(rotateParent.rotation, Quaternion.LookRotation(forward2, Vector3.up), Time.deltaTime * rotateSpeed);
		}
	}

	public void PlayNextVoice_Networked(Network_Player player)
	{
		int index = (playRandomVoiceData ? UnityEngine.Random.Range(0, voiceDatas.Count) : nextPlayableDataIndex);
		PlayVoice_Networked(voiceDatas[index], player);
	}

	public void PlayVoice_Networked(SO_VoiceData vD, Network_Player player, int voiceLineIndex = -1)
	{
		if (networkBehaviourID != null)
		{
			if (voiceLineIndex == -1)
			{
				voiceLineIndex = vD.GetVoiceLineFromPlayMode();
			}
			Message_Dialogue message = new Message_Dialogue(Messages.Dialogue, localPlayer.Network.NetworkIDManager, localPlayer.steamID, networkBehaviourID.ObjectIndex, Raft_Network.IsHost ? GetIndexFromData(vD) : (-1), voiceLineIndex);
			if (Raft_Network.IsHost)
			{
				PlayVoice(vD, player, voiceLineIndex);
				localPlayer.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				localPlayer.Network.SendP2P(localPlayer.Network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public void PlayVoice(Message_Dialogue msg, Network_Player player)
	{
		if (Raft_Network.IsHost && msg.voiceDataIndex == -1)
		{
			msg.voiceDataIndex = (playRandomVoiceData ? UnityEngine.Random.Range(0, voiceDatas.Count) : nextPlayableDataIndex);
		}
		SO_VoiceData dataFromIndex = GetDataFromIndex(msg.voiceDataIndex);
		if (dataFromIndex != null)
		{
			PlayVoice(dataFromIndex, player, msg.voiceLineIndex);
		}
	}

	public void PlayVoice(SO_VoiceData vD, Network_Player player, int voiceLineIndex = -1)
	{
		SO_VoiceData.Instance_VoiceData instance_VoiceData = GetInstanceFromVoiceData(vD);
		if (instance_VoiceData == null)
		{
			instance_VoiceData = CreateInstanceFromVoiceData(vD);
		}
		currentDialogueInstance = instance_VoiceData;
		currentDialogueInstance.startedPlayer = player;
		currentDialogueInstance.Play(this, voiceLineIndex);
		nextPlayableDataIndex = GetIndexFromData(vD) + 1;
		if (nextPlayableDataIndex >= voiceDatas.Count)
		{
			nextPlayableDataIndex = 0;
		}
	}

	public void CancelVoice()
	{
		if (currentDialogueInstance != null)
		{
			currentDialogueInstance.Cancel();
			currentDialogueInstance = null;
		}
	}

	private void HandleTalkingAnimator()
	{
		if (animatorController != null)
		{
			animatorController.SetBool("Talking", allowTalkingAnimator && IsVoiceActive);
		}
	}

	private SO_VoiceData.Instance_VoiceData CreateInstanceFromVoiceData(SO_VoiceData vD)
	{
		SO_VoiceData.Instance_VoiceData instance_VoiceData = new SO_VoiceData.Instance_VoiceData(vD, null);
		instance_VoiceData.OnStarted = (Action<SO_VoiceData.Instance_VoiceData>)Delegate.Combine(instance_VoiceData.OnStarted, (Action<SO_VoiceData.Instance_VoiceData>)((SO_VoiceData.Instance_VoiceData instance) =>
		{
			OnVoiceDataPlayed?.Invoke(instance);
		}));
		instance_VoiceData.OnCanceled = (Action<SO_VoiceData.Instance_VoiceData>)Delegate.Combine(instance_VoiceData.OnCanceled, (Action<SO_VoiceData.Instance_VoiceData>)((SO_VoiceData.Instance_VoiceData instance) =>
		{
			if (instance == currentDialogueInstance)
			{
				currentDialogueInstance = null;
			}
			OnVoiceDataCancelled?.Invoke(instance);
		}));
		instance_VoiceData.OnVoiceLinePlayed = (Action<SO_VoiceLine>)Delegate.Combine(instance_VoiceData.OnVoiceLinePlayed, (Action<SO_VoiceLine>)((SO_VoiceLine voiceLine) =>
		{
			OnVoiceLinePlayed?.Invoke(voiceLine);
		}));
		voiceDataInstances.Add(instance_VoiceData);
		return instance_VoiceData;
	}

	private SO_VoiceData.Instance_VoiceData GetInstanceFromVoiceData(SO_VoiceData vD)
	{
		foreach (SO_VoiceData.Instance_VoiceData voiceDataInstance in voiceDataInstances)
		{
			if (voiceDataInstance != null && voiceDataInstance.voiceData == vD)
			{
				return voiceDataInstance;
			}
		}
		return null;
	}

	private SO_VoiceData GetDataFromIndex(int index)
	{
		if (index < 0 || index >= voiceDatas.Count)
		{
			return null;
		}
		return voiceDatas[index];
	}

	private int GetIndexFromData(SO_VoiceData data)
	{
		for (int i = 0; i < voiceDatas.Count; i++)
		{
			SO_VoiceData sO_VoiceData = voiceDatas[i];
			if (!(sO_VoiceData == null) && sO_VoiceData == data)
			{
				return i;
			}
		}
		return 0;
	}
}
