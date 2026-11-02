using UnityEngine;

[AddComponentMenu("Daikon Forge/Input/Gestures/Tap")]
public class dfTapGesture : dfGestureBase
{
	[SerializeField]
	private float timeout = 0.25f;

	[SerializeField]
	private float maxDistance = 25f;

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

	public event dfGestureEventHandler<dfTapGesture> TapGesture;

	protected void Start()
	{
	}

	public void OnMouseDown(dfControl source, dfMouseEventArgs args)
	{
		Vector2 startPosition = (CurrentPosition = args.Position);
		StartPosition = startPosition;
		State = dfGestureState.Possible;
		StartTime = Time.realtimeSinceStartup;
	}

	public void OnMouseMove(dfControl source, dfMouseEventArgs args)
	{
		if (State == dfGestureState.Possible || State == dfGestureState.Began)
		{
			CurrentPosition = args.Position;
			if (Vector2.Distance(args.Position, StartPosition) > maxDistance)
			{
				State = dfGestureState.Failed;
			}
		}
	}

	public void OnMouseUp(dfControl source, dfMouseEventArgs args)
	{
		if (State == dfGestureState.Possible)
		{
			if (Time.realtimeSinceStartup - StartTime <= timeout)
			{
				CurrentPosition = args.Position;
				State = dfGestureState.Ended;
				if (TapGesture != null)
				{
					TapGesture(this);
				}
				gameObject.Signal("OnTapGesture", this);
			}
			else
			{
				State = dfGestureState.Failed;
			}
		}
		else
		{
			State = dfGestureState.None;
		}
	}

	public void OnMultiTouch(dfControl source, dfTouchEventArgs args)
	{
		State = dfGestureState.Failed;
	}
}
