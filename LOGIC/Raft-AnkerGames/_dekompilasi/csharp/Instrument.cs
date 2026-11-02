using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Instrument : MonoBehaviour_ID_Network
{
	[Serializable]
	protected class InstrumentKey
	{
		public string interactHotkey;

		public StudioEventEmitter soundEmitter;

		public bool isKeyDown;

		[SerializeField]
		private int keyIndex = -1;

		[Space(5f)]
		[SerializeField]
		public Instrument owner;

		[SerializeField]
		private float minAngle;

		[SerializeField]
		private float maxAngle;

		private bool delayedStop;

		public InstrumentKey(int keyIndex, string interactHotkey, StudioEventEmitter emitter, Instrument owner)
		{
			this.keyIndex = keyIndex;
			this.interactHotkey = interactHotkey;
			soundEmitter = emitter;
			this.owner = owner;
		}

		public void HandleKeyPress()
		{
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
			{
				bool flag = !owner.negativeY && maxAngle > 360f;
				if (owner.currentAngleSet && owner.currentAngle > (flag ? (minAngle - 360f) : minAngle) && owner.currentAngle < (flag ? (maxAngle - 360f) : maxAngle))
				{
					SimpleMonoBehaviourSingleton<PianoUI>.Instance.SetNote(keyIndex);
					if (delayedStop)
					{
						return;
					}
					if (owner.inputPress.action.IsPressed())
					{
						if (!isKeyDown)
						{
							OnPressed(null);
						}
					}
					else if (isKeyDown)
					{
						OnReleased(sendNetwork: true);
					}
				}
				else if (isKeyDown && !delayedStop)
				{
					owner.StartCoroutine(StopInSeconds());
					delayedStop = true;
				}
			}
			else if (IsPressedDown())
			{
				if (!isKeyDown)
				{
					OnPressed(null);
				}
			}
			else if (isKeyDown)
			{
				OnReleased(sendNetwork: true);
			}
		}

		public bool IsPressedDown()
		{
			if (MyInput.GetButton(interactHotkey))
			{
				return true;
			}
			return false;
		}

		private IEnumerator StopInSeconds()
		{
			yield return new WaitForSeconds(0.033f);
			OnReleased(sendNetwork: true);
			delayedStop = false;
		}

		public void OnPressed(Message_InstrumentKeyModified msg_key)
		{
			isKeyDown = true;
			int num = keyIndex + 1 + 7;
			if (msg_key == null)
			{
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
				{
					if (owner.inputLowerOctave.action.IsPressed())
					{
						num -= 7;
					}
					else if (owner.inputHigherOctave.action.IsPressed())
					{
						num += 7;
					}
				}
				else if (Input.GetKey(KeyCode.Space))
				{
					num -= 7;
				}
				else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				{
					num += 7;
				}
				AchievementHandler.AddPlayerInstrumentNotesPlayed();
			}
			else
			{
				num = msg_key.pitchIndex;
			}
			if (num > 24)
			{
				num = 24;
				Helper.LogBuild("clamping down index");
			}
			SimpleMonoBehaviourSingleton<PianoUI>.Instance.SetNote(keyIndex);
			soundEmitter.Stop();
			soundEmitter.Play();
			soundEmitter.SetParameter("pitch", num);
			if (owner.currentUser != null)
			{
				owner.animationLogic.UpdatePlayingAnimationLogic(owner.currentUser);
			}
			if (!(msg_key == null))
			{
				return;
			}
			Message_InstrumentKeyModified message_InstrumentKeyModified = CreateKeyModifiedMessage(pressed: true, num);
			if (message_InstrumentKeyModified != null)
			{
				if (Raft_Network.IsHost)
				{
					owner.network.RPC(message_InstrumentKeyModified, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
				else
				{
					owner.network.SendP2P(owner.network.HostID, message_InstrumentKeyModified, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
		}

		public void OnReleased(bool sendNetwork)
		{
			isKeyDown = false;
			soundEmitter.EventInstance.keyOff();
			if (owner.currentUser != null)
			{
				owner.animationLogic.UpdatePlayingAnimationLogic(owner.currentUser);
			}
			if (!sendNetwork)
			{
				return;
			}
			Message_InstrumentKeyModified message_InstrumentKeyModified = CreateKeyModifiedMessage(pressed: false, 0);
			if (message_InstrumentKeyModified != null)
			{
				if (Raft_Network.IsHost)
				{
					owner.network.RPC(message_InstrumentKeyModified, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
				else
				{
					owner.network.SendP2P(owner.network.HostID, message_InstrumentKeyModified, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
		}

		private Message_InstrumentKeyModified CreateKeyModifiedMessage(bool pressed, int pitchIndex)
		{
			return new Message_InstrumentKeyModified(Messages.Instrument_KeyModified, owner.network.NetworkIDManager, owner.ObjectIndex, keyIndex, pressed, pitchIndex);
		}
	}

	[SerializeField]
	private FOVStyle fovStyle;

	[SerializeField]
	private PlayerSeat playerSeat;

	[SerializeField]
	protected List<InstrumentKey> keys;

	[SerializeField]
	private InputActionReference inputPress;

	[SerializeField]
	private InputActionReference inputStick;

	[SerializeField]
	private InputActionReference inputLowerOctave;

	[SerializeField]
	private InputActionReference inputHigherOctave;

	[SerializeField]
	private MeshRenderer renderer;

	[SerializeField]
	private Texture albedoTextureConsole;

	private float currentAngle;

	private bool negativeY;

	private bool currentAngleSet;

	private Network_Player localPlayer;

	private Network_Player currentUser;

	private Raft_Network network;

	private Instrument_Animator animationLogic;

	private AttachPlayer seat;

	public virtual bool CanBeInteractedWith
	{
		get
		{
			if (localPlayer == null && network != null)
			{
				localPlayer = network.GetLocalPlayer();
			}
			if (currentUser != null)
			{
				return currentUser == localPlayer;
			}
			return false;
		}
	}

	public void FindEmittersAssignKeys()
	{
		StudioEventEmitter[] componentsInChildren = GetComponentsInChildren<StudioEventEmitter>();
		int num = 0;
		keys.Clear();
		StudioEventEmitter[] array = componentsInChildren;
		foreach (StudioEventEmitter emitter in array)
		{
			keys.Add(new InstrumentKey(num, "Hotbar" + num, emitter, this));
			num++;
		}
	}

	public void Awake()
	{
		seat = playerSeat.Seats[0];
		AttachPlayer attachPlayer = seat;
		attachPlayer.OnStartCarryPlayer = (Action<Network_Player>)Delegate.Combine(attachPlayer.OnStartCarryPlayer, new Action<Network_Player>(SetCurrentUser));
		AttachPlayer attachPlayer2 = seat;
		attachPlayer2.OnStopCarryPlayer = (Action<Network_Player>)Delegate.Combine(attachPlayer2.OnStopCarryPlayer, (Action<Network_Player>)((Network_Player player) =>
		{
			SetCurrentUser(null);
		}));
		animationLogic = new Instrument_Animator(0.8f);
	}

	public void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
	}

	private void Update()
	{
		if (CanBeInteractedWith)
		{
			HandleInput();
		}
	}

	protected override void OnDestroy()
	{
		AttachPlayer attachPlayer = seat;
		attachPlayer.OnStartCarryPlayer = (Action<Network_Player>)Delegate.Remove(attachPlayer.OnStartCarryPlayer, new Action<Network_Player>(SetCurrentUser));
		AttachPlayer attachPlayer2 = seat;
		attachPlayer2.OnStopCarryPlayer = (Action<Network_Player>)Delegate.Remove(attachPlayer2.OnStopCarryPlayer, (Action<Network_Player>)((Network_Player player) =>
		{
			SetCurrentUser(null);
		}));
		NetworkIDManager.RemoveNetworkID(this);
		base.OnDestroy();
	}

	private void OnBlockPlaced()
	{
		ObjectIndex = GetComponentInParent<Block>().ObjectIndex;
		NetworkIDManager.AddNetworkID(this);
	}

	private void SetCurrentUser(Network_Player player)
	{
		Network_Player network_Player = currentUser;
		currentUser = player;
		if (currentUser == null)
		{
			if (network_Player != null)
			{
				if (network_Player.IsLocalPlayer)
				{
					ThirdPerson thirdPersonSettings = network_Player.currentModel.thirdPersonSettings;
					thirdPersonSettings.OnThirdpersonStateChange = (Action<bool>)Delegate.Remove(thirdPersonSettings.OnThirdpersonStateChange, new Action<bool>(ThirdPersonStateChange));
					UpdateMouseLookScripts(network_Player, canLook: true);
					UpdateFOVStyle(network_Player, FOVStyle.CurrentSetting);
					UpdateGamepadUI(show: false);
				}
				animationLogic.SetIsUsedAnimation(network_Player, value: false);
			}
		}
		else
		{
			animationLogic.SetIsUsedAnimation(currentUser, value: true);
			if (currentUser.IsLocalPlayer)
			{
				ThirdPerson thirdPersonSettings2 = currentUser.currentModel.thirdPersonSettings;
				thirdPersonSettings2.OnThirdpersonStateChange = (Action<bool>)Delegate.Combine(thirdPersonSettings2.OnThirdpersonStateChange, new Action<bool>(ThirdPersonStateChange));
				UpdateMouseLookScripts(currentUser, canLook: false);
				UpdateFOVStyle(currentUser, fovStyle);
				UpdateGamepadUI(show: true);
			}
		}
	}

	private void HandleInput()
	{
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			Vector2 vector = inputStick.action.ReadValue<Vector2>();
			if (vector.sqrMagnitude > 0f)
			{
				float num = (currentAngle = 57.29578f * Mathf.Atan2(vector.y, vector.x));
				currentAngleSet = true;
				negativeY = num < 0f;
				if (negativeY)
				{
					currentAngle += 360f;
				}
			}
			else
			{
				currentAngleSet = false;
				SimpleMonoBehaviourSingleton<PianoUI>.Instance.ResetNote();
			}
			SimpleMonoBehaviourSingleton<PianoUI>.Instance.SetAIsPressed(inputPress.action.IsPressed());
			SimpleMonoBehaviourSingleton<PianoUI>.Instance.SetLTIsPressed(inputLowerOctave.action.IsPressed());
			SimpleMonoBehaviourSingleton<PianoUI>.Instance.SetRTIsPressed(inputHigherOctave.action.IsPressed());
		}
		foreach (InstrumentKey key in keys)
		{
			key.HandleKeyPress();
		}
	}

	private void ThirdPersonStateChange(bool newThirdPersonState)
	{
		if (localPlayer == null)
		{
			localPlayer = network.GetLocalPlayer();
		}
		UpdateMouseLookScripts(localPlayer, newThirdPersonState);
		UpdateFOVStyle(localPlayer, (!newThirdPersonState) ? fovStyle : FOVStyle.CurrentSetting);
	}

	private void UpdateMouseLookScripts(Network_Player player, bool canLook)
	{
		if (player != null && player.IsLocalPlayer)
		{
			player.PlayerScript.SetMouseLookScripts(canLook);
		}
	}

	private void UpdateFOVStyle(Network_Player player, FOVStyle style)
	{
		if (player != null && player.IsLocalPlayer)
		{
			player.currentModel.SetFOVStyle(style);
		}
	}

	private void UpdateGamepadUI(bool show)
	{
		if (show)
		{
			if (ComponentManager<CanvasHelper>.Value.OpenMenuCloseOther(MenuType.Piano, force: true))
			{
				ComponentManager<CanvasHelper>.Value.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Piano);
			}
		}
		else
		{
			ComponentManager<CanvasHelper>.Value.CloseMenu(MenuType.Piano);
			ComponentManager<CanvasHelper>.Value.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Inventory);
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.Instrument_KeyModified:
			return DeserializeInstrumentMessage(msg);
		case Messages.PlayerSeat_Take:
		case Messages.PlayerSeat_Leave:
			return playerSeat.HandleDeserialize(msg, remoteID);
		default:
			return base.Deserialize(msg, remoteID);
		}
	}

	public bool DeserializeInstrumentMessage(Message_NetworkBehaviour message)
	{
		if (message.Type == Messages.Instrument_KeyModified)
		{
			Message_InstrumentKeyModified message_InstrumentKeyModified = message as Message_InstrumentKeyModified;
			if (message_InstrumentKeyModified != null)
			{
				InstrumentKey instrumentKey = null;
				if (keys.Count >= message_InstrumentKeyModified.keyIndex)
				{
					instrumentKey = keys[message_InstrumentKeyModified.keyIndex];
				}
				if (instrumentKey != null)
				{
					if (message_InstrumentKeyModified.pressed)
					{
						instrumentKey.OnPressed(message_InstrumentKeyModified);
					}
					else
					{
						instrumentKey.OnReleased(sendNetwork: false);
					}
					return true;
				}
				Helper.LogBuild("instrument key at index" + message_InstrumentKeyModified.keyIndex + " is invalid, out of range or not found! on" + transform.name);
			}
		}
		return false;
	}
}
