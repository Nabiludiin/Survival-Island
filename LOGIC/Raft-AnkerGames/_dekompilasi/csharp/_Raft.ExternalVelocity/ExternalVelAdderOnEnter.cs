using UnityEngine;

namespace _Raft.ExternalVelocity;

public class ExternalVelAdderOnEnter : ExternalVelAdder
{
	private void OnTriggerEnter(Collider col)
	{
		AddVelocity(col, velocityMode);
	}
}
