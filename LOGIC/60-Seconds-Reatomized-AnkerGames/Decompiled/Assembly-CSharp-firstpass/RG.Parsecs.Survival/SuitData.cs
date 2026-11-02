using I2.Loc;
using RG.Core.SaveSystem;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Systems/Suit/Suit Data")]
public class SuitData : ShipSystem, ISaveable
{
	[SerializeField]
	private SaveEvent _saveEvent;

	private const string SYSTEM_ID = "system_suit";

	[SerializeField]
	private StaticSuitData _staticData;

	[SerializeField]
	private RuntimeSuitData _runtimeData;

	public string ID => Guid;

	public int SuitLevel
	{
		get
		{
			return _runtimeData.SuitLevel;
		}
		set
		{
			_runtimeData.SuitLevel = value;
		}
	}

	public override bool IsFocusedByDesign => false;

	public override string Id => ID;

	public override int SystemLevel
	{
		get
		{
			return _runtimeData.SuitLevel;
		}
		set
		{
			_runtimeData.SuitLevel = value;
		}
	}

	public override bool IsUpgradable => _runtimeData.SuitLevel < _staticData.MaxLevel;

	public override Sprite Icon
	{
		get
		{
			Sprite sprite = ContentManager.GetSprite(_staticData.Icon.name);
			if (sprite != null)
			{
				return sprite;
			}
			return _staticData.Icon;
		}
	}

	public override SystemEvent UpgradeEvent => _staticData.UpgradeEvent;

	public override bool IsAvailable
	{
		get
		{
			return _runtimeData.IsActive;
		}
		set
		{
			_runtimeData.IsActive = value;
		}
	}

	public StaticSuitData StaticData => _staticData;

	public RuntimeSuitData RuntimeData => _runtimeData;

	public override bool AvailableInCraftingDuringExpedition => _staticData.AvailableInCraftingDuringExpedition;

	public override LocalizedString Name => _staticData.Name;

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
		_runtimeData = new RuntimeSuitData();
		_runtimeData.IsActive = true;
		_runtimeData.SuitLevel = 1;
	}

	private void OnEnable()
	{
		Register();
	}

	public override CraftingOperation GetUpgradeCostForCurrentLevel()
	{
		return _staticData.UpgradeCosts[_runtimeData.SuitLevel - 1];
	}

	public override void Upgrade()
	{
		_runtimeData.SuitLevel++;
	}

	public override void Downgrade()
	{
		_runtimeData.SuitLevel--;
	}

	public override int MaxLevel()
	{
		return _staticData.MaxLevel;
	}

	public override void SetDisabledReason(LocalizedString title, LocalizedString reason)
	{
		if (!string.IsNullOrEmpty(title.mTerm) || !string.IsNullOrEmpty(reason.mTerm))
		{
			Debug.LogWarning("Suit System does not implements displaying blocked reason.");
		}
	}
}
