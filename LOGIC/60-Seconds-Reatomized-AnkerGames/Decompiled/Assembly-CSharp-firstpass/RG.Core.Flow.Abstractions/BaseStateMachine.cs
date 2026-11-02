using System;
using UnityEngine;

namespace RG.Core.Flow.Abstractions;

[Serializable]
public abstract class BaseStateMachine<T> : IStateMachine where T : MonoBehaviour
{
	[SerializeField]
	private T _machine;

	public abstract bool Active { get; }

	public abstract string CurrentStateName { get; }

	protected T Machine
	{
		get
		{
			return _machine;
		}
		set
		{
			_machine = value;
		}
	}

	public void AssignMachine(MonoBehaviour machine)
	{
		Machine = machine as T;
	}

	public abstract void Start();

	public abstract void Stop();

	public abstract void Restart();

	public abstract void Update();

	public abstract V GetValue<V>(string key);

	public abstract bool SetValue<V>(string key, V val);
}
