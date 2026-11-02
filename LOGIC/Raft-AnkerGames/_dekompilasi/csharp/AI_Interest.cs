using System;
using System.Collections.Generic;
using UnityEngine;

public class AI_Interest : MonoBehaviour
{
	public TriggerBoxEnterAction enterAction;

	public TriggerBoxExitAction exitAction;

	private List<InterestPoint> _interestPoints = new List<InterestPoint>();

	public bool HasInterest => InterestPoints.Count > 0;

	public List<InterestPoint> InterestPoints => _interestPoints;

	private void OnValidate()
	{
		if (enterAction == null)
		{
			enterAction = GetComponent<TriggerBoxEnterAction>();
		}
		if (exitAction == null)
		{
			exitAction = GetComponent<TriggerBoxExitAction>();
		}
	}

	private void Start()
	{
		TriggerBoxEnterAction triggerBoxEnterAction = enterAction;
		triggerBoxEnterAction.OnTriggerAction = (Action<Collider>)Delegate.Combine(triggerBoxEnterAction.OnTriggerAction, new Action<Collider>(OnEnter));
		TriggerBoxExitAction triggerBoxExitAction = exitAction;
		triggerBoxExitAction.OnTriggerAction = (Action<Collider>)Delegate.Combine(triggerBoxExitAction.OnTriggerAction, new Action<Collider>(OnExit));
	}

	public InterestPoint GetValidInterest()
	{
		foreach (InterestPoint interestPoint in InterestPoints)
		{
			if (!interestPoint.Occupied())
			{
				return interestPoint;
			}
		}
		return null;
	}

	private void OnEnter(Collider other)
	{
		if (other.transform.CompareTag("Interest"))
		{
			InterestPoint componentInChildren = other.GetComponentInChildren<InterestPoint>();
			if (componentInChildren != null && !InterestPoints.Contains(componentInChildren))
			{
				InterestPoints.Add(componentInChildren);
			}
		}
	}

	private void OnExit(Collider other)
	{
		if (other.transform.CompareTag("Interest"))
		{
			InterestPoint componentInChildren = other.GetComponentInChildren<InterestPoint>();
			if (componentInChildren != null && InterestPoints.Contains(componentInChildren))
			{
				InterestPoints.Remove(componentInChildren);
			}
		}
	}
}
