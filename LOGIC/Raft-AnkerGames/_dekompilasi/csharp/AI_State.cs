using UnityEngine;

public class AI_State : MonoBehaviour
{
	public uint stateIndex;

	[HideInInspector]
	public AI_StateMachine stateMachine;

	protected bool active;

	public bool IsFearlessInState;

	protected virtual void Awake()
	{
		stateMachine = GetComponentInParent<AI_StateMachine>();
	}

	protected virtual void Start()
	{
	}

	protected virtual void Destroy()
	{
	}

	public virtual void OnWorldShift(Vector3 shift)
	{
	}

	public virtual RGD_AI_State SaveState()
	{
		return null;
	}

	public virtual void UpdateState()
	{
	}

	public virtual void EnterState()
	{
		active = true;
	}

	public virtual void ExitState()
	{
		active = false;
	}

	public virtual void OnStateChange<T>(params T[] parameters)
	{
	}

	public bool IsActive()
	{
		return active;
	}
}
