using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public abstract class EventAction : MonoBehaviour, ISerializationCallbackReceiver
{
	public bool OneTime;

	[HideInInspector]
	[SerializeField]
	protected float Delay = -1f;

	public FloatOptionalRange NewDelay;

	public bool DelayUseUnscaledTime;

	private bool _called;

	public EventAction()
	{
	}

	public virtual void OnBeforeSerialize()
	{
	}

	public virtual void OnAfterDeserialize()
	{
		if (!float.IsNaN(Delay))
		{
			NewDelay = Delay;
		}
		Delay = float.NaN;
	}

	protected virtual void Start()
	{
	}

	public void Invoke(EventActionArgs args = null)
	{
		if (!OneTime || !_called)
		{
			_called = true;
			FloatOptionalRange newDelay = NewDelay;
			if ((float)newDelay > 0f)
			{
				StartCoroutine(DelayedInvoke(args, newDelay));
			}
			else
			{
				Act(args);
			}
		}
	}

	private IEnumerator DelayedInvoke(EventActionArgs args, float delay)
	{
		float t = delay;
		while (t > 0f)
		{
			t -= (DelayUseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
			yield return null;
		}
		Act(args);
	}

	protected abstract void Act(EventActionArgs args = null);

	public void ForceInvoke()
	{
		Invoke();
	}
}
