using System;
using RG.Core.Base;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public abstract class Variable : RGScriptableObject
{
	[SerializeField]
	protected SaveEvent _saveEvent;

	[SerializeField]
	protected bool _isStatic;

	public abstract string TypeConnection { get; }

	public abstract Type GetVariableType { get; }

	public SaveEvent SaveEvent
	{
		get
		{
			return _saveEvent;
		}
		set
		{
			_saveEvent = value;
		}
	}

	public abstract void SetValue(object o);

	public abstract object GetValue();
}
public abstract class Variable<T> : Variable
{
	[SerializeField]
	protected T _initialValue;

	public T Value;

	protected virtual void OnDestroy()
	{
		Value = _initialValue;
	}

	public override void SetValue(object o)
	{
		Value = (T)o;
	}

	public virtual void SetValue(T value)
	{
		SetValue((object)value);
	}
}
