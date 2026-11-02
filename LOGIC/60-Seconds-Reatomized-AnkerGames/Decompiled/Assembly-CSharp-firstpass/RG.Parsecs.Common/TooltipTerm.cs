using System;
using I2.Loc;
using RG.Core.Base;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.Common;

[Serializable]
[CreateAssetMenu(menuName = "60 Parsecs!/Tooltips/New Tooltip Term", fileName = "New Tooltip Term")]
public class TooltipTerm : RGScriptableObject, ISaveable
{
	[HideInInspector]
	[SerializeField]
	private string _id;

	[Tooltip("Event to which data will be save and load")]
	[SerializeField]
	private SaveEvent _saveEvent;

	[SerializeField]
	[Tooltip("The term that will be used by the tooltip")]
	private LocalizedString _initialTerm;

	[SerializeField]
	[Tooltip("The term that will be used by the tooltip")]
	private LocalizedString _term;

	private const string TOOLTIP_TERM_PREFIX = "tooltipTerm";

	public LocalizedString Term
	{
		get
		{
			return _term;
		}
		set
		{
			_term = value;
		}
	}

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

	public string ID => Guid;

	public override string GuidPrefix => "tooltipTerm";

	private void OnDestroy()
	{
		Unregister();
	}

	private void OnEnable()
	{
		Register();
	}

	public string Serialize()
	{
		return JsonUtility.ToJson(new LocalizedStringWrapper
		{
			Term = _term
		});
	}

	public void Deserialize(string jsonData)
	{
		Term = JsonUtility.FromJson<LocalizedStringWrapper>(jsonData).Term;
	}

	public void Register()
	{
		if ((bool)_saveEvent)
		{
			_saveEvent.RegisterListener(this);
		}
		else
		{
			Debug.Log($"Tooltip term {name} has not set Save Event");
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
		if ((string)_initialTerm == null || _initialTerm.IsNullOrEmpty())
		{
			_term = string.Empty;
		}
		else
		{
			_term = _initialTerm;
		}
	}
}
