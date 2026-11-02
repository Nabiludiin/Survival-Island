using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
	private static SO_Buff[] allBuffAssets;

	[HideInInspector]
	public Network_Player networkPlayer;

	[SerializeField]
	public List<Buff> buffs = new List<Buff>();

	[SerializeField]
	private Network_Entity entity;

	private BuffUpdateResult result = new BuffUpdateResult();

	private Network_Host networkHost;

	private bool isAttachedToPlayer;

	public BuffUpdateResult BuffResult => result;

	public static SO_Buff GetBuffFromIndex(int uniqueBuffIndex)
	{
		if (uniqueBuffIndex < 0)
		{
			return null;
		}
		if (allBuffAssets == null)
		{
			allBuffAssets = Resources.LoadAll<SO_Buff>("BuffAssets");
		}
		SO_Buff[] array = allBuffAssets;
		foreach (SO_Buff sO_Buff in array)
		{
			if (sO_Buff.UniqueBuffIndex == uniqueBuffIndex)
			{
				return sO_Buff;
			}
		}
		return null;
	}

	protected virtual void Start()
	{
		networkPlayer = GetComponent<Network_Player>();
		networkHost = ComponentManager<Network_Host>.Value;
		isAttachedToPlayer = this is PlayerBuffManager;
	}

	protected virtual void OnDestroy()
	{
		networkPlayer = null;
	}

	public void UpdateBuffs()
	{
		if (entity != null)
		{
			HandleBuffs();
		}
	}

	public virtual Buff AddBuff(SO_Buff buffAsset)
	{
		if (buffAsset == null)
		{
			return null;
		}
		if (buffAsset.ResetDurationOnSameBuffType)
		{
			for (int i = 0; i < buffs.Count; i++)
			{
				Buff buff = buffs[i];
				if (buff != null && buff.Asset.BuffType == buffAsset.BuffType)
				{
					buff.ResetDuration();
				}
			}
		}
		if (!buffAsset.CanStack && BuffTypeIsAlreadyActive(buffAsset.BuffType))
		{
			return null;
		}
		Buff buff2 = (isAttachedToPlayer ? buffAsset.GetPlayerBuff() : buffAsset.GetEnemyBuff());
		buff2.OnBuffApplied(this, buffAsset);
		buffs.Add(buff2);
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		for (int j = 0; j < buffs.Count; j++)
		{
			if (buffs[j].Asset.BuffType == BuffType.Movement_Ground)
			{
				flag = true;
			}
			else if (buffs[j].Asset.BuffType == BuffType.Movement_Water)
			{
				flag2 = true;
			}
			else if (buffs[j].Asset.BuffType == BuffType.Oxygen)
			{
				flag3 = true;
			}
			else if (buffs[j].Asset.BuffType == BuffType.DeathPrevention)
			{
				flag4 = true;
			}
		}
		if (flag & flag2 & flag3 & flag4)
		{
			AchievementHandler.UnlockAchievement(AchievementType.ach_allFoodBuffs);
		}
		return buff2;
	}

	public virtual void RemoveBuffDuration(BuffType buffType)
	{
		foreach (Buff buff in buffs)
		{
			if (buff.Asset.BuffType == buffType)
			{
				buff.SetDurationComplete();
				break;
			}
		}
	}

	public void ClearBuffs()
	{
		if (buffs != null)
		{
			for (int i = 0; i < buffs.Count; i++)
			{
				buffs[i]?.SetDurationComplete();
			}
		}
	}

	private void HandleBuffs()
	{
		result.Clear();
		for (int i = 0; i < buffs.Count; i++)
		{
			Buff buff = buffs[i];
			if (buff == null || !buff.UpdateTimer())
			{
				buff?.OnBuffRemoved(this);
				buffs.RemoveAt(i);
				i--;
			}
			else
			{
				buff.UpdateBuff(result);
			}
		}
		if (result.damageTaken != 0)
		{
			result.damageTaken = Mathf.FloorToInt((float)result.damageTaken * GameModeValueManager.GetCurrentGameModeValue().specialEffectsVariables.damageOverTimeMultiplier);
			if (result.damageTaken < 0)
			{
				entity.Damage(result.damageTaken, Vector3.zero, Vector3.up, EntityType.None, isLocal: false);
			}
			else if (Raft_Network.IsHost)
			{
				networkHost.DamageEntity(entity, null, result.damageTaken, entity.transform.position, Vector3.up, EntityType.None);
			}
		}
	}

	private bool BuffTypeIsAlreadyActive(BuffType buffType)
	{
		for (int i = 0; i < buffs.Count; i++)
		{
			Buff buff = buffs[i];
			if (buff != null && !(buff.Asset == null) && buff.Asset.BuffType == buffType)
			{
				return true;
			}
		}
		return false;
	}
}
