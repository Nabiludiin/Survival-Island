using System;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[CreateAssetMenu(fileName = "ScavangeCharacter", menuName = "Scavenge/ScavangeCharacter")]
public class ScavengeCharacterItem : IItem
{
	public Character character;

	public int amount = 1;

	public CharacterList characterList;

	[SerializeField]
	private ItemStaticData _staticData;

	public override IItemStaticData BaseStaticData => _staticData;

	public override IItemRuntimeData BaseRuntimeData => null;

	public override void Add()
	{
		characterList.AddCharToList(character);
	}

	public override void CreateNewRuntimeData()
	{
		characterList.ResetCharFromList();
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
		characterList.RemoveCharFromList(character);
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
