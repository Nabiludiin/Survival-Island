using System;
using UnityEngine;

[Serializable]
public class AnimationConnection
{
	[HideInInspector]
	public string name;

	public string layerName;

	public string stateName;

	public string functionName;

	public string functionParameter;

	public GameObject objectReciever;

	public float timeOfEvent;

	[HideInInspector]
	public bool active;

	[HideInInspector]
	public bool calledEvent;

	[HideInInspector]
	public float lastNormalizedTime;
}
