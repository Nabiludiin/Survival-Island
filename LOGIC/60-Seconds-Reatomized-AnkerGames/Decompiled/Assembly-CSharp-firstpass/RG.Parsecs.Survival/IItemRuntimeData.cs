using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class IItemRuntimeData
{
	[SerializeField]
	private int _level = 1;

	[SerializeField]
	protected bool _isAvailable;

	[SerializeField]
	private bool _lock;

	[SerializeField]
	private bool _isOnExpedition;

	[Tooltip("IItems can be disabled/enabled by the design team, so that they can easily filter them out.")]
	[SerializeField]
	private bool _isEnabled = true;

	public bool IsOnExpedition
	{
		get
		{
			return _isOnExpedition;
		}
		set
		{
			_isOnExpedition = value;
		}
	}

	public int Level
	{
		get
		{
			return _level;
		}
		set
		{
			_level = value;
		}
	}

	public bool Lock
	{
		get
		{
			return _lock;
		}
		set
		{
			_lock = value;
		}
	}

	public virtual bool IsAvailable
	{
		get
		{
			return _isAvailable;
		}
		set
		{
			_isAvailable = value;
		}
	}

	public virtual bool IsEnabled
	{
		get
		{
			return _isEnabled;
		}
		set
		{
			_isEnabled = value;
		}
	}
}
