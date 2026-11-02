using System;
using System.Collections.Generic;
using RG.Core.Data.Abstractions;
using UnityEngine;

namespace RG.Core.Data.DataTypes;

[Serializable]
public class Timer : ISerializable
{
	[SerializeField]
	private string _id = string.Empty;

	[SerializeField]
	private float _time;

	[SerializeField]
	private bool _paused;

	[SerializeField]
	private float _multiplier = 1f;

	[SerializeField]
	private HashSet<string> _tags;

	public string Id => _id;

	public float Now => _time * _multiplier;

	public bool Paused
	{
		get
		{
			return _paused;
		}
		set
		{
			_paused = value;
		}
	}

	public Timer(string id, bool paused = false, float startTime = 0f, float multiplier = 1f, params string[] tags)
	{
		_id = id;
		_paused = paused;
		_time = startTime;
		_multiplier = multiplier;
		_tags = new HashSet<string>(tags);
	}

	public void Update(float delta)
	{
		if (!_paused)
		{
			_time += delta;
		}
	}

	public int CompareTo(float time)
	{
		return time.CompareTo(Now);
	}

	public bool IsLater(float time)
	{
		return CompareTo(time) > 0;
	}

	public bool IsEarlier(float time)
	{
		return CompareTo(time) < 0;
	}

	public bool TestTag(string tag)
	{
		if (_tags == null)
		{
			return false;
		}
		return _tags.Contains(tag);
	}

	public void Reset()
	{
		_time = 0f;
		_paused = false;
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
