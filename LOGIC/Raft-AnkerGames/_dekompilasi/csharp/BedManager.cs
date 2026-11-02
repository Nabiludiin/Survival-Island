using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.AzureSky;

public class BedManager : MonoBehaviour_Network, SceneEventInterface
{
	public static Bed RespawnPointBed;

	[Range(0f, 24f)]
	public float canSleepAfterTime;

	[Range(0f, 24f)]
	public float wakeUpTime;

	private static List<Bed> allBeds = new List<Bed>();

	private static Raft_Network network = null;

	private static AzureSkyController skyController;

	private static Raft raft;

	private static float _canSleepAfterTime;

	private static float _wakeUpTime;

	private static bool slumbering;

	public static bool RespawnAvailable => allBeds.Count > 0;

	public static bool Slumbering => slumbering;

	private void Awake()
	{
		ComponentManager<BedManager>.Value = this;
		_canSleepAfterTime = canSleepAfterTime;
		_wakeUpTime = wakeUpTime;
		RespawnPointBed = GameObject.FindWithTag("RespawnPointBed").GetComponent<Bed>();
	}

	private void Start()
	{
		raft = ComponentManager<Raft>.Value;
		skyController = ComponentManager<AzureSkyController>.Value;
		network = ComponentManager<Raft_Network>.Value;
		if (Raft_Network.IsHost && GameManager.IsInNewGame)
		{
			NetworkUpdateManager.SetIndexForBehaviour(this);
			NetworkUpdateManager.AddBehaviour(this);
		}
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			allBeds.Clear();
			slumbering = false;
		}
	}

	private void Update()
	{
		if (Raft_Network.IsHost && AllPlayersSleeping() && BedTime() && !slumbering)
		{
			network.RPC(new Message_BedManager_Slumber(Messages.SlumberTime, this, raft.IsAnchored), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			StartCoroutine(Slumber(raft.IsAnchored));
		}
	}

	private void OnValidate()
	{
		if (wakeUpTime > 12f)
		{
			wakeUpTime = 12f;
		}
		if (canSleepAfterTime < 12f)
		{
			canSleepAfterTime = 12f;
		}
	}

	public void Button_Respawn()
	{
		Network_Player localPlayer = network.GetLocalPlayer();
		Message_NetworkBehaviour message = new Message_NetworkBehaviour(Messages.Respawn_Start, localPlayer);
		if (Raft_Network.IsHost)
		{
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			if (localPlayer.RessurectComponent.BeingCarried)
			{
				localPlayer.RessurectComponent.CarriedByPlayer.RessurectComponent.StopCarryingPlayer(manipulatePosition: false);
			}
			localPlayer.PlayerScript.StartRespawn(FindClosestBedToPlayer(localPlayer), clearInventory: true);
		}
		else
		{
			localPlayer.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public static void AddBed(Bed bed)
	{
		allBeds.Add(bed);
	}

	public static void RemoveBed(Bed bed)
	{
		allBeds.Remove(bed);
	}

	public static Bed FindClosestBedToPlayer(Network_Player player)
	{
		if (allBeds.Count <= 0)
		{
			return RespawnPointBed;
		}
		float num = float.MaxValue;
		Bed result = RespawnPointBed;
		for (int i = 0; i < allBeds.Count; i++)
		{
			Bed bed = allBeds[i];
			if (!bed.Busy)
			{
				float num2 = Vector3.Distance(bed.transform.position, player.transform.position);
				if (num2 < num)
				{
					num = num2;
					result = bed;
				}
			}
		}
		return result;
	}

	public static Bed GetBedFromIndex(uint objectIndex)
	{
		return allBeds.Find((Bed b) => b.ObjectIndex == objectIndex);
	}

	public static bool BedTime()
	{
		if (skyController == null)
		{
			return false;
		}
		float hour = skyController.timeOfDay.hour;
		if (hour < 12f && hour < _wakeUpTime)
		{
			return true;
		}
		if (hour > 12f && hour >= _canSleepAfterTime)
		{
			return true;
		}
		return false;
	}

	public static float GetNormalizedNightTime()
	{
		float num = 24f - _canSleepAfterTime;
		float num2 = 24f - (24f - _wakeUpTime);
		float num3 = num + num2;
		float num4 = 0f;
		float hour = skyController.timeOfDay.hour;
		if (hour < 12f && hour < _wakeUpTime)
		{
			num4 = _wakeUpTime - hour;
		}
		else if (hour > 12f && hour >= _canSleepAfterTime)
		{
			num4 = num2 + (24f - hour);
		}
		if (num4 == 0f)
		{
			return 0f;
		}
		float num5 = Mathf.Clamp01(num4 / num3);
		if (num5 >= 0.5f)
		{
			num5 = 1f - num5;
		}
		return Helper.MapValue(0f, 0.5f, num5, 0f, 1f);
	}

	public static bool AllPlayersSleeping()
	{
		if (network.remoteUsers.Count == 0)
		{
			return false;
		}
		foreach (Network_Player value in network.remoteUsers.Values)
		{
			if (!value.BedComponent.Sleeping)
			{
				return false;
			}
		}
		return true;
	}

	private static IEnumerator Slumber(bool raftIsAnchored)
	{
		slumbering = true;
		FadePanel fadePanel = SingletonGeneric<GameManager>.Singleton.sleepFadePanel;
		fadePanel.StartCoroutine(fadePanel.FadeToAlpha(1f, 1f));
		yield return new WaitForSeconds(2f);
		fadePanel.StopAllCoroutines();
		fadePanel.StartCoroutine(fadePanel.FadeToAlpha(0f, 0.5f));
		float hour = skyController.timeOfDay.hour;
		float num = 24f - _canSleepAfterTime + _wakeUpTime;
		float num2 = ((!(hour > _canSleepAfterTime)) ? (_wakeUpTime - hour) : (24f - hour + _wakeUpTime));
		if (hour >= _canSleepAfterTime && hour <= 24f)
		{
			WorldManager.DayCounter++;
		}
		float num3 = num2 / num;
		float num4 = skyController.timeOfDay.dayCycle / 24f * num2 * 60f;
		Network_Player localPlayer = network.GetLocalPlayer();
		if (GameModeValueManager.GetCurrentGameModeValue().gameMode != GameMode.Creative)
		{
			Bed currentBed = localPlayer.BedComponent.CurrentBed;
			PlayerStats stats = network.GetLocalPlayer().Stats;
			if (currentBed != null)
			{
				stats.stat_health.Value += currentBed.HealthRegen * num3;
				float amount = currentBed.ThirstDecay * num3;
				stats.stat_thirst.RemoveValue(amount);
				float amount2 = currentBed.HungerDecay * num3;
				stats.stat_hunger.RemoveValue(amount2);
			}
		}
		skyController.timeOfDay.GotoTime(_wakeUpTime);
		localPlayer.PlantManager.ForwardTime(num4);
		CookingStand[] networkdIDs = NetworkIDManager.GetNetworkdIDs<CookingStand>();
		if (networkdIDs != null)
		{
			CookingStand[] array = networkdIDs;
			foreach (CookingStand cookingStand in array)
			{
				if (cookingStand == null || cookingStand.blockCookingStand == null || cookingStand.blockCookingStand.fuel == null || !cookingStand.blockCookingStand.fuel.IsBurning())
				{
					continue;
				}
				float burnSecondsLeft = cookingStand.blockCookingStand.fuel.GetBurnSecondsLeft();
				float amount3 = Mathf.Clamp(num4, 0f, burnSecondsLeft);
				float num5 = 0f;
				if (cookingStand.blockCookingStand.cookingSlots != null)
				{
					CookingSlot[] cookingSlots = cookingStand.blockCookingStand.cookingSlots;
					foreach (CookingSlot cookingSlot in cookingSlots)
					{
						if (!(cookingSlot == null) && cookingSlot.IsBusy && !cookingSlot.IsComplete)
						{
							if (cookingSlot.CookingTimeLeft >= num5)
							{
								num5 = cookingSlot.CookingTimeLeft;
							}
							cookingSlot.IncrementTimer(amount3);
						}
					}
				}
				float amount4 = ((burnSecondsLeft < num5) ? burnSecondsLeft : num5);
				cookingStand.blockCookingStand.fuel.IncrementTimer(amount4);
			}
		}
		foreach (Bed allBed in allBeds)
		{
			allBed.DettachPlayer();
		}
		slumbering = false;
	}

	public override RGD Serialize_Save()
	{
		return new RGD_NetworkBehaviour(RGDType.NetworkBehaviour_BedManager, this);
	}

	public override void Serialize_Create()
	{
		AddMessage(new Message_NetworkBehaviour(Messages.CreateBedManager, this));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.SlumberTime)
		{
			Message_BedManager_Slumber message_BedManager_Slumber = msg as Message_BedManager_Slumber;
			StartCoroutine(Slumber(message_BedManager_Slumber.raftIsAnchored));
			return false;
		}
		return false;
	}
}
