using UnityEngine;

public class AI_Component : MonoBehaviour
{
	public AI_State connectedState;

	[Tooltip("Offsets the TimeStamp in Random-Range T seconds")]
	[SerializeField]
	private float minimumSwitchStateCooldownOffsetT;

	[Tooltip("Offsets the TimeStamp in Random-Range T seconds")]
	[SerializeField]
	private float maximumSwitchStateCooldownOffsetT;

	[SerializeField]
	private float switchStateCooldown;

	protected float switchStateTimeStamp;

	public virtual bool CanSwitchState => Time.time - switchStateTimeStamp >= switchStateCooldown;

	protected virtual void Awake()
	{
		if (connectedState == null)
		{
			connectedState = GetComponent<AI_State>();
		}
	}

	protected virtual void OnValidate()
	{
		connectedState = GetComponent<AI_State>();
	}

	private void Update()
	{
		if (connectedState != null && connectedState.IsActive())
		{
			OnUpdateComponent();
		}
	}

	protected virtual void OnUpdateComponent()
	{
	}

	public virtual void OnEnterState()
	{
	}

	public virtual void OnExitState()
	{
	}

	protected virtual bool SwitchState<T>(AI_State state, params T[] parameters)
	{
		if (CanSwitchState)
		{
			state.stateMachine.ChangeState(state, parameters);
			switchStateTimeStamp = GetRandomCooldownTimeStamp();
			return true;
		}
		return false;
	}

	protected virtual bool SwitchState(AI_State state)
	{
		if (CanSwitchState)
		{
			state.stateMachine.ChangeState(state);
			switchStateTimeStamp = GetRandomCooldownTimeStamp();
			return true;
		}
		return false;
	}

	protected virtual float GetRandomCooldownTimeStamp()
	{
		if (minimumSwitchStateCooldownOffsetT == maximumSwitchStateCooldownOffsetT && minimumSwitchStateCooldownOffsetT == 0f)
		{
			return Time.time;
		}
		return Time.time + Random.Range(minimumSwitchStateCooldownOffsetT, maximumSwitchStateCooldownOffsetT);
	}

	public virtual void SetCooldownTimeStamp(float time)
	{
		switchStateTimeStamp = Time.time + time - switchStateCooldown;
	}
}
