using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using Unity.Netcode;
using UnityEngine;

public class ExplosiveThrower : MonoBehaviour_ID_Network
{
	[Serializable]
	public class Message_ThrowExplosive : Message_NetworkBehaviour_ID
	{
		public int pointIndex;

		protected bool Equals(Message_ThrowExplosive other)
		{
			if (Equals((Message_NetworkBehaviour_ID)other))
			{
				return pointIndex == other.pointIndex;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((Message_ThrowExplosive)obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(base.GetHashCode(), pointIndex);
		}

		public static bool operator ==(Message_ThrowExplosive left, Message_ThrowExplosive right)
		{
			return object.Equals(left, right);
		}

		public static bool operator !=(Message_ThrowExplosive left, Message_ThrowExplosive right)
		{
			return !object.Equals(left, right);
		}

		public Message_ThrowExplosive()
		{
		}

		public Message_ThrowExplosive(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, int pointIndex)
			: base(type, behaviour, networkID_ObjectIndex)
		{
			this.pointIndex = pointIndex;
		}

		public override void SerializeFast(FastBufferWriter writer)
		{
			base.SerializeFast(writer);
			writer.WriteValueSafe(in pointIndex, default(FastBufferWriter.ForPrimitives));
		}

		public override void DeserializeFast(FastBufferReader reader)
		{
			base.DeserializeFast(reader);
			reader.ReadValue(out pointIndex, default(FastBufferWriter.ForPrimitives));
		}
	}

	[SerializeField]
	private Dialogue dialogue;

	[SerializeField]
	private Transform throwablePrefab;

	[SerializeField]
	private Transform startThrowPoint;

	[SerializeField]
	private Interval_Float startThrowInterval;

	[SerializeField]
	private Interval_Float throwIntervalInbetween;

	[SerializeField]
	private Interval_Int randomThrows;

	[SerializeField]
	private int maxThrows = 10;

	[SerializeField]
	private float validPlayerRadius = 5f;

	[SerializeField]
	private float throwTotalTime = 5f;

	[SerializeField]
	private float throwHeight = 5f;

	[SerializeField]
	private float validPlayerYDiff = 4f;

	[SerializeField]
	private float bombOccupationDownTime = 1.5f;

	[SerializeField]
	private List<Transform> validPoints = new List<Transform>();

	[SerializeField]
	private AnimatorController throwAnimator;

	[SerializeField]
	private Transform handParent;

	public EventReference er_throw;

	public EventReference er_land;

	[SerializeField]
	private float gizmoRadius = 3f;

	private Raft_Network network;

	private bool updateThrowTimer = true;

	private float throwTimer;

	private List<Transform> occupiedPoints = new List<Transform>();

	private bool waitingForThrowAnimation;

	private void Awake()
	{
		throwTimer = startThrowInterval.GetRandomValue();
		if (throwAnimator != null)
		{
			AnimatorController animatorController = throwAnimator;
			animatorController.OnAnimationEvent = (Action<string>)Delegate.Combine(animatorController.OnAnimationEvent, new Action<string>(OnAnimationEvent));
		}
	}

	private void Update()
	{
		if (Raft_Network.IsHost && updateThrowTimer)
		{
			throwTimer -= Time.deltaTime;
			if (throwTimer <= 0f)
			{
				throwTimer = startThrowInterval.GetRandomValue();
				CoroutineManager.Singleton.AddCoroutine(ThrowTowardsPlayers());
			}
		}
	}

	private void OnAnimationEvent(string eventName)
	{
		if (eventName == "Throw")
		{
			waitingForThrowAnimation = false;
		}
	}

	private IEnumerator ThrowFireworkToPoint(Transform point)
	{
		Transform throwableModel = ((!(handParent == null)) ? UnityEngine.Object.Instantiate(throwablePrefab, handParent.position, Quaternion.identity, handParent) : UnityEngine.Object.Instantiate(throwablePrefab, transform.position, Quaternion.identity, transform));
		if (throwAnimator != null)
		{
			throwAnimator.SetTrigger("Throw");
			waitingForThrowAnimation = true;
		}
		while (waitingForThrowAnimation)
		{
			yield return null;
		}
		if (handParent != null)
		{
			startThrowPoint.position = handParent.position;
		}
		throwableModel.SetParentSafe(startThrowPoint);
		throwableModel.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
		CoroutineManager.Singleton.AddCoroutine(MoveThrowable(throwableModel, startThrowPoint, point, throwTotalTime));
		occupiedPoints.AddUniqueOnly(point);
		float delay = throwTotalTime + 10f + bombOccupationDownTime;
		Explosive componentInChildren = throwableModel.GetComponentInChildren<Explosive>();
		if (componentInChildren != null)
		{
			delay = throwTotalTime + componentInChildren.fuseTime + componentInChildren.fireTime + bombOccupationDownTime;
		}
		CoroutineManager.Singleton.AddCoroutine(RemoveOccupation(point, delay));
		RuntimeManager.PlayOneShot(er_throw, transform.position);
	}

	private IEnumerator ThrowTowardsPlayers()
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		List<Transform> list = new List<Transform>();
		if (network != null && network.remoteUsers != null)
		{
			foreach (Network_Player value in network.remoteUsers.Values)
			{
				if (value == null)
				{
					continue;
				}
				Vector3 position = value.transform.position;
				foreach (Transform validPoint in validPoints)
				{
					if (validPoint == null || occupiedPoints.Contains(validPoint))
					{
						continue;
					}
					Vector3 position2 = validPoint.position;
					float num = position.y - position2.y;
					if (num >= 0f && num < validPlayerYDiff && position.DistanceXZ(position2) <= validPlayerRadius)
					{
						list.AddUniqueOnly(validPoint);
						if (list.Count >= maxThrows)
						{
							break;
						}
					}
				}
				if (list.Count >= maxThrows)
				{
					break;
				}
			}
		}
		if (list.Count <= 0)
		{
			yield break;
		}
		int randomValue = randomThrows.GetRandomValue();
		for (int i = 0; i < randomValue; i++)
		{
			if (list.Count >= maxThrows)
			{
				break;
			}
			Transform transform = validPoints[UnityEngine.Random.Range(0, validPoints.Count)];
			if (!occupiedPoints.Contains(transform))
			{
				list.AddUniqueOnly(transform);
			}
		}
		if (dialogue != null)
		{
			dialogue.PlayNextVoice_Networked(null);
		}
		updateThrowTimer = false;
		Message_ThrowExplosive msg_throw = new Message_ThrowExplosive(Messages.ThrowExplosive, network.NetworkIDManager, ObjectIndex, -1);
		foreach (Transform item in list)
		{
			msg_throw.pointIndex = GetPointIndexFromPoint(item);
			network.RPC(msg_throw, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			yield return ThrowFireworkToPoint(item);
			yield return new WaitForSeconds(throwIntervalInbetween.GetRandomValue());
		}
		updateThrowTimer = true;
	}

	private IEnumerator RemoveOccupation(Transform point, float delay)
	{
		yield return new WaitForSeconds(delay);
		if (occupiedPoints != null)
		{
			occupiedPoints.Remove(point);
		}
	}

	private IEnumerator MoveThrowable(Transform throwable, Transform startPoint, Transform endPoint, float totalTime)
	{
		float startTime = Time.time;
		while (!(throwable == null) && !(startPoint == null) && !(endPoint == null))
		{
			float num = Mathf.Clamp01((Time.time - startTime) / totalTime);
			throwable.position = Helper.GetBezierCurveValue(startPoint.position, endPoint.position, throwHeight, num);
			if (num >= 1f)
			{
				throwable.GetComponentInChildren<Explosive>().OnLand();
				RuntimeManager.PlayOneShot(er_land, throwable.position);
				break;
			}
			yield return null;
		}
	}

	private int GetPointIndexFromPoint(Transform point)
	{
		for (int i = 0; i < validPoints.Count; i++)
		{
			if (validPoints[i] == point)
			{
				return i;
			}
		}
		return -1;
	}

	private void ResetThrowTimer()
	{
		throwTimer = startThrowInterval.GetRandomValue() * GameModeValueManager.GetCurrentGameModeValue().landmarkVariables.explosiveThrowerIntervallMultiplier;
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		switch (msg.Type)
		{
		case Messages.ThrowExplosive:
		{
			Message_ThrowExplosive message_ThrowExplosive = msg as Message_ThrowExplosive;
			if (message_ThrowExplosive.pointIndex >= 0 && message_ThrowExplosive.pointIndex < validPoints.Count)
			{
				CoroutineManager.Singleton.AddCoroutine(ThrowFireworkToPoint(validPoints[message_ThrowExplosive.pointIndex]));
			}
			return true;
		}
		case Messages.Dialogue:
		{
			Message_Dialogue message_Dialogue = msg as Message_Dialogue;
			dialogue.PlayVoice(message_Dialogue, network.GetPlayerFromID(message_Dialogue.SteamID));
			return true;
		}
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
