using System;
using UnityEngine;

public class PlayerBuffManager : BuffManager
{
	[HideInInspector]
	public ScreenEffectManager screenEffectManager;

	[SerializeField]
	public Network_Player player;

	protected override void Start()
	{
		base.Start();
		screenEffectManager = ComponentManager<ScreenEffectManager>.Value;
		if (player == null)
		{
			player = GetComponentInChildren<Network_Player>();
		}
		HazmatSuit hazmatSuit = player.HazmatSuit;
		hazmatSuit.OnHazmatEquipped = (Action)Delegate.Combine(hazmatSuit.OnHazmatEquipped, new Action(OnHazmatEquipped));
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		player = null;
	}

	private void OnHazmatEquipped()
	{
		if (buffs == null || buffs.Count == 0)
		{
			return;
		}
		for (int i = 0; i < buffs.Count; i++)
		{
			Buff buff = buffs[i];
			if (buff != null && buff.Asset.BuffType == BuffType.Radiation)
			{
				buff.OnBuffRemoved(this);
				buffs.RemoveAt(i);
				i--;
			}
		}
	}

	public override Buff AddBuff(SO_Buff buffAsset)
	{
		if (buffAsset != null && buffAsset.BuffType == BuffType.Radiation && player != null && player.HazmatSuit.Equipped)
		{
			return null;
		}
		return base.AddBuff(buffAsset);
	}
}
