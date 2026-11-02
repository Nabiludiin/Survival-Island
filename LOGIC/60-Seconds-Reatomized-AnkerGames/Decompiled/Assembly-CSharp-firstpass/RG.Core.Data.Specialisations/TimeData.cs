using System;
using System.Collections.Generic;
using RG.Core.Data.Abstractions;
using RG.Core.Data.DataTypes;
using UnityEngine;

namespace RG.Core.Data.Specialisations;

[Serializable]
[CreateAssetMenu(fileName = "TimeData", menuName = "RG/Data/Core/TimeData", order = 1)]
public class TimeData : DataObject, ISerializable
{
	[SerializeField]
	private List<Timer> _timers = new List<Timer>();

	public void UpdateTimers(float delta)
	{
		for (int i = 0; i < _timers.Count; i++)
		{
			_timers[i].Update(delta);
		}
	}

	public bool IsLater(string id, float time)
	{
		return GetTimer(id)?.IsLater(time) ?? false;
	}

	public bool IsEarlier(string id, float time)
	{
		return GetTimer(id)?.IsEarlier(time) ?? false;
	}

	public float GetTime(string id)
	{
		return GetTimer(id)?.Now ?? 0f;
	}

	public Timer[] GetTimers(string tag)
	{
		List<Timer> list = null;
		for (int i = 0; i < _timers.Count; i++)
		{
			if (_timers[i].TestTag(tag))
			{
				if (list == null)
				{
					list = new List<Timer>();
				}
				list.Add(_timers[i]);
			}
		}
		return list?.ToArray();
	}

	public Timer GetTimer(string id)
	{
		for (int i = 0; i < _timers.Count; i++)
		{
			if (_timers[i].Id == id)
			{
				return _timers[i];
			}
		}
		return null;
	}

	public Timer AddTimer(string id, bool paused = false, float startTime = 0f, float multiplier = 1f, params string[] tags)
	{
		if (GetTimer(id) != null)
		{
			return null;
		}
		Timer timer = new Timer(id, paused, startTime, multiplier, tags);
		_timers.Add(timer);
		return timer;
	}

	public bool RemoveTimer(string id)
	{
		Timer timer = GetTimer(id);
		if (timer != null)
		{
			_timers.Remove(timer);
			return true;
		}
		return false;
	}

	public override void ResetData()
	{
		_timers.Clear();
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
