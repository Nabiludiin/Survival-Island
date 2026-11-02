using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[Serializable]
public abstract class ObjectPoolEntry<T> where T : MonoBehaviour
{
	[SerializeField]
	public string Name;

	[SerializeField]
	public T Prefab;

	[SerializeField]
	public int Count;

	private int _runtimeCount = -1;

	[HideInInspector]
	internal Stack<T> pool;

	public int RuntimeCount
	{
		get
		{
			if (_runtimeCount < 0)
			{
				_runtimeCount = Count;
			}
			return _runtimeCount;
		}
		set
		{
			_runtimeCount = value;
		}
	}

	public void ResetRuntimeCount()
	{
		_runtimeCount = Count;
	}
}
