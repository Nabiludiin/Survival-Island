using System;

namespace RG.Parsecs.Common;

public class StateEventArgs<T> : EventArgs
{
	private T _currentState;

	private T _previousState;

	public T CurrentState => _currentState;

	public T PreviousState => _previousState;

	public StateEventArgs(T currentState, T previousState)
	{
		_currentState = currentState;
		_previousState = previousState;
	}
}
