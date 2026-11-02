using UnityEngine;

[AddComponentMenu("Daikon Forge/Input/Gestures/Hold")]
public class dfHoldGesture : dfGestureBase
{
	[SerializeField]
	private float minTime = 0.75f;

	[SerializeField]
	private float maxDistance = 25f;

	public float MinimumTime
	{
		get
		{
			return minTime;
		}
		set
		{
			minTime = value;
		}
	}

	public float MaximumDistance
	{
		get
		{
			return maxDistance;
		}
		set
		{
			maxDistance = value;
		}
	}

	public float HoldLength
	{
		get
		{
			if (State == dfGestureState.Began)
			{
				return Time.realtimeSinceStartup - StartTime;
			}
			return 0f;
		}
	}

	public event dfGestureEventHandler<dfHoldGesture> HoldGestureStart;

	public event dfGestureEventHandler<dfHoldGesture> HoldGestureEnd;

	protected void Start()
	{
	}

	protected void Update()
	{
		if (State == dfGestureState.Possible && Time.realtimeSinceStartup - StartTime >= minTime)
		{
			State = dfGestureState.Began;
			if (HoldGestureStart != null)
			{
				HoldGestureStart(this);
			}
			gameObject.Signal("OnHoldGestureStart", this);
		}
	}

	public void OnMouseDown(dfControl source, dfMouseEventArgs args)
	{
		State = dfGestureState.Possible;
		Vector2 startPosition = (CurrentPosition = args.Position);
		StartPosition = startPosition;
		StartTime = Time.realtimeSinceStartup;
	}

	public void OnMouseMove(dfControl source, dfMouseEventArgs args)
	{
		if (State != dfGestureState.Possible && State != dfGestureState.Began)
		{
			return;
		}
		CurrentPosition = args.Position;
		if (!(Vector2.Distance(args.Position, StartPosition) > maxDistance))
		{
			return;
		}
		if (State == dfGestureState.Possible)
		{
			State = dfGestureState.Failed;
		}
		else if (State == dfGestureState.Began)
		{
			State = dfGestureState.Cancelled;
			if (HoldGestureEnd != null)
			{
				HoldGestureEnd(this);
			}
			gameObject.Signal("OnHoldGestureEnd", this);
		}
	}

	public void OnMouseUp(dfControl source, dfMouseEventArgs args)
	{
		if (State == dfGestureState.Began)
		{
			CurrentPosition = args.Position;
			State = dfGestureState.Ended;
			if (HoldGestureEnd != null)
			{
				HoldGestureEnd(this);
			}
			gameObject.Signal("OnHoldGestureEnd", this);
		}
		State = dfGestureState.None;
	}

	public void OnMultiTouch(dfControl source, dfTouchEventArgs args)
	{
		if (State == dfGestureState.Began)
		{
			State = dfGestureState.Cancelled;
			if (HoldGestureEnd != null)
			{
				HoldGestureEnd(this);
			}
			gameObject.Signal("OnHoldGestureEnd", this);
		}
		else
		{
			State = dfGestureState.Failed;
		}
	}
}
