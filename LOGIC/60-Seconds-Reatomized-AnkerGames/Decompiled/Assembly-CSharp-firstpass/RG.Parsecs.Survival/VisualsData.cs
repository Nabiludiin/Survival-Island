using System;
using RG.Core.Base;
using RG.Core.SaveSystem;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
[CreateAssetMenu(menuName = "60 Parsecs!/Visuals/Visual Data", fileName = "New Visual Data")]
public class VisualsData : RGScriptableObject, ISaveable
{
	[Serializable]
	public class StaticVisualsData
	{
		[Tooltip("The SoundSlot used to play sounds when clicking on the visual")]
		[SerializeField]
		private SoundSlot _soundSlot;

		[Tooltip("This should be the tooltip term set at the VisualsTooltipContent.")]
		[SerializeField]
		private TooltipTerm _tooltipTerm;

		[Tooltip("The default id of the visual that should be displayed. If set to null (none), the Visitor will be inactive by default.")]
		[SerializeField]
		private VisualId _defaultVisualToDisplay;

		public VisualId DefaultVisualToDisplay => _defaultVisualToDisplay;

		public SoundSlot SoundSlot => _soundSlot;

		public TooltipTerm TooltipTerm => _tooltipTerm;
	}

	[Serializable]
	public class RuntimeVisualsData
	{
		[Tooltip("The id of the visual that should be displayed. If set to null (none), the Visitor will be inactive.")]
		[SerializeField]
		private VisualId _visualToDisplay;

		public VisualId VisualToDisplay
		{
			get
			{
				return _visualToDisplay;
			}
			set
			{
				_visualToDisplay = value;
			}
		}
	}

	[SerializeField]
	private StaticVisualsData _staticData;

	[SerializeField]
	private RuntimeVisualsData _runtimeData;

	[SerializeField]
	private SaveEvent _saveEvent;

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

	public StaticVisualsData StaticData => _staticData;

	public RuntimeVisualsData RuntimeData => _runtimeData;

	public string ID => Guid;

	public void ResetToInitialState()
	{
		_runtimeData.VisualToDisplay = _staticData.DefaultVisualToDisplay;
	}

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
		return JsonUtility.ToJson(new StringWrapper
		{
			Value = ((_runtimeData.VisualToDisplay != null) ? _runtimeData.VisualToDisplay.Id : string.Empty)
		});
	}

	public void Deserialize(string jsonData)
	{
		StringWrapper stringWrapper = JsonUtility.FromJson<StringWrapper>(jsonData);
		if (!string.IsNullOrEmpty(stringWrapper.Value))
		{
			_runtimeData.VisualToDisplay = (VisualId)_saveEvent.GetReferenceObjectByID(stringWrapper.Value);
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
			Debug.Log($"Visuals data {name} has not connected Save Event");
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
