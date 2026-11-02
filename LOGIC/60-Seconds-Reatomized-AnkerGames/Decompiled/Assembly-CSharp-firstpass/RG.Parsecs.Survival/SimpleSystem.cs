using I2.Loc;
using RG.Core.SaveSystem;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Systems/Simple System")]
public class SimpleSystem : ShipSystem, ISaveable
{
	[SerializeField]
	private SaveEvent _saveEvent;

	private const string PREFIX = "system";

	[SerializeField]
	private StaticSimpleData _staticData;

	[SerializeField]
	private RuntimeSimpleData _runtimeData;

	public override LocalizedString Name => _staticData.Name;

	public override string Id => ID;

	public override int SystemLevel
	{
		get
		{
			return _runtimeData.SystemLevel;
		}
		set
		{
			_runtimeData.SystemLevel = value;
		}
	}

	public override bool IsUpgradable => _runtimeData.SystemLevel < _staticData.MaxLevel;

	public override Sprite Icon => _staticData.Icon;

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

	public override bool IsFocusedByDesign => false;

	public override bool AvailableInCraftingDuringExpedition => _staticData.AvailableInCraftingDuringExpedition;

	public string ID => Guid;

	public override string GuidPrefix => "system";

	private void OnEnable()
	{
		Register();
	}

	private void OnDestroy()
	{
		Unregister();
	}

	public override CraftingOperation GetUpgradeCostForCurrentLevel()
	{
		return _staticData.UpgradeCosts[_runtimeData.SystemLevel - 1];
	}

	public override void Upgrade()
	{
		_runtimeData.SystemLevel++;
	}

	public override void SetDisabledReason(LocalizedString title, LocalizedString reason)
	{
		if (!string.IsNullOrEmpty(title.mTerm) || !string.IsNullOrEmpty(reason.mTerm))
		{
			Debug.LogWarning(name + " System does not implements displaying blocked reason.");
		}
	}

	public override void Downgrade()
	{
		_runtimeData.SystemLevel--;
	}

	public override int MaxLevel()
	{
		return _staticData.MaxLevel;
	}

	public string Serialize()
	{
		return JsonUtility.ToJson(_runtimeData);
	}

	public void Deserialize(string jsonData)
	{
		_runtimeData = JsonUtility.FromJson<RuntimeSimpleData>(jsonData);
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
		_runtimeData = new RuntimeSimpleData();
		_runtimeData.IsActive = false;
		_runtimeData.SystemLevel = 1;
	}
}
