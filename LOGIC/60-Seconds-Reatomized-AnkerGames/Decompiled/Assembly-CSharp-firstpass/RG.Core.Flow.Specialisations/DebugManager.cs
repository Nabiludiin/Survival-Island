using System;
using RG.Core.Base;
using RG.Core.Flow.Abstractions;
using UnityEngine;

namespace RG.Core.Flow.Specialisations;

[Serializable]
public class DebugManager : RGMonoBehaviourSingleton<DebugManager>, IManager
{
	private const string TIMESTAMP_FORMAT = "[{0}][{1}]";

	[SerializeField]
	private bool _logging = true;

	private ConsoleManager _console;

	public bool Logging
	{
		get
		{
			return _logging;
		}
		set
		{
			_logging = value;
		}
	}

	protected bool ActiveConsole
	{
		get
		{
			if (_console != null)
			{
				return _console.Active;
			}
			return false;
		}
	}

	protected override void CustomStart()
	{
		base.CustomStart();
		_console = RGMonoBehaviourSingleton<ConsoleManager>.TheInstance;
	}

	protected virtual void DoLog(string msg, UnityEngine.Object context = null)
	{
		if (Logging)
		{
			if (ActiveConsole)
			{
				_console.Log(msg, null, null);
			}
			else
			{
				Debug.Log(msg, context);
			}
		}
	}

	protected virtual void DoLogFormat(string format, params string[] args)
	{
		if (Logging)
		{
			if (ActiveConsole)
			{
				ConsoleManager console = _console;
				object[] args2 = args;
				console.Log(null, null, format, args2);
			}
			else
			{
				object[] args2 = args;
				Debug.LogFormat(format, args2);
			}
		}
	}

	protected virtual void DoLogError(string msg, UnityEngine.Object context = null)
	{
		if (Logging)
		{
			if (ActiveConsole)
			{
				_console.Log(msg, context, null);
			}
			else
			{
				Debug.LogError(msg, context);
			}
		}
	}

	protected virtual void DoLogErrorFormat(string format, UnityEngine.Object context = null, params object[] args)
	{
		if (Logging)
		{
			if (ActiveConsole)
			{
				_console.Log(null, context, format, args);
			}
			else
			{
				Debug.LogErrorFormat(context, format, args);
			}
		}
	}

	protected virtual void DoLogWarning(string msg, UnityEngine.Object context = null)
	{
		if (Logging)
		{
			if (ActiveConsole)
			{
				_console.Log(msg, context, null);
			}
			else
			{
				Debug.LogWarning(msg, context);
			}
		}
	}

	protected virtual void DoLogWarningFormat(string format, params object[] args)
	{
		if (Logging)
		{
			if (ActiveConsole)
			{
				_console.Log(null, null, format, args);
			}
			else
			{
				Debug.LogWarningFormat(format, args);
			}
		}
	}

	private static string GenerateTimestamp()
	{
		TimeManager theInstance = RGMonoBehaviourSingleton<TimeManager>.TheInstance;
		return string.Format("[{0}][{1}]", theInstance.GetTime("GameTime"), theInstance.GetTime("PlayTime"));
	}

	public static void Log(string msg)
	{
		RGMonoBehaviourSingleton<DebugManager>.TheInstance.DoLog(msg);
	}

	public static void LogFormat(string format, params string[] args)
	{
		RGMonoBehaviourSingleton<DebugManager>.TheInstance.DoLogFormat(format, args);
	}

	public static void LogError(string msg, UnityEngine.Object context = null)
	{
		RGMonoBehaviourSingleton<DebugManager>.TheInstance.DoLogError(msg, context);
	}

	public static void LogErrorFormat(string format, UnityEngine.Object context = null, params object[] args)
	{
		RGMonoBehaviourSingleton<DebugManager>.TheInstance.DoLogErrorFormat(format, context, args);
	}

	public static void LogWarning(string msg, UnityEngine.Object context = null)
	{
		RGMonoBehaviourSingleton<DebugManager>.TheInstance.DoLogWarning(msg, context);
	}

	public static void LogWarningFormat(string format, params object[] args)
	{
		RGMonoBehaviourSingleton<DebugManager>.TheInstance.DoLogWarningFormat(format, args);
	}
}
