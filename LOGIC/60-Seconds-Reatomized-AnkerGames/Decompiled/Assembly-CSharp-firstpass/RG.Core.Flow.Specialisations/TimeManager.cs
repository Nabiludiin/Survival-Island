using RG.Core.Base;
using RG.Core.Data.Abstractions;
using RG.Core.Data.Specialisations;
using RG.Core.Flow.Abstractions;
using UnityEngine;

namespace RG.Core.Flow.Specialisations;

public class TimeManager : RGMonoBehaviourSingleton<TimeManager>, IManager, IDataOwner<TimeData>
{
	[SerializeField]
	private TimeData _data;

	public TimeData Data
	{
		protected get
		{
			return _data;
		}
		set
		{
			_data = value;
		}
	}

	protected override void CustomUpdate()
	{
		base.CustomUpdate();
		_data.UpdateTimers(Time.deltaTime);
	}

	public bool AddTimer(string id, bool paused = false, float startTime = 0f, float multiplier = 1f, params string[] tags)
	{
		if (_data.AddTimer(id, paused, startTime, multiplier, tags) != null)
		{
			DebugManager.LogFormat("Timer {0} added.", id);
			return true;
		}
		DebugManager.LogErrorFormat("Timer {0} already exists.", this, id);
		return false;
	}

	public bool RemoveTimer(string id)
	{
		if (_data.RemoveTimer(id))
		{
			DebugManager.LogFormat("Timer {0} removed.", id);
			return true;
		}
		DebugManager.LogErrorFormat("Timer {0} does not exist.", this, id);
		return false;
	}

	public float GetTime(string id)
	{
		return _data.GetTime(id);
	}

	public bool IsLater(string id, float time)
	{
		return _data.IsLater(id, time);
	}

	public bool IsEarlier(string id, float time)
	{
		return _data.IsEarlier(id, time);
	}

	public void ResetData()
	{
		_data.ResetData();
	}
}
