using RG.Core.Base;
using RG.Core.Flow.Abstractions;
using UnityEngine;

namespace RG.Core.Flow.Specialisations;

public abstract class BaseController<T> : RGMonoBehaviour, IController where T : class, IStateMachine
{
	[SerializeField]
	private T _stateMachine;

	public IStateMachine StateMachine
	{
		protected get
		{
			return _stateMachine;
		}
		set
		{
			_stateMachine = (T)value;
		}
	}
}
