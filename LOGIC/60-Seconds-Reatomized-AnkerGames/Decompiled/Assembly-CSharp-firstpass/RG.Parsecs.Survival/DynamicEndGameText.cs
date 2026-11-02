using System;
using I2.Loc;
using RG.Core.Base;
using RG.Core.SaveSystem;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
[CreateAssetMenu(menuName = "60 Parsecs!/End Game Editor/Dynamic End Game Text", fileName = "New Dynamic Text")]
public class DynamicEndGameText : RGScriptableObject, ISaveable
{
	[Serializable]
	public class StaticEndGameTextData
	{
		[SerializeField]
		private LocalizedString _defaultValue = null;

		public LocalizedString DefaultValue => _defaultValue;
	}

	[Serializable]
	public class RuntimeEndGameTextData
	{
		[SerializeField]
		private LocalizedString _runtimeValue;

		public LocalizedString RuntimeValue
		{
			get
			{
				return _runtimeValue;
			}
			set
			{
				_runtimeValue = value;
			}
		}
	}

	[SerializeField]
	private SaveEvent _saveEvent;

	[SerializeField]
	private StaticEndGameTextData _staticData;

	[SerializeField]
	private RuntimeEndGameTextData _runtimeData;

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

	public StaticEndGameTextData StaticData => _staticData;

	public RuntimeEndGameTextData RuntimeData => _runtimeData;

	public string ID => Guid;

	public override string ToString()
	{
		return ID;
	}

	public override bool Equals(object other)
	{
		VisualsData visualsData = other as VisualsData;
		if (visualsData != null)
		{
			return ID.Equals(visualsData.ID);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ID.GetHashCode();
	}

	public void ResetToInitialState()
	{
		_runtimeData.RuntimeValue = _staticData.DefaultValue;
	}

	private void OnEnable()
	{
		Register();
	}

	private void OnDestroy()
	{
		Unregister();
	}

	public string Serialize()
	{
		return JsonUtility.ToJson(new LocalizedStringWrapper
		{
			Term = RuntimeData.RuntimeValue
		});
	}

	public void Deserialize(string jsonData)
	{
		LocalizedStringWrapper localizedStringWrapper = JsonUtility.FromJson<LocalizedStringWrapper>(jsonData);
		if (!string.IsNullOrEmpty(localizedStringWrapper.Term))
		{
			_runtimeData.RuntimeValue = localizedStringWrapper.Term;
		}
	}

	public void Register()
	{
		if ((bool)_saveEvent)
		{
			_saveEvent.RegisterListener(this);
		}
		else
		{
			Debug.Log($"DynamicEndGameText {name} has not connected Save Event");
		}
	}

	public void Unregister()
	{
		if ((bool)_saveEvent)
		{
			_saveEvent.UnregisterListener(this);
		}
	}

	public void ResetData()
	{
		ResetToInitialState();
	}
}
