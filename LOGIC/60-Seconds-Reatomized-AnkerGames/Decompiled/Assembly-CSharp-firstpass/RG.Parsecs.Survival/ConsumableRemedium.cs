using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Crafting/New Consumable Remedium", fileName = "New Consumable Remedium")]
public class ConsumableRemedium : IItem, ISaveable
{
	[SerializeField]
	private ConsumableStaticData _staticData;

	[SerializeField]
	private ConsumableRuntimeData _runtimeData;

	private const float WHOLE_CONSUMABLE_AMOUNT = 1f;

	public override IItemStaticData BaseStaticData => _staticData;

	public override IItemRuntimeData BaseRuntimeData => _runtimeData;

	public ConsumableStaticData StaticData => _staticData;

	public ConsumableRuntimeData RuntimeData => _runtimeData;

	public string ID => Guid;

	public override void CreateNewRuntimeData()
	{
		_runtimeData = new ConsumableRuntimeData();
	}

	private void OnEnable()
	{
		Register();
	}

	private void OnDestroy()
	{
		Unregister();
	}

	public override bool IsUpgradable()
	{
		return false;
	}

	public override void Upgrade()
	{
		throw new UnityException("Cannot use Upgrade method in consumable object");
	}

	public override void Downgrade()
	{
		throw new UnityException("Cannot use Downgrade method in consumable object");
	}

	public InfluencesList GetInfluencesForCurrentLevel()
	{
		return _staticData.InfluencesLevel[_runtimeData.Level - 1];
	}

	public void Add(float amount)
	{
		_runtimeData.Amount += amount;
		if (_staticData.UseMaxAmount)
		{
			_runtimeData.Amount = Mathf.Min(_runtimeData.Amount, _staticData.MaxAmount);
		}
	}

	public void Remove(float amount)
	{
		_runtimeData.Amount -= amount;
		_runtimeData.Amount = Mathf.Max(0f, _runtimeData.Amount);
	}

	public float RemoveAndGetRemovedAmount(float amount)
	{
		float num = _runtimeData.Amount - _runtimeData.PlannedConsumption;
		if (num < amount)
		{
			_runtimeData.Amount -= num;
			return num;
		}
		_runtimeData.Amount -= amount;
		return amount;
	}

	public void SetAmountBasedOnPlannedConsumption()
	{
		_runtimeData.Amount -= _runtimeData.PlannedConsumption;
		_runtimeData.PlannedConsumption = 0f;
	}

	public override void Use()
	{
		_runtimeData.Amount -= _staticData.ConsumeRatio;
	}

	public override void InitializeWithDefaultData()
	{
		base.InitializeWithDefaultData();
		_runtimeData.Amount = _staticData.Amount;
	}

	public override void Remove()
	{
		Remove(1f);
	}

	public override void Add()
	{
		Add(1f);
	}

	public override void Lock()
	{
		Lock(1f);
	}

	public override void Unlock()
	{
		Unlock(1f);
	}

	public override bool IsLockable()
	{
		return CanLock(1f);
	}

	public bool CanLock(float amount)
	{
		return _runtimeData.Amount - _runtimeData.PlannedConsumption >= amount;
	}

	public void Lock(float amount)
	{
		if (CanLock(amount))
		{
			_runtimeData.PlannedConsumption += amount;
		}
	}

	public void Unlock(float amount)
	{
		if (_runtimeData.PlannedConsumption - amount >= 0f)
		{
			_runtimeData.PlannedConsumption -= amount;
		}
	}

	public string Serialize()
	{
		return JsonUtility.ToJson(_runtimeData);
	}

	public void Deserialize(string jsonData)
	{
		_runtimeData = JsonUtility.FromJson<ConsumableRuntimeData>(jsonData);
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
