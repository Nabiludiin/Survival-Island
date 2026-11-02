using UnityEngine;

[AddComponentMenu("Daikon Forge/Input/Gestures/Flick")]
public class dfFlickGesture : dfGestureBase
{
	[SerializeField]
	private float timeout = 0.25f;

	[SerializeField]
	private float minDistance = 25f;

	private float hoverTime;

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

	public float MinimumDistance
	{
		get
		{
			return minDistance;
		}
		set
		{
			minDistance = value;
		}
	}

	public float DeltaTime { get; protected set; }

	public event dfGestureEventHandler<dfFlickGesture> FlickGesture;

	protected void Start()
	{
	}

	public void OnMouseDown(dfControl source, dfMouseEventArgs args)
	{
		Vector2 startPosition = (CurrentPosition = args.Position);
		StartPosition = startPosition;
		State = dfGestureState.Possible;
		StartTime = Time.realtimeSinceStartup;
		hoverTime = Time.realtimeSinceStartup;
	}

	public void OnMouseHover(dfControl source, dfMouseEventArgs args)
	{
		if (State == dfGestureState.Possible && Time.realtimeSinceStartup - hoverTime >= timeout)
		{
			Vector2 startPosition = (CurrentPosition = args.Position);
			StartPosition = startPosition;
			StartTime = Time.realtimeSinceStartup;
		}
	}

	public void OnMouseMove(dfControl source, dfMouseEventArgs args)
	{
		hoverTime = Time.realtimeSinceStartup;
		if (State == dfGestureState.Possible || State == dfGestureState.Began)
		{
			State = dfGestureState.Began;
			CurrentPosition = args.Position;
		}
	}

	public void OnMouseUp(dfControl source, dfMouseEventArgs args)
	{
		if (State != dfGestureState.Began)
		{
			return;
		}
		CurrentPosition = args.Position;
		if (Time.realtimeSinceStartup - StartTime <= timeout)
		{
			if (Vector2.Distance(CurrentPosition, StartPosition) >= minDistance)
			{
				State = dfGestureState.Ended;
				DeltaTime = Time.realtimeSinceStartup - StartTime;
				if (FlickGesture != null)
				{
					FlickGesture(this);
				}
				gameObject.Signal("OnFlickGesture", this);
			}
			else
			{
				State = dfGestureState.Failed;
			}
		}
		else
		{
			State = dfGestureState.Failed;
		}
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
		State = dfGestureState.None;
	}
}
