using System;
using System.Collections;
using FMODUnity;
using Steamworks;
using UnityEngine;

public class SinkingObstacle : MonoBehaviour_ID_Network
{
	[SerializeField]
	private Transform sinkTransform;

	[SerializeField]
	private float sinkDepth = 1f;

	[SerializeField]
	private float animationTime = 1f;

	[SerializeField]
	private bool sinking;

	[SerializeField]
	private TriggerBoxEnterAction triggerBoxEnter;

	[SerializeField]
	private ParticleController particleController;

	[SerializeField]
	private StudioEventEmitter sinkingEmitter;

	private Raft_Network network;

	private Vector3 startPosition;

	private void OnDisable()
	{
		ResetSink();
	}

	private void Awake()
	{
		startPosition = sinkTransform.localPosition;
		if (Raft_Network.IsHost)
		{
			TriggerBoxEnterAction triggerBoxEnterAction = triggerBoxEnter;
			triggerBoxEnterAction.OnTriggerAction = (Action<Collider>)Delegate.Combine(triggerBoxEnterAction.OnTriggerAction, new Action<Collider>(OnTriggerBoxEnter));
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Raft_Network.IsHost)
		{
			TriggerBoxEnterAction triggerBoxEnterAction = triggerBoxEnter;
			triggerBoxEnterAction.OnTriggerAction = (Action<Collider>)Delegate.Remove(triggerBoxEnterAction.OnTriggerAction, new Action<Collider>(OnTriggerBoxEnter));
		}
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
	}

	private void OnTriggerBoxEnter(Collider other)
	{
		RPC_StartSinking();
	}

	private void RPC_StartSinking()
	{
		if (Raft_Network.IsHost && StartSinking())
		{
			Message_NetworkBehaviour_ID message = new Message_NetworkBehaviour_ID(Messages.StartSinkingJumpObstacle, network.NetworkIDManager, ObjectIndex);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private bool StartSinking()
	{
		if (Raft_Network.IsHost && sinking)
		{
			return false;
		}
		ResetSink();
		StartCoroutine(Sink());
		if (particleController != null)
		{
			particleController.PlayParticles();
		}
		return true;
	}

	private void ResetSink()
	{
		StopAllCoroutines();
		sinkTransform.localPosition = startPosition;
		sinking = false;
	}

	private IEnumerator Sink()
	{
		sinking = true;
		float startTime = Time.time;
		float halfAnimationTime = animationTime / 2f;
		bool rising = false;
		Vector3 endPosition = sinkTransform.localPosition - sinkTransform.up * sinkDepth;
		sinkingEmitter.PlaySafe();
		while (true)
		{
			float num = (Time.time - startTime) / halfAnimationTime;
			sinkingEmitter.SetParameter("depth", num);
			Vector3 a = (rising ? endPosition : startPosition);
			Vector3 b = (rising ? startPosition : endPosition);
			sinkTransform.localPosition = Vector3.Lerp(a, b, num);
			if (num >= 1f)
			{
				if (rising)
				{
					break;
				}
				rising = true;
				startTime = Time.time;
			}
			yield return null;
		}
		sinkTransform.localPosition = startPosition;
		sinking = false;
		if (particleController != null)
		{
			particleController.StopParticles();
		}
		sinkingEmitter.StopSafe();
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.StartSinkingJumpObstacle)
		{
			return StartSinking();
		}
		return base.Deserialize(msg, remoteID);
	}
}
