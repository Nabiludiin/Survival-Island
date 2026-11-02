using UnityEngine;

namespace RG.Parsecs.Scavenge;

public abstract class EventFilters<T1, T2, T3> : MonoBehaviour where T1 : MonoBehaviour, IHaveEventHub<T1, T2, T3> where T2 : IEventHubArgument<T3>
{
	public void Start()
	{
		GetComponent<IOnEventHandler<T1, T2, T3>>().Filter = this;
	}

	public abstract bool IsMatch(T1 sender, T2 args);
}
