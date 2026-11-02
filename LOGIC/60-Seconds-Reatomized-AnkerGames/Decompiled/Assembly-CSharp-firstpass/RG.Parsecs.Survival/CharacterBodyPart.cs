using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class CharacterBodyPart
{
	[SerializeField]
	private BodyPartType _bodyPartType;

	[Tooltip("The statuses the character\nneeds to have in order to\nbe able to display this bodypart.")]
	[SerializeField]
	private CharacterStatus[] _requiredStatuses;

	private bool _isValidityCached;

	private bool _isValidCache;

	[SerializeField]
	private List<StatusAlternatives> _requiredStatusAlternatives = new List<StatusAlternatives>();

	[SerializeField]
	private int _priorityModifier;

	private const int STATUS_INTERSECTION_MULTIPLIER = 100;

	private GameObject _handle;

	private string _bodyPartName = string.Empty;

	public GameObject Handle
	{
		get
		{
			return _handle;
		}
		set
		{
			_handle = value;
		}
	}

	public string BodyPartName
	{
		get
		{
			return _bodyPartName;
		}
		set
		{
			_bodyPartName = value;
		}
	}

	public BodyPartType BodyPartType => _bodyPartType;

	public CharacterStatus[] RequiredStatuses => _requiredStatuses;

	public bool RequiresStatusNotInList(List<CharacterStatus> statuses)
	{
		for (int i = 0; i < _requiredStatuses.Length; i++)
		{
			if (!statuses.Contains(_requiredStatuses[i]))
			{
				return true;
			}
		}
		if (IsRequiredStatusAlternativesValid())
		{
			for (int j = 0; j < _requiredStatusAlternatives.Count; j++)
			{
				bool flag = false;
				for (int k = 0; k < _requiredStatusAlternatives[j].Alternatives.Count; k++)
				{
					if (statuses.Contains(_requiredStatusAlternatives[j].Alternatives[k]))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool IsRequiredStatusAlternativesValid()
	{
		if (_isValidityCached)
		{
			return _isValidCache;
		}
		_isValidityCached = true;
		_isValidCache = true;
		if (_requiredStatusAlternatives == null || _requiredStatusAlternatives.Count == 0)
		{
			_isValidCache = false;
			return false;
		}
		for (int i = 0; i < _requiredStatusAlternatives.Count; i++)
		{
			if (_requiredStatusAlternatives[i].Alternatives == null)
			{
				_isValidCache = false;
				return false;
			}
		}
		return true;
	}

	public int GetIntersectionCount(List<CharacterStatus> statuses)
	{
		int num = 0;
		for (int i = 0; i < _requiredStatuses.Length; i++)
		{
			if (statuses.Contains(RequiredStatuses[i]))
			{
				num++;
			}
		}
		if (IsRequiredStatusAlternativesValid())
		{
			for (int j = 0; j < _requiredStatusAlternatives.Count; j++)
			{
				bool flag = false;
				for (int k = 0; k < _requiredStatusAlternatives[j].Alternatives.Count; k++)
				{
					if (statuses.Contains(_requiredStatusAlternatives[j].Alternatives[k]))
					{
						flag = true;
					}
				}
				if (flag)
				{
					num++;
				}
			}
		}
		return num * 100 + _priorityModifier;
	}
}
