using UnityEngine;

namespace _Raft.ExternalVelocity;

public class ExternalVelAdderContinous : ExternalVelAdder
{
	private void OnTriggerStay(Collider col)
	{
		AddVelocity(col, velocityMode);
	}
}
