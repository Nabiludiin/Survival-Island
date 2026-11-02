using UnityEngine;

public class TriggerBoxAchievementUnlocker : LandmarkItem
{
	public AchievementType achievementType;

	public BoxCollider[] triggers;

	public ulong triggersHitFlags;

	private int lastSetBits;

	private void Start()
	{
		if (triggers.Length > 64)
		{
			Debug.LogError($"Too many triggers in {this}! Max 64 allowed", gameObject);
		}
	}

	private void Update()
	{
		int num = 0;
		for (int i = 0; i < triggers.Length; i++)
		{
			if (Physics.CheckBox(triggers[i].transform.TransformPoint(triggers[i].center), triggers[i].size / 2f, triggers[i].transform.rotation, (int)LayerMasks.MASK_Players | (int)LayerMasks.MASK_RemotePlayer, QueryTriggerInteraction.Ignore))
			{
				SetBit(ref triggersHitFlags, i, value: true);
			}
			if (BitIsSet(triggersHitFlags, i))
			{
				num++;
			}
		}
		if (num > lastSetBits)
		{
			lastSetBits = num;
			CheckAchievement(num);
		}
	}

	private void CheckAchievement(int setBits)
	{
		if (setBits >= triggers.Length)
		{
			AchievementHandler.UnlockAchievement(achievementType);
		}
	}

	public override RGD_LandmarkItem GetSave()
	{
		return new RGD_TriggerBoxAchievementUnlocker(this);
	}

	public void Restore(RGD_TriggerBoxAchievementUnlocker rgd)
	{
		triggersHitFlags = rgd.triggersHitFlags;
		int num = 0;
		for (int i = 0; i < triggers.Length; i++)
		{
			if (BitIsSet(triggersHitFlags, i))
			{
				num++;
			}
		}
		CheckAchievement(num);
	}

	private static void SetBit(ref ulong flags, int bit, bool value)
	{
		if (value)
		{
			flags |= (ulong)(1L << bit);
		}
		else
		{
			flags &= (ulong)(~(1L << bit));
		}
	}

	private static bool BitIsSet(ulong flags, int bit)
	{
		return (flags & (ulong)(1L << bit)) != 0;
	}
}
