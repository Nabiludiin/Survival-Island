using System.Collections.Generic;
using RG.Core.Base;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Crafting/New Player Resources", fileName = "New Player Resources")]
public class PlayerResources : RGScriptableObject, ISaveable
{
	private const string ERROR_MESSAGE = "There is no {0} in the resource list";

	[Tooltip("Event to which data will be save and load")]
	[SerializeField]
	private SaveEvent _saveEvent;

	[SerializeField]
	private List<GameResource> _startResources;

	[SerializeField]
	private int _maxResourcesAmount = 999;

	[SerializeField]
	private List<GameResource> _runtimeResources;

	private const string PLAYER_RESOURCE_PREFIX = "playerResource";

	public SaveEvent SaveEvent
	{
		get
		{
			return _saveEvent;
		}
		set
		{
			_saveEvent = value;
		}
	}

	public List<GameResource> RuntimeResources => _runtimeResources;

	public string ID => Guid;

	public override string GuidPrefix => "playerResource";

	private void OnEnable()
	{
		Register();
	}

	private void OnDestroy()
	{
		_runtimeResources.Clear();
		Unregister();
	}

	public void AddResources(List<GameResource> resources)
	{
		for (int i = 0; i < resources.Count; i++)
		{
			for (int j = 0; j < _runtimeResources.Count; j++)
			{
				if (_runtimeResources[j].Resource.Equals(resources[i].Resource))
				{
					_runtimeResources[j].Amount += resources[i].Amount;
					_runtimeResources[j].Amount = Mathf.Clamp(_runtimeResources[j].Amount, 0, _maxResourcesAmount);
				}
			}
		}
	}

	public void ClearResources()
	{
		for (int i = 0; i < _runtimeResources.Count; i++)
		{
			_runtimeResources[i].Amount = 0;
		}
	}

	public int GetResourceValue(Resource s)
	{
		for (int i = 0; i < _runtimeResources.Count; i++)
		{
			if (_runtimeResources[i].Resource.Equals(s))
			{
				return _runtimeResources[i].Amount;
			}
		}
		throw new UnityException($"There is no {s.Name} in the resource list");
	}

	public int GetResourceValueForCraftingUI(Resource resource)
	{
		CraftingData craftingSystem = CraftingManager.GetCraftingSystem();
		GameResource gameResourceById = GetGameResourceById(resource.ID);
		if (craftingSystem.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction != EPlannedCraftingAction.NONE && craftingSystem.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction != EPlannedCraftingAction.RECYCLE)
		{
			CraftingOperation currentCraftingOperation = craftingSystem.RuntimeData.GetCurrentCraftingOperation();
			if (currentCraftingOperation == null)
			{
				return gameResourceById.CurrentAmount;
			}
			for (int i = 0; i < currentCraftingOperation.Resources.Count; i++)
			{
				if (currentCraftingOperation.Resources[i].Resource.ID == gameResourceById.Resource.ID)
				{
					return gameResourceById.CurrentAmount + currentCraftingOperation.Resources[i].Amount;
				}
			}
		}
		return gameResourceById.CurrentAmount;
	}

	public void SetResource(Resource r, int amount)
	{
		for (int i = 0; i < _runtimeResources.Count; i++)
		{
			if (_runtimeResources[i].Resource.Equals(r))
			{
				_runtimeResources[i].Amount = amount;
				_runtimeResources[i].Amount = Mathf.Clamp(_runtimeResources[i].Amount, 0, _maxResourcesAmount);
				return;
			}
		}
		throw new UnityException($"There is no {r.Name} in the resource list");
	}

	private void SetResource(string id, int amount)
	{
		for (int i = 0; i < _runtimeResources.Count; i++)
		{
			if (_runtimeResources[i].Resource.ID == id)
			{
				_runtimeResources[i].Amount = amount;
				_runtimeResources[i].Amount = Mathf.Clamp(_runtimeResources[i].Amount, 0, _maxResourcesAmount);
				return;
			}
		}
		throw new UnityException($"There is no {id} in the resource list");
	}

	public int AddResource(Resource r, int amount)
	{
		for (int i = 0; i < _runtimeResources.Count; i++)
		{
			if (_runtimeResources[i].Resource.Equals(r))
			{
				int amount2 = _runtimeResources[i].Amount;
				_runtimeResources[i].Amount += amount;
				if (_runtimeResources[i].Amount > _maxResourcesAmount)
				{
					_runtimeResources[i].Amount = Mathf.Clamp(_runtimeResources[i].Amount, 0, _maxResourcesAmount);
					return 999 - amount2;
				}
				return amount;
			}
		}
		throw new UnityException($"There is no {r.Name} in the resource list");
	}

	public void RemoveResource(Resource r, int amount)
	{
		for (int i = 0; i < _runtimeResources.Count; i++)
		{
			if (_runtimeResources[i].Resource.Equals(r))
			{
				_runtimeResources[i].Amount -= amount;
				_runtimeResources[i].Amount = Mathf.Clamp(_runtimeResources[i].Amount, 0, _maxResourcesAmount);
				return;
			}
		}
		throw new UnityException($"There is no {r.Name} in the resource list");
	}

	public int RemoveResourceAndGetRemovedAmount(Resource r, int amount)
	{
		for (int i = 0; i < _runtimeResources.Count; i++)
		{
			if (_runtimeResources[i].Resource.Equals(r))
			{
				_runtimeResources[i].Amount -= amount;
				if (_runtimeResources[i].Amount < 0)
				{
					int result = amount + _runtimeResources[i].Amount;
					_runtimeResources[i].Amount = 0;
					return result;
				}
				return amount;
			}
		}
		throw new UnityException($"There is no {r.Name} in the resource list");
	}

	public bool CanLock(List<GameResource> resources)
	{
		for (int i = 0; i < resources.Count; i++)
		{
			if (!CanLock(resources[i]))
			{
				return false;
			}
		}
		return true;
	}

	public bool CanLockForCraftingUI(List<GameResource> resources)
	{
		for (int i = 0; i < resources.Count; i++)
		{
			if (!CanLockForCraftingUI(resources[i]))
			{
				return false;
			}
		}
		return true;
	}

	public bool CanLockForCraftingUI(GameResource resource)
	{
		CraftingData craftingSystem = CraftingManager.GetCraftingSystem();
		GameResource gameResourceById = GetGameResourceById(resource.Resource.ID);
		if (craftingSystem.RuntimeData.PlannedCraftingActionData.Item != null)
		{
			if (craftingSystem.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction != EPlannedCraftingAction.NONE && craftingSystem.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction != EPlannedCraftingAction.RECYCLE)
			{
				CraftingOperation currentCraftingOperation = craftingSystem.RuntimeData.GetCurrentCraftingOperation();
				if (currentCraftingOperation == null)
				{
					return gameResourceById.CanLock(resource.Amount);
				}
				for (int i = 0; i < currentCraftingOperation.Resources.Count; i++)
				{
					if (currentCraftingOperation.Resources[i].Resource.ID == resource.Resource.ID)
					{
						return gameResourceById.CanLock(resource.Amount - currentCraftingOperation.Resources[i].Amount);
					}
				}
			}
		}
		else if (craftingSystem.RuntimeData.PlannedCraftingActionData.System != null && craftingSystem.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction == EPlannedCraftingAction.UPGRADE)
		{
			CraftingOperation currentCraftingOperation2 = craftingSystem.RuntimeData.GetCurrentCraftingOperation();
			if (currentCraftingOperation2 == null)
			{
				return gameResourceById.CanLock(resource.Amount);
			}
			for (int j = 0; j < currentCraftingOperation2.Resources.Count; j++)
			{
				if (currentCraftingOperation2.Resources[j].Resource.ID == resource.Resource.ID)
				{
					return gameResourceById.CanLock(resource.Amount - currentCraftingOperation2.Resources[j].Amount);
				}
			}
		}
		return gameResourceById.CanLock(resource.Amount);
	}

	public GameResource GetGameResourceById(string id)
	{
		for (int i = 0; i < _runtimeResources.Count; i++)
		{
			if (_runtimeResources[i].Resource.ID == id)
			{
				return _runtimeResources[i];
			}
		}
		return null;
	}

	public bool RemoveResources(List<GameResource> resources, bool safe = true)
	{
		if (safe && !CanLock(resources))
		{
			return false;
		}
		for (int i = 0; i < resources.Count; i++)
		{
			RemoveResource(resources[i].Resource, resources[i].Amount);
		}
		return true;
	}

	public bool CanLock(GameResource resource)
	{
		return GetGameResourceById(resource.Resource.ID).CanLock(resource.Amount);
	}

	public bool CanLock(Resource resource, int amount)
	{
		return GetGameResourceById(resource.ID).CanLock(amount);
	}

	public void Lock(GameResource resource)
	{
		GetGameResourceById(resource.Resource.ID).Lock(resource.Amount);
	}

	public void Unlock(GameResource resource)
	{
		GetGameResourceById(resource.Resource.ID).Unlock(resource.Amount);
	}

	public string Serialize()
	{
		PlayerResourceWrapper playerResourceWrapper = new PlayerResourceWrapper
		{
			ResourceAmount = new List<int>(),
			ResourceID = new List<string>()
		};
		for (int i = 0; i < _runtimeResources.Count; i++)
		{
			playerResourceWrapper.ResourceAmount.Add(_runtimeResources[i].Amount);
			playerResourceWrapper.ResourceID.Add(_runtimeResources[i].Resource.ID);
		}
		return JsonUtility.ToJson(playerResourceWrapper);
	}

	public void Deserialize(string jsonData)
	{
		PlayerResourceWrapper playerResourceWrapper = JsonUtility.FromJson<PlayerResourceWrapper>(jsonData);
		for (int i = 0; i < playerResourceWrapper.ResourceAmount.Count; i++)
		{
			SetResource(playerResourceWrapper.ResourceID[i], playerResourceWrapper.ResourceAmount[i]);
		}
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
		_runtimeResources.Clear();
		for (int i = 0; i < _startResources.Count; i++)
		{
			_runtimeResources.Add((GameResource)_startResources[i].Clone());
		}
	}
}
