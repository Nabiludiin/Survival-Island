using System;
using RG.Core.Data.Abstractions;
using UnityEngine;

namespace RG.Core.Data.Specialisations;

[Serializable]
public class UnlockData : DataObject, ISerializable
{
	[SerializeField]
	private string _id = string.Empty;

	[SerializeField]
	private bool _enabled = true;

	[SerializeField]
	private bool _unlocked;

	public bool Unlocked
	{
		get
		{
			return _unlocked;
		}
		set
		{
			_unlocked = value;
		}
	}

	public bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			_enabled = value;
		}
	}

	public override void ResetData()
	{
		_enabled = true;
		_unlocked = false;
	}

	public void Load()
	{
		throw new NotImplementedException();
	}

	public void Save()
	{
		throw new NotImplementedException();
	}
}
