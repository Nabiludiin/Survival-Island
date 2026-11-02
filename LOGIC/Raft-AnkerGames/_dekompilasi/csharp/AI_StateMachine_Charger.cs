using UnityEngine;

public class AI_StateMachine_Charger : AI_StateMachine
{
	public Network_Player target;

	[HideInInspector]
	public Network_Host networkHost;

	public override void Start()
	{
		base.Start();
		networkHost = ComponentManager<Network_Host>.Value;
	}
}
