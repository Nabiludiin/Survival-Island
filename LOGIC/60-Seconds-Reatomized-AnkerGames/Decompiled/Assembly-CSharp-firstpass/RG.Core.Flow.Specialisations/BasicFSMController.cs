using UnityEngine;

namespace RG.Core.Flow.Specialisations;

[RequireComponent(typeof(BasicFSM))]
public class BasicFSMController : BaseController<BasicStateMachine>
{
	private void Reset()
	{
		StateMachine = new BasicStateMachine();
		StateMachine.AssignMachine(gameObject.transform.GetComponent<BasicFSM>());
	}
}
