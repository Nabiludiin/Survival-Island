using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Daikon Forge/Input/Gestures/Resize")]
public class dfResizeGesture : dfGestureBase
{
	private float lastDistance;

	public float SizeDelta { get; protected set; }

	public event dfGestureEventHandler<dfResizeGesture> ResizeGestureStart;

	public event dfGestureEventHandler<dfResizeGesture> ResizeGestureUpdate;

	public event dfGestureEventHandler<dfResizeGesture> ResizeGestureEnd;

	protected void Start()
	{
	}

	public void OnMultiTouchEnd()
	{
		endGesture();
	}

	public void OnMultiTouch(dfControl sender, dfTouchEventArgs args)
	{
		List<dfTouchInfo> touches = args.Touches;
		if (State == dfGestureState.None || State == dfGestureState.Cancelled || State == dfGestureState.Ended)
		{
			State = dfGestureState.Possible;
		}
		else if (State == dfGestureState.Possible)
		{
			if (isResizeMovement(args.Touches))
			{
				State = dfGestureState.Began;
				Vector2 startPosition = (CurrentPosition = getCenter(touches));
				StartPosition = startPosition;
				lastDistance = Vector2.Distance(touches[0].position, touches[1].position);
				SizeDelta = 0f;
				if (ResizeGestureStart != null)
				{
					ResizeGestureStart(this);
				}
				gameObject.Signal("OnResizeGestureStart", this);
			}
		}
		else if ((State == dfGestureState.Began || State == dfGestureState.Changed) && isResizeMovement(touches))
		{
			State = dfGestureState.Changed;
			CurrentPosition = getCenter(touches);
			float num = Vector2.Distance(touches[0].position, touches[1].position);
			SizeDelta = num - lastDistance;
			lastDistance = num;
			if (ResizeGestureUpdate != null)
			{
				ResizeGestureUpdate(this);
			}
			gameObject.Signal("OnResizeGestureUpdate", this);
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

	private bool isResizeMovement(List<dfTouchInfo> list)
	{
		if (list.Count < 2)
		{
			return false;
		}
		dfTouchInfo dfTouchInfo2 = list[0];
		Vector2 normalized = (dfTouchInfo2.deltaPosition * (Time.deltaTime / dfTouchInfo2.deltaTime)).normalized;
		dfTouchInfo dfTouchInfo3 = list[1];
		Vector2 normalized2 = (dfTouchInfo3.deltaPosition * (Time.deltaTime / dfTouchInfo3.deltaTime)).normalized;
		float f = Vector2.Dot(normalized, (dfTouchInfo2.position - dfTouchInfo3.position).normalized);
		float f2 = Vector2.Dot(normalized2, (dfTouchInfo3.position - dfTouchInfo2.position).normalized);
		if (!(Mathf.Abs(f) >= 0.21460181f))
		{
			return Mathf.Abs(f2) >= 0.21460181f;
		}
		return true;
	}

	private void endGesture()
	{
		if (State == dfGestureState.Began || State == dfGestureState.Changed)
		{
			if (State == dfGestureState.Began)
			{
				State = dfGestureState.Cancelled;
			}
			else
			{
				State = dfGestureState.Ended;
			}
			float num = (SizeDelta = 0f);
			lastDistance = num;
			if (ResizeGestureEnd != null)
			{
				ResizeGestureEnd(this);
			}
			gameObject.Signal("OnResizeGestureEnd", this);
		}
		else
		{
			State = dfGestureState.None;
		}
	}
}
