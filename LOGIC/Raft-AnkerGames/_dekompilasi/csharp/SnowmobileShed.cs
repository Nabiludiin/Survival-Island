using System;
using FMODUnity;
using Steamworks;
using UnityEngine;

public class SnowmobileShed : MonoBehaviour_ID_Network
{
	private Raft_Network _network;

	[SerializeField]
	private Snowmobile snowmobilePrefab;

	[SerializeField]
	private Transform snowmobileParent;

	[SerializeField]
	private Transform spawnPoint;

	public EventReference er_respawn;

	public Snowmobile spawnedSnowmobile;

	private Raft_Network Network
	{
		get
		{
			if (_network == null)
			{
				_network = ComponentManager<Raft_Network>.Value;
			}
			return _network;
		}
	}

	private void OnSnowmobileReset(Snowmobile snowmobile, bool viaWater)
	{
		if (snowmobile == spawnedSnowmobile)
		{
			int integerValue = (viaWater ? 1 : 0);
			Message_NetworkBehaviour_ID_Int message = new Message_NetworkBehaviour_ID_Int(Messages.SnowmobileShed_Reset, Network.NetworkIDManager, ObjectIndex, integerValue);
			if (Raft_Network.IsHost)
			{
				if (ResetSnowmobileToShed(viaWater))
				{
					Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
			else
			{
				Network.SendP2P(Network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		if (viaWater)
		{
			AchievementHandler.UnlockAchievement(AchievementType.ach_snowmobileIsNotABoat);
		}
	}

	public void SpawnSnowmobileNetwork()
	{
		if (Raft_Network.IsHost)
		{
			CreateSnowmobile(SaveAndLoad.GetUniqueObjectIndex(), NetworkUpdateManager.GetUniqueBehaviourIndex(), spawnPoint.position, spawnPoint.rotation);
			Network.RPC(new Message_Snowmobile_Spawn(Messages.SnowmobileShed_Spawn, Network.NetworkIDManager, ObjectIndex, spawnedSnowmobile.ObjectIndex, spawnedSnowmobile.BehaviourIndex), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public void Restore(RGD_Snowmobile rgdSnowmobile)
	{
		if (rgdSnowmobile != null)
		{
			bool flag = rgdSnowmobile.Position == Vector3.zero;
			CreateSnowmobile((uint)rgdSnowmobile.mobileObjectIndex, (uint)rgdSnowmobile.mobileBehaviourIndex, flag ? spawnPoint.position : (rgdSnowmobile.Position + Vector3.up), flag ? spawnPoint.rotation : Quaternion.Euler(rgdSnowmobile.Rotation));
			spawnedSnowmobile.RestoreSeats(rgdSnowmobile);
		}
	}

	private void CreateSnowmobile(uint objectIndex, uint behaviourIndex, Vector3 position, Quaternion rotation)
	{
		if (spawnedSnowmobile != null)
		{
			DestroySnowmobile();
		}
		spawnedSnowmobile = UnityEngine.Object.Instantiate(snowmobilePrefab, position, rotation, snowmobileParent);
		spawnedSnowmobile.ObjectIndex = objectIndex;
		spawnedSnowmobile.BehaviourIndex = behaviourIndex;
		NetworkIDManager.AddNetworkID(spawnedSnowmobile, typeof(Snowmobile));
		NetworkIDManager.AddNetworkIDTick(spawnedSnowmobile);
		Snowmobile snowmobile = spawnedSnowmobile;
		snowmobile.OnSnowmobileReset = (Action<Snowmobile, bool>)Delegate.Combine(snowmobile.OnSnowmobileReset, new Action<Snowmobile, bool>(OnSnowmobileReset));
	}

	private void DestroySnowmobile()
	{
		if (spawnedSnowmobile != null)
		{
			UnityEngine.Object.Destroy(spawnedSnowmobile.gameObject);
			spawnedSnowmobile = null;
		}
	}

	private bool ResetSnowmobileToShed(bool viaWater = false)
	{
		if (spawnedSnowmobile != null)
		{
			RuntimeManager.PlayOneShot(er_respawn, spawnedSnowmobile.transform.position);
			spawnedSnowmobile.MakeAllPlayersLeave();
			spawnedSnowmobile.transform.position = spawnPoint.position;
			spawnedSnowmobile.transform.rotation = spawnPoint.rotation;
			spawnedSnowmobile.ResetToDefaults();
			spawnedSnowmobile.transform.SetParentSafe(snowmobileParent);
			spawnedSnowmobile.PlayImpactSound(1f, networked: false);
			if (viaWater)
			{
				AchievementHandler.UnlockAchievement(AchievementType.ach_snowmobileIsNotABoat);
			}
			return true;
		}
		return false;
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.SnowmobileShed_Spawn:
		{
			Message_Snowmobile_Spawn message_Snowmobile_Spawn = msg as Message_Snowmobile_Spawn;
			CreateSnowmobile(message_Snowmobile_Spawn.mobileObjectIndex, message_Snowmobile_Spawn.mobileBehaviourIndex, spawnPoint.position, spawnPoint.rotation);
			return false;
		}
		case Messages.SnowmobileShed_Reset:
		{
			Message_NetworkBehaviour_ID_Int message_NetworkBehaviour_ID_Int = msg as Message_NetworkBehaviour_ID_Int;
			bool viaWater = false;
			if (message_NetworkBehaviour_ID_Int != null)
			{
				viaWater = message_NetworkBehaviour_ID_Int.IntegerValue == 1;
			}
			if (Raft_Network.IsHost)
			{
				return ResetSnowmobileToShed(viaWater);
			}
			ResetSnowmobileToShed(viaWater);
			return false;
		}
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
