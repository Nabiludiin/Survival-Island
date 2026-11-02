using UnityEngine;

namespace RG.Core.Flow.Abstractions;

public interface IStateMachine
{
	bool Active { get; }

	string CurrentStateName { get; }

	void Start();

	void Stop();

	void Restart();

	void Update();

	void AssignMachine(MonoBehaviour machine);

	T GetValue<T>(string key);

	bool SetValue<T>(string key, T val);
}
