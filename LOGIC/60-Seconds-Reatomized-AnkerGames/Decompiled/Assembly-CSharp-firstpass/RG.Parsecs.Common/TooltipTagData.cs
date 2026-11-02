using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Common;

[CreateAssetMenu(fileName = "New Tooltip Tag Data", menuName = "60 Parsecs!/Tooltips/New Tooltip Tag Data")]
public class TooltipTagData : ScriptableObject
{
	public delegate void OnTagDataChanged();

	[SerializeField]
	private List<OnTagDataChanged> listeners = new List<OnTagDataChanged>();

	[SerializeField]
	private List<TooltipTag> _allTags = new List<TooltipTag>();

	[SerializeField]
	private List<TooltipTag> _startingFilteredTags = new List<TooltipTag>();

	[SerializeField]
	[Tooltip("Tooltips marked with these tags will not be displayed")]
	private List<TooltipTag> _currentFilteredTags = new List<TooltipTag>();

	public List<TooltipTag> CurrentFilteredTags => _currentFilteredTags;

	public List<TooltipTag> AllTags => _allTags;

	public void TagDataChanged()
	{
		for (int i = 0; i < listeners.Count; i++)
		{
			listeners[i]();
		}
	}

	public void Register(OnTagDataChanged method)
	{
		listeners.Add(method);
	}

	public void Unregister(OnTagDataChanged method)
	{
		listeners.Remove(method);
	}

	public void AddTag(string tagId)
	{
		TooltipTag tagById = GetTagById(tagId);
		if (tagById != null && !_currentFilteredTags.Contains(tagById))
		{
			_currentFilteredTags.Add(tagById);
			TagDataChanged();
		}
	}

	public void AddTag(TooltipTag tag)
	{
		if (tag != null && !_currentFilteredTags.Contains(tag))
		{
			_currentFilteredTags.Add(tag);
			TagDataChanged();
		}
	}

	public void RemoveTag(string tagId)
	{
		TooltipTag tagById = GetTagById(tagId);
		if (tagById != null && _currentFilteredTags.Contains(tagById))
		{
			_currentFilteredTags.Remove(tagById);
			TagDataChanged();
		}
	}

	public void RemoveTag(TooltipTag tag)
	{
		if (tag != null && _currentFilteredTags.Contains(tag))
		{
			_currentFilteredTags.Remove(tag);
			TagDataChanged();
		}
	}

	public void ResetTags()
	{
		_currentFilteredTags.Clear();
		_currentFilteredTags.AddRange(_startingFilteredTags);
		TagDataChanged();
	}

	public void ClearTags()
	{
		_currentFilteredTags.Clear();
		TagDataChanged();
	}

	public TooltipTag GetTagById(string tagId)
	{
		for (int i = 0; i < _allTags.Count; i++)
		{
			if (tagId == _allTags[i].Id)
			{
				return _allTags[i];
			}
		}
		return null;
	}
}
