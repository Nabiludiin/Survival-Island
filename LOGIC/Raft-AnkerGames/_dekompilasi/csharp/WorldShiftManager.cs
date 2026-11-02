using System;
using Steamworks;
using UnityEngine;

public class WorldShiftManager : MonoBehaviour, SceneEventInterface
{
	public static Action<Vector3> OnWorldShift;

	private Transform _raftTransform;

	[SerializeField]
	private Color gizmoColor;

	public int shiftCounter;

	public int recievedShiftCount;

	private Raft_Network network;

	public Transform RaftTransform
	{
		get
		{
			if (_raftTransform == null)
			{
				_raftTransform = SingletonGeneric<GameManager>.Singleton.raftTransform;
			}
			return _raftTransform;
		}
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			OnWorldShift = null;
		}
	}

	private void Update()
	{
		HandleWorldShift();
	}

	private void HandleWorldShift()
	{
		if (Raft_Network.IsHost)
		{
			Vector3 position = RaftTransform.position;
			position.y = 0f;
			float x = RaftTransform.position.x;
			float z = RaftTransform.position.z;
			if (Mathf.Abs(x) >= (float)ChunkManager.ChunkSize)
			{
				Vector3 vector = new Vector3(RaftTransform.position.x, 0f, 0f);
				network.RPC(new Message_WorldShiftManager_WorldShift(Messages.WorldManager_ResetWorldToCenter, network.hostNetwork, vector, shiftCounter + 1), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				ResetToCenter(vector);
			}
			else if (Mathf.Abs(z) >= (float)ChunkManager.ChunkSize)
			{
				Vector3 vector2 = new Vector3(0f, 0f, RaftTransform.position.z);
				network.RPC(new Message_WorldShiftManager_WorldShift(Messages.WorldManager_ResetWorldToCenter, network.hostNetwork, vector2, shiftCounter + 1), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				ResetToCenter(vector2);
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = gizmoColor;
		Gizmos.DrawWireCube(Vector3.zero, new Vector3(ChunkManager.ChunkSize * 2, 0.25f, ChunkManager.ChunkSize * 2));
	}

	public void ResetToCenter(float xOffset, float zOffset)
	{
		ResetToCenter(new Vector3(xOffset, 0f, zOffset));
	}

	public void ResetToCenter(Vector3 shift)
	{
		shift.y = 0f;
		if (Raft_Network.IsHost)
		{
			shiftCounter++;
		}
		if (OnWorldShift != null)
		{
			OnWorldShift(shift);
		}
	}

	public void ResetToCenter()
	{
		network.RPC(new Message_WorldShiftManager_WorldShift(Messages.WorldManager_ResetWorldToCenter, network.hostNetwork, RaftTransform.position, shiftCounter + 1), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		ResetToCenter(RaftTransform.position);
	}
}
