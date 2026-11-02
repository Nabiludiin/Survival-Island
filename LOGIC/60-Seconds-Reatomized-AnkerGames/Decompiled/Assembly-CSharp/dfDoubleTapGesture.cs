using UnityEngine;

[AddComponentMenu("Daikon Forge/Input/Gestures/Double Tap")]
public class dfDoubleTapGesture : dfGestureBase
{
	[SerializeField]
	private float timeout = 0.5f;

	[SerializeField]
	private float maxDistance = 35f;

	public float Timeout
	{
		get
		{
			return timeout;
		}
		set
		{
			timeout = value;
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

	public event dfGestureEventHandler<dfDoubleTapGesture> DoubleTapGesture;

	protected void Start()
	{
	}

	public void OnMouseDown(dfControl source, dfMouseEventArgs args)
	{
		if (State == dfGestureState.Possible && Time.realtimeSinceStartup - StartTime <= timeout && Vector2.Distance(args.Position, StartPosition) <= maxDistance)
		{
			Vector2 startPosition = (CurrentPosition = args.Position);
			StartPosition = startPosition;
			State = dfGestureState.Began;
			if (DoubleTapGesture != null)
			{
				DoubleTapGesture(this);
			}
			gameObject.Signal("OnDoubleTapGesture", this);
			endGesture();
		}
		else
		{
			Vector2 startPosition = (CurrentPosition = args.Position);
			StartPosition = startPosition;
			State = dfGestureState.Possible;
			StartTime = Time.realtimeSinceStartup;
		}
	}

	public void OnMouseLeave()
	{
		endGesture();
	}

	public void OnMultiTouchEnd()
	{
		endGesture();
	}

	public void OnMultiTouch()
	{
		endGesture();
	}

	private void endGesture()
	{
		if (State == dfGestureState.Began || State == dfGestureState.Changed)
		{
			State = dfGestureState.Ended;
		}
		else if (State == dfGestureState.Possible)
		{
			State = dfGestureState.Cancelled;
		}
		else
		{
			State = dfGestureState.None;
		}
	}
}
