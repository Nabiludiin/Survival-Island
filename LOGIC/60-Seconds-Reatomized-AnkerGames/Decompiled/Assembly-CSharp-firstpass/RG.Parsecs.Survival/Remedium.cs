using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Crafting/New Remedium", fileName = "New Remedium")]
public class Remedium : IItem, ISaveable
{
	[SerializeField]
	private RemediumStaticData _staticData;

	[SerializeField]
	private IItemRuntimeData _runtimeData;

	private string _id;

	public virtual RemediumStaticData StaticData => _staticData;

	public virtual IItemRuntimeData RuntimeData => _runtimeData;

	public override IItemStaticData BaseStaticData => _staticData;

	public override IItemRuntimeData BaseRuntimeData => _runtimeData;

	public string ID => Guid;

	public override void CreateNewRuntimeData()
	{
		_runtimeData = new IItemRuntimeData();
	}

	private void OnEnable()
	{
		Register();
	}

	public override bool IsUpgradable()
	{
		if (_runtimeData.Level < _staticData.MaxLevel)
		{
			return _runtimeData.IsAvailable;
		}
		return false;
	}

	public override void Upgrade()
	{
		_runtimeData.Level++;
	}

	public override void Downgrade()
	{
		_runtimeData.Level--;
	}

	public override void Use()
	{
		_runtimeData.IsAvailable = false;
	}

	public override void InitializeWithDefaultData()
	{
		base.InitializeWithDefaultData();
		_runtimeData.IsAvailable = _staticData.IsAvailable;
		_runtimeData.Level = _staticData.Level;
	}

	public override void Remove()
	{
		_runtimeData.IsAvailable = false;
	}

	public override void Add()
	{
		if (!_runtimeData.IsAvailable)
		{
			InitializeWithDefaultData();
		}
		_runtimeData.IsAvailable = true;
	}

	public InfluencesList GetInfluencesForCurrentLevel()
	{
		return _staticData.InfluencesLevel[_runtimeData.Level - 1];
	}

	public string Serialize()
	{
		return InnerSerialize();
	}

	protected virtual string InnerSerialize()
	{
		return JsonUtility.ToJson(_runtimeData);
	}

	public void Deserialize(string jsonData)
	{
		InnerDeserialize(jsonData);
	}

	protected virtual void InnerDeserialize(string jsonData)
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
