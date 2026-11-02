using System;
using System.Collections;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterUnlock : MonoBehaviour_ID_Network, IRaycastable
{
	[SerializeField]
	private QuestInteractable questInteractable;

	[SerializeField]
	private float questInteractDelay;

	[SerializeField]
	private SO_Character characterToUnlock;

	[SerializeField]
	private Dialogue dialogue;

	[SerializeField]
	private GameObject replaySphere;

	[SerializeField]
	private GameObject characterModel;

	[SerializeField]
	private Animator characterAnimator;

	[SerializeField]
	private Animator objectAnimator;

	[SerializeField]
	private float objectAnimationDelay;

	[SerializeField]
	private float fadeSpeed;

	[SerializeField]
	private float startTalkingDelay;

	[SerializeField]
	private float endTalkingDelay;

	[SerializeField]
	private Renderer[] defaultRenderers;

	[SerializeField]
	private Renderer[] dissolveRenderers;

	[SerializeField]
	private HeadTrackingIK headTrackingIK;

	[SerializeField]
	private StudioEventEmitter emitter_unlock;

	[SerializeField]
	private bool playBeforeObjectAnimationDelay;

	private Raft_Network network;

	private DisplayTextManager displayTextManager;

	private bool unlocked;

	private Coroutine coroutine;

	private MaterialPropertyBlock mpb;

	public bool Unlocked
	{
		get
		{
			return unlocked;
		}
		set
		{
			unlocked = value;
		}
	}

	private void Start()
	{
		if (this.questInteractable != null)
		{
			QuestInteractable questInteractable = this.questInteractable;
			questInteractable.OnInteract = (Action<int, bool, Network_Player, bool>)Delegate.Combine(questInteractable.OnInteract, new Action<int, bool, Network_Player, bool>(OnQuestInteract));
		}
		if (headTrackingIK != null)
		{
			headTrackingIK.InitializeStartState();
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (!GameManager.IsQuittingApplication && coroutine != null)
		{
			CoroutineManager.Singleton.StopCoroutine(coroutine);
			coroutine = null;
		}
	}

	private void OnQuestInteract(int objectStateIndex, bool atStart, Network_Player player, bool success)
	{
		if ((Raft_Network.IsHost & success) && !atStart && questInteractable.IsAtMaxStateIndex)
		{
			if (questInteractDelay > 0f)
			{
				Invoke("LateInteract", questInteractDelay);
			}
			else
			{
				LateInteract();
			}
		}
	}

	private void LateInteract()
	{
		InteractNetworked();
	}

	void IRaycastable.OnIsRayed()
	{
		if (displayTextManager == null)
		{
			displayTextManager = ComponentManager<CanvasHelper>.Value.displayTextManager;
		}
		if (coroutine == null)
		{
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
			LocalizationParameters.itemX = ((dialogue.voiceActor != null) ? dialogue.voiceActor.displayName : string.Empty);
			displayTextManager.ShowText(Helper.GetTerm("Game/TalkToX", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				displayTextManager.HideDisplayTexts();
				InteractNetworked();
			}
		}
	}

	void IRaycastable.OnRayEnter()
	{
	}

	void IRaycastable.OnRayExit()
	{
		if (displayTextManager != null)
		{
			displayTextManager.HideDisplayTexts();
		}
	}

	private void InteractNetworked()
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		Message_NetworkBehaviour_ID_SteamID message = new Message_NetworkBehaviour_ID_SteamID(Messages.CharacterUnlock_Interact, network.NetworkIDManager, ObjectIndex, network.LocalSteamID);
		if (Raft_Network.IsHost)
		{
			if (Interact(ComponentManager<Network_Player>.Value))
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private bool Interact(Network_Player interactor)
	{
		if (coroutine != null)
		{
			return false;
		}
		headTrackingIK.ResetToStartState();
		headTrackingIK.target = interactor.CameraTransform;
		coroutine = CoroutineManager.Singleton.AddCoroutine(Unlock(restoring: false, interactor));
		return true;
	}

	private IEnumerator Unlock(bool restoring, Network_Player interactor)
	{
		Unlocked = true;
		if (replaySphere.activeSelf)
		{
			yield return StartCoroutine(FadeCharacter(fadeSpeed, visible: true));
		}
		if (!playBeforeObjectAnimationDelay && !restoring && emitter_unlock != null)
		{
			emitter_unlock.Play();
		}
		if (restoring && objectAnimator != null)
		{
			objectAnimator.SetBool("Open", value: true);
		}
		characterAnimator.SetBool("Interact", value: true);
		if (!restoring)
		{
			yield return new WaitForSeconds(startTalkingDelay);
			float voiceLineDelay = dialogue.voiceDatas[0].totalVoiceLineDuration;
			dialogue.PlayVoice(dialogue.voiceDatas[0], interactor);
			yield return new WaitForSeconds(objectAnimationDelay);
			if (playBeforeObjectAnimationDelay && !restoring && emitter_unlock != null)
			{
				emitter_unlock.Play();
			}
			if (objectAnimator != null)
			{
				objectAnimator.SetBool("Open", value: true);
			}
			yield return new WaitForSeconds(voiceLineDelay - objectAnimationDelay);
			if (characterToUnlock == null)
			{
				Debug.LogError("Can't unlock character because 'characterToUnlock' reference is null");
			}
			else
			{
				CharacterManager.UnlockCharacterByIndex(characterToUnlock.index);
			}
		}
		characterAnimator.SetBool("Interact", value: false);
		if (!restoring)
		{
			yield return new WaitForSeconds(endTalkingDelay);
		}
		yield return StartCoroutine(FadeCharacter(fadeSpeed, visible: false));
		coroutine = null;
	}

	private void SetDissolveMaterialActive(bool active)
	{
		Renderer[] array = defaultRenderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(!active);
		}
		array = dissolveRenderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(active);
		}
	}

	private IEnumerator FadeCharacter(float time, bool visible)
	{
		if (mpb == null)
		{
			mpb = new MaterialPropertyBlock();
		}
		SetDissolveMaterialActive(active: true);
		float startTime = Time.time;
		float normalizedTime = 0f;
		if (visible)
		{
			characterModel.SetActiveSafe(value: true);
			replaySphere.gameObject.SetActiveSafe(value: false);
			while (normalizedTime < 1f)
			{
				normalizedTime = Mathf.Clamp01((Time.time - startTime) / time);
				Renderer[] array = dissolveRenderers;
				foreach (Renderer renderer in array)
				{
					for (int j = 0; j < renderer.materials.Length; j++)
					{
						renderer.GetPropertyBlock(mpb, j);
						mpb.SetFloat("_Dissolve", normalizedTime);
						renderer.SetPropertyBlock(mpb, j);
					}
				}
				yield return null;
			}
		}
		else
		{
			while (normalizedTime < 1f)
			{
				normalizedTime = Mathf.Clamp01((Time.time - startTime) / time);
				Renderer[] array = dissolveRenderers;
				foreach (Renderer renderer2 in array)
				{
					for (int k = 0; k < renderer2.materials.Length; k++)
					{
						renderer2.GetPropertyBlock(mpb, k);
						mpb.SetFloat("_Dissolve", 1f - normalizedTime);
						renderer2.SetPropertyBlock(mpb, k);
					}
				}
				yield return null;
			}
			characterModel.SetActiveSafe(value: false);
			replaySphere.gameObject.SetActiveSafe(value: true);
		}
		SetDissolveMaterialActive(active: false);
	}

	public void Restore(RGD_LandmarkItem_CharacterUnlock rgd)
	{
		if (rgd.unlocked)
		{
			Unlocked = true;
			if (objectAnimator != null)
			{
				objectAnimator.SetBool("Open", value: true);
			}
			if (characterAnimator != null)
			{
				characterAnimator.SetBool("Interact", value: false);
			}
			characterModel.SetActiveSafe(value: false);
			replaySphere.gameObject.SetActiveSafe(value: true);
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.CharacterUnlock_Interact:
		{
			Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID = msg as Message_NetworkBehaviour_ID_SteamID;
			if (message_NetworkBehaviour_ID_SteamID != null)
			{
				if (network == null)
				{
					network = ComponentManager<Raft_Network>.Value;
				}
				return Interact(network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID));
			}
			break;
		}
		case Messages.Dialogue:
		{
			Message_Dialogue message_Dialogue = msg as Message_Dialogue;
			if (msg != null)
			{
				if (network == null)
				{
					network = ComponentManager<Raft_Network>.Value;
				}
				dialogue.PlayVoice(message_Dialogue, network.GetPlayerFromID(message_Dialogue.SteamID));
				return true;
			}
			break;
		}
		}
		return base.Deserialize(msg, remoteID);
	}
}
