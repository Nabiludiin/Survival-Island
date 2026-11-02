using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class VisitorAnimationTrigger : MonoBehaviour
{
	public List<string> AnimationTriggerNames = new List<string>();

	public float MinTriggerInterval = 5f;

	public float MaxTriggerInterval = 12f;

	[HideInInspector]
	public float nextTriggerTime;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
		nextTriggerTime = Time.time + Random.Range(MinTriggerInterval, MaxTriggerInterval);
	}

	private void Update()
	{
		if (Time.time >= nextTriggerTime)
		{
			Trigger();
		}
	}

	public void Trigger(int triggerIndex = 0)
	{
		nextTriggerTime = Time.time + Random.Range(MinTriggerInterval, MaxTriggerInterval);
		if (AnimationTriggerNames != null && AnimationTriggerNames.Count > triggerIndex && animator != null)
		{
			animator.SetTrigger(AnimationTriggerNames[triggerIndex]);
		}
	}

	public void RandomTrigger()
	{
		Trigger(Random.Range(0, AnimationTriggerNames.Count));
	}
}
