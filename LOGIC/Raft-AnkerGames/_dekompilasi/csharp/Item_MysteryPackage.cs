using System;
using FMODUnity;
using UnityEngine;

public class Item_MysteryPackage : UseableItem
{
	[SerializeField]
	private Item_Base itemMysteryPackage;

	[SerializeField]
	private SO_MysteryPackageLoot so_loot;

	[SerializeField]
	private ParticleSystem openParticles;

	[SerializeField]
	private StudioEventEmitter emitter_openPackage;

	private Inventory_ResearchTable researchTable;

	protected override void Start()
	{
		base.Start();
		if (playerNetwork.IsLocalPlayer)
		{
			researchTable = ComponentManager<Inventory_ResearchTable>.Value;
			OnChannelFinished = (Action)Delegate.Combine(OnChannelFinished, new Action(ChannelFinished));
			OnReleaseUseButton = (Action)Delegate.Combine(OnReleaseUseButton, new Action(UseButtonReleased));
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (playerNetwork.IsLocalPlayer)
		{
			OnChannelFinished = (Action)Delegate.Remove(OnChannelFinished, new Action(ChannelFinished));
			OnReleaseUseButton = (Action)Delegate.Remove(OnReleaseUseButton, new Action(UseButtonReleased));
		}
	}

	private void UseButtonReleased()
	{
		if (emitter_openPackage != null)
		{
			emitter_openPackage.Stop();
		}
		StopParticles();
	}

	private void ChannelFinished()
	{
		if (playerNetwork.IsLocalPlayer)
		{
			ExtractLoot();
			playerNetwork.Inventory.RemoveItem(itemMysteryPackage.UniqueName, 1);
			playerNetwork.Animator.ReselectAnimation();
		}
	}

	public override bool ChannelItem()
	{
		PlayParticles();
		return base.ChannelItem();
	}

	protected override void OnStartChanneling()
	{
		base.OnStartChanneling();
		if (emitter_openPackage != null)
		{
			emitter_openPackage.PlaySafe();
		}
	}

	private void PlayParticles()
	{
		if (openParticles != null)
		{
			openParticles.transform.SetParent(playerNetwork.transform.parent);
			openParticles.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
			openParticles.transform.position = transform.position;
			openParticles.PlaySafe();
		}
	}

	private void StopParticles()
	{
		if (openParticles != null)
		{
			openParticles.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmitting);
		}
	}

	private void ExtractLoot()
	{
		Item_Base item_Base = so_loot.GetRandomItemFromPossibles();
		if (!(item_Base == null))
		{
			bool flag = false;
			Item_Base baseSkinItem = item_Base.settings_recipe.baseSkinItem;
			if (baseSkinItem != null && baseSkinItem.UniqueIndex != item_Base.UniqueIndex && !baseSkinItem.settings_recipe.Learned && !baseSkinItem.settings_recipe.LearnedFromBeginning && baseSkinItem.settings_recipe.HiddenInResearchTable)
			{
				flag = true;
			}
			if (flag)
			{
				item_Base = baseSkinItem;
			}
			if (!(item_Base == null))
			{
				researchTable.LearnItemNetworked(item_Base, playerNetwork.steamID);
			}
		}
	}
}
