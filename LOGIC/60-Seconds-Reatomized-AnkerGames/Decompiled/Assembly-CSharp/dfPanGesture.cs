using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Daikon Forge/Input/Gestures/Pan")]
public class dfPanGesture : dfGestureBase
{
	[SerializeField]
	protected float minDistance = 25f;

	private bool multiTouchMode;

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

	public Vector2 Delta { get; protected set; }

	public event dfGestureEventHandler<dfPanGesture> PanGestureStart;

	public event dfGestureEventHandler<dfPanGesture> PanGestureMove;

	public event dfGestureEventHandler<dfPanGesture> PanGestureEnd;

	protected void Start()
	{
	}

	public void OnMouseDown(dfControl source, dfMouseEventArgs args)
	{
		Vector2 startPosition = (CurrentPosition = args.Position);
		StartPosition = startPosition;
		State = dfGestureState.Possible;
		StartTime = Time.realtimeSinceStartup;
		Delta = Vector2.zero;
	}

	public void OnMouseMove(dfControl source, dfMouseEventArgs args)
	{
		if (State == dfGestureState.Possible)
		{
			if (Vector2.Distance(args.Position, StartPosition) >= minDistance)
			{
				State = dfGestureState.Began;
				CurrentPosition = args.Position;
				Delta = args.Position - StartPosition;
				if (PanGestureStart != null)
				{
					PanGestureStart(this);
				}
				gameObject.Signal("OnPanGestureStart", this);
			}
		}
		else if (State == dfGestureState.Began || State == dfGestureState.Changed)
		{
			State = dfGestureState.Changed;
			Delta = args.Position - CurrentPosition;
			CurrentPosition = args.Position;
			if (PanGestureMove != null)
			{
				PanGestureMove(this);
			}
			gameObject.Signal("OnPanGestureMove", this);
		}
	}

	public void OnMouseUp(dfControl source, dfMouseEventArgs args)
	{
		endPanGesture();
	}

	public void OnMultiTouchEnd()
	{
		endPanGesture();
		multiTouchMode = false;
	}

	public void OnMultiTouch(dfControl source, dfTouchEventArgs args)
	{
		Vector2 center = getCenter(args.Touches);
		if (!multiTouchMode)
		{
			endPanGesture();
			multiTouchMode = true;
			State = dfGestureState.Possible;
			StartPosition = center;
		}
		else if (State == dfGestureState.Possible)
		{
			if (Vector2.Distance(center, StartPosition) >= minDistance)
			{
				State = dfGestureState.Began;
				CurrentPosition = center;
				Delta = CurrentPosition - StartPosition;
				if (PanGestureStart != null)
				{
					PanGestureStart(this);
				}
				gameObject.Signal("OnPanGestureStart", this);
			}
		}
		else if (State == dfGestureState.Began || State == dfGestureState.Changed)
		{
			State = dfGestureState.Changed;
			Delta = center - CurrentPosition;
			CurrentPosition = center;
			if (PanGestureMove != null)
			{
				PanGestureMove(this);
			}
			gameObject.Signal("OnPanGestureMove", this);
		}
	}

	private Vector2 getCenter(List<dfTouchInfo> list)
	{
		Vector2 zero = Vector2.zero;
		for (int i = 0; i < list.Count; i++)
		{
			zero += list[i].position;
		}
		return zero / list.Count;
	}

	private void endPanGesture()
	{
		Delta = Vector2.zero;
		StartPosition = Vector2.one * float.MinValue;
		if (State == dfGestureState.Began || State == dfGestureState.Changed)
		{
			State = dfGestureState.Ended;
			if (PanGestureEnd != null)
			{
				PanGestureEnd(this);
			}
			gameObject.Signal("OnPanGestureEnd", this);
		}
		else if (State == dfGestureState.Possible)
		{
			State = dfGestureState.Cancelled;
		}
	}
}
