using System;
using System.Collections;
using Steamworks;
using UnityEngine;

public class Boss_OlofPart2 : MonoBehaviour_ID_Network
{
	public Action OnThrowMeat;

	public Transform meat;

	public Transform handParent;

	public Transform leashPoint;

	public Transform bossLandPoint;

	public QuestEventBase questEvent_LarsIntro;

	[HideInInspector]
	public AI_State_Wait_HyenaBossEat hyenaState;

	[SerializeField]
	private AnimatorController animatorController;

	public Dialogue dialogue;

	public float totalThrowTime;

	[SerializeField]
	private float throwHeight;

	private Coroutine throwRoutine;

	private bool waitingForThrowEvent;

	private Raft_Network network;

	private void Start()
	{
		AnimatorController animatorController = this.animatorController;
		animatorController.OnAnimationEvent = (Action<string>)Delegate.Combine(animatorController.OnAnimationEvent, new Action<string>(OnAnimationEvent));
		network = ComponentManager<Raft_Network>.Value;
	}

	public void OnAnimationEvent(string eventName)
	{
		if (eventName == "Throw")
		{
			waitingForThrowEvent = false;
		}
	}

	public void ResetThrow()
	{
		if (throwRoutine != null)
		{
			CoroutineManager.Singleton.StopCoroutine(throwRoutine);
			throwRoutine = null;
		}
	}

	public void StartThrowingMeat()
	{
		if (throwRoutine != null)
		{
			return;
		}
		throwRoutine = CoroutineManager.Singleton.AddCoroutine(ThrowMeat());
		if (Raft_Network.IsHost)
		{
			if (dialogue != null)
			{
				dialogue.PlayVoice_Networked(dialogue.voiceDatas[1], null);
			}
			Message_NetworkBehaviour_ID message = new Message_NetworkBehaviour_ID(Messages.OlofThrowMeat, network.NetworkIDManager, ObjectIndex);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private IEnumerator ThrowMeat()
	{
		Transform targetPoint = hyenaState.meatTargetPoint;
		meat.SetParent(handParent);
		Transform transform = meat;
		Vector3 localPosition = (meat.localEulerAngles = Vector3.zero);
		transform.localPosition = localPosition;
		meat.gameObject.SetActiveSafe(value: true);
		animatorController.SetTrigger("Throw");
		waitingForThrowEvent = true;
		while (waitingForThrowEvent)
		{
			yield return null;
		}
		Vector3 startScale = Vector3.one * 0.5f;
		Vector3 startEuler = meat.eulerAngles;
		meat.transform.SetParentSafe(base.transform);
		meat.transform.localScale = startScale;
		OnThrowMeat?.Invoke();
		float timer = 0f;
		float normalizedTime = 0f;
		while (normalizedTime < 1f)
		{
			timer += Time.deltaTime;
			normalizedTime = Mathf.Clamp01(timer / totalThrowTime);
			meat.transform.position = Helper.GetBezierCurveValue(handParent.position, targetPoint.position, throwHeight, normalizedTime);
			meat.transform.eulerAngles = Vector3.Lerp(startEuler, targetPoint.eulerAngles, normalizedTime);
			meat.transform.localScale = Vector3.Lerp(startScale, Vector3.one, normalizedTime);
			yield return null;
		}
		meat.transform.localScale = Vector3.one;
		meat.transform.SetParentSafe(targetPoint);
		Transform transform2 = meat.transform;
		localPosition = (meat.transform.localEulerAngles = Vector3.zero);
		transform2.localPosition = localPosition;
		throwRoutine = null;
		yield return null;
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.OlofThrowMeat)
		{
			StartThrowingMeat();
			return false;
		}
		return base.Deserialize(msg, remoteID);
	}
}
