using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class RepeatAction : StopableEventAction
{
	[Tooltip("Less than zero means every frame.")]
	[HideInInspector]
	[SerializeField]
	public float Interval = -1f;

	[HideInInspector]
	[SerializeField]
	public float Duration = -1f;

	public FloatOptionalRange NewInterval;

	public FloatOptionalRange NewDuration;

	public List<EventAction> Actions;

	private Coroutine _act;

	public override void OnAfterDeserialize()
	{
		if (!float.IsNaN(Interval))
		{
			NewInterval = Interval;
		}
		Interval = float.NaN;
		if (!float.IsNaN(Duration))
		{
			NewDuration = Duration;
		}
		Duration = float.NaN;
		base.OnAfterDeserialize();
	}

	public override void OnBeforeSerialize()
	{
		base.OnBeforeSerialize();
	}

	protected override void Act(EventActionArgs args = null)
	{
		if (_act != null)
		{
			StopCoroutine(_act);
		}
		_act = StartCoroutine(Repeat(NewDuration, NewInterval, args));
	}

	private IEnumerator Repeat(FloatOptionalRange duraton, FloatOptionalRange interval, EventActionArgs args = null)
	{
		float t = 0f;
		if ((float)duraton <= 0f)
		{
			duraton = float.MaxValue;
		}
		float intervalValue = interval;
		if (intervalValue <= 0f)
		{
			intervalValue = 0f;
		}
		float it = intervalValue;
		float dt;
		for (; (float)duraton > t; t += dt)
		{
			if (it >= intervalValue)
			{
				it -= intervalValue;
				_invokeActions(args);
				intervalValue = interval;
				if (intervalValue <= 0f)
				{
					intervalValue = 0f;
				}
			}
			dt = Time.deltaTime;
			yield return null;
			it += dt;
		}
	}

	private void _invokeActions(EventActionArgs args = null)
	{
		for (int i = 0; i < Actions.Count; i++)
		{
			EventAction eventAction = Actions[i];
			if (eventAction == this)
			{
				Debug.LogErrorFormat("RepeatAction set to repeat itself. Circular reference.");
			}
			else
			{
				eventAction.Invoke(args);
			}
		}
	}

	protected override void Stop(EventActionArgs args = null)
	{
		if (_act != null)
		{
			StopCoroutine(_act);
		}
	}
}
