using System.Collections.Generic;
using RG.Core.Base;
using RG.Core.SaveSystem;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[CreateAssetMenu(fileName = "NewEventTagList", menuName = "60 Parsecs!/Event Editor/New Event Tag List")]
public class EventTagList : RGScriptableObject, ISaveable
{
	[SerializeField]
	private SaveEvent _saveEvent;

	[Tooltip("Check if the list have to clear on reset game")]
	[SerializeField]
	private bool _clearOnEnable;

	[HideInInspector]
	[SerializeField]
	private bool _isDirty;

	private const string EVENT_TAG_LIST_GUID_PREFIX = "tagList";

	[SerializeField]
	private List<EventTag> _eventTags;

	public bool IsDirty => _isDirty;

	public List<EventTag> EventTags => _eventTags;

	public string ID => Guid;

	public override string GuidPrefix => "tagList";

	public void AddTag(EventTag tag)
	{
		if (!_eventTags.Contains(tag))
		{
			_eventTags.Add(tag);
			_isDirty = true;
		}
	}

	public void RemoveTag(EventTag tag)
	{
		if (_eventTags.Contains(tag))
		{
			_eventTags.Remove(tag);
			_isDirty = true;
		}
	}

	public void ClearTagList()
	{
		if (_eventTags.Count > 0)
		{
			_eventTags.Clear();
			_isDirty = true;
		}
	}

	public bool IsTagOnList(EventTag tag)
	{
		return _eventTags.Contains(tag);
	}

	public void SetAsClean()
	{
		_isDirty = false;
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
		StringListWrapper stringListWrapper = new StringListWrapper
		{
			List = new List<string>()
		};
		for (int i = 0; i < _eventTags.Count; i++)
		{
			stringListWrapper.List.Add("etag_" + _eventTags[i].name);
		}
		return JsonUtility.ToJson(stringListWrapper);
	}

	public void Deserialize(string jsonData)
	{
		StringListWrapper stringListWrapper = JsonUtility.FromJson<StringListWrapper>(jsonData);
		for (int i = 0; i < stringListWrapper.List.Count; i++)
		{
			_eventTags.Add((EventTag)_saveEvent.GetReferenceObjectByID(stringListWrapper.List[i]));
		}
		_isDirty = true;
	}

	public void Register()
	{
		if (_saveEvent != null)
		{
			_saveEvent.RegisterListener(this);
		}
	}

	public void Unregister()
	{
		if (_saveEvent != null)
		{
			_saveEvent.UnregisterListener(this);
		}
	}

	public void ResetData()
	{
		ClearTagList();
	}
}
