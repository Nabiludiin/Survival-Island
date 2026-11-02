using System;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public class Reciever : MonoBehaviour_ID_Network, SceneEventInterface, IRaycastable
{
	public static List<Reciever> AllRecievers = new List<Reciever>();

	[SerializeField]
	private RecieverFrequency currentFrequency;

	public Raft_Network network;

	public CanvasHelper canvas;

	public bool hasSignal;

	public Reciever_Antenna[] antennas;

	public Battery battery;

	[SerializeField]
	private List<ChunkPointType> typesToDisplay = new List<ChunkPointType>();

	[SerializeField]
	private Reciever_Dot dotPrefab;

	[SerializeField]
	private Interval_Float requiredAntennaDistance;

	[SerializeField]
	private float minDistanceBetweenAntennas = 0.5f;

	[SerializeField]
	private int antennasRequired;

	[SerializeField]
	private float radarNoticePointLength = 50f;

	[SerializeField]
	private float radarLength = 50f;

	[SerializeField]
	private float radarUIWidth = 270f;

	[SerializeField]
	private BlockButton button_Cycle;

	[SerializeField]
	private BlockButton button_Up;

	[SerializeField]
	private BlockButton button_Down;

	[SerializeField]
	private AntennaPanel[] antennaPanels;

	public Transform[] sockets;

	[SerializeField]
	private GameObject altitudeSection;

	[SerializeField]
	private GameObject canvasSection;

	[SerializeField]
	private GameObject antennaSection;

	[SerializeField]
	private GameObject radarSection;

	[SerializeField]
	private GameObject noFrequencySection;

	[SerializeField]
	private GameObject updateRequirecSection;

	[SerializeField]
	private GameObject numberSection;

	[SerializeField]
	private GameObject lightGreen;

	[SerializeField]
	private GameObject lightRed;

	[SerializeField]
	private GameObject lightOff;

	[SerializeField]
	private RectTransform dotParent;

	[SerializeField]
	private Slider batterySlider;

	[SerializeField]
	private TextMeshPro[] frequencyTextNumbers;

	[SerializeField]
	private Transform frequencyArrow;

	[SerializeField]
	private float frequencyArrowSpeed = 5f;

	[SerializeField]
	private Animator buttonUpAnimator;

	[SerializeField]
	private Animator buttonDownAnimator;

	[SerializeField]
	private Animator leverAnimator;

	public EventReference er_radarOn;

	public EventReference er_radarOff;

	public EventReference er_radarJingle;

	public EventReference er_radarSearch;

	public EventReference er_radarBip;

	public EventReference er_radarButton;

	public EventReference er_radarLever;

	[SerializeField]
	private StudioEventEmitter eventEmitter_batteryHum;

	private List<Reciever_Dot> radarPoints = new List<Reciever_Dot>();

	private ChunkManager chunkManager;

	private bool hasBeenPlaced;

	private bool active;

	private bool prevHasSignal;

	private bool isCurrentlyShowingRadarDot;

	private int frequencyNumberIndex;

	private bool addNoteBookNoteOnActivation = true;

	public bool HasCorrectAltitude
	{
		get
		{
			if (!Cheat.ForceRecieverToggle)
			{
				return transform.localPosition.y > 2.22f;
			}
			return true;
		}
	}

	public int FrequencyNumberIndex => frequencyNumberIndex;

	public RecieverFrequency CurrentFrequency => currentFrequency;

	private void Awake()
	{
		canvasSection.SetActive(value: false);
		altitudeSection.SetActive(value: false);
		this.battery = GetComponentInChildren<Battery>();
		Battery battery = this.battery;
		battery.OnStateChange = (Action)Delegate.Combine(battery.OnStateChange, new Action(OnBatteryStateChange));
		antennas = new Reciever_Antenna[antennasRequired];
		for (int i = 0; i < antennaPanels.Length; i++)
		{
			antennaPanels[i].Hide();
		}
		currentFrequency = new RecieverFrequency(ChunkPointType.None, randomizeFrequency: false);
		BlockButton blockButton = button_Cycle;
		blockButton.OnButtonPressed = (Action)Delegate.Combine(blockButton.OnButtonPressed, new Action(OnButtonCyclePressed));
		BlockButton blockButton2 = button_Up;
		blockButton2.OnButtonPressed = (Action)Delegate.Combine(blockButton2.OnButtonPressed, new Action(OnButtonUpPressed));
		BlockButton blockButton3 = button_Down;
		blockButton3.OnButtonPressed = (Action)Delegate.Combine(blockButton3.OnButtonPressed, new Action(OnButtonDownPressed));
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		canvas = ComponentManager<CanvasHelper>.Value;
		chunkManager = ComponentManager<ChunkManager>.Value;
	}

	private void Update()
	{
		active = hasSignal && battery.CanGiveElectricity && battery.On;
		if (active)
		{
			RecieverFrequency currentMatchingFrequency = GetCurrentMatchingFrequency();
			if (currentMatchingFrequency != null)
			{
				currentFrequency.chunkPointType = currentMatchingFrequency.chunkPointType;
			}
			if (Raft_Network.IsHost && chunkManager != null && currentFrequency.IsValid() && currentMatchingFrequency != null && currentMatchingFrequency.chunkPointType != ChunkPointType.None && !chunkManager.GetChunkPointsFromPointType(currentMatchingFrequency.chunkPointType).ContainsItems())
			{
				chunkManager.AddChunkPointForcibly(currentMatchingFrequency.chunkPointType);
			}
			UpdateRadarPointsFromChunkManager();
		}
		HandleUI();
		if (!prevHasSignal && active)
		{
			RuntimeManager.PlayOneShotAttached(er_radarJingle, gameObject);
		}
		prevHasSignal = hasSignal;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (AllRecievers != null && AllRecievers.Contains(this))
		{
			AllRecievers.Remove(this);
		}
		for (int i = 0; i < antennas.Length; i++)
		{
			if (antennas[i] != null)
			{
				antennas[i].DisconnectFromReciever();
			}
		}
		NetworkIDManager.RemoveNetworkID(this);
	}

	void IRaycastable.OnIsRayed()
	{
		if (!hasBeenPlaced)
		{
			return;
		}
		if (CanvasHelper.ActiveMenu != MenuType.None || !Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance))
		{
			canvas.displayTextManager.HideDisplayTexts();
			return;
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		if (battery.BatterySlotIsEmpty)
		{
			canvas.displayTextManager.ShowText(Helper.GetTerm("Battery/Required"));
			return;
		}
		if (!battery.CanGiveElectricity)
		{
			canvas.displayTextManager.ShowText(Helper.GetTerm("Battery/Empty"));
			return;
		}
		bool flag = !battery.On;
		canvas.displayTextManager.ShowText(Helper.GetTerm(flag ? "Game/TurnOn" : "Game/TurnOff"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			canvas.displayTextManager.HideDisplayTexts();
			Message_Battery_OnOff message = new Message_Battery_OnOff(Messages.Battery_OnOff, network.NetworkIDManager, 0uL, ObjectIndex, battery.BatteryUses, flag);
			if (Raft_Network.IsHost)
			{
				battery.On = flag;
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	void IRaycastable.OnRayEnter()
	{
	}

	void IRaycastable.OnRayExit()
	{
		if (hasBeenPlaced && canvas != null)
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}

	public void Animation_StartSearch()
	{
		if (active)
		{
			RuntimeManager.PlayOneShotAttached(er_radarSearch, gameObject);
		}
	}

	public void Animation_Blip()
	{
		if (active && isCurrentlyShowingRadarDot)
		{
			RuntimeManager.PlayOneShot(er_radarBip, transform.position);
		}
	}

	public void OnBlockPlaced()
	{
		NetworkIDManager.AddNetworkID(this);
		hasBeenPlaced = true;
		if (!AllRecievers.Contains(this))
		{
			AllRecievers.Add(this);
		}
		SetFrequencyIndexCheat(0);
		CheckSignal();
	}

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame && AllRecievers.ContainsItems())
		{
			AllRecievers.Clear();
		}
	}

	private void OnBatteryStateChange()
	{
		if (battery.On)
		{
			eventEmitter_batteryHum.Play();
			RuntimeManager.PlayOneShotAttached(er_radarOn, gameObject);
			if (Raft_Network.IsHost && addNoteBookNoteOnActivation)
			{
				NoteBook value = ComponentManager<NoteBook>.Value;
				if (value != null)
				{
					value.UnlockSpecificNoteNetworked(901, showNotification: true);
				}
			}
		}
		else
		{
			eventEmitter_batteryHum.Stop();
			RuntimeManager.PlayOneShotAttached(er_radarOff, gameObject);
		}
	}

	private void OnButtonCyclePressed()
	{
		if (Raft_Network.IsHost)
		{
			ButtonCyclePressed();
			network.RPC(new Message_InteractableButtonPress(network.NetworkIDManager, ObjectIndex, 0, null), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, new Message_InteractableButtonPress(network.NetworkIDManager, ObjectIndex, 0, null), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void ButtonCyclePressed()
	{
		RuntimeManager.PlayOneShot(er_radarLever, button_Cycle.transform.position);
		leverAnimator.SetTrigger("Pull");
		if (Raft_Network.IsHost)
		{
			IncrementFrequencyIndex();
			SendFrequencyUpdateMessage();
		}
	}

	private void OnButtonUpPressed()
	{
		if (Raft_Network.IsHost)
		{
			ButtonUpPress();
			network.RPC(new Message_InteractableButtonPress(network.NetworkIDManager, ObjectIndex, 1, null), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, new Message_InteractableButtonPress(network.NetworkIDManager, ObjectIndex, 1, null), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void ButtonUpPress()
	{
		RuntimeManager.PlayOneShot(er_radarButton, button_Up.transform.position);
		buttonUpAnimator.SetTrigger("Press");
		if (active && Raft_Network.IsHost)
		{
			IncrementCurrentFrequencyNumber(1);
			SendFrequencyUpdateMessage();
		}
	}

	private void OnButtonDownPressed()
	{
		if (Raft_Network.IsHost)
		{
			ButtonDownPress();
			network.RPC(new Message_Reciever_FrequencyUpdate(Messages.Reciever_FrequencyUpdate, network.NetworkIDManager, ObjectIndex, currentFrequency.numbers, frequencyNumberIndex), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, new Message_InteractableButtonPress(network.NetworkIDManager, ObjectIndex, 2, null), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void ButtonDownPress()
	{
		RuntimeManager.PlayOneShot(er_radarButton, button_Down.transform.position);
		buttonDownAnimator.SetTrigger("Press");
		if (active && Raft_Network.IsHost)
		{
			IncrementCurrentFrequencyNumber(-1);
			SendFrequencyUpdateMessage();
		}
	}

	public void SetFrequencyIndexCheat(int frequencyIndex)
	{
		if (RecieverFrequency.AllFrequencies != null && frequencyIndex >= 0 && frequencyIndex < RecieverFrequency.AllFrequencies.Length)
		{
			SetFrequencyNumbers(RecieverFrequency.AllFrequencies[frequencyIndex].numbers);
		}
	}

	public void CheckSignal()
	{
		int num = 0;
		for (int i = 0; i < antennas.Length; i++)
		{
			Reciever_Antenna reciever_Antenna = antennas[i];
			if (reciever_Antenna != null)
			{
				float num2 = Vector3.Distance(reciever_Antenna.transform.position, transform.position);
				reciever_Antenna.toCloseToReciever = num2 < requiredAntennaDistance.minValue;
				reciever_Antenna.toFarAwayFromReciever = num2 > requiredAntennaDistance.maxValue;
				float num3 = Mathf.Abs(antennas[i].transform.localPosition.y - transform.localPosition.y);
				reciever_Antenna.onSameAltitudeAsReciever = num3 <= 1.3100001f;
				for (int j = 0; j < antennas.Length; j++)
				{
					Reciever_Antenna reciever_Antenna2 = antennas[j];
					if (!(reciever_Antenna2 == null) && i != j)
					{
						float num4 = Vector3.Distance(reciever_Antenna.transform.position, reciever_Antenna2.transform.position);
						reciever_Antenna.toCloseToOtherAntenna = num4 < minDistanceBetweenAntennas;
						if (reciever_Antenna.toCloseToOtherAntenna)
						{
							break;
						}
					}
				}
				if (reciever_Antenna.HasValidConnection)
				{
					num++;
				}
			}
			antennaPanels[i].RefreshUI(reciever_Antenna);
		}
		hasSignal = num == antennasRequired || Cheat.ForceRecieverToggle;
	}

	public bool AddAntenna(Reciever_Antenna antenna)
	{
		for (int i = 0; i < antennas.Length; i++)
		{
			if (antennas[i] == null)
			{
				antennas[i] = antenna;
				antenna.socketNumber = i;
				CheckSignal();
				return true;
			}
		}
		return false;
	}

	public void RemoveAntenna(Reciever_Antenna antenna)
	{
		for (int i = 0; i < antennas.Length; i++)
		{
			if (antennas[i] == antenna)
			{
				antennas[i] = null;
				CheckSignal();
				break;
			}
		}
	}

	public bool HasMaxAntennas()
	{
		for (int i = 0; i < antennas.Length; i++)
		{
			if (antennas[i] == null)
			{
				return false;
			}
		}
		return true;
	}

	public void UpdateRadarPointsFromChunkManager()
	{
		if (chunkManager == null)
		{
			chunkManager = ComponentManager<ChunkManager>.Value;
		}
		NoteBook value = ComponentManager<NoteBook>.Value;
		int num = 0;
		foreach (ChunkPointType item in typesToDisplay)
		{
			List<ChunkPoint> chunkPointsFromPointType = chunkManager.GetChunkPointsFromPointType(item);
			if (!chunkPointsFromPointType.ContainsItems() || value == null || !NoteBook.unlockedChunkPointType.Contains(item))
			{
				continue;
			}
			for (int i = 0; i < chunkPointsFromPointType.Count; i++)
			{
				ChunkPoint chunkPoint = chunkPointsFromPointType[i];
				if (num < radarPoints.Count)
				{
					radarPoints[num].chunkPoint = chunkPoint;
				}
				else
				{
					Reciever_Dot reciever_Dot = UnityEngine.Object.Instantiate(dotPrefab, dotParent);
					reciever_Dot.transform.localScale = dotPrefab.transform.localScale;
					reciever_Dot.chunkPoint = chunkPoint;
					radarPoints.Add(reciever_Dot);
				}
				num++;
			}
		}
		for (int j = 0; j < radarPoints.Count; j++)
		{
			Reciever_Dot reciever_Dot2 = radarPoints[j];
			if (j >= num + 1)
			{
				if (reciever_Dot2 != null && reciever_Dot2.gameObject != null)
				{
					UnityEngine.Object.Destroy(reciever_Dot2.gameObject);
				}
				radarPoints.RemoveAt(j);
				j--;
			}
		}
	}

	public List<Reciever_Dot> GetRecieverRadarDots()
	{
		return radarPoints;
	}

	private void HandleUI()
	{
		RefreshFrequencyIndexArrow();
		lightOff.SetActiveSafe(battery.BatterySlotIsEmpty || !battery.On);
		if (!battery.BatterySlotIsEmpty && battery.CanGiveElectricity && battery.On)
		{
			canvasSection.SetActiveSafe(value: true);
			if (!HasCorrectAltitude)
			{
				altitudeSection.SetActiveSafe(value: true);
				antennaSection.SetActiveSafe(value: false);
				radarSection.SetActiveSafe(value: false);
				numberSection.SetActiveSafe(value: false);
				lightOff.SetActiveSafe(value: true);
				lightGreen.SetActiveSafe(value: false);
				lightRed.SetActiveSafe(value: false);
			}
			else
			{
				bool flag = currentFrequency.IsValid();
				altitudeSection.SetActiveSafe(value: false);
				antennaSection.SetActiveSafe(!hasSignal);
				radarSection.SetActiveSafe(hasSignal);
				numberSection.SetActiveSafe(hasSignal);
				lightOff.SetActiveSafe(value: false);
				lightGreen.SetActiveSafe(hasSignal & flag);
				lightRed.SetActiveSafe(hasSignal && !flag);
				if (radarSection.activeSelf)
				{
					Vector2 vector = new Vector2(transform.position.x, transform.position.z);
					new Vector2(transform.forward.x, transform.forward.z);
					isCurrentlyShowingRadarDot = false;
					for (int i = 0; i < radarPoints.Count; i++)
					{
						Reciever_Dot reciever_Dot = radarPoints[i];
						if (reciever_Dot == null || reciever_Dot.chunkPoint == null || reciever_Dot.chunkPoint.IsCorrupt)
						{
							if (reciever_Dot != null && reciever_Dot.gameObject != null)
							{
								UnityEngine.Object.Destroy(reciever_Dot.gameObject);
							}
							radarPoints.RemoveAt(i);
							i--;
							continue;
						}
						reciever_Dot.SetTargetedByReciever(flag && reciever_Dot.chunkPoint.rule.ChunkPointType == currentFrequency.chunkPointType);
						Vector2 dirTwo = new Vector2(reciever_Dot.chunkPoint.worldPosition.x, reciever_Dot.chunkPoint.worldPosition.z) - vector;
						float angle = GetAngle(Vector2.up, dirTwo);
						_ = transform.eulerAngles;
						float num = 360f - transform.eulerAngles.y + angle;
						Vector3 vector2 = new Vector3(Mathf.Sin(num * (MathF.PI / 180f)), Mathf.Cos(num * (MathF.PI / 180f)), 0f);
						vector2 *= -1f;
						float magnitude = dirTwo.magnitude;
						float num2 = Mathf.Clamp01(magnitude / radarLength);
						float num3 = radarUIWidth / 2f;
						reciever_Dot.gameObject.SetActiveSafe(magnitude < radarNoticePointLength || reciever_Dot.chunkPoint.rule.isFrequencyPoint);
						if (reciever_Dot.gameObject.activeSelf)
						{
							isCurrentlyShowingRadarDot = true;
							reciever_Dot.SetLocalPosition(Vector3.zero + vector2 * (num3 * num2));
							reciever_Dot.SetLengthToPoint(magnitude);
							reciever_Dot.SetText(magnitude.ToString("F0") + "m");
						}
					}
				}
			}
			batterySlider.value = battery.NormalizedBatteryLeft;
		}
		else
		{
			canvasSection.SetActiveSafe(value: false);
			numberSection.SetActiveSafe(value: false);
			lightGreen.SetActiveSafe(value: false);
			lightRed.SetActiveSafe(value: false);
		}
	}

	private float GetAngle(Vector2 dirOne, Vector2 dirTwo)
	{
		float num = Vector2.SignedAngle(dirOne, dirTwo);
		if (num < 0f)
		{
			num = 360f + num;
		}
		return 360f - num;
	}

	private Reciever_Antenna GetAntennaFromBlockIndex(uint blockIndex)
	{
		Block blockByObjectIndex = BlockCreator.GetBlockByObjectIndex(blockIndex);
		if (blockByObjectIndex == null)
		{
			return null;
		}
		return blockByObjectIndex.GetComponent<Reciever_Antenna>();
	}

	private void IncrementFrequencyIndex()
	{
		frequencyNumberIndex.IncrementCycle(1, 0, currentFrequency.numbers.Length - 1);
	}

	private void IncrementCurrentFrequencyNumber(int increment)
	{
		if (frequencyNumberIndex < currentFrequency.numbers.Length)
		{
			currentFrequency.numbers[frequencyNumberIndex].IncrementCycle(increment, 0, 9);
		}
		RefreshFrequencyNumberText();
	}

	private void HandleFrequencyUpdateMessage(Message_Reciever_FrequencyUpdate msgFrequencyUpdate)
	{
		SetFrequencyNumbers(msgFrequencyUpdate.frequencyNumbers);
		SetFrequencyNumberIndex(msgFrequencyUpdate.frequencyIndex);
	}

	private void SetFrequencyNumberIndex(int number)
	{
		frequencyNumberIndex = number;
	}

	private void SetFrequencyNumbers(int[] numbers)
	{
		for (int i = 0; i < currentFrequency.numbers.Length; i++)
		{
			if (i < numbers.Length)
			{
				currentFrequency.numbers[i] = numbers[i];
			}
		}
		RefreshFrequencyNumberText();
	}

	private void RefreshFrequencyIndexArrow()
	{
		Vector3 vector = new Vector3(-0.3715f - 0.066f * (float)frequencyNumberIndex, frequencyArrow.localPosition.y, frequencyArrow.localPosition.z);
		if (frequencyArrow.localPosition != vector)
		{
			frequencyArrow.localPosition = Vector3.MoveTowards(frequencyArrow.localPosition, vector, Time.deltaTime * frequencyArrowSpeed);
		}
	}

	private void RefreshFrequencyNumberText()
	{
		if (!frequencyTextNumbers.ContainsItems())
		{
			return;
		}
		for (int i = 0; i < currentFrequency.numbers.Length; i++)
		{
			if (i < frequencyTextNumbers.Length)
			{
				frequencyTextNumbers[i].text = currentFrequency.numbers[i].ToString();
			}
		}
	}

	private RecieverFrequency GetCurrentMatchingFrequency()
	{
		for (int i = 0; i < RecieverFrequency.AllFrequencies.Length; i++)
		{
			RecieverFrequency recieverFrequency = RecieverFrequency.AllFrequencies[i];
			if (recieverFrequency != null && recieverFrequency.Equals(currentFrequency))
			{
				return recieverFrequency;
			}
		}
		return null;
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Reciever(RGDType.Block_Reciever, this, currentFrequency.numbers, frequencyNumberIndex);
	}

	public void Restore(RGD_Reciever rgdReciever)
	{
		if (rgdReciever.frequencyNumbers != null)
		{
			SetFrequencyNumbers(rgdReciever.frequencyNumbers);
		}
		SetFrequencyNumberIndex(rgdReciever.frequencyNumberIndex);
		if (rgdReciever.rgdBattery != null)
		{
			rgdReciever.rgdBattery.RestoreBattery(battery);
		}
		battery.On = rgdReciever.batteryOn;
		if (rgdReciever.antennaBlockIndexes == null)
		{
			return;
		}
		for (int i = 0; i < rgdReciever.antennaBlockIndexes.Length; i++)
		{
			int num = rgdReciever.antennaBlockIndexes[i];
			if (num == -1)
			{
				continue;
			}
			Block blockByObjectIndex = BlockCreator.GetBlockByObjectIndex((uint)num);
			if (blockByObjectIndex != null)
			{
				Reciever_Antenna component = blockByObjectIndex.GetComponent<Reciever_Antenna>();
				if (component != null)
				{
					component.ConnectToReciever(this);
				}
			}
		}
	}

	private void SendFrequencyUpdateMessage()
	{
		network.RPC(new Message_Reciever_FrequencyUpdate(Messages.Reciever_FrequencyUpdate, network.NetworkIDManager, ObjectIndex, currentFrequency.numbers, frequencyNumberIndex), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.Reciever_Antenna_Connect:
		{
			Message_Reciever_Antenna message_Reciever_Antenna = msg as Message_Reciever_Antenna;
			Reciever_Antenna antennaFromBlockIndex = GetAntennaFromBlockIndex(message_Reciever_Antenna.antennaBlockIndex);
			if (antennaFromBlockIndex != null)
			{
				return antennaFromBlockIndex.ConnectToReciever(this);
			}
			break;
		}
		case Messages.InteractableButtonPress:
		case Messages.InteractableButtonPress_Int:
			switch ((msg as Message_InteractableButtonPress).buttonIndex)
			{
			case 0:
				ButtonCyclePressed();
				return true;
			case 1:
				ButtonUpPress();
				return true;
			case 2:
				ButtonDownPress();
				return true;
			default:
				return false;
			}
		case Messages.Reciever_FrequencyUpdate:
		{
			Message_Reciever_FrequencyUpdate msgFrequencyUpdate = msg as Message_Reciever_FrequencyUpdate;
			HandleFrequencyUpdateMessage(msgFrequencyUpdate);
			return false;
		}
		case Messages.Battery_Insert:
		case Messages.Battery_Take:
		case Messages.Battery_Update:
		case Messages.Battery_OnOff:
			return battery.OnBatteryMessage(msg);
		}
		return false;
	}
}
