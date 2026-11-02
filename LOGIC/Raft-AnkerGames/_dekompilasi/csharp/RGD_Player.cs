using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class RGD_Player : SerializableBehaviour
{
	public SerializableVector3 Position;

	public SerializableVector3 RaftOffset;

	public float rotY;

	public float hunger;

	public float bonusHunger;

	public float thirst;

	public float bonusThirst;

	public float health;

	public float bonusHealth;

	public float oxygen;

	public float hazmatSuitTimer;

	public bool dead;

	public RGD_PlayerInventory rgdInventory;

	public bool parentedToRaft;

	public int shiftCount;

	public RGD_Buff[] rgdBuffs;

	public RGD_Player(Network_Player player)
	{
		parentedToRaft = player.PersonController.HasRaftAsParent;
		if (parentedToRaft)
		{
			Position = new SerializableVector3(player.transform.localPosition);
		}
		else
		{
			Position = new SerializableVector3(player.transform.position);
		}
		rotY = player.transform.localEulerAngles.y;
		PlayerStats stats = player.Stats;
		hunger = ((stats.stat_hunger.Normal.statTarget.NormalValue > stats.stat_hunger.Normal.NormalValue) ? stats.stat_hunger.Normal.statTarget.NormalValue : stats.stat_hunger.Normal.NormalValue);
		bonusHunger = ((stats.stat_hunger.Bonus.statTarget.NormalValue > stats.stat_hunger.Bonus.NormalValue) ? stats.stat_hunger.Bonus.statTarget.NormalValue : stats.stat_hunger.Bonus.NormalValue);
		thirst = ((stats.stat_thirst.Normal.statTarget.NormalValue > stats.stat_thirst.Normal.NormalValue) ? stats.stat_thirst.Normal.statTarget.NormalValue : stats.stat_thirst.Normal.NormalValue);
		bonusThirst = ((stats.stat_thirst.Bonus.statTarget.NormalValue > stats.stat_thirst.Bonus.NormalValue) ? stats.stat_thirst.Bonus.statTarget.NormalValue : stats.stat_thirst.Bonus.NormalValue);
		health = stats.stat_health.NormalValue;
		bonusHealth = stats.stat_BonusHealth.NormalValue;
		oxygen = stats.stat_oxygen.NormalValue;
		if (player.HazmatSuit != null)
		{
			hazmatSuitTimer = player.HazmatSuit.Timer;
		}
		dead = player.PlayerScript.IsDead;
		if (player.Inventory != null)
		{
			rgdInventory = new RGD_PlayerInventory(player.Inventory);
		}
		shiftCount = ComponentManager<Network_Host>.Value.worldShiftManager.shiftCounter;
		RaftOffset = new SerializableVector3(player.transform.position - ComponentManager<Raft>.Value.transform.position);
		List<Buff> buffs = player.BuffManager.buffs;
		if (buffs == null || buffs.Count <= 0)
		{
			return;
		}
		rgdBuffs = new RGD_Buff[buffs.Count];
		for (int i = 0; i < rgdBuffs.Length; i++)
		{
			if (buffs[i] != null)
			{
				rgdBuffs[i] = new RGD_Buff(buffs[i]);
			}
		}
	}

	public void RestorePlayer(Network_Player player)
	{
		GameObject gameObject = GameObject.FindWithTag("RespawnPointBed");
		if (parentedToRaft)
		{
			player.transform.SetParent(SingletonGeneric<GameManager>.Singleton.lockedPivot);
			if (Position.Value == Vector3.positiveInfinity)
			{
				if (gameObject != null)
				{
					player.transform.position = gameObject.transform.position;
				}
				else
				{
					player.transform.localPosition = new Vector3(0f, 1.5f, 0f);
				}
			}
			else
			{
				player.transform.localPosition = Position.Value;
			}
		}
		else
		{
			player.transform.SetParent(null);
			WorldShiftManager worldShiftManager = ComponentManager<Network_Host>.Value.worldShiftManager;
			if (shiftCount != worldShiftManager.recievedShiftCount)
			{
				worldShiftManager.shiftCounter = worldShiftManager.recievedShiftCount;
				if (RaftOffset.Value == Vector3.positiveInfinity)
				{
					if (gameObject != null)
					{
						player.transform.position = gameObject.transform.position;
					}
					else
					{
						player.transform.SetParent(SingletonGeneric<GameManager>.Singleton.lockedPivot);
						player.transform.localPosition = new Vector3(0f, 1.5f, 0f);
					}
				}
				else
				{
					player.transform.position = ComponentManager<Raft>.Value.transform.position + RaftOffset.Value;
				}
			}
			else
			{
				player.transform.position = Position.Value;
			}
		}
		player.transform.localEulerAngles = new Vector3(0f, rotY, 0f);
		if (player.IsLocalPlayer)
		{
			player.SetToValidSpawnPoint();
		}
		_ = player.Stats;
		if (hunger != -1f)
		{
			player.Stats.stat_hunger.Normal.NormalValue = hunger;
		}
		if (bonusHunger != -1f)
		{
			player.Stats.stat_hunger.Bonus.NormalValue = bonusHunger;
		}
		if (thirst != -1f)
		{
			player.Stats.stat_thirst.Normal.NormalValue = thirst;
		}
		if (bonusThirst != -1f)
		{
			player.Stats.stat_thirst.Bonus.NormalValue = bonusThirst;
		}
		if (health != -1f)
		{
			player.Stats.stat_health.NormalValue = health;
		}
		if (bonusHealth != -1f)
		{
			player.Stats.stat_BonusHealth.NormalValue = bonusHealth;
		}
		if (oxygen != -1f)
		{
			player.Stats.stat_oxygen.NormalValue = oxygen;
		}
		if (rgdInventory != null)
		{
			rgdInventory.RestoreInventory(player.Inventory);
		}
		if (hazmatSuitTimer > 0f && !dead && player.HazmatSuit != null)
		{
			player.HazmatSuit.EquipNetworked(hazmatSuitTimer);
		}
		if (rgdBuffs != null && rgdBuffs.Length != 0)
		{
			for (int i = 0; i < rgdBuffs.Length; i++)
			{
				RGD_Buff rGD_Buff = rgdBuffs[i];
				if (rGD_Buff != null && !(rGD_Buff.NormalizedTimer < 0f) && rGD_Buff.UniqueBuffIndex >= 0)
				{
					player.BuffManager.AddBuff(BuffManager.GetBuffFromIndex(rGD_Buff.UniqueBuffIndex))?.SetNormalizedTimer(rGD_Buff.NormalizedTimer);
				}
			}
		}
		if (dead)
		{
			player.StartCoroutine(player.PlayerScript.KillLater(2f, replicating: true, useNetwork: true));
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Position", Position);
		info.AddValue("RotY", rotY);
		info.AddValue("ParentedToRaft", parentedToRaft);
		info.AddValue("Hunger", hunger);
		info.AddValue("BonusHunger", bonusHunger);
		info.AddValue("BonusThirst", bonusThirst);
		info.AddValue("Thirst", thirst);
		info.AddValue("Health", health);
		info.AddValue("BonusHealth", bonusHealth);
		info.AddValue("Oxygen", oxygen);
		info.AddValue("Dead", dead);
		info.AddValue("Inventory", rgdInventory);
		info.AddValue("ShiftCount", shiftCount);
		info.AddValue("RaftOffset", RaftOffset);
		info.AddValue("Buffs", rgdBuffs);
	}

	public RGD_Player(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "Position":
				Position = (SerializableVector3)item.Value;
				break;
			case "RotY":
				rotY = (float)item.Value;
				break;
			case "ParentedToRaft":
				parentedToRaft = (bool)item.Value;
				break;
			case "Hunger":
				hunger = (float)item.Value;
				break;
			case "BonusHunger":
				bonusHunger = (float)item.Value;
				break;
			case "Thirst":
				thirst = (float)item.Value;
				break;
			case "BonusThirst":
				bonusThirst = (float)item.Value;
				break;
			case "Health":
				health = (float)item.Value;
				break;
			case "BonusHealth":
				bonusHealth = (float)item.Value;
				break;
			case "Oxygen":
				oxygen = (float)item.Value;
				break;
			case "Dead":
				dead = (bool)item.Value;
				break;
			case "Inventory":
				rgdInventory = (RGD_PlayerInventory)item.Value;
				break;
			case "ShiftCount":
				shiftCount = (int)item.Value;
				break;
			case "RaftOffset":
				RaftOffset = (SerializableVector3)item.Value;
				break;
			case "Buffs":
				rgdBuffs = (RGD_Buff[])item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		Position = new SerializableVector3(Vector3.positiveInfinity);
		RaftOffset = new SerializableVector3(Vector3.positiveInfinity);
		rotY = 0f;
		parentedToRaft = false;
		hunger = -1f;
		bonusHunger = -1f;
		thirst = -1f;
		bonusThirst = -1f;
		health = -1f;
		bonusHealth = -1f;
		oxygen = -1f;
		dead = false;
		rgdInventory = null;
		shiftCount = -1;
		rgdBuffs = null;
	}
}
