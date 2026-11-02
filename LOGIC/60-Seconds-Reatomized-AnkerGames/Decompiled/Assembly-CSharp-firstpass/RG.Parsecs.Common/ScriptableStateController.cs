using System;
using UnityEngine;

namespace RG.Parsecs.Common;

public abstract class ScriptableStateController<T> : ScriptableObject
{
	[NonSerialized]
	private T _currentState;

	[NonSerialized]
	private T _previousState;

	public T CurrentState
	{
		get
		{
			return _currentState;
		}
		set
		{
			_previousState = _currentState;
			_currentState = value;
			SwitchState(_currentState, _previousState);
		}
	}

	public event EventHandler OnStateChange;

	protected virtual void SwitchState(T currentState, T previousState)
	{
		if (OnStateChange != null)
		{
			OnStateChange(this, new StateEventArgs<T>(_currentState, _previousState));
		}
	}
}
