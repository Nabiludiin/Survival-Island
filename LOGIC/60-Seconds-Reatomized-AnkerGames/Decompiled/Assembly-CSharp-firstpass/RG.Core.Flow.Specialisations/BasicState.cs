using System;
using RG.Core.Base;
using RG.Core.Data.Abstractions;
using RG.Core.Data.Specialisations;
using RG.Core.Flow.Abstractions;
using UnityEngine;

namespace RG.Core.Flow.Specialisations;

[Serializable]
public abstract class BasicState : RGScriptableObject, IState, IDataOwner<BasicStateData>
{
	[SerializeField]
	private string _stateId = string.Empty;

	[SerializeField]
	private BasicStateData _data;

	private BasicFSM _parent;

	private bool _active;

	public bool Active
	{
		get
		{
			return _active;
		}
		protected set
		{
			_active = value;
		}
	}

	public string Name => _stateId;

	public BasicStateData Data
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

	public BasicFSM Parent
	{
		protected get
		{
			return _parent;
		}
		set
		{
			_parent = value;
		}
	}

	public virtual void Init()
	{
	}

	public virtual void Start()
	{
		Active = true;
	}

	public virtual void Stop()
	{
		Active = false;
	}

	public virtual void Update()
	{
	}

	public virtual bool CanRun()
	{
		return false;
	}

	public virtual bool CanStop()
	{
		return true;
	}

	private void Reset()
	{
		_stateId = GetType().Name;
	}

	public void ResetData()
	{
		Data.ResetData();
	}
}
