using UnityEngine;

namespace RG.Parsecs.Scavenge;

public interface IOnEventHandler<T1, T2, T3> where T1 : MonoBehaviour, IHaveEventHub<T1, T2, T3> where T2 : IEventHubArgument<T3>
{
	EventFilters<T1, T2, T3> Filter { get; set; }
}
