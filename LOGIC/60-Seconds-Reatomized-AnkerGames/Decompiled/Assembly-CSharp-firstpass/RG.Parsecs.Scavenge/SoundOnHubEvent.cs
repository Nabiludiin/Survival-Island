using UnityEngine;

namespace RG.Parsecs.Scavenge;

public abstract class SoundOnHubEvent<T1, T2, T3> : OnEventHandler<T1, T2, T3> where T1 : MonoBehaviour, IHaveEventHub<T1, T2, T3> where T2 : IEventHubArgument<T3>
{
	public bool AddChildrenSounds = true;

	protected override void Start()
	{
		base.Start();
		_ = AddChildrenSounds;
	}

	protected override void OnEvent(T1 actor, T2 e)
	{
	}
}
