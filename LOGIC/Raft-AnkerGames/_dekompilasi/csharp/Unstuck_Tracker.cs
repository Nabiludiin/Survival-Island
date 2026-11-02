using System;
using UnityEngine;

public class Unstuck_Tracker : MonoBehaviour
{
	public Action<bool, Unstuck_Tracker> OnStuckValueChanged;

	private Vector3 latestTrackedPos;

	[SerializeField]
	protected Transform tracked;

	[SerializeField]
	private float trackedPosDistance = 3f;

	[SerializeField]
	private float stuckDurationRequiredSeconds = 30f;

	[SerializeField]
	private float stuckDurationRequiredDeadSeconds = 120f;

	[SerializeField]
	protected float teleportedPointDestinationYOffset = 1f;

	private float currentStuckProgress;

	private bool currentlyStuck;

	private Raft_Network network;

	public bool IsConsideredStuck => currentStuckProgress >= stuckDurationRequiredSeconds;

	public bool IsConsideredStuckWhileDead => currentStuckProgress >= stuckDurationRequiredDeadSeconds;

	public Vector3 LatestTrackedPos
	{
		get
		{
			return latestTrackedPos;
		}
		set
		{
			latestTrackedPos = value;
		}
	}

	public virtual bool IsUpdateValid => tracked != null;

	private void TestStuckValueChanged(bool value, Unstuck_Tracker tracker)
	{
	}

	protected virtual void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		if (tracked == null)
		{
			tracked = transform;
		}
		OnStuckValueChanged = (Action<bool, Unstuck_Tracker>)Delegate.Combine(OnStuckValueChanged, new Action<bool, Unstuck_Tracker>(TestStuckValueChanged));
	}

	private void Update()
	{
		if (!IsUpdateValid)
		{
			return;
		}
		Vector3 position = tracked.transform.position;
		if (IsWithinDistanceOfPosition(LatestTrackedPos, tracked.transform))
		{
			currentStuckProgress += Time.deltaTime;
		}
		else
		{
			LatestTrackedPos = position;
			currentStuckProgress = 0f;
		}
		if (IsConsideredStuck)
		{
			if (!currentlyStuck)
			{
				SetIsStuck(isStuck: true);
			}
		}
		else if (currentlyStuck)
		{
			SetIsStuck(isStuck: false);
		}
	}

	public virtual void Unstuck()
	{
		Vector3 position = tracked.transform.position;
		Vector3 closestUnstuckPoint = Unstuck_PointManager.GetClosestUnstuckPoint(position);
		if (ShouldTPToRaft(position, closestUnstuckPoint))
		{
			StuckFixTPRaft();
		}
		else
		{
			Teleport(closestUnstuckPoint);
		}
		SetIsStuck(isStuck: false);
	}

	protected virtual void Teleport(Vector3 point)
	{
		Vector3 position = point;
		position.y += teleportedPointDestinationYOffset;
		tracked.transform.position = position;
	}

	protected void SetIsStuck(bool isStuck)
	{
		if (!isStuck)
		{
			currentStuckProgress = 0f;
		}
		currentlyStuck = isStuck;
		SendOnStuckValueChanged(isStuck);
	}

	protected virtual bool ShouldTPToRaft(Vector3 originPoint, Vector3 destinationPoint)
	{
		if (originPoint == destinationPoint || tracked.transform.ParentedToRaft())
		{
			return true;
		}
		return false;
	}

	protected virtual void StuckFixTPRaft()
	{
	}

	private void SendOnStuckValueChanged(bool value)
	{
		OnStuckValueChanged?.Invoke(value, this);
	}

	private bool IsWithinDistanceOfPosition(Vector3 pos, Transform tracked)
	{
		if (Vector3.Distance(pos, tracked.position) < trackedPosDistance)
		{
			return true;
		}
		return false;
	}
}
