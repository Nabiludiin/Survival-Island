using RG.Core.Base;
using RG.Core.Data.Abstractions;
using RG.Core.Data.Specialisations;
using RG.Core.Flow.Abstractions;
using UnityEngine;

namespace RG.Core.Flow.Specialisations;

public class ConsoleManager : RGMonoBehaviourSingleton<ConsoleManager>, IManager, IDataOwner<ConsoleData>
{
	[SerializeField]
	private bool _active;

	[SerializeField]
	private ConsoleData _data;

	public ConsoleData Data
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

	public bool Active
	{
		get
		{
			return _active;
		}
		set
		{
			_active = value;
		}
	}

	protected override void CustomAwake()
	{
		base.CustomAwake();
		_active = true;
	}

	public void ExportLog()
	{
		_data.Save();
	}

	public void Log(string msg, Object context = null, string format = null, params object[] args)
	{
		_data.LogInfo(msg, format, context, args);
	}

	public void LogWarning(string msg, Object context = null, string format = null, params object[] args)
	{
		_data.LogWarning(msg, format, context, args);
	}

	public void LogError(string msg, Object context = null, string format = null, params object[] args)
	{
		_data.LogError(msg, format, context, args);
	}

	public void ResetData()
	{
		_data.ResetData();
	}
}
