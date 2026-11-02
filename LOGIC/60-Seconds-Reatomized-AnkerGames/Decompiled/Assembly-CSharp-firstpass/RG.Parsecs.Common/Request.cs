using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Common;

public abstract class Request
{
	protected Coroutine Coroutine;

	private bool _started;

	private readonly MonoBehaviour _owner;

	public abstract bool IsDone { get; protected set; }

	protected Request(MonoBehaviour owner)
	{
		_owner = owner;
	}

	public Coroutine StartRequest()
	{
		if (!_started)
		{
			Coroutine = _owner.StartCoroutine(Run());
			_started = true;
		}
		return Coroutine;
	}

	protected abstract IEnumerator Run();
}
