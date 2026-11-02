using Steamworks;
using UnityEngine;

public class RaftLandmark : MonoBehaviour_NetworkTick
{
	[SerializeField]
	private WaterFloatSemih2 waterFloatScript;

	public Sink sinkScript;

	[SerializeField]
	private float positionLerpSpeed = 5f;

	[SerializeField]
	private float rotationLerpSpeed = 10f;

	private Raft_Network network;

	private Vector3 networkPosition;

	private Quaternion networkRotation;

	private bool triggedSinking;

	public void OnSpawn()
	{
		if (network == null)
		{
			network = Object.FindObjectOfType<Raft_Network>();
		}
		if (!Raft_Network.IsHost)
		{
			networkPosition = transform.position;
			networkRotation = transform.rotation;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (triggedSinking || !(other.transform.tag == "Player"))
		{
			return;
		}
		triggedSinking = true;
		if (!(network == null))
		{
			Message_NetworkBehaviour message = new Message_NetworkBehaviour(Messages.RaftLandmark_StartSinking, this);
			if (Raft_Network.IsHost)
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				StartSinking();
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	private void Update()
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		if (!Raft_Network.IsHost)
		{
			UpdateRemoteClient();
		}
		else
		{
			UpdateLocalClient();
		}
	}

	private void UpdateRemoteClient()
	{
		if (sinkScript.Sinking && transform != null)
		{
			transform.position = Vector3.Lerp(transform.position, networkPosition, positionLerpSpeed * Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, rotationLerpSpeed * Time.deltaTime);
		}
	}

	private void UpdateLocalClient()
	{
		if (sinkScript.Sinking)
		{
			UpdateNetworkTick();
		}
	}

	public void OnLandmarkReset()
	{
		waterFloatScript.setToSurface = true;
		sinkScript.Sinking = false;
		triggedSinking = false;
	}

	public void StartSinking(bool playParticles = true)
	{
		triggedSinking = true;
		sinkScript.Sinking = true;
		waterFloatScript.setToSurface = false;
		if (!Raft_Network.IsHost)
		{
			networkPosition = transform.position;
			networkRotation = transform.rotation;
		}
	}

	public override void Serialize_Update()
	{
		AddMessage(new Message_RaftLandmark_Update(Messages.RaftLandmark_Update, this, this));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.RaftLandmark_StartSinking:
			StartSinking();
			return true;
		case Messages.RaftLandmark_Update:
		{
			Message_RaftLandmark_Update message_RaftLandmark_Update = msg as Message_RaftLandmark_Update;
			networkPosition = message_RaftLandmark_Update.Position.Value;
			networkRotation = Quaternion.Euler(message_RaftLandmark_Update.Euler.Value);
			return true;
		}
		default:
			return false;
		}
	}
}
