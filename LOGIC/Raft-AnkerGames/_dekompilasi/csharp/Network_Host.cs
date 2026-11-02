using Steamworks;
using UnityEngine;
using UnityEngine.AzureSky;

public class Network_Host : MonoBehaviour_NetworkTick
{
	public Network_Water water;

	public Raft raft;

	public RaftCollisionManager raftCollisionManager;

	[HideInInspector]
	public AzureSkyController skyController;

	[SerializeField]
	private float lerpSkySpeed = 4f;

	[Space(20f)]
	[Header("References")]
	public ObjectSpawnerManager spawnerManager;

	public WorldManager worldManager;

	public WorldShiftManager worldShiftManager;

	public TextWriterManager textWriterManager;

	public WaterRoughnessManager waterRoughnessManager;

	public WeatherManager weatherManager;

	private Raft_Network network;

	private Network_Host_Entities hostEntities;

	private void Awake()
	{
		ComponentManager<Network_Host>.Value = this;
	}

	private void Start()
	{
		if (water == null)
		{
			water = ComponentManager<Network_Water>.Value;
		}
		skyController = ComponentManager<AzureSkyController>.Value;
		network = ComponentManager<Raft_Network>.Value;
		weatherManager = ComponentManager<WeatherManager>.Value;
		hostEntities = ComponentManager<Network_Host_Entities>.Value;
		if (waterRoughnessManager == null)
		{
			waterRoughnessManager = weatherManager.GetComponent<WaterRoughnessManager>();
		}
		network.hostNetwork = this;
		if (Raft_Network.IsHost && GameManager.IsInNewGame)
		{
			NetworkUpdateManager.SetIndexForBehaviour(this);
			NetworkUpdateManager.AddBehaviour(this);
		}
	}

	private void Update()
	{
		if (Raft_Network.IsHost)
		{
			UpdateNetworkTick();
		}
	}

	public void DamageEntity(Network_Entity entity, Transform hitTransform, float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageInflictorEntityType, SO_Buff buffAsset = null)
	{
		bool flag = entity.entityType == EntityType.Player && damageInflictorEntityType == EntityType.Player;
		float num = damage;
		if (!GameManager.FriendlyFire & flag)
		{
			return;
		}
		if (flag)
		{
			float num2 = 3f;
			float num3 = 2f;
			if (hitTransform != null)
			{
				if (hitTransform.tag == "Entity_Head")
				{
					num *= num2;
				}
				else if (hitTransform.tag == "Entity_Chest")
				{
					num *= num3;
				}
			}
		}
		if (entity.entityType == EntityType.Enemy && GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.negateOutgoingPlayerDamage)
		{
			num = 0f;
		}
		if (entity.entityType == EntityType.Player && damageInflictorEntityType == EntityType.Enemy)
		{
			num *= GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.damageTakenMultiplier;
		}
		else if (entity.entityType == EntityType.Enemy && damageInflictorEntityType == EntityType.Player)
		{
			num *= GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.outgoingDamageMultiplierPVE;
		}
		if (hostEntities == null)
		{
			hostEntities = ComponentManager<Network_Host_Entities>.Value;
		}
		Message_NetworkEntity_Damage message = new Message_NetworkEntity_Damage(Messages.DamageEntity, network.NetworkIDManager, hostEntities.ObjectIndex, entity.ObjectIndex, num, hitPoint, hitNormal, damageInflictorEntityType, buffAsset);
		if (Raft_Network.IsHost)
		{
			entity.Damage(num, hitPoint, hitNormal, damageInflictorEntityType, isLocal: true, buffAsset);
			if (num >= 0f)
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else if (num >= 0f)
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_NetworkBehaviour(RGDType.NetworkBehaviour_NetworkHost, this);
	}

	public override void Serialize_Create()
	{
		AddMessage(new Message_NetworkBehaviour(Messages.CreateHostNetwork, this));
		AddMessage(new Message_WorldInfo_Create(Messages.WorldInfo_Create, this, water, skyController, worldShiftManager.shiftCounter));
		AddMessage(new Message_Raft(Messages.Raft_Create, this, raft, raftCollisionManager));
		AddMessage(new Message_WorldManager_CreateLandmarks(Messages.WorldManager_CreateLandmarks, this));
		AddMessage(new Message_WeatherManager_SetWeather(Messages.WeatherManager_SetWeather, this, weatherManager.GetCurrentWeather(), instant: true));
	}

	public override void Serialize_Update()
	{
		AddMessage(new Message_WorldInfo_Update(Messages.WorldInfo_Update, this, water, skyController, waterRoughnessManager));
		AddMessage(new Message_Raft(Messages.Raft_Update, this, raft, raftCollisionManager));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.WorldInfo_Create:
		{
			Message_WorldInfo_Create message_WorldInfo_Create = msg as Message_WorldInfo_Create;
			water.transform.position = new Vector3(message_WorldInfo_Create.waterXpos, 0f, message_WorldInfo_Create.waterZpos);
			water.SetWaterTime(message_WorldInfo_Create.waterTime);
			skyController.timeOfDay.GotoTime(message_WorldInfo_Create.timeOfDay);
			WorldManager.DayCounter = message_WorldInfo_Create.dayCounter;
			GameManager.TimePlayed = message_WorldInfo_Create.timePlayedSeconds;
			message_WorldInfo_Create.rgdResearchInfo.RestoreResearchTableWorld(ComponentManager<Inventory_ResearchTable>.Value);
			if (message_WorldInfo_Create.rgdRecieverFrequencies != null)
			{
				message_WorldInfo_Create.rgdRecieverFrequencies.RestoreFrequencies();
			}
			if (!Raft_Network.IsHost)
			{
				worldShiftManager.recievedShiftCount = message_WorldInfo_Create.shiftCount;
			}
			return false;
		}
		case Messages.WorldInfo_Update:
		{
			Message_WorldInfo_Update message_WorldInfo_Update = msg as Message_WorldInfo_Update;
			water.SetNetworkWaterTime(message_WorldInfo_Update.WaterTime);
			waterRoughnessManager.SetWeightMessage(message_WorldInfo_Update.GetWaterWeightList());
			if (Mathf.Abs(skyController.timeOfDay.hour - message_WorldInfo_Update.TimeOfDay) > 1f)
			{
				skyController.timeOfDay.GotoTime(message_WorldInfo_Update.TimeOfDay);
			}
			else
			{
				skyController.timeOfDay.GotoTime(Mathf.Lerp(skyController.timeOfDay.hour, message_WorldInfo_Update.TimeOfDay, Time.deltaTime * lerpSkySpeed));
			}
			return false;
		}
		case Messages.Raft_Create:
		{
			Message_Raft message_Raft = msg as Message_Raft;
			Transform transform = raft.transform;
			Vector3 position = (raftCollisionManager.transform.position = new Vector3(message_Raft.PositionX, 0f, message_Raft.PositionZ));
			transform.position = position;
			raftCollisionManager.transform.eulerAngles = new Vector3(0f, message_Raft.RotationY, 0f);
			return false;
		}
		case Messages.Raft_Update:
		{
			Message_Raft networkProperties = msg as Message_Raft;
			if (raft != null)
			{
				raft.SetNetworkProperties(networkProperties);
			}
			return false;
		}
		case Messages.WorldManager_ResetWorldToCenter:
		{
			Message_WorldShiftManager_WorldShift message_WorldShiftManager_WorldShift = msg as Message_WorldShiftManager_WorldShift;
			worldShiftManager.ResetToCenter(message_WorldShiftManager_WorldShift.offsetX, message_WorldShiftManager_WorldShift.offsetZ);
			if (!Raft_Network.IsHost)
			{
				worldShiftManager.shiftCounter = message_WorldShiftManager_WorldShift.newShiftCount;
			}
			return false;
		}
		case Messages.WorldManager_CreateLandmarks:
		{
			Message_WorldManager_CreateLandmarks msg2 = msg as Message_WorldManager_CreateLandmarks;
			worldManager.RestoreLandmarks(msg2);
			return false;
		}
		case Messages.WeatherManager_SetWeather:
		{
			Message_WeatherManager_SetWeather message_WeatherManager_SetWeather = msg as Message_WeatherManager_SetWeather;
			weatherManager.SetWeather((UniqueWeatherType)message_WeatherManager_SetWeather.weatherIndex, message_WeatherManager_SetWeather.instant);
			return false;
		}
		default:
			return false;
		}
	}
}
