using System;
using RG.Core.Flow.Abstractions;

namespace RG.Core.Flow.Specialisations;

[Serializable]
public class BasicStateMachine : BaseStateMachine<BasicFSM>
{
	private IState _currentState;

	private bool _active;

	private IState CurrentState => _currentState;

	public override bool Active => _active;

	public override string CurrentStateName => Machine.ActiveStateName;

	public VariableDictionary Values => Machine.Values;

	public override void Start()
	{
		SetActive(active: true);
	}

	public override void Stop()
	{
		SetActive(active: false);
	}

	public override void Restart()
	{
		Start();
	}

	public override void Update()
	{
	}

	public override bool SetValue<V>(string key, V val)
	{
		object obj = val;
		if (typeof(V) == typeof(string))
		{
			Values.SetString(key, obj as string);
			return true;
		}
		if (typeof(V) == typeof(int))
		{
			Values.SetInt(key, (int)obj);
			return true;
		}
		if (typeof(V) == typeof(float))
		{
			Values.SetFloat(key, (float)obj);
			return true;
		}
		return false;
	}

	public override V GetValue<V>(string key)
	{
		object obj = null;
		if (typeof(V) == typeof(string))
		{
			obj = Values.GetString(key);
		}
		else if (typeof(V) == typeof(int))
		{
			obj = Values.GetInt(key);
		}
		else if (typeof(V) == typeof(float))
		{
			obj = Values.GetFloat(key);
		}
		if (obj != null)
		{
			return (V)obj;
		}
		return default;
	}

	private void SetActive(bool active)
	{
		_active = active;
	}
}
