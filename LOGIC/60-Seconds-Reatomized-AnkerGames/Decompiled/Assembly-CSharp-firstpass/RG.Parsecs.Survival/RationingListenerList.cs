using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Rationing Listener List")]
public class RationingListenerList : ScriptableObject
{
	public delegate void OnRationingChanged();

	[SerializeField]
	private List<OnRationingChanged> listeners = new List<OnRationingChanged>();

	public void RationingChanged()
	{
		for (int i = 0; i < listeners.Count; i++)
		{
			listeners[i]();
		}
	}

	public void Register(OnRationingChanged method)
	{
		listeners.Add(method);
	}

	public void Unregister(OnRationingChanged method)
	{
		listeners.Remove(method);
	}

	public void UpdateRationingOnEndOfDay()
	{
		RationingChanged();
	}
}
