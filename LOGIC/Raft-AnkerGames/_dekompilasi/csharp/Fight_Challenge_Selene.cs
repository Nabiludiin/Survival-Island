using System;
using System.Collections;
using FMODUnity;
using Steamworks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fight_Challenge_Selene : MonoBehaviour_ID_Network, IRaycastable
{
	[Serializable]
	public class Message_ToggleHeld : Message_NetworkBehaviour_ID_SteamID
	{
		public bool toggledValue;

		protected bool Equals(Message_ToggleHeld other)
		{
			if (Equals((Message_NetworkBehaviour_ID_SteamID)other))
			{
				return toggledValue == other.toggledValue;
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
			return Equals((Message_ToggleHeld)obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(base.GetHashCode(), toggledValue);
		}

		public static bool operator ==(Message_ToggleHeld left, Message_ToggleHeld right)
		{
			return object.Equals(left, right);
		}

		public static bool operator !=(Message_ToggleHeld left, Message_ToggleHeld right)
		{
			return !object.Equals(left, right);
		}

		public Message_ToggleHeld()
		{
		}

		public Message_ToggleHeld(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, bool value, Network_UserId ID)
			: base(type, behaviour, networkID_ObjectIndex, ID)
		{
			toggledValue = value;
		}

		public override void SerializeFast(FastBufferWriter writer)
		{
			base.SerializeFast(writer);
			writer.WriteValueSafe(in toggledValue, default(FastBufferWriter.ForPrimitives));
		}

		public override void DeserializeFast(FastBufferReader reader)
		{
			base.DeserializeFast(reader);
			reader.ReadValue(out toggledValue, default(FastBufferWriter.ForPrimitives));
		}
	}

	private float holdProgress;

	public Action<Fight_Challenge_Selene> OnCompletion;

	public LandmarkItem landmarkItem;

	[SerializeField]
	private string interactionButton;

	[SerializeField]
	private float holdDuration = 5f;

	[SerializeField]
	private float durationBetweenSpawn;

	[SerializeField]
	private bool resetHeldDurationOnRelease = true;

	[SerializeField]
	private Animator doorAnimator;

	[SerializeField]
	private Animator[] doorProgressAnimators;

	[SerializeField]
	private LandmarkEntityCollection spawner;

	[SerializeField]
	private GameObject lights;

	[SerializeField]
	private MeshRenderer[] shimmerRenderers;

	[HideInInspector]
	public bool isCompleted;

	[HideInInspector]
	public bool isFirstInteraction = true;

	[SerializeField]
	public string holdPrompt = "Game/RotateSmooth2";

	[SerializeField]
	private StudioEventEmitter emitter_handleSpin;

	[SerializeField]
	private StudioEventEmitter emitter_doorClose;

	[SerializeField]
	private StudioEventEmitter emitter_doorComplete;

	[SerializeField]
	private StudioEventEmitter emitter_doorOpen;

	[SerializeField]
	private AnimatorMessageForwarder animatorMessage;

	private bool isHeld;

	private Network_UserId holdingPlayerID;

	private Raft_Network network;

	private CanvasHelper canvasHelper;

	private MaterialPropertyBlock matPropBlock;

	public float HoldProgress
	{
		get
		{
			return holdProgress;
		}
		set
		{
			holdProgress = value;
		}
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		canvasHelper = ComponentManager<CanvasHelper>.Value;
		if (emitter_doorOpen != null && animatorMessage != null)
		{
			AnimatorMessageForwarder animatorMessageForwarder = animatorMessage;
			animatorMessageForwarder.OnAnimatorMessage = (Action<string>)Delegate.Combine(animatorMessageForwarder.OnAnimatorMessage, new Action<string>(OnAnimatorMessage));
		}
	}

	private void Update()
	{
		if (!isCompleted)
		{
			if (isHeld)
			{
				Held();
			}
			else if (resetHeldDurationOnRelease)
			{
				NotHeld();
			}
		}
	}

	private void OnAnimatorMessage(string msg)
	{
		if (msg == "nudge" || msg == "complete")
		{
			emitter_doorOpen.EventInstance.keyOff();
		}
	}

	public void OnIsRayed()
	{
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		if (!isHeld && !isCompleted)
		{
			canvasHelper.displayTextManager.ShowText(Helper.GetTerm(holdPrompt), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		}
		if (!isHeld && !isCompleted && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			StartHolding(network.LocalSteamID, networkSend: true);
			canvasHelper.displayTextManager.HideDisplayTexts();
		}
		else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(action, "Interact"))
		{
			StopHolding(network.LocalSteamID, networkSend: true);
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (isHeld)
		{
			StopHolding(network.LocalSteamID, networkSend: true);
		}
		canvasHelper.displayTextManager.HideDisplayTexts();
	}

	public void StartHolding(Network_UserId holdingPlayer, bool networkSend)
	{
		if (isCompleted)
		{
			return;
		}
		isHeld = true;
		HandleDoorProgressAnimators(1f);
		if (doorAnimator != null)
		{
			doorAnimator.SetInteger("StateIndex", 1);
		}
		holdingPlayerID = holdingPlayer;
		if (Raft_Network.IsHost && isFirstInteraction)
		{
			if (spawner != null && spawner.spawners.Length != 0)
			{
				CoroutineManager.Singleton.AddCoroutine(FirstInteractionSpawn());
			}
			isFirstInteraction = false;
		}
		if (emitter_handleSpin != null)
		{
			emitter_handleSpin.Stop();
			emitter_handleSpin.Play();
		}
		if (emitter_doorOpen != null)
		{
			emitter_doorOpen.Stop();
			emitter_doorOpen.Play();
		}
		if (networkSend)
		{
			Message_ToggleHeld message = new Message_ToggleHeld(Messages.Toggle_Held, network.NetworkIDManager, ObjectIndex, value: true, network.LocalSteamID);
			if (Raft_Network.IsHost)
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public void StopHolding(Network_UserId ID, bool networkSend)
	{
		if (isCompleted)
		{
			return;
		}
		if (ID == holdingPlayerID)
		{
			isHeld = false;
			holdingPlayerID = 0uL;
			if (doorAnimator != null)
			{
				doorAnimator.SetInteger("StateIndex", 0);
			}
			HandleDoorProgressAnimators(0f);
			if (canvasHelper != null)
			{
				canvasHelper.SetLoadCircle(0f);
			}
			if (networkSend)
			{
				Message_ToggleHeld message = new Message_ToggleHeld(Messages.Toggle_Held, network.NetworkIDManager, ObjectIndex, value: false, network.LocalSteamID);
				if (Raft_Network.IsHost)
				{
					network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
				else
				{
					network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
		}
		if (emitter_handleSpin != null && emitter_handleSpin.IsPlaying())
		{
			emitter_handleSpin.EventInstance.keyOff();
		}
		if (emitter_doorOpen != null)
		{
			emitter_doorOpen.Stop();
		}
		if (emitter_doorClose != null)
		{
			emitter_doorClose.Play();
		}
	}

	public void CompleteChallenge()
	{
		isCompleted = true;
		HoldProgress = holdDuration;
		if (doorAnimator != null)
		{
			doorAnimator.SetInteger("StateIndex", 2);
		}
		HandleDoorProgressAnimators(0f);
		SetDoorProgressAnimatorsTime(1f);
		if (holdingPlayerID != 0uL && holdingPlayerID == network.LocalSteamID)
		{
			UpdateUIChannelRing(0f);
		}
		if (emitter_handleSpin != null && emitter_handleSpin.IsPlaying())
		{
			emitter_handleSpin.EventInstance.keyOff();
		}
		if (emitter_doorComplete != null)
		{
			emitter_doorComplete.Stop();
			emitter_doorComplete.Play();
		}
		if (lights != null)
		{
			lights.SetActiveSafe(value: true);
		}
		SetShimmerState(shimmer: false);
		OnCompletion?.Invoke(this);
	}

	private void SetShimmerState(bool shimmer)
	{
		if (matPropBlock == null)
		{
			matPropBlock = new MaterialPropertyBlock();
		}
		MeshRenderer[] array = shimmerRenderers;
		foreach (MeshRenderer meshRenderer in array)
		{
			if (!(meshRenderer == null))
			{
				float value = (shimmer ? 1f : 0f);
				meshRenderer.GetPropertyBlock(matPropBlock);
				matPropBlock.SetFloat("_Shimmer", value);
				meshRenderer.SetPropertyBlock(matPropBlock);
			}
		}
	}

	private void UpdateUIChannelRing(float normalizedFill)
	{
		if (canvasHelper != null)
		{
			canvasHelper.SetLoadCircle(normalizedFill);
		}
	}

	private void Held()
	{
		HoldProgress += Time.deltaTime;
		if (HoldProgress >= holdDuration)
		{
			CompleteChallenge();
		}
		if (!isCompleted && holdingPlayerID == network.LocalSteamID && HoldProgress > 0f)
		{
			UpdateUIChannelRing(HoldProgress / holdDuration);
		}
	}

	private void NotHeld()
	{
		HoldProgress = 0f;
	}

	private IEnumerator FirstInteractionSpawn()
	{
		LandmarkEntitySpawner[] spawners = spawner.spawners;
		for (int i = 0; i < spawners.Length; i++)
		{
			spawners[i].ManuallySpawn();
			yield return new WaitForSeconds(durationBetweenSpawn);
		}
	}

	private void HandleDoorProgressAnimators(float speed)
	{
		if (doorProgressAnimators == null)
		{
			return;
		}
		Animator[] array = doorProgressAnimators;
		foreach (Animator animator in array)
		{
			if (animator != null)
			{
				animator.SetFloat("SpeedModifyer", speed);
			}
		}
	}

	private void SetDoorProgressAnimatorsTime(float normalizedtime)
	{
		Animator[] array = doorProgressAnimators;
		foreach (Animator animator in array)
		{
			if (animator != null)
			{
				animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, -1, normalizedtime);
			}
		}
	}

	public RGD_LandmarkItem_SeleneFightChallenge GetRGDSave()
	{
		return new RGD_LandmarkItem_SeleneFightChallenge(landmarkItem, this);
	}

	public void RestoreRGDSave(RGD_LandmarkItem_SeleneFightChallenge landmarkItem)
	{
		isFirstInteraction = !landmarkItem.firstInteractionCompleted;
		isCompleted = landmarkItem.complete;
		HoldProgress = landmarkItem.holdProgress;
		SetDoorProgressAnimatorsTime(Mathf.Clamp01(holdProgress / holdDuration));
		if (isCompleted)
		{
			CompleteChallenge();
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.Toggle_Held)
		{
			Message_ToggleHeld message_ToggleHeld = msg as Message_ToggleHeld;
			if (msg != null)
			{
				if (Raft_Network.IsHost)
				{
					bool flag = false;
					if (isHeld != message_ToggleHeld.toggledValue)
					{
						flag = true;
					}
					if (flag)
					{
						if (message_ToggleHeld.toggledValue)
						{
							StartHolding(message_ToggleHeld.SteamID, networkSend: false);
						}
						else
						{
							StopHolding(message_ToggleHeld.SteamID, networkSend: false);
						}
					}
					return flag;
				}
				if (message_ToggleHeld.toggledValue)
				{
					StartHolding(message_ToggleHeld.SteamID, networkSend: false);
				}
				else
				{
					StopHolding(message_ToggleHeld.SteamID, networkSend: false);
				}
				isHeld = message_ToggleHeld.toggledValue;
				return false;
			}
		}
		return base.Deserialize(msg, remoteID);
	}
}
