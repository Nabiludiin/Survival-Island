using System.Collections.Generic;
using RG.Core.Base;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/New Computer Lamps Listener List", fileName = "Computer Lamps Listener List")]
public class ComputerLampsListenerList : RGScriptableObject
{
	public delegate void OnLampsUpdate();

	private List<OnLampsUpdate> _lampsUpdates = new List<OnLampsUpdate>();

	public void RegisterOnLampsUpdate(OnLampsUpdate method)
	{
		if (_lampsUpdates == null)
		{
			_lampsUpdates = new List<OnLampsUpdate>();
		}
		_lampsUpdates.Add(method);
	}

	public void UnregisterOnLampsUpdate(OnLampsUpdate method)
	{
		if (_lampsUpdates == null)
		{
			_lampsUpdates = new List<OnLampsUpdate>();
		}
		_lampsUpdates.Remove(method);
	}

	public void NotifyOnLampsUpdate()
	{
		for (int i = 0; i < _lampsUpdates.Count; i++)
		{
			_lampsUpdates[i]();
		}
	}

	public void UpdateLampsOnEndOfDay()
	{
		NotifyOnLampsUpdate();
	}
}
