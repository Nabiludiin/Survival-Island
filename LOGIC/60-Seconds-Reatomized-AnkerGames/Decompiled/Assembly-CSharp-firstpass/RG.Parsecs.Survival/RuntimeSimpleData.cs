using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class RuntimeSimpleData
{
	[SerializeField]
	private int _systemLevel = 1;

	[SerializeField]
	private bool _isActive;

	public int SystemLevel
	{
		get
		{
			return _systemLevel;
		}
		set
		{
			_systemLevel = value;
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
