using UnityEngine;

public class AI_State_Dead : AI_State
{
	[SerializeField]
	protected bool removeBody = true;

	[SerializeField]
	protected float removeBodyTime = 60f;

	[SerializeField]
	protected AI_State decayState;

	public override void EnterState()
	{
		base.EnterState();
		if (removeBody && Raft_Network.IsHost)
		{
			CancelInvoke("RemoveBody");
			Invoke("RemoveBody", removeBodyTime);
		}
	}

	protected virtual void RemoveBody()
	{
		if (decayState == null)
		{
			AI_NetworkBehaviour componentInChildren = stateMachine.GetComponentInChildren<AI_NetworkBehaviour>();
			if (componentInChildren != null)
			{
				NetworkIDManager.SendIDBehaviourDead(componentInChildren.ObjectIndex, typeof(AI_NetworkBehaviour));
			}
		}
		else
		{
			stateMachine.ChangeState(decayState);
		}
	}
}
