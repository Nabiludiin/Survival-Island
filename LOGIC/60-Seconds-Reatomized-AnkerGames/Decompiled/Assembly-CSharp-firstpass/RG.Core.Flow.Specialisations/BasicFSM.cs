using RG.Core.Base;
using RG.Core.Data.Abstractions;
using RG.Core.Data.Specialisations;
using RG.Core.Flow.Abstractions;
using UnityEngine;

namespace RG.Core.Flow.Specialisations;

public class BasicFSM : RGMonoBehaviour, IDataOwner<BasicFSMData>
{
	[SerializeField]
	private BasicFSMData _data;

	[SerializeField]
	private float _updateFrequency = 1f;

	private float _nextUpdate;

	private IState _currentState;

	public BasicFSMData Data
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

	protected IState CurrentState => _currentState;

	public string ActiveStateName
	{
		get
		{
			if (CurrentState != null)
			{
				return CurrentState.Name;
			}
			return null;
		}
	}

	public VariableDictionary Values => Data.Values;

	protected override void CustomAwake()
	{
		base.CustomAwake();
		for (int i = 0; i < Data.States.Length; i++)
		{
			(Data.States[i] as BasicState).Parent = this;
			Data.States[i].Init();
		}
	}

	protected override void CustomStart()
	{
		base.CustomStart();
		PushNextState();
	}

	protected override void CustomUpdate()
	{
		if (!(_nextUpdate <= Time.time))
		{
			return;
		}
		base.CustomUpdate();
		if (_currentState != null)
		{
			if (_currentState.Active && !_currentState.CanStop())
			{
				_currentState.Update();
			}
			else
			{
				PushNextState();
			}
		}
		_nextUpdate = Time.time + _updateFrequency;
	}

	private void PushNextState()
	{
		IState state = SelectNextState();
		if (state != null)
		{
			SwitchState(state);
		}
	}

	private void SwitchState(IState state)
	{
		if (_currentState != null && _currentState.Active)
		{
			_currentState.Stop();
		}
		_currentState = state;
		if (_currentState != null)
		{
			_currentState.Start();
		}
	}

	private IState SelectNextState()
	{
		IState[] array = ((Data == null) ? null : Data.States);
		if (array != null)
		{
			IState result = null;
			for (int i = 0; i < array.Length; i++)
			{
				if (_currentState != array[i] && array[i].CanRun())
				{
					result = array[i];
					break;
				}
			}
			return result;
		}
		return null;
	}

	public void ResetData()
	{
		Data.ResetData();
	}
}
