using System;
using UnityEngine;

[Serializable]
public class DomesticVariables
{
	public float startHunger = 300f;

	public float starveTime = 480f;

	[Tooltip("How much hunger is added when the animal eats")]
	public float hungerGain = 180f;
}
