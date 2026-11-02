using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class RuntimeSuitData
{
	[SerializeField]
	private int _suitLevel = 1;

	[SerializeField]
	private bool _isActive = true;

	public int SuitLevel
	{
		get
		{
			return _suitLevel;
		}
		set
		{
			_suitLevel = value;
		}
	}

	public bool IsActive
	{
		get
		{
			return _isActive;
		}
		set
		{
			_isActive = value;
		}
	}
}
