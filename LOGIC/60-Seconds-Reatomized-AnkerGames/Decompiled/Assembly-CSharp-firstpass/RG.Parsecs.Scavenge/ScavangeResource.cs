using System;
using System.Collections.Generic;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[CreateAssetMenu(fileName = "ScavangeResource", menuName = "Scavenge/ScavangeResource")]
public class ScavangeResource : IItem
{
	public List<Resource> resources;

	public IntRange amount;

	public PlayerResources resourcesFile;

	[SerializeField]
	private ItemStaticData _staticData;

	public override IItemStaticData BaseStaticData => _staticData;

	public override IItemRuntimeData BaseRuntimeData => null;

	public override void Add()
	{
		resourcesFile.AddResource(resources.RandomElement(), amount.RandomValue);
	}

	public override void CreateNewRuntimeData()
	{
		foreach (Resource resource in resources)
		{
			resourcesFile.SetResource(resource, 0);
		}
	}

	public override void Downgrade()
	{
	}

	public override bool IsLockable()
	{
		throw new NotImplementedException();
	}

	public override bool IsUpgradable()
	{
		throw new NotImplementedException();
	}

	public override void Lock()
	{
		throw new NotImplementedException();
	}

	public override void Remove()
	{
	}

	public override void Unlock()
	{
		throw new NotImplementedException();
	}

	public override void Upgrade()
	{
	}

	public override void Use()
	{
	}
}
