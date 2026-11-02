using System;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using UnityEngine;

public class OlofFinalRoom : MonoBehaviour_ID_Network
{
	[SerializeField]
	private Dialogue dialogue_olofFinal;

	[SerializeField]
	private Dialogue dialogue_olofCaught;

	[SerializeField]
	private Transform olofParent;

	[SerializeField]
	private Renderer[] olofHangingRenderers;

	[SerializeField]
	private Transform olofPointA;

	[SerializeField]
	private Transform olofPointB;

	[SerializeField]
	private Transform playerStartPoint;

	[SerializeField]
	private AreaZone_PlayerCounter playerCounterZone;

	[SerializeField]
	private float maxPlayerDistance;

	[SerializeField]
	private float movementSpeed;

	[SerializeField]
	private TimerEventer caughtVoiceLineTimer;

	[SerializeField]
	private Animator olofAnimator;

	[SerializeField]
	private Renderer rescueKeyRenderer;

	[SerializeField]
	private Collider rescueKeyCollider;

	[SerializeField]
	private float fallDelay;

	[SerializeField]
	private HeadTrackingIK headIK;

	[SerializeField]
	private HeadTrackingIK handingOlofHeadIK;

	public EventReference er_fall;

	public EventReference er_scream;

	[SerializeField]
	private string screamKey;

	private Raft_Network network;

	private float distanceInterval;

	private int furthestDistanceIndex;

	private float furthestNormalizedDistance;

	private int numberOfVoiceLines;

	private bool hasBeenCompleted;

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		numberOfVoiceLines = dialogue_olofFinal.voiceDatas[0].voiceLines.Count;
		distanceInterval = 1f / (float)numberOfVoiceLines;
		furthestDistanceIndex = -1;
	}

	private void OnEnable()
	{
		rescueKeyRenderer.enabled = false;
		rescueKeyCollider.enabled = false;
		Quest questByType = QuestProgressTracker.GetQuestByType(QuestType.Utopia_People_Rescued);
		questByType.OnQuestComplete = (Action)Delegate.Combine(questByType.OnQuestComplete, new Action(DisableSelf));
		if (QuestProgressTracker.GetQuestByType(QuestType.Utopia_OlofFall).isQuestComplete || questByType.isQuestComplete)
		{
			OnFinalRoomCompleteDelayed();
			if (questByType.isQuestComplete)
			{
				DisableSelf();
			}
		}
	}

	private void Update()
	{
		if (hasBeenCompleted)
		{
			if (!Raft_Network.IsHost)
			{
				return;
			}
			if (caughtVoiceLineTimer.DoUpdate(Time.deltaTime, autoResetTimer: true))
			{
				List<Network_Player> list = Helper.PlayersWithinRange(olofParent.position, 15f, includeDeadPlayers: true);
				if (list != null && list.ContainsItems())
				{
					dialogue_olofCaught.PlayNextVoice_Networked(null);
				}
			}
			olofAnimator.SetInteger("Speed", 0);
		}
		else
		{
			if (!playerCounterZone.PlayersIsInArea)
			{
				return;
			}
			Network_Player playerInFrontOfRoom = GetPlayerInFrontOfRoom();
			if (!(playerInFrontOfRoom != null))
			{
				return;
			}
			if (headIK.target == null)
			{
				headIK.target = playerInFrontOfRoom.CameraTransform;
				headIK.HeadTrackingOn();
			}
			float normalizedPlayerDistanceToEndOfRoom = GetNormalizedPlayerDistanceToEndOfRoom(playerInFrontOfRoom);
			if (normalizedPlayerDistanceToEndOfRoom > furthestNormalizedDistance)
			{
				furthestNormalizedDistance = normalizedPlayerDistanceToEndOfRoom;
			}
			int num = Mathf.FloorToInt(furthestNormalizedDistance / distanceInterval);
			if (num > furthestDistanceIndex && num < numberOfVoiceLines)
			{
				furthestDistanceIndex = num;
				if (Raft_Network.IsHost && !dialogue_olofFinal.IsVoiceActive)
				{
					dialogue_olofFinal.PlayVoice_Networked(dialogue_olofFinal.voiceDatas[0], null, furthestDistanceIndex);
				}
			}
			Vector3 vector = olofPointB.position - olofPointA.position;
			Vector3 vector2 = olofPointA.position + vector * furthestNormalizedDistance;
			bool flag = Vector3.Distance(vector2, olofParent.transform.position) < 0.01f;
			olofParent.position = Vector3.MoveTowards(olofParent.position, vector2, Time.deltaTime * movementSpeed);
			olofAnimator.SetBool("Retreating", value: true);
			if (!flag)
			{
				olofAnimator.SetInteger("Speed", 2);
			}
			else
			{
				olofAnimator.SetInteger("Speed", 0);
			}
			if ((Raft_Network.IsHost && furthestNormalizedDistance >= 1f) & flag)
			{
				OnFinalRoomComplete();
				Message_NetworkBehaviour_ID message = new Message_NetworkBehaviour_ID(Messages.FinalRoomOlofFall, network.NetworkIDManager, ObjectIndex);
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (playerStartPoint != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(playerStartPoint.position, 0.2f);
			Gizmos.DrawSphere(playerStartPoint.position + playerStartPoint.forward * maxPlayerDistance, 0.2f);
			Gizmos.DrawLine(playerStartPoint.position, playerStartPoint.position + playerStartPoint.forward * maxPlayerDistance);
		}
		if (olofPointA != null && olofPointB != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(olofPointA.position, 0.2f);
			Gizmos.DrawSphere(olofPointB.position, 0.2f);
			Gizmos.DrawLine(olofPointA.position, olofPointB.position);
		}
	}

	private void OnFinalRoomCompleteDelayed()
	{
		hasBeenCompleted = true;
		olofParent.gameObject.SetActiveSafe(value: false);
		Renderer[] array = olofHangingRenderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}
		rescueKeyRenderer.enabled = true;
		rescueKeyCollider.enabled = true;
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
			if (network == null)
			{
				return;
			}
		}
		Network_Player localPlayer = network.GetLocalPlayer();
		if (localPlayer != null)
		{
			handingOlofHeadIK.target = localPlayer.CameraTransform;
			handingOlofHeadIK.HeadTrackingOn();
		}
	}

	private void OnFinalRoomComplete()
	{
		hasBeenCompleted = true;
		olofAnimator.SetBool("Falling", value: true);
		olofAnimator.SetBool("Retreating", value: false);
		olofAnimator.SetFloat("Speed", 0f);
		RuntimeManager.PlayOneShotAttached(er_fall, olofAnimator.gameObject);
		SoundManager.PlayProgrammerSound(er_scream, screamKey, olofAnimator.transform);
		QuestProgressTracker.FinishQuestRPC(QuestType.Utopia_OlofFall);
		Invoke("OnFinalRoomCompleteDelayed", fallDelay);
	}

	private Network_Player GetPlayerInFrontOfRoom()
	{
		float num = 0f;
		Network_Player network_Player = null;
		foreach (Network_Player value in network.remoteUsers.Values)
		{
			if (!(value == null))
			{
				Vector3 vector = playerStartPoint.InverseTransformPoint(value.FeetPosition);
				if (!(vector.z < 0f) && (network_Player == null || vector.z > num))
				{
					network_Player = value;
					num = vector.z;
				}
			}
		}
		return network_Player;
	}

	private float GetNormalizedPlayerDistanceToEndOfRoom(Network_Player player)
	{
		Vector3 vector = playerStartPoint.InverseTransformPoint(player.FeetPosition);
		if (vector.z < 0f)
		{
			return 0f;
		}
		return Mathf.Clamp01(vector.z / maxPlayerDistance);
	}

	private void DisableSelf()
	{
		olofParent.gameObject.SetActiveSafe(value: false);
		rescueKeyRenderer.enabled = false;
		rescueKeyCollider.enabled = false;
		Renderer[] array = olofHangingRenderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		enabled = false;
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.FinalRoomOlofFall:
			if (msg as Message_NetworkBehaviour_ID != null)
			{
				OnFinalRoomComplete();
			}
			return true;
		case Messages.Dialogue:
		{
			Message_Dialogue message_Dialogue = msg as Message_Dialogue;
			(hasBeenCompleted ? dialogue_olofCaught : dialogue_olofFinal).PlayVoice(message_Dialogue, network.GetPlayerFromID(message_Dialogue.SteamID));
			return true;
		}
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
