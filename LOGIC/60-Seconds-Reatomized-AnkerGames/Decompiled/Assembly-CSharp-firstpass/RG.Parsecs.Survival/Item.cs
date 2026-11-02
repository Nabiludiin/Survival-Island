using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Crafting/New Item", fileName = "New Item")]
public class Item : IItem, ISaveable
{
	private const int DEFAULT_DURABAILITY_USE = 1;

	[SerializeField]
	private ItemStaticData _staticData;

	[SerializeField]
	private ItemRuntimeData _runtimeData;

	public override IItemStaticData BaseStaticData => _staticData;

	public override IItemRuntimeData BaseRuntimeData => _runtimeData;

	public ItemStaticData StaticData => _staticData;

	public ItemRuntimeData RuntimeData => _runtimeData;

	public string ID => Guid;

	public override void CreateNewRuntimeData()
	{
		_runtimeData = new ItemRuntimeData();
	}

	private void OnEnable()
	{
		Register();
	}

	public virtual void SetDamage()
	{
		_runtimeData.Durability = 0;
		_runtimeData.IsDamaged = true;
	}

	public void Repair()
	{
		_runtimeData.IsDamaged = false;
		_runtimeData.Durability = _staticData.Durabilites[_runtimeData.Level - 1];
	}

	public override bool IsUpgradable()
	{
		if (_runtimeData.Level < _staticData.MaxLevel && !_runtimeData.IsDamaged)
		{
			return _runtimeData.IsAvailable;
		}
		return false;
	}

	public override void Upgrade()
	{
		_runtimeData.Level++;
		_runtimeData.Durability = _staticData.Durabilites[_runtimeData.Level - 1];
	}

	public override void Downgrade()
	{
		_runtimeData.Level--;
		if (_runtimeData.Level < 1)
		{
			_runtimeData.Level = 1;
		}
		if (_runtimeData.Durability > _staticData.Durabilites[_runtimeData.Level - 1])
		{
			_runtimeData.Durability = _staticData.Durabilites[_runtimeData.Level - 1];
		}
	}

	public CraftingOperation GetRepairCostsForCurrentLevel()
	{
		return _staticData.RepairCost[_runtimeData.Level - 1];
	}

	public override bool IsDamaged()
	{
		return RuntimeData.IsDamaged;
	}

	public override void Use()
	{
		UseItem(1);
	}

	public virtual void UseItem(int value)
	{
		_runtimeData.Durability -= value;
	}

	public override void InitializeWithDefaultData()
	{
		base.InitializeWithDefaultData();
		_runtimeData.Durability = _staticData.Durability;
		_runtimeData.IsDamaged = _staticData.IsDamaged;
	}

	public override void Remove()
	{
		_runtimeData.IsAvailable = false;
	}

	public override void Add()
	{
		if (_runtimeData.IsAvailable)
		{
			_runtimeData.Durability = _staticData.Durability;
			_runtimeData.IsDamaged = false;
		}
		else
		{
			InitializeWithDefaultData();
		}
		_runtimeData.IsAvailable = true;
	}

	public string Serialize()
	{
		return JsonUtility.ToJson(_runtimeData);
	}

	public void Deserialize(string jsonData)
	{
		JsonUtility.FromJsonOverwrite(jsonData, _runtimeData);
	}

	public void Register()
	{
		_saveEvent.RegisterListener(this);
	}

	public void Unregister()
	{
		_saveEvent.UnregisterListener(this);
	}

	public void ResetData()
	{
		InitializeWithDefaultData();
	}
}
