using System.Collections;
using FMODUnity;
using Steamworks;
using UnityEngine;

public class Sprinkler : MonoBehaviour_ID_Network
{
	public Battery battery;

	public Tank waterTank;

	[SerializeField]
	private Transform findGrassPlotCenterPoint;

	[SerializeField]
	private ParticleSystem waterParticles;

	[SerializeField]
	private float waterInterval;

	[SerializeField]
	private int batteryUsesPerPlot;

	[SerializeField]
	private int waterUsesPerPlot;

	[SerializeField]
	private LayerMask waterableLayer;

	public EventReference er_sprinker;

	[SerializeField]
	private int numberOfSprays = 1;

	[SerializeField]
	private Interval_Float sprayInterval;

	private Raft_Network network;

	private PlantManager plantManager;

	private float waterTimer;

	public bool CanWater
	{
		get
		{
			if (waterTank.CurrentTankAmount > 0f)
			{
				return battery.CanGiveElectricity;
			}
			return false;
		}
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		plantManager = ComponentManager<Network_Player>.Value.PlantManager;
		waterInterval += Random.Range(1f, 3f);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkIDManager.RemoveNetworkID(this);
	}

	private void Update()
	{
		if (Raft_Network.IsHost)
		{
			HandleWaterTimer();
		}
	}

	private void HandleWaterTimer()
	{
		if (CanWater)
		{
			waterTimer += Time.deltaTime;
			if (waterTimer >= waterInterval)
			{
				waterTimer = 0f;
				WaterNearbyPlots();
			}
		}
	}

	private void Button_PlayWaterEffect()
	{
		if (!Application.isPlaying)
		{
			Debug.Log("Can't play water effect in editor mode. Hit play button first");
		}
		else
		{
			StartCoroutine(PlayWaterEffects());
		}
	}

	public void OnBlockPlaced()
	{
		NetworkIDManager.AddNetworkID(this);
	}

	private void WaterNearbyPlots()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		bool flag = false;
		Collider[] array = Physics.OverlapSphere(findGrassPlotCenterPoint.position, 2.1f, waterableLayer);
		for (int i = 0; i < array.Length; i++)
		{
			if (!CanWater)
			{
				break;
			}
			if (Mathf.Abs(array[i].transform.position.y - transform.position.y) <= 1.21f)
			{
				Cropplot componentInChildren = array[i].GetComponentInChildren<Cropplot>();
				if (componentInChildren != null && componentInChildren.SlotsNeedWater())
				{
					WaterCropplot(componentInChildren);
					flag = true;
				}
			}
		}
		if (flag)
		{
			StartCoroutine(PlayWaterEffects());
			network.RPC(new Message_NetworkBehaviour_ID(Messages.Sprinkler_Water, network.NetworkIDManager, ObjectIndex), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private IEnumerator PlayWaterEffects()
	{
		for (int i = 0; i < numberOfSprays; i++)
		{
			if (waterParticles != null)
			{
				waterParticles.Play();
			}
			RuntimeManager.PlayOneShot(er_sprinker, waterParticles.transform.position);
			yield return new WaitForSeconds(sprayInterval.GetRandomValue());
		}
	}

	private void WaterCropplot(Cropplot cropplot)
	{
		plantManager.WaterCropplot(cropplot, playSound: false);
		Message_WaterCrop message = new Message_WaterCrop(Messages.PlantManager_WaterPlant, plantManager, cropplot, playSound: false);
		network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		waterTank.ModifyTankRPC(-waterUsesPerPlot, null);
		battery.RemoveBatteryUsesNetworked(batteryUsesPerPlot);
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Block_Sprinkler(RGDType.Block_Sprinkler, GetComponent<Block>(), this);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.Sprinkler_Water:
			StartCoroutine(PlayWaterEffects());
			return false;
		case Messages.Tank:
			return waterTank.OnTankModifyMessage(msg as Message_Tank);
		case Messages.Tank_SetValue:
			waterTank.OnTankSetValueMessage(msg as Message_Tank);
			break;
		case Messages.Battery_Insert:
		case Messages.Battery_Take:
		case Messages.Battery_Update:
			return battery.OnBatteryMessage(msg);
		}
		return base.Deserialize(msg, remoteID);
	}
}
