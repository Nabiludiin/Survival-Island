using I2.Loc;
using RG.Core.Base;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.Survival;

public abstract class IItem : RGScriptableObject, ILocalizationParamsManager
{
	[SerializeField]
	protected SaveEvent _saveEvent;

	protected const string ITEM_NAME = "ITEM_NAME";

	public abstract IItemStaticData BaseStaticData { get; }

	public abstract IItemRuntimeData BaseRuntimeData { get; }

	public virtual void InitializeWithDefaultData()
	{
		CreateNewRuntimeData();
		BaseRuntimeData.Level = BaseStaticData.Level;
		if (!(this is ConsumableRemedium))
		{
			BaseRuntimeData.IsAvailable = BaseStaticData.IsAvailable;
		}
	}

	public abstract void CreateNewRuntimeData();

	public abstract bool IsUpgradable();

	public abstract void Upgrade();

	public abstract void Downgrade();

	public CraftingOperation GetCreateCost()
	{
		return BaseStaticData.CreateCost;
	}

	public CraftingOperation GetUpgradeCostForCurrentLevel()
	{
		return BaseStaticData.UpgradeCosts[BaseRuntimeData.Level - 1];
	}

	public CraftingOperation GetRecyclingCostForCurrentLevel()
	{
		return BaseStaticData.RecyclingCosts[BaseRuntimeData.Level - 1];
	}

	public bool IsMaxLevel()
	{
		return BaseRuntimeData.Level == BaseStaticData.MaxLevel;
	}

	public abstract void Use();

	public abstract void Remove();

	public abstract void Add();

	public virtual void Lock()
	{
		BaseRuntimeData.Lock = true;
	}

	public virtual void Unlock()
	{
		BaseRuntimeData.Lock = false;
	}

	public virtual bool IsLockable()
	{
		return !BaseRuntimeData.Lock;
	}

	public virtual bool IsDamaged()
	{
		return false;
	}

	public void RegisterToI2Loc()
	{
		if (!LocalizationManager.ParamManagers.Contains(this))
		{
			LocalizationManager.ParamManagers.Add(this);
			LocalizationManager.LocalizeAll(Force: true);
		}
	}

	public void UnregisterFromI2Loc()
	{
		LocalizationManager.ParamManagers.Remove(this);
	}

	public string GetParameterValue(string param)
	{
		if (param == "ITEM_NAME")
		{
			return BaseStaticData.Name;
		}
		return null;
	}
}
